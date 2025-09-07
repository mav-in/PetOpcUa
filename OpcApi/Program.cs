using OpcApi.DependencyInjection;
using OpcApi.Services.MockOpc;
using OpcDomain;
using static OpcDomain.Constants;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Mock data
builder.Services.AddFileOpcMock(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/opc/last-read", (IOpcDataProvider provider, CancellationToken ct) =>
{
    return provider.GetAsync(ct);
});

app.MapGet("/api/opc/tags", async (string? status, IOpcDataProvider provider, CancellationToken ct) =>
{
    var data = await provider.GetAsync(ct);

    if (string.IsNullOrWhiteSpace(status))
    {
        return Results.Ok(data.Results);
    }

    if (!Enum.TryParse<QualityGroup>(status, true, out var g))
    {
        return Results.BadRequest("Wrong status!");
    }
    
    var filtered = data.Results
        .Where(w => Quality.GroupOf(w.StatusCode) == g);

    return Results.Ok(filtered);
});

app.Run();

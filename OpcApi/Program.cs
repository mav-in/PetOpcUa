using OpcApi.DependencyInjection;
using OpcApi.Extension;
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

app.MapGet("/api/opc/summary", async (IOpcDataProvider provider, CancellationToken ct) =>
{
    var data = await provider.GetAsync(ct);

    var groups = data.Results
        .GroupBy(t => Quality.GroupOf(t.StatusCode))
        .ToDictionary(g => g.Key.ToString(), g => g.Count());

    var types = data.Results
        .Select(t => t.DataType ?? "Unknown")
        .Distinct()
        .OrderBy(x => x);

    var allSrc = data.Results
        .Select(t => t.SourceTimestamp)
        .Where(d => d.HasValue)
        .Select(d => d!.Value)
        .ToList();

    var allSrv = data.Results
        .Select(t => t.ServerTimestamp)
        .Where(d => d.HasValue)
        .Select(d => d!.Value)
        .ToList();

    var (srcMin, srcMax) = allSrc.MinMaxSinglePass();
    var (srvMin, srvMax) = allSrv.MinMaxSinglePass();

    var summary = new
    {
        counts = new
        {
            Good = groups.GetValueOrDefault(QualityGroup.Good.ToString(), 0),
            Uncertain = groups.GetValueOrDefault(QualityGroup.Uncertain.ToString(), 0),
            Bad = groups.GetValueOrDefault(QualityGroup.Bad.ToString(), 0)
        },
        dataTypes = types,
        sourceTimestamp = new { min = srcMin, max = srcMax },
        serverTimestamp = new { min = srvMin, max = srvMax }
};
    return Results.Ok(summary);
});

app.Run();

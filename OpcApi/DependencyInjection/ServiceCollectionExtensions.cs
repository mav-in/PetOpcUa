using OpcApi.Services.MockOpc;
using System.Text.Json;
using Microsoft.AspNetCore.Http.Json;

namespace OpcApi.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFileOpcMock(this IServiceCollection services, IConfiguration config)
    {
        var mockPath = config["OpcMockData:FilePath"]
                       ?? Environment.GetEnvironmentVariable("OPC_MOCK_PATH")
                       ?? Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "data", "opc_mock.json");

        services.AddSingleton(new MockSourceOptions(mockPath));
        services.AddSingleton<IOpcDataProvider, FileOpcDataProvider>();

        return services;
    }
}

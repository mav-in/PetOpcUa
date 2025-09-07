using OpcDomain;
using System.Text.Json;

namespace OpcApi.Services.MockOpc;

sealed class FileOpcDataProvider(MockSourceOptions mockSourceOptions) : IOpcDataProvider
{
    private readonly MockSourceOptions _mockSourceOptions = mockSourceOptions;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new() {
        PropertyNameCaseInsensitive = true
    };

    public async Task<OpcRead> GetAsync(CancellationToken ct)
    {
        using (var fs = File.OpenRead(_mockSourceOptions.FilePath))
        {
            var data = await JsonSerializer.DeserializeAsync<OpcRead>(fs, _jsonSerializerOptions, ct)
                ?? throw new InvalidOperationException("Invalid JSON mock data!");
            return data;
        }
    }
}

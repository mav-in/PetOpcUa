using System.Net.Http;
using System.Net.Http.Json;
using OpcDomain;
using OpcClient.DtoModel;

namespace OpcClient.Services;

public class OpcApiClient(IHttpClientFactory factory)
{
    private readonly HttpClient _http = factory.CreateClient("api");

    public async Task<OpcRead?> GetLastReadAsync(CancellationToken ct)
    {
        return await _http.GetFromJsonAsync<OpcRead>("/api/opc/last-read", ct);
    }

    public Task<IReadOnlyList<OpcTag>?> GetTagsAsync(string? status, CancellationToken ct)
    {
        return _http.GetFromJsonAsync<IReadOnlyList<OpcTag>>($"/api/opc/tags{(string.IsNullOrEmpty(status) ? "" : $"?status={status}")}", ct);
    }

    public Task<SummaryDto?> GetSummaryAsync(CancellationToken ct)
    {
        return _http.GetFromJsonAsync<SummaryDto>("/api/opc/summary", ct);
    }
}

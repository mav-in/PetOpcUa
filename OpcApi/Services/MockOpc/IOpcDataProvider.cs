using OpcDomain;

namespace OpcApi.Services.MockOpc;

interface IOpcDataProvider
{
    Task<OpcRead> GetAsync(CancellationToken ct);
}

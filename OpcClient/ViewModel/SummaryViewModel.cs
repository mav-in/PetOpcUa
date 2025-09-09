using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpcClient.Services;

namespace OpcClient.ViewModels;

public partial class SummaryViewModel : ObservableObject
{
    private readonly OpcApiClient _api;
    private CancellationTokenSource? _cts;

    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private object? summary;
    [ObservableProperty] private string? error;

    public IAsyncRelayCommand RefreshCommand { get; }

    public SummaryViewModel(OpcApiClient api)
    {
        _api = api;
        RefreshCommand = new AsyncRelayCommand(LoadAsync);
    }

    public async Task LoadAsync()
    {
        IsBusy = true;
        Error = null;

        try
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            
            Summary = await _api.GetSummaryAsync(_cts.Token);
        }
        catch (Exception ex)
        {
            Error = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }
}

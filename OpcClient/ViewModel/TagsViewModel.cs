using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpcClient.Services;
using System.Collections.ObjectModel;

namespace OpcClient.ViewModels;

public partial class TagsViewModel : ObservableObject
{
    private readonly OpcApiClient _api;
    private CancellationTokenSource? _cts;

    public ObservableCollection<TagItemViewModel> Tags { get; } = [];

    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private string? error;
    [ObservableProperty] private bool autoRefresh;
    [ObservableProperty] private string filter = string.Empty;

    public IAsyncRelayCommand RefreshCommand { get; }
    public IRelayCommand ToggleAutoCommand { get; }

    public TagsViewModel(OpcApiClient api)
    {
        _api = api;
        RefreshCommand = new AsyncRelayCommand(RefreshAsync);
        ToggleAutoCommand = new RelayCommand(() => AutoRefresh = !AutoRefresh);
    }

    partial void OnAutoRefreshChanged(bool value)
    {
        if (value)
        {
            StartAuto();
        }
        else
        {
            StopAuto();
        }    
    }

    public async Task RefreshAsync()
    {
        IsBusy = true;
        Error = null;

        try
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            var data = await _api.GetLastReadAsync(_cts.Token);
            Tags.Clear();
            if (data != null)
            {
                foreach (var t in data.Results)
                {
                    Tags.Add(new TagItemViewModel(t));
                }
            }
        }
        catch (Exception ex)
        {
            Error = ex.Message;
        }
        finally {
            IsBusy = false;
        }
    }

    private async void StartAuto()
    {
        while (AutoRefresh)
        {
            await RefreshAsync();
            // TODO cfg
            await Task.Delay(TimeSpan.FromSeconds(5));
        }
    }

    private void StopAuto() => _cts?.Cancel();
}

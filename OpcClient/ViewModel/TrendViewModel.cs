using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpcClient.Services;
using OpcDomain;
using OxyPlot.Series;
using OxyPlot;
using System.Collections.ObjectModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OpcClient.ViewModels;

public partial class TrendViewModel : ObservableObject
{
    private readonly OpcApiClient _api;
    private CancellationTokenSource? _cts;

    public ObservableCollection<double> Points { get; } = [];

    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private string? error;
    [ObservableProperty] private string? currentLevel;
    [ObservableProperty] private string? currentStatus;
    [ObservableProperty] private PlotModel? trendModel;
    [ObservableProperty] private string tankLevelText = string.Empty;
    [ObservableProperty] private string tankLevelStatus = string.Empty;

    public IAsyncRelayCommand RefreshCommand { get; }

    public TrendViewModel(OpcApiClient api)
    {
        _api = api;
        RefreshCommand = new AsyncRelayCommand(RefreshAsync);
    }

    public async Task RefreshAsync()
    {
        IsBusy = true;
        Error = null;

        try
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            var list = await _api.GetLastReadAsync(_cts.Token) ?? new();

            Points.Clear();

            var trend = list.Results.FirstOrDefault(t => t.DisplayName == "Tank1.TrendLevel");
            var level = list.Results.FirstOrDefault(t => t.DisplayName == "Tank1.Level");

            BuildTrend(trend);

            if (level != null)
            {
                CurrentStatus = level.StatusCode;
                if (level.Value.TryGetDouble(out var d))
                    CurrentLevel = d.ToString();
                else
                    CurrentLevel = level.Value.ToString();
            }

            if (level is not null)
            {
                TankLevelText = ValueFormat.ToDisplay(level).Display;
                TankLevelStatus = level.StatusCode;
            }
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

    private void BuildTrend(OpcTag? trend)
    {
        if (trend is null || trend.Value.ValueKind != System.Text.Json.JsonValueKind.Array)
        {
            TrendModel = null;
            return;
        }

        var series = new LineSeries { MarkerType = MarkerType.Circle, MarkerSize = 2 };
        int i = 0;

        foreach (var v in trend.Value.EnumerateArray())
        {
            if (v.TryGetDouble(out var d))
                series.Points.Add(new DataPoint(i++, d));
        }
        var pm = new PlotModel { Title = "Tank1.TrendLevel" };
        pm.Series.Add(series);
        TrendModel = pm;
    }
}

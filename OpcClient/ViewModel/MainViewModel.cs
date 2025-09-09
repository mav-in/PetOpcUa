using CommunityToolkit.Mvvm.ComponentModel;

namespace OpcClient.ViewModels;

public partial class MainViewModel : ObservableObject
{
    public TagsViewModel TagsVM { get; }
    public TrendViewModel TrendVM { get; }
    public SummaryViewModel SummaryVM { get; }

    public MainViewModel(
        TagsViewModel tagsVM,
        TrendViewModel trendVM,
        SummaryViewModel summaryVM)
    {
        TagsVM = tagsVM;
        TrendVM = trendVM;
        SummaryVM = summaryVM;
    }
}

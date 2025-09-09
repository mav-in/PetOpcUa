using System.Windows.Controls;

namespace OpcClient.Views
{
    /// <summary>
    /// Interaction logic for TrendView.xaml
    /// </summary>
    public partial class TrendView : UserControl
    {
        public TrendView()
        {
            InitializeComponent();
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"TrendView DataContext (ctor): {this.DataContext}");
#endif
        }
    }
}

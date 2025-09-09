using System.Windows.Controls;

namespace OpcClient.Views
{
    /// <summary>
    /// Interaction logic for TagsView.xaml
    /// </summary>
    public partial class TagsView : UserControl
    {
        public TagsView()
        {
            InitializeComponent();
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"TagsView DataContext (ctor): {this.DataContext}");
#endif
        }
    }
}

using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using OpcDomain;

namespace OpcClient.Converters;

public class QualityToBrushConverter : IValueConverter
{
    private static readonly Brush Green = new SolidColorBrush(Color.FromRgb(204, 255, 153));
    private static readonly Brush Yellow = new SolidColorBrush(Color.FromRgb(255, 255, 204));
    private static readonly Brush Red = new SolidColorBrush(Color.FromRgb(255, 204, 153));

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var status = value as string ?? string.Empty;

        return Quality.GroupOf(status) switch
        {
            Constants.QualityGroup.Good => Green,
            Constants.QualityGroup.Uncertain => Yellow,
            _ => Red
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

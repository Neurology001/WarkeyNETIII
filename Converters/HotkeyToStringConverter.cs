using System.Globalization;
using System.Windows.Data;
using Dota1Warkey.Items;

namespace Dota1Warkey.Converters;

public class HotkeyToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not HotkeyItem item || item.Key == System.Windows.Input.Key.None)
            return "-";

        return item.Alt ? $"Alt+{item.Key}" : item.Key.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}

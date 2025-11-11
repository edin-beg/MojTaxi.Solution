using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace MojTaxi.ClientApp.Converters
{
    public class BoolToDefaultColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isDefault && isDefault)
                return Color.FromArgb("#FEF000"); // žuta
            return Color.FromArgb("#8A99B2"); // siva
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => Color.FromArgb("#8A99B2");
    }
}

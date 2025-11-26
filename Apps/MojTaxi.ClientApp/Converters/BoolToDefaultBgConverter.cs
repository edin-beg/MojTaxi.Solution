using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace MojTaxi.ClientApp.Converters
{
    public class BoolToDefaultBgConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isDefault && isDefault)
                return Color.FromArgb("#FEF000"); // žuta kada je default
            return Color.FromArgb("#2B2B2B"); // tamna kada nije
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => false;
    }
}

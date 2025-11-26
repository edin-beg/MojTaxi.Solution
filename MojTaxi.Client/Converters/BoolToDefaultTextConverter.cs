using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace MojTaxi.Client.Converters
{
    public class BoolToDefaultTextConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isDefault && isDefault)
                return "DEFAULT";
            return string.Empty;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => false;
    }
}

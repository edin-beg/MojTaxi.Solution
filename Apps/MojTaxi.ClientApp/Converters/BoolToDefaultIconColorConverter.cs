using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MojTaxi.ClientApp.Converters
{
    public class BoolToDefaultIconColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool flag)
            {
                return flag ? Colors.Green : Colors.Gray;
            }

            return Colors.Gray;
        }


        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
           return null;
        }

    }

}

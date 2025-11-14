using System;
using System.Globalization;
using System.Windows.Data;

namespace McDonalds.Converters
{
    public class DivideBy100Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intValue)
            {
                return (double)intValue / 100.0;
            }
            else if (value is double doubleValue)
            {
                return doubleValue / 100.0;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double doubleValue)
            {
                return doubleValue * 100.0;
            }

            return value;
        }
    }
}
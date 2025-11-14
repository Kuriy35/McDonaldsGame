using System;
using System.Globalization;
using System.Windows.Data;

namespace McDonalds.Converters
{
    public class IntToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intValue)
            {
                return intValue.ToString();
            }
            return "1";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                string cleanValue = stringValue.Trim().Replace("`", "").Replace("'", "").Replace("\"", "");
                
                if (int.TryParse(cleanValue, out int result))
                {
                    return result > 0 ? result : 1;
                }
            }
            
            return 1;
        }
    }
}


using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace McDonalds.Converters
{
    public class PatienceToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            float patience = (float)value;

            if (patience > 50) return Colors.Green;
            if (patience > 40) return Colors.Yellow;
            if (patience > 35) return Colors.Orange;
            return Colors.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

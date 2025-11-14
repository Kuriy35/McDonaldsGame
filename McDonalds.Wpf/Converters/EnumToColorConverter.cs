using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using McDonalds.Models.Orders;

namespace McDonalds.Converters
{
    public class EnumToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ProductState state)
            {
                if (state == ProductState.Raw) return new SolidColorBrush(Colors.Red);
                if (state == ProductState.Processing) return new SolidColorBrush(Colors.Orange);
                if (state == ProductState.Ready) return new SolidColorBrush(Colors.Green);
                if (state == ProductState.Burned) return new SolidColorBrush(Colors.DarkRed);
                return new SolidColorBrush(Colors.Gray);
            }

            return new SolidColorBrush(Colors.Black);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
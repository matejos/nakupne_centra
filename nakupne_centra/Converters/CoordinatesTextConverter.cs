using System;
using Windows.UI.Xaml.Data;

namespace nakupne_centra.Converters
{
    public class CoordinatesTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                string[] coords = (value as string).Split(',');
                double x = Double.Parse(coords[0]);
                double y = Double.Parse(coords[1]);
                return (x - 20) + "," + (y - 64 - 40) + ",0,0";
            }
            return "0";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

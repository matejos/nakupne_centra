using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.UI.Xaml.Data;

namespace nakupne_centra.Converters
{
    public class EmptyCollectionToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Debug.WriteLine(value != null);
            Debug.WriteLine((value as ICollection).Count > 0);
            return (value != null && (value as ICollection).Count > 0) ? Windows.UI.Xaml.Visibility.Collapsed : Windows.UI.Xaml.Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

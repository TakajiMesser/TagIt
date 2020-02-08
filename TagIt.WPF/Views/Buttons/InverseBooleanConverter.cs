using System;
using System.Globalization;
using System.Windows.Data;

namespace TagIt.WPF.Views.Buttons
{
    public class InverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            !(value as bool?);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            !(value as bool?);
    }
}

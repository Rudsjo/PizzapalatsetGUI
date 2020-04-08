using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;

namespace AdminTerminal_v2
{
    public class BooleanToVisibilityConverter : BaseValueConverter<BooleanToVisibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // if the parameter is null...
            if (parameter == null)
                // we return hidden if value is true and visible if value is false
                return (bool)value ? Visibility.Hidden : Visibility.Visible;

            // if parameter is not null...
            else
                // we return visible if value is true and hidden if value is false
                return (bool)value ? Visibility.Visible : Visibility.Hidden;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;

namespace CookingTerminal
{
    /// <summary>
    /// Converter that takes a boolean and returns a Visibility
    /// </summary>
    public class BooleanToVisibilityConverter : BaseValueConverter<BooleanToVisibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Checking for parameters
            if (parameter == null)
                // If no parameters are sent in, true will return visible
                return (bool)value ? Visibility.Visible : Visibility.Hidden;

            else if ((string)parameter == "PreSpinner")
                // In case it's a button with a spinner, normal text will be shown before spinner
                return (bool)value ? Visibility.Hidden : Visibility.Visible;

            else
                // If any other parameter is sent in, true vill return hidden
                return (bool)value ? Visibility.Visible : Visibility.Hidden;               
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

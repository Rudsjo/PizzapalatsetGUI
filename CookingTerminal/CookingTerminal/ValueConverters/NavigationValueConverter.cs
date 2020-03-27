using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace CookingTerminal
{
    /// <summary>
    /// The Value Converter to instancing a new page
    /// </summary>
    public class NavigationValueConverter : BaseValueConverter<NavigationValueConverter>
    {
        /// <summary>
        /// Method to convert the enum to a new page
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Check the current page
            switch ((Navigator)value)
            {
                // Login
                case Navigator.Login:
                    return null;

                // Main
                case Navigator.Main:
                    return new MainPage();
                
                // Cooking
                case Navigator.Cooking:
                    return new CookingPage();

                // Default is login
                default:
                    return null;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

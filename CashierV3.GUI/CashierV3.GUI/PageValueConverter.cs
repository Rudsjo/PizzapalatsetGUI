using CashierV3.GUI.Pages;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace CashierV3.GUI
{
    public class PageValueConverter : MarkupExtension, IValueConverter
    {
        #region Converter for navigating pages with an enumerator

        /// <summary>
        /// Converts an enum to an object and sets the current page
        /// </summary>
        /// <param name="value">Input for enum</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((Navigator)value)
            {
                case Navigator.Login:
                    var login = new LoginPage();
                    login.DataContext = new LoginPageViewModel();
                    return login;

                case Navigator.Cashier:
                    var cashier = new CashierPage();
                    cashier.DataContext = new CashierPageViewModel();
                    return cashier;

                default:
                    var defaultLogin = new LoginPage();
                    defaultLogin.DataContext = new LoginPageViewModel();
                    return defaultLogin;
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

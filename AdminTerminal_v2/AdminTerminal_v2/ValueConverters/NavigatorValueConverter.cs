using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace AdminTerminal_v2
{
    /// <summary>
    /// Converts the <see cref="Navigator"/> to an actual page
    /// </summary>
    public class NavigatorValueConverter : BaseValueConverter<NavigatorValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Find the right page
            switch ((Navigator)value)
            {
                case Navigator.Login:
                    return new LoginPage();

                case Navigator.Employees:
                    return new EmployeePage();

                case Navigator.Condiments:
                    return new CondimentPage();

                case Navigator.Extras:
                    return new ExtraPage();

                case Navigator.OldOrders:
                    return new OldOrderPage();

                case Navigator.Pizzas:
                    return new PizzaPage();

                default:
                    return new LoginPage();
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace AdminTerminal_v2
{
    public class PizzaBottomValueConverter : BaseValueConverter<PizzaBottomValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((int)value)
            {
                case 1:
                    if ((string)parameter == "Italian")
                        return true;

                    else
                        return false;

                case 2:
                    if ((string)parameter == "American")
                        return true;

                    else
                        return false;

                default:
                    if ((string)parameter == "Italian")
                        return true;

                    else
                        return false;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isChecked = (bool)value;

            if (isChecked && (string)parameter == "Italian")
                return 1;

            else
                return 2;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace CookingTerminal
{
    public class CookingButtonValueConverter : BaseValueConverter<CookingButtonValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((CookingStatus)value)
            {
                case CookingStatus.IsNotCooked:
                    {
                        return App.Current.Resources[string.Format("GreenActionButton")];
                    }

                case CookingStatus.IsCooking:
                    {
                        return App.Current.Resources[string.Format("CookingTimer")];
                    }

                case CookingStatus.IsCooked:
                    {
                        return App.Current.Resources[string.Format("RedActionButton")];
                    }

                default:
                    return App.Current.Resources[string.Format("GreenActionButton")];
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

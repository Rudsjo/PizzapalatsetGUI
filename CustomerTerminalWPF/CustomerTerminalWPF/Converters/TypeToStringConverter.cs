namespace CustomerTerminalWPF
{
    /// <summary>
    /// Required namespaces
    /// </summary>
    #region Namespaces
    using System;
    using System.Globalization;
    #endregion

    public class TypeToStringConverter : BaseValueConverter<TypeToStringConverter>
    {
        /// <summary>
        /// Convert menu to string
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Check wich menu the user is on
            switch ((Enums)value)
            {
                // Return corresponding string
                case Enums.Pizza:  return "Pizzor";
                case Enums.Drink:  return "Dryck";
                case Enums.Sallad: return "Sallader";
                case Enums.Pasta:  return "Pasta";
                case Enums.Extra:  return "Tillbehör";

                default: return "";
            }
        }

        /// <summary>
        /// Convert back to menu enum
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

namespace CustomerTerminalWPF
{
    /// <summary>
    /// Required namespaces
    /// </summary>
    #region Namespaces
    using System;
    using System.Globalization;
    using System.Windows;
    #endregion

    /// <summary>
    /// Bool to visibility converter
    /// </summary>
    public class BoolToVisibilityConverter : BaseValueConverter<BoolToVisibilityConverter>
    {
        /// <summary>
        /// Convert the provided value to visibility
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Check the provided value
            switch ((bool)value)
            {
                // If item is editable, return visible
                case true: return Visibility.Visible;

                //If item is'nt editable, return collapsed
                case false: return Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Convert back to a bool
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

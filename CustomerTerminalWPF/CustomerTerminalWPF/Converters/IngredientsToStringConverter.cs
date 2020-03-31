namespace CustomerTerminalWPF
{
    /// <summary>
    /// Required namespaces
    /// </summary>
    #region Namespaces
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using CustomerTerminalWPF.ViewModels;
    using System.Globalization;
    #endregion

    /// <summary>
    /// Convert list for display
    /// </summary>
    public class IngredientsToStringConverter : BaseValueConverter<IngredientsToStringConverter>
    {
        /// <summary>
        /// Covnert the provided string list to a presentable string
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Return null if the converter recieves null value
            if (value == null) return null;

            // Result string
            string Result = "";

            // Go through each string in the array and add it to the result
            foreach (var ing in (ObservableCollection<OrderItemViewModel>)value)
                Result += ing.Name + ", ";

            // Remove the last coma
            return (String.IsNullOrEmpty(Result)) ? "" :  Result.Substring(0, Result.Length - 2);
        }

        /// <summary>
        /// Convert back to a list
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

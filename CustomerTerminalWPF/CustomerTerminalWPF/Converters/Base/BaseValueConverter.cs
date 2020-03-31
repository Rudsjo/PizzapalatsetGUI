namespace CustomerTerminalWPF
{
    /// <summary>
    /// Required namespaces
    /// </summary>
    #region Namespaces
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Markup;
    #endregion

    /// <summary>
    /// Base converter for all the other converters, this is so that
    /// the converter can be accessed through XAML
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseValueConverter<T> : MarkupExtension, IValueConverter
        where T : class, new()
    {
        /// <summary>
        /// Instance of this converter
        /// </summary>
        private T ThisConverter;

        /// <summary>
        /// Overide the provide value, provide this converter
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            // If instance is null, return a new one
            return ThisConverter ?? new T();
        }

        /// <summary>
        /// Convert the provided value
        /// This is set by the converter
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        /// <summary>
        /// Convert the procided value back
        /// This is set by the converter
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
    }
}

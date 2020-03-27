using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;

namespace CookingTerminal
{
    /// <summary>
    /// A base value converter that allows direct XAML usage
    /// </summary>
    /// <typeparam name="Converter">The ValueConverter to use</typeparam>
    public abstract class BaseValueConverter<Converter> : MarkupExtension, IValueConverter
        where Converter : class, new()
    {
        #region Private Members

        /// <summary>
        /// Static instanc of the value converter
        /// </summary>
        private static Converter _converter = null;

        #endregion

        #region Value Converter Methods

        /// <summary>
        /// The method to convert one value to another
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        /// <summary>
        /// The method to convert back the value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);

        #endregion


        #region Markup Extension Methods

        /// <summary>
        /// Provides a static interface of the value converter
        /// </summary>
        /// <param name="serviceProvider">The service provider</param>
        /// <returns>The value converter as a new static interface</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null)
                _converter = new Converter();

            return _converter;
        }

        #endregion
    }
}

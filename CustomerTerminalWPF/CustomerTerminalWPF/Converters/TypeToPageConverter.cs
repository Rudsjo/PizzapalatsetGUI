namespace CustomerTerminalWPF
{
    /// <summary>
    /// Required Namespaces
    /// </summary>
    #region Namespaces
    using System;
    using System.Globalization;
    #endregion

    /// <summary>
    /// A magical converter!
    /// The heart for the entire program
    /// </summary>
    public class TypeToPageConverter : BaseValueConverter<TypeToPageConverter>
    {
        /// <summary>
        /// Return a page from a type
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            /* Converter should anlways recieve an Int Value
             */
            if (int.TryParse((string)parameter, out int ParameterValue))
            {
                /* 
                 * ************************************************
                 * parameter value 0: - return a MainPage.
                 * parameter value 1: - Ckeck if a SubPage should 
                 *                      be shown.
                 * parameter value 2: - Return the correct SubPage.
                 * parameter value 3: - Return a SubPage for the PaymentBaseFrame
                 * ************************************************
                 */

                switch (ParameterValue)
                {
                    case 0:
                        // Main Page
                        switch ((MainPages)value)
                        {
                            // Set the current page to a new WellcomePage
                            case MainPages.WellcomeScreen: return new Views.Pages.WellcomePage();

                            // Set the current page to a new OrderPage
                            case MainPages.OrderScreen: return new Views.Pages.OrderPage();


                            // The converter should never recieve any other value
                            default:
                                throw new Exception("Page does not exist");
                        }
                    // Is there a SubOrderPage to be shown? Then prepare the overlaying page
                    case 1:
                        switch ((OrderSubPages)value)
                        {
                            // Hide the overlaying frame
                            case OrderSubPages.None: return null;

                            // Show the overlaying frame else
                            default:
                                return new Views.Pages.Sub_Pages.OverlayingPage();
                        }
                    // What SubOrderPage should be shown
                    case 2:
                        switch ((OrderSubPages)value)
                        {
                            /* Covnerter should only recieve this value through the designer!
                             * The converter should never revcieve this value when the program
                             * is running!
                             */
                            case OrderSubPages.None: return null;

                            // Return a new instance of the PaymentPage
                            case OrderSubPages.PaymentPage: return new Views.Pages.Sub_Pages.PaymentFrameBase();

                            // Return a new instance of the EditOrderPage
                            case OrderSubPages.EditObjectPage: return new Views.Pages.Sub_Pages.EditObjectFrame();

                            // The converter should never recieve any other value
                            default:
                                throw new Exception("Page does not exist");
                        }
                    // Wich payment frame shold be shown
                    case 3:
                        switch ((PaymentStatueses)value)
                        {
                            // Return a new instance of the Waiting for PaymenFrame
                            case PaymentStatueses.WaitingForPayment: return new Views.Pages.Sub_Pages.PaymentWaitingFrame();

                            // Return a new instance of the PaymentOKFrame
                            case PaymentStatueses.PaymentOK: return new Views.Pages.Sub_Pages.PaymentOKFrame();

                            // Return a new instance of the PaymentNotOK frame
                            case PaymentStatueses.PaymentNotOK: return null;

                            // The converter should never recieve any other value
                            default:
                                throw new Exception("Page does not exist");
                        }

                    /* The converter should never recieve any other 
                     * value then the ones listed above
                     */
                    default:
                        throw new Exception("Invalid page category");
                }
            }

            // Should never fail to parse!
            else throw new Exception("Critical designer error!, failed to parse value to Page");
        }

        /// <summary>
        /// Convert back to a type
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            /* This function does not need any
             * implementation as for now
             */
            throw new NotImplementedException();
        }
    }
}

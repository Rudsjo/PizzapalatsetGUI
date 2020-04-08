namespace CustomerTerminalWPF.ViewModels
{
    /// <summary>
    /// ViewModel for the payment base page
    /// </summary>
    public class PaymentPageBaseViewModel : 
                 BaseViewModel
    {
        /// <summary>
        /// Public properties
        /// </summary>
        #region Properties

        /// <summary>
        /// Single instance of this ViewModel
        /// </summary>
        public static PaymentPageBaseViewModel VM { get; set; }

        /// <summary>
        /// Status of the paymet, also chooses wich frame should
        /// be displayed
        /// </summary>
        public PaymentStatueses CurrentStatus { get; set; }

        #endregion

        /// <summary>
        /// Default constructor
        /// </summary>
        public PaymentPageBaseViewModel()
        {
            // Set VM to this
            VM = this;

            // Default value
            CurrentStatus = PaymentStatueses.WaitingForPayment;
        }

    }
}

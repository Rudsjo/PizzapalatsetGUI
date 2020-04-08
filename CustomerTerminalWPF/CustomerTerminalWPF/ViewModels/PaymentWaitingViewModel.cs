namespace CustomerTerminalWPF.ViewModels
{
    /// <summary>
    /// Required Namespaces
    /// </summary>
    #region Namespaces
    using CustomerTerminalWPF.ViewModels.Commands;
    using System;
    using System.Threading;
    #endregion

    /// <summary>
    /// ViewModel for the BasePaymentPage
    /// </summary>
    public class PaymentWaitingViewModel : BaseViewModel
    {
        #region Properties

        /// <summary>
        /// Command for canceling the order
        /// </summary>
        public RelayCommand CancelPaymentCommand { get; set; }

        // Timer for changing the screen, (Emulate that the order was payed)
        private Timer PaymentSimulator { get; set; }

        #endregion

        /// <summary>
        /// Default constructor
        /// </summary>
        public PaymentWaitingViewModel()
        {
            PaymentSimulator = new Timer(new TimerCallback(ChangeScreen), null, 3000, 0);

            // Create the command for the 'Cancel' button
            CancelPaymentCommand = new RelayCommand((o) => { OrderPageViewModel.VM.SubPage = OrderSubPages.None; });
        }

        /// <summary>
        /// Change the state of the order
        /// </summary>
        /// <param name="state"></param>
        private void ChangeScreen(object state)
        {
            // Change state
            PaymentPageBaseViewModel.VM.CurrentStatus = PaymentStatueses.PaymentOK;

            // Dispose the timer
            PaymentSimulator.Dispose();
        }
    }
}

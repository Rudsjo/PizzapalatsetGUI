using System.Threading;

namespace CustomerTerminalWPF.ViewModels
{
    /// <summary>
    /// ViewModel for the PaymentOKFrame
    /// </summary>
    public class PaymentOKViewModel : BaseViewModel
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public PaymentOKViewModel()
        {
            CompleteOrder();
        }

        public async void CompleteOrder()
        {
            // Notify server if connected
            if (ProgramState.ServerConnection.ConnectionState == ConnectionStates.Connected)
            {
                /* This function will wait for a response from the database
                 * before it notifies the server so that no sync isssues
                 * will occur 
                /*==================================================================> */
                await DatabaseHelpers.CreateOrder(OrderPageViewModel.VM.OrderItems);
                /*==================================================================> 
                 */

                // Notify the server that a new order has been created
                ProgramState.ServerConnection.SendMessage("[NEWORDER]");
            }

            // Go back to the WellcomePage
            Thread.Sleep(2000);
            MainWindowViewModel.VM.CurrentScreen = MainPages.WellcomeScreen;
        }
    }
}

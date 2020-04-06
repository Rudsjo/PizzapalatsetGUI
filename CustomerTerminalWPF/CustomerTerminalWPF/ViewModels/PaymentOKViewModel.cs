using System.Threading;

namespace CustomerTerminalWPF.ViewModels
{
    /// <summary>
    /// ViewModel for the PaymentOKFrame
    /// </summary>
    public class PaymentOKViewModel : BaseViewModel
    {
        // The order ID that will be displayed on the screen
        public string OrderID { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public PaymentOKViewModel()
        {
            if(ProgramState.IsRunning)
                CompleteOrder();
        }

        public async void CompleteOrder()
        {           
            // Send notification to the server if the terminal is connected
            if (ProgramState.ServerConnection.ConnectionState == ConnectionStates.Connected)
            {
                // Notify the server that a new order has been created
                ProgramState.ServerConnection.SendMessage("[NEWORDER]");
            }

            /* This function will wait for a response from the database
             * before it notifies the server so that no sync isssues
             * will occur 
            /*==================================================================> */
            var Order = await DatabaseHelpers.CreateOrder(OrderPageViewModel.VM.OrderItems);
            OrderID = Order.OrderID.ToString();
            /*==================================================================> 
             */

            // Change screen after 4 seconds
            Timer ChangeScreen = new Timer((ee) => { MainWindowViewModel.VM.CurrentScreen = MainPages.WellcomeScreen; }, null, 4000, 0);
        }
    }
}

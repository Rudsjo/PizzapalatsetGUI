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
            // Notify server if connected
            if(ProgramState.ServerConnection.ConnectionState == ConnectionStates.Connected)
                ProgramState.ServerConnection.SendMessage("[NEWORDER]");
        }
    }
}

namespace CustomerTerminalWPF.ViewModels
{
    /// <summary>
    /// Required namespaces
    /// </summary>
    #region Namespaces
    using ViewModels.Commands;
    #endregion

    /// <summary>
    /// ViewModel for the wellcome page
    /// </summary>
    public class WellcomePageViewModel : 
                 BaseViewModel
    {
        /// <summary>
        /// Command for the buttons that will begin a new order
        /// </summary>
        public RelayCommand BeginOrderCommand { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public WellcomePageViewModel()
        {
            // Create the BeginOrder command
            BeginOrderCommand = new RelayCommand(BeginOrder, (o) => { return true; });
        }


        /// <summary>
        /// Begin a new order
        /// </summary>
        /// <param name="o"></param>
        public void BeginOrder(object o) 
        {
            MainWindowViewModel.VM.CurrentScreen = MainPages.OrderScreen;
        }
    }
}

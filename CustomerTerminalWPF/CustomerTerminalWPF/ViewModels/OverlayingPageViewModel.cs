namespace CustomerTerminalWPF.ViewModels
{
    /// <summary>
    /// ViewModel for the overlaying page
    /// </summary>
    public class OverlayingPageViewModel : BaseViewModel
    {
        /// <summary>
        /// What Page should Be Shown
        /// </summary>
        public OrderSubPages Display { get; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public OverlayingPageViewModel()
        {
            // Get the SubPage from the OrderViewModel
            if (ProgramState.IsRunning)
                Display = OrderPageViewModel.VM.SubPage;
        }
    }
}

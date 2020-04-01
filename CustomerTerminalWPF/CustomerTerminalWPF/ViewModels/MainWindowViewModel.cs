namespace CustomerTerminalWPF.ViewModels
{
    using System.Threading.Tasks;

    /// <summary>
    /// All of the required namespaces
    /// </summary>
    #region Namespaces
    using System.Windows;
    #endregion

    /// <summary>
    /// ViewModel for the main window
    /// </summary>
    public class MainWindowViewModel : 
                 BaseViewModel
    {
        #region Properties

        /// <summary>
        /// The current screen
        /// </summary>
        public MainPages CurrentScreen { get; set; }

        /// <summary>
        /// Single instance of this VM so other VM's can access root properties
        /// </summary>
        public static MainWindowViewModel VM { get; set; }

        #endregion

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainWindowViewModel()
        {
            Init();
        }

        /// <summary>
        /// First init
        /// </summary>
        private void Init()
        {
            // Set rpogramstate to running
            ProgramState.IsRunning = true;

            // Read the config files on startup
            ConfigFileHelpers.ReadServerConfigFile();

            // Try to connect to the server
            Task.Run(async () => 
            {
                await ProgramState.ServerConnection.ConnectAsync();
                await DatabaseHelpers.Init();
            });

            // Set the current screen
            CurrentScreen = MainPages.WellcomeScreen;

            // The await does not work for some f****** reason!
            while (DatabaseHelpers.AllProducts == null) ;

            // Set this VM
            VM = this;
        }
    }
}

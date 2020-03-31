namespace CustomerTerminalWPF.ViewModels
{
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
        /// Single instance of this window
        /// </summary>
        public static Window ThisWindow { get; private set; }

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
        public MainWindowViewModel(Window window)
        {
            Init();

            // Set this window
            ThisWindow = window;

            // Set the current screen
            CurrentScreen = MainPages.WellcomeScreen;

            // Set this VM
            VM = this;
        }

        /// <summary>
        /// First init
        /// </summary>
        private async void Init()
        {
            // Set rpogramstate to running
            ProgramState.IsRunning = true;

            // Read the config files on startup
            ConfigFileHelpers.ReadServerConfigFile();

            // Try to connect to the server
            if ( ! await ProgramState.ServerConnection.ConnectAsync() )
                // MessageBox is against MVVM, this is just temp
                MessageBox.Show("Ansultningen till serverns misslyckades, terminalen\nkommer köras i offline läge", "Varning",
                                MessageBoxButton.OK,
                                MessageBoxImage.Exclamation
                    );

            /* Load items from database
             * Procedure can fail so check
             * the list before exiting
             */
            while(DatabaseData.AllProducts == null) await DatabaseData.Init();
        }
    }
}

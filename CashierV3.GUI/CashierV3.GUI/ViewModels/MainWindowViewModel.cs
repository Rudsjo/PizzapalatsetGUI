using System.Windows;

namespace CashierV3.GUI
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Properties

        /// <summary>
        /// Navigator for keeping track of which page the user is currently on. Also used for changing the page
        /// </summary>
        private Navigator currentPage;
        public Navigator CurrentPage
        {
            get => currentPage;
            set
            {
                currentPage = value;        
                OnPropertyChanged("CurrentPage");
            }
        }

        #endregion

        #region Singleton
        public static MainWindowViewModel Instance { get; set; }
        #endregion

        #region Constructor
        public MainWindowViewModel()
        {
            Instance = this;
            Init();
        }

        #endregion

        #region ServerConnection and message
        /// <summary>
        /// First Init
        /// </summary>
        private async void Init()
        {
            ConfigFileHelpers.ReadServerConfigFile();
            if (!await ProgramState.Server.ConnectAsync())
                MessageBox.Show("Kunde inte etablera anslutning till servern.\nProgrammet körs i offline-läge.");
        }
        #endregion
    }
}

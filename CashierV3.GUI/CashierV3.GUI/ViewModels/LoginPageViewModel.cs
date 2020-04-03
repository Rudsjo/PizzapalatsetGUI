using System.Windows;

namespace CashierV3.GUI
{
    public class LoginPageViewModel : BaseViewModel
    {
        #region Singleton
        public static LoginPageViewModel Instance { get; set; }
        #endregion

        #region Properties

        /// <summary>
        /// Property that gets the login userID
        /// </summary>
        private string _userID;
        public string UserID
        {
            get { return _userID; }
            set
            {
                _userID = value;
                OnPropertyChanged("UserID"); 
            }
        }

        /// <summary>
        /// Property for getting the password from user
        /// </summary>
        private string _password;
        public string Password
        {
            get { return _password; }
            set 
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }

        /// <summary>
        /// This property is not used in this program and can be removed
        /// </summary>
        private string _role;
        public string Role
        {
            get { return _role; }
            set 
            {
                _role = value;
                OnPropertyChanged("Role"); 
            }
        }

        #endregion

        #region Constructor
        public LoginPageViewModel()
        {
            Instance = this;
            Login = new RelayCommand(LoginToCashierPage);
        }
        #endregion

        #region ActionMethods
        /// <summary>
        /// Method for logging in as admin or cashier. Bakers cannot login on this application.
        /// Method checks the userID and Password against the database and returns true if it finds a match
        /// Then the cashierPage can open.
        /// </summary>
        /// <param name="u"></param>
        private async void LoginToCashierPage(object u)
        {
            var id = int.TryParse(UserID, out int validInput);
            var result = await rep.CheckUserIdAndPassword(validInput, Password);

            if (result.Item1 == true && result.Item2.ToLower() != "bagare")
            {
                MainWindowViewModel.Instance.CurrentPage = Navigator.Cashier;
            }
            else
            {
                MessageBox.Show("Fel användarnamn eller lösenord");
            }
        }
        #endregion

    }
}

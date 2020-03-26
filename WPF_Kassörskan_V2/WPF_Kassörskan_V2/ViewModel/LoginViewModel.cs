namespace WPF_Kassörskan_V2.ViewModel
{
    using BackendHandler;
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using WPF_Kassörskan_V2.Command;
    using WPF_Kassörskan_V2.Model;

    public class LoginViewModel : INotifyPropertyChanged
    {

        private string _userID;
        public string UserID
        {
            get { return _userID; }
            set { OnPropertyChanged(ref _userID, value); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { OnPropertyChanged(ref _password, value); }
        }

        private string _role;
        public string Role
        {
            get { return _role; }
            set { OnPropertyChanged(ref _role, value); }
        }

        private object _selectedViewModel;
        public object SelectedViewModel
        {
            get { return _selectedViewModel; }
            set { if (SelectedViewModel == null) value = new CashierViewModel(); OnPropertyChanged(ref _selectedViewModel, value); }
        }

        private Navigate _switchView;
        public Navigate SwitchView
        {
            get { return _switchView; }
            set { OnPropertyChanged(ref _switchView, value); }
        }

        public LoginViewModel()
        {
            CashierPageCommand = new PageRelayCommand(ShowCashierPage);
            LoginPageCommand = new PageRelayCommand(ShowLoginPage);
        }

        private async void ShowCashierPage(object u)
        {
            var id = int.TryParse(UserID, out int validInput);
            var result = await rep.CheckUserIdAndPassword(validInput, Password);


            if (result.Item1 == true && result.Item2.ToLower() != "bagare")
            {
                SwitchView = Navigate.Base;
                SelectedViewModel = new CashierViewModel();
            }
            else
            {
                MessageBox.Show("Fel användarnamn eller lösenord");
            }

        }

        private async void ShowLoginPage(object u)
        {
            UserID = "";
            Password = "";
            SwitchView = Navigate.Login;
            SelectedViewModel = new LoginViewModel();
            SelectedViewModel = null;
        }


        #region RelayCommands for Buttons
        public RelayCommand UpdateRelayCommand { get; set; }

        public PageRelayCommand LoginPageCommand { get; set; }

        public PageRelayCommand CashierPageCommand { get; set; }
        #endregion

        #region Connects to database
        public static IDatabase rep { get; set; } = Helpers.GetSelectedBackend();
        #endregion

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged<T>(ref T prop, T value, [CallerMemberName] string propertyChanged = "")
        {
            prop = value;

            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyChanged));
            }
        }

        public event PropertyChangedEventHandler PropertyChangedV2;

        public void OnPropertyChangedV2(string propertyName)
        {
            if (PropertyChangedV2 != null)
            {
                PropertyChangedV2(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

    }
}

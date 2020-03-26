using BackendHandler;
using CashierTerminalPP.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace CashierTerminalPP.ViewModel
{
    class LoginPageViewModel : INotifyPropertyChanged
    {
        public static IDatabase rep { get; set; } = Helpers.GetSelectedBackend();
        public RelayCommand Update { get; set; }
        public RelayCommand Login { get; set; }

        public PageRelayCommand CashierPageCommand { get; set; }
        public PageRelayCommand LoginPageCommand { get; set; }


        #region Properties
        private object selectedViewModel;
        public object SelectedViewModel
        {
            get { return selectedViewModel; }
            set
            {
                if (selectedViewModel == null)
                    value = new CashierPageViewModel();

                selectedViewModel = value;
                OnPropertyChanged("SelectedViewModel");
            }
        }


        private Navigate switchView;
        public Navigate SwitchView
        {
            get { return switchView; }
            set
            {
                switchView = value;
                OnPropertyChanged("SwitchView");
            }
        }


        private string userID;
        public string UserID
        {
            get { return userID; }
            set
            {
                userID = value;
                OnPropertyChanged("UserID");
            }
        }


        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }
        #endregion
        public LoginPageViewModel()
        {
            CashierPageCommand = new PageRelayCommand(ShowCashierPage);
            LoginPageCommand = new PageRelayCommand(ShowLoginPage);
        }

        private async void ShowCashierPage(object p)
        {

            SwitchView = Navigate.Base;
            SelectedViewModel = new CashierPageViewModel();
            //var id = int.TryParse(UserID, out int validInput);
            //var result = await rep.CheckUserIdAndPassword(validInput, Password);

            //if (result.Item1 == true)
            //{
            //    SwitchView = Navigate.Base;
            //    SelectedViewModel = new CashierPageViewModel();
            //    UserID = "";
            //    Password = "";
            //}
            //else
            //{
            //    MessageBox.Show("Fel användarnamn eller lösenord");
            //}

        }

        private void ShowLoginPage(object p)
        {
            SwitchView = Navigate.Login;
            SelectedViewModel = new LoginPageViewModel();
            SelectedViewModel = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CookingTerminal
{
    public class LoginPageViewModel : BaseViewModel
    {

        #region Public Properties

        public int UserID { get; set; }

        public bool LoginIsRunning { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// The command to run the login action
        /// </summary>
        public ICommand Login { get; set; }

        #endregion

        #region Constructor

        public LoginPageViewModel()
        {
            Login = new RelayCommand(RunLogin);
        }

        #endregion


        #region Command Methods

        /// <summary>
        /// Method to run the login with the information given on login screen.
        /// </summary>
        /// <param name="parameter">The password as a <see cref="SecureString"/></param>
        private async void RunLogin(object parameter)
        {
            // Checking if the login is already running, if it is it's just returning
            // This is used to avoid the user from clicking too many times and overload the database calls
            if (LoginIsRunning)
                return;

            // If it's not running it will start doing so
            LoginIsRunning = true;


            try
            {
                // Checks if any UserID has been put in
                if (UserID == 0)
                    return;

                // Controlling the information with the database, result gets returned as a ValueTuple
                // Item 1 is returned as a boolean
                // Item 2 is returnes as a string of EmployeeRole
                var result = await rep.CheckUserIdAndPassword(UserID, (parameter as IHavePassword).SecurePassword.Unsecure());

                // For visual effect sleep the program for half a second
                Thread.Sleep(500);

                // Checking if the login went through
                if (result.Item1)
                {
                    // Then checking the credentials through the role
                    if (result.Item2 == "Admin" || result.Item2 == "Bagare")
                    {
                        // Changing page and confirm login as complete
                        MainWindowViewModel.VM.CurrentPage = Navigator.Main;
                        LoginIsRunning = false;
                    }
                    else
                    {
                        MessageBox.Show("Du saknar behörighet för denna terminal.");
                        LoginIsRunning = false;
                    }
                }

                else
                {
                    MessageBox.Show("Inloggning misslyckades.\nFelaktigt användarnamn eller lösenord.");
                    LoginIsRunning = false;
                }
            }
            finally
            {
                // At last we say that login is not running anymore, in case of crash or anything unforseen
                LoginIsRunning = false;
            }
            

        }

        #endregion

    }
}

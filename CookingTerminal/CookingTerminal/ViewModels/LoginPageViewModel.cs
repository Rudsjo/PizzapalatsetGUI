using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CookingTerminal
{
    public class LoginPageViewModel : BaseViewModel
    {

        #region Public Properties

        public int UserID { get; set; }

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
            
                // Checks if any UsereID has been put in
                if (UserID == 0)
                    return;

                // Controlling the information with the database, result gets returned as a ValueTuple
                    // Item 1 is returned as a boolean
                    // Item 2 is returnes as a string of EmployeeRole
                var result = await rep.CheckUserIdAndPassword(UserID, (parameter as IHavePassword).SecurePassword.Unsecure());

                // Checking if the login went through
                if (result.Item1)
                {
                    // Then checking the credentials through the role
                    if (result.Item2 == "Admin" || result.Item2 == "Bagare")
                    {
                        // Changing page
                        MainWindowViewModel.VM.CurrentPage = Navigator.Main;
                    }
                    else
                        MessageBox.Show("Du saknar behörighet för denna terminal.");
                }

                else
                    MessageBox.Show("Inloggning misslyckades.\nFelaktigt användarnamn eller lösenord.");
        }

        #endregion

    }
}

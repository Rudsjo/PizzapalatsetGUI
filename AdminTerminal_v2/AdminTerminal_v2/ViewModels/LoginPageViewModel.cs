using BackendHandler;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AdminTerminal_v2
{
    public class LoginPageViewModel : BaseViewModel
    {
        #region public Properties

        /// <summary>
        /// The ID of the user
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// A flag indicating id the login command is running
        /// </summary>
        public bool LoginIsRunning { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// The command to login
        /// </summary>
        public ICommand LoginCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public LoginPageViewModel()
        {
            // Create commands
            LoginCommand = new RelayLoginCommand(UserID, async (parameter) => await Login(parameter));
        }

        #endregion

        /// <summary>
        /// Attempts to log the user in
        /// </summary>
        /// <param name="parameter">The <see cref="SecureString"/>passed in from the view for the users password</param>
        /// <returns></returns>
        public async Task Login(object parameter)
        {

            await RunCommand(() => this.LoginIsRunning, async () =>
            {

                if (UserID == 0)
                    return;

                await Task.Delay(500);

                // Creates a ValueTuple
                    // Item 1 = bool
                    // Item 2 = string of EmployeeRole
                var t = await rep.CheckUserIdAndPassword(UserID, (parameter as IHavePassword).SecurePassword.Unsecure());

                if (t.Item1)
                {
                    LoginIsRunning = false;
                    MainWindowViewModel.VM.CurrentPage = Navigator.Pizzas;
                }

                else
                    MessageBox.Show("Inloggning misslyckades");
            });
        }

        public async Task ChangePage(Navigator navigator)
        {
            MainWindowViewModel.VM.CurrentPage = navigator;
        }

    }
}

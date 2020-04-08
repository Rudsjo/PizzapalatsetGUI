using BackendHandler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace CookingTerminal
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region PropertyChanged
        /// <summary>
        /// The event to fire when a child-property changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        /// <summary>
        /// Method to shorten the call for a property change
        /// </summary>
        /// <param name="name"></param>
        public void OnPropertyChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        #region Public Properties

        /// <summary>
        /// The repository to use
        /// </summary>
        public IDatabase rep { get; set; } = Helpers.GetSelectedBackend();

        /// <summary>
        /// The selected order to cook
        /// </summary>
        public static OrderViewModel OrderToCook { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// The command to call the logout action in all view models
        /// </summary>
        public ICommand Logout { get; set; }


        #region Command Methods

        /// <summary>
        /// Logs out the user and returns to the login page
        /// </summary>
        /// <param name="parameter"></param>
        protected void RunLogout(object parameter)
        {
            MainWindowViewModel.VM.CurrentPage = Navigator.Login;
        }

        #endregion

        #endregion




    }
}

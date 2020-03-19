using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace AdminTerminal_v2
{

    /// <summary>
    /// A basic command that runs an Action
    /// </summary>
    public class RelayLoginCommand : ICommand
    {
        #region Private Members

        /// <summary>
        /// The action to run
        /// </summary>
        private Action<object> _action;

        /// <summary>
        /// Property of the UserID being put in
        /// </summary>
        private int _userID { get; set; }

        #endregion

        #region Public Events
        /// <summary>
        /// The event that's fired when the <see cref="CanExecute(object)"/> value has changed
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public RelayLoginCommand(int UserID, Action<object> action)
        {
            _userID = UserID;
            _action = action;
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Checking if the input for UserID is only numbers
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            // Creating a regex containing only numbers
            Regex regex = new Regex(@"^\d$");

            // If the box is filled with anything else than numbers it won't execute
            if (!regex.IsMatch(_userID.ToString()))
                return false;

            // If everything is good it will execute the sent in action
            else
                return true;
        }

        /// <summary>
        /// Execute the sent in action
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter) => _action(parameter);

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace CookingTerminal
{
    public class RelayCommand : ICommand
    {

        #region Private Members

        /// <summary>
        /// The action to execute
        /// </summary>
        private Action<object> _action;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="action">The action to execute</param>
        public RelayCommand(Action<object> action)
        {
            // Setting the action to execute
            _action = action;
        }

        #endregion

        #region EventHandler
        /// <summary>
        /// Checks for changes in the predicate
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Checks if command execution is possible
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter) => true;

        /// <summary>
        /// Executing the sent i action
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter) => _action.Invoke(parameter);

        #endregion
    }
}

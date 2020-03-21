using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace AdminTerminal_v2
{
    public class RelayParameterlessCommand : ICommand
    {
        #region Private Members

        private Action<object> _action;

        #endregion

        /// <summary>
        /// Default constructor
        /// </summary>
        public RelayParameterlessCommand(Action<object> action)
        {
            _action = action;
        }

        /// <summary>
        /// Event that fires every time the 
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Checker if execution is possible
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter) => true;

        /// <summary>
        /// Executes the sent in action
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter) => _action.Invoke(parameter);
    }
}

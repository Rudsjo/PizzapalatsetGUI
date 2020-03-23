using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AdminTerminal_v2
{
    /// <summary>
    /// Basic command that runs an action
    /// </summary>
    public class RelayParameterizedCommand : ICommand
    {
        private Action<object> _action;
        //private Predicate<object> _condition;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="action">Action to be executed</param>
        public RelayParameterizedCommand(Action<object> action)
        {
            _action = action;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => _action.Invoke(parameter);
    }
}

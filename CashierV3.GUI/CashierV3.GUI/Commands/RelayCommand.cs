using System;
using System.Windows.Input;

namespace CashierV3.GUI
{
    public class RelayCommand : ICommand
    {
        private Action<object> _action;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> action)
        {
            _action = action;
        }

        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) => _action.Invoke(parameter);
    }
}

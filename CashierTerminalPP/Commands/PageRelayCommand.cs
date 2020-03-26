using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace CashierTerminalPP.Commands
{
    public class PageRelayCommand : ICommand
    {
        #region Private Members
        private Action<object> _action;
        private Predicate<object> _condition;
        #endregion

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #region Constructor
        public PageRelayCommand(Action<object> action)
            : this(action, null)
        {
        }
        /// <summary>
        /// Constructor that takes two parameters, one action and one predicate
        /// </summary>
        /// <param name="action">What method to run</param>
        /// <param name="condition">If the method can run</param>
        public PageRelayCommand(Action<object> action, Predicate<object> condition)
        {
            _action = action;
            _condition = condition;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Validation for the method. Method runs on true and doesnt run on false
        /// </summary>
        /// <param name="parameter">the returned boolean that decides if the method should run</param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            if (_condition == null)
                return true;

            return _condition(parameter);
        }

        /// <summary>
        /// Executes the action that is sent in
        /// </summary>
        /// <param name="parameter">The action that is being invoked</param>
        public void Execute(object parameter) => _action.Invoke(parameter);
        #endregion
    }
}

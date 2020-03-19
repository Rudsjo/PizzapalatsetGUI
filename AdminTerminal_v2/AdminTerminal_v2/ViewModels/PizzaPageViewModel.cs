using System;
using System.Collections.Generic;
using System.Text;

namespace AdminTerminal_v2
{
    public class PizzaPageViewModel : BaseViewModel
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public PizzaPageViewModel()
        {
            UpdateList();

            Navigate = new RelayParameterlessCommand(MainWindowViewModel.VM.NavigatorCommand);
        }
    }
}

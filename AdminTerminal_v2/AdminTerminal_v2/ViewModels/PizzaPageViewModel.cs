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

            Add = new RelayParameterlessCommand(AddCommand);
            Update = new RelayParameterlessCommand(UpdateCommand);
            Delete = new RelayParameterlessCommand(DeleteCommand);

            Clear = new RelayParameterlessCommand(ClearCommand);

            Navigate = new RelayParameterlessCommand(MainWindowViewModel.VM.NavigatorCommand);
        }
    }
}

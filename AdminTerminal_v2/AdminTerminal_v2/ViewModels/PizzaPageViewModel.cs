using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdminTerminal_v2
{
    public class PizzaPageViewModel : BaseViewModel
    {

        /// <summary>
        /// Default Constructor
        /// </summary>
        public PizzaPageViewModel()
        {
            Task.Run(async () => UpdateList()).Wait();

            Add = new RelayAsyncCommand(AddCommand);
            Update = new RelayAsyncCommand(UpdateCommand);
            Delete = new RelayAsyncCommand(DeleteCommand);

            Clear = new RelayParameterlessCommand(ClearCommand);

            Navigate = new RelayParameterlessCommand(MainWindowViewModel.VM.NavigatorCommand);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdminTerminal_v2
{
    public class ExtraPageViewModel : BaseViewModel
    {

        public ExtraPageViewModel()
        {
            Task.Run(async () => UpdateList()).Wait();

            Extra = new ExtraViewModel();

            Add = new RelayAsyncCommand(AddCommand);
            Update = new RelayAsyncCommand(UpdateCommand);
            Delete = new RelayAsyncCommand(DeleteCommand);

            Clear = new RelayParameterlessCommand(ClearCommand);

            Navigate = new RelayParameterlessCommand(MainWindowViewModel.VM.NavigatorCommand);
        }

    }
}

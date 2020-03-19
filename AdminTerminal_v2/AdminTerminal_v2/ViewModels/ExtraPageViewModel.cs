using System;
using System.Collections.Generic;
using System.Text;

namespace AdminTerminal_v2
{
    public class ExtraPageViewModel : BaseViewModel
    {

        public ExtraPageViewModel()
        {
            UpdateList();

            Extra = new ExtraViewModel();

            Add = new RelayParameterlessCommand(AddCommand);
            Update = new RelayParameterlessCommand(UpdateCommand);
            Delete = new RelayParameterlessCommand(DeleteCommand);

            Navigate = new RelayParameterlessCommand(MainWindowViewModel.VM.NavigatorCommand);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace AdminTerminal_v2
{
    public class CondimentPageViewModel : BaseViewModel
    {
        #region Constructor

        public CondimentPageViewModel()
        {
            UpdateList();
            Condiment = new CondimentViewModel();

            Add = new RelayAsyncCommand(AddCommand);
            Update = new RelayAsyncCommand(UpdateCommand);
            Delete = new RelayAsyncCommand(DeleteCommand);

            Clear = new RelayParameterlessCommand(ClearCommand);

            Navigate = new RelayParameterlessCommand(MainWindowViewModel.VM.NavigatorCommand);
        }

        #endregion

    }
}

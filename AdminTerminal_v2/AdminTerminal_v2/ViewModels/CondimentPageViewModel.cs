using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdminTerminal_v2
{
    public class CondimentPageViewModel : BaseViewModel
    {
        #region Constructor

        public CondimentPageViewModel()
        {
            Task.Run(async () => await UpdateList()).Wait();

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

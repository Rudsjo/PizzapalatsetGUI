using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AdminTerminal_v2
{
    public class EmployeePageViewModel : BaseViewModel
    {

        #region Constructor

        public EmployeePageViewModel()
        {
            UpdateList();
            Employee = new EmployeeViewModel();

            Add = new RelayAsyncCommand(AddCommand);
            Update = new RelayAsyncCommand(UpdateCommand);
            Delete = new RelayAsyncCommand(DeleteCommand);
            Clear = new RelayParameterlessCommand(ClearCommand);


            Navigate = new RelayParameterlessCommand(MainWindowViewModel.VM.NavigatorCommand);
        }

        #endregion

    }
}

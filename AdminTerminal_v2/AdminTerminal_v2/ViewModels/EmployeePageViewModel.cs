﻿using System;
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

            Add = new RelayParameterlessCommand(AddCommand);
            Update = new RelayParameterlessCommand(UpdateCommand);
            Delete = new RelayParameterlessCommand(DeleteCommand);

            Navigate = new RelayParameterlessCommand(MainWindowViewModel.VM.NavigatorCommand);
        }

        #endregion

    }
}

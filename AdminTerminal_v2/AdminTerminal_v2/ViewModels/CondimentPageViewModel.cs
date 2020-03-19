﻿using System;
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

            Add = new RelayParameterlessCommand(AddCommand);
            Update = new RelayParameterlessCommand(UpdateCommand);
            Delete = new RelayParameterlessCommand(DeleteCommand);

            Clear = new RelayParameterlessCommand(ClearCommand);

            Navigate = new RelayParameterlessCommand(MainWindowViewModel.VM.NavigatorCommand);
        }

        #endregion

    }
}

﻿using System;
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

            Add = new RelayAsyncCommand(AddCommand);
            Update = new RelayAsyncCommand(UpdateCommand);
            Delete = new RelayAsyncCommand(DeleteCommand);

            Clear = new RelayParameterlessCommand(ClearCommand);

            Navigate = new RelayParameterlessCommand(MainWindowViewModel.VM.NavigatorCommand);
        }

    }
}

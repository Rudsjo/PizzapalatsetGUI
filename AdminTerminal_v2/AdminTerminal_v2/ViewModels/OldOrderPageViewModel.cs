using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace AdminTerminal_v2
{
    public class OldOrderPageViewModel : BaseViewModel
    {

        /// <summary>
        /// Default constructor
        /// </summary>
        public OldOrderPageViewModel()
        {
            Task.Run(async () => await UpdateList()).Wait();

            CurrentOrderPizzaList = new List<BackendHandler.Pizza>();
            CurrentOrderExtraList = new List<BackendHandler.Extra>();

            Delete = new RelayAsyncCommand(DeleteCommand);

            Navigate = new RelayParameterlessCommand(MainWindowViewModel.VM.NavigatorCommand);
        }
    }
}

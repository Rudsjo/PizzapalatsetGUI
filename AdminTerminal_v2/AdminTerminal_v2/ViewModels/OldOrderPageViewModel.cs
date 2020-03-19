using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AdminTerminal_v2
{
    public class OldOrderPageViewModel : BaseViewModel
    {

        private OrderViewModel oldOrder;

        public List<BackendHandler.Pizza> CurrentOrderPizzaList { get; set; }

        public List<BackendHandler.Extra> CurrentOrderExtraList { get; set; }

        public OrderViewModel OldOrder
        {
            get { return oldOrder; }
            set
            {
                if (oldOrder == value)
                    return;

                oldOrder = value;
                CurrentOrderPizzaList = oldOrder.PizzaList;
                CurrentOrderExtraList = oldOrder.ExtraList;

            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public OldOrderPageViewModel()
        {
            UpdateList();

            Navigate = new RelayParameterlessCommand(MainWindowViewModel.VM.NavigatorCommand);
        }
    }
}

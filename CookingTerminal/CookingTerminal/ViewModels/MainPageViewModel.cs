using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Linq;

namespace CookingTerminal
{
    /// <summary>
    /// ViewModel for the Main page
    /// </summary>
    public class MainPageViewModel : BaseViewModel
    {
        #region Public Properties
        /// <summary>
        /// The property used to save the items from database
        /// </summary>
        public IEnumerable DatabaseList { get; set; }

        /// <summary>
        /// All orders with status 0 (ready to be cooked)
        /// </summary>
        public ObservableCollection<OrderViewModel> OrdersReadyToCook { get; set; }

        /// <summary>
        /// The orders that waited the longest
        /// </summary>
        public ObservableCollection<OrderViewModel> PrioritizedOrders { get; set; }

        /// <summary>
        /// Orders that not fit in main window but still need to be cooked
        /// </summary>
        public ObservableCollection<OrderViewModel> OrdersWaiting { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// Command to start cooking an order
        /// </summary>
        public ICommand CookOrder { get; set; }

        #endregion

        #region Constructor

        public MainPageViewModel()
        {
            // Creating the lists
            OrdersReadyToCook = new ObservableCollection<OrderViewModel>();
            PrioritizedOrders = new ObservableCollection<OrderViewModel>();
            OrdersWaiting = new ObservableCollection<OrderViewModel>();

            Task.Run(async () =>
            {
                // Getting all items from the database
                DatabaseList = await rep.GetOrderByStatus(0);

                // Setting it to the UI list
                OrdersReadyToCook = await ViewModelhelpers.FillListFromDatabase<BackendHandler.Order, OrderViewModel>(DatabaseList);

            }).Wait();

            FillWaitingLists();

            // Creating the commands
            CookOrder = new RelayCommand((object order) => 
            {
                // Gets the selected order
                var ChosenOrderToCook = OrdersReadyToCook.Where(x => x.OrderID == (int)order).First();

                // Updates the static property of which ordet to cook
                OrderToCook = ChosenOrderToCook;

                // Changes page
                MainWindowViewModel.VM.CurrentPage = Navigator.Cooking;
            });
            Logout = new RelayCommand(RunLogout);
        }

        #endregion


        #region Private Methods

        /// <summary>
        /// Method to fill the waiting lists
        /// The six first orders will be shown in the main boxes. 
        /// If it's more than 6 orders the rest will be put to another list.
        /// </summary>
        public void FillWaitingLists()
        {
            // Fills up the the different waiting lists
            foreach (var order in OrdersReadyToCook)
            {
                for (int i = 0; i < OrdersReadyToCook.Count; i++)
                {
                    // Adds the six longest-waiting order to show fully
                    if (PrioritizedOrders.Count < 6 && order == OrdersReadyToCook[i] && order.PizzaList.Count > 0)
                        PrioritizedOrders.Add(order);

                    // Adds the rest to the waiting list
                    else if (PrioritizedOrders.Count >=6 && order == OrdersReadyToCook[i] && order.PizzaList.Count > 0)
                        OrdersWaiting.Add(order);
                }
            }
        }

        #endregion


    }
}

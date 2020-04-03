using BackendHandler;
using System.Collections;


namespace CashierV3.GUI
{
    public class CashierPageViewModel : BaseViewModel
    {

        #region Singleton
        public static CashierPageViewModel Instance { get; set; }
        #endregion

        #region Lists
        /// <summary>
        /// List that fills with objects recieved from database
        /// </summary>
        private IEnumerable _dataBaseList;
        public IEnumerable DataBaseList
        {
            get { return _dataBaseList; }
            set
            {
                _dataBaseList = value;
                OnPropertyChanged("DataBaseList");
            }
        }

        /// <summary>
        /// List that is used for displaying items in View
        /// </summary>
        private ThreadSafeObservableCollection<OrderViewModel> _orderList;
        public ThreadSafeObservableCollection<OrderViewModel> OrderList
        {
            get { return _orderList; }
            set
            {
                _orderList = value;
                OnPropertyChanged("OrderList");
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property that acts as selected item and gets OrderID for updating purposes
        /// </summary>
        private OrderViewModel _currentOrder;
        public OrderViewModel CurrentOrder
        {
            get { return _currentOrder; }
            set 
            {
                _currentOrder = value;
                OnPropertyChanged("CurrentOrder"); 
            }
        }

        /// <summary>
        /// Property to get and set the status of an order
        /// </summary>
        private int _status;
        public int Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged("Status");
            }
        }

        #endregion

        #region Constructor to Get all orders, CurrentOrder and UpdateRelayCommand
        public CashierPageViewModel()
        {
            Instance = this;
            OrderList = new ThreadSafeObservableCollection<OrderViewModel>();
            CurrentOrder = new OrderViewModel();
            GetAllOrders();
            Logout = new RelayCommand(LogoutFromCashier);
            UpdateOrderWhenServed = new RelayCommand(UpdateOrder);
        }
        #endregion

        #region Get all orders

        /// <summary>
        /// Method for getting all orders which holds status 2, indicating that an order is ready to be served
        /// </summary>
        public async void GetAllOrders()
        {
            DataBaseList = await rep.GetOrderByStatus(2);
            OrderList.Clear();

            foreach (Order item in DataBaseList)
            {
                OrderViewModel model = new OrderViewModel();

                model.OrderID = item.OrderID;
                model.Price = item.Price;
                model.Status = item.Status;

                OrderList.Add(model);
            }
        }
        #endregion

        #region ActionMethod

        /// <summary>
        /// Updates the status of selected order (CurrentOrder) and sends it to finished orders with status 3
        /// Method also updates the list of orders and lastly it sends a ping to the server that an order has been served.
        /// </summary>
        /// <param name="u"></param>
        public async void UpdateOrder(object u)
        {
            Order orderToUpdate = new Order();
            Employee employeeLoggedIn = new Employee();
            employeeLoggedIn = await rep.GetSingleEmployee(int.Parse(LoginPageViewModel.Instance.UserID));
            orderToUpdate.Status = CurrentOrder.Status;
            orderToUpdate.OrderID = CurrentOrder.OrderID;
            await rep.UpdateOrderStatusWhenOrderIsServed(employeeLoggedIn, orderToUpdate);
            GetAllOrders();

            if (ProgramState.Running) { ProgramState.Server.SendMessage("[ORDERDONE]"); }               
        }

        /// <summary>
        /// Method fort Logout Command that sends the user back to the login screen
        /// </summary>
        /// <param name="l"></param>
        public void LogoutFromCashier(object l)
        {
            MainWindowViewModel.Instance.CurrentPage = Navigator.Login;
        }

        #endregion
    }
}

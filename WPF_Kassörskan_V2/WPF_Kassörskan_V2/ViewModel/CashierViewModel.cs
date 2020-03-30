namespace WPF_Kassörskan_V2.ViewModel
{
    using BackendHandler;
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using WPF_Kassörskan_V2.Command;
    using WPF_Kassörskan_V2.Model;

    public class CashierViewModel : LoginViewModel
    {
        #region Properties

        private OrderModel _currentOrder;
        public OrderModel CurrentOrder
        {
            get { return _currentOrder; }
            set { OnPropertyChanged(ref _currentOrder, value); }
        }

        private int _status;
        public int Status
        {
            get { return _status; }
            set { OnPropertyChanged(ref _status, value); }
        }

        private IEnumerable _dataBaseList;
        public IEnumerable DataBaseList
        {
            get { return _dataBaseList; }
            set { OnPropertyChanged(ref _dataBaseList, value); }
        }

        private ObservableCollection<OrderModel> _orderList;
        public ObservableCollection<OrderModel> OrderList
        {
            get { return _orderList; }
            set { OnPropertyChanged(ref _orderList, value); }
        }
        #endregion

        #region Get all orders
        public async Task<ObservableCollection<OrderModel>> GetAllOrders()
        {
            DataBaseList = await rep.GetOrderByStatus(2);
            OrderList = new ObservableCollection<OrderModel>();

            foreach(Order item in DataBaseList)
            {
                OrderModel model = new OrderModel();

                model.OrderID = item.OrderID;
                model.Price = item.Price;
                model.Status = item.Status;

                OrderList.Add(model);
            }
            return OrderList;
        }
        #endregion

        #region Constructor to Get all orders, CurrentOrder and UpdateRelayCommand
        public CashierViewModel()
        {
            GetAllOrders();
            CurrentOrder = new OrderModel();
            UpdateRelayCommand = new RelayCommand(Update, CanUpdate);
        }
        #endregion

        #region Updates the list of orders
        public async void Update(object u)
        {
            Order orderToUpdate = new Order();
            Employee employeeLoggedIn = new Employee();
            employeeLoggedIn = await rep.GetSingleEmployee(int.Parse(UserID));
            orderToUpdate.Status = CurrentOrder.Status;
            orderToUpdate.OrderID = CurrentOrder.OrderID;
            await rep.UpdateOrderStatusWhenOrderIsServed(employeeLoggedIn, orderToUpdate);
            await GetAllOrders();
        }

        #region Predicate
        private bool CanUpdate(object u)
        {
            if (CurrentOrder != null)
                return true;
            else
                return false;
        }
        #endregion

        #endregion
    }
}

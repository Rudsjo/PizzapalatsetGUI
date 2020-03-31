namespace CustomerTerminalWPF.ViewModels
{
    /// <summary>
    /// Required namespaces
    /// </summary>
    #region Namespaces
    using System;
    using System.Linq;
    using System.Collections.ObjectModel;
    using CustomerTerminalWPF.ViewModels.Commands;
    using System.ComponentModel;
    #endregion

    /// <summary>
    /// ViewModel for the order page
    /// </summary>
    public class OrderPageViewModel : BaseViewModel                
    {
        #region Properties

        /// <summary>
        /// Single instance of this ViewModel
        /// </summary>
        public static OrderPageViewModel VM       { get; set; }

        /// <summary>
        /// Command for the category buttons
        /// </summary>
        public RelayCommand CategorySelectCommand { get; set; }
        /// <summary>
        /// Command for the 'AddProduct' buttons
        /// </summary>
        public RelayCommand AddProductCommand     { get; set; }
        /// <summary>
        /// Command for the 'RemoveProduct' button
        /// </summary>
        public RelayCommand RemoveProductCommand  { get; set; }
        /// <summary>
        /// Command for the 'Cancel' button
        /// </summary>
        public RelayCommand CancelCommand         { get; set; }
        /// <summary>
        /// Command for the 'Payment' button
        /// </summary>
        public RelayCommand PayCommand            { get; set; }
        /// <summary>
        /// Command for the 'Edit' buttons
        /// </summary>
        public RelayCommand EditObjectCommand     { get; set; }

        /// <summary>
        /// Current food menu
        /// </summary>
        public Enums CurrentMenu                  { get; set; }

        /// <summary>
        /// Sub page for the order page
        /// </summary>
        public OrderSubPages SubPage              { get; set; }

        /// <summary>
        /// List of all the available products
        /// </summary>
        public ObservableCollection<OrderItemViewModel> PagedProducts { get; set; }

        /// <summary>
        /// Contains static properties for the currnet order.
        /// They will be reset when a new instance of this
        /// ViewModel is being set. (When a new order starts)
        /// </summary>
        #region Customer order data         

        /// <summary>
        /// Price display for the UI
        /// </summary>
        public string OrderPriceString { get; set; }

        /// <summary>
        /// This order's total price
        /// </summary>
        public float OrderPrice { get {
                if (OrderItems == null) return 0;
                float Price = 0;
                OrderItems.ToList().ForEach(o => { Price += o.Price; });
                return Price;
            } }

        /// <summary>
        /// All items in the current order
        /// </summary>
        public ObservableCollection<OrderItemViewModel> OrderItems { get; set;  }

        /// <summary>
        /// Private member of selected item
        /// </summary>
        private OrderItemViewModel selectedItem;
        /// <summary>
        /// If user want to edit an object, set this instance to that object
        /// </summary>
        public OrderItemViewModel SelectedItem
        {
            get 
            {
                return selectedItem;
            } 
            set
            {
                selectedItem = value;
                OrderItems[OrderItems.IndexOf(OrderItems.First(i => i.BasketID.Equals(value.BasketID)))] = value;
            }
        }

        #endregion

        #endregion

        /// <summary>
        /// Default constructor
        /// </summary>
        public OrderPageViewModel() { if(ProgramState.IsRunning) Init(); }

        /// <summary>
        /// First init
        /// </summary>
        private void Init()
        {
            // Set VM to this
            VM = this;

            // Skip if the list is empy
            if (DatabaseData.AllProducts != null)
            {
                // Load products from the Database class
                PagedProducts = new ObservableCollection<OrderItemViewModel>(
                                                                             DatabaseData.AllProducts.Where(
                                                                             p => p.Type == CurrentMenu));
            }
            else throw new Exception("List is null");

            /// <summary>
            /// Create all commands associated with
            /// this View
            /// </summary>
            #region Create Commands
            // Create the command for the category buttons
            CategorySelectCommand = new RelayCommand(CategoryChanged);
            // Create the command for the Add Product buttons
            AddProductCommand     = new RelayCommand(AddProduct);
            // Create the command for 'RemoveProduct' buttons
            RemoveProductCommand  = new RelayCommand( (o) => { OrderItems.Remove(OrderItems.First(i => i.BasketID.Equals((int)o) )); });
            // Create the command for the 'Cancel' button
            CancelCommand         = new RelayCommand( (o) => { MainWindowViewModel.VM.CurrentScreen = MainPages.WellcomeScreen;      });
            // Create the command for the 'Cancel' button
            PayCommand            = new RelayCommand( (o) => { SubPage = OrderSubPages.PaymentPage;    });
            // Create the command for the 'Edit' buttons 
            EditObjectCommand     = new RelayCommand( (o) => { SelectedItem = OrderItems.First(i => i.BasketID.Equals((int)o)).Copy(); 
                                                               SubPage = OrderSubPages.EditObjectPage; });
            #endregion

            // Set the first food menu
            CurrentMenu = Enums.Extra;
            // Reset the static properties for the order
            OrderItems = new ObservableCollection<OrderItemViewModel>();
            // Create the event for the collection
            OrderItems.CollectionChanged += OrderItems_CollectionChanged;
            // Call it once to set price string to '0'
            OrderItems_CollectionChanged(null, null);
        }

        #region Events

        /// <summary>
        /// Update price string when item is chaged, added or removed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OrderItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // If list is null or empty, set price to 0
            if (sender == null || ((ObservableCollection<OrderItemViewModel>)sender).Count == 0)
                OrderPriceString = "0";
            else
            {
                // Declare result valur
                float Price = 0;
                // Get the price from all of the items in the list
                ((ObservableCollection<OrderItemViewModel>)sender).ToList().ForEach(o => { Price += o.Price; });
                // Set the pricestring to the calculated value as a string
                OrderPriceString = Price.ToString();
            }

            // Update BasketID's
            for (int i = 0; i < OrderItems.Count; i++)
                OrderItems[i].BasketID = i;
        }
        #endregion

        #region Command Functions

        /// <summary>
        /// Function for the category buttons command
        /// </summary>
        /// <param name="o"></param>
        private void CategoryChanged(object o)
        {
            // Convert object to responding ProductScreen
            if (Enum.TryParse<Enums>(o.ToString(), out Enums res) && res != CurrentMenu)
            {
                CurrentMenu   = res;
                // Get all of the products from the database that is the correct type
                PagedProducts = new ObservableCollection<OrderItemViewModel>(
                                                                             DatabaseData.AllProducts.Where(
                                                                             p => p.Type == res));
            }
        }

        /// <summary>
        /// Add the selected item to the basket
        /// </summary>
        /// <param name="o"></param>
        private void AddProduct(object o) 
        {
            OrderItems.Add(PagedProducts.First(p => p.ProductID.Equals((int)o)));
            OrderItems[OrderItems.Count - 1].BasketID = OrderItems.Count;
        }


        #endregion
    }
}

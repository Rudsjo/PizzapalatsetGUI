namespace CustomerTerminalWPF.ViewModels
{
    /// <summary>
    /// Required namespaces
    /// </summary>
    #region Namespaces
    using System.Collections.ObjectModel;
    #endregion

    /// <summary>
    /// This class can represent any kind of database object
    /// Varibles should be set depending on the kind of object
    /// </summary>
    public class OrderItemViewModel :
                 BaseViewModel
    {
        #region Properties

        #region Properties for all items

        /// <summary>
        /// Product ID
        /// </summary>
        public int ProductID { get; }

        /// <summary>
        /// Refference ID
        /// </summary>
        public int BasketID { get; set; } = -1;

        /// <summary>
        /// Image source for this product
        /// </summary>
        public string ImageSource  { get; set; }

        /// <summary>
        /// What this order is
        /// </summary>
        public Enums Type     { get; set; }

        /// <summary>
        /// Item name
        /// </summary>
        public string Name         { get; set; }

        /// <summary>
        /// Item price
        /// </summary>
        public float Price         { get; set; } = 0;

        /// <summary>
        /// Indexes of all the standard ingredients
        /// </summary>
        public ObservableCollection<int> StandardIngredients        { get; set; }
        /// <summary>
        /// Pizza ingredients
        /// </summary>
        public ObservableCollection<OrderItemViewModel> Ingredients { get; set; }

        /// <summary>
        /// Can this item be edited, view checks this once
        /// </summary>
        public bool IsEditable  { get { return ( Type == Enums.Pizza  ||
                                                 Type == Enums.Sallad ||
                                                 Type == Enums.Pasta) 
                                                 ? true : false; 
            } 
        }

        #endregion

        /// <summary>
        /// These properties will be null it item
        /// is not a pizza
        /// </summary>
        #region Pizza properties

        /// <summary>
        /// Pizza base
        /// </summary>
        public string PizzaBase                  { get; set; }

        #endregion

        #endregion

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="Type"></param>
        public OrderItemViewModel(Enums Type, int ID)
        {
            this.Type = Type;

            if (Type == Enums.Pizza || Type == Enums.Pasta || Type == Enums.Sallad)
                Ingredients = new ObservableCollection<OrderItemViewModel>();

            this.ProductID = ID;
        }

        /// <summary>
        /// returns an independent copy of this class
        /// </summary>
        /// <returns></returns>
        public OrderItemViewModel Copy() => new OrderItemViewModel(Type, this.ProductID) {
                                                                   BasketID            = this.BasketID,
                                                                   ImageSource         = this.ImageSource ?? null,
                                                                   Name                = this.Name        ?? null,
                                                                   Price               = this.Price,
                                                                   StandardIngredients = new ObservableCollection<int>(this.StandardIngredients        ?? new ObservableCollection<int>()),
                                                                   Ingredients         = new ObservableCollection<OrderItemViewModel>(this.Ingredients ?? new ObservableCollection<OrderItemViewModel>())
                                                              };

    }
}

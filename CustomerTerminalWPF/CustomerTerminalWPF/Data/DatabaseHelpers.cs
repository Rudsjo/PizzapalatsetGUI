namespace CustomerTerminalWPF
{
    /// <summary>
    /// Required namespaces
    /// </summary>
    #region Namespaces
    using System.Linq;
    using ViewModels;
    using System.Collections.ObjectModel;
    using BackendHandler;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    #endregion

    /// <summary>
    /// Helpers for the DatabaseHelpers class
    /// </summary>
    public static class DatabaseHelpers
    {
        /// <summary>
        /// Backend for this class
        /// </summary>
        private static IDatabase Backend { get; set; } = Helpers.GetSelectedBackend();

        /// <summary>
        /// Contains all of the available products
        /// </summary>
        public static ObservableCollection<OrderItemViewModel> AllProducts { get; private set; }
        public static ObservableCollection<OrderItemViewModel> AllIngredients { get; private set; }

        /// <summary>
        /// Load all of the items from the database and covnert them into
        /// corresponding collections
        /// </summary>
        /// <returns></returns>
        public static async Task Init()
        {
            AllIngredients = new ObservableCollection<OrderItemViewModel>(await GetAllIngredients().ToListAsync());
            AllProducts = new ObservableCollection<OrderItemViewModel>(await GetAllProductsAsync().ToListAsync());
        }

        /// <summary>
        /// Saves an order to the database
        /// </summary>
        /// <param name="Items">Collection of all the order items</param>
        public static async Task<Order> CreateOrder(ObservableCollection<OrderItemViewModel> Items)
        {
            // Crate an empty order model
            Order _order = new Order()
            {
                ExtraList = new List<Extra>(),
                PizzaList = new List<Pizza>(),
                Price = 0
            };

            /// <summary>
            /// Create all PizzaModel's and add them to the order
            /// </summary>
            #region PizzaModels
            foreach (var p in Items.Where(i => i.Type == Enums.Pizza))
            {
                // Create the pizza model
                var pi = new Pizza { Type = p.Name,
                    Pizzabase = p.PizzaBase,
                    Price = p.Price,
                    StandardIngredientsDeffinition = p.StandardIngredients.ToList(),
                    PizzaIngredients = new List<Condiment>() };

                // Add all ingredients to the pizza model
                foreach (var i in p.Ingredients) pi.PizzaIngredients.Add(new Condiment { Type = i.Name, Price = i.Price });

                // Add the pizza and the price to the order
                _order.PizzaList.Add(pi);
                _order.Price += pi.Price;
            }
            #endregion

            foreach (var e in Items.Where(i => i.Type == Enums.Extra))
            {
                _order.ExtraList.Add(new Extra { Type = e.Name, Price = e.Price });
                _order.Price += e.Price;
            }

            // Wait until the order has been saved to the database
            int OrderID = await Backend.AddOrder(_order);
            // Set the received order id to the order object
            _order.OrderID = OrderID;
            // Return the order with its id
            return _order;
        }

        /// <summary>
        /// Load all items from the database as an IEnumerable<OrderItemViewModel>
        /// </summary>
        /// <returns></returns>
        private static async
            IAsyncEnumerable<OrderItemViewModel> GetAllProductsAsync() {

            // Product index
            int index = 0;

            /// <summary>
            /// Load all of the pizzas from the database and
            /// convert them into OrderItemViewModels
            /// </summary>
            #region Pizzas

            { // > Scope >
                var Pizzas = (await Backend.GetAllPizzas()).GetEnumerator();
                while (Pizzas.MoveNext())
                {
                    var ing = await Backend.GetIngredientsFromSpecificPizza(Pizzas.Current.PizzaID);
                    Pizzas.Current.PizzaIngredients = ing.ToList();
                    Pizzas.Current.StandardIngredientsDeffinition = Pizzas.Current.PizzaIngredients.Select(i => i.CondimentID).ToList();

                    // Create new Pizza
                    var Item = new OrderItemViewModel(Enums.Pizza, index++) { Name      = Pizzas.Current.Type      ,
                                                                              Price     = Pizzas.Current.Price     ,
                                                                              PizzaBase = Pizzas.Current.Pizzabase ,
                                                                              StandardIngredients = new ObservableCollection<int>(Pizzas.Current.StandardIngredientsDeffinition)
                    };

                    // Giv it all of the standatd ingredients
                    Item.StandardIngredients.ToList().ForEach(id => {
                        // Get the correct ingredient
                        OrderItemViewModel Current = AllIngredients.First( i =>
                                                                           i.ProductID.Equals(id) &&
                                                                           i.Type.Equals(Enums.PizzaIngredient));
                        // Add it to the ingredients
                        Item.Ingredients.Add(new OrderItemViewModel(Enums.PizzaIngredient, Current.ProductID) { Price = 0.0F, Name = Current.Name });

                    });

                    yield return Item;
                }
            } // < Endscope <

            #endregion

            /// <summary>
            /// Load all of the Drinks from the database and
            /// convert them into OrderItemViewModels
            /// </summary>
            #region Drinks

            {
                var Drinks = (await Backend.GetAllExtras()).GetEnumerator();
                while (Drinks.MoveNext())
                {
                    yield return new OrderItemViewModel(Enums.Drink, index++)
                    {
                        Name = Drinks.Current.Type,
                        Price = Drinks.Current.Price
                    };
                }

            }

            #endregion

        }

        /// <summary>
        /// Load all of the ingredients from the database as IEnumerable<Ingredient>
        /// </summary>
        private static async
            IAsyncEnumerable<OrderItemViewModel> GetAllIngredients() {
            /// <summary>
            /// Load all of the ingredients from the database and convert them
            /// into ingredient objects
            /// </summary>
            #region Pizza Ingredients

            var PizzaIngredients = (await Backend.GetAllCondiments()).GetEnumerator();
            while (PizzaIngredients.MoveNext())
            {
                yield return new OrderItemViewModel( Enums.PizzaIngredient,
                                                     PizzaIngredients.Current.CondimentID ) {
                                             Price = PizzaIngredients.Current.Price,
                                             Name  = PizzaIngredients.Current.Type };
            }

            #endregion
        }
    }
}

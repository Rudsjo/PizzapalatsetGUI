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
    /// Helper class
    /// </summary>
    public static class DatabaseData
    {
        /// <summary>
        /// Backend for this class
        /// </summary>
        private static IDatabase Backend { get; set; } = Helpers.GetSelectedBackend();

        /// <summary>
        /// Contains all of the available products
        /// </summary>
        public static ObservableCollection<OrderItemViewModel> AllProducts    { get; private set; }
        public static ObservableCollection<OrderItemViewModel> AllIngredients { get; private set; }

        /// <summary>
        /// Load all of the items from the database and covnert them into
        /// corresponding collections
        /// </summary>
        /// <returns></returns>
        public static async Task Init() 
        {
            AllIngredients = new ObservableCollection<OrderItemViewModel> (await GetAllIngredients().ToListAsync()   );
            AllProducts    = new ObservableCollection<OrderItemViewModel> (await GetAllProductsAsync().ToListAsync() );
        }

        public static async void AddOrder(ObservableCollection<OrderItemViewModel> Items)
        {
            Order o = new Order();
            o.PizzaList = new List<Pizza>();
            o.ExtraList = new List<Extra>();
            o.Price = 0;
            foreach(var p in Items.Where(i => i.Type == Enums.Pizza))
            {
                var pi = new Pizza { Pizzabase = p.PizzaBase, Price = p.Price, StandardIngredientsDeffinition = p.StandardIngredients.ToList() };
                pi.PizzaIngredients = new List<Condiment>();
                foreach(var i in p.Ingredients)
                    pi.PizzaIngredients.Add(new Condiment { Type = i.Name, Price = i.Price });
                o.PizzaList.Add(pi);
                o.Price += pi.Price;
            }
            foreach (var e in Items.Where(i => i.Type == Enums.Extra))
            {
                o.ExtraList.Add(new Extra { Type = e.Name, Price = e.Price });
                o.Price += e.Price;
            }

            await Backend.AddOrder(o);
        }

        /// <summary>
        /// Load all items from the database as an IEnumerable<OrderItemViewModel>
        /// </summary>
        /// <returns></returns>
        public static async
            IAsyncEnumerable<OrderItemViewModel> GetAllProductsAsync() {

            // Produt index
            int index = 0;

            /// <summary>
            /// Load all of the pizzas from the database and
            /// convert them into OrderItemViewModels
            /// </summary>
            #region Pizzas

            {
                var Pizzas = (await Backend.GetAllPizzas()).GetEnumerator();
                while (Pizzas.MoveNext())
                {
                    var ing = await Backend.GetIngredientsFromSpecificPizza(Pizzas.Current.PizzaID);
                    Pizzas.Current.PizzaIngredients = ing.ToList();
                    Pizzas.Current.StandardIngredientsDeffinition = Pizzas.Current.PizzaIngredients.Select(i => i.CondimentID).ToList();

                    // Create new Pizza
                    var Item = new OrderItemViewModel(Enums.Pizza, index++) {      Name                = Pizzas.Current.Type,
                                                                                   Price               = Pizzas.Current.Price,
                                                                                   PizzaBase           = Pizzas.Current.Pizzabase,
                                                                                   StandardIngredients = new ObservableCollection<int>(Pizzas.Current.StandardIngredientsDeffinition)                   
                    };

                    // Giv it all of the standatd ingredients
                    Item.StandardIngredients.ToList().ForEach(id => {
                        // Get the correct ingredient
                        OrderItemViewModel Current = AllIngredients.First(i =>
                                                                          i.ProductID.Equals(id) &&
                                                                          i.Type.Equals(Enums.PizzaIngredient));
                        // Add it to the ingredients
                        Item.Ingredients.Add(new OrderItemViewModel(Enums.PizzaIngredient, Current.ProductID) { Price = 0.0F, Name = Current.Name });

                    });

                    yield return Item;
                }
            }

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
                    yield return new OrderItemViewModel(Enums.Drink, index++) { Name  = Drinks.Current.Type,
                                                                                   Price = Drinks.Current.Price };
                }

            }

            #endregion

        }

        /// <summary>
        /// Load all of the ingredients from the database as IEnumerable<Ingredient>
        /// </summary>
        public static async
            IAsyncEnumerable<OrderItemViewModel> GetAllIngredients()
        
        {
            /// <summary>
            /// Load all of the ingredients from the database and convert them
            /// into ingredient objects
            /// </summary>
            #region Pizza Ingredients

            var PizzaIngredients = (await Backend.GetAllCondiments()).GetEnumerator();
            while (PizzaIngredients.MoveNext())
            {
                yield return new OrderItemViewModel(Enums.PizzaIngredient, 
                                                    PizzaIngredients.Current.CondimentID)  { 
                                                    Price = PizzaIngredients.Current.Price,
                                                    Name = PizzaIngredients.Current.Type   };
            }

            #endregion
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace CookingTerminal
{
    /// <summary>
    /// ViewModel of the model of Pizza
    /// </summary>
    public class PizzaViewModel
    {
        public int PizzaID { get; set; }
        public string Type { get; set; }
        public float Price { get; set; }
        public int PizzabaseID { get; set; }
        public string Pizzabase { get; set; }
        public List<int> StandardIngredientsDeffinition { get; set; }
        public List<BackendHandler.Condiment> PizzaIngredients { get; set; }

        /// <summary>
        /// The status of the pizza when an order has been chosen to be cooked.
        ///     1 = Waiting to be cooked
        ///     2 = Cooked
        /// </summary>
        public int CookingStatus { get; set; } = 1;

    }
}

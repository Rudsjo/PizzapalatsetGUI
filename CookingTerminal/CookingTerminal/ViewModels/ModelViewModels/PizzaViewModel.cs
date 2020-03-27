using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace CookingTerminal
{
    /// <summary>
    /// ViewModel of the model of Pizza
    /// </summary>
    public class PizzaViewModel : BaseViewModel
    {
        public int PizzaID { get; set; }
        public string Type { get; set; }
        public float Price { get; set; }
        public int PizzabaseID { get; set; }
        public string Pizzabase { get; set; }
        public List<int> StandardIngredientsDeffinition { get; set; }
        public List<BackendHandler.Condiment> PizzaIngredients { get; set; }

        /// <summary>
        /// The cooking status of the pizza
        /// </summary>

        public CookingStatus CookingStatus { get; set; } = CookingStatus.IsNotCooked;

        /// <summary>
        /// The Content of the Cooking Button
        /// </summary>
        public string CookingButtonContent { get; set; } = "Stoppa i ugn";

    }
}

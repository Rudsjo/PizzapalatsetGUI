using System;
using System.Collections.Generic;
using System.Text;

namespace AdminTerminal_v2
{
    public class PizzaViewModel
    {
        public int PizzaID { get; set; }
        public string Type { get; set; }
        public float Price { get; set; }
        public int PizzabaseID { get; set; }
        public string Pizzabase { get; set; }
        public List<int> StandardIngredientsDeffinition { get; set; }
        public List<BackendHandler.Condiment> PizzaIngredients { get; set; }

    }
}

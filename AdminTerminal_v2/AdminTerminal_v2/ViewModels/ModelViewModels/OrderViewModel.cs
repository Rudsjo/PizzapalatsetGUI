using System;
using System.Collections.Generic;
using System.Text;

namespace AdminTerminal_v2
{
    public class OrderViewModel
    {
        public int OrderID { get; set; }
        public float Price { get; set; }
        public int Status { get; set; }
        public List<BackendHandler.Extra> ExtraList { get; set; }
        public List<BackendHandler.Pizza> PizzaList { get; set; }

    }
}

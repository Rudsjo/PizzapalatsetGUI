using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace CookingTerminal
{
    public class OrderViewModel
    {
        /// <summary>
        /// The unique ID for the order
        /// </summary>
        public int OrderID { get; set; }
        public float Price { get; set; }
        public int Status { get; set; }
        public List<BackendHandler.Extra> ExtraList { get; set; }
        public List<BackendHandler.Pizza> PizzaList { get; set; }

        /// <summary>
        /// The waiting time since the order was ordered
        /// </summary>
        public Timer WaitingTime { get; set; }

    }
}

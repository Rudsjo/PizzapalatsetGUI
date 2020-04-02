using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Windows.Threading;

namespace CookingTerminal
{
    /// <summary>
    /// ViewModel of the model of Pizza
    /// </summary>
    public class PizzaViewModel : BaseViewModel
    {
        #region Standard Properties

        public int PizzaID { get; set; }
        public string Type { get; set; }
        public float Price { get; set; }
        public int PizzabaseID { get; set; }
        public string Pizzabase { get; set; }
        public List<int> StandardIngredientsDeffinition { get; set; }
        public List<BackendHandler.Condiment> PizzaIngredients { get; set; }

        #endregion

        #region Added Properties

        /// <summary>
        /// The cooking status of the pizza
        /// </summary>

        public CookingStatus CookingStatus { get; set; } = CookingStatus.IsNotCooked;

        /// <summary>
        /// The Content of the Cooking Button
        /// </summary>
        public string CookingButtonContent { get; set; } = "Stoppa i ugn";

        /// <summary>
        /// The time it takes for a pizza to get ready in the oven
        /// </summary>
        public int CookingTime { get; set; } = 3;

        /// <summary>
        /// The index of the pizza in the list of orders to be cooked
        /// </summary>
        public int ListIndex { get; set; }

        /// <summary>
        /// The timer to start when some food is placed in the oven
        /// </summary>
        public DispatcherTimer CookingTimer { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public PizzaViewModel()
        {
            // Creating the timer and setting the interval to 1 second
            CookingTimer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 1) };
        }

        #endregion


        #region Public Methods

        /// <summary>
        /// Starts the cooking timer
        /// </summary>
        public void StartTimer()
        {
            // Creating the timer and setting the interval to 1 second
            CookingTimer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 1) };

            CookingTimer.Start();

            //CookingButtonContent = TimeSpan.FromSeconds(CookingTime).ToString("mm\\:ss");

            // Triggering the event when the interval has elapsed
            CookingTimer.Tick += CookingTimer_Tick;
        }

        /// <summary>
        /// Event to run every time the interval of the Cooking Timer runs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CookingTimer_Tick(object sender, EventArgs e)
        {
            if (CookingTime > 1)
            {
                CookingTime--;
                CookingButtonContent = TimeSpan.FromSeconds(CookingTime).ToString("mm\\:ss");
            }
            else
            {
                CookingTimer.Stop();
                CookingButtonContent = "Ta ut ur ugn";
                CookingStatus = CookingStatus.IsCooked;
            }
        }

        #endregion
    }
}

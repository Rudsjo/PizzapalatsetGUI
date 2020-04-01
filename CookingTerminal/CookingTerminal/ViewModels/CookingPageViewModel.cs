using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace CookingTerminal
{
    public class CookingPageViewModel : BaseViewModel
    {

        #region Private Members
        /// <summary>
        /// The selected order to cook
        /// </summary>
        private OrderViewModel _orderToCook { get; set; }

        #endregion

        #region Public Properties

        /// <summary>
        /// The lsit of foods to cook
        /// </summary>
        public ObservableCollection<PizzaViewModel> FoodsToCook { get; set; }

        /// <summary>
        /// The list of cooked foods
        /// </summary>
        public ObservableCollection<PizzaViewModel> CookedFoods { get; set; }

        /// <summary>
        /// The visibility property of the serve button
        /// It goes visible when there is no more foods to cook
        /// </summary>
        public bool ServeButtonVisibility { get; set; } = false;

        #endregion

        #region Commands

        /// <summary>
        /// Command to put a pizza in the oven
        /// </summary>
        public ICommand CookFood { get; set; }


        /// <summary>
        /// Command to complete an order and send it to cashier
        /// </summary>
        public ICommand MarkOrderAsServed { get; set; }

        #endregion

        #region Constructor
        public CookingPageViewModel()
        {

            // Getting the chosen order from the static reference
            _orderToCook = OrderToCook;

            // Creating new lists
            FoodsToCook = new ObservableCollection<PizzaViewModel>();
            CookedFoods = new ObservableCollection<PizzaViewModel>();

            // Gets all pizzas from the chosen order
            Task.Run(async () => await GetFoodsFromOrder()).Wait();

            // Updates the lists
            //UpdateCookingList();

            // Creating new commands
            CookFood = new RelayCommand(CookFoodCommand);
            Logout = new RelayCommand(RunLogout);
            MarkOrderAsServed = new RelayCommand((o) => { });

           }

        #endregion

        #region Private Methods

        /// <summary>
        /// Converts all sent in foods to their ViewModelTypes
        /// </summary>
        /// <returns></returns>
        private async Task GetFoodsFromOrder()
        {
            // Getting all the foods
            FoodsToCook = await ViewModelhelpers.FillListFromDatabase<BackendHandler.Pizza, PizzaViewModel>(_orderToCook.PizzaList);

            // Setting the index
            foreach(var food in FoodsToCook)
            {
                food.ListIndex = FoodsToCook.IndexOf(food);
            }
        }

        /// <summary>
        /// Checks if a food has been cooked and updates the lists
        /// </summary>
        private void UpdateCookingList(PizzaViewModel food)
        {
            // Changes place of the cooked food
            CookedFoods.Add(food);
            FoodsToCook.Remove(food);

            // Checks if there is any food left to cook
            // If it's not, the serve button will be visible
            if (FoodsToCook.Count == 0)
                ServeButtonVisibility = true;
        }


        #endregion

        #region Command Methods

        /// <summary>
        /// Command to be used to cook or take out food from the oven.
        /// Also starts the timer when the pizza goes in the oven.
        /// </summary>
        /// <param name="foodID">The ID of the food to cook</param>
        public void CookFoodCommand(object index)
        {
            // Get the selected food
            var CookedPizza = FoodsToCook.Where(x => x.ListIndex == (int)index).First();

            // Check the Cooking status of the pizza
            switch (CookedPizza.CookingStatus)
            {
                // If the food is not cooked and the button is clicked...
                case CookingStatus.IsNotCooked:
                    {
                        // The status will change to cooking
                        CookedPizza.CookingStatus = CookingStatus.IsCooking;

                        // And the content will update to the timer
                        CookedPizza.CookingButtonContent = TimeSpan.FromSeconds(CookedPizza.CookingTime).ToString("mm\\:ss");

                        // And the timer will start
                        // The timer will set the status to cooked when it's finished
                        CookedPizza.StartTimer();

                        return;
                    }

                // When the food is cooked...
                case CookingStatus.IsCooked:
                    {
                        // The list will update
                        UpdateCookingList(CookedPizza);
                        return;
                    }
            }
        }

        #endregion
    }
}

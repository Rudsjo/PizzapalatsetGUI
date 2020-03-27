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

        private DispatcherTimer CookingTimer { get; set; }

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
        public Visibility ServeButtonVisibility { get; set; } = Visibility.Hidden;

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
            UpdateCookingList();

            // Creating new commands
            CookFood = new RelayCommand((object pizzaID) =>
            {

                // Get the selected pizza
                var CookedPizza = FoodsToCook.Where(x => x.PizzaID == (int)pizzaID).First();

                // Check the Cooking status of the pizza
                switch (CookedPizza.CookingStatus)
                {
                    case CookingStatus.IsNotCooked:
                        {
                            CookedPizza.CookingStatus = CookingStatus.IsCooked;
                            CookedPizza.CookingButtonContent = "Ta ut ur ugn";
                            return;
                        }

                    // Placeholder in case of implementation of cooking timer
                    case CookingStatus.IsCooking:
                        {
                            CookedPizza.CookingStatus = CookingStatus.IsCooked;
                            CookedPizza.CookingButtonContent = "Ta ut ur ugn";
                            return;
                        }

                    case CookingStatus.IsCooked:
                        {
                            UpdateCookingList();
                            return;
                        }
                }
                
            });
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Converts all sent in foods to their ViewModelTypes
        /// </summary>
        /// <returns></returns>
        private async Task GetFoodsFromOrder()
        {
            FoodsToCook = await ViewModelhelpers.FillListFromDatabase<BackendHandler.Pizza, PizzaViewModel>(_orderToCook.PizzaList);
        }

        /// <summary>
        /// Checks if a food has been cooked and updates the lists
        /// </summary>
        private void UpdateCookingList()
        {
            // Goes thorugh all foods and places them in the right list
            FoodsToCook.ToList().ForEach(food => 
            {
                if (food.CookingStatus == CookingStatus.IsCooked)
                {
                    CookedFoods.Add(food);
                    FoodsToCook.Remove(food);
                }
            });


            // Checks if there is any food left to cook
            // If it's not, the serve button will be visible
            if (FoodsToCook.Count == 0)
                ServeButtonVisibility = Visibility.Visible;
        }

        #endregion
    }
}

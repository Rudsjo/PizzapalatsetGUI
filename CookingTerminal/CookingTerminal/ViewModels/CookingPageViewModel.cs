using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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
        public ObservableCollection<PizzaViewModel> FoodsToCook { get; set; }

        public ObservableCollection<PizzaViewModel> CookedFoods { get; set; }

        public bool IsVisible { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// Command to put a pizza in the oven
        /// </summary>
        public ICommand PutPizzaInOven { get; set; }

        /// <summary>
        /// Command to take out a pizza from the oven
        /// </summary>
        public ICommand TakeOutPizzaFromOven { get; set; }

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
            PutPizzaInOven = new RelayCommand((object pizzaID) => 
            {
                // Get the selected pizza
                var CookedPizza = FoodsToCook.Where(x => x.PizzaID == (int)pizzaID).First();
                // Changes the cooking status
                CookedPizza.CookingStatus = 2;
                // Updates the list
                UpdateCookingList();
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
            FoodsToCook.ToList().ForEach(food => 
            {
                if (food.CookingStatus == 2)
                {
                    CookedFoods.Add(food);
                    FoodsToCook.Remove(food);
                }


            });
        }

        #endregion


    }
}

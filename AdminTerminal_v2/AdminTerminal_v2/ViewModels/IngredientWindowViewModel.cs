using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AdminTerminal_v2
{
    public class IngredientWindowViewModel : BaseViewModel
    {
        #region Public Properties
        public PizzaViewModel PizzaToUpdate { get; set; }

        public BackendHandler.Condiment CondimentToAdd { get; set; }

        public BackendHandler.Condiment CondimentToRemove { get; set; }

        public IEnumerable AllAvailableIngredientsList { get; set; }

        public ObservableCollection<BackendHandler.Condiment> CondimentsOnPizza { get; set; }

        #region Sizes

        /// <summary>
        /// The minimum popup window height
        /// </summary>
        public static int MinPopupWindowHeight { get; set; } = (MainWindowViewModel.MinWindowHeight - 100);

        /// <summary>
        /// The minimum popup window width
        /// </summary>
        public static int MinPopupWindowWidth { get; set; } = (MainWindowViewModel.MinWindowWidth - 100);

        /// <summary>
        /// The maximum height of the popup window
        /// </summary>
        public static int MaxPopupWindowHeight { get; set; } = (MainWindowViewModel.MaxWindowHeight - 100);

        /// <summary>
        /// The maximum width of the popup window
        /// </summary>
        public static int MaxPopupWindowWidth { get; set; } = (MainWindowViewModel.MaxWindowWidth - 100);

        #endregion

        #endregion

        #region Command Properties

        public ICommand AddIngredient { get; set; }

        public ICommand RemoveIngredient { get; set; }

        public ICommand Confirm { get; set; }

        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public IngredientWindowViewModel(PizzaViewModel pizza)
        {
            PizzaToUpdate = pizza;
            //{ Type = CurrentType, Price = 80, PizzabaseID = 1 };

            LoadIngredientsToExistingPizza();

            CondimentsOnPizza = new ObservableCollection<BackendHandler.Condiment>();
           
            AddIngredient = new RelayParameterlessCommand(AddIngredientCommand);
            RemoveIngredient = new RelayParameterlessCommand(RemoveIngredientCommand);
            Confirm = new RelayParameterlessCommand(ConfirmCommand);

            UpdateIngredientLists();
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Command to add the selected ingredient in a list to the pizza
        /// </summary>
        public void AddIngredientCommand(object e)
        {
            CondimentsOnPizza.Add(CondimentToAdd);

            UpdateIngredientLists();
        }

        /// <summary>
        /// Command to remove a selected ingredient in a list from the pizza
        /// </summary>
        public void RemoveIngredientCommand(object e)
        {
            CondimentsOnPizza.Remove(CondimentToRemove);

            AllAvailableIngredientsList.Cast<BackendHandler.Condiment>().ToList().Add(CondimentToRemove);

            UpdateIngredientLists();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public void ConfirmCommand(object e)
        {
            BackendHandler.Pizza newPizza = new BackendHandler.Pizza();

            newPizza.Type = PizzaToUpdate.Type;
            newPizza.PizzabaseID = PizzaToUpdate.PizzabaseID;
            newPizza.PizzaIngredients = PizzaToUpdate.PizzaIngredients;
            newPizza.Price = PizzaToUpdate.Price;

            rep.AddPizza(newPizza);

            IngredientPopupWindow.DialogResult = true;
            
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Method to load previously added ingredients to the chosen pizza
        /// </summary>
        /// <returns></returns>
        public async Task LoadIngredientsToExistingPizza()
        {
            if (PizzaToUpdate.PizzaID != 0)
            {
                // getting all ingredients of the sent in pizza
                var ingredients = await rep.GetIngredientsFromSpecificPizza(PizzaToUpdate.PizzaID);

                // and updates the pizza with it
                PizzaToUpdate.PizzaIngredients = ingredients.ToList();

                // Putting all the ingredients that a pizza has to the temporary condiment list
                PizzaToUpdate.PizzaIngredients.ForEach(x =>
                {
                    CondimentsOnPizza.Add(x);
                });
            }
        }

        /// <summary>
        /// Method to update the list of available ingredients
        /// </summary>
        /// <returns></returns>
        public async Task UpdateIngredientLists()
        {
            // Getting all ingredients from the database...
            AllAvailableIngredientsList = await rep.GetAllCondiments();

            // ...and casting it to queryable list
            var IngredientsToChooseFrom = AllAvailableIngredientsList.Cast<BackendHandler.Condiment>().ToList();

            // Goes through the already set ingredients
            CondimentsOnPizza.Cast<BackendHandler.Condiment>().ToList().ForEach(x =>
            {
                // and removing them from the list of available condiments
                for(int i = 0; i < IngredientsToChooseFrom.Count; i++)
                {
                    if (x.CondimentID == IngredientsToChooseFrom[i].CondimentID)
                        IngredientsToChooseFrom.Remove(IngredientsToChooseFrom[i]);
                }

            });

            // Updates the list of available ingredients
            AllAvailableIngredientsList = IngredientsToChooseFrom;

            PizzaToUpdate.PizzaIngredients = CondimentsOnPizza.Cast<BackendHandler.Condiment>().ToList();
        }

        #endregion


    }
}

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
    /// <summary>
    /// View Model of the Ingredient Window that pops up when a user adds or updates a pizza
    /// </summary>
    public class IngredientWindowViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Porperty of a the pizza to hold information before adding/updating the database
        /// </summary>
        public PizzaViewModel BasePizza { get; set; }

        /// <summary>
        /// The selected item in the list of available condiments
        /// </summary>
        public BackendHandler.Condiment CondimentToAdd { get; set; }

        /// <summary>
        /// The selected item in the lsit of pizza condiments
        /// </summary>
        public BackendHandler.Condiment CondimentToRemove { get; set; }

        /// <summary>
        /// The list of all available condiments
        /// </summary>
        public IEnumerable AllAvailableIngredientsList { get; set; }

        /// <summary>
        /// The list of items on the pizza in the UI
        /// </summary>
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
        /// Default constructor that takes in a pizza with values set in the main window
        /// </summary>
        public IngredientWindowViewModel(PizzaViewModel pizza)
        {
            // Sets the Pizza property to the sent in pizza
            BasePizza = pizza;

            // And creates a new instance of its ingredientlist...
            BasePizza.PizzaIngredients = new List<BackendHandler.Condiment>();

            // ...then loads the already existing condiments (if there is any) into that list
            LoadIngredientsToExistingPizza();

            // Creating a new instance of the temporary list to hold condiments
            CondimentsOnPizza = new ObservableCollection<BackendHandler.Condiment>();
           
            // Calling commands
            AddIngredient = new RelayParameterlessCommand(AddIngredientCommand);
            RemoveIngredient = new RelayParameterlessCommand(RemoveIngredientCommand);
            Confirm = new RelayParameterlessCommand(ConfirmCommand);

            // Fills upp the lists
            UpdateIngredientLists();
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Command to add the selected ingredient in a list to the pizza
        /// </summary>
        public void AddIngredientCommand(object e)
        {
            // Adds the selected condiment to the pizza
            CondimentsOnPizza.Add(CondimentToAdd);

            // And updates the UI
            UpdateIngredientLists();
        }

        /// <summary>
        /// Command to remove a selected ingredient in a list from the pizza
        /// </summary>
        public void RemoveIngredientCommand(object e)
        {
            // Removes the selected condiment
            CondimentsOnPizza.Remove(CondimentToRemove);

            // And adds it back to the list of available condiments
            AllAvailableIngredientsList.Cast<BackendHandler.Condiment>().ToList().Add(CondimentToRemove);

            // Updates the lists
            UpdateIngredientLists();
        }

        /// <summary>
        /// Command to run when the user clicks confirm in the UI
        /// </summary>
        /// <param name="e"></param>
        public void ConfirmCommand(object e)
        {
            // Method to add or update a pizza in the database depending on the user choice
            AddOrUpdatePizza();

            // Closes the window and returns true since the user pressed the button
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
            if (BasePizza.PizzaID != 0)
            {
                // getting all ingredients of the sent in pizza
                var ingredients = await rep.GetIngredientsFromSpecificPizza(BasePizza.PizzaID);

                // and updates the pizza with it
                BasePizza.PizzaIngredients = ingredients.ToList();

                // Putting all the ingredients that a pizza has to the temporary condiment list
                BasePizza.PizzaIngredients.ForEach(x =>
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

            BasePizza.PizzaIngredients = CondimentsOnPizza.Cast<BackendHandler.Condiment>().ToList();
        }

        #endregion

        #region Command Helpers

        /// <summary>
        /// Method to update or add a pizza in the database depending on the user choice
        /// </summary>
        /// <returns></returns>
        private async Task AddOrUpdatePizza()
        {
            // Creating a new instance of a Model-Pizza
            BackendHandler.Pizza PizzaToAddOrUpdate = new BackendHandler.Pizza();

            // Checking if the pizza is a new one or one to update by checking if it has an ID
            if(BasePizza.PizzaID != 0)
                PizzaToAddOrUpdate = await rep.GetSinglePizza(BasePizza.PizzaID);

            // Setting all the base values from the previous page to the pizza
            PizzaToAddOrUpdate.Type = BasePizza.Type;
            PizzaToAddOrUpdate.PizzabaseID = BasePizza.PizzabaseID;
            PizzaToAddOrUpdate.Price = BasePizza.Price;

            // Checking if the pizza is a new one or one to update by checking if it has an ID
            // to see what methods to use towards the database
            if (PizzaToAddOrUpdate.PizzaID != 0)
            {
                // we instantly start by temporarily remove all ingredients from the pizza
                foreach (var condiment in PizzaToAddOrUpdate.PizzaIngredients)
                {
                    await rep.DeleteCondimentFromPizza(PizzaToAddOrUpdate, condiment);
                    PizzaToAddOrUpdate.PizzaIngredients.Remove(condiment);
                }

                // Then we set the pizza ingredients to the same as the UI list
                PizzaToAddOrUpdate.PizzaIngredients = BasePizza.PizzaIngredients;

                // and send them back in to the database
                await rep.AddCondimentToPizza(PizzaToAddOrUpdate);

                // lastly we update the pizzas name, price etc.
                await rep.UpdatePizza(PizzaToAddOrUpdate);
            }

            else
            {
                await rep.AddPizza(PizzaToAddOrUpdate);
                var PizzaToAdd = (await rep.GetAllPizzas()).Last();
                PizzaToAdd.PizzaIngredients = BasePizza.PizzaIngredients;
                await rep.AddCondimentToPizza(PizzaToAdd);
            }
        }

        #endregion


    }
}

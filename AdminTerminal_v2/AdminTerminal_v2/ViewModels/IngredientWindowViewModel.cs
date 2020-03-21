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

        public IAsyncCommand AddIngredient { get; set; }

        public IAsyncCommand RemoveIngredient { get; set; }

        public IAsyncCommand Confirm { get; set; }

        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor that takes in a pizza with values set in the main window
        /// </summary>
        public IngredientWindowViewModel(PizzaViewModel pizza)
        {
            // Sets the Pizza property to the sent in pizza
            BasePizza = new PizzaViewModel();
            BasePizza = pizza;

            // And creates a new instance of its ingredientlist...
            BasePizza.PizzaIngredients = new List<BackendHandler.Condiment>();

            // ...then loads the already existing condiments (if there is any) into that list
            LoadIngredientsToExistingPizza();

            // Creating a new instance of the temporary list to hold condiments
            CondimentsOnPizza = new ObservableCollection<BackendHandler.Condiment>();
           
            // Calling commands
            AddIngredient = new RelayAsyncCommand(AddIngredientCommand);
            RemoveIngredient = new RelayAsyncCommand(RemoveIngredientCommand);
            Confirm = new RelayAsyncCommand(ConfirmCommand);

            // Fills upp the lists
            UpdateIngredientLists();
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Command to add the selected ingredient in a list to the pizza
        /// </summary>
        public async Task AddIngredientCommand()
        {
            // Adds the selected condiment to the pizza
            CondimentsOnPizza.Add(CondimentToAdd);

            // And updates the UI
            await UpdateIngredientLists();
        }

        /// <summary>
        /// Command to remove a selected ingredient in a list from the pizza
        /// </summary>
        public async Task RemoveIngredientCommand()
        {
            // Removes the selected condiment
            CondimentsOnPizza.Remove(CondimentToRemove);

            // And adds it back to the list of available condiments
            AllAvailableIngredientsList.Cast<BackendHandler.Condiment>().ToList().Add(CondimentToRemove);

            // Updates the lists
            await UpdateIngredientLists();
        }

        /// <summary>
        /// Command to run when the user clicks confirm in the UI
        /// </summary>
        /// <param name="e"></param>
        public async Task ConfirmCommand()
        {

                // Method to add or update a pizza in the database depending on the user choice
                await AddOrUpdatePizza2();

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

                // Ordering the ingredients
                var OrderedIngredients = ingredients.OrderBy(x => x.CondimentID);

                // and updates the pizza with it
                BasePizza.PizzaIngredients = OrderedIngredients.ToList();

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

        private async Task AddOrUpdatePizza2()
        {
            // If the pizza has a sent in ID it means it salready exists and should be updated
            if(BasePizza.PizzaID > 0)
            {
                // Getting the pizza from the database to be able to update it
                var PizzaToUpdate = await rep.GetSinglePizza(BasePizza.PizzaID);

                // Get the previous ingredients from the sent in pizza
                var PizzaToUpdateIngredientsList = (await rep.GetIngredientsFromSpecificPizza(PizzaToUpdate.PizzaID)).ToList();

                // To make sure we update all the ingredients correctly,
                // we first delete all existing condiments from the sent in pizza
                foreach(var condiment in PizzaToUpdateIngredientsList)
                {
                    await rep.DeleteCondimentFromPizza(PizzaToUpdate, condiment);
                }

                // Then setting the actual ingredients to the pizza
                PizzaToUpdate.PizzaIngredients = BasePizza.PizzaIngredients;

                // And sending it back to the database
                await rep.AddCondimentToPizza(PizzaToUpdate);

                // Setting all the new values to the pizza
                PizzaToUpdate.Type = BasePizza.Type;
                PizzaToUpdate.Price = BasePizza.Price;
                PizzaToUpdate.PizzabaseID = BasePizza.PizzabaseID;

                // And updating the pizza in the database
                await rep.UpdatePizza(PizzaToUpdate);

            }

            // Else it's a new pizza
            else
            {
                // Creating an instance of a new Pizza with the sent in properties
                var PizzaToAdd = new BackendHandler.Pizza() { Type = BasePizza.Type, Price = BasePizza.Price, PizzabaseID = BasePizza.PizzabaseID };

                // Adding the pizza to the database
                await rep.AddPizza(PizzaToAdd);

                // Calling it back to get it with its ID
                PizzaToAdd = (await rep.GetAllPizzas()).Last();

                // Adding the ingredients
                PizzaToAdd.PizzaIngredients = BasePizza.PizzaIngredients;

                // And sending the information back to the database
                await rep.AddCondimentToPizza(PizzaToAdd);
            }
        }

        #endregion


    }
}

namespace CustomerTerminalWPF.ViewModels
{
    /// <sumamry>
    /// Required namespaces
    /// </sumamry>
    #region Namespaces
    using CustomerTerminalWPF.ViewModels.Commands;
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    #endregion

    /// <summary>
    /// ViewModel for the EditObject frame
    /// </summary>
    public class EditObjectViewModel :
                 BaseViewModel
    {
        /// <summary>
        /// Command for the 'Accept' button
        /// </summary>
        public RelayCommand AcceptCommand           { get; set; }
        /// <summary>
        /// Command for the 'Add' button
        /// </summary>
        public RelayCommand AddIngredientCommand    { get; set; }
        /// <summary>
        /// Command for the 'Remove' button
        /// </summary>
        public RelayCommand RemoveIngredientCommand { get; set; }

        /// <summary>
        /// The item to be edited
        /// </summary>
        public OrderItemViewModel CurrentItem       { get; set; }

        /// <summary>
        /// Available ingredients
        /// </summary>
        public ObservableCollection<OrderItemViewModel> Ingredients { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public EditObjectViewModel()
        {
            // Load data if program is running
            if (ProgramState.IsRunning) Init();
        }

        /// <summary>
        /// Create all of the logic for this class
        /// </summary>
        private void Init()
        {
            // Get the selected item from the OrderViewModel
            CurrentItem = OrderPageViewModel.VM.SelectedItem.Copy();

            // Event for the current items ingredients list
            CurrentItem.Ingredients.CollectionChanged += (sender, e) => UpdatePrices();

            // Load the ingredients available for this item
            switch (CurrentItem.Type)
            {
                // Load all of the pizza ingredients
                case Enums.Pizza:
                    /* Copy over all of the ingredients to this class's list so 
                     * that display prices can be changed without effecting the source
                     */
                    Ingredients = new ObservableCollection<OrderItemViewModel>();
                    DatabaseData.AllIngredients.ToList().Where(i => i.Type == Enums.PizzaIngredient).ToList().ForEach(i => { Ingredients.Add(i.Copy()); });
                    break;

                // Should never go here
                default: throw new Exception("Invalid object");
            }

            // Check once if the pizza is standard or if it has been changed before
            UpdatePrices();

            #region Create the commands

            // Create the accept command
            AcceptCommand = new RelayCommand((o) => {
                // Return the new item and close this page
                OrderPageViewModel.VM.SelectedItem = CurrentItem.Copy();
                OrderPageViewModel.VM.SubPage = OrderSubPages.None;
            });

            // Create the AddIngredient command
            AddIngredientCommand = new RelayCommand((o) => {
                CurrentItem.Price += ((OrderItemViewModel)o).Price;
                CurrentItem.Ingredients.Add(((OrderItemViewModel)o).Copy()); 
            });

            // Create the AddIngredient command
            RemoveIngredientCommand = new RelayCommand((o) =>
            {
                /* If a free item is removed and the object contains 1 or more of the same
                 * ingredient that is an extra and not free, remove one of those first and
                 * keep the one that is free.
                 */
                if (((OrderItemViewModel)o).Price.Equals(0) && CurrentItem.Ingredients.ToList().Where(i => i.ProductID.Equals(((OrderItemViewModel)o).ProductID) && i.Price > 0).Count() > 0)
                {
                    CurrentItem.Price -= CurrentItem.Ingredients.First(i => i.ProductID.Equals(((OrderItemViewModel)o).ProductID) && i.Price > 0).Price;
                    CurrentItem.Ingredients.Remove(CurrentItem.Ingredients.First(i => i.ProductID.Equals(((OrderItemViewModel)o).ProductID) && i.Price > 0));
                }

                /* Else if an extra ingredient is removed, just 
                 * remove that one as normal
                 */
                else
                {
                    CurrentItem.Price -= ((OrderItemViewModel)o).Price;
                    CurrentItem.Ingredients.Remove((OrderItemViewModel)o);
                }
            });

            #endregion
        }

        #region Methods

        /// <summary>
        /// Updates the display prices
        /// </summary>
        private void UpdatePrices()
        {
            /* The correct price should be displayed, so this
             * loop compares the ingredients in the
             * current item with the standard ingredients
             * and sets the correct prices based on the calculation
             */
            for (int index = 0; index < Ingredients.Count(); index++)
            {
                OrderItemViewModel Ing = Ingredients[index];
                if (CurrentItem.StandardIngredients.Contains(Ing.ProductID))
                    Ingredients[index].Price = (CurrentItem.StandardIngredients.Where(i => i.Equals(Ing.ProductID)).Count() > CurrentItem.Ingredients.Where(i => i.ProductID.Equals(Ing.ProductID)).Count())
                    ? 0 : DatabaseData.AllIngredients.First(i => i.Type == Ing.Type && i.ProductID == Ing.ProductID).Price;
                else Ingredients[index].Price = DatabaseData.AllIngredients.First(i => i.Type == Ing.Type && i.ProductID == Ing.ProductID).Price;
            }
        }

        #endregion
    }
}

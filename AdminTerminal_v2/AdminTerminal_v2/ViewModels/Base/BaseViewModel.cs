﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BackendHandler;

namespace AdminTerminal_v2
{
    /// <summary>
    /// A base view model that fires Property Changed events as needed
    /// </summary>
    /// 
    public class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The event that is fired when any child-property changes its value
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };


        #region Private Members
        /// <summary>
        /// The property used to save the items from database
        /// </summary>
        public IEnumerable DatabaseList { get; set; }

        /// <summary>
        /// The current employee selected
        /// </summary>
        private EmployeeViewModel employee;

        /// <summary>
        /// The current condiment selected
        /// </summary>
        private CondimentViewModel condiment;

        /// <summary>
        /// The current extra selected
        /// </summary>
        private ExtraViewModel extra;

        /// <summary>
        /// The current pizza selected
        /// </summary>
        private PizzaViewModel pizza;

        /// <summary>
        /// The current price of a condiment, extra or pizza
        /// </summary>
        private string currentPrice;

        /// <summary>
        /// The current old order selected
        /// </summary>
        private OrderViewModel oldOrder;

        #endregion

        #region Public Properties

        /// <summary>
        /// Bool to indicate if a task i running or not
        /// </summary>
        public bool TaskIsRunning { get; set; }

        /// <summary>
        /// The repository to use
        /// </summary>
        public IDatabase rep { get; set; } = Helpers.GetSelectedBackend();


        /// <summary>
        /// The List to showcase in the UI
        /// </summary>
        public IEnumerable CurrentList { get; set; }

        /// <summary>
        /// The property used to showcase the ID of a selected item
        /// </summary>
        public string CurrentID { get; set; }

        /// <summary>
        /// The property to showcase the price of a selected item
        /// </summary>
        public string CurrentPrice
        {
            get { return currentPrice; }
            set
            {
                // if the price is 0, the textbox-field will be empty
                // this is to prevent the textbox to show "0" before any item is selected
                if (value == "0")
                    value = "";

                currentPrice = value;
            }
        }

        /// <summary>
        /// The property to showcase the type/name of a selected item
        /// </summary>
        public string CurrentType { get; set; }

        /// <summary>
        /// The public property to showcase the role of the selected employee
        /// </summary>
        public string CurrentRole { get; set; }

        /// <summary>
        /// The property to set the pizzabase of a pizza
        /// </summary>
        public int CurrentPizzaBaseID { get; set; } = 1;

        /// <summary>
        /// The propety of the pizzabase
        /// </summary>
        public string CurrentPizzaBase { get; set; }

        /// <summary>
        /// The public property of the selected employee
        /// </summary>
        public EmployeeViewModel Employee
        {
            get { return employee; }
            set
            {
                if (employee == value || value == null)
                    return;

                employee = value;

                CurrentRole = employee.Role;

                if (employee.UserID != 0)
                    CurrentID = employee.UserID.ToString();
            }
        }

        /// <summary>
        /// The public property of the selected condiment
        /// </summary>
        public CondimentViewModel Condiment
        {
            get { return condiment; }

            set
            {
                if (condiment == value || value == null)
                    return;

                condiment = value;

                if (condiment.CondimentID != 0)
                    CurrentID = condiment.CondimentID.ToString();

                CurrentType = condiment.Type;
                CurrentPrice = condiment.Price.ToString();
            }
        }

        public ExtraViewModel Extra
        {
            get { return extra; }

            set
            {
                if (extra == value || value == null)
                    return;

                extra = value;

                if (extra.ProductID != 0)
                    CurrentID = extra.ProductID.ToString();

                CurrentType = extra.Type;
                CurrentPrice = extra.Price.ToString();
            }
        }

        public PizzaViewModel Pizza
        {
            get { return pizza; }
            set
            {
                if (pizza == value || value == null)
                    return;

                pizza = value;

                if (pizza.PizzaID != 0)
                    CurrentID = pizza.PizzaID.ToString();

                CurrentPrice = pizza.Price.ToString();
                CurrentType = pizza.Type;
                CurrentPizzaBaseID = pizza.PizzabaseID;
            }
        }

        public OrderViewModel OldOrder
        {
            get { return oldOrder; }
            set
            {
                if (oldOrder == value || value == null)
                    return;

                oldOrder = value;

                CurrentOrderPizzaList = oldOrder.PizzaList;
                CurrentOrderExtraList = oldOrder.ExtraList;

            }
        }

        /// <summary>
        /// The list of pizzas in the selected Old order
        /// </summary>
        public List<BackendHandler.Pizza> CurrentOrderPizzaList { get; set; }

        /// <summary>
        /// The list of extras in the selected Old order
        /// </summary>
        public List<BackendHandler.Extra> CurrentOrderExtraList { get; set; }

        /// <summary>
        /// Static instance of the ingredient window
        /// </summary>
        public static IngredientWindow IngredientPopupWindow { get; set; }

        /// <summary>
        /// Instance of the pizza to add
        /// </summary>
        public static Pizza PizzaToAdd { get; set; }

        /// <summary>
        /// The window to pop up if the user tries to delete an item
        /// </summary>
        public static WarningOnDeleteWindow deleteWindow { get; set; }

        #endregion

        #region Command Properties

        /// <summary>
        /// The command to add an item to the database
        /// </summary>
        public IAsyncCommand Add { get; set; }

        /// <summary>
        /// The command to update an item in the database
        /// </summary>
        public IAsyncCommand Update { get; set; }

        /// <summary>
        /// The command to delete an item from the database
        /// </summary>
        public IAsyncCommand Delete { get; set; }

        /// <summary>
        /// The command to clear all current values
        /// </summary>
        public ICommand Clear { get; set; }

        /// <summary>
        ///  The command to trigger the navigation enum
        /// </summary>
        public ICommand Navigate { get; set; }

        #endregion

        #region Public Methods
        /// <summary>
        /// Call this to fire a <see cref="PropertyChanged"/> event
        /// </summary>
        /// <param name="name"></param>
        public void OnPropertyChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }


        /// <summary>
        /// Checking the current page and gets update the current list depending on the page
        /// </summary>
        /// <returns></returns>
        public async Task UpdateList()
        {

            // check the current page
            switch (MainWindowViewModel.VM.CurrentPage)
            {
                case Navigator.Employees:
                    {
                    // get all items from the database..
                    DatabaseList = await rep.GetAllEmployees();

                    // and update the lsit to show in UI
                    CurrentList = await ViewModelHelpers.FillListFromDatabase<BackendHandler.Employee, EmployeeViewModel>(DatabaseList);
                    return;
                    }

                case Navigator.Condiments:
                    {
                        // get all items from the database..
                        DatabaseList = await rep.GetAllCondiments();

                        // and update the lsit to show in UI
                        CurrentList = await ViewModelHelpers.FillListFromDatabase<BackendHandler.Condiment, CondimentViewModel>(DatabaseList);
                        return;
                    }

                case Navigator.Extras:
                    {
                        // get all items from the database..
                        DatabaseList = await rep.GetAllExtras();

                        // and update the lsit to show in UI
                        CurrentList = await ViewModelHelpers.FillListFromDatabase<BackendHandler.Extra, ExtraViewModel>(DatabaseList);
                        return;
                    }

                case Navigator.OldOrders:
                    {
                        // get all items from the database..
                        DatabaseList = await rep.GetOrderByStatus(3);

                        // and update the list to show in UI
                        CurrentList = await ViewModelHelpers.FillListFromDatabase<BackendHandler.Order, OrderViewModel>(DatabaseList);
                        return;
                    }

                case Navigator.Pizzas:
                    {
                        // get all items from the database..
                        DatabaseList = await rep.GetAllPizzas();

                        // and update the list to show in UI
                        CurrentList = await ViewModelHelpers.FillListFromDatabase<BackendHandler.Pizza, PizzaViewModel>(DatabaseList);
                        return;
                    }
            }

        }

        #endregion

        #region Base Commands Actions

        /// <summary>
        /// Command to add an item to the database
        /// </summary>
        /// <returns></returns>
        public async Task AddCommand()
        {
            CurrentID = "";

            switch (MainWindowViewModel.VM.CurrentPage)
            {

                case Navigator.Condiments:
                    {
                        // Creating a new instance of the item and fills up the properties
                        var CondimentToAdd = new BackendHandler.Condiment() { Type = CurrentType, Price = float.Parse(CurrentPrice) };

                        // Calling the database and adds the item
                        await rep.AddCondiment(CondimentToAdd);

                        // Updates the list and the UI
                        await UpdateList();
                        return;
                    }

                case Navigator.Extras:
                    {
                        // Creating a new instance of the item and fills up the properties
                        var ExtraToAdd = new BackendHandler.Extra() { Type = CurrentType, Price = float.Parse(CurrentPrice) };

                        // Calling the database and adds the item
                        await rep.AddExtra(ExtraToAdd);

                        // Updates the list and the UI
                        await UpdateList();
                        return;
                    }

                case Navigator.Pizzas:
                    {
                        // Checks that nothing is null
                        if (CurrentType == null || CurrentPrice == null)
                            return;

                        // Creating a new instance of the item and fills up the properties
                        var PizzaToUpdate = new PizzaViewModel() { Type = CurrentType, Price = float.Parse(CurrentPrice), PizzabaseID = CurrentPizzaBaseID };

                        // Creating a new instance of the window
                        IngredientPopupWindow = new IngredientWindow(PizzaToUpdate);

                        // And opens it up
                        Nullable<bool> result = IngredientPopupWindow.ShowDialog(); 

                        // If the user closes the window without saving any changes, the return will be false
                        if(result == false)
                        {
                            return;
                        }

                        // If the user clicka tha apply button, the return will be true
                        else
                        {
                            await UpdateList();
                            return;
                        }
                    }
            }
        }

        public async Task UpdateCommand()
        {
            switch (MainWindowViewModel.VM.CurrentPage)
            {

                case Navigator.Condiments:
                    {
                        // Finding that item in the database list
                        var CondimentToUpdate = GetSelectedItemAsModelType<BackendHandler.Condiment, CondimentViewModel>(Condiment);

                        // updates the type
                        CondimentToUpdate.Type = CurrentType;

                        // updates the price
                        CondimentToUpdate.Price = float.Parse(CurrentPrice);

                        // calling the database and updates the item
                        await rep.UpdateCondiment(CondimentToUpdate);

                        // refreshes the list in the UI
                        await UpdateList();
                        return;
                    }

                case Navigator.Extras:
                    {
                        // Finding that item in the database list
                        var ExtraToUpdate = GetSelectedItemAsModelType<BackendHandler.Extra, ExtraViewModel>(Extra);

                        // updates the type
                        ExtraToUpdate.Type = CurrentType;

                        // updates the price
                        ExtraToUpdate.Price = float.Parse(CurrentPrice);

                        // calling the database and updates the item
                        await rep.UpdateExtra(ExtraToUpdate);

                        // refreshes the list in the UI
                        await UpdateList();
                        return;
                    }

                case Navigator.Pizzas:
                    {
                        // Checks that nothing is null
                        if (CurrentID == null || CurrentType == null || CurrentPrice == null)
                            return;

                        // Creating a new instance of the item and fills up the properties
                        var PizzaToUpdate = new PizzaViewModel() { PizzaID = int.Parse(CurrentID), Type = CurrentType, Price = float.Parse(CurrentPrice), PizzabaseID = CurrentPizzaBaseID };

                        // Creating a new instance of the window
                        IngredientPopupWindow = new IngredientWindow(PizzaToUpdate);

                        // And opens it up
                        Nullable<bool> result = IngredientPopupWindow.ShowDialog();

                        // If the suser closes the window without saving any changes, the return will be false
                        if (result == false)
                            return;

                        // If the user clicks the apply button, the return will be true
                        else
                        {
                            CurrentID = null;
                            CurrentType = null;
                            CurrentPrice = null;

                            await UpdateList();
                            return;
                        }
                    }
            }
        }

        public async Task DeleteCommand()
        {
            CurrentID = "";
            deleteWindow = new WarningOnDeleteWindow();

            switch (MainWindowViewModel.VM.CurrentPage)
            {
                case Navigator.Employees:
                    {
                        Nullable<bool> result = deleteWindow.ShowDialog();

                        if (result == false)
                            return;

                        else
                        {
                            var EmployeeToDelete = GetSelectedItemAsModelType<BackendHandler.Employee, EmployeeViewModel>(Employee);
                            await rep.DeleteEmployee(EmployeeToDelete);
                            await UpdateList();
                            return;
                        }
                    }

                case Navigator.Condiments:
                    {
                        Nullable<bool> result = deleteWindow.ShowDialog();

                        if (result == false)
                            return;

                        else
                        {
                            var CondimentToDelete = GetSelectedItemAsModelType<BackendHandler.Condiment, CondimentViewModel>(Condiment);
                            await rep.DeleteCondiment(CondimentToDelete);
                            await UpdateList();
                            return;
                        }
                    }

                case Navigator.Extras:
                    {
                        Nullable<bool> result = deleteWindow.ShowDialog();

                        if (result == false)
                            return;

                        else
                        {
                            var ExtraToDelete = GetSelectedItemAsModelType<BackendHandler.Extra, ExtraViewModel>(Extra);
                            await rep.DeleteExtra(ExtraToDelete);
                            await UpdateList();
                            return;
                        }
                    }

                case Navigator.Pizzas:
                    {
                        Nullable<bool> result = deleteWindow.ShowDialog();

                        if (result == false)
                            return;

                        else
                        {
                            var PizzaToDelete = GetSelectedItemAsModelType<BackendHandler.Pizza, PizzaViewModel>(Pizza);
                            await rep.DeletePizza(PizzaToDelete);
                            await UpdateList();
                            return;
                        }
                    }

                case Navigator.OldOrders:
                    {
                        Nullable<bool> result = deleteWindow.ShowDialog();

                        if (result == false)
                            return;

                        else
                        {
                            var OldOrderToDelete = await rep.GetSingleOrder(oldOrder.OrderID);
                            await rep.DeleteOldOrder(OldOrderToDelete);
                            await UpdateList();
                            return;
                        }
                    }

            }
        }

        /// <summary>
        /// Command to clear all current values
        /// </summary>
        /// <param name="o"></param>
        public void ClearCommand(object o)
        {
            CurrentID = "";
            CurrentPrice = "";
            CurrentType = "";

            CurrentRole = "";
        }

        #endregion

        #region Command Helpers

        /// <summary>
        /// Runs a command if the updating flag is not set.
        /// If the flag is true (indicating the function is already running) then the action is not run.
        /// If the flag is false (indicating no running function) then the action is run.
        /// Once the action i finished if it was run, then the flag is reset to false
        /// </summary>
        /// <param name="updatingFlag">the boolean property flag defining if the command is already running</param>
        /// <param name="action">The action to run if the command is not already running</param>
        /// <returns></returns>
        protected async Task RunCommand(Expression<Func<bool>> updatingFlag, Func<Task> action)
        {
            // Check if the flag property is true (meaning the function is already running)
            if (updatingFlag.GetPropertyValue())
                return;

            // Set the property flag to true to indicate we are running
            updatingFlag.SetPropertyValue(true);

            try
            {
                // Run the passed in action
                await action();
            }
            finally
            {
                // Set the property flag back to false now it's finished
                updatingFlag.SetPropertyValue(false);
            }
        }

        /// <summary>
        /// Method to return an item sent in as a ModelViewModelType as a ModelType
        /// </summary>
        /// <typeparam name="ModelType">The type to be returned</typeparam>
        /// <typeparam name="ModelViewModelType">The type to get the value from</typeparam>
        /// <param name="Instance">The current instance needed to get the right item</param>
        /// <returns>The sent in item as the desired model type</returns>
        public ModelType GetSelectedItemAsModelType<ModelType, ModelViewModelType>(ModelViewModelType Instance)
        {
            // casting the database result to a queryable list
            var ModelTypeList = DatabaseList.Cast<ModelType>().ToList();

            // casting the view model version to a queryable list
            var ModelViewModelList = CurrentList.Cast<ModelViewModelType>().ToList();

            // getting the index of the chosen item
            int index = ModelViewModelList.IndexOf(Instance);

            // return the same item but as a Model Type
            return ModelTypeList[index];

        }
        #endregion

    }
}

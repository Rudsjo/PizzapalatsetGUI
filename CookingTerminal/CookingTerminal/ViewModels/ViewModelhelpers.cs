using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookingTerminal
{
    /// <summary>
    /// Helper class for all ViewModels
    /// </summary>
    public static class ViewModelhelpers
    {

        #region Methods

        /// <summary>
        /// Generic method that fills an Observable Collection from a IEnumerable from the database
        /// </summary>
        /// <typeparam name="ModelType">The model to take basic properties from</typeparam>
        /// <typeparam name="ModelViewModelType">The ViewModel of the model to create a Observable collection of</typeparam>
        /// <param name="dbList">The actual IEnumerable returned from the database</param>
        /// <returns></returns>
        public static async Task<ObservableCollection<ModelViewModelType>> FillListFromDatabase<ModelType, ModelViewModelType>(IEnumerable dbList)
                    where ModelViewModelType : new()
        {

            IEnumerable<ModelType> databaseList = (dbList as IEnumerable<ModelType>);

            // the new list to return
            ObservableCollection<ModelViewModelType> result = new ObservableCollection<ModelViewModelType>();

            var properties = typeof(ModelType).GetProperties();

            // go through every item in the sent in database list
            databaseList.ToList().ForEach(item =>
            {
                // creates a new instance of the sent in ModelViewModel
                ModelViewModelType newItem = new ModelViewModelType();

                // goes trough every property in the database list and copies it to the new list
                Parallel.ForEach(databaseList, (currentProperty) =>
                {
                    for (int i = 0; i < properties.Length; i++)
                    {

                        newItem.GetType().GetProperty(properties[i].Name).SetValue(newItem,
                           item.GetType().GetProperty(properties[i].Name).GetValue(item));

                    }
                });

                // adds the new item to the list
                result.Add(newItem);
            });

            // returns the whole list as a Observable collection of the desired ModelViewModel type
            return result;

        }
        #endregion
    }
}

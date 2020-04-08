namespace CustomerTerminalWPF.Views
{
    /// <summary>
    /// Required Namespaces
    /// </summary>
    #region Namespaces
    using System.Windows.Controls;
    #endregion

    /// <summary>
    /// Base page for all other pages to inherit from
    /// 
    /// *********************************************
    /// This will make so that the constructors will be
    /// fired in the designer when all of pages is 
    /// building.
    /// *********************************************
    /// Main goal of this class is to set the
    /// DataContext for the pages when they are loading
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BasePage<T> : Page 
        where T : BaseViewModel, new()
    {
        /// <summary>
        /// Private instance of this ViewModel
        /// </summary>
        private T dataContext;

        /// <summary>
        /// public property for this ViewModel
        /// </summary>
        public T tDataContext
        {
            // Returns the DataContext of this Page
            get { return dataContext; }

            // Sets the DataContext of this Page if it has changed
            set{ if(dataContext != value) { dataContext = value; this.DataContext = dataContext; } }
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public BasePage()
        {
            // Set the new DataContext
            tDataContext = new T();
        }
    }
}

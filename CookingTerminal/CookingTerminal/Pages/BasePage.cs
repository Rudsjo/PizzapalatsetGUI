using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace CookingTerminal
{
    /// <summary>
    /// Base page for all pages to gain base functionality
    /// </summary>
    /// <typeparam name="VM">The ViewModel for the page</typeparam>
    public class BasePage<VM> : Page
        where VM : BaseViewModel, new()
    {

        #region Private Members

        /// <summary>
        /// The ViewModel associated to this page
        /// </summary>
        private VM _viewModel;

        #endregion

        #region Public Properties

        /// <summary>
        /// The ViewModel associated with this page.
        /// The DataContext is defined in the setter.
        /// </summary>
        public VM ViewModel
        {
            get { return _viewModel; }
            set
            {
                // If nothing has changed, return
                if (_viewModel == value)
                    return;

                // Update the value
                _viewModel = value;

                // Setting the datacontext
                this.DataContext = _viewModel;
            }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Default Constructor
        /// </summary>
        public BasePage()
        {
            // Creating a default view model
            this.ViewModel = new VM();
        }

        #endregion

    }
}

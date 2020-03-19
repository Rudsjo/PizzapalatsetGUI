using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using BackendHandler;

namespace AdminTerminal_v2
{
    public class MainWindowViewModel : BaseViewModel
    {
        /// <summary>
        /// Static instance och MainWindowViewModel to access properties
        /// </summary>
        public static MainWindowViewModel VM { get; set; }

        #region Public properties

        /// <summary>
        /// Property to define the current page. 
        /// Standard is Login page.
        /// </summary>
        public Navigator CurrentPage { get; set; } = Navigator.Pizzas;

        public ObservableCollection<EmployeeViewModel> EmployeeList { get; set; }

        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public MainWindowViewModel()
        {
            VM = this;
        }

        #endregion


        #region Navigator Methods

        public void NavigatorCommand(object e)
        {
            if (Enum.TryParse<Navigator>((string)e, out Navigator nav))
            {
                CurrentPage = nav;
            }
        }

        #endregion

    }
}

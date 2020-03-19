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


        #region Sizes
        /// <summary>
        /// The regular/maximum window height
        /// </summary>
        public static int MaxWindowHeight { get; set; } = 600;

        /// <summary>
        /// The regular/maximum window width
        /// </summary>
        public static int MaxWindowWidth { get; set; } = (MaxWindowHeight * 2);

        /// <summary>
        /// The minimum window height
        /// </summary>
        public static int MinWindowHeight { get; set; } = 450;

        /// <summary>
        /// The minimum window width
        /// </summary>
        public static int MinWindowWidth { get; set; } = (MinWindowHeight * 2);

        #endregion

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
            CurrentList = null;
            DatabaseList = null;
            CurrentPage = Navigator.Extras;

            if (Enum.TryParse<Navigator>((string)e, out Navigator nav))
            {
                CurrentPage = nav;
            }
        }

        #endregion

    }
}

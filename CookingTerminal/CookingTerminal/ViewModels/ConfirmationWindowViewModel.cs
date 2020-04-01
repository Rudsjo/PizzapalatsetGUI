using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace CookingTerminal
{
    /// <summary>
    /// View model for the Confirmation Window
    /// </summary>
    public class ConfirmationWindowViewModel : BaseViewModel
    {

        #region Window Sizes

        /// <summary>
        /// The height of the popup window
        /// </summary>
        public int Height { get; set; } = (MainWindowViewModel.VM.CurrentHeight / 2);

        /// <summary>
        /// The width of the popup window
        /// </summary>
        public int Width { get; set; } = (MainWindowViewModel.VM.CurrentWidth / 2);

        #endregion

        #region Public Properties

        /// <summary>
        /// The message to show in the window
        /// </summary>
        public string Message { get; set; } = "Är du säker på att du vill ta bort ordern?";

        /// <summary>
        /// Content of the confirm button
        /// </summary>
        public string ConfirmButtonContent { get; set; } = "Ja";

        /// <summary>
        /// Content of the decline button
        /// </summary>
        public string DeclineButtonContent { get; set; } = "Nej";

        /// <summary>
        /// The command to run if user presses confirm
        /// </summary>
        public ICommand Confirm { get; set; }

        /// <summary>
        /// The command to run if the user presses no
        /// </summary>
        public ICommand Decline { get; set; }

        #endregion

        #region Constructor

        public ConfirmationWindowViewModel()
        {

            // Creating commands
            Confirm = new RelayCommand((o) => { });

            Decline = new RelayCommand((o) => { });
        }

        #endregion


    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace AdminTerminal_v2
{
    public class WarningOnDeleteWindowViewModel : BaseViewModel
    {

        #region Public Properties

        /// <summary>
        /// The Message to appear within the window
        /// </summary>
        public string Message { get; set; } = "Är du säker på att du vill ta bort objektet?";

        /// <summary>
        /// Content of confirm button
        /// </summary>
        public string ConfirmButtonContent { get; set; } = "Ja";

        /// <summary>
        /// Content of decline button
        /// </summary>
        public string DeclineButtonContent { get; set; } = "Nej";

        #endregion

        #region Commands

        /// <summary>
        /// The command to run an action if the user confirms a deletion
        /// </summary>
        public ICommand Confirm { get; set; }

        /// <summary>
        /// The command to run an action if the user declines a deletion
        /// </summary>
        public ICommand Decline { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public WarningOnDeleteWindowViewModel()
        {

            // Creating commands
            Confirm = new RelayParameterlessCommand((o) =>
            {
                deleteWindow.DialogResult = true;
            });

            Decline = new RelayParameterlessCommand((o) =>
            {
                deleteWindow.DialogResult = false;
            });
        }

        #endregion
    }
}

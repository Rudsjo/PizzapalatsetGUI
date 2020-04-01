using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.Windows.Input;
using System.Windows.Threading;

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
        public int Height { get; set; } = (MainWindowViewModel.VM.CurrentHeight / 4);

        /// <summary>
        /// The width of the popup window
        /// </summary>
        public int Width { get; set; } = (MainWindowViewModel.VM.CurrentWidth / 4);

        #endregion

        #region Public Properties

        /// <summary>
        /// The message to show in the window
        /// </summary>
        public string Message { get; set; } = "Ordern serverad";

        /// <summary>
        /// Timer of how long the popup window should be open
        /// </summary>
        public DispatcherTimer WindowTimer { get; set; }

        /// <summary>
        /// The amount of seconds the window should be shown
        /// </summary>
        public int ShowingTime { get; set; } = 1;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ConfirmationWindowViewModel()
        {
            // Starting the timer and closing it when the set time has expired
            WindowTimer = new DispatcherTimer();
            WindowTimer.Interval = new TimeSpan(0, 0, ShowingTime);
            WindowTimer.Start();
            WindowTimer.Tick += WindowTimer_Tick;
        }

        /// <summary>
        /// Event to close the window when the timer has elapsed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowTimer_Tick(object sender, EventArgs e)
        {
            // Returns that the window was closed correctly and stops the timer
            CookingPageViewModel.VM.confirmationWindow.DialogResult = true;
            WindowTimer.Stop();
        }


        #endregion
    }
}

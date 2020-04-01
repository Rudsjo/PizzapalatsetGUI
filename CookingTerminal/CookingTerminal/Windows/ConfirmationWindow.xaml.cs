using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CookingTerminal
{
    /// <summary>
    /// Interaction logic for ConfirmationWindow.xaml
    /// </summary>
    public partial class ConfirmationWindow : Window
    {
        public ConfirmationWindow()
        {
            InitializeComponent();

            // Setting the DataContext
            this.DataContext = new ConfirmationWindowViewModel();

            // Setting the Owner window
            this.Owner = Window.GetWindow(MainWindow.Instance);

            // Sets the startup location to center of the parent window
            this.WindowStartupLocation = WindowStartupLocation.Manual;
            this.Top = ((Window.GetWindow(MainWindow.Instance).ActualHeight / 2) - this.ActualHeight);
            this.Left = ((Window.GetWindow(MainWindow.Instance).ActualWidth / 2) - this.ActualWidth);

            this.WindowStyle = WindowStyle.None;
        }
    }
}

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

namespace AdminTerminal_v2
{
    /// <summary>
    /// Interaction logic for WarningOnDeleteWindow.xaml
    /// </summary>
    public partial class WarningOnDeleteWindow : Window
    {
        public WarningOnDeleteWindow()
        {
            InitializeComponent();

            // Setting the DataContext
            this.DataContext = new WarningOnDeleteWindowViewModel();

            // Setting the Owner window
            this.Owner = GetWindow(MainWindow.Instance);

            // Setting Window sizes
            this.MaxHeight = GetWindow(MainWindow.Instance).ActualHeight / 2;
            this.MaxWidth = GetWindow(MainWindow.Instance).ActualWidth / 2;
            this.MinHeight = GetWindow(MainWindow.Instance).ActualHeight / 2;
            this.MinWidth = GetWindow(MainWindow.Instance).ActualWidth / 2;

            // Setting startup location
            this.WindowStartupLocation = WindowStartupLocation.Manual;
            this.Top = GetWindow(MainWindow.Instance).ActualHeight / 2;
            this.Left = GetWindow(MainWindow.Instance).ActualWidth / 2;

            // Removes the border of the window
            this.WindowStyle = WindowStyle.None;
        }
    }
}

namespace CustomerTerminalWPF
{
    // Required namespace
    using System.Windows;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // Set the datacontext of this window
            this.DataContext = new ViewModels.MainWindowViewModel(this);
        }
    }
}

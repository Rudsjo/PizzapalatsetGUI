using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_Kassörskan_V2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        public async void Init()
        {
            ProgramState.IsRunning = true;

            ConfigFileHelpers.ReadServerConfigFile();

            if (await ProgramState.ServerConnection.ConnectAsync())
                MessageBox.Show("Terminal is connected to the server");
            
            else
                MessageBox.Show("Could not connect to the server, RUNNING IN OFFLINE MODE");




        }
    }
}

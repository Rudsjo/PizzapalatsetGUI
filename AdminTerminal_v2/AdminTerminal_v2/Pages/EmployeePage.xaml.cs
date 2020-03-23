using System;
using System.Collections.Generic;
using System.Security;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AdminTerminal_v2
{
    /// <summary>
    /// Interaction logic for EmployeePage.xaml
    /// </summary>
    public partial class EmployeePage : BasePage<EmployeePageViewModel>, IHavePassword
    {
        public EmployeePage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The secure password for the employee page when add or update
        /// </summary>
        public SecureString SecurePassword => NewPasswordText.SecurePassword;

        public static EmployeePage Accessor { get; set; }

    }
}
       


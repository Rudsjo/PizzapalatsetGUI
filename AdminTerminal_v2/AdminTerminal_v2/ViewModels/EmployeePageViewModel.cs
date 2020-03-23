using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Security;

namespace AdminTerminal_v2
{
    public class EmployeePageViewModel : BaseViewModel
    {

        #region Private Members

        /// <summary>
        /// Array containing all available roles in the restaurant
        /// </summary>
        private string[] AvailableRoles = new string[] { "Admin", "Bagare", "Kassör" };

        #endregion

        /// <summary>
        /// The command to add an employee
        /// </summary>
        public ICommand AddEmployee { get; set; }

        /// <summary>
        /// The command to update an employee
        /// </summary>
        public ICommand UpdateEmployee { get; set; }


        #region Constructor

        public EmployeePageViewModel()
        {
            UpdateList();
            Employee = new EmployeeViewModel();

            AddEmployee = new RelayParameterizedCommand(async (parameter) => await AddEmployeeCommand(parameter));
            UpdateEmployee = new RelayParameterizedCommand(async (parameter) => await UpdateEmployeeCommand(parameter));
            Delete = new RelayAsyncCommand(DeleteCommand);
            Clear = new RelayParameterlessCommand(ClearCommand);

            Navigate = new RelayParameterlessCommand(MainWindowViewModel.VM.NavigatorCommand);
        }

        #endregion

        /// <summary>
        /// The command to add a new employee with a safe password
        /// </summary>
        /// <param name="parameter">The secure password</param>
        /// <returns></returns>
        public async Task AddEmployeeCommand(object parameter)
        {
            await RunCommand(() => this.TaskIsRunning, async () =>
            {

                // check that role is correct
                for(int i = 0; i < AvailableRoles.Length; i++)
                {
                    // if the role is found, the command will move on
                    if (AvailableRoles[i].Equals(CurrentRole, StringComparison.InvariantCultureIgnoreCase))
                        break;

                    // if it's not found, it will give the message to the user
                    else if((i == AvailableRoles.Length - 1) && !AvailableRoles[i].Equals(CurrentRole, StringComparison.InvariantCultureIgnoreCase))
                    {
                        MessageBox.Show("Rollen finns inte");
                        return;
                    }
                }

                // check if the password has the right amount of characters
                if ((parameter as IHavePassword).SecurePassword.Length < 2)
                {
                    MessageBox.Show("Lösenordet måste innehålla minst 2 tecken");
                    return;
                }


                    // Calling the database and adds the item
                    await rep.AddEmployee(new BackendHandler.Employee() { Role = CurrentRole, Password = (parameter as IHavePassword).SecurePassword.Unsecure() });

                    // Updates the list and the UI
                    await UpdateList();

                    // clears the info
                    CurrentID = "";
                    CurrentRole = "";


            });
        }


        public async Task UpdateEmployeeCommand(object parameter)
        {

            await RunCommand(() => this.TaskIsRunning, async () =>
            {

                // check that role is correct
                for (int i = 0; i < AvailableRoles.Length; i++)
                {
                    // if the role is found, the command will move on
                    if (AvailableRoles[i].Equals(CurrentRole, StringComparison.InvariantCultureIgnoreCase))
                        break;

                    // if it's not found, it will give the message to the user
                    else if ((i == AvailableRoles.Length - 1) && !AvailableRoles[i].Equals(CurrentRole, StringComparison.InvariantCultureIgnoreCase))
                    {
                        MessageBox.Show("Rollen finns inte");
                        return;
                    }
                }

                // check if the password has the right amount of characters
                if ((parameter as IHavePassword).SecurePassword.Length < 2)
                {
                    MessageBox.Show("Lösenordet måste innehålla minst 2 tecken");
                    return;
                }

                    // Finding that item in the database list
                    var EmployeeToUpdate = GetSelectedItemAsModelType<BackendHandler.Employee, EmployeeViewModel>(Employee);

                    // updates the role
                    EmployeeToUpdate.Role = CurrentRole;

                    // calling the database and updates the item
                    await rep.UpdateEmployee(EmployeeToUpdate, (parameter as IHavePassword).SecurePassword.Unsecure());

                    // refreshes the list in the UI
                    await UpdateList();

                    // clears the info
                    CurrentID = "";
                    CurrentRole = "";


            });
        }



    }
}

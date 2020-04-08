using System;
using System.Collections.Generic;
using System.Text;

namespace AdminTerminal_v2
{
    public class EmployeeViewModel : BaseViewModel
    {
        public int UserID { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

    }
}

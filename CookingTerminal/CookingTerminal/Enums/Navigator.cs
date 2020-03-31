using System;
using System.Collections.Generic;
using System.Text;

namespace CookingTerminal
{
    /// <summary>
    /// Enum  of different pages
    /// </summary>
    public enum Navigator
    {
        // Login page
        Login = 1,

        // Main page showing orders ready to be cooked
        Main = 2,

        // Page to show chosen order
        Cooking = 3,
    }
}

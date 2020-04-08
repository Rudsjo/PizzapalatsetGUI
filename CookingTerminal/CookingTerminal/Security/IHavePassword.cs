using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace CookingTerminal
{
    public interface IHavePassword
    {
        /// <summary>
        /// The secure password to get
        /// </summary>
        public SecureString SecurePassword { get; }

    }
}

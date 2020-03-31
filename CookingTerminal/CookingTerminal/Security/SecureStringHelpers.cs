using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace CookingTerminal
{
    public static class SecureStringHelpers
    {
        /// <summary>
        /// Converts a SecureString to plain text
        /// </summary>
        /// <param name="secureString">The SecureString to Convert</param>
        /// <returns></returns>
        public static string Unsecure(this SecureString secureString)
        {
            // Make sure we have string to unsecure
            if (secureString == null)
                return string.Empty;

            // get a pointer for an unsercure string in memory
            var unmanagedString = IntPtr.Zero;

            try
            {
                // Unsecures the password
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                // Clean up any memory allocation
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }


    }
}

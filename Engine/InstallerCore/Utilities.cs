using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Installer.Core
{
    public static class Utilities
    {
        /// <summary>
        /// Pre-validation of a one time use key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool ValidateUID(string key)
        {
            int result = 0;
            foreach (char c in key)
            {
                result ^= c;
            }
            return (result % 16) == 0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace WindowsInstaller
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                RegistryKey k = RegistryKey.OpenBaseKey( RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(@"SOFTWARE\Shiversoft\csse");
                if (k.GetValue("NoInstall").ToString() == "1")
                    return;
            }
            catch
            {
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Installer());
        }
    }
}

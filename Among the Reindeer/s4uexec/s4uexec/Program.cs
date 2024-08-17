using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Principal;
using static s4uexec.NativeMethods;
using System.Diagnostics;

namespace s4uexec
{
    class Program
    {
        static void Main(string[] args)
        {
            var privs = new[] {Privilege.TrustedComputingBase, Privilege.AssignPrimaryToken};
            Privilege.RunWithPrivileges(() =>
            {
                var ident = WindowsIdentity.
                var context = ident.Impersonate();
                var proc = Process.Start("cmd.exe", "/C " + args[1]);
                proc.WaitForExit();
                context.Undo();
            }, privs);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey(true);
        }
    }
}

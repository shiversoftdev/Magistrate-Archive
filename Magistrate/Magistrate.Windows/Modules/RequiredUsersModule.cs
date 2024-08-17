using Magistrate.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace Magistrate.Windows.Modules
{
    internal sealed class RequiredUsersModule : BaseModule
    {
        private static bool DebugVerbose = false;
        public RequiredUsersModule()
        {
            SetTickRate(60000);
        }

        protected override List<CheckState> QueryState(CheckInfo info)
        {
            string ucsv = info.GetArgument(0)?.ToString();
            if (ucsv == null) return SingleState(CheckInfo.DEFAULT);
            string[] users = ucsv.ToLower().Split(',');
            List<CheckState> states = new List<CheckState>();
            try
            {
                List<string> CurrentUsers = GetActiveMachineUsers();
                foreach (var user in users)
                {
                    if (!CurrentUsers.Contains(user))
                    {
                        states.Add(new CheckState(info.ComputeHash($"{user}:deleted")));
                        continue;
                    }
                    if (!Directory.Exists($"C:\\Users\\{user}") ||
                        !Directory.Exists($"C:\\Users\\{user}\\Desktop") ||
                        !Directory.Exists($"C:\\Users\\{user}\\Documents"))
                    {
                        states.Add(new CheckState(info.ComputeHash($"{user}:damaged")));
                        continue;
                    }
                    states.Add(new CheckState(info.ComputeHash($"{user}:active")));
                }
            }
            catch (Exception e)
            {
#if DEBUG
                if(DebugVerbose) Console.WriteLine(e.ToString());
#endif
                return info.Maintain();
            }
            return states;
        }

        private const int UF_ACCOUNTDISABLE = 0x0002;

        private List<string> GetActiveMachineUsers()
        {
            List<string> returnValue = new List<string>();
            DirectoryEntry localMachine = new DirectoryEntry("WinNT://" + Environment.MachineName);
            foreach (DirectoryEntry user in localMachine.Children)
            {
                if (user.SchemaClassName == "User")
                {
                    if (((int)user.Properties["UserFlags"].Value & UF_ACCOUNTDISABLE) != UF_ACCOUNTDISABLE)
                    {
#if DEBUG
                        if (DebugVerbose) Console.WriteLine(user.Properties["Name"].Value.ToString().ToLower());
#endif
                        returnValue.Add(user.Properties["Name"].Value.ToString().ToLower());
                    }
                }
            }
            return returnValue;
        }
    }
}

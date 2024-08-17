using Magistrate.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace Magistrate.Windows.Modules
{
    internal sealed class ADOUUsersModule : BaseModule
    {
        private PowerShell PSInst;
        private static bool DebugVerbose = false;
        public ADOUUsersModule() : base()
        {
            PSInst = PowerShell.Create();

            SetTickRate(37000);
        }

        protected override List<CheckState> QueryState(CheckInfo info)
        {
            List<CheckState> States = new List<CheckState>();
            try
            {
                PSInst.AddCommand("Get-ADObject").
                    AddParameter("Filter", "ObjectClass -eq \"group\"").
                    AddParameter("SearchBase", info.GetArgument(0)?.ToString() ?? "").
                    AddCommand("Get-ADGroupMember");

                Collection<PSObject> PSOutput = PSInst.Invoke();
                PSInst.Commands.Clear();                    

                foreach (var obj in PSOutput)
                {
                    string sstate = $"{obj.Properties["objectClass"]?.Value?.ToString().ToLower() ?? "badobject"}:{obj.Properties["Name"]?.Value?.ToString().ToLower() ?? "who?"}";
#if DEBUG
                    if (DebugVerbose)
                        Console.WriteLine(sstate);
#endif
                    States.Add(new CheckState(info.ComputeHash(sstate)));
                }

                PSInst.AddCommand("Get-ADObject").
                    AddParameter("Filter", "ObjectClass -eq \"user\"").
                    AddParameter("SearchBase", info.GetArgument(0)?.ToString() ?? "");

                PSOutput = PSInst.Invoke();
                PSInst.Commands.Clear();

                if (PSOutput.Count < 1)
                    return States;

                foreach (var obj in PSOutput)
                {
                    string sstate = $"{obj.Properties["objectClass"]?.Value?.ToString().ToLower() ?? "badobject"}:{obj.Properties["Name"]?.Value?.ToString().ToLower() ?? "who?"}";
#if DEBUG
                    if (DebugVerbose)
                        Console.WriteLine(sstate);
#endif
                    States.Add(new CheckState(info.ComputeHash(sstate)));
                }
            }
            catch
            {
                return info.Maintain();
            }

            return States;
        }
    }
}

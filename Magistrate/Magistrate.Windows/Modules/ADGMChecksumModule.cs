using Magistrate.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Magistrate.Windows.Modules
{
    internal sealed class ADGMChecksumModule : BaseModule
    {
        private PowerShell PSInst;
        private static bool DebugVerbose = false;
        public ADGMChecksumModule() : base()
        {
            PSInst = PowerShell.Create();

            SetTickRate(25171);
        }

        protected override List<CheckState> QueryState(CheckInfo info)
        {
            List<CheckState> States = new List<CheckState>();
            try
            {
                PSInst.AddCommand("Get-ADObject").
                    AddParameter("Filter", "ObjectClass -eq \"group\"").
                    AddParameter("SearchBase", info.GetArgument(0)?.ToString() ?? "");

                Collection<PSObject> PSOutput = PSInst.Invoke();
                PSInst.Commands.Clear();

                if (PSOutput.Count < 1)
                    return SingleState(CheckInfo.DEFAULT);

                foreach (var obj in PSOutput)
                {
                    try
                    {
                        string gname = obj.Properties["Name"]?.Value?.ToString();
                        string result = gname.ToLower() + ":";
                        List<string> users = new List<string>();
                        PSInst.AddCommand("Get-ADGroupMember").
                            AddParameter("Identity", gname);

                        Collection<PSObject> gmoutput = PSInst.Invoke();
                        PSInst.Commands.Clear();

                        foreach(var gmember in gmoutput)
                        {
                            if (gmember.Properties["objectClass"]?.Value?.ToString() != "user")
                                continue;

                            string guser = gmember.Properties["Name"]?.Value?.ToString().ToLower();

                            if (guser == null)
                                continue;

                            users.Add(guser);
                        }

                        users.Sort();

                        result += string.Join(":", users);

                        States.Add(new CheckState(info.ComputeHash(result)));
#if DEBUG
                        if (DebugVerbose)
                            Console.WriteLine(result);
#endif
                    }
                    catch
                    {

                    }
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

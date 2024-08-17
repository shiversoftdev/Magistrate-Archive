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
    class ADUACModule : BaseModule
    {
        private PowerShell PSInst;
        private static bool DebugVerbose = false;
        public ADUACModule() : base()
        {
            PSInst = PowerShell.Create();

            SetTickRate(30000);
        }
        //reports a constant state for development testing
        protected override List<CheckState> QueryState(CheckInfo info)
        {
            List<CheckState> States = new List<CheckState>();
            try
            {
                PSInst.AddCommand("Get-ADObject").
                    AddParameter("Filter", "ObjectClass -eq \"user\"").
                    AddParameter("SearchBase", info.GetArgument(0)?.ToString() ?? "").
                    AddParameter("Properties", "userAccountControl");

                Collection<PSObject> PSOutput = PSInst.Invoke();
                PSInst.Commands.Clear();

                if (PSOutput.Count < 1)
                    return SingleState(CheckInfo.DEFAULT);

                foreach(var obj in PSOutput)
                {
                    try
                    {
                        string uname = obj.Properties["Name"]?.Value?.ToString();
                        uint uac = uint.Parse(obj.Properties["userAccountControl"]?.Value?.ToString());

                        for(int i = 0; i < 32; i++)
                        {
                            uint tval = ((uint)1 << i);
                            string dstr = $"{uname.ToLower()}:{tval}:{((tval & uac) > 0).ToString().ToLower()}";

#if DEBUG
                            if (DebugVerbose)
                                Console.WriteLine(dstr);
#endif
                            States.Add(new CheckState(info.ComputeHash(dstr)));
                        }
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

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
    internal sealed class ADDSHeuristicsModule : BaseModule
    {
        private PowerShell PSInst;
        private static bool DebugVerbose = false;
        public ADDSHeuristicsModule() : base()
        {
            PSInst = PowerShell.Create();
            
            SetTickRate(20024);
        }

        protected override List<CheckState> QueryState(CheckInfo info)
        {
            try
            {
                PSInst.AddCommand("Get-ADObject").
                    AddParameter("Filter", "ObjectClass -eq \"ntDSService\"").
                    AddParameter("SearchBase", info.GetArgument(0)?.ToString() ?? "").
                    AddParameter("Properties", "*");

                Collection <PSObject> PSOutput = PSInst.Invoke();
                PSInst.Commands.Clear();

                if (PSOutput.Count < 1)
                    return SingleState(CheckInfo.DEFAULT);

                string dstr = PSOutput[0]?.Properties["dSHeuristics"]?.Value?.ToString() ?? "0";

#if DEBUG
                if (DebugVerbose)
                    Console.WriteLine(dstr);
#endif

                List<CheckState> cs = new List<CheckState>();

                for(int i = 0; i < 26; i++)
                {
                    if(i >= dstr.Length)
                    {
                        cs.Add(new CheckState(info.ComputeHash($"dsh:{i + 1}:{0}")));
                        continue;
                    }
                    cs.Add(new CheckState(info.ComputeHash($"dsh:{i + 1}:{dstr[i]}")));
                }

                return cs;
            }
            catch (Exception e)
            {
#if DEBUG
                if(DebugVerbose)
                    Console.WriteLine(e.ToString());
#endif
            }

            return info.Maintain();
        }
    }
}

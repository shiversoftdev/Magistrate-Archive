using Magistrate.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace Magistrate.Windows.Modules
{
    internal sealed class DSACLSModule : BaseModule
    {
        private PowerShell PSInst;
        private static bool DebugVerbose = false;
        public DSACLSModule() : base()
        {
            PSInst = PowerShell.Create();

            SetTickRate(10000);
        }

        protected override List<CheckState> QueryState(CheckInfo info)
        {
            List<CheckState> States = new List<CheckState>();
            HashSet<string> AllPerms = new HashSet<string>();
            Dictionary<string, List<UserPermission>> Perms = new Dictionary<string, List<UserPermission>>();
            try
            {
                PSInst.AddCommand($"dsacls.exe").AddArgument($"{info.GetArgument(0)?.ToString()}");

                Collection<PSObject> PSOutput = PSInst.Invoke();
                PSInst.Commands.Clear();

                if (PSOutput.Count < 1)
                    return SingleState(CheckInfo.DEFAULT);

                for (int i = 0; i < PSOutput.Count; i++)
                {
                    if (!(PSOutput[i].ToString().ToLower().StartsWith("allow") || PSOutput[i].ToString().ToLower().StartsWith("deny")))
                        continue;

                    string line = PSOutput[i].ToString().ToLower().Trim();

                    if (line.IndexOf(' ') < 0)
                        continue;

                    string state = line.Substring(0, line.IndexOf(' '));
                    string name = line.IndexOf("  ") > -1 ? line.Substring(line.IndexOf(' ') + 1, line.IndexOf("  ")).Trim() : line.Substring(line.IndexOf(' ') + 1).Trim();
                    string perm = line.LastIndexOf("  ") > -1 ? line.Substring(line.LastIndexOf("  ") + 2).Trim() : null;

                    string sstr = perm != null ? $"{state}:{name}:{perm}" : null;

                    if (perm != null && !AllPerms.Contains(perm))
                        AllPerms.Add(perm);

#if DEBUG
                    if (DebugVerbose && sstr != null)
                        Console.WriteLine(sstr);
#endif
                    i++;

                    if(sstr != null)
                    {
                        States.Add(new CheckState(info.ComputeHash(sstr)));

                        if (!Perms.ContainsKey(perm))
                            Perms[perm] = new List<UserPermission>();

                        Perms[perm].Add(new UserPermission() { State = state, User = name });
                    }
                        

                    while(i < PSOutput.Count && (line = PSOutput[i].ToString().ToLower().Trim()) != "" && !line.StartsWith("allow") && !line.StartsWith("deny") && !line.StartsWith("the command"))
                    {
                        perm = line.Trim();
                        sstr = $"{state}:{name}:{perm}";

                        if (!AllPerms.Contains(perm))
                            AllPerms.Add(perm);

                        if (!Perms.ContainsKey(perm))
                            Perms[perm] = new List<UserPermission>();

                        Perms[perm].Add(new UserPermission() { State = state, User = name });
#if DEBUG
                        if (DebugVerbose)
                            Console.WriteLine(sstr);
#endif
                        i++;

                        States.Add(new CheckState(info.ComputeHash(sstr)));
                    }

                    if (line == "")
                        continue;

                    if (i >= PSOutput.Count)
                        break;

                    i--;
                }

                // Next, for each user, construct an effective override permissions map
                PSInst.AddCommand($"Get-ADUser").AddParameter("Filter", "*");

                PSOutput = PSInst.Invoke();
                PSInst.Commands.Clear();

                foreach(var obj in PSOutput)
                {
                    if (obj.Properties["Name"]?.Value == null || obj.Properties["DistinguishedName"]?.Value == null)
                        continue;

                    string dnstr = obj.Properties["DistinguishedName"]?.Value.ToString().ToLower();

                    if (dnstr.IndexOf("dc=") < 0)
                        continue;

                    string dcstr = dnstr.Substring(dnstr.IndexOf("dc=") + 3);
                    dcstr = dcstr.Substring(0, (dcstr.IndexOf(",") > -1) ? dcstr.IndexOf(",") : dcstr.Length);

                    string ustr = dcstr + @"\" + obj.Properties["Name"].Value.ToString().ToLower().Trim();

                    foreach(var p in AllPerms)
                    {
                        string csx = "";
                        if (!Perms.ContainsKey(p))
                            csx = $"u:deny:{ustr}:{p}";
                        else
                            csx = $"u:{EffectivePermission(Perms[p], ustr)}:{ustr}:{p}";
                        States.Add(new CheckState(info.ComputeHash(csx)));

#if DEBUG
                        if (DebugVerbose)
                            Console.WriteLine(csx);
#endif
                    }
                }
            }
            catch(Exception e)
            {
#if DEBUG
                if (DebugVerbose)
                    Console.WriteLine(e.ToString());
#endif
                return info.Maintain();
            }
            return States;
        }

        private string EffectivePermission(List<UserPermission> perms, string username)
        {
            string result = "ideny";

            foreach(var perm in perms)
            {
                if(perm.User == username)
                {
                    if (perm.State == "deny")
                        return "deny";
                    if (perm.State == "allow")
                        result = "allow";
                }
            }

            return result == "ideny" ? "deny" : result;
        }

        private class UserPermission
        {
            internal string User;
            internal string State;
        }
    }
}

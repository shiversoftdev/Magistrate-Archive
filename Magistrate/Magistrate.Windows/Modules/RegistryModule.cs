using Magistrate.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.IO;

namespace Magistrate.Windows.Modules
{
    // * means all subkeys
    // ** means all subkeys recursively

    // expects: fullpath.tolower()+valuestring??""
    internal sealed class RegistryModule : BaseModule
    {
        private static bool DebugVerbose = false;
        public RegistryModule() : base()
        {
            SetTickRate(15027);
        }

        //reports a constant state for development testing
        protected override List<CheckState> QueryState(CheckInfo info)
        {
            string root = info.GetArgument(0)?.ToString();
            string KeyPath = info.GetArgument(1)?.ToString();
            string Valuestring = info.GetArgument(2)?.ToString();

            bool Is64 = (info.GetArgument(3)?.ToString().ToLower() ?? "x64") == "x64";

            if (root == null || KeyPath == null || Valuestring == null)
                return SingleState(CheckInfo.DEFAULT);

            var key = OpenBase(root, Is64);

            if(key == null)
                return SingleState(CheckInfo.DEFAULT);

            List<CheckState> CheckStates = new List<CheckState>();

            string[] KeyPathInfos = KeyPath.Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);

            var ResultantKeyset = GetApplicableKeys(new List<RegistryKey>() { key }, KeyPathInfos, 0);

            foreach(var k in ResultantKeyset)
            {
                if (Valuestring == "*")
                {
                    foreach (var v in k.GetValueNames())
                    {
                        try
                        {
                            string sval = k.Name.ToLower() + $"\\{v.ToLower()}+" + (k.GetValue(v)?.ToString() ?? "");
#if DEBUG
                            if (DebugVerbose)
                                Console.WriteLine(sval);
#endif
                            CheckStates.Add(new CheckState(info.ComputeHash(sval)));
                        }
                        catch { }
                    }
                        
                    continue;
                }

                try
                {
                    string rk = k.Name.ToLower() + $"\\{Valuestring.ToLower()}+" + (k.GetValue(Valuestring)?.ToString() ?? "");
#if DEBUG
                    if (DebugVerbose)
                        Console.WriteLine(rk);
#endif
                    CheckStates.Add(new CheckState(info.ComputeHash(rk)));
                }
                catch { }
            }


            return CheckStates;
        }

        private List<RegistryKey> GetApplicableKeys(List<RegistryKey> RootKeys, string[] KeyPathInfos, int KPI)
        {
            if (KPI >= KeyPathInfos.Length)
                return RootKeys;

            List<RegistryKey> NextApplicableSet = new List<RegistryKey>();

            string CurrentKPI = KeyPathInfos[KPI++];

            foreach(var key in RootKeys)
            {
                if (CurrentKPI == "*" || CurrentKPI == "**")
                {
                    foreach (var sk in key.GetSubKeyNames())
                    {
                        try { NextApplicableSet.Add(key.OpenSubKey(sk)); } catch { }
                    }

                    continue;
                }

                foreach (var sk in key.GetSubKeyNames())
                {
                    if (sk.ToLower() == CurrentKPI.ToLower())
                        try { NextApplicableSet.Add(key.OpenSubKey(sk)); } catch{}
                }
            }

            if(CurrentKPI == "**")
            {
                KPI--;

                if (NextApplicableSet.Count < 1)
                    return RootKeys;

                RootKeys.AddRange(GetApplicableKeys(NextApplicableSet, KeyPathInfos, KPI));

                return RootKeys;
            }
                

            return GetApplicableKeys(NextApplicableSet, KeyPathInfos, KPI);
        }

        private RegistryKey OpenBase(string arg, bool Reg64)
        {
            switch (arg.ToUpper())
            {
                case "HKCR":
                case "HKEY_CLASSES_ROOT":
                case "CLASSES_ROOT":
                case "CLASSESROOT":
                case "CLASSES":
                    return Registry.ClassesRoot;

                case "HKCC":
                case "HKEY_CURRENT_CONFIG":
                case "CURRENT_CONFIG":
                case "CURRENTCONFIG":
                case "CONFIG":
                    return Registry.CurrentConfig;

                case "HKCU":
                case "HKEY_CURRENT_USER":
                case "CURRENT_USER":
                case "CURRENTUSER":
                case "USER":
                    return Registry.CurrentUser;

                case "HKPD":
                case "HKEY_PERFORMANCE_DATA":
                case "PERFORMANCE_DATA":
                case "PERFORMANCEDATA":
                case "PERFORMANCE":
                    return Registry.PerformanceData;

                case "HKU":
                case "HKEY_USERS":
                case "USERS":
                    return Registry.Users;

                default:
                    return RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, Reg64 ? RegistryView.Registry64 : RegistryView.Registry32);
            }
        }
    }
}

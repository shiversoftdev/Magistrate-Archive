using Engine.Installer.Core;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsInstaller.Patches
{
    /*
     * Currently only supports DWORD and STRING
     * RootKey
     * Path
     * ValueKind
     * Key
     * ExpectedValue
     * Reg64? = 32 | 64
     */
    internal static class patch_reg
    {
        /// <summary>
        /// Patch a registry key to a default value
        /// </summary>
        /// <param name="patch"></param>
        /// <returns></returns>
        internal static async Task<Installation.InstallationResult> Patch(PatchDefinition patch)
        {
            if (patch.NumArgs < 5)
                return Installation.InstallationResult.Failure("Failed to patch a registry value because the patch was malformed" + patch.PatchKey);
            return await PatchFromArgs(patch.Args);
        }

        internal static async Task<Installation.InstallationResult> PatchFromArgs(params string[] args)
        {
            RegistryKey Root = null;
            bool Reg64;
            string RegPath = args[1];
            Enum.TryParse(args[2].ToLower().Trim(), true, out RegistryValueKind kind);
            string RegVal = args[3];
            string ExpectedValue = args[4];

            Reg64 = false;

            if (args.Length > 5)
            {
                Reg64 = args[5].Trim() == "64";
            }

            switch (args[0].ToUpper())
            {
                case "HKEY_CLASSES_ROOT":
                case "CLASSES_ROOT":
                case "CLASSESROOT":
                case "CLASSES":
                    Root = Registry.ClassesRoot;
                    break;
                case "HKEY_CURRENT_CONFIG":
                case "CURRENT_CONFIG":
                case "CURRENTCONFIG":
                case "CONFIG":
                    Root = Registry.CurrentConfig;
                    break;
                case "HKEY_CURRENT_USER":
                case "CURRENT_USER":
                case "CURRENTUSER":
                case "USER":
                    Root = Registry.CurrentUser;
                    break;
                case "HKEY_PERFORMANCE_DATA":
                case "PERFORMANCE_DATA":
                case "PERFORMANCEDATA":
                case "PERFORMANCE":
                    Root = Registry.PerformanceData;
                    break;
                case "HKEY_USERS":
                case "USERS":
                    Root = Registry.Users;
                    break;
                default:
                    Root = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine,
                                                    Reg64 ? RegistryView.Registry64 : RegistryView.Registry32);
                    break;
            }
            try
            {
                RegistryKey RegKey = Root.CreateSubKey(RegPath, true);
                switch (kind)
                {
                    case RegistryValueKind.String:
                        RegKey.SetValue(RegVal, ExpectedValue, RegistryValueKind.String);
                        break;
                    case RegistryValueKind.DWord:
                        RegKey.SetValue(RegVal, Convert.ToInt32(ExpectedValue), RegistryValueKind.DWord);
                        break;
                }

            }
            catch
            {
                return Installation.InstallationResult.Failure("Registry key couldnt be patched!");
            }

            return Installation.InstallationResult.Success;
        }

        internal static void CopyRegTo(this RegistryKey src, RegistryKey dst)
        {
            // copy the values
            foreach (var name in src.GetValueNames())
            {
                dst.SetValue(name, src.GetValue(name, null, RegistryValueOptions.DoNotExpandEnvironmentNames), src.GetValueKind(name));
            }

            // copy the subkeys
            foreach (var name in src.GetSubKeyNames())
            {
                using (var srcSubKey = src.OpenSubKey(name, false))
                {
                    var dstSubKey = dst.CreateSubKey(name);
                    srcSubKey.CopyRegTo(dstSubKey);
                }
            }
        }
    }
}

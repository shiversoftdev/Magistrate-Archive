using Engine.Installer.Core;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WindowsInstaller.Patches
{
    /*
     *  key
     *  value
     */
    internal static class patch_secedit
    {

        /// <summary>
        /// Patch a security policy into the installation process
        /// </summary>
        /// <param name="patch"></param>
        /// <returns></returns>
        internal static async Task<Installation.InstallationResult> Patch(PatchDefinition patch)
        {
            if (patch.NumArgs < 2)
                return Installation.InstallationResult.Failure("Failed to patch a security policy because the patch was malformed " + patch.PatchKey);

            string path = Path.GetTempFileName();

            string exportargs = String.Format(@"/c secedit /export /cfg ""{0}"" /quiet", path);
            string importargs = String.Format(@"/c secedit /configure /db secedit.sdb /cfg ""{0}"" /quiet", path);

            await Extensions.StartProcess("cmd.exe", exportargs, Environment.CurrentDirectory, null, Console.Out, Console.Error);

            string[] lines = File.ReadAllLines(path);
            
            for(int i = 0; i < lines.Length; i++)
            {
                string[] split = lines[i].Split('=');
                if (split.Length < 2 || split[0].Contains("["))
                    continue;
                if(split[0].Trim().ToLower() == patch.Args[0].Trim().ToLower())
                {
                    bool isReg = false;
                    if (split[0].Contains('\\'))
                        isReg = true;

                    split[1] = patch.Args[1].Trim();
                    lines[i] = String.Format("{1}{0}={0}{2}", isReg ? " " : "", split[0], split[1]);
                }
            }

            File.WriteAllLines(path, lines);

            await Extensions.StartProcess("cmd.exe", importargs, Environment.CurrentDirectory, null, Console.Out, Console.Error);

            if (File.Exists("secedit.sdb"))
                File.Delete("secedit.sdb");
            if (File.Exists(path))
                File.Delete(path);

            return Installation.InstallationResult.Success;
        }

    }
}

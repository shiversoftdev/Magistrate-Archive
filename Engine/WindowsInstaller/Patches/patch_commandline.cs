using Engine.Installer.Core;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsInstaller.Patches
{
    /*
     *  exe
     *  args
     */
    internal static class patch_commandline
    {

        /// <summary>
        /// Patch a command into the installation process
        /// </summary>
        /// <param name="patch"></param>
        /// <returns></returns>
        internal static async Task<Installation.InstallationResult> Patch(PatchDefinition patch)
        {
            if (patch.NumArgs < 2)
                return Installation.InstallationResult.Failure("Failed to patch a command because the command line was malformed " + patch.PatchKey);

            try { await Extensions.StartProcess(patch.Args[0], patch.Args[1], Environment.CurrentDirectory, null, Console.Out, Console.Error); } 
            catch { return Installation.InstallationResult.Failure("Failed to patch a command because the process could not start " + patch.PatchKey); }

            return Installation.InstallationResult.Success;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Installer.Core;

namespace WindowsInstaller.Patches
{
    /// <summary>
    /// Installs firefox
    /// </summary>
    internal static class patch_firefox
    {
        /// <summary>
        /// Patches in firefox
        /// </summary>
        /// <param name="patch"></param>
        /// <returns></returns>
        internal static async Task<Installation.InstallationResult> Patch(PatchDefinition patch)
        {
            return await patch_choco.ChocoRequest("install", "firefox", "35.0");
        }
    }
}

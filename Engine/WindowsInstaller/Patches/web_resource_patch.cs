using Engine.Installer.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsInstaller.Patches
{
    internal static class web_resource_patch
    {
        /// <summary>
        /// Args: string WebLocation, bool IsZip, string... local_install_locations
        /// </summary>
        /// <param name="patch"></param>
        /// <returns></returns>
        internal static async Task<Installation.InstallationResult> Patch(PatchDefinition patch)
        {
            if (patch.NumArgs < 3)
                return Installation.InstallationResult.Failure("Failed to install web resource because the patch was malformed.." + patch.PatchKey);
            string url = patch.Args[0];
            bool IsZip = patch.Args[1].Trim().ToLower() == "true"; // boolean.parse would probably throw an exception i dont want to handle.
            string[] installLocations = patch.Args.ToList().GetRange(2, patch.Args.Length - 2).ToArray();

            bool Result;
            if(IsZip)
            {
                foreach(string s in installLocations)
                {
                    Result = await Networking.DownloadZip(url, s, patch.PatchKey + "__.zip");
                    if (!Result)
                        return Installation.InstallationResult.Failure("Failed to install web resource because an install location couldnt be handled");
                }
            }
            else
            {
                Uri uri = new Uri(url);
                string filename;
                try
                {
                    filename = Path.GetFileName(uri.LocalPath);
                }
                catch
                {
                    return Installation.InstallationResult.Failure("Failed to install web resource because an install was not a file");
                }

                foreach (string s in installLocations)
                {
                    Result = await Networking.DownloadResource(url, Environment.ExpandEnvironmentVariables(s), filename);
                    if (!Result)
                        return Installation.InstallationResult.Failure("Failed to install web resource because an install location couldnt be handled");
                }
            }

            return Installation.InstallationResult.Success;
        }
    }
}

using Engine.Installer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsInstaller.Patches
{
    internal static class patch_choco
    {
        private static bool ChocoInstalled
        {
            get
            {
                return System.IO.File.Exists(ChocoPath); //Todo: this is janky. Not as janky as before but we could do better id say
            }
        }

        internal static string ChocoPath
        {
            get
            {
                return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"chocolatey\bin\choco.exe");
            }
        }

        /// <summary>
        /// Patches in choco
        /// </summary>
        /// <param name="patch"></param>
        /// <returns></returns>
        internal static async Task<Installation.InstallationResult> Install()
        {
            if(ChocoInstalled)
                return Installation.InstallationResult.Success;

            int exit_code = await Extensions.StartProcess(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), @"WindowsPowerShell\v1.0\powershell.exe"), "-NoProfile -InputFormat None -ExecutionPolicy Bypass -Command \"iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))\"", null, null, Console.Out, Console.Out);

            if (exit_code != 0)
                return new Installation.InstallationResult() { ErrorLevel = Installation.InstallationResultStatus.Failed, Message = "Couldn't install choco" };

            Environment.SetEnvironmentVariable("PATH", Environment.GetEnvironmentVariable("PATH") + @";%ALLUSERSPROFILE%\chocolatey\bin", EnvironmentVariableTarget.Machine);

            return Installation.InstallationResult.Success;
        }

        /// <summary>
        /// Request a choco package
        /// </summary>
        /// <param name="command">The command to request</param>
        /// <param name="package">The package name</param>
        /// <param name="version">The version (if any) to request</param>
        /// <param name="source">The source repo to consider</param>
        /// <returns></returns>
        internal static async Task<Installation.InstallationResult> ChocoRequest(string command, string package, string version = null, string source = null)
        {
            await Install();

            if (!ChocoInstalled)
                return new Installation.InstallationResult() { ErrorLevel = Installation.InstallationResultStatus.Failed, Message = "Choco failed to install"};
            
            string arguments = command + " " + package + " --yes -x --force --ignoredetectedreboot --svc";

            if (version != null)
                arguments += " --version " + version;

            //TODO Source support

            int exit_code = await Extensions.StartProcess(ChocoPath, arguments, null, null, Console.Out, Console.Out);

            if (exit_code != 0)
                return new Installation.InstallationResult() { ErrorLevel = Installation.InstallationResultStatus.Failed, Message = "Failed to" + command + " package " + package };

            return Installation.InstallationResult.Success;
        }

        /// <summary>
        /// Choco request patch
        /// </summary>
        /// <param name="p">Patch definition for the patch</param>
        /// <returns></returns>
        internal static async Task<Installation.InstallationResult> Patch(PatchDefinition p)
        {
            if (p.Args.Length < 2)
                return new Installation.InstallationResult() { ErrorLevel = Installation.InstallationResultStatus.Failed, Message = "Improperly formatted choco request received" };

            string command = p.Args[0];
            string package = p.Args[1];
            string version = null;
            string source = null;

            if (p.Args.Length > 2)
                version = p.Args[2];

            if (p.Args.Length > 3)
                source = p.Args[3];

            return await ChocoRequest(command, package, version, source);
        }
    }
}

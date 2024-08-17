using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LinuxInstaller
{
    class Program
    {
        
        [STAThread]
        static async Task Main(string[] args) //TODO: Check uid. must be root. 
        {
            if (System.IO.File.Exists("noinstall.lck"))
                return;
            if (Engine.Installer.Core.Installation.LoadState())
                goto loaded;
uidEnter:
            Report("Please enter your one time installation key:");
            string mkey;
            while(!Engine.Installer.Core.Utilities.ValidateUID(mkey = Console.ReadLine()))
                Report("Invalid key.\nPlease enter your one time installation key:");

            Report("Key validated... Downloading installation data...");
            byte[] IData = await Engine.Installer.Core.Installation.RetrieveInstallationFromUID(mkey); //TODO: move this logic to the core so we can handle stuff the same way
            if(IData == null)
            {
                Report("Installation package failed to download. Please verify your internet connection and that you can access www.shiversoft.net.");
                goto uidEnter;
            }

            Report("Installation Data downloaded. Collecting Templates...");
            Engine.Installer.Core.Installation.LoadInstallationInformation(IData);

loaded:
            Engine.Installer.Core.Installation.CollectTemplates(@"Templates");

            Engine.Installer.Core.Installation.BuildEngine = BuildLinux;
            Engine.Installer.Core.Installation.InstallEngine = InstallLinux;
            //Engine.Installer.Core.Installation.PlatformSealEngine = SealLinux;
            Engine.Installer.Core.Installation.ReportInstallationProgress = ReportProgress;
            Engine.Installer.Core.Installation.RestartSequenceDelegate = RestartSequenceInvoked;
            DefinePatchingMethods();
            await Engine.Installer.Core.Installation.Install();
            Console.ReadKey(true);
        }

        private static async System.Threading.Tasks.Task<Engine.Installer.Core.Installation.InstallationResult> InstallLinux()
        {
            //TODO
            return Engine.Installer.Core.Installation.InstallationResult.Success;
        }

        private static async System.Threading.Tasks.Task<Engine.Installer.Core.Installation.InstallationResult> RestartSequenceInvoked()
        {
            //Wont work for the .net reinstallation, but we dont need to restart the installer for .net on this so idk
            try { Process.Start("/bin/bash", "echo \"sleep 10; \\\"" + System.Reflection.Assembly.GetEntryAssembly().Location + "\\\"\" | at now"); } catch { };
            return Engine.Installer.Core.Installation.InstallationResult.Success;
        }

        private static void DefinePatchingMethods()
        {
            //TODO
        }

        private static async System.Threading.Tasks.Task<Engine.Installer.Core.Installation.InstallationResult> BuildLinux()
        {
            return await Engine.Installer.Core.Installation.BuildLinuxEngine();
        }

        private static void ReportProgress(string status, float progress, bool finished, bool failed)
        {
            if (finished)
            {
                if (failed)
                    Report(status); //TODO: Invoke a retry mechanism
                else
                    Report(status); //TODO: Invoke a success mechanism
                return;
            }
            Report("(" + Math.Round(progress * 100, 1) + "%) " + status);
        }

        static void Report(string message)
        {
            Console.WriteLine(message);
        }
    }
}

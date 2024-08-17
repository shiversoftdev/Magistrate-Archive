using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

//References for later in case engine builds run into issues
//https://docs.microsoft.com/en-us/visualstudio/install/workload-component-id-vs-build-tools?view=vs-2017
//https://docs.microsoft.com/en-us/nuget/tools/nuget-exe-cli-reference
//https://docs.microsoft.com/en-us/visualstudio/msbuild/common-msbuild-project-properties?view=vs-2017
//https://docs.microsoft.com/en-us/visualstudio/install/use-command-line-parameters-to-install-visual-studio?view=vs-2017


namespace Engine.Installer.Core
{//TODO: proper error levels, ie: enumerate them
 //TODO: Add random offsets to every check expected value based on the installation package we receive. Prevents memory searching for values because the hashing gets salted
 //TODO: Create a delegate to clean up old installations and force the installation to work. Really need to work on making this installation process work regardless of certain things failing. Like adding retries, etc.
    /// <summary>
    /// The global installation singleton
    /// </summary>
    public static class Installation
    {
        private const string WinEnginePath = @"C:\ss-scoring";
        private const string LinEnginePath = @"/ss-scoring";
        public const string InstallDir = @"Installation";
        public const string EnginesrcDir = @"Engine";
        private const string RepoName = @"csse-master";
        private const string CoreName = @"InstallerCore";
        private const string TemplateFilename = @"CheckTemplate.cs";
        private const string MSBuildInstaller = @"vs_buildtools.exe";

        private const string SolutionName = @"ScoringEngine.sln";
        private const string NugetURL = @"https://dist.nuget.org/win-x86-commandline/v4.9.3/nuget.exe";

        private const string LinuxProjectPath = "LinuxInstaller";
        private const string LinuxProjectName = "LinuxInstaller.csproj";

        private const string InstallerProgFile = "in.stall";
        private const string InstallerEngineFile = "bin.stall";

        private static bool ErrorEncountered = false;

        private enum InstallerStates
        {
            Start,
            Finished,
            Build,
            CopyBin,
            PlatformInstall,
            Patching,
            Finalize
        }

        private static InstallerStates CurrentState;

        private static readonly string[] NonLinuxProjects = new string[] //TODO remember this lol
        {
            "CPVulnerabilityFramework/WindowsTemplates.csproj",
            "notify/SSWinNotify.csproj",
            "TestingGUI/WindowsTestGUI.csproj",
            "WindowsEngine/WindowsEngine.csproj",
            "WindowsInstaller/WindowsInstaller.csproj",
            "WindowsOfflineForensics/WindowsOfflineForensics.csproj",
            "srpview/srpview.csproj",
            "LinuxTestingConsole/LinuxTestingConsole.csproj"
        };

        public static Dictionary<PatchDefinition.PatchKeys, InstallerPatchDelegate> PatchingMethods = new Dictionary<PatchDefinition.PatchKeys, InstallerPatchDelegate>();

        /// <summary>
        /// Delegate for handling platform specific building steps
        /// </summary>
        public static InstallationTaskDelegate BuildEngine;

        /// <summary>
        /// Delegate for handling platform specific installation steps
        /// </summary>
        public static InstallationTaskDelegate InstallEngine;

        /// <summary>
        /// Delegate for sealing the engine per platform
        /// </summary>
        public static InstallationTaskDelegate PlatformSealEngine;

        /// <summary>
        /// Delegate for preparing the engine for installation per platform
        /// </summary>
        public static InstallationTaskDelegate PlatformPrepareInstallPath;

        /// <summary>
        /// Reports progress back to the owning thread
        /// </summary>
        public static ReportProgressDelegate ReportInstallationProgress;

        /// <summary>
        /// A delegate to perform when restarting the installer
        /// </summary>
        public static InstallationTaskDelegate RestartSequenceDelegate;



        private static string PublicReadPath
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "csse-pub");
            }
        }

        public static string EnginePath
        {
            get
            {
                return (CurrentInstallation?.HasFlag(InstallationPackage.InstallFlags.Linux) ?? false) ? LinEnginePath : WinEnginePath;
            }
        }

        public static string NugetPath
        {
            get
            {
                return Path.Combine(RepoPath, "nuget.exe");
            }
        }

        public static string ConfigurationName
        {
            get
            {
#if DEBUG
                string mode = "Debug" + ((CurrentInstallation?.HasFlag(InstallationPackage.InstallFlags.Offline) ?? false) ? "Offline" : "Online");
#else
                string mode = "Release";
#endif
                mode = "Release";
                return mode;
            }
        }

        public static string SolutionPath
        {
            get
            {
                return Path.Combine(RepoPath, SolutionName);
            }
        }

        private static string LinuxInstallerPath
        { 
            get
            {
                return Path.Combine(RepoPath, LinuxProjectPath, LinuxProjectName);
            }
        }

        public static string InstallPath
        {
            get
            {
                return Path.Combine(EnginePath, InstallDir);
            }
        }

        public static string SourcePath
        {
            get
            {
                return Path.Combine(EnginePath, InstallDir, EnginesrcDir);
            }
        }

        public static string RepoPath
        {
            get
            {
                return Path.Combine(SourcePath, RepoName);
            }
        }

        public static string CheckTemplatePath
        {
            get
            {
                return Path.Combine(RepoPath, CoreName, TemplateFilename);
            }
        }

        public static string MSBuildInstallerPath
        {
            get
            {
                return Path.Combine(RepoPath, MSBuildInstaller);
            }
        }

        public static string EngineBinariesPath
        {
            get
            {
                return (CurrentInstallation?.HasFlag(InstallationPackage.InstallFlags.Linux) ?? false) ? Path.Combine(RepoPath, "LinuxEngine", "bin", ConfigurationName) : Path.Combine(RepoPath, "WindowsEngine", "bin", ConfigurationName);
            }
        }

        public static string EngineLocation
        {
            get
            {
                return Path.Combine(EnginePath, "Engine");
            }
        }

        /// <summary>
        /// The current installation data
        /// </summary>
        internal static InstallationPackage CurrentInstallation;

        /// <summary>
        /// Try to parse installation info from the provided IData
        /// </summary>
        /// <param name="IData">The Installation data to parse</param>
        public static void LoadInstallationInformation(byte[] IData)
        {
            CurrentInstallation = new InstallationPackage(IData);
        }

        /// <summary>
        /// Retrieve an installation package based on a UID input
        /// </summary>
        /// <param name="UID">One time use UID</param>
        /// <returns></returns>
        public static async Task<byte[]> RetrieveInstallationFromUID(string UID)
        {
#if DEBUG
            try
            {
                return System.IO.File.ReadAllBytes(Path.Combine("Testing", "install.bin"));
            }
            catch
            {
                return null;
            }
#endif
            //TODO grab from server
            return null;
        }

        /// <summary>
        /// Collect the templates from the current installation
        /// <paramref name="templatesdirectory">The directory to load templates from</paramref>
        /// </summary>
        public static void CollectTemplates(string templatesdirectory) //Potentially obsolete because of the build process
        {
            if (CurrentInstallation == null)
                return;
            try
            {
                DirectoryInfo TemplateDir = new DirectoryInfo(templatesdirectory);
                foreach (FileInfo template in TemplateDir.GetFiles("*.cst", SearchOption.AllDirectories))
                {
                    string templatename = template.Name.ToLower().Replace(".cst", "");
                    if(Enum.TryParse(templatename, true, out CheckTypes checktype))
                    {
                        CurrentInstallation.RuntimeTemplates[checktype] = template.FullName;
                    }
                }
            }
            catch
            {
                //todo report the security error to the installer
            }
        }

        /// <summary>
        /// Try to parse installation info from the provided install.bin
        /// </summary>
        /// <param name="FilePath">The file path to install.bin</param>
        /// <exception cref="FileNotFoundException"></exception>
        public static void LoadInstallationInformation(string FilePath)
        {
            LoadInstallationInformation(File.ReadAllBytes(FilePath));
        }

        /// <summary>
        /// Begin the installation
        /// </summary>
        public static async System.Threading.Tasks.Task<InstallationResult> Install()
        {

            //Step 0. Pre-Setup
#if DEBUG
#else
            try { foreach (var process in System.Diagnostics.Process.GetProcessesByName("explorer")) process.Kill();} catch { } //Kill windows explorer so installation ops are un-interrupted
#endif

            //Installer state machine for resuming installations
            InstallationResult currentResult = InstallationResult.Failure("");
            InstallerStates NextState = InstallerStates.Finished;
            while (CurrentState != InstallerStates.Finished)
            {
                currentResult = InstallationResult.Failure("");
                switch (CurrentState)
                {
                    case InstallerStates.Start:
                        currentResult = await StartInstall();
                        NextState = InstallerStates.Build;
                        break;

                    case InstallerStates.Build:
                        currentResult = await BuildSequence();
                        NextState = InstallerStates.CopyBin;
                        break;

                    case InstallerStates.CopyBin:
                        currentResult = await CopyBinaries();
                        NextState = InstallerStates.PlatformInstall;
                        break;

                    case InstallerStates.PlatformInstall:
                        if (InstallEngine == null)
                            return Error("Engine Installation tool is corrupted!");
                        currentResult = await InstallEngine();
                        NextState = InstallerStates.Patching;
                        break;

                    case InstallerStates.Patching:
                        currentResult = await ExecutePatching();
                        NextState = InstallerStates.Finalize;
                        break;

                    case InstallerStates.Finalize:
                        currentResult = await Finalize();
                        NextState = InstallerStates.Finished;
                        break;

                    default:
                        break;
                }

                if (currentResult == null || currentResult.ErrorLevel != InstallationResultStatus.Working)
                    CurrentState = InstallerStates.Finished;
                else
                    CommitState(NextState);
            }

            if (currentResult != null && currentResult.ErrorLevel != InstallationResultStatus.Complete)
                return Error(currentResult);

            else if (currentResult == null)
                currentResult = InstallationResult.Success;

            return currentResult;
        }

        private static async Task<InstallationResult> StartInstall()
        {
            CommitState(InstallerStates.Start);

            Report("Preparing Engine...", 0);

            try
            {
                await PlatformPrepareInstallPath?.Invoke();
            }
            catch (NullReferenceException)
            {
                //No worries, no prep defined
            }
            catch
            {
                return Error("Couldn't prepare an installation for the engine! Was the engine previously installed??");
            }

            Report("Downloading engine...", .05f);

            try //cleanup
            {
                if(Directory.Exists(WinEnginePath))
                    Directory.Delete(WinEnginePath, true);
                if (Directory.Exists(LinEnginePath))
                    Directory.Delete(LinEnginePath, true);
                if(File.Exists("Log.txt"))
                    File.Delete("Log.txt");
            }
            catch (IOException e)
            {
                return Error("Couldnt clear the installation path. " + e.ToString());
            }

            //Step 1. Get the engine source
            bool GotEngine = await Networking.DownloadEngine(SourcePath);

            if (!GotEngine)
            {
                return Error("Failed to download the engine source. Please verify that you are connected to the internet and have access to http://www.shiversoft.net");
            }

            Report("Patching engine...", .1f);
            //Step 2. Patch the engine encryption key (or.... dont. Translator will do this)
            //await PatchCheckKey();

            //Step 3. Copy the installation data into the engine build folder
            await PatchInstallBinary();
            return InstallationResult.Success;
        }

        private static async Task<InstallationResult> BuildSequence()
        {
            Report("Building engine...", .15f);
            //Step 4. Compile the engine using the correct compiler for the platform, including the target build mode for the platform
            //need to target Release configuration, define or not define OFFLINE constant, clean solution, and call rebuild on the whole solution
            //the target release package will be in a const folder with the filename "install.bin"
            if (BuildEngine == null || InstallEngine == null)
            {
                return Error("Current platform is not supported!");
            }
            else
            {
                return await BuildEngine();
            }
        }

        private static async Task<InstallationResult> CopyBinaries()
        {
            for(int i = 0; i < 3; i++)
            {
                try
                {
                    if (Directory.Exists(PublicReadPath))
                        Directory.Delete(PublicReadPath, true);
                    if (Directory.Exists(EngineLocation))
                    {
                        var EDDI = new DirectoryInfo(EngineLocation);
                        EDDI.Attributes &= ~FileAttributes.Hidden & ~FileAttributes.System;
                        Directory.Delete(EngineLocation, true); //wont work if there is a running service here
                    }


                    Directory.CreateDirectory(PublicReadPath);
                    var PRDI = new DirectoryInfo(PublicReadPath);
                    PRDI.Attributes |= FileAttributes.Hidden | FileAttributes.System;

                    Directory.Move(EngineBinariesPath, EngineLocation);

                    Report("Installing engine...", .6f);
                    return InstallationResult.Success;
                }
                catch
                {
                    Log("Directory unavailable... retrying in 250ms");
                    await Task.Delay(250);
                }
            }
            return Error("Failed to copy engine binaries!");
        }

        private static async Task<InstallationResult> ExecutePatching()
        {
            Report("Patching environment...", .75f);
            //Step 7. Deploy system patching

            List<PatchDefinition> FinalList = new List<PatchDefinition>();
            List<PatchDefinition> Others = new List<PatchDefinition>();
            Others.AddRange(CurrentInstallation.Patches);
            for(int i = 0; i < Others.Count; i++)
            {
                try
                {
                    PatchDefinition.PatchKeys key = (PatchDefinition.PatchKeys)Others[i].PatchKey;
                    if(key == PatchDefinition.PatchKeys.UserPatch)
                    {
                        FinalList.Add(Others[i]);
                        Others.RemoveAt(i);
                        i--;
                    }
                }
                catch
                {

                }
            }

            FinalList.AddRange(Others);

            foreach (PatchDefinition p in FinalList)
            {
                try
                {
                    PatchDefinition.PatchKeys key = (PatchDefinition.PatchKeys)p.PatchKey;
                    if (PatchingMethods.ContainsKey(key))
                    {
                        InstallationResult r = await PatchingMethods[key]?.Invoke(p) ?? InstallationResult.Failure("Patch " + key + " failed to install because the patching method was not defined!");
                        if (r == null)
                        {
                            Log("Patch " + key + " failed to install because the patching method was not defined!");
                            WarnUser();
                        }
                            

                        if (r != null && r.ErrorLevel != InstallationResultStatus.Working)
                        {
                            Log(r.Message);
                            WarnUser();
                        }
                    }
                    else
                    {
                        Log("Patch with key " + key + " was found but the key has no patch application definition...");
                        WarnUser();
                    }
                }
                catch (Exception e)
                {
                    Log("Failed to apply patch! " + e.ToString());
                    WarnUser();
                }
            }
            return InstallationResult.Success;
        }

        private static async Task<InstallationResult> Finalize()
        {
            Report("Sealing Engine...", .85f);
            //Step 8. Remove any installation artifacts and remove this process TODO

            try
            {
                Directory.Delete(InstallPath, true); 
            }
            catch
            {
                Log("Failed to delete installation path... attempting to cleanse what we can"); //TODO if we cant delete the directory. delete everything we can.
                WarnUser();

            }

            try
            {
                SealEngine();
            }
            catch (Exception e)
            {
                Log(e.ToString() + e.ToString());
                return Error("Failed to seal the engine. Critical security error.");
            }

            Report("Finished...", 1.0f, true, false);
            return InstallationResult.Success;
        }

        private static void Log(string message)
        {
#if DEBUG
            File.AppendAllLines("Log.txt", new string[] { message });
#endif
        }

        public static InstallationResult Error(InstallationResult r)
        {
            return Error(r?.Message ?? "Unrecognized error!");
        }

        public static InstallationResult Error(string message)
        {
            WarnUser();
            Report(message, 0, true, true);
            Log("ERROR: " + message);
            return new InstallationResult() { Message = message, ErrorLevel = InstallationResultStatus.Failed };
        }

        private static void SealEngine()
        {

            var EDI = new DirectoryInfo(EngineLocation);
            EDI.Attributes |= FileAttributes.Hidden | FileAttributes.System | FileAttributes.ReadOnly;

            foreach (FileInfo file in EDI.GetFiles())
            {
                file.Attributes |= FileAttributes.Hidden | FileAttributes.System | FileAttributes.ReadOnly;
            }

            EDI.LastAccessTime = DateTime.Today;
            EDI.LastAccessTimeUtc = DateTime.Today;
            EDI.LastWriteTime = DateTime.Today;
            EDI.LastWriteTimeUtc = DateTime.Today;
            EDI.CreationTime = DateTime.Today;

            //TODO: Seal the installer. Should write a lck file to prevent the installer from running twice on the same image. Can also detect an installation.
            //Anti-cheat will handle installation cancellations in the future.
            PlatformSealEngine?.Invoke();
        }

        /// <summary>
        /// Patch in the installation binary information to use when building a release configuration
        /// </summary>
        /// <returns></returns>
        private static async Task PatchInstallBinary()
        {
            try
            {
                File.WriteAllBytes(Path.Combine(RepoPath, "_build", "_install.bin"), CurrentInstallation);
            }
            catch
            {
                Error("Failed to patch installation binary");
            }
        }

        private static async Task PatchCheckKey()
        {
            try
            {
                string text = File.ReadAllText(CheckTemplatePath);
                byte[] newkey = new byte[16];
                Random r = new Random((int)DateTime.Now.Ticks);
                await Task.Delay(r.Next(5) * 10); //some entropy to offset actual random time seeding
                r.NextBytes(newkey);
                string keytext = "{";
                for(int i = 0; i < 16; i++)
                {
                    keytext += newkey[i] + (i != 15 ? "," : "};//");
                }
                text = text.Replace("/*?installer.key*/", keytext);
                File.WriteAllText(CheckTemplatePath, text);
            }
            catch
            {
                //Report as a fatal error. this is a security compromise and cannot be waivered. TODO
            }
        }


#pragma warning disable IDE0051 // Remove unused private members
        /// <summary>
                               /// Installs MSBuild
                               /// </summary>
                               /// <returns></returns>
        public static async Task<bool> InstallMSBuild()
#pragma warning restore IDE0051 // Remove unused private members
        {
            if (!File.Exists(MSBuildInstallerPath))
                return false;
            //vs_buildtools.exe --add Microsoft.VisualStudio.Workload.MSBuildTools --quiet
            //MSBuildPath
            try
            {
                int result = await Extensions.StartProcess(MSBuildInstallerPath, "--add Microsoft.VisualStudio.Workload.MSBuildTools --add Microsoft.VisualStudio.Component.NuGet --add Microsoft.VisualStudio.Component.NuGet.BuildTools --add Microsoft.Net.Component.4.7.2.SDK --add Microsoft.Net.Component.4.7.2.TargetingPack --add Microsoft.VisualStudio.Component.Roslyn.Compiler --add Microsoft.VisualStudio.Workload.NetCoreBuildTools --wait --quiet --norestart", null, null, Console.Out, Console.Out);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Build the windows engine using MSBuild
        /// </summary>
        /// <returns></returns>
        public static async Task<InstallationResult> BuildWindowsEngine(string MSBuildPath)
        {
            //msbuild CommunityServer.Sync.sln /p:Configuration=Release
            try
            {
                int result = 0;
                if(!File.Exists(NugetPath))
                {
                    bool gotNuget = await Networking.DownloadResource(NugetURL, RepoPath, "nuget.exe");
                    if (!gotNuget)
                        return InstallationResult.Failure("Couldn't retrieve nuget");
                }

                StringWriter logwriter = new StringWriter();

                result = await Extensions.StartProcess(NugetPath, "restore \"" + SolutionPath + "\"", RepoPath, null, logwriter, logwriter);

                if (result == 0)
                    result += await Extensions.StartProcess(MSBuildPath, "\"" + SolutionPath + "\" -p:WarningLevel=0 -p:Configuration=" + ConfigurationName + ((CurrentInstallation?.HasFlag(InstallationPackage.InstallFlags.Offline) ?? false) ? " -p:DefineConstants=OFFLINE" : ""), RepoPath, null, logwriter, logwriter);

                if(result != 0)
                {
                    return InstallationResult.Failure(logwriter.ToString());
                }

                return InstallationResult.Success;
            }
            catch(Exception e)
            {
                return InstallationResult.Failure(e.ToString());
            }
        }

        public static async Task<InstallationResult> BuildLinuxEngine()
        {
            try
            {
                int result = 0;

                StringWriter logwriter = new StringWriter();

                result += await Extensions.StartProcess("dotnet", "sln \"" + SolutionPath + "\" remove " + GetLinuxToRemoveList(), RepoPath, null, logwriter, logwriter);

                result = await Extensions.StartProcess("dotnet", "restore \"" + SolutionPath + "\"", RepoPath, null, logwriter, logwriter);

                result += await Extensions.StartProcess("dotnet", "msbuild \"" + SolutionPath + "\" -p:WarningLevel=0 -p:Configuration=" + ConfigurationName + ((CurrentInstallation?.HasFlag(InstallationPackage.InstallFlags.Offline) ?? false) ? " -p:DefineConstants=OFFLINE" : ""), RepoPath, null, logwriter, logwriter);

                if (result != 0)
                {
                    return InstallationResult.Failure(logwriter.ToString());
                }
                else
                {
                    File.WriteAllText("MSBuild.log", logwriter.ToString());
                }

                return InstallationResult.Success;
            }
            catch (Exception e)
            {
                return InstallationResult.Failure(e.ToString());
            }
        }

        private static string GetLinuxToRemoveList()
        {
            string final = "";
            foreach(string s in NonLinuxProjects)
            {
                final += s + " ";
            }
            return final;
        }

        public delegate System.Threading.Tasks.Task<InstallationResult> InstallationTaskDelegate();

        public delegate Task<InstallationResult> InstallerPatchDelegate(PatchDefinition Patch);

        public delegate void ReportProgressDelegate(string StatusMessage, float progress, bool finished, bool failed);

        public enum InstallationResultStatus
        {
            Working,
            Failed,
            Complete
        }

        /// <summary>
        /// The result of an installation procedure
        /// </summary>
        public sealed class InstallationResult
        {
            /// <summary>
            /// The error level for the installation
            /// </summary>
            public InstallationResultStatus ErrorLevel = InstallationResultStatus.Working;

            /// <summary>
            /// The message of the installation
            /// </summary>
            public string Message = "An unknown error occurred in the installation";

            /// <summary>
            /// The installation finished successfully
            /// </summary>
            public static InstallationResult Success
            {
                get
                {
                    return new InstallationResult() { Message = "The installation completed successfully" };
                }
            }

            /// <summary>
            /// The installation failed
            /// </summary>
            /// <param name="message"></param>
            /// <returns></returns>
            public static InstallationResult Failure(string message)
            {
                return new InstallationResult() { Message = message, ErrorLevel = InstallationResultStatus.Failed };
            }
        }

        private static void Report(string message, float percent, bool complete = false, bool failed = false)
        {
            ReportInstallationProgress?.Invoke(message, percent, complete, failed);
        }

        /// <summary>
        /// Run nuget on solution (windows)
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> RunNugetOnSolution()
        {
            if (!File.Exists(NugetPath))
            {
                bool gotNuget = await Networking.DownloadResource(NugetURL, RepoPath, "nuget.exe");
                if (!gotNuget)
                    return false;
            }
            int result = await Extensions.StartProcess(NugetPath, "restore \"" + SolutionPath + "\"", RepoPath, null, Console.Out, Console.Out);
            return result == 0;
        }

        /// <summary>
        /// Commit the installer state to disk
        /// </summary>
        /// <param name="state"></param>
        private static void CommitState(InstallerStates state)
        {
            try { 
                File.WriteAllBytes(InstallerProgFile, BitConverter.GetBytes((int)state));
                File.WriteAllBytes(InstallerEngineFile, CurrentInstallation ?? new byte[0]);
            } catch { }
            CurrentState = state;
        }

        /// <summary>
        /// Attempt to load an installation state from the disk
        /// </summary>
        /// <returns>True only if an engine state was loaded</returns>
        public static bool LoadState()
        {
            if (!File.Exists(InstallerProgFile))
                return false;
            try
            {
                byte[] bytes = File.ReadAllBytes(InstallerProgFile);
                CurrentState = (InstallerStates)BitConverter.ToInt32(bytes, 0);
                if(File.Exists(InstallerEngineFile))
                {
                    LoadInstallationInformation(File.ReadAllBytes(InstallerEngineFile));
                    return true;
                }
            }
            catch
            {
                try
                {
                    CommitState(CurrentState);
                }
                catch
                {
                }
            }
            return false;
        }

        public static void RestartInstaller()
        {
            CommitState(CurrentState);
            RestartSequenceDelegate?.Invoke();
            Environment.Exit(0);
        }

        private static void WarnUser()
        {
            ErrorEncountered = true;
        }
    }
}

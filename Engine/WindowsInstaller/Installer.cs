using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;
using Engine.Installer.Core;

/* TODO:
 * Call commence
 * Commence does a backend load with reporting (can use background manager with error reporting, i think thats the safest bet
 * Meanwhile, do the windows flashy install thing with random quote messages and color changes
 * 
 * 
 * 
 * 
 * 
 * https://docs.microsoft.com/en-us/windows/win32/services/interactive-services -- Force interactive services in the registry
 * 
 */

namespace WindowsInstaller
{
    public partial class Installer : Form
    {
        private const bool StandaloneInstaller = true;
        private string Key;
        private byte phase;
        private int count;
        private byte[] IData;
        private bool Resumed;
        //private readonly Image LoadIcon = Properties.Resources.win_load;
        //BackgroundWorker InstallerWorker;

        private readonly List<Color> RandomColors = new List<Color>()
        {
            Color.FromArgb(255,32,32,48),
            Color.FromArgb(255,142,0,0),
            Color.FromArgb(255,35,94,188),
            Color.FromArgb(255,0,112,26),
            Color.FromArgb(255,255,143,0),
            Color.FromArgb(255,86,0,39),
            Color.FromArgb(255,0,77,64),
            Color.FromArgb(255,26,35,126),
            Color.FromArgb(255,183,28,28),
            Color.FromArgb(255,191,54,12),
            Color.FromArgb(255,120,0,46),
        };

        private int colorindex = 0;
        private Color CurrentColor
        {
            get
            {
                return RandomColors[colorindex];
            }
        }

        private Color NextColor
        {
            get
            {
                if (colorindex + 1 >= RandomColors.Count)
                    return RandomColors[0];
                return RandomColors[colorindex + 1];
            }
        }

        public Installer()
        {
            InitializeComponent();
#if DEBUG
            Debug();
#endif
            //InstallerWorker = new BackgroundWorker();
            UniqueID.Location = new Point(0, (int)(this.Height * .6));
            StatusMessage.Location = new Point(0, UniqueID.Location.Y + 67);
            UniqueID.TextChanged += UniqueID_TextChanged;
            IntroTimer.Tick += IntroTimer_Tick;

            if (Resumed = Engine.Installer.Core.Installation.LoadState())
            {
                IntroTimer.Interval = 10;
                WelcomeLabel.Text = "This will take several minutes...";
                StatusMessage.ForeColor = Color.White;
                phase = 6; //resumestate
            }

            IntroTimer.Start();
        }

        private void UniqueID_TextChanged(object sender, EventArgs e)
        {
            if(UniqueID.MaskFull)
            {
                bool result = KeyMeetsCriteria(UniqueID.Text);
                if (result)
                {
                    StatusMessage.ForeColor = Color.White;
                    StatusMessage.Text = "";
                    UniqueID.ForeColor = Color.LightGreen;
                    UniqueID.ReadOnly = true;
                    StatusMessage.Focus();
                    phase = 3;
                    IntroTimer.Start();
                }
                else
                {
                    StatusMessage.ForeColor = Color.LightCoral;
                    StatusMessage.Text = "Invalid Unique Identifier.";
                }
            }
            else
            {
                StatusMessage.ForeColor = Color.White;
                StatusMessage.Text = "";
            }
        }

        /// <summary>
        /// Precheck for key validation
        /// </summary>
        /// <param name="key">The key to validate</param>
        /// <returns></returns>
        private bool KeyMeetsCriteria(string key) //note: includes dashes, so final mask needs to reflect 0b1101
        {
            return Engine.Installer.Core.Utilities.ValidateUID(key);
        }

        private void IntroTimer_Tick(object sender, EventArgs e)
        {
            switch(phase)
            {
                case 0:
                    IntroTimer.Stop();
                    IntroTimer.Interval = 10;
                    WelcomeLabel.Text = "Welcome";
                    phase = 1;
                    IntroTimer.Start();
                    break;
                case 1:
                    count++;
                    WelcomeLabel.ForeColor = Color.FromArgb(255, (int)FloatLerp(255, 32, (float)count / 50), (int)FloatLerp(255, 32, (float)count / 50), (int)FloatLerp(255, 48, (float)count / 50));
                    if(count >= 50)
                    {
                        count = 0;
                        WelcomeLabel.Text = "Please enter your Unique ID";
                        UniqueID.Visible = true;
                        UniqueID.Focus();
                        phase = 2;
                    }
                    break;
                case 2:
                    count++;
                    WelcomeLabel.ForeColor = Color.FromArgb(255, (int)FloatLerp(32, 255, (float)count / 50), (int)FloatLerp(32, 255, (float)count / 50), (int)FloatLerp(38, 255, (float)count / 50));
                    UniqueID.ForeColor = WelcomeLabel.ForeColor;
                    if (count >= 50)
                    {
                        phase = 3;
                        count = 0;
                        IntroTimer.Stop();
                    }
                    break;
                case 3:
                    count++;
                    WelcomeLabel.ForeColor = Color.FromArgb(255, (int)FloatLerp(255, 32, (float)count / 50), (int)FloatLerp(255, 32, (float)count / 50), (int)FloatLerp(255, 48, (float)count / 50));
                    if (count >= 50)
                    {
                        count = 0;
                        WelcomeLabel.Text = "Just a second...";
                        phase = 4;
                    }
                    break;
                case 4:
                    count++;
                    WelcomeLabel.ForeColor = Color.FromArgb(255, (int)FloatLerp(32, 255, (float)count / 50), (int)FloatLerp(32, 255, (float)count / 50), (int)FloatLerp(48, 255, (float)count / 50));
                    if (count >= 50)
                    {
                        phase = 5;
                        count = 0;
                        IntroTimer.Stop();
                        ValidateID();
                    }
                    break;
                case 5:
                    count++;
                    WelcomeLabel.ForeColor = Color.FromArgb(255, (int)FloatLerp(255, 32, (float)count / 50), (int)FloatLerp(255, 32, (float)count / 50), (int)FloatLerp(255, 48, (float)count / 50));
                    UniqueID.ForeColor = Color.FromArgb(255, (int)FloatLerp(Color.LightGreen.R, 32, (float)count / 50), (int)FloatLerp(Color.LightGreen.G, 32, (float)count / 50), (int)FloatLerp(Color.LightGreen.B, 48, (float)count / 50));
                    if (count >= 50)
                    {
                        phase = 6;
                        WelcomeLabel.Text = "This will take several minutes...";
                        Controls.Remove(UniqueID);
                        count = 0;
                    }
                    break;
                case 6:
                    count++;
                    WelcomeLabel.ForeColor = Color.FromArgb(255, (int)FloatLerp(32, 255, (float)count / 50), (int)FloatLerp(32, 255, (float)count / 50), (int)FloatLerp(48, 255, (float)count / 50));
                    if (count >= 50)
                    {
                        phase = 7;
                        Controls.Remove(UniqueID);
                        StatusMessage.Visible = false;
                        count = 0;
                        Commence();
                    }
                    break;
                case 7:
                    count++;
                    if (count < 50)
                    {
                        BackColor = Color.FromArgb(255, (int)FloatLerp(CurrentColor.R, NextColor.R, (float)count / 50), (int)FloatLerp(CurrentColor.G, NextColor.G, (float)count / 50), (int)FloatLerp(CurrentColor.B, NextColor.B, (float)count / 50));
                    }
                    else
                    {
                        phase = 8;
                    }
                    break;
                case 8:
                    count++;
                    if (count >= 250)
                    {
                        count = 0;
                        colorindex++;
                        if (colorindex >= RandomColors.Count)
                            colorindex = 0;
                        phase = 7;
                    }
                    break;
            }
        }

        private async void ValidateID()
        {
            //for current, we are going to accept the key using offline installer mode
            if(StandaloneInstaller)
            {
                Key = UniqueID.Text;
                IData = await Engine.Installer.Core.Installation.RetrieveInstallationFromUID(Key);
                if(IData == null)
                {
                    UniqueID.Text = "";
                    WelcomeLabel.Text = "Invalid UID. Could not retrieve installation data. Please try again...";
                    await System.Threading.Tasks.Task.Delay(3000);
                    phase = 1;
                    IntroTimer.Start();
                    return;
                }
                phase = 5;
                IntroTimer.Start();
            }
        }

        /// <summary>
        /// Time to start installing
        /// </summary>
        private void Commence()
        {
            if(StandaloneInstaller)
            {
                if(!Resumed)
                    Engine.Installer.Core.Installation.LoadInstallationInformation(IData);

                Engine.Installer.Core.Installation.CollectTemplates(@"Templates");
                Engine.Installer.Core.Installation.BuildEngine = BuildWindows;
                Engine.Installer.Core.Installation.InstallEngine = InstallWindows;
                Engine.Installer.Core.Installation.PlatformSealEngine = SealWindows;
                Engine.Installer.Core.Installation.ReportInstallationProgress = ReportProgress;
                Engine.Installer.Core.Installation.PlatformPrepareInstallPath = PrepPlatform;
                Engine.Installer.Core.Installation.RestartSequenceDelegate = RestartInvoked;
                DefinePatchingMethods();

                Task.Run(Engine.Installer.Core.Installation.Install);
            }
        }

        private async Task<Engine.Installer.Core.Installation.InstallationResult> PrepPlatform()
        {
            //1: Was going to do some unregister service stuff but it defeats the anticheat. We will just return false if its not ready
            try
            {
                if (Directory.Exists(Engine.Installer.Core.Installation.InstallPath)) Directory.Delete(Engine.Installer.Core.Installation.InstallPath, true);
            }
            catch
            {
                return Installation.InstallationResult.Failure("Couldn't secure the installation directory");
            }

            return Installation.InstallationResult.Success;
        }

        private async System.Threading.Tasks.Task<Engine.Installer.Core.Installation.InstallationResult> RestartInvoked()
        {
            try
            {
                if (!File.Exists("Exec_Installer.bat"))
                {
                    //Get pub read zip and extract
                    using (var resource = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("WindowsInstaller.Exec_Installer.bat"))
                    {
                        using (var file = new FileStream("Exec_Installer.bat", FileMode.Create, FileAccess.Write))
                        {
                            resource.CopyTo(file);
                        }
                    }
                }
                System.Diagnostics.Process.Start("CMD.exe", "/c Exec_Installer.bat");
            } catch { }

            

            return Installation.InstallationResult.Success;
        }

        private void ReportProgress(string status, float progress, bool finished, bool failed)
        {
            StatusMessage.Visible = true;
            if(finished)
            {
                if (failed)
                    StatusMessage.Text = status; //TODO: Invoke a retry mechanism
                else
                    StatusMessage.Text = status; //TODO: Invoke a success mechanism

                MessageBox.Show("Installation Finished"); //TODO make this not jenky
                Application.Exit();
                return;
            }
            StatusMessage.Text = "(" + Math.Round(progress * 100, 1) + "%) " + status;
        }

        private void DefinePatchingMethods()
        {
            Engine.Installer.Core.Installation.PatchingMethods[Engine.Installer.Core.PatchDefinition.PatchKeys.Firefox] = Patches.patch_firefox.Patch;
            Engine.Installer.Core.Installation.PatchingMethods[Engine.Installer.Core.PatchDefinition.PatchKeys.ChocoRequest] = Patches.patch_choco.Patch;
            Engine.Installer.Core.Installation.PatchingMethods[Engine.Installer.Core.PatchDefinition.PatchKeys.WebRequest] = Patches.web_resource_patch.Patch;
            Engine.Installer.Core.Installation.PatchingMethods[Engine.Installer.Core.PatchDefinition.PatchKeys.RegPatch] = Patches.patch_reg.Patch;
            Engine.Installer.Core.Installation.PatchingMethods[Engine.Installer.Core.PatchDefinition.PatchKeys.UserPatch] = Patches.patch_userpatch.Patch;
            Engine.Installer.Core.Installation.PatchingMethods[Engine.Installer.Core.PatchDefinition.PatchKeys.CommandPatch] = Patches.patch_commandline.Patch;
            Engine.Installer.Core.Installation.PatchingMethods[Engine.Installer.Core.PatchDefinition.PatchKeys.SecEditPatch] = Patches.patch_secedit.Patch;
        }



        private async System.Threading.Tasks.Task<Engine.Installer.Core.Installation.InstallationResult> BuildWindows()
        {
            string MSBuildPath = GetMSBuildPath();
            
            if(MSBuildPath == null)
            {
                bool MSInstall = await Engine.Installer.Core.Installation.InstallMSBuild();
                if(!MSInstall)
                    return new Engine.Installer.Core.Installation.InstallationResult() { Message = "Failed to build the engine (msbuild failed to install e1)!", ErrorLevel = Engine.Installer.Core.Installation.InstallationResultStatus.Failed };
                Engine.Installer.Core.Installation.RestartInstaller(); //fresh install requires restart of the .net framework
                MSBuildPath = GetMSBuildPath();
            }

            if(MSBuildPath == null)
                return new Engine.Installer.Core.Installation.InstallationResult() { Message = "Failed to build the engine (msbuild failed to install e2)!", ErrorLevel = Engine.Installer.Core.Installation.InstallationResultStatus.Failed };

            Engine.Installer.Core.Installation.InstallationResult BuildResult = await Engine.Installer.Core.Installation.BuildWindowsEngine(MSBuildPath);

            if(BuildResult.ErrorLevel == Engine.Installer.Core.Installation.InstallationResultStatus.Failed)
                return BuildResult;

            return Engine.Installer.Core.Installation.InstallationResult.Success;
        }

        private async System.Threading.Tasks.Task<Engine.Installer.Core.Installation.InstallationResult> InstallWindows()
        {
            string WinSVLoc = Path.Combine(Engine.Installer.Core.Installation.EngineLocation, "WinCSSE.exe");
            string WinSVArgString = "create compscoring binpath= \"" + WinSVLoc + "\"" + " type= own start= auto error= critical displayname= compscoring";

            int result = await Extensions.StartProcess("sc.exe", WinSVArgString, null, null, Console.Out, Console.Error);
            if (result != 0)
                return Engine.Installer.Core.Installation.Error("Failed to configure the engine service");

            return Engine.Installer.Core.Installation.InstallationResult.Success;
        }

        private async System.Threading.Tasks.Task<Engine.Installer.Core.Installation.InstallationResult> SealWindows()
        {
            System.Security.AccessControl.DirectorySecurity EngineSecurity = Directory.GetAccessControl(Engine.Installer.Core.Installation.EngineLocation);
            //TODO Modify the directory security ACLs to not allow any editing
            Directory.SetAccessControl(Engine.Installer.Core.Installation.EngineLocation, EngineSecurity);
            return Engine.Installer.Core.Installation.InstallationResult.Success;
        }

        private string GetMSBuildPath()
        {
            if (System.IO.File.Exists(@"C:\Program Files (x86)\Microsoft Visual Studio\2017\BuildTools\MSBuild\15.0\Bin\MSBuild.exe"))
                return @"C:\Program Files (x86)\Microsoft Visual Studio\2017\BuildTools\MSBuild\15.0\Bin\MSBuild.exe";
            if (System.IO.File.Exists(@"C:\Program Files (x86)\Microsoft Visual Studio\2017\BuildTools\MSBuild\15.0\Bin\MSBuild.exe"))
                return @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Preview\MSBuild\Current\Bin\MSBuild.exe";
            if (System.IO.File.Exists(@"C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\MSBuild.exe"))
                return @"C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\MSBuild.exe";
            if (System.IO.File.Exists(@"C:\Program Files (x86)\Microsoft Visual Studio\2019\BuildTools\MSBuild\Current\Bin\MSBuild.exe"))
                return @"C:\Program Files (x86)\Microsoft Visual Studio\2019\BuildTools\MSBuild\Current\Bin\MSBuild.exe";
            return null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Linear float interpolation
        /// </summary>
        /// <param name="from">Starting Point</param>
        /// <param name="to">End Point</param>
        /// <param name="alpha">Alpha</param>
        /// <returns></returns>
        private float FloatLerp(float from, float to, float alpha)
        {
            return from * (1 - alpha) + to * alpha;
        }

        private void UniqueID_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
        }
#if DEBUG
        /// <summary>
        /// A function to test debug anything
        /// </summary>
        private void Debug()
        {
            if(File.Exists(@"debugchecks.xml"))
                Engine.Installer.Core.InstallationPackage.MakeDebugInstall(System.IO.File.ReadAllText(@"debugchecks.xml"));
        }
#endif
    }
}

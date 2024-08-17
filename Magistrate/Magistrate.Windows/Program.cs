using SimpleImpersonation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace Magistrate.Windows
{
    internal static class WINDOWS_CONST
    {
        private const string SQLUSERNAME = "MUSHROOM\\MAGISTRATE";
        private const string SQLPASSWORD = "_MagistrateScoring000";
        public static UserCredentials MAGISTRATECREDS = new UserCredentials(SQLUSERNAME, SQLPASSWORD);
    }
    public sealed class Program : ServiceBase
    {
        public Program()
        {
            this.AutoLog = false;
            this.CanStop = false;
            this.ServiceName = "Magistrate";
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
#if DEBUG
            Dev();
            Application.Run();
#else
            Run(new Program());
#endif
        }
        [STAThread]
        protected override void OnStart(string[] args)
        {
#if DEBUG
            //Prevents a debug service deployment (to ensure we dont make any.... mistakes...)
#else
            // Set Correct Working Directory
            Environment.CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            new Generated.Engine()
            {
                FixFWriteACL = FixWriteACL
            }.BeginScoring();
#endif
        }

        private void FixWriteACL(string file)
        {
            var SysSID = new SecurityIdentifier("S-1-5-18");
            var AdminsSID = new SecurityIdentifier("S-1-5-32-544");
            var WorldSID = new SecurityIdentifier("S-1-1-0");

            FileSecurity sec = new FileSecurity();
            sec.AddAccessRule(new FileSystemAccessRule(WorldSID, FileSystemRights.FullControl, AccessControlType.Allow));
            sec.AddAccessRule(new FileSystemAccessRule(SysSID, FileSystemRights.FullControl, AccessControlType.Allow));
            sec.AddAccessRule(new FileSystemAccessRule(AdminsSID, FileSystemRights.FullControl, AccessControlType.Allow));

            File.SetAccessControl(file, sec);
        }

        /// <summary>
        /// Executes when the service stops
        /// </summary>
        protected override void OnStop()
        {

        }

#if DEBUG
        private static void Dev()
        {
            Generated.Engine engine = new Generated.Engine();
            engine.BeginScoring();
        }
#endif
    }
}
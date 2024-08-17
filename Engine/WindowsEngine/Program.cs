using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using System.ServiceProcess;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;
using System.Reflection;
using System.Management;
using System.DirectoryServices;
using Engine.Core;

//https://social.technet.microsoft.com/Forums/ie/en-US/47c71ecd-e9e8-4e82-b2e2-daa6eb308ff2/prelogin-script-when-computer-turns-on?forum=winserver8gen
//HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon\AutoAdminLogin 
//HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon\DefaultUserName
//HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon\DefaultPassword
//https://stackoverflow.com/questions/33510244/subscription-to-windows-event-log
//https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.eventing.reader.eventlogwatcher?redirectedfrom=MSDN&view=netframework-4.8
//https://docs.microsoft.com/en-us/windows/security/threat-protection/auditing/event-4660
//TODO: subscribe to events for service removal, re-add and bluescreen
//TODO: tick 100ms, check clipboard for anything to do with the engine. if found, bluescreen

namespace WindowsEngine
{
    public sealed class Program : ServiceBase
    {
        private Engine.Core.Engine Win32Engine;
        private List<SSWinNotify.NotifyMain> Notifiers = new List<SSWinNotify.NotifyMain>();

        public Program()
        {
            InitializeComponent();
            Application.ApplicationExit += Application_ApplicationExit;
        }

        private void Application_ApplicationExit(object sender, EventArgs e)
        {
            ProcessProtection.Unprotect();
        }

        public static void Main(string[] args)
        {
            ProcessProtection.Protect();
            Run(new Program());
        }

        /// <summary>
        /// Start for a service
        /// </summary>
        /// <param name="args"></param>
        [STAThread]
        protected override void OnStart(string[] args)
        {
            //Skeleton code here.... Needs to actually handle interfaced logins TODO
            ProcessProtection.Protect();
            Environment.CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); //Set correct Working Directory
            Win32Engine = new Engine.Core.Engine() { ScoresUpdate = ScoresUpdate };
            Engine.Core.Scoring.StartEngine(Win32Engine);
            try { WriteSRPLNK(); } catch { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="UpdateType"></param>
        /// <param name="ScoringUpdateParams"></param>
        public void ScoresUpdate(string message, string title, Engine.Core.EngineFrame.ScoringUpdateType UpdateType, params object[] ScoringUpdateParams)
        {
            //Note: you cannot reference a winforms assembly directly here because winservice wont allow desktop interaction. That means you have to actually invoke a separate user space application
            try
            {
                string AudioName = "";
                if(ScoringUpdateParams != null && ScoringUpdateParams.Length > 1 )
                    AudioName = ScoringUpdateParams[1]?.ToString() ?? "";
                ProcessLib.Launch(Path.Combine(Environment.CurrentDirectory, "SSWinNotify.exe").Replace("\"", ""), 
                    concatargs("\"" + Path.Combine(Environment.CurrentDirectory, "SSWinNotify.exe") + "\"", "\"" + title + "\"", "\"" + message + "\"", 4.ToString(), 
                    @"""file:///" + Path.Combine(Win32Engine.ScoringReportIndex) + "\"", @"""" + AudioName + @""""));

            }
            catch (Exception e)
            {
                LogTemp(e.ToString());
            }
            WriteSRPLNK(); //dont need to try catch, this doesnt throw exceptions
        }

        internal static void LogTemp(string message)
        {
            try { File.AppendAllText(@"C:\NeedToFixThis.log", message + "\n"); } catch { }
        }

        private string concatargs(params string[] args)
        {
            string final = "";
            foreach (var a in args)
                final += a + " ";
            return final;
        }

        private ManagementBaseObject __cur_usr;

        private ManagementBaseObject CurrentUser
        {
            get
            {
                if(__cur_usr == null)
                {
                    try
                    {
                        ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT UserName FROM Win32_ComputerSystem");
                        ManagementObjectCollection collection = searcher.Get();
                        __cur_usr = collection.Cast<ManagementBaseObject>().First();
                    }
                    catch { }
                }
                return __cur_usr;
            }
        }
        private string GetCurrentUserName
        {
            get
            {
                if (CurrentUser == null)
                    return Environment.UserName;
                return (string) CurrentUser["UserName"];
            }
        }

        private void WriteSRPLNK() //dont let this throw exceptions. ever.
        {
            try
            {
                string LNKPATH = Path.Combine(ProcessLib.GetActiveDesktop(), "Scoring Report.lnk");

                if (!File.Exists(LNKPATH))
                {
                    //Get pub read zip and extract
                    using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream("WindowsEngine.ScoringReport.shortcut"))
                    {
                        using (var file = new FileStream(LNKPATH, FileMode.Create, FileAccess.Write))
                        {
                            resource.CopyTo(file);
                        }
                    }
                }
            }
            catch
            {
            }
        }


        private void Notifier_FormClosed(object sender, FormClosedEventArgs e)
        {
            Notifiers.Remove(sender as SSWinNotify.NotifyMain);
        }

        /// <summary>
        /// Executes when the service stops
        /// </summary>
        protected override void OnStop()
        {
            
        }

        private void InitializeComponent()
        {
            // 
            // Program
            // 
            this.AutoLog = false;
            this.CanStop = false;
            this.ServiceName = "CSSEngine";

        }

        /// <summary>
        /// Class responsible for exposing undocumented functionality making the host process unkillable.
        /// </summary>
        private static class ProcessProtection
        {
            [DllImport("ntdll.dll", SetLastError = true)]
            private static extern void RtlSetProcessIsCritical(UInt32 v1, UInt32 v2, UInt32 v3);

            /// <summary>
            /// Flag for maintaining the state of protection.
            /// </summary>
            private static volatile bool s_isProtected = false;

            /// <summary>
            /// For synchronizing our current state.
            /// </summary>
            private static ReaderWriterLockSlim s_isProtectedLock = new ReaderWriterLockSlim();

            /// <summary>
            /// Gets whether or not the host process is currently protected.
            /// </summary>
            internal static bool IsProtected
            {
                get
                {
                    try
                    {
                        s_isProtectedLock.EnterReadLock();

                        return s_isProtected;
                    }
                    finally
                    {
                        s_isProtectedLock.ExitReadLock();
                    }
                }
            }

            /// <summary>
            /// If not alreay protected, will make the host process a system-critical process so it
            /// cannot be terminated without causing a shutdown of the entire system.
            /// </summary>
            internal static void Protect()
            {
                try
                {
                    s_isProtectedLock.EnterWriteLock();

                    if (!s_isProtected)
                    {
                        System.Diagnostics.Process.EnterDebugMode();
                        RtlSetProcessIsCritical(1, 0, 0);
                        s_isProtected = true;
                    }
                }
                finally
                {
                    s_isProtectedLock.ExitWriteLock();
                }
            }

            /// <summary>
            /// If already protected, will remove protection from the host process, so that it will no
            /// longer be a system-critical process and thus will be able to shut down safely.
            /// </summary>
            internal static void Unprotect()
            {
                try
                {
                    s_isProtectedLock.EnterWriteLock();

                    if (s_isProtected)
                    {
                        RtlSetProcessIsCritical(0, 0, 0);
                        s_isProtected = false;
                    }
                }
                finally
                {
                    s_isProtectedLock.ExitWriteLock();
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct PROCESS_INFORMATION
    {
        public IntPtr hProcess;
        public IntPtr hThread;
        public uint dwProcessId;
        public uint dwThreadId;
    }



    [StructLayout(LayoutKind.Sequential)]
    internal struct SECURITY_ATTRIBUTES
    {
        public uint nLength;
        public IntPtr lpSecurityDescriptor;
        public bool bInheritHandle;
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct STARTUPINFO
    {
        public uint cb;
        public string lpReserved;
        public string lpDesktop;
        public string lpTitle;
        public uint dwX;
        public uint dwY;
        public uint dwXSize;
        public uint dwYSize;
        public uint dwXCountChars;
        public uint dwYCountChars;
        public uint dwFillAttribute;
        public uint dwFlags;
        public short wShowWindow;
        public short cbReserved2;
        public IntPtr lpReserved2;
        public IntPtr hStdInput;
        public IntPtr hStdOutput;
        public IntPtr hStdError;

    }

    internal enum SECURITY_IMPERSONATION_LEVEL
    {
        SecurityAnonymous,
        SecurityIdentification,
        SecurityImpersonation,
        SecurityDelegation
    }

    internal enum TOKEN_TYPE
    {
        TokenPrimary = 1,
        TokenImpersonation
    }

    internal class ProcessLib
    {
        /*
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool CreateProcessAsUser(
            IntPtr hToken,
            string lpApplicationName,
            string lpCommandLine,
            ref SECURITY_ATTRIBUTES lpProcessAttributes,
            ref SECURITY_ATTRIBUTES lpThreadAttributes,
            bool bInheritHandles,
            uint dwCreationFlags,
            IntPtr lpEnvironment,
            string lpCurrentDirectory,
            ref STARTUPINFO lpStartupInfo,
            out PROCESS_INFORMATION lpProcessInformation);
            */
        [DllImport("advapi32.dll", EntryPoint = "CreateProcessAsUserA", SetLastError = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        internal extern static bool CreateProcessAsUser(IntPtr hToken, [MarshalAs(UnmanagedType.LPStr)] string lpApplicationName,
                                            [MarshalAs(UnmanagedType.LPStr)] string lpCommandLine, IntPtr lpProcessAttributes,
                                            IntPtr lpThreadAttributes, bool bInheritHandle, uint dwCreationFlags, IntPtr lpEnvironment,
                                            [MarshalAs(UnmanagedType.LPStr)] string lpCurrentDirectory, ref STARTUPINFO lpStartupInfo,
                                            out PROCESS_INFORMATION lpProcessInformation);

        public static readonly Guid Desktop = new Guid("B4BFCC3A-DB2C-424C-B029-7FE99A87C641");

        [DllImport("shell32.dll")]
        static extern int SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, IntPtr hToken, out IntPtr pszPath);


        [DllImport("advapi32.dll", EntryPoint = "DuplicateTokenEx", SetLastError = true)]
        private static extern bool DuplicateTokenEx(
            IntPtr hExistingToken,
            uint dwDesiredAccess,
            ref SECURITY_ATTRIBUTES lpThreadAttributes,
            Int32 ImpersonationLevel,
            Int32 dwTokenType,
            ref IntPtr phNewToken);


        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool OpenProcessToken(
            IntPtr ProcessHandle,
            UInt32 DesiredAccess,
            ref IntPtr TokenHandle);

        [DllImport("userenv.dll", SetLastError = true)]
        private static extern bool CreateEnvironmentBlock(
                ref IntPtr lpEnvironment,
                IntPtr hToken,
                bool bInherit);


        [DllImport("userenv.dll", SetLastError = true)]
        private static extern bool DestroyEnvironmentBlock(
                IntPtr lpEnvironment);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(
            IntPtr hObject);

        private const short SW_SHOW = 5;
        private const uint TOKEN_QUERY = 0x0008;
        private const uint TOKEN_DUPLICATE = 0x0002;
        private const uint TOKEN_ASSIGN_PRIMARY = 0x0001;
        private const int GENERIC_ALL_ACCESS = 0x10000000;
        private const int STARTF_USESHOWWINDOW = 0x00000001;
        private const int STARTF_FORCEONFEEDBACK = 0x00000040;
        private const uint CREATE_UNICODE_ENVIRONMENT = 0x00000400;


        private static bool LaunchProcessAsUser(string AppName, string cmdLine, IntPtr token, IntPtr envBlock)
        {
            bool result = false;
            PROCESS_INFORMATION pi = new PROCESS_INFORMATION();
            SECURITY_ATTRIBUTES saProcess = new SECURITY_ATTRIBUTES();
            SECURITY_ATTRIBUTES saThread = new SECURITY_ATTRIBUTES();
            saProcess.nLength = (uint)Marshal.SizeOf(saProcess);
            saThread.nLength = (uint)Marshal.SizeOf(saThread);

            STARTUPINFO si = new STARTUPINFO();
            si.cb = (uint)Marshal.SizeOf(si);


            //if this member is NULL, the new process inherits the desktop
            //and window station of its parent process. If this member is
            //an empty string, the process does not inherit the desktop and
            //window station of its parent process; instead, the system
            //determines if a new desktop and window station need to be created.
            //If the impersonated user already has a desktop, the system uses the
            //existing desktop.

            si.lpDesktop = @"WinSta0\Default"; //Modify as needed
            si.dwFlags = STARTF_USESHOWWINDOW | STARTF_FORCEONFEEDBACK;
            si.wShowWindow = SW_SHOW;
            //Set other si properties as required.

            result = CreateProcessAsUser(
                token,
                AppName,
                cmdLine,
                IntPtr.Zero,
                IntPtr.Zero,
                false,
                CREATE_UNICODE_ENVIRONMENT,
                envBlock,
                Environment.CurrentDirectory,
                ref si,
                out pi);


            if (result == false)
            {
                int error = Marshal.GetLastWin32Error();
                string message = string.Format("CreateProcessAsUser Error: {0}", error);
                Program.LogTemp(message);
            }

            return result;
        }


        private static IntPtr GetPrimaryToken(int processId)
        {
            IntPtr token = IntPtr.Zero;
            IntPtr primaryToken = IntPtr.Zero;
            bool retVal = false;
            Process p = null;

            try
            {
                p = Process.GetProcessById(processId);
            }

            catch (ArgumentException)
            {

                string details = String.Format("ProcessID {0} Not Available", processId);
                Program.LogTemp(details);
                throw;
            }


            //Gets impersonation token
            retVal = OpenProcessToken(p.Handle, TOKEN_DUPLICATE, ref token);
            if (retVal == true)
            {

                SECURITY_ATTRIBUTES sa = new SECURITY_ATTRIBUTES();
                sa.nLength = (uint)Marshal.SizeOf(sa);

                //Convert the impersonation token into Primary token
                retVal = DuplicateTokenEx(
                    token,
                    TOKEN_ASSIGN_PRIMARY | TOKEN_DUPLICATE | TOKEN_QUERY,
                    ref sa,
                    (int)SECURITY_IMPERSONATION_LEVEL.SecurityIdentification,
                    (int)TOKEN_TYPE.TokenPrimary,
                    ref primaryToken);

                //Close the Token that was previously opened.
                CloseHandle(token);
                if (retVal == false)
                {
                    string message = String.Format("DuplicateTokenEx Error: {0}", Marshal.GetLastWin32Error());
                    Program.LogTemp(message);
                }

            }

            else
            {

                string message = String.Format("OpenProcessToken Error: {0}", Marshal.GetLastWin32Error());
                Program.LogTemp(message);

            }

            //We'll Close this token after it is used.
            return primaryToken;

        }

        private static IntPtr GetEnvironmentBlock(IntPtr token)
        {

            IntPtr envBlock = IntPtr.Zero;
            bool retVal = CreateEnvironmentBlock(ref envBlock, token, false);
            if (retVal == false)
            {

                //Environment Block, things like common paths to My Documents etc.
                //Will not be created if "false"
                //It should not adversley affect CreateProcessAsUser.

                string message = String.Format("CreateEnvironmentBlock Error: {0}", Marshal.GetLastWin32Error());
                Debug.WriteLine(message);

            }
            return envBlock;
        }

        public static bool Launch(string AppName, string appCmdLine /*,int processId*/)
        {

            bool ret = false;

            Process[] ps = Process.GetProcessesByName("explorer");
            int processId = -1;//=processId
            if (ps.Length > 0)
            {
                processId = GetRealExplorer(ps)?.Id ?? -1;
            }

            if (processId > 1)
            {
                IntPtr token = GetPrimaryToken(processId);

                if (token != IntPtr.Zero)
                {

                    IntPtr envBlock = GetEnvironmentBlock(token);
                    ret = LaunchProcessAsUser(AppName, appCmdLine, token, envBlock);
                    if (envBlock != IntPtr.Zero)
                        try { DestroyEnvironmentBlock(envBlock); } catch { }

                    try { CloseHandle(token); } catch { }
                }
            }
            return ret;
        }

        public static string GetActiveDesktop()
        {
            string result = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); //useless
            Process[] ps = Process.GetProcessesByName("explorer");
            int processId = -1;//=processId

            if (ps.Length > 0)
            {
                processId = GetRealExplorer(ps)?.Id ?? -1;
            }

            if (processId > 1)
            {
                IntPtr token = GetPrimaryToken(processId);

                if (token != IntPtr.Zero)
                {
                    if (SHGetKnownFolderPath(Desktop, 0, token, out IntPtr pPath) == 0)
                    {
                        result = Marshal.PtrToStringUni(pPath);
                    }

                    Marshal.FreeCoTaskMem(pPath);
                    CloseHandle(token);
                }
            }

            return result;
        }

        private static Process GetRealExplorer(Process[] potential)
        {
            foreach (var p in potential)
            {
                if (_p(GetMainModuleFilepath(p.Id)).Equals(_p(@"C:\Windows\explorer.exe"), 
                    StringComparison.OrdinalIgnoreCase))
                    return p;
            }
            return null;
        }

        private static string GetMainModuleFilepath(int processId)
        {
            string wmiQueryString = "SELECT ProcessId, ExecutablePath FROM Win32_Process WHERE ProcessId = " + processId;
            using (var searcher = new ManagementObjectSearcher(wmiQueryString))
            {
                using (var results = searcher.Get())
                {
                    ManagementObject mo = results.Cast<ManagementObject>().FirstOrDefault();
                    if (mo != null)
                    {
                        return (string)mo["ExecutablePath"];
                    }
                }
            }
            return null;
        }

        private static string _p(string path)
        {
            return Path.GetFullPath(new Uri(path).LocalPath)
                       .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                       .ToUpperInvariant();
        }

    }
}

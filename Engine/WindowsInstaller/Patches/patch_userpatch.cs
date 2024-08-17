using Engine.Installer.Core;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//https://gist.github.com/crshnbrn66/7e81bf20408c05ddb2b4fdf4498477d8

namespace WindowsInstaller.Patches
{
    /*
     * Currently only supports DWORD and STRING
     * string UserName|SID
     * bIsSID
     * UserActions Action
     * object ExtraArgs[]
     */
    internal static class patch_userpatch
    {
        private enum UserActions
        {
            /// <summary>
            /// Create a new user. If the user is the primary user, we will simply rename the active account.
            /// </summary>
            Create, //bPrimaryAccount, string Password, csv-string DefaultGroups
            SetLocked, //bActive

        }
        /// <summary>
        /// Patch a registry key to a default value
        /// </summary>
        /// <param name="patch"></param>
        /// <returns></returns>
        internal static async Task<Installation.InstallationResult> Patch(PatchDefinition patch)
        {
            if (patch.NumArgs < 3)
                return Installation.InstallationResult.Failure("Failed to patch a user account because the patch was malformed " + patch.PatchKey);

            UserActions target = UserActions.Create;
            bool IsSID = false;
            string UserName = "";
            try
            {
                Enum.TryParse(patch.Args[2], true, out target);
                IsSID = patch.Args[1].ToLower().Trim() == "true";
                UserName = patch.Args[0].Replace("\"", "");
                if (IsSID)
                    return Installation.InstallationResult.Failure("Failed to patch a user account because SID support is not implemented yet " + patch.PatchKey);
            }
            catch
            {
                return Installation.InstallationResult.Failure("Failed to patch a user account because the patch data was malformed " + patch.PatchKey);
            }

            Installation.InstallationResult result = Installation.InstallationResult.Success;

            try
            {
                switch (target)
                {
                    case UserActions.Create:
                        if (patch.NumArgs < 5)
                            return Installation.InstallationResult.Failure("Failed to patch a user account because the patch extra arguments did not contain a required parameter " + patch.PatchKey);
                        bool bIsPrimary = patch.Args[3].ToLower().Trim() == "true";
                        string pw = patch.Args[4].Trim();
                        result = await CreateUser(
                            UserName,
                            bIsPrimary,
                            pw,
                            patch.NumArgs > 5 ? patch.Args[5] : null
                            );
                        break;
                    case UserActions.SetLocked:

                        if (patch.NumArgs < 4)
                            return Installation.InstallationResult.Failure("Failed to patch a user account because the patch extra arguments did not contain a required parameter " + patch.PatchKey);
                        bool ShouldLock = patch.Args[3].ToLower().Trim() == "true";
                        result = await SetLocked(UserName, ShouldLock);
                        break;
                }
            }
            catch
            {
                return Installation.InstallationResult.Failure("Failed to patch a user account because an exception was encountered while performing an action " + patch.PatchKey);
            }

            return result;
        }

        [DllImport("userenv.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int CreateProfile(
                      [MarshalAs(UnmanagedType.LPWStr)] string pszUserSid,
                      [MarshalAs(UnmanagedType.LPWStr)] string pszUserName,
                      [Out][MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszProfilePath,
                      uint cchProfilePath);

        private static async Task<Installation.InstallationResult> CreateUser(string UserName, bool IsPrimary, string Password, string DefaultGroups)
        {
            string[] groups = DefaultGroups?.Split(',') ?? new string[0];

            
            bool profileResult = true;

            await Extensions.StartProcess("cmd", "/c net user \"" + UserName + "\" \"" + Password + "\" /add /Y", Environment.CurrentDirectory, null, Console.Out, Console.Error);

            if (IsPrimary)
            {
                //Autologin
                await patch_reg.PatchFromArgs("HKEY_LOCAL_MACHINE", @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "String", "DefaultUserName", UserName, "64");
                await patch_reg.PatchFromArgs("HKEY_LOCAL_MACHINE", @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "String", "DefaultPassword", Password, "64");
                await patch_reg.PatchFromArgs("HKEY_LOCAL_MACHINE", @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "DWORD", "AutoAdminLogon", "1", "64");

                //rename and set password
                //await Extensions.StartProcess("cmd", "/c wmic useraccount where name='" + Environment.UserName + "' rename "  + UserName, Environment.CurrentDirectory, null, Console.Out, Console.Error);
                //await Extensions.StartProcess("cmd", "/c net user \"" + UserName + "\" \"" + Password + "\" /Y", Environment.CurrentDirectory, null, Console.Out, Console.Error);

                //await patch_reg.PatchFromArgs("HKEY_LOCAL_MACHINE", @"Software\Microsoft\Windows\CurrentVersion\RunOnce", "String", "csse_envclean", @"C:\env-cleaner.bat", "64");
                //File.WriteAllText(@"C:\env-cleaner.bat", @"rd /S /Q """ + OldUPath + @"""");
            }

            try
            {
                NTAccount acct = new NTAccount(UserName);
                SecurityIdentifier si = (SecurityIdentifier)acct.Translate(typeof(SecurityIdentifier));
                string SID = "";

                SID = si.ToString();
                int ret = 0;
                uint buf = 260;
                StringBuilder profpath = new StringBuilder((int)buf);
                ret = CreateProfile(SID, UserName, profpath, buf);

                if (ret != 0)
                    profileResult = false;

                /*
                if (OldUPath.Trim().ToLower() != profpath.ToString().Trim().ToLower()) //redirect current profile and destroy old path on logout
                {
                    var dest = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)
                        .CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\ProfileList\" + SID, true);
                    dest.SetValue("ProfileImagePath", profpath, RegistryValueKind.ExpandString);
                }
                */
            }
            catch
            {
                profileResult = false;
            }
            /*
            try
            {
                    
                var SysSID = new SecurityIdentifier("S-1-5-18");
                var AdminsSID = new SecurityIdentifier("S-1-5-32-544");
                string UserProfilePath = @"C:\Users\" + UserName;

                    //CopyRegTo
                    var src = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)
                    .OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\ProfileList\" + UserPrincipal.Current.Sid.ToString());
                var dest = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)
                    .OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\ProfileList\" + SID, true);

                src.CopyRegTo(dest);

                dest.SetValue("ProfileImagePath", UserProfilePath, RegistryValueKind.ExpandString);
                dest.SetValue("Sid", SIDStringToBytes(SID), RegistryValueKind.Binary);

                if(!IsPrimary)
                    DirectoryCopy(@"C:\Users\Default", UserProfilePath, true);
                else
                    DirectoryCopy(OldUPath, UserProfilePath, true);

                DirectorySecurity dis = new DirectorySecurity();
                FileSystemAccessRule usr = new FileSystemAccessRule(newUsr, FileSystemRights.FullControl, AccessControlType.Allow);
                FileSystemAccessRule sys = new FileSystemAccessRule(SysSID, FileSystemRights.FullControl, AccessControlType.Allow);
                FileSystemAccessRule adm = new FileSystemAccessRule(AdminsSID, FileSystemRights.FullControl, AccessControlType.Allow);
                dis.AddAccessRule(usr);
                dis.AddAccessRule(sys);
                dis.AddAccessRule(adm);
                DirectoryInfo d = new DirectoryInfo(UserProfilePath);
                d.SetAccessControl(dis);
            }
            catch(Exception e)
            {
                return Installation.InstallationResult.Failure("User patch failed... (" + UserName + ")" + "\n" + e.ToString());
            }
            */

            foreach (var group in groups)
            {
                await Extensions.StartProcess("cmd", "/c net localgroup \"" + group + "\" /add /Y", Environment.CurrentDirectory, null, Console.Out, Console.Error); //will error if it already exists, which is fine
                await Extensions.StartProcess("cmd", "/c net localgroup \"" + group + "\" \"" + UserName + "\" /add /Y", Environment.CurrentDirectory, null, Console.Out, Console.Error); //add user to group
            }

            if(profileResult)
                return Installation.InstallationResult.Success;
            else
                return Installation.InstallationResult.Failure("User patch failed... (" + UserName + ")");
        }

        private static async Task<Installation.InstallationResult> SetLocked(string UserName, bool ShouldLock)
        {
            await Extensions.StartProcess("cmd", "/c wmic useraccount where name='" + UserName + "' set disabled=" + ShouldLock.ToString(), Environment.CurrentDirectory, null, Console.Out, Console.Error);

            return Installation.InstallationResult.Success;
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

//TODO: User accounts is really fragile right now in the sense that if a user is renamed and the engine is restarted, we will run into an issue where the user technically doesnt exist.
//      The fix is to, on engine generation, record the SID of the user account in the check. This will prevent rename scenarios from affecting the image.


    class UserAccountsTemplate : CheckTemplate
    {
        
        private readonly UA_CheckType CType;
        private enum UA_CheckType
        {
            /// <summary>
            /// Check for this user in the users list. Requires string UserName
            /// </summary>
            Exists,//done
            /// <summary>
            /// Is this user enabled. Requires string UserName. Will fail if the user is deleted.
            /// </summary>
            Enabled,//done
            /// <summary>
            /// Can this user's password expire. Requires string UserName. Will fail if the user is deleted.
            /// </summary>
            PasswordCanExpire,//done
            /// <summary>
            /// Will this user's password expire on login. Requires string UserName. Will fail if the user is deleted.
            /// </summary>
            PasswordWillExpire,//done
            /// <summary>
            /// Does this user's account store its password using reversable encryption. Requires string UserName. Will fail if the user is deleted.
            /// </summary>
            ReversableEncryptionAllowed,//done
            /// <summary>
            /// Can this user change its account password. Requires string UserName. Will fail if the user is deleted.
            /// </summary>
            PasswordCanChange,//done
            /// <summary>
            /// Will this user account expire (or has expired already). Requires string UserName. Will fail if the user is deleted.
            /// </summary>
            AccountWillExpire,//todo
            /// <summary>
            /// Is this user a local administrator. Requires string UserName. Will fail if the user is deleted.
            /// </summary>
            IsAdministrator,//done
            /// <summary>
            /// Is this account locked. Requires string UserName. Will fail if the user is deleted.
            /// </summary>
            AccountLocked,//done
            /// <summary>
            /// Is this account part of a specific local group. Requires string UserName, string GroupName. Will fail if the user is deleted.
            /// </summary>
            PartOfGroup//done
        }

        private DirectoryEntry user;
        /// <summary>
        /// The user principle we are evaluating
        /// </summary>
        protected DirectoryEntry User
        {
            get
            {
                if (user == null)
                    try
                    {
                        if (ComputerEntry == null)
                            return null;

                        foreach (DirectoryEntry childEntry in ComputerEntry.Children)
                            if (childEntry.SchemaClassName == "User" && childEntry.Name.ToLower().Trim() == ((string)UserName).ToLower().Trim())
                                user = childEntry;
                    }
                    catch { }

                TickDelay = user == null ? 15000u : 5000u;

                return user;
            }
        }

    internal override SafeString CompletedMessage 
    {
        get
        {
            return UserName + " account check passed.";
        }
    }

    internal override SafeString FailedMessage
    {
        get
        {
            return UserName + " account check failed.";
        }
    }

    private readonly string ComputerPath = string.Format("WinNT://{0},computer", Environment.MachineName);
        private readonly DirectoryEntry ComputerEntry;
        private readonly SafeString UserName;
        private readonly SafeString GroupName;
        private bool DisableOnDelete = false;

        internal override Task<byte[]> GetCheckValue()
        {
            try
            {
                user = null;
                switch (CType)
                {
                    case UA_CheckType.Exists:
                        return Task.FromResult(PrepareState32(User != null));

                    case UA_CheckType.Enabled:
                        return Task.FromResult(PrepareState32(!UserFlagSet(UserFlags.ADS_UF_ACCOUNTDISABLE)));

                    case UA_CheckType.PasswordCanExpire:
                        return Task.FromResult(PrepareState32(!UserFlagSet(UserFlags.ADS_UF_DONT_EXPIRE_PASSWD)));

                    case UA_CheckType.PasswordWillExpire:
                        return Task.FromResult(PrepareState32(((DateTime)User.InvokeGet("PasswordExpirationDate") - DateTime.Today).TotalMilliseconds <= 0));

                    case UA_CheckType.ReversableEncryptionAllowed:
                        return Task.FromResult(PrepareState32(UserFlagSet(UserFlags.ADS_UF_ENCRYPTED_TEXT_PASSWORD_ALLOWED)));

                    case UA_CheckType.PasswordCanChange:
                        return Task.FromResult(PrepareState32(!UserFlagSet(UserFlags.ADS_UF_PASSWD_CANT_CHANGE)));

                    case UA_CheckType.AccountWillExpire:
                        Enabled = false;
                        break; //Not implemented yet.

                    case UA_CheckType.IsAdministrator:
                        return Task.FromResult(PrepareState32(HasGroup("administrators")));

                    case UA_CheckType.AccountLocked:
                        return Task.FromResult(PrepareState32(UserFlagSet(UserFlags.ADS_UF_LOCKOUT)));

                    case UA_CheckType.PartOfGroup:
                        return Task.FromResult(PrepareState32(HasGroup(GroupName)));

                }
            }
            catch(ArgumentNullException)
            {
                if (DisableOnDelete)
                {
                    Enabled = false;
                }
            }
            catch(ArgumentOutOfRangeException)
            {
            }

            return Task.FromResult(new byte[0]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args">0: UA_CheckType,1:FailOnDeleted, 2+: Check args</param>
        internal UserAccountsTemplate(params string[] args)
        {
            if(args.Length < 3)
            {
                Enabled = false;
                return;
            }

            try
            {
                Enum.TryParse(args[0], true, out CType);
                ComputerEntry = new DirectoryEntry(ComputerPath);
            }
            catch
            {
                Enabled = false;
                return;
            }

            UserName = args[1];

            try
            {
                DisableOnDelete = Convert.ToBoolean(args[2]);
            }
            catch (FormatException) { }
            
            if(User == null && DisableOnDelete)
            {
                Enabled = false;
                return;
            }

            switch(CType)
            {
                case UA_CheckType.PartOfGroup:
                    if (args.Length > 2)
                        GroupName = args[3];
                    else
                        Enabled = false;
                    break;
            }
        }

        /// <summary>
        /// All possible flags for a win32 user account
        /// </summary>
        private enum UserFlags : int
        {
            /// <summary>
            /// Uses script
            /// </summary>
            ADS_UF_SCRIPT = 1,
            /// <summary>
            /// Is the account disabled
            /// </summary>
            ADS_UF_ACCOUNTDISABLE = 2,
            /// <summary>
            /// Does this user require a home directory
            /// </summary>
            ADS_UF_HOMEDIR_REQUIRED = 8,
            /// <summary>
            /// Is this account locked out
            /// </summary>
            ADS_UF_LOCKOUT = 16,
            /// <summary>
            /// Does this account require a password
            /// </summary>
            ADS_UF_PASSWD_NOTREQD = 32,
            /// <summary>
            /// Can the user change their password
            /// </summary>
            ADS_UF_PASSWD_CANT_CHANGE = 64,
            /// <summary>
            /// Is reversible encryption allowed
            /// </summary>
            ADS_UF_ENCRYPTED_TEXT_PASSWORD_ALLOWED = 128,
            /// <summary>
            /// Is this a temporary duplicate account
            /// </summary>
            ADS_UF_TEMP_DUPLICATE_ACCOUNT = 256,
            /// <summary>
            /// Is this a normal account
            /// </summary>
            ADS_UF_NORMAL_ACCOUNT = 512,
            /// <summary>
            /// Is this an interdomain trust account
            /// </summary>
            ADS_UF_INTERDOMAIN_TRUST_ACCOUNT = 2048,
            /// <summary>
            /// Is this a workstation trust account
            /// </summary>
            ADS_UF_WORKSTATION_TRUST_ACCOUNT = 4096,
            /// <summary>
            /// Is this a server trust account
            /// </summary>
            ADS_UF_SERVER_TRUST_ACCOUNT = 8192,
            /// <summary>
            /// Does this user's password expire
            /// </summary>
            ADS_UF_DONT_EXPIRE_PASSWD = 65536,
            /// <summary>
            /// Is this a MNS account
            /// </summary>
            ADS_UF_MNS_LOGON_ACCOUNT = 131072,
            /// <summary>
            /// Does this account require a smartcard
            /// </summary>
            ADS_UF_SMARTCARD_REQUIRED = 262144,
            /// <summary>
            /// Is this account trusted for delegation
            /// </summary>
            ADS_UF_TRUSTED_FOR_DELEGATION = 524288,
            /// <summary>
            /// Is this account not delegated
            /// </summary>
            ADS_UF_NOT_DELEGATED = 1048576,
            /// <summary>
            /// Does this account only use a DES key
            /// </summary>
            ADS_UF_USE_DES_KEY_ONLY = 2097152,
            /// <summary>
            /// Does this account not require preauth
            /// </summary>
            ADS_UF_DONT_REQUIRE_PREAUTH = 4194304,
            /// <summary>
            /// Has this account's password expired
            /// </summary>
            ADS_UF_PASSWORD_EXPIRED = 8388608,
            /// <summary>
            /// Do we trust this account for delegation
            /// </summary>
            ADS_UF_TRUSTED_TO_AUTHENTICATE_FOR_DELEGATION = 16777216,
        }

    /// <summary>
    /// Returns true if the user flag is set
    /// </summary>
    /// <param name="flag"></param>
    /// <exception cref="ArgumentNullException">If the user doesnt exist</exception>
    /// /// <exception cref="ArgumentOutOfRangeException">If the user flags are busted</exception>
    /// <returns></returns>
    private bool UserFlagSet(UserFlags flag)
    {
        if (User == null)
            throw new ArgumentNullException();
        if (User.Properties["UserFlags"] == null || User.Properties["UserFlags"].Value == null)
            throw new ArgumentOutOfRangeException();

        int value = (int)(User.Properties["UserFlags"].Value);

        return (value & (int)flag) != 0;
    }

    /// <summary>
    /// Does the current user have this group assigned to them
    /// </summary>
    /// <param name="groupname"></param>
    /// <exception cref="NullReferenceException">Thrown if the user is invalid</exception>
    /// <returns></returns>
    protected bool HasGroup(string groupname)
    {
        if (User == null)
            throw new ArgumentException(); 
        object obGroups = User.Invoke("Groups");
        bool found = false;
        foreach (object ob in (System.Collections.IEnumerable)obGroups)
        {
            DirectoryEntry obGpEntry = new DirectoryEntry(ob);
            if (obGpEntry.Name.ToLower() == groupname.ToLower())
            {
                obGpEntry.Dispose();
                found = true;
                break;
            }
            obGpEntry.Dispose();
        }
        return found;
    }

        /*
        private sealed class SamServer : IDisposable
        {
            private IntPtr _handle;

            internal SamServer(string name, SERVER_ACCESS_MASK access)
            {
                Name = name;
                Check(SamConnect(new UNICODE_STRING(name), out _handle, access, IntPtr.Zero));
            }

            internal string Name { get; }

            public void Dispose()
            {
                if (_handle != IntPtr.Zero)
                {
                    SamCloseHandle(_handle);
                    _handle = IntPtr.Zero;
                }
            }

            internal void SetDomainPasswordInformation(SecurityIdentifier domainSid, DOMAIN_PASSWORD_INFORMATION passwordInformation)
            {
                if (domainSid == null)
                    throw new ArgumentNullException(nameof(domainSid));

                var sid = new byte[domainSid.BinaryLength];
                domainSid.GetBinaryForm(sid, 0);

                Check(SamOpenDomain(_handle, DOMAIN_ACCESS_MASK.DOMAIN_WRITE_PASSWORD_PARAMS, sid, out IntPtr domain));
                IntPtr info = Marshal.AllocHGlobal(Marshal.SizeOf(passwordInformation));
                Marshal.StructureToPtr(passwordInformation, info, false);
                try
                {
                    Check(SamSetInformationDomain(domain, DOMAIN_INFORMATION_CLASS.DomainPasswordInformation, info));
                }
                finally
                {
                    Marshal.FreeHGlobal(info);
                    SamCloseHandle(domain);
                }
            }

            internal DOMAIN_PASSWORD_INFORMATION GetDomainPasswordInformation(SecurityIdentifier domainSid)
            {
                if (domainSid == null)
                    throw new ArgumentNullException(nameof(domainSid));

                var sid = new byte[domainSid.BinaryLength];
                domainSid.GetBinaryForm(sid, 0);

                Check(SamOpenDomain(_handle, DOMAIN_ACCESS_MASK.DOMAIN_READ_PASSWORD_PARAMETERS, sid, out IntPtr domain));
                var info = IntPtr.Zero;
                try
                {
                    Check(SamQueryInformationDomain(domain, DOMAIN_INFORMATION_CLASS.DomainPasswordInformation, out info));
                    return (DOMAIN_PASSWORD_INFORMATION)Marshal.PtrToStructure(info, typeof(DOMAIN_PASSWORD_INFORMATION));
                }
                finally
                {
                    SamFreeMemory(info);
                    SamCloseHandle(domain);
                }
            }

            internal SecurityIdentifier GetDomainSid(string domain)
            {
                if (domain == null)
                    throw new ArgumentNullException(nameof(domain));

                Check(SamLookupDomainInSamServer(_handle, new UNICODE_STRING(domain), out IntPtr sid));
                return new SecurityIdentifier(sid);
            }

            internal IEnumerable<string> EnumerateDomains()
            {
                int cookie = 0;
                while (true)
                {
                    var status = SamEnumerateDomainsInSamServer(_handle, ref cookie, out IntPtr info, 1, out int count);
                    if (status != NTSTATUS.STATUS_SUCCESS && status != NTSTATUS.STATUS_MORE_ENTRIES)
                        Check(status);

                    if (count == 0)
                        break;

                    var us = (UNICODE_STRING)Marshal.PtrToStructure(info + IntPtr.Size, typeof(UNICODE_STRING));
                    SamFreeMemory(info);
                    yield return us.ToString();
                    us.Buffer = IntPtr.Zero; // we don't own this one
                }
            }

            private enum DOMAIN_INFORMATION_CLASS
            {
                DomainPasswordInformation = 1,
            }
            //https://docs.microsoft.com/en-us/windows/desktop/api/ntsecapi/ns-ntsecapi-_domain_password_information
            [Flags]
            internal enum PASSWORD_PROPERTIES
            {
                DOMAIN_PASSWORD_COMPLEX = 0x00000001,
                DOMAIN_PASSWORD_NO_ANON_CHANGE = 0x00000002,
                DOMAIN_PASSWORD_NO_CLEAR_CHANGE = 0x00000004,
                DOMAIN_LOCKOUT_ADMINS = 0x00000008,
                DOMAIN_PASSWORD_STORE_CLEARTEXT = 0x00000010,
                DOMAIN_REFUSE_PASSWORD_CHANGE = 0x00000020,
            }

            [Flags]
            private enum DOMAIN_ACCESS_MASK
            {
                DOMAIN_READ_PASSWORD_PARAMETERS = 0x00000001,
                DOMAIN_WRITE_PASSWORD_PARAMS = 0x00000002,
                DOMAIN_READ_OTHER_PARAMETERS = 0x00000004,
                DOMAIN_WRITE_OTHER_PARAMETERS = 0x00000008,
                DOMAIN_CREATE_USER = 0x00000010,
                DOMAIN_CREATE_GROUP = 0x00000020,
                DOMAIN_CREATE_ALIAS = 0x00000040,
                DOMAIN_GET_ALIAS_MEMBERSHIP = 0x00000080,
                DOMAIN_LIST_ACCOUNTS = 0x00000100,
                DOMAIN_LOOKUP = 0x00000200,
                DOMAIN_ADMINISTER_SERVER = 0x00000400,
                DOMAIN_ALL_ACCESS = 0x000F07FF,
                DOMAIN_READ = 0x00020084,
                DOMAIN_WRITE = 0x0002047A,
                DOMAIN_EXECUTE = 0x00020301
            }

            [Flags]
            internal enum SERVER_ACCESS_MASK
            {
                SAM_SERVER_CONNECT = 0x00000001,
                SAM_SERVER_SHUTDOWN = 0x00000002,
                SAM_SERVER_INITIALIZE = 0x00000004,
                SAM_SERVER_CREATE_DOMAIN = 0x00000008,
                SAM_SERVER_ENUMERATE_DOMAINS = 0x00000010,
                SAM_SERVER_LOOKUP_DOMAIN = 0x00000020,
                SAM_SERVER_ALL_ACCESS = 0x000F003F,
                SAM_SERVER_READ = 0x00020010,
                SAM_SERVER_WRITE = 0x0002000E,
                SAM_SERVER_EXECUTE = 0x00020021
            }

            [StructLayout(LayoutKind.Sequential)]
            internal struct DOMAIN_PASSWORD_INFORMATION
            {
                internal short MinPasswordLength;
                internal short PasswordHistoryLength;
                internal PASSWORD_PROPERTIES PasswordProperties;
                private long _maxPasswordAge;
                private long _minPasswordAge;

                internal TimeSpan MaxPasswordAge
                {
                    get
                    {
                        return -new TimeSpan(_maxPasswordAge);
                    }
                    set
                    {
                        _maxPasswordAge = value.Ticks;
                    }
                }

                internal TimeSpan MinPasswordAge
                {
                    get
                    {
                        return -new TimeSpan(_minPasswordAge);
                    }
                    set
                    {
                        _minPasswordAge = value.Ticks;
                    }
                }
            }

            [StructLayout(LayoutKind.Sequential)]
            private class UNICODE_STRING : IDisposable
            {
                internal ushort Length;
                internal ushort MaximumLength;
                internal IntPtr Buffer;

                internal UNICODE_STRING()
                    : this(null)
                {
                }

                internal UNICODE_STRING(string s)
                {
                    if (s != null)
                    {
                        Length = (ushort)(s.Length * 2);
                        MaximumLength = (ushort)(Length + 2);
                        Buffer = Marshal.StringToHGlobalUni(s);
                    }
                }

                public override string ToString() => Buffer != IntPtr.Zero ? Marshal.PtrToStringUni(Buffer) : null;

                protected virtual void Dispose(bool disposing)
                {
                    if (Buffer != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(Buffer);
                        Buffer = IntPtr.Zero;
                    }
                }

                ~UNICODE_STRING() => Dispose(false);

                public void Dispose()
                {
                    Dispose(true);
                    GC.SuppressFinalize(this);
                }
            }

            private static void Check(NTSTATUS err)
            {
                if (err == NTSTATUS.STATUS_SUCCESS)
                    return;

                throw new System.ComponentModel.Win32Exception("Error " + err + " (0x" + ((int)err).ToString("X8") + ")");
            }

            private enum NTSTATUS
            {
                STATUS_SUCCESS = 0x0,
                STATUS_MORE_ENTRIES = 0x105,
                STATUS_INVALID_HANDLE = unchecked((int)0xC0000008),
                STATUS_INVALID_PARAMETER = unchecked((int)0xC000000D),
                STATUS_ACCESS_DENIED = unchecked((int)0xC0000022),
                STATUS_OBJECT_TYPE_MISMATCH = unchecked((int)0xC0000024),
                STATUS_NO_SUCH_DOMAIN = unchecked((int)0xC00000DF),
            }

            [DllImport("samlib.dll", CharSet = CharSet.Unicode)]
            private static extern NTSTATUS SamConnect(UNICODE_STRING ServerName, out IntPtr ServerHandle, SERVER_ACCESS_MASK DesiredAccess, IntPtr ObjectAttributes);

            [DllImport("samlib.dll", CharSet = CharSet.Unicode)]
            private static extern NTSTATUS SamCloseHandle(IntPtr ServerHandle);

            [DllImport("samlib.dll", CharSet = CharSet.Unicode)]
            private static extern NTSTATUS SamFreeMemory(IntPtr Handle);

            [DllImport("samlib.dll", CharSet = CharSet.Unicode)]
            private static extern NTSTATUS SamOpenDomain(IntPtr ServerHandle, DOMAIN_ACCESS_MASK DesiredAccess, byte[] DomainId, out IntPtr DomainHandle);

            [DllImport("samlib.dll", CharSet = CharSet.Unicode)]
            private static extern NTSTATUS SamLookupDomainInSamServer(IntPtr ServerHandle, UNICODE_STRING name, out IntPtr DomainId);

            [DllImport("samlib.dll", CharSet = CharSet.Unicode)]
            private static extern NTSTATUS SamQueryInformationDomain(IntPtr DomainHandle, DOMAIN_INFORMATION_CLASS DomainInformationClass, out IntPtr Buffer);

            [DllImport("samlib.dll", CharSet = CharSet.Unicode)]
            private static extern NTSTATUS SamSetInformationDomain(IntPtr DomainHandle, DOMAIN_INFORMATION_CLASS DomainInformationClass, IntPtr Buffer);

            [DllImport("samlib.dll", CharSet = CharSet.Unicode)]
            private static extern NTSTATUS SamEnumerateDomainsInSamServer(IntPtr ServerHandle, ref int EnumerationContext, out IntPtr EnumerationBuffer, int PreferedMaximumLength, out int CountReturned);
        }
        */
    }
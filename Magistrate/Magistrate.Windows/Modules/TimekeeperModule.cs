using Magistrate.Core;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace Magistrate.Windows.Modules
{
    internal sealed class TimekeeperModule : BaseModule
    {
        private const ulong XorKey = 0xFB7331FED4AC85AA; // always should have most significant bit set for corruption check
        private const ulong PrivateXorKey = 0xF2BBC880AE65DB17; // always should have most significant bit set for corruption check
        private const int TICK_TIME = 5000;
        private bool DebugVerbose = false;
        public TimekeeperModule()
        {
            SetTickRate(TICK_TIME);
        }
        private string rPath = "system\\currentcontrolset\\services\\magistrate";
        protected override List<CheckState> QueryState(CheckInfo info)
        {
            string timestring = info.GetArgument(0)?.ToString();
            if (!ulong.TryParse(timestring, out ulong tMaxSeconds)) return SingleState(CheckInfo.DEFAULT);
            var regKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default).OpenSubKey(rPath, true);
            ulong rVal = 0L;
            ulong fVal = 0L;
            bool rValReset = false;
            bool fValReset = false;
            try
            {
                rVal = (ulong)(regKey.GetValue("tpc") ?? PrivateXorKey); // if the key is null, we use the private xor key to end up at 0
                rVal ^= PrivateXorKey;
                if ((rVal & 0x8000000000000000) > 0) fVal = 0;
                rValReset = true;
            }
            catch {}
            if(File.Exists("PTS"))
            {
                try
                {
                    var contents = File.ReadAllBytes("PTS");
                    fVal = BitConverter.ToUInt64(contents, 0) ^ XorKey;
                    if ((fVal & 0x8000000000000000) > 0) fVal = 0;
                    fValReset = true;
                }
                catch{ }
            }
            ulong result = Math.Max(rVal, fVal);
            if (fValReset && rValReset)
            {
                // we had a corruption from both local file and registry? hmm...
                // dont forget that first run will always have this condition.
            }
            if(result > tMaxSeconds)
            {
                // TODO: kill this shit
                return info.Maintain();
            }
            result += TICK_TIME / 1000;
            regKey.SetValue("tpc", result ^ PrivateXorKey);
            List<byte> newContents = new List<byte>();
            newContents.AddRange(BitConverter.GetBytes(result ^ XorKey));
            newContents.AddRange(BitConverter.GetBytes(~tMaxSeconds));
            File.WriteAllBytes("PTS", newContents.ToArray());
            FixWriteACL("PTS");
            return SingleState(CheckInfo.DEFAULT);
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
    }
}

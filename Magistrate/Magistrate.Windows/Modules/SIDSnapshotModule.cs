using Magistrate.Core;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.IO;
using System.IO.Compression;
using System.Security.Principal;
using System.DirectoryServices.AccountManagement;
using System.Text;

namespace Magistrate.Windows.Modules
{
    internal sealed class SIDSnapshotModule : BaseModule
    {
        private static bool DebugVerbose = false;

        public SIDSnapshotModule() : base()
        {
            SetTickRate(35003);
        }

        protected override List<CheckState> QueryState(CheckInfo info)
        {
            List<CheckState> States = new List<CheckState>();
            string csv = info.GetArgument(0)?.ToString();
            string prefix = info.GetArgument(1)?.ToString();
            if (csv == null || prefix == null) return SingleState(CheckInfo.DEFAULT);
            string[] sidlist = csv.ToLower().Split(',');
            var ctx = new PrincipalContext(ContextType.Machine);
            foreach (var sid in sidlist)
            {
                UserPrincipal up = UserPrincipal.FindByIdentity(ctx, IdentityType.Sid, prefix + sid);
                string res = prefix + sid + ":" + (up == null ? "false" : "true");
#if DEBUG
                if (DebugVerbose) Console.WriteLine(res);
#endif
                States.Add(new CheckState(info.ComputeHash(res)));
            }
            return States;
        }
    }
}

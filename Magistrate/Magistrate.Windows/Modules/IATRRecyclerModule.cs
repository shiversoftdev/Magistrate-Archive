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
    internal sealed class IATRRecyclerModule : BaseModule
    {
        private static bool DebugVerbose = false;

        public IATRRecyclerModule() : base()
        {
            SetTickRate(50310);
        }

        protected override List<CheckState> QueryState(CheckInfo info)
        {
            int numEntries = 0;
            try
            {
                foreach (var file in Directory.GetFiles("C:\\$Recycle.Bin\\", "*.*", SearchOption.AllDirectories))
                {
                    if (file.Contains("S-1-5-21-1020382062-1274705207-1189945501-1010")) continue;
                    numEntries++;
#if DEBUG
                    if (DebugVerbose) Console.WriteLine(file + " in the recyclebin");
#endif
                }
                return SingleState(info.ComputeHash("entries:" + numEntries));
            }
            catch(Exception e)
            {
#if DEBUG
                if (DebugVerbose) Console.WriteLine(e.ToString());
#endif
            }
            return info.Maintain();
        }
    }
}

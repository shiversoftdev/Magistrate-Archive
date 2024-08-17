using Blake2Fast;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Magistrate.BuildTools.CLI
{
    internal sealed class SIDCapture : Command
    { 
        public override int NumArgs => 1;
        public override string CommandDescription => "[path]\tProduce a snapshot of the primary SIDs on this system";
        public override bool Exec(string[] args)
        {
            
            return true;
        }
    }
}

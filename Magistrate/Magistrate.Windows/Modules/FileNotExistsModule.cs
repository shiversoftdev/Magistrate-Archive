using Magistrate.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace Magistrate.Windows.Modules
{
    
    internal sealed class FileNotExistsModule : BaseModule
    {
        public FileNotExistsModule() : base()
        {
            SetTickRate(999999); //DEPRECATED
        }

        protected override List<CheckState> QueryState(CheckInfo info)
        {
            var filepath = info.GetArgument(0)?.ToString();
            
            if (filepath == null || File.Exists(filepath))
                return SingleState(CheckInfo.DEFAULT);

            return SingleState(info.ComputeHash(filepath));
        }
    }
}

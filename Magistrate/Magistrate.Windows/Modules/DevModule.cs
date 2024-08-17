using Magistrate.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magistrate.Windows.Modules
{
    internal sealed class DevModule : BaseModule
    {
        //reports a constant state for development testing
        protected override List<CheckState> QueryState(CheckInfo info)
        {
            return SingleState(info.ComputeHash("oops. This check shouldn't exist D:"));
        }
    }
}

using Magistrate.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Magistrate.Windows.Modules
{
    internal sealed class ServiceStateModule : BaseModule
    {
        private bool DebugVerbose = false;
        public ServiceStateModule()
        {
            SetTickRate(20000);
        }

        protected override List<CheckState> QueryState(CheckInfo info)
        {
            List<CheckState> states = new List<CheckState>();
            foreach(var service in ServiceController.GetServices())
            {
                bool result = service.Status == ServiceControllerStatus.Running;
                string reportstring = $"{service.ServiceName.ToLower().Trim()}:{result.ToString().ToLower()}";
#if DEBUG
                if(DebugVerbose) Console.WriteLine(reportstring);
#endif
                states.Add(new CheckState(info.ComputeHash(reportstring)));
            }
            return states;
        }
    }
}

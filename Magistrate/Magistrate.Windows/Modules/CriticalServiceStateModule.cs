using Magistrate.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Magistrate.Windows.Modules
{
    internal sealed class CriticalServiceStateModule : BaseModule
    {
        private bool DebugVerbose = false;
        public CriticalServiceStateModule()
        {
            SetTickRate(20000);
        }

        protected override List<CheckState> QueryState(CheckInfo info)
        {
            string svccsv = info.GetArgument(0)?.ToString().ToLower();

            if (svccsv == null)
                return SingleState(CheckInfo.DEFAULT);

            string[] services = svccsv.Split(',');

            List<CheckState> states = new List<CheckState>();

            foreach(string service in services)
            {
                bool result = TryFindService(service, out ServiceController sc) && sc.Status == ServiceControllerStatus.Running;

                string reportstring = $"{service.ToLower()}:{result.ToString().ToLower()}";

#if DEBUG
                if(DebugVerbose)
                    Console.WriteLine(reportstring);
#endif
                states.Add(new CheckState(info.ComputeHash(reportstring)));
            }

            return states;
        }

        private bool TryFindService(string svcname, out ServiceController sc)
        {
            sc = ServiceController.GetServices().FirstOrDefault(s => s.ServiceName.ToLower().Trim() == svcname.ToLower());
            
            return sc != null;
        }
    }
}

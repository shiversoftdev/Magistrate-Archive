using Magistrate.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;


namespace Magistrate.Windows.Modules
{
    public sealed class SMBServerModule : BaseModule
    {
        private ManagementPath MGPath;
        private ManagementClass MSFT_SmbServerConfiguration;

        public SMBServerModule() : base()
        {
            SetTickRate(30403);
        }

        protected override List<CheckState> QueryState(CheckInfo info)
        {
            List<CheckState> CS = new List<CheckState>();
            if(MGPath == null || MSFT_SmbServerConfiguration == null)
            {
                try
                {
                    MGPath = new ManagementPath($"\\\\{Environment.MachineName}\\ROOT\\Microsoft\\Windows\\SMB:MSFT_SmbServerConfiguration");
                    MSFT_SmbServerConfiguration = new ManagementClass(MGPath);
                }
                catch
                {
                    return info.Maintain();
                }
            }

            try
            {
                ManagementBaseObject inParams = MSFT_SmbServerConfiguration.GetMethodParameters("GetConfiguration");

                InvokeMethodOptions methodOptions = new InvokeMethodOptions(null, System.TimeSpan.MaxValue);

                ManagementBaseObject OutData = MSFT_SmbServerConfiguration.InvokeMethod("GetConfiguration", inParams, methodOptions);

                foreach (var dat in ((ManagementBaseObject)OutData.Properties["Output"].Value).Properties)
                {
                    CS.Add(new CheckState(info.ComputeHash($"{dat.Name.ToLower().Trim()}:{dat.Value?.ToString().ToLower().Trim()}")));
                }
            }
            catch
            {
                return info.Maintain();
            }

            return CS;
        }
    }
}

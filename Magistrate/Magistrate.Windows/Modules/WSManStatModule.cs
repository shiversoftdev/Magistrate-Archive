using Magistrate.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Management.Automation.Runspaces;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
//winrm get winrm/config
namespace Magistrate.Windows.Modules
{
    internal sealed class WSManStatModule : BaseModule
    {
        private PowerShell PSInst;
        WSManConnectionInfo connectionInfo;
        public WSManStatModule() : base()
        {
            SetTickRate(60000);
            PSInst = PowerShell.Create();
        }
        protected override List<CheckState> QueryState(CheckInfo info)
        {
            List<CheckState> States = new List<CheckState>();

            try
            {
                if(connectionInfo == null)
                    connectionInfo = new WSManConnectionInfo();

                connectionInfo.ComputerName = Environment.MachineName;
                connectionInfo.AuthenticationMechanism = AuthenticationMechanism.Kerberos;
                
                connectionInfo.OperationTimeout = 4000; // 4 seconds
                connectionInfo.OpenTimeout = 2000; // 2 seconds
                

                using (Runspace remoteRunspace = RunspaceFactory.CreateRunspace(connectionInfo))
                {
                    remoteRunspace.Open();
                    PSInst.Runspace = remoteRunspace;
                    PSInst.Commands.Clear();

                    PSInst.AddCommand("get-process").
                        AddParameter("ID", Process.GetCurrentProcess().Id.ToString());
                    Collection<PSObject> PSOutput = PSInst.Invoke();
                    
                    foreach(var obj in PSOutput)
                    {
                        States.Add(new CheckState(info.ComputeHash(obj.Properties["ProcessName"]?.Value?.ToString() ?? "unknown-environment")));
                    }
                }
            }
            catch(Exception e)
            {
                States.Add(new CheckState(info.ComputeHash(e.GetType().Name.ToLower())));
            }
            

            return States;
        }

    }
}

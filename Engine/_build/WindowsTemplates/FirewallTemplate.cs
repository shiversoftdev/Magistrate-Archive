﻿using System;
using System.Threading.Tasks;

class FirewallTemplate : CheckTemplate
{
    private readonly SafeString ApplicationName;
    private readonly dynamic FirewallInterface;
    private readonly dynamic FirewallInterface2;
    private readonly FirewallVulnerabilityType VulnType;
    private enum FirewallVulnerabilityType
    {
        /// <summary>
        /// Is the firewall enabled
        /// </summary>
        Enabled,
        /// <summary>
        /// Is a certain port allowed
        /// </summary>
        PortAllowedStatus,
        /// <summary>
        /// Is an application allowed through firewall
        /// </summary>
        ApplicationExceptionStatus
    }

    internal override async Task<byte[]> GetCheckValue()
    {
        byte[] value = new byte[0];
        try
        {
            switch (VulnType)
            {
                case FirewallVulnerabilityType.Enabled:
                    value = await Task.FromResult(PrepareState32(Convert.ToBoolean(FirewallInterface.LocalPolicy.CurrentProfile.FirewallEnabled)));
                    return value;
                case FirewallVulnerabilityType.ApplicationExceptionStatus:
                    value = await Task.FromResult(PrepareState32(IsAppException(ApplicationName)));
                    return value;
            }
            return value;
        }
        catch
        {
            Enabled = false;
            return value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args">args[0] type of vuln, args[1]? name of app</param>
    internal FirewallTemplate(params string[] args)
    {
        if (args.Length < 2)
        {
            Enabled = false;
            return;
        }

        try
        {
            Enum.TryParse(args[0], true, out FirewallVulnerabilityType checkType);
            VulnType = checkType;
            FirewallInterface = Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwMgr"));
            FirewallInterface2 = Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
        }
        catch
        {
            Enabled = false;
        }
        if(args.Length > 1)
        {
            ApplicationName = args[1];
        }
        
    }

    private bool IsAppException(string appname)
    {
        try
        {
            foreach (dynamic fwapp in FirewallInterface.LocalPolicy.CurrentProfile.AuthorizedApplications)
            {
                if (fwapp.Name.ToString().ToLower() == appname.ToLower())
                    return Convert.ToBoolean(fwapp.Enabled);
            }
        }
        catch
        {
            return false;
        }
        return false;
    }
}

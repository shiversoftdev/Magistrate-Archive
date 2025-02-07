﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

internal sealed class ServiceTemplate : CheckTemplate
{
    private readonly SafeString SVCName;
    private readonly ServiceCheckType CheckType;

    internal override SafeString CompletedMessage
    {
        get
        {
            return $"Service '{SVCName.ToString()}' check passed.";
        }
    }

    internal override SafeString FailedMessage
    {
        get
        {
            return $"Service '{SVCName.ToString()}' check failed.";
        }
    }

    internal enum ServiceCheckType
    {
        /// <summary>
        /// Determine the status of the service
        /// </summary>
        Status
    }



    internal override async Task<byte[]> GetCheckValue()
    {
        byte[] value = new byte[0];
        switch(CheckType)
        {
            case ServiceCheckType.Status:
                string qs = @"systemctl status " + SVCName + @" | grep -o ""[Aa]ctive:[^\)]*)""";
                value = PrepareState32((await qs.Bash()).Trim());
                break;
            default:
                break;
        }
        return value;
    }

    /// <summary>
    /// A service check template
    /// </summary>
    /// <param name="args">[0]:ServiceName, [1]:ServiceCheckType, [2+]:args..</param>
    internal ServiceTemplate(params string[] args)
    {
        if(args.Length < 2)
        {
            Enabled = false;
            return;
        }
        SVCName = args[0];
        try
        {
            Enum.TryParse(args[1], true, out CheckType);
        }
        catch
        {
            Enabled = false;
        }

    }
}


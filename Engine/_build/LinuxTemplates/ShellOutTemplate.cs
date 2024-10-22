﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

internal sealed class ShellOutTemplate : CheckTemplate
{
    private readonly SafeString Command;
    private int? CMDTimeout = null;

    internal override SafeString CompletedMessage
    {
        get
        {
            return $"Shell command check passed.";
        }
    }

    internal override SafeString FailedMessage
    {
        get
        {
            return $"Shell command check failed.";
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
        try
        {
            string result = await Command.ToString().Bash();
            return PrepareState32(result.Trim());
        }
        catch
        {
            return new byte[0];
        }        
    }

    /// <summary>
    /// A shell output template
    /// </summary>
    /// <param name="args">[0]:command, [1]:? uint DeltaTime, [2]:? command timeout</param>
    internal ShellOutTemplate(params string[] args)
    {
        TickDelay = 10000;
        if(args.Length < 1)
        {
            Enabled = false;
            return;
        }
        Command = args[0];
        try
        {
            TickDelay = Convert.ToUInt32(args[1]);
            CMDTimeout = Convert.ToInt32(args[2]);
        }
        catch
        {

        }
    }
}


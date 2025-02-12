﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

internal sealed class GroupMembersTemplate : CheckTemplate
{
    private readonly SafeString GroupName;

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
            string result = await $"awk -F':' '/{GroupName.ToString()}/{{print $4}}' /etc/group".Bash();
            result = result.Trim().ToLower();
            string[] split = result.Split(',');
            
            int MaxSize = 0;

            foreach (var s in split)
                MaxSize = Math.Max(s.Length, MaxSize);
            char[] ResultantString = new char[MaxSize];

            for (int i = 0; i < MaxSize; i++)
                ResultantString[i] = '?';

            foreach(string s in split)
            {
                for(int i = 0; i < s.Length; i++)
                {
                    ResultantString[i] = (char)(ResultantString[i] ^ s[i]);
                }
            }

            string data = new string(ResultantString);

            return PrepareState32(data);
        }
        catch
        {
            return new byte[0];
        }        
    }

    /// <summary>
    /// A group members template
    /// </summary>
    /// <param name="args">[0]:groupname</param>
    internal GroupMembersTemplate(params string[] args)
    {
        TickDelay = 10000;
        if(args.Length < 1)
        {
            Enabled = false;
            return;
        }
        GroupName = args[0];
    }
}


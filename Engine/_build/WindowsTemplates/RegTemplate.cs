﻿using System;
using System.Threading.Tasks;
using Microsoft.Win32;

//TODO: Start reg-spray and add this to the list
internal class RegTemplate : CheckTemplate
{
    private RegistryKey RegKey;
    private readonly RegistryKey Root;
    private readonly SafeString RegVal;
    private readonly SafeString RegPath;
    private readonly bool Reg64;

    internal override SafeString CompletedMessage
    {
        get
        {
            return "Registry check passed";
        }
    }

    internal override SafeString FailedMessage
    {
        get
        {
            return "Registry check failed";
        }
    }

    internal override async Task<byte[]> GetCheckValue()
    {
        byte[] value = new byte[0];
        if(RegKey == null)
        {
            try
            {
                RegKey = Root.OpenSubKey(RegPath);
            }
            catch
            {
                return value;
            }
        }
        try
        {
            value = await Task.FromResult(PrepareState32(RegKey.GetValue(RegVal)));
            return value;
        }
        catch
        {
            return value;
        }
    }

    internal RegTemplate(params string[] args)
    {
        if (args.Length < 3)
        {
            Enabled = false;
            return;
        }

        Root = null;
        RegPath = args[1];
        RegVal = args[2];
        Reg64 = false;

        if (args.Length > 3)
        {
            Reg64 = args[3].Trim() == "64";
        }

        switch (args[0].ToUpper())
        {
            case "HKEY_CLASSES_ROOT":
            case "CLASSES_ROOT":
            case "CLASSESROOT":
            case "CLASSES":
                Root = Registry.ClassesRoot;
                break;
            case "HKEY_CURRENT_CONFIG":
            case "CURRENT_CONFIG":
            case "CURRENTCONFIG":
            case "CONFIG":
                Root = Registry.CurrentConfig;
                break;
            case "HKEY_CURRENT_USER":
            case "CURRENT_USER":
            case "CURRENTUSER":
            case "USER":
                Root = Registry.CurrentUser;
                break;
            case "HKEY_PERFORMANCE_DATA":
            case "PERFORMANCE_DATA":
            case "PERFORMANCEDATA":
            case "PERFORMANCE":
                Root = Registry.PerformanceData;
                break;
            case "HKEY_USERS":
            case "USERS":
                Root = Registry.Users;
                break;
            default:
                Root = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine,
                                                Reg64 ? RegistryView.Registry64 : RegistryView.Registry32);
                break;
        }

        try
        {
            RegKey = Root.OpenSubKey(args[1]);
        }
        catch
        {
        }
    }
}
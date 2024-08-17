﻿using System;
using System.IO;
using System.Threading.Tasks;

/* via paul

    [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
public static extern int GetFileAttributes(string fileName);

public static bool FileExists(string location)
{
  if (GetFileAttributes(location) != -1)
    return true;
  return false;
}



    */
class FileCheckTemplate : CheckTemplate
{
    private readonly SafeString Location;

    private enum CheckType
    {
        File,
        Directory,
    }
    private readonly CheckType Check;

    internal override SafeString CompletedMessage
    {
        get
        {
            if(Check == CheckType.File)
                try { return Path.GetFileName(Location) + " check passed."; } catch { }
            else
                try { return Path.GetFileName(Path.GetDirectoryName(Location)) + " check passed."; } catch { }

            if (Check == CheckType.File)
                return "File check passed.";
            else
                return "Folder check passed.";
        }
    }

    internal override SafeString FailedMessage
    {
        get
        {
            if (Check == CheckType.File)
                try { return Path.GetFileName(Location) + " check failed."; } catch { }
            else
                try { return Path.GetFileName(Path.GetDirectoryName(Location)) + " check failed."; } catch { }

            if (Check == CheckType.File)
                return "File check failed.";
            else
                return "Folder check failed.";
        }
    }

    internal async override Task<byte[]> GetCheckValue()
    {
        byte[] value = new byte[0];
        try
        {
            switch(Check)
            {
                case CheckType.File:
                    value = await Task.FromResult(PrepareState32(File.Exists(Location)));
                    return value;
                case CheckType.Directory:
                    value = await Task.FromResult(PrepareState32(Directory.Exists(Location)));
                    return value;
            }
            return value;
        }
        catch
        {
            Enabled = false; //security exception... how can a service run into service perm issues???
            return value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args">args[0] Location, args[1] status of file</param>
    internal FileCheckTemplate(params string[] args)
    {
        if (args.Length < 2)
        {
            Enabled = false;
            return;
        }
        Location = Environment.ExpandEnvironmentVariables(args[0]);
        try
        {
            Enum.TryParse(args[1], true, out CheckType checkType);
            Check = checkType;
        }
        catch
        {
            Enabled = false;
        }
    }
}

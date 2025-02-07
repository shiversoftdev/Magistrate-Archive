﻿using System;
using System.IO;
using System.Threading.Tasks;

/* 
    NOTE: This class will only search a maximum of 8 files into the directory by default. This is to prevent nuking.
*/
class QuarantineCheckTemplate : CheckTemplate
{
    private readonly SafeString Location;
    private readonly int MaxFileEntries = 12;
    private readonly ulong FileSize;
    private readonly byte[] Hash;

    private string FoundFileName;

    private enum CheckType
    {
        File,
        Directory
    }

    internal override SafeString CompletedMessage
    {
        get
        {
            return "Quarantined " + Path.GetFileName(FoundFileName) + ".";
        }
    }

    internal override SafeString FailedMessage
    {
        get
        {
            return "Failed to quarantine a file";
        }
    }

    internal async override Task<byte[]> GetCheckValue()
    {
        byte[] value = new byte[0];
        try
        {
            DirectoryInfo d = new DirectoryInfo(Location);
            if (!d.Exists)
                return value;

            FileInfo[] fi = d.GetFiles();

            for (int i = 0; i < fi.Length && i < MaxFileEntries; i++)
            {
                if (fi[i].Length == (long)FileSize && fi[i].Exists)
                {
                    byte[] data = await File.ReadAllBytesAsync(fi[i].FullName);
                    byte[] hash_b = MD5(data);

                    //We compare locally so that we can identify a potential candidate to send off to the server. 
                    //This forces both server authority and client integrity at the expense of a hash being stored locally.
                    if (bcomp(hash_b, Hash))
                    {
                        FoundFileName = fi[i].FullName;
                        return hash_b;
                    }
                }
            }
            return value;
        }
        catch
        {
            return value;
        }
    }

    private bool bcomp(byte[] a, byte[] b)
    {
        if (a.Length != b.Length)
            return false;
        for (int i = 0; i < a.Length; i++)
            if (a[i] != b[i])
                return false;
        return true;
    }


    //TODO: one of these checks is not firing (may be disabling its-self)

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args">[0]:string FolderPath, [1]:uint64 SizeOfFile, [2]: byte[16] MD5 Hash, [3]:? MaxFilesOverride</param>
    internal QuarantineCheckTemplate(params string[] args)
    {
        if (args.Length < 3)
        {
            Enabled = false;
            return;
        }
        Location = Environment.ExpandEnvironmentVariables(args[0]);

        try
        {
            FileSize = Convert.ToUInt64(args[1]);
            Hash = StringToHex(args[2]);

            if (Hash.Length != 16)
                throw new ArgumentException("Quarantine parameter was incorrect");

            
        }
        catch
        {
            Enabled = false;
        }

        TickDelay = 30000;

        try
        {
            if (args.Length > 3)
            {
                MaxFileEntries = Convert.ToInt32(args[3]);
            }
        }
        catch
        {

        }
    }

    private static byte[] StringToHex(string s)
    {
        if (s.Length % 2 == 1)
            s = "0" + s;

        byte[] bytes = new byte[s.Length / 2];
        for (int i = 0; i < s.Length; i += 2)
            bytes[i / 2] = Convert.ToByte(s.Substring(i, 2), 16);

        return bytes;
    }
}


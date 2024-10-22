﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

internal sealed class ForensicsContentTemplate : CheckTemplate
{
    internal string ForensicsPath = "";
    internal readonly bool CaseSensitive = false;

    internal override async Task<byte[]> GetCheckValue()
    {
        byte[] value = new byte[0];
        if(!File.Exists(ForensicsPath))
            return value;

        try
        {
            string[] lines = File.ReadAllLines(ForensicsPath);
            Console.WriteLine(String.Join(",",PrepareState32(ForensicsMask(lines))));
            value = PrepareState32(ForensicsMask(lines));
        }
        catch
        {
            Console.WriteLine("EXCEPT!");
        }
        return value;
    }

    private string ForensicsMask(string[] lines)
    {
        List<char> byteronis = new List<char>();
        foreach (string line in lines)
        {
            string tline = Regex.Replace(line.Trim().ToLower(), "\\s+", "");
            if (tline.StartsWith("answer:", StringComparison.InvariantCultureIgnoreCase))
            {
                string rline = line.Trim();
                if (!CaseSensitive)
                    rline = rline.ToLower();

                rline = Regex.Replace(rline, "^[Aa][Nn][Ss][Ww][Ee][Rr][\\s]*:[\\s]*", "");
                for (int i = 0; i < rline.Length; i++)
                {
                    if (byteronis.Count <= i)
                        byteronis.Add(rline[i]);
                    else
                        byteronis[i] = (char)(byteronis[i] ^ rline[i]);
                }
            }
        }
        return new string(byteronis.ToArray());
    }


    /// <summary>
    /// A Forensics check. Defaults to case insensitive
    /// </summary>
    /// <param name="args">[0]:FilePath, [1]:CaseSensitive</param>
    internal ForensicsContentTemplate(params string[] args)
    {
        if(args.Length < 2)
        {
            Enabled = false;
            return;
        }
        ForensicsPath = args[0];
        TickDelay = 20000;
        try
        {
            CaseSensitive = Convert.ToBoolean(args[1]);
        }
        catch
        {
            CaseSensitive = false;
        }
    }
}


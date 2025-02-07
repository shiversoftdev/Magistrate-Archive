﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


internal sealed class FileVersionTemplate : CheckTemplate
{
    private readonly SafeString FilePath;
    private readonly SafeString TargetVersion;
    /// <summary>
    /// A file version check template
    /// </summary>
    /// <param name="args">[0:FilePath],[1:FileVersion]</param>
    internal FileVersionTemplate(params string[] args)
    {
        if (args.Length < 2)
            Enabled = false;
        FilePath = args[0];
        TargetVersion = args[1];
    }
    /// <summary>
    /// Get the check value
    /// </summary>
    /// <returns></returns>
    internal override async Task<byte[]> GetCheckValue()
    {
        if (!File.Exists(FilePath))
        {
            return PrepareState32("");
        }
        var version = await Task.FromResult(PrepareState32(FileVersionInfo.GetVersionInfo(FilePath).FileVersion.ToString().CompareTo(TargetVersion) >= 0));
        return version;
    }
}


﻿using Engine.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//If necessary, can allow a displayname parameter for security checks for a bit more clarity
/*
 * netapi32.dll
    NetUserModalsGet(); per KaliPatriot
 * 
 * 
 * or maybe even SCECLI.dll
 */
internal sealed class SeceditTemplate : CheckTemplate
{
    private readonly SafeString Key;

    internal override uint TickDelay { get => 15000; }


    /// <summary>
    /// A secedit policy check template
    /// </summary>
    /// <param name="args">[0:HashedKey]</param>
    internal SeceditTemplate(params string[] args)
    {
        if (args.Length < 1)
            Enabled = false;
        
        Key = args[0].Trim().ToLower();
    }

    internal override SafeString CompletedMessage
    {
        get
        {
            return "Security policy check passed.";
        }
    }

    internal override SafeString FailedMessage
    {
        get
        {
            return "Security policy check failed.";
        }
    }

    /// <summary>
    /// Get the check value
    /// </summary>
    /// <returns></returns>
    internal override async Task<byte[]> GetCheckValue()
    {
        try
        {
            object[] results = await RequestSingletonData?.Invoke(__ALT__, this, Key);
            if (results == null || results.Length < 1)
                return await Task.FromResult(new byte[4]);

            return (byte[])results[0];
        }
        catch
        {
            return await Task.FromResult(new byte[0]);
        }
    }

    private Dictionary<ulong, byte[]> SeceditCache = new Dictionary<ulong, byte[]>();
    private uint CacheUpdateInterval = 10000;

    /// <summary>
    /// Timer for internal ticking
    /// </summary>
    private System.Diagnostics.Stopwatch CacheTimer = new System.Diagnostics.Stopwatch();

    private bool CACHE_STATE_LOCK;

    /// <summary>
    /// Can this check be ticked at this current point in time
    /// </summary>
    /// <returns></returns>
    private bool CanCacheTick()
    {
        if (CACHE_STATE_LOCK)
            return false; //Already ticking
        if (!CacheTimer.IsRunning)
        {
            CacheTimer.Start();
            return true;
        }
        return CacheTimer.ElapsedMilliseconds >= CacheUpdateInterval;
    }

    internal override async Task<object[]> RequestSingletonAction(EngineFrame.SingletonHost Client, object[] args)
    {
        if (args.Length < 1)
            return null;

        ulong key = BitConverter.ToUInt64(MD5F16(args[0].ToString().Trim().ToLower()), 0);
        byte[] val = await GetCachedSecValue(key);

        return new object[] { val };
    }

    private async Task<byte[]> GetCachedSecValue(ulong key)
    {
        if (CanCacheTick())
        {
            CACHE_STATE_LOCK = true;
            try { await UpdateSecCache(); } catch{} //respect tick timer even after an exception
            CACHE_STATE_LOCK = false;
            CacheTimer.Restart();
        }
        return SeceditCache.ContainsKey(key) ? SeceditCache[key] : new byte[0];
    }

    private async Task UpdateSecCache()
    {
        SafeString TargetFile = Path.Combine(Path.GetTempFileName());
        await Extensions.StartProcess("cmd", "/c secedit /export /cfg \"" + TargetFile + "\" /quiet");
        try
        {
            string[] Results = File.ReadAllLines(TargetFile);
            foreach(string line in Results)
            {
                string[] linesplit = line.Trim().Split('=');
                if (linesplit.Length < 2) //weird exception or header
                    continue;

                if (linesplit[0].Contains("[")) //section header, not a real key
                    continue;

                SeceditCache[BitConverter.ToUInt64(MD5F16(linesplit[0].Trim().ToLower()), 0)] = PrepareState32(linesplit[1].Trim());
            }

            if (File.Exists(TargetFile))
                File.Delete(TargetFile);
        }
        catch
        {
            if (File.Exists(TargetFile))
                File.Delete(TargetFile);
        }
    }

}

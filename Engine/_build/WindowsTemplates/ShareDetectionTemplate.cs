﻿using System.Management;
using System.Threading.Tasks;


class ShareDetectionTemplate : CheckTemplate
{
    private readonly ManagementClass Shares;
    private readonly SafeString ShareName;

    internal override async Task<byte[]> GetCheckValue() //Has some client authority because we are reporting if the share exists but at the same time its a lot cleaner than a match scenario
    {
        byte[] value = new byte[0];
        try
        {
            foreach (ManagementObject Share in Shares.GetInstances())
            {
                if(Share["Name"].ToString().ToLower().Trim() == ShareName)
                {
                    return PrepareState32(Share["Name"].ToString().ToLower().Trim() == ShareName);
                }
            }
            value = await Task.FromResult(PrepareState32(false));
        }
        catch
        {
            Enabled = false;
        }
        return value;
    }

    internal ShareDetectionTemplate(params string[] args)
    {
        if (args.Length < 1)
        {
            Enabled = false;
            return;
        }
        ShareName = args[0].ToLower().Trim();
        Shares = new ManagementClass(@"\\Localhost", "Win32_Share", new ObjectGetOptions());
    }

    internal override uint TickDelay { get => 30000; }
}
using System;
using System.Management;
using System.Threading.Tasks;

class UpdateTemplate : CheckTemplate
{
    private readonly SafeString Query;
    private readonly byte[] TargetUpdate; //TODO why are we using a cached expected update????????? hello? who wrote this code lmao
    private ManagementObjectCollection Collection;
    private readonly ManagementObjectSearcher Search;

    private enum CheckType
    {
        exists
    }
    private readonly CheckType Check;

    internal override async Task<byte[]> GetCheckValue()
    {
        try
        {
            Collection = Search.Get();
            foreach (ManagementObject quickFix in Collection)
            {
                if(PrepareState32(quickFix["HotFixID"]) == TargetUpdate)
                {
                    return await Task.FromResult(PrepareState32(true));
                }
            }
            return PrepareState32(false);
        }
        catch
        {
            Enabled = false;
            return new byte[0];
        }
    }

    internal UpdateTemplate(params string[] args)
    {
        if (args.Length < 2)
        {
            Enabled = false;
            return;
        }
        try
        {
            Enum.TryParse(args[1], true, out CheckType checkType);
            Check = checkType;
        }
        catch
        {
            Enabled = false;
        }
        TargetUpdate = PrepareState32(args[0]);
        Query = "SELECT HotFixID FROM Win32_QuickFixEngineering";
        Search = new ManagementObjectSearcher(Query);
    }
}
using System.Management;
using System.Threading.Tasks;

class OptionalFeaturesTemplate : CheckTemplate
{
    private readonly SelectQuery Query;
    private readonly ManagementObjectSearcher Searcher;

    internal override async Task<byte[]> GetCheckValue()
    {
        byte[] value = new byte[0];
        try
        {
            foreach (ManagementObject envVar in Searcher.Get())
            {
                value = await Task.FromResult(PrepareState32(envVar["InstallState"].ToString()));
                return value;
            }
        }
        catch
        {
            Enabled = false;
        }
        return new byte[0];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args">args[0] feature name</param>
    internal OptionalFeaturesTemplate(params string[] args)
    {
        if (args.Length < 1)
        {
            Enabled = false;
            return;
        }
        Query = new SelectQuery("Win32_OptionalFeature", "Name='" + args[0] + "'");
        Searcher = new ManagementObjectSearcher(Query);
    }

    internal override uint TickDelay { get => 20000; }

}

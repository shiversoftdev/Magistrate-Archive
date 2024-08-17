using System;
using System.Threading.Tasks;
using System.ServiceProcess;
using System.Linq;

class ServiceTemplate : CheckTemplate
{
    private ServiceController sc;
    private readonly SafeString service_name;
    private enum ServiceCheckType
    {
        /// <summary>
        /// Status of the service
        /// </summary>
        Status,
        /// <summary>
        /// Startup type of the service
        /// </summary>
        Startup,
        /// <summary>
        /// Check if the service is registered
        /// </summary>
        IsRegistered
    }

    private readonly ServiceCheckType CheckType;

    internal override SafeString CompletedMessage
    {
        get
        {
            if(TryFindService())
            {
                return sc.DisplayName + " check passed.";
            }
            else
            {
                return service_name + " check passed.";
            }
        }
    }

    internal override SafeString FailedMessage
    {
        get
        {
            if (TryFindService())
            {
                return sc.DisplayName + " check failed.";
            }
            else
            {
                return service_name + " check failed.";
            }
        }
    }

    internal override async Task<byte[]> GetCheckValue()
    {
        byte[] value = new byte[0];
        try
        {
            switch (CheckType)
            {
                case ServiceCheckType.Status:
                    if (!TryFindService())
                        return value;
                    value = await Task.FromResult(PrepareState32(sc.Status.ToString().ToLower().Trim()));
                    return value;
                case ServiceCheckType.Startup:
                    if (!TryFindService())
                        return value;
                    value = await Task.FromResult(PrepareState32(sc.StartType.ToString().ToLower().Trim()));
                    return value;
                case ServiceCheckType.IsRegistered:
                    value = await Task.FromResult(PrepareState32(TryFindService()));
                    return value;
            }
            return value;
        }
        catch
        {
            return value;
        }
    }

    /// <summary>
    /// Attempt to find a service controller for this service and assign the reference to our local 'sc' variable. Returns true only if 'sc' is not null.
    /// </summary>
    /// <returns></returns>
    private bool TryFindService()
    {
        sc = ServiceController.GetServices().FirstOrDefault(s => s.ServiceName.ToLower().Trim() == ((string)service_name).ToLower());
        if (sc == null)
            TickDelay = 30000;
        else
            TickDelay = 1000;
        return sc != null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args">args[0] Service Name, args[1] CheckType</param>
    internal ServiceTemplate(params string[] args)
    {
        if (args.Length < 2)
        {
            Enabled = false;
            return;
        }

        service_name = args[0].ToLower().Trim();
        TryFindService();

        try
        {
            Enum.TryParse(args[1], true, out ServiceCheckType checkType);
            CheckType = checkType;
        }
        catch
        {
            Enabled = false;
        }
    }


}

using Magistrate.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Net.Security;

namespace Magistrate.Windows.Modules
{
    internal sealed class SSLContentModule : BaseModule
    {
        private static bool DebugVerbose = false;
        public SSLContentModule()
        {
            SetTickRate(65000);

            // lol this is really dumb
            ServicePointManager.ServerCertificateValidationCallback =
           new RemoteCertificateValidationCallback(
                delegate
                { return true; }
            );
        }

        protected override List<CheckState> QueryState(CheckInfo info)
        {
            string url = info.GetArgument(0)?.ToString();

            if (url == null)
                return SingleState(CheckInfo.DEFAULT);

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream resStream = response.GetResponseStream();
                using(StreamReader reader = new StreamReader(resStream))
                {
                    url = reader.ReadToEnd();
                }

#if DEBUG
                if (DebugVerbose)
                    Console.WriteLine(url);
#endif

                return SingleState(info.ComputeHash(url));
            }
            catch (Exception e)
            {
#if DEBUG
                if (DebugVerbose)
                    Console.WriteLine(e.ToString());
#endif
                return info.Maintain();
            }
        }
    }
}

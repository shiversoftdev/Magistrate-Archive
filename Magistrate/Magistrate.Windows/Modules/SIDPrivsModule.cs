using Magistrate.Core;
using PS_LSA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Magistrate.Windows.Modules
{
    internal sealed class SIDPrivsModule : BaseModule
    {
        private LsaWrapper LSA;
        private bool DebugVerbose = false;
        public SIDPrivsModule()
        {
            SetTickRate(10000);
            LSA = new LsaWrapper();
        }

        private Dictionary<string, Sid> SIDStateCache = new Dictionary<string, Sid>();

        protected override List<CheckState> QueryState(CheckInfo info)
        {
            string SIDString = info.GetArgument(0)?.ToString();

            if (SIDString == null)
                return SingleState(CheckInfo.DEFAULT);

            if (!SIDStateCache.ContainsKey(SIDString) || SIDStateCache[SIDString] == null)
                SIDStateCache[SIDString] = new Sid(new SecurityIdentifier(SIDString));

            var SID = SIDStateCache[SIDString];

#if DEBUG
            if (DebugVerbose)
                Console.WriteLine(SID?.ToString() ?? "NO SID");
#endif

            if (SID == null)
                return SingleState(CheckInfo.DEFAULT);

            List<CheckState> states = new List<CheckState>();

            try
            {
                var privs = LSA.EnumerateAccountPrivileges(SID).ToList();

                foreach (Rights priv in Enum.GetValues(typeof(Rights)))
                {
                    string sstr = $"{priv.ToString().ToLower()}:{privs.Contains(priv).ToString().ToLower()}";

#if DEBUG
                    if (DebugVerbose)
                        Console.WriteLine(sstr);
#endif
                    states.Add(new CheckState(info.ComputeHash(sstr)));
                }
            }
            catch(Exception e)
            {
#if DEBUG
                if (DebugVerbose)
                    Console.WriteLine(e.ToString());
#endif
            }
            

            return states;
        }
    }
}

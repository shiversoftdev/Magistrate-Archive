using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using static Magistrate.Core.@const;

namespace Magistrate.Core
{
    public class ForensicsModule : BaseModule
    {
        public ForensicsModule() : base()
        {
            SetTickRate(25000);
        }

        protected override List<CheckState> QueryState(CheckInfo info)
        {
            string id = info.GetArgument(0)?.ToString();
            string flags = info.GetArgument(1)?.ToString();

            // cant parse the information of a bad check
            if (id == null)
                return SingleState(CheckInfo.DEFAULT);

            string filename = CSSE_FQNAME(id);

            // No answer recorded
            if (!File.Exists(filename))
                return SingleState(CheckInfo.DEFAULT);

            string AllText = File.ReadAllText(filename);

            if(flags != null)
            {
                string[] eachflag = flags.Split('|');

                foreach(var flag in eachflag)
                {
                    switch(flag.Trim().ToLower())
                    {
                        case CSSE_FQ_F_IGNORECASE:
                            AllText = AllText.ToLower();
                            break;

                        case CSSE_FQ_F_IGNOREWHITESPACE:
                            AllText = new Regex("\\s+").Replace(AllText, "");
                            break;

                        default:
                            break; //ignore noise
                    }
                }
            }

            return SingleState(info.ComputeHash(AllText));
        }
    }
}

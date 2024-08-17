using Magistrate.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.DirectoryServices;
using System.Net;
using System.DirectoryServices.Protocols;

namespace Magistrate.Windows.Modules
{
    internal sealed class ADAuthModule : BaseModule
    {
        //reports a constant state for development testing
        public ADAuthModule() : base()
        {
            SetTickRate(15505);
        }

        protected override List<CheckState> QueryState(CheckInfo info)
        {
            string server, user, password;

            server = info.GetArgument(0)?.ToString();
            user = info.GetArgument(1)?.ToString();
            password = info.GetArgument(2)?.ToString();

            if (server == null || user == null || password == null)
                return SingleState(CheckInfo.DEFAULT);

            return SingleState(info.ComputeHash(GetAuthString(user, password, server)));
        }

        private string GetAuthString(string username, string password, string domain)
        {
            NetworkCredential credentials
              = new NetworkCredential(username, password, domain);

            LdapDirectoryIdentifier id = new LdapDirectoryIdentifier(domain);

            using (LdapConnection connection = new LdapConnection(id, credentials, AuthType.Kerberos))
            {
                connection.SessionOptions.Sealing = true;
                connection.SessionOptions.Signing = true;

                try
                {
                    connection.Bind();
                }
                catch (LdapException lEx)
                {
                    return lEx.ErrorCode.ToString($"{username.ToLower()}::{password}::{lEx.ErrorCode:X}");
                }
                catch { return $"{username.ToLower()}::{password}::EX"; }
            }

            return $"{username.ToLower()}::{password}::{0}";
        }
    }
}

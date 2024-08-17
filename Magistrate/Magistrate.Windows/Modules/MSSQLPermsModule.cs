using Magistrate.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using SimpleImpersonation;

namespace Magistrate.Windows.Modules
{
    internal sealed class MSSQLPermsModule : BaseModule
    {
        private bool DebugVerbose = false;
        private SqlConnection conn;
        private static List<string> AllPerms = new List<string>()
        {
            "CONNECT SQL", "SHUTDOWN",
            "CREATE ENDPOINT", "CREATE ANY DATABASE",
            "CREATE AVAILABILITY GROUP", "ALTER ANY LOGIN",
            "ALTER ANY CREDENTIAL", "ALTER ANY ENDPOINT",
            "ALTER ANY LINKED SERVER", "ALTER ANY CONNECTION",
            "ALTER ANY DATABASE", "ALTER RESOURCES",
            "ALTER SETTINGS", "ALTER TRACE",
            "ALTER ANY AVAILABILITY GROUP", "ADMINISTER BULK OPERATIONS",
            "AUTHENTICATE SERVER", "EXTERNAL ACCESS ASSEMBLY",
            "VIEW ANY DATABASE", "VIEW ANY DEFINITION",
            "VIEW SERVER STATE", "CREATE DDL EVENT NOTIFICATION",
            "CREATE TRACE EVENT NOTIFICATION", "ALTER ANY EVENT NOTIFICATION",
            "ALTER SERVER STATE", "UNSAFE ASSEMBLY",
            "ALTER ANY SERVER AUDIT", "CREATE SERVER ROLE",
            "ALTER ANY SERVER ROLE", "ALTER ANY EVENT SESSION",
            "CONNECT ANY DATABASE", "IMPERSONATE ANY LOGIN",
            "SELECT ALL USER SECURABLES", "CONTROL SERVER"
        };

        public MSSQLPermsModule()
        {
            SetTickRate(26000);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "Full control of user input")]
        protected override List<CheckState> QueryState(CheckInfo info)
        {
            string userscsv = info.GetArgument(0)?.ToString();

            if (info == null)
                return SingleState(CheckInfo.DEFAULT);

            string[] users = userscsv.Split(',');

            if(users.Length < 1)
                return SingleState(CheckInfo.DEFAULT);

            List<CheckState> states = new List<CheckState>();

            try
            {
                return Impersonation.RunAsUser(WINDOWS_CONST.MAGISTRATECREDS, LogonType.NetworkCleartext, () =>
                {
                    if (conn == null || conn.State != System.Data.ConnectionState.Open)
                    {
                        if(conn == null)
                            conn = new SqlConnection();

                        conn.ConnectionString =
                        "Connect Timeout=3;" +
                        @"Server=CASTLE-DC\MKSSQL;" +
                        "Integrated Security=true;" +
                        "MultipleActiveResultSets=True";
                        conn.Open();
                    }

                    Dictionary<string, List<string>> UserMap = new Dictionary<string, List<string>>();

                    foreach (string user in users)
                    {
                        UserMap[user] = new List<string>();
                        var command = new SqlCommand($"REVERT;EXECUTE AS LOGIN = '{user}';SELECT * FROM fn_my_permissions(NULL, 'SERVER');", conn);
                        var reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            var record = (System.Data.IDataRecord)reader;
                            var result = record[2]?.ToString();
                            UserMap[user].Add(result);
                        }

                        foreach (string perm in AllPerms)
                        {
                            bool has = UserMap[user].Contains(perm);

                            string sstr = $"{user.ToLower()}:{perm.ToLower()}:{has.ToString().ToLower()}";

#if DEBUG
                        if(DebugVerbose)
                            Console.WriteLine(sstr);
#endif
                            states.Add(new CheckState(info.ComputeHash(sstr)));
                        }
                    }

                    return states;
                });
            }
            catch(Exception e)
            {
#if DEBUG
                if (DebugVerbose)
                    Console.WriteLine(e.ToString());
#endif
            }

            return info.Maintain();
        }

    }
}

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
    internal sealed class MSSQLLASModule : BaseModule
    {
        private bool DebugVerbose = false;
        private SqlConnection conn;
        public MSSQLLASModule()
        {
            SetTickRate(22000);
        }

        protected override List<CheckState> QueryState(CheckInfo info)
        {
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

                    var command = new SqlCommand("EXEC xp_loginconfig 'audit level';", conn);
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();


                    var record = (System.Data.IDataRecord)reader;
                    var result = record[1]?.ToString();
#if DEBUG
                if (DebugVerbose)
                    Console.WriteLine(result ?? "NO RESULT");
#endif
                    return SingleState(info.ComputeHash(result));
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

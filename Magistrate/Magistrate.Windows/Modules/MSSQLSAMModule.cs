using Magistrate.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using SimpleImpersonation;
using System.IO;
using System.Diagnostics;

namespace Magistrate.Windows.Modules
{
    internal sealed class MSSQLSAMModule : BaseModule
    {
        private bool DebugVerbose = false;
        private SqlConnection conn;
        public MSSQLSAMModule()
        {
            SetTickRate(40000);
        }

        protected override List<CheckState> QueryState(CheckInfo info)
        {
            try
            {
                return Impersonation.RunAsUser(WINDOWS_CONST.MAGISTRATECREDS, LogonType.NetworkCleartext, () =>
                {
                    if (conn == null || conn.State != System.Data.ConnectionState.Open)
                    {
                        if (conn == null)
                            conn = new SqlConnection();

                        conn.ConnectionString =
                        "Connect Timeout=3;" +
                        @"Server=CASTLE-DC\MKSSQL;" +
                        "Integrated Security=true;" +
                        "MultipleActiveResultSets=True";
                        conn.Open();
                    }

                    var command = new SqlCommand($"SELECT SERVERPROPERTY('IsIntegratedSecurityOnly');", conn);
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();


                    var record = (System.Data.IDataRecord)reader;
                    var result = record[0]?.ToString() == "1";
#if DEBUG
                if (DebugVerbose)
                    Console.WriteLine(record[0]?.ToString());
#endif

                    return SingleState(info.ComputeHash($"isintegratedsecurityonly:{result.ToString().ToLower()}"));
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

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Text;

using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace Adform.Academy.DataTransfer.Core.DataTransfer.Actions
{
    public class CreateTables : IAction
    {
        public void ExecuteAction(ExecutingProjectData data)
        {
            var srcSrvConnection = new ServerConnection(data.SrcConnection);
            var server = new Server(srcSrvConnection);
            Database database = server.Databases[srcSrvConnection.DatabaseName];
            var scripter = new Scripter(server);

            var options = new ScriptingOptions { DriAll = false, Indexes = false, ClusteredIndexes = false };
            scripter.Options = options;

            List<string> tablesToCopy = new List<string>();
            foreach (var filter in data.Project.Filters)
                tablesToCopy.Add(filter.TableName);

            UrnCollection urns = new UrnCollection();
            foreach (Table tbl in database.Tables)
                if (tablesToCopy.Contains(tbl.Name))
                    urns.Add(tbl.Urn);


            StringCollection scriptCollection = scripter.Script(urns);
            string concatedScript = String.Join(" ", scriptCollection);

            var sqlCmd = new SqlCommand(concatedScript, data.DesConnection);
            sqlCmd.ExecuteNonQuery();
        }
    }
}

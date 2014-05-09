using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Linq;
using Adform.Academy.DataTransfer.Core.DTO.Models;
using Adform.Academy.DataTransfer.Core.DTO.NHibernate;
using Adform.Academy.DataTransfer.Core.DTO.Types;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using NHibernate.Criterion;
using Database = Microsoft.SqlServer.Management.Smo.Database;

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

            var urns = new UrnCollection();
            foreach (Table tbl in database.Tables)
                if (data.Project.Filters.Any(f => f.TableName == tbl.Name))
                    urns.Add(tbl.Urn);


            StringCollection scriptCollection = scripter.Script(urns);
            string concatedScript = String.Join(Environment.NewLine, scriptCollection.Cast<string>());

            new SqlCommand(concatedScript, data.DesConnection).ExecuteNonQuery();
            string removeUnusedColumnsQuery = GetExcludeColumnsQuery(data);
            if (!String.IsNullOrWhiteSpace(removeUnusedColumnsQuery))
                new SqlCommand(removeUnusedColumnsQuery, data.DesConnection).ExecuteNonQuery();

        }

        public bool ValidateStepExecution(Project project)
        {
            return project.ExecutionState == ExecutionStepsTypes.CreatingTables;
        }

        private string GetExcludeColumnsQuery(ExecutingProjectData data)
        {

            string query = "";

            using (var idbConnection = SessionFactory.CreateIdbConnection(data.Project.DatabaseSource))
            using (var session = SessionFactory.OpenSession(idbConnection))
            {
                idbConnection.Open();

                foreach (Filter filter in data.Project.Filters)
                {
                    IList<TableStructure> tableStructList = session.CreateCriteria(typeof(TableStructure))
                        .Add(Restrictions.Eq("TableName", filter.TableName))
                        .List<TableStructure>();

                    var excludeColumns = new List<string>();
                    foreach (var rowItem in tableStructList)
                    {
                        if (filter.Columns.All(c => c.ColumnName != rowItem.ColumnName))
                        {
                            excludeColumns.Add(string.Concat("[", rowItem.ColumnName, "]"));
                        }
                    }

                    if (excludeColumns.Count != 0)
                    {
                        query = query + string.Format(
                            "ALTER TABLE {0} DROP COLUMN {1}\r\n",
                            filter.TableName,
                            String.Join(",", excludeColumns)
                        );
                    }
                }
            }

            return query;
        }
    }
}

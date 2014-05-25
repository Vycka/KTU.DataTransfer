using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using Adform.Academy.DataTransfer.Core.DataTransfer.ValueParsers;
using Adform.Academy.DataTransfer.Core.DTO.Models;
using Adform.Academy.DataTransfer.Core.DTO.Types;
using Adform.Academy.DataTransfer.Logger.Events;

namespace Adform.Academy.DataTransfer.Core.DataTransfer.Actions
{
    public class Verify : ActionBase, IAction
    {
        public void ExecuteAction(ExecutingProjectData data)
        {
            SetStep(data, ExecutionStepsTypes.Verify);

            if (data.Project.Filters.Any(f => f.Batches.Any(b => b.Checksum == null)))
            {
                ClearChecksums(data);
                data.Logger.Log(new LogEvent("Generating checksums for source database...",data.Project.ProjectId, null));
                GenerateChecksumsForCurrentStateInSource(data);
                if (data.ProjectRunner.CancelationPending)
                    return;
                data.Logger.Log(new LogEvent("Generating checksums for source database DONE", data.Project.ProjectId, null));
            }
            data.Logger.Log(new LogEvent("Verifying destination database data integrity...", data.Project.ProjectId, null));
            VerifyDestinationData(data);
            if (data.ProjectRunner.CancelationPending)
                return;
            data.Logger.Log(new LogEvent("Verifying destination database data integrity DONE", data.Project.ProjectId, null));
            SetStep(data, data.Project.Filters.All(f => f.Batches.All(b => b.BatchState == BatchStateTypes.Verified)) ? ExecutionStepsTypes.Completed : ExecutionStepsTypes.Copy);
        }

        public bool ValidateStepExecution(Project project)
        {
            return project.ExecutionState == ExecutionStepsTypes.Verify;
        }

        private void GenerateChecksumsForCurrentStateInSource(ExecutingProjectData data)
        {
            foreach (var filter in data.Project.Filters)
            {
                var parsedFilter = new FilterValueParsed(filter);

                foreach (var batch in filter.Batches)
                {
                    if (data.ProjectRunner.CancelationPending)
                        return;

                    batch.Checksum = CalculateBatchChecksum(data.SrcConnection, parsedFilter, batch);
                    data.Session.Update(batch);
                    data.Session.Flush();
                }
            }
        }

        private void VerifyDestinationData(ExecutingProjectData data)
        {
            foreach (var filter in data.Project.Filters)
            {
                var parsedFilter = new FilterValueParsed(filter);

                foreach (var batch in filter.Batches)
                {
                    if (batch.BatchState == BatchStateTypes.Verified)
                        continue;

                    if (data.ProjectRunner.CancelationPending)
                        return;
                    
                    int destinationChecksum = CalculateBatchChecksum(data.DesConnection, parsedFilter, batch);
                    batch.BatchState = batch.Checksum == destinationChecksum ? BatchStateTypes.Verified : BatchStateTypes.NotCopied;

                    if (batch.BatchState != BatchStateTypes.Verified)
                        data.Logger.Log(new LogEvent("Checksum mismatch detected on batch ID:" + batch.BatchId, data.Project.ProjectId));

                    data.Session.Update(batch);
                    data.Session.Flush();
                }
            }
        }

        private int CalculateBatchChecksum(SqlConnection sqlConnection, FilterValueParsed parsedFilter, Batch batch)
        {

            var command = sqlConnection.CreateCommand();
            command.CommandText = string.Format(
//                @"
//                    SELECT CHECKSUM(
//                        Stuff(
//                            (SELECT CHECKSUM({2}) FROM TestData1M FOR XML PATH(''),TYPE)
//                            .value('text()[1]','nvarchar(max)'),1,2,N'')
//                        )
//                    FROM [{0}] WHERE [{1}] >= @MinValue AND [{1}] < @MaxValue ORDER BY {1}
//                ",
//                @"
//                    SELECT CHECKSUM({2}) FROM [{0}] WHERE [{1}] >= @MinValue AND [{1}] < @MaxValue ORDER BY {1}
//                ",
                @"
                    SELECT SUM(CAST(CHECKSUM({2}) AS BIGINT)) FROM [{0}] WHERE [{1}] >= @MinValue AND [{1}] < @MaxValue
                ",
                parsedFilter.TableName,
                parsedFilter.IndexColumn,
                BuildChecksumSelect(parsedFilter)
            );

            command.Parameters.AddWithValue("MinValue", batch.BatchFilterMin);
            command.Parameters.AddWithValue("MaxValue", batch.BatchFilterMax);
            command.CommandTimeout = 600;


            Int64 result = 0;

            object resultObj = command.ExecuteScalar();
            if (!(resultObj is DBNull))
                result = (long) resultObj;

            return (int) result;

            //int checksum = 0;
            //using (var reader = command.ExecuteReader())
            //{
            //    if (!reader.HasRows)
            //        return 0;
            //    while (reader.Read())
            //        checksum += (int) reader[0];
            //}
            //return checksum;
        }

        private string BuildChecksumSelect(FilterValueParsed parsedFilter)
        {
            return String.Join(
                "+'|'+",
                parsedFilter.ColumnList.Select(c => string.Concat(" CAST([", c.ColumnName, "] as nvarchar)"))
            );

        }

        private void ClearChecksums(ExecutingProjectData data)
        {
            foreach (var filter in data.Project.Filters)
            {
                foreach (var batch in filter.Batches)
                {
                    batch.Checksum = null;
                    batch.BatchState = BatchStateTypes.Copied;
                }
            }
            data.Session.Update(data.Project);
        }
    }
}

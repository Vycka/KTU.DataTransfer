using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Adform.Academy.DataTransfer.Core.DataTransfer.ValueParsers;
using Adform.Academy.DataTransfer.Core.DTO.Models;
using Adform.Academy.DataTransfer.Core.DTO.Types;
using Adform.Academy.DataTransfer.Logger.Events;

namespace Adform.Academy.DataTransfer.Core.DataTransfer.Actions
{
    public class CopyData : ActionBase, IAction
    {
        private ProjectRunner _ownerProjectRunner;
        public void ExecuteAction(ExecutingProjectData data)
        {
            _ownerProjectRunner = data.ProjectRunner;

            SetStep(data, ExecutionStepsTypes.Copy);

            foreach (var filter in data.Project.Filters)
            {
                var parsedFilter = new FilterValueParsed(filter);

                //var command = data.DesConnection.CreateCommand();
                //command.CommandText = "SET IDENTITY_INSERT ["+ filter.TableName +"] ON";
                //command.ExecuteNonQuery();

                foreach (var batch in filter.Batches)
                {
                    if (data.ProjectRunner.CancelationPending)
                        return;

                    if (batch.BatchState != BatchStateTypes.NotCopied)
                        continue;

                    //bool copyFailed = false;


                    DelteBatchFromDestination(data, parsedFilter, batch);
                    CopyBatch(data, parsedFilter, batch);
                    //try
                    //{
                    //    DelteBatchFromDestination(data, parsedFilter, batch);
                    //    CopyBatch(data, parsedFilter, batch);
                    //}
                    //catch (Exception)
                    //{
                    //    copyFailed = true;
                    //}

                    //if (copyFailed)
                    //{
                    //    data.Logger.Log(new LogEvent(string.Format("Unable to transfer batch ID:{0}, deleting data from destination table matching the batch and trying to copy again",batch.BatchId)));
                    //    DelteBatchFromDestination(data, parsedFilter, batch);
                    //    CopyBatch(data, parsedFilter, batch);
                    //}
                }

                //command.CommandText = "SET IDENTITY_INSERT [" + filter.TableName + "] OFF";
                //command.ExecuteNonQuery();
            }

            SetStep(data, ExecutionStepsTypes.AppendAnalyze);
        }

        public bool ValidateStepExecution(Project project)
        {
            return project.Filters.Any(filter => filter.Batches.Any(b => b.BatchState == BatchStateTypes.NotCopied));
        }

        private void CopyBatch(ExecutingProjectData data, FilterValueParsed parsedFilter, Batch batch)
        {
            try
            {
                var srcReadCommand = data.SrcConnection.CreateCommand();
                srcReadCommand.CommandText = string.Format(
                    "SELECT {0} FROM [{1}] WHERE [{2}] >= @MinValue AND [{2}] < @MaxValue",
                    parsedFilter.ColumnsListSqlFriendly,
                    parsedFilter.TableName,
                    parsedFilter.IndexColumn
                );

                srcReadCommand.Parameters.AddWithValue("MinValue", batch.BatchFilterMin);
                srcReadCommand.Parameters.AddWithValue("MaxValue", batch.BatchFilterMax);

                using (var srcReader = srcReadCommand.ExecuteReader())
                {
                    //transaction = data.DesConnection.BeginTransaction();
                    using (var bulkCopy = new SqlBulkCopy(
                        data.DesConnection,
                        SqlBulkCopyOptions.KeepIdentity | 
                        SqlBulkCopyOptions.KeepNulls |
                        SqlBulkCopyOptions.TableLock | 
                        SqlBulkCopyOptions.UseInternalTransaction
                    , null))
                    {
                        bulkCopy.BatchSize = 50000;
                        bulkCopy.EnableStreaming = true;
                        bulkCopy.DestinationTableName = parsedFilter.TableName;
                        bulkCopy.SqlRowsCopied += bulkCopy_SqlRowsCopied;
                        bulkCopy.NotifyAfter = 50000;

                        bulkCopy.WriteToServer(srcReader);

                        //transaction.Commit();
                        batch.BatchState = BatchStateTypes.Copied;
                        batch.Checksum = null;

                        data.Session.Update(batch);
                        data.Session.Flush();
                    }
                    
                }
            }
            catch (Exception ex)
            {
                //if (transaction != null)
                //    transaction.Rollback();
                if (!(ex is OperationAbortedException))
                    throw;
            }
        }

        void bulkCopy_SqlRowsCopied(object sender, SqlRowsCopiedEventArgs e)
        {
            if (_ownerProjectRunner.CancelationPending)
                e.Abort = true;
        }

        private void DelteBatchFromDestination(ExecutingProjectData data, FilterValueParsed parsedFilter, Batch batch)
        {
            var sqlCommand = BuildDeleteCommand(data.DesConnection, parsedFilter.TableName, parsedFilter.IndexColumn, batch);
            sqlCommand.ExecuteNonQuery();
        }


        private SqlCommand BuildDeleteCommand(SqlConnection connection, string tableName, string indexField, Batch batch)
        {
            var command = connection.CreateCommand();
            command.CommandText = string.Format(
                "DELETE FROM [{0}] WHERE [{1}] >= @MinValue AND [{1}] < @MaxValue",
                tableName,
                indexField
            );

            command.Parameters.AddWithValue("MinValue", batch.BatchFilterMin);
            command.Parameters.AddWithValue("MaxValue", batch.BatchFilterMax);

            return command;
        }

    }

}

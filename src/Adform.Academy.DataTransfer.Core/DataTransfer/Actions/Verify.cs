using System.Data.SqlClient;
using System.Linq;
using Adform.Academy.DataTransfer.Core.DataTransfer.ValueParsers;
using Adform.Academy.DataTransfer.Core.DTO.Models;
using Adform.Academy.DataTransfer.Core.DTO.Types;

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
                GenerateChecksumsForCurrentStateInSource(data);
            }

            VerifyDestinationData(data);

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
                    batch.Checksum = CalculateBatchChecksum(data.SrcConnection, parsedFilter, batch);
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
                    
                    int destinationChecksum = CalculateBatchChecksum(data.DesConnection, parsedFilter, batch);
                    batch.BatchState = batch.Checksum == destinationChecksum ? BatchStateTypes.Verified : BatchStateTypes.NotCopied;

                    data.Session.Update(batch);
                }
            }
        }

        private int CalculateBatchChecksum(SqlConnection sqlConnection, FilterValueParsed parsedFilter, Batch batch)
        {
            //TODO Get checksum
            return 0;
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

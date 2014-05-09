using System;
using Adform.Academy.DataTransfer.Core.DTO.Models;
using Adform.Academy.DataTransfer.Core.DTO.Types;

namespace Adform.Academy.DataTransfer.Core.DataTransfer.Actions
{
    public class Verify : ActionBase, IAction
    {
        public void ExecuteAction(ExecutingProjectData data)
        {
            SetStep(data, ExecutionStepsTypes.Verify);

            //do some mad verification..
            foreach (var filter in data.Project.Filters)
            {
                foreach (var batch in filter.Batches)
                {
                    batch.BatchState = BatchStateTypes.Verified;
                }
            }
            SetStep(data, ExecutionStepsTypes.Completed);
        }

        public bool ValidateStepExecution(Project project)
        {
            return project.ExecutionState == ExecutionStepsTypes.Verify;
        }
    }
}

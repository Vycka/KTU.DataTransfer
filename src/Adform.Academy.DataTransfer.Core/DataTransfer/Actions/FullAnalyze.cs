using Adform.Academy.DataTransfer.Core.DataTransfer.ValueParsers;
using Adform.Academy.DataTransfer.Core.DTO.Models;
using Adform.Academy.DataTransfer.Core.DTO.Types;

namespace Adform.Academy.DataTransfer.Core.DataTransfer.Actions
{
    public class FullAnalyze : AnalyzeBase, IAction
    {
        public void ExecuteAction(ExecutingProjectData data)
        {
            SetStep(data, ExecutionStepsTypes.FullAnalyze);

            foreach (var filter in data.Project.Filters)
            {
                var parsedFilter = new FilterValueParsed(filter);
                var minMax = GetMinMaxValues(data.SrcConnection, parsedFilter);
                filter.Batches.Clear();
                GenerateBatchesAsc(minMax, filter, parsedFilter);
            }

            SetStep(data, ExecutionStepsTypes.CreatingTables);
        }


        public bool ValidateStepExecution(Project project)
        {
            return (project.ExecutionState == ExecutionStepsTypes.FullAnalyze ||
                    project.ExecutionState == ExecutionStepsTypes.NotStarted ||
                    project.ExecutionState == ExecutionStepsTypes.Canceled);
        }

    }
}

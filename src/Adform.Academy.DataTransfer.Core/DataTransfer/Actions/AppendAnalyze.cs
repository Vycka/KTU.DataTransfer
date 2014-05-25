using System.Linq;
using Adform.Academy.DataTransfer.Core.DataTransfer.ValueParsers;
using Adform.Academy.DataTransfer.Core.DTO.Models;
using Adform.Academy.DataTransfer.Core.DTO.Types;

namespace Adform.Academy.DataTransfer.Core.DataTransfer.Actions
{
    public class AppendAnalyze : AnalyzeBase, IAction
    {
        public void ExecuteAction(ExecutingProjectData data)
        {
            SetStep(data, ExecutionStepsTypes.AppendAnalyze);

            foreach (var filter in data.Project.Filters)
            {
                var parsedFilter = new FilterValueParsed(filter);

                var currentMinMax = GetCurrentMinMaxFromBatches(filter, parsedFilter.IndexColumnType);
                var newMinMax = GetMinMaxValues(data.SrcConnection, parsedFilter);


                var descMinMax = new MinMaxItems
                {
                    MinValue = newMinMax.MinValue,
                    MaxValue = currentMinMax.MinValue
                };

                var ascMinMax = new MinMaxItems
                {
                    MinValue = currentMinMax.MaxValue,
                    MaxValue = newMinMax.MaxValue
                };

                GenerateBatchesDesc(descMinMax, filter, parsedFilter);
                GenerateBatchesAsc(ascMinMax, filter, parsedFilter);
            }

            SetStep(data, CanAdvanceToNextAction(data.Project) ? ExecutionStepsTypes.Verify : ExecutionStepsTypes.Copy);
        }

        public bool ValidateStepExecution(Project project)
        {
            return CanAdvanceToNextAction(project);
            //return project.ExecutionState == ExecutionStepsTypes.AppendAnalyze;
        }

        private bool CanAdvanceToNextAction(Project project)
        {
            return project.Filters.All(filter => filter.Batches.All(b => b.BatchState == BatchStateTypes.Copied || b.BatchState == BatchStateTypes.Verified));
        }
    }
}

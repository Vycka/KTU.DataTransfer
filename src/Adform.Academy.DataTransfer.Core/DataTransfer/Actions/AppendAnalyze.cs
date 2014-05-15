using System;
using System.Data.SqlClient;
using System.Linq;
using Adform.Academy.DataTransfer.Core.DataTransfer.ValueParsers;
using Adform.Academy.DataTransfer.Core.DTO.Models;
using Adform.Academy.DataTransfer.Core.DTO.Types;

namespace Adform.Academy.DataTransfer.Core.DataTransfer.Actions
{
    public class AppendAnalyze : ActionBase, IAction
    {
        public void ExecuteAction(ExecutingProjectData data)
        {
            SetStep(data, ExecutionStepsTypes.AppendAnalyze);



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

        //private MinMaxItems GetCurrentMinMax()
        //{
        //    var MaxBatchj
        //}

        //private struct MinMaxItems
        //{
        //    public object MinValue;
        //    public object MaxValue;
        //}
    }
}

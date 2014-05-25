using Adform.Academy.DataTransfer.Core.DTO.Types;
using Adform.Academy.DataTransfer.Logger.Events;

namespace Adform.Academy.DataTransfer.Core.DataTransfer.Actions
{
    public class ActionBase
    {
        protected void SetStep(ExecutingProjectData data, ExecutionStepsTypes step)
        {
            if (data.Project.ExecutionState == step)
                return;

            data.Logger.Log(new LogEvent(string.Format("Execution action changed: {0}", step), data.Project.ProjectId));
            data.Project.ExecutionState = step;

            data.Session.Merge(data.Project);
            data.Session.Flush();
        }
    }
}

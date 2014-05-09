using Adform.Academy.DataTransfer.Core.DTO.Models;

namespace Adform.Academy.DataTransfer.Core.DataTransfer.Actions
{
    interface IAction
    {
        void ExecuteAction(ExecutingProjectData data);
        bool ValidateStepExecution(Project project);
    }
}

using Adform.Academy.DataTransfer.Core.DTO.Models;

namespace Adform.Academy.DataTransfer.Logger.Events
{
    public class ProjectDeleted : LogEvent
    {
        public ProjectDeleted(Project project, int userId) : base("", null, userId)
        {
            Message = string.Format("Deleted project: [{0}]", project.ProjectId);
        }
    }
}

using Adform.Academy.DataTransfer.Core.DTO.Models;

namespace Adform.Academy.DataTransfer.Logger.Events
{
    public class ProjectModified : LogEvent
    {
        public ProjectModified(Project project, int userId) : base("Modified project", project.ProjectId, userId)
        {

        }
    }
}

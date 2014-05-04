using Adform.Academy.DataTransfer.Core.DTO.Models;

namespace Adform.Academy.DataTransfer.Logger.Events
{
    public class ProjectCreatedEvent : LogEvent
    {
        public ProjectCreatedEvent(Project project, int userId) : base("Created project", project.ProjectId, userId)
        {
        }
    }
}

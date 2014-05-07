namespace Adform.Academy.DataTransfer.Logger.Events
{
    public class ProjectCreatedEvent : LogEvent
    {
        public ProjectCreatedEvent(int projectId, int userId) : base("Created project", projectId, userId)
        {
        }
    }
}

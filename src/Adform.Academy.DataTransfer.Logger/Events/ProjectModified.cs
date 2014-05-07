

namespace Adform.Academy.DataTransfer.Logger.Events
{
    public class ProjectModified : LogEvent
    {
        public ProjectModified(int projectId, int userId) : base("Modified project", projectId, userId)
        {

        }
    }
}

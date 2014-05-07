
namespace Adform.Academy.DataTransfer.Logger.Events
{
    public class ProjectDeleted : LogEvent
    {
        public ProjectDeleted(int projectId, int userId) : base("", null, userId)
        {
            Message = string.Format("Deleted project: [{0}]", projectId);
        }
    }
}

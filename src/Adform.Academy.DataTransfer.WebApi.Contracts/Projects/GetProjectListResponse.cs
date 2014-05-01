using System.Collections.Generic;
using Adform.Academy.DataTransfer.WebApi.Contracts.Projects.Types;

namespace Adform.Academy.DataTransfer.WebApi.Contracts.Projects
{
    public class GetProjectListResponse
    {
        public List<ProjectListItem> Projects;
    }

    public struct ProjectListItem
    {
        public int ProjectId;
        public string Name;
        public ExecutionStepsTypes ExecutionStep;
        public ProjectStateTypes ProjectState;
        public string CreatedByUserName;
        //public int? PendingUserAction;
    }
}

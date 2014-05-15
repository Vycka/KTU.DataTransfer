using System.Collections.Generic;

namespace Adform.Academy.DataTransfer.WebApi.Contracts.Projects
{
    public class GetProjectProgressResponse : ResponseBase
    {
        public List<ProjectStateItem> StateItems;
    }

    public struct ProjectStateItem
    {
        public int StateId;
        public int Count;
    }
}

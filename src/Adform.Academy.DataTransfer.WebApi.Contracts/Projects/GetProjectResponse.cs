using System.Collections.Generic;

namespace Adform.Academy.DataTransfer.WebApi.Contracts.Projects
{
    public class GetProjectResponse : ResponseBase
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int SourceDatabaseId { get; set; }
        public int DestinationDatabaseId { get; set; }
        public List<FilterItem> Filters { get; set; }
    }
}

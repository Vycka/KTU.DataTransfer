namespace Adform.Academy.DataTransfer.WebApi.Contracts.Projects
{
    public class GetProjectListRequest : RequestBase
    {
        public int? CreatedByUserId;
        public bool ShowArchived;
    }
}

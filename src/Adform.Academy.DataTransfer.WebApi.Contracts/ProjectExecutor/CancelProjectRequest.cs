namespace Adform.Academy.DataTransfer.WebApi.Contracts.ProjectExecutor
{
    public class CancelProjectRequest : RequestBase
    {
        public int ProjectId { get; set; }
    }
}

namespace Adform.Academy.DataTransfer.WebApi.Contracts.Logs
{
    public class GetLogsRequest : RequestBase
    {
        public int? ProjectId;
        public int BeginFromId;
    }
}

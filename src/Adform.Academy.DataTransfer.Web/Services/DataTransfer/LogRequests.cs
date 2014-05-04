using Adform.Academy.DataTransfer.WebApi.Contracts.Logs;
using Newtonsoft.Json;

namespace Adform.Academy.DataTransfer.Web.Services.DataTransfer
{
    static public class LogRequests
    {
        public static GetLogsResponse Get(int? projectId, int beginFromId = 0)
        {
            var request = new GetLogsRequest()
            {
                BeginFromId = beginFromId,
                ProjectId = projectId
            };

            string responseString = ServiceClient.PostRequest("Logs/Get", request);
            var response = JsonConvert.DeserializeObject<GetLogsResponse>(responseString);
            return response;
        }
    }
}
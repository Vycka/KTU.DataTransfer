using Adform.Academy.DataTransfer.WebApi.Contracts.ProjectExecutor;
using Newtonsoft.Json;

namespace Adform.Academy.DataTransfer.Web.Services.DataTransfer
{
    public class ProjectExecutorRequests
    {
        public static StartResponse Start(int projectId)
        {
            var request = new StartRequest
            {
                ProjectId = projectId
            };
            string responseString = ServiceClient.PostRequest("ProjectExecutor/Start", request);
            var response = JsonConvert.DeserializeObject<StartResponse>(responseString);
            return response;
        }

        public static ContinueResponse Continue(int projectId)
        {
            var request = new ContinueRequest
            {
                ProjectId = projectId
            };
            string responseString = ServiceClient.PostRequest("ProjectExecutor/Continue", request);
            var response = JsonConvert.DeserializeObject<ContinueResponse>(responseString);
            return response;
        }

        public static ArchiveResponse Archive(int projectId)
        {
            var request = new ArchiveRequest
            {
                ProjectId = projectId
            };
            string responseString = ServiceClient.PostRequest("ProjectExecutor/Archive", request);
            var response = JsonConvert.DeserializeObject<ArchiveResponse>(responseString);
            return response;
        }

        public static DeleteResponse Delete(int projectId)
        {
            var request = new DeleteRequest
            {
                ProjectId = projectId
            };
            string responseString = ServiceClient.PostRequest("ProjectExecutor/Delete", request);
            var response = JsonConvert.DeserializeObject<DeleteResponse>(responseString);
            return response;
        }
    }
}
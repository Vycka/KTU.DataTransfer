using Adform.Academy.DataTransfer.Web.Models;
using Adform.Academy.DataTransfer.WebApi.Contracts.Projects;
using Newtonsoft.Json;

namespace Adform.Academy.DataTransfer.Web.Services.DataTransfer
{
    public class ProjectRequests
    {
        public static GetProjectListResponse GetProjectList(int? createdByUserId)
        {
            var request = new GetProjectListRequest {CreatedByUserId = createdByUserId};
            string responseString = ServiceClient.PostRequest("Projects/GetProjectList", request);
            var response = JsonConvert.DeserializeObject<GetProjectListResponse>(responseString);
            return response;
        }

        public static SaveProjectResponse Save(ProjectEditorModel model)
        {
            var request = new SaveProjectRequest
            {
                ProjectId = model.ProjectId,
                ProjectName = model.ProjectName,
                SourceDatabaseId = model.SourceDatabaseId,
                DestinationDatabaseId = model.DestinationDatabaseId,
                Filters = model.Filters
            };

            string responseString = ServiceClient.PostRequest("Projects/Save", request);
            var response = JsonConvert.DeserializeObject<SaveProjectResponse>(responseString);
            return response;
        }
    }
}
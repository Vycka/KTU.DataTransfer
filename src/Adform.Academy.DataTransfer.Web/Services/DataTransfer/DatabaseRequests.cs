using Adform.Academy.DataTransfer.Web.Models;
using Adform.Academy.DataTransfer.WebApi.Contracts.Databases;
using Newtonsoft.Json;

namespace Adform.Academy.DataTransfer.Web.Services.DataTransfer
{
    public class DatabaseRequests
    {
        public static GetDatabasesListResponse GetDatabasesList()
        {
            string responseString = ServiceClient.PostRequest("Databases/GetDatabasesList", new GetDatabasesListRequest());
            var response = JsonConvert.DeserializeObject<GetDatabasesListResponse>(responseString);
            return response;
        }

        public static GetDatabaseResponse GetDatabase(int databaseId)
        {
            string responseString = ServiceClient.PostRequest("Databases/Get", new GetDatabaseRequest { DatabaseId = databaseId});
            var response = JsonConvert.DeserializeObject<GetDatabaseResponse>(responseString);
            return response;
        }

        public static SaveDatabaseResponse SaveDatabase(DatabaseItemModel database)
        {
            var request = new SaveDatabaseRequest
            {
                    DatabaseId = database.DatabaseId,
                    ConnectionName = database.ConnectionName,
                    Host = database.Host,
                    Port = database.Port,
                    UserName = database.UserName,
                    Password = database.Password,
                    DatabaseName = database.DatabaseName
            };

            string responseString = ServiceClient.PostRequest("Databases/Save", request);
            var response = JsonConvert.DeserializeObject<SaveDatabaseResponse>(responseString);
            return response;
        }

        public static DeleteDatabaseResponse Delete(int databaseId)
        {
            string responseString = ServiceClient.PostRequest("Databases/Delete", new DeleteDatabaseRequest { DatabaseId = databaseId });
            var response = JsonConvert.DeserializeObject<DeleteDatabaseResponse>(responseString);
            return response;
        }

        public static GetDatabaseStructureResponse GetDatabaseStructure(int databaseId)
        {
            string responseString = ServiceClient.PostRequest(
                "Databases/GetDatabaseStructure",
                new GetDatabaseStructureRequest
                {
                    DatabaseId = databaseId
                }
            );
            var response = JsonConvert.DeserializeObject<GetDatabaseStructureResponse>(responseString);
            return response;
        }
    }
}
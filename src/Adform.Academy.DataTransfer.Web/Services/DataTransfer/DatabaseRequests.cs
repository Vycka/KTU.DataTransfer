using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adform.Academy.DataTransfer.WebApi.Contracts.Databases;
using Newtonsoft.Json;

namespace Adform.Academy.DataTransfer.Web.Services.DataTransfer
{
    public class DatabaseRequests
    {
        public static GetDatabasesListResponse GetDatabasesList()
        {
            string responseString = ServiceClient.PostRequest("Databases/GetDatabasesList", new GetDatabasesListRequest());
            GetDatabasesListResponse response = JsonConvert.DeserializeObject<GetDatabasesListResponse>(responseString);
            return response;
        }

        public static DeleteDatabaseResponse Delete(int databaseId)
        {
            string responseString = ServiceClient.PostRequest("Databases/Delete", new DeleteDatabaseRequest { DatabaseId = databaseId});
            DeleteDatabaseResponse response = JsonConvert.DeserializeObject<DeleteDatabaseResponse>(responseString);
            return response;
        }
    }
}
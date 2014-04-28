using System.Collections.Generic;

namespace Adform.Academy.DataTransfer.WebApi.Contracts.Databases
{
    public class GetDatabasesListResponse : ResponseBase
    {
        public List<DatabaseItem> DatabasesList;
    }

    public struct DatabaseItem
    {
        public string ConnectionName;
        public int DatabaseId;
    }
}

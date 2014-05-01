using System.Collections.Generic;

namespace Adform.Academy.DataTransfer.WebApi.Contracts.Databases
{
    public class GetDatabaseStructureResponse : ResponseBase
    {
        public List<TableInformation> Tables;
    }
    public class TableInformation
    {
        public TableInformation()
        {
            Fields = new List<FieldInformation>();
        }

        public string TableName;
        public List<FieldInformation> Fields;
    }

    public class FieldInformation
    {
        public string FieldName;
        public string FieldType;
    }
}

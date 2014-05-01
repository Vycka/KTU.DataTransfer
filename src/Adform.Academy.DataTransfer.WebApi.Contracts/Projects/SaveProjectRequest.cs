using System.Collections.Generic;

namespace Adform.Academy.DataTransfer.WebApi.Contracts.Projects
{
    public class SaveProjectRequest : RequestBase
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int SourceDatabaseId { get; set; }
        public int DestinationDatabaseId { get; set; }
        public List<FilterItem> Filters { get; set; }
    }

    public class FilterItem
    {
        public FilterValueItem FilterValue;

        public string TableName;
        public List<ColumnItem> Columns;
    }

    public class FilterValueItem
    {
        public string IndexColumn;
        public string IndexStepName;
        public string IndexStep;
    }

    public class ColumnItem
    {
        public string ColumnName;
        public string ColumnType;
    }
}

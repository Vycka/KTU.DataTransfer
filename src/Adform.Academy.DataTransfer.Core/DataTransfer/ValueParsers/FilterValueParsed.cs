using System;
using System.Collections.Generic;
using System.Linq;
using Adform.Academy.DataTransfer.Core.DTO.Models;
using Newtonsoft.Json;
using Column = Adform.Academy.DataTransfer.Core.DTO.Models.Column;

namespace Adform.Academy.DataTransfer.Core.DataTransfer.ValueParsers
{

    public class FilterValueParsed
    {
        public FilterValueParsed(Filter filter)
        {
            var parsedFilter = JsonConvert.DeserializeObject<FilterValue>(filter.FilterValue);

            IndexColumn = parsedFilter.IndexColumn;
            IndexStepName = parsedFilter.IndexStepName;
            IndexStep = parsedFilter.IndexStep;
            IndexColumnType = parsedFilter.IndexColumnType;

            TableName = filter.TableName;
            ColumnsListSqlFriendly = String.Join(",", filter.Columns.Select(c => string.Concat("[", c.ColumnName, "]")));
            ColumnList = filter.Columns;
        }

        public string IndexColumn;
        public string IndexStepName;
        public string IndexStep;
        public string IndexColumnType;
        public string TableName;
        public string ColumnsListSqlFriendly;
        public IEnumerable<Column> ColumnList;
    }

}

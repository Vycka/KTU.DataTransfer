using System.Linq;
using Adform.Academy.DataTransfer.Core.DTO.Models;
using Microsoft.SqlServer.Management.Smo;
using Newtonsoft.Json;

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
        }

        public string IndexColumn;
        public string IndexStepName;
        public string IndexStep;
        public string IndexColumnType;
        public string TableName;
    }

}

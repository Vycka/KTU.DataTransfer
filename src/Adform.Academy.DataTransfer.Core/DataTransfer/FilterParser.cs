using System.Linq;
using Adform.Academy.DataTransfer.Core.DTO.Models;
using Newtonsoft.Json;

namespace Adform.Academy.DataTransfer.Core.DataTransfer
{
    public class FilterParser
    {
        private readonly Filter _filter;
        private FilterValue _parsedFilter;
        private string _indexFieldType;

        public FilterParser(Filter filter)
        {
            _filter = filter;
            _parsedFilter = JsonConvert.DeserializeObject<FilterValue>(filter.FilterValue);

            _indexFieldType = filter.Columns.First(c => c.ColumnName == _parsedFilter.IndexColumn).ColumnType;
        }
    }

}

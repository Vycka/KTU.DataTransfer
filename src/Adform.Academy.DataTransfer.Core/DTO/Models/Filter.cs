using System.Collections.Generic;
using Newtonsoft.Json;

namespace Adform.Academy.DataTransfer.Core.DTO.Models
{
    public class Filter
    {
        public virtual int FilterId { get; set; }
        public virtual Project Project { get; set; }

        public virtual string FilterValue { get; set; }
        public virtual string TableName { get; set; }
        public virtual IList<Column> Columns { get; set; }
        public virtual IList<Batch> Batches { get; set; }
    }

    public struct FilterValue
    {
        public string IndexColumn;
        public string IndexStepName;
        public string IndexStep;
    }
}

using Adform.Academy.DataTransfer.Core.DTO.Types;

namespace Adform.Academy.DataTransfer.Core.DTO.Models
{
    public class Batch
    {
        public virtual int BatchId { get; set; }
        public virtual Filter Filter { get; set; }
        public virtual string BatchFilterMin { get; set; }
        public virtual string BatchFilterMax { get; set; }
        public virtual BatchStateTypes BatchState { get; set; }

        public virtual int? Checksum { get; set; }
    }
}

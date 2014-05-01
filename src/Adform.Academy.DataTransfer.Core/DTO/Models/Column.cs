namespace Adform.Academy.DataTransfer.Core.DTO.Models
{
    public class Column
    {
        public virtual int ColumnId { get; set; }
        public virtual Filter Filter { get; set; }
        public virtual string ColumnName { get; set; }

        public virtual string ColumnType { get; set; }
    }
}

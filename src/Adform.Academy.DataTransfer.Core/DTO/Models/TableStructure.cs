namespace Adform.Academy.DataTransfer.Core.DTO.Models
{
    public class TableStructure
    {
        public virtual string TableName { get; set; }
        public virtual string ColumnName { get; set; }
        public virtual string DataType { get; set; }

        public override bool Equals(object o)
        {
            var other = o as TableStructure;
            if (other == null)
                return false;
            return (TableName == other.TableName && ColumnName == other.ColumnName);
        }

        public override int GetHashCode()
        {
            return (TableName + "|" + ColumnName).GetHashCode();
        }
    }
}

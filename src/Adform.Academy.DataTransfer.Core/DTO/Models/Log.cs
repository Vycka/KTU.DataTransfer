namespace Adform.Academy.DataTransfer.Core.DTO.Models
{
    public class Log
    {
        public virtual int LogId { get; set; }
        public virtual Project Project { get; set; }
        public virtual string LogMessage { get; set; }
    }
}

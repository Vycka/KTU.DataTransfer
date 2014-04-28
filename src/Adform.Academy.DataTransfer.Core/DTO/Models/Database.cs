using System;
using System.ComponentModel;

namespace Adform.Academy.DataTransfer.Core.DTO.Models 
{
    public class Database : IEquatable<Database>
    {
        public virtual int DatabaseId { get; set; }
        public virtual string Host { get; set; }
        public virtual string Port { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Password { get; set; }
        public virtual string DatabaseName { get; set; }
        public virtual string ConnectionName { get; set; }
        public virtual bool Equals(Database other)
        {
            return DatabaseId == other.DatabaseId;
        }
    }
}

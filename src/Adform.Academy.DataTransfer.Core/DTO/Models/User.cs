using System;

namespace Adform.Academy.DataTransfer.Core.DTO.Models
{
    public class User : IEquatable<User>
    {
        public virtual int UserId { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Password { get; set; }
        public virtual bool IsAdmin { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual bool Equals(User other)
        {
            return UserId == other.UserId;
        }
    }
}

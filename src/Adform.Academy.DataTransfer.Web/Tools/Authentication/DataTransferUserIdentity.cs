using System.Security.Principal;

namespace Adform.Academy.DataTransfer.Web.Tools.Authentication
{
    public class DataTransferUserIdentity : GenericIdentity
    {
        public int UserId { get; private set; }
        public DataTransferUserIdentity(string name, int userId) : base(name)
        {
            UserId = userId;
        }
    }

}
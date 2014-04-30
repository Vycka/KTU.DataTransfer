using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

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
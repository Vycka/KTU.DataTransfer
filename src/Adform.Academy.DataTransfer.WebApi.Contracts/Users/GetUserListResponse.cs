using System.Collections.Generic;

namespace Adform.Academy.DataTransfer.WebApi.Contracts.Users
{
    public class GetUserListResponse
    {
        public List<UserListItem> Users;
    }
    public struct UserListItem
    {
        public int UserId;
        public string UserName;
        public bool IsActive;
        public bool IsAdmin;
    }
}

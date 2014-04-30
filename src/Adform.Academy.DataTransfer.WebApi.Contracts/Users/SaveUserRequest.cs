namespace Adform.Academy.DataTransfer.WebApi.Contracts.Users
{
    public class SaveUserRequest : RequestBase
    {
        public int UserId;
        public string UserName;
        public string Password;
        public bool IsActive;
        public bool IsAdmin;
    }
}

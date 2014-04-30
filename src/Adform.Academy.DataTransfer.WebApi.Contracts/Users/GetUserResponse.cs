namespace Adform.Academy.DataTransfer.WebApi.Contracts.Users
{
    public class GetUserResponse : ResponseBase
    {
        public int UserId;
        public string UserName;
        public bool IsActive;
        public bool IsAdmin;
    }
}

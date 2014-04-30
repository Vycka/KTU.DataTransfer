namespace Adform.Academy.DataTransfer.WebApi.Contracts.Users
{
    public class CheckLoginResponse : ResponseBase
    {
        public string UserName;
        public int UserId;
        public bool IsActive;
        public bool IsAdmin;
    }
}

namespace Adform.Academy.DataTransfer.WebApi.Contracts.Users
{
    public class CheckLoginRequest : RequestBase
    {
        public string UserName;
        public string Password;
    }
}

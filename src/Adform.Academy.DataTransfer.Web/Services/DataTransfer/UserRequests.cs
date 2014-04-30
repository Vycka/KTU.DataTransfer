using Adform.Academy.DataTransfer.Web.Models;
using Adform.Academy.DataTransfer.WebApi.Contracts.Users;
using Newtonsoft.Json;

namespace Adform.Academy.DataTransfer.Web.Services.DataTransfer
{
    public class UserRequests
    {
        public static GetUserListResponse GetUserList()
        {
            string responseString = ServiceClient.PostRequest("Users/GetUserList", new GetUserListRequest());
            var response = JsonConvert.DeserializeObject<GetUserListResponse>(responseString);
            return response;
        }

        public static GetUserResponse GetUser(int userId)
        {
            string responseString = ServiceClient.PostRequest("Users/Get", new GetUserRequest() { UserId =  userId });
            var response = JsonConvert.DeserializeObject<GetUserResponse>(responseString);
            return response;
        }

        public static SaveUserResponse SaveUser(UserItemModel user)
        {
            var request = new SaveUserRequest
            {
                IsActive = user.IsActive,
                IsAdmin = user.IsAdmin,
                Password = user.Password ?? string.Empty,
                UserName = user.UserName,
                UserId = user.UserId
            };
            string responseString = ServiceClient.PostRequest("Users/Save", request);
            var response = JsonConvert.DeserializeObject<SaveUserResponse>(responseString);
            return response;
        }

        public static CheckLoginResponse CheckUser(string userName, string password)
        {
            var request = new CheckLoginRequest
            {
                Password = password,
                UserName = userName
            };

            string responseString = ServiceClient.PostRequest("Users/CheckLogin", request);
            var response = JsonConvert.DeserializeObject<CheckLoginResponse>(responseString);
            return response;
        }
    }
}
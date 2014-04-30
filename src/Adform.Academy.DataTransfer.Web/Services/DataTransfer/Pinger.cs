using Adform.Academy.DataTransfer.WebApi.Contracts;

namespace Adform.Academy.DataTransfer.Web.Services.DataTransfer
{
    public class Pinger
    {
        public static string Ping()
        {
            return ServiceClient.PostRequest("Ping", new RequestBase());
        }

        public static string PingError()
        {
            return ServiceClient.PostRequest("Ping3", new RequestBase());
        }
    }
}
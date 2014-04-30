using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Adform.Academy.DataTransfer.Web.Tools.Authentication;
using Adform.Academy.DataTransfer.WebApi.Contracts;

namespace Adform.Academy.DataTransfer.Web.Services.DataTransfer
{

    public class ServiceClient
    {
        private static HttpClient CreateClient()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(ConfigurationManager.AppSettings["DataTransferServiceUrl"])
            };

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        private static string PostAsyncRequest(string relativeUrl, RequestBase request)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {

                var identity = HttpContext.Current.User.Identity as DataTransferUserIdentity;
                if (identity != null)
                {
                    request.InvokerUserName = identity.Name;
                    request.InvokerUserId = identity.UserId;
                }
                else
                {
                    request.InvokerUserName = "System";
                    request.InvokerUserId = -1;
                }
            }

            using (HttpClient client = CreateClient())
            using (Task<HttpResponseMessage> repsonse = client.PostAsJsonAsync("Adform.Academy.DataTransfer/v1/" + relativeUrl, request))
            {
                repsonse.Result.EnsureSuccessStatusCode();
                Task<string> responseString = repsonse.Result.Content.ReadAsStringAsync();
                return responseString.Result;
            }
        }

        public static string PostRequest(string relativeUrl, RequestBase request)
        {
            var response = PostAsyncRequest(relativeUrl, request);
            return response;
        }

    }
}
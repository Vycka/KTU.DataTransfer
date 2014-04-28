using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

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


        private static async Task<string> ExtractResponse(Task<HttpResponseMessage> response)
        {
            string responseString = await response.Result.Content.ReadAsStringAsync();
            return responseString;

        }
        private static string PostAsyncRequest(string relativeUrl, object request)
        {
            using (HttpClient client = CreateClient())
            using (Task<HttpResponseMessage> repsonse = client.PostAsJsonAsync("Adform.Academy.DataTransfer/v1/" + relativeUrl, request))
            {
                repsonse.Result.EnsureSuccessStatusCode();
                Task<string> responseString = repsonse.Result.Content.ReadAsStringAsync();
                return responseString.Result;
            }
        }

        public static string PostRequest(string relativeUrl, object request)
        {
            var response = PostAsyncRequest(relativeUrl, request);
            return response;
        }

    }
}
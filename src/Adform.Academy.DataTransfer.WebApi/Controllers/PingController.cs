using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Adform.Academy.DataTransfer.WebApi.Controllers
{

    [RoutePrefix("Adform.Academy.DataTransfer/v1")]
    public class PingController : ApiController
    {
        [Route("Ping")]
        [HttpGet]
        public string Ping()
        {
            return "PONG";
        }

        [Route("Ping2")]
        [HttpGet]
        public HttpResponseMessage Ping2()
        {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}

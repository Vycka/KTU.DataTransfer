using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Adform.Academy.DataTransfer.WebApi.Controllers
{

    [RoutePrefix("Adform.Academy.DataTransfer/v1")]
    public class PingController : ControllerBase
    {
        [Route("Ping")]
        [HttpGet, HttpPost]
        public string Ping()
        {
            return "PONG";
        }

        [Route("Ping2")]
        [HttpGet, HttpPost]
        public HttpResponseMessage Ping2()
        {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [Route("Ping3")]
        [HttpGet, HttpPost]
        public HttpResponseMessage Ping3()
        {
            return new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }
    }
}

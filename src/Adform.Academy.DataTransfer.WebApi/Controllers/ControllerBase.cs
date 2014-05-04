using System.Web.Http;
using Adform.Academy.DataTransfer.Logger;

namespace Adform.Academy.DataTransfer.WebApi.Controllers
{
    public class ControllerBase : ApiController
    {
        protected ILogger Logger = new Log4NetLogger("DataTransferService");
    }
}

using Adform.Academy.DataTransfer.Core.DataTransfer;
using Adform.Academy.DataTransfer.Logger;

namespace Adform.Academy.DataTransfer.WebApi.Controllers
{
    public class ControllerBase : System.Web.Http.ApiController
    {
        public static ILogger Logger;
        public static DataTransferServiceRunner ServiceRunner;
    }
}

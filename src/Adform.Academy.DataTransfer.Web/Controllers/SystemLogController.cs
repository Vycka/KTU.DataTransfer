using System.Web.Mvc;
using Adform.Academy.DataTransfer.Web.Models;
using Adform.Academy.DataTransfer.Web.Services.DataTransfer;

namespace Adform.Academy.DataTransfer.Web.Controllers
{
    public class SystemLogController : Controller
    {
        //
        // GET: /SystemLog/
        public ActionResult Index(SystemLogModel model)
        {
            return View(model);
        }

        public JsonResult Get(SystemLogModel model)
        {
            return Json(LogRequests.Get(model.ProjectId, model.BeginFromId));
        }
	}
}
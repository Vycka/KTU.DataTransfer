using System.Web.Mvc;
using Adform.Academy.DataTransfer.Web.Services.DataTransfer;

namespace Adform.Academy.DataTransfer.Web.Controllers
{
    public class ProjectExecutorController : Controller
    {
        [Authorize]
        public JsonResult Start(int id)
        {
            return Json(ProjectExecutorRequests.Start(id));
        }

        [Authorize]
        public JsonResult Continue(int id)
        {
            return Json(ProjectExecutorRequests.Continue(id));
        }

        [Authorize]
        public JsonResult Pause(int id)
        {
            return Json(ProjectExecutorRequests.Pause(id));
        }

        [Authorize]
        public JsonResult Cancel(int id)
        {
            return Json(ProjectExecutorRequests.Cancel(id));
        }

        [Authorize]
        public JsonResult Archive(int id)
        {
            return Json(ProjectExecutorRequests.Archive(id));
        }

        [Authorize(Roles = "admin")]
        public JsonResult Delete(int id)
        {
            return Json(ProjectExecutorRequests.Delete(id));
        }
	}
}
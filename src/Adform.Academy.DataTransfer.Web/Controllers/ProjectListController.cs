using System.Web.Mvc;
using Adform.Academy.DataTransfer.Web.Models;
using Adform.Academy.DataTransfer.Web.Services.DataTransfer;
using Adform.Academy.DataTransfer.Web.Tools.Authentication;

namespace Adform.Academy.DataTransfer.Web.Controllers
{
    public class ProjectListController : Controller
    {
        //
        // GET: /ProjectList/

        [Authorize]
        public ActionResult Index()
        {
            var response = ProjectRequests.GetProjectList(Principal.UserId);
            var model = new ProjectListModel
            {
                Projects = response.Projects,
                ShowCreator = false,
                ShowProjectsAll = false
                
            };
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public ActionResult All()
        {
            var response = ProjectRequests.GetProjectList(null,true);
            var model = new ProjectListModel
            {
                Projects = response.Projects,
                ShowCreator = true,
                ShowProjectsAll = true

            };
            return View("Index",model);
        }

        [Authorize]
        public JsonResult GetListRaw()
        {
            var response = ProjectRequests.GetProjectList(Principal.UserId);
            return Json(response.Projects, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "admin")]
        public JsonResult GetListAllRaw()
        {
            var response = ProjectRequests.GetProjectList(null, true);
            return Json(response.Projects, JsonRequestBehavior.AllowGet);
        }

        private DataTransferUserIdentity Principal
        {
            get { return HttpContext.User.Identity as DataTransferUserIdentity; }
        }
    }
}

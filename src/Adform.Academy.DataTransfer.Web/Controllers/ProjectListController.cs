using System.Web.Mvc;
using Adform.Academy.DataTransfer.Web.Models;
using Adform.Academy.DataTransfer.Web.Services.DataTransfer;

namespace Adform.Academy.DataTransfer.Web.Controllers
{
    public class ProjectListController : Controller
    {
        //
        // GET: /ProjectList/

        [Authorize]
        public ActionResult Index()
        {
            var bla = new ProjectListModel();
            bla.PingResponse = Pinger.Ping();
            return View(bla);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Adform.Academy.DataTransfer.Web.Models;
using Adform.Academy.DataTransfer.Web.Services.DataTransfer;

namespace Adform.Academy.DataTransfer.Web.Controllers
{
    public class ProjectListController : Controller
    {
        //
        // GET: /ProjectList/

        public ActionResult Index()
        {
            ViewBag.IsAdmin = true;

            var bla = new ProjectListModel();
            bla.PingResponse = Pinger.Ping();
            return View(bla);
        }

    }
}

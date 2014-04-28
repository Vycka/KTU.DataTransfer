using System.Web.Mvc;
using Adform.Academy.DataTransfer.Web.Models;
using Adform.Academy.DataTransfer.Web.Services.DataTransfer;
using Filter = Adform.Academy.DataTransfer.Core.DTO.Models.Filter;

namespace Adform.Academy.DataTransfer.Web.Controllers
{
    public class DatabasesController : Controller
    {
        //
        // GET: /Databases/

        public ActionResult Index()
        {
            ViewBag.IsAdmin = true;

            var databases = DatabaseRequests.GetDatabasesList();
            return View(
                new DatabaseListModel
                {
                    DatabasesList = databases.DatabasesList
                }
            );
        }

        //
        // GET: /Databases/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Databases/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Databases/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Databases/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Databases/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        //
        // POST: /Databases/Delete/5

        [HttpPost]
        public JsonResult Delete(int databaseId)
        {
            var response = DatabaseRequests.Delete(databaseId);
            return Json(response);
        }

    }
}

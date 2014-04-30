using System;
using System.Web.Mvc;
using Adform.Academy.DataTransfer.Web.Models;
using Adform.Academy.DataTransfer.Web.Services.DataTransfer;

namespace Adform.Academy.DataTransfer.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class DatabasesController : Controller
    {
        public ActionResult Index()
        {
            var databases = DatabaseRequests.GetDatabasesList();
            return View(
                new DatabaseListModel
                {
                    DatabasesList = databases.DatabasesList
                }
            );
        }

        public ActionResult Create()
        {
            return View("Edit", new DatabaseItemModel { Port = "1434" });
        }

        [HttpPost]
        public ActionResult Save(DatabaseItemModel model)
        {
            if (model.Password == null)
                model.Password = String.Empty;

            var response = DatabaseRequests.SaveDatabase(model);

            if (!response.Success)
            {
                ModelState.AddModelError("ErrorSummary", response.Message);
                return View("Edit", model);
            }

            return RedirectToAction("Index");

        }

        public ActionResult Edit(int id)
        {
            var response = DatabaseRequests.GetDatabase(id);
            var model = new DatabaseItemModel
            {

                DatabaseId = response.DatabaseId,
                ConnectionName = response.ConnectionName,
                Host = response.Host,
                Port = response.Port,
                UserName = response.UserName,
                Password = response.Password,
                DatabaseName = response.DatabaseName
            };
            return View("Edit", model);
        }

        [HttpPost]
        public JsonResult Delete(int databaseId)
        {
            var response = DatabaseRequests.Delete(databaseId);
            return Json(response);
        }

    }
}

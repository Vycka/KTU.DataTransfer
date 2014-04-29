using System;
using System.Runtime.Remoting.Messaging;
using System.Web.Mvc;
using Adform.Academy.DataTransfer.Web.Models;
using Adform.Academy.DataTransfer.Web.Services.DataTransfer;
using Adform.Academy.DataTransfer.WebApi.Contracts.Databases;
using Filter = Adform.Academy.DataTransfer.Core.DTO.Models.Filter;

namespace Adform.Academy.DataTransfer.Web.Controllers
{
    public class DatabasesController : Controller
    {
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

        public ActionResult Create()
        {
            return View("Edit", new DatabaseItemModel { Port = "1434" });
        }

        [HttpPost]
        public ActionResult Save(DatabaseItemModel model)
        {
            if (model.Password == null)
                model.Password = String.Empty;

            DatabaseRequests.Save(model);

            return RedirectToAction("Index");

        }

        public ActionResult Edit(int id)
        {
            var response = DatabaseRequests.Get(id);
            var model = new DatabaseItemModel
            {

                DatabaseId = response.Database.DatabaseId,
                ConnectionName = response.Database.ConnectionName,
                Host = response.Database.Host,
                Port = response.Database.Port,
                UserName = response.Database.UserName,
                Password = response.Database.Password,
                DatabaseName = response.Database.DatabaseName
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

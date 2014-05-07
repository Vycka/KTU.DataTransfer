using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Adform.Academy.DataTransfer.Web.Models;
using Adform.Academy.DataTransfer.Web.Services.DataTransfer;
using Adform.Academy.DataTransfer.WebApi.Contracts.Projects;

namespace Adform.Academy.DataTransfer.Web.Controllers
{
    [Authorize]
    public class ProjectEditorController : Controller
    {
        //
        // GET: /ProjectEditor/
        public ActionResult Index()
        {
            ModelState.Clear();

            var model = new ProjectEditorModel();
            model.Databases = BuildDatabasesList();
            return View("ProjectSummary", model);
        }

        public ActionResult Edit(int id)
        {
            ModelState.Clear();

            
            var response = ProjectRequests.GetProject(id);
            var model = new ProjectEditorModel(response);

            model.Databases = BuildDatabasesList();

            return View("ProjectSummary", model);
        }

        public ActionResult ProjectSummary(ProjectEditorModel model)
        {
            ModelState.Clear();

            model.Databases = BuildDatabasesList();
            return View("ProjectSummary", model);
        }

        public ActionResult Columns(ProjectEditorModel model)
        {
            ModelState.Clear();

            var response = DatabaseRequests.GetDatabaseStructure(model.SourceDatabaseId);
            if (!response.Success)
            {
                ModelState.AddModelError("ErrorSummary", response.Message);
                model.Databases = BuildDatabasesList();
                return View("ProjectSummary", model);
            }

            model.Tables = DatabaseRequests.GetDatabaseStructure(model.SourceDatabaseId).Tables;
            return View(model);
        }

        public ActionResult Details(int projectId)
        {
            var model = new ProjectEditorModel(ProjectRequests.GetProject(projectId));
            var sourceDatabase = DatabaseRequests.GetDatabase(model.SourceDatabaseId);
            var destinationDatabase = DatabaseRequests.GetDatabase(model.DestinationDatabaseId);
            model.SourceDatabaseName = sourceDatabase.ConnectionName;
            model.DestinationDatabaseName = destinationDatabase.ConnectionName;
            return View("ReviewFiltersFrame", model);
        }

        private IEnumerable<SelectListItem> BuildDatabasesList()
        {
            var emptyItemList = new[]
            {
                new SelectListItem
                {
                    Text = "",
                    Value = ""
                }
            };
            var databasesList = DatabaseRequests.GetDatabasesList().DatabasesList
                    .Select(d => new SelectListItem { Text = d.ConnectionName, Value = d.DatabaseId.ToString(CultureInfo.InvariantCulture) });

            return emptyItemList.Concat(databasesList);
        }

        public ActionResult EditFilters(ProjectEditorModel model)
        {
            ModelState.Clear();

            return View(model);
        }

        public ActionResult Review(ProjectEditorModel model)
        {
            ModelState.Clear();

            model.SourceDatabaseName = DatabaseRequests.GetDatabase(model.SourceDatabaseId).DatabaseName;
            model.DestinationDatabaseName = DatabaseRequests.GetDatabase(model.DestinationDatabaseId).DatabaseName;

            return View(model);
        }

        public ActionResult Save(ProjectEditorModel model)
        {
            ModelState.Clear();
            ProjectRequests.Save(model);

            return View();
        }
    }
}
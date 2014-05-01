using System.Collections.Generic;
using Adform.Academy.DataTransfer.WebApi.Contracts.Projects;

namespace Adform.Academy.DataTransfer.Web.Models
{
    public class ProjectListModel
    {
        public ProjectListModel()
        {
            ShowCreator = false;
            ShowProjectsAll = false;
        }
        public List<ProjectListItem> Projects { get; set; }

        public bool ShowCreator { get; set; }
        public bool ShowProjectsAll { get; set; }
    }
}
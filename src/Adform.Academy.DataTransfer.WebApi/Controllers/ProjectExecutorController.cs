using System.Web.Http;
using Adform.Academy.DataTransfer.Core.DTO.Models;
using Adform.Academy.DataTransfer.Core.DTO.NHibernate;
using Adform.Academy.DataTransfer.Core.DTO.Types;
using Adform.Academy.DataTransfer.WebApi.Contracts.ProjectExecutor;
using Adform.Academy.DataTransfer.WebApi.Contracts.Projects;
using ExecutionStepsTypes = Adform.Academy.DataTransfer.WebApi.Contracts.Projects.Types.ExecutionStepsTypes;

namespace Adform.Academy.DataTransfer.WebApi.Controllers
{
    [RoutePrefix("Adform.Academy.DataTransfer/v1/ProjectExecutor")]
    public class ProjectExecutorController : ApiController
    {
        [Route("Start")]
        [HttpGet, HttpPost]
        public StartResponse Start(StartRequest request)
        {
            var session = SessionFactory.GetSession();
            var project = session.Get<Project>(request.ProjectId);

            //TODO: Logging..
            //TODO: Start Project For Real;
            project.ProjectState = ProjectStateTypes.Running;
            project.ExecutionState = (project.ExecutionState != Core.DTO.Types.ExecutionStepsTypes.Completed
                    ? Core.DTO.Types.ExecutionStepsTypes.FullAnalyze
                    : Core.DTO.Types.ExecutionStepsTypes.AppendAnalyze);

            session.Merge(project);
            session.Flush();

            return new StartResponse
            {
                ProjectState = new ProjectListItem
                {
                    CreatedByUserName = project.CreatedBy.UserName,
                    ExecutionStep = (ExecutionStepsTypes) project.ExecutionState,
                    Name = project.Name,
                    ProjectId = project.ProjectId,
                    ProjectState = (Contracts.Projects.Types.ProjectStateTypes) project.ProjectState
                }
            };
        }

        [Route("Archive")]
        [HttpGet, HttpPost]
        public ArchiveResponse Archive(ArchiveRequest request)
        {
            var session = SessionFactory.GetSession();
            var project = session.Get<Project>(request.ProjectId);

            if (project.ProjectState == ProjectStateTypes.Running)
            {
                return new ArchiveResponse
                {
                    Success = false,
                    Message = "Cannot archive project while it's running!"
                };
            }
            
            project.ProjectState = ProjectStateTypes.Archived;

            session.Merge(project);
            session.Flush();

            return new ArchiveResponse
            {
                ProjectState = new ProjectListItem
                {
                    CreatedByUserName = project.CreatedBy.UserName,
                    ExecutionStep = (ExecutionStepsTypes)project.ExecutionState,
                    Name = project.Name,
                    ProjectId = project.ProjectId,
                    ProjectState = (Contracts.Projects.Types.ProjectStateTypes)project.ProjectState
                }
            };
        }

        [Route("Delete")]
        [HttpGet, HttpPost]
        public DeleteResponse Delete(DeleteRequest request)
        {
            var session = SessionFactory.GetSession();
            var project = session.Get<Project>(request.ProjectId);

            if (project.ProjectState == ProjectStateTypes.Running)
            {
                return new DeleteResponse
                {
                    Success = false,
                    Message = "Cannot delete project while it's running!"
                };
            }
            session.Delete(project);
            session.Flush();

            return new DeleteResponse();
        }
    }
}

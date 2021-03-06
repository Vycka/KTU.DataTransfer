﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Adform.Academy.DataTransfer.Core.DTO.Models;
using Adform.Academy.DataTransfer.Core.DTO.NHibernate;
using Adform.Academy.DataTransfer.Core.DTO.Types;
using Adform.Academy.DataTransfer.Logger.Events;
using Adform.Academy.DataTransfer.WebApi.Contracts.Projects;
using Newtonsoft.Json;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace Adform.Academy.DataTransfer.WebApi.Controllers
{
    [RoutePrefix("Adform.Academy.DataTransfer/v1/Projects")]
    public class ProjectRequestController : ControllerBase
    {
        [Route("GetProjectList")]
        [HttpGet, HttpPost]
        public GetProjectListResponse GetProjectList(GetProjectListRequest request)
        {
            using (var session = SessionFactory.OpenSession())
            {
                var criteria = session.CreateCriteria(typeof (Project));

                // request != null to be able to see request from web browser for debugging
                if (request != null && request.CreatedByUserId.HasValue)
                {
                    var creatorUser = session.Get<User>(request.CreatedByUserId);
                    criteria = criteria.Add(Restrictions.Eq("CreatedBy", creatorUser));
                }
                if (request == null || !request.ShowArchived)
                {
                    criteria =
                        criteria.Add(Restrictions.Not(Restrictions.Eq("ProjectState", ProjectStateTypes.Archived)));
                }

                IList<ProjectListItem> projectList = criteria
                    .CreateAlias("CreatedBy", "u", NHibernate.SqlCommand.JoinType.LeftOuterJoin)
                    .SetProjection(Projections.ProjectionList()
                        .Add(Projections.Property("ProjectId"), "ProjectId")
                        .Add(Projections.Property("Name"), "Name")
                        .Add(Projections.Property("ExecutionState"), "ExecutionStep")
                        .Add(Projections.Property("ProjectState"), "ProjectState")
                        .Add(Projections.Property("u.UserName"), "CreatedByUserName")
                    ).SetResultTransformer(Transformers.AliasToBean<ProjectListItem>()
                    ).List<ProjectListItem>();

                return new GetProjectListResponse
                {
                    Projects = projectList.ToList()
                };
            }
        }

        [Route("GetProject")]
        [HttpGet, HttpPost]
        public GetProjectResponse GetProject(GetProjectRequest request)
        {
            using (var session = SessionFactory.OpenSession())
            {
                var project = session.Get<Project>(request.ProjectId);
                var response = new GetProjectResponse
                {
                    ProjectId = project.ProjectId,
                    ProjectName = project.Name,
                    SourceDatabaseId = project.DatabaseSource.DatabaseId,
                    DestinationDatabaseId = project.DatabaseDestination.DatabaseId,
                    Filters = project.Filters.Select(f => new FilterItem
                    {
                        TableName = f.TableName,

                        Columns = f.Columns.Select(
                            c => new ColumnItem {ColumnName = c.ColumnName, ColumnType = c.ColumnType}
                            ).ToList(),

                        FilterValue = JsonConvert.DeserializeObject<FilterValueItem>(f.FilterValue)
                    }).ToList()
                };

                return response;
            }
        }

        [Route("GetProjectProgress")]
        [HttpGet, HttpPost]
        public GetProjectProgressResponse GetProjectProgress(GetProjectProgressRequest request)
        {
            using (var session = SessionFactory.OpenSession())
            {
                var project = session.Get<Project>(request.ProjectId);

                IList<int> filterIds = session.CreateCriteria(typeof(Filter))
                    .SetProjection(Projections.ProjectionList()
                        .Add(Projections.Property("FilterId"), "ID_Filters")
                    ).Add(Restrictions.Eq("Project", project)
                ).List<int>();


                //TODO TO HQL
                var result = session.CreateSQLQuery(
                    @"SELECT
                        B.[ID_BatchState] AS StateId,COUNT(*) as Count
                    FROM [DataTransfer].[Batches] AS B
                    WHERE B.ID_Filters IN (:IDLIst)
                    GROUP BY B.[ID_BatchState]"
                )
                .SetParameterList("IDLIst", filterIds)
                .List<object[]>();

                var resultParsed = result.Select(obj => new ProjectStateItem {StateId = (int) obj[0], Count = (int) obj[1]});

                return new GetProjectProgressResponse
                {
                    StateItems = resultParsed.ToList()
                };
            }

        }

        [Route("Save")]
        [HttpGet, HttpPost]
        public SaveProjectResponse Save(SaveProjectRequest request)
        {
            using (var session = SessionFactory.OpenSession())
            {
                Project project = request.ProjectId == 0
                    ? new Project
                    {
                        Filters = new List<Filter>()
                    }
                    : session.Get<Project>(request.ProjectId);

                if (project.ProjectId != 0 &&
                    (project.ProjectState == ProjectStateTypes.Running ||
                     project.ProjectState == ProjectStateTypes.Paused))
                    return new SaveProjectResponse
                    {
                        Success = false,
                        Message = "Can't edit not fully stopped project"
                    };

                project.Name = request.ProjectName;
                project.ExecutionState = ExecutionStepsTypes.NotStarted;
                project.ProjectState = ProjectStateTypes.Stopped;
                project.DatabaseSource = session.Get<Database>(request.SourceDatabaseId);
                project.DatabaseDestination = session.Get<Database>(request.DestinationDatabaseId);
                project.CreatedBy = session.Get<User>(request.InvokerUserId);
                project.Filters.Clear();

                foreach (FilterItem filter in request.Filters)
                {
                    var filterDto = new Filter
                    {
                        Batches = new List<Batch>(),
                        Columns = new List<Column>(),
                        FilterValue = JsonConvert.SerializeObject(filter.FilterValue),
                        Project = project,
                        TableName = filter.TableName
                    };

                    filterDto.Columns = filter.Columns.Select(c => new Column
                    {
                        ColumnName = c.ColumnName,
                        ColumnType = c.ColumnType,
                        Filter = filterDto
                    }).ToList();

                    project.Filters.Add(filterDto);
                }

                if (project.ProjectId == 0)
                    session.Save(project);
                else
                    session.Merge(project);
                session.Flush();

                if (request.ProjectId == 0)
                {
                    Logger.Log(new ProjectCreatedEvent(project.ProjectId, request.InvokerUserId));
                }
                else
                    Logger.Log(new ProjectModified(project.ProjectId, request.InvokerUserId));

                return new SaveProjectResponse();
            }
        }

        [Route("Delete")]
        [HttpGet, HttpPost]
        public DeleteProjectResponse Delete(DeleteProjectRequest request)
        {
            using (var session = SessionFactory.OpenSession())
            {
                var project = session.Get<Project>(request.ProjectId);

                Logger.Log(new ProjectDeleted(project.ProjectId, request.InvokerUserId));

                session.Delete(project);
                session.Flush();

                return new DeleteProjectResponse();
            }
        }
    }
}

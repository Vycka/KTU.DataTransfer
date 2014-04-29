using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Adform.Academy.DataTransfer.Core.DTO.Models;
using Adform.Academy.DataTransfer.Core.DTO.NHibernate;
using Adform.Academy.DataTransfer.WebApi.Contracts.Databases;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace Adform.Academy.DataTransfer.WebApi.Controllers
{
    [RoutePrefix("Adform.Academy.DataTransfer/v1/Databases")]
    public class DatabasesRequestController : ApiController
    {
        [Route("GetDatabasesList")]
        [HttpGet, HttpPost]
        public GetDatabasesListResponse GetDatabasesList(GetDatabasesListRequest request)
        {
            using (ISession session = SessionFactory.OpenSession())
            {
                IList<DatabaseItem> databasesList = session.CreateCriteria(typeof(Database)).SetProjection(Projections.ProjectionList()
                    .Add(Projections.Property("DatabaseId"), "DatabaseId")
                    .Add(Projections.Property("ConnectionName"), "ConnectionName"))
                    .SetResultTransformer(Transformers.AliasToBean<DatabaseItem>())
                    .List<DatabaseItem>();

                return new GetDatabasesListResponse
                {
                    DatabasesList = databasesList.ToList()
                };
            }
        }

        [Route("Get")]
        [HttpGet, HttpPost]
        public GetDatabaseResponse Get(GetDatabaseRequest request)
        {
            using (ISession session = SessionFactory.OpenSession())
            {
                var database = session.Get<Database>(request.DatabaseId);

                session.Flush();
                return new GetDatabaseResponse
                {
                    Database = database
                };
            }
        }

        [Route("Save")]
        [HttpGet, HttpPost]
        public SaveDatabaseResponse Save(SaveDatabaseRequest request)
        {
            using (ISession session = SessionFactory.OpenSession())
            {
                //TODO: Logging
                session.SaveOrUpdate(request.Database);

                session.Flush();
                return new SaveDatabaseResponse
                {
                    Message = "Success!"
                };
            }
        }

        [Route("Delete")]
        [HttpGet, HttpPost]
        public DeleteDatabaseResponse Delete(DeleteDatabaseRequest request)
        {
            using (ISession session = SessionFactory.OpenSession())
            {
                var databaseToDelete = session.Get<Database>(request.DatabaseId);

                IList<Project> result = session.CreateCriteria(typeof (Project))
                    .Add(
                        Restrictions.Or(
                            Restrictions.Eq("DatabaseSource", databaseToDelete),
                            Restrictions.Eq("DatabaseDestination", databaseToDelete)
                            )
                    ).List<Project>();

                if (result.Count == 0)
                {
                    //TODO: Logging
                    session.Delete(databaseToDelete);
                    session.Flush();
                    return new DeleteDatabaseResponse
                    {
                        Message = "Success!"
                    };
                }

                return new DeleteDatabaseResponse
                {
                    Message = "Cannot delete this database, because it's used in these projects: "
                              + string.Join(", ", result.Select(p => p.Name)),
                    Success = false
                };
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Adform.Academy.DataTransfer.Core.DTO.Models;
using Adform.Academy.DataTransfer.Core.DTO.NHibernate;
using Adform.Academy.DataTransfer.Logger.Events;
using Adform.Academy.DataTransfer.WebApi.Contracts.Databases;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace Adform.Academy.DataTransfer.WebApi.Controllers
{
    [RoutePrefix("Adform.Academy.DataTransfer/v1/Databases")]
    public class DatabasesRequestController : ControllerBase
    {
        [Route("GetDatabasesList")]
        [HttpGet, HttpPost]
        public GetDatabasesListResponse GetDatabasesList(GetDatabasesListRequest request)
        {
            using (var session = SessionFactory.OpenSession())
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

                return new GetDatabaseResponse
                {
                    DatabaseId = database.DatabaseId,
                    ConnectionName = database.ConnectionName,
                    Host = database.Host,
                    Port = database.Port,
                    UserName = database.UserName,
                    Password = database.Password,
                    DatabaseName = database.DatabaseName
                };
            }
        }

        [Route("Save")]
        [HttpGet, HttpPost]
        public SaveDatabaseResponse Save(SaveDatabaseRequest request)
        {
            var existingDatabaseByName = GetDatabaseByConnectionName(request.ConnectionName);
            if (existingDatabaseByName != null)
            {
                if (existingDatabaseByName.DatabaseId != request.DatabaseId)
                {
                    return new SaveDatabaseResponse
                    {
                        Success = false,
                        Message = "Connection with that name already exists!"
                    };
                }
            }

            using (ISession session = SessionFactory.OpenSession())
            {
                var database = new Database
                {
                    DatabaseId = request.DatabaseId,
                    ConnectionName = request.ConnectionName,
                    Host = request.Host,
                    Port = request.Port,
                    UserName = request.UserName,
                    Password = request.Password,
                    DatabaseName = request.DatabaseName
                };

                var existingDatabaseById = session.Get<Database>(database.DatabaseId);
                Logger.Log(new DatabaseChangedEvent(database.DatabaseId, database.ConnectionName, (existingDatabaseById != null ? existingDatabaseById.ConnectionName : ""), request.InvokerUserId));

                session.Merge(database);
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
                    Logger.Log(new DatabaseDeleteEvent(databaseToDelete.ConnectionName, request.InvokerUserId));

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

        [Route("GetDatabaseStructure")]
        [HttpGet, HttpPost]
        public GetDatabaseStructureResponse GetDatabaseStructure(GetDatabaseStructureRequest request)
        {
            try
            {
                var tables = new List<TableInformation>();
                using (ISession session = SessionFactory.OpenSession())
                {
                    var database = session.Get<Database>(request.DatabaseId);

                    using (var connection = SessionFactory.CreateIdbConnection(database))
                    using (var dbSession = SessionFactory.OpenSession(connection))
                    {
                        connection.Open();

                        IList<TableStructure> tableStructure = dbSession
                            .CreateCriteria(typeof (TableStructure))
                            .List<TableStructure>();

                        foreach (var structureItem in tableStructure)
                        {
                            int existingTableIndex = FindIndexOfExistingTable(tables, structureItem.TableName);
                            if (existingTableIndex == -1)
                            {
                                tables.Add(new TableInformation {TableName = structureItem.TableName});
                                existingTableIndex = tables.Count - 1;
                            }
                            tables[existingTableIndex].Fields.Add(
                                new FieldInformation
                                {
                                    FieldName = structureItem.ColumnName,
                                    FieldType = structureItem.DataType
                                }
                                );
                        }

                        return new GetDatabaseStructureResponse
                        {
                            Tables = tables
                        };
                    }
                }
            }
            catch (Exception)
            {
                return new GetDatabaseStructureResponse
                {
                    Success = false,
                    Message = "Failed to retrieve database information. Is Source connection working right?"
                };
            }
        }

        public int FindIndexOfExistingTable(List<TableInformation> tables, string tableName)
        {
            for (int x = 0; x < tables.Count; x++)
            {
                if (tables[x].TableName == tableName)
                    return x;
            }
            return -1;
        }

        public Database GetDatabaseByConnectionName(string name)
        {
            using (ISession session = SessionFactory.OpenSession())
            {
                var database = session.CreateCriteria(typeof (Database))
                    .Add(Restrictions.Eq("ConnectionName", name))
                    .UniqueResult<Database>();

                return database;
            }
        }
    }
}

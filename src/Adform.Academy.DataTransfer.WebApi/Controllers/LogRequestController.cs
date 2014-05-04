using System.Linq;
using System.Web.Http;
using Adform.Academy.DataTransfer.Core.DTO.Models;
using Adform.Academy.DataTransfer.Core.DTO.NHibernate;
using Adform.Academy.DataTransfer.WebApi.Contracts.Logs;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace Adform.Academy.DataTransfer.WebApi.Controllers
{
    [RoutePrefix("Adform.Academy.DataTransfer/v1/Logs")]
    public class LogRequestController : ControllerBase
    {
        [Route("Get")]
        [HttpGet, HttpPost]
        public GetLogsResponse Get(GetLogsRequest request)
        {
            using (var session = SessionFactory.OpenSession())
            {
                var criteria = session.CreateCriteria(typeof (Log))
                    .CreateAlias("User", "u", NHibernate.SqlCommand.JoinType.LeftOuterJoin)
                    .SetProjection(Projections.ProjectionList()
                        .Add(Projections.Property("LogId"), "LogId")
                        .Add(Projections.Property("TimeStamp"), "TimeStamp")
                        .Add(Projections.Property("LogMessage"), "Message")
                        .Add(Projections.Property("u.UserName"), "UserName")
                    ).SetResultTransformer(Transformers.AliasToBean<LogItem>())
                    .AddOrder(Order.Desc("LogId"))
                    .Add(Restrictions.Gt("LogId", request.BeginFromId))
                    .SetMaxResults(25);
                if (request.ProjectId != null)
                {
                    var project = session.Load<Project>(request.ProjectId);
                    criteria.Add(Restrictions.Eq("Project", project));
                }
                else
                    criteria.Add(Restrictions.IsNull("Project"));

                var logs = criteria.List<LogItem>();

                return new GetLogsResponse
                {
                    Logs = logs.ToList()
                };
            }
        }
    }
}

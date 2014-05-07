using System.Data.SqlClient;
using Adform.Academy.DataTransfer.Core.DTO.Models;
using Adform.Academy.DataTransfer.Logger;
using NHibernate;

namespace Adform.Academy.DataTransfer.Core.DataTransfer.Actions
{
    public class ExecutingProjectData
    {
        public Project Project;
        public SqlConnection SrcConnection;
        public SqlConnection DesConnection;
        public ISession Session;
        public ILogger Logger;
    }
}

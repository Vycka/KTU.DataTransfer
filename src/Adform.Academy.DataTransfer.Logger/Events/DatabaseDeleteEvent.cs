using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Adform.Academy.DataTransfer.Core.DTO.Models;

namespace Adform.Academy.DataTransfer.Logger.Events
{
    public class DatabaseDeleteEvent : LogEvent
    {
        public DatabaseDeleteEvent(Database database, int userId) : base("", null, userId)
        {
            Message = string.Format("Database deleted: [{0}]", database.ConnectionName);
        }
    }
}

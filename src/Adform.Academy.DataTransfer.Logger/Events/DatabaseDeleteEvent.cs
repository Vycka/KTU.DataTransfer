using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Adform.Academy.DataTransfer.Logger.Events
{
    public class DatabaseDeleteEvent : LogEvent
    {
        public DatabaseDeleteEvent(string databaseName, int userId) : base("", null, userId)
        {
            Message = string.Format("Database deleted: [{0}]", databaseName);
        }
    }
}

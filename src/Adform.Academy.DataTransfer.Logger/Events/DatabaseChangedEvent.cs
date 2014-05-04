using Adform.Academy.DataTransfer.Core.DTO.Models;

namespace Adform.Academy.DataTransfer.Logger.Events
{
    public class DatabaseChangedEvent : LogEvent
    {
        public DatabaseChangedEvent(Database newDatabase, Database existingDatabase, int userId) : base("", null, userId)
        {
            if (newDatabase.DatabaseId == 0)
                Message = string.Format("Database connection added: [{0}]", newDatabase.ConnectionName);
            else
            {
                if (existingDatabase.ConnectionName != newDatabase.ConnectionName)
                {
                    Message = string.Format(
                        "Database connection modified: [{0}] -> [{1}]",
                        existingDatabase.ConnectionName,
                        newDatabase.ConnectionName
                    );
                }
                else
                {
                    Message = string.Format(
                        "Database connection modified: [{0}]",
                        newDatabase.ConnectionName
                    );
                }
            }
        }
    }
}

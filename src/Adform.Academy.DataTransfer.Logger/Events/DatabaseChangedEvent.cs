namespace Adform.Academy.DataTransfer.Logger.Events
{
    public class DatabaseChangedEvent : LogEvent
    {
        public DatabaseChangedEvent(int newDatabaseId, string newDatabase, string existingDatabase, int userId) : base("", null, userId)
        {
            if (newDatabaseId == 0)
                Message = string.Format("Database connection added: [{0}]", newDatabase);
            else
            {
                if (existingDatabase != newDatabase)
                {
                    Message = string.Format(
                        "Database connection modified: [{0}] -> [{1}]",
                        existingDatabase,
                        newDatabase
                    );
                }
                else
                {
                    Message = string.Format(
                        "Database connection modified: [{0}]",
                        newDatabase
                    );
                }
            }
        }
    }
}

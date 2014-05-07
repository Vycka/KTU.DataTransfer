namespace Adform.Academy.DataTransfer.Logger.Events
{
    public class UserModifiedEvent : LogEvent
    {
        public UserModifiedEvent(string existingUserName, string newUserName, int userId) : base("", null, userId)
        {
            Message = existingUserName == newUserName 
                ? string.Format("Modified user: [{0}]", newUserName)
                : string.Format("Modified user: [{0}] -> [{1}]", existingUserName, newUserName);
        }
    }
}

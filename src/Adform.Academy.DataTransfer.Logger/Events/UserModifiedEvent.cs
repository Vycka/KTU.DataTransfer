using Adform.Academy.DataTransfer.Core.DTO.Models;

namespace Adform.Academy.DataTransfer.Logger.Events
{
    public class UserModifiedEvent : LogEvent
    {
        public UserModifiedEvent(User existingUser, string newUserName, int userId) : base("", null, userId)
        {
            Message = existingUser.UserName == newUserName 
                ? string.Format("Modified user: [{0}]", newUserName) 
                : string.Format("Modified user: [{0}] -> [{1}]", existingUser.UserName, newUserName);
        }
    }
}

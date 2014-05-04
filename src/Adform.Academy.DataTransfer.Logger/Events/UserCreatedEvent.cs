namespace Adform.Academy.DataTransfer.Logger.Events
{
    public class UserCreatedEvent : LogEvent
    {
        public UserCreatedEvent(int userId, string createdUserUserName) : base("", null, userId)
        {
            Message = string.Format("Created new user: [{0}]", createdUserUserName);
        }
    }
}

using System;
using System.Globalization;

namespace Adform.Academy.DataTransfer.Logger.Events
{
    public class LogEvent
    {
        public int? ProjectId { get; protected set; }
        public int? UserId { get; protected set; }
        public string Message { get; protected set; }
        public DateTime EventDate { get; protected set; }

        public string EventDateStr
        {
            get { return EventDate.ToString("yyyy-MM-dd HH:mm:ss.fff"); }
        }

        public LogEvent(string message, int? projectId = null, int? userId = null)
        {
            ProjectId = projectId;
            Message = message;
            EventDate = DateTime.UtcNow;
            UserId = userId;
        }

        public override string ToString()
        {
            return string.Format(
                "[{0}] - [{1}][{2}]: {3}",
                EventDate, 
                UserId.GetValueOrDefault(),
                ProjectId.GetValueOrDefault(),
                Message);
        }
    }
}

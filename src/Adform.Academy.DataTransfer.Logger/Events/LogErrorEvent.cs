using System;

namespace Adform.Academy.DataTransfer.Logger.Events
{
    public class LogErrorEvent : LogEvent
    {
        public LogErrorEvent(Exception ex, int? projectId = null) : base(ex.ToString(), projectId)
        {
        }
    }
}

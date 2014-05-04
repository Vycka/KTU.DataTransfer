using Adform.Academy.DataTransfer.Logger.Events;

namespace Adform.Academy.DataTransfer.Logger
{
    public interface ILogger
    {
        void Log(LogEvent logObject);
        void LogError(LogErrorEvent logObject);
    }
}

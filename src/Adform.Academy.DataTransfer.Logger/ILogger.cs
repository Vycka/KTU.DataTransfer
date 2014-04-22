namespace Adform.Academy.DataTransfer.Logger
{
    public interface ILogger
    {
        void Log(object logObject);
        void LogError(object logObject);
    }
}

namespace Adform.Academy.DataTransfer.Logger
{
    public interface ILogging
    {
        void Log(object logObject);
        void LogError(object logObject);
    }
}

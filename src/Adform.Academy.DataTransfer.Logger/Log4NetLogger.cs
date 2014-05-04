using Adform.Academy.DataTransfer.Logger.Events;
using log4net;
using log4net.Config;

namespace Adform.Academy.DataTransfer.Logger
{
    public class Log4NetLogger : ILogger
    {
        private readonly ILog _logger;

        static Log4NetLogger()
        {
            XmlConfigurator.Configure();
        }

        public Log4NetLogger(string loggerInstanceName)
        {
            _logger = LogManager.GetLogger(loggerInstanceName); 
        }

        public void Log(LogEvent logObject)
        {
            _logger.Info(logObject);
        }

        public void LogError(LogErrorEvent logObject)
        {
            _logger.Error(logObject);
        }
    }
}

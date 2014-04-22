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

        public void Log(object logObject)
        {
            _logger.Info(logObject);
        }

        public void LogError(object logObject)
        {
            _logger.Error(logObject);
        }
    }
}

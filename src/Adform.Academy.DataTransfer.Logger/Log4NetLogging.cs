using log4net;
using log4net.Config;

namespace Adform.Academy.DataTransfer.Logger
{
    public class Log4NetLogging : ILogging
    {
        private readonly ILog _logger;

        static Log4NetLogging()
        {
            XmlConfigurator.Configure();
        }

        public Log4NetLogging(string loggerInstanceName)
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

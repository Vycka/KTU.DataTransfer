using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Adform.Academy.DataTransfer.Logger
{
    public interface ILogging
    {
        void Log(object logObject);
        void LogError(object logObject);
    }

    public class Log4NetLogging : ILogging
    {
        private readonly ILog _logger;
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

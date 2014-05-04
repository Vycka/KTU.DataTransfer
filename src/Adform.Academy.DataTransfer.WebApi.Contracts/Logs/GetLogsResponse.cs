using System;
using System.Collections.Generic;

namespace Adform.Academy.DataTransfer.WebApi.Contracts.Logs
{
    public class GetLogsResponse : ResponseBase
    {
        public List<LogItem> Logs;
    }

    public struct LogItem
    {
        public int LogId;
        public DateTime TimeStamp;
        public string Message;
        public string UserName;
    }
}

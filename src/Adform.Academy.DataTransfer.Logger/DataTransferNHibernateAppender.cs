using System;
using System.Data;
using System.Data.SqlClient;
using Adform.Academy.DataTransfer.Logger.Events;
using log4net.Appender;
using log4net.Core;

namespace Adform.Academy.DataTransfer.Logger
{
    public class DataTransferNHibernateAppender : IAppender
    {

        private readonly SqlConnection _sqlConnection;
        public DataTransferNHibernateAppender()
        {
            _sqlConnection = new SqlConnection();
        }

        public void Close()
        {
            _sqlConnection.Close();
            _sqlConnection.Dispose();
        }

        public void DoAppend(LoggingEvent loggingEvent)
        {
            var logEvent = loggingEvent.MessageObject as LogEvent;
            if (logEvent != null)
            {
                if (_sqlConnection.State != ConnectionState.Open)
                {
                    _sqlConnection.ConnectionString = ConnectionString;
                    _sqlConnection.Open();
                }
                var command = new SqlCommand(
                    @"INSERT INTO
                        [DataTransfer].[Logs]
                        ([ID_Projects],[LogMessage],[TimeStamp], [ID_Users])
                    VALUES
                        (@ProjectId, @LogMessage, @LogTime, @UserId)",
                    _sqlConnection
                );

                command.Parameters.AddWithValue("ProjectId", (logEvent.ProjectId != null ? (object)logEvent.ProjectId.Value : DBNull.Value));
                command.Parameters.AddWithValue("UserId", (logEvent.UserId != null ? (object)logEvent.UserId.Value : DBNull.Value));
                command.Parameters.AddWithValue("LogTime", logEvent.EventDate);
                command.Parameters.AddWithValue("LogMessage", logEvent.Message);

                command.ExecuteNonQuery();
            }
        }

        public string ConnectionString { get; set; }

        public string Name { get; set; }
    }
}

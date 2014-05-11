using System;
using System.Data;
using System.Data.SqlClient;
using Adform.Academy.DataTransfer.Core.DTO.Models;
using NHibernate;
using NHibernate.Cfg;

namespace Adform.Academy.DataTransfer.Core.DTO.NHibernate
{
    public class SessionFactory
    {
        private static readonly ISessionFactory InitializedSessionFactory;

        static SessionFactory()
        {
            InitializedSessionFactory = CreateNewSessionFactory();
        }
        private static ISessionFactory CreateNewSessionFactory(String connectionString = null)
        {
            var baseConfiguration = new Configuration();
            baseConfiguration.Configure();

            if (connectionString != null)
                baseConfiguration.SetProperty("connection.connection_string", connectionString);

            baseConfiguration.AddAssembly(typeof(Project).Assembly);
            //baseConfiguration.AddAssembly(typeof(Batch).Assembly);
            return baseConfiguration.BuildSessionFactory();
        }

        public static ISession OpenSession()
        {
            return InitializedSessionFactory.OpenSession();
        }

        public static IDbConnection CreateIdbConnection(Database database)
        {
            var connectionInformation = new SqlConnectionStringBuilder
            {
                DataSource = database.Host,
                InitialCatalog = database.DatabaseName,
                UserID = database.UserName,
                Password = database.Password
                //Encrypt = true
            };

            string sqlConnectionString = connectionInformation.ToString();
            return new SqlConnection(sqlConnectionString);
        }

        public static ISession OpenSession(IDbConnection connection)
        {
            return InitializedSessionFactory.OpenSession(connection);
        }
    }
}

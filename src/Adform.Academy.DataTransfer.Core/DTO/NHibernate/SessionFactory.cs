using System;
using Adform.Academy.DataTransfer.Core.DTO.Models;
using NHibernate;
using NHibernate.Cfg;

namespace Adform.Academy.DataTransfer.Core.DTO.NHibernate
{
    public class SessionFactory
    {
        private static readonly ISessionFactory InitializedSessionFactory;
        private static ISession _activeSession = null;

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

        public static ISession GetSession()
        {
            if (_activeSession == null || !_activeSession.IsConnected)
                _activeSession = InitializedSessionFactory.OpenSession();
            return _activeSession;
        }
    }
}

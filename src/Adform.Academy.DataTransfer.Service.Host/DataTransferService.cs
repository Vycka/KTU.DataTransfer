using System;
using System.Configuration;
using System.ServiceProcess;
using Adform.Academy.DataTransfer.Logger;
using Adform.Academy.DataTransfer.WebApi;
using Autofac.Integration.WebApi;


namespace Adform.Academy.DataTransfer.Service.Host
{
    public class DataTransferService : ServiceBase
    {
        private readonly ILogger _logging;

        private WebHost _webApiHost;

        public DataTransferService(ILogger logging)
        {
            if (logging == null) throw new ArgumentNullException("logging");
            _logging = logging;

            ServiceName = "Adform.Academy.DataTransfer";
        }

        public void Run(string[] args)
        {
            if (Environment.UserInteractive)
            {
                OnStart(args);
                HandleUserInteraction();
                OnStop();
            }
            else
            {
                Run(this);
            }
        }

        protected override void OnStart(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException -= OnUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;


            string url = ConfigurationManager.AppSettings["DataTransferServiceUrl"];
            var dependencyResolver = new AutofacWebApiDependencyResolver(DI.Container.Instance);
            _webApiHost = new WebHost(url, dependencyResolver, _logging);
            _webApiHost.Start();

            _logging.Log("Service Started");

            base.OnStart(args);
        }

        protected override void OnStop()
        {
            _webApiHost.Dispose();

            _logging.Log("Service Stopped!");
            base.OnStop();
        }

        private void HandleUserInteraction()
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }


        protected virtual void OnUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            var exception = args.ExceptionObject as Exception;

            _logging.LogError(exception);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                AppDomain.CurrentDomain.UnhandledException -= OnUnhandledException;
            }
            base.Dispose(disposing);
        }
    }
}

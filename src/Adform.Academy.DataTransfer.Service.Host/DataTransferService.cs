using System;
using System.Configuration;
using System.ServiceProcess;
using Adform.Academy.DataTransfer.Core.DataTransfer;
using Adform.Academy.DataTransfer.Logger;
using Adform.Academy.DataTransfer.Logger.Events;
using Adform.Academy.DataTransfer.WebApi;
using Autofac.Integration.WebApi;


namespace Adform.Academy.DataTransfer.Service.Host
{
    public class DataTransferService : ServiceBase
    {
        private readonly ILogger _logger;
        private DataTransferServiceRunner _serviceRunner;
        private WebHost _webApiHost;

        public DataTransferService(ILogger logger)
        {
            if (logger == null) throw new ArgumentNullException("logger");
            _logger = logger;

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
            _logger.Log(new LogEvent("ServiceHost starting..."));

            AppDomain.CurrentDomain.UnhandledException -= OnUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            _serviceRunner = new DataTransferServiceRunner(_logger);
            _serviceRunner.StartService();

            WebApi.Controllers.ControllerBase.Logger = _logger;
            WebApi.Controllers.ControllerBase.ServiceRunner = _serviceRunner;


            string url = ConfigurationManager.AppSettings["DataTransferServiceUrl"];
            var dependencyResolver = new AutofacWebApiDependencyResolver(DI.Container.Instance);
            _webApiHost = new WebHost(url, dependencyResolver, _logger);
            _webApiHost.Start();

            _logger.Log(new LogEvent("ServiceHost started"));

            base.OnStart(args);
        }

        protected override void OnStop()
        {
            _serviceRunner.StopService();
            _webApiHost.Dispose();

            _logger.Log(new LogEvent("ServiceHost stopped"));
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

            _logger.LogError(new LogErrorEvent(exception));
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

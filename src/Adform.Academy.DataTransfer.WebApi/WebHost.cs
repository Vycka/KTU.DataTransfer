using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dependencies;
using System.Web.Http.SelfHost;
using Adform.Academy.DataTransfer.Logger;
using Adform.Academy.DataTransfer.WebApi.Formatters;


namespace Adform.Academy.DataTransfer.WebApi
{
    public class WebHost : IDisposable
    {
        private readonly string _baseUrl;
        private readonly IDependencyResolver _dependencyResolver;
        private readonly ILogger _logging;

        private readonly HttpSelfHostServer _httpSelfHostServer;

        public WebHost(string baseUrl, IDependencyResolver dependencyResolver, ILogger logging)
        {
            if (baseUrl == null) throw new ArgumentNullException("baseUrl");
            if (dependencyResolver == null) throw new ArgumentNullException("dependencyResolver");
            if (logging == null) throw new ArgumentNullException("logging");

            _baseUrl = baseUrl;
            _dependencyResolver = dependencyResolver;
            _logging = logging;

            var configuration = CreateSelfHostConfiguration();
            _httpSelfHostServer = new HttpSelfHostServer(configuration);
        }

        private HttpSelfHostConfiguration CreateSelfHostConfiguration()
        {
            var configuration = new HttpSelfHostConfiguration(_baseUrl);
            
            configuration.DependencyResolver = _dependencyResolver;

            configuration.MapHttpAttributeRoutes();

            configuration.Formatters.Clear();
            configuration.Formatters.Add(new JsonFormatter());

            //foreach (IHttpRoute route in configuration.Routes)
            //{
            //    _logging.Log("Registrated route: " + route.);
            //}

            return configuration;
        }


        public void Start()
        {
            _logging.Log("DataTransferService starting... " + _baseUrl);

            _httpSelfHostServer.OpenAsync()
                .ContinueWith(t => _logging.LogError(t.Exception), TaskContinuationOptions.OnlyOnFaulted);
        }

        public void Dispose()
        {
            _httpSelfHostServer.Dispose();
        }
    }
}

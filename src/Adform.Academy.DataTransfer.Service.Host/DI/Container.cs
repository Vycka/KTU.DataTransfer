using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Integration.WebApi;

namespace Adform.Academy.DataTransfer.Service.Host.DI
{
    public static class Container 
    {
        static Container()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new DataTransferServiceModule());
            builder.RegisterApiControllers(typeof(WebApi.WebHost).Assembly);
            Instance = builder.Build();
        }

        public static IContainer Instance { get; set; }

        public static T Resolve<T>() where T : class
        {
            return Instance.Resolve<T>();
        }
    }
}

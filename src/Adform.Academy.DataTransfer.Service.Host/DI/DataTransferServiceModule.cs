using Adform.Academy.DataTransfer.Logger;
using Autofac;

namespace Adform.Academy.DataTransfer.Service.Host.DI
{
    class DataTransferServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DataTransferService>();
            builder.Register(CreateLogger).SingleInstance().AsSelf();
        }

        private ILogger CreateLogger(IComponentContext container)
        {
            return new Log4NetLogger("DataTransferService");
        }
    }
}

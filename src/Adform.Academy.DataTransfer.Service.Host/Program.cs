using Adform.Academy.DataTransfer.Service.Host.DI;

namespace Adform.Academy.DataTransfer.Service.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = Container.Resolve<DataTransferService>();

            service.Run(args);
        }
    }
}

using Adform.Academy.DataTransfer.Service.Host.DI;

namespace Adform.Academy.DataTransfer.Service.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            //ISession session = SessionFactory.OpenSession();
            
            //Database db = session.Get<Database>(1);
            //Project p = session.Get<Project>(1);
            var service = Container.Resolve<DataTransferService>();

            service.Run(args);
        }
    }
}

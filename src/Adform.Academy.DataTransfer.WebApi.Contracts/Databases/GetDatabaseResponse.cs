namespace Adform.Academy.DataTransfer.WebApi.Contracts.Databases
{
    public class GetDatabaseResponse : ResponseBase
    {
        public int DatabaseId { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DatabaseName { get; set; }
        public string ConnectionName { get; set; }
    }
}

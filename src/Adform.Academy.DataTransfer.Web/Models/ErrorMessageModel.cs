namespace Adform.Academy.DataTransfer.Web.Models
{
    public class ErrorMessageModel
    {
        public string Message;

        public ErrorMessageModel(string message = "")
        {
            Message = message;
        }
    }
}
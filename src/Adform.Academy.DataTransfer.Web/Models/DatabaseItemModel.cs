using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Adform.Academy.DataTransfer.Web.Models
{
    public class DatabaseItemModel
    {
        public int DatabaseId { get; set; }


        [Required(ErrorMessage = "Connection Name is Required")]
        [DataType(DataType.Text)]
        [DisplayName("Connection Name")]
        public string ConnectionName { get; set; }


        [DisplayName("Host Address")]
        [Required(ErrorMessage = "Host Address is Required")]
        [DataType(DataType.Text)]
        public string Host { get; set; }


        [Range(0, 65535, ErrorMessage = "Valid port must be between 0 .. 65535")]
        [Required(ErrorMessage = "Valid port must be between 0 .. 65535")]
        [DisplayName("Port")]
        public string Port { get; set; }


        [Required(ErrorMessage = "User Name is Required")]
        [DataType(DataType.Text)]
        [DisplayName("User Name")]
        public string UserName { get; set; }


        [DisplayName("Password")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Database Name is Required")]
        [DataType(DataType.Text)]
        [DisplayName("Database Name")]
        public string DatabaseName { get; set; }
    }
}
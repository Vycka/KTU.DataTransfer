using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Adform.Academy.DataTransfer.Web.Models
{
    public class UserItemModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "User Name is Required")]
        [DataType(DataType.Text)]
        [DisplayName("User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Text)]
        [DisplayName("Password")]
        [DefaultValue("")]
        public string Password { get; set; }


        [DisplayName("Active")]
        public bool IsActive { get; set; }

        [DisplayName("Admin")]
        public bool IsAdmin { get; set; }
    }
}
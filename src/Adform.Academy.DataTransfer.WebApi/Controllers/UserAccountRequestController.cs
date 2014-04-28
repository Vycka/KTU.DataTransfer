using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Security;

namespace Adform.Academy.DataTransfer.WebApi.Controllers
{

    [RoutePrefix("Adform.Academy.DataTransfer/v1/UserAccount")]
    public class UserAccountRequestController : ApiController 
    {
        [Route("Login")]
        [HttpGet]
        public void Login()
        {
            //FormsAuthenticationTicket ticket = new FormsAuthenticationTicket("Viki!", true, 525600);
            //string encrypted = FormsAuthentication.Encrypt(ticket);

            //HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
            //cookie.Expires = System.DateTime.Now.AddMinutes(525600);

            //HttpContext.Current.Response.Cookies.Add(cookie);


            FormsAuthentication.SetAuthCookie("Viki!", true);
        }

        [Route("Test")]
        [HttpGet]
        public string Test()
        {
            //HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(FormsAuthentication.FormsCookieName);
            //if (cookie != null)
            //{
            //    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
            //    var userName = ticket.UserData;
            //    return userName;
            //}
            //else
            //{
            //    return "NO AUTH";
            //}
            return "";
        }
    }
}

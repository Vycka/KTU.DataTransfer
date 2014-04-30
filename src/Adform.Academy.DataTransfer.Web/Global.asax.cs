using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web.ClientServices;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Adform.Academy.DataTransfer.Web.Services.DataTransfer;
using Adform.Academy.DataTransfer.Web.Tools.Authentication;
using Adform.Academy.DataTransfer.WebApi.Contracts.Users;

namespace Adform.Academy.DataTransfer.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            if (!FormsAuthentication.CookiesSupported)
            {
                throw new NotSupportedException("Cookieless authentication not supported!");
            }

            if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
            {
                var roles = new List<string>();

                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value);
                if (ticket != null)
                {
                    try
                    {
                        GetUserResponse userInformation = UserRequests.GetUser(int.Parse(ticket.Name));

                        if (userInformation.Success)
                        {
                            if (userInformation.IsActive)
                                roles.Add("user");
                            if (userInformation.IsActive && userInformation.IsAdmin)
                                roles.Add("admin");
                        }

                        Context.User = new GenericPrincipal(
                            new DataTransferUserIdentity(
                                userInformation.UserName, 
                                userInformation.UserId
                            ),
                            roles.ToArray()
                        );
                    }
                    catch (Exception)
                    {
                        
                    }

                }
            }
        }

    }
}
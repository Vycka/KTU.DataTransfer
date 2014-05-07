using System;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using Adform.Academy.DataTransfer.Web.Models;
using Adform.Academy.DataTransfer.Web.Services.DataTransfer;
using Adform.Academy.DataTransfer.Web.Tools.Authentication;
using Adform.Academy.DataTransfer.WebApi.Contracts.Users;

namespace Adform.Academy.DataTransfer.Web.Controllers
{
    public class UsersController : Controller
    {
        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            var model = new UserListModel
            {
                Users = UserRequests.GetUserList().Users
            };

            return View(model);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            return View("Edit", new UserItemModel { IsActive = true, IsAdmin = false});
        }

        [Authorize(Roles = "admin")]
        public ActionResult Save(UserItemModel model)
        {
            var response = UserRequests.SaveUser(model);
            if (!response.Success)
            {
                ModelState.AddModelError("ErrorSummary", response.Message);
                return View("Edit", model);
            }

            if (model.UserId == Principal.UserId)
                return RedirectToAction("Logout", "Users");

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id)
        {
            var response = UserRequests.GetUser(id);
            var model = new UserItemModel
            {
                UserName = response.UserName,
                Password = String.Empty,
                IsActive = response.IsActive,
                IsAdmin = response.IsAdmin,
                UserId = response.UserId
            };
            return View("Edit", model);
        }

        [HttpGet]
        public ActionResult Login()
        {
            string returnUrl = Request.QueryString["ReturnUrl"] ?? String.Empty;

            return View((object)returnUrl); // If passed as string, it will be treated as Controller name
        }

        [HttpPost]
        public ActionResult Login(string userName, string password, string returnUrl = "")
        {
            CheckLoginResponse response = UserRequests.CheckUser(userName, password);

            if (response.Success)
            {
                AuthenticateUser(response);

                if (returnUrl != string.Empty)
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("Index","ProjectList");
            }
            else
            {
                ViewBag.Error = response.Message;
                return View((object)returnUrl); // If passed as string, it will be treated as Controller name
            }

        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        private void AuthenticateUser(CheckLoginResponse response)
        {
            if (response.Success)
            {
                FormsAuthentication.SetAuthCookie(response.UserId.ToString(CultureInfo.InvariantCulture), false);
            }
        }

        private DataTransferUserIdentity Principal
        {
            get { return HttpContext.User.Identity as DataTransferUserIdentity; }
        }
    }
}

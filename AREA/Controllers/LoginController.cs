using AREA.Models;
using AREA.Models.Entity;
using DotNetOpenAuth.GoogleOAuth2;
using Microsoft.AspNet.Membership.OpenAuth;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AREA.Controllers
{
    [Authorize]
    public class LoginController : Controller
    {
        // GET: Login
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(VerifyLogin elem)
        {
            if (ModelState.IsValid)
            {
                using (AreaEntities db = new AreaEntities())
                {
                    var UserValid = db.users.Any(data => data.Email == elem.Email && data.Password == elem.Password);
                    if (UserValid)
                    {
                        FormsAuthentication.SetAuthCookie(elem.Email, false);
                        return Redirect("/");
                    }
                }
            }
            return View(elem);
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(VerifyRegister elem)
        {
            if (ModelState.IsValid)
            {
                using (AreaEntities db = new AreaEntities())
                {
                    var tmp = await db.users.Where(m => m.Email == elem.Email).FirstOrDefaultAsync();
                    if (tmp != null)
                    {
                        ModelState.AddModelError("", "This Email is already taken.");
                        return View(elem);
                    }
                    user ToAdd = new user()
                    {
                        Email = elem.Email,
                        Name = elem.Name,
                        Password = elem.Password
                    };
                    db.users.Add(ToAdd);
                    await db.SaveChangesAsync();
                    FormsAuthentication.SetAuthCookie(elem.Email, false);
                    return Redirect("/");
                }
            }
            return View(elem);
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }

        [AllowAnonymous]
        public ActionResult Google()
        {
            string provider = "google";
            string returnUrl = "";
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        [AllowAnonymous]
        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OpenAuth.RequestAuthentication(Provider, ReturnUrl);
            }
        }
        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            string ProviderName = OpenAuth.GetProviderNameFromCurrentRequest();

            if (ProviderName == null || ProviderName == "")
            {
                NameValueCollection nvs = Request.QueryString;
                if (nvs.Count > 0)
                {
                    if (nvs["state"] != null)
                    {
                        NameValueCollection provideritem = HttpUtility.ParseQueryString(nvs["state"]);
                        if (provideritem["__provider__"] != null)
                        {
                            ProviderName = provideritem["__provider__"];
                        }
                    }
                }
            }
            GoogleOAuth2Client.RewriteRequest();

            var redirectUrl = Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl });
            var retUrl = returnUrl;
            var authResult = OpenAuth.VerifyAuthentication(redirectUrl);

            if (!authResult.IsSuccessful)
            {
                return Redirect(Url.Action("Index", "Login"));
            }

            // User has logged in with provider successfully
            // Check if user is already registered locally
            //You can call you user data access method to check and create users based on your 
            string Email = null;
            if (Email == null && authResult.ExtraData.ContainsKey("email"))
            {
                Email = authResult.ExtraData["email"];
            }
            using (AreaEntities db = new AreaEntities())
            {
                if (db.users.Any(m => m.Email == Email))
                {
                    FormsAuthentication.SetAuthCookie(Email, false);
                    return Redirect(Url.Action("Index", "Home"));
                }
            }


            //Get provider user details
            string ProviderUserId = authResult.ProviderUserId;
            string ProviderUserName = authResult.UserName;

            if (Email == null && authResult.ExtraData.ContainsKey("email"))
            {
                Email = authResult.ExtraData["email"];
            }

            if (User.Identity.IsAuthenticated)
            {
                // User is already authenticated, add the external login and redirect to return url
                //OpenAuth.AddAccountToExistingUser(ProviderName, ProviderUserId, ProviderUserName, User.Identity.Name);
                return Redirect(Url.Action("Index", "Home"));
            }
            else
            {
                // User is new, save email as username
                string membershipUserName = Email ?? ProviderUserId;
                using (AreaEntities db = new AreaEntities())
                {
                    user elem = new user()
                    {
                        Name = ProviderUserName,
                        Email = Email,
                        Password = ""
                    };
                    if (db.users.Any(m => m.Email == Email))
                    {
                        ViewBag.Message = "User cannot be created";
                        ModelState.AddModelError("", "This Email is already taken.");
                        return Redirect("/login/");
                    }
                    else
                    {
                        db.users.Add(elem);
                        db.SaveChanges();
                        FormsAuthentication.SetAuthCookie(Email, false);
                        return Redirect(Url.Action("Index", "Home"));
                    }
                }
                //var createResult = OpenAuth.CreateUser(ProviderName, ProviderUserId, ProviderUserName, membershipUserName);


            }
            return View();
        }

    }
}
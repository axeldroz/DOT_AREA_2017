using AREA.Models;
using AREA.Models.Entity;
using System;
using System.Collections.Generic;
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
                    bool UserValid = db.users.Any(data => data.Email == elem.Email && data.Password == elem.Password);
                    if (UserValid)
                    {
                        FormsAuthentication.SetAuthCookie(elem.Email, false);
                        return Redirect("/");
                    }
                }
            }
            return View(elem);
        }

        public ActionResult Register()
        {
            return View();
        }

       [HttpPost]
       [ValidateAntiForgeryToken]
       public ActionResult Register(VerifyRegister elem)
        {
            if (ModelState.IsValid)
            {
                using (AreaEntities db = new AreaEntities())
                {
                    user ToAdd = new user()
                    {
                        Email = elem.Email,
                        Name = elem.Name,
                        Password = elem.Password
                    };
                    db.users.Add(ToAdd);
                    db.SaveChanges();
                    FormsAuthentication.SetAuthCookie(elem.Email, false);
                    return Redirect("/login");
                }
            }
            return View(elem);
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }
    }
}
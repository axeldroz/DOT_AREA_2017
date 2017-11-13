using AREA.Models;
using AREA.Models.Entity;
using System;
using System.Collections.Generic;
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
    }
}
using AREA.Models.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AREA.Controllers
{
    public class MyServicesController : Controller
    {
        // GET: MyServices
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PostOnWallForEver()
        {
            /* Envoi base de donne */
            // token, Action = WaitForNothing, Reaction = PostOnWall
            return (RedirectToAction("Index", "Home"));
        }

        [HttpPost]
        public ActionResult AddService(string action, string reaction)
        {
            Debug.WriteLine("Oui");
            try
            {
                using (AreaEntities db = new AreaEntities())
                {
                    if (Session["Email"] == null)
                        return Json(new { success = false });
                    string Email = Session["Email"].ToString();
                    int user = db.users.Where(m => m.Email == Email).FirstOrDefault().Id;
                    action elem = new action()
                    {
                        Action1 = action,
                        Reaction = reaction,
                        Date = DateTime.Now,
                        Id_user = user,
                        Token = "" // Put the token here
                    };
                    db.actions.Add(elem);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new { success = true });
        }

        private async Task<string> GetToken()
        {
            return "";
        }
    }
}
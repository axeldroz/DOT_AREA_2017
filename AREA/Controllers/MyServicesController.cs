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
        private string Token = "";
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

        /*private Uri RedirectUri
        {
            get
            {
                UriBuilder uriBuilder = new UriBuilder(Request.Url)
                {
                    Query = null,
                    Fragment = 
            }null,
                    Path = Url.Action("FacebookToken")
                };
                return (uriBuilder.Uri);
        }
        
        private async Task<ActionResult> FacebookToken()
        {
            Facebook.FacebookClient fb = new Facebook.FacebookClient();
            Debug.WriteLine("Debug Uri : " + RedirectUri.AbsoluteUri);
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = "790703331101924",
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email"
            });
            return (Redirect(loginUrl.AbsoluteUri));
        }
        private async Task<ActionResult> FacebookToken(string code)
        {
            var fb = new Facebook.FacebookClient();
            dynamic result = await fb.PostTaskAsync("oauth/access_token",
                new
                {
                    client_id = "790703331101924",
                    client_secret = "555f37fad9618104665ce1c9ada19878",
                    redirect_uri = RedirectUri.AbsoluteUri,
                    code = code
                });
            return (RedirectToAction(RedirectUri.AbsoluteUri));
        }*/

        [HttpPost]
        public ActionResult AddService(string action, string reaction)
        {
            Debug.WriteLine("Oui");
            Token = "";
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
                        Token_facebook = db.users.Where(m => m.Id == user).FirstOrDefault().Token_facebook
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
    }
}
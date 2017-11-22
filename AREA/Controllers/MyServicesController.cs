using AREA.Models.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace AREA.Controllers
{
    [Authorize]
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

        [HttpPost]
        public string GetAction()
        {
            string result = null;
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                if (Session["Email"] == null)
                    return "Error";
                var ActionName = Action.AreaActionReactionManager.GetActionNames();
                result = jss.Serialize(ActionName);
                if (result == null)
                    return "error";

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        [HttpPost]
        public string GetReaction()
        {
            string result = null;
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                if (Session["Email"] == null)
                    return "Error";
                var ReactionName = Action.AreaActionReactionManager.GetReactionNames();
                result = jss.Serialize(ReactionName);
                if (result == null)
                    return "error";

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        [HttpPost]
        public string GetServices()
        {
            string result;
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                using (AreaEntities db = new AreaEntities())
                {
                    if (Session["Email"] == null)
                        return "Error";
                    string Email = Session["Email"].ToString();
                    int user = db.users.Where(m => m.Email == Email).FirstOrDefault().Id;
                    var services = db.actions.Where(m => m.Id_user == user).ToList();
                    result = jss.Serialize(services);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        [HttpPost]
        public String DeleteService(string id)
        {
            try
            {
                int id_service = Int32.Parse(id);
                using (AreaEntities db = new AreaEntities())
                {
                    if (Session["Email"] == null)
                        return "Error";
                    string Email = Session["Email"].ToString();
                    var service = db.actions.Where(m => m.Id == id_service).FirstOrDefault();
                    if (service.Equals(null))
                    {
                        return "error";
                    }
                    else
                    {
                        db.actions.Attach(service);
                        db.actions.Remove(service);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return "success";
        }
    }
}
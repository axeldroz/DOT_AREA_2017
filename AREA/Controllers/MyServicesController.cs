using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
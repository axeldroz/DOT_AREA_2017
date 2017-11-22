using AREA.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AREA.Action;
using AREA.Controllers;
using System.Web.Security;

namespace AREA
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Task task = new Task(() => AuthConfig.RegisterAuth());
            task.Start();
            AreaActionReactionManager mngr = new AreaActionReactionManager();
            mngr.RunAll();

        }
    }
}

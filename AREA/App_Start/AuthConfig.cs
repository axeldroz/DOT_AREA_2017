using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Membership.OpenAuth;
using DotNetOpenAuth.GoogleOAuth2;

namespace AREA.App_Start
{

    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            GoogleOAuth2Client clientGoog = new GoogleOAuth2Client("271715026055-qd1vef8o8jf17ituksv477apsvbd66dm.apps.googleusercontent.com", "bgD4e0mvw-xp6iByc9KpGBrG");
            IDictionary<string, string> extraData = new Dictionary<string, string>();
            OpenAuth.AuthenticationClients.Add("google", () => clientGoog, extraData);
        }
    }

}
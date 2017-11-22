using Facebook;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace AREA.Action
{
    public class AreaAction
    {
        public class ActionArgs : EventArgs
        {
            public string Token_facebook { get; set; }
            public string Token_google { get; set; }
            public TaskEventHandler TheReaction { get; set; }
            public string Arg1 { get; set; }
            public string Arg2 { get; set; }
        }
        public static async Task<int> WaitForNothing(ActionArgs args)
        {
            FacebookClient fb = new FacebookClient(args.Token_facebook);

            fb.GetCompleted +=
                (o, e) =>
                {
                    dynamic result = e.GetResultData();
                    if (e.Error == null)
                    {
                        args.Arg1 = result.message;
                        args.TheReaction(o, args);
                    }
                };
            await Task.Delay(200);
            await fb.GetTaskAsync("me/feed");
            return (0);
        }

        public static async Task<int> WhenCommentInPost(ActionArgs args)
        {
            FacebookClient fb = new FacebookClient(args.Token_facebook);
            JavaScriptSerializer jss = new JavaScriptSerializer();

            fb.GetCompleted +=
                (o, e) =>
                {
                    dynamic result = (IDictionary<string, object>)e.GetResultData();
                    if (e.Error == null)
                    {
                        Debug.WriteLine("result = ");
                        args.Arg1 = /*result[0].message + */" OKkkk " + result["data"][0]["message"];
                        args.TheReaction(o, args);
                        //Debug.WriteLine("When comment, post : " + result.message);
                    }
                };
            await Task.Delay(200);
            await fb.GetTaskAsync("me/feed");
            return (0);
        }

        public static async Task<int> WhenLike(ActionArgs args)
        {
            FacebookClient fb = new FacebookClient(args.Token_facebook);

            fb.GetCompleted +=
                (o, e) =>
                {
                    dynamic result = e.GetResultData();
                    if (e.Error == null)
                    {
                        Debug.WriteLine("Last like : " + result.created_time);
                        args.Arg1 = result.message;
                        args.TheReaction(o, args);
                    }
                };
            await Task.Delay(200);
            await fb.GetTaskAsync("me/likes");
            return (0);
        }
    }
}
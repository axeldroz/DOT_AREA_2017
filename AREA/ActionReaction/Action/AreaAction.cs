using AREA.Models.Entity;
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
            public action TheActionDb { get; set; }
            public string Arg1 { get; set; }
            public string Arg2 { get; set; }
            public string Arg3 { get; set; }
        }

        public static bool SaveLast(action tab, string arg)
        {
            using (AreaEntities db = new AreaEntities())
            {
                var elem = db.actions.Where(m => m.Id == tab.Id).FirstOrDefault();
                db.actions.Attach(elem);
                if (elem.Last_elem.Equals(arg))
                    return (false);
                elem.Last_elem = arg;
                db.Entry(elem).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return (true);
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

        public static async Task<int> WhenNewLikePage(ActionArgs args)
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
                        bool ok = true;

                        try
                        {
                            args.Arg1 = result["data"][0]["id"];
                            args.Arg2 = result["data"][0]["name"];
                            ok = SaveLast(args.TheActionDb, args.Arg1);
                            if (ok)
                                args.TheReaction(o, args);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Exception : " + ex.Message);
                        }
                    }
                    else
                        Console.WriteLine("NOP");
                };
            await Task.Delay(200);
            await fb.GetTaskAsync("me/likes");
            return (0);
        }

        public static async Task<int> WhenCommentInPost(ActionArgs args)
        {
            FacebookClient fb = new FacebookClient(args.Token_facebook);
            FacebookClient fb2 = new FacebookClient(args.Token_facebook);
            JavaScriptSerializer jss = new JavaScriptSerializer();


            fb2.GetCompleted +=
                (o, e) =>
                {
                    dynamic result = (IDictionary<string, object>)e.GetResultData();
                    if (e.Error == null)
                    {
                        int nb = 1;
                        string res = jss.Serialize(result);
                        if (((JsonArray)result["data"]).Count() > 0)
                        {
                            Debug.WriteLine("nb = " + nb);
                            Debug.WriteLine("json : " + res);
                            args.TheReaction(o, args);
                        }
                    }
                };

            fb.GetCompleted +=
                (o, e) =>
                {
                    dynamic result = (IDictionary<string, object>)e.GetResultData();
                    if (e.Error == null)
                    {
                        Debug.WriteLine("result = ");
                        args.Arg1 = " OKkkk " + result["data"][0]["message"];
                        string post_id = result["data"][0]["id"];
                        fb2.GetTaskAsync(post_id + "/comments");
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
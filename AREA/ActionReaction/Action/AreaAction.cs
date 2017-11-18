using Facebook;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

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
using Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AREA.Reaction
{
    public class AreaReaction
    {
        public class ReactionArgs : EventArgs
        {
            public string Token_facebook { get; set; }
            public string Token_google { get; set; }
            public string Arg1 { get; set; }
            public string Arg2 { get; set; }
        }
        public static async Task<int> PostOnMyWall(Action.AreaAction.ActionArgs args)
        {
            var fb = new FacebookClient(args.Token_facebook);

            fb.PostCompleted += (o, e) => {
                if (e.Error == null)
                {
                    var result = (IDictionary<string, object>)e.GetResultData();
                }
            };

            var parameters = new Dictionary<string, object>();
            parameters["message"] = " Hello :" + args.Arg1;
            await fb.PostTaskAsync("me/feed", parameters);
            return (0);
        }

        public static async Task<int> PostOnWall(Action.AreaAction.ActionArgs args)
        {
            var fb = new FacebookClient(args.Token_facebook);
            var fb2 = new FacebookClient(args.Token_facebook);

           fb2.GetCompleted += (o, e) => {
                if (e.Error == null)
                {
                    var result = (IDictionary<string, object>)e.GetResultData();
                    if (!result["id"].Equals(args.Arg1))
                    {
                        var parameters = new Dictionary<string, object>();
                        parameters["message"] = "I like " + args.Arg2 + "'s page ! ";
                        fb.PostTaskAsync("me/feed", parameters);
                    }
                        
                }
            };

            fb.PostCompleted += (o, e) => {
                if (e.Error == null)
                {
                    var result = (IDictionary<string, object>)e.GetResultData();
                }
            };
            await fb2.GetTaskAsync("me");

            return (0);
        }

        public static async Task<int> CommentAPost(Action.AreaAction.ActionArgs args)
        {
            try
            {
                var fb = new FacebookClient(args.Token_facebook);

                fb.PostCompleted += (o, e) =>
                {
                    if (e.Error == null)
                    {
                        var result = (IDictionary<string, object>)e.GetResultData();
                        Console.WriteLine("Comment posted !");
                    }
                    else
                        Console.WriteLine("Comment ERROR !");
                };

                Console.WriteLine("Reaction Comment");
                 var parameters = new Dictionary<string, object>();
                parameters["message"] = "Thanks !";
                await fb.PostTaskAsync(args.Arg1 + "/comments", parameters);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception e :" + e.Message);
            }
            return (0);
        }

        public static async Task<int> LikeAPost(Action.AreaAction.ActionArgs args)
        {
            var fb = new FacebookClient(args.Token_facebook);

            await fb.PostTaskAsync(args.Arg1 + "/likes", null);
            return (0);
        }
    }
}
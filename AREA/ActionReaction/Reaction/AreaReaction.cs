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
            public string Token { get; set; }
            public string Arg1 { get; set; }
            public string Arg2 { get; set; }
        }
        public static async Task<int> PostOnWall(ReactionArgs args)
        {
            var fb = new FacebookClient(args.Token);

            fb.PostCompleted += (o, e) => {
                if (e.Error == null)
                {
                    var result = (IDictionary<string, object>)e.GetResultData();
                    //string newPostId = (string)result.id;
                }
            };

            var parameters = new Dictionary<string, object>();
            parameters["message"] = " Hello";
            await fb.PostTaskAsync("me/feed", parameters);
            return (0);
        }
    }
}
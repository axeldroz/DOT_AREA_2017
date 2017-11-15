using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Facebook;
using System.Collections.Generic;

namespace AREA.Reaction
{
    public class FacebookReaction : IAreaReaction
    {
        private string _name;
       // private Dictionary<string, TaskFactory<int, string>> _methods; 

        public FacebookReaction(string name) : base()
        {
            _name = name;
            Init();
        }

        public void Init()
        {
            //_methods = new Dictionary<string, TaskFactory<int, string>>();
            //_methods.Add("PostOnWall", new TaskFactory<int,string,string>(PostOnWall));
        }

        public void Do(string token, string arg)
        {
            Task task = new Task(async () => await PostOnWall(token,arg));
            task.Start();
        }
        
        private static async Task<int> PostOnWall(string token, string arg)
        {
            var fb = new FacebookClient("access_token");

            fb.PostCompleted += (o, e) => {
                if (e.Error == null)
                {
                    var result = (IDictionary<string, object>)e.GetResultData();
                    //string newPostId = (string)result.id;
                }
            };

            var parameters = new Dictionary<string, object>();
            parameters["message"] = arg + " Hello";
            await fb.PostTaskAsync("me/feed", parameters);
            return (0);
        }
    }
}
using Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AREA.Action
{
    public class FacebookAction : AreaAction
    {
        private Reaction.IAreaReaction _reaction;
        string _token;
        string _actionName;
        public FacebookAction(string token, string actionName, Reaction.IAreaReaction reaction) : base()
        {
            _token = token;
            _reaction = reaction;
        }

        public void Run()
        {
            Task task = null;

            if (_actionName.Equals("WaitForNothing"))
            {
                task = new Task(() => WaitForNothing(_reaction)); 
            }
            if (task != null)
                task.Start();
        }

        public void WaitForNothing(Reaction.IAreaReaction react)
        {
            FacebookClient fb = new FacebookClient(_token);

            fb.GetCompleted +=
                (o, e) =>
                {
                    dynamic result = e.GetResultData();
                    if (e.Error == null)
                    {
                        react.Do(_token, result.message);
                    }
                };
            while (true)
            {
                Task.Delay(200);
                fb.GetTaskAsync("me/feed");
            }
        }
    }
}
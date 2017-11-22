using AREA.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AREA.Action
{
    public class AreaActionReactionManager
    {
        private Dictionary<string, TaskEventHandler> _actions;
        private Dictionary<string, TaskEventHandler> _reactions;
        public AreaActionReactionManager()
        {
            //_actions = new Dictionary<string, TaskEventHandler>();
            //_reactions = new Dictionary<string, TaskEventHandler>();
            Init();
        }
        /* Actions */
        public static Dictionary<string, TaskEventHandler> GetActionDict()
        {
            Dictionary<string, TaskEventHandler>  list = new Dictionary<string, TaskEventHandler>();

            list.Add("ForEver", (s, e) => Action.AreaAction.WaitForNothing((Action.AreaAction.ActionArgs)e));
            list.Add("WhenLike", (s, e) => Action.AreaAction.WhenLike((Action.AreaAction.ActionArgs)e));
            list.Add("WhenComment", (s, e) => Action.AreaAction.WhenCommentInPost((Action.AreaAction.ActionArgs)e));
            return (list);
        }
        
        /* Reactions */
        public static Dictionary<string, TaskEventHandler> GetReactionDict()
        {
            Dictionary<string, TaskEventHandler> list = new Dictionary<string, TaskEventHandler>();

            list.Add("PostOnWall", (s, e) => Reaction.AreaReaction.PostOnWall((Action.AreaAction.ActionArgs)e));

            return (list);
        }

        public static List<string> GetActionNames()
        {
            Dictionary<string, TaskEventHandler> dic = GetActionDict();
            List<string> list = new List<string>();

            foreach (KeyValuePair<string, TaskEventHandler> p in dic)
            {
                list.Add(p.Key);
            }
            return (list);
        }

        public static List<string> GetReactionNames()
        {
            Dictionary<string, TaskEventHandler> dic = GetReactionDict();
            List<string> list = new List<string>();

            foreach (KeyValuePair<string, TaskEventHandler> p in dic)
            {
                list.Add(p.Key);
            }
            return (list);
        }

        public void InitAction()
        {
            _actions.Add("ForEver", (s, e) => Action.AreaAction.WaitForNothing((Action.AreaAction.ActionArgs)e));
        }

        public void InitReaction()
        {
            _reactions.Add("PostOnWall", (s, e) => Reaction.AreaReaction.PostOnWall((Action.AreaAction.ActionArgs)e));
        }

        public void Init()
        {
            //InitAction();
            //InitReaction();
            _actions = GetActionDict();
            _reactions = GetReactionDict();
        }

        /*public void RunExample()
        {
            AreaAction.ActionArgs arg = new AreaAction.ActionArgs
            {
                Token = "",
                TheReaction = _reactions["PostOnWall"]
            };
            _actions["ForEver"](this, arg).Start();
        }*/

        public async Task<int> RunOne(string token_facebook, string token_google, string act, string react)
        {
            AreaAction.ActionArgs arg = new AreaAction.ActionArgs
            {
                Token_facebook = token_facebook,
                Token_google = token_google,
                TheReaction = _reactions["PostOnWall"]
            };
            _actions["WhenComment"](this, arg).Start();
            return (1);
        }

        public void Run()
        {
            //string token = ""; Get From DB
            //string actionName = ""; Get From DB
            //string reactionName = ""; Get From DB
            //FacebookAction action = new FacebookAction(token, actionName, new Reaction.FacebookReaction(reactionName));
            // foreach DateBase
        }

        public async Task<int> RunAsync()
        {
            using (AreaEntities db = new AreaEntities())
            {
                /* get record with where clause */
                //var user = await db.users.Where(m => m.Email == "bite").FirstOrDefaultAsync();
                //   {

                //}
                string token = "";
                var actions = await db.actions.ToListAsync();
                foreach (var a in actions)
                {
                    RunOne(a.Token_facebook, a.Token_google, a.Action1, a.Reaction);
                }
            }
            return (0);
        }

        public async Task<int> RunAll()
        {
            using (AreaEntities db = new AreaEntities())
            {
                string token = null;
                var actions = await db.actions.ToListAsync();

                if (actions.Count > 0)
                {
                    Debug.WriteLine("actions.Count = " + actions.Count);
                    foreach (var a in actions)
                    {
                        Debug.WriteLine("actions.Id" + a.Id);
                        Debug.WriteLine("actions.Action" + a.Action1);
                        Debug.WriteLine("action.Reaction" + a.Reaction);
                        Debug.WriteLine("action.Token_facebook" + a.Token_facebook);
                        if (a.Token_facebook != "" && a.Action1 != "" && a.Reaction != "")
                        {
                            Debug.WriteLine("HELLO");
                            RunOne(a.Token_facebook, a.Token_google, a.Action1, a.Reaction);
                        }
                    }
                }
            }
            await Task.Delay(10000);
            //RunAll();
            return (0);
        }
    }
}
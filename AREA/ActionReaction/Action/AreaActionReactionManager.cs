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
            Init();
        }
        /* Actions */
        public static Dictionary<string, TaskEventHandler> GetActionDict()
        {
            Dictionary<string, TaskEventHandler>  list = new Dictionary<string, TaskEventHandler>();

            list.Add("ForEver", (s, e) => Action.AreaAction.WaitForNothing((Action.AreaAction.ActionArgs)e));
            list.Add("WhenLike", (s, e) => Action.AreaAction.WhenLike((Action.AreaAction.ActionArgs)e));
            list.Add("WhenComment", (s, e) => Action.AreaAction.WhenCommentInPost((Action.AreaAction.ActionArgs)e));
            list.Add("WhenNewLikePage", (s, e) => Action.AreaAction.WhenNewLikePage((Action.AreaAction.ActionArgs)e));
            return (list);
        }
        
        /* Reactions */
        public static Dictionary<string, TaskEventHandler> GetReactionDict()
        {
            Dictionary<string, TaskEventHandler> list = new Dictionary<string, TaskEventHandler>();

            list.Add("PostOnWall", (s, e) => Reaction.AreaReaction.PostOnWall((Action.AreaAction.ActionArgs)e));
            list.Add("LikeAPost", (s, e) => Reaction.AreaReaction.LikeAPost((Action.AreaAction.ActionArgs)e));
            list.Add("CommentAPost", (s, e) => Reaction.AreaReaction.CommentAPost((Action.AreaAction.ActionArgs)e));
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

        public void Init()
        {
            _actions = GetActionDict();
            _reactions = GetReactionDict();
        }

        public async Task<int> RunOne(action act)
        {
            if (!GetActionNames().Contains(act.Action1))
                throw new Exception("Action not found : could be a wrong name");
            if (!GetReactionNames().Contains(act.Reaction))
                throw new Exception("Reaction not found : could be a wrong name");
            AreaAction.ActionArgs arg = new AreaAction.ActionArgs
            {
                Token_facebook = act.Token_facebook,
                Token_google = act.Token_google,
                TheReaction = _reactions[act.Reaction],
                TheActionDb = act
            };
            _actions[act.Action1](this, arg).Start();
            return (1);
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
                        Dictionary<string, List<string>> addArgument = new Dictionary<string, List<string>>();

                        if (a.Token_facebook != "" && a.Action1 != "" && a.Reaction != "")
                        {
                            Debug.WriteLine("HELLO");
                            try
                            {
                                RunOne(a);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Exception : " + e.Message);
                            }
                        }
                    }
                }
            }
            await Task.Delay(5000);
            RunAll();
            return (0);
        }
    }
}
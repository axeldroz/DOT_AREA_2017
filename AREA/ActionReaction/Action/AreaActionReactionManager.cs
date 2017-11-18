using AREA.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            _actions = new Dictionary<string, TaskEventHandler>();
            _reactions = new Dictionary<string, TaskEventHandler>();
        }

        public void InitAction()
        {
            _actions.Add("ForEver", (s, e) => Action.AreaAction.WaitForNothing((Action.AreaAction.ActionArgs)e));
        }

        public void InitReaction()
        {
            _reactions.Add("PostOnWall", (s, e) => Reaction.AreaReaction.PostOnWall((Reaction.AreaReaction.ReactionArgs)e));
        }

        public void Init()
        {
            InitAction();
            InitReaction();
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

        public void RunOne(string token_facebook, string token_google, string act, string react)
        {
            AreaAction.ActionArgs arg = new AreaAction.ActionArgs
            {
                Token_facebook = token_facebook,
                Token_google = token_google,
                TheReaction = _reactions["PostOnWall"]
            };
            _actions["ForEver"](this, arg).Start();
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
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session
{
    public class PushHttpHandler : AsyncHttpHandlerBase
    {
        protected async override Task<string> DoWork(ISessionStore db, string user, Dictionary<string, string> qsDict, long sessionId = 0, long contextId = 0)
        {
            var session = Session.GetSessionForUser(db, user);
            var currentContext = (contextId > 0) ? session.GetContext(contextId) : session.GetCurrentContext();
            int count = 0;
            string action = "add";
            if(currentContext.ContextId == 0)
            {
                action = "update";
                foreach (var qsPair in qsDict)
                {
                    await currentContext.UpdateValue(qsPair.Key, qsPair.Value);
                    count++;
                }
            }
            else
            {
                foreach (var qsPair in qsDict)
                {
                    await currentContext.AddValue(qsPair.Key, qsPair.Value);
                    count++;
                }
            }

            return action + " " + count;
        }
    }
}

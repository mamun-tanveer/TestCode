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
            var session = new Session(db, user);
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

        private async Task DoWork(string user, string customer, string order, long contextId)
        {
            var db = new LocalMongoDB();
            var session = new Session(db, user);
            var context = await session.CreateNewContext(contextId);
            long customerId, orderId; 
            if(long.TryParse(customer, out customerId)) await context.AddValue("AdId", customerId);
            if (long.TryParse(order, out orderId)) await context.AddValue("WoId", orderId);

        }
    }
}

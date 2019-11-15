using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session
{
    public class RegisterHttpHandler : AsyncHttpHandlerBase
    {
        protected async override Task<string> DoWork(ISessionStore db, string user, Dictionary<string, string> qsDict, long sessionId = 0, long contextId = 0)
        {
            var session = new Session(db, user);
            string customerTermId, orderTermId, externalSessionIdTxt; 
            qsDict.TryGetValue("customerTermId", out customerTermId);
            qsDict.TryGetValue("orderTermId", out orderTermId);
            if(qsDict.TryGetValue("extSessionId", out externalSessionIdTxt))
            {
                session.ExternalSessionId = long.Parse(externalSessionIdTxt); 
            }
            session.OrderTermId = orderTermId;
            session.CustomerTermId = customerTermId;
            await db.Write(Session.COLLECTION_NAME, session);
            return Newtonsoft.Json.JsonConvert.SerializeObject(session);
        }
    }
}

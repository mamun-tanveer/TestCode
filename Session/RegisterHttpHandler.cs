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
            var session = Session.GetSessionForUser(db, user);
            string customerTermId, orderTermId, externalSessionIdTxt; 
            if(qsDict.TryGetValue("customerTermId", out customerTermId)) session.CustomerTermId = customerTermId;
            if(qsDict.TryGetValue("orderTermId", out orderTermId)) session.OrderTermId = orderTermId;
            if(qsDict.TryGetValue("extSessionId", out externalSessionIdTxt)) session.ExternalSessionId = long.Parse(externalSessionIdTxt);
            await db.Write(Session.COLLECTION_NAME, session);
            return Newtonsoft.Json.JsonConvert.SerializeObject(session);
        }
    }
}

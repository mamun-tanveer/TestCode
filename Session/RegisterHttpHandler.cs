using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session
{
    public class RegisterHttpHandler : AsyncHttpHandlerBase
    {
        protected async override Task<string> DoWork(Session userSession, Dictionary<string, string> qsDict, long contextId = 0)
        {
            string customerTermId, orderTermId, externalSessionIdTxt; 
            if(qsDict.TryGetValue("customerTermId", out customerTermId)) userSession.CustomerTermId = customerTermId;
            if(qsDict.TryGetValue("orderTermId", out orderTermId)) userSession.OrderTermId = orderTermId;
            if(qsDict.TryGetValue("extSessionId", out externalSessionIdTxt)) userSession.ExternalSessionId = long.Parse(externalSessionIdTxt);
            await userSession.Save();
            return Newtonsoft.Json.JsonConvert.SerializeObject(userSession);
        }
    }
}

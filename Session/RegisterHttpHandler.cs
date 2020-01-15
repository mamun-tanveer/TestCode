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
            string customerTermId, orderTermId, externalSessionIdTxt, lastActiveTicks;
            bool saveIndicator = false;
            if (qsDict.TryGetValue("customerNetName", out customerTermId))
            {
                userSession.CustomerTermId = customerTermId;
                saveIndicator = true;
            }
            if (qsDict.TryGetValue("orderNetName", out orderTermId))
            {
                userSession.OrderTermId = orderTermId;
                saveIndicator = true;
            }

            if (qsDict.TryGetValue("extSessionId", out externalSessionIdTxt))
            {
                userSession.ExternalSessionId = long.Parse(externalSessionIdTxt);
                saveIndicator = true;
            }

            if (qsDict.TryGetValue("lastActiveTicks", out lastActiveTicks))
            {
                userSession.LastActiveTicks = long.Parse(lastActiveTicks);
                saveIndicator = true;
            }

            if(saveIndicator) await userSession.Save();

            return Newtonsoft.Json.JsonConvert.SerializeObject(userSession);
        }


    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace Session
{
    public class ReadHttpHandler : AsyncHttpHandlerBase
    {

        protected async override Task<string> DoWork(Session userSession, Dictionary<string, string> qsDict, long contextId = 0)
        {
            string returnValue = string.Empty;
            Context readContext = (contextId > 0) ? userSession.GetContext(contextId) : userSession.GetCurrentContext();
            string keyName = string.Empty;
            qsDict.TryGetValue("key", out keyName);
            if (readContext == null)
            {
                //just return the session 
                returnValue = Newtonsoft.Json.JsonConvert.SerializeObject(userSession);
            }
            else if (string.IsNullOrWhiteSpace(keyName))
            {
                //return all the context values
                var returnValues = await readContext.GetAllValues();
                returnValue = string.Join(";", returnValues);
            }
            else
            {
                returnValue = await readContext.GetValue<string>(keyName);
            }

            return returnValue;
        }
    }
}

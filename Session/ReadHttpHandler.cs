using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace Session
{
    public class ReadHttpHandler : AsyncHttpHandlerBase
    {

        protected async override Task<string> DoWork(ISessionStore db, string user, Dictionary<string, string> qsDict, long sessionId = 0, long contextId = 0)
        {
            string returnValue = string.Empty;
            var session = Session.GetSessionForUser(db, user);
            Context readContext = (contextId > 0) ? session.GetContext(contextId) : session.GetCurrentContext();
            string keyName = string.Empty;
            qsDict.TryGetValue("key", out keyName);
            if (readContext == null)
            {
                //just return the session 
                returnValue = Newtonsoft.Json.JsonConvert.SerializeObject(session);
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

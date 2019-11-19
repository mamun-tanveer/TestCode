using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session
{
    public class PushHttpHandler : AsyncHttpHandlerBase
    {
        protected async override Task<string> DoWork(Session userSession, Dictionary<string, string> qsDict, long contextId = 0)
        {
            var currentContext = (contextId > 0) ? userSession.GetContext(contextId) : userSession.GetCurrentContext();
            int count = 0;
            string action = "add";
            if(currentContext.ContextId == 0)
            {
                action = "update";                
                foreach (var qsPair in qsDict)
                {
                    if(qsPair.Key =="adId" || qsPair.Key == "WkordId")
                    {
                        action = "delete/add";
                        //mainframe special values, delete all, add the new one
                        await currentContext.DeleteValue("adId");
                        await currentContext.DeleteValue("WkordId");
                        await currentContext.AddValue(qsPair.Key, qsPair.Value);
                    }
                    else
                    {
                        await currentContext.UpdateValue(qsPair.Key, qsPair.Value);
                    }
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

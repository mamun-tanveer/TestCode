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
                //clear the previous context on update. empty key delet
                await currentContext.DeleteValue(string.Empty);
                             
                foreach (var qsPair in qsDict)
                {
                    await currentContext.AddValue(qsPair.Key, qsPair.Value);
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

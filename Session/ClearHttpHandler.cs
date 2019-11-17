using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session
{
    public class ClearHttpHandler : AsyncHttpHandlerBase
    {
        protected async override Task<string> DoWork(Session userSession, Dictionary<string, string> qsDict, long contextId = 0)
        {

            string returnMessage = string.Empty;
                        
            if(contextId == userSession.CurrentContextId)
            {
                if (contextId == 0)
                {
                    Context updatingContext = userSession.GetContext(0);
                    long deleteCount = await updatingContext.DeleteValue(string.Empty);
                    returnMessage = deleteCount + " values deleted";
                }
                else
                {
                    await userSession.ClearCurrentContext();
                    returnMessage = contextId + " cleared from session";
                }
            }
            else
            {
                returnMessage = contextId + " no longer assoicated to your session. Context frozen";
            }

            return returnMessage;
        }
    }
}

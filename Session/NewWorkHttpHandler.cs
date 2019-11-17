using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session
{
    public class NewWorkHttpHandler : AsyncHttpHandlerBase
    {
        protected async override Task<string> DoWork(Session userSession, Dictionary<string, string> qsDict, long contextId = 0)
        {
            if (contextId == 0)
            {
                throw new ArgumentNullException("Must supply a context id > 0 ");
            }

            var newContext = await userSession.CreateNewContext(contextId);

            return contextId.ToString();
        }
    }
}

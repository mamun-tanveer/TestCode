﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session
{
    public class ClearHttpHandler : AsyncHttpHandlerBase
    {
        protected async override Task<string> DoWork(ISessionStore db, string user, Dictionary<string, string> qsDict, long sessionId = 0, long contextId = 0)
        {

            string returnMessage = string.Empty;
            var userSession = Session.GetSessionForUser(db, user);
            
            if(contextId == 0)
            {
                Context updatingContext = userSession.GetContext(0);
                long deleteCount = await updatingContext.DeleteValue(string.Empty);
                returnMessage = deleteCount + " values deleted";    
            }
            else if(contextId == userSession.CurrentContextId)
            {
                await db.Write(Session.COLLECTION_NAME, userSession);
                returnMessage = contextId + " cleared from session";
            }
            else
            {
                returnMessage = contextId + " no longer assoicated to your session. Context frozen";
            }

            return returnMessage;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session
{
    public class Session : ISessionObject
    {
        private IStore mSessionDB;
        private IStore mContextDB;
        public object _id { get; set; }
        public string User { get; set;  }
        public string OrderTermId { get; set; }
        public string CustomerTermId { get; set; }
        public long ExternalSessionId { get; set; }
        public long CurrentContextId { get; set; }
        public long HkUpdateTicks { get; set; }
        
        public long LastActiveTicks { get; set; }

        public static Session GetSessionForUser(IStore sessionDb, IStore contextDb, string userName)
        {
            Session returnSession = null; 
            var task = Task.Run(async() => await sessionDb.Read<string, Session>(userName.ToLower(), "_id", userName.ToLower()));
            if (task.Wait(10000)) returnSession = task.Result.FirstOrDefault();

            if (returnSession == null)
            {
                //nothing in the database;
                returnSession = new Session(sessionDb, userName);
            }
            else
            {
                // need to reset the collaborator
                returnSession.mSessionDB = sessionDb;
                returnSession.mContextDB = contextDb;
            }
           
            return returnSession;
        }

        private Session(IStore db, string inputUserId)
        {
            _id = inputUserId;
            mSessionDB = db;
            User = inputUserId;
        }

        public Context GetContext(long contextId)
        {
            return new Context(contextId, User, mContextDB);
        }

        public Context GetCurrentContext()
        {
            return new Context(CurrentContextId, User, mContextDB);
        }

        public Task ClearCurrentContext()
        {
            CurrentContextId = 0;
            return mSessionDB.Write(this);
        }

        public async Task<Context> CreateNewContext(long contextId)
        {
            CurrentContextId = contextId;
            await mSessionDB.Write(this);
            return new Context(contextId, User, mContextDB);
        }

        public Task Save()
        {
            return mSessionDB.Write(this);
        }

        public async Task<Tuple<long, long, long>> HasChanges(DateTime since, long contextId = 0)
        {
            long sessionLastUpdate = await mSessionDB.HasChanges(User, since);
            Context context = (contextId == 0) ? GetCurrentContext() : GetContext(contextId);
            long contextLastUpdate = await context.HasChanges(since);
            return new Tuple<long, long, long>(sessionLastUpdate, contextLastUpdate, context.ContextId);
        }
    }
}

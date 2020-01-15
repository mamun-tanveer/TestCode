using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session
{
    public class Session : ISessionObject
    {
        public const string COLLECTION_NAME = "Header";

        private ISessionStore mSessionDB;
        public object _id { get; set; }
        public string User { get; set;  }
        public string OrderTermId { get; set; }
        public string CustomerTermId { get; set; }
        public long ExternalSessionId { get; set; }
        public long CurrentContextId { get; set; }
        public long HkUpdateTicks { get; set; }
        
        public long LastActiveTicks { get; set; }

        public static Session GetSessionForUser(ISessionStore db, string userName)
        {
            Session returnSession = null; 
            var task = Task.Run(async() => await db.Read<string, Session>(COLLECTION_NAME, userName.ToLower(), "_id", userName.ToLower()));
            if (task.Wait(10000)) returnSession = task.Result.FirstOrDefault();

            if (returnSession == null)
            {
                //nothing in the database;
                returnSession = new Session(db, userName);
            }
            else
            {
                // need to reset the collaborator
                returnSession.mSessionDB = db;
            }
           
            return returnSession;
        }

        private Session(ISessionStore db, string inputUserId)
        {
            _id = inputUserId;
            mSessionDB = db;
            User = inputUserId;
        }

        public Context GetContext(long contextId)
        {
            return new Context(contextId, User, mSessionDB);
        }

        public Context GetCurrentContext()
        {
            return new Context(CurrentContextId, User, mSessionDB);
        }

        public Task ClearCurrentContext()
        {
            CurrentContextId = 0;
            return mSessionDB.Write(COLLECTION_NAME, this);
        }

        public async Task<Context> CreateNewContext(long contextId)
        {
            CurrentContextId = contextId;
            await mSessionDB.Write(COLLECTION_NAME, this);
            return new Context(contextId, User, mSessionDB);
        }

        public Task Save()
        {
            return mSessionDB.Write(COLLECTION_NAME, this);
        }

        public async Task<Tuple<long, long, long>> HasChanges(DateTime since, long contextId = 0)
        {
            long sessionLastUpdate = await mSessionDB.HasChanges(COLLECTION_NAME, User, since);
            Context context = (contextId == 0) ? GetCurrentContext() : GetContext(contextId);
            long contextLastUpdate = await context.HasChanges(since);
            return new Tuple<long, long, long>(sessionLastUpdate, contextLastUpdate, context.ContextId);
        }
    }
}

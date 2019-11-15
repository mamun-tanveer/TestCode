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

        public Session(ISessionStore db, string inputUserId)
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

        public void ClearCurrentContext()
        {
            CurrentContextId = 0;
            mSessionDB.Write(COLLECTION_NAME, this);
        }

        public async Task<Context> CreateNewContext(long workloadId)
        {
            CurrentContextId = workloadId;
            await mSessionDB.Write(COLLECTION_NAME, this);
            return new Context(workloadId, User, mSessionDB);
        }

        public async Task<Session> RefreshFromDB()
        {
            var matches = await mSessionDB.Read<string, Session>(COLLECTION_NAME, "_id", _id.ToString());
            var dbSession = matches.FirstOrDefault();
            if(dbSession?._id == _id)
            {
                User = dbSession.User;
                OrderTermId = dbSession.OrderTermId;
                CustomerTermId = dbSession.CustomerTermId;
                ExternalSessionId = dbSession.ExternalSessionId;
                CurrentContextId = dbSession.CurrentContextId;               
            }

            return this;
        }
    }
}

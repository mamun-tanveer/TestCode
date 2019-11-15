using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session
{
    public class Session : ISessionObject
    {
        public const string COLLECTION_NAME = "HEADER";

        private ISessionStore mSessionDB;
        public object _id { get; set; }
        public string UserId { get; set;  }
        public string OrderTermId { get; set; }
        public string CustomerTermId { get; set; }
        public long ExternalSessionId { get; set; }
        public long CurrentContextId { get; set; }

        public Session(ISessionStore db, object sessionID)
        {
            _id = sessionID;
            mSessionDB = db;
        }

        public Context GetContext(long contextId)
        {
            return new Context(_id, contextId, mSessionDB);
        }

        public Context GetCurrentContext()
        {
            return new Context(_id, CurrentContextId, mSessionDB);
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
            return new Context(_id, workloadId, mSessionDB);
        }

        public async Task<Session> RefreshFromDB()
        {
            var matches = await mSessionDB.Read<string, Session>(COLLECTION_NAME, "_id", UserId);
            var dbSession = matches.FirstOrDefault();
            if(dbSession?._id == _id)
            {
                UserId = dbSession.UserId;
                OrderTermId = dbSession.OrderTermId;
                CustomerTermId = dbSession.CustomerTermId;
                ExternalSessionId = dbSession.ExternalSessionId;
                CurrentContextId = dbSession.CurrentContextId;
            }

            return dbSession;
        }
    }
}

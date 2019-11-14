using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session
{
    public class Session : ISessionObject
    {
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
            mSessionDB.Write("Header", this);
        }

        public async Task<Context> CreateNewContext(long workloadId)
        {
            CurrentContextId = workloadId;
            await mSessionDB.Write("Header", this);
            return new Context(_id, workloadId, mSessionDB);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session
{
    public class ChangesResponse
    {
        public bool RegistrationChanged { get; set; }
       
        public bool ContextChanged { get; set; }

        public long Since { get; set; }

        public long LastUpdate { get; set; }

        public long ContextId { get; set; }

        public ChangesResponse(long sessionLastUpdateTicks, long contextLastUpdateTicks, long inputContextId, DateTime since)
        {
            Since = since.Ticks;
            RegistrationChanged = (sessionLastUpdateTicks > Since);
            ContextChanged = (contextLastUpdateTicks > Since);
            LastUpdate = Math.Max(sessionLastUpdateTicks, contextLastUpdateTicks);
            ContextId = inputContextId;        
        }
    }

}

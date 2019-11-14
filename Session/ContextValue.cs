using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session
{
    internal class ContextValue<T> : ISessionObject
    {
        public object _id { get; set; }
        public long HkUpdateTicks { get; set; }
        public string Key { get; set; }
        public T Value { get; set; }
        public long ContextId { get; set;} 
    }
}

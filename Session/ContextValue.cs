using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session
{
   
    internal class ContextValue : ISessionObject
    {
        public object _id { get; set; }
        public long HkUpdateTicks { get; set; }
        public string User { get; set; }
        public string Key { get; set; }
        public object Value { get; set; }
        public long ContextId { get; set;}

        public T GetValue<T>()
        {
            return (T)Value;
        }
        public override string ToString()
        {
            return Key + ": " + Value.ToString();
        }
    }
}

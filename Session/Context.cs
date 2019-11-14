using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session
{
    public class Context
    {
        public object SessionId { get; private set; }
        public long ContextId { get; private set; }

        private ISessionStore mSessionDB;

        public Context(object inputSessionId, long inputContextId, ISessionStore db)
        {
            SessionId = inputSessionId;
            ContextId = inputContextId;

            mSessionDB = db;
        }

        public async Task<IEnumerable<string>> GetAllValues()
        {
            var dbValues = await mSessionDB.Read<ContextValue<dynamic>>("Context", "ContextId", ContextId.ToString());
            IEnumerable<string> stringValues = dbValues.Select(x => x.ToString());
            return stringValues;    
        }

        public async Task<string> GetValueText(string key)            
        {
            var dbValues = await mSessionDB.Read<ContextValue<dynamic>>("Context", "ContextId", ContextId.ToString());
            string stringValue = dbValues.First().ToString();
            return stringValue;
        }

        public T GetValue<T>(string key)
        {
            throw new NotImplementedException();
        }

        public Task AddValue<T>(string key, T newValue)
        {
            var value = new ContextValue<T> { ContextId = ContextId, Key = key, Value = newValue, HkUpdateTicks = DateTime.Now.Ticks };
            return mSessionDB.Write("Context", value);
        }

        public long UpdateValue<T>(string key, T newValue, long lastUpdateTicks = 0)
        {
            throw new NotImplementedException();
        }

        public void DeleteValue(string key)
        {
            throw new NotImplementedException();
        }
    }
}

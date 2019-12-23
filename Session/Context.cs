using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session
{
    public class Context
    {
        public const string COLLECTION_NAME = "Context";
        
        public string SessionUser { get; private set; } 
        public long ContextId { get; private set; }

        private ISessionStore mSessionDB;

        public Context(long inputContextId, string userName,  ISessionStore db)
        {
            ContextId = inputContextId;
            SessionUser = userName;
            mSessionDB = db;
        }

        public async Task<IEnumerable<string>> GetAllValues()
        {
            var dbValues = await mSessionDB.ReadAll<ContextValue<string>>(COLLECTION_NAME, SessionUser, ContextId);
            IEnumerable<string> stringValues = dbValues.Select(x => x.ToString());
            return stringValues;    
        }

        public async Task<T> GetValue<T>(string key)
        {
            var dbValues = await mSessionDB.Read<string, ContextValue<T>>(COLLECTION_NAME, SessionUser, "Key", key, ContextId);

            ContextValue<T> firstValue = dbValues.FirstOrDefault();
            if (firstValue == null) return default(T);
            else return firstValue.Value;                        
        }

        public Task AddValue<T>(string key, T newValue)
        {
            var value = new ContextValue<T> { ContextId = ContextId, User = SessionUser, Key = key, Value = newValue, HkUpdateTicks = DateTime.Now.Ticks };
            return mSessionDB.Write(COLLECTION_NAME, value);
        }

        public async Task UpdateValue<T>(string key, T newValue)
        {
            await mSessionDB.Delete(COLLECTION_NAME, SessionUser, "Key", key, ContextId);
            var valueObject = new ContextValue<T>{ ContextId = ContextId, User = SessionUser, Key = key, Value = newValue };
            await mSessionDB.Write(COLLECTION_NAME, valueObject);
        }

        public Task<long> DeleteValue(string key)
        {
            if (string.IsNullOrEmpty(key)) return mSessionDB.Delete(COLLECTION_NAME, SessionUser, string.Empty, string.Empty, ContextId);
            else return mSessionDB.Delete(COLLECTION_NAME, SessionUser, "Key", key, ContextId);
        }

        public Task<long> HasChanges(DateTime since)
        {
            return mSessionDB.HasChanges(COLLECTION_NAME, SessionUser, since, ContextId);
        }
    }
}

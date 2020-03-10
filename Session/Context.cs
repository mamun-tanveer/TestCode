using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

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
            var dbValues = await mSessionDB.ReadAll<ContextValue>(COLLECTION_NAME, SessionUser, ContextId);
            IEnumerable<string> stringValues = dbValues.Select(x => x.ToString());
            return stringValues;    
        }

        public async Task<string> GetValuesJson(string key)
        {
            List<ContextValue> dbValues = await mSessionDB.Read<string, ContextValue>(COLLECTION_NAME, SessionUser, "Key", key, ContextId);
            if(dbValues != null)
            {
                IEnumerable<string> returnValues = dbValues.OrderByDescending(x=>x.HkUpdateTicks).Select(x => x.Value);
                return returnValues.ToJson();
            }
            else
            {
                return string.Empty;
            }                       
        }

        public Task AddValue(string key, string newValue)
        {
            var value = new ContextValue { ContextId = ContextId, User = SessionUser, Key = key, Value = newValue, HkUpdateTicks = DateTime.UtcNow.Ticks };
            return mSessionDB.Write(COLLECTION_NAME, value);
        }

        public async Task UpdateValue(string key, string newValue)
        {
            await mSessionDB.Delete(COLLECTION_NAME, SessionUser, "Key", key, ContextId);
            var valueObject = new ContextValue { ContextId = ContextId, User = SessionUser, Key = key, Value = newValue };
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

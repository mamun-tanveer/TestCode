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
        public string SessionUser { get; private set; } 
        public long ContextId { get; private set; }

        private IStore mContextDB;

        public Context(long inputContextId, string userName,  IStore db)
        {
            ContextId = inputContextId;
            SessionUser = userName;
            mContextDB = db;
        }

        public async Task<IEnumerable<string>> GetAllValues()
        {
            var dbValues = await mContextDB.ReadAll<ContextValue>(SessionUser, ContextId);
            IEnumerable<string> stringValues = dbValues.Select(x => x.ToString());
            return stringValues;    
        }

        public async Task<string> GetValuesJson(string key)
        {
            List<ContextValue> dbValues = await mContextDB.Read<string, ContextValue>(SessionUser, "Key", key, ContextId);
            if(dbValues != null)
            {
                IEnumerable<dynamic> returnValues = dbValues.OrderByDescending(x=>x.HkUpdateTicks).Select(x => x.Value);
                return returnValues.ToJson();
            }
            else
            {
                return string.Empty;
            }                       
        }

        public Task AddValue<T>(string key, T newValue)
        {
            var value = new ContextValue { ContextId = ContextId, User = SessionUser, Key = key, Value = newValue, HkUpdateTicks = DateTime.UtcNow.Ticks };
            return mContextDB.Write(value);
        }

        public Task<long> DeleteValue(string key)
        {
            if (string.IsNullOrEmpty(key)) return mContextDB.Delete(SessionUser, string.Empty, string.Empty, ContextId);
            else return mContextDB.Delete(SessionUser, "Key", key, ContextId);
        }

        public Task<long> HasChanges(DateTime since)
        {
            return mContextDB.HasChanges(SessionUser, since, ContextId);
        }
    }
}

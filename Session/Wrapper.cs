using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session
{
    public static class Wrapper
    {
        public const int TIMEOUT_MS = 10000; 
        public static Session GetSession()
        {
            return Session.GetSessionForUser(new LocalMongoDB(), Environment.UserName);            
        }

        public static Context GetContext(long contextId = 0)
        {
            if (contextId > 0) return GetSession().GetContext(contextId);
            else return GetSession().GetCurrentContext();
        }

        public static T GetValue<T>(string key, long contextId = 0)
        {
            var context = GetContext(contextId);
            var task = Task.Run(async () => await context.GetValue<T>(key));
            if (task.Wait(TIMEOUT_MS)) return task.Result;
            else throw new TimeoutException("GetValue exceeded timeout ms = " + TIMEOUT_MS);
        }

        public static void AddValue<T>(string key, T value, long contextId = 0)
        {
            var context = GetContext(contextId);
            context.AddValue(key, value);
        }

        public static void UpdateValue<T>(string key, T value, long contextId = 0)
        {
            var context = GetContext(contextId);
            var task = Task.Run(async () => await context.UpdateValue(key, value));
            if(!task.Wait(TIMEOUT_MS))
            {
                throw new TimeoutException("UpdateValue exceed timeout ms = " + TIMEOUT_MS);
            }
        }

        public static void DeleteValue(string key, long contextId = 0)
        {
            var context = GetContext(contextId);
            context.DeleteValue(key);
        }
    }
}

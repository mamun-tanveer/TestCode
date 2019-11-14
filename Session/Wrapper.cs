using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session
{
    public static class Wrapper
    {
        public static Session GetSession(long sessionId = 0)
        {
            if (sessionId > 0) return new Session(getDatabase(), sessionId);
            else return retrieveSessionForUser(Environment.UserName);
        }

        public static Context GetContext(long contextId = 0)
        {
            if (contextId > 0) return GetSession().GetContext(contextId);
            else return GetSession().GetCurrentContext();
        }

        public static T GetValue<T>(string key, long contextId = 0)
        {
            var context = GetContext(contextId);
            return context.GetValue<T>(key);
        }

        public static void AddValue<T>(string key, T value, long contextId = 0)
        {
            var context = GetContext(contextId);
            context.AddValue(key, value);
        }

        public static long UpdateValue<T>(string key, T value, long updateTicks = 0, long contextId = 0)
        {
            var context = GetContext(contextId);
            return context.UpdateValue(key, value, updateTicks);
        }

        public static void DeleteValue(string key, long contextId = 0)
        {
            var context = GetContext(contextId);
            context.DeleteValue(key);
        }

        private static ISessionStore getDatabase()
        {
            throw new NotImplementedException();
        }

        private static Session retrieveSessionForUser(string userId)
        {
            throw new NotImplementedException();
        }
    }
}

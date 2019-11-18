using System;
using System.Threading.Tasks;
using System.Web;
using System.Collections.Generic;

namespace Session
{
    public abstract class AsyncHttpHandlerBase : HttpTaskAsyncHandler
    {
        public async override Task ProcessRequestAsync(HttpContext context)
        {
            try
            {
                context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                context.Response.Cache.SetNoStore();
                context.Response.Cache.SetNoServerCaching();

                string user = string.Empty;
                long contextId = 0;
                var qsDict = new Dictionary<string, string>();
                foreach (string qsName in context.Request.QueryString.AllKeys)
                {
                    switch (qsName.ToLower())
                    {
                        case "user":
                            user = getQSValue(context, qsName).ToLower();
                            break;
                        case "context":
                            long.TryParse(getQSValue(context, qsName), out contextId);
                            break;
                        default:
                            qsDict[qsName] = getQSValue(context, qsName);
                            break;
                    }
                }
                if (string.IsNullOrEmpty(user))
                {
                    context.Response.Write("Must Supply a User");
                }
                else
                {
                    ISessionStore db = new LocalMongoDB();

                    string result = await DoWork(Session.GetSessionForUser(db, user), qsDict, contextId);
                    context.Response.Write(result);
                    context.Response.ContentType = "text/json";
                }
            }
            catch (Exception ex)
            {
                handleFailures(context, ex);
            }
        }

        private string getQSValue(HttpContext context, string key)
        {
            return context.Request.QueryString.Get(key);
        }

        private void handleFailures(HttpContext context, Exception ex)
        {
            context.Response.StatusCode = 500;
            context.Response.Write(ex.ToString());
            context.Response.ContentType = "text/plain";
        }

        protected abstract Task<string> DoWork(Session userSession, Dictionary<string, string> qsDict, long contextId = 0);

    }
}

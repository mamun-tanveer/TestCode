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
                string user = string.Empty;
                long contextId = 0; long sessionId = 0;
                var keys = new Dictionary<string, string>();
                foreach (string qsName in context.Request.QueryString.AllKeys)
                {
                    switch (qsName.ToLower())
                    {
                        case "user":
                            user = getQSValue(context, qsName);
                            break;
                        case "context":
                            long.TryParse(getQSValue(context, qsName), out contextId);
                            break;
                        case "session":
                            long.TryParse(getQSValue(context, qsName), out sessionId);
                            break;
                        default:
                            keys[qsName] = getQSValue(context, qsName);
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
                    string result = await DoWork(db, user, keys, contextId, sessionId);
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
        }

        protected abstract Task<string> DoWork(ISessionStore db, string user, Dictionary<string, string> qsDict, long sessionId = 0, long contextId = 0);

    }
}

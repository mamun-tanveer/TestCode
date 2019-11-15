using System;
using System.Threading.Tasks;
using System.Web;

namespace Session
{
    public class ReadHttpHandler : HttpTaskAsyncHandler
    {
      
        public async override Task ProcessRequestAsync(HttpContext context)
        {
            try
            {
                string user = string.Empty;
                string key = string.Empty;
                string format = string.Empty;
                long contextId = 0;
                foreach (string qsName in context.Request.QueryString.AllKeys)
                {
                    switch (qsName.ToLower())
                    {
                        case "user":
                            user = getQSValue(context, qsName);
                            break;
                        case "key":
                            key = getQSValue(context, qsName);
                            break;
                        case "format":
                            key = getQSValue(context, qsName);
                            break;
                        case "context":
                            long.TryParse(getQSValue(context, qsName), out contextId);
                            break;
                    }
                }
                if (string.IsNullOrEmpty(user))
                {
                    context.Response.Write("Must Supply a User");
                }
                else
                {
                    string result = await DoWork(user, key, format, contextId);
                    context.Response.Write(result);
                    context.Response.ContentType = "text/" + format;
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

        private async Task<string> DoWork(string user, string key, string format, long contextId)
        {
            string returnValue = string.Empty;
            var db = new LocalMongoDB();
            var session = new Session(db, user);
            session = await session.RefreshFromDB();
            Context readContext = (contextId > 0) ? session.GetContext(contextId) : session.GetCurrentContext();
            if (string.IsNullOrEmpty(format)) format = "json";
            if(readContext == null)
            {
                //just return the session 
                returnValue = Newtonsoft.Json.JsonConvert.SerializeObject(session);            
            }
            else if(string.IsNullOrEmpty(key))
            {
                //return all the context values
                var returnValues = await readContext.GetAllValues();
                returnValue = string.Join(";", returnValues);
            }
            else
            {
                returnValue = await readContext.GetValueText(key);
            }

            return returnValue;      
        }      
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Session
{
    public class PushHttpHandler : System.Web.HttpTaskAsyncHandler
    {
        public override async Task ProcessRequestAsync(HttpContext context)
        {
            try
            {
                string user = string.Empty;
                string customer = string.Empty;
                string order = string.Empty;
                long contextId = 0; 
                foreach(string key in context.Request.QueryString.AllKeys)
                {
                    switch(key.ToLower())
                    {
                        case "user":
                            user = getQSValue(context, key);
                            break;
                        case "customer":
                            customer = getQSValue(context, key);
                            break;
                        case "order":
                            order = getQSValue(context, key);
                            break;
                        case "context":
                            long.TryParse(getQSValue(context, key), out contextId);
                            break;                            
                    }
                }
                if(string.IsNullOrEmpty(user))
                {
                    context.Response.Write("Must Supply a User");
                }
                else
                {
                    await DoWork(user, customer, order, contextId);
                    context.Response.Write("Done");
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

        private async Task DoWork(string user, string customer, string order, long contextId)
        {
            var db = new LocalMongoDB();
            var session = new Session(db, user);
            var context = await session.CreateNewContext(contextId);
            long customerId, orderId; 
            if(long.TryParse(customer, out customerId)) await context.AddValue("AdId", customerId);
            if (long.TryParse(order, out orderId)) await context.AddValue("WoId", orderId);

        }
    }
}

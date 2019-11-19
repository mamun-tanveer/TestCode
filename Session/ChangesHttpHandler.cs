using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session
{
    public class ChangesHttpHandler : AsyncHttpHandlerBase
    {
        private const string TIME_FORMAT = "MM/dd/yyyy HH:mm:ss.fff";
        protected async override Task<string> DoWork(Session userSession, Dictionary<string, string> qsDict, long contextId = 0)
        {
            DateTime since = parseTime(qsDict["since"]);
            bool answer = await userSession.HasChanges(since, contextId);
            var response = new ChangesResponse { HasChanged = answer, Since = since };

            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
        }

        private DateTime parseTime(string qsValue)
        {
            DateTime returnValue = DateTime.Now; 
            if(string.IsNullOrEmpty(qsValue))
            {
                throw new ArgumentNullException("Must pass query string value since=[date time expression]");
            }

            long ticks; 
            if(DateTime.TryParse(qsValue, out returnValue))
            {
                if (returnValue > DateTime.Now)
                    throw new ArgumentNullException("Cannot pass a since time from the future " + returnValue.ToString(TIME_FORMAT));
            }
            else if(long.TryParse(qsValue, out ticks))
            {
                if (ticks < 0) returnValue = new DateTime(DateTime.Now.Ticks + ticks);
                else returnValue = new DateTime(ticks);
            }
            else
            {
                throw new ArgumentOutOfRangeException(qsValue + " not a parseable into a time since");
            }

            return returnValue;
        }
    }
}

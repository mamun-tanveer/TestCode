using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Linq;

namespace ResponseCompare
{
    public class RequestResponse : IRequestResponse
    {

        private HttpWebRequest request {get; set; }

        public string ID { get; private set; }
        public int ResponseCode { get; private set; } = 0;

        public IEnumerable<string> RegExs { get; private set; } = Enumerable.Empty<string>();

        public string ResponseText { get; private set; } = string.Empty;
       
        public string Url
        {
            get
            {
                string returnValue = string.Empty;
                if (request != null)
                {
                    returnValue = request.RequestUri.AbsolutePath;
                }
                return returnValue;
            }
        }

        public RequestResponse(string baseUrl, IRequestParse parsedText)
        {

            var uri = new Uri(new Uri(baseUrl), parsedText.UriStem + "?" + parsedText.QueryString); 
            request = (HttpWebRequest) HttpWebRequest.Create(uri);
            request.CookieContainer = parsedText.Cookies;
            request.UserAgent = parsedText.UserAgent;        
        }

        public void MakeRequest()
        {
            try
            {
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                ResponseCode = (int) response.StatusCode;

                using (StreamReader reader = new System.IO.StreamReader(request.GetResponse().GetResponseStream()))
                {
                    ResponseText = reader.ReadToEnd();
                }
            }
     
            catch (WebException ex)
            {
                ResponseCode = (int) ((HttpWebResponse)ex.Response).StatusCode;                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
      
        public string Serialize()
        {
            return string.Empty;   
        }
    }
}

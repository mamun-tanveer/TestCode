using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Xml;

namespace ResponseCompare
{
    class XmlFileParse : IRequestParse
    {
        public XmlFileParse(string filePath)
        {
            this.ID = System.IO.Path.GetFileName(filePath);
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);
            ID = xmlDoc.SelectSingleNode("ID").Value;
            QueryString = xmlDoc.SelectSingleNode("QueryString").Value;
            UriStem = xmlDoc.SelectSingleNode("UriStem").Value;
            UserAgent = xmlDoc.SelectSingleNode("UserAgent").Value;
            Response = xmlDoc.SelectSingleNode("Response").Value;            
        }

        private CookieContainer parseCookies(string cookiesText)
        {
            CookieContainer returnValue = new CookieContainer();
            foreach(string cookieText in cookiesText.Split(';'))
            {
                string[] cookieParts = cookieText.Split('=');
                
                returnValue.Add(new Cookie(cookieParts[0], cookieParts[1]));

            }

            return returnValue;
        }

        public string ID { get; set; }

        public CookieContainer Cookies { get; set; }

        public string QueryString { get; set; }

        public string UriStem { get; set; }

        public string UserAgent { get; set; }

        public IEnumerable<string> RegExs {get; set; }

        public string Response { get; set; }
    }
}

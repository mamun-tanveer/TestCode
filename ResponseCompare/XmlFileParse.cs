using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Linq;
using System.IO;
using System.Text;

namespace ResponseCompare
{
    class XmlFileParse : IRequestParse
    {
        public XmlFileParse(string filePath)
        {
            this.ID = Path.GetFileName(filePath);
            using (var reader = new StreamReader(filePath))
            {

            }
        }

        public string ID { get;  set; }

        public string InputText { get;  set; }

        public CookieContainer Cookies  {get; set; }

        public NameValueCollection QueryString { get; set; }

        public string Url { get; set; }

        public string UserAgent { get; set; }

        public string Response { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Linq;
using System.IO;
using System.Text;

namespace ResponseCompare
{
    class RequestFileParse : IRequestParse
    {
        public RequestFileParse(string filePath)
        {
            this.FilePath = filePath;
            using (var reader = new StreamReader(filePath))
            {
                //do work   
            }
        }

        public string FilePath { get; private set; }

        public string InputText { get; private set; }

        public CookieContainer Cookies  {get; set; }

        public NameValueCollection QueryString { get; set; }

        public string Url { get; set; }

        public string UserAgent { get; set; }
    }
}

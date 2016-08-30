using System.Collections.Generic;

namespace ResponseCompare
{
    public interface IRequestParse
    {
        string ID { get; set; }
        string UriStem { get; set; }
        string UserAgent { get; set; }
        IEnumerable<string> RegExs { get; set; }
        string QueryString { get; set; }
        System.Net.CookieContainer Cookies {get; set; }
    }
}

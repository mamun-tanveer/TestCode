namespace ResponseCompare
{
    public interface IRequestParse
    {
        string Url { get; set; }
        string UserAgent { get; set; }
        System.Collections.Specialized.NameValueCollection QueryString { get; set; }
        System.Net.CookieContainer Cookies {get; set; }
    }
}

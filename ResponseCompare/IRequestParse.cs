namespace ResponseCompare
{
    public interface IRequestParse
    {
        string ID { get; set; }
        string InputText { get; set; }
        string Url { get; set; }
        string UserAgent { get; set; }
        System.Collections.Specialized.NameValueCollection QueryString { get; set; }
        System.Net.CookieContainer Cookies {get; set; }
    }
}

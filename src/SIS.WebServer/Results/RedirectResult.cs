namespace SIS.WebServer.Results
{
    using HTTP.Responses;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Headers;

    public class RedirectResult : ActionResult
    {
        public RedirectResult(string location) : base(HttpResponseStatusCode.SeeOther)
        {
            this.Headers.Addheader(new HttpHeader("Location", location));
        }
    }
}


namespace SIS.WebServer.Results
{
    using HTTP.Responses;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Headers;
    using System.Text;

    public class HtmlResult : ActionResult
    {
        public HtmlResult(string content,HttpResponseStatusCode responseStatusCode)
            : base(responseStatusCode)
        {
            this.Headers.Addheader(new HttpHeader(
                "Content=Type", "text/html; charset=utd-8"));
            this.Content = Encoding.UTF8.GetBytes(content);
        }
    }
}

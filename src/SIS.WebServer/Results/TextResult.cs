namespace SIS.WebServer.Results
{
    using HTTP.Responses;
    using HTTP.Enums;
    using HTTP.Headers;
    using System.Text;

    public class TextResult : HttpResponse
    {
        public TextResult(string content, HttpResponseStatusCode responseStatusCode,
            string contentType = "text/plain; charset=utd-8") : base(responseStatusCode)
        {
            this.Headers.Addheader(new HttpHeader("Content-Type", contentType));
            this.Content = Encoding.UTF8.GetBytes(content);
        }

        public TextResult(byte[] content, HttpResponseStatusCode responseStatusCode,
    string contentType = "text/plain; charset=utd-8") : base(responseStatusCode)
        {
            this.Content = content;
            this.Headers.Addheader(new HttpHeader("Content-Type", contentType));
        }
    }
}

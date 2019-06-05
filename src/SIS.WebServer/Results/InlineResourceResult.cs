using SIS.HTTP.Enums;
using SIS.HTTP.Headers;

namespace SIS.WebServer.Results
{
    class InlineResourceResult : ActionResult
    {
        public InlineResourceResult(byte[] content,HttpResponseStatusCode responseStatusCode) : base(responseStatusCode)
        {
            this.Headers.Addheader(new HttpHeader(HttpHeader.ContentLenght, content.Length.ToString()));
            this.Headers.Addheader(new HttpHeader(HttpHeader.ContentDisposition, "inline"));
            this.Content = content;
        }
        
    }
}

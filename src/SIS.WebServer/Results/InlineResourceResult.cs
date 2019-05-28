using SIS.HTTP.Enums;
using SIS.HTTP.Headers;
using SIS.HTTP.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.WebServer.Results
{
    class InlineResourceResult : HttpResponse
    {
        public InlineResourceResult(byte[] content,HttpResponseStatusCode responseStatusCode) : base(responseStatusCode)
        {
            this.Headers.Addheader(new HttpHeader(HttpHeader.ContentLenght, content.Length.ToString()));
            this.Headers.Addheader(new HttpHeader(HttpHeader.ContentDisposition, "inline"));
            this.Content = content;
        }
        
    }
}

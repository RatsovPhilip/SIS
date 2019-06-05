using SIS.HTTP.Enums;
using SIS.HTTP.Headers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.WebServer.Results
{
    public class FileResult:ActionResult
    {
        public FileResult(byte[] fileContent, HttpResponseStatusCode httpResponseStatusCode = HttpResponseStatusCode.Ok) : base(httpResponseStatusCode)
        {
            this.Headers.Addheader(new HttpHeader(HttpHeader.ContentLenght, fileContent.Length.ToString()));
            this.Headers.Addheader(new HttpHeader(HttpHeader.ContentDisposition, "attachment"));
            this.Content = fileContent;
        }
    }
}

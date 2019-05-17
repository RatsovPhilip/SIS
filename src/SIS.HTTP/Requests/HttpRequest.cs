namespace SIS.HTTP.Requests
{
    using Common;
    using Enums;
    using Headers;
    using Headers.Contracts;
    using Requests.Contracts;
    using Exceptions;
    using System;
    using System.Collections.Generic;
    public class HttpRequest : IHttpRequest
    {

        public HttpRequest(string requestString)
        {
            CoreValidator.ThrowIfNullOrEmty(requestString, nameof(requestString));

            this.FormData = new Dictionary<string, object>();
            this.QueryData = new Dictionary<string, object>();
            this.Headers = new HttpHeaderCollection();

            this.ParseRequest(requestString);
        }


        public string Path { get; private set; }

        public string Url { get; private set; }

        public Dictionary<string, object> FormData { get; }

        public Dictionary<string, object> QueryData { get; }

        public IHttpHeaderCollection Headers { get; }

        public HttpRequestMethod RequestMethod { get; private set; }

        private void ParseRequest(string requestString)
        {
            string[] splitRequestContent = requestString
                .Split(new[] { GlobalConstants.HttpNewLine }, StringSplitOptions.None);

            string[] requestLine = splitRequestContent[0].Trim()
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (!this.IsValidRequestLine(requestLine))
            {
                throw new BadRequestException();
            }

            this.ParseRequestMethod(requestLine);
            this.ParseRequestUrl(requestLine);
            this.ParseRequestPath();

            this.ParseHeaders(splitRequestContent.Skip(1).ToArray());
            //this.ParseCookies();

            this.ParseRequestParameters(splitRequestContent[splitRequestContent.Length - 1]);
        }

        private bool IsValidRequestLine(string[] requestLine)
        {
            if (requestLine.Length == 3 && requestLine[2] == GlobalConstants.HttpOneprotocolFragment)
            {
                return true;
            }

            return false;
        }
    }
}

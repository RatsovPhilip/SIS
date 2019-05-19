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
    using System.Linq;
    using SIS.HTTP.Extensions;

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

        private bool IsValidRequestLine(string[] requestLine)
        {
            if (requestLine.Length == 3 && requestLine[2] == GlobalConstants.HttpOneprotocolFragment)
            {
                return true;
            }

            return false;
        }
        private bool IsValidRequestQueryString(string queryString, string[] queryParameters)
        {
            CoreValidator.ThrowIfNullOrEmty(queryString, nameof(queryString));

            return true;//TODO: RegEx querystring
        }

        private IEnumerable<string> ParsePlainRequestHeaders(string[] splitRequestContent)
        {
            for (int i = 1; i < splitRequestContent.Length - 1; i++)
            {
                if (!string.IsNullOrEmpty(splitRequestContent[i]))
                {
                    yield return splitRequestContent[i];
                }
            }
        }
        private void ParseRequestMethod(string[] requestLine)
        {
            string capitalizedMethod = StringExtensions.Capitalize(requestLine[0]);

            bool parseMethodResult = Enum.TryParse(capitalizedMethod, out HttpRequestMethod method);

            if (!parseMethodResult)
            {
                throw new BadRequestException
                    (string.Format(GlobalConstants.NotValidRequestMethodErrorMessage,
                            capitalizedMethod));
            }

            this.RequestMethod = method;
        }
        private void ParseRequestUrl(string[] requestLine)
        {
            this.Url = requestLine[1];
        }
        private void ParseRequestPath()
        {
            this.Path = this.Url.Split('?')[0];
        }
        private void ParseRequestHeaders(string[] requestLine)
        {
            //TODO : the headers are until the CLRF in the request line.
            //Check to see if the method stops at the right place!
            //It should not reach the body.
            //Throw and exception if there is no "Host" header!

            requestLine.Select(rl => rl.Split(new[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries))
            .ToList()
            .ForEach(headerKeyValuePair => this.Headers.Addheader(new HttpHeader(headerKeyValuePair[0], headerKeyValuePair[1])));
        }
        //private void ParseCookies()
        //{
        //    throw new NotImplementedException();
        //}
        private void ParseQueryParameters()
        {
            if (this.HasQueryString())
            {

                this.Url.Split('?', '#')[1]
                    .Split('&')
                    .Select(queryParam => queryParam.Split('='))
                    .ToList()
                    .ForEach(queryData => this.QueryData.Add(queryData[0], queryData[1]));
            }
        }

        private bool HasQueryString()
        {
            return this.Url.Split('?').Length > 1;
        }

        private void ParseFormDataParameters(string formData)
        {
            if (!string.IsNullOrEmpty(formData))
            {

                formData
                    .Split('&')
                    .Select(queryParam => queryParam.Split('='))
                    .ToList()
                    .ForEach(queryData => this.FormData.Add(queryData[0], queryData[1]));
            }
        }
        private void ParseRequestParameters(string formData)
        {
            this.ParseQueryParameters();
            this.ParseFormDataParameters(formData);
        }
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

            this.ParseRequestHeaders(this.ParsePlainRequestHeaders(splitRequestContent).ToArray());
            //this.ParseCookies();

            this.ParseRequestParameters(splitRequestContent[splitRequestContent.Length - 1]);
        }

    }
}

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
    using SIS.HTTP.Cookies.Contracts;
    using SIS.HTTP.Cookies;
    using SIS.HTTP.Sessions.Contracts;

    public class HttpRequest : IHttpRequest
    {

        public HttpRequest(string requestString)
        {
            CoreValidator.ThrowIfNullOrEmty(requestString, nameof(requestString));

            this.FormData = new Dictionary<string, object>();
            this.QueryData = new Dictionary<string, object>();
            this.Headers = new HttpHeaderCollection();
            this.Cookies = new HttpCookieCollection();

            this.ParseRequest(requestString);
        }


        public string Path { get; private set; }

        public string Url { get; private set; }

        public IHttpCookieCollection Cookies { get; }

        public Dictionary<string, object> FormData { get; }

        public Dictionary<string, object> QueryData { get; }

        public IHttpHeaderCollection Headers { get; }

        public HttpRequestMethod RequestMethod { get; private set; }
        public IHttpSession Session { get; set; }

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

            requestLine.Select(rl => rl.Split(new[] { ": " }, StringSplitOptions.RemoveEmptyEntries))
            .ToList()
            .ForEach(headerKeyValuePair => this.Headers.Addheader(new HttpHeader(headerKeyValuePair[0], headerKeyValuePair[1])));
        }

        private void ParseCookies()
        {
            if (this.Headers.ContainsHeader(HttpHeader.Cookies))
            {
                string cookie = this.Headers.GetHeader(HttpHeader.Cookies).Value;
                string[] values = cookie.Split(new[] { "; " }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var value in values)
                {
                    string[] keyValuePairs = value.Split('=');
                    HttpCookie httpCookie = new HttpCookie(keyValuePairs[0], keyValuePairs[1], false);
                    this.Cookies.AddCookie(httpCookie);
                }
            }
        }

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
            if (string.IsNullOrEmpty(formData) == false)
            {
                //TODO: Parse Multiple Parameters By Name
                var paramsPairs = formData
                   .Split('&')
                   .Select(plainQueryParameter => plainQueryParameter.Split('='))
                   .ToList();

                foreach (var paramPair in paramsPairs)
                {
                    string key = paramPair[0];
                    string value = paramPair[1];

                    if (this.FormData.ContainsKey(key) == false)
                    {
                        this.FormData.Add(key, new HashSet<string>());
                    }

                    ((ISet<string>)this.FormData[key]).Add(value);
                }
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
            this.ParseCookies();

            this.ParseRequestParameters(splitRequestContent[splitRequestContent.Length - 1]);
        }
    }
}

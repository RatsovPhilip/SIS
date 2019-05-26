namespace SIS.HTTP.Responses
{
    using Contracts;
    using SIS.HTTP.Common;
    using SIS.HTTP.Cookies;
    using SIS.HTTP.Cookies.Contracts;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Headers;
    using SIS.HTTP.Headers.Contracts;
    using System.Text;

    public class HttpResponse : IHttpResponse
    {

        public HttpResponse()
        {
            this.Headers = new HttpHeaderCollection();
            this.Content = new byte[0];
            this.Cookies = new HttpCookieCollection();
        }

        public HttpResponse(HttpResponseStatusCode statusCode) : this()
        {
            CoreValidator.ThrowIfNull(statusCode, nameof(statusCode));
            this.StatusCode = statusCode;
        }
        public HttpResponseStatusCode StatusCode { get; set; }

        public IHttpHeaderCollection Headers { get; }

        public IHttpCookieCollection Cookies { get; }

        public byte[] Content { get; set; }

        public void AddHeader(HttpHeader header)
        {
            CoreValidator.ThrowIfNull(header, nameof(header));
            this.Headers.Addheader(header);
        }

        public void AddCookie(HttpCookie cookie)
        {
            this.Cookies.AddCookie(cookie);
        }

        public byte[] GetBytes()
        {
            byte[] bytesWithoutBody = Encoding.UTF8.GetBytes(this.ToString());
            byte[] bytesWithBody = new byte[bytesWithoutBody.Length + this.Content.Length];


            for (int i = 0; i < bytesWithoutBody.Length; i++)
            {
                bytesWithBody[i] = bytesWithoutBody[i];
            }

            for (int i = 0; i < bytesWithBody.Length-bytesWithoutBody.Length; i++)
            {
                bytesWithBody[i+bytesWithoutBody.Length] = this.Content[i];
            }

            return bytesWithBody;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result
                .Append($"{GlobalConstants.HttpOneprotocolFragment} {(int)this.StatusCode} {this.StatusCode.ToString()}")
                .Append(GlobalConstants.HttpNewLine)
                .Append(this.Headers)
                .Append(GlobalConstants.HttpNewLine);

            if (this.Cookies.HasCookie())
            {
                result.Append($"Set-Cookie: {this.Cookies}").Append(GlobalConstants.HttpNewLine);
            }

            result.Append(GlobalConstants.HttpNewLine);

            return result.ToString();
        }
    }
}

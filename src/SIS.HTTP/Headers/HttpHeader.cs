using SIS.HTTP.Common;

namespace SIS.HTTP.Headers
{
    public class HttpHeader
    {
        public const string Cookies = "Cookie";

        public const string ContentType = "Content-Type";

        public const string ContentLenght = "Content-Lenght";

        public const string ContentDisposition = "Content-Disposition";

        public HttpHeader(string key, string value)
        {
            CoreValidator.ThrowIfNullOrEmty(key, nameof(key));
            CoreValidator.ThrowIfNullOrEmty(value, nameof(value));

            this.Key = key;
            this.Value = value;
        }

        public string Key { get; }
        public string Value { get; }

        public override string ToString() => $"{this.Key}: {this.Value}";

    }
}

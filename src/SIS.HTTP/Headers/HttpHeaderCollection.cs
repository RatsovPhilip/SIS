using SIS.HTTP.Headers.Contracts;
using System.Collections.Generic;

namespace SIS.HTTP.Headers
{
    public class HttpHeaderCollection : IHttpHeaderCollection
    {
        private readonly Dictionary<string, HttpHeader> heders;

        public HttpHeaderCollection()
        {
            this.heders = new Dictionary<string, HttpHeader>();
        }

        public void Addheader(HttpHeader header)
        {
            string key = header.Key;
            string value = header.Value;

            if (this.heders.ContainsKey(key) == false)
            {
                this.heders.Add(key, new HttpHeader(key, value));
            }
        }

        public bool ContainsHeader(string key)
        {
            throw new System.NotImplementedException();
        }

        public HttpHeader GetHeader(string key)
        {
            throw new System.NotImplementedException();
        }
    }
}

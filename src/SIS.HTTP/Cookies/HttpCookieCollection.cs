using SIS.HTTP.Common;
using SIS.HTTP.Cookies.Contracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP.Cookies
{
    public class HttpCookieCollection : IHttpCookieCollection
    {
        private Dictionary<string, HttpCookie> httpCookies;

        public HttpCookieCollection()
        {
            this.httpCookies = new Dictionary<string, HttpCookie>();
        }

        public void AddCookie(HttpCookie cookie)
        {
            CoreValidator.ThrowIfNull(cookie, nameof(cookie));

            if (!this.ContainsCookie(cookie.Key))
            {
                this.httpCookies.Add(cookie.Key, cookie);
            }
        }

        public bool ContainsCookie(string key)
        {
            CoreValidator.ThrowIfNullOrEmty(key, nameof(key));

            return this.httpCookies.ContainsKey(key);
        }

        public HttpCookie GetCookie(string key)
        {
            CoreValidator.ThrowIfNullOrEmty(key, nameof(key));

            return this.httpCookies[key];
        }

        public IEnumerator<HttpCookie> GetEnumerator()
        {
            return this.httpCookies.Values.GetEnumerator();
        }

        public bool HasCookie()
        {
            return this.httpCookies.Count != 0;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public override string ToString()
        {
            return string.Join(";", this.httpCookies.Values);
        }
    }
}

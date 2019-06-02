namespace SIS.WebServer.Routing
{
    using HTTP.Enums;
    using SIS.HTTP.Requests;
    using SIS.HTTP.Responses;
    using System;
    public interface IServerRoutingTable
    {
        void Add(HttpRequestMethod method, string path, Func<IHttpRequest, IHttpResponse> func);

        bool Contains(HttpRequestMethod requestMethod, string path);

        Func<IHttpRequest, IHttpResponse> Get(HttpRequestMethod requestMethod, string path);
    }
}

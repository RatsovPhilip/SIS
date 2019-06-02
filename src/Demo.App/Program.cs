using Demo.App.Controllers;
using SIS.HTTP.Enums;
using SIS.HTTP.Headers;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.WebServer;
using SIS.WebServer.Routing;
using System;
using System.Globalization;
using System.Text;

namespace Demo.App
{
    class Program
    {
        static void Main(string[] args)
        {
            //string request =
            //    "POST /url/asd?name=Krio&id=1#fragment HTTP/1.1\r\n"
            //    + "Authorization: Basic 12312312\r\n"
            //    + "Date:" + DateTime.Now + "\r\n"
            //    + "Host: localhost:5000\r\n"
            //    + "\r\n"
            //    + "username=KiroBacev&pass=123";

            //HttpRequest httpRequest = new HttpRequest(request);

            //HttpResponseStatusCode statusCode = HttpResponseStatusCode.Unauthorized;

            //HttpResponse response = new HttpResponse(HttpResponseStatusCode.InternalServerError);
            //response.AddHeader(new HttpHeader("Host", "localhost:5000"));
            //response.AddHeader(new HttpHeader("Date", DateTime.Now.ToString(CultureInfo.InvariantCulture)));

            //response.Content = Encoding.UTF8.GetBytes("<h1>Hello World!</h1>");

            //Console.WriteLine(Encoding.UTF8.GetString(response.GetBytes()));

            IServerRoutingTable serverRoutingTable = new ServerRoutingTable();

            serverRoutingTable.Add(HttpRequestMethod.Get, "/",
                request => new HomeController().Home(request));

            serverRoutingTable.Add(HttpRequestMethod.Get, "/login",
                request => new HomeController().Login(request));

            serverRoutingTable.Add(HttpRequestMethod.Get, "/logout",
    request => new HomeController().Logout(request));

            Server server = new Server(8000, serverRoutingTable);

            server.Run();

        }
    }
}

using SIS.HTTP.Cookies;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using SIS.WebServer.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Demo.App.Controllers

{
    public abstract class BaseController
    {
        protected IHttpRequest HttpRequest { get; set; }

        private bool IsLoggedIn()
        {
            return this.HttpRequest.Session.ContainsParameter("username");
        }
        private string ParseTemplete(string viewContent)
        {
            if (this.IsLoggedIn())
            {
                return viewContent.Replace("@Model.HelloMessage", $"Hello {this.HttpRequest.Session.GetParameter("username")}");

            }
            else
            {
                return viewContent.Replace("@Model.HelloMessage", "Hello guys, I am SIS the handmade server!");
            }
        }
        public IHttpResponse View([CallerMemberName] string view = null)
        {
            string controllerName = this.GetType().Name.Replace("Controller", string.Empty);
            string viewName = view;


            string viewContent = File.ReadAllText("Views/" + controllerName + "/" + viewName + ".html");
            viewContent = this.ParseTemplete(viewContent);

            HtmlResult htmlResult = new HtmlResult(viewContent, HttpResponseStatusCode.Ok);
            htmlResult.Cookies.AddCookie(new HttpCookie("lang", "en"));

            return htmlResult;
        }

        public IHttpResponse Redirect(string url)
        {
           return new RedirectResult(url);
        }
    }
}

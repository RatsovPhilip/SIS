using SIS.HTTP.Enums;
using SIS.HTTP.Requests;
using SIS.WebServer.Extensions;
using SIS.WebServer.Identity;
using SIS.WebServer.Results;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SIS.WebServer
{
    public abstract class Controller
    {
        protected Dictionary<string, object> ViewData;

        protected Controller()
        {
            ViewData = new Dictionary<string, object>();
        }

        protected Principal User => (Principal)this.Request.Session.GetParameter("principal");

        protected IHttpRequest Request { get; set; }

        private string ParseTemplate(string viewContent)
        {
            foreach (var param in ViewData)
            {
                viewContent = viewContent.Replace($"@Model.{param.Key}", param.Value.ToString());
            }

            return viewContent;
        }

        protected bool IsLoggedIn()
        {
            return this.Request.Session.ContainsParameter("username");
        }
        protected void SignOut()
        {
           this.Request.Session.ClearParameter();

        }

        protected void SignIn(string id, string username, string email)
        {
            this.Request.Session.AddParameter("principal", new Principal
            {
                Id = id,
                UserName = username, 
                Email = email
            });

        }

        protected ActionResult View([CallerMemberName] string view = null)
        {
            string controllerName = GetType().Name.Replace("Controller", string.Empty);
            string viewName = view;

            string viewContent = System.IO.File.ReadAllText("Views/" + controllerName + "/" + viewName + ".html");

            viewContent = ParseTemplate(viewContent);

            HtmlResult htmlResult = new HtmlResult(viewContent, HttpResponseStatusCode.Ok);

            return htmlResult;
        }

        protected ActionResult Redirect(string url)
        {
            return new RedirectResult(url);
        }

        protected ActionResult Xml(object param)
        {
            return new XmlResult(param.ToXml());
        }

        protected ActionResult Json(object param)
        {
            return new JsonResult(param.ToJson());
        }

        protected ActionResult File(byte[] fileContent)
        {
            return new FileResult(fileContent);

        }

        protected ActionResult NotFound(string message = "")
        {
            return new NotFoundResult(message);
        }
    }
}

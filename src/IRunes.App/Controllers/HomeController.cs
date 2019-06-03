using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.WebServer;
using SIS.WebServer.Atributes.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRunes.App.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet(Url = "/")]
        public IHttpResponse IndexSlash(IHttpRequest httpRequest)
        {
            return Index(httpRequest);
        }

        public IHttpResponse Index(IHttpRequest httpRequest)
        {
            if (this.IsLoggedIn(httpRequest))
            {
                this.ViewData["Username"] = httpRequest.Session.GetParameter("username");

                return this.View("Home");
            }

            return this.View();
        }
    }
}

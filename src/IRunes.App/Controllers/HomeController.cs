using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.WebServer;
using SIS.WebServer.Atributes.Http;
using SIS.WebServer.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRunes.App.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet(Url = "/")]
        public ActionResult IndexSlash()
        {
            return Index();
        }

        public ActionResult Index()
        {
            if (this.IsLoggedIn())
            { 
                this.ViewData["Username"] = this.User.UserName;

                return this.View("Home");
            }

            return this.View(); //10-0
        }
    }
}

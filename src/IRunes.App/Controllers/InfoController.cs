using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.WebServer;
using SIS.WebServer.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRunes.App.Controllers
{
    public class InfoController : Controller
    {
        public IHttpResponse About(IHttpRequest httpRequest)
        {
            return this.View();
        }

        public ActionResult Json(IHttpRequest request)
        {
            return Json(new
            {
                Name = "Pesho",
                Age = 25,
                Occupation = "Bezraboten",
                Married = false
            });
        }


    }
}

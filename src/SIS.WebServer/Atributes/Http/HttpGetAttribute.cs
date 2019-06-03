using System;
using System.Collections.Generic;
using System.Text;
using SIS.HTTP.Enums;

namespace SIS.WebServer.Atributes.Http
{
    public class HttpGetAttribute : BaseHttpAttribute
    {
        public override HttpRequestMethod Method => HttpRequestMethod.Get;
    }
}

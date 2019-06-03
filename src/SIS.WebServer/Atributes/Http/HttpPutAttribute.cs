using System;
using System.Collections.Generic;
using System.Text;
using SIS.HTTP.Enums;

namespace SIS.WebServer.Atributes.Http
{
    public class HttpPutAttribute : BaseHttpAttribute
    {
        public override HttpRequestMethod Method => HttpRequestMethod.Put;
    }
}

using SIS.HTTP.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.WebServer.Atributes.Http
{
    public abstract class BaseHttpAttribute : Attribute
    {
        public string ActionName { get; set; }

        public string Url { get; set; }

        public abstract HttpRequestMethod Method { get; }
    }
}

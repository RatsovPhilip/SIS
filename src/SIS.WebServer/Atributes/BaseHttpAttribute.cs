using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.WebServer.Atributes
{
    public abstract class BaseHttpAttribute : Attribute
    {
        public string ActionName { get; set; }

        public string Url { get; set; }
    }
}

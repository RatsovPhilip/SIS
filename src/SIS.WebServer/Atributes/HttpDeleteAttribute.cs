using SIS.HTTP.Enums;
using SIS.WebServer.Atributes;

namespace SIS.WebServer
{
    internal class HttpDeleteAttribute : BaseHttpAttribute
    {
        public override HttpRequestMethod Method => HttpRequestMethod.Delete;

    }
}
using SIS.HTTP.Enums;

namespace SIS.WebServer.Atributes.Http
{
    internal class HttpDeleteAttribute : BaseHttpAttribute
    {
        public override HttpRequestMethod Method => HttpRequestMethod.Delete;

    }
}
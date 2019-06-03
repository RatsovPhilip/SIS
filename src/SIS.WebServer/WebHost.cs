using SIS.HTTP.Enums;
using SIS.HTTP.Responses;
using SIS.WebServer.Atributes;
using SIS.WebServer.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SIS.WebServer
{
    public static class WebHost
    {
        public static void Start(IMvcApplication application)
        {
            IServerRoutingTable serverRoutingTable = new ServerRoutingTable();
            AutoRegisterRoutes(application, serverRoutingTable);
            application.ConfigureServices();
            application.Configure(serverRoutingTable);
            Server server = new Server(8000, serverRoutingTable);
            server.Run();
        }

        private static void AutoRegisterRoutes(IMvcApplication application, IServerRoutingTable serverRoutingTable)
        {
            var controllers = application.GetType()
                       .Assembly
                       .GetTypes()
                       .Where(type => type.IsClass && !type.IsAbstract
                                && typeof(Controller).IsAssignableFrom(type));

            foreach (var controller in controllers)
            {
                var actions = controller.GetMethods(BindingFlags.DeclaredOnly
                    | BindingFlags.Public
                    | BindingFlags.Instance)
                     .Where(x => !x.IsSpecialName);

                foreach (var action in actions)
                {
                    var url = $"/{controller.Name.Replace("Controller", string.Empty)}/{ action.Name}";
                    var attribute = action.GetCustomAttributes.Where(x => x.AttributeType.IsSubclassOf(typeof(BaseHttpAttribute))).LastOrDefault();
                    Console.WriteLine($"/{controller.Name.Replace("Controller", string.Empty)}/{action.Name}");
                    Console.WriteLine(attribute?.AttributeType.Name);
                    var httpMethod = HttpRequestMethod.Get;
                    if (attribute?.AttributeType == typeof(HttpPostAttribute))
                    {
                        httpMethod = HttpRequestMethod.Post;
                    }
                    else if (attribute?.AttributeType == typeof(HttpDeleteAttribute))
                    {
                        httpMethod = HttpRequestMethod.Delete;
                    }

                    if(attribute?.NamedArguments.Where(x=>x.))

                    serverRoutingTable.Add(httpMethod, url, request =>
                     {
                         var controllerInstance = Activator.CreateInstance(controller);
                         var response = action.Invoke(controllerInstance, new[] { request }) as IHttpResponse;
                         return response;
                     });
                }




            }
        }
    }
}

﻿using SIS.HTTP.Enums;
using SIS.HTTP.Responses;
using SIS.WebServer.Atributes.Action;
using SIS.WebServer.Atributes.Http;
using SIS.WebServer.Routing;
using System;
using System.Linq;
using System.Reflection;

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
                     .Where(x => !x.IsSpecialName)
                .Where(x => x.GetCustomAttributes().All(a => a.GetType() != typeof(NonActionAttribute)));

                foreach (var action in actions)
                {
                    var url = $"/{controller.Name.Replace("Controller", string.Empty)}/{ action.Name}";
                    var attribute = action.GetCustomAttributes()
                        .Where(x=>x.GetType().IsSubclassOf(typeof(BaseHttpAttribute)))
                        .LastOrDefault() as BaseHttpAttribute;
                    var httpMethod = HttpRequestMethod.Get;

                    if (attribute != null)
                    {
                        httpMethod = attribute.Method;
                    }

                    if (attribute?.Url != null)
                    {
                        url = attribute.Url;
                    }

                    if (attribute?.ActionName != null)
                    {
                        url = $"/{controller.Name.Replace("Controller", string.Empty)}/{attribute?.ActionName}";
                    }

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

namespace IRunes.App
{
    using SIS.WebServer;
    using SIS.WebServer.Results;
    using SIS.WebServer.Routing;
    using SIS.HTTP.Enums;
    using Data;
    using IRunes.App.Controllers;

    public class Launcher
    {
        public static void Main(string[] args)
        {
            using (var context = new RunesDbContext())
            {
                context.Database.EnsureCreated();
            }

            ServerRoutingTable serverRoutingTable = new ServerRoutingTable();
            Configure(serverRoutingTable);

            Server server = new Server(8000, serverRoutingTable);
            server.Run();
        }

        private static void Configure(ServerRoutingTable serverRoutingTable)
        {
            serverRoutingTable.Add(HttpRequestMethod.Get, "/", request => new RedirectResult("/Home/Index"));
            serverRoutingTable.Add(HttpRequestMethod.Get, "/Home/Index", request => new HomeController().Index(request));

        }
    }
}

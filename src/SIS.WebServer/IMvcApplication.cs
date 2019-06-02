using SIS.WebServer.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.WebServer
{
    public interface IMvcApplication
    {
        void Configure(ServerRoutingTable serverRoutingTable);
        void ConfigureServices();
    }
}

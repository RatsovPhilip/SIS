﻿namespace SIS.WebServer
{
    using Routing.Contracts;
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    public class Server
    {
        private const string LocalhostIpAddress = "127.0.0.1";

        private readonly int port;

        private readonly TcpListener tcpListener;

        private IServerRoutingTable serverRoutingTable;

        private bool isRunning;

        public Server(int port,IServerRoutingTable serverRoutingTable)
        {
            this.port = port;
            this.tcpListener = new TcpListener(IPAddress.Parse(LocalhostIpAddress), port);

            this.serverRoutingTable = serverRoutingTable;
        }

        public void Run()
        {
            this.tcpListener.Start();
            this.isRunning = true;

            Console.WriteLine($"Server start on http://{LocalhostIpAddress}:{this.port}");

            while (this.isRunning)
            {
                Console.WriteLine("Waiting for client...");

                var client = this.tcpListener.AcceptSocket();

                this.Listen(client);
            }
        }

        public void Listen(Socket client)
        {
            var connectionHandler = new ConnectionHandler(client,this.serverRoutingTable);
            connectionHandler.ProcessRequest();
        }
    }
}

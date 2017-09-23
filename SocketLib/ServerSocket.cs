using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SocketLib {
    
    public class ServerSocket {

        public delegate void ClientHandler(Client client);

        public event ClientHandler OnClientConnected;

        public event ClientHandler OnClientDisconnect;
        
        public readonly int port;
        protected TcpListener listener;
        protected Task listenTask;
        protected bool running;

        public List<Client> clients;
        
        public ServerSocket(int port) {
            this.port = port;
            this.listener = new TcpListener(IPAddress.Any, port);
            this.clients = new List<Client>();
        }

        public void start() {
            this.running = true;
            this.listener.Start();
            this.listenTask = new Task(() => {
                while (running) {
                    Client client = new Client(listener.AcceptSocket());
                    clients.Add(client);
                    if (OnClientConnected != null) OnClientConnected(client);
                    client.OnDisconnect += () => {
                        if (OnClientDisconnect != null) OnClientDisconnect(client);
                        this.clients.Remove(client);
                        client.Dispose();
                    };
                }
            });
            this.listenTask.RunSynchronously();
        }

        public void close() {
            this.clients.ForEach(client => client.close());
            this.listener.Stop();
        }
        
    }
}
using System.Collections.Generic;
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
            listener = new TcpListener(IPAddress.Any, port);
            clients = new List<Client>();
        }

        public void start() {
            running = true;
            listener.Start();
            listenTask = new Task(() => {
                while (running) {
                    Client client = new Client(listener.AcceptSocket());
                    clients.Add(client);
                    if (OnClientConnected != null) OnClientConnected(client);
                    client.OnDisconnect += () => {
                        if (OnClientDisconnect != null) OnClientDisconnect(client);
                        clients.Remove(client);
                        client.Dispose();
                    };
                }
            });
            listenTask.RunSynchronously();
        }

        public void close() {
            clients.ForEach(client => client.close());
            listener.Stop();
        }
        
    }
}
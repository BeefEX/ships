using System;
using System.Text;
using System.Threading.Tasks;
using SocketLib;

namespace Ships_Server {
    
    internal class Program {
        private static ServerSocket server;
        
        public static void Main(string[] args) {
            new Task (() => {
                string line = Console.ReadLine();
                if (line == "close" || line == "exit")
                    server.close();
            }).Start();
            
            server = new ServerSocket(8080);
            server.OnClientConnected += client => {
                Console.WriteLine("Client connected");
                client.OnMessage += message => {
                    if (message.StartsWith("cr"))
                    Console.WriteLine("Received => " + message);
                };
            };
            server.OnClientDisconnect += client => Console.WriteLine("Client disconnect");
            server.start();
        }
    }
}
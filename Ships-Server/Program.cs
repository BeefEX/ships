using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ships_Common.Net;
using Ships_Server.Handlers;
using Ships_Server.Rooms;
using SocketLib;

namespace Ships_Server {
    
    public struct Packet<T> {
        public RoomManager roomManager;
        public Client client;
        public T data;

        public Packet(RoomManager roomManager, Client client, T data) {
            this.roomManager = roomManager;
            this.client = client;
            this.data = data;
        }
    }
    
    internal class Program {
        
        private static ServerSocket server;
        private static RoomManager rooms;
        public static EventSystem<Packet<string[]>> eventSystem;
        
        public static void Main(string[] args) {
            eventSystem = new EventSystem<Packet<string[]>>();
            Server.Init();
            
            new Task (() => {
                string line = Console.ReadLine();
                if (line == "close" || line == "exit")
                    server.close();
            }).Start();
            
            rooms = new RoomManager();
            server = new ServerSocket(8080);
            
            server.OnClientConnected += client => {
                
                client.send(Encoding.ASCII.GetBytes("ig~00000000"));
                Console.WriteLine("Client connected");
                
                client.OnMessage += message => {
                    string[] packet = Packet.readPacket(Encoding.ASCII.GetBytes(message));
                    
                    Console.WriteLine("Received => " + message);
                    
                    eventSystem.Invoke(packet[0], new Packet<string[]>(rooms, client, packet.Skip(1).Take(packet.Length - 1).ToArray()));
                };
            };
            server.OnClientDisconnect += client => Console.WriteLine("Client disconnect");
            server.start();
        }
    }
}
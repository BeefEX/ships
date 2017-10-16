using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ships_Common;
using Ships_Common.Net;
using Ships_Server.Handlers;
using Ships_Server.Rooms;
using SocketLib;
using Client = Ships_Server.Rooms.Client;

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
        public static RoomManager rooms;
        public static EventSystem<Packet<string[]>> eventSystem;
        
        public static void Main(string[] args) {
            eventSystem = new EventSystem<Packet<string[]>>();
            Server.Init();
            Handlers.Room.Init();
            
            new Task (() => {
                while (true) {
                    var readLine = Console.ReadLine();
                    if (readLine == null)
                        continue;
                        
                    string[] command = readLine.Split(' ');
                    if (command[0] == "close" || command[0] == "exit") {
                        server.close();
                        return;
                    }

                    if (command[0] == "listRooms") {
                        foreach (Rooms.Room room in rooms.rooms.Values) {
                            Console.WriteLine(room.id + "\t-\t" + room.name + "\tIs open: " + room.open);
                        }
                    }
                }
            }).Start();
            
            rooms = new RoomManager();
            server = new ServerSocket(8080);
            
            server.OnClientConnected += client => {
                
                client.send(Encoding.ASCII.GetBytes("ig~00000000"));
                Console.WriteLine("Client connected");
                Client _client = new Client(client, null, false);
                
                client.OnMessage += message => {
                    string[] packet = PacketUtils.readPacket(Encoding.ASCII.GetBytes(message));
                    
                    Console.WriteLine("Received => " + message);
                    
                    eventSystem.Invoke(packet[0], new Packet<string[]>(rooms, _client, packet.Skip(1).Take(packet.Length - 1).ToArray()));
                };
            };
            server.OnClientDisconnect += client => Console.WriteLine("Client disconnect");
            server.start();
        }
    }
}
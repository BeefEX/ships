using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ships_Common.Net;
using Ships_Server.Rooms;
using SocketLib;

namespace Ships_Server {
    
    internal class Program {
        private static ServerSocket server;
        private static RoomManager rooms;
        
        public static void Main(string[] args) {
            new Task (() => {
                string line = Console.ReadLine();
                if (line == "close" || line == "exit")
                    server.close();
            }).Start();
            
            rooms = new RoomManager();
            
            server = new ServerSocket(8080);
            server.OnClientConnected += client => {
                Console.WriteLine("Client connected");
                client.OnMessage += message => {
                    
                    string[] packet = Packet.readPacket(Encoding.ASCII.GetBytes(message));
                    
                    switch (packet[0]) {
                        case "cr":
                            rooms.createRoom(packet[1], packet[2], client);
                            break;
                        case "ls":
                            Room[] roomArray = rooms.getOpenRooms();
                            string[] roomStrings = new string[roomArray.Length];
                            for (int i = 0; i < roomStrings.Length; i++) {
                                roomStrings[i] = roomArray[i].ToString();
                            }
                            client.send(Packet.constructPacket("ls-rs", roomStrings));
                            break;
                        default:
                            Console.WriteLine("Unknown packet received.");
                            break;
                    }
                    
                    Console.WriteLine("Received => " + message);
                };
            };
            server.OnClientDisconnect += client => Console.WriteLine("Client disconnect");
            server.start();
        }
    }
}
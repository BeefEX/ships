using System;
using System.Collections.Generic;
using System.Text;
using Ships_Client.States;
using Ships_Common.Net;

namespace Ships_Client.GameFlow.Scenes {

    struct Room {
        public string name;
        public string id;
        public bool passwordProtected;

        public Room(string name, string id, bool passwordProtected) {
            this.name = name;
            this.id = id;
            this.passwordProtected = passwordProtected;
        }
    }
    
    public class RoomListSceneScript : IScript {

        private bool shouldRender;
        private List<Room> rooms;
        
        public void Start() {
            shouldRender = true;
            rooms = new List<Room>();
            
            ConnectionState.client.OnMessage += message => {
                Console.WriteLine(message);
                string[] packet = Packet.readPacket(Encoding.ASCII.GetBytes(message));
                if (packet[0] == "ls-rs") {
                    for (int i = 1; i < packet.Length; i++) {
                        string[] room = packet[i].Split('$');
                        rooms.Add(new Room(room[1], room[0], Boolean.Parse(room[2])));
                    }
                    shouldRender = true;
                }
            };
            ConnectionState.client.send(Packet.constructPacket("ls"));
        }

        public void Update() {
            if (!shouldRender)
                return;
            shouldRender = false;
            
            Console.Clear();
            string roomCountString = "Room count: " + rooms.Count;
            Console.SetCursorPosition(Console.WindowWidth - 1 - roomCountString.Length, 1);
            Console.Write(roomCountString);

            int maxRooms = Math.Min(Console.WindowHeight - 7, rooms.Count);
            
            Console.SetCursorPosition(1, 3);
            for (int j = 0; j < Console.WindowWidth - 2; j++) {
                Console.Write('=');                    
            }
                
            Console.SetCursorPosition(1, 4);
            Console.Write("|| Room name");
                
            Console.SetCursorPosition(Console.WindowWidth - 14, 4);
            Console.Write("| Password ||");
                
            Console.SetCursorPosition(1, 5);
            for (int j = 0; j < Console.WindowWidth - 2; j++) {
                Console.Write('=');                    
            }
            
            for (int i = 0; i < maxRooms; i++) {
                Console.SetCursorPosition(1, 6 + i);
                Console.Write("|| " + rooms[i].name);
                Console.SetCursorPosition(Console.WindowWidth - 14, 6 + i);
                if (rooms[i].passwordProtected)
                    Console.Write("|   Yes    ||");
                else
                    Console.Write("|   No     ||");
            }
            
            Console.SetCursorPosition(1, 6 + maxRooms);
            for (int j = 0; j < Console.WindowWidth - 2; j++) {
                Console.Write('=');                    
            }
        }

        public void KeyPressed(ConsoleKeyInfo key) {
            
        }
    }
}
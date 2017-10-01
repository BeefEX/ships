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

        private int selected;

        private void OnRoomListReceived(string[] packet) {
            for (int i = 0; i < packet.Length; i++) {
                string[] room = packet[i].Split('$');
                rooms.Add(new Room(room[1], room[0], Boolean.Parse(room[2])));
            }
            shouldRender = true;
        }
        
        public void Start() {
            shouldRender = true;
            rooms = new List<Room>();
            selected = 0;
            
            ConnectionState.OnMessage.addTrigger(new PacketHandler(Packets.ROOM_LIST, OnRoomListReceived));
            ConnectionState.Send(PacketUtils.constructPacket(Packets.ROOM_LIST.ToString()));
        }

        public void Unload() {
            shouldRender = true;
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
                if (selected == i) {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                Console.SetCursorPosition(1, 6 + i);
                Console.Write("|| " + rooms[i].name);
                
                for (int j = ("|| " + rooms[i].name).Length; j < Console.WindowWidth - 15; j++) {
                    Console.Write(' ');                    
                }
                if (rooms[i].passwordProtected)
                    Console.Write("|   Yes    ||");
                else
                    Console.Write("|   No     ||");
                
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            }
            
            Console.SetCursorPosition(1, 6 + maxRooms);
            for (int j = 0; j < Console.WindowWidth - 2; j++) {
                Console.Write('=');                    
            }
        }

        public void KeyPressed(ConsoleKeyInfo key) {
            shouldRender = true;
            if (key.Key == ConsoleKey.UpArrow || (key.Key == ConsoleKey.Tab && key.Modifiers == ConsoleModifiers.Control))
                selected--;
            else if (key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.Tab)
                selected++;
            else
                shouldRender = false;

            selected = Math.Max(0, Math.Min(rooms.Count - 1, selected));

            if (key.Key == ConsoleKey.RightArrow || key.Key == ConsoleKey.Enter) {
                RoomState.isHost = false;
                RoomState.roomID = rooms[selected].id;
                RoomState.roomName = rooms[selected].name;
                
                if (rooms[selected].passwordProtected)
                    Program.game.SwitchScene("RoomLoginScene");
                else
                    Program.game.SwitchScene("RoomWaitingScene");
            }
        }
    }
}
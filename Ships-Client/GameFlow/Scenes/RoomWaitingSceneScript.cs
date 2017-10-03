using System;
using System.Text;
using Ships_Client.Rendering;
using Ships_Client.States;
using Ships_Common;
using Ships_Common.Net;

namespace Ships_Client.GameFlow.Scenes {
    
    public class RoomWaitingSceneScript : IScript {

        private Loader loader;

        private void OnOpponentJoined(string[] packet) {
            loader.Reset();
            
            string loadingString = "Opponent connected";

            RoomState.connected = true;
            loader.showLoader = false;
                
            Console.Clear();
            Console.SetCursorPosition(Console.WindowWidth / 2 - loadingString.Length / 2, Console.WindowHeight / 2);
            Console.Write(loadingString);
        }

        private void OnJoin(string[] packet) {
            loader.Reset();
            
            string loadingString;
                
            if (packet[0] == "True") {
                RoomState.connected = true;
                loadingString = "Succesfully connected";
            } else {
                loadingString = "Failed to connect (maybe wrong password)";
            }
            loader.showLoader = false;
                
            Console.Clear();
            Console.SetCursorPosition(Console.WindowWidth / 2 - loadingString.Length / 2, Console.WindowHeight / 2);
            Console.Write(loadingString);
        }
        
        public void Start() {
            loader = new Loader(new Vector2(Console.WindowWidth / 2f, Console.WindowHeight / 2f + 4));
            loader.OnLoaderFinish += OnLoaderFinish;
            
            string loadingString;
            
            if (RoomState.isHost)
                loadingString = "Waiting for an opponent to connect ...";
            else
                loadingString = "Joining the selected room ...";
            
            Console.Clear();
            Console.SetCursorPosition(Console.WindowWidth / 2 - loadingString.Length / 2, Console.WindowHeight / 2);
            Console.Write(loadingString);
            
            if (RoomState.isHost)
                ConnectionState.Send(PacketUtils.constructPacket(Packets.CREATE_ROOM.ToString(), RoomState.roomName, RoomState.roomPassword));
            else
                ConnectionState.Send(PacketUtils.constructPacket(Packets.JOIN_ROOM.ToString(), RoomState.roomID, RoomState.roomPassword));
            
            ConnectionState.OnMessage.addTrigger(new PacketHandler(Packets.OPPONENT_JOINED, OnOpponentJoined));
            ConnectionState.OnMessage.addTrigger(new PacketHandler(Packets.JOIN_ROOM, OnJoin));
        }

        public void Unload() {
            loader.OnLoaderFinish -= OnLoaderFinish;
        }

        public void Update() {
            loader.Render();
        }

        public void OnLoaderFinish() {
            if (RoomState.connected && !loader.showLoader)
                Program.game.SwitchScene("ShipPlacementScene");
            else if (!loader.showLoader)
                Program.game.SwitchScene("MainMenu");
        }
        
        public void KeyPressed(ConsoleKeyInfo key) { }
    }
}
using System;
using System.Text;
using Ships_Client.States;
using Ships_Common;
using Ships_Common.Net;

namespace Ships_Client.GameFlow.Scenes {
    
    public class RoomWaitingSceneScript : IScript {

        private char[][,] loadingCircle = {
            new [,] {
                {'\\', '_', '/'}, {' ', ' ', ' '}, {' ', ' ', ' '}
            },
            new [,] {
                {'\\', '_', ' '},  {'|', ' ', ' '},  {' ', ' ', ' '}
            },
            new [,] {
                {'\\', ' ', ' '},  {'|', ' ', ' '},  {'/', ' ', ' '}
            },
            new [,] {
                {' ', ' ', ' '},  {'|', ' ', ' '},  {'/', '=', ' '}
            },
            new [,] {
                {' ', ' ', ' '},  {' ', ' ', ' '},  {'/', '=', '\\'}
            },
            new [,] {
                {' ', ' ', ' '},  {' ', ' ', '|'},  {' ', '=', '\\'}
            },
            new [,] {
                {' ', ' ', '/'},  {' ', ' ', '|'},  {' ', ' ', '\\'}
            },
            new [,] {
                {' ', '_', '/'},  {' ', ' ', '|'},  {' ', ' ', ' '}
            }
        };

        private bool showLoader = true;
        private int index;
        private int counter;

        private void OnOpponentJoined(string[] packet) {
            counter = 0;
            index = 0;
                
            string loadingString;
                
            if (packet[1] == "True") {
                RoomState.connected = true;
                showLoader = false;
                loadingString = "Succesfully connected";
            } else {
                showLoader = false;
                loadingString = "Failed to connect (maybe wrong password)";
            }
                
            Console.Clear();
            Console.SetCursorPosition(Console.WindowWidth / 2 - loadingString.Length / 2, Console.WindowHeight / 2);
            Console.Write(loadingString);
        }

        private void OnJoin(string[] packet) {
            counter = 0;
            index = 0;
                
            string loadingString = "Opponent connected";

            RoomState.connected = true;
            showLoader = false;
                
            Console.Clear();
            Console.SetCursorPosition(Console.WindowWidth / 2 - loadingString.Length / 2, Console.WindowHeight / 2);
            Console.Write(loadingString);            
        }
        
        public void Start() {
            string loadingString;
            
            if (RoomState.isHost)
                loadingString = "Waiting for an opponent to connect ...";
            else
                loadingString = "Joining the selected room ...";
            
            Console.Clear();
            Console.SetCursorPosition(Console.WindowWidth / 2 - loadingString.Length / 2, Console.WindowHeight / 2);
            Console.Write(loadingString);
            
            if (RoomState.isHost)
                ConnectionState.client.send(PacketUtils.constructPacket(Packets.CREATE_ROOM.ToString(), RoomState.roomName, RoomState.roomPassword));
            else
                ConnectionState.client.send(PacketUtils.constructPacket(Packets.JOIN_ROOM.ToString(), RoomState.roomID, RoomState.roomPassword));
            
            ConnectionState.OnMessage.addTrigger(new PacketHandler(Packets.OPPONENT_JOINED, OnOpponentJoined));
            ConnectionState.OnMessage.addTrigger(new PacketHandler(Packets.JOIN_ROOM, OnJoin));
        }

        public void Unload() { }

        public void Update() {
            counter++;
            counter = counter % 9;
            
            if (counter == 0) {
                index++;
                index = index % loadingCircle.Length;

                if (showLoader)
                    Renderer.drawPatternAsPixel(new Vector2(Console.WindowWidth / 2f, Console.WindowHeight / 2f + 4),
                        loadingCircle[index]);
                else if (index == 0) {
                    if (RoomState.connected)
                        Program.game.SwitchScene("ShipPlacementScene");
                    else
                        Program.game.SwitchScene("MainMenu");
                }
            }
        }

        public void KeyPressed(ConsoleKeyInfo key) {
            
        }
    }
}
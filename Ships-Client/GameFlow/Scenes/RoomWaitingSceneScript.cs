using System;
using System.Diagnostics.Contracts;
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

        private int index;
        private int counter;

        private void onMessage(string message) {
            string[] packet = Packet.readPacket(Encoding.ASCII.GetBytes(message));

            if (packet[0] == "jn-rs") {
                string loadingString;
                if (packet[1] == "True") {
                    RoomState.connected = true;
                    loadingString = "Succesfully connected";
                } else {
                    counter = 0;
                    index = 0;
                    RoomState.connected = true;
                    loadingString = "Failed to connect (maybe wrong password)";
                }
                Console.Clear();
                Console.SetCursorPosition(Console.WindowWidth / 2 - loadingString.Length / 2, Console.WindowHeight / 2);
                Console.Write(loadingString);
                ConnectionState.client.OnMessage -= onMessage;
            }
        }
        
        public void Start() {
            string loadingString;
            
            if (RoomState.isHost) {
                ConnectionState.client.send(Packet.constructPacket("cr", RoomState.roomName, RoomState.roomPassword));
                loadingString = "Waiting for an opponent to connect ...";
            } else {
                ConnectionState.client.OnMessage += onMessage;
                ConnectionState.client.send(Packet.constructPacket("jn", RoomState.roomID, RoomState.roomPassword));
                loadingString = "Joining the selected room ...";
            }
            
            Console.Clear();
            Console.SetCursorPosition(Console.WindowWidth / 2 - loadingString.Length / 2, Console.WindowHeight / 2);
            Console.Write(loadingString);
        }

        public void Unload() {
            ConnectionState.client.OnMessage -= onMessage;
        }

        public void Update() {
            counter++;
            counter = counter % 9;
            
            if (counter == 0) {
                index++;
                index = index % loadingCircle.Length;

                if (!RoomState.connected)
                    Renderer.drawPatternAsPixel(new Vector2(Console.WindowWidth / 2, Console.WindowHeight / 2 + 4),
                        loadingCircle[index]);
                else if (index == 0) {
                    RoomState.connected = false;
                    Program.game.SwitchScene("MainMenu");
                }
            }
        }

        public void KeyPressed(ConsoleKeyInfo key) {
            
        }
    }
}
using System;
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

        private int index = 0;
        private int counter = 0;
        
        public void Start() {
            string loadingString = "";
            if (RoomState.isHost) {
                ConnectionState.client.send(Packet.constructPacket("cr", RoomState.roomName, RoomState.roomPassword));
                loadingString = "Waiting for an opponent to connect ...";
            } else {
                ConnectionState.client.send(Packet.constructPacket("jn", RoomState.roomID, RoomState.roomPassword));
                loadingString = "Joining the selected room ...";
            }
            Console.Clear();
            Console.SetCursorPosition(Console.WindowWidth / 2 - loadingString.Length / 2, Console.WindowHeight / 2);
            Console.Write(loadingString);
        }

        public void Unload() { }

        public void Update() {
            counter++;
            counter = counter % 9;
            
            if (counter == 0) {
                index++;
                index = index % loadingCircle.Length;
                
                Renderer.drawPatternAsPixel(new Vector2(Console.WindowWidth / 2, Console.WindowHeight / 2 + 4), loadingCircle[index]);
            }
        }

        public void KeyPressed(ConsoleKeyInfo key) {
            
        }
    }
}
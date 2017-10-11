using System;
using System.Collections.Generic;
using Ships_Client.Rendering;
using Ships_Client.States;
using Ships_Common;
using Ships_Common.Net;

namespace Ships_Client.GameFlow.Scenes {
    public class GameSceneScript : IScript {

        private bool waiting;
        private bool shouldRender;
        private Loader loader;
        
        public void Start() {
            loader = new Loader(new Vector2(Console.WindowWidth / 2f, Console.WindowHeight / 2f + 4));
            
            List<string> ships = new List<string>();
            
            for (int i = 0; i < GameState.yourShips.Count; i++) {
                ships.Add(GameState.yourShips[i].Instantiate(new Vector2()).ToString());
            }
            
            ConnectionState.OnMessage.addTrigger(new PacketHandler(Packets.OPPONENT_READY, OnOpponentJoin));
            ConnectionState.Send(PacketUtils.constructPacket(Packets.SUBMIT_SHIP_POSITIONS.ToString(), ships.ToArray()));

            string loadingString = "Waiting for the opponent to place ships";
            
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();
            Console.SetCursorPosition(Console.WindowWidth / 2 - loadingString.Length / 2, Console.WindowHeight / 2);
            Console.Write(loadingString);
            
            waiting = true;
            shouldRender = true;
        }

        private void RenderWait() {
            loader.Render();
        }

        private void RenderGame() {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            
            for (int i = 0; i < 10; i++) {
                Console.SetCursorPosition(i + 1, 0);
                Console.Write(i);
                Console.SetCursorPosition(0, i + 1);
                Console.Write((char) (65 + i));
            }

            for (int i = 0; i < GameState.yourShips.Count; i++) {
                Console.ForegroundColor = ConsoleColor.White;
                
                Renderer.renderShip(GameState.yourShips[i]);
            }
            
            for (int i = 0; i < 10; i++) {
                Console.SetCursorPosition(i + 1 + 20, 0);
                Console.Write(i);
                Console.SetCursorPosition(20, i + 1);
                Console.Write((char) (65 + i));
            }
            
            for (int i = 0; i < GameState.yourShips.Count; i++) {
                Console.ForegroundColor = ConsoleColor.White;
                
                Renderer.renderShip(GameState.yourShips[i], new Vector2(20));
            }
        }
        
        public void Unload() {
            shouldRender = true;
        }

        public void Update() {
            if (waiting)
                RenderWait();
            else {
                if (!shouldRender)
                    return;
                shouldRender = false;
                
                RenderGame();
            }
        }

        public void KeyPressed(ConsoleKeyInfo key) {
            
        }

        private void OnOpponentJoin(string[] message) {
            waiting = false;
            shouldRender = true;
        }
    }
}
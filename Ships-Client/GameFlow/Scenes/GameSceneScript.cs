using System;
using System.Collections.Generic;
using Ships_Client.States;
using Ships_Common;
using Ships_Common.Net;

namespace Ships_Client.GameFlow.Scenes {
    public class GameSceneScript : IScript {

        private bool shouldRender;
        
        public void Start() {
            
            List<string> ships = new List<string>();
            
            for (int i = 0; i < GameState.yourShips.Count; i++) {
                ships.Add(GameState.yourShips[i].Instantiate(new Vector2()).ToString());
            }
            
            ConnectionState.Send(PacketUtils.constructPacket(Packets.SUBMIT_SHIP_POSITIONS.ToString(), ships.ToArray()));
            
            shouldRender = true;
        }

        public void Unload() {
            shouldRender = true;
        }

        public void Update() {
            if (!shouldRender)
                return;
            shouldRender = false;

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

        public void KeyPressed(ConsoleKeyInfo key) {
            
        }
    }
}
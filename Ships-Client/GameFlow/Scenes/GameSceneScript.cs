using System;
using Ships_Client.States;

namespace Ships_Client.GameFlow.Scenes {
    public class GameSceneScript : IScript {

        private bool shouldRender;
        
        public void Start() {
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
        }

        public void KeyPressed(ConsoleKeyInfo key) {
            
        }
    }
}
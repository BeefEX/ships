using System;
using System.Collections.Generic;
using Ships_Common;

namespace Ships_Client.GameFlow.Scenes {
    public class ShipPlacementSceneScript : IScript {

        private static readonly Vector2[] offsets = { new Vector2(-1), new Vector2(1), new Vector2(0, -1), new Vector2(0, 1) };
        
        private bool shouldRender;

        private List<Ship> ships;
        private bool wrongPosition;
        
        public void Start() {
            wrongPosition = false;
            ships = new List<Ship> {Ship.defaultShips[Ship.defaultInventory[0]].Instantiate(new Vector2(5, 5))};
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
                Console.Write(i);
            }

            for (int i = 0; i < ships.Count; i++) {
                if (i == ships.Count - 1)
                    Console.ForegroundColor = wrongPosition ? ConsoleColor.Red : ConsoleColor.Green;
                else
                    Console.ForegroundColor = ConsoleColor.White;
                
                Renderer.renderShip(ships[i]);
            }
        }

        public void KeyPressed(ConsoleKeyInfo key) {
            shouldRender = true;

            Vector2 delta = new Vector2();

            if (key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.S)
                delta.y += 1;
            if (key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.W)
                delta.y -= 1;
            if (key.Key == ConsoleKey.RightArrow || key.Key == ConsoleKey.D)
                delta.x += 1;
            if (key.Key == ConsoleKey.LeftArrow || key.Key == ConsoleKey.A)
                delta.x -= 1;
            if (key.Key == ConsoleKey.R)
                ships[ships.Count - 1].Rotate();
            if (key.Key == ConsoleKey.Enter && !wrongPosition) {
                if (ships.Count < Ship.defaultInventory.Length)
                    ships.Add(Ship.defaultShips[Ship.defaultInventory[ships.Count]].Instantiate(new Vector2(5, 5)));
                else
                    Program.game.SwitchScene("MainMenu");
            }

            Ship selected = ships[ships.Count - 1];
            bool collides = false;

            foreach (Vector2 part in selected.shape) {
                Vector2 tmp = selected.position + part + delta;
                
                if (tmp.x < 1 || tmp.x > 10 || tmp.y < 1 || tmp.y > 10)
                    delta = new Vector2();
            }

            for (int i = 0; i < ships.Count - 1; i++) {
                foreach (Vector2 vector in selected.shape) {
                    foreach (Vector2 offset in offsets) {
                        if (ships[i].checkShape(selected.position + offset + vector + delta)) {
                            collides = true;
                        }
                    }
                }
            }
            
            wrongPosition = collides;
            
            ships[ships.Count - 1].position += delta;
        }
    }
}
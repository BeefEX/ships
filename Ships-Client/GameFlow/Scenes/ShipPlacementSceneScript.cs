using System;
using System.Collections.Generic;
using Ships_Common;

namespace Ships_Client.GameFlow.Scenes {
    public class ShipPlacementSceneScript : IScript {

        private bool shouldRender;

        private List<Ship> ships;
        private bool wrongPosition;
        
        public void Start() {
            wrongPosition = false;
            ships = new List<Ship>();
            ships.Add(Ship.SHIP_BIG.Instantiate(new Vector2(1, 2)));
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
                    if (wrongPosition)
                        Console.ForegroundColor = ConsoleColor.Red;
                    else
                        Console.ForegroundColor = ConsoleColor.Green;
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
            if (key.Key == ConsoleKey.Enter && !wrongPosition)
                ships.Add(Ship.SHIP_PLANE.Instantiate(new Vector2(2, 2)));
            
            Ship selected = ships[ships.Count - 1];
            bool collides = false;
   
            Vector2 tmp = selected.position + delta;
            
            if (tmp.x < 2 || tmp.x > 12 || tmp.y < 2 || tmp.y > 12)
                delta = new Vector2();
            
            for (int i = 0; i < ships.Count - 1; i++) {
                foreach (Vector2 vector in selected.shape) {
                    if (ships[i].checkShape(selected.position + vector + delta)) {
                        collides = true;
                    }
                }
            }
            
            wrongPosition = collides;
            
            ships[ships.Count - 1].position += delta;
        }
    }
}
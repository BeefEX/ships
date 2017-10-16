using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ships_Client.States;
using Ships_Common;
using Ships_Common.Net;

namespace Ships_Client.GameFlow.Scenes {
    public class ShipPlacementSceneScript : IScript {

        private static readonly Vector2[] offsets = { new Vector2(-1), new Vector2(1), new Vector2(0, -1), new Vector2(0, 1) };

        private Ship[] _ships;
        
        private bool shouldRender;

        private List<Ship> ships;
        private bool wrongPosition;
        
        public void Start() {
            ships = new List<Ship>();
            
            ConnectionState.OnMessage.addTrigger(new PacketHandler(Packets.OPPONENT_DISCONNECTED, OnOpponentDisconnected));
            ConnectionState.OnMessage.addTrigger(new PacketHandler(Packets.SHIP_LIST, OnShipListReceived));
            ConnectionState.Send(Encoding.ASCII.GetBytes(Packets.SHIP_LIST.ToString()));
            
            string loadingString = "Fetching ship list ...";
                
            Console.Clear();
            Console.SetCursorPosition(Console.WindowWidth / 2 - loadingString.Length / 2, Console.WindowHeight / 2);
            Console.Write(loadingString);
            
            wrongPosition = false;
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

            for (int i = 0; i < ships.Count; i++) {
                if (i == ships.Count - 1)
                    Console.ForegroundColor = wrongPosition ? ConsoleColor.Red : ConsoleColor.Green;
                else
                    Console.ForegroundColor = ConsoleColor.White;
                
                Renderer.renderShip(ships[i]);
            }
        }

        private void OnOpponentDisconnected(string[] message) {
            RoomState.finished = false;
            Program.game.SwitchScene("EndGameScene");
        }

        private void OnShipListReceived(string[] message) {
            _ships = new Ship[message.Length];
            
            for (int i = 0; i < message.Length; i++) {
                _ships[i] = Ship.FromString(message[i]);
            }
            
            shouldRender = true;
            ships.Add(_ships[0].Instantiate(new Vector2(5, 5)));
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
                if (ships.Count < _ships.Length)
                    ships.Add(_ships[ships.Count].Instantiate(new Vector2(5, 5)));
                else {
                    GameState.yourShips = ships;
                    GameState.yourHits = new List<Hit>();
                    GameState.opponentsHits = new List<Hit>();
                    Program.game.SwitchScene("GameScene");
                }
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
            
            selected.position += delta;

            bool moving = true;
            
            while (moving) {
                foreach (Vector2 part in selected.shape) {
                    delta = new Vector2();
                    Vector2 tmp = selected.position + part + delta;

                    moving = true;
                    
                    if (tmp.x < 1)
                        delta.x += 1;
                    else if (tmp.x > 10)
                        delta.x -= 1;
                    else if (tmp.y < 1)
                        delta.y += 1;
                    else if (tmp.y > 10)
                        delta.y -= 1;
                    else
                        moving = false;
                    
                    selected.position += delta;
                }
            }
        }
    }
}
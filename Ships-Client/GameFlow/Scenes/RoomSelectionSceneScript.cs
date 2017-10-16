using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Ships_Client.Rendering;
using Ships_Client.States;

namespace Ships_Client.GameFlow.Scenes {
    public class RoomSelectionSceneScript : IScript {

        private Menu menu;
        
        private int counter = 60;
        
        private static List<MenuOption> options = new List<MenuOption> {
            new MenuOption("Host game", "RoomCreationScene"),
            new MenuOption("Join game", "RoomListScene")
        };
        
        public void Start() {
            string loadingString = "Connecting to server ...";
            Console.Clear();
            Console.SetCursorPosition(Console.WindowWidth / 2 - loadingString.Length / 2, Console.WindowHeight / 2);
            Console.Write(loadingString);
            try {
                if (!ConnectionState.connected)
                    ConnectionState.Init();
            } catch (SocketException e) {
                loadingString = "Connection failed.";
                Console.Clear();
                Console.SetCursorPosition(Console.WindowWidth / 2 - loadingString.Length / 2, Console.WindowHeight / 2);
                Console.Write(loadingString);
            }

            menu = new Menu(options);
        }

        public void Unload() {
            menu.Unload();
        }

        public void Update() {
            if (ConnectionState.connected)
                menu.Render();
            else {
                counter--;
                if (counter <= 0)
                    Program.game.SwitchScene("MainMenu");
            }
        }

        public void KeyPressed(ConsoleKeyInfo key) {
            menu.KeyPressed(key);
        }
    }
}
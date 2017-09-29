using System;
using System.Collections.Generic;
using Ships_Client.Rendering;
using Ships_Client.States;

namespace Ships_Client.GameFlow.Scenes {
    public class RoomSelectionSceneScript : IScript {

        private Menu menu;
        
        private static List<MenuOption> options = new List<MenuOption> {
            new MenuOption("Host game", "RoomCreationScene"),
            new MenuOption("Join game", "RoomListScene")
        };
        
        public void Start() {
            string loadingString = "Connecting to server ...";
            Console.Clear();
            Console.SetCursorPosition(Console.WindowWidth / 2 - loadingString.Length / 2, Console.WindowHeight / 2);
            Console.Write(loadingString);
            ConnectionState.Init();
            
            menu = new Menu(options);
        }

        public void Unload() {
            menu.Unload();
        }

        public void Update() {
            menu.Render();
        }

        public void KeyPressed(ConsoleKeyInfo key) {
            menu.KeyPressed(key);
        }
    }
}
using System;
using System.Collections.Generic;
using Ships_Client.Rendering;

namespace Ships_Client.GameFlow.Scenes {

     
    
    public class MainMenuScript : IScript {

        private Menu menu;
        
        private static List<MenuOption> options = new List<MenuOption> {
            new MenuOption("Play", "RoomSelectionScene"),
            new MenuOption("Exit", "ExitScene")
        };

        public void Start() {
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
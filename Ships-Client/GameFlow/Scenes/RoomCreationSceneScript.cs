using System;
using System.Collections.Generic;
using Ships_Client.Rendering;
using Ships_Client.States;

namespace Ships_Client.GameFlow.Scenes {
    
    public class RoomCreationSceneScript : IScript {

        private static string allowedChars = "abcdefghijklmnopqrstuvwxyz123456789_-+*!#()& ";

        private Menu menu;
        
        private static List<MenuOption> options = new List<MenuOption> {
            new MenuOption("", "", "Room name:"),
            new MenuOption("", "", "Room password:"),
            new MenuOption("Create", "RoomWaitingScene")
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
            
            RoomState.roomName = options[0].name;
            RoomState.roomPassword = options[1].name;

            bool shouldRender = true;
            
            if (key.Key == ConsoleKey.Backspace && options[menu.selected].scene == "")
                options[menu.selected].name = options[menu.selected].name.Substring(0, options[menu.selected].name.Length - 1);
            else if (allowedChars.Contains(key.KeyChar.ToString()) && options[menu.selected].scene == "")
                options[menu.selected].name += key.KeyChar;
            else
                shouldRender = false;

            if (shouldRender)
                menu.Render(true);
        }
    }
}
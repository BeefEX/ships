using System;
using System.Collections.Generic;
using Ships_Client.States;

namespace Ships_Client.GameFlow.Scenes {
    
    public class RoomCreationSceneScript : IScript {

        private static string allowedChars = "abcdefghijklmnopqrstuvwxyz123456789_-+*!#()& ";

        private bool shouldRender = true;
        
        private static List<Renderer.Option> options = new List<Renderer.Option> {
            new Renderer.Option("", "", "Room name:"),
            new Renderer.Option("", "", "Room password:"),
            new Renderer.Option("Create", "RoomWaitingScene")
        };

        private int selected = 0;
        
        public void Start() {
            
        }

        public void Unload() {
            shouldRender = true;
        }

        public void Update() {
            if (!shouldRender)
                return;
            shouldRender = false;
            Console.Clear();
            Renderer.renderMenu(options, selected);
        }

        public void KeyPressed(ConsoleKeyInfo key) {
            
            RoomState.roomName = options[0].name;
            RoomState.roomPassword = options[1].name;

            shouldRender = true;
            
            if (key.Key == ConsoleKey.UpArrow)
                selected--;
            else if (key.Key == ConsoleKey.DownArrow)
                selected++;
            else if (key.Key == ConsoleKey.RightArrow || key.Key == ConsoleKey.Enter)
                Program.game.SwitchScene(options[selected].scene);
            else if (key.Key == ConsoleKey.Backspace)
                options[selected].name = options[selected].name.Substring(0, options[selected].name.Length - 1);
            else if (allowedChars.Contains(key.KeyChar.ToString()))
                options[selected].name += key.KeyChar;
            else
                shouldRender = false;
            
            selected = Math.Max(0, Math.Min(options.Count - 1, selected));
        }
    }
}
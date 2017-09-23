using System;
using System.Collections.Generic;
using Ships_Client.States;

namespace Ships_Client.GameFlow.Scenes {
    public class RoomSelectionSceneScript : IScript {
        
        private static List<Renderer.Option> options = new List<Renderer.Option> {
            new Renderer.Option("Host game", "RoomCreationScene"),
            new Renderer.Option("Join game", "RoomListScene")
        };
        
        private int selected = 0;
        private bool shouldRender = true;
        
        
        public void Start() {
            string loadingString = "Connecting to server ...";
            Console.Clear();
            Console.SetCursorPosition(Console.WindowWidth / 2 - loadingString.Length / 2, Console.WindowHeight / 2);
            Console.Write(loadingString);
            ConnectionState.Init();
        }

        public void Update() {
            if (!shouldRender)
                return;
            shouldRender = false;
            
            Console.Clear();
            Renderer.renderMenu(options, selected);            
        }

        public void KeyPressed(ConsoleKeyInfo key) {
            shouldRender = true;
            if (key.Key == ConsoleKey.UpArrow)
                selected--;
            else if (key.Key == ConsoleKey.DownArrow)
                selected++;
            else
                shouldRender = false;

            selected = Math.Max(0, Math.Min(options.Count - 1, selected));
            
            if (key.Key == ConsoleKey.RightArrow || key.Key == ConsoleKey.Enter)
                Program.game.SwitchScene(options[selected].scene);
        }
    }
}
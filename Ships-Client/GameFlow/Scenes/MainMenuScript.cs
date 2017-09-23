using System;
using System.Collections.Generic;

namespace Ships_Client.GameFlow.Scenes {

     
    
    public class MainMenuScript : IScript {

        private static List<Renderer.Option> options = new List<Renderer.Option> {
            new Renderer.Option("Play", "RoomSelectionScene"),
            new Renderer.Option("Exit", "ExitScene")
        };

        private bool shouldRender = true;
        private int selected = 0;

        public void Start() { }

        public void Unload() { }

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
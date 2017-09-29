using System;
using System.Collections.Generic;

namespace Ships_Client.Rendering {
    
    public class MenuOption {
        public string label;
        public string name;
        public string scene;

        public MenuOption(string name, string scene, string label = null) {
            this.name = name;
            this.scene = scene;
            this.label = label;
        }
    }
    
    public class Menu {

        protected List<MenuOption> options;

        protected bool shouldRender;

        public int selected { get; protected set; }
        
        public Menu(List<MenuOption> list, int begin = 0) {
            options = list;
            selected = begin;
            shouldRender = true;
        }
        
        public void Render(bool force = false, bool clear = true) {
            if (!force && !shouldRender)
                return;
            shouldRender = false;
            
            if (clear)
                Console.Clear();
            
            int middle = Console.WindowWidth / 2;

            for (int i = 0; i < options.Count; i++) {
                MenuOption option = options[i];
                int offset = options[i].name.Length / 2;
				
                if (option.label != default(string) && option.label != "") {
                    Console.SetCursorPosition(middle - offset - 4 - option.label.Length, Console.WindowHeight / 2 + 4 * i);
                    Console.Write(option.label);
                }
				
                if (selected == i) {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                
                Console.SetCursorPosition(middle - offset - 2, Console.WindowHeight / 2 + 4 * i - 1);
                for (int j = 0; j < option.name.Length + 6; j++) {
                    Console.Write("=");
                }

                Console.SetCursorPosition(middle - offset - 2, Console.WindowHeight / 2 + 4 * i);
                Console.Write("|| " + option.name + " ||");
                
                Console.SetCursorPosition(middle - offset - 2, Console.WindowHeight / 2 + 4 * i + 1);
                for (int j = 0; j < option.name.Length + 6; j++) {
                    Console.Write("=");
                }
                
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public void Unload() {
            shouldRender = true;
        }
        
        public void KeyPressed(ConsoleKeyInfo key) {
            shouldRender = true;
            if (key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.Tab && key.Modifiers == ConsoleModifiers.Shift)
                selected--;
            else if (key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.Tab)
                selected++;
            else
                shouldRender = false;
            
            selected = Math.Max(0, Math.Min(options.Count - 1, selected));
            
            if (key.Key == ConsoleKey.RightArrow || key.Key == ConsoleKey.Enter)
                Program.game.SwitchScene(options[selected].scene);
        }
        
    }
}
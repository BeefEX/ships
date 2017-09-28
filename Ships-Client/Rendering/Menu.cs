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
        protected ColorScheme colorScheme;

        protected bool shouldRender;

        protected int selected;
        
        public Menu(List<MenuOption> list, ColorScheme colorScheme = new ColorScheme(), int begin = 0) {
            options = list;
            this.colorScheme = colorScheme;
            selected = begin;
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
                    Console.BackgroundColor = colorScheme.backgroundSelected;
                    Console.ForegroundColor = colorScheme.foregroundSelected;
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
                
                Console.BackgroundColor = colorScheme.background;
                Console.ForegroundColor = colorScheme.foreground;
            }
        }

        public void Unload() {
            shouldRender = true;
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
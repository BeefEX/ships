using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Ships_Common;

namespace Ships_Client {
	
	public static class Renderer {
		
		public static void renderShip (Ship ship) {
			if (ship == default(Ship))
				return;

			Vector2 pos = ship.position;
			
			for (int i = 0; i < ship.shape.Length; i++) {
				Vector2 vector = new Vector2((pos.x + ship.shape[i].x) * 3 + 1, (pos.y + ship.shape[i].y) * 3);
				
				if (ship.hits[i])
					drawPatternAsPixel(vector, Ship.Parts.PART_EXPLODED);
				else
					drawPixel(vector, '#');
			}
		}
		
		public static void drawPixel (Vector2 position, char character) {
			for (int x = position.x - 1; x <= position.x + 1; x++) {
				for (int y = position.y - 1; y <= position.y + 1; y++) {
					if (x < 0 || y < 0 || x > Console.WindowWidth || y > Console.WindowHeight)
						continue;
					Console.SetCursorPosition(x, y);
					Console.Write(character);
				}
			}
		}
		
		public static void drawPatternAsPixel (Vector2 position, char[,] pattern) {
			for (int x = position.x - 1; x <= position.x + 1; x++) {
				for (int y = position.y - 1; y <= position.y + 1; y++) {
					if (x < 0 || y < 0 || x > Console.WindowWidth || y > Console.WindowHeight)
						continue;
					Console.SetCursorPosition(x, y);
					Console.Write(pattern[1 - (y - position.y), 1 + (x - position.x)]);
				}
			}
		}
		
		public class Option {
			public string label;
			public string name;
			public string scene;

			public Option(string name, string scene, string label = null) {
				this.name = name;
				this.scene = scene;
				this.label = label;
			}
		}

		public static void renderMenu(List<Option> options, int selected) {
			int middle = Console.WindowWidth / 2;

			for (int i = 0; i < options.Count; i++) {
				Option option = options[i];
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
	}
}

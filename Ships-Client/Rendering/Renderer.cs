using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Ships_Common;

namespace Ships_Client {
	
	public static class Renderer {
		
		public static char[][,] loadingCircle = {
			new [,] {
				{'\\', '_', '/'}, {' ', ' ', ' '}, {' ', ' ', ' '}
			},
			new [,] {
				{'\\', '_', ' '},  {'|', ' ', ' '},  {' ', ' ', ' '}
			},
			new [,] {
				{'\\', ' ', ' '},  {'|', ' ', ' '},  {'/', ' ', ' '}
			},
			new [,] {
				{' ', ' ', ' '},  {'|', ' ', ' '},  {'/', '=', ' '}
			},
			new [,] {
				{' ', ' ', ' '},  {' ', ' ', ' '},  {'/', '=', '\\'}
			},
			new [,] {
				{' ', ' ', ' '},  {' ', ' ', '|'},  {' ', '=', '\\'}
			},
			new [,] {
				{' ', ' ', '/'},  {' ', ' ', '|'},  {' ', ' ', '\\'}
			},
			new [,] {
				{' ', '_', '/'},  {' ', ' ', '|'},  {' ', ' ', ' '}
			}
		};
		
		public static void renderShip (Ship ship, Vector2 offset = new Vector2()) {
			if (ship == default(Ship))
				return;

			Vector2 pos = ship.position + offset;
			
			for (int i = 0; i < ship.shape.Length; i++) {
				Vector2 vector = pos + ship.shape[i];
				
				if (ship.hits[i])
					drawPixel(vector, 'X');//drawPatternAsPixel(vector, Ship.Parts.PART_EXPLODED);
				else
					drawPixel(vector, '#');
			}
		}
		
		public static void drawPixel (Vector2 position, char character) {
			/*
			for (int x = position.x; x <= position.x + 5; x++) {
				for (int y = position.y; y <= position.y + 2; y++) {
					if (x < 0 || y < 0 || x > Console.WindowWidth || y > Console.WindowHeight)
						continue;
					Console.SetCursorPosition(x, y);
					Console.Write(character);
				}
			}
			*/
			Console.SetCursorPosition((int)position.x, (int)position.y);
			Console.Write(character);
		}
		
		public static void drawPatternAsPixel (Vector2 position, char[,] pattern) {
			for (int x = (int)position.x - 1; x <= position.x + 1; x++) {
				for (int y = (int)position.y - 1; y <= position.y + 1; y++) {
					if (x < 0 || y < 0 || x > Console.WindowWidth || y > Console.WindowHeight)
						continue;
					Console.SetCursorPosition(x, y);
					Console.Write(pattern[1 - (y - (int)position.y), 1 + (x - (int)position.x)]);
				}
			}
		}
		
		
	}
}

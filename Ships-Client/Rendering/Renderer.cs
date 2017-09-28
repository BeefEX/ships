﻿using System;
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
				Vector2 vector = new Vector2(pos.x + ship.shape[i].x, pos.y + ship.shape[i].y);
				
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
			Console.SetCursorPosition(position.x, position.y);
			Console.Write(character);
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
		
		
	}
}

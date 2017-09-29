using System;
using System.Collections.Generic;
using System.Linq;

namespace Ships_Common {
    
    public class Ship {

		public static class Parts {
			public static char[,] PART_EXPLODED = {{'\\', '#', '/'}, {'@', '#', '@'}, {'/', '@', '\\'}};
		}

	    public static string[] defaultInventory =
		    { "SHIP_HUGE", "SHIP_BIG", "SHIP_BIG", "SHIP_NORMAL", "SHIP_NORMAL", "SHIP_NORMAL", "SHIP_SMALL", "SHIP_SMALL" };
	    
	    public static Dictionary<string, Ship> defaultShips = new Dictionary<string, Ship> {
		    { "SHIP_SMALL", new Ship(default(Vector2), new[] { new Vector2() }) },
		    { "SHIP_NORMAL", new Ship(default(Vector2), new[] { new Vector2(), new Vector2(0, -1) }) },
		    { "SHIP_BIG", new Ship(default(Vector2), new[] { new Vector2(0, 1), new Vector2(), new Vector2(0, -1) }) },
		    { "SHIP_HUGE", new Ship(default(Vector2), new[] { new Vector2(0, 1), new Vector2(), new Vector2(0, -1), new Vector2(0, -2) }) }
	    };

	    public Rotation rotation { get; private set; }

	    public Vector2[] shape { get; private set; }

	    public bool[] hits { get; private set; }

	    public Vector2 position;

        protected Ship(Vector2 position, Vector2[] shape) {
            this.position = position;
            this.shape = shape;
	        hits = new bool[this.shape.Length];
	        rotation = new Rotation(90);
        }

        public bool isHit(Vector2 pos) {
	        bool hit = checkShape(pos);
	        if (hit) {
		        int index = 0;
		        for (int i = 0; i < shape.Length; i++) {
			        if (Equals(shape[i], position - pos)) {
				        index = i;
				        break;
			        }
		        }
		        hits[index] = true;
	        }
	        return hit;
        }

	    public void Rotate(float degrees) {
		    rotation.Rotate(degrees);
	    }

	    public bool checkShape(Vector2 pos) {
		    return shape.Contains(pos - position);
	    }

        public Ship Instantiate(Vector2 pos) {
            return new Ship(pos, shape);
        }
    }
}
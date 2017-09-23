using System;
using System.Collections.Generic;
using System.Linq;

namespace Ships_Common {
    
    public class Ship {

		public static class Parts {
			public static char[,] PART_EXPLODED = {{'\\', '#', '/'}, {'@', '#', '@'}, {'/', '@', '\\'}};
		}
		
        public static Ship SHIP_SMALL = new Ship(default(Vector2), new[] { new Vector2(0, 0) });
        public static Ship SHIP_NORMAL = new Ship(default(Vector2), new[] { new Vector2(0, 0), new Vector2(0, -1) });
        public static Ship SHIP_BIG = new Ship(default(Vector2), new[] { new Vector2(0, 1), new Vector2(0, 0), new Vector2(0, -1) });
        public static Ship SHIP_PLANE = new Ship(default(Vector2), new[] { new Vector2(0, 1), new Vector2(0, 0), new Vector2(0, -1), new Vector2(1, 0) });
        
        public Vector2[] shape { get; private set; }

	    public bool[] hits { get; private set; }

	    public Vector2 position { get; private set;  }

        protected Ship(Vector2 position, Vector2[] shape) {
            this.position = position;
            this.shape = shape;
	        this.hits = new bool[this.shape.Length];
        }

        public bool isHit(Vector2 pos) {
	        bool hit = checkShape(pos);
	        if (hit) {
		        int index = 0;
		        for (int i = 0; i < this.shape.Length; i++) {
			        if (Equals(this.shape[i], this.position - pos)) {
				        index = i;
				        break;
			        }
		        }
		        hits[index] = true;
	        }
	        return hit;
        }

	    public bool checkShape(Vector2 pos) {
		    return this.shape.Contains(this.position - pos);
	    }

        public Ship Instantiate(Vector2 pos) {
            return new Ship(pos, this.shape);
        }
    }
}
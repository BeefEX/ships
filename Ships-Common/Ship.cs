using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ships_Common {
    
    public class Ship {

	    #region Static members
	    
	    public static string[] defaultInventory =
		    { "SHIP_HUGE", "SHIP_BIG", "SHIP_BIG", "SHIP_NORMAL", "SHIP_NORMAL", "SHIP_NORMAL", "SHIP_SMALL", "SHIP_SMALL" };
	    
	    public static Dictionary<string, Ship> defaultShips = new Dictionary<string, Ship> {
		    { "SHIP_SMALL", new Ship(default(Vector2), new[] { new Vector2() }) },
		    { "SHIP_NORMAL", new Ship(default(Vector2), new[] { new Vector2(), new Vector2(0, -1) }) },
		    { "SHIP_BIG", new Ship(default(Vector2), new[] { new Vector2(0, 1), new Vector2(), new Vector2(0, -1) }) },
		    { "SHIP_HUGE", new Ship(default(Vector2), new[] { new Vector2(0, 1), new Vector2(), new Vector2(0, -1), new Vector2(0, -2) }) }
	    };
	    
	    #endregion

	    #region Variables
	    
	    public bool rotated { get; private set; }
	    
	    public Vector2[] shape { get; private set; }

	    public bool[] hits { get; private set; }

	    public Vector2 position;
	    
	    #endregion

	    #region Constructors
	    
        protected Ship(Vector2 position, Vector2[] shape) {
            this.position = position;
            this.shape = shape;
	        hits = new bool[this.shape.Length];
	        rotated = false;
        }
	    
	    #endregion

	    #region Methods
	    
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

	    public void Rotate() {
		    rotated = !rotated;
		    for (int i = 0; i < shape.Length; i++) {
			    float tmp = shape[i].x;
			    shape[i].x = shape[i].y;
			    shape[i].y = tmp;
		    }
	    }

	    public bool checkShape(Vector2 pos) {
		    return shape.Contains(pos - position);
	    }

        public Ship Instantiate(Vector2 pos) {
            return new Ship(pos, (Vector2[]) shape.Clone());
        }

	    #endregion

	    #region String serialization

	    public override string ToString() {
		    StringBuilder builder = new StringBuilder();
		    
		    for (int i = 0; i < shape.Length; i++) {
			    builder.Append(shape[i] + "/");
		    }
		    builder.Remove(builder.Length - 1, 1);

		    builder.Append("$");
		    
		    for (int i = 0; i < hits.Length; i++) {
			    builder.Append(hits[i] + "/");
		    }
		    builder.Remove(builder.Length - 1, 1);
		    
		    return rotated + "$" + position + "$" + builder;
	    }

	    public static Ship FromString(string ship) {
		    string[] split = ship.Split('$');
		    
		    Vector2 position = Vector2.FromString(split[1]);
		    List<Vector2> shape = new List<Vector2>();

		    foreach (string s in split[2].Split('/')) {
			    shape.Add(Vector2.FromString(s));
		    }
		    
		    List<bool> hits = new List<bool>();
		    
		    foreach (string s in split[3].Split('/')) {
			    hits.Add(bool.Parse(s));
		    }
		    
		    Ship _ship = new Ship(position, shape.ToArray());
		    
			_ship.rotated = bool.Parse(split[0]);
		    _ship.hits = hits.ToArray();
		    
		    return _ship;
	    }
	    #endregion
    }
}
namespace Ships_Common {
    
    public struct Vector2 {

        #region Members
        
        public float x, y;
        
        #endregion

        #region Constructors
        
        public Vector2(float x = 0, float y = 0) {
            this.x = x;
            this.y = y;
        }
        
        #endregion

        #region Methods
        
        public void add(Vector2 other) {
            x += other.x;
            y += other.y;
        }

        public void subtract(Vector2 other) {
            x -= other.x;
            y -= other.y;
        }

        public void multiply(Vector2 other) {
            x *= other.x;
            y *= other.y;
        }

        public void divide(Vector2 other) {
            x /= other.x;
            y /= other.y;
        }

        public bool Equals(Vector2 other) {
            return x.Equals(other.x) && y.Equals(other.y);
        }
        
        #endregion

        #region String serialization
        
        public override string ToString() {
            return x + "#" + y;
        }

        public static Vector2 FromString(string vector) {
            string[] split = vector.Split('#');
            return new Vector2(float.Parse(split[0]), float.Parse(split[1]));
        }
        
        #endregion

        #region Operators
        
        public static Vector2 operator +(Vector2 a, Vector2 b) {
            Vector2 vec = new Vector2(a.x, a.y);
            vec.add(b);
            return vec;
        }
        
        public static Vector2 operator -(Vector2 a, Vector2 b) {
            Vector2 vec = new Vector2(a.x, a.y);
            vec.subtract(b);
            return vec;
        }
        
        public static Vector2 operator *(Vector2 a, Vector2 b) {
            Vector2 vec = new Vector2(a.x, a.y);
            vec.multiply(b);
            return vec;
        }
        
        public static Vector2 operator /(Vector2 a, Vector2 b) {
            Vector2 vec = new Vector2(a.x, a.y);
            vec.divide(b);
            return vec;
        }
        
        #endregion
    }
}
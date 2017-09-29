namespace Ships_Common {
    
    public struct Vector2 {

        public float x, y;

        public Vector2(float x = 0, float y = 0) {
            this.x = x;
            this.y = y;
        }

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

        public override int GetHashCode() {
            unchecked {
                return (x.GetHashCode() * 397) ^ y.GetHashCode();
            }
        }

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
    }
}
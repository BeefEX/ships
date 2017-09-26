namespace Ships_Common {
    
    public struct Vector2 {

        public int x, y;

        public Vector2(int x = 0, int y = 0) {
            this.x = x;
            this.y = y;
        }

        public void add(Vector2 other) {
            this.x += other.x;
            this.y += other.y;
        }

        public void subtract(Vector2 other) {
            this.x -= other.x;
            this.y -= other.y;
        }

        public override bool Equals(object obj) {
            if (obj is Vector2) {
                Vector2 vec = (Vector2) obj;
                return this.x == vec.x && this.y == vec.y;
            }
            return false;
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
    }
}
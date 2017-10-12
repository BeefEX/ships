namespace Ships_Common {
    
    public struct Hit {

        public Vector2 location;
        public bool succesful;

        public Hit(Vector2 location, bool succesful) {
            this.location = location;
            this.succesful = succesful;
        }

        public override string ToString() {
            return location + "$" + succesful;
        }

        public static Hit FromString(string hit) {
            string[] split = hit.Split('$');
            Vector2 loc = Vector2.FromString(split[0]);
            bool success = bool.Parse(split[1]);
            return new Hit(loc, success);
        } 
    }
}
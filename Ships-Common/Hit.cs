namespace Ships_Common {
    
    public struct Hit {

        public readonly Vector2 location;
        public readonly bool succesful;

        public Hit(Vector2 location, bool succesful) {
            this.location = location;
            this.succesful = succesful;
        }
    }
}
using System;

namespace Ships_Common {
    
    public struct Rotation {

        public float degrees { get; private set; }
        public Vector2 rotation { get; private set; }

        public Rotation(Vector2 rotation) : this() {
            this.rotation = rotation;
            degrees = (float)Math.Atan2(rotation.y, rotation.x);
        }

        public Rotation(float degrees) : this() {
            rotation = new Vector2((float)Math.Cos(degrees), (float)Math.Sin(degrees));
            this.degrees = degrees;
        }

        public void Rotate(float _degrees) {
            degrees += _degrees;
            rotation = new Vector2((float)Math.Cos(degrees), (float)Math.Sin(degrees));
        }
    }
}
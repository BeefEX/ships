using System;

namespace Ships_Common {
    
    public class Rotation {

        private const double DegToRad = Math.PI / 180;
        
        public float degrees { get; private set; }
        public Vector2 rotation { get; private set; }

        public Rotation(Vector2 rotation) {
            this.rotation = rotation;
            //degrees = (float)Math.Atan2(rotation.y, rotation.x);
        }

        public Rotation(float degrees) {
            rotation = new Vector2((float)Math.Cos(degrees * DegToRad), (float)Math.Sin(degrees * DegToRad));
            this.degrees = degrees;
        }

        public void Rotate(float _degrees) {
            degrees += _degrees;
            rotation = new Vector2((float)Math.Cos(degrees * DegToRad), (float)Math.Sin(degrees * DegToRad));
        }

        public static Vector2 operator *(Vector2 vector, Rotation rotation) {
            Vector2 result = new Vector2();
            vector.x = (float)(vector.x * Math.Cos(rotation.degrees * DegToRad) - vector.y * Math.Sin(rotation.degrees * DegToRad));
            result.y = (float)(vector.x * Math.Sin(rotation.degrees * DegToRad) + vector.y * Math.Cos(rotation.degrees * DegToRad));
            return result;
        }
    }
}
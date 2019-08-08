using OpenTK;

namespace RT
{
    public class Ray
    {
        public Vector3 a;
        public Vector3 b;

        public Ray()
        {
            a = Vector3.Zero;
            b = Vector3.One;
        }
        
        public Ray(Vector3 a, Vector3 b)
        {
            this.a = a;
            this.b = b;
        }
        

        public Vector3 GetOrigin()
        {
            return a;
        }

        public Vector3 GetPointAt(float p)
        {
            return a + p * b;
        }
    }
}
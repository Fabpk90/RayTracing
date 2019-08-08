using OpenTK;

namespace RT
{
    public struct HitInfo
    {
        public float distance;
        public Vector3 p;
        public Vector3 normal;
    }
    public abstract class Hitable
    {
        public abstract bool hit(Ray r, float tMin, float tMax, ref HitInfo info);
    }
}
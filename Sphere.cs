using System;
using OpenTK;

namespace RT
{
    public class Sphere : Hitable
    {
        public float radius;
        public Vector3 position;

        public Sphere(Vector3 position, float radius)
        {
            this.radius = radius;
            this.position = position;
        }
        
        public override bool hit(Ray r, float tMin, float tMax, ref HitInfo info)
        {
            Vector3 direction = r.a - position;
            float a = Vector3.Dot(r.b, r.b);
            float b = 2.0f * Vector3.Dot(direction, r.b);
            float c = Vector3.Dot(direction, direction) - radius * radius;

            float discriminant = b * b - 4 * a * c;

            if (discriminant > 0)
            {
                //delta
                float temp = (float) ((-b - Math.Sqrt(b * b - a * c)) / a);

                //if the sphere is at a non culling distance
                if (temp < tMax && temp > tMin)
                {
                    info.distance = temp;
                    info.p = r.GetPointAt(temp);
                    info.normal = (info.p - position) / radius;

                    return true;
                }

                temp = (float) ((-b + Math.Sqrt(b * b - a * c)) / a);

                if (temp < tMax && temp > tMin)
                {
                    info.distance = temp;
                    info.p = r.GetPointAt(temp);
                    info.normal = (info.p - position) / radius;

                    return true;
                }
            }

            return false;
        }
    }
}
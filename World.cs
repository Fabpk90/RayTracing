using System;
using System.Collections.Generic;

namespace RT
{
    public class World
    {
        public List<Hitable> entities;

        public World()
        {
            entities = new List<Hitable>();
        }
        
        public void Add(Hitable hitable)
        {
            entities.Add(hitable);
        }

        public bool hit(Ray r, float distanceMin, float distanceMax, ref HitInfo info)
        {
            HitInfo hitInfo = new HitInfo();
            bool hitSomething = false;
            
            float closest = Single.MaxValue;

            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i].hit(r, distanceMin, closest, ref hitInfo))
                {
                    hitSomething = true;
                    closest = hitInfo.distance;
                    info = hitInfo;
                }
            }

            return hitSomething;
        }
    }
}
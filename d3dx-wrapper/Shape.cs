using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d3dx_wrapper
{
    abstract class Shape
    {
        public abstract bool RayCast(Ray ray, Vector3 position);

        protected Vector3 Nearest(Ray ray, Vector3 position)
        {
            float lamda = (float)( (ray.direction.X * (position.X - ray.position.X) +
                                    ray.direction.Y * (position.Y - ray.position.Y) +
                                    ray.direction.Z * (position.Z - ray.position.Z)) 
                                    /
                                   (Math.Pow(ray.direction.X, 2) + 
                                    Math.Pow(ray.direction.Y, 2) +
                                    Math.Pow(ray.direction.Z, 2)) );

            float x = ray.position.X + ray.direction.X * lamda;
            float y = ray.position.Y + ray.direction.Y* lamda;
            float z = ray.position.Z + ray.direction.Z * lamda;
            return new Vector3(x, y, z);
        }
    }
}

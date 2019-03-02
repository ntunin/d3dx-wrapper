using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.DirectX;

namespace D3DX
{
    public class BoxShape : Shape
    {
        private float width;
        private float height;
        private float depth;

        public BoxShape(float width, float height, float depth)
        {
            this.width = width;
            this.height = height;
            this.depth = depth;
        }

        public override bool RayCast(Ray ray, Vector3 position)
        {
            float lamda = 0;
            Vector3 nearest = Nearest(ray, position, out lamda);

            return lamda <= 0 &&
                nearest.X >= position.X - width / 2 && nearest.X <= position.X + width / 2 &&
                nearest.Y >= position.Y - height / 2 && nearest.Y <= position.Y + height / 2 &&
                nearest.Z >= position.Z - depth / 2 && nearest.Z <= position.Z + depth / 2;
        }
    }
}

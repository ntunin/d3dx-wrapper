using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3DX
{
    public class RotationBehaviour : Behaviour
    {
        public override void Update(float deltaTime, Body body)
        {
            body.Rotation.X += deltaTime / 5;
            body.Rotation.Y += deltaTime / 5;
            body.Rotation.Z += deltaTime / 5;
        }
    }
}

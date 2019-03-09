using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace D3DX
{
    public class Prefab
    {
        public Body Body;
        public Skin Skin;
        public Shape Bound;
        private Device device = null;
        public List<Prefab> children = new List<Prefab>();
        public Prefab parent;

        public Prefab(Body body, Skin skin, Shape bound, List<Prefab> children)
        {
            Skin = skin;
            Body = body;
            Bound = bound;
            this.children = children;
            if (children != null)
            {
                foreach (Prefab child in children)
                {
                    child.parent = this;
                }
            }
            device = SceneContext.Shared.Device;
        }

        public void Present(float deltaTime)
        {
            Matrix parentTransform = device.Transform.World;
            Matrix transform = parentTransform;
            if (Body != null)
            {
                if(Body.Behaviour != null)
                {
                    foreach(Behaviour bevaviour in Body.Behaviour)
                    {
                        bevaviour.Update(deltaTime, Body);
                    }
                }
                Matrix rotation = Matrix.RotationYawPitchRoll(Body.Rotation.Y, Body.Rotation.X, Body.Rotation.Z);
                Matrix translation = Matrix.Translation(Body.Position.X, Body.Position.Y, Body.Position.Z);
                transform = Body.StartTransform * rotation * translation * Body.FinalTransform;
                transform *= parentTransform;
            }

            if (children != null)
            {
                foreach (Prefab prefab in children)
                {
                    device.Transform.World = transform;
                    prefab.Present(deltaTime);
                }
            }

            if(Skin != null)
            {
                device.Transform.World = transform;
                Skin.Present();
            }
        }

        public void AddChild(Prefab child)
        {
            if(children == null)
            {
                children = new List<Prefab>();
            }
            children.Add(child);
            child.parent = this;
        }

        public void RemoveChild(Prefab child)
        {
            if (children == null)
            {
                return;
            }
            children.Remove(child);
            child.parent = null;
        }

        public Vector3 GetGlobalPosition()
        {
            Vector3 position = new Vector3();
            Prefab prefab = this;
            while(prefab != null)
            {
                position.Add(prefab.Body.Position);
                prefab = prefab.parent;
            }
            return position;
        }
    }
}

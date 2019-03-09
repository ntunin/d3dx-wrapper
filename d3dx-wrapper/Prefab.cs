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
        private List<Prefab> children = new List<Prefab>();
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

        public Prefab RayCast(Ray ray)
        {
            Prefab raycastedPrefab = null;
            if (Bound != null && Bound.RayCast(ray, Body.Position))
            {
                raycastedPrefab = this;
            }
            if (children != null)
            {
                List<Prefab> raycastedChildren = new List<Prefab>();
                Ray prefabRay = new Ray(new Vector3(ray.Position.X - Body.Position.X,
                                                        ray.Position.Y - Body.Position.Y,
                                                        ray.Position.Z - Body.Position.Z), ray.Direction);
                foreach (Prefab child in children)
                {
                    Prefab childRaycastedPrefab = child.RayCast(prefabRay);
                    if (childRaycastedPrefab != null)
                    {
                        raycastedChildren.Add(childRaycastedPrefab);
                    }
                }
                Prefab nearest = null;
                float minDistance = 1e8f;
                foreach (Prefab child in raycastedChildren)
                {
                    float distance = new Vector3(child.Body.Position.X - prefabRay.Position.X,
                                                   child.Body.Position.Y - prefabRay.Position.Y,
                                                   child.Body.Position.Z - prefabRay.Position.Z).Length();
                    if (distance < minDistance)
                    {
                        nearest = child;
                        minDistance = distance;
                    }
                }
                if (nearest != null)
                {
                    raycastedPrefab = nearest;
                }

            }
            return raycastedPrefab;
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

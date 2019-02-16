using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using System.Windows.Forms;
using System.Drawing;

namespace D3DX
{
    public abstract class Scene
    {
        protected List<Prefab> prefabs;
        protected List<Light> light;
        protected Camera camera;

        public Scene(Control control)
        {
            camera = CreateCamera(control);
            light = new List<Light>();
            prefabs = new List<Prefab>();
        }

        protected abstract Camera CreateCamera(Control control);
        protected virtual void AddLight()
        {
        }
        protected virtual void AddPrefabs()
        {
        }

        public void Prepare()
        {
            SetupCamera();
            SetupLight();
        }

        private void SetupCamera()
        {
            camera.Setup();
        }

        private void SetupLight()
        {
            Device device = SceneContext.Shared.Device;
            device.RenderState.Lighting = true;
            for(int i = 0; i < device.Lights.Count; i++)
            {
                device.Lights[i].Enabled = false;
            } 
            int index = 0;
            foreach (Light l in light)
            {
                if (l.IsAvailableForIndex(index))
                {
                    l.Enable(index++);
                }
            }
        }

        public void Present(float deltaTime)
        {
            Device device = SceneContext.Shared.Device;
            device.Transform.World = Matrix.Identity;
            foreach (Prefab prefab in prefabs)
            {
                prefab.Present(deltaTime);
            }
        }
    }
}

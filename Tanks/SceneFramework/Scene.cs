using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using System.Windows.Forms;
using System.Drawing;

namespace Tanks
{
    public class Scene
    {
        private List<Prefab> prefabs;
        private List<Light> light;
        private Camera camera;

        private Light light1;
        private Prefab landscape;
        private Prefab tank;

        public Scene(Control control)
        {
            camera = new PerspectiveCamera((float)Math.PI/4, control, 0.1f, 1000f, new Vector3(0, 0, 500), new Vector3(0, 0, 0), new Vector3(0, 1, 0));

            light = new List<Light>();
            light1 = new Light(new Dictionary<string, object> {
                {"Type", LightType.Point},
                {"Position", new Vector3(0, 100, 0) },
                {"Diffuse", Color.White },
                {"Attenuation", 0.6f },
                {"Range", 10000f }
            });
            light.Add(light1);

            var landscapeSkin = (Skin) new SkinBuilder(new Dictionary<string, object> {
                { "Name", "Landscape" },
                { "File", "teapot.X" },
                { "MeshFlags", "Dynamic" },
            }).Create();

            landscape = (Prefab)new PrefabBuilder(new Dictionary<string, object> {
                {"Name", "Landscape"},
                {"Body", new Dictionary<string, object>{
                    {"Position", new Dictionary<string, object>{
                        {"x", 0},
                        {"y", 0},
                        {"z", 0}
                    }}
                }},
                {"Skin", "Landscape" }
            }).Create();
            //tank = (Prefab)new PrefabBuilder(new Dictionary<string, object> { }).Create();

            prefabs = new List<Prefab>();
            prefabs.Add(landscape);
           // prefabs.Add(tank);
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
            Device device = AppContext.Shared.Device;
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
            Device device = AppContext.Shared.Device;
            device.Transform.World = Matrix.Identity;
            foreach (Prefab prefab in prefabs)
            {
                prefab.Present(deltaTime);
            }
        }
    }
}

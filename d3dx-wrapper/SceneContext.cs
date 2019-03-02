using D3DX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3DX
{
    public class SceneContext
    {
        private static SceneContext shared;

        public class MeshData
        {
            public Mesh mesh;
            public ExtendedMaterial[] materials;

            public MeshData(Mesh mesh, ExtendedMaterial[] materials)
            {
                this.mesh = mesh;
                this.materials = materials;
            }

        }

        public static SceneContext Shared
        {
            get
            {
                if (shared == null)
                {
                    shared = new SceneContext();
                }
                return shared;
            }
        }

        public Device Device;

        public Dictionary<string, Body> Bodies = new Dictionary<string, Body>();
        public Dictionary<string, Skin> Skins = new Dictionary<string, Skin>();
        public Dictionary<string, Prefab> Prefabs = new Dictionary<string, Prefab>();
        public Dictionary<string, MeshData> Meshes = new Dictionary<string, MeshData>();

    }
}

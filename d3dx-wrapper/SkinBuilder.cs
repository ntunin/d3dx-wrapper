using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using System.Windows.Forms;

namespace D3DX
{
    public class SkinBuilder : Builder
    {
        public SkinBuilder(Dictionary<string, object> configs) : base(configs)
        {
        }

        public override object Create()
        {
            string fileName = null;
            MeshFlags meshFlags = 0;
            Mesh mesh = null;
            ExtendedMaterial[] mtrl = null;
            Material[] materials = null;
            Texture[] textures = null;
            Dictionary<string, string> customTextureNames = null;
            string texturesPath = "";
            string name = null;

            HandleActionMap(new Dictionary<string, Action<object>>
            {
                {"File", (object o)=>{ fileName = (string)o;  } },
                {"MeshFlags", (object o)=>{ meshFlags = ParseMeshFlags((string)o);  } },
                {"TexturesPath", (object o)=>{ texturesPath = (string)o + @"\";  } },
                {"Name", (object o)=>{ name = (string)o; } },
                {"CustomTextures", (object o)=>{ customTextureNames = ParseTextures(o); } }
            });
            Device device = SceneContext.Shared.Device;



            SceneContext.MeshData cachedMeshData = null;
            try
            {
                cachedMeshData = SceneContext.Shared.Meshes[fileName];
            } catch (Exception e)
            {
              //TODO: log  
            }
            string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            if (cachedMeshData == null)
            {
                mesh = Mesh.FromFile(fileName, meshFlags, device, out mtrl);
                LoadDecorations(device, mtrl, out materials, out textures, texturesPath, customTextureNames);
                SceneContext.Shared.Meshes[fileName] = new SceneContext.MeshData(mesh, mtrl);
            } else
            {
                mesh = cachedMeshData.mesh;
                mtrl = cachedMeshData.materials;
                LoadDecorations(device, mtrl, out materials, out textures, texturesPath, customTextureNames);
            }
           
                
            var skin = new Skin(mesh, materials, textures);

            if (name != null)
            {
                SceneContext.Shared.Skins[name] = skin;
            }

            return skin;
        }

        private void LoadDecorations(Device device, 
                                    ExtendedMaterial[] mtrl, 
                                    out Material[] materials, 
                                    out Texture[] textures, 
                                    string texturesPath, 
                                    Dictionary<string, string> customTextureNames)
        {
            if ((mtrl != null) && mtrl.Length > 0)
            {
                materials = new Material[mtrl.Length];
                textures = new Texture[mtrl.Length];
                for (int i = 0; i < mtrl.Length; i++)
                {
                    materials[i] = mtrl[i].Material3D;
                    textures[i] = LoadTexture(device, mtrl[i], texturesPath, customTextureNames);
                }
            } else
            {
                materials = null;
                textures = null;
            }
        }

        private Texture LoadTexture(Device device, 
                                    ExtendedMaterial mtrl, 
                                    string texturesPath,
                                    Dictionary<string, string> customTextureNames)
        {
            if (mtrl.TextureFilename != null && mtrl.TextureFilename != String.Empty)
            {
                string texturefileName = mtrl.TextureFilename;
                if (customTextureNames != null)
                {
                    string customTextureName = customTextureNames[texturefileName];
                    if (customTextureName != null)
                    {
                        texturefileName = customTextureName;
                    }
                }
                if (texturesPath != null)
                {
                    texturefileName = texturesPath + texturefileName;
                }
                return TextureLoader.FromFile(device, texturefileName);
            }
            return null;
        }

        private Dictionary<string, string> ParseTextures(object o)
        {
            Dictionary<string, object> configs = (Dictionary<string, object>)o;
            Dictionary<string, string> textures = new Dictionary<string, string>();
            foreach(string key in configs.Keys)
            {
                textures[key] = (string)configs[key];
            }
            return textures;
        }

        private MeshFlags ParseMeshFlags(string flagsString)
        {
            MeshFlags flags = 0;
            string[] flagStrings = flagsString.Split(',');
            foreach(string flagString in flagStrings)
            {
                MeshFlags flag = ParseMeshFlag(flagString.Trim());
                flags |= flag;
            }
            return flags;
        }

        private MeshFlags ParseMeshFlag(string flagString)
        {
            return new Dictionary<string, MeshFlags>
            {
                {"DoNotClip", MeshFlags.DoNotClip },
                {"Dynamic", MeshFlags.Dynamic },
                {"IbDynamic", MeshFlags.IbDynamic },
                {"IbManaged", MeshFlags.IbManaged },
                {"IbSoftwareProcessing", MeshFlags.IbSoftwareProcessing },
                {"IbSystemMem", MeshFlags.IbSystemMem },
                {"IbWriteOnly", MeshFlags.IbWriteOnly },
                {"Managed", MeshFlags.Managed },
                {"NPatches", MeshFlags.NPatches },
                {"OptimizeAttributeSort", MeshFlags.OptimizeAttributeSort },
                {"OptimizeCompact", MeshFlags.OptimizeCompact },
                {"OptimizeDeviceIndependent", MeshFlags.OptimizeDeviceIndependent },
                {"OptimizeDoNotSplit", MeshFlags.OptimizeDoNotSplit },
                {"OptimizeIgnoreVerts", MeshFlags.OptimizeIgnoreVerts },
                {"OptimizeStripeReorder", MeshFlags.OptimizeStripeReorder },
                {"OptimizeVertexCache", MeshFlags.OptimizeVertexCache },
                {"Points", MeshFlags.Points },
                {"RtPatches", MeshFlags.RtPatches },
                {"SimplifyFace", MeshFlags.SimplifyFace },
                {"SimplifyVertex", MeshFlags.SimplifyVertex },
                {"SoftwareProcessing", MeshFlags.SoftwareProcessing },
                {"SystemMemory", MeshFlags.SystemMemory },
                {"Use32Bit", MeshFlags.Use32Bit },
                {"UseHardwareOnly", MeshFlags.UseHardwareOnly },
                {"VbDynamic", MeshFlags.VbDynamic },
                {"VbManaged", MeshFlags.VbManaged },
                {"VbShare", MeshFlags.VbShare },
                {"VbSoftwareProcessing", MeshFlags.VbSoftwareProcessing },
                {"VbSystemMem", MeshFlags.VbSystemMem },
                {"VbWriteOnly", MeshFlags.VbWriteOnly },
                {"WriteOnly", MeshFlags.WriteOnly }
            }[flagString];
        }
    }
}

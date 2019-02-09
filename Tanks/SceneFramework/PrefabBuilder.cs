using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace Tanks
{
    public class PrefabBuilder : Builder
    {
        public PrefabBuilder(Dictionary<string, object> configs) : base(configs)
        {
        }

        public override object Create()
        {
            Body body = null;
            Skin skin = null;
            List<Prefab> children = null;
            string name = null;
            HandleActionMap(new Dictionary<string, Action<object>>
            {
                {"Body", (object o)=>{ body = ParseBody(o);  } },
                {"Skin", (object o)=>{ skin = AppContext.Shared.Skins[(string)o];  } },
                {"Children", (object o)=>{ children = ParseChildren(o);  } },
                {"Name", (object o)=>{ name = (string)o;  } }
            });
            var prefab = new Prefab(body, skin, children);
            if (name != null)
            {
                AppContext.Shared.Prefabs[name] = prefab;
            }
            return prefab;
        }

        private Body ParseBody(object o)
        {
            if(o is string)
            {
                return AppContext.Shared.Bodies[(string)o];
            } else if(o is Dictionary<string, object>)
            {
                Dictionary<string, object> data = (Dictionary<string, object>)o;
                object body = new BodyBuilder(data).Create();
                return (Body)body;
            }
            return null;
        }

        private List<Prefab> ParseChildren(object objectsDescription)
        {
            List<object> objectsDescriptions = (List<object>)objectsDescription;
            List<Prefab> result = new List<Prefab>();
            foreach(object objectDecription in objectsDescriptions)
            {
                if(objectDecription is string)
                {
                    string description = (string)objectDecription;
                    result.Add(AppContext.Shared.Prefabs[description]);
                } else if (objectDecription is Dictionary<string, object>)
                {
                    Dictionary<string, object> description = (Dictionary<string, object>)objectDecription;
                    Prefab prefab = (Prefab)new PrefabBuilder(description).Create();
                    result.Add(prefab);
                }
            }
            return result;
        }
    }
}

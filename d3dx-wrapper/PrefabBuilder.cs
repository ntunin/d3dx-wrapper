﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace D3DX
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
            Shape bound = null;
            string name = null;
            HandleActionMap(new Dictionary<string, Action<object>>
            {
                {"Body", (object o)=>{ body = ParseBody(o);  } },
                {"Skin", (object o)=>{ skin = SceneContext.Shared.Skins[(string)o];  } },
                {"Bound", (object o)=>{ bound = ParseBound(o);  } },
                {"Children", (object o)=>{ children = ParseChildren(o);  } },
                {"Name", (object o)=>{ name = (string)o;  } }
            });
            var prefab = new Prefab(body, skin, bound, children);
            if (name != null)
            {
                SceneContext.Shared.Prefabs[name] = prefab;
            }
            return prefab;
        }

        private Body ParseBody(object o)
        {
            if(o is string)
            {
                return SceneContext.Shared.Bodies[(string)o];
            } else if(o is Dictionary<string, object>)
            {
                Dictionary<string, object> data = (Dictionary<string, object>)o;
                object body = new BodyBuilder(data).Create();
                return (Body)body;
            }
            return null;
        }

        private Shape ParseBound(object o)
        {
            Dictionary<string, object> objectDescriptions = (Dictionary<string, object>)o;
            string type = (string)objectDescriptions["Type"];
            switch (type)
            {
                case "Box":
                    return ParseBoxShape(objectDescriptions);
            }
            return null;
        }

        private Shape ParseBoxShape(Dictionary<string, object> objectDescriptions)
        {
            float width = float.Parse((string)objectDescriptions["Width"]);
            float height = float.Parse((string)objectDescriptions["Height"]);
            float depth = float.Parse((string)objectDescriptions["Depth"]);
            return new BoxShape(width, height, depth);
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
                    result.Add(SceneContext.Shared.Prefabs[description]);
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

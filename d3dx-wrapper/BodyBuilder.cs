using System;
using System.Collections.Generic;
using Microsoft.DirectX;

namespace D3DX
{
    class BodyBuilder : DirectXComponentBuilder
    {
        public BodyBuilder(Dictionary<string, object> configs) : base(configs)
        {
        }

        public override object Create()
        {
            Vector3 position = new Vector3();
            Vector3 rotation = new Vector3();
            List<Body> children = new List<Body>();
            List<Behaviour> behaviour = new List<Behaviour>();
            string name = null;

            HandleActionMap(new Dictionary<string, Action<object>>
            {
                {"Position", (object o)=>{ position = ParseVector3(o);  } },
                {"Rotation", (object o)=>{ rotation = ParseAngleVector3(o);  } },
                {"Children", (object o)=>{ children = ParseChildren((List<object>)o);  } },
                {"Name", (object o) => { name = (string)o; } }
            });
            Body body = new Body(position, rotation, behaviour);
            if (name != null)
            {
                SceneContext.Shared.Bodies[name] = body;
            }
            return body;
        }


        private List<Body> ParseChildren(List<object> childNames)
        {
            List<Body> children = new List<Body>();
            foreach (string name in childNames)
            {
                children.Add(SceneContext.Shared.Bodies[name]);
            }
            return children;
        }
    }
        
}

using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks
{
    public class AppContext
    {
        private static AppContext shared;

        public static AppContext Shared
        {
            get
            {
                if (shared == null)
                {
                    shared = new AppContext();
                }
                return shared;
            }
        }

        public Device Device;

        public Dictionary<string, Body> Bodies = new Dictionary<string, Body>();
        public Dictionary<string, Skin> Skins = new Dictionary<string, Skin>();
        public Dictionary<string, Prefab> Prefabs = new Dictionary<string, Prefab>();

    }
}

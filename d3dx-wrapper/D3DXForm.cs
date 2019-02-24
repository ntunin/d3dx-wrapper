using D3DX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace D3DX
{
    public abstract partial class D3DXForm : Form
    {
        private Device device = null;
        private Scene scene = null;
        private Stopwatch stopwatch = null;
        private float elapsedTime = 0;

        public D3DXForm()
        {
        }

        protected void InitializeGraphics()
        {
            SetStyle(ControlStyles.Opaque | ControlStyles.AllPaintingInWmPaint, true);
            PresentParameters parameters = new PresentParameters();
            parameters.Windowed = true;
            parameters.SwapEffect = SwapEffect.Discard;
            device = new Device(0, DeviceType.Hardware, this, CreateFlags.SoftwareVertexProcessing, parameters);
            device.RenderState.CullMode = Cull.Clockwise;
            SceneContext.Shared.Device = device;
     
            scene = CreateScene();
            stopwatch = Stopwatch.StartNew();
            device.DeviceReset += new EventHandler(OnDeviceReset);
            device.DeviceResizing += new CancelEventHandler(OnCancelResize);
            scene.Prepare();
            Invalidate();
        }

        protected abstract Scene CreateScene();

        private void OnDeviceReset(object sender, EventArgs e)
        {
            scene.Prepare();
        }

        private void OnCancelResize(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (stopwatch == null || !stopwatch.IsRunning)
            {
                return;
            }
            float deltaTime = CalculateNewDeltaTime();
            device.Clear(ClearFlags.Target, Color.Gray, 1.0f, 0);

            device.BeginScene();
            scene.Present(deltaTime);
            device.EndScene();
            device.Present();
            Invalidate();
        }

        private float CalculateNewDeltaTime()
        {
            float elapsedTime = stopwatch.ElapsedMilliseconds;
            if (this.elapsedTime == 0)
            {
                this.elapsedTime = elapsedTime;
                return 0;
            }
            float deltaTime = elapsedTime - this.elapsedTime;
            this.elapsedTime = elapsedTime;
            return deltaTime / 1000;
        }
    }
}

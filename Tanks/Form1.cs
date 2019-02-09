using Microsoft.DirectX;
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

namespace Tanks
{
    public partial class Form1 : Form
    {
        private Device device = null;
        private Scene scene = null;
        private Stopwatch stopwatch = null;
        private float elapsedTime = 0;
        private Camera camera = null;
        private Light light = null;

        public Form1()
        {
            InitializeComponent();
            InitializeGraphics();
        }

        private void Form1_Load(object sender, EventArgs e)
        {            
            scene = new Scene(this);
            scene.Prepare();
            stopwatch = Stopwatch.StartNew();
        }

        private void InitializeGraphics()
        {
            SetStyle(ControlStyles.Opaque | ControlStyles.AllPaintingInWmPaint, true);
            PresentParameters parameters = new PresentParameters();
            parameters.Windowed = true;
            parameters.SwapEffect = SwapEffect.Discard;
            device = new Device(0, DeviceType.Hardware, this, CreateFlags.SoftwareVertexProcessing, parameters);
            AppContext.Shared.Device = device;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
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

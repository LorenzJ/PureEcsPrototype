using Game;
using GameGl;
using OpenGL;
using System;
using System.Windows.Forms;

namespace WindowsGame
{
    public partial class GameForm : Form
    {
        private Game.Game game;
        private DateTime previousTime;
        private DateTime currentTime;
        private double accumulatedTime;
        private double timeStep = 1 / 60.0;
        private Renderer renderer;
        private DebugInfo debugInfo;
        private DebugInfoForm debugInfoForm;

        public GameForm()
        {
            InitializeComponent();
            debugInfoForm = new DebugInfoForm();
            debugInfoForm.Show();
        }

        private void GlControl1_ContextCreated(object sender, GlControlEventArgs e)
        {
            Focus();
            game = new Game.Game();
            renderer = game.World.GetResource<Renderer>();
            debugInfo = game.World.GetResource<DebugInfo>();

            Gl.Enable(EnableCap.Blend);
            Gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        }

        private void GameForm_Load(object sender, EventArgs e)
        {
            previousTime = DateTime.Now;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            currentTime = DateTime.Now;
            var deltaTime = (currentTime - previousTime).TotalSeconds;
            previousTime = currentTime;
            accumulatedTime += deltaTime;
            while (accumulatedTime > 0)
            {
                accumulatedTime -= timeStep;
                game.Update((float)timeStep);
            }
            debugInfo.DeltaTime = (float)deltaTime;
            debugInfoForm.UpdateValues(debugInfo);
            glControl.Invalidate();
        }

        private void glControl_Render(object sender, GlControlEventArgs e)
        {
            Gl.Clear(ClearBufferMask.ColorBufferBit);
            renderer.Render(game.Time);
        }

        private void glControl_Resize(object sender, EventArgs e)
        {
            Gl.Viewport(0, 0, ClientSize.Width, ClientSize.Height);
        }
    }
}

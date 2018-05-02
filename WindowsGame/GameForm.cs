using Game;
using Game.Dependencies;
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
        }

        private void GlControl1_ContextCreated(object sender, GlControlEventArgs e)
        {
            Focus();
            game = new Game.Game();
            renderer = game.World.GetDependency<Renderer>();
            debugInfo = game.World.GetDependency<DebugInfo>(); 

            Gl.Enable(EnableCap.Blend);
            Gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        }

        private void GameForm_Load(object sender, EventArgs e)
        {
            previousTime = DateTime.Now;
            timer.Start();
            debugInfoForm = new DebugInfoForm();
            debugInfoForm.Show();
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

        private void GlControl_Render(object sender, GlControlEventArgs e)
        {
            Gl.Clear(ClearBufferMask.ColorBufferBit);
            renderer.Render(game.Time);
        }

        private void GlControl_Resize(object sender, EventArgs e)
        {
            Gl.Viewport(0, 0, ClientSize.Width, ClientSize.Height);
        }

        private void GlControl_KeyDown(object sender, KeyEventArgs e)
        {
            HandleKeyEvent(e, true);
        }

        private void HandleKeyEvent(KeyEventArgs e, bool keyDown)
        {
            
        }

        private void glControl_KeyUp(object sender, KeyEventArgs e)
        {
            HandleKeyEvent(e, false);
        }
    }
}

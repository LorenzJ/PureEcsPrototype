using Game;
using Game.Dependencies;
using GameGl;
using GameGl.Core.Textures;
using OpenGL;
using System;
using System.Numerics;
using System.Threading;
using System.Windows.Forms;

namespace WindowsGame
{
    public partial class GameForm : Form
    {
        private Game.Game game;
        private DateTime previousTime;
        private DateTime currentTime;
        private double accumulatedTime;
        private double timeStep = 1 / 120.0;
        private Renderer renderer;
        private DebugInfo debugInfo;
        private DebugInfoForm debugInfoForm;
        private KeyBinds keyBinds;
        private Vector2[] directions = new Vector2[8];

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
            keyBinds = GetKeyBindings();

            Gl.Enable(EnableCap.Blend);
            Gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        }

        private void DoLogicUpdate()
        {
            currentTime = DateTime.Now;
            var deltaTime = (currentTime - previousTime).TotalSeconds;
            previousTime = currentTime;
            accumulatedTime += deltaTime;
            var inputCommands = keyBinds.InputCommands;
            game.World.Post(new InputMessage(inputCommands, directions));
            while (accumulatedTime > 0)
            {
                accumulatedTime -= timeStep;
                game.Update((float)timeStep);
            }
            debugInfo.DeltaTime = (float)deltaTime;
        }

        private KeyBinds GetKeyBindings()
        {
            var keyBinds = new KeyBinds();
            keyBinds.Bind(Keys.Left, 0, InputCommands.MoveLeft);
            keyBinds.Bind(Keys.Up, 0, InputCommands.MoveUp);
            keyBinds.Bind(Keys.Right, 0, InputCommands.MoveRight);
            keyBinds.Bind(Keys.Down, 0, InputCommands.MoveDown);
            keyBinds.Bind(Keys.Z, 0, InputCommands.Fire);
            return keyBinds;
        }

        private void GameForm_Load(object sender, EventArgs e)
        {
            previousTime = DateTime.Now;
            //timer.Start();
        }

        private void GlControl_Render(object sender, GlControlEventArgs e)
        {
            DoLogicUpdate();
            debugInfoForm?.UpdateValues(debugInfo);
            renderer.Render(game.Time);
            glControl.Invalidate();
        }

        private void GlControl_Resize(object sender, EventArgs e)
        {
            Gl.Viewport(0, 0, ClientSize.Width, ClientSize.Height);
        }

        private void GlControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                if (debugInfoForm == null || debugInfoForm.IsDisposed)
                {
                    debugInfoForm = new DebugInfoForm();
                }
                debugInfoForm.Show();
            }
            keyBinds.HandleKeyDown(e.KeyCode);
        }

        private void GlControl_KeyUp(object sender, KeyEventArgs e)
        {
            keyBinds.HandleKeyUp(e.KeyCode);
        }

        private void GlControl_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = true;
        }
    }
}

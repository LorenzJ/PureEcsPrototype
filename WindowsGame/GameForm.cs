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

        public GameForm()
        {
            InitializeComponent();
        }

        private void GlControl1_ContextCreated(object sender, GlControlEventArgs e)
        {
            
        }

        private void GameForm_Load(object sender, EventArgs e)
        {
            game = new Game.Game();
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
            Text = $"Game ({1 / deltaTime} fps";
            Invalidate();
        }
    }
}

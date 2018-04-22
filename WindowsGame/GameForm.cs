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
            previousTime = currentTime;
            currentTime = DateTime.Now;
            var deltaTime = (currentTime - previousTime).TotalSeconds;
            game.Update((float)deltaTime);
            Text = $"Game ({1 / deltaTime} fps";
            Invalidate();
        }
    }
}

using OpenGL;
using System;
using System.Windows.Forms;

namespace WindowsGame
{
    public partial class GameForm : Form
    {
        private Game.Game game;

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
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            game.Update(1 / 60.0f);
            Invalidate();
            game.Flush();
        }
    }
}

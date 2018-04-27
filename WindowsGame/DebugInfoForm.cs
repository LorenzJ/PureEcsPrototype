using Game;
using System;
using System.Windows.Forms;

namespace WindowsGame
{
    public partial class DebugInfoForm : Form
    {
        private delegate void SetDebugInfo(DebugInfo debugInfo);
        private SetDebugInfo UpdateValuesDelegate;

        public DebugInfoForm()
        {
            InitializeComponent();
            UpdateValuesDelegate = new SetDebugInfo(UpdateValues);
        }

        private void DebugInfo_Load(object sender, EventArgs e)
        {

        }

        internal void UpdateValues(DebugInfo debugInfo)
        {
            if (InvokeRequired)
            {
                Invoke(UpdateValuesDelegate, debugInfo);
            }
            framerateLabel.Text = $"{1.0 / debugInfo.DeltaTime:0.##} fps";
            bulletCountLabel.Text = debugInfo.BulletCount.ToString();
            playerCountLabel.Text = debugInfo.PlayerCount.ToString();
            enemyCountLabel.Text = debugInfo.EnemyCount.ToString();
            shipCountLabel.Text = debugInfo.ShipCount.ToString();
        }
    }
}

using Game.Dependencies;
using System;
using System.Linq;
using System.Windows.Forms;

namespace WindowsGame
{
    public partial class DebugInfoForm : Form
    {
        private delegate void SetDebugInfo(DebugInfo debugInfo);
        private SetDebugInfo UpdateValuesDelegate;
        private float[] deltaTimes = new float[16];
        private int index;

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
                return;
            }
            deltaTimes[index++] = debugInfo.DeltaTime;
            index &= 0xF;
            var averageDeltaTime = deltaTimes.Aggregate((f1, f2) => f1 + f2) / 16;
            framerateLabel.Text = $"{1.0 / debugInfo.DeltaTime:0.##} fps (average: {1.0 / averageDeltaTime:0.##} fps)";
            bulletCountLabel.Text = debugInfo.BulletCount.ToString();
            playerCountLabel.Text = debugInfo.PlayerCount.ToString();
            enemyCountLabel.Text = debugInfo.EnemyCount.ToString();
            shipCountLabel.Text = debugInfo.ShipCount.ToString();
        }
    }
}

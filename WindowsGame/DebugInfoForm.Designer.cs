namespace WindowsGame
{
    partial class DebugInfoForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.bulletCountLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.framerateLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.shipCountLabel = new System.Windows.Forms.Label();
            this.enemyCountLabel = new System.Windows.Forms.Label();
            this.playerCountLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Bullet count:";
            // 
            // bulletCountLabel
            // 
            this.bulletCountLabel.AutoSize = true;
            this.bulletCountLabel.Location = new System.Drawing.Point(90, 31);
            this.bulletCountLabel.Name = "bulletCountLabel";
            this.bulletCountLabel.Size = new System.Drawing.Size(14, 13);
            this.bulletCountLabel.TabIndex = 1;
            this.bulletCountLabel.Text = "#";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Framerate:";
            // 
            // framerateLabel
            // 
            this.framerateLabel.AutoSize = true;
            this.framerateLabel.Location = new System.Drawing.Point(90, 12);
            this.framerateLabel.Name = "framerateLabel";
            this.framerateLabel.Size = new System.Drawing.Size(14, 13);
            this.framerateLabel.TabIndex = 3;
            this.framerateLabel.Text = "#";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Ship count:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Enemy count:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 88);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Player count";
            // 
            // shipCountLabel
            // 
            this.shipCountLabel.AutoSize = true;
            this.shipCountLabel.Location = new System.Drawing.Point(90, 50);
            this.shipCountLabel.Name = "shipCountLabel";
            this.shipCountLabel.Size = new System.Drawing.Size(14, 13);
            this.shipCountLabel.TabIndex = 7;
            this.shipCountLabel.Text = "#";
            // 
            // enemyCountLabel
            // 
            this.enemyCountLabel.AutoSize = true;
            this.enemyCountLabel.Location = new System.Drawing.Point(90, 69);
            this.enemyCountLabel.Name = "enemyCountLabel";
            this.enemyCountLabel.Size = new System.Drawing.Size(14, 13);
            this.enemyCountLabel.TabIndex = 8;
            this.enemyCountLabel.Text = "#";
            // 
            // playerCountLabel
            // 
            this.playerCountLabel.AutoSize = true;
            this.playerCountLabel.Location = new System.Drawing.Point(90, 88);
            this.playerCountLabel.Name = "playerCountLabel";
            this.playerCountLabel.Size = new System.Drawing.Size(14, 13);
            this.playerCountLabel.TabIndex = 9;
            this.playerCountLabel.Text = "#";
            // 
            // DebugInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(237, 112);
            this.ControlBox = false;
            this.Controls.Add(this.playerCountLabel);
            this.Controls.Add(this.enemyCountLabel);
            this.Controls.Add(this.shipCountLabel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.framerateLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.bulletCountLabel);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DebugInfoForm";
            this.Text = "DebugInfo";
            this.Load += new System.EventHandler(this.DebugInfo_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label bulletCountLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label framerateLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label shipCountLabel;
        private System.Windows.Forms.Label enemyCountLabel;
        private System.Windows.Forms.Label playerCountLabel;
    }
}
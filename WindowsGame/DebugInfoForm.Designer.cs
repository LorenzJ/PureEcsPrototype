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
            this.label6 = new System.Windows.Forms.Label();
            this.frametimeLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 11);
            this.label1.TabIndex = 0;
            this.label1.Text = "Bullet count:";
            // 
            // bulletCountLabel
            // 
            this.bulletCountLabel.AutoSize = true;
            this.bulletCountLabel.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bulletCountLabel.Location = new System.Drawing.Point(108, 50);
            this.bulletCountLabel.Name = "bulletCountLabel";
            this.bulletCountLabel.Size = new System.Drawing.Size(12, 11);
            this.bulletCountLabel.TabIndex = 1;
            this.bulletCountLabel.Text = "#";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 11);
            this.label2.TabIndex = 2;
            this.label2.Text = "Framerate:";
            // 
            // framerateLabel
            // 
            this.framerateLabel.AutoSize = true;
            this.framerateLabel.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.framerateLabel.Location = new System.Drawing.Point(108, 12);
            this.framerateLabel.Name = "framerateLabel";
            this.framerateLabel.Size = new System.Drawing.Size(12, 11);
            this.framerateLabel.TabIndex = 3;
            this.framerateLabel.Text = "#";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 11);
            this.label3.TabIndex = 4;
            this.label3.Text = "Ship count:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(13, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 11);
            this.label4.TabIndex = 5;
            this.label4.Text = "Enemy count:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(12, 107);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 11);
            this.label5.TabIndex = 6;
            this.label5.Text = "Player count";
            // 
            // shipCountLabel
            // 
            this.shipCountLabel.AutoSize = true;
            this.shipCountLabel.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.shipCountLabel.Location = new System.Drawing.Point(108, 69);
            this.shipCountLabel.Name = "shipCountLabel";
            this.shipCountLabel.Size = new System.Drawing.Size(12, 11);
            this.shipCountLabel.TabIndex = 7;
            this.shipCountLabel.Text = "#";
            // 
            // enemyCountLabel
            // 
            this.enemyCountLabel.AutoSize = true;
            this.enemyCountLabel.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.enemyCountLabel.Location = new System.Drawing.Point(108, 88);
            this.enemyCountLabel.Name = "enemyCountLabel";
            this.enemyCountLabel.Size = new System.Drawing.Size(12, 11);
            this.enemyCountLabel.TabIndex = 8;
            this.enemyCountLabel.Text = "#";
            // 
            // playerCountLabel
            // 
            this.playerCountLabel.AutoSize = true;
            this.playerCountLabel.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.playerCountLabel.Location = new System.Drawing.Point(108, 107);
            this.playerCountLabel.Name = "playerCountLabel";
            this.playerCountLabel.Size = new System.Drawing.Size(12, 11);
            this.playerCountLabel.TabIndex = 9;
            this.playerCountLabel.Text = "#";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(12, 31);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 11);
            this.label6.TabIndex = 10;
            this.label6.Text = "Frametime:";
            // 
            // frametimeLabel
            // 
            this.frametimeLabel.AutoSize = true;
            this.frametimeLabel.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.frametimeLabel.Location = new System.Drawing.Point(108, 31);
            this.frametimeLabel.Name = "frametimeLabel";
            this.frametimeLabel.Size = new System.Drawing.Size(12, 11);
            this.frametimeLabel.TabIndex = 11;
            this.frametimeLabel.Text = "#";
            // 
            // DebugInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 140);
            this.ControlBox = false;
            this.Controls.Add(this.frametimeLabel);
            this.Controls.Add(this.label6);
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
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label frametimeLabel;
    }
}
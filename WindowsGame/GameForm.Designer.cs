namespace WindowsGame
{
    partial class GameForm
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
            this.components = new System.ComponentModel.Container();
            this.glControl = new OpenGL.GlControl();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // glControl
            // 
            this.glControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.glControl.ColorBits = ((uint)(32u));
            this.glControl.ContextProfile = OpenGL.GlControl.ProfileType.Core;
            this.glControl.DepthBits = ((uint)(24u));
            this.glControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glControl.Location = new System.Drawing.Point(0, 0);
            this.glControl.MultisampleBits = ((uint)(0u));
            this.glControl.Name = "glControl";
            this.glControl.Size = new System.Drawing.Size(623, 449);
            this.glControl.StencilBits = ((uint)(0u));
            this.glControl.TabIndex = 0;
            this.glControl.ContextCreated += new System.EventHandler<OpenGL.GlControlEventArgs>(this.GlControl1_ContextCreated);
            this.glControl.Render += new System.EventHandler<OpenGL.GlControlEventArgs>(this.GlControl_Render);
            this.glControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GlControl_KeyDown);
            this.glControl.KeyUp += new System.Windows.Forms.KeyEventHandler(this.glControl_KeyUp);
            this.glControl.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.glControl_PreviewKeyDown);
            this.glControl.Resize += new System.EventHandler(this.GlControl_Resize);
            // 
            // timer
            // 
            this.timer.Interval = 1;
            this.timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(623, 449);
            this.Controls.Add(this.glControl);
            this.KeyPreview = true;
            this.Name = "GameForm";
            this.Text = "GameForm";
            this.Load += new System.EventHandler(this.GameForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private OpenGL.GlControl glControl;
        private System.Windows.Forms.Timer timer;
    }
}
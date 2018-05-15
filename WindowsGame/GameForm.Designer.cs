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
            this.updateInfo = new System.Windows.Forms.Timer(this.components);
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.glControl = new OpenGL.GlControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.entityList = new System.Windows.Forms.ListBox();
            this.componentList = new System.Windows.Forms.ListBox();
            this.fieldList = new System.Windows.Forms.ListBox();
            this.pauseResumeButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.frametimeLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.framerateLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.timestepInput = new System.Windows.Forms.MaskedTextBox();
            this.timescaleInput = new System.Windows.Forms.MaskedTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.glControl);
            this.splitContainer.Panel1MinSize = 200;
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer.Panel2.Controls.Add(this.pauseResumeButton);
            this.splitContainer.Panel2.Controls.Add(this.tableLayoutPanel);
            this.splitContainer.Panel2MinSize = 200;
            this.splitContainer.Size = new System.Drawing.Size(845, 427);
            this.splitContainer.SplitterDistance = 334;
            this.splitContainer.TabIndex = 0;
            // 
            // glControl
            // 
            this.glControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.glControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.glControl.ColorBits = ((uint)(32u));
            this.glControl.ContextProfile = OpenGL.GlControl.ProfileType.Core;
            this.glControl.DepthBits = ((uint)(24u));
            this.glControl.Location = new System.Drawing.Point(12, 12);
            this.glControl.MultisampleBits = ((uint)(0u));
            this.glControl.Name = "glControl";
            this.glControl.Size = new System.Drawing.Size(304, 396);
            this.glControl.StencilBits = ((uint)(0u));
            this.glControl.SwapInterval = 0;
            this.glControl.TabIndex = 1;
            this.glControl.ContextCreated += new System.EventHandler<OpenGL.GlControlEventArgs>(this.GlControl1_ContextCreated);
            this.glControl.Render += new System.EventHandler<OpenGL.GlControlEventArgs>(this.GlControl_Render);
            this.glControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GlControl_KeyDown);
            this.glControl.KeyUp += new System.Windows.Forms.KeyEventHandler(this.GlControl_KeyUp);
            this.glControl.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.GlControl_PreviewKeyDown);
            this.glControl.Resize += new System.EventHandler(this.GlControl_Resize);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.entityList, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.componentList, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.fieldList, 2, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 173);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(490, 247);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // entityList
            // 
            this.entityList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.entityList.FormattingEnabled = true;
            this.entityList.Location = new System.Drawing.Point(4, 4);
            this.entityList.Name = "entityList";
            this.entityList.Size = new System.Drawing.Size(156, 239);
            this.entityList.TabIndex = 3;
            this.entityList.SelectedValueChanged += new System.EventHandler(this.EntityList_SelectedValueChanged);
            // 
            // componentList
            // 
            this.componentList.DisplayMember = "Name";
            this.componentList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.componentList.FormattingEnabled = true;
            this.componentList.Location = new System.Drawing.Point(167, 4);
            this.componentList.Name = "componentList";
            this.componentList.Size = new System.Drawing.Size(156, 239);
            this.componentList.TabIndex = 4;
            this.componentList.SelectedValueChanged += new System.EventHandler(this.ComponentList_SelectedValueChanged);
            // 
            // fieldList
            // 
            this.fieldList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fieldList.FormattingEnabled = true;
            this.fieldList.Location = new System.Drawing.Point(330, 4);
            this.fieldList.Name = "fieldList";
            this.fieldList.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.fieldList.Size = new System.Drawing.Size(156, 239);
            this.fieldList.TabIndex = 5;
            // 
            // pauseResumeButton
            // 
            this.pauseResumeButton.Location = new System.Drawing.Point(3, 3);
            this.pauseResumeButton.Name = "pauseResumeButton";
            this.pauseResumeButton.Size = new System.Drawing.Size(33, 19);
            this.pauseResumeButton.TabIndex = 1;
            this.pauseResumeButton.Text = "||►";
            this.pauseResumeButton.UseVisualStyleBackColor = true;
            this.pauseResumeButton.Click += new System.EventHandler(this.PauseResumeButton_Click);
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble;
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.frametimeLabel, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.framerateLabel, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.timestepInput, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.timescaleInput, 1, 3);
            this.tableLayoutPanel.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayoutPanel.Location = new System.Drawing.Point(3, 28);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.Padding = new System.Windows.Forms.Padding(5, 10, 5, 10);
            this.tableLayoutPanel.RowCount = 4;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(491, 139);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(11, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 26);
            this.label1.TabIndex = 1;
            this.label1.Text = "Frametime";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // frametimeLabel
            // 
            this.frametimeLabel.AutoSize = true;
            this.frametimeLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.frametimeLabel.Location = new System.Drawing.Point(81, 13);
            this.frametimeLabel.Name = "frametimeLabel";
            this.frametimeLabel.Size = new System.Drawing.Size(399, 26);
            this.frametimeLabel.TabIndex = 2;
            this.frametimeLabel.Text = "value";
            this.frametimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(11, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 26);
            this.label2.TabIndex = 3;
            this.label2.Text = "Framerate";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // framerateLabel
            // 
            this.framerateLabel.AutoSize = true;
            this.framerateLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.framerateLabel.Location = new System.Drawing.Point(81, 42);
            this.framerateLabel.Name = "framerateLabel";
            this.framerateLabel.Size = new System.Drawing.Size(399, 26);
            this.framerateLabel.TabIndex = 4;
            this.framerateLabel.Text = "value";
            this.framerateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(11, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 26);
            this.label3.TabIndex = 5;
            this.label3.Text = "Timestep";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(11, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 26);
            this.label4.TabIndex = 7;
            this.label4.Text = "Timescale";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // timestepInput
            // 
            this.timestepInput.Culture = new System.Globalization.CultureInfo("");
            this.timestepInput.Location = new System.Drawing.Point(81, 74);
            this.timestepInput.Mask = "0.0000";
            this.timestepInput.Name = "timestepInput";
            this.timestepInput.PromptChar = '0';
            this.timestepInput.Size = new System.Drawing.Size(46, 20);
            this.timestepInput.TabIndex = 8;
            this.timestepInput.TextMaskFormat = System.Windows.Forms.MaskFormat.IncludePromptAndLiterals;
            this.timestepInput.Validating += new System.ComponentModel.CancelEventHandler(this.TimestepInput_Validating);
            this.timestepInput.Validated += new System.EventHandler(this.TimestepInput_Validated);
            // 
            // timescaleInput
            // 
            this.timescaleInput.Culture = new System.Globalization.CultureInfo("");
            this.timescaleInput.Location = new System.Drawing.Point(81, 103);
            this.timescaleInput.Mask = "0.0000";
            this.timescaleInput.Name = "timescaleInput";
            this.timescaleInput.PromptChar = '0';
            this.timescaleInput.Size = new System.Drawing.Size(46, 20);
            this.timescaleInput.TabIndex = 9;
            this.timescaleInput.TextMaskFormat = System.Windows.Forms.MaskFormat.IncludePromptAndLiterals;
            this.timescaleInput.Validating += new System.ComponentModel.CancelEventHandler(this.TimescaleInput_Validating);
            this.timescaleInput.Validated += new System.EventHandler(this.TimescaleInput_Validated);
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(845, 427);
            this.Controls.Add(this.splitContainer);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Crimson;
            this.KeyPreview = true;
            this.Name = "GameForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Game";
            this.Load += new System.EventHandler(this.GameForm_Load);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer updateInfo;
        private System.Windows.Forms.SplitContainer splitContainer;
        private OpenGL.GlControl glControl;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label frametimeLabel;
        private System.Windows.Forms.Button pauseResumeButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label framerateLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.MaskedTextBox timestepInput;
        private System.Windows.Forms.MaskedTextBox timescaleInput;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ListBox entityList;
        private System.Windows.Forms.ListBox componentList;
        private System.Windows.Forms.ListBox fieldList;
    }
}
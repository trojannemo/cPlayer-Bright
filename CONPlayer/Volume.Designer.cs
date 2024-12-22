namespace cPlayer
{
    partial class Volume
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
            this.lblVolume = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.picSlider = new System.Windows.Forms.PictureBox();
            this.picBackground = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBackground)).BeginInit();
            this.SuspendLayout();
            // 
            // lblVolume
            // 
            this.lblVolume.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblVolume.BackColor = System.Drawing.Color.Transparent;
            this.lblVolume.ForeColor = System.Drawing.Color.Black;
            this.lblVolume.Location = new System.Drawing.Point(0, 173);
            this.lblVolume.Name = "lblVolume";
            this.lblVolume.Size = new System.Drawing.Size(146, 13);
            this.lblVolume.TabIndex = 4;
            this.lblVolume.Text = "Vol: 50";
            this.lblVolume.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picSlider
            // 
            this.picSlider.BackColor = System.Drawing.Color.Transparent;
            this.picSlider.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picSlider.Image = global::cPlayer.Properties.Resources.vol_slider;
            this.picSlider.Location = new System.Drawing.Point(10, 68);
            this.picSlider.Name = "picSlider";
            this.picSlider.Size = new System.Drawing.Size(126, 30);
            this.picSlider.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picSlider.TabIndex = 1;
            this.picSlider.TabStop = false;
            this.toolTip1.SetToolTip(this.picSlider, "Click to drag the slider");
            this.picSlider.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picSlider_MouseDown);
            this.picSlider.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picSlider_MouseMove);
            this.picSlider.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picSlider_MouseUp);
            // 
            // picBackground
            // 
            this.picBackground.Image = global::cPlayer.Properties.Resources.vol_bg;
            this.picBackground.Location = new System.Drawing.Point(34, 12);
            this.picBackground.Name = "picBackground";
            this.picBackground.Size = new System.Drawing.Size(80, 150);
            this.picBackground.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBackground.TabIndex = 0;
            this.picBackground.TabStop = false;
            this.toolTip1.SetToolTip(this.picBackground, "Move the slider to adjust the volume");
            this.picBackground.Click += new System.EventHandler(this.picBackground_Click);
            // 
            // Volume
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(146, 189);
            this.Controls.Add(this.picSlider);
            this.Controls.Add(this.picBackground);
            this.Controls.Add(this.lblVolume);
            this.ForeColor = System.Drawing.Color.Transparent;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Volume";
            this.Opacity = 0.9D;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Volume";
            this.Deactivate += new System.EventHandler(this.Volume_Deactivate);
            this.Shown += new System.EventHandler(this.Volume_Shown);
            this.Click += new System.EventHandler(this.Volume_Click);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Volume_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.picSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBackground)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picBackground;
        private System.Windows.Forms.PictureBox picSlider;
        private System.Windows.Forms.Label lblVolume;
        private System.Windows.Forms.ToolTip toolTip1;

    }
}
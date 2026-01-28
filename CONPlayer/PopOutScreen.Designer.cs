namespace cPlayer
{
    partial class PopOutScreen
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
            this.picVisuals = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picVisuals)).BeginInit();
            this.SuspendLayout();
            // 
            // picVisuals
            // 
            this.picVisuals.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picVisuals.BackColor = System.Drawing.Color.AliceBlue;
            this.picVisuals.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picVisuals.Image = global::cPlayer.Properties.Resources.logo;
            this.picVisuals.Location = new System.Drawing.Point(0, 0);
            this.picVisuals.Name = "picVisuals";
            this.picVisuals.Size = new System.Drawing.Size(1096, 754);
            this.picVisuals.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picVisuals.TabIndex = 0;
            this.picVisuals.TabStop = false;
            // 
            // PopOutScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(1096, 754);
            this.Controls.Add(this.picVisuals);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PopOutScreen";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.picVisuals)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picVisuals;
    }
}
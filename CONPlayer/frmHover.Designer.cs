namespace cPlayer
{
    partial class frmHover
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
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.picGear = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picGear)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 250;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // picGear
            // 
            this.picGear.Image = global::cPlayer.Properties.Resources.settings;
            this.picGear.Location = new System.Drawing.Point(0, 0);
            this.picGear.Name = "picGear";
            this.picGear.Size = new System.Drawing.Size(50, 50);
            this.picGear.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picGear.TabIndex = 0;
            this.picGear.TabStop = false;
            this.toolTip1.SetToolTip(this.picGear, "Click to open display settings");
            this.picGear.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picGear_MouseClick);
            this.picGear.MouseEnter += new System.EventHandler(this.picGear_MouseEnter);
            this.picGear.MouseLeave += new System.EventHandler(this.picGear_MouseLeave);
            // 
            // frmHover
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(50, 50);
            this.Controls.Add(this.picGear);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmHover";
            this.Opacity = 0.5D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.Black;
            ((System.ComponentModel.ISupportInitialize)(this.picGear)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox picGear;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
namespace cPlayer
{
    partial class BTAVSync
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
            this.trackbarAV = new System.Windows.Forms.TrackBar();
            this.btnLeft = new System.Windows.Forms.Button();
            this.btnRight = new System.Windows.Forms.Button();
            this.chkEnable = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.trackbarAV)).BeginInit();
            this.SuspendLayout();
            // 
            // trackbarAV
            // 
            this.trackbarAV.Enabled = false;
            this.trackbarAV.LargeChange = 50;
            this.trackbarAV.Location = new System.Drawing.Point(56, 35);
            this.trackbarAV.Maximum = 3000;
            this.trackbarAV.Name = "trackbarAV";
            this.trackbarAV.Size = new System.Drawing.Size(383, 45);
            this.trackbarAV.SmallChange = 50;
            this.trackbarAV.TabIndex = 0;
            this.trackbarAV.TickFrequency = 50;
            this.trackbarAV.ValueChanged += new System.EventHandler(this.trackbarAV_ValueChanged);
            // 
            // btnLeft
            // 
            this.btnLeft.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLeft.Enabled = false;
            this.btnLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLeft.Location = new System.Drawing.Point(12, 35);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(38, 45);
            this.btnLeft.TabIndex = 1;
            this.btnLeft.Text = "<<<";
            this.btnLeft.UseVisualStyleBackColor = true;
            this.btnLeft.Click += new System.EventHandler(this.btnLeft_Click);
            // 
            // btnRight
            // 
            this.btnRight.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRight.Enabled = false;
            this.btnRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRight.Location = new System.Drawing.Point(445, 35);
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size(38, 45);
            this.btnRight.TabIndex = 2;
            this.btnRight.Text = ">>>";
            this.btnRight.UseVisualStyleBackColor = true;
            this.btnRight.Click += new System.EventHandler(this.btnRight_Click);
            // 
            // chkEnable
            // 
            this.chkEnable.AutoSize = true;
            this.chkEnable.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkEnable.Location = new System.Drawing.Point(12, 12);
            this.chkEnable.Name = "chkEnable";
            this.chkEnable.Size = new System.Drawing.Size(182, 17);
            this.chkEnable.TabIndex = 3;
            this.chkEnable.Text = "Enable Bluetooth AV Offset Sync";
            this.chkEnable.UseVisualStyleBackColor = true;
            this.chkEnable.CheckedChanged += new System.EventHandler(this.chkEnable_CheckedChanged);
            // 
            // BTAVSync
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(496, 91);
            this.Controls.Add(this.chkEnable);
            this.Controls.Add(this.btnRight);
            this.Controls.Add(this.btnLeft);
            this.Controls.Add(this.trackbarAV);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BTAVSync";
            this.Opacity = 0.95D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bluetooth AV Offset: 0ms";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BTAVSync_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.trackbarAV)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar trackbarAV;
        private System.Windows.Forms.Button btnLeft;
        private System.Windows.Forms.Button btnRight;
        private System.Windows.Forms.CheckBox chkEnable;
    }
}
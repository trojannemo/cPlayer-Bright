using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace cPlayer
{
    partial class MIDISelector
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
            this.chkDrums = new System.Windows.Forms.CheckBox();
            this.chkBass = new System.Windows.Forms.CheckBox();
            this.chkGuitar = new System.Windows.Forms.CheckBox();
            this.btnAll = new System.Windows.Forms.Button();
            this.btnNone = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.radioNoVocals = new System.Windows.Forms.RadioButton();
            this.radioVocals = new System.Windows.Forms.RadioButton();
            this.radioHarms = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radioNoKeys = new System.Windows.Forms.RadioButton();
            this.radioKeys = new System.Windows.Forms.RadioButton();
            this.radioProKeys = new System.Windows.Forms.RadioButton();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkHarmColorOnVocals = new System.Windows.Forms.CheckBox();
            this.chkBWKeys = new System.Windows.Forms.CheckBox();
            this.chkHighlightSolos = new System.Windows.Forms.CheckBox();
            this.chkNameProKeys = new System.Windows.Forms.CheckBox();
            this.chkNameVocals = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cboWindow = new System.Windows.Forms.ComboBox();
            this.cboSizing = new System.Windows.Forms.ComboBox();
            this.chkNameTracks = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkDrums
            // 
            this.chkDrums.AutoSize = true;
            this.chkDrums.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkDrums.Location = new System.Drawing.Point(15, 23);
            this.chkDrums.Name = "chkDrums";
            this.chkDrums.Size = new System.Drawing.Size(56, 17);
            this.chkDrums.TabIndex = 0;
            this.chkDrums.Text = "Drums";
            this.chkDrums.UseVisualStyleBackColor = true;
            this.chkDrums.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chkDrums_MouseUp);
            // 
            // chkBass
            // 
            this.chkBass.AutoSize = true;
            this.chkBass.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkBass.Location = new System.Drawing.Point(112, 23);
            this.chkBass.Name = "chkBass";
            this.chkBass.Size = new System.Drawing.Size(49, 17);
            this.chkBass.TabIndex = 1;
            this.chkBass.Text = "Bass";
            this.chkBass.UseVisualStyleBackColor = true;
            this.chkBass.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chkDrums_MouseUp);
            // 
            // chkGuitar
            // 
            this.chkGuitar.AutoSize = true;
            this.chkGuitar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkGuitar.Location = new System.Drawing.Point(201, 23);
            this.chkGuitar.Name = "chkGuitar";
            this.chkGuitar.Size = new System.Drawing.Size(54, 17);
            this.chkGuitar.TabIndex = 2;
            this.chkGuitar.Text = "Guitar";
            this.chkGuitar.UseVisualStyleBackColor = true;
            this.chkGuitar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chkDrums_MouseUp);
            // 
            // btnAll
            // 
            this.btnAll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAll.Location = new System.Drawing.Point(6, 102);
            this.btnAll.Name = "btnAll";
            this.btnAll.Size = new System.Drawing.Size(60, 23);
            this.btnAll.TabIndex = 7;
            this.btnAll.Text = "All";
            this.btnAll.UseVisualStyleBackColor = true;
            this.btnAll.Click += new System.EventHandler(this.btnAll_Click);
            // 
            // btnNone
            // 
            this.btnNone.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNone.Location = new System.Drawing.Point(72, 102);
            this.btnNone.Name = "btnNone";
            this.btnNone.Size = new System.Drawing.Size(60, 23);
            this.btnNone.TabIndex = 8;
            this.btnNone.Text = "None";
            this.btnNone.UseVisualStyleBackColor = true;
            this.btnNone.Click += new System.EventHandler(this.btnNone_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Controls.Add(this.chkDrums);
            this.groupBox1.Controls.Add(this.btnNone);
            this.groupBox1.Controls.Add(this.chkBass);
            this.groupBox1.Controls.Add(this.btnAll);
            this.groupBox1.Controls.Add(this.chkGuitar);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(269, 134);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "MIDI charts to display:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.radioNoVocals);
            this.groupBox4.Controls.Add(this.radioVocals);
            this.groupBox4.Controls.Add(this.radioHarms);
            this.groupBox4.Location = new System.Drawing.Point(139, 38);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(124, 58);
            this.groupBox4.TabIndex = 12;
            this.groupBox4.TabStop = false;
            // 
            // radioNoVocals
            // 
            this.radioNoVocals.AutoSize = true;
            this.radioNoVocals.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radioNoVocals.Location = new System.Drawing.Point(26, 34);
            this.radioNoVocals.Name = "radioNoVocals";
            this.radioNoVocals.Size = new System.Drawing.Size(74, 17);
            this.radioNoVocals.TabIndex = 3;
            this.radioNoVocals.Text = "No Vocals";
            this.radioNoVocals.UseVisualStyleBackColor = true;
            this.radioNoVocals.CheckedChanged += new System.EventHandler(this.radioHarms_CheckedChanged);
            // 
            // radioVocals
            // 
            this.radioVocals.AutoSize = true;
            this.radioVocals.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radioVocals.Location = new System.Drawing.Point(66, 11);
            this.radioVocals.Name = "radioVocals";
            this.radioVocals.Size = new System.Drawing.Size(57, 17);
            this.radioVocals.TabIndex = 2;
            this.radioVocals.Text = "Vocals";
            this.radioVocals.UseVisualStyleBackColor = true;
            this.radioVocals.CheckedChanged += new System.EventHandler(this.radioHarms_CheckedChanged);
            // 
            // radioHarms
            // 
            this.radioHarms.AutoSize = true;
            this.radioHarms.Checked = true;
            this.radioHarms.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radioHarms.Location = new System.Drawing.Point(6, 11);
            this.radioHarms.Name = "radioHarms";
            this.radioHarms.Size = new System.Drawing.Size(55, 17);
            this.radioHarms.TabIndex = 1;
            this.radioHarms.TabStop = true;
            this.radioHarms.Text = "Harms";
            this.radioHarms.UseVisualStyleBackColor = true;
            this.radioHarms.CheckedChanged += new System.EventHandler(this.radioHarms_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.radioNoKeys);
            this.groupBox3.Controls.Add(this.radioKeys);
            this.groupBox3.Controls.Add(this.radioProKeys);
            this.groupBox3.Location = new System.Drawing.Point(6, 38);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(128, 58);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            // 
            // radioNoKeys
            // 
            this.radioNoKeys.AutoSize = true;
            this.radioNoKeys.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radioNoKeys.Location = new System.Drawing.Point(34, 34);
            this.radioNoKeys.Name = "radioNoKeys";
            this.radioNoKeys.Size = new System.Drawing.Size(65, 17);
            this.radioNoKeys.TabIndex = 2;
            this.radioNoKeys.Text = "No Keys";
            this.radioNoKeys.UseVisualStyleBackColor = true;
            this.radioNoKeys.CheckedChanged += new System.EventHandler(this.radioProKeys_CheckedChanged);
            // 
            // radioKeys
            // 
            this.radioKeys.AutoSize = true;
            this.radioKeys.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radioKeys.Location = new System.Drawing.Point(78, 11);
            this.radioKeys.Name = "radioKeys";
            this.radioKeys.Size = new System.Drawing.Size(48, 17);
            this.radioKeys.TabIndex = 1;
            this.radioKeys.Text = "Keys";
            this.radioKeys.UseVisualStyleBackColor = true;
            this.radioKeys.CheckedChanged += new System.EventHandler(this.radioProKeys_CheckedChanged);
            // 
            // radioProKeys
            // 
            this.radioProKeys.AutoSize = true;
            this.radioProKeys.Checked = true;
            this.radioProKeys.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radioProKeys.Location = new System.Drawing.Point(6, 11);
            this.radioProKeys.Name = "radioProKeys";
            this.radioProKeys.Size = new System.Drawing.Size(67, 17);
            this.radioProKeys.TabIndex = 0;
            this.radioProKeys.TabStop = true;
            this.radioProKeys.Text = "Pro-Keys";
            this.radioProKeys.UseVisualStyleBackColor = true;
            this.radioProKeys.CheckedChanged += new System.EventHandler(this.radioProKeys_CheckedChanged);
            // 
            // btnSave
            // 
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.Location = new System.Drawing.Point(203, 102);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(60, 23);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "Close";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkHarmColorOnVocals);
            this.groupBox2.Controls.Add(this.chkBWKeys);
            this.groupBox2.Controls.Add(this.chkHighlightSolos);
            this.groupBox2.Controls.Add(this.chkNameProKeys);
            this.groupBox2.Controls.Add(this.chkNameVocals);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.cboWindow);
            this.groupBox2.Controls.Add(this.cboSizing);
            this.groupBox2.Controls.Add(this.chkNameTracks);
            this.groupBox2.Location = new System.Drawing.Point(12, 152);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(269, 157);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Misc options:";
            // 
            // chkHarmColorOnVocals
            // 
            this.chkHarmColorOnVocals.AutoSize = true;
            this.chkHarmColorOnVocals.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkHarmColorOnVocals.Location = new System.Drawing.Point(15, 111);
            this.chkHarmColorOnVocals.Name = "chkHarmColorOnVocals";
            this.chkHarmColorOnVocals.Size = new System.Drawing.Size(197, 17);
            this.chkHarmColorOnVocals.TabIndex = 8;
            this.chkHarmColorOnVocals.Text = "Use Harm1 color for PART VOCALS";
            this.chkHarmColorOnVocals.UseVisualStyleBackColor = true;
            this.chkHarmColorOnVocals.CheckedChanged += new System.EventHandler(this.chkHarmColorOnVocals_CheckedChanged);
            // 
            // chkBWKeys
            // 
            this.chkBWKeys.AutoSize = true;
            this.chkBWKeys.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkBWKeys.Location = new System.Drawing.Point(15, 65);
            this.chkBWKeys.Name = "chkBWKeys";
            this.chkBWKeys.Size = new System.Drawing.Size(147, 17);
            this.chkBWKeys.TabIndex = 7;
            this.chkBWKeys.Text = "Black and white Pro-Keys";
            this.chkBWKeys.UseVisualStyleBackColor = true;
            this.chkBWKeys.CheckedChanged += new System.EventHandler(this.chkBWKeys_CheckedChanged);
            // 
            // chkHighlightSolos
            // 
            this.chkHighlightSolos.AutoSize = true;
            this.chkHighlightSolos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkHighlightSolos.Location = new System.Drawing.Point(15, 42);
            this.chkHighlightSolos.Name = "chkHighlightSolos";
            this.chkHighlightSolos.Size = new System.Drawing.Size(158, 17);
            this.chkHighlightSolos.TabIndex = 6;
            this.chkHighlightSolos.Text = "Highlight tracks during solos";
            this.chkHighlightSolos.UseVisualStyleBackColor = true;
            this.chkHighlightSolos.CheckedChanged += new System.EventHandler(this.chkHighlightSolos_CheckedChanged);
            // 
            // chkNameProKeys
            // 
            this.chkNameProKeys.AutoSize = true;
            this.chkNameProKeys.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkNameProKeys.Location = new System.Drawing.Point(15, 88);
            this.chkNameProKeys.Name = "chkNameProKeys";
            this.chkNameProKeys.Size = new System.Drawing.Size(171, 17);
            this.chkNameProKeys.TabIndex = 5;
            this.chkNameProKeys.Text = "Show note names for Pro-Keys";
            this.chkNameProKeys.UseVisualStyleBackColor = true;
            this.chkNameProKeys.CheckedChanged += new System.EventHandler(this.chkNameProKeys_CheckedChanged);
            // 
            // chkNameVocals
            // 
            this.chkNameVocals.AutoSize = true;
            this.chkNameVocals.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkNameVocals.Location = new System.Drawing.Point(15, 134);
            this.chkNameVocals.Name = "chkNameVocals";
            this.chkNameVocals.Size = new System.Drawing.Size(216, 17);
            this.chkNameVocals.TabIndex = 4;
            this.chkNameVocals.Text = "Show note names for Vocals/Harmonies";
            this.chkNameVocals.UseVisualStyleBackColor = true;
            this.chkNameVocals.CheckedChanged += new System.EventHandler(this.chkNameVocals_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 187);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "MIDI chart window:";
            // 
            // cboWindow
            // 
            this.cboWindow.Enabled = false;
            this.cboWindow.FormattingEnabled = true;
            this.cboWindow.Items.AddRange(new object[] {
            "1 second",
            "2 seconds",
            "3 seconds",
            "4 seconds",
            "5 seconds",
            "6 seconds",
            "7 seconds",
            "8 seconds",
            "9 seconds",
            "10 seconds"});
            this.cboWindow.Location = new System.Drawing.Point(122, 184);
            this.cboWindow.Name = "cboWindow";
            this.cboWindow.Size = new System.Drawing.Size(121, 21);
            this.cboWindow.TabIndex = 2;
            this.cboWindow.TabStop = false;
            this.cboWindow.SelectedIndexChanged += new System.EventHandler(this.cboWindow_SelectedIndexChanged);
            // 
            // cboSizing
            // 
            this.cboSizing.Enabled = false;
            this.cboSizing.FormattingEnabled = true;
            this.cboSizing.Items.AddRange(new object[] {
            "Size notes using mixed mode",
            "Size notes by charted note range",
            "Size notes by total valid note range"});
            this.cboSizing.Location = new System.Drawing.Point(15, 157);
            this.cboSizing.Name = "cboSizing";
            this.cboSizing.Size = new System.Drawing.Size(228, 21);
            this.cboSizing.TabIndex = 1;
            this.cboSizing.TabStop = false;
            this.cboSizing.SelectedIndexChanged += new System.EventHandler(this.cboSizing_SelectedIndexChanged);
            // 
            // chkNameTracks
            // 
            this.chkNameTracks.AutoSize = true;
            this.chkNameTracks.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkNameTracks.Location = new System.Drawing.Point(15, 19);
            this.chkNameTracks.Name = "chkNameTracks";
            this.chkNameTracks.Size = new System.Drawing.Size(132, 17);
            this.chkNameTracks.TabIndex = 0;
            this.chkNameTracks.Text = "Label each MIDI track";
            this.chkNameTracks.UseVisualStyleBackColor = true;
            this.chkNameTracks.CheckedChanged += new System.EventHandler(this.chkNameTracks_CheckedChanged);
            // 
            // MIDISelector
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(292, 317);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MIDISelector";
            this.Opacity = 0.95D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MIDI Settings";
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.MIDISelector_Shown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MIDISelector_KeyUp);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

}

        #endregion

        private System.Windows.Forms.CheckBox chkDrums;
        private System.Windows.Forms.CheckBox chkBass;
        private System.Windows.Forms.CheckBox chkGuitar;
        private System.Windows.Forms.Button btnAll;
        private System.Windows.Forms.Button btnNone;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkNameTracks;
        private System.Windows.Forms.ComboBox cboSizing;
        private System.Windows.Forms.ComboBox cboWindow;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkNameProKeys;
        private System.Windows.Forms.CheckBox chkNameVocals;
        private System.Windows.Forms.CheckBox chkHighlightSolos;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.CheckBox chkBWKeys;
        private System.Windows.Forms.CheckBox chkHarmColorOnVocals;
        private GroupBox groupBox3;
        private RadioButton radioKeys;
        private RadioButton radioProKeys;
        private GroupBox groupBox4;
        private RadioButton radioVocals;
        private RadioButton radioHarms;
        private RadioButton radioNoVocals;
        private RadioButton radioNoKeys;
    }
}
namespace cPlayer
{
    partial class AudioMixer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AudioMixer));
            this.chkCrowd = new System.Windows.Forms.CheckBox();
            this.chkBacking = new System.Windows.Forms.CheckBox();
            this.chkKeys = new System.Windows.Forms.CheckBox();
            this.chkVocals = new System.Windows.Forms.CheckBox();
            this.chkGuitar = new System.Windows.Forms.CheckBox();
            this.chkBass = new System.Windows.Forms.CheckBox();
            this.chkDrums = new System.Windows.Forms.CheckBox();
            this.picBassSlider = new System.Windows.Forms.PictureBox();
            this.picBassBackground = new System.Windows.Forms.PictureBox();
            this.picDrumsSlider = new System.Windows.Forms.PictureBox();
            this.picDrumsBackground = new System.Windows.Forms.PictureBox();
            this.picKeysSlider = new System.Windows.Forms.PictureBox();
            this.picKeysBackground = new System.Windows.Forms.PictureBox();
            this.picGuitarSlider = new System.Windows.Forms.PictureBox();
            this.picGuitarBackground = new System.Windows.Forms.PictureBox();
            this.picCrowdSlider = new System.Windows.Forms.PictureBox();
            this.picCrowdBackground = new System.Windows.Forms.PictureBox();
            this.picBackingSlider = new System.Windows.Forms.PictureBox();
            this.picBackingBackground = new System.Windows.Forms.PictureBox();
            this.picVocalsSlider = new System.Windows.Forms.PictureBox();
            this.picVocalsBackground = new System.Windows.Forms.PictureBox();
            this.lblBass = new System.Windows.Forms.Label();
            this.lblDrums = new System.Windows.Forms.Label();
            this.lblGuitar = new System.Windows.Forms.Label();
            this.lblKeys = new System.Windows.Forms.Label();
            this.lblVocals = new System.Windows.Forms.Label();
            this.lblBacking = new System.Windows.Forms.Label();
            this.lblCrowd = new System.Windows.Forms.Label();
            this.picSpectrum = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.picMasterSlider = new System.Windows.Forms.PictureBox();
            this.picMasterBackground = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblMaster = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picBassSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBassBackground)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDrumsSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDrumsBackground)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picKeysSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picKeysBackground)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGuitarSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGuitarBackground)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCrowdSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCrowdBackground)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBackingSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBackingBackground)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picVocalsSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picVocalsBackground)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSpectrum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMasterSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMasterBackground)).BeginInit();
            this.SuspendLayout();
            // 
            // chkCrowd
            // 
            this.chkCrowd.AutoSize = true;
            this.chkCrowd.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.chkCrowd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkCrowd.ForeColor = System.Drawing.Color.White;
            this.chkCrowd.Location = new System.Drawing.Point(624, 324);
            this.chkCrowd.Name = "chkCrowd";
            this.chkCrowd.Size = new System.Drawing.Size(41, 31);
            this.chkCrowd.TabIndex = 13;
            this.chkCrowd.TabStop = false;
            this.chkCrowd.Text = "Crowd";
            this.chkCrowd.UseVisualStyleBackColor = true;
            this.chkCrowd.CheckedChanged += new System.EventHandler(this.chkBass_CheckedChanged);
            // 
            // chkBacking
            // 
            this.chkBacking.AutoSize = true;
            this.chkBacking.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.chkBacking.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkBacking.ForeColor = System.Drawing.Color.White;
            this.chkBacking.Location = new System.Drawing.Point(546, 324);
            this.chkBacking.Name = "chkBacking";
            this.chkBacking.Size = new System.Drawing.Size(50, 31);
            this.chkBacking.TabIndex = 12;
            this.chkBacking.TabStop = false;
            this.chkBacking.Text = "Backing";
            this.chkBacking.UseVisualStyleBackColor = true;
            this.chkBacking.CheckedChanged += new System.EventHandler(this.chkBass_CheckedChanged);
            // 
            // chkKeys
            // 
            this.chkKeys.AutoSize = true;
            this.chkKeys.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.chkKeys.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkKeys.ForeColor = System.Drawing.Color.White;
            this.chkKeys.Location = new System.Drawing.Point(403, 324);
            this.chkKeys.Name = "chkKeys";
            this.chkKeys.Size = new System.Drawing.Size(34, 31);
            this.chkKeys.TabIndex = 11;
            this.chkKeys.TabStop = false;
            this.chkKeys.Text = "Keys";
            this.chkKeys.UseVisualStyleBackColor = true;
            this.chkKeys.CheckedChanged += new System.EventHandler(this.chkBass_CheckedChanged);
            // 
            // chkVocals
            // 
            this.chkVocals.AutoSize = true;
            this.chkVocals.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.chkVocals.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkVocals.ForeColor = System.Drawing.Color.White;
            this.chkVocals.Location = new System.Drawing.Point(473, 324);
            this.chkVocals.Name = "chkVocals";
            this.chkVocals.Size = new System.Drawing.Size(43, 31);
            this.chkVocals.TabIndex = 10;
            this.chkVocals.TabStop = false;
            this.chkVocals.Text = "Vocals";
            this.chkVocals.UseVisualStyleBackColor = true;
            this.chkVocals.CheckedChanged += new System.EventHandler(this.chkBass_CheckedChanged);
            // 
            // chkGuitar
            // 
            this.chkGuitar.AutoSize = true;
            this.chkGuitar.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.chkGuitar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkGuitar.ForeColor = System.Drawing.Color.White;
            this.chkGuitar.Location = new System.Drawing.Point(325, 324);
            this.chkGuitar.Name = "chkGuitar";
            this.chkGuitar.Size = new System.Drawing.Size(39, 31);
            this.chkGuitar.TabIndex = 9;
            this.chkGuitar.TabStop = false;
            this.chkGuitar.Text = "Guitar";
            this.chkGuitar.UseVisualStyleBackColor = true;
            this.chkGuitar.CheckedChanged += new System.EventHandler(this.chkBass_CheckedChanged);
            // 
            // chkBass
            // 
            this.chkBass.AutoSize = true;
            this.chkBass.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.chkBass.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkBass.ForeColor = System.Drawing.Color.White;
            this.chkBass.Location = new System.Drawing.Point(178, 324);
            this.chkBass.Name = "chkBass";
            this.chkBass.Size = new System.Drawing.Size(34, 31);
            this.chkBass.TabIndex = 8;
            this.chkBass.TabStop = false;
            this.chkBass.Text = "Bass";
            this.chkBass.UseVisualStyleBackColor = true;
            this.chkBass.CheckedChanged += new System.EventHandler(this.chkBass_CheckedChanged);
            // 
            // chkDrums
            // 
            this.chkDrums.AutoSize = true;
            this.chkDrums.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.chkDrums.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkDrums.ForeColor = System.Drawing.Color.White;
            this.chkDrums.Location = new System.Drawing.Point(249, 324);
            this.chkDrums.Name = "chkDrums";
            this.chkDrums.Size = new System.Drawing.Size(41, 31);
            this.chkDrums.TabIndex = 7;
            this.chkDrums.TabStop = false;
            this.chkDrums.Text = "Drums";
            this.chkDrums.UseVisualStyleBackColor = true;
            this.chkDrums.CheckedChanged += new System.EventHandler(this.chkBass_CheckedChanged);
            // 
            // picBassSlider
            // 
            this.picBassSlider.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picBassSlider.Enabled = false;
            this.picBassSlider.Image = ((System.Drawing.Image)(resources.GetObject("picBassSlider.Image")));
            this.picBassSlider.Location = new System.Drawing.Point(160, 248);
            this.picBassSlider.Name = "picBassSlider";
            this.picBassSlider.Size = new System.Drawing.Size(70, 20);
            this.picBassSlider.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBassSlider.TabIndex = 88;
            this.picBassSlider.TabStop = false;
            this.picBassSlider.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.picBassSlider_MouseDoubleClick);
            this.picBassSlider.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picBassSlider_MouseDown);
            this.picBassSlider.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picBassSlider_MouseMove);
            this.picBassSlider.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picBassSlider_MouseUp);
            // 
            // picBassBackground
            // 
            this.picBassBackground.BackColor = System.Drawing.Color.Silver;
            this.picBassBackground.Image = ((System.Drawing.Image)(resources.GetObject("picBassBackground.Image")));
            this.picBassBackground.Location = new System.Drawing.Point(175, 198);
            this.picBassBackground.Name = "picBassBackground";
            this.picBassBackground.Size = new System.Drawing.Size(40, 120);
            this.picBassBackground.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBassBackground.TabIndex = 87;
            this.picBassBackground.TabStop = false;
            // 
            // picDrumsSlider
            // 
            this.picDrumsSlider.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picDrumsSlider.Enabled = false;
            this.picDrumsSlider.Image = ((System.Drawing.Image)(resources.GetObject("picDrumsSlider.Image")));
            this.picDrumsSlider.Location = new System.Drawing.Point(235, 248);
            this.picDrumsSlider.Name = "picDrumsSlider";
            this.picDrumsSlider.Size = new System.Drawing.Size(70, 20);
            this.picDrumsSlider.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picDrumsSlider.TabIndex = 90;
            this.picDrumsSlider.TabStop = false;
            this.picDrumsSlider.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.picDrumsSlider_MouseDoubleClick);
            this.picDrumsSlider.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picDrumsSlider_MouseDown);
            this.picDrumsSlider.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picDrumsSlider_MouseMove);
            this.picDrumsSlider.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picDrumsSlider_MouseUp);
            // 
            // picDrumsBackground
            // 
            this.picDrumsBackground.BackColor = System.Drawing.Color.Silver;
            this.picDrumsBackground.Image = ((System.Drawing.Image)(resources.GetObject("picDrumsBackground.Image")));
            this.picDrumsBackground.Location = new System.Drawing.Point(249, 198);
            this.picDrumsBackground.Name = "picDrumsBackground";
            this.picDrumsBackground.Size = new System.Drawing.Size(40, 120);
            this.picDrumsBackground.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picDrumsBackground.TabIndex = 89;
            this.picDrumsBackground.TabStop = false;
            // 
            // picKeysSlider
            // 
            this.picKeysSlider.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picKeysSlider.Enabled = false;
            this.picKeysSlider.Image = ((System.Drawing.Image)(resources.GetObject("picKeysSlider.Image")));
            this.picKeysSlider.Location = new System.Drawing.Point(385, 248);
            this.picKeysSlider.Name = "picKeysSlider";
            this.picKeysSlider.Size = new System.Drawing.Size(70, 20);
            this.picKeysSlider.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picKeysSlider.TabIndex = 94;
            this.picKeysSlider.TabStop = false;
            this.picKeysSlider.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.picKeysSlider_MouseDoubleClick);
            this.picKeysSlider.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picKeysSlider_MouseDown);
            this.picKeysSlider.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picKeysSlider_MouseMove);
            this.picKeysSlider.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picKeysSlider_MouseUp);
            // 
            // picKeysBackground
            // 
            this.picKeysBackground.BackColor = System.Drawing.Color.Silver;
            this.picKeysBackground.Image = ((System.Drawing.Image)(resources.GetObject("picKeysBackground.Image")));
            this.picKeysBackground.Location = new System.Drawing.Point(399, 198);
            this.picKeysBackground.Name = "picKeysBackground";
            this.picKeysBackground.Size = new System.Drawing.Size(40, 120);
            this.picKeysBackground.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picKeysBackground.TabIndex = 93;
            this.picKeysBackground.TabStop = false;
            // 
            // picGuitarSlider
            // 
            this.picGuitarSlider.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picGuitarSlider.Enabled = false;
            this.picGuitarSlider.Image = ((System.Drawing.Image)(resources.GetObject("picGuitarSlider.Image")));
            this.picGuitarSlider.Location = new System.Drawing.Point(310, 248);
            this.picGuitarSlider.Name = "picGuitarSlider";
            this.picGuitarSlider.Size = new System.Drawing.Size(70, 20);
            this.picGuitarSlider.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picGuitarSlider.TabIndex = 92;
            this.picGuitarSlider.TabStop = false;
            this.picGuitarSlider.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.picGuitarSlider_MouseDoubleClick);
            this.picGuitarSlider.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picGuitarSlider_MouseDown);
            this.picGuitarSlider.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picGuitarSlider_MouseMove);
            this.picGuitarSlider.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picGuitarSlider_MouseUp);
            // 
            // picGuitarBackground
            // 
            this.picGuitarBackground.BackColor = System.Drawing.Color.Silver;
            this.picGuitarBackground.Image = ((System.Drawing.Image)(resources.GetObject("picGuitarBackground.Image")));
            this.picGuitarBackground.Location = new System.Drawing.Point(324, 198);
            this.picGuitarBackground.Name = "picGuitarBackground";
            this.picGuitarBackground.Size = new System.Drawing.Size(40, 120);
            this.picGuitarBackground.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picGuitarBackground.TabIndex = 91;
            this.picGuitarBackground.TabStop = false;
            // 
            // picCrowdSlider
            // 
            this.picCrowdSlider.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picCrowdSlider.Enabled = false;
            this.picCrowdSlider.Image = ((System.Drawing.Image)(resources.GetObject("picCrowdSlider.Image")));
            this.picCrowdSlider.Location = new System.Drawing.Point(610, 248);
            this.picCrowdSlider.Name = "picCrowdSlider";
            this.picCrowdSlider.Size = new System.Drawing.Size(70, 20);
            this.picCrowdSlider.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picCrowdSlider.TabIndex = 100;
            this.picCrowdSlider.TabStop = false;
            this.picCrowdSlider.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.picCrowdSlider_MouseDoubleClick);
            this.picCrowdSlider.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picCrowdSlider_MouseDown);
            this.picCrowdSlider.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picCrowdSlider_MouseMove);
            this.picCrowdSlider.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picCrowdSlider_MouseUp);
            // 
            // picCrowdBackground
            // 
            this.picCrowdBackground.BackColor = System.Drawing.Color.Silver;
            this.picCrowdBackground.Image = ((System.Drawing.Image)(resources.GetObject("picCrowdBackground.Image")));
            this.picCrowdBackground.Location = new System.Drawing.Point(624, 198);
            this.picCrowdBackground.Name = "picCrowdBackground";
            this.picCrowdBackground.Size = new System.Drawing.Size(40, 120);
            this.picCrowdBackground.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picCrowdBackground.TabIndex = 99;
            this.picCrowdBackground.TabStop = false;
            // 
            // picBackingSlider
            // 
            this.picBackingSlider.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picBackingSlider.Enabled = false;
            this.picBackingSlider.Image = ((System.Drawing.Image)(resources.GetObject("picBackingSlider.Image")));
            this.picBackingSlider.Location = new System.Drawing.Point(535, 248);
            this.picBackingSlider.Name = "picBackingSlider";
            this.picBackingSlider.Size = new System.Drawing.Size(70, 20);
            this.picBackingSlider.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBackingSlider.TabIndex = 98;
            this.picBackingSlider.TabStop = false;
            this.picBackingSlider.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.picBackingSlider_MouseDoubleClick);
            this.picBackingSlider.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picBackingSlider_MouseDown);
            this.picBackingSlider.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picBackingSlider_MouseMove);
            this.picBackingSlider.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picBackingSlider_MouseUp);
            // 
            // picBackingBackground
            // 
            this.picBackingBackground.BackColor = System.Drawing.Color.Silver;
            this.picBackingBackground.Image = ((System.Drawing.Image)(resources.GetObject("picBackingBackground.Image")));
            this.picBackingBackground.Location = new System.Drawing.Point(549, 198);
            this.picBackingBackground.Name = "picBackingBackground";
            this.picBackingBackground.Size = new System.Drawing.Size(40, 120);
            this.picBackingBackground.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBackingBackground.TabIndex = 97;
            this.picBackingBackground.TabStop = false;
            // 
            // picVocalsSlider
            // 
            this.picVocalsSlider.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picVocalsSlider.Enabled = false;
            this.picVocalsSlider.Image = ((System.Drawing.Image)(resources.GetObject("picVocalsSlider.Image")));
            this.picVocalsSlider.Location = new System.Drawing.Point(460, 248);
            this.picVocalsSlider.Name = "picVocalsSlider";
            this.picVocalsSlider.Size = new System.Drawing.Size(70, 20);
            this.picVocalsSlider.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picVocalsSlider.TabIndex = 96;
            this.picVocalsSlider.TabStop = false;
            this.picVocalsSlider.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.picVocalsSlider_MouseDoubleClick);
            this.picVocalsSlider.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picVocalsSlider_MouseDown);
            this.picVocalsSlider.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picVocalsSlider_MouseMove);
            this.picVocalsSlider.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picVocalsSlider_MouseUp);
            // 
            // picVocalsBackground
            // 
            this.picVocalsBackground.BackColor = System.Drawing.Color.Silver;
            this.picVocalsBackground.Image = ((System.Drawing.Image)(resources.GetObject("picVocalsBackground.Image")));
            this.picVocalsBackground.Location = new System.Drawing.Point(474, 198);
            this.picVocalsBackground.Name = "picVocalsBackground";
            this.picVocalsBackground.Size = new System.Drawing.Size(40, 120);
            this.picVocalsBackground.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picVocalsBackground.TabIndex = 95;
            this.picVocalsBackground.TabStop = false;
            // 
            // lblBass
            // 
            this.lblBass.BackColor = System.Drawing.Color.Transparent;
            this.lblBass.ForeColor = System.Drawing.Color.White;
            this.lblBass.Location = new System.Drawing.Point(169, 175);
            this.lblBass.Name = "lblBass";
            this.lblBass.Size = new System.Drawing.Size(55, 20);
            this.lblBass.TabIndex = 101;
            this.lblBass.Text = "0.0 dB";
            this.lblBass.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDrums
            // 
            this.lblDrums.BackColor = System.Drawing.Color.Transparent;
            this.lblDrums.ForeColor = System.Drawing.Color.White;
            this.lblDrums.Location = new System.Drawing.Point(243, 175);
            this.lblDrums.Name = "lblDrums";
            this.lblDrums.Size = new System.Drawing.Size(55, 20);
            this.lblDrums.TabIndex = 102;
            this.lblDrums.Text = "0.0 dB";
            this.lblDrums.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblGuitar
            // 
            this.lblGuitar.BackColor = System.Drawing.Color.Transparent;
            this.lblGuitar.ForeColor = System.Drawing.Color.White;
            this.lblGuitar.Location = new System.Drawing.Point(318, 175);
            this.lblGuitar.Name = "lblGuitar";
            this.lblGuitar.Size = new System.Drawing.Size(55, 20);
            this.lblGuitar.TabIndex = 103;
            this.lblGuitar.Text = "0.0 dB";
            this.lblGuitar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblKeys
            // 
            this.lblKeys.BackColor = System.Drawing.Color.Transparent;
            this.lblKeys.ForeColor = System.Drawing.Color.White;
            this.lblKeys.Location = new System.Drawing.Point(393, 175);
            this.lblKeys.Name = "lblKeys";
            this.lblKeys.Size = new System.Drawing.Size(55, 20);
            this.lblKeys.TabIndex = 104;
            this.lblKeys.Text = "0.0 dB";
            this.lblKeys.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblVocals
            // 
            this.lblVocals.BackColor = System.Drawing.Color.Transparent;
            this.lblVocals.ForeColor = System.Drawing.Color.White;
            this.lblVocals.Location = new System.Drawing.Point(468, 175);
            this.lblVocals.Name = "lblVocals";
            this.lblVocals.Size = new System.Drawing.Size(55, 20);
            this.lblVocals.TabIndex = 105;
            this.lblVocals.Text = "0.0 dB";
            this.lblVocals.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblBacking
            // 
            this.lblBacking.BackColor = System.Drawing.Color.Transparent;
            this.lblBacking.ForeColor = System.Drawing.Color.White;
            this.lblBacking.Location = new System.Drawing.Point(542, 175);
            this.lblBacking.Name = "lblBacking";
            this.lblBacking.Size = new System.Drawing.Size(55, 20);
            this.lblBacking.TabIndex = 106;
            this.lblBacking.Text = "0.0 dB";
            this.lblBacking.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCrowd
            // 
            this.lblCrowd.BackColor = System.Drawing.Color.Transparent;
            this.lblCrowd.ForeColor = System.Drawing.Color.White;
            this.lblCrowd.Location = new System.Drawing.Point(618, 175);
            this.lblCrowd.Name = "lblCrowd";
            this.lblCrowd.Size = new System.Drawing.Size(55, 20);
            this.lblCrowd.TabIndex = 107;
            this.lblCrowd.Text = "0.0 dB";
            this.lblCrowd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picSpectrum
            // 
            this.picSpectrum.BackColor = System.Drawing.Color.Black;
            this.picSpectrum.Location = new System.Drawing.Point(12, 13);
            this.picSpectrum.Name = "picSpectrum";
            this.picSpectrum.Size = new System.Drawing.Size(676, 135);
            this.picSpectrum.TabIndex = 108;
            this.picSpectrum.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 33;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // picMasterSlider
            // 
            this.picMasterSlider.BackColor = System.Drawing.Color.Transparent;
            this.picMasterSlider.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picMasterSlider.Image = global::cPlayer.Properties.Resources.vol_slider;
            this.picMasterSlider.Location = new System.Drawing.Point(22, 235);
            this.picMasterSlider.Name = "picMasterSlider";
            this.picMasterSlider.Size = new System.Drawing.Size(120, 28);
            this.picMasterSlider.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picMasterSlider.TabIndex = 110;
            this.picMasterSlider.TabStop = false;
            this.picMasterSlider.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.picMasterSlider_MouseDoubleClick);
            this.picMasterSlider.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picMasterSlider_MouseDown);
            this.picMasterSlider.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picMasterSlider_MouseMove);
            this.picMasterSlider.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picMasterSlider_MouseUp);
            // 
            // picMasterBackground
            // 
            this.picMasterBackground.BackColor = System.Drawing.Color.White;
            this.picMasterBackground.Image = global::cPlayer.Properties.Resources.vol_bg;
            this.picMasterBackground.Location = new System.Drawing.Point(43, 177);
            this.picMasterBackground.Name = "picMasterBackground";
            this.picMasterBackground.Size = new System.Drawing.Size(76, 155);
            this.picMasterBackground.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picMasterBackground.TabIndex = 109;
            this.picMasterBackground.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(59, 338);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 16);
            this.label1.TabIndex = 111;
            this.label1.Text = "Master";
            // 
            // lblMaster
            // 
            this.lblMaster.BackColor = System.Drawing.Color.Transparent;
            this.lblMaster.ForeColor = System.Drawing.Color.White;
            this.lblMaster.Location = new System.Drawing.Point(59, 154);
            this.lblMaster.Name = "lblMaster";
            this.lblMaster.Size = new System.Drawing.Size(55, 20);
            this.lblMaster.TabIndex = 112;
            this.lblMaster.Text = "0.0 dB";
            this.lblMaster.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AudioMixer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(700, 370);
            this.Controls.Add(this.lblMaster);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.picMasterSlider);
            this.Controls.Add(this.picMasterBackground);
            this.Controls.Add(this.picSpectrum);
            this.Controls.Add(this.lblCrowd);
            this.Controls.Add(this.lblBacking);
            this.Controls.Add(this.lblVocals);
            this.Controls.Add(this.lblKeys);
            this.Controls.Add(this.lblGuitar);
            this.Controls.Add(this.lblDrums);
            this.Controls.Add(this.lblBass);
            this.Controls.Add(this.picCrowdSlider);
            this.Controls.Add(this.picCrowdBackground);
            this.Controls.Add(this.picBackingSlider);
            this.Controls.Add(this.picBackingBackground);
            this.Controls.Add(this.picVocalsSlider);
            this.Controls.Add(this.picVocalsBackground);
            this.Controls.Add(this.picKeysSlider);
            this.Controls.Add(this.picKeysBackground);
            this.Controls.Add(this.picGuitarSlider);
            this.Controls.Add(this.picGuitarBackground);
            this.Controls.Add(this.picDrumsSlider);
            this.Controls.Add(this.picDrumsBackground);
            this.Controls.Add(this.picBassSlider);
            this.Controls.Add(this.picBassBackground);
            this.Controls.Add(this.chkCrowd);
            this.Controls.Add(this.chkBacking);
            this.Controls.Add(this.chkKeys);
            this.Controls.Add(this.chkVocals);
            this.Controls.Add(this.chkGuitar);
            this.Controls.Add(this.chkBass);
            this.Controls.Add(this.chkDrums);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AudioMixer";
            this.Opacity = 0.9D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Audio Mixer";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AudioMixer_FormClosing);
            this.Shown += new System.EventHandler(this.AudioMixer_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.picBassSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBassBackground)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDrumsSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDrumsBackground)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picKeysSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picKeysBackground)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGuitarSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGuitarBackground)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCrowdSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCrowdBackground)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBackingSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBackingBackground)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picVocalsSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picVocalsBackground)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSpectrum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMasterSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMasterBackground)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox chkCrowd;
        private System.Windows.Forms.CheckBox chkBacking;
        private System.Windows.Forms.CheckBox chkKeys;
        private System.Windows.Forms.CheckBox chkVocals;
        private System.Windows.Forms.CheckBox chkGuitar;
        private System.Windows.Forms.CheckBox chkBass;
        private System.Windows.Forms.CheckBox chkDrums;
        private System.Windows.Forms.PictureBox picBassSlider;
        private System.Windows.Forms.PictureBox picBassBackground;
        private System.Windows.Forms.PictureBox picDrumsSlider;
        private System.Windows.Forms.PictureBox picDrumsBackground;
        private System.Windows.Forms.PictureBox picKeysSlider;
        private System.Windows.Forms.PictureBox picKeysBackground;
        private System.Windows.Forms.PictureBox picGuitarSlider;
        private System.Windows.Forms.PictureBox picGuitarBackground;
        private System.Windows.Forms.PictureBox picCrowdSlider;
        private System.Windows.Forms.PictureBox picCrowdBackground;
        private System.Windows.Forms.PictureBox picBackingSlider;
        private System.Windows.Forms.PictureBox picBackingBackground;
        private System.Windows.Forms.PictureBox picVocalsSlider;
        private System.Windows.Forms.PictureBox picVocalsBackground;
        private System.Windows.Forms.Label lblBass;
        private System.Windows.Forms.Label lblDrums;
        private System.Windows.Forms.Label lblGuitar;
        private System.Windows.Forms.Label lblKeys;
        private System.Windows.Forms.Label lblVocals;
        private System.Windows.Forms.Label lblBacking;
        private System.Windows.Forms.Label lblCrowd;
        private System.Windows.Forms.PictureBox picSpectrum;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox picMasterSlider;
        private System.Windows.Forms.PictureBox picMasterBackground;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblMaster;
    }
}
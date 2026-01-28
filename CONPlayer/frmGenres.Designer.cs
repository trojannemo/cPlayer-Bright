namespace cPlayer
{
    partial class frmFilters
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
            this.lstGenres = new System.Windows.Forms.ListView();
            this.btnClose = new System.Windows.Forms.Button();
            this.lstInstruments = new System.Windows.Forms.ListView();
            this.lstLanguages = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // lstGenres
            // 
            this.lstGenres.BackColor = System.Drawing.Color.AliceBlue;
            this.lstGenres.CheckBoxes = true;
            this.lstGenres.FullRowSelect = true;
            this.lstGenres.HideSelection = false;
            this.lstGenres.Location = new System.Drawing.Point(12, 12);
            this.lstGenres.MultiSelect = false;
            this.lstGenres.Name = "lstGenres";
            this.lstGenres.Size = new System.Drawing.Size(155, 446);
            this.lstGenres.TabIndex = 0;
            this.lstGenres.UseCompatibleStateImageBehavior = false;
            this.lstGenres.View = System.Windows.Forms.View.List;
            this.lstGenres.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lstGenres_ItemChecked);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Location = new System.Drawing.Point(101, 464);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(155, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Apply Filters";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lstInstruments
            // 
            this.lstInstruments.BackColor = System.Drawing.Color.AliceBlue;
            this.lstInstruments.CheckBoxes = true;
            this.lstInstruments.FullRowSelect = true;
            this.lstInstruments.HideSelection = false;
            this.lstInstruments.Location = new System.Drawing.Point(173, 12);
            this.lstInstruments.MultiSelect = false;
            this.lstInstruments.Name = "lstInstruments";
            this.lstInstruments.Size = new System.Drawing.Size(155, 220);
            this.lstInstruments.TabIndex = 3;
            this.lstInstruments.UseCompatibleStateImageBehavior = false;
            this.lstInstruments.View = System.Windows.Forms.View.List;
            this.lstInstruments.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lstInstruments_ItemChecked);
            // 
            // lstLanguages
            // 
            this.lstLanguages.BackColor = System.Drawing.Color.AliceBlue;
            this.lstLanguages.CheckBoxes = true;
            this.lstLanguages.FullRowSelect = true;
            this.lstLanguages.HideSelection = false;
            this.lstLanguages.Location = new System.Drawing.Point(173, 238);
            this.lstLanguages.MultiSelect = false;
            this.lstLanguages.Name = "lstLanguages";
            this.lstLanguages.Size = new System.Drawing.Size(155, 220);
            this.lstLanguages.TabIndex = 4;
            this.lstLanguages.UseCompatibleStateImageBehavior = false;
            this.lstLanguages.View = System.Windows.Forms.View.List;
            this.lstLanguages.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lstLanguages_ItemChecked);
            // 
            // frmGenres
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkGray;
            this.ClientSize = new System.Drawing.Size(341, 499);
            this.Controls.Add(this.lstLanguages);
            this.Controls.Add(this.lstInstruments);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lstGenres);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmGenres";
            this.Opacity = 0.95D;
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Select a genre";
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.frmGenres_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lstGenres;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ListView lstInstruments;
        private System.Windows.Forms.ListView lstLanguages;
    }
}
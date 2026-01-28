using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;

namespace cPlayer
{
    public partial class SongDetailsForm : Form
    {
        private string FilePath;

        public SongDetailsForm(string details, string location)
        {
            InitializeComponent();
            rtb.Text = details;
            FilePath = location;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(rtb.Text);
            MessageBox.Show("Song details copied to clipboard", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnGoTo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FilePath))
                return;

            if (!File.Exists(FilePath))
            {
                MessageBox.Show("The song file could not be found.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Process.Start("explorer.exe", $"/select,\"{FilePath}\"");
        }
    }
}

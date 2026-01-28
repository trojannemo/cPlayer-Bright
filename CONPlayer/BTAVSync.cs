using System.Windows.Forms;

namespace cPlayer
{
    public partial class BTAVSync : Form
    {
        private readonly frmMain mainForm;

        public BTAVSync(frmMain parent, int sync, bool enable)
        {
            InitializeComponent();
            mainForm = parent;
            UpdateSync(sync, true);
            chkEnable.Checked = enable;
        }

        private void UpdateSync(int sync, bool loading = false)
        {
            Text = "Bluetooth AV Offset: " + sync + "ms";
            mainForm.BTAVOffsetSync = sync;
            mainForm.enableBTAVOffsetSync = chkEnable.Checked;
            if (!loading) return;
            trackbarAV.Value = sync;
        }

        private void trackbarAV_ValueChanged(object sender, System.EventArgs e)
        {
            UpdateSync(trackbarAV.Value);
        }

        private void BTAVSync_FormClosing(object sender, FormClosingEventArgs e)
        {
            UpdateSync(trackbarAV.Value);
        }

        private void btnRight_Click(object sender, System.EventArgs e)
        {
            if (trackbarAV.Value >= trackbarAV.Maximum - trackbarAV.LargeChange)
            {
                trackbarAV.Value = trackbarAV.Maximum;
                return;
            }
            trackbarAV.Value += trackbarAV.LargeChange;
        }

        private void btnLeft_Click(object sender, System.EventArgs e)
        {
            if (trackbarAV.Value <= trackbarAV.Minimum + trackbarAV.LargeChange)
            {
                trackbarAV.Value = trackbarAV.Minimum;
                return;
            }
            trackbarAV.Value -= trackbarAV.LargeChange;
        }

        private void chkEnable_CheckedChanged(object sender, System.EventArgs e)
        {
            btnLeft.Enabled = chkEnable.Checked;
            btnRight.Enabled = chkEnable.Checked;
            trackbarAV.Enabled = chkEnable.Checked;
            mainForm.enableBTAVOffsetSync = chkEnable.Checked;
        }
    }
}

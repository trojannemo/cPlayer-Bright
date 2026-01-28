using System;
using System.Windows.Forms;

namespace cPlayer
{
    public partial class frmHover : Form
    {
        private readonly frmMain mainForm;

        public frmHover(frmMain parent)
        {
            InitializeComponent();
            mainForm = parent;
        }               

        private void timer1_Tick(object sender, EventArgs e)
        {
            Visible = mainForm.MonitorApplicationFocus();
        }

        private void picGear_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            mainForm.OpenSettingsForm();
        }

        private void picGear_MouseLeave(object sender, EventArgs e)
        {
            Opacity = 0.5;
        }

        private void picGear_MouseEnter(object sender, EventArgs e)
        {
            Opacity = 1.0;
        }
    }
}

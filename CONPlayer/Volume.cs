using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace cPlayer
{
    public partial class Volume : Form
    {
        private readonly frmMain xParent;
        private readonly Point StartLocation;
        private double CurrentVolume;
        private const double MinVolume = 50;
        private int yOffset;

        public Volume(frmMain parent, Point start)
        {
            InitializeComponent();
            xParent = parent;
            StartLocation = start;
            CurrentVolume = xParent.VolumeLevel;
            yOffset = picBackground.Top;
        }

        private void Volume_Shown(object sender, EventArgs e)
        {
            Location = new Point(StartLocation.X - (Width / 2), StartLocation.Y - (Height / 2));
            picBackground.Left = (Width - picBackground.Width) / 2;
            picSlider.Left= (Width - picSlider.Width) / 2;
                        
            var percent = CurrentVolume / MinVolume;
            picSlider.Top = (int)((picBackground.Height - picSlider.Height) * (1.0 - (CurrentVolume / MinVolume))) + yOffset;
            if (picSlider.Top < yOffset)
            {
                picSlider.Top = yOffset;
            }
            if (picSlider.Top > yOffset + picBackground.Height - picSlider.Height)
            {
                picSlider.Top = yOffset + picBackground.Height - picSlider.Height;
            }
            picSlider.Top = (int)((picBackground.Height - picSlider.Height) * (1.0 - percent)) + yOffset;
            lblVolume.Text = "Vol: " + (int)(CurrentVolume * 2);
        }

        private void picBackground_Click(object sender, EventArgs e)
        {
            SaveVolume();
        }

        private void Volume_Click(object sender, EventArgs e)
        {
            SaveVolume();
        }

        private void Volume_KeyUp(object sender, KeyEventArgs e)
        {
            SaveVolume();
        }
        
        private void picSlider_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            picSlider.Top = PointToClient(MousePosition).Y - (picSlider.Height/2);
            if (picSlider.Top < picBackground.Top)
            {
                picSlider.Top = picBackground.Top;
            }
            var bottom = picBackground.Top + picBackground.Height - picSlider.Height;
            if (picSlider.Top > bottom)
            {
                picSlider.Top = bottom;
            }

            CurrentVolume = Math.Round(-1 * (-MinVolume + (MinVolume * (picSlider.Top - yOffset) / (picBackground.Height - picSlider.Height))), 1);
            xParent.UpdateVolume(MinVolume-CurrentVolume);
            lblVolume.Text = "Vol: " + (int)(CurrentVolume*2);
        }

        private void picSlider_MouseUp(object sender, MouseEventArgs e)
        {
            picSlider.Cursor = Cursors.Hand;
            if (picSlider.Top < picBackground.Top)
            {
                picSlider.Top = picBackground.Top;
            }
            var bottom = picBackground.Top + picBackground.Height - picSlider.Height;
            if (picSlider.Top > bottom)
            {
                picSlider.Top = bottom;
            }
            SaveVolume();
        }

        private void picSlider_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            picSlider.Cursor = Cursors.NoMoveVert;

            if (picSlider.Top < picBackground.Top)
            {
                picSlider.Top = picBackground.Top;
            }
            var bottom = picBackground.Top + picBackground.Height - picSlider.Height;
            if (picSlider.Top > bottom)
            {
                picSlider.Top = bottom;
            }
        }

        private void Volume_Deactivate(object sender, EventArgs e)
        {
            SaveVolume();
        }

        private void SaveVolume()
        {
            xParent.VolumeLevel = CurrentVolume;
            Dispose();
        }
    }
}

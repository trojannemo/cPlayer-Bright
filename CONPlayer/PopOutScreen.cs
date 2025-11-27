using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System;

namespace cPlayer
{
    public partial class PopOutScreen : Form
    {
        private bool isFullScreen = false;
        private Bitmap visuals;
        private Rectangle _restoreBounds;
        private FormBorderStyle _restoreBorder;
        private bool _restoreTopMost;
        private FormWindowState _restoreState;
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        public PopOutScreen()
        {
            InitializeComponent();         
        }

        public void changeBackgroundImage(Image image, bool zoom = false)
        {
            picVisuals.Image = image;
            if (zoom)
            {
                picVisuals.SizeMode = PictureBoxSizeMode.Zoom;
            }
            else
            {
                picVisuals.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        public Size RenderSize()
        {
            return picVisuals.ClientSize;
        }

        public Rectangle PictureBounds()
        {
            return picVisuals.Bounds;
        }

        public void UpdateVisuals(Bitmap bmp)
        {
            visuals = bmp;           
            picVisuals.Invalidate();
        }

        private void picVisuals_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var scr = Screen.FromControl(this);      // or Screen.FromPoint(Cursor.Position)
            var b = scr.Bounds;                      // TRUE full screen area (includes taskbar)

            if (!isFullScreen)
            {
                // save current windowed state
                _restoreBounds = this.Bounds;
                _restoreBorder = this.FormBorderStyle;
                _restoreTopMost = this.TopMost;
                _restoreState = this.WindowState;

                // switch to borderless fullscreen on this monitor
                this.WindowState = FormWindowState.Normal;  // important: avoid "maximized to working area"
                this.FormBorderStyle = FormBorderStyle.None;
                this.StartPosition = FormStartPosition.Manual;
                this.TopMost = true;                        // keep above taskbar/other windows
                this.SetDesktopBounds(b.Left, b.Top, b.Width, b.Height);

                isFullScreen = true;
            }
            else
            {
                // restore previous windowed state
                this.TopMost = _restoreTopMost;
                this.FormBorderStyle = _restoreBorder;
                this.StartPosition = FormStartPosition.Manual;

                // restore geometry/state (order matters)
                this.WindowState = FormWindowState.Normal;
                this.Bounds = _restoreBounds;
                this.WindowState = _restoreState;

                isFullScreen = false;
            }
        }

        private void picVisuals_Paint(object sender, PaintEventArgs e)
        {
            if (visuals == null) return;
            if (picVisuals.ClientSize == visuals.Size)
            {
                e.Graphics.DrawImageUnscaled(visuals, 0, 0);
            }
            else
            {
                e.Graphics.DrawImage(visuals, new Rectangle(Point.Empty, picVisuals.ClientSize));
            }
        }

        private void picVisuals_MouseDown(object sender, MouseEventArgs e)
        {
            if (isFullScreen) return;
            if (e.Button != MouseButtons.Left) return;

            // If this is part of a double-click sequence, don't start a drag.
            if (e.Clicks > 1) return;

            ReleaseCapture();
            SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);            
        }        
    }
}

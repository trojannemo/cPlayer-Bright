using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace cPlayer
{
    public partial class frmSplash : Form
    {
        private readonly Timer _timer = new Timer();
        private int _phase = 0;              // 0 = fade in, 1 = hold, 2 = fade out
        private int _holdTicks = 0;

        public frmSplash()
        {
            InitializeComponent();

            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterScreen;
            ShowInTaskbar = false;
            TopMost = true;
            DoubleBuffered = true;

            // Make it feel like a splash
            Cursor = Cursors.AppStarting;

            // Use splash image
            BackgroundImage = Properties.Resources.splashscreen;
            BackgroundImageLayout = ImageLayout.Stretch;

            // Size to a reasonable splash size (or match image aspect)
            ClientSize = new Size(922, 614);
            ApplyRoundedRegion(25);

            Opacity = 0.0;

            lblVersion.Text = "cPlayer " + GetAppVersion();

            _timer.Interval = 15; // ~60fps-ish
            _timer.Tick += Timer_Tick;
        }

        private void ApplyRoundedRegion(int radius)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            int d = radius * 2;

            path.AddArc(0, 0, d, d, 180, 90);
            path.AddArc(Width - d, 0, d, d, 270, 90);
            path.AddArc(Width - d, Height - d, d, d, 0, 90);
            path.AddArc(0, Height - d, d, d, 90, 90);
            path.CloseFigure();

            Region = new Region(path);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            _timer.Start();
        }
        private static string GetAppVersion()
        {
            var vers = Assembly.GetExecutingAssembly().GetName().Version;
            return "v" + String.Format("{0}.{1}.{2}", vers.Major, vers.Minor, vers.Build);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            const double fadeInStep = 0.05;   // speed up/down as desired
            const double fadeOutStep = 0.05;

            if (_phase == 0) // fade in
            {
                Opacity = Math.Min(1.0, Opacity + fadeInStep);
                if (Opacity >= 1.0)
                {
                    _phase = 1;
                    _holdTicks = 0;
                }
            }
            else if (_phase == 1) // hold
            {
                _holdTicks++;
                if (_holdTicks >= 135)
                {
                    _phase = 2;
                }
            }
            else // fade out
            {
                Opacity = Math.Max(0.0, Opacity - fadeOutStep);
                if (Opacity <= 0.0)
                {
                    _timer.Stop();
                    Close();
                }
            }
        }
    }
}
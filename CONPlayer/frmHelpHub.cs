using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.IO;

namespace cPlayer
{
    public partial class frmHelpHub : Form
    {
        private readonly Color activeColor = Color.LimeGreen;
        private readonly Color inactiveColor = Color.SteelBlue;
        private readonly string filePath;

        public frmHelpHub()
        {
            InitializeComponent();
            filePath = Application.StartupPath + "\\bin\\help\\";
        }

        private void btnWelcome_Click(object sender, EventArgs e)
        {
            ClickWelcome();
        }

        public void ClickWelcome()
        {
            ResetAllButtons();
            btnWelcome.ForeColor = activeColor;
            LoadFileToRTB("welcome");           
            rtbRightPane.Rtf = rtbRightPane.Rtf.Replace("(app version)", GetAppVersion());
        }

        private static string GetAppVersion()
        {
            var vers = Assembly.GetExecutingAssembly().GetName().Version;
            return "v" + String.Format("{0}.{1}.{2}", vers.Major, vers.Minor, vers.Build);
        }

        private void ResetAllButtons()
        {
            btnWelcome.ForeColor = inactiveColor;
            btnQuickStart.ForeColor = inactiveColor;
            btnPlayback.ForeColor = inactiveColor;
            btnMultiScreen.ForeColor = inactiveColor;
            btnKaraoke.ForeColor = inactiveColor;
            btnVisuals.ForeColor = inactiveColor;
            btnTroubleshooting.ForeColor = inactiveColor;
        }

        private void btnQuickStart_Click(object sender, EventArgs e)
        {
            ResetAllButtons();
            btnQuickStart.ForeColor = activeColor;
            ClickQuickStart();
        }
        
        public void ClickQuickStart()
        {
            LoadFileToRTB("quickstart");
        }

        private void LoadFileToRTB(string file)
        {
            var path = filePath + file;
            if (!File.Exists(path)) return;
            try
            {
                rtbRightPane.LoadFile(path, RichTextBoxStreamType.RichText);
            }
            catch { }

            // Move caret to start
            rtbRightPane.SelectionStart = 0;
            rtbRightPane.SelectionLength = 0;

            // Scroll to caret
            rtbRightPane.ScrollToCaret();
        }

        private void btnPlayback_Click(object sender, EventArgs e)
        {
            ResetAllButtons();
            btnPlayback.ForeColor = activeColor;
            ClickPlayback();
        }

        public void ClickPlayback()
        {
            LoadFileToRTB("playback");
        }

        private void btnMultiScreen_Click(object sender, EventArgs e)
        {
            ResetAllButtons();
            btnMultiScreen.ForeColor = activeColor;
            ClickMultiScreen();
        }

        public void ClickMultiScreen()
        {
            LoadFileToRTB("screen");
        }

        private void btnKaraoke_Click(object sender, EventArgs e)
        {
            ResetAllButtons();
            btnKaraoke.ForeColor = activeColor;
            ClickKaraoke();
        }

        public void ClickKaraoke()
        {
            LoadFileToRTB("karaoke");            
        }

        private void btnVisuals_Click(object sender, EventArgs e)
        {
            ResetAllButtons();
            btnVisuals.ForeColor = activeColor;
            ClickVisuals();
        }

        public void ClickVisuals()
        {
            LoadFileToRTB("visuals");
        }
        
        private void btnTroubleshooting_Click(object sender, EventArgs e)
        {
            ResetAllButtons();
            btnTroubleshooting.ForeColor = activeColor;
            ClickTroubleshooting();
        }

        public void ClickTroubleshooting()
        {
            LoadFileToRTB("trouble");
        }

        private void btnWebsite_Click(object sender, EventArgs e)
        {
            Process.Start("https://nemosnautilus.com/cplayer/");
        }

        private void btnDiscord_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.com/invite/ydGmRyKGVW");
        }
    }
}

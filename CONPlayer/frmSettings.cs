using System;
using System.Drawing;
using System.Windows.Forms;

namespace cPlayer
{
    public partial class frmSettings : Form
    {
        private readonly frmMain mainForm;
        private readonly Color defaultColor = Color.LightCyan;
        private readonly Color enabledTopColor = Color.PaleGreen;
        private readonly Color enabledSubColor = Color.PaleGreen; //Color.Thistle;
        private const int expandedWidth = 450;
        private const int contractedWidth = 80;
        private const int expandedHeight = 492;
        private const int contractedHeight = 246;
        private const int shortHeight = 176;
        private bool isKaraokeMode;

        public frmSettings(frmMain parent)
        {
            InitializeComponent();
            mainForm = parent;
            HideTabHeaders();
            DisableAllTopButtons();

            btnBGColor.BackColor = mainForm.KaraokeModeBackgroundColor;
            btnHarm1Color.BackColor = mainForm.KaraokeModeHarm1Text;
            btnHarm1HighlightColor.BackColor = mainForm.KaraokeModeHarm1Highlight;
            btnHarm2Color.BackColor = mainForm.KaraokeModeHarm2Text;
            btnHarm2HighlightColor.BackColor = mainForm.KaraokeModeHarm2Highlight;
            btnHarm3Color.BackColor = mainForm.KaraokeModeHarm3Text;
            btnHarm3HighlightColor.BackColor = mainForm.KaraokeModeHarm3Highlight;

            if (mainForm.GetDisplayAlbumArtIsChecked())
            {
                EnableAlbumArt();
                return;
            }
            if (mainForm.GetDisplayAudioSpectrumIsChecked())
            {
                EnableVisualizer();
                return;
            }
            if (mainForm.GetClassicKaraokeModeIsChecked())
            {
                EnableKaraoke();
                EnableModernKaraoke();
                radioSolidColor.Checked = mainForm.GetSolidColorBackgroundIsChecked();
                radioStatic.Checked = mainForm.GetEnableBackgroundImageIsChecked();
                radioAnimated.Checked = mainForm.GetAnimatedBackground2IsChecked();
                return;
            }
            if (mainForm.GetcPlayerStyleIsChecked())
            {
                EnableKaraoke();
                EnableCPlayerKaraoke();
                return;
            }
            if (mainForm.GetRockBandKaraokeIsChecked())
            {
                EnableKaraoke();
                EnableRBKaraoke();
                radioBGAnimated.Checked = mainForm.GetAnimatedBackgroundIsChecked();
                radioBGStatic.Checked = mainForm.GetStaticBackgroundIsChecked();
                return;
            }
            if (mainForm.GetRBStyleIsChecked())
            {
                EnableChartVisuals();
                EnableRBVisuals();
                radioBGVideos.Checked = mainForm.GetBackgroundVideosIsChecked();
                radioBGImages.Checked = mainForm.GetBackgroundImagesIsChecked();
                return;
            }
            if (mainForm.GetChartVerticalIsChecked())
            {
                EnableChartVisuals();
                EnableVerticalVisuals();
                return;
            }
            if (mainForm.GetChartSnippetIsChecked())
            {
                EnableChartVisuals();
                EnableMIDIVisuals();
                return;
            }
        }

        private void EnableKaraoke()
        {
            SuspendLayout();
            Width = expandedWidth;
            Height = expandedHeight;
            button1.Visible = true;
            button2.Visible = true;
            button3.Visible = true;
            tabSettings.Visible = true;
            ResumeLayout();
            DisableAllTopButtons();
            btnKaraoke.BackColor = enabledTopColor;
            button1.Text = "Modern";
            button2.Text = "cPlayer";
            button3.Text = "Rock Band";
            tabSettings.TabPages.Add(tabKaraokeModern);
            tabSettings.TabPages.Add(tabKaraokecPlayer);
            tabSettings.TabPages.Add(tabKaraokeRB);
            isKaraokeMode = true;
        }

        private void EnableChartVisuals()
        {
            SuspendLayout();
            Width = expandedWidth;
            Height = contractedHeight;
            button1.Visible = true;
            button2.Visible = true;
            button3.Visible = true;
            tabSettings.Visible = true;
            ResumeLayout();
            DisableAllTopButtons();
            btnChartVisuals.BackColor = enabledTopColor;
            button1.Text = "Rock Band";
            button2.Text = "Vertical";
            button3.Text = "MIDI";
            tabSettings.TabPages.Add(tabVisualsRB);
            tabSettings.TabPages.Add(tabVisualsVertical);
            tabSettings.TabPages.Add(tabVisualsMIDI);
            isKaraokeMode = false;
        }

        private void EnableAlbumArt()
        {
            SuspendLayout();
            tabSettings.Visible = false;
            button1.Visible = false;
            Width = contractedWidth;
            Height = shortHeight;
            ResumeLayout();
            DisableAllTopButtons();
            btnAlbumArt.BackColor = enabledTopColor;
            mainForm.ClickDisplayAlbumArt();
        }

        private void EnableVisualizer()
        {
            SuspendLayout();
            tabSettings.Visible = true;
            button1.Visible = true;
            button1.Text = "Spectrum++";
            button2.Visible = true;
            button2.Text = "Spectrum--";
            button3.Visible = false;
            Width = expandedWidth;
            Height = contractedHeight;
            ResumeLayout();
            DisableAllTopButtons();
            btnVisualizer.BackColor = enabledTopColor;
            tabSettings.TabPages.Add(tabVisualizer);
            mainForm.ClickDisplayAudioSpectrum();
        }

        private void EnableModernKaraoke()
        {
            DisableAllSubButtons();
            button1.BackColor = enabledSubColor;
            tabSettings.SelectedTab = tabKaraokeModern;
            mainForm.ClickClassicKaraokeMode();
        }

        private void EnableCPlayerKaraoke()
        {
            DisableAllSubButtons();
            button2.BackColor = enabledSubColor;
            tabSettings.SelectedTab = tabKaraokecPlayer;
            mainForm.ClickCPlayerStyle();
        }

        private void EnableRBKaraoke()
        {
            DisableAllSubButtons();
            button3.BackColor = enabledSubColor;
            tabSettings.SelectedTab = tabKaraokeRB;
            mainForm.ClickRockBandKaraoke();
        }

        private void EnableRBVisuals()
        {
            DisableAllSubButtons();
            button1.BackColor = enabledSubColor;
            tabSettings.SelectedTab = tabVisualsRB;
            mainForm.ClickRBStyle();
        }

        private void EnableVerticalVisuals()
        {
            DisableAllSubButtons();
            button2.BackColor = enabledSubColor;
            tabSettings.SelectedTab = tabVisualsVertical;
            mainForm.ClickChartVertical();
        }

        private void EnableMIDIVisuals()
        {
            DisableAllSubButtons();
            button3.BackColor = enabledSubColor;
            tabSettings.SelectedTab = tabVisualsMIDI;
            mainForm.ClickChartSnippet();
        }

        private void DisableAllSubButtons()
        {
            button1.BackColor = defaultColor;
            button2.BackColor = defaultColor;
            button3.BackColor = defaultColor;
        }

        private void DisableAllTopButtons()
        {
            btnAlbumArt.BackColor = defaultColor;
            btnVisualizer.BackColor = defaultColor;
            btnKaraoke.BackColor = defaultColor;
            btnChartVisuals.BackColor = defaultColor;
            tabSettings.TabPages.Remove(tabKaraokecPlayer);
            tabSettings.TabPages.Remove(tabKaraokeModern);
            tabSettings.TabPages.Remove(tabKaraokeRB);
            tabSettings.TabPages.Remove(tabVisualsRB);
            tabSettings.TabPages.Remove(tabVisualsMIDI);
            tabSettings.TabPages.Remove(tabVisualsVertical);
            tabSettings.TabPages.Remove(tabVisualizer);
            DisableAllSubButtons();
        }                    

        private void HideTabHeaders()
        {
            tabSettings.Appearance = TabAppearance.FlatButtons;
            tabSettings.SizeMode = TabSizeMode.Fixed;
            tabSettings.ItemSize = new Size(0, 1);
            tabSettings.Padding = new Point(0, 0);
        }

        private void btnAlbumArt_Click(object sender, EventArgs e) { EnableAlbumArt(); }

        private void btnVisualizer_Click(object sender, EventArgs e) { EnableVisualizer(); }

        private void btnKaraoke_Click(object sender, EventArgs e) { EnableKaraoke(); }

        private void btnChartVisuals_Click(object sender, EventArgs e) { EnableChartVisuals(); }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text.Contains("Spectrum"))
            {
                if (mainForm.SpectrumID == 6)
                {
                    mainForm.SpectrumID = 0;
                    return;
                }
                mainForm.SpectrumID++;
                return;
            }
            if(isKaraokeMode)
            {
                EnableModernKaraoke();
            } else
            {
                EnableRBVisuals();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button1.Text.Contains("Spectrum"))
            {
                if (mainForm.SpectrumID == 0)
                {
                    mainForm.SpectrumID = 6;
                    return;
                }
                mainForm.SpectrumID--;
                return;
            }
            if (isKaraokeMode)
            {
                EnableCPlayerKaraoke();
            } else
            {
                EnableVerticalVisuals();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(isKaraokeMode)
            {
                EnableRBKaraoke();
            } else
            {
                EnableMIDIVisuals();
            }
        }

        private void radioSolidColor_CheckedChanged(object sender, EventArgs e)
        {
            if (radioSolidColor.Checked)
            {
                mainForm.ClickSolidColorBackground();
            }
        }

        private void radioStatic_CheckedChanged(object sender, EventArgs e)
        {
            if (radioStatic.Checked)
            {
                mainForm.ClickEnableBackgroundImage();
            }
        }

        private void radioAnimated_CheckedChanged(object sender, EventArgs e)
        {
            if (radioAnimated.Checked)
            {
                mainForm.ClickAnimatedBackground2();
            }
        }

        private void radioForceSolo_CheckedChanged(object sender, EventArgs e)
        {
            if (radioForceSolo.Checked)
            {
                mainForm.ClickForceSoloVocals();
            }
        }

        private void radioForceHarmonies_CheckedChanged(object sender, EventArgs e)
        {
            if (radioForceHarmonies.Checked)
            {
                mainForm.ClickForceTwoPartHarmonies();
            }
        }

        private void radioBGAnimated_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBGAnimated.Checked)
            {
                mainForm.ClickAnimatedBackground();
            }
        }

        private void radioBGStatic_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBGStatic.Checked)
            {
                mainForm.ClickStaticBackground();
            }
        }

        private void radioBGVideos_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBGVideos.Checked)
            {
                mainForm.ClickBackgroundVideos();
            }
        }

        private void radioBGImages_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBGImages.Checked)
            {
                mainForm.ClickBackgroundImages();
            }
        }

        private Color GetUserColor(Color currentColor)
        {
            Color selectedColor = currentColor;
            using (var dlg = new ColorDialog())
            {
                dlg.FullOpen = true;           // shows custom colors section
                dlg.AnyColor = true;
                dlg.Color = currentColor;      // optional: preselect

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    selectedColor = dlg.Color;
                }
            }
            return selectedColor;
        }

        private void btnBGColor_Click(object sender, EventArgs e)
        {
            btnBGColor.BackColor = GetUserColor(btnBGColor.BackColor);
            mainForm.KaraokeModeBackgroundColor = btnBGColor.BackColor;
        }

        private void btnHarm1Color_Click(object sender, EventArgs e)
        {
            btnHarm1Color.BackColor = GetUserColor(btnHarm1Color.BackColor);
            mainForm.KaraokeModeHarm1Text = btnHarm1Color.BackColor;
        }

        private void btnHarm1HighlightColor_Click(object sender, EventArgs e)
        {
            btnHarm1HighlightColor.BackColor = GetUserColor(btnHarm1HighlightColor.BackColor);
            mainForm.KaraokeModeHarm1Highlight = btnHarm1HighlightColor.BackColor;
        }

        private void btnHarm2Color_Click(object sender, EventArgs e)
        {
            btnHarm2Color.BackColor = GetUserColor(btnHarm2Color.BackColor);
            mainForm.KaraokeModeHarm2Text = btnHarm2Color.BackColor;
        }

        private void btnHarm2HighlightColor_Click(object sender, EventArgs e)
        {
            btnHarm2HighlightColor.BackColor = GetUserColor(btnHarm2HighlightColor.BackColor);
            mainForm.KaraokeModeHarm2Highlight = btnHarm2HighlightColor.BackColor;
        }

        private void btnHarm3Color_Click(object sender, EventArgs e)
        {
            btnHarm3Color.BackColor = GetUserColor(btnHarm3Color.BackColor);
            mainForm.KaraokeModeHarm3Text = btnHarm3Color.BackColor;
        }

        private void btnHarm3HighlightColor_Click(object sender, EventArgs e)
        {
            btnHarm3HighlightColor.BackColor = GetUserColor(btnHarm3HighlightColor.BackColor);
            mainForm.KaraokeModeHarm3Highlight = btnHarm3HighlightColor.BackColor;
        }

        private void btnDefaults_Click(object sender, EventArgs e)
        {
            mainForm.KaraokeModeBackgroundColor = Color.Orange;
            mainForm.KaraokeModeHarm1Text = Color.White;
            mainForm.KaraokeModeHarm1Highlight = Color.DeepSkyBlue;
            mainForm.KaraokeModeHarm2Text = Color.LightGray;
            mainForm.KaraokeModeHarm2Highlight = Color.LightPink;
            mainForm.KaraokeModeHarm3Text = Color.DarkGray;
            mainForm.KaraokeModeHarm3Highlight = Color.DarkSeaGreen;

            btnBGColor.BackColor = Color.Orange;
            btnHarm1Color.BackColor = Color.White;
            btnHarm1HighlightColor.BackColor = Color.DeepSkyBlue;
            btnHarm2Color.BackColor = Color.LightGray;
            btnHarm2HighlightColor.BackColor = Color.LightPink;
            btnHarm3Color.BackColor = Color.DarkGray;
            btnHarm3HighlightColor.BackColor = Color.DarkSeaGreen;
        }

        private void chkDefault_CheckedChanged(object sender, EventArgs e)
        {
            if (mainForm.GetForceSoloVocalsIsChecked())
            {
                mainForm.ClickForceSoloVocals();
            }
            else if (mainForm.GetForceTwoPartHarmoniesIsChecked())
            {
                mainForm.ClickForceTwoPartHarmonies();
            }
        }

        private void btnSpectrumBG_Click(object sender, EventArgs e)
        {
            btnSpectrumBG.BackColor = GetUserColor(btnSpectrumBG.BackColor);
            mainForm.SpectrumColor = btnSpectrumBG.BackColor;
        }
    }
}

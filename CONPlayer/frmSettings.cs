using System;
using System.Drawing;
using System.Threading.Tasks;
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
        private const int shortHeight = 219;
        private bool isKaraokeMode;
        private bool isChangingMode;

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
                        
            if (mainForm.doModernKaraokeMode)
            {
                EnableKaraoke();
                EnableModernKaraoke();
                return;
            }
            if (mainForm.doCPlayerStyleKaraoke)
            {
                EnableKaraoke();
                EnableCPlayerKaraoke();
                return;
            }
            if (mainForm.doRockBandKaraoke)
            {
                EnableKaraoke();
                EnableRBKaraoke();                
                return;
            }
            if (mainForm.doRockBandChart)
            {
                EnableChartVisuals();                
                EnableRBVisuals();                
                return;
            }
            if (mainForm.doVerticalChart)
            {
                EnableChartVisuals();
                EnableVerticalVisuals();
                return;
            }
            if (mainForm.doMIDIChart)
            {
                EnableChartVisuals();
                EnableMIDIVisuals();
                return;
            }
            if (mainForm.displayAlbumArt)
            {
                EnableAlbumArt();
                return;
            }
            if (mainForm.displayAudioSpectrum)
            {
                EnableVisualizer();
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
            mainForm.doAnimatedSpectrum = false;
            mainForm.doUseBackgroundVideos = false;
            button1.PerformClick();
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
            button1.PerformClick();
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
            mainForm.doAnimatedSpectrum = false;
            mainForm.doUseBackgroundVideos = false;
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
            mainForm.doAnimatedSpectrum = false;
            mainForm.doUseBackgroundVideos = false;
        }

        private void EnableModernKaraoke()
        {
            DisableAllSubButtons();
            button1.BackColor = enabledSubColor;
            tabSettings.SelectedTab = tabKaraokeModern;
            mainForm.ClickClassicKaraokeMode();
            radioSolidColor.Checked = mainForm.doSolidColorBackground;
            radioStatic.Checked = mainForm.doStaticBackground2;
            radioAnimated.Checked = mainForm.doAnimatedBackground2;
            radioDefault.Checked = !mainForm.doForceSoloVocals && !mainForm.doForceTwoPartHarmonies;
            radioForceHarmonies.Checked = mainForm.doForceTwoPartHarmonies;
            radioForceSolo.Checked = mainForm.doForceSoloVocals;
            if (!radioDefault.Checked && !radioForceSolo.Checked && !radioForceHarmonies.Checked)
            {
                radioDefault.Checked = true;
            }
            if (!radioSolidColor.Checked && !radioStatic.Checked & !radioAnimated.Checked)
            {
                radioSolidColor.Checked = true;
            }
            mainForm.doSolidColorBackground = radioSolidColor.Checked;
            mainForm.doStaticBackground2 = radioStatic.Checked;
            mainForm.doAnimatedBackground2 = radioAnimated.Checked;                      
            if (radioDefault.Checked)
            {
                mainForm.doForceSoloVocals = false;
                mainForm.doForceTwoPartHarmonies = false;
            }
            else
            {
                mainForm.doForceSoloVocals = radioForceSolo.Checked;
                mainForm.doForceTwoPartHarmonies = radioForceHarmonies.Checked;
            }
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
            radioBGAnimated.Checked = mainForm.doAnimatedBackground;
            radioBGStatic.Checked = mainForm.doStaticBackground;
            if (!radioBGAnimated.Checked && !radioBGStatic.Checked)
            {
                radioBGAnimated.Checked = true;
            }
            mainForm.doAnimatedBackground = radioBGAnimated.Checked;
            mainForm.doStaticBackground = radioBGStatic.Checked;
        }

        private void EnableRBVisuals()
        {
            DisableAllSubButtons();
            button1.BackColor = enabledSubColor;
            tabSettings.SelectedTab = tabVisualsRB;
            mainForm.ClickRBStyle();
            radioBGVideos.Checked = mainForm.doUseBackgroundVideos;
            radioBGImages.Checked = mainForm.doBackgroundImages;
            chkFocusMode.Checked = mainForm.doFocusMode;
            radioAnimSpectrum.Checked = mainForm.doAnimatedSpectrum;
            chkColorful.Checked = mainForm.doSpectrumColors;
            if (!radioBGVideos.Checked && !radioBGImages.Checked && !radioAnimSpectrum.Checked)
            {
                radioAnimSpectrum.Checked = true;
            }
            if (chkFocusMode.Checked)
            {
                groupBox4.Enabled = false;               
            }
        }

        private void EnableVerticalVisuals()
        {
            DisableAllSubButtons();
            button2.BackColor = enabledSubColor;
            tabSettings.SelectedTab = tabVisualsVertical;
            mainForm.ClickVerticalChart();
            mainForm.doAnimatedSpectrum = false;
            mainForm.doUseBackgroundVideos = false;
        }

        private void EnableMIDIVisuals()
        {
            DisableAllSubButtons();
            button3.BackColor = enabledSubColor;
            tabSettings.SelectedTab = tabVisualsMIDI;
            mainForm.ClickMIDIChart();
            mainForm.doAnimatedSpectrum = false;
            mainForm.doUseBackgroundVideos = false;
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
            } 
            else
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
                mainForm.doAnimatedSpectrum = false;
                mainForm.doUseBackgroundVideos = false;
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
                mainForm.doAnimatedSpectrum = false;
                mainForm.doUseBackgroundVideos = false;
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
            try
            {
                if (!radioBGVideos.Checked) return;
                if (isChangingMode)
                {
                    isChangingMode = false;
                    return;
                }
                isChangingMode = true;
                EnableDisableRadioButtons(false);
                if (radioBGVideos.Checked)
                {
                    mainForm.doUseBackgroundVideosLast = false;
                    mainForm.ClickBackgroundVideos();
                    mainForm.doUseBackgroundVideos = true;
                }
                EnableDisableRadioButtons(true);
                isChangingMode = false;
            }
            catch { }
        }

        private void EnableDisableRadioButtons(bool enable)
        {
            radioBGImages.Enabled = enable;
            radioBGVideos.Enabled = enable;
            radioAnimSpectrum.Enabled = enable;
        }

        private void radioBGImages_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!radioBGImages.Checked) return;
                if (isChangingMode)
                {
                    isChangingMode = false;
                    return;
                }
                isChangingMode = true;
                EnableDisableRadioButtons(false);
                if (radioBGImages.Checked)
                {                    
                    mainForm.ClickBackgroundImages();
                }
                EnableDisableRadioButtons(true);
                isChangingMode = false;
            }
            catch { }            
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
            mainForm.ClearKaraokeLineCache();
        }

        private void btnHarm1Color_Click(object sender, EventArgs e)
        {
            btnHarm1Color.BackColor = GetUserColor(btnHarm1Color.BackColor);
            mainForm.KaraokeModeHarm1Text = btnHarm1Color.BackColor;
            mainForm.ClearKaraokeLineCache();
        }

        private void btnHarm1HighlightColor_Click(object sender, EventArgs e)
        {
            btnHarm1HighlightColor.BackColor = GetUserColor(btnHarm1HighlightColor.BackColor);
            mainForm.KaraokeModeHarm1Highlight = btnHarm1HighlightColor.BackColor;
            mainForm.ClearKaraokeLineCache();
        }

        private void btnHarm2Color_Click(object sender, EventArgs e)
        {
            btnHarm2Color.BackColor = GetUserColor(btnHarm2Color.BackColor);
            mainForm.KaraokeModeHarm2Text = btnHarm2Color.BackColor;
            mainForm.ClearKaraokeLineCache();
        }

        private void btnHarm2HighlightColor_Click(object sender, EventArgs e)
        {
            btnHarm2HighlightColor.BackColor = GetUserColor(btnHarm2HighlightColor.BackColor);
            mainForm.KaraokeModeHarm2Highlight = btnHarm2HighlightColor.BackColor;
            mainForm.ClearKaraokeLineCache();
        }

        private void btnHarm3Color_Click(object sender, EventArgs e)
        {
            btnHarm3Color.BackColor = GetUserColor(btnHarm3Color.BackColor);
            mainForm.KaraokeModeHarm3Text = btnHarm3Color.BackColor;
            mainForm.ClearKaraokeLineCache();
        }

        private void btnHarm3HighlightColor_Click(object sender, EventArgs e)
        {
            btnHarm3HighlightColor.BackColor = GetUserColor(btnHarm3HighlightColor.BackColor);
            mainForm.KaraokeModeHarm3Highlight = btnHarm3HighlightColor.BackColor;
            mainForm.ClearKaraokeLineCache();
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

            mainForm.ClearKaraokeLineCache();
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

        private void radioAnimSpectrum_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!radioAnimSpectrum.Checked) return;
                if (isChangingMode)
                {
                    isChangingMode = false;
                    return;
                }
                isChangingMode = true;
                EnableDisableRadioButtons(false);
                if (radioAnimSpectrum.Checked)
                {
                    mainForm.doAnimatedSpectrum = true;
                    mainForm.doUseBackgroundVideos = false;
                    mainForm.doUseBackgroundImages = false;
                    mainForm.doBackgroundImages = false;
                    mainForm.StopAllVideoPlayback();
                }
                EnableDisableRadioButtons(true);
                isChangingMode = false;
            }
            catch { }
        }

        private async void chkFocusMode_CheckedChanged(object sender, EventArgs e)
        {
            try
            {                
                groupBox4.Enabled = !chkFocusMode.Checked;
                if (chkFocusMode.Checked)
                {
                    isChangingMode = true;
                    radioBGVideos.Checked = false;
                    radioBGImages.Checked = false;
                    radioAnimSpectrum.Checked = false;
                    isChangingMode = false;
                }
                mainForm.ClickFocusMode(chkFocusMode.Checked);
                await mainForm.DrawRockBandStyleAsync(null, true).ConfigureAwait(true);
                await Task.Delay(1000).ConfigureAwait(true);
            }
            catch { }
        }

        private void lblRandomVideo_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            mainForm.doUseBackgroundVideosLast = false;
            mainForm.ChangeRBStyleBackground();
        }

        private void lblRandomImage_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            mainForm.doUseBackgroundImagesLast = false;
            mainForm.ChangeRBStyleBackground();
        }

        private void chkColorful_CheckedChanged(object sender, EventArgs e)
        {
            mainForm.doSpectrumColors = chkColorful.Checked;
        }

        private void btnFullScreen_Click(object sender, EventArgs e)
        {
            mainForm.doResizeVisuals();
        }
    }
}

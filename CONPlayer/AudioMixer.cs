using System;
using System.Drawing;
using System.Windows.Forms;
using Un4seen.Bass;
using Un4seen.Bass.Misc;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace cPlayer
{
    public partial class AudioMixer : Form
    {
        private readonly frmMain MainForm;
        private readonly Song PlayingSong;
        private readonly Visuals Spectrum = new Visuals();
        private readonly Color ChartRed = Color.FromArgb(255, 0, 0);
        private readonly Color ChartGreen = Color.FromArgb(0, 255, 0);
        private bool isLoading;
        private bool suppressNextMouseUp;
        private const float defaultGain = 1.0f;
        private const float MasterMinDb = -40.0f;
        private const float MasterMaxDb = 0.0f;
        private const float defaultMasterGain = 0.8f;
        private readonly Graphics spectrumGraphics;


        public AudioMixer(frmMain mainForm, Song song)
        {
            InitializeComponent();
            MainForm = mainForm;
            PlayingSong = song;
            spectrumGraphics = picSpectrum.CreateGraphics();
        }

        private void AudioMixer_Shown(object sender, EventArgs e)
        {
            isLoading = true;

            try
            {
                chkDrums.Checked = MainForm.doAudioDrums;
                chkBass.Checked = MainForm.doAudioBass;
                chkGuitar.Checked = MainForm.doAudioGuitar;
                chkVocals.Checked = MainForm.doAudioVocals;
                chkKeys.Checked = MainForm.doAudioKeys;
                chkBacking.Checked = MainForm.doAudioBacking;
                chkCrowd.Checked = MainForm.doAudioCrowd;

                if (PlayingSong != null)
                {
                    if (PlayingSong.ChannelsBass == 0) chkBass.Checked = false;
                    if (PlayingSong.ChannelsDrums == 0) chkDrums.Checked = false;
                    if (PlayingSong.ChannelsKeys == 0) chkKeys.Checked = false;
                    if (PlayingSong.ChannelsVocals == 0) chkVocals.Checked = false;
                    if (PlayingSong.ChannelsGuitar == 0) chkGuitar.Checked = false;
                    if (PlayingSong.ChannelsCrowd == 0) chkCrowd.Checked = false;
                    if (PlayingSong.ChannelsBacking == 0) chkBacking.Checked = false;

                    Text = "Audio Mixer: " + PlayingSong.Artist + " - " + PlayingSong.Name;
                }

                VerifySliderEnabled();

                SetVerticalSliderFromGain(picBassSlider, picBassBackground, MainForm.bassVol, Stem.Bass);
                SetVerticalSliderFromGain(picDrumsSlider, picDrumsBackground, MainForm.drumsVol, Stem.Drums);
                SetVerticalSliderFromGain(picGuitarSlider, picGuitarBackground, MainForm.guitarVol, Stem.Guitar);
                SetVerticalSliderFromGain(picKeysSlider, picKeysBackground, MainForm.keysVol, Stem.Keys);
                SetVerticalSliderFromGain(picVocalsSlider, picVocalsBackground, MainForm.vocalsVol, Stem.Vocals);
                SetVerticalSliderFromGain(picBackingSlider, picBackingBackground, MainForm.backingVol, Stem.Backing);
                SetVerticalSliderFromGain(picCrowdSlider, picCrowdBackground, MainForm.crowdVol, Stem.Crowd);
                SetVerticalSliderFromMasterGain(picMasterSlider, picMasterBackground, MainForm.masterVol);
                
                GainToDbText(MainForm.masterVol, Stem.Master);
                GainToDbText(MainForm.bassVol, Stem.Bass);
                GainToDbText(MainForm.drumsVol, Stem.Drums);
                GainToDbText(MainForm.guitarVol, Stem.Guitar);
                GainToDbText(MainForm.keysVol, Stem.Keys);
                GainToDbText(MainForm.vocalsVol, Stem.Vocals);
                GainToDbText(MainForm.backingVol, Stem.Backing);
                GainToDbText(MainForm.crowdVol, Stem.Crowd);
            }
            finally
            {
                isLoading = false;
            }
        }

        private void SetVerticalSliderFromMasterGain(PictureBox slider, PictureBox background, float gain)
        {
            float minGain = DbToGain(MasterMinDb);
            float maxGain = DbToGain(MasterMaxDb); // 1.0f

            if (gain < minGain) gain = minGain;
            if (gain > maxGain) gain = maxGain;

            int minTop = background.Top;
            int maxTop = background.Bottom - slider.Height;

            if (maxTop < minTop)
            {
                slider.Top = minTop;
                return;
            }

            float range = maxTop - minTop;

            float db = GainToDb(gain);

            if (Math.Abs(db) < 0.15f)
                db = 0.0f;

            if (db < MasterMinDb) db = MasterMinDb;
            if (db > MasterMaxDb) db = MasterMaxDb;

            float percent = (db - MasterMinDb) / (MasterMaxDb - MasterMinDb);

            float newTop = maxTop - (percent * range);

            if (newTop < minTop) newTop = minTop;
            if (newTop > maxTop) newTop = maxTop;

            slider.Top = (int)Math.Round(newTop);

            MainForm.masterVol = gain;
            
            GainToDbText(gain, Stem.Master);
        }

        private void ApplyMasterGain(PictureBox slider, PictureBox background)
        {
            float minTop = background.Top;
            float maxTop = background.Bottom - slider.Height;
            float range = maxTop - minTop;

            float db;
            float gain;

            if (range <= 0)
            {
                db = GainToDb(defaultMasterGain);
                gain = defaultMasterGain;
            }
            else
            {
                float clampedTop = Math.Max(minTop, Math.Min(slider.Top, maxTop));

                float percent = 1.0f - ((clampedTop - minTop) / range);

                db = MasterMinDb + (percent * (MasterMaxDb - MasterMinDb));

                if (Math.Abs(db) < 0.15f)
                    db = 0.0f;

                gain = DbToGain(db);
            }

            MainForm.masterVol = gain;
            GainToDbText(gain, Stem.Master);

            if (!isLoading)
                MainForm.ApplyMasterVolume();
        }

        private void VerifySliderEnabled()
        {
            picBassSlider.Enabled = chkBass.Checked;
            picDrumsSlider.Enabled = chkDrums.Checked;
            picGuitarSlider.Enabled = chkGuitar.Checked;
            picKeysSlider.Enabled = chkKeys.Checked;
            picVocalsSlider.Enabled = chkVocals.Checked;
            picBackingSlider.Enabled = chkBacking.Checked;
            picCrowdSlider.Enabled = chkCrowd.Checked;

            if (!chkBass.Checked) picBassBackground.BackColor = Color.Silver;
            if (!chkDrums.Checked) picDrumsBackground.BackColor = Color.Silver;
            if (!chkGuitar.Checked) picGuitarBackground.BackColor = Color.Silver;
            if (!chkKeys.Checked) picKeysBackground.BackColor = Color.Silver;
            if (!chkVocals.Checked) picVocalsBackground.BackColor = Color.Silver;
            if (!chkBacking.Checked) picBackingBackground.BackColor = Color.Silver;
            if (!chkCrowd.Checked) picCrowdBackground.BackColor = Color.Silver;
        }

        private void UpdateAudioPlayback()
        {
            if (isLoading) return;
            MainForm.doAudioDrums = chkDrums.Checked;
            MainForm.doAudioBass = chkBass.Checked;
            MainForm.doAudioGuitar = chkGuitar.Checked;
            MainForm.doAudioVocals = chkVocals.Checked;
            MainForm.doAudioKeys = chkKeys.Checked;
            MainForm.doAudioBacking = chkBacking.Checked;
            MainForm.doAudioCrowd = chkCrowd.Checked;

            ApplyGainToStem(picBassSlider, picBassBackground, Stem.Bass);
            ApplyGainToStem(picDrumsSlider, picDrumsBackground, Stem.Drums);
            ApplyGainToStem(picGuitarSlider, picGuitarBackground, Stem.Guitar);
            ApplyGainToStem(picKeysSlider, picKeysBackground, Stem.Keys);
            ApplyGainToStem(picVocalsSlider, picVocalsBackground, Stem.Vocals);
            ApplyGainToStem(picBackingSlider, picBackingBackground, Stem.Backing);
            ApplyGainToStem(picCrowdSlider, picCrowdBackground, Stem.Crowd);

            UpdateAudioStems();
            VerifySliderEnabled();
        }

        private void chkBass_CheckedChanged(object sender, EventArgs e)
        {
            UpdateAudioPlayback();
        }

        private void picBassSlider_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            ResetStemSlider(picBassSlider, picBassBackground, Stem.Bass);
        }

        private void picDrumsSlider_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            ResetStemSlider(picDrumsSlider, picDrumsBackground, Stem.Drums);
        }

        private void picGuitarSlider_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            ResetStemSlider(picGuitarSlider, picGuitarBackground, Stem.Guitar);
        }

        private void picKeysSlider_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            ResetStemSlider(picKeysSlider, picKeysBackground, Stem.Keys);
        }

        private void picVocalsSlider_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            ResetStemSlider(picVocalsSlider, picVocalsBackground, Stem.Vocals);
        }

        private void picBackingSlider_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            ResetStemSlider(picBackingSlider, picBackingBackground, Stem.Backing);
        }

        private void picCrowdSlider_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            ResetStemSlider(picCrowdSlider, picCrowdBackground, Stem.Crowd);
        }

        private enum Stem
        {
            Master, Bass, Drums, Guitar, Keys, Vocals, Backing, Crowd
        }

        private void SliderMouseDown(PictureBox slider, PictureBox background)
        {
            slider.Cursor = Cursors.NoMoveVert;
            ClampSliderToBackground(slider, background);            
        }

        private void SliderMouseUp(PictureBox slider, PictureBox background, Stem stem)
        {
            if (suppressNextMouseUp)
            {
                suppressNextMouseUp = false;
                slider.Cursor = Cursors.Hand;
                return;
            }

            slider.Cursor = Cursors.Hand;
            ClampSliderToBackground(slider, background);
            ApplyGainToStem(slider, background, stem);
        }

        private void picBassSlider_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            SliderMouseDown(picBassSlider, picBassBackground);
        }

        private void picDrumsSlider_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            SliderMouseDown(picDrumsSlider, picDrumsBackground);
        }

        private void picGuitarSlider_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            SliderMouseDown(picGuitarSlider, picGuitarBackground);
        }

        private void picKeysSlider_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            SliderMouseDown(picKeysSlider, picKeysBackground);
        }

        private void picVocalsSlider_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            SliderMouseDown(picVocalsSlider, picVocalsBackground);
        }

        private void picBackingSlider_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            SliderMouseDown(picBackingSlider, picBackingBackground);
        }

        private void picCrowdSlider_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            SliderMouseDown(picCrowdSlider, picCrowdBackground);
        }

        private void picBassSlider_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            SliderMouseMove(picBassSlider, picBassBackground, Stem.Bass);
        }

        private void SliderMouseMove(PictureBox slider, PictureBox background, Stem stem)
        {
            Point mouseInParent = slider.Parent.PointToClient(Cursor.Position);

            SetSliderTopCenteredOnMouse(
                slider,
                background,
                mouseInParent.Y
            );
            ApplyGainToStem(slider, background, stem);
        }

        private void ApplyGainToStem(PictureBox slider, PictureBox background, Stem stem)
        {
            const float minDb = -24.0f;
            const float maxDb = 12.0f;

            float minTop = background.Top;
            float maxTop = background.Bottom - slider.Height;
            float range = maxTop - minTop;

            float db;
            float gain;

            if (range <= 0)
            {
                db = 0.0f;
                gain = 1.0f;
            }
            else
            {
                float clampedTop = Math.Max(minTop, Math.Min(slider.Top, maxTop));

                // 0.0 at bottom, 1.0 at top
                float percent = 1.0f - ((clampedTop - minTop) / range);

                // Slider now maps to dB, not direct linear gain
                db = minDb + (percent * (maxDb - minDb));

                // Snap very near unity to exactly 0.0 dB.
                // This prevents +0.1 / -0.1 dB caused by integer pixel rounding.
                if (Math.Abs(db) < 0.15f)
                    db = 0.0f;

                // Convert dB to BASS linear gain
                gain = DbToGain(db);
            }

            SetStemGain(stem, gain);
            GainToDbText(gain, stem);
            UpdateSliderColorFromDb(db, background);

            if (!isLoading)
                UpdateAudioStems();
        }        

        private void UpdateAudioStems()
        {
            if (MainForm.BassStream != 0 && MainForm.BassMixer != 0) //this should be instant
            {
                MainForm.UpdateStemVolumes();
            }
            else //this is fallback, will flicker for a split second
            {
                MainForm.UpdatePlayback(false);
            }
        }

        private void SetStemGain(Stem stem, float gain)
        {
            switch (stem)
            {
                case Stem.Bass:
                    MainForm.bassVol = gain;
                    break;

                case Stem.Guitar:
                    MainForm.guitarVol = gain;
                    break;

                case Stem.Drums:
                    MainForm.drumsVol = gain;
                    break;

                case Stem.Keys:
                    MainForm.keysVol = gain;
                    break;

                case Stem.Vocals:
                    MainForm.vocalsVol = gain;
                    break;

                case Stem.Backing:
                    MainForm.backingVol = gain;
                    break;

                case Stem.Crowd:
                    MainForm.crowdVol = gain;
                    break;
            }
        }

        private void UpdateSliderColorFromDb(float db, PictureBox background, bool enabled = true)
        {
            if (!enabled)
            {
                background.BackColor = Color.Silver;
                return;
            }

            if (db > 6.0f)
            {
                // Strong boost
                background.BackColor = Color.FromArgb(255, 215, 0); // gold/yellow
            }
            else if (db > 0.0f)
            {
                // Above original volume
                background.BackColor = Color.Orange;
            }
            else
            {
                // Original or attenuated
                background.BackColor = Color.LimeGreen;
            }
        }

        private void ResetStemSlider(PictureBox slider, PictureBox background, Stem stem)
        {
            suppressNextMouseUp = true;
            SetVerticalSliderFromGain(slider, background, defaultGain, stem);

            SetStemGain(stem, defaultGain);
            GainToDbText(defaultGain, stem);
            UpdateSliderColorFromDb(0.0f, background);

            if (!isLoading)
                UpdateAudioStems();
        }

        private void SetVerticalSliderFromGain(PictureBox slider, PictureBox background, float gain, Stem stem)
        {
            const float minDb = -24.0f;
            const float maxDb = 12.0f;

            // Clamp linear gain to the dB slider's valid gain range
            float minGain = DbToGain(minDb);
            float maxGain = DbToGain(maxDb); 

            if (gain < minGain) gain = minGain;
            if (gain > maxGain) gain = maxGain;

            int minTop = background.Top;
            int maxTop = background.Bottom - slider.Height;

            if (maxTop < minTop)
            {
                slider.Top = minTop;
                return;
            }

            float range = maxTop - minTop;

            // Convert linear gain to dB
            float db = GainToDb(gain);

            // Clamp dB
            if (db < minDb) db = minDb;
            if (db > maxDb) db = maxDb;

            // -24 dB = bottom, +12 dB = top
            float percent = (db - minDb) / (maxDb - minDb);

            // Invert because smaller Top means higher on screen
            float newTop = maxTop - (percent * range);

            // Clamp slider.Top to background bounds
            if (newTop < minTop) newTop = minTop;
            if (newTop > maxTop) newTop = maxTop;

            slider.Top = (int)Math.Round(newTop);

            var enabled = false;

            switch (stem)
            {
                case Stem.Bass:
                    MainForm.bassVol = gain;
                    enabled = chkBass.Checked;
                    break;
                case Stem.Drums:
                    MainForm.drumsVol = gain;
                    enabled = chkDrums.Checked;
                    break;
                case Stem.Guitar:
                    MainForm.guitarVol = gain;
                    enabled = chkGuitar.Checked;
                    break;
                case Stem.Keys:
                    MainForm.keysVol = gain;
                    enabled = chkKeys.Checked;
                    break;
                case Stem.Vocals:
                    MainForm.vocalsVol = gain;
                    enabled = chkVocals.Checked;
                    break;
                case Stem.Backing:
                    MainForm.backingVol = gain;
                    enabled = chkBacking.Checked;
                    break;
                case Stem.Crowd:
                    MainForm.crowdVol = gain;
                    enabled = chkCrowd.Checked;
                    break;
            }

            GainToDbText(gain, stem);
            UpdateSliderColorFromDb(db, background, enabled);
        }

        private float DbToGain(float db)
        {
            return (float)Math.Pow(10.0, db / 20.0);
        }

        private float GainToDb(float gain)
        {
            if (gain <= 0.0001f)
                return -60.0f;

            return (float)(20.0 * Math.Log10(gain));
        }

        private void GainToDbText(float gain, Stem stem)
        {
            string dB;

            if (gain <= 0.0001f)
            {
                dB = "-∞ dB"; // muted / silence
            }
            else
            {
                double db = 20.0 * Math.Log10(gain);

                if (Math.Abs(db) < 0.05)
                    db = 0.0; // avoid showing "-0.0 dB"

                dB = $"{db:+0.0;-0.0;0.0} dB";
            }

            switch (stem)
            {
                case Stem.Master:
                    lblMaster.Text = dB;
                    break;
                case Stem.Bass:
                    lblBass.Text = dB;
                    break;
                case Stem.Drums:
                    lblDrums.Text = dB;
                    break;
                case Stem.Guitar:
                    lblGuitar.Text = dB;
                    break;
                case Stem.Keys:
                    lblKeys.Text = dB;
                    break;
                case Stem.Vocals:
                    lblVocals.Text = dB;
                    break;
                case Stem.Backing:
                    lblBacking.Text = dB;
                    break;
                case Stem.Crowd:
                    lblCrowd.Text = dB;
                    break;
            }
        }

        private void picDrumsSlider_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            SliderMouseMove(picDrumsSlider, picDrumsBackground, Stem.Drums);
        }

        private void picGuitarSlider_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            SliderMouseMove(picGuitarSlider, picGuitarBackground, Stem.Guitar);
        }

        private void picKeysSlider_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            SliderMouseMove(picKeysSlider, picKeysBackground, Stem.Keys);
        }

        private void picVocalsSlider_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            SliderMouseMove(picVocalsSlider, picVocalsBackground, Stem.Vocals);
        }

        private void picBackingSlider_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            SliderMouseMove(picBackingSlider, picBackingBackground, Stem.Backing);
        }

        private void picCrowdSlider_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            SliderMouseMove(picCrowdSlider, picCrowdBackground, Stem.Crowd);
        }        

        private void ClampSliderToBackground(PictureBox slider, PictureBox background)
        {
            int minTop = background.Top;
            int maxTop = background.Bottom - slider.Height;

            if (slider.Top < minTop)
                slider.Top = minTop;

            if (slider.Top > maxTop)
                slider.Top = maxTop;
        }

        private void SetSliderTopCenteredOnMouse(PictureBox slider, PictureBox background, int mouseY)
        {
            int newTop = mouseY - (slider.Height / 2);

            int minTop = background.Top;
            int maxTop = background.Bottom - slider.Height;

            if (newTop < minTop)
                newTop = minTop;

            if (newTop > maxTop)
                newTop = maxTop;

            slider.Top = newTop;
        }

        private void picCrowdSlider_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            SliderMouseUp(picCrowdSlider, picCrowdBackground, Stem.Crowd);
        }

        private void picBackingSlider_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            SliderMouseUp(picBackingSlider, picBackingBackground, Stem.Backing);
        }

        private void picVocalsSlider_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            SliderMouseUp(picVocalsSlider, picVocalsBackground, Stem.Vocals);
        }

        private void picKeysSlider_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            SliderMouseUp(picKeysSlider, picKeysBackground, Stem.Keys);
        }

        private void picGuitarSlider_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            SliderMouseUp(picGuitarSlider, picGuitarBackground, Stem.Guitar);
        }

        private void picDrumsSlider_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            SliderMouseUp(picDrumsSlider, picDrumsBackground, Stem.Drums);
        }

        private void picBassSlider_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            SliderMouseUp(picBassSlider, picBassBackground, Stem.Bass);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var mixerState = Bass.BASS_ChannelIsActive(MainForm.BassMixer);
            if (mixerState != BASSActive.BASS_ACTIVE_PLAYING)
            {
                return;
            }
            DrawSpectrum();
        }
               
        private void DrawSpectrum()
        {
            Spectrum.ChannelIsMixerSource = false;
            Spectrum.MaxFFT = BASSData.BASS_DATA_FFT4096;
            try
            {               
                Spectrum.CreateSpectrumLine(MainForm.BassMixer, spectrumGraphics, picSpectrum.ClientRectangle, ChartGreen, ChartRed, Color.Black, 2, 2, false, false, false);
            }
            catch
            { }
        }

        private void AudioMixer_FormClosing(object sender, FormClosingEventArgs e)
        {
            spectrumGraphics.Dispose();
        }

        private void picMasterSlider_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            SliderMouseDown(picMasterSlider, picMasterBackground);
        }

        private void picMasterSlider_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            Point mouseInParent = picMasterSlider.Parent.PointToClient(Cursor.Position);

            SetSliderTopCenteredOnMouse(
                picMasterSlider,
                picMasterBackground,
                mouseInParent.Y
            );

            ApplyMasterGain(picMasterSlider, picMasterBackground);
        }

        private void picMasterSlider_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            picMasterSlider.Cursor = Cursors.Hand;

            ClampSliderToBackground(picMasterSlider, picMasterBackground);
            ApplyMasterGain(picMasterSlider, picMasterBackground);

            MainForm.ApplyMasterVolume();
        }

        private void picMasterSlider_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            SetVerticalSliderFromMasterGain(picMasterSlider, picMasterBackground, defaultMasterGain);

            MainForm.masterVol = defaultMasterGain;
            GainToDbText(defaultMasterGain, Stem.Master);
            UpdateSliderColorFromDb(GainToDb(defaultMasterGain), picMasterBackground);

            if (!isLoading)
                MainForm.ApplyMasterVolume();
        }
    }
}

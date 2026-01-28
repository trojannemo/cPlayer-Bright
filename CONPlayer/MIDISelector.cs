using System;
using System.Windows.Forms;

namespace cPlayer
{
    public partial class MIDISelector : Form
    {
        private readonly frmMain MainForm;
        public MIDISelector(frmMain xParent)
        {
            InitializeComponent();
            MainForm = xParent;
            ControlBox = false;
        }

        private void MIDISelector_Shown(object sender, EventArgs e)
        {
            chkDrums.Checked = MainForm.doMIDIDrums;
            chkBass.Checked = MainForm.doMIDIBass;
            chkGuitar.Checked = MainForm.doMIDIGuitar;
            radioVocals.Checked = MainForm.doMIDIVocals;
            radioHarms.Checked = MainForm.doMIDIHarmonies;
            radioNoVocals.Checked = MainForm.doMIDINoVocals;
            radioKeys.Checked = MainForm.doMIDIKeys;
            radioProKeys.Checked = MainForm.doMIDIProKeys;
            radioNoKeys.Checked = MainForm.doMIDINoKeys;
            cboSizing.SelectedIndex = MainForm.NoteSizingType;
            chkNameTracks.Checked = MainForm.doMIDINameTracks;
            chkNameVocals.Checked = MainForm.doMIDINameVocals;
            chkNameProKeys.Checked = MainForm.doMIDINameProKeys;
            cboWindow.SelectedIndex = (int)MainForm.PlaybackWindow - 1;
            chkHighlightSolos.Checked = MainForm.doMIDIHighlightSolos;
            chkBWKeys.Checked = MainForm.doMIDIBWKeys;
            chkHarmColorOnVocals.Checked = MainForm.doMIDIHarm1onVocals;
        }

        private void CheckAll(bool enabled)
        {
            chkDrums.Checked = enabled;
            chkBass.Checked = enabled;
            chkGuitar.Checked = enabled;
            radioHarms.Checked = enabled;
            radioProKeys.Checked = enabled;
            if (enabled) return;
            radioKeys.Checked = false;
            radioVocals.Checked = false;
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            CheckAll(true);
            UpdateMIDITracks();
        }

        private void btnNone_Click(object sender, EventArgs e)
        {
            CheckAll(false);
            UpdateMIDITracks();
        }
        
        private void UpdateMIDITracks()
        {
            MainForm.ClearVisuals();
            MainForm.doMIDIDrums = chkDrums.Checked;
            MainForm.doMIDIBass = chkBass.Checked;
            MainForm.doMIDIGuitar = chkGuitar.Checked;
            MainForm.doMIDIVocals = radioVocals.Checked;
            MainForm.doMIDIHarmonies = radioHarms.Checked;
            MainForm.doMIDIKeys = radioKeys.Checked;
            MainForm.doMIDIProKeys = radioProKeys.Checked;
        }

        private void chkDrums_MouseUp(object sender, MouseEventArgs e)
        {
            UpdateMIDITracks();
        }

        private void chkNameTracks_CheckedChanged(object sender, EventArgs e)
        {
            MainForm.doMIDINameTracks = chkNameTracks.Checked;
        }

        private void cboSizing_SelectedIndexChanged(object sender, EventArgs e)
        {
            MainForm.NoteSizingType = cboSizing.SelectedIndex >= 0 ? cboSizing.SelectedIndex : 0;
        }

        private void cboWindow_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboWindow.SelectedIndex == -1)
            {
                MainForm.PlaybackWindow = 3.0;//default
            }
            else
            {
                MainForm.PlaybackWindow = cboWindow.SelectedIndex + 1.0;
            }
        }

        private void chkNameVocals_CheckedChanged(object sender, EventArgs e)
        {
            MainForm.doMIDINameVocals = chkNameVocals.Checked;
        }

        private void chkNameProKeys_CheckedChanged(object sender, EventArgs e)
        {
            MainForm.doMIDINameProKeys = chkNameProKeys.Checked;
        }

        private void chkHighlightSolos_CheckedChanged(object sender, EventArgs e)
        {
            MainForm.doMIDIHighlightSolos = chkHighlightSolos.Checked;
        }

        private void MIDISelector_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Dispose();
            }
        }       

        private void btnSave_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void chkBWKeys_CheckedChanged(object sender, EventArgs e)
        {
            MainForm.doMIDIBWKeys = chkBWKeys.Checked;
            MainForm.ClearNoteColors(false, true);
        }

        private void chkHarmColorOnVocals_CheckedChanged(object sender, EventArgs e)
        {
            MainForm.doMIDIHarm1onVocals = chkHarmColorOnVocals.Checked;
            MainForm.ClearNoteColors(true);
        }

        private void radioProKeys_CheckedChanged(object sender, EventArgs e)
        {
            MainForm.doMIDIProKeys = radioProKeys.Checked;
            MainForm.doMIDIKeys = radioKeys.Checked;
            MainForm.doMIDINoKeys = radioNoKeys.Checked;
        }

        private void radioHarms_CheckedChanged(object sender, EventArgs e)
        {
            MainForm.doMIDIHarmonies = radioHarms.Checked;
            MainForm.doMIDIVocals = radioVocals.Checked;
            MainForm.doMIDINoVocals = radioNoVocals.Checked;
        }        
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace cPlayer
{
    public partial class frmFilters : Form
    {

        public frmMain mainForm;

        public frmFilters(frmMain main)
        {
            InitializeComponent();
            ApplyRoundedRegion(15);
            mainForm = main;
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

        private void LoadGenres()
        {
            List<string> allGenres = new List<string>() {
                "(All Genres)",
                "Alternative",
                "Blues",
                "Classical",
                "Classic Rock",
                "Country",
                "Emo",
                "Fusion",
                "Glam",
                "Grunge",
                "Hip-Hop/Rap",
                "Indie Rock",
                "Inspirational",
                "Jazz",
                "J-Rock",
                "Latin",
                "Metal",
                "New Wave",
                "Novelty",
                "Nu-Metal",
                "Pop/Dance/Electronic",
                "Pop-Rock",
                "Prog",
                "Punk",
                "R&B/Soul/Funk",
                "Reggae/Ska",
                "Rock",
                "Southern Rock",
                "World",
                "Other"
            };
                        
            lstGenres.Items.Clear();

            foreach (var genre in allGenres) // List<string>
            {
                var item = new ListViewItem(genre)
                {
                    Checked = false
                };
                lstGenres.Items.Add(item);
            }

            var allItem = lstGenres.Items[0];
            allItem.Focused = true;
            allItem.Checked = true;
            allItem.EnsureVisible();
        }

        private void LoadInstruments()
        {
            List<string> allInstruments = new List<string>() {
                "(Any Instrument)",
                "Bass",
                "Drums",
                "Guitar",
                "Keys",
                "Pro Keys",
                "Vocals",
                "2x Harmonies",
                "3x Harmonies",                
            };

            lstInstruments.Items.Clear();

            foreach (var instrument in allInstruments) // List<string>
            {
                var item = new ListViewItem(instrument)
                {
                    Checked = false
                };
                lstInstruments.Items.Add(item);
            }

            var anyItem = lstInstruments.Items[0];
            anyItem.Focused = true;
            anyItem.Checked = true;
            anyItem.EnsureVisible();
        }

        private void LoadLanguages()
        {
            List<string> allLanguages = new List<string>() {
                "(Any Language)",
                "English",
                "French",
                "German",
                "Italian",
                "Japanese",
                "Spanish",
                "Unknown",
            };

            lstLanguages.Items.Clear();

            foreach (var language in allLanguages) // List<string>
            {
                var item = new ListViewItem(language)
                {
                    Checked = false
                };
                lstLanguages.Items.Add(item);
            }

            var anyItem = lstLanguages.Items[0];
            anyItem.Focused = true;
            anyItem.Checked = true;
            anyItem.EnsureVisible();
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            string genreText = string.Join(" | ",
            lstGenres.Items.Cast<ListViewItem>().Where(i => i.Checked).Select(i => i.Text));

            if (genreText.Contains("(All Genres"))
            {
                genreText = "";
            }
            mainForm.genreFilter = genreText;

            string instrumentText = string.Join(" | ",
            lstInstruments.Items.Cast<ListViewItem>().Where(i => i.Checked).Select(i => i.Text));

            if (instrumentText.Contains("(Any Instrument"))
            {
                instrumentText = "";
            }
            mainForm.instrumentFilter = instrumentText;

            string languageText = string.Join(" | ",
            lstLanguages.Items.Cast<ListViewItem>().Where(i => i.Checked).Select(i => i.Text));

            if (languageText.Contains("(Any Language"))
            {
                languageText = "";
            }
            mainForm.languageFilter = languageText;

            mainForm.ReloadPlaylist(mainForm.Playlist, true, true, false);
            Close();
        }

        private void lstGenres_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            try
            {
                // Index 0 = "All Genres"
                if (e.Item.Index == 0 && e.Item.Checked)
                {
                    // "All Genres" checked → uncheck everything else
                    for (int i = 1; i < lstGenres.Items.Count; i++)
                    {
                        lstGenres.Items[i].Checked = false;
                        lstGenres.Items[i].Selected = false;
                    }

                    return;
                }

                // Any real genre checked → uncheck "All Genres"
                if (e.Item.Index != 0 && e.Item.Checked)
                {
                    lstGenres.Items[0].Checked = false;
                    lstGenres.Items[0].Selected = false;
                }
            }
            catch { }
        }

        private void frmGenres_Shown(object sender, System.EventArgs e)
        {
            lstGenres.BeginUpdate();

            LoadGenres();
            foreach (ListViewItem item in lstGenres.Items)
                item.Checked = false;

            // Blank/null => All Genres
            if (string.IsNullOrWhiteSpace(mainForm.genreFilter))
            {
                if (lstGenres.Items.Count > 0)
                    lstGenres.Items[0].Checked = true; // "All Genres"                
            }

            if (!string.IsNullOrWhiteSpace(mainForm.genreFilter))
            {
                // Parse "Rock | Jazz | Blues"
                var selected = new HashSet<string>(
                    mainForm.genreFilter.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries)
                               .Select(s => s.Trim()),
                    StringComparer.OrdinalIgnoreCase
                );

                // Check matching items (skip index 0)
                for (int i = 1; i < lstGenres.Items.Count; i++)
                {
                    var item = lstGenres.Items[i];
                    if (selected.Contains(item.Text))
                        item.Checked = true;
                }
            }
                        
            bool anyChecked = lstGenres.Items.Cast<ListViewItem>().Any(i => i.Checked && i.Index != 0);
            if (!anyChecked && lstGenres.Items.Count > 0)
                lstGenres.Items[0].Checked = true;

            lstGenres.EndUpdate();

            lstInstruments.BeginUpdate();

            LoadInstruments();
            foreach (ListViewItem item in lstInstruments.Items)
                item.Checked = false;

            // Blank/null => Any Instrument
            if (string.IsNullOrWhiteSpace(mainForm.instrumentFilter))
            {
                if (lstInstruments.Items.Count > 0)
                    lstInstruments.Items[0].Checked = true; // "Any Instrument"
            }

            if (!string.IsNullOrWhiteSpace(mainForm.instrumentFilter))
            {
                // Parse
                var selectedInstruments = new HashSet<string>(
                    mainForm.instrumentFilter.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries)
                               .Select(s => s.Trim()),
                    StringComparer.OrdinalIgnoreCase
                );

                // Check matching items (skip index 0)
                for (int i = 1; i < lstInstruments.Items.Count; i++)
                {
                    var item = lstInstruments.Items[i];
                    if (selectedInstruments.Contains(item.Text))
                        item.Checked = true;
                }
            }

            bool anyInstrument = lstInstruments.Items.Cast<ListViewItem>().Any(i => i.Checked && i.Index != 0);
            if (!anyInstrument && lstInstruments.Items.Count > 0)
                lstInstruments.Items[0].Checked = true;

            lstInstruments.EndUpdate();

            lstLanguages.BeginUpdate();

            LoadLanguages();
            foreach (ListViewItem item in lstLanguages.Items)
                item.Checked = false;

            // Blank/null => Any language
            if (string.IsNullOrWhiteSpace(mainForm.languageFilter))
            {
                if (lstLanguages.Items.Count > 0)
                    lstLanguages.Items[0].Checked = true; // "Any language"
            }

            if (!string.IsNullOrWhiteSpace(mainForm.languageFilter))
            {
                // Parse
                var selectedLanguages = new HashSet<string>(
                    mainForm.languageFilter.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries)
                               .Select(s => s.Trim()),
                    StringComparer.OrdinalIgnoreCase
                );

                // Check matching items (skip index 0)
                for (int i = 1; i < lstLanguages.Items.Count; i++)
                {
                    var item = lstLanguages.Items[i];
                    if (selectedLanguages.Contains(item.Text))
                        item.Checked = true;
                }
            }

            bool anyLanguage = lstLanguages.Items.Cast<ListViewItem>().Any(i => i.Checked && i.Index != 0);
            if (!anyLanguage && lstLanguages.Items.Count > 0)
                lstLanguages.Items[0].Checked = true;

            lstLanguages.EndUpdate();
        }

        private void lstInstruments_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            try
            {
                // Index 0 = "Any Instrument"
                if (e.Item.Index == 0 && e.Item.Checked)
                {
                    // "Any Instrument" checked → uncheck everything else
                    for (int i = 1; i < lstInstruments.Items.Count; i++)
                    {
                        lstInstruments.Items[i].Checked = false;
                        lstInstruments.Items[i].Selected = false;
                    }

                    return;
                }

                // Any real instrument checked → uncheck "Any Instrument"
                if (e.Item.Index != 0 && e.Item.Checked)
                {
                    lstInstruments.Items[0].Checked = false;
                    lstInstruments.Items[0].Selected = false;
                }

                //this works in theory but I don't have a way to detect Harm2/Harm3 without getting into the MIDI file, it's not in the DTA/INI file
                /*var vocalsIndex = lstInstruments.Items.Count - 3;
                if (e.Item.Index == vocalsIndex && e.Item.Checked) //Solo Vocals
                {
                    lstInstruments.Items[vocalsIndex + 1].Checked = false; //harm2
                    lstInstruments.Items[vocalsIndex + 1].Selected = false; //harm2
                    lstInstruments.Items[vocalsIndex + 2].Checked = false; //harm3
                    lstInstruments.Items[vocalsIndex + 2].Selected = false; //harm3
                }
                else if (e.Item.Index == vocalsIndex + 1 && e.Item.Checked)//Harm2
                {
                    lstInstruments.Items[vocalsIndex].Checked = false; //vocals
                    lstInstruments.Items[vocalsIndex].Selected = false; //vocals
                    lstInstruments.Items[vocalsIndex + 2].Checked = false; //harm3
                    lstInstruments.Items[vocalsIndex + 2].Selected = false; //harm3
                }
                else if (e.Item.Index == vocalsIndex + 2 && e.Item.Checked)//Harm3
                {
                    lstInstruments.Items[vocalsIndex].Checked = false; //vocals
                    lstInstruments.Items[vocalsIndex].Selected = false; //vocals
                    lstInstruments.Items[vocalsIndex + 1].Checked = false; //harm2
                    lstInstruments.Items[vocalsIndex + 1].Selected = false; //harm2
                }*/
            }
            catch { }
        }

        private void lstLanguages_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            try
            {
                // Index 0 = "Any Language"
                if (e.Item.Index == 0 && e.Item.Checked)
                {
                    // "Any Language" checked → uncheck everything else
                    for (int i = 1; i < lstLanguages.Items.Count; i++)
                    {
                        lstLanguages.Items[i].Checked = false;
                        lstLanguages.Items[i].Selected = false;
                    }

                    return;
                }

                // Any real Language checked → uncheck "Any Language"
                if (e.Item.Index != 0 && e.Item.Checked)
                {
                    lstLanguages.Items[0].Checked = false;
                    lstLanguages.Items[0].Selected = false;
                }
            }
            catch { }
        }
    }
}

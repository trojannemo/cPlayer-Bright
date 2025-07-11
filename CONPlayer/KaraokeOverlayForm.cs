﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace cPlayer
{
    public partial class KaraokeOverlayForm : Form
    {
        private frmMain mainForm;

        public KaraokeOverlayForm(frmMain MainForm)
        {
            this.FormBorderStyle = FormBorderStyle.None; // No borders
            this.BackColor = Color.Black; // Background color to make transparent
            this.TransparencyKey = Color.Black; // Make the background transparent
            this.StartPosition = FormStartPosition.Manual; // Custom positioning
            this.TopMost = true; // Always on top
            this.ShowInTaskbar = false; // Don't show in the taskbar
            this.Visible = false;
            mainForm = MainForm;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            this.UpdateStyles();
        }          

        public IList<LyricPhrase> Phrases { get; set; }
        public IEnumerable<Lyric> Lyrics { get; set; }
        public double CorrectedTime { get; set; }
        public Color KaraokeBackgroundColor { get; set; } = Color.Transparent; // Background color for karaoke text

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Do nothing to maintain transparency
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x08000000; // WS_EX_NOACTIVATE
                return cp;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (Phrases == null || Lyrics == null || CorrectedTime == 0)
                return;

            base.OnPaint(e);

            // Clear the previous drawing by filling with the transparent color
            e.Graphics.Clear(this.TransparencyKey);
            // Enable high-quality rendering
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            // Call the karaoke drawing method
            DoKaraokeMode(e.Graphics, Phrases, Lyrics);
        }

        private void DoKaraokeMode(Graphics graphics, IList<LyricPhrase> phrases, IEnumerable<Lyric> lyrics)
        {
            var time = mainForm.GetCorrectedTime();
            LyricPhrase currentLine = null;
            LyricPhrase nextLine = null;
            LyricPhrase lastLine = null;
            //get active and next phrase, and store last used phrase
            for (var i = 0; i < phrases.Count(); i++)
            {
                var phrase = phrases[i];
                if (string.IsNullOrEmpty(phrase.PhraseText)) continue;
                if (phrase.PhraseEnd < time)
                {
                    lastLine = phrases[i];
                    continue;
                }
                if (phrase.PhraseStart > time)
                {
                    nextLine = phrases[i];
                    break;
                }
                currentLine = phrase;
                if (i < phrases.Count - 1)
                {
                    nextLine = phrases[i + 1];
                }
                break;
            }
            var currentLineTop = mainForm.GetKaraokeCurrentLineTop();
            var nextLineTop = mainForm.GetKaraokeNextLineTop();
            string lineText;
            Font lineFont;
            Size lineSize;
            int posX;
            if (currentLine != null && !string.IsNullOrEmpty(currentLine.PhraseText))
            {
                //draw entire current phrase on top
                lineText = mainForm.ProcessLine(currentLine.PhraseText, true).Replace("‿", " ");
                lineFont = new Font("Tahoma", mainForm.GetScaledFontSize(graphics, lineText, new Font("Tahoma", (float)12.0), 120));
                lineSize = TextRenderer.MeasureText(lineText, lineFont);
                posX = (Width - lineSize.Width) / 2;
                TextRenderer.DrawText(graphics, lineText, lineFont, new Point(posX, currentLineTop), mainForm.KaraokeModeText, KaraokeBackgroundColor);

                //draw portion of current phrase that's already been sung
                var line2 = lyrics.Where(lyr => !(lyr.LyricStart < currentLine.PhraseStart)).TakeWhile(lyr => !(lyr.LyricStart > time)).Aggregate("", (current, lyr) => current + " " + lyr.LyricText);
                line2 = mainForm.ProcessLine(line2, true).Replace("‿", " ");
                if (!string.IsNullOrEmpty(line2))
                {
                    TextRenderer.DrawText(graphics, line2, lineFont, new Point(posX, currentLineTop), mainForm.KaraokeModeHighlight, KaraokeBackgroundColor);
                }

                var lyricsList = lyrics.ToList();
                var wordList = new List<ActiveWord>();
                if (currentLine.PhraseStart <= time - 0.1)
                {
                    var word = "";
                    double wordStart = 0, wordEnd = 0;
                    var activeWord = new ActiveWord(word, wordStart, wordEnd);

                    for (int i = 0; i < lyricsList.Count(); i++)
                    {
                        var lyric = lyricsList[i];

                        // Skip lyrics outside the proper time
                        if (lyric.LyricStart < currentLine.PhraseStart || lyric.LyricStart > currentLine.PhraseEnd)
                        {
                            continue;
                        }
                        if (string.IsNullOrEmpty(word))
                        {
                            wordStart = lyric.LyricStart;
                        }
                        if (lyric.LyricText.Contains("-")) //is a syllable
                        {
                            word += mainForm.ProcessLine(lyric.LyricText, true);
                            wordEnd = lyric.LyricStart + lyric.LyricDuration;
                            continue;
                        }
                        // Handle sustains
                        else if (!string.IsNullOrEmpty(word) && lyric.LyricText.Contains("+"))
                        {
                            //word += "+";
                            wordEnd = lyric.LyricStart + lyric.LyricDuration;

                            // Extend for consecutive sustains
                            for (var a = i + 1; a < lyricsList.Count; a++)
                            {
                                if (lyricsList[a].LyricText.Contains("+"))
                                {
                                    //word += "+"; // Append the sustain
                                    wordEnd = lyricsList[a].LyricStart + lyricsList[a].LyricDuration;
                                    i = a; // Update `i` to skip processed sustain notes
                                }
                                else
                                {
                                    break; // Exit sustain processing
                                }
                            }
                            continue; // Continue to process the next lyric
                        }
                        else
                        {
                            // Append regular lyrics to the word
                            word += mainForm.ProcessLine(lyric.LyricText, true).Replace("‿", " ");
                            wordEnd = lyric.LyricStart + lyric.LyricDuration;

                            //look ahead to double check next lyric(s) aren't + sustains
                            for (var z = i + 1; z < lyricsList.Count - i - 1; z++)
                            {
                                if (lyricsList[z].LyricText.Contains("+"))
                                {
                                    //word += "+"; // Append the sustain
                                    wordEnd = lyricsList[z].LyricStart + lyricsList[z].LyricDuration;
                                    i = z;
                                    continue;
                                }
                                else
                                {
                                    i = z - 1;
                                    break;
                                }
                            }

                            // Finalize the word if it’s not a middle syllable
                            if (!string.IsNullOrEmpty(word))
                            {
                                wordList.Add(new ActiveWord(word.Trim(), wordStart, wordEnd));
                                word = ""; // Reset the word
                            }
                        }
                    }

                    // Find the active word matching playback time
                    activeWord = wordList.FirstOrDefault(w => w.WordStart <= time && w.WordEnd > time);

                    if (activeWord != null && !string.IsNullOrEmpty(activeWord.Text))
                    {
                        activeWord.Text = activeWord.Text.Replace("‿", " ");

                        // Measure the word size for centering
                        lineFont = new Font("Tahoma", mainForm.GetScaledFontSize(graphics, activeWord.Text, new Font("Tahoma", (float)12.0), 200));
                        lineSize = TextRenderer.MeasureText(activeWord.Text, lineFont);
                        posX = (Width - lineSize.Width) / 2;
                        var posY = (Height - lineSize.Height) / 2;

                        // Draw the entire word in white
                        TextRenderer.DrawText(graphics, activeWord.Text, lineFont, new Point(posX, posY), mainForm.KaraokeModeText, KaraokeBackgroundColor);

                        // Calculate progress for the sung portion
                        var timeElapsed = time - activeWord.WordStart;
                        var progress = mainForm.Clamp((float)(timeElapsed / (activeWord.WordEnd - activeWord.WordStart)), 0.0f, 1.0f);

                        // Determine the portion of the word to highlight
                        var numCharsToHighlight = (int)Math.Ceiling(progress * activeWord.Text.Length); // Ensure rounding up
                        numCharsToHighlight = Math.Min(numCharsToHighlight, activeWord.Text.Length);

                        // Extract the sung portion
                        var sungPortion = activeWord.Text.Substring(0, numCharsToHighlight);

                        // Overlay the sung portion in blue
                        if (!string.IsNullOrEmpty(sungPortion))
                        {
                            TextRenderer.DrawText(graphics, sungPortion, lineFont, new Point(posX, posY), mainForm.KaraokeModeHighlight, KaraokeBackgroundColor);
                        }
                    }
                }
            }
            if (nextLine != null && !string.IsNullOrEmpty(nextLine.PhraseText))
            {
                //draw entire next phrase on bottom
                lineText = mainForm.ProcessLine(nextLine.PhraseText, true).Replace("‿", " ");
                lineFont = new Font("Tahoma", mainForm.GetScaledFontSize(graphics, lineText, new Font("Tahoma", (float)12.0), 120));
                lineSize = TextRenderer.MeasureText(lineText, lineFont);
                posX = (Width - lineSize.Width) / 2;
                TextRenderer.DrawText(graphics, lineText, lineFont, new Point(posX, nextLineTop - lineSize.Height), mainForm.KaraokeModeText, KaraokeBackgroundColor);
            }

            //draw waiting/countdown info
            if (currentLine != null && nextLine != null) return;
            if (lastLine != null && nextLine != null)
            {
                var difference = nextLine.PhraseStart - lastLine.PhraseEnd;
                if (difference < 5) return;
            }
            var middleText = "";
            var textColor = mainForm.KaraokeModeText;
            if (currentLine == null && nextLine != null)
            {
                var wait = nextLine.PhraseStart - time;
                if (wait < 1.5) return;
                middleText = wait <= 5 ? "[GET READY]" : "[WAIT: " + ((int)(wait + 0.5)) + "]";
                textColor = wait <= 5 ? mainForm.KaraokeModeHighlight : mainForm.KaraokeModeText;// Color.FromArgb(185, 216, 76) : Color.FromArgb(255, 187, 52);
            }
            else if (currentLine == null)
            {
                middleText = "[fin]";
            }
            lineFont = new Font("Tahoma", mainForm.GetScaledFontSize(graphics, middleText, new Font("Tahoma", (float)12.0), 200));
            lineSize = TextRenderer.MeasureText(middleText, lineFont);
            posX = (Width - lineSize.Width) / 2;
            TextRenderer.DrawText(graphics, middleText, lineFont, new Point(posX, (Height - lineSize.Height) / 2), textColor, KaraokeBackgroundColor);
        }
    }
}

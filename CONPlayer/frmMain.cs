﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using cPlayer.Properties;
using cPlayer.x360;
using Microsoft.VisualBasic;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Mix;
using Un4seen.Bass.Misc;
using Un4seen.Bass.AddOn.Opus;
using WMPLib;
using AxWMPLib;
using NautilusFREE;
using static cPlayer.YARGSongFileStream;
using Un4seen.Bass.AddOn.Enc;
using System.Runtime.InteropServices;
using NAudio.Wave;
using cPlayer.StageKit;
using SlimDX.XInput;
using NAudio.Dsp;

namespace cPlayer
{
    public partial class frmMain : Form
    {
        private readonly Color ChartOrange = Color.FromArgb(255, 126, 0);
        private readonly Color ChartBlue = Color.FromArgb(0, 0, 255);
        private readonly Color ChartYellow = Color.FromArgb(242, 226, 0);
        private readonly Color ChartRed = Color.FromArgb(255, 0, 0);
        private readonly Color ChartGreen = Color.FromArgb(0, 255, 0);
        private readonly Color Harm1Color = Color.FromArgb(29, 163, 201);
        private readonly Color Harm2Color = Color.FromArgb(227, 144, 24);
        private readonly Color Harm3Color = Color.FromArgb(168, 74, 4);
        private readonly Color LabelBackgroundColor = Color.FromArgb(127, 40, 40, 40);
        private readonly Color TrackBackgroundColor1 = Color.FromArgb(40, 40, 40);
        private readonly Color TrackBackgroundColor2 = Color.FromArgb(80, 80, 80);
        private readonly Color RBStyleVocalsBackgroundColor = Color.FromArgb(127, 0, 0, 0);
        private readonly Color KaraokeBackgroundColor = Color.Transparent;
        public double VolumeLevel = 12.5;
        private string PlayerConsole = "xbox";
        private double FadeLength = 1.0;
        public bool doAudioDrums = true;
        public bool doAudioBass = true;
        public bool doAudioGuitar = true;
        public bool doAudioKeys = true;
        public bool doAudioVocals = true;
        public bool doAudioBacking = true;
        public bool doAudioCrowd = false;
        public bool doMIDIDrums = true;
        public bool doMIDIBass = true;
        public bool doMIDIGuitar = true;
        public bool doMIDIProKeys = true;
        public bool doMIDIKeys = false;
        public bool doMIDIVocals = false;
        public bool doMIDIHarmonies = true;
        public bool doMIDINameVocals = false;
        public bool doMIDINameProKeys = false;
        public bool doStaticLyrics = false;
        public bool doScrollingLyrics = false;
        public bool doKaraokeLyrics = true;
        public bool doWholeWordsLyrics = true;
        public bool doHarmonyLyrics = true;
        public bool doMIDINameTracks = true;
        public bool doMIDIHighlightSolos = true;
        public bool doMIDIBWKeys = false;
        public bool doMIDIHarm1onVocals = false;
        private readonly Visuals Spectrum = new Visuals();
        public double PlaybackWindow = 3.0;
        private readonly double PlaybackWindowRB = 2.0;
        private readonly double PlaybackWindowRBVocals = 5.0;
        public int NoteSizingType = 0;
        private const string AppName = "cPlayer (Bright Edition)";
        private const int BassBuffer = 1000;
        private readonly NemoTools Tools;
        private readonly DTAParser Parser;
        private int mouseX;
        private int mouseY;
        private string SongToLoad;
        private List<Song> StaticPlaylist;
        private List<Song> Playlist;
        private string PlaylistPath;
        private string PlaylistName;
        private Song ActiveSong;
        private Song PlayingSong;
        private Song NextSong;
        private byte[] CurrentSongAudio;
        private string CurrentSongAudioPath;
        private string CurrentSongArt;
        private string CurrentSongArtBlurred;
        private string CurrentSongMIDI;
        private string NextSongArtPNG;
        private string NextSongArtJPG;
        private string NextSongArtBlurred;
        private string NextSongMIDI;
        public double PlaybackSeconds;
        private double PlaybackSeek;
        private bool reset;
        private readonly string config;
        private int NextSongIndex;
        private int StartingCount;
        private bool isScanning;
        private readonly MIDIStuff MIDITools;
        private Graphics Chart;
        private Bitmap ChartBitmap;
        private readonly string TempFolder;
        public bool CancelWorkers;
        private readonly string EXE;
        private readonly string[] RecentPlaylists;
        private int BassMixer;
        private int BassStream;
        private readonly List<int> BassStreams;
        private int SpectrumID;
        private bool VideoIsPlaying;
        public List<PracticeSection> PracticeSessions;
        private string ImgToUpload;
        private string ImgURL;
        private bool showUpdateMessage;
        private bool AlreadyTried;
        private bool DrewFullChart;
        private double IntroSilence;
        private double OutroSilence;
        private double IntroSilenceNext;
        private double OutroSilenceNext;
        private float SilenceThreshold = 0.25f;
        private bool AlreadyFading;
        private PlaylistSorting SortingStyle;
        private bool isClosing;
        private bool ShowingNotFoundMessage;
        private readonly nTools nautilus;
        private SongData ActiveSongData;
        private AxWindowsMediaPlayer MediaPlayer;
        private string[] opusFiles;
        private string[] oggFiles;
        private string[] mp3Files;
        private string[] wavFiles;
        private string[] cltFiles;
        private string[] m4aFiles;
        private string currentKLIC;
        private string pkgPath;
        private string sngPath;
        private string oggPath;
        private string psarcPath;
        private string ghwtPath;
        public bool isChoosingStems;
        private bool hasNoMIDI;
        private int overrideSongLength;
        private string XML_PATH;
        private string XMA_EXT_PATH;
        private string XMA_PATH;
        private string BandFusePath;
        private readonly NemoFnFParser fnfParser;
        public bool isPlayingM4A;
        public string activeM4AFile;
        private KaraokeOverlayForm KaraokeOverlay;
        private gifOverlay GIFOverlay;
        private const string strSearchPlaylist = "Type to search playlist...";
        private const int KICK_HEIGHT = 6;
        private int ChartGoal = 630;
        private const int vocalsHeight = 160;
        private const double MinVolume = 50;
        private readonly Bitmap bmpDrumsCymbalB;
        private readonly Bitmap bmpDrumsCymbalY;
        private readonly Bitmap bmpDrumsCymbalG;
        private readonly Bitmap bmpDrumsCymbalOD;
        private readonly Bitmap bmpNoteBlue;
        private readonly Bitmap bmpNoteGreen;
        private readonly Bitmap bmpNoteYellow;
        private readonly Bitmap bmpNoteRed;
        private readonly Bitmap bmpNoteOrange;
        private readonly Bitmap bmpNoteOD;
        private readonly Bitmap bmpProKeysNoteWhite;
        private readonly Bitmap bmpProKeysNoteWhiteOD;
        private readonly Bitmap bmpProKeysNoteBlack;
        private readonly Bitmap bmpProKeysNoteBlackOD;
        private readonly Bitmap bmpBackgroundDrums;
        private readonly Bitmap bmpBackgroundDrumsSolo;
        private readonly Bitmap bmpBackgroundBass;
        private readonly Bitmap bmpBackgroundBassSolo;
        private readonly Bitmap bmpBackgroundGuitar;
        private readonly Bitmap bmpBackgroundGuitarSolo;
        private readonly Bitmap bmpBackgroundKeys;
        private readonly Bitmap bmpBackgroundKeysSolo;
        private readonly Bitmap bmpBackgroundProKeys;
        private readonly Bitmap bmpBackgroundProKeysSolo;
        private readonly Bitmap bmpHitbox;
        private readonly Bitmap bmpHitboxVocals;
        private readonly Bitmap bmpBackgroundVocals;
        private readonly Bitmap bmpBackgroundLyrics;
        private readonly Bitmap bmpProKeysChordMarker;
        private const int HitboxVocalsX = 200;
        private WaveInEvent waveIn;
        private WaveOutEvent waveOut;
        private BufferedWaveProvider bufferedWaveProvider;
        public VolumeWaveProvider16 volumeProvider;
        public int microphoneIndex = -1;
        private StageKitController stageKit;
        private LedDisplay ledDisplay;
        private Rectangle skDrums;
        private Rectangle skBass;
        private Rectangle skGuitar;
        private Rectangle skKeys;
        private Rectangle skProKeys;
        private Instrument skActiveInstrument;
        private LED redLED1 = new LED() { Index = 0, Color = LEDColor.Red };
        private LED redLED2 = new LED() { Index = 1, Color = LEDColor.Red };
        private LED redLED3 = new LED() { Index = 2, Color = LEDColor.Red };
        private LED redLED4 = new LED() { Index = 3, Color = LEDColor.Red };
        private LED redLED5 = new LED() { Index = 4, Color = LEDColor.Red };
        private LED redLED6 = new LED() { Index = 5, Color = LEDColor.Red };
        private LED redLED7 = new LED() { Index = 6, Color = LEDColor.Red };
        private LED redLED8 = new LED() { Index = 7, Color = LEDColor.Red };
        private LED yellowLED1 = new LED() { Index = 0, Color = LEDColor.Yellow };
        private LED yellowLED2 = new LED() { Index = 1, Color = LEDColor.Yellow };
        private LED yellowLED3 = new LED() { Index = 2, Color = LEDColor.Yellow };
        private LED yellowLED4 = new LED() { Index = 3, Color = LEDColor.Yellow };
        private LED yellowLED5 = new LED() { Index = 4, Color = LEDColor.Yellow };
        private LED yellowLED6 = new LED() { Index = 5, Color = LEDColor.Yellow };
        private LED yellowLED7 = new LED() { Index = 6, Color = LEDColor.Yellow };
        private LED yellowLED8 = new LED() { Index = 7, Color = LEDColor.Yellow };
        private LED greenLED1 = new LED() { Index = 0, Color = LEDColor.Green };
        private LED greenLED2 = new LED() { Index = 1, Color = LEDColor.Green };
        private LED greenLED3 = new LED() { Index = 2, Color = LEDColor.Green };
        private LED greenLED4 = new LED() { Index = 3, Color = LEDColor.Green };
        private LED greenLED5 = new LED() { Index = 4, Color = LEDColor.Green };
        private LED greenLED6 = new LED() { Index = 5, Color = LEDColor.Green };
        private LED greenLED7 = new LED() { Index = 6, Color = LEDColor.Green };
        private LED greenLED8 = new LED() { Index = 7, Color = LEDColor.Green };
        private LED blueLED1 = new LED() { Index = 0, Color = LEDColor.Blue };
        private LED blueLED2 = new LED() { Index = 1, Color = LEDColor.Blue };
        private LED blueLED3 = new LED() { Index = 2, Color = LEDColor.Blue };
        private LED blueLED4 = new LED() { Index = 3, Color = LEDColor.Blue };
        private LED blueLED5 = new LED() { Index = 4, Color = LEDColor.Blue };
        private LED blueLED6 = new LED() { Index = 5, Color = LEDColor.Blue };
        private LED blueLED7 = new LED() { Index = 6, Color = LEDColor.Blue };
        private LED blueLED8 = new LED() { Index = 7, Color = LEDColor.Blue };
        private LED strobe = new LED() { Index = 0, Color = LEDColor.White };
        private readonly List<LED> LEDs;
        private int activeBlueLED = 0;
        private int activeYellowLED = 0;
        private int activeRedLED = 0;
        private int activeGreenLED = 0;

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        public frmMain()
        {
            InitializeComponent();
            Log("Initialized");
            EXE = "." + "e" + "x" + "e";
            Tools = new NemoTools();
            nautilus = new nTools();
            fnfParser = new NemoFnFParser();
            Parser = new DTAParser();
            SongsToAdd = new List<string>();
            Playlist = new List<Song>();
            StaticPlaylist = new List<Song>();
            MIDITools = new MIDIStuff();
            BassStreams = new List<int>();
            opusFiles = new string[20];
            oggFiles = new string[20];
            mp3Files = new string[20];
            m4aFiles = new string[20];
            RecentPlaylists = new string[5];
            PracticeSessions = new List<PracticeSection>();
            MediaPlayer = new AxWindowsMediaPlayer();
            MediaPlayer.CreateControl();
            // Handle the MouseDown event to show the context menu
            MediaPlayer.MouseDownEvent += (object sender, _WMPOCXEvents_MouseDownEvent e) =>
            {
                if (e.nButton == 2) // Right mouse button
                {
                    VisualsContextMenu.Show(Cursor.Position); // Show the context menu at the mouse position
                }
            };
            KaraokeOverlay = new KaraokeOverlayForm(this)
            {
                StartPosition = FormStartPosition.Manual,
                TopMost = true
            };
            UpdateOverlayPosition();
            KaraokeOverlay.Show();
            // Hook into relevant events to keep the overlay aligned
            this.Resize += (s, e) => UpdateOverlayPosition();
            this.Move += (s, e) => UpdateOverlayPosition();
            MediaPlayer.Resize += (s, e) => UpdateOverlayPosition();

            for (var i = 0; i < 5; i++)
            {
                RecentPlaylists[i] = "";
            }
            SetDefaultPaths();
            if (!Directory.Exists(Application.StartupPath + "\\playlists\\"))
            {
                Directory.CreateDirectory(Application.StartupPath + "\\playlists\\");
            }
            TempFolder = Application.StartupPath + "\\bin\\temp\\";
            config = Application.StartupPath + "\\bin\\player.config";
            DeleteUsedFiles();
            CreateHiddenFolder();
            ActiveSongData = new SongData();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            this.UpdateStyles();
            bmpDrumsCymbalB = Resources.drums_cymbal_b;
            bmpDrumsCymbalY = Resources.drums_cymbal_y;
            bmpDrumsCymbalG = Resources.drums_cymbal_g;
            bmpDrumsCymbalOD = Resources.drums_cymbal_od;
            bmpNoteBlue = Resources.note_blue;
            bmpNoteGreen = Resources.note_green;
            bmpNoteYellow = Resources.note_yellow;
            bmpNoteRed = Resources.note_red;
            bmpNoteOrange = Resources.note_orange;
            bmpNoteOD = Resources.note_od;
            bmpProKeysNoteWhite = Resources.note_white;
            bmpProKeysNoteWhiteOD = Resources.note_white_od;
            bmpProKeysNoteBlack = Resources.note_black;
            bmpProKeysNoteBlackOD = Resources.note_black_od;
            bmpBackgroundDrums = Resources.background_drums;
            bmpBackgroundDrumsSolo = Resources.background_drums_solo;
            bmpBackgroundBass = Resources.background_bass;
            bmpBackgroundBassSolo = Resources.background_bass_solo;
            bmpBackgroundGuitar = Resources.background_guitar;
            bmpBackgroundGuitarSolo = Resources.background_guitar_solo;
            bmpBackgroundKeys = Resources.background_keys;
            bmpBackgroundKeysSolo = Resources.background_keys_solo;
            bmpBackgroundProKeys = Resources.background_prokeys;
            bmpBackgroundProKeysSolo = Resources.background_prokeys_solo;
            bmpHitbox = Resources.hitbox;
            bmpHitboxVocals = Resources.hitbox_vocals;
            bmpBackgroundVocals = Resources.frostedglass50;
            bmpBackgroundLyrics = Resources.frostedglasslyrics25;
            bmpProKeysChordMarker = Resources.prokeyschord;
            ledDisplay = new LedDisplay();
            LEDs = new List<LED>() { redLED1, redLED2, redLED3, redLED4, redLED5, redLED6, redLED7, redLED8,
                                     greenLED1, greenLED2, greenLED3, greenLED4, greenLED5,greenLED6, greenLED7, greenLED8,
                                    yellowLED1, yellowLED2, yellowLED3, yellowLED4, yellowLED5, yellowLED6, yellowLED7, yellowLED8,
                                    blueLED1, blueLED2, blueLED3, blueLED4, blueLED5, blueLED6,blueLED7,blueLED8, strobe};
        }

        public void StartPassthrough(int deviceIndex, int volume)
        {
            try
            {
                // Initialize microphone input
                waveIn = new WaveInEvent
                {
                    DeviceNumber = deviceIndex,
                    WaveFormat = new WaveFormat(44100, 1), // 44.1kHz, mono
                    BufferMilliseconds = 10 // Reduce buffer size
                };
                waveIn.DataAvailable += WaveIn_DataAvailable;

                // Initialize buffered wave provider
                bufferedWaveProvider = new BufferedWaveProvider(waveIn.WaveFormat)
                {
                    BufferDuration = TimeSpan.FromSeconds(1), // Optional: Adjust as needed
                    DiscardOnBufferOverflow = true
                };

                // Initialize volume provider
                volumeProvider = new VolumeWaveProvider16(bufferedWaveProvider)
                {
                    Volume = volume / 100f // Set initial volume
                };

                // Initialize speaker output with reduced latency
                waveOut = new WaveOutEvent
                {
                    DesiredLatency = 50 // Lower playback latency
                };
                waveOut.Init(volumeProvider);

                // Start processing
                waveIn.StartRecording();
                waveOut.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting passthrough: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StopPassthrough();
            }
        }        

        public void StopPassthrough()
        {
            waveIn?.StopRecording();
            waveIn?.Dispose();
            waveIn = null;

            waveOut?.Stop();
            waveOut?.Dispose();
            waveOut = null;
        }

        private BiQuadFilter highPassFilter = BiQuadFilter.HighPassFilter(44100, 100, 1); // 100 Hz cutoff, Q factor 1
        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            // Apply the high-pass filter to each sample
            var buffer = new float[e.BytesRecorded / 2];
            for (int i = 0; i < buffer.Length; i++)
            {
                short sample = BitConverter.ToInt16(e.Buffer, i * 2);
                buffer[i] = highPassFilter.Transform(sample / 32768f) * 32768;
            }

            // Write processed samples to the buffer
            bufferedWaveProvider.AddSamples(e.Buffer, 0, e.BytesRecorded);
        }        

        private bool MonitorApplicationFocus()
        {
            IntPtr foregroundWindow = GetForegroundWindow();
            GetWindowThreadProcessId(foregroundWindow, out uint foregroundProcessId);
            uint currentProcessId = (uint)Process.GetCurrentProcess().Id;

            if (foregroundProcessId != currentProcessId)
            {
                // Application is not focused, hide the overlay if visible
                return false;
            }
            else //(foregroundProcessId == currentProcessId)
            {
                // Application is focused, show the overlay if hidden                    
                return true;
            }
        }

        private void UpdateOverlayPosition()
        {
            if (KaraokeOverlay != null && !KaraokeOverlay.IsDisposed)
            {
                KaraokeOverlay.Location = new Point(this.Left + picVisuals.Location.X, this.Top + picVisuals.Location.Y);
                KaraokeOverlay.Size = picVisuals.ClientSize;
            }
            if (GIFOverlay != null && !GIFOverlay.IsDisposed)
            {
                // Position the overlay so it is centered on the ListView
                GIFOverlay.Left = Left + panelPlaylist.Left + ((panelPlaylist.Width - GIFOverlay.Width) / 2);
                GIFOverlay.Top = Top + panelPlaylist.Top + ((panelPlaylist.Height - GIFOverlay.Height) / 2);
            }
        }

        private void SetDefaultPaths()
        {
            CurrentSongArt = Path.GetTempPath() + "play.png";
            CurrentSongArtBlurred = Path.GetTempPath() + "playb.png";
            CurrentSongMIDI = Path.GetTempPath() + "play.mid";
            NextSongArtPNG = Path.GetTempPath() + "next.png";
            NextSongArtJPG = Path.GetTempPath() + "next.jpg";
            NextSongArtBlurred = Path.GetTempPath() + "nextb.png";
            NextSongMIDI = Path.GetTempPath() + "next.mid";
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CreateHiddenFolder()
        {
            Tools.DeleteFolder(TempFolder, true);
            var di = Directory.CreateDirectory(TempFolder);
            di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
        }

        private void frmMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            Cursor = Cursors.NoMove2D;
            mouseX = MousePosition.X;
            mouseY = MousePosition.Y;
            if (!displayAudioSpectrum.Checked || PlayingSong == null) return;
            SpectrumID++;
            picVisuals.Image = null;
            Spectrum.ClearPeaks();
            Log("Changed audio spectrum visualization to #" + SpectrumID);
        }

        private void frmMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (Cursor != Cursors.NoMove2D)
            {
                if (stageKit != null && (skDrums.Contains(e.Location) || skBass.Contains(e.Location) || skGuitar.Contains(e.Location) ||
                    skKeys.Contains(e.Location) || skProKeys.Contains(e.Location)))
                {
                    Cursor = Cursors.Hand;
                }
                else
                {
                    Cursor = Cursors.Default;
                }
                return;
            }
            if (MousePosition.X != mouseX)
            {
                if (MousePosition.X > mouseX)
                {
                    Left = Left + (MousePosition.X - mouseX);
                }
                else if (MousePosition.X < mouseX)
                {
                    Left = Left - (mouseX - MousePosition.X);
                }
                mouseX = MousePosition.X;
            }

            if (MousePosition.Y == mouseY) return;
            if (MousePosition.Y > mouseY)
            {
                Top = Top + (MousePosition.Y - mouseY);
            }
            else if (MousePosition.Y < mouseY)
            {
                Top = Top - (mouseY - MousePosition.Y);
            }
            mouseY = MousePosition.Y;
        }

        private void frmMain_MouseUp(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void createNewPlaylist_Click(object sender, EventArgs e)
        {
            Log("createNewPlaylist_Click");
            StartNew(true);
        }

        private void StartNew(bool confirm)
        {
            if (Text.Contains("*") && confirm)
            {
                Log("There are unsaved changes. Confirm?");
                if (MessageBox.Show("You have unsaved changes on the current playlist\nAre you sure you want to do that?",
                        AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    Log("No");
                    return;
                }
                Log("Yes");
            }

            Tools.DeleteFile(CurrentSongArt);
            Tools.DeleteFile(CurrentSongArtBlurred);
            Tools.DeleteFile(CurrentSongMIDI);
            Tools.DeleteFile(NextSongArtPNG);
            Tools.DeleteFile(NextSongArtJPG);
            Tools.DeleteFile(NextSongArtBlurred);
            Tools.DeleteFile(NextSongMIDI);

            PlaylistPath = "";
            PlaylistName = "";
            Playlist = new List<Song>();
            lblUpdates.Text = "";
            lstPlaylist.Items.Clear();
            btnClear.PerformClick();
            ClearAll();
            ClearVisuals();
            ActiveSong = null;
            PlayingSong = null;
            Text = AppName;
            DeleteUsedFiles();
            Tools.DeleteFile(activeM4AFile);
            activeM4AFile = "";
            Log("Created new " + PlayerConsole + " playlist");
        }

        private void ClearAll()
        {
            reset = true;
            StopPlayback();
            MediaPlayer.Visible = false;
            picPreview.Image = Resources.noart3;
            picPreview.Cursor = Cursors.Default;
            lblSections.Invoke(new MethodInvoker(() => lblSections.Text = ""));
            lblSections.Invoke(new MethodInvoker(() => lblSections.Image = null));
            lblSections.Invoke(new MethodInvoker(() => lblSections.CreateGraphics().Clear(LabelBackgroundColor)));
            picVisuals.Invoke(new MethodInvoker(() => picVisuals.Image = null));
            toolTip1.SetToolTip(picPreview, "");
            toolTip1.SetToolTip(lblArtist, "");
            toolTip1.SetToolTip(lblSong, "");
            toolTip1.SetToolTip(lblAlbum, "");
            lblArtist.Text = "Artist:";
            lblSong.Text = "Song:";
            lblAlbum.Text = "Album:";
            lblGenre.Text = "Genre:";
            lblTrack.Text = "Track #:";
            lblYear.Text = "Year:";
            lblTime.Text = "0:00";
            lblDuration.Text = "0:00";
            lblAuthor.Text = "";
            panelSlider.Left = panelLine.Left;
            SongToLoad = "";
            EnableDisableButtons(false);
            PlaybackSeconds = 0;
            PlaybackTimer.Enabled = false;
            UpdateTime();
            panelSlider.Cursor = Cursors.Default;
            panelLine.Cursor = Cursors.Default;
            btnClear.PerformClick();
            PlayingSong = null;
            MIDITools.Initialize(true);
            AlreadyFading = false;
            reset = false;
        }

        private void lstPlaylist_DragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            Environment.CurrentDirectory = Path.GetDirectoryName(files[0]);
            Log("lstPlaylist_DragDrop " + files.Count() + " file(s)");
            if (files[0].EndsWith(".playlist", StringComparison.Ordinal))
            {
                Log("Drag/dropped playlist file " + files[0]);
                PrepareToLoadPlaylist(files[0]);
                return;
            }

            SongsToAdd = new List<string> { };

            if (xbox360.Checked || bandFuse.Checked)
            {
                SongsToAdd.AddRange(files.Where(file => VariousFunctions.ReadFileType(file) == XboxFileType.STFS).ToList());
            }
            else if (yarg.Checked)
            {
                SongsToAdd.AddRange(files.Where(file => Path.GetFileName(file) == "song.ini").ToList());
                SongsToAdd.AddRange(files.Where(file => Path.GetFileName(file) == "songs.dta").ToList());
                SongsToAdd.AddRange(files.Where(file => Path.GetExtension(file) == ".sng").ToList());
                SongsToAdd.AddRange(files.Where(file => Path.GetExtension(file) == ".yargsong").ToList());
            }
            else if (pS3.Checked)
            {
                SongsToAdd.AddRange(files.Where(file => Path.GetFileName(file) == "songs.dta").ToList());
                SongsToAdd.AddRange(files.Where(file => Path.GetExtension(file) == ".pkg").ToList());
            }
            else if (rockSmith.Checked)
            {
                SongsToAdd.AddRange(files.Where(file => Path.GetExtension(file) == ".psarc").ToList());
            }
            else if (wii.Checked)
            {
                SongsToAdd.AddRange(files.Where(file => Path.GetFileName(file) == "songs.dta").ToList());
            }
            else if (guitarHero.Checked)
            {
                SongsToAdd.AddRange(files.Where(file => Path.GetFileName(file) == "song.ini").ToList());
            }
            else if (fortNite.Checked)
            {
                SongsToAdd.AddRange(files.Where(file => Path.GetExtension(file) == ".fnf").ToList());
            }
            else if (powerGig.Checked)
            {
                SongsToAdd.AddRange(files.Where(file => Path.GetExtension(file) == ".xml").ToList());
            }

            if (!SongsToAdd.Any())
            {
                Log("Drag/dropped file(s) not valid");
                MessageBox.Show(files.Count() == 1 ? "That's not a valid file" : "Those are not valid files", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (batchSongLoader.IsBusy || songLoader.IsBusy)
            {
                Log("Background worker is busy, not adding song(s) now");
                MessageBox.Show("Please wait while I finish extracting the last file(s)", AppName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            btnClear.PerformClick();
            EnableDisable(false);
            StartingCount = lstPlaylist.Items.Count;
            Log("Songs to add: " + SongsToAdd.Count);
            Log("Starting batch song loader");
            InitiateGIFOverlay();
            batchSongLoader.RunWorkerAsync();
        }

        private void EnableDisable(bool enabled, bool hide = false)
        {
            fileToolStripMenuItem.Enabled = enabled && !isScanning;
            toolsToolStripMenuItem.Enabled = fileToolStripMenuItem.Enabled;
            optionsToolStripMenuItem.Enabled = fileToolStripMenuItem.Enabled;
            helpToolStripMenuItem.Enabled = fileToolStripMenuItem.Enabled;
            equipmentToolStripMenuItem.Enabled = fileToolStripMenuItem.Enabled;
            txtSearch.Enabled = enabled && !isScanning;
        }

        private void InitiateGIFOverlay()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(InitiateGIFOverlay));
                return;
            }

            GIFOverlay = new gifOverlay(this)
            {
                StartPosition = FormStartPosition.Manual,
                TopMost = true,
                Width = 256, // Set overlay size
                Height = 256,
                Owner = this
            };

            UpdateOverlayPosition();

            GIFOverlay.Start();
        }

        private bool ValidateDTAFile(string file, bool message)
        {
            if (string.IsNullOrEmpty(file) || !File.Exists(file)) return false;
            CreateHiddenFolder();
            Log("Validating DTA file from: " + file);
            if (xbox360.Checked)
            {
                Log("Extracting DTA file from CON file");
                if (!Parser.ExtractDTA(file))
                {
                    if (message)
                    {
                        MessageBox.Show("Something went wrong extracting the songs.dta file, can't add to the playlist", AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    Log("Failed");
                    return false;
                }
                Log("Success");
            }
            Log("Parsing DTA file");
            if (!Parser.ReadDTA(xbox360.Checked ? Parser.DTA : File.ReadAllBytes(file)) || !Parser.Songs.Any())
            {
                if (message)
                {
                    MessageBox.Show("Something went wrong reading that songs.dta file, can't add to the playlist", AppName,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Log("Failed");
                return false;
            }
            Log("Success - " + Parser.Songs.Count + " song(s) found in DTA file");
            if (Parser.Songs.Count == 1) return true;
            isScanning = true;
            UpdateNotifyTray();
            return true;
        }

        private bool ValidateNewSong(SongData song, int index, string location, bool scanning, bool message, out Song newsong)
        {
            Log("Validating new song: '" + song.Artist + " - " + song.Name + "'");
            var new_song = new Song
            {
                Name = CleanArtistSong(song.Name),
                Artist = CleanArtistSong(song.Artist),
                Location = location,
                Length = song.Length,
                InternalName = song.InternalName,
                Album = song.Album,
                Year = song.YearReleased,
                Track = song.TrackNumber,
                Genre = Parser.doGenre(song.RawGenre),
                Index = -1,
                AddToPlaylist = true,
                AttenuationValues = song.AttenuationValues.Replace("\t", ""),
                PanningValues = song.PanningValues.Replace("\t", ""),
                Charter = song.ChartAuthor,
                ChannelsDrums = song.ChannelsDrums,
                ChannelsBass = song.ChannelsBass,
                ChannelsGuitar = song.ChannelsGuitar,
                ChannelsKeys = song.ChannelsKeys,
                ChannelsVocals = song.ChannelsVocals,
                ChannelsCrowd = song.ChannelsCrowd,
                ChannelsBacking = song.ChannelsBacking(),
                ChannelsBassStart = song.ChannelsBassStart,
                ChannelsCrowdStart = song.ChannelsCrowdStart,
                ChannelsGuitarStart = song.ChannelsGuitarStart,
                ChannelsKeysStart = song.ChannelsKeysStart,
                ChannelsDrumsStart = song.ChannelsDrumsStart,
                ChannelsVocalsStart = song.ChannelsVocalsStart,
                ChannelsTotal = song.ChannelsTotal,
                DTAIndex = index,
                isRhythmOnBass = song.RhythmBass,
                isRhythmOnKeys = song.RhythmKeys || (song.Name.Contains("Rhythm Version") && !song.RhythmBass),
                hasProKeys = song.ProKeysDiff > 0,
                PSDelay = song.PSDelay,
                yargPath = song.YargPath

            };

            ActiveSongData = song;
            newsong = new_song;
            if (!scanning) return true;
            var exists = Playlist.Any(oldsong => String.Equals(oldsong.Artist, new_song.Artist, StringComparison.InvariantCultureIgnoreCase) &&
                                                 String.Equals(oldsong.Name, new_song.Name, StringComparison.InvariantCultureIgnoreCase));
            if (!exists)
            {
                Log("Song didn't exist in playlist, adding");
                return true;
            }
            if (message)
            {
                MessageBox.Show("Song '" + new_song.Artist + " - " + new_song.Name + "' is already in your playlist", AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            Log("Song already existed in playlist, not adding");
            return false;
        }

        private void loadDTA(string dta, bool message = false, bool scanning = true, bool next = false, bool prep = false)
        {
            Log("Loading DTA file " + dta);
            if (!ValidateDTAFile(dta, message))
            {
                Log("Failed to validate that DTA file, skipping");
                return;
            }

            Log("DTA file contains " + Parser.Songs.Count + " song(s)");
            hasNoMIDI = false;
            for (var i = 0; i < Parser.Songs.Count; i++)
            {
                if (CancelWorkers) return;
                var song = prep ? Parser.Songs[ActiveSong.DTAIndex] : (next ? Parser.Songs[NextSong.DTAIndex] : Parser.Songs[i]);
                Log("Processing song '" + song.Artist + " - " + song.Name + "'");

                string internalName = "";
                string PNG = "";
                var EDAT = "";
                string audioPath = "";
                string yarg = "";

                if (wii.Checked)
                {
                    Log("It's Wii playlist - processing as Wii song");
                    var index = song.FilePath.LastIndexOf("/", StringComparison.Ordinal) + 1;
                    song.InternalName = song.FilePath.Substring(index, song.FilePath.Length - index);
                    internalName = song.InternalName;
                    PNG = Path.GetDirectoryName(dta) + "\\" + internalName + "\\gen\\" + internalName + "_keep.png_wii";
                    audioPath = Path.GetDirectoryName(dta).Replace("_meta", "_song") + "\\" + internalName + "\\" + internalName + ".mogg";
                    NextSongMIDI = Path.GetDirectoryName(audioPath) + "\\" + internalName + ".mid";
                    Log("MIDI - " + NextSongMIDI);
                }
                else if (pS3.Checked)
                {
                    Log("It's PS3 playlist - processing as PS3 song");
                    internalName = song.InternalName;
                    PNG = Path.GetDirectoryName(dta) + "\\" + internalName + "\\gen\\" + internalName + "_keep.png_ps3";
                    audioPath = Path.GetDirectoryName(dta) + "\\" + internalName + "\\" + internalName + ".mogg";
                    EDAT = Path.GetDirectoryName(audioPath) + "\\" + internalName + ".mid.edat";
                    NextSongMIDI = EDAT.Replace(".mid.edat", ".mid");
                    Log("EDAT - " + EDAT);
                }
                else //is YARG
                {
                    Log("It's YARG playlist - processing as YARG loose song");
                    internalName = song.InternalName;
                    PNG = Path.GetDirectoryName(dta) + "\\" + internalName + "\\gen\\" + internalName + "_keep.png_xbox";
                    audioPath = Path.GetDirectoryName(dta) + "\\" + internalName + "\\" + internalName + ".mogg";
                    yarg = audioPath.Replace(".mogg", ".yarg_mogg");
                    NextSongMIDI = Path.GetDirectoryName(audioPath) + "\\" + internalName + ".mid";
                    Log("MIDI - " + NextSongMIDI);
                    CurrentSongAudioPath = yarg;
                }
                Log("Audio - " + audioPath);
                Log("Art - " + PNG);
                Log("Internal name - " + internalName);

                if (!File.Exists(audioPath) && !File.Exists(yarg))
                {
                    if (message)
                    {
                        MessageBox.Show("Couldn't locate audio file(s) for song '" + song.Artist + " - " + song.Name + "', can't add to the playlist",
                            AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    Log("Audio file(s) not found");
                    if (next || prep) return;
                    continue;
                }

                if (File.Exists(yarg))
                {
                    if (!nautilus.DecY(yarg, DecryptMode.ToMemory))
                    {
                        if (message)
                        {
                            MessageBox.Show("Song '" + song.Artist + " - " + song.Name + "' is YARG encrypted and I couldn't decrypt it, can't add to the playlist", AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        Log("YARG audio file is encrypted and failed to decrypt");
                        if (next || prep) return;
                        continue;
                    }
                }
                else
                {
                    var mData = File.ReadAllBytes(audioPath);
                    if (!nautilus.DecM(mData, false, true, DecryptMode.ToMemory))
                    {
                        if (message)
                        {
                            MessageBox.Show("Song '" + song.Artist + " - " + song.Name + "' is encrypted, can't add to the playlist", AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        Log("Audio file is encrypted and failed to decrypt");
                        if (next || prep) return;
                        continue;
                    }
                }
                Log("Audio file is not encrypted or decrypted successfully");

                Song newSong;
                if (!ValidateNewSong(song, i, string.IsNullOrEmpty(pkgPath) ? dta : pkgPath, scanning, message, out newSong)) continue;

                if (CancelWorkers) return;
                try
                {
                    newSong.BPM = 120;//default in case something fails below
                    if (File.Exists(EDAT) && pS3.Checked)
                    {
                        Log("Decrypting EDAT file");
                        DecryptPS3EDAT(EDAT, message);
                    }
                    if (File.Exists(NextSongMIDI))
                    {
                        hasNoMIDI = false;
                        Log("Reading MIDI file for contents");
                        MIDITools.Initialize(false);
                        if (MIDITools.ReadMIDIFile(NextSongMIDI, false))
                        {
                            newSong.BPM = MIDITools.MIDIInfo.AverageBPM;
                            Log("Success - Average BPM: " + newSong.BPM);
                        }
                        else
                        {
                            Log("Failed");
                        }
                    }
                    else
                    {
                        hasNoMIDI = true;
                        Log("MIDI file not found");
                    }

                    if (next || prep) //only do when processing for playback
                    {
                        Tools.DeleteFile(NextSongArtPNG);
                        Tools.DeleteFile(NextSongArtBlurred);
                        if (File.Exists(PNG))
                        {
                            NextSongArtPNG = Path.GetDirectoryName(PNG) + "\\" + Path.GetFileNameWithoutExtension(PNG) + ".png";
                            NextSongArtBlurred = NextSongArtPNG.Replace(".png", "_b.png");
                            Log("Converting album art from Rock Band format to PNG");
                            var converted = wii.Checked ? Tools.ConvertWiiImage(PNG, NextSongArtPNG, "png", false) :
                                Tools.ConvertRBImage(PNG, NextSongArtPNG, "png", false);
                            if (converted)
                            {
                                Log("Success");
                                Log("Creating album art composite");
                                Tools.CreateBlurredArt(NextSongArtPNG, NextSongArtBlurred);
                                Log(File.Exists(NextSongArtBlurred) ? "Success" : "Failed");
                            }
                            else
                            {
                                Log("Failed");
                            }
                        }
                        else
                        {
                            Log("Album art not found");
                        }
                    }

                    long length;
                    ProcessMogg(scanning, song.Length, "", out length);
                    newSong.Length = length;
                    Log("Song length: " + length);

                    if (!scanning) return;

                    Playlist.Add(newSong);
                    Log("Added '" + newSong.Artist + " - " + newSong.Name + "' to playlist");
                    if (isScanning)
                    {
                        ShowUpdate("Added '" + newSong.Artist + " - " + newSong.Name + "'");
                    }
                }
                catch (Exception ex)
                {
                    Log("Error loading DTA: " + ex.Message);
                    if (message)
                    {
                        MessageBox.Show("Error reading that file:\n" + ex.Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private bool ValidateINIFile(string file, bool message)
        {
            Log("Validating INI file: " + file);
            if (Parser.ReadINIFile(file))
            {
                Log("Success");
                Log("INI file contains song '" + Parser.Songs[0].Artist + " - " + Parser.Songs[0].Name + "'");
                return true;
            }
            Log("Failed to read INI file");
            if (message)
            {
                MessageBox.Show("Something went wrong reading that INI file, can't add to the playlist", AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }

        private void loadINI(string input, bool message = false, bool scanning = true, bool next = false, bool prep = false)
        {
            var INI = "";
            if (Path.GetExtension(input) == ".yargsong")
            {
                INI = DecryptExtractYARG(input, message, scanning, next, prep);
            }
            else if (Path.GetExtension(input) == ".fnf" || Path.GetExtension(input) == ".ini")
            {
                INI = input;
            }
            Log("Loading INI file " + input);
            if (!ValidateINIFile(INI, message))
            {
                Log("Failed to validate that INI file, skipping");
                return;
            }

            if (CancelWorkers) return;
            var song = Parser.Songs[0];
            Log("Processing song '" + song.Artist + " - " + song.Name + "'");
            Log("It's YARG / CH / PS or FNF playlist - processing as that type of song");

            NextSongArtPNG = Path.GetDirectoryName(INI) + "\\album.png";
            NextSongArtJPG = Path.GetDirectoryName(INI) + "\\album.jpg";
            var notesMIDI = Path.GetDirectoryName(INI) + "\\notes.mid";
            var nameMIDI = Path.GetDirectoryName(INI) + "\\" + song.ShortName + ".mid";

            if (File.Exists(nameMIDI))
            {
                NextSongMIDI = nameMIDI; //this is primarily for Fornite Festival songs
            }
            else
            {
                NextSongMIDI = notesMIDI;
            }

            oggFiles = Directory.GetFiles(Path.GetDirectoryName(INI), "*.ogg", SearchOption.TopDirectoryOnly);
            opusFiles = Directory.GetFiles(Path.GetDirectoryName(INI), "*.opus", SearchOption.TopDirectoryOnly);
            m4aFiles = Directory.GetFiles(Path.GetDirectoryName(INI), "*.m4a", SearchOption.TopDirectoryOnly);

            if (!oggFiles.Any() && !opusFiles.Any() && !m4aFiles.Any())
            {
                if (message)
                {
                    MessageBox.Show("Couldn't find audio files for song '" + song.Artist + " - " + song.Name + "', can't add to the playlist",
                        AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }
            Log("MIDI - " + NextSongMIDI);
            var audio = opusFiles.Any() ? opusFiles.Aggregate("", (current, opus) => current + " " + Path.GetFileName(opus)) : oggFiles.Aggregate("", (current, ogg) => current + " " + Path.GetFileName(ogg));
            if (m4aFiles.Any())
            {
                foreach (var m4a in m4aFiles)
                {
                    if (Path.GetFileName(m4a) != "preview.m4a")
                    {
                        audio = m4a;
                        break;
                    }
                }
            }
            Log("Audio - " + audio);
            Log("Art - " + (File.Exists(NextSongArtPNG) ? NextSongArtPNG : NextSongArtJPG));

            Song newSong;
            if (!ValidateNewSong(song, 0, string.IsNullOrEmpty(sngPath) ? INI : sngPath, scanning, message, out newSong)) return;
            newSong.Location = input;//for .yargsong files

            if (CancelWorkers) return;
            try
            {
                newSong.BPM = 120;//default in case something fails below
                if (File.Exists(NextSongMIDI))
                {
                    Log("MIDI file found");
                    Log("Reading MIDI file for contents");
                    MIDITools.Initialize(false);
                    if (MIDITools.ReadMIDIFile(NextSongMIDI, false))
                    {
                        hasNoMIDI = false;
                        newSong.BPM = MIDITools.MIDIInfo.AverageBPM;
                        Log("Success - Average BPM: " + newSong.BPM);
                    }
                    else
                    {
                        Log("Failed");
                    }
                }
                else
                {
                    hasNoMIDI = true;
                    Log("MIDI file not found");
                }

                if (next || prep) //only do when processing for playback
                {
                    Tools.DeleteFile(NextSongArtBlurred);
                    if (File.Exists(NextSongArtPNG) || File.Exists(NextSongArtJPG))
                    {
                        if (!File.Exists(NextSongArtPNG) && File.Exists(NextSongArtJPG))
                        {
                            NextSongArtBlurred = NextSongArtBlurred.Replace(".png", ".jpg");
                        }
                        Log("Album art found");
                        Log("Creating album art composite");
                        Tools.CreateBlurredArt(File.Exists(NextSongArtPNG) ? NextSongArtPNG : NextSongArtJPG, NextSongArtBlurred);
                        Log(File.Exists(NextSongArtBlurred) ? "Success" : "Failed");
                    }
                    else
                    {
                        Log("Album art not found");
                    }
                }

                newSong.Length = song.Length;
                if (newSong.Length <= 0)
                {
                    if (opusFiles.Any())
                    {
                        foreach (var opus in opusFiles)
                        {
                            long length;
                            ProcessMogg(true, 0, opus, out length);
                            if (length > newSong.Length)
                            {
                                newSong.Length = length;
                            }
                        }
                    }
                    foreach (var ogg in oggFiles)
                    {
                        nautilus.NextSongOggData = File.ReadAllBytes(ogg);
                        long length;
                        ProcessMogg(true, 0, "", out length);
                        if (length > newSong.Length)
                        {
                            newSong.Length = length;
                        }
                    }
                }
                Log("Song length: " + newSong.Length);

                if (!scanning)
                {
                    if (m4aFiles.Any())
                    {
                        PrepareFortniteM4A();
                    }
                    return;
                }

                Playlist.Add(newSong);
                Log("Added '" + newSong.Artist + " - " + newSong.Name + "' to playlist");
                if (isScanning)
                {
                    ShowUpdate("Added '" + newSong.Artist + " - " + newSong.Name + "'");
                }
            }
            catch (Exception ex)
            {
                Log("Error loading INI: " + ex.Message);
                if (message)
                {
                    MessageBox.Show("Error reading that file:\n" + ex.Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void PrepareFortniteM4A()
        {
            var audio = "";
            activeM4AFile = "";
            foreach (var m4a in m4aFiles)
            {
                if (Path.GetFileName(m4a) != "preview.m4a")
                {
                    audio = m4a;
                    break;
                }
            }
            Bass.BASS_ChannelFree(BassStream);
            BassStream = fnfParser.m4aToBassStream(audio, 10);//always 10 channels, no preview allowed here
            if (BassStream == 0)
            {
                MessageBox.Show("File '" + audio + "' is not a valid input file", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Bass.BASS_ChannelFree(BassStream);
                return;
            }

            var tempFile = Path.GetTempFileName();

            //this next bit is an ugly hack but temporary until Ian @ BASS implements a better solution
            //writes the raw opus data to a temporary wav file (fastest encoder) and then reads it back in the StartPlayback function
            BassEnc.BASS_Encode_Start(BassStream, tempFile, BASSEncode.BASS_ENCODE_PCM | BASSEncode.BASS_ENCODE_AUTOFREE, null, IntPtr.Zero);
            while (true)
            {
                var buffer = new byte[20000];
                var c = Bass.BASS_ChannelGetData(BassStream, buffer, buffer.Length);
                if (c <= 0) break;
            }
            Bass.BASS_ChannelFree(BassStream);

            BassStream = Bass.BASS_StreamCreateFile(tempFile, 0L, 0L, BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_SAMPLE_FLOAT);
            if (BassStream == 0)
            {
                MessageBox.Show("That is not a valid .m4a input file", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                File.Delete(tempFile);
                Bass.BASS_ChannelFree(BassStream);
                return;
            }

            activeM4AFile = tempFile;
        }

        private void ProcessMogg(bool scanning, long in_length, string file, out long Length)
        {
            Length = in_length;
            if (scanning && in_length == 0)
            {
                Log("Processing audio file for length");
                try
                {
                    var stream = 0;
                    if (opusFiles.Any())
                    {
                        stream = BassOpus.BASS_OPUS_StreamCreateFile(file, 0, File.ReadAllBytes(file).Length, BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_SAMPLE_FLOAT);
                    }
                    else
                    {
                        stream = Bass.BASS_StreamCreateFile(nautilus.GetOggStreamIntPtr(true), 0L, nautilus.NextSongOggData.Length, BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_SAMPLE_FLOAT);
                    }
                    var len = Bass.BASS_ChannelGetLength(stream);
                    var totaltime = Bass.BASS_ChannelBytes2Seconds(stream, len); // the total time length
                    Length = (int)(totaltime * 1000);
                    if (!opusFiles.Any())
                    {
                        nautilus.ReleaseStreamHandle(true);
                    }
                }
                catch (Exception ex)
                {
                    Log("Error processing audio file: " + ex.Message);
                }
            }
        }

        private void loadCON(string con, bool message = false, bool scanning = true, bool next = false, bool prep = false)
        {
            Log("Loading CON file " + con);
            if (!ValidateDTAFile(con, message))
            {
                Log("Failed to validate that CON file, skipping");
                return;
            }
            hasNoMIDI = false;
            if (message && isScanning)
            {
                message = false;
            }
            var xPackage = new STFSPackage(con);
            if (!xPackage.ParseSuccess)
            {
                Log("There was an error parsing that " + (Parser.Songs.Count > 1 ? "pack" : "song"));
                if (message)
                {
                    MessageBox.Show("There was an error parsing that " + (Parser.Songs.Count > 1 ? "pack" : "song"), AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return;
            }

            Log("CON file contains " + Parser.Songs.Count + " song(s)");
            for (var i = 0; i < Parser.Songs.Count; i++)
            {
                if (CancelWorkers) return;
                var song = prep ? Parser.Songs[ActiveSong.DTAIndex] : (next ? Parser.Songs[NextSong.DTAIndex] : Parser.Songs[i]);
                Log("Processing song '" + song.Artist + " - " + song.Name + "'");

                Song newsong;
                if (!ValidateNewSong(song, i, con, scanning, message, out newsong)) continue;
                if (ActiveSongData == null || prep)
                {
                    ActiveSongData = song;
                }

                if (CancelWorkers) return;
                var internalname = song.InternalName;
                Log("Internal name: " + internalname);
                try
                {
                    Log("Searching for mogg file");
                    var xFile = xPackage.GetFile("songs/" + internalname + "/" + internalname + ".mogg");
                    if (xFile == null)
                    {
                        Log("Mogg file not found");
                        xPackage.CloseIO();
                        return;
                    }
                    Log("Success");
                    Log("Extracting mogg file");
                    var mData = xFile.Extract();
                    if (mData == null || mData.Length == 0)
                    {
                        Log("Failed");
                        xPackage.CloseIO();
                        return;
                    }
                    Log("Success");

                    Tools.DeleteFile(NextSongMIDI);
                    newsong.BPM = 120;//default in case something fails below
                    xFile = xPackage.GetFile("songs/" + internalname + "/" + internalname + ".mid");
                    Log("Searching for MIDI file");
                    if (xFile != null)
                    {
                        Log("Success");
                        Log("Extracting MIDI file");
                        if (xFile.ExtractToFile(NextSongMIDI))
                        {
                            Log("Reading MIDI file for contents");
                            MIDITools.Initialize(false);
                            if (MIDITools.ReadMIDIFile(NextSongMIDI, false))
                            {
                                newsong.BPM = MIDITools.MIDIInfo.AverageBPM;
                                Log("Success - Average BPM: " + newsong.BPM);
                            }
                            else
                            {
                                Log("Failed");
                            }
                        }
                        else
                        {
                            Log("Failed");
                        }
                    }
                    else
                    {
                        Log("MIDI file not found");
                    }

                    if (next || prep) //only do when processing for playback
                    {
                        Tools.DeleteFile(NextSongArtPNG);
                        Tools.DeleteFile(NextSongArtBlurred);
                        xFile = xPackage.GetFile("songs/" + internalname + "/gen/" + internalname + "_keep.png_xbox");
                        Log("Searching for album art file");
                        if (xFile != null)
                        {
                            var art = Path.GetTempPath() + "next.png_xbox";
                            Tools.DeleteFile(art);

                            if (xFile.ExtractToFile(art))
                            {
                                Log("Converting album art from Rock Band format to PNG");
                                var converted = Tools.ConvertRBImage(art, NextSongArtPNG, "png", true);
                                if (converted)
                                {
                                    Log("Success");
                                    Log("Creating album art composite");
                                    Tools.CreateBlurredArt(NextSongArtPNG, NextSongArtBlurred);
                                    Log(File.Exists(NextSongArtBlurred) ? "Success" : "Failed");
                                }
                                else
                                {
                                    Log("Failed");
                                }
                            }
                            else
                            {
                                Log("Failed");
                            }
                        }
                        else
                        {
                            Log("Album art file not found");
                        }
                    }

                    if (CancelWorkers) return;
                    if (!nautilus.DecM(mData, false, true, DecryptMode.ToMemory))
                    {
                        if (message && Parser.Songs.Count == 1)
                        {
                            MessageBox.Show("Song '" + song.Artist + " - " + song.Name + "' is encrypted, can't add to the playlist", AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        Log("Audio file is encrypted and failed to decrypt");
                        xPackage.CloseIO();
                        return;
                    }

                    Log("Audio file is not encrypted or decrypted successfully");

                    long length;
                    ProcessMogg(scanning, song.Length, "", out length);
                    newsong.Length = length;
                    Log("Song length: " + length);

                    if (!scanning)
                    {
                        xPackage.CloseIO();
                        return;
                    }

                    Playlist.Add(newsong);
                    Log("Added '" + newsong.Artist + " - " + newsong.Name + "' to playlist");
                    if (isScanning)
                    {
                        ShowUpdate("Added '" + newsong.Artist + " - " + newsong.Name + "'");
                    }
                }
                catch (Exception ex)
                {
                    Log("Error loading CON: " + ex.Message);
                    if (message)
                    {
                        MessageBox.Show("Error reading that file:\n" + ex.Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            try
            {
                xPackage.CloseIO();
            }
            catch (Exception ex)
            {
                Log("Error closing STFS file IO: " + ex.Message);
            }
        }

        private void DecryptPS3EDAT(string edat, bool message)
        {
            if (!File.Exists(edat)) return;
            Log("Decrypting EDAT to MIDI");
            Tools.DeleteFile(NextSongMIDI);
            if (!Tools.DecryptEdat(edat, NextSongMIDI, currentKLIC))
            {
                Log("Failed");
            }
            if (File.Exists(NextSongMIDI))
            {
                Log("Decrypted to MIDI successfully");
            }
            else
            {
                Log("Decrypting to MIDI failed");
                if (message)
                {
                    MessageBox.Show("Failed to decrypt that song's EDAT file to a usable MIDI", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void batchSongLoader_DoWork(object sender, DoWorkEventArgs e)
        {
            Log("Batch song loader working");
            if (xbox360.Checked)
            {
                Log("loadCON: " + SongsToAdd[0]);
                loadCON(SongsToAdd[0], !isScanning);
            }
            else if (yarg.Checked)
            {
                if (Path.GetExtension(SongsToAdd[0]) == ".yargsong")
                {
                    pkgPath = "";
                    sngPath = SongsToAdd[0];
                    Log("DecryptExtractYARG: " + SongsToAdd[0]);
                    loadINI(SongsToAdd[0], !isScanning);
                }
                else if (Path.GetExtension(SongsToAdd[0]) == ".sng")
                {
                    sngPath = SongsToAdd[0];
                    Log("loadSNG: " + SongsToAdd[0]);
                    loadSNG(SongsToAdd[0], !isScanning);
                }
                else if (Path.GetFileName(SongsToAdd[0]) == "songs.dta")
                {
                    pkgPath = "";
                    sngPath = "";
                    Log("loadDTA: " + SongsToAdd[0]);
                    loadDTA(SongsToAdd[0], !isScanning);
                }
                else
                {
                    sngPath = "";
                    Log("loadINI: " + SongsToAdd[0]);
                    loadINI(SongsToAdd[0], !isScanning);
                }
            }
            else if (rockSmith.Checked)
            {
                Log("loadPSARC: " + SongsToAdd[0]);
                loadPSARC(SongsToAdd[0], !isScanning);
            }
            else if (guitarHero.Checked)
            {
                ghwtPath = SongsToAdd[0];
                Log("loadGHWT: " + SongsToAdd[0]);
                loadGHWT(SongsToAdd[0], !isScanning);
            }
            else if (fortNite.Checked)
            {
                Log("loadFNF: " + SongsToAdd[0]);
                loadINI(SongsToAdd[0], !isScanning);
            }
            else if (powerGig.Checked)
            {
                Log("ExtractXMA: " + SongsToAdd[0]);
                ExtractXMA(SongsToAdd[0], !isScanning);
            }
            else if (bandFuse.Checked)
            {
                BandFusePath = SongsToAdd[0];
                Log("ExtractBandFuse: " + SongsToAdd[0]);
                ExtractBandFuse(SongsToAdd[0], !isScanning);
            }
            else
            {
                if (pS3.Checked && Path.GetExtension(SongsToAdd[0]) == ".pkg")
                {
                    pkgPath = SongsToAdd[0];
                    Log("loadPKG: " + SongsToAdd[0]);
                    loadPKG(SongsToAdd[0], !isScanning);
                }
                else
                {
                    pkgPath = "";
                    ActiveSong.yargPath = "";
                    Log("loadDTA: " + SongsToAdd[0]);
                    loadDTA(SongsToAdd[0], !isScanning);
                }
            }
            SongsToAdd.RemoveAt(0);
        }

        private void ExtractBandFuse(string file, bool message = false, bool scanning = true, bool next = false, bool prep = false)
        {
            var temp = Application.StartupPath + "\\temp\\";
            Tools.DeleteFolder(temp, true);//clean up before starting with new song
            Directory.CreateDirectory(temp);

            var songFuse = Application.StartupPath + "\\bin\\songfuse.exe";
            if (!File.Exists(songFuse))
            {
                MessageBox.Show("Could not find songfuse.exe in the \\bin\\ folder, can't continue", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            var package = new STFSPackage(file);
            if (package.Header.TitleID != (uint)1296435155)
            {
                package.CloseIO();
                Log("Right kind of file but invalid game ID, skipping...");
                if (message)
                {
                    MessageBox.Show("This is the right kind of file but it has an invalid game ID, this is not a match for BandFuse, skipping this file", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }

            var header = package.Header.Description;
            var index = header.IndexOf(" - ");

            SongData song = new SongData();
            song.Initialize();
            song.Name = header.Substring(0, index).Trim();
            song.Artist = header.Substring(index + 3, header.Length - (index + 3)).Trim();
            song.ChartAuthor = "BandFuse";
            Parser.Songs = new List<SongData> { song };

            Song newSong;
            if (!ValidateNewSong(song, 0, file, scanning, message, out newSong)) return;

            if (scanning)
            {
                package.CloseIO();

                Playlist.Add(newSong);
                Log("Added '" + newSong.Artist + " - " + newSong.Name + "' to playlist");
                if (isScanning)
                {
                    ShowUpdate("Added '" + newSong.Artist + " - " + newSong.Name + "'");
                }

                return;
            }

            if (!package.ExtractPayload(temp, true, false))
            {
                MessageBox.Show("Failed to extract file contents, can't play this song", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                package.CloseIO();
                return;
            }
            package.CloseIO();

            if (CancelWorkers) return;
            var art = temp + "album.png";
            var album_art = "";
            var xpr = Directory.GetFiles(temp, "*.xpr", SearchOption.AllDirectories);
            if (xpr.Count() > 0)
            {
                if (!Tools.ConvertBandFuse("texture", xpr[0], art))
                {
                    MessageBox.Show("Failed to convert album art texture", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    album_art = art;
                }
            }

            NextSongArtPNG = album_art;
            NextSongMIDI = "";
            hasNoMIDI = true;

            if (next || prep) //only do when processing for playback
            {
                Tools.DeleteFile(NextSongArtBlurred);
                if (File.Exists(NextSongArtPNG))
                {
                    Log("Album art found");
                    Log("Creating album art composite");
                    Tools.CreateBlurredArt(NextSongArtPNG, NextSongArtBlurred);
                    Log(File.Exists(NextSongArtBlurred) ? "Success" : "Failed");
                }
                else
                {
                    Log("Album art not found");
                }
            }

            cltFiles = Directory.GetFiles(temp, "*.clt", SearchOption.AllDirectories);
            if (!cltFiles.Any())
            {
                MessageBox.Show("No audio files found to decrypt, can't play this song", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            var didBacking = false;
            var didBass = false;
            var didDrums = false;
            var didGuitar1 = false;
            var didGuitar2 = false;
            var didVocals = false;

            if (CancelWorkers) return;
            foreach (var clt in cltFiles)
            {
                if (clt.EndsWith("\\back\\audio.clt") && !didBacking)
                {
                    Tools.ConvertBandFuse("audio", clt, temp + "backing.wav");
                    didBacking = true;
                }
                else if (clt.EndsWith("\\bass\\audio.clt") && !didBass)
                {
                    Tools.ConvertBandFuse("audio", clt, temp + "bass.wav");
                    didBass = true;
                }
                else if (clt.EndsWith("\\drums\\audio.clt") && !didDrums)
                {
                    Tools.ConvertBandFuse("audio", clt, temp + "drums.wav");
                    didDrums = true;
                }
                else if (clt.EndsWith("\\gtr1\\audio.clt") && !didGuitar1)
                {
                    Tools.ConvertBandFuse("audio", clt, temp + "guitar_1.wav");
                    didGuitar1 = true;
                }
                else if (clt.EndsWith("\\gtr2\\audio.clt") && !didGuitar2)
                {
                    Tools.ConvertBandFuse("audio", clt, temp + "guitar_2.wav");
                    didGuitar2 = true;
                }
                else if (clt.EndsWith("\\vox\\audio.clt") && !didVocals)
                {
                    Tools.ConvertBandFuse("audio", clt, temp + "vocals.wav");
                    didVocals = true;
                }
            }

            BandFusePath = temp;
            wavFiles = Directory.GetFiles(temp, "*.wav", SearchOption.TopDirectoryOnly);
            if (wavFiles.Any())
            {
                try
                {
                    //the metadata doesn't contain song data, let's get it from the wav files
                    var stream = Bass.BASS_StreamCreateFile(wavFiles[0], 0L, File.ReadAllBytes(wavFiles[0]).Length, BASSFlag.BASS_SAMPLE_FLOAT);
                    var len = Bass.BASS_ChannelGetLength(stream);
                    var totaltime = Bass.BASS_ChannelBytes2Seconds(stream, len); // the total time length
                    Parser.Songs[0].Length = (int)(totaltime * 1000);
                    overrideSongLength = Parser.Songs[0].Length;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error:\n" + ex.Message + "\n\nBASS status:\n" + Bass.BASS_ErrorGetCode());
                }

            }
            else
            {
                if (!message) return;
                MessageBox.Show("Failed to extract audio streams from XMA file, can't play this song", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void loadPKG(string pkg, bool message = false, bool scanning = true, bool next = false, bool prep = false)
        {
            var folder = Application.StartupPath + "\\temp\\";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            var outFolder = folder + Path.GetFileNameWithoutExtension(pkg).Replace(" ", "").Replace("-", "").Replace("_", "").Trim() + "_ex";
            Tools.DeleteFolder(outFolder, true);
            if (!Tools.ExtractPKG(pkg, outFolder, out currentKLIC))
            {
                MessageBox.Show("Failed to process that PKG file, can't play it", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            var DTA = Directory.GetFiles(outFolder, "songs.dta", SearchOption.AllDirectories);
            if (DTA.Count() == 0)
            {
                MessageBox.Show("No songs.dta file found, can't play that PKG file", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            pkgPath = pkg;
            loadDTA(DTA[0], message, scanning, next, prep);
        }

        private void loadSNG(string sng, bool message = false, bool scanning = true, bool next = false, bool prep = false)
        {
            var outFolder = Application.StartupPath + "\\temp";
            if (Directory.Exists(outFolder))
            {
                Tools.DeleteFolder(outFolder, true);
            }
            Directory.CreateDirectory(outFolder);

            if (!Tools.ExtractSNG(sng, outFolder))
            {
                MessageBox.Show("Failed to process that SNG file, can't play it", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            var INI = Directory.GetFiles(outFolder, "song.ini", SearchOption.TopDirectoryOnly);
            if (INI.Count() == 0)
            {
                MessageBox.Show("No song.ini file found, can't play that SNG file", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            sngPath = sng;
            loadINI(INI[0], message, scanning, next, prep);
        }

        private void ExtractXMA(string xml, bool message = false, bool scanning = true, bool next = false, bool prep = false)
        {
            XML_PATH = xml;
            var XMAs = Directory.GetFiles(Path.GetDirectoryName(XML_PATH), "*.xma", SearchOption.TopDirectoryOnly);
            var ogXMA = XMAs[0];

            Log("Loading XML file " + xml);
            if (!ValidateXMLFile(xml, message))
            {
                Log("Failed to validate that XML file, skipping");
                return;
            }

            var album_art = "";
            var albumPNG = Path.GetDirectoryName(XML_PATH) + "\\album.png";
            var albumJPG = Path.GetDirectoryName(XML_PATH) + "\\album.jpg";
            if (File.Exists(albumPNG))
            {
                album_art = albumPNG;
            }
            else if (File.Exists(albumJPG))
            {
                album_art = albumJPG;
            }

            if (CancelWorkers) return;
            var song = Parser.Songs[0];
            Log("Processing song '" + song.Artist + " - " + song.Name + "'");
            Log("It's Power Gig playlist - processing as that type of song");

            NextSongArtPNG = album_art;
            NextSongMIDI = "";
            hasNoMIDI = true;

            Log("No MIDI file for this type of song!");
            Log("Audio - WAV FILES");
            Log("Art - " + (File.Exists(NextSongArtPNG) ? NextSongArtPNG : "NONE"));

            Song newSong;
            if (!ValidateNewSong(song, 0, xml, scanning, message, out newSong)) return;

            if (CancelWorkers) return;
            try
            {
                if (next || prep) //only do when processing for playback
                {
                    Tools.DeleteFile(NextSongArtBlurred);
                    if (File.Exists(NextSongArtPNG))
                    {
                        Log("Album art found");
                        Log("Creating album art composite");
                        Tools.CreateBlurredArt(NextSongArtPNG, NextSongArtBlurred);
                        Log(File.Exists(NextSongArtBlurred) ? "Success" : "Failed");
                    }
                    else
                    {
                        Log("Album art not found");
                    }
                }

                if (scanning)
                {
                    Playlist.Add(newSong);
                    Log("Added '" + newSong.Artist + " - " + newSong.Name + "' to playlist");
                    if (isScanning)
                    {
                        ShowUpdate("Added '" + newSong.Artist + " - " + newSong.Name + "'");
                    }
                    return;
                }
            }
            catch (Exception ex)
            {
                Log("Error loading XML: " + ex.Message);
                if (message)
                {
                    MessageBox.Show("Error reading that file:\n" + ex.Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return;
            }

            var temp = Application.StartupPath + "\\temp\\";
            Tools.DeleteFolder(temp, true);
            Directory.CreateDirectory(temp);
            XMA_EXT_PATH = temp + (string.IsNullOrEmpty(Parser.Songs[0].InternalName) ? "temp" : Parser.Songs[0].InternalName);
            if (!Directory.Exists(XMA_EXT_PATH))
            {
                Directory.CreateDirectory(XMA_EXT_PATH);
            }
            XMA_PATH = XMA_EXT_PATH + "\\" + Path.GetFileNameWithoutExtension(XML_PATH) + "_all.xma";


            if (!File.Exists(ogXMA))
            {
                MessageBox.Show("Expected XMA file '" + Path.GetFileName(ogXMA) + "' not found, can't play this song", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            File.Copy(ogXMA, XMA_PATH, true);

            if (!Tools.XMASH(XMA_PATH))
            {
                MessageBox.Show("Failed to extract the audio streams from '" + Path.GetFileName(XMA_PATH) + "' - can't play this song", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Tools.DeleteFile(XMA_EXT_PATH + "\\xmash.exe");
                return;
            }
            Tools.DeleteFile(XMA_EXT_PATH + "\\xmash.exe");
            Tools.DeleteFile(XMA_PATH);

            var sepXMAs = Directory.GetFiles(XMA_EXT_PATH, "*.xma", SearchOption.TopDirectoryOnly);
            if (sepXMAs.Count() == 0)
            {
                MessageBox.Show("Failed to extract the audio streams from '" + Path.GetFileName(XMA_PATH) + "' - can't play this song", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            foreach (var xma in sepXMAs)
            {
                if (!Tools.toWAV(xma))
                {
                    MessageBox.Show("Failed to convert XMA file to WAV - can't play this song", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Tools.DeleteFile(XMA_EXT_PATH + "\\towav.exe");
                    return;
                }
            }
            Tools.DeleteFile(XMA_EXT_PATH + "\\towav.exe");

            foreach (var xma in sepXMAs)
            {
                Tools.DeleteFile(xma);
            }

            //rename the files based on assumed order
            wavFiles = Directory.GetFiles(XMA_EXT_PATH, "*.wav");
            if (wavFiles.Any())
            {
                try
                {
                    //the metadata doesn't contain song data, let's get it from the wav files
                    var stream = Bass.BASS_StreamCreateFile(wavFiles[0], 0L, File.ReadAllBytes(wavFiles[0]).Length, BASSFlag.BASS_SAMPLE_FLOAT);
                    var len = Bass.BASS_ChannelGetLength(stream);
                    var totaltime = Bass.BASS_ChannelBytes2Seconds(stream, len); // the total time length
                    Parser.Songs[0].Length = (int)(totaltime * 1000);
                    overrideSongLength = Parser.Songs[0].Length;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error:\n" + ex.Message + "\n\nBASS status:\n" + Bass.BASS_ErrorGetCode());
                }

            }
            else
            {
                if (!message) return;
                MessageBox.Show("Failed to extract audio streams from XMA file, can't play this song", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            for (var i = wavFiles.Count(); i >= 0; i--)
            {
                if (i == wavFiles.Count() - 1)
                {
                    File.Move(wavFiles[i], XMA_EXT_PATH + "\\song.wav");
                }
                else if (i == wavFiles.Count() - 2)
                {
                    File.Move(wavFiles[i], XMA_EXT_PATH + "\\guitar.wav");
                }
                else if (i == wavFiles.Count() - 3)
                {
                    File.Move(wavFiles[i], XMA_EXT_PATH + "\\vocals.wav");
                }
                else if (i <= wavFiles.Count() - 4)
                {
                    switch (i)
                    {
                        case 0:
                            File.Move(wavFiles[i], XMA_EXT_PATH + "\\drums_1.wav");
                            i = -1;
                            break;
                        case 1:
                            File.Move(wavFiles[i - 1], XMA_EXT_PATH + "\\drums_1.wav");
                            File.Move(wavFiles[i], XMA_EXT_PATH + "\\drums_2.wav");
                            i = -1;
                            break;
                        case 2:
                            File.Move(wavFiles[i - 3], XMA_EXT_PATH + "\\drums_1.wav");
                            File.Move(wavFiles[i - 1], XMA_EXT_PATH + "\\drums_2.wav");
                            File.Move(wavFiles[i], XMA_EXT_PATH + "\\drums_3.wav");
                            i = -1;
                            break;
                        case 3:
                            File.Move(wavFiles[i - 3], XMA_EXT_PATH + "\\drums_1.wav");
                            File.Move(wavFiles[i - 2], XMA_EXT_PATH + "\\drums_2.wav");
                            File.Move(wavFiles[i - 1], XMA_EXT_PATH + "\\drums_3.wav");
                            File.Move(wavFiles[i], XMA_EXT_PATH + "\\drums_4.wav");
                            i = -1;
                            break;
                    }
                }
            }
        }

        private void loadPSARC(string psarc, bool message = false, bool scanning = true, bool next = false, bool prep = false)
        {
            var outFolder = Application.StartupPath + "\\temp";
            if (Directory.Exists(outFolder))
            {
                Tools.DeleteFolder(outFolder, true);
            }
            Directory.CreateDirectory(outFolder);

            var rsFolder = outFolder + "\\" + Path.GetFileNameWithoutExtension(psarc) + "_psarc_RS2014_Pc";

            if (!Tools.ExtractPsArc(psarc, outFolder, rsFolder))
            {
                MessageBox.Show("Failed to process that PsArc file, can't play it", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            PlayARCFolder(rsFolder, psarc, message, scanning, next, prep);
        }

        private bool ValidateHSANFile(string file, bool message)
        {
            Log("Validating HSAN file: " + file);
            if (Parser.ReadHSANFile(file))
            {
                Log("Success");
                Log("HSAN file contains song '" + Parser.Songs[0].Artist + " - " + Parser.Songs[0].Name + "'");
                return true;
            }
            Log("Failed to read HSAN file");
            if (message)
            {
                MessageBox.Show("Something went wrong reading that HSAN file, can't add to the playlist", AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }

        private bool ValidateXMLFile(string file, bool message)
        {
            Log("Validating XML file: " + file);
            if (Parser.ReadXMLFile(file))
            {
                Log("Success");
                Log("XML file contains song '" + Parser.Songs[0].Artist + " - " + Parser.Songs[0].Name + "'");
                return true;
            }
            Log("Failed to read XML file");
            if (message)
            {
                MessageBox.Show("Something went wrong reading that XML file, can't add to the playlist", AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }

        private void PlayARCFolder(string folder, string psarc, bool message = false, bool scanning = true, bool next = false, bool prep = false)
        {
            var audioFolder = folder + "\\audio\\windows\\";
            var OggFiles = Directory.GetFiles(audioFolder, "*.ogg");
            var artFolder = folder + "\\gfxassets\\album_art\\";
            var pngFiles = Directory.GetFiles(artFolder, "*.png");
            var ddsFiles = Directory.GetFiles(artFolder, "*.dds");
            var album_art = "";
            var manifestFolder = folder + "\\manifests\\";
            var metadataFiles = Directory.GetFiles(manifestFolder, "*.hsan", SearchOption.AllDirectories);
            var sngFolder = folder + "\\songs\\bin\\generic\\";
            var sngFiles = Directory.GetFiles(sngFolder, "*.sng");
            var HSAN = "";

            //bool hasBass = false;
            //bool hasLead = false;
            bool hasRhythm = false;
            bool hasVocals = false;

            if (metadataFiles.Count() > 0)
            {
                HSAN = metadataFiles[0];
            }
            Log("Loading HSAN file " + HSAN);
            if (!ValidateHSANFile(HSAN, message))
            {
                Log("Failed to validate that HSAN file, skipping");
                return;
            }

            if (pngFiles.Count() > 0)
            {
                album_art = pngFiles[0];
            }
            else if (ddsFiles.Count() > 0)
            {
                for (var i = 0; i < ddsFiles.Count(); i++)
                {
                    if (ddsFiles[i].Contains("256"))
                    {
                        album_art = ddsFiles[i];
                        break;
                    }
                }
            }

            if (sngFiles.Count() > 0)
            {
                foreach (var sng in sngFiles)
                {
                    if (sng.Contains("_bass.sng"))
                    {
                        //hasBass = true;
                    }
                    else if (sng.Contains("_lead.sng"))
                    {
                        //hasLead = true;
                    }
                    else if (sng.Contains("_rhythm.sng"))
                    {
                        hasRhythm = true;
                    }
                    else if (sng.Contains("_vocals.sng"))
                    {
                        hasVocals = true;
                    }
                }
            }

            if (hasVocals)
            {
                Parser.Songs[0].VocalsDiff = 1;
            }
            else if (hasRhythm)
            {
                Parser.Songs[0].RhythmBass = true;
            }

            var bigOgg = "";
            var sortedOggs = from f in OggFiles orderby new FileInfo(f).Length ascending select f;
            foreach (var sorted in sortedOggs)
            {
                bigOgg = sorted.ToString();
            }
            var songAudio = Path.GetDirectoryName(audioFolder) + "\\song.ogg";
            Tools.MoveFile(bigOgg, songAudio);

            if (CancelWorkers) return;
            var song = Parser.Songs[0];
            Log("Processing song '" + song.Artist + " - " + song.Name + "'");
            Log("It's RS2014 playlist - processing as that type of song");

            NextSongArtPNG = album_art;
            NextSongMIDI = "";
            hasNoMIDI = true;

            if (!OggFiles.Any())
            {
                MessageBox.Show("Couldn't find audio files for song '" + song.Artist + " - " + song.Name + "', can't add to the playlist",
                        AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Log("No MIDI file for this type of song!");
            Log("Audio - OGG FILE");
            Log("Art - " + (File.Exists(NextSongArtPNG) ? NextSongArtPNG : "NONE"));

            Song newSong;
            if (!ValidateNewSong(song, 0, psarc, scanning, message, out newSong)) return;

            if (CancelWorkers) return;
            try
            {
                hasNoMIDI = true;

                if (next || prep) //only do when processing for playback
                {
                    Tools.DeleteFile(NextSongArtBlurred);
                    if (File.Exists(NextSongArtPNG))
                    {
                        Log("Album art found");
                        Log("Creating album art composite");
                        Tools.CreateBlurredArt(NextSongArtPNG, NextSongArtBlurred);
                        Log(File.Exists(NextSongArtBlurred) ? "Success" : "Failed");
                    }
                    else
                    {
                        Log("Album art not found");
                    }
                }

                nautilus.NextSongOggData = File.ReadAllBytes(songAudio);
                newSong.Length = song.Length;
                long length;
                ProcessMogg(true, 0, "", out length);
                if (length > newSong.Length)
                {
                    newSong.Length = length;
                }
                Log("Song length: " + newSong.Length);

                if (scanning)
                {
                    Playlist.Add(newSong);
                    Log("Added '" + newSong.Artist + " - " + newSong.Name + "' to playlist");
                    if (isScanning)
                    {
                        ShowUpdate("Added '" + newSong.Artist + " - " + newSong.Name + "'");
                    }
                }
            }
            catch (Exception ex)
            {
                Log("Error loading HSAN: " + ex.Message);
                if (message)
                {
                    MessageBox.Show("Error reading that file:\n" + ex.Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            psarcPath = audioFolder;
            MoveSongFiles();
            PrepareForPlayback();
        }

        private void loadGHWT(string ini, bool message = false, bool scanning = true, bool next = false, bool prep = false)
        {
            var outFolder = Application.StartupPath + "\\temp";
            if (Directory.Exists(outFolder))
            {
                Tools.DeleteFolder(outFolder, true);
            }
            Directory.CreateDirectory(outFolder);

            Parser.ReadGHWTDEFile(ini);

            if (CancelWorkers) return;
            var song = Parser.Songs[0];

            Song newSong;
            if (!ValidateNewSong(song, 0, ini, scanning, message, out newSong)) return;

            if (scanning)
            {
                Playlist.Add(newSong);
                Log("Added '" + newSong.Artist + " - " + newSong.Name + "' to playlist");
                if (isScanning)
                {
                    ShowUpdate("Added '" + newSong.Artist + " - " + newSong.Name + "'");
                }
                return;
            }

            var albumPNG = Path.GetDirectoryName(ini) + "\\Content\\album.png";
            var albumJPG = Path.GetDirectoryName(ini) + "\\Content\\album.jpg";

            if (File.Exists(albumPNG))
            {
                NextSongArtPNG = albumPNG;
            }
            else if (File.Exists(albumJPG))
            {
                NextSongArtPNG = albumJPG;
            }
            NextSongMIDI = "";
            hasNoMIDI = true;

            var temp = Application.StartupPath + "\\temp\\";
            if (!Directory.Exists(temp))
            {
                Directory.CreateDirectory(temp);
            }
            var ext_path = temp + (string.IsNullOrEmpty(Parser.Songs[0].InternalName) ? "temp" : Parser.Songs[0].InternalName);
            if (!Directory.Exists(ext_path))
            {
                Directory.CreateDirectory(ext_path);
            }
            ghwtPath = ext_path;

            var fsb1 = Path.GetDirectoryName(ini) + "\\Content\\MUSIC\\" + Parser.Songs[0].InternalName + "_1.fsb.xen";
            var fsb2 = Path.GetDirectoryName(ini) + "\\Content\\MUSIC\\" + Parser.Songs[0].InternalName + "_2.fsb.xen";
            var fsb3 = Path.GetDirectoryName(ini) + "\\Content\\MUSIC\\" + Parser.Songs[0].InternalName + "_3.fsb.xen";

            var decFSB1 = temp + Path.GetFileName(fsb1);
            var decFSB2 = temp + Path.GetFileName(fsb2);
            var decFSB3 = temp + Path.GetFileName(fsb3);

            if (CancelWorkers) return;
            if (Tools.fsbIsEncrypted(fsb1))
            {
                var dec = Tools.DecryptFSBFile(fsb1);
                File.WriteAllBytes(decFSB1, dec);
                fsb1 = decFSB1;
            }
            if (File.Exists(decFSB1) && Tools.fsbIsEncrypted(decFSB1))
            {
                MessageBox.Show("File '" + Path.GetFileName(fsb1) + "' is encrypted and I failed to decrypt it\nCan't continue", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Tools.DeleteFile(decFSB1);
                return;
            }

            if (CancelWorkers) return;
            if (Tools.fsbIsEncrypted(fsb2))
            {
                var dec = Tools.DecryptFSBFile(fsb2);
                File.WriteAllBytes(decFSB2, dec);
                fsb2 = decFSB2;
            }
            if (File.Exists(decFSB2) && Tools.fsbIsEncrypted(decFSB2))
            {
                MessageBox.Show("File '" + Path.GetFileName(fsb2) + "' is encrypted and I failed to decrypt it\nCan't continue", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Tools.DeleteFile(decFSB2);
                return;
            }

            if (CancelWorkers) return;
            if (Tools.fsbIsEncrypted(fsb3))
            {
                var dec = Tools.DecryptFSBFile(fsb3);
                File.WriteAllBytes(decFSB3, dec);
                fsb3 = decFSB3;
            }
            if (File.Exists(decFSB3) && Tools.fsbIsEncrypted(decFSB3))
            {
                MessageBox.Show("File '" + Path.GetFileName(fsb3) + "' is encrypted and I failed to decrypt it\nCan't continue", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Tools.DeleteFile(decFSB3);
                return;
            }

            //extract drum tracks
            const int frame = 384;//size of each frame of audio
            const int spacer1 = 1152;//the spacer from one track past the other three
            const int spacer2 = 768;//the spacer from one track past the other two
            const int spacer3 = 384;//the spacer from one track past the other

            var kick_audio = ext_path + "\\drums_1.mp3";
            var snare_audio = ext_path + "\\drums_2.mp3";
            var cymbal_audio = ext_path + "\\drums_3.mp3";
            var tom_audio = ext_path + "\\drums_4.mp3";
            var guitar_audio = ext_path + "\\guitar.mp3";
            var bass_audio = ext_path + "\\bass.mp3";
            var vocals_audio = ext_path + "\\vocals.mp3";
            var backing_audio = ext_path + "\\song.mp3";
            var crowd_audio = ext_path + "\\crowd.mp3";

            var kick_offset = 0x80;
            var snare_offset = kick_offset + frame;
            var cymbal_offset = snare_offset + frame;
            var tom_offset = cymbal_offset + frame;

            if (CancelWorkers) return;
            ExtractFSBAudio(fsb1, kick_offset, spacer1, kick_audio);
            ExtractFSBAudio(fsb1, snare_offset, spacer1, snare_audio);
            ExtractFSBAudio(fsb1, cymbal_offset, spacer1, cymbal_audio);
            ExtractFSBAudio(fsb1, tom_offset, spacer1, tom_audio);

            //extract guitar bass vocals
            var guitar_offset = 0x80;
            var bass_offset = guitar_offset + frame;
            var vocals_offset = bass_offset + frame;

            if (CancelWorkers) return;
            ExtractFSBAudio(fsb2, guitar_offset, spacer2, guitar_audio);
            ExtractFSBAudio(fsb2, bass_offset, spacer2, bass_audio);
            ExtractFSBAudio(fsb2, vocals_offset, spacer2, vocals_audio);

            //extract backing and crowd
            var backing_offset = 0x80;
            var crowd_offset = backing_offset + frame;

            if (CancelWorkers) return;
            ExtractFSBAudio(fsb3, backing_offset, spacer3, backing_audio);
            ExtractFSBAudio(fsb3, crowd_offset, spacer3, crowd_audio);

            mp3Files = Directory.GetFiles(ext_path, "*.mp3");
            if (mp3Files.Any())
            {
                //the metadata doesn't contain song data, let's get it from the mp3 files
                var stream = Bass.BASS_StreamCreateFile(mp3Files[0], 0L, File.ReadAllBytes(mp3Files[0]).Length, BASSFlag.BASS_SAMPLE_FLOAT);
                var len = Bass.BASS_ChannelGetLength(stream);
                var totaltime = Bass.BASS_ChannelBytes2Seconds(stream, len); // the total time length
                Parser.Songs[0].Length = (int)(totaltime * 1000);
                overrideSongLength = Parser.Songs[0].Length;
            }
            else
            {
                if (!message) return;
                MessageBox.Show("Failed to extract audio files from FSB files, can't play this song", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void ExtractFSBAudio(string fsb, long offset, int spacer, string mp3)
        {
            byte[] mp3_data;
            const int frame = 384;//size of each frame of audio

            using (var ms = new MemoryStream(File.ReadAllBytes(fsb)))
            {
                using (var br = new BinaryReader(ms))
                {
                    br.BaseStream.Seek(offset, SeekOrigin.Begin);
                    while (br.BaseStream.Position < br.BaseStream.Length)
                    {
                        mp3_data = br.ReadBytes(frame);
                        br.BaseStream.Seek(spacer, SeekOrigin.Current);

                        using (var bw = new BinaryWriter(new FileStream(mp3, FileMode.Append)))
                        {
                            bw.Write(mp3_data);
                        }
                    }
                }
            }
        }

        private void songLoader_DoWork(object sender, DoWorkEventArgs e)
        {
            Log("Song loader working");
            if (xbox360.Checked)
            {
                Log("loadCON: " + SongToLoad);
                loadCON(SongToLoad, true);
            }
            else if (yarg.Checked)
            {
                if (Path.GetFileName(SongToLoad) == ".yargsong")
                {
                    sngPath = SongToLoad;
                    pkgPath = "";
                    Log("DecryptExtractYARG: " + SongToLoad);
                    loadINI(SongToLoad, true);
                }
                else if (Path.GetExtension(SongToLoad) == ".sng")
                {
                    sngPath = SongToLoad;
                    Log("loadSNG: " + SongToLoad);
                    loadSNG(SongToLoad, true);
                }
                else if (Path.GetFileName(SongToLoad) == "songs.dta")
                {
                    sngPath = "";
                    pkgPath = "";
                    Log("loadDTA: " + SongToLoad);
                    loadDTA(SongToLoad, true);
                }
                else
                {
                    sngPath = "";
                    Log("loadINI: " + SongToLoad);
                    loadINI(SongToLoad, true);
                }
            }
            else if (rockSmith.Checked)
            {
                Log("loadPSARC: " + SongToLoad);
                loadPSARC(SongToLoad, true);
            }
            else if (powerGig.Checked)
            {
                Log("ExtractXMA: " + SongToLoad);
                ExtractXMA(SongToLoad, true);
            }
            else if (bandFuse.Checked)
            {
                BandFusePath = SongToLoad;
                Log("ExtractBandFuse: " + SongToLoad);
                ExtractBandFuse(SongToLoad, true);
            }
            else if (guitarHero.Checked)
            {
                ghwtPath = SongToLoad;
                Log("loadGHWTDE: " + SongToLoad);
                loadGHWT(SongToLoad, true);
            }
            else
            {
                if (pS3.Checked && Path.GetExtension(SongToLoad) == ".pkg")
                {
                    pkgPath = SongToLoad;
                    Log("loadPKG: " + SongToLoad);
                    loadPKG(SongToLoad, true);
                }
                else
                {
                    pkgPath = "";
                    ActiveSong.yargPath = "";
                    Log("loadDTA: " + SongToLoad);
                    loadDTA(SongToLoad, true);
                }
            }
        }

        private void batchSongLoader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Log("Batch song loader finished");
            if (SongsToAdd.Any() && !CancelWorkers)
            {
                Log("There are more songs left to be processed, moving to next song");
                batchSongLoader.RunWorkerAsync();
                return;
            }
            StaticPlaylist = Playlist;
            ReloadPlaylist(Playlist, false);
            isScanning = false;
            UpdateNotifyTray();
            if (GIFOverlay != null)
            {
                GIFOverlay.Close();
                GIFOverlay = null;
            }
            consoleToolStripMenuItem.Enabled = true;
            EnableDisable(true);
            CancelWorkers = false;
            if (WindowState == FormWindowState.Minimized)
            {
                NotifyTray_MouseDoubleClick(null, null);
            }
            AddedSongs();
        }

        private void AddedSongs()
        {
            var added = lstPlaylist.Items.Count - StartingCount;
            if (added == 0)
            {
                const string msg = "No new songs were added";
                Log(msg);
                ShowUpdate(msg);
            }
            else
            {
                var msg = "Added " + added + " new " + (added == 1 ? "song" : "songs");
                Log(msg);
                ShowUpdate(msg);
                MarkAsModified();
                if (PlayingSong == null) return;
                if (picShuffle.Tag.ToString() != "shuffle" && PlayingSong.Index == lstPlaylist.Items.Count - 1)
                {
                    GetNextSong();
                }
            }
        }

        private void UpdateNotifyTray()
        {
            string text;
            if (menuStrip1.InvokeRequired)
            {
                menuStrip1.Invoke(new MethodInvoker(() => consoleToolStripMenuItem.Enabled = !isScanning));
            }
            else
            {
                consoleToolStripMenuItem.Enabled = !isScanning;
            }
            if (isScanning)
            {
                text = "Scanning for songs...";
            }
            else if (Bass.BASS_ChannelIsActive(BassMixer) == BASSActive.BASS_ACTIVE_PLAYING)
            {
                var notify = "Playing: " + PlayingSong.Artist + " - " + PlayingSong.Name;
                text = notify.Length > 63 ? notify.Substring(0, 63) : notify;
            }
            else if (PlaybackSeconds == 0 || PlayingSong == null)
            {
                text = "Inactive";
            }
            else
            {
                var notify = "Paused: " + PlayingSong.Artist + " - " + PlayingSong.Name;
                text = notify.Length > 63 ? notify.Substring(0, 63) : notify;
            }
            Log("Updating notification tray text: " + text);
            NotifyTray.Text = text;
        }

        private void songLoader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Log("Song loader finished");
            StaticPlaylist = Playlist;
            ReloadPlaylist(Playlist, false);
            isScanning = batchSongLoader.IsBusy || songLoader.IsBusy;
            UpdateNotifyTray();
            consoleToolStripMenuItem.Enabled = !isScanning;
            EnableDisable(true);
            CancelWorkers = false;
            AddedSongs();
        }

        private void lstPlaylist_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (GIFOverlay != null)
            {
                Log("User tried to close program - please wait until the current process finishes");
                MessageBox.Show("Please wait until the current process finishes", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Cancel = true;
                return;
            }
            if (Text.Contains("*"))
            {
                Log("Tried to close with unsaved changes - confirm?");
                if (MessageBox.Show("You have unsaved changes on the current playlist\nAre you sure you want to do that?",
                    AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    Log("No");
                    e.Cancel = true;
                    return;
                }
                Log("Yes");
            }
            isClosing = true;
            StopPassthrough();
            StopPlayback();
            StopStageKit();
            Bass.BASS_Free();
            SaveConfig();
            DeleteUsedFiles();
            var folder = Application.StartupPath + "\\temp\\";
            Tools.DeleteFolder(folder, true);
        }

        private void SelectStageKitController(UserIndex index)
        {
            try
            {                
                stageKit = new StageKitController(((int)index) + 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating Stage Kit controller:\n" + ex.Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UncheckAllStageKits()
        {
            StopStageKit();
            controller1.Checked = false;
            controller2.Checked = false;
            controller3.Checked = false;
            controller4.Checked = false;
        }

        private void DeleteUsedFiles(bool all_files = true)
        {
            Log("Cleaning used and temporary files");
            //let's not leave over any files by mistake
            Tools.DeleteFile(Path.GetTempPath() + "o");
            Tools.DeleteFile(Path.GetTempPath() + "m");
            Tools.DeleteFile(Path.GetTempPath() + "temp");
            Tools.DeleteFolder(TempFolder, true);
            Tools.DeleteFile(NextSongArtBlurred);
            if (xbox360.Checked || pS3.Checked)
            {
                Tools.DeleteFile(NextSongMIDI);
            }
            if (!yarg.Checked && !fortNite.Checked && rockSmith.Checked && !guitarHero.Checked)
            {
                Tools.DeleteFile(NextSongArtPNG);
            }
            if (!all_files) return;
            Tools.DeleteFile(CurrentSongArtBlurred);
            if (xbox360.Checked || pS3.Checked)
            {
                Tools.DeleteFile(CurrentSongMIDI);
            }
            if (!yarg.Checked)
            {
                Tools.DeleteFile(CurrentSongArt);
            }
        }

        private void lstPlaylist_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstPlaylist.SelectedItems.Count == 0 || GIFOverlay != null) return;
            GetActiveSong(lstPlaylist.SelectedItems[0].SubItems[0]);
        }

        private void GetActiveSong(ListViewItem.ListViewSubItem item)
        {
            var index = Convert.ToInt16(item.Text) - 1;
            Playlist[index].Index = lstPlaylist.SelectedIndices[0];
            ActiveSong = Playlist[index];
            Log("Selected new song in playlist: index: " + index + " '" + ActiveSong.Artist + " - " + ActiveSong.Name + "'");
        }

        private void DoClickStop()
        {
            StopBASS();
            PlaybackTimer.Enabled = false;
            ClearVisuals();
            lblSections.Text = "";
            PlaybackSeconds = 0;
            StopPlayback();
            UpdateTime();
            UpdateNotifyTray();
        }

        public int[] ArrangeStreamChannels(int totalChannels, bool isOgg)
        {
            var channels = new int[totalChannels];
            if (isOgg)
            {
                switch (totalChannels)
                {
                    case 3:
                        channels[0] = 0;
                        channels[1] = 2;
                        channels[2] = 1;
                        break;
                    case 5:
                        channels[0] = 0;
                        channels[1] = 2;
                        channels[2] = 1;
                        channels[3] = 3;
                        channels[4] = 4;
                        break;
                    case 6:
                        channels[0] = 0;
                        channels[1] = 2;
                        channels[2] = 1;
                        channels[3] = 4;
                        channels[4] = 5;
                        channels[5] = 3;
                        break;
                    case 7:
                        channels[0] = 0;
                        channels[1] = 2;
                        channels[2] = 1;
                        channels[3] = 4;
                        channels[4] = 5;
                        channels[5] = 6;
                        channels[6] = 3;
                        break;
                    case 8:
                        channels[0] = 0;
                        channels[1] = 2;
                        channels[2] = 1;
                        channels[3] = 4;
                        channels[4] = 5;
                        channels[5] = 6;
                        channels[6] = 7;
                        channels[7] = 3;
                        break;
                    default:
                        goto DoAllChannels;
                }
                return channels;
            }
        DoAllChannels:
            for (var i = 0; i < totalChannels; i++)
            {
                channels[i] = i;
            }
            return channels;
        }

        public float[,] GetChannelMatrix(int chans)
        {
            //initialize matrix
            //matrix must be float[output_channels, input_channels]
            var matrix = new float[2, chans];
            var ArrangedChannels = ArrangeStreamChannels(chans, true);
            if (ActiveSongData.ChannelsDrums > 0 && doAudioDrums)
            {
                //for drums it's a bit tricky because of the possible combinations
                switch (ActiveSongData.ChannelsDrums)
                {
                    case 2:
                        //stereo kit
                        matrix = DoMatrixPanning(matrix, ArrangedChannels, 2, 0);
                        break;
                    case 3:
                        //mono kick
                        matrix = DoMatrixPanning(matrix, ArrangedChannels, 1, 0);
                        //stereo kit
                        matrix = DoMatrixPanning(matrix, ArrangedChannels, 2, 1);
                        break;
                    case 4:
                        //mono kick
                        matrix = DoMatrixPanning(matrix, ArrangedChannels, 1, 0);
                        //mono snare
                        matrix = DoMatrixPanning(matrix, ArrangedChannels, 1, 1);
                        //stereo kit
                        matrix = DoMatrixPanning(matrix, ArrangedChannels, 2, 2);
                        break;
                    case 5:
                        //mono kick
                        matrix = DoMatrixPanning(matrix, ArrangedChannels, 1, 0);
                        //stereo snare
                        matrix = DoMatrixPanning(matrix, ArrangedChannels, 2, 1);
                        //stereo kit
                        matrix = DoMatrixPanning(matrix, ArrangedChannels, 2, 3);
                        break;
                    case 6:
                        //stereo kick
                        matrix = DoMatrixPanning(matrix, ArrangedChannels, 2, 0);
                        //stereo snare
                        matrix = DoMatrixPanning(matrix, ArrangedChannels, 2, 2);
                        //stereo kit
                        matrix = DoMatrixPanning(matrix, ArrangedChannels, 2, 4);
                        break;
                }
            }
            //var channel = song.ChannelsDrums;
            if (ActiveSongData.ChannelsBass > 0 && doAudioBass)
            {
                matrix = DoMatrixPanning(matrix, ArrangedChannels, ActiveSongData.ChannelsBass, ActiveSongData.ChannelsBassStart);//channel);
            }
            //channel = channel + song.ChannelsBass;
            if (ActiveSongData.ChannelsGuitar > 0 && doAudioGuitar)
            {
                matrix = DoMatrixPanning(matrix, ArrangedChannels, ActiveSongData.ChannelsGuitar, ActiveSongData.ChannelsGuitarStart);//channel);
            }
            //channel = channel + song.ChannelsGuitar;
            if (ActiveSongData.ChannelsVocals > 0 && doAudioVocals)
            {
                matrix = DoMatrixPanning(matrix, ArrangedChannels, ActiveSongData.ChannelsVocals, ActiveSongData.ChannelsVocalsStart);//channel);
            }
            //channel = channel + song.ChannelsVocals;
            if (ActiveSongData.ChannelsKeys > 0 && doAudioKeys)
            {
                matrix = DoMatrixPanning(matrix, ArrangedChannels, ActiveSongData.ChannelsKeys, ActiveSongData.ChannelsKeysStart);//channel);
            }
            //channel = channel + song.ChannelsKeys;
            if (ActiveSongData.ChannelsCrowd > 0 && doAudioCrowd)
            {
                matrix = DoMatrixPanning(matrix, ArrangedChannels, ActiveSongData.ChannelsCrowd, ActiveSongData.ChannelsCrowdStart);//channel);
            }
            //channel = channel + song.ChannelsCrowd;
            if (doAudioBacking) //song.ChannelsBacking > 0 &&  ---- should always be enabled per specifications
            {
                if (ActiveSongData.ChannelsTotal == 0 && chans > 0)
                {
                    ActiveSongData.ChannelsTotal = chans;
                }
                var backing = ActiveSongData.ChannelsTotal - ActiveSongData.ChannelsBass - ActiveSongData.ChannelsDrums - ActiveSongData.ChannelsGuitar - ActiveSongData.ChannelsKeys - ActiveSongData.ChannelsVocals - ActiveSongData.ChannelsCrowd;
                if (backing > 0) //backing not required 
                {
                    if (ActiveSongData.ChannelsCrowdStart + ActiveSongData.ChannelsCrowd == ActiveSongData.ChannelsTotal) //crowd channels are last
                    {
                        matrix = DoMatrixPanning(matrix, ArrangedChannels, backing, ActiveSongData.ChannelsCrowdStart - backing);//channel);                        
                    }
                    else
                    {
                        matrix = DoMatrixPanning(matrix, ArrangedChannels, backing, ActiveSongData.ChannelsTotal - backing);//channel);
                    }
                }
            }
            return matrix;
        }

        private float[,] DoMatrixPanning(float[,] in_matrix, IList<int> ArrangedChannels, int inst_channels, int curr_channel)
        {
            //by default matrix values will be 0 = 0 volume
            //if nothing is assigned here, it stays at 0 so that channel won't be played
            //otherwise we assign a volume level based on the dta volumes

            //initialize output matrix based on input matrix, just in case something fails there's something going out
            var matrix = in_matrix;

            //split attenuation and panning info from DTA file for index access
            var volumes = ActiveSongData.AttenuationValues.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
            var pans = ActiveSongData.PanningValues.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

            //BASS.NET lets us specify maximum volume when converting dB to Level
            //in case we want to change this later, it's only one value to change
            const double max_dB = 1.0;

            //technically we could do each channel, but Magma only allows us to specify volume per track, 
            //so both channels should have same volume, let's save a tiny bit of processing power
            float vol;
            try
            {
                vol = (float)Utils.DBToLevel(Convert.ToDouble(volumes[ArrangedChannels[curr_channel]]), max_dB);
            }
            catch (Exception)
            {
                vol = (float)1.0;
            }

            //assign volume level to channels in the matrix
            if (inst_channels == 2) //is it a stereo track
            {
                try
                {
                    //assign current channel (left) to left channel
                    matrix[0, ArrangedChannels[curr_channel]] = vol;
                }
                catch (Exception)
                { }
                try
                {
                    //assign next channel (right) to the right channel
                    matrix[1, ArrangedChannels[curr_channel + 1]] = vol;
                }
                catch (Exception)
                { }
            }
            else
            {
                //it's a mono track, let's assign based on the panning value
                double pan;
                try
                {
                    pan = Convert.ToDouble(pans[ArrangedChannels[curr_channel]]);
                }
                catch (Exception)
                {
                    pan = 0.0; // in case there's an error above, it gets centered
                }

                if (pan <= 0) //centered or left, assign it to the left channel
                {
                    matrix[0, ArrangedChannels[curr_channel]] = vol;
                }
                if (pan >= 0) //centered or right, assignt to the right channel
                {
                    matrix[1, ArrangedChannels[curr_channel]] = vol;
                }
            }
            return matrix;
        }

        private void UpdateTime(bool seek = false, bool update = false)
        {
            string time;
            var TimeSelection = seek ? PlaybackSeek : PlaybackSeconds;
            if (PlayingSong != null && TimeSelection * 1000 > PlayingSong.Length)
            {
                picNext_MouseClick(null, null);
                return;
            }
            if (TimeSelection >= 3600)
            {
                var hours = (int)(TimeSelection / 3600);
                var minutes = (int)(TimeSelection - (hours * 3600));
                var seconds = (int)(TimeSelection - (minutes * 60));
                time = hours + ":" + (minutes < 10 ? "0" : "") + minutes + ":" + (seconds < 10 ? "0" : "") + seconds;
            }
            else if (TimeSelection >= 60)
            {
                var minutes = (int)(TimeSelection / 60);
                var seconds = (int)(TimeSelection - (minutes * 60));
                time = minutes + ":" + (seconds < 10 ? "0" : "") + seconds;
            }
            else
            {
                time = "0:" + (TimeSelection < 10 ? "0" : "") + (int)TimeSelection;
            }
            if (lblTime.InvokeRequired)
            {
                lblTime.Invoke(new MethodInvoker(() => lblTime.Text = time));
            }
            else
            {
                lblTime.Text = time;
            }
            if (panelSlider.Cursor == Cursors.NoMoveHoriz || reset || PlayingSong == null) return;
            var percent = TimeSelection / ((double)PlayingSong.Length / 1000);
            panelSlider.Invoke(new MethodInvoker(() => panelSlider.Left = panelLine.Left + (int)((panelLine.Width - panelSlider.Width) * percent)));
            if (!update || displayKaraokeMode.Checked) return;
            DoPracticeSessions();
        }

        private void panelLine_MouseClick(object sender, MouseEventArgs e)
        {
            if (panelSlider.Cursor != Cursors.Hand || panelLine.Cursor != Cursors.Hand) return;
            if (e.Button == MouseButtons.Right && PracticeSessions != null && PracticeSessions.Any())
            {
                var selector = new SectionSelector(this, Cursor.Position);
                selector.Show();
                return;
            }
            if (e.Button != MouseButtons.Left) return;
            Log("panelLine_MouseClick");
            ClearVisuals();
            PlaybackSeconds = ((double)PlayingSong.Length / 1000) * ((double)(e.X - (panelSlider.Width / 2)) / (panelLine.Width - panelSlider.Width));
            if (Bass.BASS_ChannelIsActive(BassMixer) == BASSActive.BASS_ACTIVE_PAUSED || Bass.BASS_ChannelIsActive(BassMixer) == BASSActive.BASS_ACTIVE_PLAYING)
            {
                Log("Setting audio location based on user input: " + PlaybackSeconds + " seconds");
                SetPlayLocation(PlaybackSeconds);
                var track_vol = (float)Utils.DBToLevel(Convert.ToDouble(-1 * (MinVolume - VolumeLevel)), 1.0);
                Bass.BASS_ChannelSetAttribute(BassMixer, BASSAttribute.BASS_ATTRIB_VOL, track_vol);
            }
            UpdateTime(false, !PlaybackTimer.Enabled);
        }

        private void panelLine_MouseHover(object sender, EventArgs e)
        {
            if (PlayingSong == null) return;
            var mouse = panelLine.PointToClient(Cursor.Position);
            var time = ((double)PlayingSong.Length / 1000) * ((double)(mouse.X - (panelSlider.Width / 2)) / (panelLine.Width - panelSlider.Width));
            toolTip1.Show(GetJumpMessage(time).Trim(), panelLine, mouse.X, mouse.Y - 30, 1000);
        }

        public void UpdatePlayback(bool doFade)
        {
            if (Bass.BASS_ChannelIsActive(BassMixer) != BASSActive.BASS_ACTIVE_PLAYING) return;
            PlaybackTimer.Enabled = false;
            StopPlayback();
            StartPlayback(doFade, false);
        }

        private int ShuffleSongs(bool can_repeat = false)
        {
            Log("Shuffling songs");
            var random = new Random();
            int index;
            do
            {
                index = random.Next(0, lstPlaylist.Items.Count);
            }
            while ((PlayingSong != null && index == PlayingSong.Index) || (lstPlaylist.Items[index].Tag.ToString() == "1" && !can_repeat));
            Log("Index: " + index);
            return index;
        }

        private void playNowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log("playNowToolStripMenuItem_Click");
            doSongPreparer();
        }

        private void doSongPreparer()
        {
            GetActiveSong(lstPlaylist.SelectedItems[0].SubItems[0]);
            Log("Active song: '" + ActiveSong.Artist + " - " + ActiveSong.Name + "");
            NextSongIndex = lstPlaylist.SelectedIndices[0];
            EnableDisable(false);
            nautilus.NextSongOggData = new byte[0];
            nautilus.ReleaseStreamHandle(true);
            ActiveSong.yargPath = "";
            StopVideoPlayback(true);
            InitiateGIFOverlay();
            songPreparer.RunWorkerAsync();
        }

        private void UpdateHighlights()
        {
            for (var i = 0; i < lstPlaylist.Items.Count; i++)
            {
                lstPlaylist.Items[i].BackColor = Color.White;
                lstPlaylist.Items[i].ForeColor = lstPlaylist.Items[i].Tag.ToString() == "1" ? Color.Gray : Color.Black;
            }
            if (lstPlaylist.SelectedItems.Count <= 0) return;
            var index = PlayingSong == null || PlayingSong.Index >= lstPlaylist.Items.Count ? lstPlaylist.SelectedIndices[0] : PlayingSong.Index;
            lstPlaylist.EnsureVisible(index);
            if (PlayingSong == null) return;
            var it = Convert.ToInt16(lstPlaylist.Items[index].SubItems[0].Text) - 1;
            if (Playlist[it].Artist != PlayingSong.Artist || Playlist[it].Name != PlayingSong.Name) return;
            lstPlaylist.Items[index].BackColor = Color.BurlyWood;
            lstPlaylist.Items[index].Tag = 1; //played
        }

        private void songPreparer_DoWork(object sender, DoWorkEventArgs e)
        {
            Log("Song preparer working");
            if (songExtractor.IsBusy && NextSong.Location == ActiveSong.Location)
            {
                do
                {//wait here
                } while (songExtractor.IsBusy);
            }

            if (xbox360.Checked)
            {
                Log("loadCON: " + ActiveSong.Location);
                loadCON(ActiveSong.Location, false, false, false, true);
            }
            else if (yarg.Checked)
            {
                if (Path.GetExtension(ActiveSong.Location) == ".yargsong")
                {
                    sngPath = ActiveSong.Location;
                    Log("loadINI: " + ActiveSong.Location);
                    loadINI(ActiveSong.Location, false, false, false, true);
                }
                else if (Path.GetExtension(ActiveSong.Location) == ".sng")
                {
                    sngPath = ActiveSong.Location;
                    Log("loadSNG: " + ActiveSong.Location);
                    loadSNG(ActiveSong.Location, false, false, false, true);
                }
                else if (Path.GetFileName(ActiveSong.Location) == "songs.dta")
                {
                    pkgPath = "";
                    sngPath = "";
                    Log("loadDTA: " + ActiveSong.Location);
                    loadDTA(ActiveSong.Location, false, false, false, true);
                }
                else
                {
                    sngPath = "";
                    Log("loadINI: " + ActiveSong.Location);
                    loadINI(ActiveSong.Location, false, false, false, true);
                }
            }
            else if (fortNite.Checked)
            {
                Log("loadFNF: " + ActiveSong.Location);
                loadINI(ActiveSong.Location, false, false, false, true);
            }
            else if (guitarHero.Checked)
            {
                ghwtPath = ActiveSong.Location;
                Log("loadGHWT: " + ActiveSong.Location);
                loadGHWT(ActiveSong.Location, false, false, false, true);
            }
            else if (rockSmith.Checked)
            {
                Log("loadPSARC: " + ActiveSong.Location);
                loadPSARC(ActiveSong.Location, false, false, false, true);
            }
            else if (powerGig.Checked)
            {
                Log("ExtractXMA: " + ActiveSong.Location);
                ExtractXMA(ActiveSong.Location, false, false, false, true);
            }
            else if (bandFuse.Checked)
            {
                BandFusePath = ActiveSong.Location;
                Log("ExtractBandFuse: " + ActiveSong.Location);
                ExtractBandFuse(ActiveSong.Location, false, false, false, true);
            }
            else
            {
                if (pS3.Checked && Path.GetExtension(ActiveSong.Location) == ".pkg")
                {
                    pkgPath = ActiveSong.Location;
                    Log("loadPKG: " + ActiveSong.Location);
                    loadPKG(ActiveSong.Location, false, false, false, true);
                }
                else
                {
                    pkgPath = "";
                    Log("loadDTA: " + ActiveSong.Location);
                    loadDTA(ActiveSong.Location, false, false, false, true);
                }
            }
            if (yarg.Checked)
            {
                GetIntroOutroSilencePS();
            }
            else
            {
                GetIntroOutroSilence();
            }
        }

        private void songPreparer_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Log("Song preparer finished");
            MoveSongFiles();
            isScanning = batchSongLoader.IsBusy || songLoader.IsBusy;
            UpdateNotifyTray();
            if (GIFOverlay != null)
            {
                GIFOverlay.Close();
                GIFOverlay = null;
            }
            PrepareForPlayback();
            UpdateHighlights();
        }

        private void PrepareForPlayback() //mainly for GHWT:DE
        {
            Log("Preparing for playback");
            if ((!yarg.Checked && !fortNite.Checked && !guitarHero.Checked && !powerGig.Checked && !bandFuse.Checked) && (CurrentSongAudio == null || CurrentSongAudio.Length == 0))
            {
                if (AlreadyTried)
                {
                    Log("Can't play that song - audio file missing or encrypted");
                    MessageBox.Show("Unable to play that song - either the song files are in use by another program or the audio file is encrypted", AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    EnableDisable(true);
                    AlreadyTried = false;
                }
                else
                {
                    AlreadyTried = true;
                    lstPlaylist_MouseDoubleClick(null, null);
                }
                return;
            }
            ClearAll();
            EnableDisable(true);
            ChangeDisplay();
            var index = Convert.ToInt16(lstPlaylist.Items[NextSongIndex].SubItems[0].Text) - 1;
            lstPlaylist.Items[NextSongIndex].Tag = 1; //played
            PlayingSong = Playlist[index];
            PlayingSong.Index = NextSongIndex;
            lblArtist.Text = "Artist: " + PlayingSong.Artist;
            lblSong.Text = "Song: " + PlayingSong.Name;
            lblAlbum.Text = string.IsNullOrEmpty(PlayingSong.Album.Trim()) ? "" : "Album: " + PlayingSong.Album;
            lblGenre.Text = string.IsNullOrEmpty(PlayingSong.Genre.Trim()) ? "" : "Genre: " + PlayingSong.Genre;
            lblTrack.Text = string.IsNullOrEmpty(PlayingSong.Album.Trim()) ? "" : "Track #: " + PlayingSong.Track;
            lblTrack.Visible = PlayingSong.Track > 0;
            lblYear.Text = PlayingSong.Year == 0 ? "" : "Year: " + PlayingSong.Year;
            if (PlayingSong.Length == 0)
            {
                PlayingSong.Length = overrideSongLength; //to accomodate songs missing that info
            }
            lblDuration.Text = Parser.GetSongDuration(PlayingSong.Length.ToString(CultureInfo.InvariantCulture));
            lblAuthor.Text = string.IsNullOrEmpty(PlayingSong.Charter.Trim()) ? "" : "Author: " + PlayingSong.Charter.Trim();
            toolTip1.SetToolTip(lblArtist, lblArtist.Text);
            toolTip1.SetToolTip(lblSong, lblSong.Text);
            toolTip1.SetToolTip(lblAlbum, lblAlbum.Text);
            toolTip1.SetToolTip(lblGenre, lblGenre.Text);
            toolTip1.SetToolTip(lblTrack, lblTrack.Text);
            toolTip1.SetToolTip(lblYear, lblYear.Text);
            toolTip1.SetToolTip(lblAuthor, lblAuthor.Text);
            EnableDisableButtons(true);
            panelSlider.Cursor = Cursors.Hand;
            panelLine.Cursor = Cursors.Hand;
            SetVideoPlayerPath(PlayingSong.Location);
            if (!File.Exists(CurrentSongArt))
            {
                displayAlbumArt.Checked = false;
                if (!displayMIDIChartVisuals.Checked && !displayKaraokeMode.Checked)
                {
                    displayAudioSpectrum.Checked = true;
                }
                if (!displayAudioSpectrum.Checked)
                {
                    toolTip1.SetToolTip(picPreview, "Click to change spectrum style");
                }
            }
            UpdateButtons();
            UpdateDisplay();
            Log("Song to play is '" + PlayingSong.Artist + " - " + PlayingSong.Name + "'");
            StartPlayback(PlaybackSeconds == 0, true);
        }

        private void EnableDisableButtons(bool enabled)
        {
            picPlay.Enabled = enabled;
            picPause.Enabled = enabled;
            picStop.Enabled = enabled;
            picNext.Enabled = enabled;
        }

        private void PrepareForDrawing()
        {
            if (PlayingSong == null) return;
            Log("Preparing to draw MIDI file");
            MIDITools.Initialize(true);
            if (!MIDITools.ReadMIDIFile(CurrentSongMIDI))
            {
                Log("Failed - can't read that MIDI file - can't draw that song");
                ShowUpdate("Error reading MIDI file!");
                displayAudioSpectrum.Checked = true;
                displayMIDIChartVisuals.Checked = false;
            }
            else
            {
                Log("Success");
            }
            PracticeSessions = MIDITools.PracticeSessions;
            if (displayKaraokeMode.Checked && (!MIDITools.PhrasesVocals.Phrases.Any() || !MIDITools.LyricsVocals.Lyrics.Any()))
            {
                displayKaraokeMode.Checked = false;
                displayAudioSpectrum.Checked = true;
            }
            DrewFullChart = false;
            try
            {
                ChartBitmap = new Bitmap(picVisuals.Width, picVisuals.Height);
                Chart = Graphics.FromImage(ChartBitmap);
            }
            catch (Exception)
            { }
        }

        private int GetTrackstoDraw()
        {
            const int tall = 2;
            var tracks = 0;
            if (MIDITools.MIDI_Chart.Drums.ChartedNotes.Any() && doMIDIDrums)
            {
                tracks++;
            }
            if (MIDITools.MIDI_Chart.Bass.ChartedNotes.Any() && doMIDIBass)
            {
                tracks++;
            }
            if (MIDITools.MIDI_Chart.Guitar.ChartedNotes.Any() && doMIDIGuitar)
            {
                tracks++;
            }
            if (MIDITools.MIDI_Chart.Keys.ChartedNotes.Any() && doMIDIKeys)
            {
                tracks++;
            }
            else if (MIDITools.MIDI_Chart.ProKeys.ChartedNotes.Any() && doMIDIProKeys && PlayingSong.hasProKeys)
            {
                if (MIDITools.MIDI_Chart.ProKeys.NoteRange.Count > 8)
                {
                    tracks += tall;
                }
                else
                {
                    tracks++;
                }
            }
            if (!MIDITools.MIDI_Chart.Vocals.ChartedNotes.Any() || (!doMIDIVocals && !doMIDIHarmonies)) return tracks;
            if (MIDITools.MIDI_Chart.Vocals.NoteRange.Count > 8 || chartVertical.Checked)
            {
                tracks += tall;
            }
            else
            {
                tracks++;
            }
            return tracks;
        }

        private void DrawMIDIFile(Graphics graphics)
        {
            if (MIDITools.MIDI_Chart == null || MIDITools.PhrasesVocals == null) return;
            const int tall = 2;
            var tracks = GetTrackstoDraw();
            if (tracks == 0) return;
            var panel_height = picVisuals.Height - GetHeightDiff();
            var track_height = panel_height / tracks;
            var track_y = lblSections.Visible ? lblSections.Height : 0;
            int Index;
            var track_color = 1;
            picVisuals.BackColor = chartVertical.Checked ? Color.Black : Color.FromArgb(200, 200, 200);
            if (!chartVertical.Checked)
            {
                if (MIDITools.MIDI_Chart.Drums.ChartedNotes.Count > 0 && doMIDIDrums)
                {
                    track_y += track_height;
                    DrawTrackBackground(graphics, track_y, track_height, track_color, "DRUMS", MIDITools.MIDI_Chart.Drums.Solos, skActiveInstrument == Instrument.Drums, Instrument.Drums);
                    DrawNotes(graphics, MIDITools.MIDI_Chart.Drums, track_height, track_y, true, -1, out Index);
                    MIDITools.MIDI_Chart.Drums.ActiveIndex = Index;
                    track_color++;
                }
                if (MIDITools.MIDI_Chart.Bass.ChartedNotes.Count > 0 && doMIDIBass)
                {
                    track_y += track_height;
                    DrawTrackBackground(graphics, track_y, track_height, track_color, PlayingSong.isRhythmOnBass ? "RHYTHM GUITAR" : "BASS", MIDITools.MIDI_Chart.Bass.Solos, skActiveInstrument == Instrument.Bass, Instrument.Bass);
                    DrawNotes(graphics, MIDITools.MIDI_Chart.Bass, track_height, track_y, false, -1, out Index);
                    MIDITools.MIDI_Chart.Bass.ActiveIndex = Index;
                    track_color++;
                }
                if (MIDITools.MIDI_Chart.Guitar.ChartedNotes.Count > 0 && doMIDIGuitar)
                {
                    track_y += track_height;
                    DrawTrackBackground(graphics, track_y, track_height, track_color, "GUITAR", MIDITools.MIDI_Chart.Guitar.Solos, skActiveInstrument == Instrument.Guitar, Instrument.Guitar);
                    DrawNotes(graphics, MIDITools.MIDI_Chart.Guitar, track_height, track_y, false, -1, out Index);
                    MIDITools.MIDI_Chart.Guitar.ActiveIndex = Index;
                    track_color++;
                }
                if (MIDITools.MIDI_Chart.ProKeys.ChartedNotes.Count > 0 && PlayingSong.hasProKeys && doMIDIProKeys)
                {
                    var multKeys = 1;
                    if (MIDITools.MIDI_Chart.ProKeys.NoteRange.Count > 8)
                    {
                        multKeys = tall;
                    }
                    track_y += track_height * multKeys;
                    DrawTrackBackground(graphics, track_y, track_height * multKeys, track_color, "PRO KEYS", MIDITools.MIDI_Chart.ProKeys.Solos, skActiveInstrument == Instrument.ProKeys, Instrument.ProKeys);
                    DrawNotes(graphics, MIDITools.MIDI_Chart.ProKeys, track_height * multKeys, track_y, false, -1, out Index);
                    MIDITools.MIDI_Chart.ProKeys.ActiveIndex = Index;
                    track_color++;
                }
                else if (MIDITools.MIDI_Chart.Keys.ChartedNotes.Count > 0 && doMIDIKeys)
                {
                    track_y += track_height;
                    DrawTrackBackground(graphics, track_y, track_height, track_color, PlayingSong.isRhythmOnKeys ? "RHYTHM GUITAR" : "KEYS", MIDITools.MIDI_Chart.Keys.Solos, skActiveInstrument == Instrument.Keys, Instrument.Keys);
                    DrawNotes(graphics, MIDITools.MIDI_Chart.Keys, track_height, track_y, false, -1, out Index);
                    MIDITools.MIDI_Chart.Keys.ActiveIndex = Index;
                    track_color++;
                }
            }
            else
            {
                DrawRockBandStyle(graphics);
            }
            if (MIDITools.MIDI_Chart.Vocals.ChartedNotes.Count <= 0) return;
            var multVocals = 1;
            if (MIDITools.MIDI_Chart.Vocals.NoteRange.Count > 8 || chartVertical.Checked)
            {
                multVocals = tall;
            }
            if (doMIDIVocals || doMIDIHarmonies)
            {
                if (chartVertical.Checked)
                {
                    graphics.DrawImage(bmpBackgroundVocals, 0, 0, picVisuals.Width, vocalsHeight + 8);
                    DrawPhraseMarkers(graphics, MIDITools.PhrasesVocals, vocalsHeight, 4);
                    track_y = vocalsHeight;
                }
                else
                {
                    track_y += track_height * multVocals;
                    DrawTrackBackground(graphics, track_y, track_height * multVocals, track_color, MIDITools.MIDI_Chart.Harm1.ChartedNotes.Any() && doMIDIHarmonies ? "HARMONIES" : "VOCALS", null, false, Instrument.Vocals);
                    DrawPhraseMarkers(graphics, MIDITools.PhrasesVocals, track_height * multVocals, track_y);
                }
            }
            DrawLyrics(graphics, displayMIDIChartVisuals.Checked ? RBStyleVocalsBackgroundColor : Color.FromArgb(127, 200, 200, 200));
            if (!doMIDIVocals && !doMIDIHarmonies) return;
            if (MIDITools.MIDI_Chart.Harm3.ChartedNotes.Count > 0 && doMIDIHarmonies)
            {
                DrawNotes(graphics, MIDITools.MIDI_Chart.Harm3, track_height * multVocals, track_y, false, 3, out Index);
                MIDITools.MIDI_Chart.Harm3.ActiveIndex = Index;
            }
            if (MIDITools.MIDI_Chart.Harm2.ChartedNotes.Count > 0 && doMIDIHarmonies)
            {
                DrawNotes(graphics, MIDITools.MIDI_Chart.Harm2, track_height * multVocals, track_y, false, 2, out Index);
                MIDITools.MIDI_Chart.Harm2.ActiveIndex = Index;
            }
            if (MIDITools.MIDI_Chart.Harm1.ChartedNotes.Count > 0 && doMIDIHarmonies)
            {
                DrawNotes(graphics, MIDITools.MIDI_Chart.Harm1, track_height * multVocals, track_y, false, 1, out Index);
                MIDITools.MIDI_Chart.Harm1.ActiveIndex = Index;
            }
            else
            {
                DrawNotes(graphics, MIDITools.MIDI_Chart.Vocals, track_height * multVocals, track_y, false, 0, out Index);
                MIDITools.MIDI_Chart.Vocals.ActiveIndex = Index;
            }
            if (!chartVertical.Checked) return;
            DrawHitbox(graphics, bmpHitboxVocals, HitboxVocalsX, 4, bmpHitboxVocals.Width, vocalsHeight, 1, false, "");
        }

        private void DrawHitbox(Graphics graphics, Bitmap image, int posX, int posY, int width, int height, float opacity,  bool doHighlight, string trackName)
        {
            // Clamp transparency value between 0 (fully transparent) and 1 (fully opaque)
            float transparency = Math.Max(0, Math.Min(1, opacity));

            // Set up the color matrix with the desired transparency
            ColorMatrix colorMatrix = new ColorMatrix
            {
                Matrix33 = transparency // Set the alpha component
            };

            // Create an ImageAttributes object and apply the color matrix
            using (ImageAttributes attributes = new ImageAttributes())
            {
                attributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                if (doHighlight && stageKit != null)
                {
                    graphics.DrawRectangle(new Pen(Color.Goldenrod, 2), new Rectangle(posX - 1, posY - 1, width + 2, height + 2));
                }
                // Draw the image with the transparency applied
                graphics.DrawImage(image, new Rectangle(posX, posY, width, height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
            }

            if (!doMIDINameTracks || !chartVertical.Checked) return;
            Font font;
            try
            {
                font = new Font("Verdana", 12f, FontStyle.Bold);
            }
            catch (Exception)
            {
                font = new Font("Times New Roman", 10f, FontStyle.Bold);
            }

            var hitbox = new Rectangle(posX, posY, width, height);
            Size textSize = TextRenderer.MeasureText(graphics, trackName, font);
            int centeredX = hitbox.X + (hitbox.Width - textSize.Width) / 2;
            int centeredY = hitbox.Y + (hitbox.Height - textSize.Height) / 2;
            TextRenderer.DrawText(graphics, trackName, font, new Point(centeredX, centeredY), Color.FromArgb(127,0,0,0));
        }

        private void DrawRockBandStyle(Graphics graphics)
        {
            var tracks = 0;
            if (MIDITools.MIDI_Chart.Drums.ChartedNotes.Any() && doMIDIDrums)
            {
                tracks++;
            }
            if (MIDITools.MIDI_Chart.Bass.ChartedNotes.Any() && doMIDIBass)
            {
                tracks++;
            }
            if (MIDITools.MIDI_Chart.Guitar.ChartedNotes.Any() && doMIDIGuitar)
            {
                tracks++;
            }
            if (MIDITools.MIDI_Chart.Keys.ChartedNotes.Any() && doMIDIKeys)
            {
                tracks++;
            }
            else if (MIDITools.MIDI_Chart.ProKeys.ChartedNotes.Any() && doMIDIProKeys)
            {
                tracks += 2;
            }
            if (tracks == 0) return;

            var drumsX = 0;
            var bassX = 0;
            var guitarX = 0;
            var keysX = 0;
            var proKeysX = 0;
            const int maxTrackWidth = 400;
            const int normalPadding = 2;
            const int maximizedPadding = 10;

            // Determine padding based on the form's WindowState
            var padding = this.WindowState == FormWindowState.Maximized ? maximizedPadding : normalPadding;

            // Calculate the effective width of each track including padding
            var track_width = (picVisuals.Width - (padding * 2 * tracks)) / tracks;
            if (track_width > maxTrackWidth)
            {
                track_width = maxTrackWidth;
            }

            // Adjust the total width of all tracks including padding
            var totalTracksWidth = (track_width * tracks) + (padding * 2 * tracks);

            // Calculate starting X position to center the tracks
            var startX = (picVisuals.Width - totalTracksWidth) / 2;

            var track_height = picVisuals.Height; // Adjust as needed
            var y = picVisuals.Height - track_height;
            var lastX = startX; // Initialize to starting position

            skDrums = new Rectangle();
            skBass = new Rectangle();
            skGuitar = new Rectangle();
            skKeys = new Rectangle();
            skProKeys = new Rectangle();
            // Draw Drums track if present
            if (MIDITools.MIDI_Chart.Drums.ChartedNotes.Any() && doMIDIDrums)
            {
                var isSolo = MIDITools.MIDI_Chart.Drums.Solos != null && MIDITools.MIDI_Chart.Drums.Solos.Any(solo => solo.MarkerBegin <= PlaybackSeconds && solo.MarkerEnd > PlaybackSeconds);
                drumsX = lastX + padding;
                graphics.DrawImage(isSolo ? bmpBackgroundDrumsSolo : bmpBackgroundDrums, drumsX, y, track_width, track_height);
                DrawHitbox(graphics, bmpHitbox, drumsX, picVisuals.Height - 52, track_width, 30, 0.75f, skActiveInstrument == Instrument.Drums, "Pro Drums");
                skDrums = new Rectangle(drumsX, picVisuals.Height - 52, track_width, 30);
                lastX += track_width + (2 * padding); // Move to the next position
            }

            // Draw Bass track if present
            if (MIDITools.MIDI_Chart.Bass.ChartedNotes.Any() && doMIDIBass)
            {
                var isSolo = MIDITools.MIDI_Chart.Bass.Solos != null && MIDITools.MIDI_Chart.Bass.Solos.Any(solo => solo.MarkerBegin <= PlaybackSeconds && solo.MarkerEnd > PlaybackSeconds);
                bassX = lastX + padding;
                graphics.DrawImage(isSolo ? bmpBackgroundBassSolo : bmpBackgroundBass, bassX, y, track_width, track_height);
                DrawHitbox(graphics, bmpHitbox, bassX, picVisuals.Height - 52, track_width, 30, 0.75f, skActiveInstrument == Instrument.Bass, "Bass");
                skBass = new Rectangle(bassX, picVisuals.Height - 52, track_width, 30);
                lastX += track_width + (2 * padding);
            }

            // Draw Guitar track if present
            if (MIDITools.MIDI_Chart.Guitar.ChartedNotes.Any() && doMIDIGuitar)
            {
                var isSolo = MIDITools.MIDI_Chart.Guitar.Solos != null && MIDITools.MIDI_Chart.Guitar.Solos.Any(solo => solo.MarkerBegin <= PlaybackSeconds && solo.MarkerEnd > PlaybackSeconds);
                guitarX = lastX + padding;
                graphics.DrawImage(isSolo ? bmpBackgroundGuitarSolo : bmpBackgroundGuitar, guitarX, y, track_width, track_height);
                DrawHitbox(graphics, bmpHitbox, guitarX, picVisuals.Height - 52, track_width, 30, 0.75f, skActiveInstrument == Instrument.Guitar, "Guitar");
                skGuitar = new Rectangle(guitarX, picVisuals.Height - 52, track_width, 30);
                lastX += track_width + (2 * padding);
            }

            // Draw Keys or ProKeys track if present
            if (MIDITools.MIDI_Chart.Keys.ChartedNotes.Any() && doMIDIKeys)
            {
                var isSolo = MIDITools.MIDI_Chart.Keys.Solos != null && MIDITools.MIDI_Chart.Keys.Solos.Any(solo => solo.MarkerBegin <= PlaybackSeconds && solo.MarkerEnd > PlaybackSeconds);
                keysX = lastX + padding;
                graphics.DrawImage(isSolo ? bmpBackgroundKeysSolo : bmpBackgroundKeys, keysX, y, track_width, track_height);
                DrawHitbox(graphics, bmpHitbox, keysX, picVisuals.Height - 52, track_width, 30, 0.75f, skActiveInstrument == Instrument.Keys, "Keys");
                skKeys = new Rectangle(keysX, picVisuals.Height - 52, track_width, 30);
            }
            else if (MIDITools.MIDI_Chart.ProKeys.ChartedNotes.Any() && doMIDIProKeys)
            {
                var isSolo = MIDITools.MIDI_Chart.ProKeys.Solos != null && MIDITools.MIDI_Chart.ProKeys.Solos.Any(solo => solo.MarkerBegin <= PlaybackSeconds && solo.MarkerEnd > PlaybackSeconds);
                proKeysX = lastX + padding;
                graphics.DrawImage(isSolo ? bmpBackgroundProKeysSolo : bmpBackgroundProKeys, proKeysX, y, track_width * 2, track_height); // Keys may take up more space
                DrawHitbox(graphics, bmpHitbox, proKeysX, picVisuals.Height - 52, track_width * 2, 30, 0.65f, skActiveInstrument == Instrument.ProKeys, "Pro Keys");
                skProKeys = new Rectangle(proKeysX, picVisuals.Height - 52, track_width * 2, 30);
            }

            if (MIDITools.MIDI_Chart.Drums.ChartedNotes.Any() && doMIDIDrums)
            {
                DrawFills(graphics, MIDITools.MIDI_Chart.Drums, GetStartingPosition(), drumsX, track_width);
                DrawDrumNotes(graphics, true, GetStartingPosition(), drumsX, track_width);
                DrawDrumNotes(graphics, false, GetStartingPosition(), drumsX, track_width);
            }
            if (MIDITools.MIDI_Chart.Bass.ChartedNotes.Any() && doMIDIBass)
            {
                DrawFills(graphics, MIDITools.MIDI_Chart.Bass, GetStartingPosition(), bassX, track_width);
                DrawFiveLaneNotes(graphics, MIDITools.MIDI_Chart.Bass, GetStartingPosition(), bassX, track_width);
            }
            if (MIDITools.MIDI_Chart.Guitar.ChartedNotes.Any() && doMIDIGuitar)
            {
                DrawFills(graphics, MIDITools.MIDI_Chart.Guitar, GetStartingPosition(), guitarX, track_width);
                DrawFiveLaneNotes(graphics, MIDITools.MIDI_Chart.Guitar, GetStartingPosition(), guitarX, track_width);
            }
            if (MIDITools.MIDI_Chart.Keys.ChartedNotes.Any() && doMIDIKeys && !doMIDIProKeys)
            {
                DrawFills(graphics, MIDITools.MIDI_Chart.Keys, GetStartingPosition(), keysX, track_width);
                DrawFiveLaneNotes(graphics, MIDITools.MIDI_Chart.Keys, GetStartingPosition(), keysX, track_width);
            }
            if (MIDITools.MIDI_Chart.ProKeys.ChartedNotes.Any() && !doMIDIKeys && doMIDIProKeys)
            {
                DrawFills(graphics, MIDITools.MIDI_Chart.ProKeys, GetStartingPosition(), proKeysX, track_width * 2);
                DrawProKeysNotes(graphics, GetStartingPosition(), proKeysX, track_width * 2);
            }
            var Solo = doMIDIVocals && MIDITools.MIDI_Chart.Vocals.Solos != null && MIDITools.MIDI_Chart.Vocals.Solos.Any(solo => solo.MarkerBegin <= PlaybackSeconds && solo.MarkerEnd > PlaybackSeconds);
            if (!Solo && doMIDIHarmonies)
            {
                Solo = doMIDIVocals && MIDITools.MIDI_Chart.Harm1.Solos != null && MIDITools.MIDI_Chart.Harm1.Solos.Any(solo => solo.MarkerBegin <= PlaybackSeconds && solo.MarkerEnd > PlaybackSeconds);
            }
        }

        private int GetStartingPosition()
        {
            var startingPosition = GetHeightDiff();
            if (doKaraokeLyrics || doStaticLyrics || doScrollingLyrics)
            {
                if (MIDITools.MIDI_Chart.Vocals.ChartedNotes.Any())
                {
                    startingPosition += 20;
                    if (doHarmonyLyrics)
                    {
                        if (MIDITools.MIDI_Chart.Harm2.ChartedNotes.Any())
                        {
                            startingPosition += 20;
                        }
                        if (MIDITools.MIDI_Chart.Harm3.ChartedNotes.Any())
                        {
                            startingPosition += 20;
                        }
                    }
                }
            }
            if (!doMIDIVocals && !doMIDIHarmonies)
            {
                startingPosition = 0;
            }
            return startingPosition;
        }              

        private void DrawPhraseMarkers(Graphics graphics, PhraseCollection phrases, int track_height, int track_y)
        {
            var time = GetCorrectedTime();
            if (phrases == null || phrases.Phrases.Count == 0) return;

            // Adjust hitbox width based on mode
            var hitboxWidth = HitboxVocalsX + (bmpHitboxVocals.Width / 2);
            if (chartSnippet.Checked)
            {
                hitboxWidth = 0;
            }

            // Iterate through all phrases in the range
            for (var p = 0; p < phrases.Phrases.Count; p++)
            {
                if (phrases.Phrases[p].PhraseStart > time + (PlaybackWindowRBVocals * 2)) break; // Stop if beyond range
                if (phrases.Phrases[p].PhraseEnd < time) continue; // Skip if already passed

                // Calculate X position for the current phrase
                float normalizedTime = (float)((phrases.Phrases[p].PhraseStart - time) / PlaybackWindowRBVocals);
                var x = (float)(normalizedTime * (picVisuals.Width - hitboxWidth)) + hitboxWidth;

                // Prevent "skipping" issues by clamping x within visible bounds
                if (x < 0) x = 0;
                if (x > picVisuals.Width) x = picVisuals.Width;

                // Ensure the marker appears and disappears at the correct locations
                if (chartVertical.Checked && x < HitboxVocalsX) continue; // Vertical mode: disappear after hitbox
                if (chartSnippet.Checked && x < 0) continue; // Snippet mode: skip if outside screen bounds

                // Adjust positions for vertical and non-vertical modes
                int top = chartVertical.Checked ? 8 : track_y - track_height + 4;
                int height = chartVertical.Checked ? vocalsHeight - 8 : track_height - 8;
                const int width = 4;

                // Draw the phrase marker
                using (var solidBrush = new SolidBrush(Color.DarkGray))
                {
                    graphics.FillRectangle(solidBrush, x, top, width, height);
                }
            }
        }

        private void DrawTrackBackground(Graphics graphics, int y, int height, int index, string name, ICollection<SpecialMarker> solos, bool doHighlight, Instrument instrument)
        {
            if (!chartSnippet.Checked && !chartVertical.Checked) return;
            var is_solo = false;
            if (solos != null && solos.Count > 0 && doMIDIHighlightSolos)
            {
                if (solos.Any(solo => solo.MarkerBegin <= PlaybackSeconds && solo.MarkerEnd > PlaybackSeconds))
                {
                    is_solo = true;
                }
            }
            if (is_solo)
            {
                using (var DrawingPen = new SolidBrush(Color.LightSteelBlue))
                {
                    graphics.FillRectangle(DrawingPen, 0, y - height, picVisuals.Width, height);
                }
            }
            else
            {
                var color = index % 2 == 0 ? TrackBackgroundColor2 : TrackBackgroundColor1;
                using (var DrawingPen = new SolidBrush(chartVertical.Checked ? RBStyleVocalsBackgroundColor : color))
                {
                    var adjustedPosY = chartVertical.Checked ? y : y - height;
                    graphics.FillRectangle(DrawingPen, 0, adjustedPosY, picVisuals.Width, height);
                }
            }
            var rectangle = new Rectangle(0, y - height, picVisuals.Width, height);
            
            Font font;
            try
            {
                font = new Font("Tahoma", 10, FontStyle.Bold);
            }
            catch (Exception)
            {
                font = new Font("Times New Roman", 10, FontStyle.Bold);
            }
            var trackText = name + (is_solo ? " SOLO!" : "");
            // Measure the size of the text
            Size textSize = TextRenderer.MeasureText(graphics, trackText, font, rectangle.Size, TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);

            // Calculate the rectangle for the text
            int x = rectangle.X + (rectangle.Width - textSize.Width) / 2;
            int y2 = rectangle.Y + (rectangle.Height - textSize.Height) / 2;

            Rectangle textRectangle = new Rectangle(x-4, y2, textSize.Width+4, textSize.Height+4);

            switch (instrument)
            {
                case Instrument.Drums: skDrums = textRectangle; break;
                case Instrument.Bass: skBass = textRectangle; break;
                case Instrument.Guitar: skGuitar = textRectangle; break;
                case Instrument.Keys: skKeys = textRectangle; break;
                case Instrument.ProKeys: skProKeys = textRectangle; break;
            }

            if (!doMIDINameTracks || chartVertical.Checked) return;
            TextRenderer.DrawText(graphics, trackText, font, rectangle, index % 2 == 0 ? TrackBackgroundColor1 : TrackBackgroundColor2, TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);
            if (doHighlight && stageKit != null)
            {
                using (Pen outlinePen = new Pen(Color.Goldenrod, 2))
                {
                    graphics.DrawRectangle(outlinePen, textRectangle);
                }
            }
        }

        private void DrawNotes(Graphics graphics, MIDITrack track, int track_height, int track_y, bool drums, int harm, out int LastPlayedIndex)
        {
            LastPlayedIndex = track.ActiveIndex;
            var correctedTime = GetCorrectedTime();
            track_y++;
            track_height--;

            var window = 0.00;
            switch (track.Name)
            {
                case "Vocals":
                case "Harm1":
                case "Harm2":
                case "Harm3":
                    window = PlaybackWindowRBVocals;
                    break;
                default:
                    window = chartVertical.Checked ? PlaybackWindowRB : PlaybackWindow;
                    break;
            }

            // Filter notes to process only visible ones
            var filteredNotes = track.ChartedNotes.Where(note => note.NoteStart <= correctedTime + (window * 2)).ToList();

            for (var z = 0; z < filteredNotes.Count(); z++)
            {
                var note = filteredNotes[z];
                if (note.NoteEnd < correctedTime && !chartFull.Checked && !chartSnippet.Checked && !chartVertical.Checked) continue;
                if (note.NoteStart > correctedTime && !chartFull.Checked && !chartSnippet.Checked && !chartVertical.Checked) break;
                if ((chartSnippet.Checked || chartVertical.Checked) && note.NoteEnd < correctedTime) continue;
                if ((chartSnippet.Checked || chartVertical.Checked) && note.NoteStart > correctedTime + (window * 2)) break;
                LastPlayedIndex = z;
                if (doMIDIBWKeys && track.Name == "ProKeys")
                {
                    note.NoteColor = note.NoteName.Contains("#") ? Color.Black : Color.WhiteSmoke;
                }
                else if (note.NoteColor == Color.Empty)
                {
                    switch (harm)
                    {
                        case 0:
                            note.NoteColor = !doMIDIHarm1onVocals ? GetNoteColor(note.NoteNumber) : Harm1Color;
                            break;
                        case 1:
                            note.NoteColor = Harm1Color;
                            break;
                        case 2:
                            note.NoteColor = Harm2Color;
                            break;
                        case 3:
                            note.NoteColor = Harm3Color;
                            break;
                        default:
                            note.NoteColor = GetNoteColor(note.NoteNumber, drums);
                            break;
                    }
                }

                var note_width = ((note.NoteLength / (PlayingSong.Length / 1000.0)) * picVisuals.Width);
                if (note_width < 1.0)
                {
                    note_width = 1.0;
                }
                var x = (note.NoteStart - correctedTime) / window * (float)picVisuals.Width / 1.33f; //3 second equivalent for this mode, 2 second equivalent for game style mode

                // Specific logic for Vocals and Harmonies
                if (track.Name == "Vocals" || track.Name == "Harm1" || track.Name == "Harm2" || track.Name == "Harm3")
                {
                    var hitboxWidth = HitboxVocalsX + (bmpHitboxVocals.Width / 2);
                    if (chartSnippet.Checked)
                    {
                        hitboxWidth = 0;
                    }
                    x = (float)((note.NoteStart - correctedTime) / window * (picVisuals.Width - hitboxWidth) + hitboxWidth);

                    // Define vocal chart dimensions and note height
                    int vocalChartTop = chartVertical.Checked ? 4 : track_y - track_height;
                    int vocalChartHeight = chartVertical.Checked ? vocalsHeight : track_height;
                    int minNote = 36;
                    int maxNote = 84;
                    int noteRange = maxNote - minNote + 1;
                    double noteHeight = (double)vocalChartHeight / noteRange; //double the height for better visibility

                    // Calculate Y position based on note number
                    var y = vocalChartTop + (int)((maxNote - note.NoteNumber) * noteHeight);
                    double width = 0;

                    IEnumerable<Lyric> source = null;
                    switch (track.Name)
                    {
                        case "Vocals":
                            source = MIDITools.LyricsVocals.Lyrics;
                            break;
                        case "Harm1":
                            source = MIDITools.LyricsHarm1.Lyrics;
                            break;
                        case "Harm2":
                            source = MIDITools.LyricsHarm2.Lyrics;
                            break;
                        case "Harm3":
                            source = MIDITools.LyricsHarm3.Lyrics;
                            break;
                    }

                    var isUnpitched = false;
                    foreach (var lyric in source)
                    {
                        if (lyric.LyricStart == note.NoteStart)
                        {
                            isUnpitched = lyric.LyricText.Trim().EndsWith("#");
                            break;
                        }
                    }
                    if (chartFull.Checked)
                    {
                        using (var DrawingPen = new SolidBrush(note.NoteColor))
                        {
                            Chart.FillRectangle(DrawingPen, (float)x, y, (float)note_width, (float)noteHeight);
                        }
                    }
                    else if (chartSnippet.Checked || chartVertical.Checked)
                    {
                        width = ((note.NoteLength / window) * picVisuals.Width) * 0.8;
                        if (width < 1)
                        {
                            width = 1; //won't draw something less than one pixel wide, and we want something to show!
                        }
                        var adjustedHeight = (float)noteHeight * 2;
                        var adjustedY = y;
                        var alpha = 255;
                        if (isUnpitched)
                        {
                            adjustedHeight = chartVertical.Checked ? vocalsHeight + 8 : track_height;
                            adjustedY = chartVertical.Checked ? 0 : track_y - track_height;
                            alpha = 192;
                        }
                        using (var solidBrush = new SolidBrush(Color.FromArgb(alpha, note.NoteColor)))
                        {
                            graphics.FillRectangle(solidBrush, (float)x, adjustedY, (float)width, adjustedHeight);
                        }
                    }
                    if (isUnpitched) continue;

                    // Handle sustain slides for vocals
                    if (z + 1 < track.ChartedNotes.Count())
                    {
                        var nextNote = track.ChartedNotes[z + 1];
                        try
                        {
                            var str = "";
                            using (var enumerator = source?.Where(lyric => lyric.LyricStart == nextNote.NoteStart).GetEnumerator())
                            {
                                if (enumerator?.MoveNext() == true)
                                {
                                    str = enumerator.Current.LyricText;
                                }
                            }

                            if (!string.IsNullOrEmpty(str) && str.Replace("-", "").Replace("$", "").Trim() == "+")
                            {
                                var x2 = x + width;
                                var x3 = (float)((nextNote.NoteStart - correctedTime) / window * (picVisuals.Width - hitboxWidth) + hitboxWidth);
                                var y2 = y;
                                var y3 = vocalChartTop + (int)((maxNote - nextNote.NoteNumber) * noteHeight);

                                var pointF1 = new PointF((float)(x + width), (float)(y + (noteHeight * 2)));
                                var pointF2 = new PointF((float)(x + width), y);
                                var pointF3 = new PointF(x3, y3);
                                var pointF4 = new PointF(x3, (float)(y3 + (noteHeight * 2)));

                                using (var solidBrush = new SolidBrush(((track.Name == "Vocals" && doMIDIHarm1onVocals) || track.Name != "Vocals") ? note.NoteColor : Color.LightGray))
                                {
                                    graphics.FillPolygon(solidBrush, new[] { pointF1, pointF2, pointF3, pointF4 });
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            Log("Error drawing vocal slide: " + ex.Message);
                        }
                    }
                    if ((!doMIDINameVocals || (track.Name != "Vocals" && track.Name != "Harm1" && (track.Name != "Harm2" && track.Name != "Harm3")))) continue;
                    var font = new Font("Impact", 12.0f);
                    TextRenderer.DrawText(graphics, note.NoteName, font, new Point((int)x + 1, y - 1), Color.White, TextFormatFlags.NoPadding);

                    continue; // Skip the rest of the logic for vocals and harmonies
                }

                // General logic for other tracks
                var y_general = track_y;
                List<int> range;
                switch (NoteSizingType)
                {
                    default:
                        if (track.Name == "ProKeys")
                        {
                            range = track.NoteRange; //for these only use present note range for measuring
                        }
                        else
                        {
                            range = track.ValidNotes; //use all possible notes for measuring
                        }
                        break;
                    case 1:
                        //only use present note range for measuring
                        range = track.NoteRange;
                        break;
                    case 2:
                        //use all possible notes for measuring
                        range = track.ValidNotes;
                        break;
                }

                var note_height_general = (double)track_height / range.Count;

                for (var i = 0; i < range.Count; i++)
                {
                    if (note.NoteNumber != range[i]) continue;
                    y_general = (int)(track_y - (note_height_general * (range.Count - i)));
                    break;
                }

                if (chartFull.Checked)
                {
                    using (var DrawingPen = new SolidBrush(note.NoteColor))
                    {
                        Chart.FillRectangle(DrawingPen, (float)x, y_general, (float)note_width, (float)note_height_general);
                    }
                }
                else if (chartSnippet.Checked)// || chartVertical.Checked)
                {
                    var width = ((note.NoteLength / window) * picVisuals.Width) * 0.8;
                    if (width < 1)
                    {
                        width = 1; //won't draw something less than one pixel wide, and we want something to show!
                    }
                    using (var solidBrush = new SolidBrush(note.NoteColor))
                    {
                        graphics.FillRectangle(solidBrush, (float)x, y_general, (float)width, (float)note_height_general);
                    }
                    if (track.Name == "ProKeys" && doMIDINameProKeys)
                    {
                        var font = new Font("Impact", 12.0f);
                        TextRenderer.DrawText(graphics, note.NoteName, font, new Point((int)x + 1, y_general - 1), (note.NoteName.Contains("#") ? Color.White : Color.Black), TextFormatFlags.NoPadding);
                    }
                }

                if (stageKit != null && note.NoteStart <= correctedTime + 0.1)
                {
                    if (skActiveInstrument == Instrument.Drums && track.Name == "Drums" ||
                        skActiveInstrument == Instrument.Bass && track.Name == "Bass" ||
                        skActiveInstrument == Instrument.Guitar && track.Name == "Guitar" ||
                        skActiveInstrument == Instrument.Keys && track.Name == "Keys")
                    {
                        GetLEDColorIndex(GetLEDColor(note.NoteColor, track.Name == "Drums"));
                    }                    
                    else if (skActiveInstrument == Instrument.ProKeys && track.Name == "ProKeys")
                    {
                        GetLEDColorIndexProKeys(note.NoteNumber);
                    }
                }
            }
        }

        private void GetLEDColorIndex(LEDColor color)
        {
            switch (color)
            {
                case LEDColor.Red:
                    activeRedLED = (activeRedLED + 1) % 8;
                    UpdateLED(LEDs[activeRedLED], true);
                    break;
                case LEDColor.Green:
                    activeGreenLED = (activeGreenLED + 1) % 8;
                    UpdateLED(LEDs[activeGreenLED + 8], true);
                    break;
                case LEDColor.Yellow:
                    activeYellowLED = (activeYellowLED + 1) % 8;
                    UpdateLED(LEDs[activeYellowLED + 16], true);
                    break;
                case LEDColor.Blue:
                    activeBlueLED = (activeBlueLED + 1) % 8;
                    UpdateLED(LEDs[activeBlueLED + 24], true);
                    break;
                case LEDColor.Orange:
                    activeRedLED = (activeRedLED + 1) % 8;
                    activeYellowLED = (activeYellowLED + 1) % 8;
                    UpdateLED(LEDs[activeRedLED], true);
                    UpdateLED(LEDs[activeYellowLED + 16], true);
                    break;
                case LEDColor.White:
                    UpdateLED(LEDs[32], true);
                    break;
            }
        }

        private void DisplayRedLed(int index, ref LedDisplay display, bool state)
        {
            switch (index)
            {
                case 0: stageKit.DisplayRedLed1(ref display, state); break;
                case 1: stageKit.DisplayRedLed2(ref display, state); break;
                case 2: stageKit.DisplayRedLed3(ref display, state); break;
                case 3: stageKit.DisplayRedLed4(ref display, state); break;
                case 4: stageKit.DisplayRedLed5(ref display, state); break;
                case 5: stageKit.DisplayRedLed6(ref display, state); break;
                case 6: stageKit.DisplayRedLed7(ref display, state); break;
                case 7: stageKit.DisplayRedLed8(ref display, state); break;
            }
        }

        private void DisplayGreenLed(int index, ref LedDisplay display, bool state)
        {
            switch (index)
            {
                case 0: stageKit.DisplayGreenLed1(ref display, state); break;
                case 1: stageKit.DisplayGreenLed2(ref display, state); break;
                case 2: stageKit.DisplayGreenLed3(ref display, state); break;
                case 3: stageKit.DisplayGreenLed4(ref display, state); break;
                case 4: stageKit.DisplayGreenLed5(ref display, state); break;
                case 5: stageKit.DisplayGreenLed6(ref display, state); break;
                case 6: stageKit.DisplayGreenLed7(ref display, state); break;
                case 7: stageKit.DisplayGreenLed8(ref display, state); break;
            }
        }

        private void DisplayYellowLed(int index, ref LedDisplay display, bool state)
        {
            switch (index)
            {
                case 0: stageKit.DisplayYellowLed1(ref display, state); break;
                case 1: stageKit.DisplayYellowLed2(ref display, state); break;
                case 2: stageKit.DisplayYellowLed3(ref display, state); break;
                case 3: stageKit.DisplayYellowLed4(ref display, state); break;
                case 4: stageKit.DisplayYellowLed5(ref display, state); break;
                case 5: stageKit.DisplayYellowLed6(ref display, state); break;
                case 6: stageKit.DisplayYellowLed7(ref display, state); break;
                case 7: stageKit.DisplayYellowLed8(ref display, state); break;
            }
        }

        private void DisplayBlueLed(int index, ref LedDisplay display, bool state)
        {
            switch (index)
            {
                case 0: stageKit.DisplayBlueLed1(ref display, state); break;
                case 1: stageKit.DisplayBlueLed2(ref display, state); break;
                case 2: stageKit.DisplayBlueLed3(ref display, state); break;
                case 3: stageKit.DisplayBlueLed4(ref display, state); break;
                case 4: stageKit.DisplayBlueLed5(ref display, state); break;
                case 5: stageKit.DisplayBlueLed6(ref display, state); break;
                case 6: stageKit.DisplayBlueLed7(ref display, state); break;
                case 7: stageKit.DisplayBlueLed8(ref display, state); break;
            }
        }

        private void UpdateLED(LED led, bool enabled)
        {
            if (stageKit == null || ledDisplay == null) return;

            if (enabled && !led.Enabled)
            {
                led.Enabled = true;
                led.Time = DateTime.Now.AddMilliseconds(150);

                // Call the appropriate display method based on color and index
                switch (led.Color)
                {
                    case LEDColor.Red:
                        DisplayRedLed(led.Index, ref ledDisplay, true);
                        break;
                    case LEDColor.Green:
                        DisplayGreenLed(led.Index, ref ledDisplay, true);
                        break;
                    case LEDColor.Yellow:
                        DisplayYellowLed(led.Index, ref ledDisplay, true);
                        break;
                    case LEDColor.Blue:
                        DisplayBlueLed(led.Index, ref ledDisplay, true);
                        break;
                    case LEDColor.White:
                        stageKit.TurnStrobeOn(StrobeSpeed.Medium);
                        break;
                }
            }
            else if (!enabled && led.Enabled)
            {
                led.Enabled = false;

                // Call the appropriate display method based on color and index
                switch (led.Color)
                {
                    case LEDColor.Red:
                        DisplayRedLed(led.Index, ref ledDisplay, false);
                        break;
                    case LEDColor.Green:
                        DisplayGreenLed(led.Index, ref ledDisplay, false);
                        break;
                    case LEDColor.Yellow:
                        DisplayYellowLed(led.Index, ref ledDisplay, false);
                        break;
                    case LEDColor.Blue:
                        DisplayBlueLed(led.Index, ref ledDisplay, false);
                        break;
                    case LEDColor.White:
                        stageKit.TurnStrobeOff();
                        break;
                }
            }
        }              

        private Color GetNoteColor(int note_number, bool drums = false)
        {
            Color color;
            switch (note_number)
            {
                case 36:
                case 48:
                case 60:
                case 72:
                case 84:
                case 96:
                    color = drums ? ChartOrange : ChartGreen;
                    break;
                case 37:
                case 49:
                case 61:
                case 73:
                case 97:
                    color = ChartRed;
                    break;
                case 38:
                case 50:
                case 62:
                case 74:
                case 98:
                case 110:
                    color = ChartYellow;
                    break;
                case 39:
                case 51:
                case 63:
                case 75:
                case 99:
                case 111:
                    color = ChartBlue;
                    break;
                case 40:
                case 52:
                case 64:
                case 76:
                case 100:
                case 112:
                    color = drums ? ChartGreen : ChartOrange;
                    break;
                case 41:
                case 53:
                case 65:
                case 77:
                    color = Color.FromArgb(183, 0, 174);
                    break;
                case 42:
                case 54:
                case 66:
                case 78:
                    color = Color.FromArgb(114, 86, 0);
                    break;
                case 43:
                case 55:
                case 67:
                case 79:
                case 103:
                case 115:
                    color = Color.FromArgb(0, 20, 130);
                    break;
                case 44:
                case 56:
                case 68:
                case 80:
                    color = Color.FromArgb(246, 200, 55);
                    break;
                case 45:
                case 57:
                case 69:
                case 81:
                    color = Color.FromArgb(64, 64, 64);
                    break;
                case 46:
                case 58:
                case 70:
                case 82:
                    color = Color.FromArgb(0, 194, 229);
                    break;
                case 47:
                case 59:
                case 71:
                case 83:
                    color = Color.FromArgb(114, 0, byte.MaxValue);
                    break;
                default:
                    color = Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue);
                    break;
            }
            return color;
        }

        private void UpdateButtons()
        {
            toolTip1.SetToolTip(picShuffle, picShuffle.Tag.ToString() == "shuffle" ? "Disable track shuffling" : "Enable track shuffling");
            toolTip1.SetToolTip(picLoop, picLoop.Tag.ToString() == "loop" ? "Disable track looping" : "Enable track looping");
        }

        private void panelSlider_MouseDown(object sender, MouseEventArgs e)
        {
            if (panelSlider.Cursor != Cursors.Hand || panelLine.Cursor != Cursors.Hand) return;
            panelSlider.Cursor = Cursors.NoMoveHoriz;
            mouseX = MousePosition.X;
        }

        private void panelSlider_MouseUp(object sender, MouseEventArgs e)
        {
            if (panelSlider.Cursor != Cursors.NoMoveHoriz || PlayingSong == null) return;
            panelSlider.Cursor = picPlay.Enabled ? Cursors.Hand : Cursors.Default;
            lblAuthor.Text = string.IsNullOrEmpty(PlayingSong.Charter.Trim()) ? "" : "Author: " + PlayingSong.Charter.Trim();
            PlaybackSeconds = PlaybackSeek;
            Log("Setting audio location based on user input: " + PlaybackSeconds + " seconds");
            UpdateTime(false, !PlaybackTimer.Enabled);
            if (MediaPlayer.playState == WMPPlayState.wmppsPlaying || MediaPlayer.playState == WMPPlayState.wmppsPaused)
            {
                MediaPlayer.Ctlcontrols.currentPosition = PlaybackSeconds;
            }
        }

        private void panelSlider_MouseMove(object sender, MouseEventArgs e)
        {
            if (panelSlider.Cursor != Cursors.NoMoveHoriz) return;
            if (MousePosition.X != mouseX)
            {
                if (MousePosition.X > mouseX)
                {
                    panelSlider.Left = panelSlider.Left + (MousePosition.X - mouseX);
                }
                else if (MousePosition.X < mouseX)
                {
                    panelSlider.Left = panelSlider.Left - (mouseX - MousePosition.X);
                }
                mouseX = MousePosition.X;
            }
            var min = panelLine.Left;
            var max = panelLine.Left + panelLine.Width - panelSlider.Width;
            if (panelSlider.Left < min)
            {
                panelSlider.Left = min;
            }
            else if (panelSlider.Left > max)
            {
                panelSlider.Left = max;
            }
            ClearVisuals();
            PlaybackSeek = (int)(((double)PlayingSong.Length / 1000) * ((double)(panelSlider.Left - panelLine.Left) / (panelLine.Width - panelSlider.Width)));
            if (PlaybackSeek < 0)
            {
                PlaybackSeek = 0;
            }
            else if (PlaybackSeek * 1000 > PlayingSong.Length)
            {
                PlaybackSeek = PlayingSong.Length / 1000;
            }
            lblAuthor.Text = GetJumpMessage(PlaybackSeek);
            UpdateTime(true);
            if (Bass.BASS_ChannelIsActive(BassMixer) != BASSActive.BASS_ACTIVE_PAUSED && Bass.BASS_ChannelIsActive(BassMixer) != BASSActive.BASS_ACTIVE_PLAYING) return;
            SetPlayLocation(PlaybackSeek, true);
            var track_vol = (float)Utils.DBToLevel(Convert.ToDouble(-1 * (MinVolume - VolumeLevel)), 1.0);
            Bass.BASS_ChannelSetAttribute(BassMixer, BASSAttribute.BASS_ATTRIB_VOL, track_vol);
        }

        private void picPreview_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || PlayingSong == null) return;
            if (!displayAlbumArt.Checked && File.Exists(CurrentSongArt))
            {
                Log("Displaying full size album art");
                var display = new Art(Cursor.Position, CurrentSongArt);
                display.Show();
                return;
            }
            if (!displayAlbumArt.Checked && (File.Exists(CurrentSongArt) || displayAudioSpectrum.Checked)) return;
            SpectrumID++;
            Log("Changed audio spectrum visualization - ID #" + SpectrumID);
            picPreview.Image = null;
            Spectrum.ClearPeaks();
        }

        private void lstPlaylist_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lstPlaylist.SelectedItems.Count != 1 || (GIFOverlay != null && !AlreadyTried)) return;
            if (songPreparer.IsBusy) return;
            Log("lstPlaylist_MouseDoubleClick");
            doSongPreparer();
        }

        private void MoveSongFiles()
        {
            Tools.MoveFile(NextSongArtBlurred, CurrentSongArtBlurred);
            if (yarg.Checked || fortNite.Checked || guitarHero.Checked)
            {
                CurrentSongArt = File.Exists(NextSongArtPNG) ? NextSongArtPNG : NextSongArtJPG;
                CurrentSongMIDI = NextSongMIDI;
                nautilus.PlayingSongOggData = nautilus.NextSongOggData;
                nautilus.NextSongOggData = new byte[0];
                nautilus.ReleaseStreamHandle(true);
                CurrentSongAudio = nautilus.PlayingSongOggData;
                return;
            }
            Tools.DeleteFile(CurrentSongArt);//delete left over from old song if this song doesn't have album art
            Tools.MoveFile(NextSongArtPNG, CurrentSongArt);
            if (nautilus.NextSongOggData != null && nautilus.NextSongOggData.Length > 0)
            {
                nautilus.PlayingSongOggData = nautilus.NextSongOggData;
                nautilus.NextSongOggData = new byte[0];
                nautilus.ReleaseStreamHandle(true);
            }
            if (wii.Checked)
            {
                CurrentSongMIDI = NextSongMIDI;
                CurrentSongAudio = nautilus.PlayingSongOggData;
                return;
            }
            Tools.MoveFile(NextSongMIDI, CurrentSongMIDI);
            CurrentSongAudio = nautilus.PlayingSongOggData;
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log("removeToolStripMenuItem_Click");
            var indexes = lstPlaylist.SelectedIndices;
            var savedIndex = lstPlaylist.SelectedIndices[0];
            var playing = Bass.BASS_ChannelIsActive(BassMixer) == BASSActive.BASS_ACTIVE_PLAYING;
            var to_remove = new List<int>();
            Log("Removing " + indexes.Count + " song(s) from the playlist");
            foreach (int index in indexes)
            {
                if (PlayingSong != null && index == PlayingSong.Index)
                {
                    ClearAll();
                    ClearVisuals(true);
                    DeleteUsedFiles();
                }
                to_remove.Add(Convert.ToInt16(lstPlaylist.Items[index].SubItems[0].Text) - 1);
            }
            to_remove.Sort();
            var ind = to_remove.Aggregate("", (current, t) => current + " t");
            Log("Indeces to remove: " + ind);
            for (var i = to_remove.Count - 1; i >= 0; i--)
            {
                var song = Playlist[to_remove[i]];
                Playlist.Remove(song);
                StaticPlaylist.Remove(song);
                Log("Removed song '" + song.Artist + " - " + song.Name + "'");
            }
            txtSearch.Text = strSearchPlaylist;
            ReloadPlaylist(Playlist, true, true, false);
            Log(lstPlaylist.Items.Count + " song(s) left in the playlist");
            if (lstPlaylist.Items.Count > 0)
            {
                if (savedIndex > lstPlaylist.Items.Count - indexes.Count)
                {
                    lstPlaylist.Items[lstPlaylist.Items.Count - indexes.Count].Selected = true;
                }
                else if (savedIndex < lstPlaylist.Items.Count)
                {
                    lstPlaylist.Items[savedIndex].Selected = true;
                }
                else
                {
                    lstPlaylist.Items[0].Selected = true;
                }
            }
            if (lstPlaylist.SelectedItems.Count > 0)
            {
                if (playing)
                {
                    lstPlaylist_MouseDoubleClick(null, null);
                }
                lstPlaylist.EnsureVisible(lstPlaylist.SelectedIndices[0]);
            }
            GetNextSong();
            UpdateHighlights();
            MarkAsModified();
        }

        private void MarkAsModified()
        {
            Text = Text.Replace("*", "") + "*";
        }

        public string CleanArtistSong(string input)
        {
            return string.IsNullOrEmpty(input) ? "" : (input.Replace("(RB3 version)", "").Replace("(2x Bass Pedal)", "").Replace("(Rhythm Version)", "").Replace("(Rhythm version)", "").Replace("featuring ", "ft. ").Replace("feat. ", "ft. ").Replace(" feat ", " ft. ").Replace("(feat ", ")ft. ")).Trim();
        }

        private void ReloadPlaylist(IList<Song> playlist, bool update = true, bool search = true, bool doExtract = true)
        {
            lstPlaylist.Items.Clear();
            lstPlaylist.Refresh();
            Log("Reloading playlist");

            var searchTerm = txtSearch.Text;
            lstPlaylist.BeginUpdate();
            for (var i = 0; i < playlist.Count; i++)
            {
                if (searchTerm != strSearchPlaylist && !string.IsNullOrEmpty(searchTerm.Trim()) && search)
                {
                    if (!playlist[i].Artist.ToLowerInvariant().Contains(searchTerm.ToLowerInvariant()) &&
                        !playlist[i].Name.ToLowerInvariant().Contains(searchTerm.ToLowerInvariant()))
                    {
                        continue;
                    }
                }

                //format leading index number
                var digits = 3; //999 songs
                var index = "000";
                if (playlist.Count > 99999)
                {
                    digits = 6; //999,999 songs ... unlikely but in case i'm not around
                    index = "000000";
                }
                else if (playlist.Count > 9999)
                {
                    digits = 5; //99,999 songs
                    index = "00000";
                }
                else if (playlist.Count > 999)
                {
                    digits = 4; //9,999 songs
                    index = "0000";
                }
                index = index + (i + 1);
                index = index.Substring(index.Length - digits, digits);

                //add entry to playlist panel
                var entry = new ListViewItem(index);
                entry.SubItems.Add(CleanArtistSong(playlist[i].Artist + " - " + CleanArtistSong(playlist[i].Name)));
                if (playlist[i].Length == 0)
                {
                    entry.SubItems.Add("");//we don't have song duration for Fornite Festival m4a files so blank it out at this point
                }
                else
                {
                    entry.SubItems.Add(Parser.GetSongDuration(playlist[i].Length.ToString(CultureInfo.InvariantCulture)));
                }
                entry.Tag = 0; //not played
                lstPlaylist.Items.Add(entry);
            }
            lstPlaylist.EndUpdate();

            var itemCount = lstPlaylist.Items.Count;
            if (itemCount > 0)
            {
                var ind = 0;
                if (PlayingSong != null && search)
                {
                    for (var i = 0; i < itemCount; i++)
                    {
                        var index = 0;
                        lstPlaylist.Invoke(new MethodInvoker(() => index = Convert.ToInt16(lstPlaylist.Items[i].SubItems[0].Text) - 1));
                        if (playlist[index].Artist != PlayingSong.Artist || playlist[index].Name != PlayingSong.Name) continue;
                        ind = i;
                        break;
                    }
                }
                lstPlaylist.Items[ind].Selected = true;
                lstPlaylist.Items[ind].Focused = true;
                lstPlaylist.EnsureVisible(ind);
                if (doExtract)
                {
                    GetNextSong();
                }
            }

            var msg = "Loaded " + itemCount + (itemCount == 1 ? " song" : " songs");
            Log(msg);
            if (!update) return;
            ShowUpdate(msg);
        }

        private void moveUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MoveSelectionUp();
        }

        private void MoveSelectionUp()
        {
            var itemsToBeMoved = lstPlaylist.SelectedItems.Cast<ListViewItem>().ToArray<ListViewItem>();
            var itemsToBeMovedEnum = itemsToBeMoved;
            foreach (var item in itemsToBeMovedEnum)
            {
                var index = item.Index - 1;
                lstPlaylist.Items.RemoveAt(item.Index);
                lstPlaylist.Items.Insert(index, item);
            }
            MarkAsModified();
        }

        private void moveDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MoveSelectionDown();
        }

        private void MoveSelectionDown()
        {
            var itemsToBeMoved = lstPlaylist.SelectedItems.Cast<ListViewItem>().ToArray<ListViewItem>();
            var itemsToBeMovedEnum = itemsToBeMoved.Reverse();
            foreach (var item in itemsToBeMovedEnum)
            {
                var index = item.Index + 1;
                lstPlaylist.Items.RemoveAt(item.Index);
                lstPlaylist.Items.Insert(index, item);
            }
            MarkAsModified();
        }

        private void playNextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log("playNextToolStripMenuItem_Click");
            var item = lstPlaylist.SelectedItems[0];
            item.Tag = 0;
            item.BackColor = Color.Black;
            item.ForeColor = Color.White;
            var substract = lstPlaylist.SelectedIndices[0] < PlayingSong.Index;
            lstPlaylist.Items.RemoveAt(item.Index);
            if (substract)
            {
                PlayingSong.Index--;
            }
            lstPlaylist.Items.Insert(PlayingSong.Index + 1, item);
            lstPlaylist.EnsureVisible(PlayingSong.Index);

            if (picShuffle.Tag.ToString() == "shuffle")
            {
                picShuffle_MouseClick(null, null);
            }
            else
            {
                GetNextSong();
            }
            MarkAsModified();
        }

        private void GetNextSong()
        {
            if (lstPlaylist.Items.Count == 0) return;
            Log("Getting next song index");
            var itemCount = lstPlaylist.Items.Count;
            if (picShuffle.Tag.ToString() == "shuffle" && itemCount > 1)
            {
                DoShuffleSongs();
                PlaybackTimer.Enabled = false;
                return;
            }
            else if (PlayingSong != null)
            {
                if (lstPlaylist.SelectedIndices.Count <= 0)
                {
                    NextSongIndex = PlayingSong.Index;
                }
                else if (PlayingSong.Index + 1 == itemCount)
                {
                    NextSongIndex = 0;
                }
                else
                {
                    NextSongIndex = PlayingSong.Index + 1;
                }
            }
            else
            {
                NextSongIndex = 0;
            }
            if (NextSongIndex >= itemCount)
            {
                NextSongIndex = itemCount - 1;
            }
            Log("Index: " + NextSongIndex);
            if (PlayingSong == null || NextSongIndex == PlayingSong.Index) return;
            var index = Convert.ToInt16(lstPlaylist.Items[NextSongIndex].SubItems[0].Text) - 1;
            NextSong = Playlist[index];
            if (songExtractor.IsBusy) return;
            Tools.DeleteFile(NextSongArtBlurred);
            if (xbox360.Checked)
            {
                Tools.DeleteFile(NextSongArtPNG);
                Tools.DeleteFile(NextSongMIDI);
            }
            else if (pS3.Checked)
            {
                Tools.DeleteFile(NextSongMIDI);
            }
            StopVideoPlayback(true);
            InitiateGIFOverlay();
            songExtractor.RunWorkerAsync();
        }

        private void saveCurrentPlaylist_Click(object sender, EventArgs e)
        {
            SavePlaylist(false);
        }

        private void SavePlaylist(bool force_new)
        {
            Log("Saving playlist: " + PlaylistPath);

            var vers = Assembly.GetExecutingAssembly().GetName().Version;
            var version = " v" + String.Format("{0}.{1}.{2}", vers.Major, vers.Minor, vers.Build);

            if (string.IsNullOrEmpty(PlaylistPath) || force_new)
            {
                const string message = "Enter playlist name:";
                var input = Interaction.InputBox(message, AppName);
                if (string.IsNullOrEmpty(input)) return;

                PlaylistName = input;
                PlaylistPath = Application.StartupPath + "\\playlists\\" + Tools.CleanString(input, true) + ".playlist";
                Tools.DeleteFile(PlaylistPath);
            }

            using (var sw = new StreamWriter(PlaylistPath, false))
            {
                sw.Write("//Created by " + AppName + version);
                sw.Write("\r\n//PlaylistConsole=" + PlayerConsole);
                sw.Write("\r\n//PlaylistName=" + PlaylistName);
                sw.Write("\r\n//TotalSongs=" + (force_new ? lstPlaylist.Items.Count : Playlist.Count));

                for (var i = 0; i < (force_new ? lstPlaylist.Items.Count : Playlist.Count); i++)
                {
                    var index = force_new ? (Convert.ToInt16(lstPlaylist.Items[i].SubItems[0].Text) - 1) : i;
                    var song = Playlist[index];

                    sw.Write("\r\n" + song.Artist + "\t");
                    sw.Write(song.Name + "\t");
                    sw.Write(song.Album + "\t");
                    sw.Write(song.Track.ToString(CultureInfo.InvariantCulture) + "\t");
                    sw.Write(song.Genre + "\t");
                    sw.Write(song.Year.ToString(CultureInfo.InvariantCulture) + "\t");
                    sw.Write(song.Length.ToString(CultureInfo.InvariantCulture) + "\t");
                    sw.Write(song.AttenuationValues + "\t");
                    sw.Write(song.PanningValues + "\t");
                    sw.Write(song.ChannelsDrums.ToString(CultureInfo.InvariantCulture) + "\t");
                    sw.Write(song.ChannelsBass.ToString(CultureInfo.InvariantCulture) + "\t");
                    sw.Write(song.ChannelsGuitar.ToString(CultureInfo.InvariantCulture) + "\t");
                    sw.Write(song.ChannelsVocals.ToString(CultureInfo.InvariantCulture) + "\t");
                    sw.Write(song.ChannelsKeys.ToString(CultureInfo.InvariantCulture) + "\t");
                    sw.Write(song.ChannelsCrowd.ToString(CultureInfo.InvariantCulture) + "\t");
                    sw.Write(song.ChannelsBacking.ToString(CultureInfo.InvariantCulture) + "\t");
                    sw.Write(song.Charter.ToString(CultureInfo.InvariantCulture) + "\t");
                    sw.Write(song.InternalName.ToString(CultureInfo.InvariantCulture) + "\t");
                    sw.Write(song.Location.ToString(CultureInfo.InvariantCulture) + "\t");
                    sw.Write(song.DTAIndex.ToString(CultureInfo.InvariantCulture) + "\t");
                    sw.Write(song.AddToPlaylist.ToString(CultureInfo.InvariantCulture) + "\t");
                    sw.Write(song.BPM.ToString(CultureInfo.InvariantCulture) + "\t");
                    sw.Write(song.isRhythmOnBass + "\t");
                    sw.Write(song.isRhythmOnKeys + "\t");
                    sw.Write(song.hasProKeys + "\t");
                    sw.Write(song.PSDelay);
                }
            }
            Log("Saved playlist with " + (force_new ? lstPlaylist.Items.Count : Playlist.Count) + " song(s)");
            UpdateRecentPlaylists(PlaylistPath);
            Text = AppName + " - " + PlaylistName;
        }

        private void loadExistingPlaylist_Click(object sender, EventArgs e)
        {
            if (Text.Contains("*"))
            {
                if (MessageBox.Show("You have unsaved changes on the current playlist\nAre you sure you want to do that?",
                        AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            }
            var ofd = new OpenFileDialog
            {
                Title = "Select " + AppName + " Playlist",
                Multiselect = false,
                InitialDirectory = Application.StartupPath + "\\playlists\\",
                Filter = AppName + " Playlist (*.playlist)|*.playlist",
            };
            if (ofd.ShowDialog() != DialogResult.OK)
            {
                ofd.Dispose();
                return;
            }
            StartNew(false);
            PrepareToLoadPlaylist(ofd.FileName);
            ofd.Dispose();
        }

        private void LoadPlaylist()
        {
            if (string.IsNullOrEmpty(PlaylistPath)) return;
            var showWait = false;
            Log("Loading playlist: " + PlaylistPath);
            if (!File.Exists(PlaylistPath))
            {
                Log("Can't find that file!");
                MessageBox.Show("Can't find that playlist file!", AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (GIFOverlay == null)
            {
                InitiateGIFOverlay();
                showWait = true;
            }
            var error = false;
            var sr = new StreamReader(PlaylistPath);
            try
            {
                var header = sr.ReadLine();
                if (!header.Contains("cPlayer"))
                {
                    Log("Not a valid cPlayer Playlist");
                    Log("Line: " + header);
                    MessageBox.Show("Not a valid cPlayer Playlist", AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    sr.Dispose();
                    if (GIFOverlay != null)
                    {
                        GIFOverlay.Close();
                        GIFOverlay = null;
                    }
                    return;
                }
                var console = Tools.GetConfigString(sr.ReadLine());
                if (console != PlayerConsole)
                {
                    var path = PlaylistPath;
                    switch (console)
                    {
                        case "xbox":
                            xbox360.PerformClick();
                            break;
                        case "wii":
                            wii.PerformClick();
                            break;
                        case "ps3":
                            pS3.PerformClick();
                            break;
                        case "yarg":
                            yarg.PerformClick();
                            break;
                        case "rocksmith":
                            rockSmith.PerformClick();
                            break;
                        case "fortnite":
                            fortNite.PerformClick();
                            break;
                        case "guitarhero":
                            guitarHero.PerformClick();
                            break;
                        case "bandfuse":
                            bandFuse.PerformClick();
                            break;
                        case "powergig":
                            powerGig.PerformClick();
                            break;
                    }
                    PlaylistPath = path;
                }
                ClearVisuals(true);
                Playlist = new List<Song>();
                StaticPlaylist = new List<Song>();
                ClearAll();
                PlayingSong = null;
                lstPlaylist.Items.Clear();
                lstPlaylist.Refresh();
                if (MIDITools.PhrasesVocals != null)
                {
                    MIDITools.PhrasesVocals.Phrases.Clear();
                }
                PlaylistName = Tools.GetConfigString(sr.ReadLine());
                var songcount = Convert.ToInt32(Tools.GetConfigString(sr.ReadLine()));
                var line_number = 4;
                for (var i = 0; i < songcount; i++)
                {
                    var line = "";
                    try
                    {
                        line_number++;
                        line = sr.ReadLine();
                        var song_info = line.Split(new[] { "\t" }, StringSplitOptions.None);
                        var song = new Song
                        {
                            Artist = song_info[0],
                            Name = song_info[1],
                            Album = song_info[2],
                            Track = Convert.ToInt16(song_info[3]),
                            Genre = song_info[4],
                            Year = Convert.ToInt16(song_info[5]),
                            Length = Convert.ToInt64(song_info[6]),
                            AttenuationValues = song_info[7],
                            PanningValues = song_info[8],
                            ChannelsDrums = Convert.ToInt16(song_info[9]),
                            ChannelsBass = Convert.ToInt16(song_info[10]),
                            ChannelsGuitar = Convert.ToInt16(song_info[11]),
                            ChannelsVocals = Convert.ToInt16(song_info[12]),
                            ChannelsKeys = Convert.ToInt16(song_info[13]),
                            ChannelsCrowd = Convert.ToInt16(song_info[14]),
                            ChannelsBacking = Convert.ToInt16(song_info[15]),
                            Charter = song_info[16],
                            InternalName = song_info[17],
                            Location = song_info[18],
                            DTAIndex = Convert.ToInt16(song_info[19]),
                            AddToPlaylist = song_info[20].Contains("True"),
                            Index = -1,
                            //v1.0 added BPM
                            BPM = song_info.Count() >= 22 ? Convert.ToDouble(song_info[21]) : 120, //default value if not already stored
                            //v2.0 added isRhythmOnBass, isRhythmOnKeys, hasProKeys
                            isRhythmOnBass = song_info.Count() >= 25 && song_info[22].Contains("True"),
                            isRhythmOnKeys = song_info.Count() >= 25 && song_info[23].Contains("True"),
                            hasProKeys = song_info.Count() >= 25 && song_info[24].Contains("True"),
                            //v2.1.1 added Phase Shift Delay
                            PSDelay = song_info.Count() >= 26 ? Convert.ToInt16(song_info[25]) : 0
                        };
                        if (File.Exists(song.Location))
                        {
                            Playlist.Add(song);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log("Error loading playlist: " + ex.Message);
                        Log("Line #" + line_number + ": " + line);
                        error = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Log("Error: " + ex.Message);
                MessageBox.Show("Error loading that Playlist\nError: " + ex.Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            sr.Dispose();

            if (error)
            {
                if (Playlist.Any())
                {
                    var msg = "Some of the song entries in that playlist were corrupt or in a format I wasn't expecting\nPlease don't modify the playlist files manually\n\nI was able to recover " + Playlist.Count + (Playlist.Count == 1 ? " song" : " songs") + " :-)\n\nSee the log file to track down the problem song(s)";
                    Log(msg);
                    MessageBox.Show(msg, AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    if (showWait)
                    {
                        if (GIFOverlay != null)
                        {
                            GIFOverlay.Close();
                            GIFOverlay = null;
                        }
                    }
                    const string msg = "Some of the song entries in that playlist were corrupt or in a format I wasn't expecting\nPlease don't modify the playlist files manually\n\nUnfortunately I wasn't able to recover any songs :-(\n\nSee the log file to track down the problem song(s)";
                    Log(msg);
                    MessageBox.Show(msg, AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            if (!Playlist.Any())
            {
                if (showWait)
                {
                    if (GIFOverlay != null)
                    {
                        GIFOverlay.Close();
                        GIFOverlay = null;
                    }
                }
                Log("Nothing could be loaded from that playlist");
                MessageBox.Show("Nothing could be loaded from that playlist", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            UpdateRecentPlaylists(PlaylistPath);
            StaticPlaylist = Playlist;
            ActiveSong = null;
            ReloadPlaylist(Playlist, true, true, false);
            Text = AppName + " - " + PlaylistName;
            if (showWait)
            {
                if (GIFOverlay != null)
                {
                    GIFOverlay.Close();
                    GIFOverlay = null;
                }
            }
            if (lstPlaylist.Items.Count == 0 || songExtractor.IsBusy || !autoPlay.Checked) return;
            if (autoPlay.Checked && picShuffle.Tag.ToString() == "shuffle")
            {
                lstPlaylist.Items[0].Selected = false;
                lstPlaylist.Items[ShuffleSongs()].Selected = true;
            }
            lstPlaylist_MouseDoubleClick(null, null);
        }

        private void UpdateRecentPlaylists(string playlist)
        {
            if (!string.IsNullOrEmpty(playlist))
            {
                //remove if already in list
                for (var i = 0; i < 5; i++)
                {
                    if (RecentPlaylists[i] == playlist)
                    {
                        RecentPlaylists[i] = "";
                    }
                }
                //move down playlists
                for (var i = 4; i > 0; i--)
                {
                    RecentPlaylists[i] = RecentPlaylists[i - 1];
                }
                RecentPlaylists[0] = playlist; //add newest one to the top
            }
            recent1.Visible = false;
            recent2.Visible = false;
            recent3.Visible = false;
            recent4.Visible = false;
            recent5.Visible = false;
            recent1.Text = Path.GetFileName(RecentPlaylists[0]);
            recent1.Visible = !string.IsNullOrEmpty(recent1.Text) && File.Exists(RecentPlaylists[0]);
            recent2.Text = Path.GetFileName(RecentPlaylists[1]);
            recent2.Visible = !string.IsNullOrEmpty(recent2.Text) && File.Exists(RecentPlaylists[1]);
            recent3.Text = Path.GetFileName(RecentPlaylists[2]);
            recent3.Visible = !string.IsNullOrEmpty(recent3.Text) && File.Exists(RecentPlaylists[2]);
            recent4.Text = Path.GetFileName(RecentPlaylists[3]);
            recent4.Visible = !string.IsNullOrEmpty(recent4.Text) && File.Exists(RecentPlaylists[3]);
            recent5.Text = Path.GetFileName(RecentPlaylists[4]);
            recent5.Visible = !string.IsNullOrEmpty(recent5.Text) && File.Exists(RecentPlaylists[4]);
        }

        private void SaveConfig()
        {
            Log("Saving configuration file");
            using (var sw = new StreamWriter(config, false))
            {
                sw.WriteLine("PlayerConsole=" + PlayerConsole);
                sw.WriteLine("LoopPlayback=" + (picLoop.Tag != null && picLoop.Tag.ToString() == "loop"));
                sw.WriteLine("ShufflePlayback=" + (picShuffle.Tag != null && picShuffle.Tag.ToString() == "shuffle"));
                sw.WriteLine("AutoloadPlaylist=" + autoloadLastPlaylist.Checked);
                sw.WriteLine("LastPlaylist=" + PlaylistPath);
                sw.WriteLine("AutoPlay=" + autoPlay.Checked);
                sw.WriteLine("PlayCrowdTrack=" + doAudioCrowd);
                sw.WriteLine("EnableCrossFade=False //feature is deprecated");
                sw.WriteLine("CrossFadeLength=0.0 //feature is deprecated");
                sw.WriteLine("ShowPracticeSessions=" + showPracticeSections.Checked);
                sw.WriteLine("ShowLyrics=" + doStaticLyrics);
                sw.WriteLine("WholeWords=" + doWholeWordsLyrics);
                sw.WriteLine("GameSyllables=" + !doWholeWordsLyrics);
                sw.WriteLine("ShowMIDIVisuals=" + displayMIDIChartVisuals.Checked);
                sw.WriteLine("OpenSideWindow=" + openSideWindow.Checked);
                sw.WriteLine("VolumeLevel=" + VolumeLevel);
                sw.WriteLine("UseGameColors=False //feature is deprecated");
                sw.WriteLine("UseRandomColors=False //feature is deprecated");
                sw.WriteLine("InvertAllColors=False //feature is deprecated"); //keep this old line from prior version so as not to break config compatibility
                sw.WriteLine("ModeBPM=False //feature is deprecated");
                sw.WriteLine("ModeAbstract=False //feature is deprecated");
                sw.WriteLine("DrawFullChart=" + chartFull.Checked);
                sw.WriteLine("DrawSnippet=" + chartSnippet.Checked);
                sw.WriteLine("DrawNoteNames=False //feature is deprecated");
                sw.WriteLine("DrawCircles=False //feature is deprecated");
                sw.WriteLine("DrawRectangles=False //feature is deprecated");
                sw.WriteLine("DrawLines=False //feature is deprecated");
                sw.WriteLine("DrawSpirals=False //feature is deprecated");
                sw.WriteLine("DrawRandomShapes=False //feature is deprecated");
                for (var i = 0; i < 5; i++)
                {
                    sw.WriteLine("RecentPlaylist" + (i + 1) + "=" + RecentPlaylists[i]);
                }
                sw.WriteLine("DrawLyrics=False //feature is deprecated");
                sw.WriteLine("DrawSpectrum=" + displayAudioSpectrum.Checked);
                sw.WriteLine("SpectrumID=" + SpectrumID);
                sw.WriteLine("DisplayAlbumArt=" + displayAlbumArt.Checked);
                sw.WriteLine("DisplayHarmonies=" + doHarmonyLyrics);
                sw.WriteLine("DontDisplayLyrics=" + (!doStaticLyrics && !doKaraokeLyrics && !doScrollingLyrics));
                sw.WriteLine("KaraokeLyrics=" + doKaraokeLyrics);
                sw.WriteLine("ScrollingLyrics=" + doScrollingLyrics);
                sw.WriteLine("LabelTracks=" + doMIDINameTracks);
                sw.WriteLine("PlaybackWindow=" + PlaybackWindow);
                sw.WriteLine("NoteSizingType=" + NoteSizingType);
                sw.WriteLine("NameProKeysNotes=" + doMIDINameProKeys);
                sw.WriteLine("NameVocalNotes=" + doMIDINameVocals);
                sw.WriteLine("DisplayBackgroundVideo=" + displayBackgroundVideo.Checked);
                sw.WriteLine("StartMaximized=" + (WindowState == FormWindowState.Maximized));
                sw.WriteLine("HighlightSolos=" + doMIDIHighlightSolos);
                sw.WriteLine("UploadtoImgur=" + uploadScreenshots.Checked);
                sw.WriteLine("BWProKeys=" + doMIDIBWKeys);
                sw.WriteLine("UseHarm1ColorOnVocals=" + doMIDIHarm1onVocals);
                sw.WriteLine("UseKaraokeMode=" + displayKaraokeMode.Checked);
                sw.WriteLine("SkipIntroOutroSilence=" + skipIntroOutroSilence.Checked);
                sw.WriteLine("SilenceThreshold=" + SilenceThreshold);
                sw.WriteLine("FadeInLength=" + FadeLength);
            }
        }

        private void LoadConfig()
        {
            Log("Loading configuration file");
            if (!File.Exists(config))
            {
                Log("Not found");
                return;
            }
            var sr = new StreamReader(config);
            try
            {
                PlayerConsole = Tools.GetConfigString(sr.ReadLine());
                xbox360.Checked = false;
                pS3.Checked = false;
                wii.Checked = false;
                yarg.Checked = false;
                rockSmith.Checked = false;
                guitarHero.Checked = false;
                fortNite.Checked = false;
                powerGig.Checked = false;
                bandFuse.Checked = false;
                chartSnippet.Checked = false;
                chartFull.Checked = false;
                switch (PlayerConsole)
                {
                    case "xbox":
                        xbox360.Checked = true;
                        consoleToolStripMenuItem.Text = "Your console: Xbox 360 (Rock Band)";
                        break;
                    case "ps3":
                        pS3.Checked = true;
                        consoleToolStripMenuItem.Text = "Your console: PlayStation 3 (Rock Band)";
                        break;
                    case "wii":
                        wii.Checked = true;
                        consoleToolStripMenuItem.Text = "Your console: Wii (Rock Band)";
                        break;
                    case "yarg":
                        yarg.Checked = true;
                        consoleToolStripMenuItem.Text = "Your console: PC (YARG / Clone Hero)";
                        break;
                    case "rocksmith":
                        rockSmith.Checked = true;
                        consoleToolStripMenuItem.Text = "Your console: PC (Rocksmith 2014)";
                        break;
                    case "guitarhero":
                        guitarHero.Checked = true;
                        consoleToolStripMenuItem.Text = "Your console: PC (GHWT:DE)";
                        break;
                    case "fortnite":
                        fortNite.Checked = true;
                        consoleToolStripMenuItem.Text = "Your console: PC (Fortnite Festival)";
                        break;
                    case "bandfuse":
                        bandFuse.Checked = true;
                        consoleToolStripMenuItem.Text = "Your console: Xbox 360 (BandFuse)";
                        break;
                    case "powergig":
                        powerGig.Checked = true;
                        consoleToolStripMenuItem.Text = "Your console: PC (Power Gig)";
                        break;
                }
                var loop = sr.ReadLine().Contains("True");
                picLoop.Tag = !loop ? "noloop" : "loop";
                toolTip1.SetToolTip(picLoop, loop ? "Disable track looping" : "Enable track looping");
                if (picLoop.Tag.ToString() == "loop")
                {
                    picLoop.Image = Resources.icon_loop_enabled;
                }
                var shuffle = sr.ReadLine().Contains("True");
                picShuffle.Tag = !shuffle ? "noshuffle" : "shuffle";
                toolTip1.SetToolTip(picShuffle, shuffle ? "Disable track shuffling" : "Enable track shuffling");
                if (picShuffle.Tag.ToString() == "shuffle")
                {
                    picShuffle.Image = Resources.icon_shuffle_enabled;
                }
                autoloadLastPlaylist.Checked = sr.ReadLine().Contains("True");
                PlaylistPath = Tools.GetConfigString(sr.ReadLine());
                autoPlay.Checked = sr.ReadLine().Contains("True");
                doAudioCrowd = sr.ReadLine().Contains("True");
                sr.ReadLine(); //no longer need this
                sr.ReadLine(); //no longer need this
                showPracticeSections.Checked = sr.ReadLine().Contains("True");
                doHarmonyLyrics = !sr.ReadLine().Contains("True");
                doWholeWordsLyrics = sr.ReadLine().Contains("True");
                sr.ReadLine(); //no longer need this
                displayMIDIChartVisuals.Checked = sr.ReadLine().Contains("True");
                openSideWindow.Checked = sr.ReadLine().Contains("True");
                VolumeLevel = Convert.ToDouble(Tools.GetConfigString(sr.ReadLine()));
                sr.ReadLine(); //no longer need this
                sr.ReadLine(); //no longer need this
                sr.ReadLine(); //no longer need this
                sr.ReadLine(); //no longer need this
                sr.ReadLine(); //no longer need this
                chartFull.Checked = sr.ReadLine().Contains("True");
                chartSnippet.Checked = sr.ReadLine().Contains("True");
                sr.ReadLine(); //no longer need this
                sr.ReadLine(); //no longer need this
                sr.ReadLine(); //no longer need this
                sr.ReadLine(); //no longer need this
                sr.ReadLine(); //no longer need this
                sr.ReadLine(); //no longer need this
                var playlists = new List<string>();
                for (var i = 0; i < 5; i++)
                {
                    var line = Tools.GetConfigString(sr.ReadLine());
                    if (!string.IsNullOrEmpty(line) && File.Exists(line))
                    {
                        playlists.Add(line);
                    }
                }
                for (var i = 0; i < playlists.Count; i++)
                {
                    RecentPlaylists[i] = playlists[i];
                }
                sr.ReadLine(); //no longer need this
                displayAudioSpectrum.Checked = sr.ReadLine().Contains("True");
                SpectrumID = Convert.ToInt16(Tools.GetConfigString(sr.ReadLine()));
                displayAlbumArt.Checked = sr.ReadLine().Contains("True");
                doHarmonyLyrics = sr.ReadLine().Contains("True");
                var no_lyrics = sr.ReadLine().Contains("True");
                doKaraokeLyrics = sr.ReadLine().Contains("True");
                doScrollingLyrics = sr.ReadLine().Contains("True");
                if (no_lyrics)
                {
                    doKaraokeLyrics = false;
                    doScrollingLyrics = false;
                }
                doMIDINameTracks = sr.ReadLine().Contains("True");
                PlaybackWindow = Convert.ToDouble(Tools.GetConfigString(sr.ReadLine()));
                NoteSizingType = Convert.ToInt16(Tools.GetConfigString(sr.ReadLine()));
                doMIDINameProKeys = sr.ReadLine().Contains("True");
                doMIDINameVocals = sr.ReadLine().Contains("True");
                displayBackgroundVideo.Checked = sr.ReadLine().Contains("True");
                playBGVideos.Checked = displayBackgroundVideo.Checked;
                if (sr.ReadLine().Contains("True"))
                {
                    WindowState = FormWindowState.Maximized;
                }
                doMIDIHighlightSolos = sr.ReadLine().Contains("True");
                uploadScreenshots.Checked = sr.ReadLine().Contains("True");
                doMIDIBWKeys = sr.ReadLine().Contains("True");
                doMIDIHarm1onVocals = sr.ReadLine().Contains("True");
                displayKaraokeMode.Checked = sr.ReadLine().Contains("True");
                skipIntroOutroSilence.Checked = sr.ReadLine().Contains("True");
                SilenceThreshold = float.Parse(Tools.GetConfigString(sr.ReadLine()));
                FadeLength = Convert.ToDouble(Tools.GetConfigString(sr.ReadLine()));
            }
            catch (Exception ex)
            {
                Log("Error loading config: " + ex.Message);
            }
            sr.Dispose();
            Log("Success");
            Log("Playlist console: " + PlayerConsole);
            styleToolStripMenuItem.Visible = displayMIDIChartVisuals.Checked;
            toolStripMenuItem8.Visible = displayMIDIChartVisuals.Checked;
            if (chartSnippet.Checked || chartFull.Checked)
            {
                chartVertical.Checked = false;
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var version = GetAppVersion();
            var message = AppName + " - The Rock Band Music Player\nVersion: " + version + "\n© TrojanNemo, 2014-2024\n\n";
            var credits = Tools.ReadHelpFile("credits");
            MessageBox.Show(message + credits, "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Log("Displayed About message");
        }

        private void picVolume_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            var Volume = new Volume(this, Cursor.Position);
            Volume.Show();
            Log("Displaying volume control form");
        }

        public void UpdateVolume(double volume)
        {
            if (PlayingSong == null) return;
            var track_vol = (float)Utils.DBToLevel(Convert.ToDouble(-1 * volume), 1.0);
            Bass.BASS_ChannelSetAttribute(BassMixer, BASSAttribute.BASS_ATTRIB_VOL, track_vol);
            Log("Update volume: " + track_vol);
            VolumeLevel = volume;
        }

        private void markAsUnplayed_Click(object sender, EventArgs e)
        {
            var indexes = lstPlaylist.SelectedIndices;
            foreach (int index in indexes)
            {
                lstPlaylist.Items[index].Tag = 0;
                lstPlaylist.Items[index].BackColor = Color.Black;
                lstPlaylist.Items[index].ForeColor = Color.White;
            }
            Log("Marked " + indexes.Count + " song(s)s as unplayed");
            GetNextSong();
        }

        private void markAsPlayed_Click(object sender, EventArgs e)
        {
            var indexes = lstPlaylist.SelectedIndices;
            foreach (int index in indexes)
            {
                lstPlaylist.Items[index].Tag = 1;
                lstPlaylist.Items[index].BackColor = Color.Black;
                lstPlaylist.Items[index].ForeColor = Color.Gray;
            }
            Log("Marked " + indexes.Count + " song(s)s as played");
            GetNextSong();
        }

        private enum PlaylistFilters
        {
            ByArtist, ByAlbum, ByGenre
        }

        private void FilterPlaylist(PlaylistFilters filter)
        {
            Log("Filtering playlist");
            Playlist = new List<Song>();
            foreach (var song in StaticPlaylist)
            {
                switch (filter)
                {
                    case PlaylistFilters.ByArtist:
                        if (CleanArtistSong(song.Artist).ToLowerInvariant().Contains(CleanArtistSong(ActiveSong.Artist).ToLowerInvariant()))
                        {
                            Playlist.Add(song);
                        }
                        break;
                    case PlaylistFilters.ByAlbum:
                        if (song.Album.Trim() == ActiveSong.Album.Trim())
                        {
                            Playlist.Add(song);
                        }
                        break;
                    case PlaylistFilters.ByGenre:
                        if (song.Genre.Trim() == ActiveSong.Genre.Trim())
                        {
                            Playlist.Add(song);
                        }
                        break;
                }
            }
            if (Playlist.Any())
            {
                switch (filter)
                {
                    case PlaylistFilters.ByArtist:
                        Playlist.Sort((a, b) => String.CompareOrdinal(a.Name.ToLowerInvariant(), b.Name.ToLowerInvariant()));
                        Log("Filtered by artist");
                        break;
                    case PlaylistFilters.ByAlbum:
                        Playlist.Sort((a, b) => a.Track.CompareTo(b.Track));
                        Log("Filtered by album");
                        break;
                    case PlaylistFilters.ByGenre:
                        Playlist.Sort((a, b) => String.CompareOrdinal(a.Artist.ToLowerInvariant(), b.Artist.ToLowerInvariant()));
                        Log("Filtered by genre");
                        break;
                }
            }
            txtSearch.Text = strSearchPlaylist;
            ReloadPlaylist(Playlist);
            UpdateButtons();
            UpdateHighlights();
        }

        private void goToArtist_Click(object sender, EventArgs e)
        {
            FilterPlaylist(PlaylistFilters.ByArtist);
        }

        private void goToAlbum_Click(object sender, EventArgs e)
        {
            FilterPlaylist(PlaylistFilters.ByAlbum);
        }

        private void goToGenre_Click(object sender, EventArgs e)
        {
            FilterPlaylist(PlaylistFilters.ByGenre);
        }

        private void songExtractor_DoWork(object sender, DoWorkEventArgs e)
        {
            Log("Song extractor working");
            if (xbox360.Checked)
            {
                NextSong.yargPath = "";
                Log("loadCON: " + NextSong.Location);
                loadCON(NextSong.Location, false, false, true);
            }
            else if (yarg.Checked)
            {
                if (Path.GetExtension(NextSong.Location) == ".yargsong")
                {
                    sngPath = NextSong.Location;
                    Log("DecryptExtractYARG: " + NextSong.Location);
                    loadINI(NextSong.Location, false, false, true);
                }
                else if (Path.GetExtension(NextSong.Location) == ".sng")
                {
                    NextSong.yargPath = "";
                    sngPath = NextSong.Location;
                    Log("loadSNG: " + NextSong.Location);
                    loadSNG(NextSong.Location, false, false, true);
                }
                else if (Path.GetFileName(NextSong.Location) == "songs.dta")
                {
                    pkgPath = "";
                    Log("loadDTA: " + NextSong.Location);
                    loadDTA(NextSong.Location, false, false, true);
                }
                else
                {
                    NextSong.yargPath = "";
                    Log("loadINI: " + NextSong.Location);
                    loadINI(NextSong.Location, false, false, true);
                }
            }
            else if (rockSmith.Checked)
            {
                Log("loadPSARC: " + NextSong.Location);
                loadPSARC(NextSong.Location, false, false, true);
            }
            else if (powerGig.Checked)
            {
                Log("ExtractXMA: " + NextSong.Location);
                ExtractXMA(NextSong.Location, false, false, true);
            }
            else if (bandFuse.Checked)
            {
                BandFusePath = NextSong.Location;
                Log("ExtractBandFuse: " + NextSong.Location);
                ExtractBandFuse(NextSong.Location, false, false, true);
            }
            else if (fortNite.Checked)
            {
                Log("loadINI: " + NextSong.Location);
                loadINI(NextSong.Location, false, false, true);
            }
            else if (guitarHero.Checked)
            {
                ghwtPath = NextSong.Location;
                Log("loadGHWT: " + NextSong.Location);
                loadGHWT(NextSong.Location, false, false, true);
            }
            else
            {
                if (pS3.Checked && Path.GetExtension(NextSong.Location) == ".pkg")
                {
                    pkgPath = NextSong.Location;
                    Log("loadPKG: " + NextSong.Location);
                    loadPKG(NextSong.Location, false, false, true);
                }
                else
                {
                    pkgPath = "";
                    NextSong.yargPath = "";
                    Log("loadDTA: " + NextSong.Location);
                    loadDTA(NextSong.Location, false, false, true);
                }
            }

            if (yarg.Checked)
            {
                GetIntroOutroSilencePS();
            }
            else
            {
                GetIntroOutroSilence();
            }
        }

        private string DecryptExtractYARG(string inFile, bool message = false, bool scanning = true, bool next = false, bool prep = false)
        {
            byte[] SNGPKG = { (byte)'S', (byte)'N', (byte)'G', (byte)'P', (byte)'K', (byte)'G' };
            var tempFolder = Application.StartupPath + "\\bin\\temp";
            var tempFile = tempFolder + "\\temp.sng";
            Tools.DeleteFolder(tempFolder, true);
            if (!Directory.Exists(tempFolder))
            {
                Directory.CreateDirectory(tempFolder);
            }
            using (FileStream fileStream = File.OpenRead(inFile))
            {
                YARGSongFileStream yargFileStream = TryLoad(fileStream);
                byte[] bytes = new byte[yargFileStream.Length];
                yargFileStream.Read(bytes, 0, bytes.Length);
                yargFileStream.Close();
                using (var fs = File.Create(tempFile))
                {
                    using (var bw = new BinaryWriter(fs))
                    {
                        bw.Write(bytes);
                    }
                }
            }
            using (FileStream fileStream = new FileStream(tempFile, FileMode.Open, FileAccess.Write))
            {
                fileStream.Write(SNGPKG, 0, SNGPKG.Length);
            }
            if (!Tools.ExtractSNG(tempFile, tempFolder))
            {
                Tools.DeleteFile(tempFile);
                if (message)
                {
                    var choice = MessageBox.Show("Decrypting YARG .yargsong files requires .NET Desktop Runtime 7\n\nIf you already have .NET Desktop Runtime 7 installed and it still doesn't work, notify Nemo\n\nIf you don't have .NET Desktop Runtime 7 installed, click OK to go to the Microsoft website and download it from there\n\nOr Click Cancel to go back", Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                    if (choice == DialogResult.OK)
                    {
                        Process.Start("https://dotnet.microsoft.com/en-us/download/dotnet/7.0");
                    }
                }
                return "";
            }
            var ini = Directory.GetFiles(tempFolder, "song.ini", SearchOption.AllDirectories);
            Tools.DeleteFile(tempFile);
            return ini[0];
        }

        private void songExtractor_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Log("Song extractor finished");
            isScanning = batchSongLoader.IsBusy || songLoader.IsBusy;
            UpdateNotifyTray();
            //picLoop.Enabled = true;
            //picShuffle.Enabled = true;
            if (GIFOverlay != null)
            {
                GIFOverlay.Close();
                GIFOverlay = null;
            }
            if (PlayingSong.Index <= lstPlaylist.Items.Count - 1)
            {
                lstPlaylist.Items[PlayingSong.Index].Selected = false;
            }
            if (NextSongIndex > lstPlaylist.Items.Count - 1)
            {
                NextSongIndex = 0;
                DeleteUsedFiles(false);
            }
            lstPlaylist.Items[NextSongIndex].Selected = true;
            lstPlaylist.Items[NextSongIndex].Focused = true;
            lstPlaylist.EnsureVisible(NextSongIndex);
            if (!yarg.Checked && lstPlaylist.Items.Count > 0)
            {
                Log("Next song files not found, processing song files again");
                lstPlaylist_MouseDoubleClick(null, null);
                return;
            }
            Log("Preparing to play next song");
            PlaybackTimer.Enabled = false;
            MoveSongFiles();
            PrepareForPlayback();
            UpdateHighlights();
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                openSideWindow.Checked = true;
            }
            MediaPlayer.Height = picVisuals.Height - GetHeightDiff();
            MediaPlayer.Width = picVisuals.Width;
            lblUpdates.Left = (openSideWindow.Checked ? picVisuals.Left + picVisuals.Width : panelPlaying.Left + panelPlaying.Width) - lblUpdates.Width;
            if (WindowState != FormWindowState.Minimized) return;
            Log("Minimized to system tray");
            NotifyTray.ShowBalloonTip(250);
            Hide();
        }

        private void NotifyTray_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Show();
                WindowState = FormWindowState.Normal;
                Activate();
                Log("Restored from system tray");
                UpdateHighlights();
            }
            else
            {
                WindowState = FormWindowState.Minimized;
            }
        }

        private void restoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log("restoreToolStripMenuItem_Click");
            NotifyTray_MouseDoubleClick(null, null);
        }

        private void sortPlaylistByArtist_Click(object sender, EventArgs e)
        {
            SortPlaylist(PlaylistSorting.BySongArtist);
        }

        private void sortPlaylistBySong_Click(object sender, EventArgs e)
        {
            SortPlaylist(PlaylistSorting.BySongName);
        }

        private void sortPlaylistByDuration_Click(object sender, EventArgs e)
        {
            SortPlaylist(PlaylistSorting.BySongDuration);
        }

        private enum PlaylistSorting
        {
            BySongArtist, BySongName, BySongDuration, ByModifiedDate, Shuffle
        }

        private void SortPlaylist(PlaylistSorting sort)
        {
            Log("Sorting playlist");
            SortingStyle = sort;
            switch (SortingStyle)
            {
                case PlaylistSorting.BySongArtist:
                    Playlist.Sort((a, b) => String.CompareOrdinal(a.Artist.ToLowerInvariant() + " - " + a.Name.ToLowerInvariant(), b.Artist.ToLowerInvariant() + " - " + b.Name.ToLowerInvariant()));
                    Log("Sorted by artist name");
                    break;
                case PlaylistSorting.BySongName:
                    Playlist.Sort((a, b) => String.CompareOrdinal(a.Name.ToLowerInvariant() + " - " + a.Artist.ToLowerInvariant(), b.Name.ToLowerInvariant() + " - " + b.Artist.ToLowerInvariant()));
                    Log("Sorted by song title");
                    break;
                case PlaylistSorting.BySongDuration:
                    Playlist.Sort((a, b) => a.Length.CompareTo(b.Length));
                    Log("Sorted by song duration");
                    break;
                case PlaylistSorting.ByModifiedDate:
                    Playlist.Sort((a, b) => File.GetLastWriteTimeUtc(a.Location).CompareTo(File.GetLastWriteTimeUtc(b.Location)));
                    Playlist.Reverse();
                    Log("Sorted by file modified date");
                    break;
                case PlaylistSorting.Shuffle:
                    Shuffle(Playlist);
                    Log("Shuffled");
                    break;
            }
            ReloadPlaylist(Playlist);
            txtSearch.Text = strSearchPlaylist;
            UpdateHighlights();
            MarkAsModified();
        }

        private void ShowUpdate(string update)
        {
            UpdateTimer.Stop();
            lblUpdates.Invoke(new MethodInvoker(() => lblUpdates.Text = update));
            UpdateTimer.Enabled = true;
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (txtSearch.Text.Trim() != "") return;
            txtSearch.Text = strSearchPlaylist;
        }

        private void txtSearch_MouseClick(object sender, MouseEventArgs e)
        {
            if (txtSearch.Text.Trim() != strSearchPlaylist) return;
            txtSearch.Text = "";
        }

        private bool lastWordWasDash;
        private bool IsMiddleOfWord(string line)
        {
            line = line.Replace("^", "").Replace("#", "").Replace("$", "").Trim();
            if (line == "+")
            {
                return lastWordWasDash;
            }
            lastWordWasDash = line.EndsWith("-", StringComparison.Ordinal);
            return lastWordWasDash;
        }

        public int GetKaraokeCurrentLineTop()
        {
            return (int)(picVisuals.Height * 0.05);
        }
        public int GetKaraokeNextLineTop()
        {
            return (int)(picVisuals.Height * 0.95);
        }

        private void DoKaraokeMode(Graphics graphics, IList<LyricPhrase> phrases, IEnumerable<Lyric> lyrics)
        {
            var time = GetCorrectedTime();
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
            var currentLineTop = GetKaraokeCurrentLineTop();
            var nextLineTop = GetKaraokeNextLineTop();
            string lineText;
            Font lineFont;
            Size lineSize;
            int posX;
            if (currentLine != null && !string.IsNullOrEmpty(currentLine.PhraseText))
            {
                //draw entire current phrase on top
                lineText = ProcessLine(currentLine.PhraseText, true);
                lineFont = new Font("Tahoma", GetScaledFontSize(graphics, lineText, new Font("Tahoma", (float)12.0), 120));
                lineSize = TextRenderer.MeasureText(lineText, lineFont);
                posX = (picVisuals.Width - lineSize.Width) / 2;
                TextRenderer.DrawText(graphics, lineText, lineFont, new Point(posX, currentLineTop), Color.FromArgb(180, 180, 180), KaraokeBackgroundColor);

                //draw portion of current phrase that's already been sung
                var line2 = lyrics.Where(lyr => !(lyr.LyricStart < currentLine.PhraseStart)).TakeWhile(lyr => !(lyr.LyricStart > time)).Aggregate("", (current, lyr) => current + " " + lyr.LyricText);
                line2 = ProcessLine(line2, true);
                if (!string.IsNullOrEmpty(line2))
                {
                    TextRenderer.DrawText(graphics, line2, lineFont, new Point(posX, currentLineTop), Color.FromArgb(95, 209, 209), KaraokeBackgroundColor);
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
                            word += ProcessLine(lyric.LyricText, true);
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
                            word += ProcessLine(lyric.LyricText, true);
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
                        // Measure the word size for centering
                        lineFont = new Font("Tahoma", GetScaledFontSize(graphics, activeWord.Text, new Font("Tahoma", (float)12.0), 200));
                        lineSize = TextRenderer.MeasureText(activeWord.Text, lineFont);
                        posX = (picVisuals.Width - lineSize.Width) / 2;
                        var posY = (picVisuals.Height - lineSize.Height) / 2;

                        // Draw the entire word in white
                        TextRenderer.DrawText(graphics, activeWord.Text, lineFont, new Point(posX, posY), Color.FromArgb(180, 180, 180), KaraokeBackgroundColor);

                        // Calculate progress for the sung portion
                        var timeElapsed = time - activeWord.WordStart;
                        var progress = Clamp((float)(timeElapsed / (activeWord.WordEnd - activeWord.WordStart)), 0.0f, 1.0f);

                        // Determine the portion of the word to highlight
                        var numCharsToHighlight = (int)Math.Ceiling(progress * activeWord.Text.Length); // Ensure rounding up
                        numCharsToHighlight = Math.Min(numCharsToHighlight, activeWord.Text.Length);

                        // Extract the sung portion
                        var sungPortion = activeWord.Text.Substring(0, numCharsToHighlight);

                        // Overlay the sung portion in blue
                        if (!string.IsNullOrEmpty(sungPortion))
                        {
                            TextRenderer.DrawText(graphics, sungPortion, lineFont, new Point(posX, posY), Color.FromArgb(95, 209, 209), KaraokeBackgroundColor);
                        }
                    }
                }
            }
            if (nextLine != null && !string.IsNullOrEmpty(nextLine.PhraseText))
            {
                //draw entire next phrase on bottom
                lineText = ProcessLine(nextLine.PhraseText, true);
                lineFont = new Font("Tahoma", GetScaledFontSize(graphics, lineText, new Font("Tahoma", (float)12.0), 120));
                lineSize = TextRenderer.MeasureText(lineText, lineFont);
                posX = (picVisuals.Width - lineSize.Width) / 2;
                TextRenderer.DrawText(graphics, lineText, lineFont, new Point(posX, nextLineTop - lineSize.Height), Color.FromArgb(180, 180, 180), KaraokeBackgroundColor);
            }

            //draw waiting/countdown info
            if (currentLine != null && nextLine != null) return;
            if (lastLine != null && nextLine != null)
            {
                var difference = nextLine.PhraseStart - lastLine.PhraseEnd;
                if (difference < 5) return;
            }
            var middleText = "";
            var textColor = Color.FromArgb(180, 180, 180);
            if (currentLine == null && nextLine != null)
            {
                var wait = nextLine.PhraseStart - time;
                if (wait < 1.5) return;
                middleText = wait <= 5 ? "[GET READY]" : "[WAIT: " + ((int)(wait + 0.5)) + "]";
                textColor = wait <= 5 ? Color.FromArgb(185, 216, 76) : Color.FromArgb(255, 187, 52);
            }
            else if (currentLine == null)
            {
                middleText = "[fin]";
            }
            lineFont = new Font("Tahoma", GetScaledFontSize(graphics, middleText, new Font("Tahoma", (float)12.0), 200));
            lineSize = TextRenderer.MeasureText(middleText, lineFont);
            posX = (picVisuals.Width - lineSize.Width) / 2;
            TextRenderer.DrawText(graphics, middleText, lineFont, new Point(posX, (picVisuals.Height - lineSize.Height) / 2), textColor, KaraokeBackgroundColor);
        }

        public float Clamp(float value, float min, float max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        public float GetScaledFontSize(Graphics g, string line, Font PreferedFont, float maxSize)
        {
            var maxWidth = picVisuals.Width * 0.95;
            var RealSize = g.MeasureString(line, PreferedFont);
            var ScaleRatio = maxWidth / RealSize.Width;
            var ScaledSize = PreferedFont.Size * ScaleRatio;
            if (ScaledSize > maxSize)
            {
                return maxSize;
            }
            return (float)ScaledSize;
        }

        private double GetCorrectedTime()
        {
            return PlaybackSeconds - ((double)BassBuffer / 1000) - ((double)PlayingSong.PSDelay / 1000);
        }

        public void ClearVisuals(bool clear_chart = false)
        {
            if (clear_chart && Chart != null)
            {
                Chart.Clear(chartSnippet.Checked ? TrackBackgroundColor1 : GetNoteColor(100));
            }
        }

        private void UpdateVisualStyle(object sender, EventArgs e)
        {
            ClearVisuals();
            UncheckAll();
            ((ToolStripMenuItem)sender).Checked = true;
            PrepareForDrawing();
        }

        private void UncheckAll()
        {
            chartFull.Checked = false;
            chartSnippet.Checked = false;
            chartVertical.Checked = false;
        }

        private void UpdateConsole(object sender, EventArgs e)
        {
            if (songLoader.IsBusy || batchSongLoader.IsBusy) return;
            if (Text.Contains("*"))
            {
                if (MessageBox.Show("You have unsaved changes on the current playlist\nAre you sure you want to do that?",
                    AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }
            }

            var sentBy = (ToolStripMenuItem)sender;
            string newConsole;
            if (sentBy == xbox360)
            {
                newConsole = "xbox";
                consoleToolStripMenuItem.Text = "Your console: Xbox 360 (Rock Band)";
                SetDefaultPaths();
            }
            else if (sentBy == pS3)
            {
                newConsole = "ps3";
                consoleToolStripMenuItem.Text = "Your console: PlayStation 3 (Rock Band)";
            }
            else if (sentBy == wii)
            {
                newConsole = "wii";
                consoleToolStripMenuItem.Text = "Your console: Wii (Rock Band)";
            }
            else if (sentBy == yarg)
            {
                newConsole = "yarg";
                consoleToolStripMenuItem.Text = "Your console: PC (YARG / Clone Hero)";
            }
            else if (sentBy == rockSmith)
            {
                newConsole = "rocksmith";
                consoleToolStripMenuItem.Text = "Your console: PC (Rocksmith 2014)";
            }
            else if (sentBy == guitarHero)
            {
                newConsole = "guitarhero";
                consoleToolStripMenuItem.Text = "Your console: PC (GHWT:DE)";
            }
            else if (sentBy == fortNite)
            {
                newConsole = "fortnite";
                consoleToolStripMenuItem.Text = "Your console: PC (Fortnite Festival)";
            }
            else if (sentBy == powerGig)
            {
                newConsole = "powergig";
                consoleToolStripMenuItem.Text = "Your console: PC (Power Gig)";
            }
            else if (sentBy == bandFuse)
            {
                newConsole = "bandfuse";
                consoleToolStripMenuItem.Text = "Your console: Xbox 360 (BandFuse)";
            }
            else
            {
                return;
            }
            if (PlayerConsole == newConsole) return;
            Log("Updated console to " + newConsole);
            DeleteUsedFiles();
            xbox360.Checked = sentBy == xbox360;
            pS3.Checked = sentBy == pS3;
            wii.Checked = sentBy == wii;
            yarg.Checked = sentBy == yarg;
            rockSmith.Checked = sentBy == rockSmith;
            guitarHero.Checked = sentBy == guitarHero;
            fortNite.Checked = sentBy == fortNite;
            bandFuse.Checked = sentBy == bandFuse;
            powerGig.Checked = sentBy == powerGig;
            PlayerConsole = newConsole;
            StartNew(false);
        }

        private void PlaybackTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Bass.BASS_ChannelIsActive(BassMixer) == BASSActive.BASS_ACTIVE_PLAYING)
                {
                    if (stageKit != null)
                    {
                        foreach (var led in LEDs)
                        {
                            if (DateTime.Now > led.Time && led.Enabled)
                            {
                                UpdateLED(led, false); // Turn off LED
                            }
                        }
                    }

                    // the stream is still playing...                    
                    var pos = Bass.BASS_ChannelGetPosition(BassStream); // position in bytes
                    PlaybackSeconds = Bass.BASS_ChannelBytes2Seconds(BassStream, pos); // the elapsed time length                   

                    //calculate how many seconds are left to play
                    var time_left = ((double)PlayingSong.Length / 1000) - PlaybackSeconds;
                    if ((((!skipIntroOutroSilence.Checked || OutroSilence == 0.0) && time_left <= FadeLength) ||
                        (skipIntroOutroSilence.Checked && OutroSilence > 0.0 && PlaybackSeconds + FadeLength >= OutroSilence)) && !AlreadyFading) //skip to next song
                    {
                        Bass.BASS_ChannelSlideAttribute(BassMixer, BASSAttribute.BASS_ATTRIB_VOL, 0, (int)FadeLength * 1000);
                        AlreadyFading = true;
                    }
                    if (skipIntroOutroSilence.Checked && OutroSilence > 0.0)
                    {
                        if (PlaybackSeconds >= OutroSilence)
                        {
                            goto GoToNextSong;
                        }
                    }
                    else if (PlaybackSeconds * 1000 >= PlayingSong.Length && picShuffle.Tag.ToString() == "shuffle")
                    {
                        DoShuffleSongs();
                        PlaybackTimer.Enabled = false;
                        return;
                    }
                    UpdateTime();
                    DoPracticeSessions();
                    if ((displayAlbumArt.Checked && File.Exists(CurrentSongArt)) || (!File.Exists(CurrentSongArt) && !displayAudioSpectrum.Checked))
                    {
                        picPreview.Invalidate();
                    }
                    if (displayBackgroundVideo.Checked && VideoIsPlaying && displayKaraokeMode.Checked)
                    {
                        KaraokeOverlay.Visible = MonitorApplicationFocus();
                        if (KaraokeOverlay.Visible)
                        {
                            KaraokeOverlay.Phrases = MIDITools.PhrasesVocals.Phrases;
                            KaraokeOverlay.Lyrics = MIDITools.LyricsVocals.Lyrics;
                            KaraokeOverlay.CorrectedTime = GetCorrectedTime();
                            KaraokeOverlay.Invalidate();
                        }
                    }
                    else
                    {
                        KaraokeOverlay.Visible = false;
                        picVisuals.Invalidate();
                    }
                    return;
                }
                goto GoToNextSong;
            }
            catch (Exception)
            {
                return;
            }

        GoToNextSong:
            if (isChoosingStems) return;
            if (picLoop.Tag.ToString() == "loop")
            {
                DoLoop();
            }
            else if (picShuffle.Tag.ToString() == "shuffle")
            {
                DoShuffleSongs();
                PlaybackTimer.Enabled = false;
                return;
            }
            else
            {
                picNext_MouseClick(null, null);
                PlaybackTimer.Enabled = false;
            }
        }

        private void DoLoop()
        {
            PlaybackTimer.Enabled = false;
            StopPlayback();
            PlaybackSeconds = 0;
            StartPlayback(true, false);
        }

        private void DoPracticeSessions()
        {
            lblSections.Visible = showPracticeSections.Checked && MIDITools.PracticeSessions.Any() && !chartVertical.Checked;
            if (!openSideWindow.Checked) return;
            if (!showPracticeSections.Checked || !MIDITools.PracticeSessions.Any())
            {
                lblSections.Text = "";
                return;
            }
            lblSections.Text = GetCurrentSection(GetCorrectedTime());
        }

        private string GetCurrentSection(double time)
        {
            var curr_session = "";
            foreach (var session in MIDITools.PracticeSessions.TakeWhile(session => session.SectionStart <= time))
            {
                curr_session = session.SectionName;
            }
            return curr_session;
        }

        private void DrawFills(Graphics graphics, MIDITrack instrument, int posY, int posX, int track_width)
        {
            if (MIDITools.MIDI_Chart.Drums.Fills.Count == 0 && MIDITools.MIDI_Chart.Drums.Overdrive.Count == 0)
                return;
            var correctedTime = GetCorrectedTime();
            var fillColor = Color.FromArgb(100, ChartGreen.R, ChartGreen.G, ChartGreen.B);
            foreach (var fill in instrument.Fills)
            {
                if (fill.MarkerEnd <= correctedTime) continue;
                if (fill.MarkerBegin > correctedTime + PlaybackWindowRB) break;
                DrawFill(graphics, fill, correctedTime, fillColor, posY, posX, track_width);
                break;
            }
            foreach (var OD in instrument.Overdrive)
            {
                if (OD.MarkerEnd <= correctedTime) continue;
                if (OD.MarkerBegin > correctedTime + PlaybackWindowRB) break;
                fillColor = Color.FromArgb(100, 255, 255, 255);
                DrawFill(graphics, OD, correctedTime, fillColor, posY, posX, track_width);
                break;
            }
        }

        private void DrawFill(Graphics graphics, SpecialMarker marker, double correctedTime, Color fillColor, int posY, int posX, int trackWidth)
        {
            // Calculate the chart goal relative to the given posY
            ChartGoal = picVisuals.Height - posY - 50; // Pre-calculated

            // Calculate the height of the fill
            var height = ((marker.MarkerEnd - marker.MarkerBegin) / PlaybackWindowRB) * ChartGoal;

            // Calculate the percentage of the fill progress
            var percent = 1.0 - ((marker.MarkerBegin - correctedTime) / PlaybackWindowRB);

            // Calculate the top Y position for the fill
            var topY = posY + (ChartGoal * percent) - height;

            // Adjust the height if the fill overlaps the starting position (posY)
            if (topY < posY)
            {
                height -= (posY - topY);
                topY = posY; // Anchor the top at startingPosition
            }

            // Prevent the fill from exceeding the hitbox area
            if (topY + height > picVisuals.Height - 50)
            {
                height = picVisuals.Height - 50 - topY;
            }

            // Draw the fill rectangle
            if (height > 0) // Only draw if there's visible height
            {
                using (var solidBrush = new SolidBrush(fillColor))
                {
                    graphics.FillRectangle(solidBrush, posX, (float)topY, trackWidth, (float)height);
                }
            }
        }

        private void DrawProKeysNotes(Graphics graphics, int startingPosition, int ChartLeft, int trackWidth)
        {
            if (MIDITools.MIDI_Chart.ProKeys.ChartedNotes.Count == 0) return;

            var correctedTime = GetCorrectedTime();
            ChartGoal = picVisuals.Height - startingPosition - 50; // Pre-calculated
            Color tailColor;

            // Filter notes to process only visible ones
            var filteredNotes = MIDITools.MIDI_Chart.ProKeys.ChartedNotes.Where(note => note.NoteStart <= correctedTime + PlaybackWindowRB).ToList();

            var noteWidth = trackWidth / 25.0;

            // Group notes by NoteStart to identify chords
            var groupedNotes = filteredNotes.GroupBy(note => note.NoteStart);

            foreach (var chord in groupedNotes)
            {
                var chordNotes = chord.ToList();
                var chordStartTime = chord.Key;

                double chordLeft = double.MaxValue;
                double chordRight = double.MinValue;
                double posY = 0;

                foreach (var note in chordNotes)
                {
                    var percent = 1.0 - ((note.NoteStart - correctedTime) / PlaybackWindowRB);
                    posY = startingPosition + (ChartGoal * percent);

                    var noteLocation = note.NoteNumber - 48;
                    var posX = ChartLeft + (noteWidth * noteLocation);

                    // Update chord bounds
                    chordLeft = Math.Min(chordLeft, posX - 10);
                    chordRight = Math.Max(chordRight, posX + noteWidth + 10);
                }

                if (chordNotes.Count() > 1 && posY <= picVisuals.Height - 50)
                {
                    // Draw the chord marker first but only if it's at least two notes
                    var chordWidth = chordRight - chordLeft;
                    graphics.DrawImage(bmpProKeysChordMarker, (float)chordLeft, (float)posY - 4, (float)chordWidth, 12);
                }

                // Now draw the notes in the chord
                foreach (var note in chordNotes)
                {
                    if (note.NoteColor == Color.Empty)
                    {
                        note.NoteColor = GetNoteColor(note.NoteNumber);
                    }

                    var percent = 1.0 - ((note.NoteStart - correctedTime) / PlaybackWindowRB);
                    posY = startingPosition + (ChartGoal * percent);

                    var img = note.NoteName.Contains("#") ? (note.hasOD ? bmpProKeysNoteBlackOD : bmpProKeysNoteBlack) : (note.hasOD ? bmpProKeysNoteWhiteOD : bmpProKeysNoteWhite);
                    tailColor = note.hasOD ? Color.LightGoldenrodYellow : (note.NoteName.Contains("#") ? Color.Black : Color.White);

                    var noteLocation = note.NoteNumber - 48;

                    // Calculate size and position
                    var noteHeight = img.Height * (noteWidth / img.Width);
                    var posX = ChartLeft + (noteWidth * noteLocation);

                    // Draw sustain tail if the note length is >= 1 second
                    if (note.NoteLength >= 1)
                    {
                        DrawSustainTail(graphics, note, tailColor, correctedTime, posY, posX, noteWidth, startingPosition);
                    }
                    if (posY > picVisuals.Height - 50) continue;

                    // Draw the note
                    graphics.DrawImage(img, (float)posX, (float)(posY - (noteHeight / 2)), (float)noteWidth, (float)noteHeight);

                    if (stageKit != null && note.NoteStart <= correctedTime + 0.1)
                    {
                        if (skActiveInstrument == Instrument.ProKeys)
                        {
                            GetLEDColorIndexProKeys(note.NoteNumber);
                        }
                    }
                }
            }
        }

        private void GetLEDColorIndexProKeys(int note)
        {
            switch (note - 12)
            {
                case 36:
                case 37:
                case 38:
                case 39:
                case 40://red
                    GetLEDColorIndex(LEDColor.Red);
                    break;
                case 41:
                case 42:
                case 43:
                case 44:
                case 45:
                case 46:
                case 47://yellow
                    GetLEDColorIndex(LEDColor.Yellow);
                    break;
                case 48:
                case 49:
                case 50:
                case 51:
                case 52://blue
                    GetLEDColorIndex(LEDColor.Blue);
                    break;
                case 53:
                case 54:
                case 55:
                case 56:
                case 57:
                case 58:
                case 59: //green
                    GetLEDColorIndex(LEDColor.Green);
                    break;
                case 60: //orange
                    GetLEDColorIndex(LEDColor.Orange);
                    break;
            }
        }

        private void DrawFiveLaneNotes(Graphics graphics, MIDITrack instrument, int startingPosition, int ChartLeft, int trackWidth)
        {
            if (instrument.ChartedNotes.Count == 0) return;

            var correctedTime = GetCorrectedTime();
            ChartGoal = picVisuals.Height - startingPosition - 50; // Pre-calculated
        
            // Filter notes to process only visible ones
            var filteredNotes = instrument.ChartedNotes.Where(note => note.NoteStart <= correctedTime + PlaybackWindowRB).ToList();

            var noteWidth = trackWidth / 5.0;

            foreach (var note in filteredNotes)
            {
                if (note.NoteColor == Color.Empty)
                {
                    note.NoteColor = GetNoteColor(note.NoteNumber);
                }

                var percent = 1.0 - ((note.NoteStart - correctedTime) / PlaybackWindowRB);
                var posY = startingPosition + (ChartGoal * percent);

                var noteLocation = 0;
                var img = note.hasOD ? bmpNoteOD : bmpNoteGreen;

                // Pre-determine image and position
                if (note.NoteColor == ChartRed)
                {
                    noteLocation = 1;
                    img = note.hasOD ? bmpNoteOD : bmpNoteRed;
                }
                if (note.NoteColor == ChartYellow)
                {
                    noteLocation = 2;
                    img = note.hasOD ? bmpNoteOD : bmpNoteYellow;
                }
                else if (note.NoteColor == ChartBlue)
                {
                    noteLocation = 3;
                    img = note.hasOD ? bmpNoteOD : bmpNoteBlue;
                }
                else if (note.NoteColor == ChartOrange)
                {
                    noteLocation = 4;
                    img = note.hasOD ? bmpNoteOD : bmpNoteOrange;
                }

                // Calculate size and position
                var noteHeight = img.Height * (noteWidth / img.Width);
                var posX = ChartLeft + (noteWidth * noteLocation);

                Color tailColor = note.hasOD ? Color.White : note.NoteColor;

                // Draw sustain tail if the note length is >= 1 second
                if (note.NoteLength >= 1)
                {
                    DrawSustainTail(graphics, note, tailColor, correctedTime, posY, posX, noteWidth, startingPosition);
                }
                if (posY > picVisuals.Height - 50) continue;
                graphics.DrawImage(img, (float)posX, (float)(posY - (noteHeight / 2)), (float)noteWidth, (float)noteHeight);

                if (stageKit != null && note.NoteStart <= correctedTime + 0.1)
                {
                    if (instrument.Name == "Guitar" && skActiveInstrument == Instrument.Guitar ||
                        instrument.Name == "Bass" && skActiveInstrument == Instrument.Bass ||
                        instrument.Name == "Keys" && skActiveInstrument == Instrument.Keys)
                    {
                        GetLEDColorIndex(GetLEDColor(note.NoteColor));
                    }
                }
            }
        }

        private LEDColor GetLEDColor (Color color, bool drums = false)
        {
            if (color == ChartRed) return LEDColor.Red;
            if (color == ChartBlue) return LEDColor.Blue;
            if (color == ChartYellow) return LEDColor.Yellow;
            if (color == ChartGreen) return LEDColor.Green;
            if (color == ChartOrange)  return drums ? LEDColor.White : LEDColor.Orange;
            return LEDColor.Red;//default
        }

        private void DrawSustainTail(Graphics graphics, MIDINote note, Color tailColor, double correctedTime, double posY, double posX, double noteWidth, int startingPosition)
        {
            // Calculate the end position of the sustain tail
            var tailEndPercent = 1.0 - (((note.NoteStart + note.NoteLength - 0.6) - correctedTime) / PlaybackWindowRB);
            var tailEndY = startingPosition + (ChartGoal * tailEndPercent);

            // Ensure the tail extends correctly above the note
            if (tailEndY < startingPosition) tailEndY = startingPosition;

            // Prevent the tail from being clipped prematurely
            if (tailEndY > picVisuals.Height - 50)
            {
                tailEndY = picVisuals.Height - 50;
            }

            // Ensure the tail continues to display even when the note hits the hitbox
            if (tailEndY > picVisuals.Height - 50)
            {
                tailEndY = picVisuals.Height - 50;
            }

            // Split the tail into a static and dynamic part
            var splitY = Math.Min(picVisuals.Height - 50, posY);

            // Draw the static part of the tail (straight line above the hitbox)
            if (posY <= picVisuals.Height - 50 && splitY > tailEndY)
            {
                using (var tailBrush = new SolidBrush(tailColor))
                {
                    graphics.FillRectangle(tailBrush, (float)(posX + (noteWidth / 2) - 3), (float)tailEndY, 6, (float)(splitY - tailEndY));
                }
            }

            // Draw the dynamic part of the tail (moving sine wave below the hitbox)
            if (posY > picVisuals.Height - 50 && tailEndY < splitY)
            {
                var dynamicHeight = (int)(splitY - tailEndY); // Ensure consistent height calculation
                using (var tailBrush = new SolidBrush(tailColor))
                {
                    for (int i = 0; i < dynamicHeight; i += 5)
                    {
                        // Ensure the sine wave stays within bounds
                        if (tailEndY + i >= splitY) break;

                        var waveOffset = 4 * Math.Sin((correctedTime + i * 0.0125) * Math.PI); // Larger and slower sine wave
                        graphics.FillRectangle(tailBrush, (float)(posX + (noteWidth / 2) - 3 + waveOffset), (float)(tailEndY + i), 6, 5); // Adjusted tail width
                    }
                }
            }
        }

        private void DrawDrumNotes(Graphics graphics, bool doKicks, int startingPosition, int ChartLeft, int trackWidth)
        {
            if (MIDITools.MIDI_Chart.Drums.ChartedNotes.Count == 0) return;

            var track = MIDITools.MIDI_Chart.Drums;
            var correctedTime = GetCorrectedTime();
            ChartGoal = picVisuals.Height - startingPosition - 50; // Pre-calculated

            // Filter notes to process only visible ones
            var filteredNotes = track.ChartedNotes.Where(note => note.NoteStart <= correctedTime + PlaybackWindowRB).ToList();            
            
            var noteWidth = trackWidth / 4.0;

            foreach (var note in filteredNotes)
            {
                if (note.NoteColor == Color.Empty)
                {
                    note.NoteColor = GetNoteColor(note.NoteNumber, true);
                }

                if (note.NoteColor == ChartOrange && !doKicks) continue;
                if (note.NoteColor != ChartOrange && doKicks) continue;

                var percent = 1.0 - ((note.NoteStart - correctedTime) / PlaybackWindowRB);
                var posY = startingPosition + (ChartGoal * percent);
                if (posY > picVisuals.Height - 50) continue; //only draw until the hit box
                var noteLocation = 0;
                var img = note.hasOD ? bmpNoteOD : bmpNoteRed;

                // Pre-determine image and position
                if (note.NoteColor == ChartYellow)
                {
                    noteLocation = 1;
                    img = note.isTom
                        ? (note.hasOD ? bmpNoteOD : bmpNoteYellow)
                        : (note.hasOD ? bmpDrumsCymbalOD : bmpDrumsCymbalY);
                }
                else if (note.NoteColor == ChartBlue)
                {
                    noteLocation = 2;
                    img = note.isTom
                        ? (note.hasOD ? bmpNoteOD : bmpNoteBlue)
                        : (note.hasOD ? bmpDrumsCymbalOD : bmpDrumsCymbalB);
                }
                else if (note.NoteColor == ChartGreen)
                {
                    noteLocation = 3;
                    img = note.isTom
                        ? (note.hasOD ? bmpNoteOD : bmpNoteGreen)
                        : (note.hasOD ? bmpDrumsCymbalOD : bmpDrumsCymbalG);
                }

                // Calculate size and position
                var noteHeight = img.Height * (noteWidth / img.Width);
                var posX = ChartLeft + (noteWidth * noteLocation);

                if (note.NoteColor == ChartOrange)
                {
                    using (var solidBrush = new SolidBrush(note.hasOD ? Color.WhiteSmoke : Color.FromArgb(255, 180, 28)))
                    {
                        graphics.FillRectangle(solidBrush, ChartLeft, (float)posY, trackWidth, KICK_HEIGHT);
                    }
                }
                else
                {
                    graphics.DrawImage(img, (float)posX, (float)(posY - (noteHeight / 2)), (float)noteWidth, (float)noteHeight);
                }

                if (stageKit != null && note.NoteStart <= correctedTime + 0.1)
                {
                    if (skActiveInstrument == Instrument.Drums && stageKit != null)
                    {
                        GetLEDColorIndex(GetLEDColor(note.NoteColor, true));
                    }
                }
            }
        }


        private void DrawLyrics(Graphics graphics, Color backColor)
        {
            if (!openSideWindow.Checked) return;
            if ((!doStaticLyrics && !doScrollingLyrics && !doKaraokeLyrics) || !MIDITools.PhrasesVocals.Phrases.Any())
            {
                return;
            }

            var phrases = doHarmonyLyrics && MIDITools.PhrasesHarm1.Phrases.Any() ? MIDITools.PhrasesHarm1.Phrases : MIDITools.PhrasesVocals.Phrases;
            var lyrics = doHarmonyLyrics && MIDITools.LyricsHarm1.Lyrics.Any() ? MIDITools.LyricsHarm1.Lyrics : MIDITools.LyricsVocals.Lyrics;
            var font = new Font("Segoe UI", 12f);
            var harm1Y = picVisuals.Height - (MIDITools.LyricsHarm3.Lyrics.Any() ? 60 : (MIDITools.LyricsHarm2.Lyrics.Any() ? 40 : 0));
            var harm2Y = picVisuals.Height - (MIDITools.LyricsHarm3.Lyrics.Any() ? 40 : 20);
            var harm3Y = picVisuals.Height - 20;
            if (!doHarmonyLyrics || (doHarmonyLyrics && !MIDITools.LyricsHarm2.Lyrics.Any()))
            {
                harm1Y = harm3Y;
            }
            if (chartVertical.Checked && displayMIDIChartVisuals.Checked)
            {
                harm1Y = GetHeightDiff() + 4;
                harm2Y = GetHeightDiff() + 24;
                harm3Y = GetHeightDiff() + 44;
            }
            if (doScrollingLyrics)
            {
                if (doHarmonyLyrics)
                {
                    DrawLyricsScrolling(MIDITools.LyricsHarm3.Lyrics, font, Harm3Color, backColor, harm3Y, graphics);
                    DrawLyricsScrolling(MIDITools.LyricsHarm2.Lyrics, font, Harm2Color, backColor, harm2Y, graphics);
                }
                DrawLyricsScrolling(lyrics, font, doHarmonyLyrics || doMIDIHarm1onVocals ? Harm1Color : Color.White, backColor, harm1Y, graphics);
            }
            else if (doKaraokeLyrics)
            {
                if (doHarmonyLyrics)
                {
                    DrawLyricsKaraoke(MIDITools.PhrasesHarm3.Phrases, MIDITools.LyricsHarm3.Lyrics, font, Harm3Color, backColor, harm3Y, graphics);
                    DrawLyricsKaraoke(MIDITools.PhrasesHarm2.Phrases, MIDITools.LyricsHarm2.Lyrics, font, Harm2Color, backColor, harm2Y, graphics);
                }
                DrawLyricsKaraoke(phrases, lyrics, font, doHarmonyLyrics || doMIDIHarm1onVocals ? Harm1Color : Color.White, backColor, harm1Y, graphics);
            }
            else if (doStaticLyrics)
            {
                if (doHarmonyLyrics)
                {
                    DrawLyricsStatic(MIDITools.PhrasesHarm3.Phrases, font, Harm3Color, backColor, harm3Y, graphics);
                    DrawLyricsStatic(MIDITools.PhrasesHarm2.Phrases, font, Harm2Color, backColor, harm2Y, graphics);
                }
                DrawLyricsStatic(phrases, font, doHarmonyLyrics || doMIDIHarm1onVocals ? Harm1Color : Color.White, backColor, harm1Y, graphics);
            }
        }

        private void DrawLyricsStatic(IEnumerable<LyricPhrase> phrases, Font font, Color foreColor, Color backColor, int posY, Graphics graphics)
        {
            if (phrases == null || phrases.Count() == 0) return;
            var time = GetCorrectedTime();
            graphics.DrawImage(bmpBackgroundLyrics, 0, posY, picVisuals.Width, 20);
            
            LyricPhrase phrase = null;
            foreach (var lyric in phrases.TakeWhile(lyric => lyric.PhraseStart <= time).Where(lyric => lyric.PhraseEnd >= time))
            {
                phrase = lyric;
            }
            string line;
            try
            {
                line = phrase == null || phrase.PhraseText == null || string.IsNullOrEmpty(phrase.PhraseText.Trim()) ? GetMusicNotes() : ProcessLine(phrase.PhraseText, doWholeWordsLyrics);
            }
            catch (Exception)
            {
                line = GetMusicNotes();
            }
            var processedLine = ProcessLine(line, doWholeWordsLyrics);
            var lineSize = graphics.MeasureString(processedLine, font);
            var left = (picVisuals.Width - (int)lineSize.Width) / 2;

            using (var textBrush = new SolidBrush(displayMIDIChartVisuals.Checked && chartVertical.Checked ? Color.White : Color.Black))//foreColor))
            {
                graphics.DrawString(processedLine, font, textBrush, new PointF(left, posY - 4));
            }
        }

        private void InitBASS()
        {
            Log("Initializing BASS and BASS_ASIO");
            //initialize BASS            
            if (Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero))
            {
                Log("Success");
                Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_BUFFER, BassBuffer);
                Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_UPDATEPERIOD, 50);
            }
            else
            {
                Log("BASS Error: " + Bass.BASS_ErrorGetCode());
                MessageBox.Show("Error initializing BASS\n" + Bass.BASS_ErrorGetCode(), AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool PrepMixerRB3(bool isM4A = false)
        {
            Log("Preparing audio mixer using RB3 mogg file");
            BassStreams.Clear();
            try
            {
                if (isM4A)
                {
                    BassStream = Bass.BASS_StreamCreateFile(activeM4AFile, 0L, 0L, BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_SAMPLE_FLOAT);
                }
                else
                {
                    BassStream = Bass.BASS_StreamCreateFile(nautilus.GetOggStreamIntPtr(), 0L, nautilus.PlayingSongOggData.Length, BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_SAMPLE_FLOAT);
                    BassStreams.Add(BassStream);
                }

                // create a decoder for the input file(s)
                var channel_info = Bass.BASS_ChannelGetInfo(BassStream);

                // create a stereo mixer with same frequency rate as the input file(s)
                BassMixer = BassMix.BASS_Mixer_StreamCreate(channel_info.freq, 2, BASSFlag.BASS_MIXER_END);//BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_MIXER_END);
                BassMix.BASS_Mixer_StreamAddChannel(BassMixer, BassStream, BASSFlag.BASS_MIXER_MATRIX | BASSFlag.BASS_MIXER_CHAN_BUFFER);

                if (isM4A)
                {
                    ActiveSongData.ChannelsDrums = 2;
                    ActiveSongData.ChannelsBassStart = 0;
                    ActiveSongData.ChannelsBass = 2;
                    ActiveSongData.ChannelsBassStart = 2;
                    ActiveSongData.ChannelsGuitar = 2;
                    ActiveSongData.ChannelsGuitarStart = 4;
                    ActiveSongData.ChannelsVocals = 2;
                    ActiveSongData.ChannelsVocalsStart = 6;
                    ActiveSongData.ChannelsTotal = 10;
                    ActiveSongData.AttenuationValues = "";
                    ActiveSongData.PanningValues = "";

                    var len = Bass.BASS_ChannelGetLength(BassStream);
                    var totaltime = Bass.BASS_ChannelBytes2Seconds(BassStream, len); // the total time length
                    ActiveSongData.Length = (int)(totaltime * 1000);
                    ActiveSong.Length = ActiveSongData.Length;
                    lblDuration.Text = Parser.GetSongDuration(ActiveSong.Length.ToString(CultureInfo.InvariantCulture));
                }

                //get and apply channel matrix
                var matrix = GetChannelMatrix(channel_info.chans);
                BassMix.BASS_Mixer_ChannelSetMatrix(BassStream, matrix);
            }
            catch (Exception ex)
            {
                Log("Error preparing Rock Band mixer: " + ex.Message);
                return false;
            }
            Log("Success");
            return true;
        }

        private bool PrepMixerPS(IList<string> audioFiles, out int mixer, out List<int> NextSongStreams)
        {
            Log("Preparing audio mixer using YARG / Clone Hero file(s)");
            BassStreams.Clear();
            try
            {
                var audioFile = opusFiles.Any() ? opusFiles[0] : (mp3Files.Any() ? mp3Files[0] : (wavFiles.Any() ? wavFiles[0] : oggFiles[0]));
                if (opusFiles.Any())
                {
                    BassStream = BassOpus.BASS_OPUS_StreamCreateFile(audioFile, 0, 0, BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_SAMPLE_FLOAT);
                }
                else //OGG or MP3 or WAV
                {
                    BassStream = Bass.BASS_StreamCreateFile(audioFile, 0, 0, BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_SAMPLE_FLOAT);
                }

                // create a decoder for the audio file(s)
                var channel_info = Bass.BASS_ChannelGetInfo(BassStream);

                // create a stereo mixer with same frequency rate as the input file
                BassMixer = BassMix.BASS_Mixer_StreamCreate(channel_info.freq, 2, BASSFlag.BASS_MIXER_END);//BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_SAMPLE_FLOAT);

                var folder = Path.GetDirectoryName(audioFile) + "\\";
                var ext = opusFiles.Any() ? "opus" : (mp3Files.Any() ? "mp3" : (wavFiles.Any() ? "wav" : "ogg"));
                var drums = folder + "drums." + ext;
                var drums1 = folder + "drums_1." + ext;
                var drums2 = folder + "drums_2." + ext;
                var drums3 = folder + "drums_3." + ext;
                var drums4 = folder + "drums_4." + ext;
                var bass = folder + "bass." + ext;
                var rhythm = folder + "rhythm." + ext;
                var guitar = folder + "guitar." + ext;
                var guitar1 = folder + "guitar_1." + ext;
                var guitar2 = folder + "guitar_2." + ext;
                var keys = folder + "keys." + ext;
                var vocals = folder + "vocals." + ext;
                var vocals1 = folder + "vocals_1." + ext;
                var vocals2 = folder + "vocals_2." + ext;
                var backing = folder + "backing." + ext;
                var song = folder + "song." + ext;
                var crowd = folder + "crowd." + ext;

                if (File.Exists(drums) || File.Exists(drums1) || File.Exists(drums2) || File.Exists(drums3) || File.Exists(drums4))
                {
                    Parser.Songs[0].ChannelsDrums = 2; //don't matter as long as it's more than 0 to enable it
                }
                if (File.Exists(bass) || File.Exists(rhythm))
                {
                    Parser.Songs[0].ChannelsBass = 2;
                }
                if (File.Exists(guitar) || File.Exists(guitar1) || File.Exists(guitar2))
                {
                    Parser.Songs[0].ChannelsGuitar = 2;
                }
                if (File.Exists(keys))
                {
                    Parser.Songs[0].ChannelsKeys = 2;
                }
                if (File.Exists(vocals) || File.Exists(vocals1) || File.Exists(vocals2))
                {
                    Parser.Songs[0].ChannelsVocals = 2;
                }
                if (File.Exists(crowd))
                {
                    Parser.Songs[0].ChannelsCrowd = 2;
                }

                if (doAudioDrums)
                {
                    if (File.Exists(drums))
                    {
                        AddAudioToMixer(drums);
                    }
                    else
                    {
                        var split_drums = new List<string> { drums1, drums2, drums3, drums4 };
                        foreach (var drum in split_drums.Where(File.Exists))
                        {
                            AddAudioToMixer(drum);
                        }
                    }
                }
                if (doAudioBass)
                {
                    if (File.Exists(bass))
                    {
                        AddAudioToMixer(bass);
                    }
                    else if (File.Exists(rhythm))
                    {
                        AddAudioToMixer(rhythm);
                    }
                }
                if (doAudioGuitar)
                {
                    if (File.Exists(guitar))
                    {
                        AddAudioToMixer(guitar);
                    }
                    else
                    {
                        var split_guitar = new List<string> { guitar1, guitar2 };
                        foreach (var gtr in split_guitar.Where(File.Exists))
                        {
                            AddAudioToMixer(gtr);
                        }
                    }
                    if (File.Exists(rhythm) && !File.Exists(bass))
                    {
                        AddAudioToMixer(rhythm);
                    }
                }
                if (doAudioKeys && File.Exists(keys))
                {
                    AddAudioToMixer(keys);
                }
                if (doAudioVocals)
                {
                    if (File.Exists(vocals))
                    {
                        AddAudioToMixer(vocals);
                    }
                    else
                    {
                        var split_vocals = new List<string> { vocals1, vocals2 };
                        foreach (var vocal in split_vocals.Where(File.Exists))
                        {
                            AddAudioToMixer(vocal);
                        }
                    }
                }
                if (doAudioBacking)
                {
                    if (File.Exists(backing))
                    {
                        AddAudioToMixer(backing);
                    }
                    else if (File.Exists(song))
                    {
                        AddAudioToMixer(song);
                    }
                    else if (audioFiles[0] == guitar)
                    {
                        AddAudioToMixer(guitar);
                    }
                }
                if (doAudioCrowd && File.Exists(crowd))
                {
                    AddAudioToMixer(crowd);
                }
            }
            catch (Exception ex)
            {
                Log("Error preparing mixer: " + ex.Message);
                mixer = 0;
                NextSongStreams = null;
                return false;
            }
            Log("Success");
            Log("Added " + BassStreams.Count + " file(s) to the mixer");
            mixer = BassMixer;
            NextSongStreams = BassStreams;
            return true;
        }

        private void AddAudioToMixer(string audioFile)
        {
            Log("Adding audio file to mixer: " + audioFile);
            if (opusFiles.Any())
            {
                BassStream = BassOpus.BASS_OPUS_StreamCreateFile(audioFile, 0, 0, BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_SAMPLE_FLOAT);
            }
            else //ogg or mp3 or wav
            {
                BassStream = Bass.BASS_StreamCreateFile(audioFile, 0, 0, BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_SAMPLE_FLOAT);
            }
            var stream_info = Bass.BASS_ChannelGetInfo(BassStream);
            if (stream_info.chans == 0) return;
            BassMix.BASS_Mixer_StreamAddChannel(BassMixer, BassStream, BASSFlag.BASS_MIXER_MATRIX);
            BassStreams.Add(BassStream);
        }

        private void StartPlayback(bool doFade, bool doNext, bool PlayAudio = true)
        {
            Log("Starting playback");            
            if (PlayAudio)
            {
                if ((!yarg.Checked && !fortNite.Checked && !guitarHero.Checked && !powerGig.Checked && !bandFuse.Checked) && (CurrentSongAudio == null || CurrentSongAudio.Length == 0))
                {
                    if (AlreadyTried || lstPlaylist.SelectedItems.Count == 0)
                    {
                        var msg = "Audio file (*.mogg) for song '" + PlayingSong.Artist + " - " + PlayingSong.Name + "' is missing";
                        Log(msg);
                        MessageBox.Show(msg, AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        StopPlayback();
                        AlreadyTried = false;
                    }
                    else
                    {
                        AlreadyTried = true;
                        lstPlaylist_MouseDoubleClick(null, null);
                    }
                    return;
                }

                var directory = Path.GetDirectoryName(PlayingSong.Location);
                if (yarg.Checked && !string.IsNullOrEmpty(sngPath))
                {
                    directory = Application.StartupPath + "\\bin\\temp\\";
                }
                else if (rockSmith.Checked && !string.IsNullOrEmpty(psarcPath))
                {
                    directory = psarcPath;
                }
                else if (guitarHero.Checked && !string.IsNullOrEmpty(ghwtPath))
                {
                    directory = ghwtPath;
                }
                else if (powerGig.Checked && !string.IsNullOrEmpty(XMA_EXT_PATH))
                {
                    directory = XMA_EXT_PATH;
                }
                else if (bandFuse.Checked && !string.IsNullOrEmpty(BandFusePath))
                {
                    directory = Application.StartupPath + "\\bin\\temp\\";
                }
                oggFiles = Directory.GetFiles(directory, "*.ogg", SearchOption.TopDirectoryOnly);
                opusFiles = Directory.GetFiles(directory, "*.opus", SearchOption.TopDirectoryOnly);
                mp3Files = Directory.GetFiles(directory, "*.mp3", SearchOption.TopDirectoryOnly);
                wavFiles = Directory.GetFiles(directory, "*.wav", SearchOption.TopDirectoryOnly);

                if (fortNite.Checked && !string.IsNullOrEmpty(activeM4AFile))
                {
                    if (!PrepMixerRB3(true))
                    {
                        MessageBox.Show("Error preparing audio mixer - can't play that song", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        StopPlayback();
                        return;
                    }
                }
                else if ((yarg.Checked || fortNite.Checked || guitarHero.Checked || powerGig.Checked || bandFuse.Checked) && (oggFiles.Any() || opusFiles.Any() || mp3Files.Any() || wavFiles.Any()))
                {
                    List<string> AudioFiles;
                    if (opusFiles.Any())
                    {
                        if (!opusFiles.Any())
                        {
                            var msg = "Audio files (*.opus) for song '" + PlayingSong.Artist + " - " + PlayingSong.Name + "' are missing";
                            Log(msg);
                            MessageBox.Show(msg, AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            StopPlayback();
                            return;
                        }
                        AudioFiles = opusFiles.ToList();
                    }
                    else if (mp3Files.Any())
                    {
                        if (!mp3Files.Any())
                        {
                            var msg = "Audio files (*.mp3) for song '" + PlayingSong.Artist + " - " + PlayingSong.Name + "' are missing";
                            Log(msg);
                            MessageBox.Show(msg, AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            StopPlayback();
                            return;
                        }
                        AudioFiles = mp3Files.ToList();
                    }
                    else if (wavFiles.Any())
                    {
                        if (!wavFiles.Any())
                        {
                            var msg = "Audio files (*.wav) for song '" + PlayingSong.Artist + " - " + PlayingSong.Name + "' are missing";
                            Log(msg);
                            MessageBox.Show(msg, AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            StopPlayback();
                            return;
                        }
                        AudioFiles = wavFiles.ToList();
                    }
                    else
                    {
                        if (!oggFiles.Any())
                        {
                            var msg = "Audio files (*.ogg) for song '" + PlayingSong.Artist + " - " + PlayingSong.Name + "' are missing";
                            Log(msg);
                            MessageBox.Show(msg, AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            StopPlayback();
                            return;
                        }
                        AudioFiles = oggFiles.ToList();
                    }
                    int mixer;
                    List<int> streams;
                    if (!PrepMixerPS(AudioFiles, out mixer, out streams))
                    {
                        const string msg = "Error preparing audio mixer - can't play that song";
                        Log(msg);
                        MessageBox.Show(msg, AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        StopPlayback();
                        return;
                    }
                }
                else
                {
                    if (File.Exists(CurrentSongAudioPath))
                    {
                        if (Path.GetExtension(CurrentSongAudioPath) == ".mogg")
                        {
                            oggPath = CurrentSongAudioPath.Replace(".mogg", ".ogg");
                            if (!nautilus.DecM(File.ReadAllBytes(CurrentSongAudioPath), false, doNext, DecryptMode.ToFile, oggPath))
                            {
                                var msg = "Audio file for '" + PlayingSong.Artist + " - " + PlayingSong.Name + "' is encrypted, can't play it";
                                Log(msg);
                                MessageBox.Show(msg, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                StopPlayback();
                                return;
                            }
                        }
                        else if (Path.GetExtension(CurrentSongAudioPath) == ".yarg_mogg")
                        {
                            oggPath = CurrentSongAudioPath.Replace(".yarg_mogg", ".ogg");
                            if (!nautilus.DecY(CurrentSongAudioPath, DecryptMode.ToFile, oggPath))
                            {
                                var msg = "Audio file for '" + PlayingSong.Artist + " - " + PlayingSong.Name + "' is encrypted, can't play it";
                                Log(msg);
                                MessageBox.Show(msg, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                StopPlayback();
                                return;
                            }
                            else
                            {
                                nautilus.RemoveMHeader(File.ReadAllBytes(oggPath), doNext, DecryptMode.ToFile, oggPath);
                            }
                        }
                    }
                    if (nautilus.PlayingSongOggData == null && nautilus.NextSongOggData != null)
                    {
                        nautilus.PlayingSongOggData = nautilus.NextSongOggData;
                    }
                    if (nautilus.PlayingSongOggData == null || nautilus.PlayingSongOggData.Length == 0)
                    {
                        if (!nautilus.DecM(CurrentSongAudio, false, false, DecryptMode.ToFile, oggPath))
                        {
                            var msg = "Audio file for '" + PlayingSong.Artist + " - " + PlayingSong.Name + "' is encrypted, can't play it";
                            Log(msg);
                            MessageBox.Show(msg, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            StopPlayback();
                            return;
                        }
                    }
                    if (!PrepMixerRB3())
                    {
                        MessageBox.Show("Error preparing audio mixer - can't play that song", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        StopPlayback();
                        return;
                    }
                }

                IntroSilence = skipIntroOutroSilence.Checked ? IntroSilenceNext : 0.0;
                OutroSilence = skipIntroOutroSilence.Checked ? OutroSilenceNext : 0.0;
                IntroSilenceNext = 0.0;
                OutroSilenceNext = 0.0;
                if (PlaybackSeconds == 0 && skipIntroOutroSilence.Checked)
                {
                    PlaybackSeconds = IntroSilence;
                }

                SetPlayLocation(PlaybackSeconds);
                Log("Playback start time: " + PlaybackSeconds + " seconds");

                //apply volume correction to entire track
                var track_vol = (float)Utils.DBToLevel(Convert.ToDouble(-1 * (MinVolume - VolumeLevel)), 1.0);
                if (doFade) //enable fade-in
                {
                    Log("Fade-in enabled");
                    Bass.BASS_ChannelSetAttribute(BassMixer, BASSAttribute.BASS_ATTRIB_VOL, 0);
                    Bass.BASS_ChannelSlideAttribute(BassMixer, BASSAttribute.BASS_ATTRIB_VOL, track_vol, (int)(FadeLength * 1000));
                }
                else //no fade-in
                {
                    Log("Fade-in disabled");
                    Bass.BASS_ChannelSetAttribute(BassMixer, BASSAttribute.BASS_ATTRIB_VOL, track_vol);
                }

                //start video playback if possible
                if (yarg.Checked && displayBackgroundVideo.Checked)
                {
                    StartVideoPlayback();
                }
                //start mix playback
                if (!Bass.BASS_ChannelPlay(BassMixer, false))
                {
                    MessageBox.Show("Error starting BASS playback:\n" + Bass.BASS_ErrorGetCode(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            PrepareForDrawing();
            UpdatePlaybackStuff();
            UpdateStats();

            if (doNext && picLoop.Tag.ToString() != "loop")
            {
                //GetNextSong();
            }
        }

        private void SetVideoPlayerPath(string ini)
        {
            Log("Video not yet loaded, trying to load");
            MediaPlayer.URL = "";
            var video_path = "";
            if (string.IsNullOrEmpty(ini) || !File.Exists(ini)) return;
            var sr = new StreamReader(ini);
            while (sr.Peek() >= 0)
            {   //read value for Phase Shift entry in song.ini
                var line = sr.ReadLine();
                if (!line.Contains("video =") && !line.Contains("video=")) continue;
                video_path = Path.GetDirectoryName(ini) + "\\" + Tools.GetConfigString(line).Trim();
                break;
            }
            sr.Dispose();
            if (string.IsNullOrEmpty(video_path) || !File.Exists(video_path))
            {
                var path = Path.GetDirectoryName(ini); //default
                //search for mid file in YARG exCON folder, video should be where the .mid file is
                var possible_folder = Directory.GetFiles(Path.GetDirectoryName(ini), "*.mid", SearchOption.AllDirectories);
                if (possible_folder.Any())
                {
                    path = Path.GetDirectoryName(possible_folder[0]);
                }
                var backgrounds = Directory.GetFiles(path);
                for (var i = 0; i < backgrounds.Count(); i++)
                {
                    switch (Path.GetFileName(backgrounds[i]).ToLowerInvariant())
                    {
                        case "background.avi":
                        case "video.mp4":
                        case "video.webm":
                        case "bg.mp4":
                        case "bg.webm":
                            video_path = backgrounds[i];
                            break;
                        default:
                            break;
                    }
                }
            }
            if (string.IsNullOrEmpty(video_path)) return;
            MediaPlayer.URL = video_path;
        }

        private void StartVideoPlayback()
        {
            if (PlayingSong == null) return;
            if (string.IsNullOrEmpty(MediaPlayer.URL))
            {
                SetVideoPlayerPath(string.IsNullOrEmpty(sngPath) ? PlayingSong.Location : Application.StartupPath + "\\temp\\song.ini");
            }
            if (string.IsNullOrEmpty(MediaPlayer.URL)) return;
            Log("Starting video playback");
            VideoIsPlaying = true;
            ClearVisuals();
            MediaPlayer.Visible = true;
            MediaPlayer.BringToFront();
            MediaPlayer.Ctlcontrols.play();
            MediaPlayer.Ctlcontrols.currentPosition = PlaybackSeconds;
        }

        public void SetPlayLocation(double time, bool seeking = false)
        {
            if (time < 0)
            {
                time = 0.0;
            }

            if (time < 0)
            {
                time = 0.0;
            }
            if ((MediaPlayer.playState == WMPPlayState.wmppsPlaying || MediaPlayer.playState == WMPPlayState.wmppsPaused) && !seeking)
            {
                MediaPlayer.Ctlcontrols.currentPosition = time;
            }
            if ((opusFiles.Any() || oggFiles.Any() || wavFiles.Any() || mp3Files.Any()) && BassStreams.Count() > 1)
            {
                foreach (var stream in BassStreams)
                {
                    try
                    {
                        BassMix.BASS_Mixer_ChannelSetPosition(stream, Bass.BASS_ChannelSeconds2Bytes(stream, time));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error setting play location: " + ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                try
                {
                    BassMix.BASS_Mixer_ChannelSetPosition(BassStream, Bass.BASS_ChannelSeconds2Bytes(BassStream, time));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error setting play location: " + ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void UpdatePlaybackStuff()
        {            
            UpdateNotifyTray();
            PlaybackTimer.Enabled = true;
        }

        private void StopPlayback(bool Pause = false)
        {
            try
            {
                PlaybackTimer.Enabled = false;
                if (Pause)
                {
                    Log("Pausing playback");
                    if (!Bass.BASS_ChannelPause(BassMixer))
                    {
                        Log("Error pausing playback: " + Bass.BASS_ErrorGetCode());
                        MessageBox.Show("Error pausing playback\n" + Bass.BASS_ErrorGetCode());
                    }
                    if (MediaPlayer.playState == WMPPlayState.wmppsPlaying)
                    {
                        MediaPlayer.Ctlcontrols.pause();
                    }
                }
                else
                {
                    Log("Stopping playback");
                    StopVideoPlayback();
                    StopBASS();
                }
            }
            catch (Exception ex)
            {
                Log("Error stopping playback: " + ex.Message);
            }
        }

        private void StopVideoPlayback(bool stop = true)
        {
            VideoIsPlaying = (MediaPlayer.playState == WMPPlayState.wmppsPlaying || MediaPlayer.playState == WMPPlayState.wmppsPaused) && displayBackgroundVideo.Checked;
            Log("Stopping video playback");
            if (stop)
            {
                MediaPlayer.Ctlcontrols.stop();
                MediaPlayer.close();
            }
            else
            {
                MediaPlayer.Ctlcontrols.pause();
            }
            MediaPlayer.Visible = false;
        }

        private void StopBASS()
        {
            Log("Releasing BASS resources");
            try
            {
                Bass.BASS_ChannelStop(BassMixer);
                Bass.BASS_StreamFree(BassMixer);
                Bass.BASS_StreamFree(BassStream);
                foreach (var stream in BassStreams)
                {
                    Bass.BASS_StreamFree(stream);
                }
            }
            catch (Exception ex)
            {
                Log("Error stopping BASS: " + ex.Message);
            }
        }

        private string GetMusicNotes()
        {
            //"♫ ♫ ♫ ♫"
            var quarter = (int)((PlaybackSeconds - (int)PlaybackSeconds) * 100);
            string notes;
            if (quarter >= 0 && quarter < 25)
            {
                notes = "♫";
            }
            else if (quarter >= 25 && quarter < 50)
            {
                notes = "♫ ♫";
            }
            else if (quarter >= 50 && quarter < 75)
            {
                notes = "♫ ♫ ♫";
            }
            else
            {
                notes = "♫ ♫ ♫ ♫";
            }
            return notes;
        }

        public string ProcessLine(string line, bool clean)
        {
            if (line == null) return "";
            string newline;
            if (clean)
            {
                newline = line.Replace("$", "");
                newline = newline.Replace("%", "");
                newline = newline.Replace("#", "");
                newline = newline.Replace("^", "");
                newline = newline.Replace("- + ", "");
                newline = newline.Replace("+- ", "");
                newline = newline.Replace("- ", "");
                newline = newline.Replace(" + ", " ");
                newline = newline.Replace(" +", "");
                newline = newline.Replace("+ ", "");
                newline = newline.Replace("+-", "");
                newline = newline.Replace("=", "-");
                newline = newline.Replace("§", "‿");
                newline = newline.Replace("- ", "-").Trim();
                if (newline.EndsWith("+", StringComparison.Ordinal))
                {
                    newline = newline.Substring(0, newline.Length - 1).Trim();
                }
                if (newline.EndsWith("-", StringComparison.Ordinal))
                {
                    newline = newline.Substring(0, newline.Length - 1);
                }
            }
            else
            {
                newline = line;
            }
            return newline.Replace("/", "").Trim();
        }

        public void UpdateDisplay(bool PrepareToDraw = true)
        {
            if (isClosing) return;
            if (PrepareToDraw)
            {
                PrepareForDrawing();
            }
            var doShow = openSideWindow.Checked;
            Width = doShow ? 1000 : 412;
            lblUpdates.Width = doShow ? 590 : 184;
            lblUpdates.Left = (openSideWindow.Checked ? picVisuals.Left + picVisuals.Width : panelPlaying.Left + panelPlaying.Width) - lblUpdates.Width;
            picVisuals.Image = displayAlbumArt.Checked && File.Exists(CurrentSongArtBlurred) ? Tools.NemoLoadImage(CurrentSongArtBlurred) : null;
            lblSections.Parent = picVisuals;
            lblSections.Visible = showPracticeSections.Checked && MIDITools.PracticeSessions.Any() && !chartVertical.Checked;
            lblSections.BackColor = yarg.Checked && displayBackgroundVideo.Checked && !string.IsNullOrEmpty(MediaPlayer.URL) ? Color.Black : LabelBackgroundColor;
            lblSections.Refresh();

            MediaPlayer.Parent = picVisuals;
            MediaPlayer.Top = lblSections.Visible ? lblSections.Height : 0;
            MediaPlayer.Left = 0;
            MediaPlayer.Height = picVisuals.Height - GetHeightDiff();
            MediaPlayer.Width = picVisuals.Width;
            if (displayMIDIChartVisuals.Checked)
            {
                PlaybackTimer.Interval = 16;
            }
            else if (displayKaraokeMode.Checked)
            {
                PlaybackTimer.Interval = 30;
            }
            else
            {
                PlaybackTimer.Interval = 50;
            }
        }

        private int GetHeightDiff()
        {
            if (!doMIDIHarmonies && !doMIDIVocals)
            {
                return 4;
            }
            if (displayMIDIChartVisuals.Checked && chartVertical.Checked)
            {
                return vocalsHeight + 4;
            }
            var heightDiff = 0;
            if (lblSections.Visible && !chartVertical.Checked)
            {
                heightDiff += lblSections.Height;
            }
            if (doScrollingLyrics || doStaticLyrics || doKaraokeLyrics)
            {
                if (doHarmonyLyrics)
                {
                    heightDiff += MIDITools.LyricsHarm3.Lyrics.Any() ? 60 : (MIDITools.LyricsHarm2.Lyrics.Any() ? 40 : 20);
                }
                else
                {
                    heightDiff += 20;
                }
            }
            return heightDiff;
        }

        private static void UpdateTextQuality(Graphics graphics)
        {
            graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            graphics.SmoothingMode = SmoothingMode.HighSpeed;
            graphics.CompositingQuality = CompositingQuality.HighSpeed;
        }

        private void howToUseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var message = Tools.ReadHelpFile("pl");
            var help = new HelpForm(AppName + " - Help", message, true);
            help.ShowDialog();
            Log("Displayed help file");
        }

        private void folderScanner_DoWork(object sender, DoWorkEventArgs e)
        {
            Log("Folder scanner working");
            Log("Searching for " + GetCurrentDataType() + " files");

            var files = Directory.GetFiles(Environment.CurrentDirectory, "*.*", SearchOption.AllDirectories);

            if (xbox360.Checked || bandFuse.Checked)
            {
                SongsToAdd.AddRange(
                    files.Where(file =>
                    {
                        try
                        {
                            return VariousFunctions.ReadFileType(file) == XboxFileType.STFS;
                        }
                        catch
                        {
                            return false; // Skip this file on error
                        }
                    }).ToList());
            }
            else if (yarg.Checked)
            {
                SongsToAdd.AddRange(files.Where(file => Path.GetFileName(file) == "song.ini").ToList());
                SongsToAdd.AddRange(files.Where(file => Path.GetFileName(file) == "songs.dta").ToList());
                SongsToAdd.AddRange(files.Where(file => Path.GetExtension(file) == ".sng").ToList());
                SongsToAdd.AddRange(files.Where(file => Path.GetExtension(file) == ".yargsong").ToList());
            }
            else if (pS3.Checked)
            {
                SongsToAdd.AddRange(files.Where(file => Path.GetFileName(file) == "songs.dta").ToList());
                SongsToAdd.AddRange(files.Where(file => Path.GetExtension(file) == ".pkg").ToList());
            }
            else if (rockSmith.Checked)
            {
                SongsToAdd.AddRange(files.Where(file => Path.GetExtension(file) == ".psarc").ToList());
            }
            else if (wii.Checked)
            {
                SongsToAdd.AddRange(files.Where(file => Path.GetFileName(file) == "songs.dta").ToList());
            }
            else if (guitarHero.Checked)
            {
                SongsToAdd.AddRange(files.Where(file => Path.GetFileName(file) == "song.ini").ToList());
            }
            else if (fortNite.Checked)
            {
                SongsToAdd.AddRange(files.Where(file => Path.GetExtension(file) == ".fnf").ToList());
            }
            else if (powerGig.Checked)
            {
                SongsToAdd.AddRange(files.Where(file => Path.GetExtension(file) == ".xml").ToList());
            }
            Log("Found " + SongsToAdd.Count + " " + GetCurrentDataType() + " file(s)");
        }

        private string GetCurrentDataType()
        {
            string type;
            if (xbox360.Checked)
            {
                type = "CON | LIVE";
            }
            else if (yarg.Checked)
            {
                type = "song.ini | songs.dta | .sng | .yargsong";
            }
            else if (pS3.Checked)
            {
                type = "songs.dta | .pkg";
            }
            else if (rockSmith.Checked)
            {
                type = ".psarc";
            }
            else if (fortNite.Checked)
            {
                type = ".fnf | .m4a";
            }
            else if (guitarHero.Checked)
            {
                type = "song.ini | fsb.xen";
            }
            else if (powerGig.Checked)
            {
                type = ".xml";
            }
            else if (bandFuse.Checked)
            {
                type = "LIVE";
            }
            else
            {
                type = "songs.dta";
            }
            return type;
        }

        private void folderScanner_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Log("Folder scanner finished");
            if (GIFOverlay != null)
            {
                GIFOverlay.Close();
                GIFOverlay = null;
            }
            var type = GetCurrentDataType();
            if (!SongsToAdd.Any())
            {
                var msg = "No " + type + " files found in that folder, nothing to add to the playlist";
                Log(msg);
                MessageBox.Show(msg, AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                isScanning = false;
                EnableDisable(true);
                return;
            }
            var found = "Found " + SongsToAdd.Count + " " + type + " " + (SongsToAdd.Count == 1 ? "file" : "files") + ", analyzing...";
            Log(found);
            ShowUpdate(found);
            StartingCount = lstPlaylist.Items.Count;
            isScanning = true;
            UpdateNotifyTray();
            InitiateGIFOverlay();
            batchSongLoader.RunWorkerAsync();
        }

        private void cancelProcess_Click(object sender, EventArgs e)
        {
            if (!batchSongLoader.IsBusy && !songLoader.IsBusy) return;
            CancelWorkers = true;
            Log("User cancelled running process");
        }

        private void openFileLocation_Click(object sender, EventArgs e)
        {
            var file = ActiveSong.Location;
            Process.Start("explorer" + EXE, "/select," + "\"" + file + "\"");
            Log("Opened file in location: " + file);
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            if (lblUpdates.InvokeRequired)
            {
                lblUpdates.Invoke(new MethodInvoker(() => lblUpdates.Text = ""));
            }
            else
            {
                UpdateStats();
            }
            UpdateTimer.Enabled = false;
        }

        private void UpdateStats()
        {
            lblUpdates.Text = "";
            if (lstPlaylist.Items.Count == 0) return;

            try
            {
                long time = 0;
                for (var i = 0; i < lstPlaylist.Items.Count; i++)
                {
                    var ind = Convert.ToInt16(lstPlaylist.Items[i].SubItems[0].Text) - 1;
                    time += Playlist[ind].Length;
                }
                lblUpdates.Text = "Songs: " + lstPlaylist.Items.Count;
                if (openSideWindow.Checked && string.IsNullOrEmpty(activeM4AFile))
                {
                    lblUpdates.Text = lblUpdates.Text + "   |   Playing Time: " + Parser.GetSongDuration(time.ToString(CultureInfo.InvariantCulture));
                }
                Log(lblUpdates.Text);
            }
            catch (Exception ex)
            {
                Log("Error in UpdateStats: " + ex.Message);
                MessageBox.Show(ex.Message);
            }
        }

        private void PlaylistContextMenu_Opening(object sender, CancelEventArgs e)
        {
            PlaylistContextMenu.Enabled = !songExtractor.IsBusy && !songPreparer.IsBusy;
            if (GIFOverlay != null || lstPlaylist.Items.Count == 0)
            {
                e.Cancel = true;
                return;
            }

            playNowToolStripMenuItem.Visible = lstPlaylist.SelectedItems.Count == 1;
            playNextToolStripMenuItem.Visible = lstPlaylist.SelectedItems.Count == 1 && PlayingSong != null;
            removeToolStripMenuItem.Visible = lstPlaylist.SelectedItems.Count > 0;
            moveUpToolStripMenuItem.Visible = lstPlaylist.SelectedItems.Count == 1;
            moveDownToolStripMenuItem.Visible = lstPlaylist.SelectedItems.Count == 1;
            goToArtist.Visible = lstPlaylist.SelectedItems.Count == 1;
            markAsPlayed.Visible = false;
            markAsUnplayed.Visible = false;
            goToAlbum.Visible = false;
            goToGenre.Visible = false;
            returnToPlaylist.Visible = Playlist.Count != StaticPlaylist.Count;
            sortPlaylistByArtist.Visible = lstPlaylist.Items.Count > 0;
            sortPlaylistByDuration.Visible = lstPlaylist.Items.Count > 0 && !m4aFiles.Any();
            sortPlaylistBySong.Visible = lstPlaylist.Items.Count > 0;
            randomizePlaylist.Visible = lstPlaylist.Items.Count > 0;
            startInstaMix.Visible = lstPlaylist.Items.Count > 0;
            openFileLocation.Visible = lstPlaylist.SelectedItems.Count == 1;
            try
            {
                var index = lstPlaylist.SelectedIndices[0];
                if (index == 0)
                {
                    moveUpToolStripMenuItem.Visible = false;
                }
                if (index == lstPlaylist.Items.Count - 1)
                {
                    moveDownToolStripMenuItem.Visible = false;
                }
                if (Bass.BASS_ChannelIsActive(BassMixer) == BASSActive.BASS_ACTIVE_PLAYING && PlayingSong.Index == index)
                {
                    playNextToolStripMenuItem.Visible = false;
                }
                var ind = Convert.ToInt16(lstPlaylist.Items[index].SubItems[0].Text) - 1;
                goToAlbum.Visible = lstPlaylist.SelectedItems.Count == 1 && !string.IsNullOrEmpty(Playlist[ind].Album);
                goToGenre.Visible = lstPlaylist.SelectedItems.Count == 1 && !string.IsNullOrEmpty(Playlist[ind].Genre);
                markAsPlayed.Visible = lstPlaylist.SelectedItems.Count > 1 ||
                                       (lstPlaylist.SelectedItems.Count == 1 &&
                                        lstPlaylist.SelectedItems[0].Tag.ToString() == "0");
                markAsUnplayed.Visible = lstPlaylist.SelectedItems.Count > 1 ||
                                         (lstPlaylist.SelectedItems.Count == 1 &&
                                          lstPlaylist.SelectedItems[0].Tag.ToString() == "1");
            }
            catch (Exception ex)
            {
                Log("Error in PlaylistContextMenu_Opening:" + ex.Message);
            }
        }

        private void NotifyContextMenu_Opening(object sender, CancelEventArgs e)
        {
            restoreToolStripMenuItem.Visible = WindowState == FormWindowState.Minimized;
            playToolStripMenuItem.Visible = Bass.BASS_ChannelIsActive(BassMixer) != BASSActive.BASS_ACTIVE_PLAYING && picPlay.Enabled;
            pauseToolStripMenuItem.Visible = Bass.BASS_ChannelIsActive(BassMixer) == BASSActive.BASS_ACTIVE_PLAYING && picPlay.Enabled;
            nextToolStripMenuItem.Visible = picNext.Enabled;
        }

        private void returnToPlaylist_Click(object sender, EventArgs e)
        {
            Log("Current playlist has " + Playlist.Count + " song(s)");
            Log("Restoring full playlist which has " + StaticPlaylist.Count + " song(s)");
            Playlist = StaticPlaylist;
            btnClear.PerformClick();
            ReloadPlaylist(Playlist);
            UpdateHighlights();
        }

        private void txtSearch_EnabledChanged(object sender, EventArgs e)
        {
            btnClear.Enabled = txtSearch.Enabled;
            btnSearch.Enabled = txtSearch.Enabled;
            btnGoTo.Enabled = txtSearch.Enabled;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text == strSearchPlaylist || btnClear.ForeColor != Color.Black) return;
            txtSearch.Invoke(new MethodInvoker(() => txtSearch.Text = strSearchPlaylist));
            if (lstPlaylist.Items.Count != StaticPlaylist.Count)
            {
                ReloadPlaylist(Playlist, true, true, false);
            }
            if (PlayingSong == null) return;
            UpdateHighlights();
        }

        private void scanForSongsAutomatically_Click(object sender, EventArgs e)
        {
            if (batchSongLoader.IsBusy)
            {
                MessageBox.Show("Wait until I finish loading the last batch of songs added", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            var ofd = new FolderPicker
            {
                Title = "Select folder to scan for songs",
                InputPath = Environment.CurrentDirectory,
            };
            if (ofd.ShowDialog(IntPtr.Zero) != true || string.IsNullOrEmpty(ofd.ResultPath)) return;
            Environment.CurrentDirectory = ofd.ResultPath;
            if (MessageBox.Show("This might take a while depending on how many subfolders and how many files are in the folder\nAre you sure you want to do this now?",
                    AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }
            EnableDisable(false);
            SongsToAdd.Clear();
            Log("scanForSongsAutomatically_Click");
            isScanning = true;
            UpdateNotifyTray();
            InitiateGIFOverlay();
            folderScanner.RunWorkerAsync();
        }

        private void selectAndAddSongsManually_Click(object sender, EventArgs e)
        {
            if (batchSongLoader.IsBusy)
            {
                MessageBox.Show("Wait until I finish loading the last batch of songs added", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            Log("selectAndAddSongsManually_Click");

            var ofd = new OpenFileDialog
            {
                Title = "Select files to add to playlist",
                Multiselect = true,
                InitialDirectory = Environment.CurrentDirectory
            };
            if (ofd.ShowDialog() != DialogResult.OK)
            {
                Log("User cancelled");
                ofd.Dispose();
                return;
            }
            Environment.CurrentDirectory = Path.GetDirectoryName(ofd.FileNames[0]);
            EnableDisable(false);
            SongsToAdd.Clear();
            SongToLoad = "";
            if (xbox360.Checked)
            {
                try
                {
                    SongsToAdd = ofd.FileNames.Where(file => VariousFunctions.ReadFileType(file) == XboxFileType.STFS).ToList();
                }
                catch (Exception ex)
                {
                    Log("Error reading file: " + ex.Message);
                    MessageBox.Show((ofd.FileNames.Count() == 1 ? "There was an error reading that file" : "One or more of those files caused a read error") + ":\n'" + ex.Message + "'\nTry again",
                        AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else if (yarg.Checked)
            {
                SongsToAdd = ofd.FileNames.Where(file => Path.GetFileName(file) == "song.ini").ToList();
                var sng = ofd.FileNames.Where(file => Path.GetExtension(file) == ".sng").ToList();
                SongsToAdd.AddRange(sng);
                var yargsong = ofd.FileNames.Where(file => Path.GetExtension(file) == ".yargsong").ToList();
                SongsToAdd.AddRange(yargsong);
            }
            else if (rockSmith.Checked)
            {
                SongsToAdd = ofd.FileNames.Where(file => Path.GetExtension(file) == ".psarc").ToList();
            }
            else
            {
                SongsToAdd = ofd.FileNames.Where(file => Path.GetFileName(file) == "songs.dta").ToList();
                if (pS3.Checked)
                {
                    var pkg = ofd.FileNames.Where(file => Path.GetExtension(file) == ".pkg").ToList();
                    SongsToAdd.AddRange(pkg);
                }
            }
            if (SongsToAdd.Any())
            {
                SongToLoad = SongsToAdd[0];
            }
            if (!SongsToAdd.Any() && string.IsNullOrEmpty(SongToLoad))
            {
                var msg = "No valid files were selected";
                Log(msg);
                MessageBox.Show(msg, AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                EnableDisable(true);
                ofd.Dispose();
                return;
            }
            Log("Selected " + SongsToAdd.Count + " file(s)");
            StartingCount = lstPlaylist.Items.Count;
            isScanning = true;
            UpdateNotifyTray();
            InitiateGIFOverlay();
            if (ofd.FileNames.Count() > 1)
            {
                batchSongLoader.RunWorkerAsync();
            }
            else
            {
                songLoader.RunWorkerAsync();
            }
            ofd.Dispose();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SavePlaylist(true);
        }

        private void renamePlaylist_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(PlaylistName) || string.IsNullOrEmpty(PlaylistPath)) return;
            const string message = "Enter playlist name:";
            var input = Interaction.InputBox(message, AppName, PlaylistName);
            if (string.IsNullOrEmpty(input) || input.Trim() == PlaylistName) return;
            Log("Renamed playlist from " + PlaylistName + " to " + input);
            PlaylistName = input;
            Tools.DeleteFile(PlaylistPath);
            PlaylistPath = Application.StartupPath + "\\playlists\\" + Tools.CleanString(input, true) + ".playlist";
            var unsaved = Text.Contains("*");
            SavePlaylist(false);
            if (unsaved)
            {
                MarkAsModified();
            }
        }

        private void frmMain_Shown(object sender, EventArgs e)
        {
            ClearAll();
            LoadConfig();
            CenterToScreen();
            picRandom.Image = Resources.dice_trippy;
            UpdateRecentPlaylists("");
            MediaPlayer.uiMode = "none";
            MediaPlayer.settings.volume = 0;
            MediaPlayer.windowlessVideo = true;
            MediaPlayer.Ctlenabled = false;
            MediaPlayer.enableContextMenu = false;
            MediaPlayer.stretchToFit = true;
            UpdateDisplay(false);
            Application.DoEvents();
            Activate();
            InitBASS();
            if (!string.IsNullOrEmpty(PlaylistPath) && autoloadLastPlaylist.Checked && File.Exists(PlaylistPath))
            {
                PrepareToLoadPlaylist();
            }
            updater.RunWorkerAsync();
        }

        private void PrepareToLoadPlaylist(string playlist = "")
        {
            if (!string.IsNullOrEmpty(playlist))
            {
                PlaylistPath = playlist;
            }
            lblUpdates.Text = "Loading Playlist...";
            lblUpdates.Refresh();
            LoadPlaylist();
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F && ModifierKeys.HasFlag(Keys.Control))
            {
                btnClear.PerformClick();
                txtSearch.Focus();
            }
            else if (e.KeyCode == Keys.Enter && ModifierKeys.HasFlag(Keys.Control))
            {
                lstPlaylist_MouseDoubleClick(null, null);
            }
            else if (e.KeyCode == Keys.Space && !txtSearch.Focused && txtSearch.BackColor == Color.Black)
            {
                picPlay_MouseClick(null, null);
            }
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter) return;
            var enabled = !string.IsNullOrEmpty(txtSearch.Text.Trim()) && txtSearch.Text != strSearchPlaylist;
            if (!enabled) return;
            btnSearch.Enabled = enabled;
            btnGoTo.Enabled = enabled;
            btnClear.Enabled = enabled;
        }

        static public Bitmap CopyChartSection(Bitmap srcBitmap, Rectangle section)
        {
            var bmp = new Bitmap(section.Width, section.Height);
            var g = Graphics.FromImage(bmp);
            g.DrawImage(srcBitmap, 0, 0, section, GraphicsUnit.Pixel);
            g.Dispose();
            return bmp;
        }

        public void ClearNoteColors(bool vocals_only = false, bool prokeys_only = false)
        {
            if (MIDITools.MIDI_Chart == null || MIDITools.PhrasesVocals == null) return;
            try
            {
                if (!vocals_only)
                {
                    foreach (var var in MIDITools.MIDI_Chart.ProKeys.ChartedNotes)
                    {
                        var.NoteColor = Color.Empty;
                    }
                }
                if (prokeys_only) return;
                foreach (var var in MIDITools.MIDI_Chart.Vocals.ChartedNotes)
                {
                    var.NoteColor = Color.Empty;
                }
                foreach (var var in MIDITools.MIDI_Chart.Harm1.ChartedNotes)
                {
                    var.NoteColor = Color.Empty;
                }
                foreach (var var in MIDITools.MIDI_Chart.Harm2.ChartedNotes)
                {
                    var.NoteColor = Color.Empty;
                }
                foreach (var var in MIDITools.MIDI_Chart.Harm3.ChartedNotes)
                {
                    var.NoteColor = Color.Empty;
                }
                if (vocals_only) return;
                foreach (var var in MIDITools.MIDI_Chart.Drums.ChartedNotes)
                {
                    var.NoteColor = Color.Empty;
                }
                foreach (var var in MIDITools.MIDI_Chart.Bass.ChartedNotes)
                {
                    var.NoteColor = Color.Empty;
                }
                foreach (var var in MIDITools.MIDI_Chart.Guitar.ChartedNotes)
                {
                    var.NoteColor = Color.Empty;
                }
                foreach (var var in MIDITools.MIDI_Chart.Keys.ChartedNotes)
                {
                    var.NoteColor = Color.Empty;
                }
            }
            catch (Exception)
            { }
        }

        private void openSideWindow_Click(object sender, EventArgs e)
        {
            UpdateDisplay();
            UpdateStats();
            if (!openSideWindow.Checked && WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
            }
        }

        private void VisualsContextMenu_Opening(object sender, CancelEventArgs e)
        {
            displayBackgroundVideo.Visible = yarg.Checked;
            toolStripMenuItem2.Visible = yarg.Checked;
            displayKaraokeMode.Enabled = PlayingSong == null || (MIDITools.PhrasesVocals.Phrases.Any() && MIDITools.LyricsVocals.Lyrics.Any());
            styleToolStripMenuItem.Visible = displayMIDIChartVisuals.Checked;
            toolStripMenuItem8.Visible = displayMIDIChartVisuals.Checked;
            displayAlbumArt.Enabled = PlayingSong == null || File.Exists(CurrentSongArtBlurred);
            displayMIDIChartVisuals.Enabled = !hasNoMIDI;
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return) return;
            e.Handled = true;
            if (ShowingNotFoundMessage) return;
            btnGoTo.PerformClick();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text == strSearchPlaylist || Playlist.Count == 0) return;
            Log("btnSearch_Click: " + txtSearch.Text);
            ReloadPlaylist(Playlist);
        }

        private void btnGoTo_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text == strSearchPlaylist || Playlist.Count == 0) return;
            Log("btnGoTo_Click: " + txtSearch.Text);
            GoToSearchTerm(txtSearch.Text.Trim().ToLowerInvariant(), true);
        }

        private void GoToSearchTerm(string search, bool UserSearch)
        {
            try
            {
                var select = -1;
                var start = lstPlaylist.SelectedIndices[0] + 1;
                //start from current selection and go down to bottom
                for (var i = start; i < lstPlaylist.Items.Count; i++)
                {
                    if (UserSearch && !lstPlaylist.Items[i].SubItems[1].Text.ToLowerInvariant().Contains(search))
                    {
                        continue;
                    }
                    if (!UserSearch && !lstPlaylist.Items[i].SubItems[1].Text.ToLowerInvariant().StartsWith(search, StringComparison.Ordinal))
                    {
                        continue;
                    }
                    select = i;
                    break;
                }
                if (select == -1)
                {
                    //nothing found, let's try from the top to the current selection
                    for (var i = 0; i < start; i++)
                    {
                        if (UserSearch && !lstPlaylist.Items[i].SubItems[1].Text.ToLowerInvariant().Contains(search))
                        {
                            continue;
                        }
                        if (!UserSearch && !lstPlaylist.Items[i].SubItems[1].Text.ToLowerInvariant().StartsWith(search, StringComparison.Ordinal))
                        {
                            continue;
                        }
                        select = i;
                        break;
                    }
                }
                if (select == -1)
                {
                    if (!UserSearch) return;
                    txtSearch.Refresh();
                    var msg = "Search term '" + search + "' was not found";
                    Log(msg);
                    ShowingNotFoundMessage = true;
                    MessageBox.Show(msg, AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ShowingNotFoundMessage = false;
                    return;
                }
                Log("Search term found, index: " + select);
                if (ActiveSong != null)
                {
                    lstPlaylist.Items[ActiveSong.Index].Selected = false;
                }
                lstPlaylist.Items[select].Selected = true;
                lstPlaylist.Items[select].Focused = true;
                lstPlaylist.EnsureVisible(select);
            }
            catch (Exception ex)
            {
                Log("Error finding search term '" + search + "':");
                Log(ex.Message);
            }
        }

        private void showPracticeSections_Click(object sender, EventArgs e)
        {
            Log("showPracticeSections_Click");
            UpdateDisplay(false);
        }

        private void lstPlaylist_KeyPress(object sender, KeyPressEventArgs e)
        {
            const string valid_keys = "abcdefghijklmnopqrstuvwxyz1234567890";
            var input = e.KeyChar.ToString(CultureInfo.InvariantCulture).ToLowerInvariant();
            if (!valid_keys.Contains(input)) return;
            try
            {
                GoToSearchTerm(e.KeyChar.ToString(CultureInfo.InvariantCulture).ToLowerInvariant(), false);
                e.Handled = true;
            }
            catch (Exception ex)
            {
                Log("Error handling lstPlaylist_KeyPress:");
                Log(ex.Message);
            }
        }

        private void lstPlaylist_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up && ModifierKeys.HasFlag(Keys.Alt))
            {
                MoveSelectionUp();
            }
            else if (e.KeyCode == Keys.Down && ModifierKeys.HasFlag(Keys.Alt))
            {
                MoveSelectionDown();
            }
        }

        public static void Shuffle<T>(IList<T> list)
        {
            var rng = new Random();
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = rng.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        private void randomizePlaylist_Click(object sender, EventArgs e)
        {
            Log("randomizePlaylist_Click");
            SortPlaylist(PlaylistSorting.Shuffle);
        }

        private void startInstaMix_Click(object sender, EventArgs e)
        {
            Log("startInstaMix_Click");
            EnableDisable(false);
            btnClear.PerformClick();
            SongMixer.RunWorkerAsync();
        }

        private void SongMixer_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Log("Song mixer finished");
            EnableDisable(true);
            ReloadPlaylist(Playlist, true, false, false);
            MarkAsModified();
            picShuffle.Tag = "noshuffle";
            toolTip1.SetToolTip(picShuffle, "Enable track shuffling");
            if (PlayingSong == null || ActiveSong != PlayingSong || Bass.BASS_ChannelIsActive(BassMixer) == BASSActive.BASS_ACTIVE_PLAYING)
            {
                lstPlaylist_MouseDoubleClick(null, null);
            }
            else
            {
                lstPlaylist.Items[0].Tag = 1; //played
                UpdateHighlights();
                GetNextSong();
            }
        }

        private void SongMixer_DoWork(object sender, DoWorkEventArgs e)
        {
            Log("Song mixer working");
            var MixSong = ActiveSong;
            const int minSongs = 25;
            //try to get at least 25 songs in the playlist
            //allow 25%, 38% and 50% discrepancy at max
            //don't go beyond 50% discrepancy even if we have less than 25 songs
            CreateSongMix(MixSong, 0.25);
            Log("Created mix with discrepancy factor: 0.25");
            if (Playlist.Count < minSongs)
            {
                CreateSongMix(MixSong, 0.38);
                Log("Created mix with discrepancy factor: 0.38");
            }
            if (Playlist.Count < minSongs)
            {
                CreateSongMix(MixSong, 0.50);
                Log("Created mix with discrepancy factor: 0.50");
            }
            Playlist.Remove(MixSong);
            Shuffle(Playlist);
            var backup = Playlist[0];
            Playlist[0] = MixSong;
            Playlist.Add(backup);
        }

        private void CreateSongMix(Song MixSong, double factor)
        {
            var maxBPM = MixSong.BPM * (1.00 + factor);
            var minBPM = MixSong.BPM * (1.00 - factor);
            var maxLength = MixSong.Length * (1.00 + factor);
            var minLength = MixSong.Length * (1.00 - factor);
            var genre = MixSong.Genre.ToLowerInvariant();
            Playlist = new List<Song>();
            foreach (var song in from song in StaticPlaylist where !(song.BPM < minBPM) && !(song.BPM > maxBPM) where !(song.Length < minLength) && !(song.Length > maxLength) where song.Genre.ToLowerInvariant() == genre select song)
            {
                Playlist.Add(song);
            }
        }

        private void LoadRecent(int playlist)
        {
            Log("Loading recent playlist #" + playlist + ": " + RecentPlaylists[playlist]);
            if (Text.Contains("*"))
            {
                if (MessageBox.Show("You have unsaved changes on the current playlist\nAre you sure you want to do that?",
                        AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            }
            StartNew(false);
            PrepareToLoadPlaylist(RecentPlaylists[playlist]);
        }

        private void recent1_Click(object sender, EventArgs e)
        {
            LoadRecent(0);
        }

        private void recent2_Click(object sender, EventArgs e)
        {
            LoadRecent(1);
        }

        private void recent3_Click(object sender, EventArgs e)
        {
            LoadRecent(2);
        }

        private void recent4_Click(object sender, EventArgs e)
        {
            LoadRecent(3);
        }

        private void recent5_Click(object sender, EventArgs e)
        {
            LoadRecent(4);
        }

        private string GetJumpMessage(double time)
        {
            var message = "Jump to: " + Parser.GetSongDuration(time);
            if (MIDITools.PracticeSessions.Any())
            {
                message = message + " " + GetCurrentSection(time);
            }
            return message;
        }

        private void DrawSpectrum(Control sender, Graphics graphics)
        {
            Spectrum.ChannelIsMixerSource = false;
            //if (displayAudioSpectrum.Checked && (MediaPlayer.playState == WMPPlayState.wmppsPlaying || (MediaPlayer.playState == WMPPlayState.wmppsPaused && displayBackgroundVideo.Checked) || VideoIsPlaying)) return;
            try
            {
                var width = displayAudioSpectrum.Checked && openSideWindow.Checked ? picVisuals.Width : picPreview.Width;
                switch (SpectrumID)
                {
                    // line spectrum (width = resolution)
                    default:
                        SpectrumID = 0;
                        Spectrum.CreateSpectrumLine(BassMixer, graphics, new Rectangle(sender.Location, sender.Size), ChartGreen, ChartRed, Color.White, 2, 2, false, false, false);
                        break;
                    // normal spectrum (width = resolution)
                    case 1:
                        Spectrum.CreateSpectrum(BassMixer, graphics, new Rectangle(sender.Location, sender.Size), ChartGreen, ChartRed, Color.White, false, false, false);
                        break;
                    // line spectrum (full resolution)
                    case 2:
                        Spectrum.CreateSpectrumLine(BassMixer, graphics, new Rectangle(sender.Location, sender.Size), ChartBlue, ChartOrange, Color.White, width / 15, 4, false, true, false);
                        break;
                    // ellipse spectrum (width = resolution)
                    case 3:
                        Spectrum.CreateSpectrumEllipse(BassMixer, graphics, new Rectangle(sender.Location, sender.Size), ChartGreen, ChartRed, Color.White, 1, 2, false, false, false);
                        break;
                    // peak spectrum (width = resolution)
                    case 4:
                        Spectrum.CreateSpectrumLinePeak(BassMixer, graphics, new Rectangle(sender.Location, sender.Size), ChartGreen, ChartYellow, ChartOrange, Color.White, 2, 1, 2, 10, false, false, false);
                        break;
                    // peak spectrum (full resolution)
                    case 5:
                        Spectrum.CreateSpectrumLinePeak(BassMixer, graphics, new Rectangle(sender.Location, sender.Size), ChartGreen, ChartBlue, ChartOrange, Color.White, width / 15, 5, 3, 5, false, true, false);
                        break;
                    // WaveForm
                    case 6:
                        Spectrum.CreateWaveForm(BassMixer, graphics, new Rectangle(sender.Location, sender.Size), ChartGreen, ChartRed, ChartYellow, Color.White, 1, true, false, false);
                        break;
                }
            }
            catch (Exception ex)
            {
                Log("Error drawing audio spectrum:" + ex.Message);
            }
        }

        private void displayAudioSpectrum_Click(object sender, EventArgs e)
        {
            picVisuals.BackColor = Color.White;
            updateDisplayType(sender);
        }

        private void displayAlbumArt_Click(object sender, EventArgs e)
        {
            picVisuals.BackColor = Color.White;
            updateDisplayType(sender);
            toolTip1.SetToolTip(picPreview, "Click to change spectrum style");
        }

        private void displayMIDIChartVisuals_Click(object sender, EventArgs e)
        {
            picVisuals.BackColor = Color.White;
            updateDisplayType(sender);
        }

        private void ChangeDisplay()
        {
            ClearVisuals();
            Log(File.Exists(CurrentSongArt) ? "Album art found, loading" : "No album art found");
            if (!displayAlbumArt.Checked && File.Exists(CurrentSongArt))
            {
                picPreview.Image = Tools.NemoLoadImage(CurrentSongArt);
                picPreview.Cursor = Cursors.Hand;
                toolTip1.SetToolTip(picPreview, "Click to view album art");
            }
            else
            {
                picPreview.Image = Resources.noart3;
                picPreview.Cursor = Cursors.Default;
                toolTip1.SetToolTip(picPreview, "No album art available");
            }
        }

        private void audioTracks_Click(object sender, EventArgs e)
        {
            isChoosingStems = true;
            var selector = new AudioSelector(this);
            selector.Show();
            Log("Displayed audio selector");
        }

        private void showMIDIVisuals_Click(object sender, EventArgs e)
        {
            var selector = new MIDISelector(this);
            selector.Show();
            Log("Displayed MIDI display setting selector");
        }

        private void DrawLyricsScrolling(List<Lyric> lyrics, Font font, Color foreColor, Color backColor, int posY, Graphics graphics)
        {
            if (!openSideWindow.Checked || PlayingSong == null || Bass.BASS_ChannelIsActive(BassMixer) != BASSActive.BASS_ACTIVE_PLAYING || !doScrollingLyrics) return;
            if (lyrics == null || lyrics.Count == 0) return;

            var time = GetCorrectedTime();
            var playbackWindow = PlaybackWindowRBVocals;
            var hitboxPosition = chartVertical.Checked ? HitboxVocalsX + (bmpHitboxVocals.Width / 2) : picVisuals.Width;

            // Draw background for lyrics
            graphics.DrawImage(bmpBackgroundLyrics, 0, posY, picVisuals.Width, 20);

            foreach (var lyric in lyrics)
            {
                if (lyric.LyricStart + lyric.LyricDuration < time) continue;

                if (lyric.LyricStart > time + playbackWindow) return;

                var left = chartVertical.Checked && displayMIDIChartVisuals.Checked
                    ? (int)(((lyric.LyricStart - time) / playbackWindow) * (picVisuals.Width - hitboxPosition)) + hitboxPosition
                    : (int)(((lyric.LyricStart - time) / playbackWindow) * picVisuals.Width);

                // Adjust disappearance for vertical mode
                if (chartVertical.Checked && displayMIDIChartVisuals.Checked)
                {
                    // Disappear halfway between x = 0 and HitboxVocalsX
                    var disappearPosition = HitboxVocalsX / 2;
                    if (left < disappearPosition)
                    {
                        continue;
                    }
                }

                // Non-vertical mode: disappear at the left edge
                if (!chartVertical.Checked && left < 0) continue;

                using (var textBrush = new SolidBrush(displayMIDIChartVisuals.Checked && chartVertical.Checked ? Color.White : Color.Black)) // Ensure good legibility
                {
                    graphics.DrawString(ProcessLine(lyric.LyricText, true), font, textBrush, new PointF(left, posY - 4));
                }
            }
        }               

        private void DrawLyricsKaraoke(IEnumerable<LyricPhrase> phrases, IEnumerable<Lyric> lyrics, Font font, Color foreColor, Color backColor, int posY, Graphics graphics)
        {
            if (lyrics == null || lyrics.Count() == 0) return;
            var time = GetCorrectedTime();

            graphics.DrawImage(bmpBackgroundLyrics, 0, posY, picVisuals.Width, 20);
            
            LyricPhrase line = null;
            foreach (var lyric in phrases.TakeWhile(lyric => lyric.PhraseStart <= time).Where(lyric => lyric.PhraseEnd >= time))
            {
                line = lyric;
            }

            if (line == null || string.IsNullOrEmpty(line.PhraseText)) return;

            var processedLine = ProcessLine(line.PhraseText, doWholeWordsLyrics);
            var lineSize = graphics.MeasureString(processedLine, font);
            var left = (picVisuals.Width - (int)lineSize.Width) / 2;

            // Draw the main line
            using (var textBrush = new SolidBrush(Color.White))
            {
                graphics.DrawString(processedLine, font, textBrush, new PointF(left, posY - 4));
            }

            // Process the second line text
            var line2 = lyrics.Where(lyr => !(lyr.LyricStart < line.PhraseStart))
                              .TakeWhile(lyr => !(lyr.LyricStart > time))
                              .Aggregate("", (current, lyr) => current + " " + lyr.LyricText);

            if (string.IsNullOrEmpty(line2)) return;

            // Draw the second line
            using (var textBrush = new SolidBrush(foreColor))
            {
                graphics.DrawString(ProcessLine(line2, doWholeWordsLyrics), font, textBrush, new PointF(left, posY - 4));
            }
        }

        private void showLyrics_Click(object sender, EventArgs e)
        {
            var selector = new LyricSelector(this);
            selector.Show();
            Log("Displayed lyrics setting selector");
        }

        private static void Log(string line)
        {
            if (string.IsNullOrEmpty(line)) return;
            var logfile = Application.StartupPath + "\\log.txt";
            if (!File.Exists(logfile))
            {
                var sw = new StreamWriter(logfile, false);
                var vers = Assembly.GetExecutingAssembly().GetName().Version;
                var version = " v" + String.Format("{0}.{1}.{2}", vers.Major, vers.Minor, vers.Build);
                sw.WriteLine("//Log file for " + AppName);
                sw.WriteLine("//Created by " + AppName + version);
                sw.Dispose();
            }
            try
            {
                var writer = new StreamWriter(logfile, true);
                writer.WriteLine(GetCurrentTime() + "\t" + line);
                writer.Dispose();
            }
            catch (Exception)
            { }
        }

        private static string GetCurrentTime()
        {
            return DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
        }

        private void panelVisuals_DoubleClick(object sender, EventArgs e)
        {
            WindowState = WindowState == FormWindowState.Maximized ? FormWindowState.Normal : FormWindowState.Maximized;
        }

        private void takeScreenshot_Click(object sender, EventArgs e)
        {
            if (!openSideWindow.Checked)
            {
                MessageBox.Show("No visuals to capture!", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (Uploader.IsBusy)
            {
                MessageBox.Show("Slow down, the other image is still uploading!", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            var folder = Application.StartupPath + "\\Screenshots\\";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            string xOut;
            if (PlayingSong == null)
            {
                xOut = folder + AppName + "_" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour +
                       DateTime.Now.Minute + DateTime.Now.Second + DateTime.Now.Millisecond + ".png";
            }
            else
            {
                var name = Tools.CleanString(PlayingSong.Name, true, true).Replace(" ", "");
                xOut = folder + AppName + "_" + name + "_" + PlaybackSeconds + ".png";
            }
            var location = PointToScreen(picVisuals.Location);
            try
            {
                using (var bitmap = new Bitmap(picVisuals.Width, picVisuals.Height))
                {
                    var g = Graphics.FromImage(bitmap);
                    g.CopyFromScreen(location, new Point(0, 0), picVisuals.Size, CopyPixelOperation.SourceCopy);
                    var myEncoder = System.Drawing.Imaging.Encoder.Quality;
                    var myEncoderParameters = new EncoderParameters(1);
                    myEncoderParameters.Param[0] = new EncoderParameter(myEncoder, 100L);
                    var myImageCodecInfo = Tools.GetEncoderInfo("image/png");
                    bitmap.Save(xOut, myImageCodecInfo, myEncoderParameters);
                }
            }
            catch (Exception ex)
            {
                Log("Error capture visuals:");
                Log(ex.Message);
                MessageBox.Show("Error capture screenshot of visuals:\n" + ex.Message + "\nTry again", AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!uploadScreenshots.Checked) return;
            ImgToUpload = xOut;
            Uploader.RunWorkerAsync();
        }

        private void Uploader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (string.IsNullOrEmpty(ImgURL))
            {
                MessageBox.Show("Failed to upload to Imgur, please try again", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                Clipboard.SetText(ImgURL);
                if (MessageBox.Show("Uploaded to Imgur successfully\nClick OK to open link in browser", AppName, MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Asterisk) != DialogResult.OK) return;
                Process.Start(ImgURL);
            }
        }

        private void Uploader_DoWork(object sender, DoWorkEventArgs e)
        {
            ImgURL = Tools.UploadToImgur(ImgToUpload);
        }

        private void viewSongDetails_Click(object sender, EventArgs e)
        {
            if (ActiveSong == null)
            {
                MessageBox.Show("No song is selected, no details to show", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                var details = "Selected song details:\n\nLocation:\n" + ActiveSong.Location + FormatInfoLine("Index", ActiveSong.Index, 26) +
                    FormatInfoLine("Artist", ActiveSong.Artist, 26) + FormatInfoLine("Title", ActiveSong.Name, 28) + FormatInfoLine("Album", ActiveSong.Album, 24) +
                    FormatInfoLine("Track Number", ActiveSong.Track, 10) + FormatInfoLine("Year", ActiveSong.Year, 28) + FormatInfoLine("Genre", ActiveSong.Genre, 25) +
                    FormatInfoLine("Length", FormatTime(ActiveSong.Length / 1000L), 23) +
                    FormatInfoLine("Charter", string.IsNullOrEmpty(ActiveSong.Charter.Trim()) ? "Unknown" : ActiveSong.Charter, 22) +
                    FormatInfoLine("Internal Name", ActiveSong.InternalName, 11) +
                    FormatInfoLine("Rhythm on Keys?", ActiveSong.isRhythmOnKeys ? "Yes" : "No", 5) + FormatInfoLine("Rhythm on Bass?", ActiveSong.isRhythmOnBass ? "Yes" : "No", 5) +
                    FormatInfoLine("Audio Delay", ActiveSong.PSDelay == 0 ? "None" : ActiveSong.PSDelay.ToString(CultureInfo.InvariantCulture) + " ms", 14) +
                    FormatInfoLine("Channels - Drums", ActiveSong.ChannelsDrums, 4) + FormatInfoLine("Channels - Bass", ActiveSong.ChannelsBass, 8) +
                    FormatInfoLine("Channels - Guitar", ActiveSong.ChannelsGuitar, 5) + FormatInfoLine("Channels - Keys", ActiveSong.ChannelsKeys, 8) +
                    FormatInfoLine("Channels - Vocals", ActiveSong.ChannelsVocals, 5) + FormatInfoLine("Channels - Backing", ActiveSong.ChannelsBacking, 2) +
                    FormatInfoLine("Channels - Crowd", ActiveSong.ChannelsCrowd, 4);
                if (ActiveSong == PlayingSong)
                {
                    var instruments = "";
                    var solos = "";
                    var rangeVocals = "";
                    var rangeHarmonies = "";
                    var rangeProKeys = "";
                    if (MIDITools.MIDI_Chart.Drums.ChartedNotes.Count > 0)
                    {
                        instruments += "D ";
                    }
                    if (MIDITools.MIDI_Chart.Bass.ChartedNotes.Count > 0)
                    {
                        instruments += "B ";
                    }
                    if (MIDITools.MIDI_Chart.Guitar.ChartedNotes.Count > 0)
                    {
                        instruments += "G ";
                    }
                    if (MIDITools.MIDI_Chart.Keys.ChartedNotes.Count > 0)
                    {
                        instruments += "K ";
                    }
                    if (MIDITools.MIDI_Chart.ProKeys.ChartedNotes.Count > 0)
                    {
                        instruments += "PK ";
                        rangeProKeys = FormatInfoLine("Range - Pro Keys", MIDITools.MIDI_Chart.ProKeys.NoteRange.Count, 6);
                    }
                    if (MIDITools.MIDI_Chart.Vocals.ChartedNotes.Count > 0)
                    {
                        instruments += "V ";
                        rangeVocals = FormatInfoLine("Range - Vocals", MIDITools.MIDI_Chart.Vocals.NoteRange.Count, 10);
                    }
                    if (MIDITools.MIDI_Chart.Harm1.ChartedNotes.Count > 0)
                    {
                        instruments += "H1 ";
                        rangeHarmonies = FormatInfoLine("Range - Harmonies", MIDITools.MIDI_Chart.Harm1.NoteRange.Count, 2);
                    }
                    if (MIDITools.MIDI_Chart.Harm2.ChartedNotes.Count > 0)
                    {
                        instruments += "H2 ";
                    }
                    if (MIDITools.MIDI_Chart.Harm3.ChartedNotes.Count > 0)
                    {
                        instruments += "H3 ";
                    }
                    if (MIDITools.MIDI_Chart.Drums.Solos.Count > 0)
                    {
                        solos += "D ";
                    }
                    if (MIDITools.MIDI_Chart.Bass.Solos.Count > 0)
                    {
                        solos += "B ";
                    }
                    if (MIDITools.MIDI_Chart.Guitar.Solos.Count > 0)
                    {
                        solos += "G ";
                    }
                    if (MIDITools.MIDI_Chart.Keys.Solos.Count > 0)
                    {
                        solos += "K ";
                    }
                    if (MIDITools.MIDI_Chart.ProKeys.Solos.Count > 0)
                    {
                        solos += "PK ";
                    }
                    details = details + FormatInfoLine("Average BPM", MIDITools.MIDI_Chart.AverageBPM.ToString(CultureInfo.InvariantCulture), 12) +
                        FormatInfoLine("Uses disco flip?", MIDITools.MIDI_Chart.DiscoFlips.Any() ? "Yes" : "No", 9) +
                        FormatInfoLine("Has Pro Keys?", PlayingSong.hasProKeys && MIDITools.MIDI_Chart.ProKeys.ChartedNotes.Count > 0 ? "Yes" : "No", 11) +
                        FormatInfoLine("Instrument Charts", instruments.Trim(), 4) + FormatInfoLine("Instrument Solos", string.IsNullOrEmpty(solos.Trim()) ? "None" : solos.Trim(), 6) +
                        rangeVocals + rangeHarmonies + rangeProKeys + FormatInfoLine("Practice Sessions", MIDITools.PracticeSessions.Count, 6);
                }
                MessageBox.Show(details + "\nAttenuation:\n" + ActiveSong.AttenuationValues.Trim() + "\nPanning:\n" + ActiveSong.PanningValues.Trim(), AppName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private static string FormatTime(double time)
        {
            if (time >= 3600.0)
            {
                var num1 = (int)(time / 3600.0);
                var num2 = (int)(time - num1 * 3600);
                var num3 = (int)(time - num2 * 60);
                return (string)(object)num1 + (object)":" + (string)(num2 < 10 ? (object)"0" : (object)"") + (string)(object)num2 + ":" + (string)(num3 < 10 ? (object)"0" : (object)"") + (string)(object)num3;
            }
            if (time < 60.0)
            {
                return "0:" + (time < 10.0 ? "0" : "") + (int)time;
            }
            var num4 = (int)(time / 60.0);
            var num5 = (int)(time - num4 * 60);
            return string.Concat(new object[] { num4, ":", num5 < 10 ? "0" : "", num5 });
        }

        private static string FormatInfoLine(string field, int data, int spacer)
        {
            return FormatInfoLine(field, data.ToString(CultureInfo.InvariantCulture), spacer);
        }

        private static string FormatInfoLine(string field, string data, int spacers)
        {
            var str = "";
            for (var index = 0; index < spacers; ++index)
            {
                str += " ";
            }
            return "\n" + field + ": " + str + " " + data;
        }

        private void picRandom_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || lstPlaylist.Items.Count <= 1 || songExtractor.IsBusy || songPreparer.IsBusy) return;
            DoShuffleSongs();
        }

        private void DoShuffleSongs()
        {
            var num1 = ShuffleSongs(true);
            if (num1 < 0 || num1 > lstPlaylist.Items.Count - 1)
            {
                MessageBox.Show("There was an error selecting a song at random, try again", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                NextSongIndex = num1;
                foreach (ListViewItem listViewItem in lstPlaylist.SelectedItems)
                {
                    listViewItem.Selected = false;
                }
                if (NextSongIndex > lstPlaylist.Items.Count - 1)
                {
                    NextSongIndex = 0;
                    DeleteUsedFiles(false);
                }
                lstPlaylist.Items[NextSongIndex].Selected = true;
                lstPlaylist.Items[NextSongIndex].Focused = true;
                lstPlaylist.EnsureVisible(NextSongIndex);
                lstPlaylist_MouseDoubleClick(null, null);
            }
        }

        private void updater_DoWork(object sender, DoWorkEventArgs e)
        {
            var path = Application.StartupPath + "\\bin\\update.txt";
            Tools.DeleteFile(path);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;// (SecurityProtocolType)3072; //TLS 1.2 for .NET 4.0
            using (var client = new WebClient())
            {
                try
                {
                    client.DownloadFile("https://nemosnautilus.com/cplayer/update.txt", path);
                }
                catch (Exception)
                { }
            }
        }

        private static string GetAppVersion()
        {
            var vers = Assembly.GetExecutingAssembly().GetName().Version;
            return "v" + String.Format("{0}.{1}.{2}", vers.Major, vers.Minor, vers.Build);
        }

        private void updater_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var path = Application.StartupPath + "\\bin\\update.txt";
            if (!File.Exists(path))
            {
                if (showUpdateMessage)
                {
                    MessageBox.Show("Unable to check for updates", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                return;
            }
            var thisVersion = GetAppVersion();
            var newVersion = "v";
            string newName;
            string releaseDate;
            string link;
            var changeLog = new List<string>();
            var sr = new StreamReader(path);
            try
            {
                var line = sr.ReadLine();
                if (line.ToLowerInvariant().Contains("html"))
                {
                    sr.Dispose();
                    if (showUpdateMessage)
                    {
                        MessageBox.Show("Unable to check for updates", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    return;
                }
                newName = Tools.GetConfigString(line);
                newVersion += Tools.GetConfigString(sr.ReadLine());
                releaseDate = Tools.GetConfigString(sr.ReadLine());
                link = Tools.GetConfigString(sr.ReadLine());
                sr.ReadLine();//ignore Change Log header
                while (sr.Peek() >= 0)
                {
                    changeLog.Add(sr.ReadLine());
                }
            }
            catch (Exception ex)
            {
                if (showUpdateMessage)
                {
                    MessageBox.Show("Error parsing update file:\n" + ex.Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                sr.Dispose();
                return;
            }
            sr.Dispose();
            Tools.DeleteFile(path);
            if (thisVersion.Equals(newVersion))
            {
                if (showUpdateMessage)
                {
                    MessageBox.Show("You have the latest version", AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }
            var newInt = Convert.ToInt16(newVersion.Replace("v", "").Replace(".", "").Trim());
            var thisInt = Convert.ToInt16(thisVersion.Replace("v", "").Replace(".", "").Trim());
            if (newInt <= thisInt)
            {
                if (showUpdateMessage)
                {
                    MessageBox.Show("You have a newer version (" + thisVersion + ") than what's on the server (" + newVersion + ")\nNo update needed!", AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }
            var updaterForm = new Updater();
            updaterForm.SetInfo(AppName, thisVersion, newName, newVersion, releaseDate, link, changeLog);
            updaterForm.ShowDialog();
        }

        private void checkForUpdates_Click(object sender, EventArgs e)
        {
            showUpdateMessage = true;
            updater.RunWorkerAsync();
        }

        private void viewChangeLog_Click(object sender, EventArgs e)
        {
            const string changelog = "cplayer_changelog.txt";
            if (!File.Exists(Application.StartupPath + "\\" + changelog))
            {
                MessageBox.Show("Changelog file is missing!", AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Process.Start(Application.StartupPath + "\\" + changelog);
        }

        private void sortPlaylistByModifiedDate_Click(object sender, EventArgs e)
        {
            SortPlaylist(PlaylistSorting.ByModifiedDate);
        }

        private void displayKaraokeMode_Click(object sender, EventArgs e)
        {
            picVisuals.BackColor = Color.White;
            updateDisplayType(sender);
        }

        private void picVisuals_Paint(object sender, PaintEventArgs e)
        {
            if (!PlaybackTimer.Enabled || !openSideWindow.Checked || PlayingSong == null || WindowState == FormWindowState.Minimized
                || (MediaPlayer.playState == WMPPlayState.wmppsPaused && displayBackgroundVideo.Checked) || VideoIsPlaying) return;
            UpdateTextQuality(e.Graphics);
            if (displayAlbumArt.Checked)
            {
                DrawLyrics(e.Graphics, Color.FromArgb(127, 200, 200, 200));
                return;
            }
            if (displayAudioSpectrum.Checked)
            {
                DrawSpectrum(picVisuals, e.Graphics);
                DrawLyrics(e.Graphics, Color.FromArgb(127, 200, 200, 200));
                return;
            }
            if (displayMIDIChartVisuals.Checked && chartFull.Checked && DrewFullChart)
            {
                var percent = (GetCorrectedTime() / ((double)PlayingSong.Length / 1000));
                var width = ((int)(picVisuals.Width * percent)) + 1;
                var chart = CopyChartSection(ChartBitmap, new Rectangle(new Point(0, 0), new Size(width, picVisuals.Height)));
                e.Graphics.DrawImage(chart, 0, 0, width, picVisuals.Height);
                chart.Dispose();
                return;
            }
            if (displayKaraokeMode.Checked && MIDITools.PhrasesVocals.Phrases.Any() && MIDITools.LyricsVocals.Lyrics.Any())
            {
                KaraokeOverlay.Visible = false;
                DoKaraokeMode(e.Graphics, MIDITools.PhrasesVocals.Phrases, MIDITools.LyricsVocals.Lyrics);
                return;
            }
            if (displayMIDIChartVisuals.Checked && chartVertical.Checked)
            {
                DrawMIDIFile(e.Graphics);
                return;
            }
            if (!displayMIDIChartVisuals.Checked || (!chartSnippet.Checked && DrewFullChart)) return;
            DrawMIDIFile(e.Graphics);
            DrewFullChart = true;
        }

        private void picPreview_Paint(object sender, PaintEventArgs e)
        {
            if (displayAlbumArt.Checked || (!File.Exists(CurrentSongArt) && !displayAudioSpectrum.Checked))
            {
                DrawSpectrum(picPreview, e.Graphics);
            }
        }

        private void GetIntroOutroSilencePS()
        {
            IntroSilenceNext = 0.0;
            OutroSilenceNext = 0.0;
            if (!skipIntroOutroSilence.Checked || NextSong == null) return;
            var OGGs = Directory.GetFiles(Path.GetDirectoryName(NextSong.Location), "*.ogg", SearchOption.TopDirectoryOnly);
            if (!OGGs.Any()) return;
            List<int> NextSongStreams;
            int mixer;
            if (!PrepMixerPS(OGGs, out mixer, out NextSongStreams)) goto ReleaseTempStreams;
            foreach (var stream in NextSongStreams.Where(stream => stream != 0))
            {
                double newIntroSilence;
                double newOutroSilence;
                ProcessStreamForSilence(stream, out newIntroSilence, out newOutroSilence);
                if (IntroSilenceNext == 0.0 || newIntroSilence < IntroSilenceNext) //we only want earliest instance of sound in all streams
                {
                    IntroSilenceNext = newIntroSilence;
                }
                if (newOutroSilence > OutroSilenceNext) //we only want latest instance of silence in all streams
                {
                    OutroSilenceNext = newOutroSilence;
                }
            }
        ReleaseTempStreams:
            foreach (var stream in NextSongStreams)
            {
                Bass.BASS_StreamFree(stream);
            }
            Bass.BASS_StreamFree(mixer);
        }

        private void GetIntroOutroSilence()
        {
            IntroSilenceNext = 0.0;
            OutroSilenceNext = 0.0;
            if (!skipIntroOutroSilence.Checked || yarg.Checked || powerGig.Checked || bandFuse.Checked || fortNite.Checked || guitarHero.Checked) return;
            if (nautilus.NextSongOggData == null || nautilus.NextSongOggData.Length == 0) return;
            var stream = Bass.BASS_StreamCreateFile(nautilus.GetOggStreamIntPtr(true), 0L, nautilus.NextSongOggData.Length, BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_SAMPLE_FLOAT);
            if (stream == 0)
            {
                nautilus.ReleaseStreamHandle(true);
                return;
            }
            double newIntroSilence;
            double newOutroSilence;
            ProcessStreamForSilence(stream, out newIntroSilence, out newOutroSilence);
            nautilus.ReleaseStreamHandle(true);
            if (IntroSilenceNext == 0.0 || newIntroSilence < IntroSilenceNext)
            {
                IntroSilenceNext = newIntroSilence;
            }
            if (newOutroSilence > OutroSilenceNext)
            {
                OutroSilenceNext = newOutroSilence;
            }
        }

        private void ProcessStreamForSilence(int stream, out double intro, out double outro)
        {
            var buffer = new float[50000];
            long count = 0;
            intro = 0.0;
            outro = 0.0;
            var length = Bass.BASS_ChannelGetLength(stream);
            try
            {
                //detect start silence
                int b;
                do
                {
                    //decode some data
                    b = BassMix.BASS_Mixer_ChannelGetData(stream, buffer, 40000);
                    if (b <= 0) break;
                    //bytes -> samples
                    b /= 4;
                    //count silent samples
                    int a;
                    for (a = 0; a < b && Math.Abs(buffer[a]) <= SilenceThreshold; a++) { }
                    //add number of silent bytes
                    count += a * 4;
                    //if sound has begun...
                    if (a >= b) continue;
                    //move back to a quieter sample (to avoid "click")
                    for (; a > SilenceThreshold / 4 && Math.Abs(buffer[a]) > SilenceThreshold / 4; a--, count -= 4) { }
                    break;
                } while (b > 0);
                intro = Bass.BASS_ChannelBytes2Seconds(stream, count);

                //detect end silence
                var pos = length;
                while (pos > count)
                {
                    //step back a bit
                    pos = pos < 200000 ? 0 : pos - 200000;
                    Bass.BASS_ChannelSetPosition(stream, pos);
                    //decode some data
                    var d = BassMix.BASS_Mixer_ChannelGetData(stream, buffer, 200000);
                    if (d <= 0) break;
                    //bytes -> samples
                    d /= 4;
                    //count silent samples
                    int c;
                    for (c = d; c > 0 && Math.Abs(buffer[c - 1]) <= SilenceThreshold / 2; c--) { }
                    //if sound has begun...
                    if (c <= 0) continue;
                    //silence begins here
                    count = pos + c * 4;
                    break;
                }
                outro = Bass.BASS_ChannelBytes2Seconds(stream, count);
            }
            catch (Exception ex)
            {
                Log("Error calculating silence: " + ex.Message);
            }
        }

        private void updateDisplayType(object sender)
        {
            Log(((ToolStripMenuItem)sender).Name + "_Click");
            displayAlbumArt.Checked = false;
            displayAudioSpectrum.Checked = false;
            displayMIDIChartVisuals.Checked = false;
            displayKaraokeMode.Checked = false;
            ((ToolStripMenuItem)sender).Checked = true;
            ChangeDisplay();
            UpdateDisplay(false);
        }

        private void MediaPlayer_ClickEvent(object sender, _WMPOCXEvents_ClickEvent e)
        {
            if (e.nButton == 2)
            {
                VisualsContextMenu.Show(MousePosition);
            }
        }

        private void displayBackgroundVideo_Click(object sender, EventArgs e)
        {
            ChangeDisplay();
            UpdateDisplay(false);
            if (displayBackgroundVideo.Checked && !string.IsNullOrEmpty(MediaPlayer.URL) && File.Exists(MediaPlayer.URL))
            {
                StartVideoPlayback();
            }
            else if (!displayBackgroundVideo.Checked)
            {
                StopVideoPlayback(false);
            }
            playBGVideos.Checked = displayBackgroundVideo.Checked;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            var enabled = !string.IsNullOrEmpty(txtSearch.Text.Trim()) && txtSearch.Text != strSearchPlaylist;
            txtSearch.ForeColor = enabled ? Color.Black : Color.Gray;
            btnSearch.Enabled = enabled;
            btnGoTo.Enabled = enabled;
            btnClear.Enabled = enabled;
        }

        private void rebuildPlaylistMetadata_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This might take a while...are you sure you want to do this now?",
                AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            DoClickStop();
            btnClear.PerformClick();
            var rebuilder = new Rebuilder(this, StaticPlaylist);
            rebuilder.ShowDialog();
            if (rebuilder.UserCanceled)
            {
                MessageBox.Show("Rebuilding was canceled, no changes to apply", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                rebuilder.Dispose();
                return;
            }
            if (rebuilder.RebuiltPlaylist.Count == 0)
            {
                MessageBox.Show("Rebuilt playlist contains 0 items, nothing to do", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                rebuilder.Dispose();
                return;

            }
            ClearAll();
            StaticPlaylist = rebuilder.RebuiltPlaylist;
            Playlist = StaticPlaylist;
            rebuilder.Dispose();
            MessageBox.Show("Rebuilding completed successfully...reloading playlist now...", AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            ReloadPlaylist(Playlist);
            UpdateHighlights();
            MarkAsModified();
        }

        private void playBGVideos_Click(object sender, EventArgs e)
        {
            displayBackgroundVideo.Checked = playBGVideos.Checked;
            displayBackgroundVideo_Click(sender, e);
        }

        private void frmMain_Move(object sender, EventArgs e)
        {
            UpdateOverlayPosition();
        }

        private void gifTmr_Tick(object sender, EventArgs e)
        {
            if (GIFOverlay == null) return;
            var visible = MonitorApplicationFocus();
            if (visible)
            {
                GIFOverlay.Start();
            }
            else
            {
                GIFOverlay.Stop();
            }
            if (!GIFOverlay.Visible) return;
            UpdateOverlayPosition();
        }

        private void picPlay_MouseClick(object sender, MouseEventArgs e)
        {
            if (sender != null && e.Button != MouseButtons.Left) return;
            if (Bass.BASS_ChannelIsActive(BassMixer) == BASSActive.BASS_ACTIVE_PLAYING) return;
            Log("btnPlayPause_Click");
            if (Bass.BASS_ChannelIsActive(BassMixer) == BASSActive.BASS_ACTIVE_PAUSED)
            {
                Log("Resuming playback");
                Bass.BASS_ChannelPlay(BassMixer, false);
                if (MediaPlayer.playState == WMPPlayState.wmppsPaused)
                {
                    MediaPlayer.Ctlcontrols.play();
                }
                UpdatePlaybackStuff();
            }
            else
            {
                Log("Starting playback");
                PlayingSong = ActiveSong;
                StartPlayback(PlaybackSeconds == 0, true);
            }
        }

        private void picStop_MouseClick(object sender, MouseEventArgs e)
        {
            if (sender != null && e.Button != MouseButtons.Left) return;
            if (Bass.BASS_ChannelIsActive(BassMixer) == BASSActive.BASS_ACTIVE_PLAYING ||
                Bass.BASS_ChannelIsActive(BassMixer) == BASSActive.BASS_ACTIVE_PAUSED)
            {
                Log("btnStop_Click");
                Log("Stopping playback");
                DoClickStop();
            }
        }

        private void picPause_MouseClick(object sender, MouseEventArgs e)
        {
            if (sender != null && e.Button != MouseButtons.Left) return;
            if (Bass.BASS_ChannelIsActive(BassMixer) == BASSActive.BASS_ACTIVE_PLAYING)
            {
                Log("Pausing playback");
                StopPlayback(true);
                UpdateNotifyTray();
            }
            else if (Bass.BASS_ChannelIsActive(BassMixer) == BASSActive.BASS_ACTIVE_PAUSED)
            {
                picPlay_MouseClick(null, null);
            }
            else if (Bass.BASS_ChannelIsActive(BassMixer) == BASSActive.BASS_ACTIVE_STOPPED)
            {
                return;
            }
        }

        private void picLoop_MouseClick(object sender, MouseEventArgs e)
        {
            if (sender != null && e.Button != MouseButtons.Left) return;
            if (songExtractor.IsBusy || songPreparer.IsBusy) return;
            Log("picLoop_Click");
            picLoop.Tag = picLoop.Tag.ToString() == "loop" ? "noloop" : "loop";
            toolTip1.SetToolTip(picLoop, picLoop.Tag.ToString() == "loop" ? "Disable track looping" : "Enable track looping");
            picShuffle.Tag = "noshuffle";
            toolTip1.SetToolTip(picShuffle, "Enable track shuffling");
            Log("Loop tag: " + picLoop.Tag);
            Log("Shuffle tag: " + picShuffle.Tag);
            if (picLoop.Tag.ToString() == "loop")
            {
                picLoop.Image = Resources.icon_loop_enabled;
                picShuffle.Image = Resources.icon_shuffle_disabled;
            }
            else
            {
                picLoop.Image = Resources.icon_loop_disabled1;
            }
        }

        private void picShuffle_MouseClick(object sender, MouseEventArgs e)
        {
            if (sender != null && e.Button != MouseButtons.Left) return;
            if (songExtractor.IsBusy || songPreparer.IsBusy) return;
            Log("picShuffle_Click");
            picShuffle.Tag = picShuffle.Tag.ToString() == "shuffle" ? "noshuffle" : "shuffle";
            toolTip1.SetToolTip(picShuffle, picShuffle.Tag.ToString() == "shuffle" ? "Disable track shuffling" : "Enable track shuffling");
            picLoop.Tag = "noloop";
            toolTip1.SetToolTip(picLoop, "Enable track looping");
            Log("Loop tag: " + picLoop.Tag);
            Log("Shuffle tag: " + picShuffle.Tag);
            if (picShuffle.Tag.ToString() == "shuffle")
            {
                picLoop.Image = Resources.icon_loop_disabled1;
                picShuffle.Image = Resources.icon_shuffle_enabled;
            }
            else
            {
                picShuffle.Image = Resources.icon_shuffle_disabled;
            }
        }

        private void picNext_MouseClick(object sender, MouseEventArgs e)
        {
            if (sender != null && e.Button != MouseButtons.Left) return;
            Log("btnNext_Click");
            if (picLoop.Tag.ToString() == "loop")
            {
                DoLoop();
                return;
            }
            GetNextSong();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            ReplacePanelWithRoundedEdges(panelPlaying, 20);
            ReplacePanelWithRoundedEdges(panelPlaylist, 20);
            ReplacePicturesWithRoundedEdges(picPreview, 20);
            ReplacePicturesWithRoundedEdges(picVisuals, 20);
            // Reapply rounded edges on resize
            picVisuals.Resize += (s, ev) => ReplacePicturesWithRoundedEdges(picVisuals, 20);
        }

        private void ReplacePicturesWithRoundedEdges(PictureBox pictureBox, int cornerRadius)
        {
            // Ensure the GraphicsPath is calculated based on the correct bounds
            Rectangle bounds = new Rectangle(0, 0, pictureBox.Width, pictureBox.Height);

            using (GraphicsPath path = CreateRoundedRectanglePath(bounds, cornerRadius))
            {
                pictureBox.Region = new Region(path);
            }
        }

        private GraphicsPath CreateRoundedRectanglePath(Rectangle bounds, int cornerRadius)
        {
            GraphicsPath path = new GraphicsPath();

            // Calculate adjusted width and height for the path
            int adjustedWidth = bounds.Width - 1;
            int adjustedHeight = bounds.Height - 1;

            // Add arcs for rounded corners
            path.AddArc(bounds.X, bounds.Y, cornerRadius, cornerRadius, 180, 90); // Top-left
            path.AddArc(adjustedWidth - cornerRadius, bounds.Y, cornerRadius, cornerRadius, 270, 90); // Top-right
            path.AddArc(adjustedWidth - cornerRadius, adjustedHeight - cornerRadius, cornerRadius, cornerRadius, 0, 90); // Bottom-right
            path.AddArc(bounds.X, adjustedHeight - cornerRadius, cornerRadius, cornerRadius, 90, 90); // Bottom-left
            path.CloseFigure();

            return path;
        }

        private void ReplacePanelWithRoundedEdges(Panel oldPanel, int cornerRadius)
        {
            if (oldPanel == null) return;

            // Save its parent and index
            var parent = oldPanel.Parent;
            if (parent == null) return;

            int index = parent.Controls.GetChildIndex(oldPanel);

            // Create the new RoundedPanel
            var roundedPanel = new RoundedPanel
            {
                Name = oldPanel.Name,
                Size = oldPanel.Size,
                Location = oldPanel.Location,
                BackColor = oldPanel.BackColor,
                ForeColor = oldPanel.ForeColor,
                CornerRadius = cornerRadius, // Apply the specified corner radius
                Anchor = oldPanel.Anchor
            };

            // Transfer child controls
            while (oldPanel.Controls.Count > 0)
            {
                Control child = oldPanel.Controls[0];
                oldPanel.Controls.RemoveAt(0);
                roundedPanel.Controls.Add(child);
            }

            // Replace the panel
            parent.Controls.Remove(oldPanel);
            parent.Controls.Add(roundedPanel);

            // Restore the original index order
            parent.Controls.SetChildIndex(roundedPanel, index);
        }

        private void microphoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selector = new MicControl(this);
            selector.Show();
            Log("Displayed Microphone Control form");
        }

        private void stageKitToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            var isChecked = stageKitToolStripMenuItem.Checked;
            if (!isChecked)
            {
                StopStageKit();
            }
            controller1.Enabled = isChecked;
            controller2.Enabled = isChecked;
            controller3.Enabled = isChecked;
            controller4.Enabled = isChecked;
        }

        private void StopStageKit()
        {
            if (stageKit == null) return;
            try
            {
                stageKit.TurnAllOff();
            }
            catch (Exception)
            { }
        }

        private void controller1_Click(object sender, EventArgs e)
        {
            UncheckAllStageKits();
            controller1.Checked = true;
            SelectStageKitController(UserIndex.One);
        }

        private void controller2_Click(object sender, EventArgs e)
        {
            UncheckAllStageKits();
            controller2.Checked = true;
            SelectStageKitController(UserIndex.Two);
        }

        private void controller3_Click(object sender, EventArgs e)
        {
            UncheckAllStageKits();
            controller3.Checked = true;
            SelectStageKitController(UserIndex.Three);
        }

        private void controller4_Click(object sender, EventArgs e)
        {
            UncheckAllStageKits();
            controller4.Checked = true;
            SelectStageKitController(UserIndex.Four);
        }
                
        private void picVisuals_MouseClick(object sender, MouseEventArgs e)
        {
            if (stageKit == null) return;
            try
            {
                if (skDrums.Contains(e.Location))
                {
                    if (stageKit != null)
                    {
                        stageKit.TurnAllOff();//reset for the next instrument
                    }
                    skActiveInstrument = Instrument.Drums;
                    var toast = new ToastNotification("Stage Kit lighting assigned to drums chart");
                    toast.ShowToast(3000); // Display for 3 seconds
                }
                else if (skBass.Contains(e.Location))
                {
                    if (stageKit != null)
                    {
                        stageKit.TurnAllOff();//reset for the next instrument
                    }
                    skActiveInstrument = Instrument.Bass;
                    var toast = new ToastNotification("Stage Kit lighting assigned to bass chart");
                    toast.ShowToast(3000); // Display for 3 seconds
                }
                else if (skGuitar.Contains(e.Location))
                {
                    if (stageKit != null)
                    {
                        stageKit.TurnAllOff();//reset for the next instrument
                    }
                    skActiveInstrument = Instrument.Guitar;
                    var toast = new ToastNotification("Stage Kit lighting assigned to guitar chart");
                    toast.ShowToast(3000); // Display for 3 seconds
                }
                else if (skKeys.Contains(e.Location))
                {
                    if (stageKit != null)
                    {
                        stageKit.TurnAllOff();//reset for the next instrument
                    }
                    skActiveInstrument = Instrument.Keys;
                    var toast = new ToastNotification("Stage Kit lighting assigned to keys chart");
                    toast.ShowToast(3000); // Display for 3 seconds
                }
                else if (skProKeys.Contains(e.Location))
                {
                    if (stageKit != null)
                    {
                        stageKit.TurnAllOff();//reset for the next instrument
                    }
                    skActiveInstrument = Instrument.ProKeys;
                    var toast = new ToastNotification("Stage Kit lighting assigned to pro keys chart");
                    toast.ShowToast(3000); // Display for 3 seconds
                }
            }
            catch(Exception)
            {
                //
            }
        }

        private void stageKitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (stageKit != null && !stageKitToolStripMenuItem.Checked)
            {
                UncheckAllStageKits();
                stageKit = null;
            }
        }
    }

    public class ActiveWord
    {
        public string Text { get; set; }
        public double WordStart { get; set; }
        public double WordEnd { get; set; }

        public ActiveWord(string text, double wordStart, double wordEnd)
        {
            Text = text;
            WordStart = wordStart;
            WordEnd = wordEnd;
        }
    }

    public class RoundedPictureBox : PictureBox
    {
        public int CornerRadius { get; set; } = 10; // Default radius

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Enable anti-aliasing for smooth edges
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath path = new GraphicsPath())
            {
                int adjustedWidth = Width - 1; // Adjust for pen width
                int adjustedHeight = Height - 1; // Adjust for pen width

                // Define the rounded rectangle path
                path.AddArc(0, 0, CornerRadius, CornerRadius, 180, 90); // Top-left
                path.AddArc(adjustedWidth - CornerRadius, 0, CornerRadius, CornerRadius, 270, 90); // Top-right
                path.AddArc(adjustedWidth - CornerRadius, adjustedHeight - CornerRadius, CornerRadius, CornerRadius, 0, 90); // Bottom-right
                path.AddArc(0, adjustedHeight - CornerRadius, CornerRadius, CornerRadius, 90, 90); // Bottom-left
                path.CloseFigure();

                // Apply the region for rounded edges
                this.Region = new Region(path);

                // Optional: Draw a border (if desired)
                using (Pen pen = new Pen(this.BackColor, 1)) // Change BackColor to the desired border color
                {
                    e.Graphics.DrawPath(pen, path);
                }
            }
        }
    }

    public class RoundedPanel : Panel
    {
        public int CornerRadius { get; set; } = 20; // Default corner radius

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Enable anti-aliasing for smooth edges
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath path = new GraphicsPath())
            {
                int adjustedWidth = Width - 1; // Adjust for the pen width
                int adjustedHeight = Height - 1; // Adjust for the pen width

                // Define the rounded rectangle path
                path.AddArc(0, 0, CornerRadius, CornerRadius, 180, 90); // Top-left
                path.AddArc(adjustedWidth - CornerRadius, 0, CornerRadius, CornerRadius, 270, 90); // Top-right
                path.AddArc(adjustedWidth - CornerRadius, adjustedHeight - CornerRadius, CornerRadius, CornerRadius, 0, 90); // Bottom-right
                path.AddArc(0, adjustedHeight - CornerRadius, CornerRadius, CornerRadius, 90, 90); // Bottom-left
                path.CloseFigure();

                // Draw the rounded panel background
                using (Brush brush = new SolidBrush(this.BackColor))
                {
                    e.Graphics.FillPath(brush, path);
                }

                // Draw the border if needed
                using (Pen pen = new Pen(this.ForeColor, 1))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            }
        }

    }

    public enum Instrument
    {
        Drums, Bass, Guitar, Keys, ProKeys, Vocals
    }

    public enum LEDColor
    {
        Red, Green, Yellow, Blue, White, Orange
    }

    public class LED
    {
        public int Index { get; set; }
        
        public bool Enabled { get; set; }

        public DateTime Time { get; set; }

        public LEDColor Color { get; set; }
    }

    public class Song
    {
        public string Name { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public long Length { get; set; }
        public string Location { get; set; }
        public string InternalName { get; set; }
        public string Genre { get; set; }
        public int Year { get; set; }
        public int Track { get; set; }
        public int Index { get; set; }
        public string PanningValues { get; set; }
        public string AttenuationValues { get; set; }
        public string Charter { get; set; }
        public int ChannelsDrums { get; set; }
        public int ChannelsBass { get; set; }
        public int ChannelsGuitar { get; set; }
        public int ChannelsKeys { get; set; }
        public int ChannelsVocals { get; set; }
        public int ChannelsCrowd { get; set; }
        public int ChannelsBacking { get; set; }
        public bool AddToPlaylist { get; set; }
        public int DTAIndex { get; set; }
        public double BPM { get; set; }
        public bool isRhythmOnBass { get; set; }
        public bool isRhythmOnKeys { get; set; }
        public bool hasProKeys { get; set; }
        public int PSDelay { get; set; }
        public int ChannelsBassStart { get; set; }
        public int ChannelsDrumsStart { get; set; }
        public int ChannelsGuitarStart { get; set; }
        public int ChannelsKeysStart { get; set; }
        public int ChannelsVocalsStart { get; set; }
        public int ChannelsCrowdStart { get; set; }
        public int ChannelsTotal { get; set; }
        public string yargPath { get; set; }
    }
}

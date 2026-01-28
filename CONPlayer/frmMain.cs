using cPlayer.Properties;
using cPlayer.StageKit;
using cPlayer.Texture;
using cPlayer.x360;
using LibForge.Midi;
using LibForge.SongData;
using LibVLCSharp.Shared;
using LibVLCSharp.WinForms;
using Microsoft.VisualBasic;
using MidiCS;
using NAudio.Dsp;
using NAudio.Wave;
using NautilusFREE;
using System;
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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Enc;
using Un4seen.Bass.AddOn.Mix;
using Un4seen.Bass.AddOn.Opus;
using Un4seen.Bass.Misc;
using static cPlayer.NemoTools;
using static cPlayer.YARGSongFileStream;

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
        public bool doMIDINoKeys = false;
        public bool doMIDIVocals = false;
        public bool doMIDIHarmonies = true;
        public bool doMIDINoVocals = false;
        public bool doMIDINameVocals = false;
        public bool doMIDINameProKeys = false;
        public bool doStaticLyrics = false;
        public bool doScrollingLyrics = true;
        public bool doKaraokeLyrics = false;
        public bool doWholeWordsLyrics = true;
        public bool doHarmonyLyrics = true;
        public bool doMIDINameTracks = true;
        public bool doMIDIHighlightSolos = true;
        public bool doMIDIBWKeys = true;
        public bool doMIDIHarm1onVocals = true;
        private readonly Visuals Spectrum = new Visuals();
        public double PlaybackWindow = 3.0;
        private readonly double PlaybackWindowRB = 2.0;
        private readonly double PlaybackWindowRBVocals = 5.0;
        public int NoteSizingType = 0;
        private const string AppName = "cPlayer";
        private const int BassBuffer = 1000;
        private readonly NemoTools Tools;
        private readonly DTAParser Parser;
        private int mouseX;
        private int mouseY;
        private string SongToLoad;
        private List<Song> StaticPlaylist;
        public List<Song> Playlist;
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
        public int SpectrumID;
        public Color SpectrumColor = Color.Black;
        private bool VideoIsPlaying;
        public List<PracticeSection> PracticeSessions;
        private string ImgToUpload;
        private string ImgURL;
        private bool showUpdateMessage;
        private bool AlreadyTried;
        private bool DrewFullChart;
        private double IntroSilence;
        private double OutroSilence;
        private float SilenceThreshold = 0.25f;
        private bool AlreadyFading;
        private PlaylistSorting SortingStyle;
        private bool isClosing;
        private bool ShowingNotFoundMessage;
        private readonly nTools nautilus;
        private SongData ActiveSongData;
        private LibVLC _libVLC;
        private MediaPlayer _mediaPlayer;
        private VideoView videoView;
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
        private OverlayForm videoOverlay;
        private gifOverlay GIFOverlay;
        private const string strSearchPlaylist = "Search...";
        private const int KICK_HEIGHT = 6;
        private int ChartGoal = 630;
        private const int vocalsHeight = 160;
        private const double MinVolume = 50;
        private readonly Bitmap bmpDrumsCymbalB;
        private readonly Bitmap bmpDrumsCymbalY;
        private readonly Bitmap bmpDrumsCymbalG;
        private readonly Bitmap bmpDrumsCymbalOD;
        private readonly Bitmap bmpDrumsCymbalBGlow;
        private readonly Bitmap bmpDrumsCymbalYGlow;
        private readonly Bitmap bmpDrumsCymbalGGlow;
        private readonly Bitmap bmpDrumsCymbalODGlow;
        private readonly Bitmap bmpNoteBlue;
        private readonly Bitmap bmpNoteGreen;
        private readonly Bitmap bmpNoteYellow;
        private readonly Bitmap bmpNoteRed;
        private readonly Bitmap bmpNoteOrange;
        private readonly Bitmap bmpNoteOD;
        private readonly Bitmap bmpNoteBlueGlow;
        private readonly Bitmap bmpNoteGreenGlow;
        private readonly Bitmap bmpNoteYellowGlow;
        private readonly Bitmap bmpNoteRedGlow;
        private readonly Bitmap bmpNoteOrangeGlow;
        private readonly Bitmap bmpNoteODGlow;
        private readonly Bitmap bmpProKeysNoteWhite;
        private readonly Bitmap bmpProKeysNoteWhiteOD;
        private readonly Bitmap bmpProKeysNoteBlack;
        private readonly Bitmap bmpProKeysNoteBlackOD;
        private readonly Bitmap bmpProKeysNoteWhiteGlow;
        private readonly Bitmap bmpProKeysNoteWhiteODGlow;
        private readonly Bitmap bmpProKeysNoteBlackGlow;
        private readonly Bitmap bmpProKeysNoteBlackODGlow;
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
        private readonly Bitmap bmpBackgroundDrumsRB;
        private readonly Bitmap bmpBackgroundDrumsSoloRB;
        private readonly Bitmap bmpBackgroundBassRB;
        private readonly Bitmap bmpBackgroundBassSoloRB;
        private readonly Bitmap bmpBackgroundGuitarRB;
        private readonly Bitmap bmpBackgroundGuitarSoloRB;
        private readonly Bitmap bmpBackgroundKeysRB;
        private readonly Bitmap bmpBackgroundKeysSoloRB;
        private readonly Bitmap bmpBackgroundProKeysRB;
        private readonly Bitmap bmpBackgroundProKeysSoloRB;
        private readonly Bitmap bmpHitbox;
        private readonly Bitmap bmpHitboxVocals;
        private readonly Bitmap bmpBackgroundVocals;
        private readonly Bitmap bmpBackgroundLyrics;
        private readonly Bitmap bmpProKeysChordMarker;
        private readonly Bitmap bmpBlueHopo;
        private readonly Bitmap bmpGreenHopo;
        private readonly Bitmap bmpYellowHopo;
        private readonly Bitmap bmpRedHopo;
        private readonly Bitmap bmpOrangeHopo;
        private readonly Bitmap bmpODHopo;
        private readonly Bitmap bmpBlueHopoGlow;
        private readonly Bitmap bmpGreenHopoGlow;
        private readonly Bitmap bmpYellowHopoGlow;
        private readonly Bitmap bmpRedHopoGlow;
        private readonly Bitmap bmpOrangeHopoGlow;
        private readonly Bitmap bmpODHopoGlow;
        private const int HitboxVocalsX = 200;
        private WaveInEvent waveIn;
        private WaveOutEvent waveOut;
        private BufferedWaveProvider bufferedWaveProvider;
        public VolumeWaveProvider16 volumeProvider;
        public int microphoneIndex = -1;
        private StageKitController stageKit;
        private LedDisplay ledDisplay;
        private int currHOPOThreshold = 170;
        public Color KaraokeModeBackgroundColor = Color.Orange;
        public Color KaraokeModeHarm1Text = Color.White;
        public Color KaraokeModeHarm1Highlight = Color.DeepSkyBlue;
        public Color KaraokeModeHarm2Text = Color.LightGray;
        public Color KaraokeModeHarm2Highlight = Color.LightPink;
        public Color KaraokeModeHarm3Text = Color.DarkGray;
        public Color KaraokeModeHarm3Highlight = Color.DarkSeaGreen;
        private Size picVisualsSize;
        private Point picVisualsPosition;
        private bool isFullScreen;
        private FormWindowState lastWindowState = FormWindowState.Maximized;
        private Point savedFormLocation;
        private Size savedFormSize;
        private readonly List<Image> stageFrames;
        private Image stageBackground;
        private int stageKitIndex = 1;
        private int redSKIndex = 0;
        private int greenSKIndex = 0;
        private int blueSKIndex = 0;
        private int yellowSKIndex = 0;
        private readonly bool[] CurrentStateYellow;
        private readonly bool[] CurrentStateRed;
        private readonly bool[] CurrentStateGreen;
        private readonly bool[] CurrentStateBlue;
        private bool enableFavorites;
        private bool enable2020s;
        private bool enable2010s;
        private bool enable2000s;
        private bool enable1990s;
        private bool enable1980s;
        private bool enable1970s;
        private bool enable1960s;
        private bool enableOldies;
        private List<FavoriteSong> favoritesList;
        public string genreFilter;
        public string instrumentFilter;
        public string languageFilter;
        private string CHVideoPath;

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);
        private List<string> BackgroundImages;
        private List<string> BackgroundVideos;
        public bool changedBackground;
        private Bitmap _renderedFrame;
        private bool doUseBackgroundVideos = true;
        private bool doUseBackgroundImages = false;
        private readonly Random rng = new Random();
        private readonly ShuffleBag _videoBag;
        private readonly ShuffleBag _imageBag;
        private readonly ToolStripLabel statusLabel;
        public int BTAVOffsetSync;
        public bool enableBTAVOffsetSync;
        private string nautilusPath;
        private readonly frmHover hoverForm;
        public frmMain()
        {
            InitializeComponent();
            Core.Initialize();
            hoverForm = new frmHover(this);
            statusLabel = new ToolStripLabel
            {
                Name = "tsStatus",
                Alignment = ToolStripItemAlignment.Right,
                TextAlign = ContentAlignment.MiddleRight,
                AutoSize = true,
                Margin = new Padding(0, 0, 8, 0) // right padding
            };
            statusLabel.IsLink = false;
            menuStrip1.Items.Add(statusLabel);
            
            _videoBag = new ShuffleBag(rng);
            _imageBag = new ShuffleBag(rng);
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
            favoritesList = new List<FavoriteSong> { };
            BackgroundImages = new List<string>();
            opusFiles = new string[20];
            oggFiles = new string[20];
            mp3Files = new string[20];
            m4aFiles = new string[20];
            CurrentStateBlue = new bool[8];
            CurrentStateGreen = new bool[8];
            CurrentStateRed = new bool[8];
            CurrentStateYellow = new bool[8];
            RecentPlaylists = new string[5];
            PracticeSessions = new List<PracticeSection>();
            
            var options = new[]
            {
            "--vout=d3d11", // Ensure Direct3D 11 is used if available
            "--no-audio", // Disable audio processing
            "--no-sub-autodetect-file", // Skip subtitle loading
            "--no-video-title-show" // Hide overlay text on videos
        };

            _libVLC = new LibVLC(options);
            _mediaPlayer = new MediaPlayer(_libVLC);
            _mediaPlayer.Volume = 0; //always muted        

            videoView = new VideoView
            {
                Width = 256,
                Height = 256,
                MediaPlayer = _mediaPlayer,
                Dock = DockStyle.Fill
            };
            this.Controls.Add(videoView);
            CreateOverlay();
            UpdateOverlayPosition();                                           

            stageFrames = new List<Image>();
            var stagePath = Application.StartupPath + "\\res\\stage\\";
            if (Directory.Exists(stagePath))
            {
                for (var i = 0; i < 200; i++)
                {
                    var framePath = stagePath + "frame_" + i.ToString("D4") + ".jpg";
                    if (File.Exists(framePath))
                    {
                        var image = Image.FromFile(framePath);
                        stageFrames.Add(image);
                    }
                }
            }

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
            bmpDrumsCymbalBGlow = Resources.drums_cymbal_b_glow;
            bmpDrumsCymbalYGlow = Resources.drums_cymbal_y_glow;
            bmpDrumsCymbalGGlow = Resources.drums_cymbal_g_glow;
            bmpDrumsCymbalODGlow = Resources.drums_cymbal_od_glow;
            bmpGreenHopo = Resources.note_green_hopo;
            bmpRedHopo = Resources.note_red_hopo;
            bmpYellowHopo = Resources.note_yellow_hopo;
            bmpBlueHopo = Resources.note_blue_hopo;
            bmpOrangeHopo = Resources.note_orange_hopo;
            bmpODHopo = Resources.note_overdrive_hopo;
            bmpGreenHopoGlow = Resources.note_green_hopo_glow;
            bmpRedHopoGlow = Resources.note_red_hopo_glow;
            bmpYellowHopoGlow = Resources.note_yellow_hopo_glow;
            bmpBlueHopoGlow = Resources.note_blue_hopo_glow;
            bmpOrangeHopoGlow = Resources.note_orange_hopo_glow;
            bmpODHopoGlow = Resources.note_overdrive_hopo_glow;
            bmpNoteBlue = Resources.note_blue;
            bmpNoteGreen = Resources.note_green;
            bmpNoteYellow = Resources.note_yellow;
            bmpNoteRed = Resources.note_red;
            bmpNoteOrange = Resources.note_orange;
            bmpNoteOD = Resources.note_od;
            bmpNoteBlueGlow = Resources.note_blue_glow;
            bmpNoteGreenGlow = Resources.note_green_glow;
            bmpNoteYellowGlow = Resources.note_yellow_glow;
            bmpNoteRedGlow = Resources.note_red_glow;
            bmpNoteOrangeGlow = Resources.note_orange_glow;
            bmpNoteODGlow = Resources.note_overdrive_glow;
            bmpProKeysNoteWhite = Resources.note_white;
            bmpProKeysNoteWhiteOD = Resources.note_white_od;
            bmpProKeysNoteBlack = Resources.note_black;
            bmpProKeysNoteBlackOD = Resources.note_black_od;
            bmpProKeysNoteWhiteGlow = Resources.note_white_glow;
            bmpProKeysNoteWhiteODGlow = Resources.note_white_od_glow;
            bmpProKeysNoteBlackGlow = Resources.note_black_glow;
            bmpProKeysNoteBlackODGlow = Resources.note_black_od_glow;
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
            bmpBackgroundDrumsRB = Resources.background_drumsRB;
            bmpBackgroundDrumsSoloRB = Resources.background_drums_soloRB;
            bmpBackgroundBassRB = Resources.background_bassRB;
            bmpBackgroundBassSoloRB = Resources.background_bass_soloRB;
            bmpBackgroundGuitarRB = Resources.background_guitarRB;
            bmpBackgroundGuitarSoloRB = Resources.background_guitar_soloRB;
            bmpBackgroundKeysRB = Resources.background_keysRB;
            bmpBackgroundKeysSoloRB = Resources.background_keys_soloRB;
            bmpBackgroundProKeysRB = Resources.background_prokeysRB;
            bmpBackgroundProKeysSoloRB = Resources.background_prokeys_soloRB;
            bmpHitbox = Resources.hitbox;
            bmpHitboxVocals = Resources.hitbox_vocals;
            bmpBackgroundVocals = Resources.frostedglass75dark;//frostedglass50;
            bmpBackgroundLyrics = Resources.frostedglass75dark;// frostedglass50;// frostedglasslyrics50black;
            bmpProKeysChordMarker = Resources.prokeyschord;
            ledDisplay = new LedDisplay();

            var path = Application.StartupPath + "\\res\\backgrounds\\";
            if (!Directory.Exists(path)) return;
            BackgroundImages = Directory.GetFiles(path, "*.jpg", SearchOption.TopDirectoryOnly).ToList();
            BackgroundImages.AddRange(Directory.GetFiles(path, "*.png", SearchOption.TopDirectoryOnly).ToList());
            BackgroundVideos = Directory.GetFiles(path, "*.mp4", SearchOption.TopDirectoryOnly).ToList();
        }

        private void CreateOverlay()
        {
            if (videoOverlay != null) return;
            videoOverlay = new OverlayForm();
            videoOverlay.Show(picVisuals);
            videoOverlay.HostMenu = picVisuals.ContextMenuStrip;
            videoOverlay.OnOverlayRightClick = () =>
            {
                ShowMenuAtCursor(picVisuals.ContextMenuStrip, this);
            };
            UpdateOverlayPosition();
        }

        private void ShowMenuAtCursor(ContextMenuStrip menu, Control owner)
        {
            if (menu == null || owner == null) return;

            if (owner.InvokeRequired)
            {
                owner.BeginInvoke(new Action(() => ShowMenuAtCursor(menu, owner)));
                return;
            }

            // Cursor in screen coords → convert to owner's client coords
            Point screen = Cursor.Position;
            Point client = owner.PointToClient(screen);

            // Show using owner so it appears correctly in z-order
            menu.Show(owner, client);
        }

        private int lastBackground = 0;
        private void randomizeBackgroundImage()
        {
            //random was annoying so just linear change from one to the next
            //var rnd = new Random().Next(1, 6);           
            //var backgroundPath = Application.StartupPath + "\\res\\stage" + rnd + ".jpg";
            lastBackground++;
            if (lastBackground > 5)
            {
                lastBackground = 1;
            }
            var backgroundPath = Application.StartupPath + "\\res\\stage" + lastBackground + ".jpg";
            if (File.Exists(backgroundPath))
            {
                stageBackground = Image.FromFile(backgroundPath);
            }
            else
            {
                randomizeBackgroundImage();
            }
        }

        private void foggerTimer_Tick(object sender, EventArgs e)
        {
            foggerTimer.Enabled = false;
            stageKit.TurnFogOff();
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

        public bool MonitorApplicationFocus()
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
            // 1) video overlay exactly over picVisuals (screen coords)
            if (videoOverlay != null && !videoOverlay.IsDisposed)
            {
                var r = picVisuals.RectangleToScreen(picVisuals.ClientRectangle);
                videoOverlay.Location = r.Location;
                videoOverlay.Size = r.Size;
            }

            Rectangle pr = new Rectangle();
            try
            {
                pr = this.RectangleToScreen(this.ClientRectangle);
            }
            catch
            {
                if (pr.Width == 0 || pr.Height == 0) return;
            }

            // 2) GIF overlay centered over form (screen coords)
            if (GIFOverlay != null && !GIFOverlay.IsDisposed)
            {                
                int x = pr.Left + (pr.Width - GIFOverlay.Width) / 2;
                int y = pr.Top + (pr.Height - GIFOverlay.Height) / 2;
                GIFOverlay.Location = new Point(x, y);
            }

            // 3) hover form anchored to bottom-right of *this window* (client area) in screen coords
            if (hoverForm != null && !hoverForm.IsDisposed)
            {
                int x = pr.Left + pr.Width - 75;
                int y = pr.Top + pr.Height - 75;
                hoverForm.Location = new Point(x, y);
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
            try
            {
                Spectrum.ClearPeaks();
            }
            catch { }            
        }

        private Point lastCursorPos = Point.Empty;       

        private void frmMain_MouseUp(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void createNewPlaylist_Click(object sender, EventArgs e)
        {
            StartNew(true);
        }

        private void StartNew(bool confirm)
        {
            if (Text.Contains("*") && confirm)
            {                
                if (MessageBox.Show("You have unsaved changes on the current playlist\nAre you sure you want to start a new playlist?",
                        AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }
            }

            if (!yarg.Checked && !fortNite.Checked && !guitarHero.Checked)
            {
                Tools.DeleteFile(CurrentSongArt);
                Tools.DeleteFile(CurrentSongArtBlurred);
                Tools.DeleteFile(CurrentSongMIDI);
                Tools.DeleteFile(NextSongArtPNG);
                Tools.DeleteFile(NextSongArtJPG);
                Tools.DeleteFile(NextSongArtBlurred);
                Tools.DeleteFile(NextSongMIDI);
            }
            DoClickStop();
            PlaylistPath = "";
            PlaylistName = "";
            Playlist = new List<Song>();
            statusLabel.Text = "";
            lstPlaylist.Items.Clear();
            lblClearSearch_MouseClick(null, null);
            ClearAll();
            ClearVisuals();
            ActiveSong = null;
            PlayingSong = null;
            Text = AppName;
            DeleteUsedFiles();
            Tools.DeleteFile(activeM4AFile);
            activeM4AFile = "";
        }

        private void ClearAll()
        {
            reset = true;
            StopPlayback();
            videoView.Visible = false;
            picPreview.Image = Resources.default_art;
            picPreview.Cursor = Cursors.Default;
            lblSections.Invoke(new MethodInvoker(() => lblSections.Text = ""));
            lblSections.Invoke(new MethodInvoker(() => lblSections.Image = null));
            lblSections.Invoke(new MethodInvoker(() => lblSections.CreateGraphics().Clear(LabelBackgroundColor)));
            picVisuals.Invoke(new MethodInvoker(() => picVisuals.Image = Resources.logo)); 
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
            PlayingSong = null;
            MIDITools.Initialize(true);
            AlreadyFading = false;            
            reset = false;
        }

        private void lstPlaylist_DragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            Environment.CurrentDirectory = Path.GetDirectoryName(files[0]);
            if (files[0].EndsWith(".playlist", StringComparison.Ordinal))
            {
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
            else if (rb4PS4.Checked)
            {
                SongsToAdd.AddRange(files.Where(file => file.EndsWith("_ps4")).ToList());
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
                MessageBox.Show(files.Count() == 1 ? "That's not a valid file" : "Those are not valid files", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (batchSongLoader.IsBusy || songLoader.IsBusy)
            {
                MessageBox.Show("Please wait while I finish processing the last file(s)", AppName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            lblClearSearch_MouseClick(null, null);
            EnableDisable(false);
            StartingCount = lstPlaylist.Items.Count;
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
            nautilusToolStripMenuItem.Enabled = fileToolStripMenuItem.Enabled;
            changeViewToolStrip.Enabled = fileToolStripMenuItem.Enabled;
            txtSearch.Enabled = enabled && !isScanning;
        }

        private void InitiateGIFOverlay()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(InitiateGIFOverlay));
                return;
            }

            if (WindowState == FormWindowState.Minimized) return;                  
            
            GIFOverlay = new gifOverlay(this)
            {
                StartPosition = FormStartPosition.Manual,
                Width = 256,
                Height = 256,
                ShowInTaskbar = false
            };

            GIFOverlay.Show();
            GIFOverlay.Start();
            UpdateOverlayPosition();
        }

        private bool ValidateDTAFile(string file, bool message)
        {
            if (string.IsNullOrEmpty(file) || !File.Exists(file)) return false;
            CreateHiddenFolder();
            if (xbox360.Checked)
            {
                if (!Parser.ExtractDTA(file))
                {
                    if (message)
                    {
                        MessageBox.Show("Something went wrong extracting the songs.dta file, can't add to the playlist", AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return false;
                }
            }
            if (!Parser.ReadDTA(xbox360.Checked ? Parser.DTA : File.ReadAllBytes(file)) || !Parser.Songs.Any())
            {
                if (message)
                {
                    MessageBox.Show("Something went wrong reading that songs.dta file, can't add to the playlist", AppName,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return false;
            }
            if (Parser.Songs.Count == 1) return true;
            isScanning = true;
            UpdateNotifyTray();
            return true;
        }

        private bool ValidateNewSong(SongData song, int index, string location, bool scanning, bool message, out Song newsong)
        {
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
                yargPath = song.YargPath,
                Languages = song.Languages
            };

            ActiveSongData = song;
            newsong = new_song;
            if (!scanning) return true;
            var exists = Playlist.Any(oldsong => String.Equals(oldsong.Artist, new_song.Artist, StringComparison.InvariantCultureIgnoreCase) &&
                                                 String.Equals(oldsong.Name, new_song.Name, StringComparison.InvariantCultureIgnoreCase));
            if (!exists) return true;
            if (message)
            {
                MessageBox.Show("Song '" + new_song.Artist + " - " + new_song.Name + "' is already in your playlist", AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }            
            return false;
        }

        private void loadPS4Files(string ps4File, bool message = false, bool scanning = true, bool next = false, bool prep = false)
        {
            var Parser = new DTAParser();
            var song = new SongData();
            Parser.Songs = new List<SongData>();
            song.Initialize();

            hasNoMIDI = false;

            //this should work with any _ps4 file as input rather than forcing songdta_ps4 to be the input file
            var path = Path.GetDirectoryName(ps4File) + "\\" + Path.GetFileNameWithoutExtension(ps4File);
            var DTA_PS4 = path + ".songdta_ps4";
            string PNG_PS4 = path + ".png_ps4";
            string PNG = path + ".png";
            var MIDI_PS4 = path + ".rbmid_ps4";
            var MIDI = path + ".mid";
            string MOGG = path + ".mogg";
            string MOGG_DTA = path + ".mogg.dta";

            var dtaBytes = File.ReadAllBytes(DTA_PS4);
            using (MemoryStream ms = new MemoryStream(dtaBytes))
            {
                var ps4Data = new SongDataReader(ms);
                var songData = ps4Data.Read();                      
                song.SongId = (int)songData.SongId;
                song.GameVersion = songData.Version;
                song.PreviewStart = (int)songData.PreviewStart;
                song.PreviewEnd = (int)songData.PreviewEnd;
                song.Name = songData.Name;
                song.Artist = songData.Artist;
                song.Album = songData.AlbumName;
                song.YearReleased = songData.AlbumYear;
                song.TrackNumber = songData.AlbumTrackNumber;
                song.Genre = Parser.doGenre(songData.Genre, false);
                song.RawGenre = songData.Genre;
                song.Length = (int)songData.SongLength;
                song.GuitarDiff = Parser.GuitarDiff((int)songData.GuitarRank);
                song.BassDiff = Parser.BassDiff((int)songData.BassRank);
                song.DrumsDiff = Parser.DrumDiff((int)songData.DrumRank);
                song.VocalsDiff = Parser.VocalsDiff((int)songData.VocalsRank);
                song.BandDiff = Parser.BandDiff((int)songData.BandRank);
                song.KeysDiff = Parser.KeysDiff((int)songData.KeysRank);
                song.ProKeysDiff = Parser.ProKeysDiff((int)songData.RealKeysRank);
                song.Master = !songData.Cover;
                song.VocalParts = songData.VocalParts;
                song.ShortName = songData.Shortname;
                song.Source = songData.GameOrigin;
                Parser.Songs.Add(song);
            }            

            //convert album art
            Image img = null;
            using (var fileStream = new FileStream(PNG_PS4, FileMode.Open, FileAccess.Read))
            {
                var converter = new TextureConverter();
                img = converter.ToBitmap(TextureReader.ReadStream(fileStream), 0);
            }
            img.Save(PNG, ImageFormat.Png);
            img.Dispose();

            //convert and decrypt audio file
            var sr = new StreamReader(MOGG_DTA);
            try
            {
                while (sr.Peek() >= 0)
                {
                    var line = sr.ReadLine();
                    if (line.Contains("(tracks"))
                    {
                        var o = 0;
                        var c = 0;
                        while (!string.IsNullOrEmpty(line.Trim()))
                        {
                            line = sr.ReadLine();
                            o += line.Count(a => a == '(');
                            c += line.Count(a => a == ')');
                            if (o == c) break;

                            if (line.ToLowerInvariant().Contains("drum"))
                            {
                                if (!line.Contains(")"))
                                {
                                    line = sr.ReadLine();
                                    if (string.IsNullOrEmpty(line.Trim()))
                                    {
                                        line = sr.ReadLine();
                                    }
                                }
                                song.ChannelsDrums += Parser.getChannels(line, "drum");
                                song.ChannelsDrumsStart = 0;
                                o += line.Count(a => a == '(');
                                c += line.Count(a => a == ')');
                            }
                            else if (line.ToLowerInvariant().Contains("bass"))
                            {
                                if (!line.Contains(")"))
                                {
                                    line = sr.ReadLine();
                                    if (string.IsNullOrEmpty(line.Trim()))
                                    {
                                        line = sr.ReadLine();
                                    }
                                }
                                song.ChannelsBass = Parser.getChannels(line, "bass");
                                song.ChannelsBassStart = Parser.getChannelsStart(line, "bass");
                                o += line.Count(a => a == '(');
                                c += line.Count(a => a == ')');
                            }
                            else if (line.ToLowerInvariant().Contains("guitar"))
                            {
                                if (!line.Contains(")"))
                                {
                                    line = sr.ReadLine();
                                    if (string.IsNullOrEmpty(line.Trim()))
                                    {
                                        line = sr.ReadLine();
                                    }
                                }
                                song.ChannelsGuitar = Parser.getChannels(line, "guitar");
                                song.ChannelsGuitarStart = Parser.getChannelsStart(line, "guitar");
                                o += line.Count(a => a == '(');
                                c += line.Count(a => a == ')');
                            }
                            else if (line.ToLowerInvariant().Contains("vocals"))
                            {
                                if (!line.Contains(")"))
                                {
                                    line = sr.ReadLine();
                                    if (string.IsNullOrEmpty(line.Trim()))
                                    {
                                        line = sr.ReadLine();
                                    }
                                }
                                song.ChannelsVocals = Parser.getChannels(line, "vocals");
                                song.ChannelsVocalsStart = Parser.getChannelsStart(line, "vocals");
                                o += line.Count(a => a == '(');
                                c += line.Count(a => a == ')');
                            }
                            else if (line.ToLowerInvariant().Contains("fake"))
                            {
                                if (!line.Contains(")"))
                                {
                                    line = sr.ReadLine();
                                    if (string.IsNullOrEmpty(line.Trim()))
                                    {
                                        line = sr.ReadLine();
                                    }
                                }
                                o += line.Count(a => a == '(');
                                c += line.Count(a => a == ')');
                            }
                        }
                    }
                    else if (line.Contains("pans"))
                    {
                        if (!line.Contains(")"))
                        {
                            line = sr.ReadLine();
                        }
                        song.PanningValues = line.Replace("(", "").Replace(")", "").Replace("'", "").Replace("pans", "");
                    }
                    else if (line.Contains("vols"))
                    {
                        if (!line.Contains(")"))
                        {
                            line = sr.ReadLine();
                        }
                        song.AttenuationValues = line.Replace("(", "").Replace(")", "").Replace("'", "").Replace("vols", "");
                        song.OriginalAttenuationValues = song.AttenuationValues;
                    }
                }
            }
            catch (Exception ex)
            {
                if (message)
                {
                    MessageBox.Show("Error processing that .mogg.dta file:\n\n" + ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                sr.Dispose();
                return;
            }
            sr.Dispose();

            unsafe
            {
                var bytes = File.ReadAllBytes(MOGG);
                fixed (byte* ptr = bytes)
                {
                    if (!TheMethod3.decrypt_mogg(ptr, (uint)bytes.Length)) return;
                    nautilus.ReleaseStreamHandle(true);                    
                    if (!nautilus.RemoveMHeader(bytes, true, DecryptMode.ToMemory, "")) return;
                }
            }

            Song newSong;
            if (!ValidateNewSong(song, 0, ps4File, scanning, message, out newSong))
            {
                return;
            }

            if (!scanning)
            {
                try
                {
                    //convert MIDI file
                    if (File.Exists(MIDI_PS4))
                    {
                        Tools.DeleteFile(CurrentSongMIDI);
                        Tools.DeleteFile(MIDI);
                        NextSongMIDI = "";
                        hasNoMIDI = true;
                        MIDITools.Initialize(true);

                        using (var inStream = File.OpenRead(MIDI_PS4))
                        {
                            var rb4mid = RBMidReader.ReadStream(inStream);
                            var rb3mid = RBMidConverter.ToMid(rb4mid);

                            using (var outStream = File.Create(MIDI))
                            {
                                MidiFileWriter.WriteSMF(rb3mid, outStream);
                            }
                        }
                        if (CancelWorkers) return;

                        newSong.BPM = 120;//default in case something fails below
                        currHOPOThreshold = song.HOPOThreshold;
                        NextSongMIDI = MIDI;
                        if (File.Exists(NextSongMIDI))
                        {
                            hasNoMIDI = false;
                            MIDITools.Initialize(false);
                            if (MIDITools.ReadMIDIFile(NextSongMIDI, song.HOPOThreshold, false))
                            {
                                newSong.BPM = MIDITools.MIDIInfo.AverageBPM;
                            }
                        }
                        else
                        {
                            hasNoMIDI = true;
                            MIDITools.Initialize(true);
                            Tools.DeleteFile(CurrentSongMIDI);
                        }
                    }
                }
                catch
                {
                    Tools.DeleteFile(MIDI);
                    hasNoMIDI = true;
                    MIDITools.Initialize(true);
                    Tools.DeleteFile(CurrentSongMIDI);
                }
            }
            
            Tools.DeleteFile(NextSongArtPNG);
            Tools.DeleteFile(NextSongArtBlurred);
            if (File.Exists(PNG))
            {
                NextSongArtPNG = PNG;
                NextSongArtBlurred = NextSongArtPNG.Replace(".png", "_b.png");
                Tools.CreateBlurredArt(NextSongArtPNG, NextSongArtBlurred);
            }            

            long length;
            ProcessMogg(scanning, song.Length, "", out length);
            newSong.Length = length;

            if (!scanning)
            {
                return;
            }

            Playlist.Add(newSong);
            if (isScanning)
            {
                ShowUpdate("Added '" + newSong.Artist + " - " + newSong.Name + "'");
            }
        }

        private void loadDTA(string dta, bool message = false, bool scanning = true, bool next = false, bool prep = false)
        {            
            if (!ValidateDTAFile(dta, message)) return;
            
            hasNoMIDI = false;
            for (var i = 0; i < Parser.Songs.Count; i++)
            {
                if (CancelWorkers) return;
                var song = prep ? Parser.Songs[ActiveSong.DTAIndex] : (next ? Parser.Songs[NextSong.DTAIndex] : Parser.Songs[i]);
                
                string internalName = "";
                string PNG = "";
                var EDAT = "";
                string audioPath = "";
                string yarg = "";

                if (wii.Checked)
                {
                    var index = song.FilePath.LastIndexOf("/", StringComparison.Ordinal) + 1;
                    song.InternalName = song.FilePath.Substring(index, song.FilePath.Length - index);
                    internalName = song.InternalName;
                    PNG = Path.GetDirectoryName(dta) + "\\" + internalName + "\\gen\\" + internalName + "_keep.png_wii";
                    audioPath = Path.GetDirectoryName(dta).Replace("_meta", "_song") + "\\" + internalName + "\\" + internalName + ".mogg";
                    NextSongMIDI = Path.GetDirectoryName(audioPath) + "\\" + internalName + ".mid";
                }
                else if (pS3.Checked)
                {
                    internalName = song.InternalName;
                    PNG = Path.GetDirectoryName(dta) + "\\" + internalName + "\\gen\\" + internalName + "_keep.png_ps3";
                    audioPath = Path.GetDirectoryName(dta) + "\\" + internalName + "\\" + internalName + ".mogg";
                    EDAT = Path.GetDirectoryName(audioPath) + "\\" + internalName + ".mid.edat";
                    NextSongMIDI = EDAT.Replace(".mid.edat", ".mid");
                }
                else //is YARG
                {
                    internalName = song.InternalName;
                    PNG = Path.GetDirectoryName(dta) + "\\" + internalName + "\\gen\\" + internalName + "_keep.png_xbox";
                    audioPath = Path.GetDirectoryName(dta) + "\\" + internalName + "\\" + internalName + ".mogg";
                    yarg = audioPath.Replace(".mogg", ".yarg_mogg");
                    NextSongMIDI = Path.GetDirectoryName(audioPath) + "\\" + internalName + ".mid";
                    CurrentSongAudioPath = yarg;
                }

                if (!File.Exists(audioPath) && !File.Exists(yarg))
                {
                    if (message)
                    {
                        MessageBox.Show("Couldn't locate audio file(s) for song '" + song.Artist + " - " + song.Name + "', can't add to the playlist",
                            AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
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
                        if (next || prep) return;
                        continue;
                    }
                }

                Song newSong;
                if (!ValidateNewSong(song, i, string.IsNullOrEmpty(pkgPath) ? dta : pkgPath, scanning, message, out newSong)) continue;

                if (CancelWorkers) return;
                try
                {
                    newSong.BPM = 120;//default in case something fails below
                    currHOPOThreshold = song.HOPOThreshold;
                    if (File.Exists(EDAT) && pS3.Checked)
                    {
                        DecryptPS3EDAT(EDAT, message);
                    }
                    if (File.Exists(NextSongMIDI))
                    {
                        hasNoMIDI = false;
                        MIDITools.Initialize(false);
                        if (MIDITools.ReadMIDIFile(NextSongMIDI, song.HOPOThreshold, false))
                        {
                            newSong.BPM = MIDITools.MIDIInfo.AverageBPM;
                        }
                    }
                    else
                    {
                        hasNoMIDI = true;
                    }

                    if (next || prep) //only do when processing for playback
                    {
                        Tools.DeleteFile(NextSongArtPNG);
                        Tools.DeleteFile(NextSongArtBlurred);
                        if (File.Exists(PNG))
                        {
                            NextSongArtPNG = Path.GetDirectoryName(PNG) + "\\" + Path.GetFileNameWithoutExtension(PNG) + ".png";
                            NextSongArtBlurred = NextSongArtPNG.Replace(".png", "_b.png");
                            var converted = wii.Checked ? Tools.ConvertWiiImage(PNG, NextSongArtPNG, "png", false) :
                                Tools.ConvertRBImage(PNG, NextSongArtPNG, "png", false);
                            if (converted)
                            {
                                Tools.CreateBlurredArt(NextSongArtPNG, NextSongArtBlurred);
                            }
                        }
                    }

                    long length;
                    ProcessMogg(scanning, song.Length, "", out length);
                    newSong.Length = length;

                    if (!scanning) return;

                    Playlist.Add(newSong);
                    if (isScanning)
                    {
                        ShowUpdate("Added '" + newSong.Artist + " - " + newSong.Name + "'");
                    }
                }
                catch (Exception ex)
                {
                    if (message)
                    {
                        MessageBox.Show("Error reading that file:\n" + ex.Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private bool ValidateINIFile(string file, bool message)
        {
            if (Parser.ReadINIFile(file)) return true;
                       
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
            else if (Path.GetExtension(input) == ".sng")
            {
                var outFolder = Application.StartupPath + "\\temp";
                if (Directory.Exists(outFolder))
                {
                    Tools.DeleteFolder(outFolder, true);
                }
                Directory.CreateDirectory(outFolder);
                    if (!Tools.ExtractSNG(input, outFolder))
                {
                    MessageBox.Show("Failed to process SNG file '" + Path.GetFileName(input) + "', can't play it", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                var INIs = Directory.GetFiles(outFolder, "song.ini", SearchOption.TopDirectoryOnly);
                if (INIs.Count() == 0)
                {
                    MessageBox.Show("No song.ini file found, can't play that SNG file", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                INI = INIs[0];
            }
            else if (Path.GetExtension(input) == ".fnf" || Path.GetExtension(input) == ".ini")
            {
                INI = input;
            }
            if (!ValidateINIFile(INI, message)) return;
            
            if (CancelWorkers) return;
            var song = Parser.Songs[0];

            NextSongArtPNG = Path.GetDirectoryName(INI) + "\\album.png";
            NextSongArtJPG = Path.GetDirectoryName(INI) + "\\album.jpg";
            NextSongArtBlurred = Path.GetDirectoryName(INI) + "\\album_blurred.png";
            var notesMIDI = Path.GetDirectoryName(INI) + "\\notes.mid";
            var nameMIDI = Path.GetDirectoryName(INI) + "\\" + song.ShortName + ".mid";
            var notesChart = Path.GetDirectoryName(INI) + "\\notes.chart";
            if (File.Exists(notesChart) &&  !File.Exists(notesMIDI))
            {
                notesMIDI = notesChart;              
            }

            if (File.Exists(notesMIDI))
            {
                NextSongMIDI = notesMIDI;
            }
            else if (File.Exists(nameMIDI))
            {
                NextSongMIDI = nameMIDI; //this is primarily for Fortnite Festival songs
            }
            else
            {
                if (message)
                {
                    MessageBox.Show("Couldn't find the MIDI file for song '" + song.Artist + " - " + song.Name + "', can't add to the playlist",
                        AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
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
            
            Song newSong;
            if (!ValidateNewSong(song, 0, string.IsNullOrEmpty(sngPath) ? INI : sngPath, scanning, message, out newSong)) return;
            newSong.Location = input;//for .yargsong files

            if (CancelWorkers) return;
            try
            {
                newSong.BPM = 120;//default in case something fails below
                if (File.Exists(NextSongMIDI))
                {
                    MIDITools.Initialize(false);
                    if (MIDITools.ReadMIDIFile(NextSongMIDI, song.HOPOThreshold,false))
                    {
                        hasNoMIDI = false;
                        newSong.BPM = MIDITools.MIDIInfo.AverageBPM;
                    }
                }
                else
                {
                    hasNoMIDI = true;
                }

                if (next || prep) //only do when processing for playback
                {                    
                    if (File.Exists(NextSongArtPNG) || File.Exists(NextSongArtJPG))
                    {                        
                        Tools.CreateBlurredArt(File.Exists(NextSongArtPNG) ? NextSongArtPNG : NextSongArtJPG, NextSongArtBlurred);
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

                if (!scanning)
                {
                    if (m4aFiles.Any())
                    {
                        PrepareFortniteM4A();
                    }
                    return;
                }

                Playlist.Add(newSong);
                if (isScanning)
                {
                    ShowUpdate("Added '" + newSong.Artist + " - " + newSong.Name + "'");
                }
            }
            catch (Exception ex)
            {
                if (message)
                {
                    MessageBox.Show("Error reading that file:\n" + ex.Message + "\n" + ex.StackTrace, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void PrepareFortniteM4A()
        {
            StopPlayback();
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
            Task.Run(() =>
            {
                Bass.BASS_ChannelFree(BassStream);
                BassStream = fnfParser.m4aToBassStream(audio, 10);
                            
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
            }).Wait(); // Blocks until it's done
        }

        private void ProcessMogg(bool scanning, long in_length, string file, out long Length)
        {
            Length = in_length;
            if (scanning && in_length == 0)
            {
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
                {  }
            }
        }

        private void loadCON(string con, bool message = false, bool scanning = true, bool next = false, bool prep = false)
        {
            if (!ValidateDTAFile(con, message)) return;
            
            hasNoMIDI = false;
            if (message && isScanning)
            {
                message = false;
            }
            var xPackage = new STFSPackage(con);
            if (!xPackage.ParseSuccess)
            {
                if (message)
                {
                    MessageBox.Show("There was an error parsing that " + (Parser.Songs.Count > 1 ? "pack" : "song"), AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return;
            }

            for (var i = 0; i < Parser.Songs.Count; i++)
            {
                if (CancelWorkers) return;
                var song = prep ? Parser.Songs[ActiveSong.DTAIndex] : (next ? Parser.Songs[NextSong.DTAIndex] : Parser.Songs[i]);

                Song newsong;
                if (!ValidateNewSong(song, i, con, scanning, message, out newsong)) continue;
                if (ActiveSongData == null || prep)
                {
                    ActiveSongData = song;
                }

                if (CancelWorkers) return;
                var internalname = song.InternalName;
                try
                {
                    var xFile = xPackage.GetFile("songs/" + internalname + "/" + internalname + ".mogg");
                    if (xFile == null)
                    {
                        xPackage.CloseIO();
                        return;
                    }
                    var mData = xFile.Extract();
                    if (mData == null || mData.Length == 0)
                    {
                        xPackage.CloseIO();
                        return;
                    }

                    Tools.DeleteFile(NextSongMIDI);
                    newsong.BPM = 120;//default in case something fails below
                    currHOPOThreshold = song.HOPOThreshold;
                    xFile = xPackage.GetFile("songs/" + internalname + "/" + internalname + ".mid");
                    
                    if (xFile != null)
                    {
                        if (xFile.ExtractToFile(NextSongMIDI))
                        {
                            MIDITools.Initialize(false);
                            if (MIDITools.ReadMIDIFile(NextSongMIDI, song.HOPOThreshold, true))
                            {
                                newsong.BPM = MIDITools.MIDIInfo.AverageBPM;
                            }
                        }
                    }

                    if (next || prep) //only do when processing for playback
                    {
                        Tools.DeleteFile(NextSongArtPNG);
                        Tools.DeleteFile(NextSongArtBlurred);
                        xFile = xPackage.GetFile("songs/" + internalname + "/gen/" + internalname + "_keep.png_xbox");
                        
                        if (xFile != null)
                        {
                            var art = Path.GetTempPath() + "next.png_xbox";
                            Tools.DeleteFile(art);

                            if (xFile.ExtractToFile(art))
                            {
                                var converted = Tools.ConvertRBImage(art, NextSongArtPNG, "png", true);
                                if (converted)
                                {
                                    Tools.CreateBlurredArt(NextSongArtPNG, NextSongArtBlurred);
                                }
                            }
                        }
                    }

                    if (CancelWorkers) return;
                    if (!nautilus.DecM(mData, false, true, DecryptMode.ToMemory))
                    {
                        if (message && Parser.Songs.Count == 1)
                        {
                            MessageBox.Show("Song '" + song.Artist + " - " + song.Name + "' is encrypted, can't add to the playlist", AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        xPackage.CloseIO();
                        return;
                    }

                    long length;
                    ProcessMogg(scanning, song.Length, "", out length);
                    newsong.Length = length;

                    if (!scanning)
                    {
                        xPackage.CloseIO();
                        return;
                    }

                    Playlist.Add(newsong);
                    if (isScanning)
                    {
                        ShowUpdate("Added '" + newsong.Artist + " - " + newsong.Name + "'");
                    }
                }
                catch (Exception ex)
                {
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
            { }
        }

        private void DecryptPS3EDAT(string edat, bool message)
        {
            if (!File.Exists(edat)) return;
            Tools.DeleteFile(NextSongMIDI);
            Tools.DecryptEdat(edat, NextSongMIDI, currentKLIC);
            
            if (!File.Exists(NextSongMIDI))
            {
                if (message)
                {
                    MessageBox.Show("Failed to decrypt that song's EDAT file to a usable MIDI", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            } 
        }

        private void batchSongLoader_DoWork(object sender, DoWorkEventArgs e)
        {

            try
            {
                if (xbox360.Checked)
                {
                    loadCON(SongsToAdd[0], !isScanning);
                }
                else if (yarg.Checked)
                {
                    if (Path.GetExtension(SongsToAdd[0]) == ".yargsong")
                    {
                        pkgPath = "";
                        sngPath = SongsToAdd[0];
                        loadINI(SongsToAdd[0], !isScanning);
                    }
                    else if (Path.GetExtension(SongsToAdd[0]) == ".sng")
                    {
                        sngPath = SongsToAdd[0];
                        loadSNG(SongsToAdd[0], !isScanning);
                    }
                    else if (Path.GetFileName(SongsToAdd[0]) == "songs.dta")
                    {
                        pkgPath = "";
                        sngPath = "";
                        loadDTA(SongsToAdd[0], !isScanning);
                    }
                    else
                    {
                        sngPath = "";
                        loadINI(SongsToAdd[0], !isScanning);
                    }
                }
                else if (rb4PS4.Checked)
                {
                    pkgPath = "";
                    sngPath = "";
                    loadPS4Files(SongsToAdd[0], !isScanning);
                }
                else if (rockSmith.Checked)
                {
                    loadPSARC(SongsToAdd[0], !isScanning);
                }
                else if (guitarHero.Checked)
                {
                    ghwtPath = SongsToAdd[0];
                    loadGHWT(SongsToAdd[0], !isScanning);
                }
                else if (fortNite.Checked)
                {
                    loadINI(SongsToAdd[0], !isScanning);
                }
                else if (powerGig.Checked)
                {
                    ExtractXMA(SongsToAdd[0], !isScanning);
                }
                else if (bandFuse.Checked)
                {
                    BandFusePath = SongsToAdd[0];
                    ExtractBandFuse(SongsToAdd[0], !isScanning);
                }
                else
                {
                    if (pS3.Checked && Path.GetExtension(SongsToAdd[0]) == ".pkg")
                    {
                        pkgPath = SongsToAdd[0];
                        loadPKG(SongsToAdd[0], !isScanning);
                    }
                    else
                    {
                        pkgPath = "";
                        ActiveSong.yargPath = "";
                        loadDTA(SongsToAdd[0], !isScanning);
                    }
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                SongsToAdd.RemoveAt(0);
            }            
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
                    Tools.CreateBlurredArt(NextSongArtPNG, NextSongArtBlurred);
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
            sngPath = sng;
            loadINI(sng, message, scanning, next, prep);
        }

        private void ExtractXMA(string xml, bool message = false, bool scanning = true, bool next = false, bool prep = false)
        {
            XML_PATH = xml;
            var XMAs = Directory.GetFiles(Path.GetDirectoryName(XML_PATH), "*.xma", SearchOption.TopDirectoryOnly);
            var ogXMA = XMAs[0];

            if (!ValidateXMLFile(xml, message)) return;
            
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

            NextSongArtPNG = album_art;
            NextSongMIDI = "";
            hasNoMIDI = true;

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
                        Tools.CreateBlurredArt(NextSongArtPNG, NextSongArtBlurred);
                    }
                }

                if (scanning)
                {
                    Playlist.Add(newSong);
                    if (isScanning)
                    {
                        ShowUpdate("Added '" + newSong.Artist + " - " + newSong.Name + "'");
                    }
                    return;
                }
            }
            catch (Exception ex)
            {
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
            if (Parser.ReadHSANFile(file)) return true;
            
            if (message)
            {
                MessageBox.Show("Something went wrong reading that HSAN file, can't add to the playlist", AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }

        private bool ValidateXMLFile(string file, bool message)
        {
            if (Parser.ReadXMLFile(file)) return true;
            
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
                        
            bool hasRhythm = false;
            bool hasVocals = false;

            if (metadataFiles.Count() > 0)
            {
                HSAN = metadataFiles[0];
            }
            if (!ValidateHSANFile(HSAN, message)) return;
            
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

            NextSongArtPNG = album_art;
            NextSongMIDI = "";
            hasNoMIDI = true;

            if (!OggFiles.Any())
            {
                MessageBox.Show("Couldn't find audio files for song '" + song.Artist + " - " + song.Name + "', can't add to the playlist",
                        AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

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
                        Tools.CreateBlurredArt(NextSongArtPNG, NextSongArtBlurred);
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

                if (scanning)
                {
                    Playlist.Add(newSong);
                    if (isScanning)
                    {
                        ShowUpdate("Added '" + newSong.Artist + " - " + newSong.Name + "'");
                    }
                }
            }
            catch (Exception ex)
            {
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
            if (xbox360.Checked)
            {
                loadCON(SongToLoad, true);
            }
            else if (yarg.Checked)
            {
                if (Path.GetFileName(SongToLoad) == ".yargsong")
                {
                    sngPath = SongToLoad;
                    pkgPath = "";
                    loadINI(SongToLoad, true);
                }
                else if (Path.GetExtension(SongToLoad) == ".sng")
                {
                    sngPath = SongToLoad;
                    loadSNG(SongToLoad, true);
                }
                else if (Path.GetFileName(SongToLoad) == "songs.dta")
                {
                    sngPath = "";
                    pkgPath = "";
                    loadDTA(SongToLoad, true);
                }
                else
                {
                    sngPath = "";
                    loadINI(SongToLoad, true);
                }
            }
            else if (rb4PS4.Checked)
            {
                loadPS4Files(SongToLoad, true);
            }
            else if (rockSmith.Checked)
            {
                loadPSARC(SongToLoad, true);
            }
            else if (powerGig.Checked)
            {
                ExtractXMA(SongToLoad, true);
            }
            else if (bandFuse.Checked)
            {
                BandFusePath = SongToLoad;
                ExtractBandFuse(SongToLoad, true);
            }
            else if (guitarHero.Checked)
            {
                ghwtPath = SongToLoad;
                loadGHWT(SongToLoad, true);
            }
            else
            {
                if (pS3.Checked && Path.GetExtension(SongToLoad) == ".pkg")
                {
                    pkgPath = SongToLoad;
                    loadPKG(SongToLoad, true);
                }
                else
                {
                    pkgPath = "";
                    ActiveSong.yargPath = "";
                    loadDTA(SongToLoad, true);
                }
            }
        }

        private void batchSongLoader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                videoOverlay.TopMost = false;
                MessageBox.Show(e.Error.ToString());
                videoOverlay.TopMost = true;
                return;
            }

            if (SongsToAdd.Any() && !CancelWorkers)
            {
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
                ShowUpdate(msg);
            }
            else
            {
                var msg = "Added " + added + " new " + (added == 1 ? "song" : "songs");
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
                var notify = "Playing " + PlayingSong.Artist + " - " + PlayingSong.Name;
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
            NotifyTray.Text = text;
            Text = AppName + " - " + PlaylistName + " - " + text;
        }

        private void songLoader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
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
                var result = MessageBox.Show("Thre is a pending process running, exiting now may corrupt data!\nClick OK to force the program to exit\nClick Cancel to return and wait", AppName, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }
            if (Text.Contains("*"))
            {
                if (MessageBox.Show("You have unsaved changes on the current playlist\nAre you sure you want to close cPlayer?",
                    AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
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

        private void SelectStageKitController()
        {
            try
            {
                stageKit = new StageKitController(stageKitIndex);
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
                Tools.DeleteFile(CurrentSongArt);
            }
            if (!all_files) return;
            Tools.DeleteFile(CurrentSongArt);
            Tools.DeleteFile(CurrentSongArtBlurred);
            if (xbox360.Checked || pS3.Checked)
            {
                Tools.DeleteFile(CurrentSongMIDI);
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
        }

        private void DoClickStop()
        {
            stageTimer.Enabled = false;            
            PlaybackTimer.Enabled = false;
            StopPlayback();
            ClearVisuals(true);
            lblSections.Text = "";
            PlaybackSeconds = 0;
            _renderedFrame = null;
            picVisuals.BackColor = Color.AliceBlue;
            picVisuals.Image = Resources.logo;
            videoOverlay.Hide();
            //videoOverlayShown = false;
            if (secondScreen != null)
            {
                secondScreen.ChangeBackgroundColor(Color.AliceBlue);
                secondScreen.ChangeBackgroundImage(Resources.logo);
                secondScreen.ClearOverlayFrame();
            }            
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
                        channels[3] = 5;
                        channels[4] = 6;
                        channels[5] = 4;
                        channels[6] = 3;
                        break;
                    case 8:
                        channels[0] = 0;
                        channels[1] = 2;
                        channels[2] = 1;
                        channels[3] = 6;
                        channels[4] = 4;
                        channels[5] = 7;
                        channels[6] = 5;
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
            ClearVisuals();
            PlaybackSeconds = ((double)PlayingSong.Length / 1000) * ((double)(e.X - (panelSlider.Width / 2)) / (panelLine.Width - panelSlider.Width));
            if (Bass.BASS_ChannelIsActive(BassMixer) == BASSActive.BASS_ACTIVE_PAUSED || Bass.BASS_ChannelIsActive(BassMixer) == BASSActive.BASS_ACTIVE_PLAYING)
            {
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

        private static readonly Random _rng = new Random();

        private int ShuffleSongs(bool can_repeat = false)
        {
            int count = lstPlaylist.Items.Count;
            if (count <= 0) return -1;
            if (count == 1) return 0;

            // Build valid candidate indices
            var candidates = new List<int>(count);

            for (int i = 0; i < count; i++)
            {
                if (PlayingSong != null && i == PlayingSong.Index)
                    continue;

                var tag = lstPlaylist.Items[i].Tag?.ToString();
                if (!can_repeat && tag == "1")
                    continue;

                candidates.Add(i);
            }

            if (candidates.Count == 0)
                return -1;

            lock (_rng)
            {
                return candidates[_rng.Next(candidates.Count)];
            }
        }

        private void playNowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            doSongPreparer();
        }

        private void doSongPreparer()
        {
            DoClickStop();
            GetActiveSong(lstPlaylist.SelectedItems[0].SubItems[0]);
            NextSongIndex = lstPlaylist.SelectedIndices[0];
            EnableDisable(false);
            nautilus.NextSongOggData = new byte[0];
            nautilus.ReleaseStreamHandle(true);
            ActiveSong.yargPath = "";
            InitiateGIFOverlay();
            songPreparer.RunWorkerAsync();
        }

        private void UpdateHighlights()
        {
            for (var i = 0; i < lstPlaylist.Items.Count; i++)
            {
                lstPlaylist.Items[i].BackColor = Color.AliceBlue;
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
            if (songExtractor.IsBusy && NextSong.Location == ActiveSong.Location)
            {
                do
                {//wait here
                } while (songExtractor.IsBusy);
            }

            if (xbox360.Checked)
            {
                loadCON(ActiveSong.Location, false, false, false, true);
            }
            else if (yarg.Checked)
            {
                if (Path.GetExtension(ActiveSong.Location) == ".yargsong")
                {
                    sngPath = ActiveSong.Location;
                    loadINI(ActiveSong.Location, false, false, false, true);
                }
                else if (Path.GetExtension(ActiveSong.Location) == ".sng")
                {
                    sngPath = ActiveSong.Location;
                    loadSNG(ActiveSong.Location, false, false, false, true);
                }
                else if (Path.GetFileName(ActiveSong.Location) == "songs.dta")
                {
                    pkgPath = "";
                    sngPath = "";
                    loadDTA(ActiveSong.Location, false, false, false, true);
                }
                else
                {
                    sngPath = "";
                    loadINI(ActiveSong.Location, false, false, false, true);
                }
            }
            else if (rb4PS4.Checked)
            {
                pkgPath = "";
                sngPath = "";
                loadPS4Files(ActiveSong.Location, false, false, false, true);
            }
            else if (fortNite.Checked)
            {
                loadINI(ActiveSong.Location, false, false, false, true);
            }
            else if (guitarHero.Checked)
            {
                ghwtPath = ActiveSong.Location;
                loadGHWT(ActiveSong.Location, false, false, false, true);
            }
            else if (rockSmith.Checked)
            {
                loadPSARC(ActiveSong.Location, false, false, false, true);
            }
            else if (powerGig.Checked)
            {
                ExtractXMA(ActiveSong.Location, false, false, false, true);
            }
            else if (bandFuse.Checked)
            {
                BandFusePath = ActiveSong.Location;
                ExtractBandFuse(ActiveSong.Location, false, false, false, true);
            }
            else
            {
                if (pS3.Checked && Path.GetExtension(ActiveSong.Location) == ".pkg")
                {
                    pkgPath = ActiveSong.Location;
                    loadPKG(ActiveSong.Location, false, false, false, true);
                }
                else
                {
                    pkgPath = "";
                    loadDTA(ActiveSong.Location, false, false, false, true);
                }
            }
        }

        private void songPreparer_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
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
            if ((!yarg.Checked && !fortNite.Checked && !guitarHero.Checked && !powerGig.Checked && !bandFuse.Checked) && (CurrentSongAudio == null || CurrentSongAudio.Length == 0))
            {
                if (AlreadyTried)
                {
                    MessageBox.Show("Unable to play that song - either the song files are in use by another program or the audio file is encrypted", AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    EnableDisable(true);
                    AlreadyTried = false;
                }
                else
                {
                    AlreadyTried = true;
                    doSongPlayback();
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
            lblAuthor.Text = string.IsNullOrEmpty(PlayingSong.Charter.Trim()) ? "" : "Author: " + RemoveCloneHeroColor(PlayingSong.Charter);
            lblAuthor.ForeColor = string.IsNullOrEmpty(PlayingSong.Charter.Trim()) ? Color.Black : GetCloneHeroColor(PlayingSong.Charter);
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
                if (!chartVisualsToolStripMenuItem.Checked && !displayKaraokeMode.Checked)
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
            StartPlayback(PlaybackSeconds == 0, true);
        }

        private void EnableDisableButtons(bool enabled)
        {
            //picPlay.Enabled = enabled;
            picPause.Enabled = enabled;
            picStop.Enabled = enabled;
            picNext.Enabled = enabled;
        }

        private void PrepareForDrawing()
        {
            if (PlayingSong == null) return;
            MIDITools.Initialize(true);
            if (!MIDITools.ReadMIDIFile(CurrentSongMIDI, currHOPOThreshold, true))
            {
                ShowUpdate("Error reading MIDI file!");
                displayAudioSpectrum.Checked = true;
                chartVisualsToolStripMenuItem.Checked = false;
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
                var size = secondScreen == null ? picVisuals.ClientSize : secondScreen.RenderSize();
                ChartBitmap = new Bitmap(size.Width, size.Height);
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
            if (!MIDITools.MIDI_Chart.Vocals.ChartedNotes.Any() || ((!doMIDIVocals && !doMIDIHarmonies) || doMIDINoVocals)) return tracks;
            if (MIDITools.MIDI_Chart.Vocals.NoteRange.Count > 8 || chartVertical.Checked || rBStyle.Checked)
            {
                tracks += tall;
            }
            else
            {
                tracks++;
            }
            return tracks;
        }

        private void DrawMIDIFile(Size size, Graphics graphics)
        {
            if (MIDITools.MIDI_Chart == null || MIDITools.PhrasesVocals == null) return;
            const int tall = 2;
            var tracks = GetTrackstoDraw();
            if (tracks == 0) return;
            var panel_height = size.Height - GetHeightDiff();
            var track_height = panel_height / tracks;
            var track_y = lblSections.Visible ? lblSections.Height : 0;
            int Index;
            var track_color = 1;
            var renderSize = new Size(1920, 1080);

            if (secondScreen != null)
            {
                secondScreen.ChangeBackgroundColor(chartVertical.Checked || rBStyle.Checked ? Color.Black : Color.FromArgb(200, 200, 200));
                picVisuals.BackColor = Color.AliceBlue;
            }
            else
            {
                picVisuals.BackColor = chartVertical.Checked || rBStyle.Checked ? Color.Black : Color.FromArgb(200, 200, 200);
            }
            if (!chartVertical.Checked && !rBStyle.Checked)
            {
                if (MIDITools.MIDI_Chart.Drums.ChartedNotes.Count > 0 && doMIDIDrums)
                {
                    track_y += track_height;
                    DrawTrackBackground(graphics, track_y, track_height, track_color, "DRUMS", MIDITools.MIDI_Chart.Drums.Solos, Instrument.Drums);
                    DrawNotes(graphics, MIDITools.MIDI_Chart.Drums, track_height, track_y, true, -1, out Index);
                    MIDITools.MIDI_Chart.Drums.ActiveIndex = Index;
                    track_color++;
                }
                if (MIDITools.MIDI_Chart.Bass.ChartedNotes.Count > 0 && doMIDIBass)
                {
                    track_y += track_height;
                    DrawTrackBackground(graphics, track_y, track_height, track_color, PlayingSong.isRhythmOnBass ? "RHYTHM GUITAR" : "BASS", MIDITools.MIDI_Chart.Bass.Solos, Instrument.Bass);
                    DrawNotes(graphics, MIDITools.MIDI_Chart.Bass, track_height, track_y, false, -1, out Index);
                    MIDITools.MIDI_Chart.Bass.ActiveIndex = Index;
                    track_color++;
                }
                if (MIDITools.MIDI_Chart.Guitar.ChartedNotes.Count > 0 && doMIDIGuitar)
                {
                    track_y += track_height;
                    DrawTrackBackground(graphics, track_y, track_height, track_color, "GUITAR", MIDITools.MIDI_Chart.Guitar.Solos, Instrument.Guitar);
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
                    DrawTrackBackground(graphics, track_y, track_height * multKeys, track_color, "PRO KEYS", MIDITools.MIDI_Chart.ProKeys.Solos, Instrument.ProKeys);
                    DrawNotes(graphics, MIDITools.MIDI_Chart.ProKeys, track_height * multKeys, track_y, false, -1, out Index);
                    MIDITools.MIDI_Chart.ProKeys.ActiveIndex = Index;
                    track_color++;
                }
                else if (MIDITools.MIDI_Chart.Keys.ChartedNotes.Count > 0 && doMIDIKeys)
                {
                    track_y += track_height;
                    DrawTrackBackground(graphics, track_y, track_height, track_color, PlayingSong.isRhythmOnKeys ? "RHYTHM GUITAR" : "KEYS", MIDITools.MIDI_Chart.Keys.Solos, Instrument.Keys);
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
            if (MIDITools.MIDI_Chart.Vocals.NoteRange.Count > 8 || chartVertical.Checked || rBStyle.Checked)
            {
                multVocals = tall;
            }
            if (doMIDIVocals || doMIDIHarmonies)
            {
                if (chartVertical.Checked || rBStyle.Checked)
                {
                    if (chartVertical.Checked)
                    {
                        using (var overlayBrush = new SolidBrush(
                        Color.FromArgb(chartVertical.Checked ? 255 : 128, Color.Black)))
                        {
                            graphics.FillRectangle(overlayBrush, 0, 0, renderSize.Width, vocalsHeight + 8);
                        }
                    }
                    graphics.DrawImage(chartVertical.Checked ? Resources.frostedglass75 : Resources.frostedglass50, 0, rBStyle.Checked ? GetYForRBVocals() : 0, renderSize.Width, vocalsHeight + 8);                    
                    DrawPhraseMarkers(graphics, MIDITools.PhrasesVocals, vocalsHeight, 4);
                    track_y = vocalsHeight;
                }
                else
                {
                    track_y += track_height * multVocals;
                    DrawTrackBackground(graphics, track_y, track_height * multVocals, track_color, MIDITools.MIDI_Chart.Harm1.ChartedNotes.Any() && doMIDIHarmonies ? "HARMONIES" : "VOCALS", null, Instrument.Vocals);
                    DrawPhraseMarkers(graphics, MIDITools.PhrasesVocals, track_height * multVocals, track_y);
                }
            }
            DrawLyrics(size, graphics, chartVisualsToolStripMenuItem.Checked ? RBStyleVocalsBackgroundColor : Color.FromArgb(127, 200, 200, 200));
            if ((!doMIDIVocals && !doMIDIHarmonies) || doMIDINoVocals) return;
            List<MIDITrack> activeTracks = new List<MIDITrack>();
            var waitY = GetYForRBVocals() + (vocalsHeight / 2);
            if (MIDITools.MIDI_Chart.Harm3.ChartedNotes.Count > 0 && doMIDIHarmonies)
            {
                activeTracks.Add(MIDITools.MIDI_Chart.Harm3);
                DrawNotes(graphics, MIDITools.MIDI_Chart.Harm3, track_height * multVocals, track_y, false, 3, out Index);
                MIDITools.MIDI_Chart.Harm3.ActiveIndex = Index;
            }
            if (MIDITools.MIDI_Chart.Harm2.ChartedNotes.Count > 0 && doMIDIHarmonies)
            {
                activeTracks.Add(MIDITools.MIDI_Chart.Harm2);
                DrawNotes(graphics, MIDITools.MIDI_Chart.Harm2, track_height * multVocals, track_y, false, 2, out Index);
                MIDITools.MIDI_Chart.Harm2.ActiveIndex = Index;
            }
            if (MIDITools.MIDI_Chart.Harm1.ChartedNotes.Count > 0 && doMIDIHarmonies)
            {
                activeTracks.Add(MIDITools.MIDI_Chart.Harm1);
                DrawNotes(graphics, MIDITools.MIDI_Chart.Harm1, track_height * multVocals, track_y, false, 1, out Index);
                MIDITools.MIDI_Chart.Harm1.ActiveIndex = Index;
            }
            else
            {
                activeTracks.Add(MIDITools.MIDI_Chart.Vocals);                
                DrawNotes(graphics, MIDITools.MIDI_Chart.Vocals, track_height * multVocals, track_y, false, 0, out Index);
                MIDITools.MIDI_Chart.Vocals.ActiveIndex = Index;
            }
            if (chartVertical.Checked || rBStyle.Checked)
            {
                double time = GetCorrectedTime();

                const double gapSeconds = 5.0;
                const double grace = 0.05;                 // match your sustain grace
                double window = PlaybackWindowRB;          // your lookahead window (seconds)

                // 1) Find the latest end time among notes that are currently "visible / relevant"
                //    Visible condition: (entered window) AND (not fully gone yet)
                double latestVisibleEnd = double.NegativeInfinity;

                for (int t = 0; t < activeTracks.Count; t++)
                {
                    var notes = activeTracks[t].ChartedNotes;
                    if (notes == null || notes.Count == 0) continue;

                    for (int i = 0; i < notes.Count; i++)
                    {
                        var n = notes[i];

                        // Visible/relevant now if:
                        // - it has entered the forward window (start <= time + window)
                        // - and it hasn't fully ended yet (end + grace >= time)
                        if (n.NoteStart <= time + window && (n.NoteEnd + grace) >= time)
                        {
                            if (n.NoteEnd > latestVisibleEnd)
                                latestVisibleEnd = n.NoteEnd;
                        }

                        // Optional micro-optimization if notes are sorted by start:
                        // once NoteStart > time + window, the rest can't be visible yet.
                        if (n.NoteStart > time + window)
                            break;
                    }
                }

                // If nothing is visible, treat "latestVisibleEnd" as "now"
                if (double.IsNegativeInfinity(latestVisibleEnd))
                    latestVisibleEnd = time;

                // 2) Find the earliest upcoming note start AFTER the visible content finishes
                MIDINote nextAfterGap = null;
                double nextStart = double.PositiveInfinity;

                for (int t = 0; t < activeTracks.Count; t++)
                {
                    var notes = activeTracks[t].ChartedNotes;
                    if (notes == null || notes.Count == 0) continue;

                    for (int i = 0; i < notes.Count; i++)
                    {
                        var n = notes[i];

                        if (n.NoteStart >= latestVisibleEnd) // next thing after what’s currently on-screen
                        {
                            if (n.NoteStart < nextStart)
                            {
                                nextStart = n.NoteStart;
                                nextAfterGap = n;
                            }
                            break; // sorted by start => first match is the earliest in this track
                        }
                    }
                }

                // 3) Only show timer if the gap is big enough AND we are inside that gap
                if (nextAfterGap != null)
                {
                    double gap = nextStart - latestVisibleEnd;

                    if (gap >= gapSeconds)
                    {
                        // We only want the timer after current visuals are done:
                        // i.e., once time is past latestVisibleEnd (plus grace if you want).
                        if (time >= latestVisibleEnd)
                        {
                            double wait = nextStart - time; // countdown to the next phrase

                            if (wait > 0)
                            {
                                DrawWaitTimeTextRB(graphics, activeTracks[0].ChartedNotes, waitY, waitY, renderSize.Width / 2, wait.ToString("0"));
                            }
                        }
                    }
                }
            }
            
            if (!chartVertical.Checked && !rBStyle.Checked) return;
            DrawHitbox(graphics, bmpHitboxVocals, HitboxVocalsX + (bmpHitboxVocals.Width / 2) - 4, GetYForRBVocals(), 4, vocalsHeight, 1, "");
        }

        private void DrawHitbox(Graphics graphics, Bitmap image, int posX, int posY, int width, int height, float opacity, string trackName)
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

                // Draw the image with the transparency applied
                graphics.DrawImage(image, new Rectangle(posX, posY, width, height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
            }

            if (!doMIDINameTracks || (!chartVertical.Checked && !rBStyle.Checked)) return;
            Font font;
            try
            {
                font = new Font("Verdana", 12f, FontStyle.Regular);
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

        private void DrawTrackPerspectiveTrapezoidFilled(
            Graphics g,
            Image trackBmp,
            int chartLeft,
            int topY,
            int trackHeight,
            int trackWidth,
            float horizonY,
            float hitboxY,
            int strips = 120
)
        {
            if (trackBmp == null || trackHeight <= 0 || trackWidth <= 0) return;

            // Clamp to track rect
            float fullTop = topY;
            float fullBottom = topY + trackHeight;

            if (hitboxY > fullBottom) hitboxY = fullBottom;
            if (hitboxY < fullTop + 2) hitboxY = fullBottom;

            if (horizonY < fullTop) horizonY = fullTop;
            if (horizonY > hitboxY - 2) horizonY = fullTop;

            float spanY = hitboxY - horizonY;
            if (spanY < 2) return;

            // Trapezoid geometry (straight sides)
            float centerX = chartLeft + (trackWidth / 2f);
                        
            const float topWidthFactor = 0.35f; // tweak 0.45–0.70
            float topW = trackWidth * topWidthFactor;
            float bottomW = trackWidth;

            float topLeftX = centerX - (topW / 2f);
            float topRightX = centerX + (topW / 2f);
            float bottomLeftX = chartLeft;
            float bottomRightX = chartLeft + trackWidth;

            // Helper: linear interpolate between two floats
            float LerpF(float a, float b, float t) => a + (b - a) * t;

            // Clip to trapezoid so nothing spills out
            using (var path = new System.Drawing.Drawing2D.GraphicsPath())
            {
                path.AddPolygon(new[]
                {
            new PointF(topLeftX, horizonY),
            new PointF(topRightX, horizonY),
            new PointF(bottomRightX, hitboxY),
            new PointF(bottomLeftX, hitboxY)
                });

                var oldClip = g.Clip;
                g.SetClip(path);

                var oldInterp = g.InterpolationMode;
                var oldPixel = g.PixelOffsetMode;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                // Source geometry
                int srcW = trackBmp.Width;
                int srcH = trackBmp.Height;

                // Draw horizontal strips; IMPORTANT: each strip width matches trapezoid edges at that Y
                for (int i = 0; i < strips; i++)
                {
                    float t0 = (float)i / strips;           // 0..1 from horizon->hitbox
                    float t1 = (float)(i + 1) / strips;

                    float y0 = horizonY + (spanY * t0);
                    float y1 = horizonY + (spanY * t1);
                    float h = Math.Max(1f, y1 - y0);

                    // Compute trapezoid left/right edges at this Y
                    float leftX = LerpF(topLeftX, bottomLeftX, t0);
                    float rightX = LerpF(topRightX, bottomRightX, t0);
                    float w = Math.Max(1f, rightX - leftX);

                    // Source slice matching this vertical band
                    int sy0 = (int)Math.Round(srcH * t0);
                    int sy1 = (int)Math.Round(srcH * t1);
                    int sh = Math.Max(1, sy1 - sy0);                                     

                    var srcRect = new Rectangle(0, sy0, srcW, sh);
                    var dstRect = new RectangleF(leftX, y0, w, h);

                    g.DrawImage(trackBmp, dstRect, srcRect, GraphicsUnit.Pixel);
                }

                g.InterpolationMode = oldInterp;
                g.PixelOffsetMode = oldPixel;
                g.Clip = oldClip;
            }
            
            using (var pen = new Pen(Color.FromArgb(60, Color.White), 1f))
            {
                g.DrawLine(pen, topLeftX, horizonY, bottomLeftX, hitboxY);
                g.DrawLine(pen, topRightX, horizonY, bottomRightX, hitboxY);
            }
        }        

        private sealed class BeatMarker
        {
            public long Tick;
            public double TimeSeconds;
            public bool IsMeasure;
            public int BeatInMeasure;
        }

        private List<BeatMarker> BuildBeatMarkers_UseGetRealtime(
            long endTick,
            int ticksPerQuarter,
            List<TimeSignature> timeSignatures
        )
        {
            var beats = new List<BeatMarker>(4096);
            if (ticksPerQuarter <= 0) return beats;

            // prep time sigs
            var sigs = new List<TimeSignature>(timeSignatures ?? new List<TimeSignature>());
            sigs.Sort((a, b) => a.AbsoluteTime.CompareTo(b.AbsoluteTime));

            if (sigs.Count == 0 || sigs[0].AbsoluteTime != 0)
                sigs.Insert(0, new TimeSignature { AbsoluteTime = 0, Numerator = 4, Denominator = 4 });

            int sigIndex = 0;
            var curSig = sigs[0];
            long nextSigTick = (sigs.Count > 1) ? sigs[1].AbsoluteTime : long.MaxValue;

            long BeatTicks(TimeSignature ts)
            {
                int den = ts.Denominator <= 0 ? 4 : ts.Denominator;
                double v = ticksPerQuarter * (4.0 / den);
                if (double.IsNaN(v) || double.IsInfinity(v) || v < 1.0) v = ticksPerQuarter;
                long bt = (long)Math.Round(v);
                return bt < 1 ? 1 : bt;
            }

            long beatTicks = BeatTicks(curSig);
            long tick = curSig.AbsoluteTime;
            int beatInMeasure = 0;

            int guard = 0;
            while (tick <= endTick && guard++ < 5_000_000)
            {
                if (tick >= nextSigTick)
                {
                    sigIndex++;
                    curSig = sigs[sigIndex];
                    nextSigTick = (sigIndex + 1 < sigs.Count) ? sigs[sigIndex + 1].AbsoluteTime : long.MaxValue;

                    beatTicks = BeatTicks(curSig);
                    tick = curSig.AbsoluteTime;
                    beatInMeasure = 0;

                    if (tick > endTick) break;
                }

                beats.Add(new BeatMarker
                {
                    Tick = tick,
                    TimeSeconds = MIDITools.GetRealtime(tick), 
                    IsMeasure = (beatInMeasure == 0),
                    BeatInMeasure = beatInMeasure
                });

                long nextTick = tick + beatTicks;
                int nextBeat = beatInMeasure + 1;
                if (nextBeat >= (curSig.Numerator <= 0 ? 4 : curSig.Numerator)) nextBeat = 0;

                if (nextSigTick != long.MaxValue && nextTick > nextSigTick)
                {
                    tick = nextSigTick;
                    beatInMeasure = 0;
                }
                else
                {
                    tick = nextTick;
                    beatInMeasure = nextBeat;
                }
            }

            return beats;
        }

        private List<BeatMarker> _beatMarkers = new List<BeatMarker>();

        private void DrawBeatLinesPerspective_FromMarkers(
            Graphics g,
            double correctedTime,
            float horizonY,
            float hitboxY,
            float overshootPx,
            int chartLeft,
            int trackWidth,
            double playbackWindow,
            double minScale,
            double maxScale,
            double depthPower
        )
        {
            if (_beatMarkers == null || _beatMarkers.Count == 0) return;

            double ClampMin0(double v) => v < 0 ? 0 : v;
            double EaseIn(double t) => Math.Pow(t, depthPower);
            double Lerp(double a, double b, double t) => a + (b - a) * t;

            float trackCenterX = chartLeft + (trackWidth / 2f);

            // Find first beat in window (linear scan ok; can optimize later)
            int start = 0;
            while (start < _beatMarkers.Count && _beatMarkers[start].TimeSeconds < correctedTime)
                start++;

            for (int i = start; i < _beatMarkers.Count; i++)
            {
                double bt = _beatMarkers[i].TimeSeconds;
                if (bt > correctedTime + playbackWindow) break;

                // ✅ EXACT SAME t->p as gems
                double tBeat = 1.0 - ((bt - correctedTime) / playbackWindow);
                tBeat = ClampMin0(tBeat);
                double pBeat = EaseIn(tBeat); // can be > 1

                // ✅ EXACT SAME Y mapping as gems
                float y = (float)Lerp(horizonY, hitboxY + overshootPx, pBeat);

                // If you don't want beat lines past hitbox, clamp:
                if (y > hitboxY) y = hitboxY;
                if (y < horizonY) continue;

                // ✅ EXACT SAME span mapping as gems (NO Clamp01)
                double scale = Lerp(minScale, maxScale, pBeat);
                double span = trackWidth * scale;

                float leftX = (float)(trackCenterX - (span / 2.0));
                float rightX = (float)(trackCenterX + (span / 2.0));

                bool isMeasure = _beatMarkers[i].IsMeasure;
                int alpha = isMeasure ? 150 : 80;
                float thickness = isMeasure ? 2f : 1f;

                using (var pen = new Pen(Color.FromArgb(alpha, 255, 255, 255), thickness))
                {
                    g.DrawLine(pen, leftX, y, rightX, y);
                }
            }
        }


        private void DrawRockBandStyle(Graphics graphics)
        {
            var renderSize = new Size(1920, 1080);
            
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
            var track_width = (renderSize.Width - (padding * 2 * tracks)) / tracks;
            if (track_width > maxTrackWidth)
            {
                track_width = maxTrackWidth;
            }

            // Adjust the total width of all tracks including padding
            var totalTracksWidth = (track_width * tracks) + (padding * 2 * tracks);

            // Calculate starting X position to center the tracks
            var startX = (renderSize.Width - totalTracksWidth) / 2;

            var track_height = renderSize.Height; // Adjust as needed
            var y = renderSize.Height - track_height;
            var lastX = startX; // Initialize to starting position

            float hitboxY = track_height - 50f;
            const float horizonPercent = 0.50f;
            float horizonY = y + ((hitboxY - y) * horizonPercent);                                             

            // Draw Bass track if present
            if (MIDITools.MIDI_Chart.Bass.ChartedNotes.Any() && doMIDIBass)
            {
                var isSolo = MIDITools.MIDI_Chart.Bass.Solos != null && MIDITools.MIDI_Chart.Bass.Solos.Any(solo => solo.MarkerBegin <= PlaybackSeconds && solo.MarkerEnd > PlaybackSeconds);
                bassX = lastX + padding;
                if (chartVertical.Checked)
                {
                    float trackCenterX = bassX + (track_width / 2f);
                    graphics.DrawImage(isSolo ? bmpBackgroundBassSolo : bmpBackgroundBass, bassX, y, track_width, track_height);
                    DrawWaitTimeRB(graphics, MIDITools.MIDI_Chart.Bass.ChartedNotes, horizonY, hitboxY, trackCenterX);
                }
                else
                {
                    DrawInstrumentHitboxLabel(
                        graphics,
                        "BASS",
                        bassX,
                        hitboxY,
                        track_width,
                        renderSize.Height - hitboxY,
                        renderSize
                    );
                    DrawTrackPerspectiveTrapezoidFilled(
                        graphics,
                        isSolo ? bmpBackgroundBassSoloRB : bmpBackgroundBassRB,
                        bassX,
                        y,
                        track_height,
                        track_width,
                        horizonY,
                        hitboxY + 20f,
                        strips: 120
                    );
                    float trackCenterX = bassX + (track_width / 2f);                    
                    DrawHighwaySideBordersPerspective(
                        graphics,
                        horizonY,
                        hitboxY + 20f,
                        trackCenterX,
                        track_width,
                        minScale: 0.35,
                        maxScale: 1.00,
                        insetPx: 4,
                        stepY: 10
                    );
                    DrawWaitTimeRB(graphics, MIDITools.MIDI_Chart.Bass.ChartedNotes, horizonY, hitboxY, trackCenterX);
                }
                DrawHitbox(graphics, chartVertical.Checked ? bmpHitbox : Resources.hitbox_5lane, bassX, renderSize.Height - 52, track_width, 30, 0.90f, chartVertical.Checked ? "Bass" : "");
                lastX += track_width + (2 * padding);
            }

            // Draw Drums track if present
            if (MIDITools.MIDI_Chart.Drums.ChartedNotes.Any() && doMIDIDrums)
            {
                var isSolo = MIDITools.MIDI_Chart.Drums.Solos != null && MIDITools.MIDI_Chart.Drums.Solos.Any(solo => solo.MarkerBegin <= PlaybackSeconds && solo.MarkerEnd > PlaybackSeconds);
                drumsX = lastX + padding;
                if (chartVertical.Checked)
                {
                    float trackCenterX = drumsX + (track_width / 2f);
                    graphics.DrawImage(isSolo ? bmpBackgroundDrumsSolo : bmpBackgroundDrums, drumsX, y, track_width, track_height);
                    DrawWaitTimeRB(graphics, MIDITools.MIDI_Chart.Drums.ChartedNotes, horizonY, hitboxY, trackCenterX);
                }
                else
                {
                    DrawInstrumentHitboxLabel(
                        graphics,
                        "PRO DRUMS",
                        drumsX,
                        hitboxY,
                        track_width,
                        renderSize.Height - hitboxY,
                        renderSize
                    );
                    DrawTrackPerspectiveTrapezoidFilled(
                         graphics,
                         isSolo ? bmpBackgroundDrumsSoloRB : bmpBackgroundDrumsRB,
                         drumsX,
                         y,
                         track_height,
                         track_width,
                         horizonY,
                         hitboxY + 20f,
                         strips: 120
                     );
                    float trackCenterX = drumsX + (track_width / 2f);                    
                    DrawHighwaySideBordersPerspective(
                        graphics,
                        horizonY,
                        hitboxY + 20f,
                        trackCenterX,
                        track_width,
                        minScale: 0.35,
                        maxScale: 1.00,
                        insetPx: 4,
                        stepY: 10
                    );
                    DrawWaitTimeRB(graphics, MIDITools.MIDI_Chart.Drums.ChartedNotes, horizonY, hitboxY, trackCenterX);
                }
                DrawHitbox(graphics, chartVertical.Checked ? bmpHitbox : Resources.hitbox_drums, drumsX, renderSize.Height - 52, track_width, 30, 0.90f, chartVertical.Checked ? "Pro Drums" : "");
                lastX += track_width + (2 * padding); // Move to the next position
            }

            // Draw Guitar track if present
            if (MIDITools.MIDI_Chart.Guitar.ChartedNotes.Any() && doMIDIGuitar)
            {
                var isSolo = MIDITools.MIDI_Chart.Guitar.Solos != null && MIDITools.MIDI_Chart.Guitar.Solos.Any(solo => solo.MarkerBegin <= PlaybackSeconds && solo.MarkerEnd > PlaybackSeconds);
                guitarX = lastX + padding;
                if (chartVertical.Checked)
                {
                    float trackCenterX = guitarX + (track_width / 2f);
                    graphics.DrawImage(isSolo ? bmpBackgroundGuitarSolo : bmpBackgroundGuitar, guitarX, y, track_width, track_height);
                    DrawWaitTimeRB(graphics, MIDITools.MIDI_Chart.Guitar.ChartedNotes, horizonY, hitboxY, trackCenterX);
                }
                else
                {
                    DrawInstrumentHitboxLabel(
                        graphics,
                        "GUITAR",
                        guitarX,
                        hitboxY,
                        track_width,
                        renderSize.Height - hitboxY,
                        renderSize
                    );
                    DrawTrackPerspectiveTrapezoidFilled(
                        graphics,
                        isSolo ? bmpBackgroundGuitarSoloRB : bmpBackgroundGuitarRB,
                        guitarX,
                        y,
                        track_height,
                        track_width,
                        horizonY,
                        hitboxY + 20f,
                        strips: 120
                    );
                    float trackCenterX = guitarX + (track_width / 2f);                    
                    DrawHighwaySideBordersPerspective(
                        graphics,
                        horizonY,
                        hitboxY + 20f,
                        trackCenterX,
                        track_width,
                        minScale: 0.35,
                        maxScale: 1.00,
                        insetPx: 4,
                        stepY: 10
                    );
                    DrawWaitTimeRB(graphics, MIDITools.MIDI_Chart.Guitar.ChartedNotes, horizonY, hitboxY, trackCenterX);
                }
                DrawHitbox(graphics, chartVertical.Checked ? bmpHitbox : Resources.hitbox_5lane, guitarX, renderSize.Height - 52, track_width, 30, 0.90f, chartVertical.Checked ? "Guitar" : "");
                lastX += track_width + (2 * padding);
            }

            // Draw Keys or ProKeys track if present
            if (MIDITools.MIDI_Chart.Keys.ChartedNotes.Any() && doMIDIKeys)
            {
                var isSolo = MIDITools.MIDI_Chart.Keys.Solos != null && MIDITools.MIDI_Chart.Keys.Solos.Any(solo => solo.MarkerBegin <= PlaybackSeconds && solo.MarkerEnd > PlaybackSeconds);
                keysX = lastX + padding;
                if (chartVertical.Checked)
                {
                    float trackCenterX = keysX + (track_width / 2f);
                    graphics.DrawImage(isSolo ? bmpBackgroundKeysSolo : bmpBackgroundKeys, keysX, y, track_width, track_height);
                    DrawWaitTimeRB(graphics, MIDITools.MIDI_Chart.Keys.ChartedNotes, horizonY, hitboxY, trackCenterX);
                }
                else
                {
                    DrawInstrumentHitboxLabel(
                        graphics,
                        "KEYS",
                        keysX,
                        hitboxY,
                        track_width,
                        renderSize.Height - hitboxY,
                        renderSize
                    );
                    DrawTrackPerspectiveTrapezoidFilled(
                        graphics,
                        isSolo ? bmpBackgroundKeysSoloRB : bmpBackgroundKeysRB,
                        keysX,
                        y,
                        track_height,
                        track_width,
                        horizonY,
                        hitboxY + 20f,
                        strips: 120
                    );
                    float trackCenterX = keysX + (track_width / 2f);
                    
                    DrawHighwaySideBordersPerspective(
                        graphics,
                        horizonY,
                        hitboxY + 20f,
                        trackCenterX,
                        track_width,
                        minScale: 0.35,
                        maxScale: 1.00,
                        insetPx: 4,
                        stepY: 10
                    );
                    DrawWaitTimeRB(graphics, MIDITools.MIDI_Chart.Keys.ChartedNotes, horizonY, hitboxY, trackCenterX);
                }
                DrawHitbox(graphics, chartVertical.Checked ? bmpHitbox : Resources.hitbox_5lane, keysX, renderSize.Height - 52, track_width, 30, 0.90f, chartVertical.Checked ? "Keys" : "");                
            }
            else if (MIDITools.MIDI_Chart.ProKeys.ChartedNotes.Any() && doMIDIProKeys)
            {
                var isSolo = MIDITools.MIDI_Chart.ProKeys.Solos != null && MIDITools.MIDI_Chart.ProKeys.Solos.Any(solo => solo.MarkerBegin <= PlaybackSeconds && solo.MarkerEnd > PlaybackSeconds);
                proKeysX = lastX + padding;
                if (chartVertical.Checked)
                {
                    float trackCenterX = proKeysX + (track_width * 2 / 2f);
                    graphics.DrawImage(isSolo ? bmpBackgroundProKeysSolo : bmpBackgroundProKeys, proKeysX, y, track_width * 2, track_height); // Keys may take up more space
                    DrawWaitTimeRB(graphics, MIDITools.MIDI_Chart.ProKeys.ChartedNotes, horizonY, hitboxY, trackCenterX);
                }
                else
                {
                    DrawInstrumentHitboxLabel(
                        graphics,
                        "PRO KEYS",
                        proKeysX,
                        hitboxY,
                        track_width * 2,
                        renderSize.Height - hitboxY,
                        renderSize
                    );
                    DrawTrackPerspectiveTrapezoidFilled(
                        graphics, isSolo? bmpBackgroundProKeysSoloRB : bmpBackgroundProKeysRB,
                        proKeysX,
                        y,
                        track_height,
                        track_width * 2,
                        horizonY,
                        hitboxY,
                        strips: 120
                        );
                    float trackCenterX = proKeysX + (track_width * 2 / 2f);                    
                    DrawHighwaySideBordersPerspective(
                        graphics,
                        horizonY,
                        hitboxY,
                        trackCenterX,
                        track_width * 2,
                        minScale: 0.35,
                        maxScale: 1.00,
                        insetPx: 0,
                        stepY: 10
                    );
                    DrawWaitTimeRB(graphics, MIDITools.MIDI_Chart.ProKeys.ChartedNotes, horizonY, hitboxY, trackCenterX);
                }
                DrawHitbox(graphics, chartVertical.Checked ? bmpHitbox : Resources.pianokeys, proKeysX, renderSize.Height - 52, track_width * 2, 30, 0.90f, chartVertical.Checked ? "Pro Keys" : "");
            }

            if (MIDITools.MIDI_Chart.Drums.ChartedNotes.Any() && doMIDIDrums)
            {
                if (chartVertical.Checked)
                {
                    DrawFills(graphics, MIDITools.MIDI_Chart.Drums, GetStartingPosition(), drumsX, track_width);
                    DrawDrumNotes(graphics, true, GetStartingPosition(), drumsX, track_width);
                    DrawDrumNotes(graphics, false, GetStartingPosition(), drumsX, track_width);
                }
                else
                {
                    DrawFillsRB(graphics, MIDITools.MIDI_Chart.Drums, GetStartingPosition(), drumsX, track_width);
                    DrawDrumNotesRB(graphics, true, GetStartingPosition(), drumsX, track_width);
                    DrawDrumNotesRB(graphics, false, GetStartingPosition(), drumsX, track_width);
                }
            }
            if (MIDITools.MIDI_Chart.Bass.ChartedNotes.Any() && doMIDIBass)
            {
                if (chartVertical.Checked)
                {
                    DrawFills(graphics, MIDITools.MIDI_Chart.Bass, GetStartingPosition(), bassX, track_width);
                    DrawFiveLaneNotes(graphics, MIDITools.MIDI_Chart.Bass, GetStartingPosition(), bassX, track_width);
                }
                else
                {
                    DrawFillsRB(graphics, MIDITools.MIDI_Chart.Bass, GetStartingPosition(), bassX, track_width);
                    DrawFiveLaneNotesRB(graphics, MIDITools.MIDI_Chart.Bass, GetStartingPosition(), bassX, track_width);
                }
            }
            if (MIDITools.MIDI_Chart.Guitar.ChartedNotes.Any() && doMIDIGuitar)
            {
                if (chartVertical.Checked)
                {
                    DrawFills(graphics, MIDITools.MIDI_Chart.Guitar, GetStartingPosition(), guitarX, track_width);
                    DrawFiveLaneNotes(graphics, MIDITools.MIDI_Chart.Guitar, GetStartingPosition(), guitarX, track_width);
                }
                else
                {
                    DrawFillsRB(graphics, MIDITools.MIDI_Chart.Guitar, GetStartingPosition(), guitarX, track_width);
                    DrawFiveLaneNotesRB(graphics, MIDITools.MIDI_Chart.Guitar, GetStartingPosition(), guitarX, track_width);
                }
            }
            if (MIDITools.MIDI_Chart.Keys.ChartedNotes.Any() && doMIDIKeys && !doMIDIProKeys)
            {
                if (chartVertical.Checked)
                {
                    DrawFills(graphics, MIDITools.MIDI_Chart.Keys, GetStartingPosition(), keysX, track_width);
                    DrawFiveLaneNotes(graphics, MIDITools.MIDI_Chart.Keys, GetStartingPosition(), keysX, track_width);
                }
                else
                {
                    DrawFillsRB(graphics, MIDITools.MIDI_Chart.Keys, GetStartingPosition(), keysX, track_width);
                    DrawFiveLaneNotesRB(graphics, MIDITools.MIDI_Chart.Keys, GetStartingPosition(), keysX, track_width);
                }
            }
            if (MIDITools.MIDI_Chart.ProKeys.ChartedNotes.Any() && !doMIDIKeys && doMIDIProKeys)
            {
                if (chartVertical.Checked)
                {
                    DrawFills(graphics, MIDITools.MIDI_Chart.ProKeys, GetStartingPosition(), proKeysX, track_width * 2);
                    DrawProKeysNotes(graphics, GetStartingPosition(), proKeysX, track_width * 2);
                }
                else
                {
                    DrawFillsRB(graphics, MIDITools.MIDI_Chart.ProKeys, GetStartingPosition(), proKeysX, track_width * 2);
                    DrawProKeysNotesRB(graphics, GetStartingPosition(), proKeysX, track_width * 2);
                }
            }
            var Solo = doMIDIVocals && MIDITools.MIDI_Chart.Vocals.Solos != null && MIDITools.MIDI_Chart.Vocals.Solos.Any(solo => solo.MarkerBegin <= PlaybackSeconds && solo.MarkerEnd > PlaybackSeconds);
            if (!Solo && doMIDIHarmonies)
            {
                Solo = doMIDIVocals && MIDITools.MIDI_Chart.Harm1.Solos != null && MIDITools.MIDI_Chart.Harm1.Solos.Any(solo => solo.MarkerBegin <= PlaybackSeconds && solo.MarkerEnd > PlaybackSeconds);
            }
        }

        private void DrawWaitTimeTextRB(Graphics graphics, List<MIDINote> notes, float horizonY, float hitboxY, float trackCenterX, string text)
        {           
            float y = horizonY;

            // Big font (pixel units)
            using (var font = new Font("Segoe UI Semibold", 72f, FontStyle.Regular, GraphicsUnit.Pixel))
            using (var shadowBrush = new SolidBrush(Color.FromArgb(160, 0, 0, 0)))
            using (var textBrush = new SolidBrush(Color.WhiteSmoke))
            {
                var oldHint = graphics.TextRenderingHint;
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

                // Measure for true centering
                SizeF sz = graphics.MeasureString(text, font);

                float x = (trackCenterX - (sz.Width / 2f));
                float yy = y - (sz.Height / 2f);

                // Subtle shadow (RB/YARG-style)
                graphics.DrawString(text, font, shadowBrush, x, yy + 2f);
                graphics.DrawString(text, font, textBrush, x, yy);

                graphics.TextRenderingHint = oldHint;
            }
        }

        private void DrawWaitTimeRB(Graphics graphics, List<MIDINote> notes, float horizonY, float hitboxY, float trackCenterX)
        {
            var time = GetCorrectedTime();
            var nextNote = notes.FirstOrDefault(n => n.NoteStart > time);
            if (nextNote != null)
            {
                var wait = nextNote.NoteEnd - time;

                if (wait > 5.0)
                {
                    string text = wait.ToString("0");
                    DrawWaitTimeTextRB(graphics, notes, horizonY, hitboxY, trackCenterX, text);
                }
            }
        }

        private void DrawInstrumentHitboxLabel(
            Graphics g,
            string label,
            float leftX,
            float topY,
            float width,
            float height,
            Size renderSize,
            Font font = null,
            int panelAlpha = 255,         // 0..255 (background panel)
            int highlightAlpha = 180,     // 0..255 (top edge highlight)
            int shadowAlpha = 140         // 0..255 (text shadow)
        )
        {
            if (g == null) return;
            if (width <= 0 || height <= 0) return;

            // Clamp the panel to the render area (optional but keeps it safe)
            float h = height;
            if (renderSize.Width > 0 || renderSize.Height > 0)
            {
                if (topY < 0) { h += topY; topY = 0; }
                if (renderSize.Height > 0 && topY + h > renderSize.Height) h = renderSize.Height - topY;
                if (h <= 1f) return;
            }

            // Save state
            var oldSmoothing = g.SmoothingMode;
            var oldPix = g.PixelOffsetMode;
            var oldHint = g.TextRenderingHint;
            var oldComp = g.CompositingMode;
            var oldCompQ = g.CompositingQuality;

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.CompositingMode = CompositingMode.SourceOver;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            var rect = new RectangleF(leftX, topY, width, h);          
            g.DrawImage(Resources.textbg, rect);

            // --- Text (centered) ---
            bool disposeFont = false;
            if (font == null)
            {
                font = new Font("Segoe UI Semibold", 14f, FontStyle.Regular, GraphicsUnit.Pixel);
                disposeFont = true;
            }

            try
            {
                // Measure
                SizeF textSize = g.MeasureString(label, font);

                float textX = rect.Left + (rect.Width - textSize.Width) / 2f;
                float textY = (rect.Top + (rect.Height - textSize.Height) / 2f) + 15f;

                // Shadow (1px down)
                using (var shadowBrush = new SolidBrush(Color.FromArgb(shadowAlpha, 0, 0, 0)))
                using (var textBrush = new SolidBrush(Color.WhiteSmoke))
                {
                    g.DrawString(label, font, shadowBrush, textX, textY + 1f);
                    g.DrawString(label, font, textBrush, textX, textY);
                }
            }
            finally
            {
                if (disposeFont && font != null) font.Dispose();

                // Restore state
                g.SmoothingMode = oldSmoothing;
                g.PixelOffsetMode = oldPix;
                g.TextRenderingHint = oldHint;
                g.CompositingMode = oldComp;
                g.CompositingQuality = oldCompQ;
            }
        }

        private void DrawHighwaySideBordersPerspective(
            Graphics g,
            float horizonY,
            float hitboxY,
            float trackCenterX,
            float trackWidth,
            double minScale,
            double maxScale,
            int insetPx,
            int stepY = 6, // polygon sampling step (6-10 is usually plenty)            
            float baseThickness = 2.0f,
            float maxThickness = 6.0f,
            Color? borderColor = null,
            Color? highlightColor = null
        )
        {
            Color cBorder = borderColor ?? Color.DarkGray;
            Color cHi = highlightColor ?? Color.White;
            double Clamp01(double v) => v < 0 ? 0 : (v > 1 ? 1 : v);
            double Lerp(double a, double b, double t) => a + (b - a) * t;

            // y -> p(0..1) linear (NOT eased) for stable geometry
            double PFromY(float y)
            {
                double denom = (hitboxY - horizonY);
                if (denom <= 1) return 1.0;
                return Clamp01((y - horizonY) / denom);
            }

            void SpanAtY(float y, out double span, out double spanLeft)
            {
                double p = PFromY(y);
                double scale = Lerp(minScale, maxScale, p);
                span = trackWidth * scale;
                spanLeft = trackCenterX - (span / 2.0);
            }

            // Collect rail edge samples
            var leftInner = new List<PointF>();
            var leftOuter = new List<PointF>();
            var rightInner = new List<PointF>();
            var rightOuter = new List<PointF>();

            var leftHiInner = new List<PointF>();
            var leftHiOuter = new List<PointF>();
            var rightHiInner = new List<PointF>();
            var rightHiOuter = new List<PointF>();

            // Ensure we include hitboxY exactly
            float yEnd = hitboxY;
            float yStart = horizonY;

            for (float y = yStart; y <= yEnd; y += Math.Max(1, stepY))
            {
                SpanAtY(y, out double span, out double spanLeft);

                float leftX = (float)spanLeft + insetPx;
                float rightX = (float)(spanLeft + span) - insetPx;

                double p = PFromY(y);
                float t = (float)Lerp(baseThickness, maxThickness, p);

                // Border rails: inner edge sits on the highway edge,
                // outer edge expands outward.
                leftInner.Add(new PointF(leftX, y));
                leftOuter.Add(new PointF(leftX - t, y));

                rightInner.Add(new PointF(rightX, y));
                rightOuter.Add(new PointF(rightX + t, y));

                // Highlight rails (a thinner band just inside the border)
                float hiT = Math.Max(1f, t * 0.35f);

                // Left highlight is inside-left (to the right of left edge)
                leftHiInner.Add(new PointF(leftX + 1f, y));
                leftHiOuter.Add(new PointF(leftX + 1f + hiT, y));

                // Right highlight is inside-right (to the left of right edge)
                rightHiInner.Add(new PointF(rightX - 1f, y));
                rightHiOuter.Add(new PointF(rightX - 1f - hiT, y));
            }

            // If stepY skipped the exact hitboxY, force-add it
            if (leftInner.Count == 0 || leftInner[leftInner.Count - 1].Y < hitboxY)
            {
                float y = hitboxY;
                SpanAtY(y, out double span, out double spanLeft);

                float leftX = (float)spanLeft;
                float rightX = (float)(spanLeft + span);

                double p = PFromY(y);
                float t = (float)Lerp(baseThickness, maxThickness, p);
                float hiT = Math.Max(1f, t * 0.35f);

                leftInner.Add(new PointF(leftX, y));
                leftOuter.Add(new PointF(leftX - t, y));

                rightInner.Add(new PointF(rightX, y));
                rightOuter.Add(new PointF(rightX + t, y));

                leftHiInner.Add(new PointF(leftX + 1f, y));
                leftHiOuter.Add(new PointF(leftX + 1f + hiT, y));

                rightHiInner.Add(new PointF(rightX - 1f, y));
                rightHiOuter.Add(new PointF(rightX - 1f - hiT, y));
            }

            // Build a closed polygon from two polylines:
            // outer (top->bottom) + inner (bottom->top)
            PointF[] BuildRibbonPolygon(List<PointF> outer, List<PointF> inner)
            {
                if (outer.Count < 2 || inner.Count < 2) return Array.Empty<PointF>();

                var poly = new List<PointF>(outer.Count + inner.Count);

                poly.AddRange(outer);

                for (int i = inner.Count - 1; i >= 0; i--)
                    poly.Add(inner[i]);

                return poly.ToArray();
            }

            var oldSmooth = g.SmoothingMode;
            var oldPix = g.PixelOffsetMode;

            // AA just for rails
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            using (var bBorder = new SolidBrush(cBorder))
            using (var bHi = new SolidBrush(cHi))
            {
                // Border polygons
                var leftPoly = BuildRibbonPolygon(leftOuter, leftInner);
                var rightPoly = BuildRibbonPolygon(rightOuter, rightInner);

                if (leftPoly.Length > 0) g.FillPolygon(bBorder, leftPoly);
                if (rightPoly.Length > 0) g.FillPolygon(bBorder, rightPoly);

                // Highlight polygons (optional)
                var leftHiPoly = BuildRibbonPolygon(leftHiOuter, leftHiInner);
                var rightHiPoly = BuildRibbonPolygon(rightHiOuter, rightHiInner);

                if (leftHiPoly.Length > 0) g.FillPolygon(bHi, leftHiPoly);
                if (rightHiPoly.Length > 0) g.FillPolygon(bHi, rightHiPoly);
            }

            // Restore modes
            g.SmoothingMode = oldSmooth;
            g.PixelOffsetMode = oldPix;
        }

        private int GetStartingPosition()
        {
            var startingPosition = GetHeightDiff();
            if (doKaraokeLyrics || doStaticLyrics || doScrollingLyrics || rBStyle.Checked)
            {
                if (MIDITools.MIDI_Chart.Vocals.ChartedNotes.Any() || rBStyle.Checked)
                {
                    startingPosition += 20;
                    if (doHarmonyLyrics || rBStyle.Checked)
                    {
                        if (MIDITools.MIDI_Chart.Harm2.ChartedNotes.Any() || rBStyle.Checked)
                        {
                            startingPosition += 20;
                        }
                        if (MIDITools.MIDI_Chart.Harm3.ChartedNotes.Any() || rBStyle.Checked)
                        {
                            startingPosition += 20;
                        }
                    }
                }
            }
            if (doMIDINoVocals && !rBStyle.Checked)
            {
                startingPosition = 0;
            }
            return startingPosition;
        }              

        private void DrawPhraseMarkers(Graphics graphics, PhraseCollection phrases, int track_height, int track_y)
        {
            var renderSize = new Size(1920, 1080);

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
                if (phrases.Phrases[p].PhraseStart > time + (PlaybackWindowRBVocals * 1)) break; // Stop if beyond range
                if (phrases.Phrases[p].PhraseEnd < time) continue; // Skip if already passed

                // Calculate X position for the current phrase
                float normalizedTime = (float)((phrases.Phrases[p].PhraseStart - time) / PlaybackWindowRBVocals);
                var x = (float)(normalizedTime * (renderSize.Width - hitboxWidth)) + hitboxWidth;

                // Prevent "skipping" issues by clamping x within visible bounds
                if (x < 0) x = 0;
                if (x > renderSize.Width) x = renderSize.Width;

                // Ensure the marker appears and disappears at the correct locations
                if ((chartVertical.Checked || rBStyle.Checked || isRBKaraoke()) && x < HitboxVocalsX) continue; // Vertical mode: disappear after hitbox
                if (chartSnippet.Checked && x < 0) continue; // Snippet mode: skip if outside screen bounds

                // Adjust positions for vertical and non-vertical modes
                int top = isRBKaraoke() ? track_y + 4 : (chartVertical.Checked || rBStyle.Checked ? GetYForRBVocals() + 4: track_y - track_height + 4);
                int height = isRBKaraoke() ? (track_height - 8) : (chartVertical.Checked || rBStyle.Checked ? vocalsHeight - 8 : track_height - 8);
                const int width = 4;

                // Draw the phrase marker
                using (var solidBrush = new SolidBrush(Color.DarkGray))
                {
                    graphics.FillRectangle(solidBrush, x, top, width, height);
                }
            }
        }

        private void DrawTrackBackground(Graphics graphics, int y, int height, int index, string name, ICollection<SpecialMarker> solos, Instrument instrument)
        {
            var renderSize = new Size(1920, 1080);
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
                    graphics.FillRectangle(DrawingPen, 0, y - height, renderSize.Width, height);
                }
            }
            else
            {
                var color = index % 2 == 0 ? TrackBackgroundColor2 : TrackBackgroundColor1;
                using (var DrawingPen = new SolidBrush(chartVertical.Checked ? RBStyleVocalsBackgroundColor : color))
                {
                    var adjustedPosY = chartVertical.Checked ? y : y - height;
                    graphics.FillRectangle(DrawingPen, 0, adjustedPosY, renderSize.Width, height);
                }
            }
            var rectangle = new Rectangle(0, y - height, renderSize.Width, height);
            
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

            if (!doMIDINameTracks || chartVertical.Checked) return;
            TextRenderer.DrawText(graphics, trackText, font, rectangle, index % 2 == 0 ? TrackBackgroundColor1 : TrackBackgroundColor2, TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);            
        }

        private bool isRBKaraoke()
        {
            return rockBandKaraoke.Checked && displayKaraokeMode.Checked;
        }

        private readonly Bitmap needleHarm1 = Resources.needle_harm1;
        private readonly Bitmap needleHarm2 = Resources.needle_harm2;
        private readonly Bitmap needleHarm3 = Resources.needle_harm3;

        private void DrawNotes(Graphics graphics, MIDITrack track, int track_height, int track_y, bool drums, int harm, out int LastPlayedIndex)
        {
            LastPlayedIndex = track.ActiveIndex;
            var correctedTime = GetCorrectedTime();
            track_y++;
            track_height--;
            var needleY = 0f;
            var needleAdjustedHeight = 0f;
            var needleUnpitched = false;
            var drawNeedle = false;
            var renderSize = new Size(1920, 1080);
            var oldSmoothingMode = graphics.SmoothingMode;
            var oldCompositingQuality = graphics.CompositingQuality;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.CompositingQuality = CompositingQuality.HighQuality;

            var window = 0.00;
            switch (track.Name)
            {
                case "Vocals":
                case "Harm1":
                case "Harm2":
                case "Harm3":
                    window = PlaybackWindowRBVocals * (isRBKaraoke() && PlayingSong.BPM > 80.0 ? 80.0 / PlayingSong.BPM : 1.0);
                    break;
                default:
                    if (isRBKaraoke())
                    {
                        window = PlaybackWindowRBVocals * (isRBKaraoke() && PlayingSong.BPM > 80.0 ? 80.0 / PlayingSong.BPM : 1.0);
                    }
                    else
                    {
                        window = chartVertical.Checked || rBStyle.Checked ? PlaybackWindowRB : PlaybackWindow;
                    }
                    break;
            }

            // Filter notes to process only visible ones
            var filteredNotes = track.ChartedNotes.Where(note => note.NoteStart <= correctedTime + (window * 2)).ToList();

            for (var z = 0; z < filteredNotes.Count(); z++)
            {
                var note = filteredNotes[z];
                if (note.NoteEnd < correctedTime && !chartFull.Checked && !chartSnippet.Checked && !chartVertical.Checked && !rockBandKaraoke.Checked && !rBStyle.Checked) continue;
                if (note.NoteStart > correctedTime && !chartFull.Checked && !chartSnippet.Checked && !chartVertical.Checked && !rockBandKaraoke.Checked && !rBStyle.Checked) break;
                if ((chartSnippet.Checked || chartVertical.Checked || rBStyle.Checked || isRBKaraoke()) && note.NoteEnd < correctedTime - 1) continue;
                if ((chartSnippet.Checked || chartVertical.Checked || rBStyle.Checked || isRBKaraoke()) && note.NoteStart > correctedTime + (window * 2)) break;
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
                            note.NoteColor = isRBKaraoke() ? KaraokeModeHarm1Highlight : (!doMIDIHarm1onVocals ? GetNoteColor(note.NoteNumber) : Harm1Color);
                            break;
                        case 1:
                            note.NoteColor = isRBKaraoke() ? KaraokeModeHarm1Highlight : Harm1Color;
                            break;
                        case 2:
                            note.NoteColor = isRBKaraoke() ? KaraokeModeHarm2Highlight : Harm2Color;
                            break;
                        case 3:
                            note.NoteColor = isRBKaraoke() ? KaraokeModeHarm3Highlight : Harm3Color;
                            break;
                        default:
                            note.NoteColor = GetNoteColor(note.NoteNumber, drums);
                            break;
                    }
                }
                
                var note_width = ((note.NoteLength / (PlayingSong.Length / 1000.0)) * renderSize.Width);
                if (note_width < 1.0)
                {
                    note_width = 1.0;
                }

                var x = (note.NoteStart - correctedTime) / window * (float)renderSize.Width / 1.33f; //3 second equivalent for this mode, 2 second equivalent for game style mode

                // Specific logic for Vocals and Harmonies
                if (track.Name == "Vocals" || track.Name == "Harm1" || track.Name == "Harm2" || track.Name == "Harm3")
                {
                    var hitboxWidth = HitboxVocalsX + (bmpHitboxVocals.Width / 2);
                    if (chartVisualsToolStripMenuItem.Checked && chartSnippet.Checked)
                    {
                        hitboxWidth = 0;
                    }
                    x = (float)((note.NoteStart - correctedTime) / window * (renderSize.Width - hitboxWidth) + hitboxWidth);
                    
                    // Define vocal chart dimensions and note height
                    int vocalChartTop = isRBKaraoke() ? track_y : (chartVertical.Checked || rBStyle.Checked ? GetYForRBVocals() : track_y - track_height);
                    int vocalChartHeight = isRBKaraoke() ? vocalsHeight * 2 : (chartVertical.Checked || rBStyle.Checked ? vocalsHeight : track_height);
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
                        if (lyric.Start == note.NoteStart)
                        {
                            isUnpitched = lyric.Text.Trim().EndsWith("#");
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
                    else if (chartSnippet.Checked || chartVertical.Checked || rBStyle.Checked || isRBKaraoke())
                    {                        
                        width = ((note.NoteLength / window) * renderSize.Width) * 0.8;
                        if (width < 1)
                        {
                            width = 1; //won't draw something less than one pixel wide, and we want something to show!
                        }
                        var adjustedHeight = (float)noteHeight * 2;
                        var adjustedY = y;
                        var alpha = 255;
                        if (isUnpitched)
                        {
                            adjustedHeight = isRBKaraoke() ? vocalsHeight * 2 : (chartVertical.Checked || rBStyle.Checked ? vocalsHeight : track_height);
                            adjustedY = isRBKaraoke() ? track_y : (chartVertical.Checked || rBStyle.Checked ? GetYForRBVocals() : track_y - track_height);
                            alpha = 192;
                        }
                        if ((note.NoteNumber == 96 || note.NoteNumber == 97) && MIDITools.MIDI_Chart.UsesPercussion)
                        {
                            const float percHeight = 20f;
                            float percY = vocalChartTop + (vocalChartHeight - percHeight) / 2;
                            float x0 = (float)x;
                            float y0 = percY;
                            float d = percHeight;

                            if (awesomenessDetection.Checked)
                            {
                                graphics.DrawImage(note.NoteNumber == 96 ? Resources.cowbellb : Resources.cowbella, x0, y0, 50f, 50f);
                            }
                            else
                            {
                                var oldSmoothing = graphics.SmoothingMode;
                                var oldPixelOffset = graphics.PixelOffsetMode;
                                var oldCompositing = graphics.CompositingQuality;

                                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                                graphics.CompositingQuality = CompositingQuality.HighQuality;                                

                                using (var penOuter = new Pen(Color.FromArgb(160, Color.Black), 3.0f)) // darker + thicker
                                {
                                    penOuter.Alignment = PenAlignment.Center;
                                    graphics.DrawEllipse(penOuter, x0 + 0.5f, y0 + 0.5f, d - 1f, d - 1f);
                                }

                                using (var penInner = new Pen(Color.WhiteSmoke, 1.5f))
                                {
                                    penInner.Alignment = PenAlignment.Center;
                                    graphics.DrawEllipse(penInner, x0 + 0.5f, y0 + 0.5f, d - 1f, d - 1f);
                                }

                                graphics.SmoothingMode = oldSmoothing;
                                graphics.PixelOffsetMode = oldPixelOffset;
                                graphics.CompositingQuality = oldCompositing;
                                continue;
                            }
                        }
                        else
                        {
                            using (var solidBrush = new SolidBrush(Color.FromArgb(alpha, note.NoteColor)))
                            {
                                graphics.FillRectangle(solidBrush, (float)x, adjustedY, (float)width, adjustedHeight);
                            }
                            if ((chartVertical.Checked || rBStyle.Checked || isRBKaraoke()) && note.NoteStart < correctedTime)
                            {
                                //draw highlight - disabled for now
                                using (var glowBrush = new SolidBrush(note.NoteColor))
                                {
                                    graphics.FillRectangle(glowBrush, (float)x, adjustedY, (float)width, adjustedHeight);
                                }
                            }
                            if (!isUnpitched)
                            {
                                if ((chartVertical.Checked || rBStyle.Checked || isRBKaraoke()) && note.NoteStart < correctedTime)
                                {
                                    drawNeedle = true;
                                    needleY = adjustedY;
                                    needleAdjustedHeight = adjustedHeight;
                                    needleUnpitched = isUnpitched;
                                }
                            }
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
                            using (var enumerator = source?.Where(lyric => lyric.Start == nextNote.NoteStart).GetEnumerator())
                            {
                                if (enumerator?.MoveNext() == true)
                                {
                                    str = enumerator.Current.Text;
                                }
                            }

                            if (!string.IsNullOrEmpty(str) && str.Replace("-", "").Replace("$", "").Trim() == "+")
                            {
                                var x2 = x + width;
                                var x3 = (float)((nextNote.NoteStart - correctedTime) / window * (renderSize.Width - hitboxWidth) + hitboxWidth);
                                var y2 = y;
                                var y3 = vocalChartTop + (int)((maxNote - nextNote.NoteNumber) * noteHeight);

                                var pointF1 = new PointF((float)(x + width), (float)(y + (noteHeight * 2)));
                                var pointF2 = new PointF((float)(x + width), y);
                                var pointF3 = new PointF(x3, y3);
                                var pointF4 = new PointF(x3, (float)(y3 + (noteHeight * 2)));

                                // Define the base polygon
                                PointF[] basePolygon = new[] { pointF1, pointF2, pointF3, pointF4 };

                                using (var solidBrush = new SolidBrush(((track.Name == "Vocals" && doMIDIHarm1onVocals) || track.Name != "Vocals") ? note.NoteColor : Color.LightGray))
                                {
                                    graphics.FillPolygon(solidBrush, basePolygon);
                                }

                                if ((chartVertical.Checked || rBStyle.Checked || isRBKaraoke()) && note.NoteStart < correctedTime)
                                {
                                    using (var solidBrush = new SolidBrush(((track.Name == "Vocals" && doMIDIHarm1onVocals) || track.Name != "Vocals") ? note.NoteColor : Color.LightGray))
                                    {
                                        graphics.FillPolygon(new SolidBrush(note.NoteColor), basePolygon);
                                    }
                                }               
                            }
                        }
                        catch (Exception ex)
                        {}
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
                    var width = ((note.NoteLength / window) * renderSize.Width) * 0.8;
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
            }

            if (drawNeedle)
            {
                var needle = needleHarm1;
                if (track.Name == "Harm2")
                {
                    needle = needleHarm2;
                }
                else if (track.Name == "Harm3")
                {
                    needle = needleHarm3;
                }
                var needleHeight = needle.Height * 0.25f;
                var needleWidth = needle.Width * 0.25f;
                var needleX = HitboxVocalsX + (bmpHitboxVocals.Width / 2) - needleWidth;
                graphics.DrawImage(Resources.glow3, HitboxVocalsX - (needleWidth * 5f), needleY - (needleUnpitched ? 0 : needleAdjustedHeight), needleWidth * 5f, needleHeight);
                graphics.DrawImage(needle, needleX, needleY - (needleUnpitched ? 0 : needleAdjustedHeight), needleWidth, needleHeight);                
            }

            graphics.SmoothingMode = oldSmoothingMode;
            graphics.CompositingQuality = oldCompositingQuality;
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
            UpdateTime(false, !PlaybackTimer.Enabled);
            if (_mediaPlayer.State == VLCState.Playing || _mediaPlayer.State == VLCState.Paused)
            {
                _mediaPlayer.Time = (long)(PlaybackSeconds * 1000) + Parser.Songs[0].VideoStartTime;
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
                var display = new Art(Cursor.Position, CurrentSongArt);
                display.Show();
                return;
            }
            if (!displayAlbumArt.Checked && (File.Exists(CurrentSongArt) || displayAudioSpectrum.Checked)) return;
            SpectrumID++;
            picPreview.Image = null;
            Spectrum.ClearPeaks();
        }

        private void lstPlaylist_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            doSongPlayback();
        }
        
        private void doSongPlayback()
        {
            if (lstPlaylist.SelectedItems.Count != 1 || (GIFOverlay != null && !AlreadyTried)) return;
            if (songPreparer.IsBusy) return;
            randomizeBackgroundImage();
            doSongPreparer();
        }

        private void MoveSongFiles()
        {            
            if (yarg.Checked || fortNite.Checked || guitarHero.Checked)
            {
                CurrentSongArt = File.Exists(NextSongArtPNG) ? NextSongArtPNG : NextSongArtJPG;
                CurrentSongMIDI = NextSongMIDI;
                CurrentSongArtBlurred = NextSongArtBlurred;
                nautilus.PlayingSongOggData = nautilus.NextSongOggData;
                nautilus.NextSongOggData = new byte[0];
                nautilus.ReleaseStreamHandle(true);
                CurrentSongAudio = nautilus.PlayingSongOggData;
                return;
            }
            else
            {
                Tools.DeleteFile(CurrentSongArt);//delete left over from old song if this song doesn't have album art
                Tools.MoveFile(NextSongArtPNG, CurrentSongArt);
                Tools.DeleteFile(CurrentSongArtBlurred);
                Tools.MoveFile(NextSongArtBlurred, CurrentSongArtBlurred);
            }
                        
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
            var indexes = lstPlaylist.SelectedIndices;
            var savedIndex = lstPlaylist.SelectedIndices[0];
            var playing = Bass.BASS_ChannelIsActive(BassMixer) == BASSActive.BASS_ACTIVE_PLAYING;
            var to_remove = new List<int>();
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
            for (var i = to_remove.Count - 1; i >= 0; i--)
            {
                var song = Playlist[to_remove[i]];
                Playlist.Remove(song);
                StaticPlaylist.Remove(song);
            }
            txtSearch.Text = strSearchPlaylist;
            ReloadPlaylist(Playlist, true, true, false);
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
                    doSongPlayback();
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

        public void ReloadPlaylist(IList<Song> playlist, bool update = true, bool search = true, bool doExtract = true)
        {
            lstPlaylist.Items.Clear();
            lstPlaylist.Refresh();

            var searchTerm = txtSearch.Text;
            lstPlaylist.BeginUpdate();
            for (var i = 0; i < playlist.Count; i++)
            {
                var year = playlist[i].Year;
                var enabledRanges = new List<(int Start, int End)>();

                if (enable2020s) enabledRanges.Add((2020, 2100));
                if (enable2010s) enabledRanges.Add((2010, 2019));
                if (enable2000s) enabledRanges.Add((2000, 2009));
                if (enable1990s) enabledRanges.Add((1990, 1999));
                if (enable1980s) enabledRanges.Add((1980, 1989));
                if (enable1970s) enabledRanges.Add((1970, 1979));
                if (enable1960s) enabledRanges.Add((1960, 1969));
                if (enableOldies) enabledRanges.Add((1000, 1959));
                                
                bool yearAllowed = enabledRanges.Count == 0 || enabledRanges.Any(r => year >= r.Start && year <= r.End);

                if (!yearAllowed) continue;

                var genre = playlist[i].Genre;
                var language = playlist[i].Languages;
                HashSet<string> selectedGenres = null;
                HashSet<string> selectedLanguages = null;

                if (!string.IsNullOrWhiteSpace(genreFilter))
                {
                    selectedGenres = new HashSet<string>(genreFilter
                            .Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(g => g.Trim()), StringComparer.OrdinalIgnoreCase
                    );
                }
                if (selectedGenres != null && !selectedGenres.Contains(genre)) continue;

                if (!string.IsNullOrWhiteSpace(languageFilter))
                {
                    selectedLanguages = new HashSet<string>(
                        languageFilter.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries)
                                      .Select(s => s.Trim())
                                      .Where(s => s.Length > 0),
                        StringComparer.OrdinalIgnoreCase);
                }

                var lang = (language ?? "").Trim();

                if (selectedLanguages != null && selectedLanguages.Count > 0 && !selectedLanguages.Contains(lang)) continue;

                string Norm(string s) => (s ?? "").Trim().ToLowerInvariant();

                var passesInstrumentFilter = true;
                if (!string.IsNullOrWhiteSpace(instrumentFilter))
                {
                    var required = instrumentFilter.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(Norm).Where(x => x.Length > 0).ToHashSet();

                    if (required.Count > 0)
                    {
                        List<string> instrumentList = new List<string>();
                        if (playlist[i].ChannelsBass > 0)
                        {
                            instrumentList.Add("bass");
                        }
                        if (playlist[i].ChannelsDrums > 0)
                        {
                            instrumentList.Add("drums");
                        }
                        if (playlist[i].ChannelsGuitar > 0)
                        {
                            instrumentList.Add("guitar");
                        }
                        if (playlist[i].ChannelsKeys > 0)
                        {
                            instrumentList.Add("keys");
                            if (playlist[i].hasProKeys)
                            {
                                instrumentList.Add("pro keys");
                            }
                        }
                        if (playlist[i].ChannelsVocals > 0)
                        {
                            if (playlist[i].VocalParts == 2)
                            {
                                instrumentList.Add("2x harmonies");
                            }
                            else if (playlist[i].VocalParts == 3)
                            {
                                instrumentList.Add("3x harmonies");
                            }
                            else
                            {
                                instrumentList.Add("vocals");
                            }
                        }
                        var present = instrumentList.Select(Norm).ToHashSet();
                        passesInstrumentFilter = required.All(present.Contains);
                    }
                }
                if (!passesInstrumentFilter) continue;

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
                entry.BackColor = Color.AliceBlue;
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
            var item = lstPlaylist.SelectedItems[0];
            item.Tag = 0;
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
            MarkAsModified();
        }

        private void GetNextSong()
        {
            if (lstPlaylist.Items.Count == 0) return;
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
            StopAllVideoPlayback();
            InitiateGIFOverlay();
            songExtractor.RunWorkerAsync();
        }

        private void saveCurrentPlaylist_Click(object sender, EventArgs e)
        {
            SavePlaylist(false);
        }

        private void SavePlaylist(bool force_new)
        {
            var version = GetAppVersion();

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
                    sw.Write(song.PSDelay + "\t");
                    sw.Write(song.Languages + "\t");
                    sw.Write(song.VocalParts.ToString(CultureInfo.InvariantCulture));
                }
            }
            UpdateRecentPlaylists(PlaylistPath);
            Text = AppName + " - " + PlaylistName;
        }

        private void loadExistingPlaylist_Click(object sender, EventArgs e)
        {
            if (Text.Contains("*"))
            {
                if (MessageBox.Show("You have unsaved changes on the current playlist\nAre you sure you want to lose those changes?",
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
            var playlistInfoCount = 0;
            if (!File.Exists(PlaylistPath))
            {
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
                            PSDelay = song_info.Count() >= 26 ? Convert.ToInt16(song_info[25]) : 0,
                            //v2.1.2 added Languages and VocalParts                            
                            Languages = song_info.Count() >= 27 ? song_info[26].Replace(";Language(s)", "").Replace(",", "") : "Unknown",
                            VocalParts = song_info.Count() >= 28 ? Convert.ToInt16(song_info[27]) : -1
                        };
                        playlistInfoCount = song_info.Count();
                        if (File.Exists(song.Location))
                        {
                            Playlist.Add(song);
                        }
                    }
                    catch (Exception ex)
                    {
                        error = true;
                    }                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading that Playlist\nError: " + ex.Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            sr.Dispose();

            if (error)
            {
                if (Playlist.Any())
                {
                    var msg = "Some of the song entries in that playlist were corrupt or in a format I wasn't expecting\nPlease don't modify the playlist files manually\n\nI was able to recover " + Playlist.Count + (Playlist.Count == 1 ? " song" : " songs") + " :-)\n\nSee the log file to track down the problem song(s)";
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
                MessageBox.Show("Nothing could be loaded from that playlist", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            
            UpdateRecentPlaylists(PlaylistPath);
            StaticPlaylist = Playlist;
            ActiveSong = null;
            AnalyzePlaylist(Playlist);
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
            if (playlistInfoCount <= 26)
            {
                var result = MessageBox.Show("You are using an outdated Playlist format that is missing some of the newest features\n\nDo you want to rebuild your Playlist?", AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    doRebuildPlaylist(false);
                    return;
                }
            }
            if (lstPlaylist.Items.Count == 0 || songExtractor.IsBusy || !autoPlay.Checked) return;
            if (autoPlay.Checked && picShuffle.Tag.ToString() == "shuffle")
            {
                lstPlaylist.Items[0].Selected = false;
                lstPlaylist.Items[ShuffleSongs()].Selected = true;
            }
            doSongPlayback();
        }

        private void AnalyzePlaylist(List<Song> playlist)
        {
            enableFavorites = favoritesList.Any();

            foreach (var song in playlist)
            {
                if (song.Year >= 2020  && !enable2020s)
                {
                    enable2020s = true;
                }
                else if (song.Year >= 2010 && song.Year <= 2019 && !enable2010s)
                {
                    enable2010s = true;
                }
                else if (song.Year >= 2000 && song.Year <= 2009 && !enable2000s)
                {
                    enable2000s = true;
                }
                else if (song.Year >= 1990 && song.Year <= 1999 && !enable1990s)
                {
                    enable1990s = true;
                }
                else if (song.Year >= 1980 && song.Year <= 1989 && !enable1980s)
                {
                    enable1980s = true;
                }
                else if (song.Year >= 1970 && song.Year <= 1979 && !enable1970s)
                {
                    enable1970s = true;
                }
                else if (song.Year >= 1960 && song.Year <= 1969 && !enable1960s)
                {
                    enable1960s = true;
                }
                else if (song.Year < 1960 && !enableOldies)
                {
                    enableOldies = true;
                }
            }

            //picFavorites.Image = enableFavorites ? Resources.favorites_enabled : Resources.favorites_disabled;
            picFavorites.Cursor = enableFavorites ? Cursors.Hand : Cursors.No;

            //pic2020s.Image = enable2020s ? Resources._2020s_enabled : Resources._2020s_disabled;
            pic2020s.Cursor = enable2020s ? Cursors.Hand : Cursors.No;

            //pic2010s.Image = enable2010s ? Resources._2010s_enabled : Resources._2010s_disabled;
            pic2010s.Cursor = enable2010s ? Cursors.Hand : Cursors.No;

            //pic2000s.Image = enable2000s ? Resources._2000s_enabled : Resources._2000s_disabled;
            pic2000s.Cursor = enable2000s ? Cursors.Hand : Cursors.No;

            //pic1990s.Image = enable1990s ? Resources._1990s_enabled : Resources._1990s_disabled;
            pic1990s.Cursor = enable1990s ? Cursors.Hand : Cursors.No;

            //pic1980s.Image = enable1980s ? Resources._1980s_enabled : Resources._1980s_disabled;
            pic1980s.Cursor = enable1980s ? Cursors.Hand : Cursors.No;

            //pic1970s.Image = enable1970s ? Resources._1970s_enabled : Resources._1970s_disabled;
            pic1970s.Cursor = enable1970s ? Cursors.Hand : Cursors.No;

            //pic1960s.Image = enable1960s ? Resources._1960s_enabled : Resources._1960s_disabled;
            pic1960s.Cursor = enable1960s ? Cursors.Hand : Cursors.Default;

            //picOldies.Image = enableOldies ? Resources.oldies_enabled : Resources.oldies_disabled;
            picOldies.Cursor = enableOldies ? Cursors.Hand : Cursors.No;

            enableFavorites = false;
            enable2020s = false;
            enable2010s = false;
            enable2000s = false;
            enable1990s = false;
            enable1980s = false;
            enable1970s = false;
            enable1960s = false;
            enableOldies = false;
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
                sw.WriteLine("ShowMIDIVisuals=" + chartVisualsToolStripMenuItem.Checked);
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
                sw.WriteLine("KaraokeModeBackground=" + ColorTranslator.ToHtml(KaraokeModeBackgroundColor));
                sw.WriteLine("KaraokeModeLyric=" + ColorTranslator.ToHtml(KaraokeModeHarm1Text));
                sw.WriteLine("KaraokeModeHighlight=" + ColorTranslator.ToHtml(KaraokeModeHarm1Highlight));
                sw.WriteLine("KaraokeModeHarmony=" + ColorTranslator.ToHtml(KaraokeModeHarm2Text));
                sw.WriteLine("KaraokeModeHarmonyHighlight=" + ColorTranslator.ToHtml(KaraokeModeHarm2Highlight));
                sw.WriteLine("KaraokeModeHarmony2=" + ColorTranslator.ToHtml(KaraokeModeHarm3Text));
                sw.WriteLine("KaraokeModeHarmony2Highlight=" + ColorTranslator.ToHtml(KaraokeModeHarm3Highlight));
                sw.WriteLine("DoRockBandKaraokeMode=" + rockBandKaraoke.Checked);
                sw.WriteLine("DoClassicKaraokeMode=" + classicKaraokeMode.Checked);
                sw.WriteLine("DocPlayerStyleKaraoke=" + cPlayerStyle.Checked);
                sw.WriteLine("DoGameChartMode=" + chartVertical.Checked);
                sw.WriteLine("UseAnimatedBackground=" + animatedBackground.Checked);
                sw.WriteLine("UseStaticBackground=" + staticBackground.Checked);
                sw.WriteLine("UseSolidColorBackground=" + solidColorBackground.Checked);
                sw.WriteLine("UseStaticBackground2=" + staticBackground2.Checked);
                sw.WriteLine("UseAnimatedBackground2=" + animatedBackground2.Checked);
                sw.WriteLine("DoNoKeys=" + doMIDINoKeys);
                sw.WriteLine("DoNoVocals=" + doMIDINoVocals);
                sw.WriteLine("UseBackgroundVideos=" + useBackgroundVideos.Checked);
                sw.WriteLine("UseBackgroundImages=" + useBackgroundImages.Checked);
                sw.WriteLine("DoRockBandChartMode=" + rBStyle.Checked);
                sw.WriteLine("EnableAVSync=" + enableBTAVOffsetSync);
                sw.WriteLine("BTAVOffset=" + BTAVOffsetSync);
                sw.WriteLine("NautilusPath=" + nautilusPath);
            }
        }

        private void LoadFavorites()
        {
            var file = Application.StartupPath + "\\bin\\favorites";
            if (!File.Exists(file)) return;

            var sr = new StreamReader(file);
            var count = Convert.ToInt16(Tools.GetConfigString(sr.ReadLine()));
            for (var i = 0; i < count; i++)
            {
                var favorite = new FavoriteSong();
                favorite.SongPath = Tools.GetConfigString(sr.ReadLine());
                favorite.PlayTimes = Convert.ToInt16(Tools.GetConfigString(sr.ReadLine()));
                favoritesList.Add(favorite);
            }
            sr.Dispose();
            picFavorites.Cursor = Cursors.Hand;
        }

        private void UncheckAllModes()
        {
            displayAlbumArt.Checked = false;
            displayAudioSpectrum.Checked = false;
            displayKaraokeMode.Checked = false;
            classicKaraokeMode.Checked = false;
            cPlayerStyle.Checked = false;
            rockBandKaraoke.Checked = false;
            chartVisualsToolStripMenuItem.Checked = false;
            rBStyle.Checked = false;
            chartVertical.Checked = false;
            chartSnippet.Checked = false;
            chartFull.Checked = false;
        }

        private void LoadConfig()
        {
            LoadFavorites();
            if (!File.Exists(config))
            {
                return;
            }
            UncheckAllModes();

            var sr = new StreamReader(config);
            try
            {
                PlayerConsole = Tools.GetConfigString(sr.ReadLine());
                xbox360.Checked = false;
                pS3.Checked = false;
                rb4PS4.Checked = false;
                wii.Checked = false;
                yarg.Checked = false;
                rockSmith.Checked = false;
                guitarHero.Checked = false;
                fortNite.Checked = false;
                powerGig.Checked = false;
                bandFuse.Checked = false;
                chartSnippet.Checked = false;
                chartFull.Checked = false;
                nautilusToolStripMenuItem.Visible = true;
                setNautilusPath.Enabled = true;
                sendToVisualizer.Enabled = true;
                var enabled = false;
                switch (PlayerConsole)
                {
                    case "xbox":
                        xbox360.Checked = true;
                        consoleToolStripMenuItem.Text = "Game | Console: Rock Band 1/2/3 | Xbox 360";
                        enabled = true;
                        break;
                    case "ps3":
                        pS3.Checked = true;
                        consoleToolStripMenuItem.Text = "Game | Console: Rock Band 1/2/3 | PlayStation 3";
                        break;
                    case "wii":
                        wii.Checked = true;
                        consoleToolStripMenuItem.Text = "Game | Console: Rock Band 1/2/3 | Wii";
                        break;
                    case "ps4":
                        rb4PS4.Checked = true;
                        consoleToolStripMenuItem.Text = "Game | Console: Rock Band 4 | PlayStation 4";
                        break;
                    case "yarg":
                        yarg.Checked = true;
                        consoleToolStripMenuItem.Text = "Game | Console: YARG / Clone Hero / Fret Smasher | PC";
                        break;
                    case "rocksmith":
                        rockSmith.Checked = true;
                        consoleToolStripMenuItem.Text = "Game | Console: Rocksmith 2014 | PC";
                        break;
                    case "guitarhero":
                        guitarHero.Checked = true;
                        consoleToolStripMenuItem.Text = "Game | Console: GHWT:DE | PC";
                        break;
                    case "fortnite":
                        fortNite.Checked = true;
                        consoleToolStripMenuItem.Text = "Game | Console: Fortnite Festival | PC";
                        break;
                    case "bandfuse":
                        bandFuse.Checked = true;
                        consoleToolStripMenuItem.Text = "Game | Console: BandFuse | Xbox 360";
                        break;
                    case "powergig":
                        powerGig.Checked = true;
                        consoleToolStripMenuItem.Text = "Game | Console: Power Gig | PC";
                        break;
                }
                sendToFileAnalyzer.Enabled = enabled;
                sendToAudioAnalyzer.Enabled = enabled;
                sendToCONExplorer.Enabled = enabled;
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
                chartVisualsToolStripMenuItem.Checked = sr.ReadLine().Contains("True");
                sr.ReadLine(); //openSideWindow.Checked = sr.ReadLine().Contains("True");
                VolumeLevel = Convert.ToDouble(Tools.GetConfigString(sr.ReadLine()));
                sr.ReadLine(); //no longer need this
                sr.ReadLine(); //no longer need this
                sr.ReadLine(); //no longer need this
                sr.ReadLine(); //no longer need this
                sr.ReadLine(); //no longer need this
                chartFull.Checked = sr.ReadLine().Contains("True");
                chartSnippet.Checked = sr.ReadLine().Contains("True") && chartVisualsToolStripMenuItem.Checked;
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
                selectBackgroundColor.Visible = displayKaraokeMode.Checked;
                selectLyricColor.Visible = displayKaraokeMode.Checked;
                selectHighlightColor.Visible = displayKaraokeMode.Checked;
                restoreDefaultsToolStripMenuItem.Visible = displayKaraokeMode.Checked;
                skipIntroOutroSilence.Checked = sr.ReadLine().Contains("True");
                SilenceThreshold = float.Parse(Tools.GetConfigString(sr.ReadLine()));
                FadeLength = Convert.ToDouble(Tools.GetConfigString(sr.ReadLine()));
                KaraokeModeBackgroundColor = ColorTranslator.FromHtml(Tools.GetConfigString(sr.ReadLine()));
                KaraokeModeHarm1Text = ColorTranslator.FromHtml(Tools.GetConfigString(sr.ReadLine()));
                KaraokeModeHarm1Highlight = ColorTranslator.FromHtml(Tools.GetConfigString(sr.ReadLine()));
                KaraokeModeHarm2Text = ColorTranslator.FromHtml(Tools.GetConfigString(sr.ReadLine()));
                KaraokeModeHarm2Highlight = ColorTranslator.FromHtml(Tools.GetConfigString(sr.ReadLine()));
                KaraokeModeHarm3Text = ColorTranslator.FromHtml(Tools.GetConfigString(sr.ReadLine()));
                KaraokeModeHarm3Highlight = ColorTranslator.FromHtml(Tools.GetConfigString(sr.ReadLine()));
                rockBandKaraoke.Checked = sr.ReadLine().Contains("True") && chartVisualsToolStripMenuItem.Checked;
                classicKaraokeMode.Checked = sr.ReadLine().Contains("True");
                cPlayerStyle.Checked = sr.ReadLine().Contains("True");
                chartVertical.Checked = sr.ReadLine().Contains("True") && chartVisualsToolStripMenuItem.Checked;
                animatedBackground.Checked = sr.ReadLine().Contains("True");
                staticBackground.Checked = sr.ReadLine().Contains("True");
                solidColorBackground.Checked = sr.ReadLine().Contains("True");
                staticBackground2.Checked = sr.ReadLine().Contains("True");
                animatedBackground2.Checked = sr.ReadLine().Contains("True");
                doMIDINoKeys = sr.ReadLine().Contains("True");
                doMIDINoVocals = sr.ReadLine().Contains("True");
                useBackgroundVideos.Checked = sr.ReadLine().Contains("True");
                useBackgroundImages.Checked = sr.ReadLine().Contains("True") && !useBackgroundVideos.Checked;
                doUseBackgroundVideos = useBackgroundVideos.Checked;
                doUseBackgroundImages = useBackgroundImages.Checked;
                rBStyle.Checked = sr.ReadLine().Contains("True");
                enableBTAVOffsetSync = sr.ReadLine().Contains("True");
                BTAVOffsetSync = Convert.ToInt16(Tools.GetConfigString(sr.ReadLine()));
                nautilusPath = Tools.GetConfigString(sr.ReadLine());
                ValidateNautilusPath();
            }
            catch (Exception ex)
            { }

            if (solidColorBackground.Checked)
            {
                animatedBackground2.Checked = false;
                staticBackground2.Checked = false;
                picVisuals.Image = Resources.gradient;
            }
            if (animatedBackground2.Checked)
            {
                staticBackground2.Checked = false;
                solidColorBackground.Checked = false;
            }
            if (staticBackground2.Checked)
            {
                solidColorBackground.Checked = false;
                animatedBackground2.Checked = false;
            }
            if (staticBackground.Checked)
            {
                animatedBackground.Checked = false;
            }
            if (animatedBackground.Checked)
            {
                staticBackground.Checked = false;
            }

            sr.Dispose();       
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var version = GetAppVersion();
            var message = AppName + " - The Rhythm Game Music Player\nVersion: " + version + "\n© TrojanNemo, 2014-2026\n\n";
            var credits = Tools.ReadHelpFile("credits");
            videoOverlay.TopMost = false;
            MessageBox.Show(message + credits + "\n\n***Just For Fun***", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
            videoOverlay.TopMost = true;
        }

        private void picVolume_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            var Volume = new Volume(this, new Point(panelPlaying.Width - 100, Cursor.Position.Y));
            Volume.Show();
        }

        public void UpdateVolume(double volume)
        {
            if (PlayingSong == null) return;
            var track_vol = (float)Utils.DBToLevel(Convert.ToDouble(-1 * volume), 1.0);
            Bass.BASS_ChannelSetAttribute(BassMixer, BASSAttribute.BASS_ATTRIB_VOL, track_vol);
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
            GetNextSong();
        }

        private enum PlaylistFilters
        {
            ByArtist, ByAlbum, ByGenre
        }

        private void FilterPlaylist(PlaylistFilters filter)
        {
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
                        break;
                    case PlaylistFilters.ByAlbum:
                        Playlist.Sort((a, b) => a.Track.CompareTo(b.Track));
                        break;
                    case PlaylistFilters.ByGenre:
                        Playlist.Sort((a, b) => String.CompareOrdinal(a.Artist.ToLowerInvariant(), b.Artist.ToLowerInvariant()));
                        break;
                }
            }
            txtSearch.Text = strSearchPlaylist;
            ReloadPlaylist(Playlist, true, true, false);
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
            if (xbox360.Checked)
            {
                NextSong.yargPath = "";
                loadCON(NextSong.Location, false, false, true);
            }
            else if (yarg.Checked)
            {
                if (Path.GetExtension(NextSong.Location) == ".yargsong")
                {
                    sngPath = NextSong.Location;
                    loadINI(NextSong.Location, false, false, true);
                }
                else if (Path.GetExtension(NextSong.Location) == ".sng")
                {
                    NextSong.yargPath = "";
                    sngPath = NextSong.Location;
                    loadSNG(NextSong.Location, false, false, true);
                }
                else if (Path.GetFileName(NextSong.Location) == "songs.dta")
                {
                    pkgPath = "";
                    loadDTA(NextSong.Location, false, false, true);
                }
                else
                {
                    NextSong.yargPath = "";
                    loadINI(NextSong.Location, false, false, true);
                }
            }
            else if (rockSmith.Checked)
            {
                loadPSARC(NextSong.Location, false, false, true);
            }
            else if (powerGig.Checked)
            {
                ExtractXMA(NextSong.Location, false, false, true);
            }
            else if (bandFuse.Checked)
            {
                BandFusePath = NextSong.Location;
                ExtractBandFuse(NextSong.Location, false, false, true);
            }
            else if (fortNite.Checked)
            {
                loadINI(NextSong.Location, false, false, true);
            }
            else if (guitarHero.Checked)
            {
                ghwtPath = NextSong.Location;
                loadGHWT(NextSong.Location, false, false, true);
            }
            else
            {
                if (pS3.Checked && Path.GetExtension(NextSong.Location) == ".pkg")
                {
                    pkgPath = NextSong.Location;
                    loadPKG(NextSong.Location, false, false, true);
                }
                else
                {
                    pkgPath = "";
                    NextSong.yargPath = "";
                    loadDTA(NextSong.Location, false, false, true);
                }
            }            
        }

        private string DecryptExtractYARG(string inFile, bool message = false, bool scanning = true, bool next = false, bool prep = false)
        {
            byte[] SNGPKG = { (byte)'S', (byte)'N', (byte)'G', (byte)'P', (byte)'K', (byte)'G' };
            var tempFolder = Application.StartupPath + "\\temp";
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
            isScanning = batchSongLoader.IsBusy || songLoader.IsBusy;
            UpdateNotifyTray();
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
                doSongPlayback();
                return;
            }
            PlaybackTimer.Enabled = false;
            MoveSongFiles();
            PrepareForPlayback();
            UpdateHighlights();
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {            
            if (WindowState != FormWindowState.Minimized)
            {
                if (Width != 412)
                {
                    Width = 412;
                }
                if (Height < 400)
                {
                    Height = 400;
                }
                if (lastWindowState == WindowState) return;
                lastWindowState = WindowState;                
            }         
            if (WindowState == FormWindowState.Maximized)
            {
                openSideWindow.Checked = true;
                UpdateDisplay();
                UpdateStats();                
                FormBorderStyle = FormBorderStyle.FixedSingle;
                if (!PlaybackTimer.Enabled)
                {
                    picVisuals.Image = Resources.logo;
                }

            }
            else if (WindowState == FormWindowState.Normal)
            {
                openSideWindow.Checked = false;
                UpdateDisplay();
                UpdateStats();                
                FormBorderStyle = FormBorderStyle.FixedSingle;                
            }
            UpdateOverlayPosition();
            videoView.Height = picVisuals.Height - GetHeightDiff();
            videoView.Width = picVisuals.Width;            
            if (WindowState != FormWindowState.Minimized) return;
            NotifyTray.ShowBalloonTip(250);
            Hide();
            if (secondScreen != null)
            {
                secondScreen.Hide();
            }
        }

        private void NotifyTray_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            doClickNotifyTray();
        }

        private void doClickNotifyTray()
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Show();
                if (secondScreen != null)
                {
                    secondScreen.Show();
                }
                WindowState = lastWindowState;
                Activate();
                UpdateHighlights();
                try
                {
                    if (VideoIsPlaying)
                    {
                        _mediaPlayer.Play();
                        if (_mediaPlayer.IsSeekable)
                        {
                            _mediaPlayer.Time = (long)(PlaybackSeconds * 1000) + Parser.Songs[0].VideoStartTime;
                        }
                    }
                    if (secondScreen != null)
                    {
                        if (secondScreen.VideoIsPlaying)
                        {
                            secondScreen._mediaPlayer.Play();
                            if (secondScreen._mediaPlayer.IsSeekable)
                            {
                                secondScreen._mediaPlayer.Time = (long)(PlaybackSeconds * 1000) + Parser.Songs[0].VideoStartTime;
                            }
                        }
                    }
                }
                catch
                { }
            }
            else
            {
                // dispose render buffers
                _renderedFrame?.Dispose(); _renderedFrame = null;
                _scaledFrame?.Dispose(); _scaledFrame = null;

                // dispose big backgrounds
                RBStyleBackground?.Dispose(); RBStyleBackground = null;

                try
                {
                    VideoIsPlaying = _mediaPlayer.State == VLCState.Playing;
                    _mediaPlayer.Stop();

                    if (secondScreen != null)
                    {
                        secondScreen.VideoIsPlaying = secondScreen._mediaPlayer.State == VLCState.Playing;
                        secondScreen._mediaPlayer.Stop();
                    }
                }
                catch
                {}
                WindowState = FormWindowState.Minimized;
            }
        }

        private void restoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            doClickNotifyTray();
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
            SortingStyle = sort;
            switch (SortingStyle)
            {
                case PlaylistSorting.BySongArtist:
                    Playlist.Sort((a, b) => String.CompareOrdinal(a.Artist.ToLowerInvariant() + " - " + a.Name.ToLowerInvariant(), b.Artist.ToLowerInvariant() + " - " + b.Name.ToLowerInvariant()));
                    break;
                case PlaylistSorting.BySongName:
                    Playlist.Sort((a, b) => String.CompareOrdinal(a.Name.ToLowerInvariant() + " - " + a.Artist.ToLowerInvariant(), b.Name.ToLowerInvariant() + " - " + b.Artist.ToLowerInvariant()));
                    break;
                case PlaylistSorting.BySongDuration:
                    Playlist.Sort((a, b) => a.Length.CompareTo(b.Length));
                    break;
                case PlaylistSorting.ByModifiedDate:
                    Playlist.Sort((a, b) => File.GetLastWriteTimeUtc(a.Location).CompareTo(File.GetLastWriteTimeUtc(b.Location)));
                    Playlist.Reverse();
                    break;
                case PlaylistSorting.Shuffle:
                    Shuffle(Playlist);
                   break;
            }
            ReloadPlaylist(Playlist, true, true, false);
            txtSearch.Text = strSearchPlaylist;
            UpdateHighlights();
            MarkAsModified();
        }

        private void ShowUpdate(string update)
        {
            UpdateTimer.Stop();
            statusLabel.Text = update;
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
            var renderSize = new Size(1920, 1080);
            return (int)(renderSize.Height * 0.05);
        }

        public int GetKaraokeNextLineTop()
        {
            var renderSize = new Size(1920, 1080);
            return (int)(renderSize.Height * 0.95);
        }

        public static (string line1, string line2) SplitLineForKaraoke(Graphics g, string fullText, Font font, int maxWidth)
        {
            var words = fullText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length == 0) return ("", "");

            List<string> line1Words = new List<string>();
            List<string> line2Words = new List<string>();

            string testLine = "";
            foreach (var word in words)
            {
                string tempLine = (testLine == "") ? word : testLine + " " + word;
                var size = TextRenderer.MeasureText(g, tempLine, font);

                if (size.Width <= maxWidth)
                {
                    testLine = tempLine;
                }
                else
                {
                    break;
                }
            }

            line1Words = testLine.Split(' ').ToList();
            line2Words = words.Skip(line1Words.Count).ToList();

            return (
                string.Join(" ", line1Words),
                string.Join(" ", line2Words)
            );
        }

        void DrawAnimatedNotes(Graphics graphics, int noteCounter, int spawnFrequency, int screenWidth, int screenHeight)
        {
            string[] musicNotes = new[] { "🎵", "🎶", "♫", "♬" };
            int multiplier = 1;
            Color[] colors = new[]
            {
            Color.FromArgb(255, 255, 105, 97),   // pastel red
            Color.FromArgb(255, 97, 168, 255),   // light blue
            Color.FromArgb(255, 144, 238, 144),  // light green
            Color.FromArgb(255, 255, 222, 89),   // light yellow
            Color.FromArgb(255, 255, 179, 255),  // soft pink
            Color.FromArgb(255, 189, 255, 255),  // soft cyan
            Color.FromArgb(255, 255, 255, 255),  // white fallback
    };

            // Spawn new notes at interval
            if (noteCounter % spawnFrequency == 0)
            {
                var fontFamily = new FontFamily("Segoe UI Emoji");
                for (int i = 0; i < 5; i++) // fewer per spawn for smoother effect
                {
                    string note = musicNotes[rand.Next(musicNotes.Length)];
                    float fontSize = rand.Next(20, 40);
                    float x = rand.Next(screenWidth);
                    float y = rand.Next(screenHeight);
                    Color baseColor = colors[rand.Next(colors.Length)];
                    int alpha = rand.Next(140, 200);
                    Color finalColor = Color.FromArgb(alpha, baseColor.R, baseColor.G, baseColor.B);

                    activeNotes.Add(new AnimatedNote
                    {
                        Note = note,
                        X = x,
                        Y = y,
                        FontSize = fontSize,
                        Color = finalColor,
                        Lifetime = 3 * 30 // ~3 seconds
                    });
                }
            }
            // Draw and update active notes
            var fontFamilyLive = new FontFamily("Segoe UI Emoji");
            for (int i = activeNotes.Count - 1; i >= 0; i--)
            {
                var n = activeNotes[i];
                using (var font = new Font(fontFamilyLive, n.FontSize * multiplier, FontStyle.Bold))
                using (var brush = new SolidBrush(n.Color))
                {
                    graphics.DrawString(n.Note, font, brush, n.X, n.Y);
                }

                n.Lifetime--;
                if (n.Lifetime <= 0)
                    activeNotes.RemoveAt(i);
            }
        }
        class AnimatedNote
        {
            public string Note;
            public float X, Y;
            public float FontSize;
            public Color Color;
            public int Lifetime; // in frames
        }

        private static RectangleF MeasureTight(Graphics g, string text, Font font)
        {
            using (var fmt = (StringFormat)StringFormat.GenericTypographic.Clone())
            {
                fmt.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;

                // Big layout box to measure within
                var layout = new RectangleF(0, 0, 10000, 1000);

                // Measure the full string as one range
                fmt.SetMeasurableCharacterRanges(new[] { new CharacterRange(0, text.Length) });

                var regions = g.MeasureCharacterRanges(text, font, layout, fmt);
                return regions[0].GetBounds(g);
            }
        }

        private List<AnimatedNote> activeNotes = new List<AnimatedNote>();
        private Random rand = new Random();
        private bool doSoloVocals = false;
        private bool doHarm2 = false;
        private bool doHarm3 = true;
        private double highlightDelay = 1.5;
        private double timeGap = 5.0;
        private bool doShowAnimatedNotes = true;
        private bool doEnableHighlightAnimation = true;
        private bool doShowLoadingBar = true;
        private const string loadingBarXL = "████████████████████████████████";
        const int spawnFrequency = 30;
        private int noteCounter = spawnFrequency;

        private void DoModernKaraoke(Size screenSize, Graphics graphics, IList<LyricPhrase> vocalPhrases, IEnumerable<Lyric> vocalLyrics, 
            IList<LyricPhrase> harm1Phrases, IEnumerable<Lyric> harm1Lyrics, 
            IList<LyricPhrase> harm2Phrases, IEnumerable<Lyric> harm2Lyrics, 
            IList<LyricPhrase> harm3Phrases, IEnumerable<Lyric> harm3Lyrics)
        {
            var time = GetCorrectedTime();
            var AvgBPM = PlayingSong.BPM;
            const int spawnFrequency = 30;
            noteCounter++;
            int resolutionX = screenSize.Width;
            int resolutionY = screenSize.Height;
            int multiplier = 1;
            double vertOffset = 0;         
            int coverWidth = 512 * multiplier;
            int coverHeight = 512 * multiplier;

            doSoloVocals = forceSoloVocals.Checked || !harm2Lyrics.Any();
            doHarm2 = !doSoloVocals || forceTwoPartHarmonies.Checked;
            doHarm3 = !forceSoloVocals.Checked && !forceTwoPartHarmonies.Checked && harm3Lyrics.Any();

            try
            {
                if (staticBackground2.Checked)
                {
                    graphics.DrawImage(stageBackground, 0, 0, resolutionX, resolutionY);
                }
                else
                {
                    if (secondScreen != null)
                    {
                        secondScreen.ChangeBackgroundColor(KaraokeModeBackgroundColor);
                        picVisuals.BackColor = Color.AliceBlue;
                    }
                    else
                    {
                        picVisuals.BackColor = KaraokeModeBackgroundColor;
                    }
                }

                LyricPhrase actualNextLineHarmony = null;
                LyricPhrase actualLastLineHarmony = null;
                LyricPhrase currentLineLead = null;
                LyricPhrase nextLineLead = null;
                LyricPhrase lastLineLead = null;
                LyricPhrase actualLastLineLead = null;
                LyricPhrase actualNextLineLead = null;
                bool hasInlineGap = false;

                var phrasesLead = harm2Lyrics.Any() && (doHarm2 || doHarm3) ? harm1Phrases : vocalPhrases;
                var lyricsLead = harm2Lyrics.Any() && (doHarm2 || doHarm3) ? harm1Lyrics : vocalLyrics;

                double previewTime = time + highlightDelay;
                int lastPhraseIndex = 0;
                bool phrase2IsDup = false;

                for (var i = lastPhraseIndex; i < phrasesLead.Count(); i++)
                {
                    i = lastPhraseIndex;
                    if (i < 0) continue;
                    if (i >= phrasesLead.Count()) break;
                    lastLineLead = lastPhraseIndex > 0 ? phrasesLead[lastPhraseIndex - 1] : null;
                    var phrase1 = phrasesLead[lastPhraseIndex];
                    var phrase2 = phrase1; //for harmonies and when there's a gap, only show one phrase per page

                    var harmonies = (doHarm2 || doHarm3) && harm2Lyrics.Any();//there's no case of Harm3Lyrics without Harm2Lyrics

                    phrase2IsDup = false;
                    if (lastPhraseIndex < phrasesLead.Count() - 1)
                    {
                        var currentIndex = lastPhraseIndex + 1;
                        if (phrasesLead[currentIndex].PhraseStart - phrase1.PhraseEnd < timeGap && !harmonies)
                        {
                            phrase2 = phrasesLead[currentIndex];
                            currentIndex++;
                        }
                        else
                        {
                            phrase2IsDup = true;
                        }
                        if (phrase2.PhraseEnd <= time)
                        {
                            actualLastLineLead = phrase2; //whether phrase1 or phrase2 based on assignment above
                            if (currentIndex <= phrasesLead.Count() - 1)
                            {
                                actualNextLineLead = phrasesLead[currentIndex];
                            }
                        }
                    }
                    else
                    {
                        phrase2IsDup = true; //last phrase, must be null for nextLine;
                    }
                    if (previewTime >= phrase1.PhraseStart && time < phrase2.PhraseEnd)
                    {
                        try
                        {
                            var gap = phrase2.PhraseStart - phrase1.PhraseEnd >= timeGap && !harmonies;
                            if (gap)
                            {
                                if (hasInlineGap && time > phrase1.PhraseEnd)
                                {
                                    currentLineLead = null;
                                    nextLineLead = phrase2; ;
                                }
                                else
                                {
                                    currentLineLead = phrase1;
                                    nextLineLead = null;
                                    hasInlineGap = true;
                                }
                                vertOffset = 1.5;
                            }
                            else
                            {
                                currentLineLead = phrase1;
                                nextLineLead = phrase2IsDup ? null : phrase2;
                                hasInlineGap = false;
                                vertOffset = phrase2IsDup ? 1.5 : 0.0;
                            }
                        }
                        catch { }
                        break;
                    }
                    if (harmonies || phrase2IsDup)
                    {
                        lastPhraseIndex++;
                    }
                    else
                    {
                        lastPhraseIndex += 2;
                    }
                }
                if (actualNextLineLead == null)
                {
                    actualNextLineLead = phrasesLead.FirstOrDefault(p => !string.IsNullOrEmpty(p.PhraseText) && p.PhraseStart > previewTime);
                }
                if (actualLastLineLead == null)
                {
                    actualLastLineLead = phrasesLead.LastOrDefault(p => !string.IsNullOrEmpty(p.PhraseText) && p.PhraseEnd <= previewTime);
                }

                LyricPhrase currentLineHarm2 = null;
                LyricPhrase lastLineHarm2 = null;
                if (doHarm2 || doHarm3)
                {
                    for (var i = 0; i < harm2Phrases.Count(); i++)
                    {
                        var phrase = harm2Phrases[i];
                        lastLineHarm2 = i > 0 ? harm2Phrases[i - 1] : null;

                        if (phrase.PhraseEnd <= time)
                        {
                            actualLastLineHarmony = harm2Phrases[i];
                            if (i < harm2Phrases.Count() - 1)
                            {
                                actualNextLineHarmony = harm2Phrases[i + 1];
                            }
                        }

                        if (previewTime >= phrase.PhraseStart && time < phrase.PhraseEnd)
                        {
                            currentLineHarm2 = phrase;
                            break;
                        }
                    }
                    if (actualNextLineHarmony == null)
                    {
                        actualNextLineHarmony = harm2Phrases.FirstOrDefault(p => !string.IsNullOrEmpty(p.PhraseText) && p.PhraseStart > previewTime);
                    }
                    if (actualLastLineHarmony == null)
                    {
                        actualLastLineHarmony = harm2Phrases.LastOrDefault(p => !string.IsNullOrEmpty(p.PhraseText) && p.PhraseEnd <= previewTime);
                    }
                }

                LyricPhrase currentLineHarm3 = null;
                LyricPhrase lastLineHarm3 = null;
                if (doHarm3)
                {
                    for (var i = 0; i < harm3Phrases.Count(); i++)
                    {
                        var phrase = harm3Phrases[i];
                        lastLineHarm3 = i > 0 ? harm3Phrases[i - 1] : null;

                        if (previewTime >= phrase.PhraseStart && time < phrase.PhraseEnd)
                        {
                            currentLineHarm3 = phrase;
                            break;
                        }
                    }
                }

                var lineHeight = resolutionY / 11;
                var harm1LineTop1 = 0; ;
                var harm1LineTop2 = 0;
                var harm1LineTop3 = 0;
                var harm1LineTop4 = 0;
                var harm2LineTop1 = 0;
                var harm2LineTop2 = 0;
                var harm3LineTop1 = 0;
                var harm3LineTop2 = 0;

                if (doSoloVocals || !harm2Lyrics.Any()) //do solo vocals
                {
                    harm1LineTop1 = (int)(lineHeight * (2.5 + vertOffset));
                    harm1LineTop2 = (int)(lineHeight * (4.0 + vertOffset));
                    harm1LineTop3 = (int)(lineHeight * 5.5);
                    harm1LineTop4 = (int)(lineHeight * 7.0);
                }
                if (doHarm3 && harm3Lyrics.Any())
                {
                    harm1LineTop1 = lineHeight * 0;
                    harm1LineTop2 = (int)(lineHeight * 1.5);
                    harm2LineTop1 = lineHeight * 4;
                    harm2LineTop2 = (int)(lineHeight * 5.5);
                    harm3LineTop1 = lineHeight * 8;
                    harm3LineTop2 = (int)(lineHeight * 9.5);
                }
                else if ((doHarm2 || doHarm3) && harm2Lyrics.Any())
                {
                    harm1LineTop1 = lineHeight * 2;
                    harm1LineTop2 = (int)(lineHeight * 3.5);
                    harm2LineTop1 = lineHeight * 6;
                    harm2LineTop2 = (int)(lineHeight * 7.5);
                }

                if (time + highlightDelay < phrasesLead.First().PhraseStart)
                {
                    var title = "\"" + PlayingSong.Name.Replace("&", "&&").Replace("feat.", "ft.").Replace("featuring", "ft.") + "\"";
                    var artist = PlayingSong.Artist.Replace("&", "&&").Replace("feat.", "ft.").Replace("featuring", "ft.");
                    var album = PlayingSong.Album.Replace("&", "&&");
                    var bpm = AvgBPM == 0 ? "" : "🎚️ Tempo: " + Math.Round(AvgBPM, 0, MidpointRounding.AwayFromZero) + " BPM";
                    var parts = 1;
                    if ((doHarm2 || doHarm3) && harm2Lyrics.Any())
                    {
                        parts++;
                    }
                    if (doHarm3 && harm3Lyrics.Any())
                    {
                        parts++;
                    }
                    var vocalParts = "🎙️ Vocals: " + ((doHarm2 || doHarm3) && harm2Lyrics.Any() ? parts + "-part harmony" : "Solo");                    
                    var songKey = "";//GetSongKey(); - need to add detection of official HMX stuff vs customs before this is usable
                    var genre = Parser.doGenre(Parser.Songs[0].Genre).Replace("&", "&&");
                    if (!string.IsNullOrEmpty(genre))
                    {
                        genre = "🎧 Genre: " + genre;
                    }

                    //optimal quality settings only for the title card
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

                    var offset = 0;
                    if (OriginalAlbumArt != null)
                    {
                        int artSize = 512 * multiplier;
                        int spacer = 100 * multiplier;

                        int outlineThickness = 5 * multiplier;

                        int x = spacer;
                        int y = (resolutionY - artSize) / 2;

                        // Draw white outline (background rectangle)
                        using (var outlineBrush = new SolidBrush(Color.White))
                        {
                            graphics.FillRectangle(
                                outlineBrush,
                                x - outlineThickness,
                                y - outlineThickness,
                                artSize + outlineThickness * 2,
                                artSize + outlineThickness * 2
                            );
                        }

                        // Draw album art on top
                        graphics.DrawImage(OriginalAlbumArt, x, y, artSize, artSize);

                        offset = artSize + (int)(1.5 * spacer);
                    }
                    var charter = PlayingSong.Charter.Replace("&", "&&");
                    if (!string.IsNullOrEmpty(charter))
                    {
                        charter = $"✍️ Charted by {charter}";
                    }
                    else
                    {
                        charter = "";
                    }                                       

                    // 1–3: Title, Artist, Album (same as now)
                    DrawCenteredLine(graphics, title, resolutionX, lineHeight * 3, 72f * multiplier, offset);
                    DrawCenteredLine(graphics, artist, resolutionX, lineHeight * 4, 60f * multiplier, offset);
                    DrawCenteredLine(graphics, album, resolutionX, lineHeight * 5, 48f * multiplier, offset);

                    // 4: Genre
                    if (!string.IsNullOrEmpty(genre))
                        DrawCenteredLine(graphics, genre, resolutionX, lineHeight * 7, 32f * multiplier, offset);

                    // 5: Vocals
                    DrawCenteredLine(graphics, vocalParts, resolutionX, (int)(lineHeight * 7.7), 32f * multiplier, offset);

                    // 6: Key
                    if (!string.IsNullOrEmpty(songKey))
                        DrawCenteredLine(graphics, songKey, resolutionX, (int)(lineHeight * 8.4), 32f * multiplier, offset);

                    // 7: BPM
                    if (!string.IsNullOrEmpty(bpm))
                        DrawCenteredLine(graphics, bpm, resolutionX, (int)(lineHeight * (string.IsNullOrEmpty(songKey) ? 8.4 : 9.1)), 32f * multiplier, offset);

                    // 8: Charter
                    if (!string.IsNullOrEmpty(charter))
                        DrawCenteredLine(graphics, charter, resolutionX, (int)(lineHeight * (string.IsNullOrEmpty(songKey) ? 9.1 : 9.8)), 32f * multiplier, offset);
                    return;
                }

                double GetFirstLyricStart(IEnumerable<Lyric> lyrics, double phraseStart, double phraseEnd)
                {
                    return lyrics
                        .Where(lyr => lyr.Start >= phraseStart && lyr.Start <= phraseEnd)
                        .OrderBy(lyr => lyr.Start)
                        .Select(lyr => lyr.Start)
                        .FirstOrDefault(); // returns 0.0 if none found

                }
                double GetLastLyricEnd(IEnumerable<Lyric> lyrics, double phraseStart, double phraseEnd)
                {
                    return lyrics
                        .Where(lyr => lyr.End >= phraseStart && lyr.Start <= phraseEnd)
                        .OrderByDescending(lyr => lyr.End)
                        .Select(lyr => lyr.End)
                        .LastOrDefault(); // returns 0.0 if none found
                }
                // where JoinWordsForDisplay merges hyphen/sustain chains into visible words:
                IEnumerable<string> JoinWordsForDisplay(List<Lyric> syls)
                {
                    var words = new List<string>();
                    var buf = new List<Lyric>();
                    for (int i = 0; i < syls.Count; i++)
                    {
                        buf.Add(syls[i]);
                        string t = syls[i].Text ?? "";
                        bool endWord = !(t.EndsWith("-") || t == "+" ||
                                        (i + 1 < syls.Count && syls[i + 1].Text == "+"));
                        if (endWord)
                        {
                            string word = string.Join("", buf.Select(b => (b.Text ?? "")
                                .Replace("+", "").Replace("-", "").Replace("‿", " "))).Trim();
                            if (word.Length > 0) words.Add(word);
                            buf.Clear();
                        }
                    }
                    if (buf.Count > 0)
                    {
                        string tail = string.Join("", buf.Select(b => (b.Text ?? "")
                            .Replace("+", "").Replace("-", "").Replace("‿", " "))).Trim();
                        if (tail.Length > 0) words.Add(tail);
                    }
                    return words;
                }

                UpdateTextQuality(graphics);
                var drewText = false;
                var baseFont = new Font("Arial", 24f);
                if ((currentLineLead != null && !string.IsNullOrEmpty(currentLineLead.PhraseText)) ||
                    (nextLineLead != null && !string.IsNullOrEmpty(nextLineLead.PhraseText)))
                {
                    if ((currentLineLead != null && !string.IsNullOrEmpty(currentLineLead.PhraseText)))
                    {
                        var phraseSyllables = lyricsLead
                        .Where(s => s.End > currentLineLead.PhraseStart && s.Start <= currentLineLead.PhraseEnd)
                        .OrderBy(s => s.Start).ToList();

                        string rawPhraseText = string.Join(" ", phraseSyllables
                        .Where(s => !string.IsNullOrWhiteSpace(s.Text) && s.Text != "+" && s.Text != "-")
                        .Select(s => s.Text.Replace("‿", " ")));
                                             
                        var (line1Syllables, line2Syllables) = SplitSyllablesByPixelWidth(phraseSyllables, baseFont, graphics);

                        // For display strings:
                        string line1Text = string.Join(" ", JoinWordsForDisplay(line1Syllables));
                        string line2Text = string.Join(" ", JoinWordsForDisplay(line2Syllables));

                        string widestLine = (line1Text.Length > line2Text.Length) ? line1Text : line2Text;
                        float scaledFontSize = GetScaledFontSize(graphics, widestLine, baseFont, 100f * multiplier, resolutionX);
                        var displayFont = new Font(baseFont.FontFamily, scaledFontSize);                                            

                        RectangleF tight = MeasureTight(graphics, line1Text.Replace("‿", " "), displayFont);                                               
                        float posXf = (resolutionX - tight.Width) / 2f - tight.Left;
                        float posX = posXf;

                        double minGapToShow = 5.0;     // only show if the pause is at least this long
                        double leadInSeconds = 1.0;    // animate during the last X seconds before lyricStart

                        double firstLyricTime = GetFirstLyricStart(lyricsLead, currentLineLead.PhraseStart, currentLineLead.PhraseEnd);
                        double lastLyricTime = lastLineLead != null
                            ? GetLastLyricEnd(lyricsLead, lastLineLead.PhraseStart, lastLineLead.PhraseEnd)
                            : 0.0;

                        double timeUntilNextPhrase = firstLyricTime - time;   // countdown to next phrase
                        double totalGapDuration = firstLyricTime - lastLyricTime;

                        // Only animate if the *gap* is long enough, and we're in the lead-in window
                        bool gapIsLongEnough = totalGapDuration >= minGapToShow;
                        bool inLeadInWindow = timeUntilNextPhrase >= 0.0 && timeUntilNextPhrase <= leadInSeconds;

                        if (gapIsLongEnough && inLeadInWindow && doEnableHighlightAnimation &&
                            !string.IsNullOrEmpty(line1Text) && line1Syllables.Count > 0)
                        {
                            DrawHighlightAnimation(
                                graphics,
                                displayFont,
                                lyricStart: firstLyricTime,
                                textStartX: posX,
                                y: harm1LineTop1,
                                color: KaraokeModeHarm1Highlight,
                                time: time,
                                leadInSeconds: leadInSeconds
                            );
                        }

                        DrawSyllableAccurateLine(
                            graphics,
                            line1Syllables,
                            displayFont,
                            resolutionX,
                            harm1LineTop1,
                            KaraokeModeHarm1Text,
                            KaraokeModeHarm1Highlight,
                            time
                        );

                        DrawSyllableAccurateLine(
                            graphics,
                            line2Syllables,
                            displayFont,
                            resolutionX,
                            harm1LineTop2,
                            KaraokeModeHarm1Text,
                            KaraokeModeHarm1Highlight,
                            time
                        );
                    }

                    if ((doSoloVocals || !harm2Lyrics.Any()) && nextLineLead != null && !string.IsNullOrEmpty(nextLineLead.PhraseText))
                    {
                        var phraseSyllables = lyricsLead
                        .Where(s => s.End > nextLineLead.PhraseStart && s.Start <= nextLineLead.PhraseEnd)
                        .OrderBy(s => s.Start).ToList();

                        string rawPhraseText = string.Join(" ", phraseSyllables
                        .Where(s => !string.IsNullOrWhiteSpace(s.Text) && s.Text != "+" && s.Text != "-")
                        .Select(s => s.Text.Replace("‿", " ")));
                                  
                        // Build phraseSyllables (time-windowed, ordered).
                        var (line3Syllables, line4Syllables) = SplitSyllablesByPixelWidth(phraseSyllables, baseFont, graphics);

                        // For display strings:
                        string line3Text = string.Join(" ", JoinWordsForDisplay(line3Syllables));
                        string line4Text = string.Join(" ", JoinWordsForDisplay(line4Syllables));

                        string widestLine = (line3Text.Length > line4Text.Length) ? line3Text : line4Text;
                        float scaledFontSize = GetScaledFontSize(graphics, widestLine, baseFont, 100f * multiplier, resolutionX);
                        var displayFont = new Font(baseFont.FontFamily, scaledFontSize);

                        RectangleF tight = MeasureTight(graphics, line3Text.Replace("‿", " "), displayFont);
                        float posXf = (resolutionX - tight.Width) / 2f - tight.Left;
                        float posX = posXf;

                        double minGapToShow = 5.0;     // only show if the pause is at least this long
                        double leadInSeconds = 1.0;    // animate during the last X seconds before lyricStart

                        double firstLyricTime = GetFirstLyricStart(lyricsLead, nextLineLead.PhraseStart, nextLineLead.PhraseEnd);
                        double lastLyricTime = lastLineLead != null
                            ? GetLastLyricEnd(lyricsLead, nextLineLead.PhraseStart, nextLineLead.PhraseEnd)
                            : 0.0;

                        double timeUntilNextPhrase = firstLyricTime - time;   // countdown to next phrase
                        double totalGapDuration = firstLyricTime - lastLyricTime;

                        // Only animate if the *gap* is long enough, and we're in the lead-in window
                        bool gapIsLongEnough = totalGapDuration >= minGapToShow;
                        bool inLeadInWindow = timeUntilNextPhrase >= 0.0 && timeUntilNextPhrase <= leadInSeconds;

                        if (gapIsLongEnough && inLeadInWindow && doEnableHighlightAnimation &&
                            !string.IsNullOrEmpty(line3Text) && line3Syllables.Count > 0)
                        {
                            DrawHighlightAnimation(
                                graphics,
                                displayFont,
                                lyricStart: firstLyricTime,
                                textStartX: posX,
                                y: harm1LineTop3,
                                color: KaraokeModeHarm1Highlight,
                                time: time,
                                leadInSeconds: leadInSeconds
                            );
                        }                        

                        DrawSyllableAccurateLine(
                            graphics,
                            line3Syllables,
                            displayFont,
                            resolutionX,
                            harm1LineTop3,
                            KaraokeModeHarm1Text,
                            KaraokeModeHarm1Highlight,
                            time
                        );

                        DrawSyllableAccurateLine(
                            graphics,
                            line4Syllables,
                            displayFont,
                            resolutionX,
                            harm1LineTop4,
                            KaraokeModeHarm1Text,
                            KaraokeModeHarm1Highlight,
                            time
                        );
                    }
                    drewText = true;
                }

                if ((doHarm2 || doHarm3) && currentLineHarm2 != null && !string.IsNullOrEmpty(currentLineHarm2.PhraseText) && harm2Lyrics != null)
                {
                    var phraseSyllables = harm2Lyrics
                        .Where(s => s.End > currentLineHarm2.PhraseStart && s.Start <= currentLineHarm2.PhraseEnd)
                        .OrderBy(s => s.Start).ToList();

                    string rawPhraseText = string.Join(" ", phraseSyllables
                    .Where(s => !string.IsNullOrWhiteSpace(s.Text) && s.Text != "+" && s.Text != "-")
                    .Select(s => s.Text.Replace("‿", " ")));
                                        
                    // Build phraseSyllables (time-windowed, ordered).
                    var (line1Syllables, line2Syllables) = SplitSyllablesByPixelWidth(phraseSyllables, baseFont, graphics);

                    // For display strings:
                    string line1Text = string.Join(" ", JoinWordsForDisplay(line1Syllables));
                    string line2Text = string.Join(" ", JoinWordsForDisplay(line2Syllables));

                    string widestLine = (line1Text.Length > line2Text.Length) ? line1Text : line2Text;
                    float scaledFontSize = GetScaledFontSize(graphics, widestLine, baseFont, 100f * multiplier, resolutionX);
                    var displayFont = new Font(baseFont.FontFamily, scaledFontSize);

                    RectangleF tight = MeasureTight(graphics, line1Text.Replace("‿", " "), displayFont);
                    float posXf = (resolutionX - tight.Width) / 2f - tight.Left;
                    float posX = posXf;

                    double minGapToShow = 5.0;     // only show if the pause is at least this long
                    double leadInSeconds = 1.0;    // animate during the last X seconds before lyricStart

                    double firstLyricTime = GetFirstLyricStart(harm2Lyrics, currentLineHarm2.PhraseStart, currentLineHarm2.PhraseEnd);
                    double lastLyricTime = lastLineHarm2 != null
                        ? GetLastLyricEnd(harm2Lyrics, lastLineHarm2.PhraseStart, lastLineHarm2.PhraseEnd)
                        : 0.0;

                    double timeUntilNextPhrase = firstLyricTime - time;
                    double totalGapDuration = firstLyricTime - lastLyricTime;

                    // Only animate if the *gap* is long enough, and we're in the lead-in window
                    bool gapIsLongEnough = totalGapDuration >= minGapToShow;
                    bool inLeadInWindow = timeUntilNextPhrase >= 0.0 && timeUntilNextPhrase <= leadInSeconds;

                    if (gapIsLongEnough && inLeadInWindow && doEnableHighlightAnimation &&
                        !string.IsNullOrEmpty(line1Text) && line1Syllables.Count > 0)
                    {
                        DrawHighlightAnimation(
                            graphics,
                            displayFont,
                            lyricStart: firstLyricTime,
                            textStartX: posX,
                            y: harm2LineTop1,
                            color: KaraokeModeHarm2Highlight,
                            time: time,
                            leadInSeconds: leadInSeconds
                        );
                    }

                    DrawSyllableAccurateLine(
                            graphics,
                            line1Syllables,
                            displayFont,
                            resolutionX,
                            harm2LineTop1,
                            KaraokeModeHarm2Text,
                            KaraokeModeHarm2Highlight,
                            time
                        );

                    DrawSyllableAccurateLine(
                        graphics,
                        line2Syllables,
                        displayFont,
                        resolutionX,
                        harm2LineTop2,
                        KaraokeModeHarm2Text,
                        KaraokeModeHarm2Highlight,
                        time
                    );
                    drewText = true;
                }

                if (doHarm3 && currentLineHarm3 != null && !string.IsNullOrEmpty(currentLineHarm3.PhraseText) && harm3Lyrics != null)
                {
                    var phraseSyllables = new List<Lyric>();
                    try
                    {
                        phraseSyllables = harm3Lyrics
                            .Where(s => s.End > currentLineHarm3.PhraseStart && s.Start <= currentLineHarm2.PhraseEnd)
                            .OrderBy(s => s.Start).ToList();
                    }
                    catch { }

                    string rawPhraseText = string.Join(" ", phraseSyllables
                    .Where(s => !string.IsNullOrWhiteSpace(s.Text) && s.Text != "+" && s.Text != "-")
                    .Select(s => s.Text.Replace("‿", " ")));
                                        
                    // Build phraseSyllables (time-windowed, ordered).
                    var (line1Syllables, line2Syllables) = SplitSyllablesByPixelWidth(phraseSyllables, baseFont, graphics);

                    // For display strings:
                    string line1Text = string.Join(" ", JoinWordsForDisplay(line1Syllables));
                    string line2Text = string.Join(" ", JoinWordsForDisplay(line2Syllables));

                    string widestLine = (line1Text.Length > line2Text.Length) ? line1Text : line2Text;
                    float scaledFontSize = GetScaledFontSize(graphics, widestLine, baseFont, 100f * multiplier, resolutionX);
                    var displayFont = new Font(baseFont.FontFamily, scaledFontSize);

                    RectangleF tight = MeasureTight(graphics, line1Text.Replace("‿", " "), displayFont);
                    float posXf = (resolutionX - tight.Width) / 2f - tight.Left;
                    float posX = posXf;

                    double minGapToShow = 5.0;     // only show if the pause is at least this long
                    double leadInSeconds = 1.0;    // animate during the last X seconds before lyricStart

                    double firstLyricTime = GetFirstLyricStart(harm3Lyrics, currentLineHarm3.PhraseStart, currentLineHarm3.PhraseEnd);
                    double lastLyricTime = lastLineHarm3 != null
                        ? GetLastLyricEnd(harm3Lyrics, lastLineHarm3.PhraseStart, lastLineHarm3.PhraseEnd)
                        : 0.0;

                    double timeUntilNextPhrase = firstLyricTime - time;
                    double totalGapDuration = firstLyricTime - lastLyricTime;

                    // Only animate if the *gap* is long enough, and we're in the lead-in window
                    bool gapIsLongEnough = totalGapDuration >= minGapToShow;
                    bool inLeadInWindow = timeUntilNextPhrase >= 0.0 && timeUntilNextPhrase <= leadInSeconds;

                    if (gapIsLongEnough && inLeadInWindow && doEnableHighlightAnimation &&
                        !string.IsNullOrEmpty(line1Text) && line1Syllables.Count > 0)
                    {
                        DrawHighlightAnimation(
                            graphics,
                            displayFont,
                            lyricStart: firstLyricTime,
                            textStartX: posX,
                            y: harm3LineTop1,
                            color: KaraokeModeHarm3Highlight,
                            time: time,
                            leadInSeconds: leadInSeconds
                        );
                    }                    

                    DrawSyllableAccurateLine(
                            graphics,
                            line1Syllables,
                            displayFont,
                            resolutionX,
                            harm3LineTop1,
                            KaraokeModeHarm3Text,
                            KaraokeModeHarm3Highlight,
                            time
                     );

                    DrawSyllableAccurateLine(
                        graphics,
                        line2Syllables,
                        displayFont,
                        resolutionX,
                        harm3LineTop2,
                        KaraokeModeHarm3Text,
                        KaraokeModeHarm3Highlight,
                        time
                    );
                    displayFont.Dispose();
                    drewText = true;
                }
                baseFont.Dispose();
                if (drewText) return;

                if (time > phrasesLead.Last().PhraseEnd)
                {
                    lineHeight = resolutionY / 11;
                    int logoX = (resolutionX - Resources.karaoke_outro.Width) / 2;
                    int logoY = (resolutionY - Resources.karaoke_outro.Height) / 2;
                    graphics.DrawImage(Resources.karaoke_outro, logoX, logoY, Resources.karaoke_outro.Width, Resources.karaoke_outro.Height);
                    DrawCenteredLine(graphics, "www.nemosnautilus.com", resolutionX, lineHeight * 10, 24f * multiplier);
                    return;
                }

                try
                {
                    if (!doShowLoadingBar) return;
                    double? LastEnd = 0.0;
                    if (actualLastLineLead?.PhraseEnd > actualLastLineHarmony?.PhraseEnd)
                    {
                        LastEnd = actualLastLineLead?.PhraseEnd;
                    }
                    else
                    {
                        LastEnd = actualLastLineHarmony?.PhraseEnd;
                    }
                    //fallback
                    if (LastEnd == null)
                    {
                        if (actualLastLineLead != null)
                        {
                            LastEnd = actualLastLineLead.PhraseEnd;
                        }
                        else if (actualLastLineHarmony != null)
                        {
                            LastEnd = actualLastLineHarmony.PhraseEnd;
                        }
                        else
                        {
                            return;
                        }
                    }
                    double? NextStart = 0.0;
                    if (actualNextLineLead?.PhraseStart < actualNextLineHarmony?.PhraseStart)
                    {
                        NextStart = actualNextLineLead?.PhraseStart;
                    }
                    else
                    {
                        NextStart = actualNextLineHarmony?.PhraseStart;
                    }
                    //fallback
                    if (NextStart == null)
                    {
                        if (actualNextLineLead != null)
                        {
                            NextStart = actualNextLineLead.PhraseStart;
                        }
                        else if (actualNextLineHarmony != null)
                        {
                            NextStart = actualNextLineHarmony.PhraseStart;
                        }
                        else
                        {
                            return;
                        }
                    }

                    var gap = NextStart - LastEnd;
                    var wait = NextStart - previewTime;

                    if (gap >= timeGap && wait > 0)
                    {
                        baseFont = new Font("Arial", 24f * multiplier);
                        var lineSize = TextRenderer.MeasureText(loadingBarXL, baseFont);
                        var posX = (resolutionX - lineSize.Width) / 2;
                        TextRenderer.DrawText(graphics, loadingBarXL, baseFont, new Point(posX, (resolutionY - lineSize.Height) / 2), KaraokeModeHarm1Text, Color.Transparent);

                        var scaledLoadingBar = loadingBarXL.Substring(0, loadingBarXL.Length - (int)(loadingBarXL.Length * (wait / gap)));
                        TextRenderer.DrawText(graphics, scaledLoadingBar, baseFont, new Point(posX, (resolutionY - lineSize.Height) / 2), KaraokeModeHarm1Highlight, Color.Transparent);

                        if (doShowAnimatedNotes)
                        {
                            DrawAnimatedNotes(graphics, noteCounter, spawnFrequency, resolutionX, resolutionY);
                        }
                        baseFont.Dispose();
                    }
                }
                catch { }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error: " + ex.Message + " \n" + ex.StackTrace);
            }  
        }

        public static List<MergedSyllable> MergeSustainedSyllables(List<Lyric> input)
        {
            var merged = new List<MergedSyllable>();
            int i = 0;

            while (i < input.Count)
            {
                Lyric s = input[i];
                string text = s.Text;
                double start = s.Start;
                double end = s.End;

                // Handle prefix with dash (e.g., "ma-")
                if (text.EndsWith("-"))
                {
                    text = text.Substring(0, text.Length - 1);
                    i++;

                    // Merge any sustain "+" symbols
                    while (i < input.Count && input[i].Text == "+")
                    {
                        end = input[i].End;
                        i++;
                    }

                    // Merge the next syllable if it exists and isn't "+"
                    if (i < input.Count && input[i].Text != "+" && input[i].Text != "-")
                    {
                        text += input[i].Text;
                        end = input[i].End;
                        i++;
                    }

                    merged.Add(new MergedSyllable
                    {
                        Lyric = text,
                        Start = start,
                        End = end
                    });
                }
                else if (text == "+" || text == "-")
                {
                    // Ignore standalone + or - symbols
                    i++;
                }
                else
                {
                    // Normal case: single syllable
                    i++;

                    // Extend end time with any consecutive "+"
                    while (i < input.Count && input[i].Text == "+")
                    {
                        end = input[i].End;
                        i++;
                    }

                    merged.Add(new MergedSyllable
                    {
                        Lyric = text,
                        Start = start,
                        End = end
                    });
                }
            }

            return merged;
        }

        public class MergedSyllable
        {
            public string Lyric { get; set; }
            public double Start { get; set; }
            public double End { get; set; }
            public float Width { get; set; }
        }

        private string GetVisibleTextForSyllable(MergedSyllable s)
        {
            // Make this mirror the logic in ReconstructPhraseTextFromSyllables
            // as closely as possible.
            var raw = s.Lyric?.Trim() ?? string.Empty;
            var clean = CleanSyllable(raw);
            return clean.Replace("‿", " ");
        }

        public void DrawSyllableAccurateLine(
            Graphics g,
            List<Lyric> syllablesForThisLine,
            Font font,
            int resolutionX,
            int y,
            Color baseColor,
            Color highlightColor,
            double adjustedTime)
        {
            if (syllablesForThisLine.Count == 0)
                return;

            var merged = MergeSustainedSyllables(syllablesForThisLine);

            string displayText = ReconstructPhraseTextFromSyllables(merged);
            
            ApplyTextRenderingSettings(g);
            
            SizeF visualSizeF = g.MeasureString(displayText, font);

            int textWidth = (int)Math.Ceiling((double)visualSizeF.Width);
            int textHeight = (int)Math.Ceiling((double)visualSizeF.Height);

            int posX = (resolutionX - textWidth) / 2;

            var pixelmap = BuildSyllablePixelMap(merged, font, g, displayText, textWidth);

            float highlightWidth = GetHighlightedPixelWidth(pixelmap, adjustedTime);

            highlightWidth = Math.Max(0f, Math.Min(highlightWidth, textWidth));

            Color strokeCol = Color.Black;

            DrawTextWithStroke(
                g,
                displayText,
                font,
                new Point(posX, y),
                baseColor,
                strokeCol,
                3
            );

            //using (Bitmap bmp = new Bitmap(visualSize.Width, visualSize.Height))
            using (Bitmap bmp = new Bitmap(textWidth, textHeight))
            using (Graphics gBmp = Graphics.FromImage(bmp))
            {
                gBmp.Clear(Color.Transparent);

                // Draw stroked highlight into bitmap
                DrawTextWithStroke(
                    gBmp,
                    displayText,
                    font,
                    new Point(0, 0),
                    highlightColor,
                    strokeCol,
                    3
                );

                // Slice highlight region
                Rectangle src = new Rectangle(0, 0, (int)highlightWidth, bmp.Height);
                Rectangle dest = new Rectangle(posX, y, (int)highlightWidth, bmp.Height);

                if (src.Width > 0)
                {
                    g.DrawImage(bmp, dest, src, GraphicsUnit.Pixel);
                }
            }
        }

        private static void ApplyTextRenderingSettings(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
        }

        private void DrawTextWithStroke(Graphics g, string text, Font font, Point pos, Color fill, Color stroke, int strokeWidth)
        {
            ApplyTextRenderingSettings(g);
                   
            using (var sf = (StringFormat)StringFormat.GenericDefault.Clone())
            using (var path = new GraphicsPath())
            using (var pen = new Pen(stroke, strokeWidth) { LineJoin = LineJoin.Round })
            using (var fillBrush = new SolidBrush(fill))
            {
                float emSize = g.DpiY * font.SizeInPoints / 72f;

                // Build outline path at the same position
                path.AddString(
                    text,
                    font.FontFamily,
                    (int)font.Style,
                    emSize,
                    pos,
                    sf);

                // Stroke behind
                g.DrawPath(pen, path);

                // Fill using DrawString
                g.DrawString(text, font, fillBrush, pos, sf);
            }
        }
        
        public List<MergedSyllable> BuildSyllablePixelMap(
            List<MergedSyllable> syllables,
            Font font,
            Graphics g,
            string displayText,
            float totalTextWidth)
        {
            ApplyTextRenderingSettings(g);

            int searchIndex = 0;
            float prevPrefixWidth = 0f;

            foreach (var syllable in syllables)
            {
                string visible = GetVisibleTextForSyllable(syllable);

                if (string.IsNullOrWhiteSpace(visible))
                {
                    syllable.Width = 0f;
                    continue;
                }

                // Find this syllable's visible text in the final display string,
                // starting from where the last one left off.
                int idx = displayText.IndexOf(visible, searchIndex, StringComparison.Ordinal);
                if (idx < 0)
                {
                    // Fallback: if we can’t find it (weird cleaning / markers),
                    // at least measure it in isolation so we don't crash.
                    SizeF sizeFallback = g.MeasureString(visible, font);
                    syllable.Width = sizeFallback.Width;
                    continue;
                }

                int endIdx = idx + visible.Length;

                // Measure prefix up to the end of this syllable in the final string
                string prefixText = displayText.Substring(0, endIdx);
                SizeF prefixSize = g.MeasureString(prefixText, font);
                float prefixWidth = prefixSize.Width;

                syllable.Width = prefixWidth - prevPrefixWidth;

                prevPrefixWidth = prefixWidth;
                searchIndex = endIdx;
            }

            // Optional but recommended: normalize widths so they sum exactly
            // to the measured total text width.
            float sumWidths = syllables.Sum(s => s.Width);
            if (sumWidths > 0.1f && Math.Abs(sumWidths - totalTextWidth) > 0.5f)
            {
                float scale = totalTextWidth / sumWidths;
                foreach (var s in syllables)
                {
                    s.Width *= scale;
                }
            }

            return syllables;
        }

        public static float GetHighlightedPixelWidth(List<MergedSyllable> syllables, double currentTime)
        {
            float total = 0f;

            foreach (var s in syllables)
            {
                if (currentTime < s.Start)
                    break;

                if (currentTime >= s.End)
                {
                    total += s.Width;
                }
                else
                {
                    double progress = (currentTime - s.Start) / (s.End - s.Start);
                    progress = MathHelper.Clamp(progress, 0.0, 1.0);
                    total += (float)(progress * s.Width);
                    break;
                }
            }

            return total;
        }

        public string ReconstructPhraseTextFromSyllables(List<MergedSyllable> phraseSyllables)
        {
            var words = new List<string>();
            int i = 0;

            while (i < phraseSyllables.Count)
            {
                var syllable = phraseSyllables[i];
                string raw = syllable.Lyric.Trim();
                string clean = CleanSyllable(raw);
                bool endsWithDash = EndsWithDash(raw) || EndsWithDash(clean);

                string word = clean;

                int j = i + 1;
                bool extended = false;

                // Keep attaching to the word as long as we’re in a broken-up word (ending in - or +)
                while (j < phraseSyllables.Count)
                {
                    var next = phraseSyllables[j];
                    string nextRaw = next.Lyric.Trim();
                    string nextClean = CleanSyllable(nextRaw);

                    bool isSustain = nextRaw == "+";
                    bool nextEndsWithDash = EndsWithDash(nextRaw) || EndsWithDash(nextClean);

                    // If the previous ends in dash or this is a sustain, keep appending
                    if (endsWithDash || isSustain)
                    {
                        word += nextClean;
                        endsWithDash = nextEndsWithDash || isSustain;
                        j++;
                        extended = true;
                    }
                    else
                    {
                        break;
                    }
                }

                if (!string.IsNullOrWhiteSpace(word))
                    words.Add(word);

                i = extended ? j : i + 1;
            }

            return string.Join(" ", words).Replace("‿", " ");
        }

        private bool EndsWithDash(string s)
        {
            return s.Replace("#", "").Replace("^", "")
                    .TrimEnd('.', ',', '!', '?', '…', ';', ':')
                    .Trim()
                    .EndsWith("-");
        }

        private string CleanSyllable(string s)
        {
            return CleanString(s)
                .Replace("-", "")
                .Replace("=", "-")
                .Replace("#", "")
                .Replace("^", "")
                .Replace("$", "")
                .Replace("+", "^")
                .Replace("§", "‿")
                .Trim();
        }

        private string CleanString(string str)
        {
            return str.Replace("#", "").Replace("^", "").Replace("\"", "").Replace("§", "‿").Replace(",", "").Replace("$", "").Replace("%", "");
        }

        private void DrawHighlightAnimation(
            Graphics g,
            Font f,
            double lyricStart,
            float textStartX,
            int y,
            Color color,
            double time,
            double leadInSeconds)
        {
            double leadTime = lyricStart - time; // seconds remaining
            if (leadTime < 0) return;            // already started
            if (leadTime > leadInSeconds) return; // only show in final lead-in window

            const string cursor = "•";

            // Measure the dot so we can stop at the left edge of the first letter.
            // MeasureString can add a little extra; measuring a single glyph is usually fine for this purpose.
            float dotWidth = g.MeasureString(cursor, f).Width;
            
            // Target: dot's RIGHT edge sits just left of the first character.
            //float targetX = textStartX - dotWidth + padding;
            float targetX = textStartX - dotWidth + 10f;

            // How far it travels from the left
            float travel = 120f;

            // normalized: 1.0 -> start of animation, 0.0 -> lyricStart
            double normalized = MathHelper.Clamp(leadTime / leadInSeconds, 0.0, 1.0);

            // Optional: smooth the motion so it doesn't look linear/stiff
            double t = 1.0 - normalized;                 // 0 -> 1 over the animation
            //double eased = t * t * (3.0 - 2.0 * t);      // SmoothStep easing
            double eased = t;

            float startX = targetX - travel;
            float cursorX = startX + (float)(travel * eased);

            using (var brush = new SolidBrush(color))
            {
                g.DrawString(cursor, f, brush, new PointF(cursorX, y));
            }
        }

        public static (List<Lyric> line1, List<Lyric> line2)
        SplitSyllablesByPixelWidth(
            List<Lyric> phraseSyllables,
            Font font,
            Graphics g)
        {
            var line1 = new List<Lyric>();
            var line2 = new List<Lyric>();
            if (phraseSyllables == null || phraseSyllables.Count == 0) return (line1, line2);
            if (phraseSyllables.Count == 1) return (phraseSyllables, line2);

            // 1) Build "words" from syllables (merge hyphenated and sustained runs)
            var words = new List<(List<Lyric> syls, string text, int widthPx)>();

            int i = 0;
            while (i < phraseSyllables.Count)
            {
                var bucket = new List<Lyric>();
                bool keepMerging = true;

                while (i < phraseSyllables.Count && keepMerging)
                {
                    var s = phraseSyllables[i];
                    bucket.Add(s);

                    string lyric = (s.Text ?? "").Trim();
                    bool endsWithDash = lyric.Replace("#", "").Replace("^", "").Replace("$", "").EndsWith("-");
                    bool isSustain = lyric == "+";

                    bool nextIsSustain = (i + 1 < phraseSyllables.Count && phraseSyllables[i + 1].Text == "+");

                    // If current ends with "-", the word definitely continues.
                    // If current is "+", it belongs to the current word (sustain).
                    // If next is "+", we also keep merging into current word.
                    keepMerging = endsWithDash || isSustain || nextIsSustain;
                    i++;
                }

                // Visible text for the word (no + or -; replace tie with space)
                string wordText = string.Join("", bucket.Select(b =>
                    (b.Text ?? "").Replace("#", "").Replace("^", "").Replace("$", "").Replace("+", "").Replace("-", "").Replace("‿", " ")))
                    .Trim();

                if (wordText.Length == 0)
                {
                    // If the “word” is only sustains, skip it (still contributes to timing, not to drawing text)
                    continue;
                }

                // Measure with the SAME API we render with
                int w = TextRenderer.MeasureText(g, wordText, font).Width;

                words.Add((bucket, wordText, w));
            }

            if (words.Count == 0) return (line1, line2);
            if (words.Count == 1) { line1.AddRange(words[0].syls); return (line1, line2); }

            // 2) Compute total pixel width incl. spaces
            // Approximate a single space width with this font
            int spaceW = TextRenderer.MeasureText(g, " ", font).Width;
            int totalPx = 0;
            for (int k = 0; k < words.Count; k++)
            {
                totalPx += words[k].widthPx;
                if (k > 0) totalPx += spaceW;
            }
            int target = totalPx / 2;

            // 3) Greedy pack into line1 until we would exceed target
            int accum = 0;
            int breakIndex = words.Count; // default all on line1 if short

            for (int k = 0; k < words.Count; k++)
            {
                int add = words[k].widthPx + (k > 0 ? spaceW : 0);
                // If adding this word would push us *far* past target and we already
                // have at least one word, break before it.
                if (k > 0 && accum + add > target)
                {
                    // Optional: consider which side is visually closer to target
                    int over = (accum + add) - target;
                    int under = target - accum;
                    if (over >= under) breakIndex = k;
                    else breakIndex = k + 1;
                    break;
                }
                accum += add;
            }

            // 4) Emit syllables to lines
            for (int k = 0; k < words.Count; k++)
            {
                if (k < breakIndex) line1.AddRange(words[k].syls);
                else line2.AddRange(words[k].syls);
            }

            return (line1, line2);
        }

        private void DrawCenteredLine(Graphics g, string text, int resolutionX, int y, float maxFontSize, int offset = 0)
        {
            var baseFont = new Font("Arial", 16f);
            float scaledFontSize = GetScaledFontSize(g, text, baseFont, maxFontSize, resolutionX - offset);
            Font font = null;

            try
            {
                font = new Font("Arial", scaledFontSize);
                var size = TextRenderer.MeasureText(g, text, font);
                int x = (resolutionX + offset - size.Width) / 2;
                TextRenderer.DrawText(g, text, font, new Point(x, y), KaraokeModeHarm1Text, Color.Transparent);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error on frame: text='{text}' | scaledFontSize={scaledFontSize} | maxFontSize={maxFontSize} | Exception={ex.Message}\n");
            }
            baseFont.Dispose();
            font.Dispose();
        }

        public float GetScaledFontSize(Graphics g, string line, Font preferedFont, float maxSize, int frameWidth)
        {
            if (string.IsNullOrEmpty(line))
                return preferedFont.Size; // Avoid divide-by-zero or nonsense scaling

            double maxWidth = frameWidth * 0.85;
            SizeF measuredSize = g.MeasureString(line, preferedFont);

            if (measuredSize.Width <= 0)
                return preferedFont.Size; // Fallback to preferred if invalid

            double scaleRatio = maxWidth / measuredSize.Width;
            double scaledSize = preferedFont.Size * scaleRatio;

            // Clamp to a reasonable range
            const float absoluteMax = 256f;
            if (scaledSize > maxSize)
                return Math.Min(maxSize, absoluteMax);
            if (scaledSize < 4f)
                return 4f; // Prevent unreadably small fonts

            return (float)scaledSize;
        }
                
        private void DoKaraokeMode(Graphics graphics, IList<LyricPhrase> phrases, IEnumerable<Lyric> lyrics)
        {
            var renderSize = new Size(1920, 1080);

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
            picVisuals.BackColor = secondScreen == null ? KaraokeModeBackgroundColor : Color.AliceBlue;
            if (currentLine != null && !string.IsNullOrEmpty(currentLine.PhraseText))
            {
                //draw entire current phrase on top
                lineText = ProcessLine(currentLine.PhraseText, true).Replace("‿", " ");
                lineFont = new Font("Tahoma", GetScaledFontSize(graphics, lineText, new Font("Tahoma", (float)12.0), 120));
                lineSize = TextRenderer.MeasureText(lineText, lineFont);
                posX = (renderSize.Width - lineSize.Width) / 2;
                TextRenderer.DrawText(graphics, lineText, lineFont, new Point(posX, currentLineTop), KaraokeModeHarm1Text, KaraokeBackgroundColor);

                //draw portion of current phrase that's already been sung
                var line2 = lyrics.Where(lyr => !(lyr.Start < currentLine.PhraseStart)).TakeWhile(lyr => !(lyr.Start > time)).Aggregate("", (current, lyr) => current + " " + lyr.Text);
                line2 = ProcessLine(line2, true).Replace("‿", " ");
                if (!string.IsNullOrEmpty(line2))
                {
                    TextRenderer.DrawText(graphics, line2, lineFont, new Point(posX, currentLineTop), KaraokeModeHarm1Highlight, KaraokeBackgroundColor);
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
                        if (lyric.Start < currentLine.PhraseStart || lyric.Start > currentLine.PhraseEnd)
                        {
                            continue;
                        }
                        if (string.IsNullOrEmpty(word))
                        {
                            wordStart = lyric.Start;
                        }
                        if (lyric.Text.Contains("-")) //is a syllable
                        {
                            word += ProcessLine(lyric.Text, true);
                            wordEnd = lyric.End;
                            continue;
                        }
                        // Handle sustains
                        else if (!string.IsNullOrEmpty(word) && lyric.Text.Contains("+"))
                        {
                            //word += "+";
                            wordEnd = lyric.End;

                            // Extend for consecutive sustains
                            for (var a = i + 1; a < lyricsList.Count; a++)
                            {
                                if (lyricsList[a].Text.Contains("+"))
                                {
                                    //word += "+"; // Append the sustain
                                    wordEnd = lyricsList[a].End;
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
                            word += ProcessLine(lyric.Text, true).Replace("‿", " ");
                            wordEnd = lyric.End;

                            //look ahead to double check next lyric(s) aren't + sustains
                            for (var z = i + 1; z < lyricsList.Count - i - 1; z++)
                            {
                                if (lyricsList[z].Text.Contains("+"))
                                {
                                    //word += "+"; // Append the sustain
                                    wordEnd = lyricsList[z].End;
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
                        lineFont = new Font("Tahoma", GetScaledFontSize(graphics, activeWord.Text, new Font("Tahoma", (float)12.0), 200));
                        lineSize = TextRenderer.MeasureText(activeWord.Text, lineFont);
                        posX = (renderSize.Width - lineSize.Width) / 2;
                        var posY = (renderSize.Height - lineSize.Height) / 2;

                        // Draw the entire word in white
                        TextRenderer.DrawText(graphics, activeWord.Text, lineFont, new Point(posX, posY), KaraokeModeHarm1Text, KaraokeBackgroundColor);

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
                            TextRenderer.DrawText(graphics, sungPortion, lineFont, new Point(posX, posY), KaraokeModeHarm1Highlight, KaraokeBackgroundColor);
                        }
                    }
                }
            }
            if (nextLine != null && !string.IsNullOrEmpty(nextLine.PhraseText))
            {
                //draw entire next phrase on bottom
                lineText = ProcessLine(nextLine.PhraseText, true).Replace("‿", " ");
                lineFont = new Font("Tahoma", GetScaledFontSize(graphics, lineText, new Font("Tahoma", (float)12.0), 120));
                lineSize = TextRenderer.MeasureText(lineText, lineFont);
                posX = (renderSize.Width - lineSize.Width) / 2;
                TextRenderer.DrawText(graphics, lineText, lineFont, new Point(posX, nextLineTop - lineSize.Height), KaraokeModeHarm1Text, KaraokeBackgroundColor);
            }

            //draw waiting/countdown info
            if (currentLine != null && nextLine != null) return;
            if (lastLine != null && nextLine != null)
            {
                var difference = nextLine.PhraseStart - lastLine.PhraseEnd;
                if (difference < 5) return;
            }
            var middleText = "";
            var textColor = KaraokeModeHarm1Text;
            if (currentLine == null && nextLine != null)
            {
                var wait = nextLine.PhraseStart - time;
                if (wait < 1.5) return;
                middleText = wait <= 5 ? "[GET READY]" : "[WAIT: " + ((int)(wait + 0.5)) + "]";
                textColor = wait <= 5 ? KaraokeModeHarm1Highlight : KaraokeModeHarm1Text;// Color.FromArgb(185, 216, 76) : Color.FromArgb(255, 187, 52);
            }
            else if (currentLine == null)
            {
                middleText = "[fin]";
            }
            lineFont = new Font("Tahoma", GetScaledFontSize(graphics, middleText, new Font("Tahoma", (float)12.0), 200));
            lineSize = TextRenderer.MeasureText(middleText, lineFont);
            posX = (renderSize.Width - lineSize.Width) / 2;
            TextRenderer.DrawText(graphics, middleText, lineFont, new Point(posX, (renderSize.Height - lineSize.Height) / 2), textColor, KaraokeBackgroundColor);
        }

        public float Clamp(float value, float min, float max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        public float GetScaledFontSize(Graphics g, string line, Font PreferedFont, float maxSize)
        {
            var renderSize = new Size(1920, 1080);
            var maxWidth = renderSize.Width * 0.95;
            var RealSize = g.MeasureString(line, PreferedFont);
            var ScaleRatio = maxWidth / RealSize.Width;
            var ScaledSize = PreferedFont.Size * ScaleRatio;
            if (ScaledSize > maxSize)
            {
                return maxSize;
            }
            return (float)ScaledSize;
        }

        public double GetCorrectedTime()
        {
            var time = PlaybackSeconds - ((double)BassBuffer / 1000) - ((double)PlayingSong.PSDelay / 1000);
            if (enableBTAVOffsetSync)
            {
                time -= ((double)BTAVOffsetSync / 1000);
            }
            return time;
        }

        public void ClearVisuals(bool clear_chart = false)
        {
            if (clear_chart && Chart != null)
            {
                Chart.Clear(chartSnippet.Checked ? TrackBackgroundColor1 : GetNoteColor(100));
            }
        }

        private void UpdateVisualStyle(object sender)
        {
            Image image = null;
            if (secondScreen != null)
            {
                secondScreen.ChangeBackgroundImage(image);
            }
            else
            {
                picVisuals.Image = image;
            }
            stageTimer.Enabled = false;
            ClearVisuals();
            PrepareForDrawing();
        }        

        private void UpdateConsole(object sender, EventArgs e)
        {
            if (songLoader.IsBusy || batchSongLoader.IsBusy) return;
            if (Text.Contains("*"))
            {
                if (MessageBox.Show("You have unsaved changes on the current playlist\nAre you sure you want to change console and lose those changes?",
                    AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }
            }

            var sentBy = (ToolStripMenuItem)sender;
            string newConsole;
            nautilusToolStripMenuItem.Visible = true;
            setNautilusPath.Enabled = true;
            sendToVisualizer.Enabled = true;
            var enabled = false;
            if (sentBy == xbox360)
            {
                newConsole = "xbox";
                consoleToolStripMenuItem.Text = "Game | Console: Rock Band 1/2/3 | Xbox 360";
                enabled = true;
                SetDefaultPaths();
            }
            else if (sentBy == pS3)
            {
                newConsole = "ps3";
                consoleToolStripMenuItem.Text = "Game | Console: Rock Band 1/2/3 | PlayStation 3";
            }
            else if (sentBy == wii)
            {
                newConsole = "wii";
                consoleToolStripMenuItem.Text = "Game | Console: Rock Band 1/2/3 | Wii";
            }
            else if (sentBy == rb4PS4)
            {
                newConsole = "ps4";
                consoleToolStripMenuItem.Text = "Game | Console: Rock Band 4 | PlayStation 4";
            }
            else if (sentBy == yarg)
            {
                newConsole = "yarg";
                consoleToolStripMenuItem.Text = "Game | Console: YARG / Clone Hero | PC";
            }            
            else if (sentBy == rockSmith)
            {
                newConsole = "rocksmith";
                consoleToolStripMenuItem.Text = "Game | Console: Rocksmith 2014 | PC";
            }
            else if (sentBy == guitarHero)
            {
                newConsole = "guitarhero";
                consoleToolStripMenuItem.Text = "Game | Console: GHWT:DE | PC";
            }
            else if (sentBy == fortNite)
            {
                newConsole = "fortnite";
                consoleToolStripMenuItem.Text = "Game | Console: Fortnite Festival | PC";
            }
            else if (sentBy == powerGig)
            {
                newConsole = "powergig";
                consoleToolStripMenuItem.Text = "Game | Console: Power Gig | PC";
            }
            else if (sentBy == bandFuse)
            {
                newConsole = "bandfuse";
                consoleToolStripMenuItem.Text = "Game | Console: BandFuse | Xbox 360";
            }
            else
            {
                sendToCONExplorer.Enabled = enabled;
                sendToFileAnalyzer.Enabled = enabled;
                sendToAudioAnalyzer.Enabled = enabled;
                return;
            }
            sendToCONExplorer.Enabled = enabled;
            sendToFileAnalyzer.Enabled = enabled;
            sendToAudioAnalyzer.Enabled = enabled;
            if (PlayerConsole == newConsole) return;
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
            rb4PS4.Checked = sentBy == rb4PS4;
            PlayerConsole = newConsole;
            StartNew(false);
        }

        private static readonly Random Rng = new Random();
        private double _nextFogEligible = 0;

        private void RandomizeStageKit(double songTimeSec)
        {            
            double bpm = PlayingSong.BPM > 0 ? PlayingSong.BPM : 120;
            double pMin = 1.0 / 16.0;  // ≈6.25% at 80 BPM
            double pMax = 0.8;         // 100% at 200+ BPM (use e.g. 0.6 if we want to cap it)
            double t = (bpm - 80.0) / 120.0; // 0 at 80, 1 at 200
            if (t < 0) t = 0; else if (t > 1) t = 1;

            double p = pMin + (pMax - pMin) * t;
            bool activate = Rng.NextDouble() < p;
            if (!activate) return;

            if (useLEDs.Checked)
            {
                var doRedCircle = Rng.NextDouble() < 0.50;
                var doRedLaser = Rng.NextDouble() < 0.50;
                var doRedRandom = Rng.NextDouble() < 0.50;

                if (doRedCircle)
                {
                    redSKIndex = (redSKIndex + 1) & 7;
                    update_red_led_state(redSKIndex, true, ref ledDisplay, ref stageKit);
                    update_red_led_state(redSKIndex == 0 ? 7 : redSKIndex - 1, false, ref ledDisplay, ref stageKit);
                }
                else if (doRedLaser)
                {
                    update_red_led_state(redSKIndex, false, ref ledDisplay, ref stageKit);
                    redSKIndex = Rng.Next(0, 8);
                    update_red_led_state(redSKIndex, true, ref ledDisplay, ref stageKit);
                }
                else if (doRedRandom)
                {
                    var index = Rng.Next(0, 8);
                    update_red_led_state(index, !CurrentStateRed[index], ref ledDisplay, ref stageKit);
                }

                var doBlueCircle = Rng.NextDouble() < 0.50;
                var doBlueLaser = Rng.NextDouble() < 0.50;
                var doBlueRandom = Rng.NextDouble() < 0.50;

                if (doBlueCircle)
                {
                    blueSKIndex = (blueSKIndex + 1) & 7;
                    update_blue_led_state(blueSKIndex, true, ref ledDisplay, ref stageKit);
                    update_blue_led_state(blueSKIndex == 0 ? 7 : blueSKIndex - 1, false, ref ledDisplay, ref stageKit);
                }
                else if (doBlueLaser)
                {
                    update_blue_led_state(blueSKIndex, false, ref ledDisplay, ref stageKit);
                    blueSKIndex = Rng.Next(0, 8);
                    update_blue_led_state(blueSKIndex, true, ref ledDisplay, ref stageKit);
                }
                else if (doBlueRandom)
                {
                    var index = Rng.Next(0, 8);
                    update_blue_led_state(index, !CurrentStateBlue[index], ref ledDisplay, ref stageKit);
                }

                var doGreenCircle = Rng.NextDouble() < 0.50;
                var doGreenLaser = Rng.NextDouble() < 0.50;
                var doGreenRandom = Rng.NextDouble() < 0.50;

                if (doGreenCircle)
                {
                    greenSKIndex = (greenSKIndex + 1) & 7;
                    update_green_led_state(greenSKIndex, true, ref ledDisplay, ref stageKit);
                    update_green_led_state(greenSKIndex == 0 ? 7 : greenSKIndex - 1, false, ref ledDisplay, ref stageKit);
                }
                else if (doGreenLaser)
                {
                    update_green_led_state(greenSKIndex, false, ref ledDisplay, ref stageKit);
                    greenSKIndex = Rng.Next(0, 8);
                    update_green_led_state(greenSKIndex, true, ref ledDisplay, ref stageKit);
                }
                else if (doGreenRandom)
                {
                    var index = Rng.Next(0, 8);
                    update_green_led_state(index, !CurrentStateGreen[index], ref ledDisplay, ref stageKit);
                }

                var doYellowCircle = Rng.NextDouble() < 0.50;
                var doYellowLaser = Rng.NextDouble() < 0.50;
                var doYellowRandom = Rng.NextDouble() < 0.50;

                if (doYellowCircle)
                {
                    yellowSKIndex = (yellowSKIndex + 1) & 7;
                    update_yellow_led_state(yellowSKIndex, true, ref ledDisplay, ref stageKit);
                    update_yellow_led_state(yellowSKIndex == 0 ? 7 : yellowSKIndex - 1, false, ref ledDisplay, ref stageKit);
                }
                else if (doYellowLaser)
                {
                    update_yellow_led_state(yellowSKIndex, false, ref ledDisplay, ref stageKit);
                    yellowSKIndex = Rng.Next(0, 8);
                    update_yellow_led_state(yellowSKIndex, true, ref ledDisplay, ref stageKit);
                }
                else if (doYellowRandom)
                {
                    var index = Rng.Next(0, 8);
                    update_yellow_led_state(index, !CurrentStateYellow[index], ref ledDisplay, ref stageKit);
                }
            }

            if (useStrobe.Checked)
            {
                try
                {
                    var turnOnStrobe = Rng.NextDouble() < 0.20;
                    var speed = PlayingSong.BPM < 80 ? StrobeSpeed.Slow
                             : PlayingSong.BPM < 140 ? StrobeSpeed.Medium
                             : StrobeSpeed.Faster;

                    if (turnOnStrobe)
                    {
                        stageKit.TurnStrobeOn(speed);
                    }
                    else
                    {
                        stageKit.TurnStrobeOff();
                    }
                }
                catch (Exception e) { }
            }

            if (useFogger.Checked)
            {
                try
                {
                    if (songTimeSec >= _nextFogEligible && Rng.Next(0, 200) == 0) // ~1–2 per 4–5 min song
                    {
                        var fogInterval = Rng.Next(500, 2001); //0.5 to 2 seconds
                        foggerTimer.Interval = fogInterval;
                        _nextFogEligible = songTimeSec + 60 + Rng.Next(0, 21);    // 60–81s cooldown
                        stageKit.TurnFogOn();
                        foggerTimer.Enabled = true;
                    }
                }
                catch (Exception ex) { }
            }
        }

        private void update_yellow_led_state(int ledIndex, bool ledState, ref LedDisplay display_panel, ref StageKitController controller_ref)
        {
            try
            {
                switch (ledIndex)
                {
                    case 0:
                        controller_ref.DisplayYellowLed1(ref display_panel, ledState);
                        break;
                    case 1:
                        controller_ref.DisplayYellowLed2(ref display_panel, ledState);
                        break;
                    case 2:
                        controller_ref.DisplayYellowLed3(ref display_panel, ledState);
                        break;
                    case 3:
                        controller_ref.DisplayYellowLed4(ref display_panel, ledState);
                        break;
                    case 4:
                        controller_ref.DisplayYellowLed5(ref display_panel, ledState);
                        break;
                    case 5:
                        controller_ref.DisplayYellowLed6(ref display_panel, ledState);
                        break;
                    case 6:
                        controller_ref.DisplayYellowLed7(ref display_panel, ledState);
                        break;
                    case 7:
                        controller_ref.DisplayYellowLed8(ref display_panel, ledState);
                        break;
                    default:
                        return;
                }
                CurrentStateYellow[ledIndex] = ledState;
            }
            catch (Exception ex) { }
        }

        private void update_blue_led_state(int ledIndex, bool ledState, ref LedDisplay display_panel, ref StageKitController controller_ref)
        {
            try
            {
                switch (ledIndex)
                {
                    case 0:
                        controller_ref.DisplayBlueLed1(ref display_panel, ledState);
                        break;
                    case 1:
                        controller_ref.DisplayBlueLed2(ref display_panel, ledState);
                        break;
                    case 2:
                        controller_ref.DisplayBlueLed3(ref display_panel, ledState);
                        break;
                    case 3:
                        controller_ref.DisplayBlueLed4(ref display_panel, ledState);
                        break;
                    case 4:
                        controller_ref.DisplayBlueLed5(ref display_panel, ledState);
                        break;
                    case 5:
                        controller_ref.DisplayBlueLed6(ref display_panel, ledState);
                        break;
                    case 6:
                        controller_ref.DisplayBlueLed7(ref display_panel, ledState);
                        break;
                    case 7:
                        controller_ref.DisplayBlueLed8(ref display_panel, ledState);
                        break;
                    default:
                        return;
                }
                CurrentStateBlue[ledIndex] = ledState;
            }
            catch (Exception e) { }
        }

        private void update_green_led_state(int ledIndex, bool ledState, ref LedDisplay display_panel, ref StageKitController controller_ref)
        {
            try
            {
                switch (ledIndex)
                {
                    case 0:
                        controller_ref.DisplayGreenLed1(ref display_panel, ledState);
                        break;
                    case 1:
                        controller_ref.DisplayGreenLed2(ref display_panel, ledState);
                        break;
                    case 2:
                        controller_ref.DisplayGreenLed3(ref display_panel, ledState);
                        break;
                    case 3:
                        controller_ref.DisplayGreenLed4(ref display_panel, ledState);
                        break;
                    case 4:
                        controller_ref.DisplayGreenLed5(ref display_panel, ledState);
                        break;
                    case 5:
                        controller_ref.DisplayGreenLed6(ref display_panel, ledState);
                        break;
                    case 6:
                        controller_ref.DisplayGreenLed7(ref display_panel, ledState);
                        break;
                    case 7:
                        controller_ref.DisplayGreenLed8(ref display_panel, ledState);
                        break;
                    default:
                        return;
                }
                CurrentStateGreen[ledIndex] = ledState;
            }
            catch (Exception e)
            { }
        }

        private void update_red_led_state(int ledIndex, bool ledState, ref LedDisplay display_panel, ref StageKitController controller_ref)
        {
            try
            {
                switch (ledIndex)
                {
                    case 0:
                        controller_ref.DisplayRedLed1(ref display_panel, ledState);
                        break;
                    case 1:
                        controller_ref.DisplayRedLed2(ref display_panel, ledState);
                        break;
                    case 2:
                        controller_ref.DisplayRedLed3(ref display_panel, ledState);
                        break;
                    case 3:
                        controller_ref.DisplayRedLed4(ref display_panel, ledState);
                        break;
                    case 4:
                        controller_ref.DisplayRedLed5(ref display_panel, ledState);
                        break;
                    case 5:
                        controller_ref.DisplayRedLed6(ref display_panel, ledState);
                        break;
                    case 6:
                        controller_ref.DisplayRedLed7(ref display_panel, ledState);
                        break;
                    case 7:
                        controller_ref.DisplayRedLed8(ref display_panel, ledState);
                        break;
                    default:
                        return;
                }
                CurrentStateRed[ledIndex] = ledState;
            }
            catch (Exception ex)
            { }
        }

        private void EnsureScaled(Size displaySize)
        {
            if (_scaledFrame == null || _scaledFrame.Width != displaySize.Width || _scaledFrame.Height != displaySize.Height)
            {
                _scaledFrame?.Dispose();
                _scaledFrame = new Bitmap(displaySize.Width, displaySize.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            }
        }

        //private bool videoOverlayShown;
        private int _advancing;
        private Bitmap _scaledFrame;

        private void PlaybackTimer_Tick(object sender, EventArgs e)
        {
            if (isChoosingStems) return;            
            try
            {
                if (Bass.BASS_ChannelIsActive(BassMixer) == BASSActive.BASS_ACTIVE_PLAYING)
                {                                                       
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
                        PlaybackTimer.Enabled = false;
                        StopPlayback();                        
                        StopStageKit();
                        if (continuousPlayback.Checked)
                        {
                            DoShuffleSongs();
                        }                        
                        return;
                    }
                    UpdateTime();
                    DoPracticeSessions();
                    if ((displayAlbumArt.Checked && File.Exists(CurrentSongArt)) || (!File.Exists(CurrentSongArt) && !displayAudioSpectrum.Checked))
                    {
                        /*if ((secondScreen == null && VideoIsPlaying) || (secondScreen != null && secondScreen.VideoIsPlaying))
                        {
                            StopAllVideoPlayback();
                        }*/
                        picPreview.Invalidate();
                    }
                    if (openSideWindow.Checked || secondScreen != null)
                    {   
                        videoOverlay.Visible = secondScreen == null;
                        if (secondScreen != null)
                        {
                            secondScreen.videoOverlay.Visible = true;
                        }

                        var displaySize = (secondScreen != null ? secondScreen.videoOverlay.Size : videoOverlay.Size);
                        if (displaySize.Width <= 0 || displaySize.Height <= 0) return;
                        
                        //let's force a 1080p internal resolution, then upscale
                        var renderSize = new Size(1920, 1080);
                        EnsureFrame(renderSize);
                        EnsureScaled(displaySize);

                        if (WindowState == FormWindowState.Minimized)// || !MonitorApplicationFocus())
                        {
                            if (secondScreen != null)
                            {
                                secondScreen.ClearOverlayFrame();
                            }
                            else
                            {
                                ClearOverlayFrame();
                            }
                            return;//don't waste resources drawing if not visible
                        }

                        using (var g = Graphics.FromImage(_renderedFrame))
                        {
                            g.Clear(Color.Transparent);
                            UpdateTextQuality(g);
                            if (chartVisualsToolStripMenuItem.Checked && rBStyle.Checked && !changedBackground)
                            {
                                ChangeRBStyleBackground();
                                changedBackground = true;
                            }
                            if (rBStyle.Checked && doUseBackgroundImages && RBStyleBackground != null)
                            {
                                g.DrawImage(RBStyleBackground, 0, 0, renderSize.Width, renderSize.Height);
                            }
                            RenderVisuals(renderSize, g);
                        }

                        //resize visuals to actual screen size
                        using (var g = Graphics.FromImage(_scaledFrame))
                        {
                            g.CompositingMode = CompositingMode.SourceCopy; // overwrite
                            g.Clear(Color.Transparent);
                            g.DrawImage(_renderedFrame, new Rectangle(0, 0, displaySize.Width, displaySize.Height));
                        }

                        if (_scaledFrame != null)
                        {
                            if (secondScreen != null)
                            {
                                secondScreen.videoOverlay.UpdateVisuals(_scaledFrame);
                                picVisuals.Image = Resources.logo;
                            }
                            else
                            {
                                videoOverlay.UpdateVisuals(_scaledFrame);
                            }
                        }  
                    }
                    return;
                }
                if (Bass.BASS_ChannelIsActive(BassMixer) != BASSActive.BASS_ACTIVE_STOPPED) return;
            }
            catch (Exception)
            {
                return;
            }
           
            if (Interlocked.Exchange(ref _advancing, 1) == 1) return;

            GoToNextSong:
            try
            {
                StopStageKit();
                if (!continuousPlayback.Checked)
                {
                    StopPlayback();
                    return;
                }
                PlaybackTimer.Enabled = false;
                if (picLoop.Tag.ToString() == "loop")
                {
                    DoLoop();
                    return;
                }
                if (picShuffle.Tag.ToString() == "shuffle")
                {
                    DoShuffleSongs();
                    return;
                }
                picNext_MouseClick(null, null);
            }
            catch { }
        }

        private string RemoveCloneHeroColor(string author)
        {
            try
            {
                int startIndex = author.IndexOf('>') + 1;
                int endIndex = author.LastIndexOf('<');
                return author.Substring(startIndex, endIndex - startIndex);
            }
            catch
            {
                try
                {
                    int startIndex = author.IndexOf('<');
                    int endIndex = author.LastIndexOf('>') + 1;
                    var color = author.Substring(startIndex, endIndex - startIndex);
                    return author.Replace(color, "").Trim();
                }
                catch
                {
                    return author;
                }
            }
        }

        private Color GetCloneHeroColor(string author)
        {
            try
            {
                int colorStartIndex = author.IndexOf('=') + 1;
                int colorEndIndex = author.IndexOf('>');
                string colorValue = author.Substring(colorStartIndex, colorEndIndex - colorStartIndex);

                Color color;
                try
                {
                    color = ColorTranslator.FromHtml(colorValue); // Convert from hex
                }
                catch
                {
                    color = Color.Black; // Default to black if invalid
                }

                return color;
            }
            catch
            {
                return Color.Black;
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

        private const double depthPower = 1.50;
        private const float horizonPercent = 0.40f;
        private const float overshootPx = 40f;

        private void DrawFillsRB(Graphics graphics, MIDITrack instrument, int posY, int posX, int track_width)
        {
            if ((instrument.Fills == null || instrument.Fills.Count == 0) &&
                (instrument.Overdrive == null || instrument.Overdrive.Count == 0))
                return;

            var renderSize = new Size(1920, 1080);
            var correctedTime = GetCorrectedTime();

            // ===== Perspective tuning =====
            const double minScale = 0.35;       // far
            const double maxScale = 1.00;       // near

            // Near plane
            float hitboxY = renderSize.Height - 50f;

            // Far plane
            float horizonY = posY + ((hitboxY - posY) * horizonPercent);

            // Colors
            var fillColor = Color.FromArgb(100, ChartGreen.R, ChartGreen.G, ChartGreen.B);

            // Draw one active fill
            foreach (var fill in instrument.Fills)
            {
                if (fill.MarkerEnd <= correctedTime) continue;
                if (fill.MarkerBegin > correctedTime + PlaybackWindowRB) break;

                DrawFillPerspective(
                    graphics,
                    fill.MarkerBegin,
                    fill.MarkerEnd,
                    correctedTime,
                    posX,
                    track_width,
                    fillColor,
                    horizonY,
                    hitboxY,
                    PlaybackWindowRB,
                    minScale,
                    maxScale,
                    depthPower,
                    50f
                );
                break;
            }

            // Draw one active OD
            foreach (var od in instrument.Overdrive)
            {
                if (od.MarkerEnd <= correctedTime) continue;
                if (od.MarkerBegin > correctedTime + PlaybackWindowRB) break;

                fillColor = Color.FromArgb(100, 255, 255, 255);

                DrawFillPerspective(
                    graphics,
                    od.MarkerBegin,
                    od.MarkerEnd,
                    correctedTime,
                    posX,
                    track_width,
                    fillColor,
                    horizonY,
                    hitboxY,
                    PlaybackWindowRB,
                    minScale,
                    maxScale,
                    depthPower,
                    50f
                );
                break;
            }
        }


        // Draw a time-span band (fill/OD) in the same perspective space
        // begin/end are in song seconds (same units as correctedTime and PlaybackWindowRB).
        private void DrawFillPerspective(
            Graphics g,
            double begin,
            double end,
            double correctedTime,
            int chartLeft,
            int trackWidth,
            Color fillColor,
            float horizonY,
            float hitboxY,
            double playbackWindow,
            double minScale,
            double maxScale,
            double depthPower,
            float overshootPx
        )
        {
            // Helpers
            double ClampMin0(double v) => v < 0 ? 0 : v;
            double EaseIn(double t, double power) => Math.Pow(t, power);
            double Lerp(double a, double b, double t) => a + (b - a) * t;

            // Cull if totally out of window
            if (end <= correctedTime) return;
            if (begin > correctedTime + playbackWindow) return;

            // Convert time -> progress (0 far .. >1 near)
            // Allow near-side overshoot by NOT clamping the top end
            double t0 = 1.0 - ((begin - correctedTime) / playbackWindow);
            double t1 = 1.0 - ((end - correctedTime) / playbackWindow);

            // Clamp only the far end (so it never goes above horizon)
            t0 = ClampMin0(t0);
            t1 = ClampMin0(t1);

            // Apply curve
            double p0 = EaseIn(t0, depthPower);
            double p1 = EaseIn(t1, depthPower);

            // Map to Y with optional overshoot
            float y0 = (float)Lerp(horizonY, hitboxY + overshootPx, p0);
            float y1 = (float)Lerp(horizonY, hitboxY + overshootPx, p1);

            // If the whole band is below the overshoot region, skip
            if (y0 > hitboxY + overshootPx && y1 > hitboxY + overshootPx) return;

            // Ensure y0 is the "top" visually
            if (y1 < y0)
            {
                float tmpY = y0; y0 = y1; y1 = tmpY;
                double tmpP = p0; p0 = p1; p1 = tmpP;
            }

            // Compute highway width at each Y (match note laneSpan behavior)
            double scale0 = Lerp(minScale, maxScale, p0);
            double scale1 = Lerp(minScale, maxScale, p1);

            double span0 = trackWidth * scale0;
            double span1 = trackWidth * scale1;

            float centerX = chartLeft + (trackWidth / 2f);

            float left0 = (float)(centerX - (span0 / 2.0));
            float right0 = (float)(centerX + (span0 / 2.0));
            float left1 = (float)(centerX - (span1 / 2.0));
            float right1 = (float)(centerX + (span1 / 2.0));

            // ---- Clamp the fill so it never renders below the hitbox ----
            if (y1 > hitboxY)
            {
                // If the entire band starts below the hitbox, nothing to draw
                if (y0 >= hitboxY) return;

                // Interpolate left/right edges at the hitbox line
                float t = (hitboxY - y0) / (y1 - y0);   // 0..1
                                                        // guard
                if (t < 0f) t = 0f;
                if (t > 1f) t = 1f;

                // Move the bottom edge up to hitboxY
                left1 = left0 + (left1 - left0) * t;
                right1 = right0 + (right1 - right0) * t;

                y1 = hitboxY;
            }

            // Build trapezoid polygon
            var pts = new[]
            {
                new PointF(left0,  y0),
                new PointF(right0, y0),
                new PointF(right1, y1),
                new PointF(left1,  y1)
            };

            using (var brush = new SolidBrush(fillColor))
            {
                g.FillPolygon(brush, pts);
            }
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
            var renderSize = new Size(1920, 1080);
            // Calculate the chart goal relative to the given posY
            ChartGoal = renderSize.Height - posY - 50; // Pre-calculated

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
            if (topY + height > renderSize.Height - 50)
            {
                height = renderSize.Height - 50 - topY;
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

        private void DrawProKeysLaneFillsPerspective(
            Graphics g,
            int chartLeft,
            int trackWidth,          // MUST match notes/highway width (e.g. track_width * 2)
            float horizonY,
            float hitboxY,
            float overshootPx,       // same overshoot you use for notes
            double minScale,
            double maxScale,
            Func<int, Color> laneColorFunc,  // keyIndex 0..24 -> base color
            int strips = 220,
            int keyCount = 25,
            int alpha = 30           // 20–60 is a good range
        )
        {
            float bottomY = hitboxY;
            float spanY = bottomY - horizonY;
            if (spanY < 2f) return;

            float trackCenterX = chartLeft + (trackWidth / 2f);

            // Helpers
            double Clamp01(double v) => v < 0 ? 0 : (v > 1 ? 1 : v);
            double Lerp(double a, double b, double t) => a + (b - a) * t;

            double PFromY(float y)
            {
                double t = (y - horizonY) / spanY;  // IMPORTANT: matches your note mapping
                return Clamp01(t);
            }

            void SpanAtY(float y, out double span, out double spanLeft)
            {
                double p = PFromY(y);
                double scale = Lerp(minScale, maxScale, p);
                span = trackWidth * scale;
                spanLeft = trackCenterX - (span / 2.0);
            }

            var oldInterp = g.InterpolationMode;
            var oldPixel = g.PixelOffsetMode;
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            // Draw lane fills as horizontal strips so they stay perfectly aligned in perspective
            for (int s = 0; s < strips; s++)
            {
                float t0 = (float)s / strips;
                float t1 = (float)(s + 1) / strips;

                float y0 = horizonY + (spanY * t0);
                float y1 = horizonY + (spanY * t1);
                float h = Math.Max(1f, y1 - y0);

                SpanAtY(y0, out double span, out double spanLeft);
                double keyW = span / keyCount;

                for (int key = 0; key < keyCount; key++)
                {
                    Color baseC = laneColorFunc(key);
                    if (baseC == Color.Empty) continue;

                    using (var brush = new SolidBrush(Color.FromArgb(alpha, baseC)))
                    {
                        float x = (float)(spanLeft + (keyW * key));
                        float w = (float)Math.Max(1.0, keyW);

                        g.FillRectangle(brush, x, y0, w, h);
                    }
                }
            }

            g.InterpolationMode = oldInterp;
            g.PixelOffsetMode = oldPixel;
        }

        private void DrawProKeysSustainTail_WaveAboveHitbox_FollowLane(
            Graphics g,
            MIDINote note,
            Color tailColor,
            double correctedTime,
            int keyIndex,           // 0..24 (note.NoteNumber - 48)
            int chartLeft,
            int trackWidth,
            float horizonY,
            float hitboxY,
            double minScale,
            double maxScale,
            double depthPower,
            float yPos,
             Func<float, double> PFromY,
             bool noteIsSharp
        )
        {
            const float sampleStepY = 4f;      // smooth ribbon sampling
            const double amplitude = 3.0;      // pro keys: slightly smaller
            const double waveYFreq = 0.0125;
            const double grace = 0.05;

            double endTime = note.NoteStart + note.NoteLength;
            if (correctedTime > endTime + grace) return;                      

            double Clamp01(double v) => v < 0 ? 0 : (v > 1 ? 1 : v);
            double ClampMin0(double v) => v < 0 ? 0 : v;
            double EaseIn(double t) => Math.Pow(t, depthPower);
            double Lerp(double a, double b, double t) => a + (b - a) * t;

            float trackCenterX = chartLeft + (trackWidth / 2f);
            const int lanes = 25;

            // Time -> eased progress (only for yHead/yEnd)
            double tHead = 1.0 - ((note.NoteStart - correctedTime) / PlaybackWindowRB);
            double tEnd = 1.0 - ((endTime - correctedTime) / PlaybackWindowRB);
            tHead = ClampMin0(tHead);
            tEnd = ClampMin0(tEnd);

            double pHead = EaseIn(tHead);
            double pEnd = EaseIn(tEnd);

            float YFromP01(double p01) => (float)Lerp(horizonY, hitboxY, Clamp01(p01));

            float yHead = YFromP01(pHead);
            float yEnd = YFromP01(pEnd);

            // Ensure end is above head
            if (yEnd > yHead)
            {
                float tmpY = yEnd; yEnd = yHead; yHead = tmpY;
                double tmpP = pEnd; pEnd = pHead; pHead = tmpP;
            }

            // If sustain end already crossed hitbox, nothing remains above it
            if (yEnd >= hitboxY) return;

            // ✅ Correct inverse for YFromP01: linear
            double P01FromY(float y)
            {
                return PFromY != null ? PFromY(y) : Clamp01((y - horizonY) / (hitboxY - horizonY));
            }

            double LaneCenterAtY(float y, out double laneW)
            {
                double p = P01FromY(y);
                double scale = Lerp(minScale, maxScale, p);
                double span = trackWidth * scale;
                laneW = span / lanes;

                double spanLeft = trackCenterX - (span / 2.0);
                return spanLeft + (laneW * (keyIndex + 0.5));
            }

            // Pro keys: skinny tail; tie it to lane width at that Y
            float TailWidthFromLaneW(double laneW)
            {
                return (float)Math.Max(2.0, Math.Min(4.0, laneW * 0.35));
            }

            // Decide vertical extent above hitbox
            float top = Math.Max(yEnd, horizonY);
            float bottom = yPos + 3f;
            bottom = Math.Max(horizonY, Math.Min(bottom, hitboxY));

            if (bottom - top <= 1f) return;

            bool doWave = (yHead >= hitboxY);

            var leftPts = new List<PointF>(256);
            var rightPts = new List<PointF>(256);

            for (float y = top; y <= bottom + 0.001f; y += sampleStepY)
            {
                float yy = (y > bottom) ? bottom : y;

                double laneW;
                double cx = LaneCenterAtY(yy, out laneW);

                float tailW = TailWidthFromLaneW(laneW);
                float halfW = tailW / 2f;

                double waveOffset = 0.0;
                if (doWave)
                {
                    double s = Math.Sin((correctedTime * 2.0 + yy * waveYFreq) * Math.PI * 2.0);
                    waveOffset = amplitude * s;

                    double maxWave = Math.Max(0.0, (laneW / 2.0) - halfW - 1.0);
                    if (waveOffset > maxWave) waveOffset = maxWave;
                    if (waveOffset < -maxWave) waveOffset = -maxWave;
                }

                leftPts.Add(new PointF((float)(cx - halfW + waveOffset), yy));
                rightPts.Add(new PointF((float)(cx + halfW + waveOffset), yy));

                if (yy >= bottom) break;
            }

            if (leftPts.Count < 2) return;

            // Build main ribbon polygon
            var poly = new List<PointF>(leftPts.Count + rightPts.Count);
            poly.AddRange(leftPts);
            for (int i = rightPts.Count - 1; i >= 0; i--)
                poly.Add(rightPts[i]);

            // ✅ NOW build glow from the actual points
            const float glowPx = 1.5f;

            var leftGlow = new List<PointF>(leftPts.Count);
            var rightGlow = new List<PointF>(rightPts.Count);

            for (int i = 0; i < leftPts.Count; i++)
            {
                leftGlow.Add(new PointF(leftPts[i].X - glowPx, leftPts[i].Y));
                rightGlow.Add(new PointF(rightPts[i].X + glowPx, rightPts[i].Y));
            }

            var glowPoly = new List<PointF>(leftGlow.Count + rightGlow.Count);
            glowPoly.AddRange(leftGlow);
            for (int i = rightGlow.Count - 1; i >= 0; i--)
                glowPoly.Add(rightGlow[i]);

            var oldSmoothing = g.SmoothingMode;
            var oldPix = g.PixelOffsetMode;

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            // 1) glow behind (guard against weird edge cases)
            if (glowPoly.Count >= 3)
            {
                using (var glowBrush = new SolidBrush(Color.FromArgb(140, 255, 200, 0)))
                    g.FillPolygon(glowBrush, glowPoly.ToArray());
            }

            // 2) main tail
            using (var brush = new SolidBrush(tailColor))
                g.FillPolygon(brush, poly.ToArray());

            // 3) crisp gold edges (only if we have enough points)
            if (leftPts.Count >= 2)
            {
                using (var pen = new Pen(noteIsSharp ? Color.FromArgb(220, 255, 255, 255) : Color.FromArgb(220, 255, 215, 0), 1f))
                {
                    pen.LineJoin = LineJoin.Round;
                    pen.StartCap = LineCap.Round;
                    pen.EndCap = LineCap.Round;

                    g.DrawLines(pen, leftPts.ToArray());
                    g.DrawLines(pen, rightPts.ToArray());
                }
            }

            g.SmoothingMode = oldSmoothing;
            g.PixelOffsetMode = oldPix;
        }

        private void DrawProKeysNotesRB(Graphics graphics, int startingPosition, int ChartLeft, int trackWidth)
        {
            var track = MIDITools.MIDI_Chart.ProKeys;
            if (track.ChartedNotes.Count == 0) return;

            var renderSize = new Size(1920, 1080);
            var correctedTime = GetCorrectedTime();

            const double minScale = 0.35;
            const double maxScale = 1.00;
            const double passedWindow = 0.05;

            float hitboxY = renderSize.Height - 50f;
            float horizonY = startingPosition + ((hitboxY - startingPosition) * horizonPercent);

            const float hitWindowPx = 10f; // tweak 2..6

            DrawBeatLinesPerspective_FromMarkers(
                graphics,
                correctedTime,
                horizonY,
                hitboxY,
                overshootPx,
                ChartLeft,
                trackWidth,
                PlaybackWindowRB,
                minScale,
                maxScale,
                depthPower
            );

            double ClampMin0(double v) => v < 0 ? 0 : v;
            double EaseIn(double t) => Math.Pow(t, depthPower);
            double Lerp(double a, double b, double t) => a + (b - a) * t;

            // We only need notes within the forward window (assumes sorted by NoteStart)
            var visible = track.ChartedNotes.Where(n => n.NoteStart <= correctedTime + PlaybackWindowRB).ToList();
            if (visible.Count == 0) return;

            // Group by NoteStart (chords)
            var grouped = visible.GroupBy(n => n.NoteStart);

            float trackCenterX = ChartLeft + (trackWidth / 2f);

            foreach (var chord in grouped)
            {
                var chordNotes = chord.ToList();
                double startTime = chord.Key;

                // p for this chord (same shape as 5-lane)
                double tHead = 1.0 - ((startTime - correctedTime) / PlaybackWindowRB);
                tHead = ClampMin0(tHead);
                double pHead = EaseIn(tHead);

                // Perspective span
                //double scaleHead = Lerp(minScale, maxScale, pHead);
                //double spanHead = trackWidth * scaleHead;
                             
                // Y mapping (same as 5-lane)
                float posY = (float)Lerp(horizonY, hitboxY + overshootPx, pHead);

                double PFromY_Notes(float y)
                {
                    double bottomY = hitboxY + overshootPx;  // MUST match your posY mapping
                    double spanY = bottomY - horizonY;
                    if (spanY <= 1) return 1.0;

                    double p = (y - horizonY) / spanY; // IMPORTANT: NO EaseIn here
                    if (p < 0) p = 0;
                    else if (p > 1) p = 1;

                    return p;
                }

                double pAtY = PFromY_Notes(posY);
                double scaleHead = Lerp(minScale, maxScale, pAtY);
                double spanHead = trackWidth * scaleHead;

                // 25 columns
                double keyWidth = spanHead / 25.0;
                double spanLeft = trackCenterX - (spanHead / 2.0);

                // Hitbox proximity flag (for glow)
                bool isAtHitbox = posY >= hitboxY - hitWindowPx;

                // Compute chord bounds in perspective X space (for chord marker)
                double chordLeft = double.MaxValue;
                double chordRight = double.MinValue;

                foreach (var n in chordNotes)
                {
                    int keyIndex = n.NoteNumber - 48; // 0..24
                    double x = spanLeft + (keyWidth * keyIndex);

                    chordLeft = Math.Min(chordLeft, x - (keyWidth * 0.15));
                    chordRight = Math.Max(chordRight, x + keyWidth + (keyWidth * 0.15));
                }

                // Chord marker (unchanged)
                if (chordNotes.Count > 1 && posY <= hitboxY)
                {
                    double chordW = chordRight - chordLeft;
                    graphics.DrawImage(bmpProKeysChordMarker, (float)chordLeft, posY - 4, (float)chordW, 12);
                }

                foreach (var note in chordNotes)
                {
                    if (note.NoteColor == Color.Empty)
                        note.NoteColor = GetNoteColor(note.NoteNumber);

                    int keyIndex = note.NoteNumber - 48; // 0..24
                    bool isSharp = note.NoteName != null && note.NoteName.Contains("#");

                    // Choose gem bitmap AFTER isAtHitbox is known (like 5-lane)
                    Image img;

                    if (isSharp)
                    {
                        if (note.hasOD)
                            img = isAtHitbox ? (bmpProKeysNoteBlackODGlow ?? bmpProKeysNoteBlackOD) : bmpProKeysNoteBlackOD;
                        else
                            img = isAtHitbox ? (bmpProKeysNoteBlackGlow ?? bmpProKeysNoteBlack) : bmpProKeysNoteBlack;
                    }
                    else
                    {
                        if (note.hasOD)
                            img = isAtHitbox ? (bmpProKeysNoteWhiteODGlow ?? bmpProKeysNoteWhiteOD) : bmpProKeysNoteWhiteOD;
                        else
                            img = isAtHitbox ? (bmpProKeysNoteWhiteGlow ?? bmpProKeysNoteWhite) : bmpProKeysNoteWhite;
                    }

                    // Tail color
                    Color tailColor = note.hasOD ? Color.LightGoldenrodYellow : (isSharp ? Color.Black : Color.White);

                    //double posX = spanLeft + (keyWidth * keyIndex);
                    const double gemWidthFactor = 1.0; // try 0.90–0.96
                    double gemW = keyWidth * gemWidthFactor;

                    // Center of this lane
                    double laneCenterX = spanLeft + (keyWidth * (keyIndex + 0.5));

                    // Draw gem centered in lane
                    double posX = laneCenterX - (gemW / 2.0);

                    // Maintain aspect ratio + distance squash
                    double noteHeight = img.Height * (keyWidth / img.Width);
                    double heightScale = Lerp(0.85, 1.00, pHead);
                    noteHeight *= heightScale;

                    // Sustain FIRST
                    if (note.NoteLength >= 1)
                    {
                        DrawProKeysSustainTail_WaveAboveHitbox_FollowLane(
                            graphics,
                            note,
                            tailColor,
                            correctedTime,
                            keyIndex,
                            ChartLeft,
                            trackWidth,
                            horizonY,
                            hitboxY,
                            minScale,
                            maxScale,
                            depthPower,
                            posY - (float)(noteHeight / 2.0),
                            PFromY_Notes,
                            isSharp
                        );
                    }

                    // Gem-only cull (unchanged)
                    if (correctedTime > startTime + passedWindow)
                        continue;

                    // Draw gem only within overshoot region (unchanged)
                    if (posY > hitboxY + overshootPx)
                        continue;
                    if (isAtHitbox)
                    {
                        DrawProKeysHitboxLaneShoot(
                        graphics,
                        note.NoteNumber,
                        trackCenterX,
                        trackWidth,
                        horizonY,
                        hitboxY,
                        overshootPx,
                        minScale,
                        maxScale,
                        keyCount: 25,
                        alpha01: 0.30f,
                        topCutPct: 0.00f
                        );
                    }
                    graphics.DrawImage(
                        img,
                        (float)posX,
                        posY - (float)(noteHeight / 2.0),
                        (float)gemW,
                        (float)noteHeight
                    );
                }
            }
        }       

        private void DrawProKeysHitboxLaneShoot(
            Graphics g,
            int noteNumber,
            float trackCenterX,
            float trackWidth,
            float horizonY,
            float hitboxY,
            float overshootPx,
            double minScale,
            double maxScale,
            int keyCount = 25,
            float alpha01 = 0.50f,
            float topCutPct = 0.15f // 0..1, how far from horizon to stop (avoid going all the way to horizon)
        )
        {
            // Map note -> key index (your mapping)
            int keyIndex = noteNumber - 48; // 48..72 => 0..24
            if (keyIndex < 0 || keyIndex >= keyCount) return;

            // Pick lane color (RB-ish: red/yellow/blue/green/orange repeating)
            Color baseColor = GetProKeysLaneColor(noteNumber); // implement below

            int a = (int)(255f * Math.Max(0f, Math.Min(1f, alpha01)));
            Color fillColor = Color.FromArgb(a, baseColor.R, baseColor.G, baseColor.B);

            double Clamp01(double v) => v < 0 ? 0 : (v > 1 ? 1 : v);
            double Lerp(double a0, double b0, double t) => a0 + (b0 - a0) * t;

            // We want the ribbon from hitbox up toward horizon.
            // We'll draw it between two depth samples: pTop..pBottom (linear p, not eased)
            float bottomY = hitboxY; // clamp to hitbox (not overshoot)
            float topY = horizonY + (hitboxY - horizonY) * topCutPct;

            // Convert y -> p (linear) so span matches your borders math
            double PFromY(float y)
            {
                double denom = (hitboxY - horizonY);
                if (denom <= 1) return 1.0;
                return Clamp01((y - horizonY) / denom);
            }

            void SpanAtY(float y, out double span, out double spanLeft, out double keyW)
            {
                double p = PFromY(y);
                double scale = Lerp(minScale, maxScale, p);
                span = trackWidth * scale;
                keyW = span / keyCount;
                spanLeft = trackCenterX - (span / 2.0);
            }

            // Compute left/right edges for the specific lane at top & bottom
            SpanAtY(topY, out double spanT, out double leftT, out double keyWT);
            SpanAtY(bottomY, out double spanB, out double leftB, out double keyWB);

            float laneLeftTop = (float)(leftT + keyWT * keyIndex);
            float laneRightTop = (float)(leftT + keyWT * (keyIndex + 1));

            float laneLeftBottom = (float)(leftB + keyWB * keyIndex);
            float laneRightBottom = (float)(leftB + keyWB * (keyIndex + 1));

            // Slightly inset so it doesn't overlap your border rails
            const float insetPx = 1.0f;
            laneLeftTop += insetPx;
            laneRightTop -= insetPx;
            laneLeftBottom += insetPx;
            laneRightBottom -= insetPx;

            var poly = new[]
            {
        new PointF(laneLeftTop,    topY),
        new PointF(laneRightTop,   topY),
        new PointF(laneRightBottom,bottomY),
        new PointF(laneLeftBottom, bottomY)
    };

            var oldSM = g.SmoothingMode;
            var oldPO = g.PixelOffsetMode;

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            using (var b = new SolidBrush(fillColor))
                g.FillPolygon(b, poly);

            g.SmoothingMode = oldSM;
            g.PixelOffsetMode = oldPO;
        }

        private Color GetProKeysLaneColor(int noteNumber)
        {
            // Map to lane index 0..24
            int keyIndex = noteNumber - 48;
            if (keyIndex < 0) keyIndex = 0;

            // 5-color repeating
            switch (keyIndex)
            {
                case 0: return Color.Red;
                case 1: return Color.Red;
                case 2: return Color.Red;
                case 3: return Color.Red;
                case 4: return Color.Red;
                case 5: return Color.Yellow;
                case 6: return Color.Yellow;
                case 7: return Color.Yellow;
                case 8: return Color.Yellow;
                case 9: return Color.Yellow;
                case 10: return Color.Yellow;
                case 11: return Color.Yellow;
                case 12: return Color.DodgerBlue;
                case 13: return Color.DodgerBlue;
                case 14: return Color.DodgerBlue;
                case 15: return Color.DodgerBlue;
                case 16: return Color.DodgerBlue;
                case 17: return Color.Green;
                case 18: return Color.Green;
                case 19: return Color.Green;
                case 20: return Color.Green;
                case 21: return Color.Green;
                case 22: return Color.Green;
                case 23: return Color.Green;
                default: return Color.Orange;
            }
        }


        private void DrawProKeysNotes(Graphics graphics, int startingPosition, int ChartLeft, int trackWidth)
        {
            if (MIDITools.MIDI_Chart.ProKeys.ChartedNotes.Count == 0) return;
            var renderSize = new Size(1920, 1080);
            var correctedTime = GetCorrectedTime();
            ChartGoal = renderSize.Height - startingPosition - 50; // Pre-calculated
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

                if (chordNotes.Count() > 1 && posY <= renderSize.Height - 50)
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
                    if (posY > renderSize.Height - 50) continue;

                    // Draw the note
                    graphics.DrawImage(img, (float)posX, (float)(posY - (noteHeight / 2)), (float)noteWidth, (float)noteHeight);                    
                }
            }
        }

        private void DrawSustainTail_WaveAboveHitbox_FollowLane(
            Graphics g,
            MIDINote note,
            Color tailColor,
            double correctedTime,
            int laneIndex,          // 0..4
            int chartLeft,
            int trackWidth,
            float horizonY,
            float hitboxY,
            double minScale,
            double maxScale,
            double depthPower, 
            float yPos
        )
        {
            const float sampleStepY = 4f;     // smaller = smoother ribbon
            const double amplitude = 4.0;
            const double waveYFreq = 0.0125;
            const double grace = 0.05;

            double endTime = note.NoteStart + note.NoteLength;
            if (correctedTime > endTime + grace) return;

            const float tailAttachPadPx = 8f;

            double Clamp01(double v) => v < 0 ? 0 : (v > 1 ? 1 : v);
            double ClampMin0(double v) => v < 0 ? 0 : v;
            double EaseIn(double t) => Math.Pow(t, depthPower);
            double Lerp(double a, double b, double t) => a + (b - a) * t;

            float trackCenterX = chartLeft + (trackWidth / 2f);
            const int lanes = 5;

            // time -> eased progress (used only to compute yHead/yEnd)
            double tHead = 1.0 - ((note.NoteStart - correctedTime) / PlaybackWindowRB);
            double tEnd = 1.0 - ((endTime - correctedTime) / PlaybackWindowRB);
            tHead = ClampMin0(tHead);
            tEnd = ClampMin0(tEnd);

            double pHead = EaseIn(tHead);
            double pEnd = EaseIn(tEnd);

            float YFromP01(double p01) => (float)Lerp(horizonY, hitboxY, Clamp01(p01));

            float yHead = YFromP01(pHead);
            float yEnd = YFromP01(pEnd);

            // ensure end is above head
            if (yEnd > yHead)
            {
                float tmpY = yEnd; yEnd = yHead; yHead = tmpY;
                double tmpP = pEnd; pEnd = pHead; pHead = tmpP;
            }

            if (yEnd >= hitboxY) return; // nothing above hitbox remains

            // ✅ Correct inverse for YFromP01: linear
            double P01FromY(float y)
            {
                double denom = (hitboxY - horizonY);
                if (denom <= 1) return 1.0;
                return Clamp01((y - horizonY) / denom);
            }

            double LaneCenterAtY(float y, out double laneW)
            {
                double p = P01FromY(y);
                double scale = Lerp(minScale, maxScale, p);
                double span = trackWidth * scale;
                laneW = span / lanes;

                double spanLeft = trackCenterX - (span / 2.0);
                return spanLeft + (laneW * (laneIndex + 0.5));
            }

            float TailWidthFromLaneW(double laneW)
            {
                return (float)Math.Max(3.0, Math.Min(6.0, laneW * 0.20));
            }

            // Decide the vertical extent of the ABOVE-hitbox tail
            // If head is above hitbox, go to head+pad (clamped to hitbox).
            // If head has passed hitbox, go to hitbox.
            float top = Math.Max(yEnd, horizonY);
            float bottom = yPos + 3f;
            bottom = Math.Max(horizonY, Math.Min(bottom, hitboxY));

            if (bottom - top <= 1f) return;

            bool doWave = (yHead >= hitboxY); // wave only once head has passed

            var leftPts = new List<PointF>(256);
            var rightPts = new List<PointF>(256);

            // Sample from top->bottom, include exact bottom
            for (float y = top; y <= bottom + 0.001f; y += sampleStepY)
            {
                float yy = (y > bottom) ? bottom : y;

                double laneW;
                double cx = LaneCenterAtY(yy, out laneW);

                float tailW = TailWidthFromLaneW(laneW);
                float halfW = tailW / 2f;

                double waveOffset = 0.0;
                if (doWave)
                {
                    // animate by time and vary by y for a pleasing “sway”
                    double s = Math.Sin((correctedTime * 2.0 + yy * waveYFreq) * Math.PI * 2.0);
                    waveOffset = amplitude * s;

                    // ✅ keep the tail inside the lane
                    double maxWave = Math.Max(0.0, (laneW / 2.0) - halfW - 1.0);
                    if (waveOffset > maxWave) waveOffset = maxWave;
                    if (waveOffset < -maxWave) waveOffset = -maxWave;
                }

                leftPts.Add(new PointF((float)(cx - halfW + waveOffset), yy));
                rightPts.Add(new PointF((float)(cx + halfW + waveOffset), yy));

                if (yy >= bottom) break;
            }

            if (leftPts.Count < 2) return;

            // Build ribbon polygon: left (top->bottom) + right (bottom->top)
            var poly = new List<PointF>(leftPts.Count + rightPts.Count);
            poly.AddRange(leftPts);
            for (int i = rightPts.Count - 1; i >= 0; i--)
                poly.Add(rightPts[i]);

            var oldSmoothing = g.SmoothingMode;
            var oldPix = g.PixelOffsetMode;

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            using (var brush = new SolidBrush(tailColor))
                g.FillPolygon(brush, poly.ToArray());

            g.SmoothingMode = oldSmoothing;
            g.PixelOffsetMode = oldPix;
        }

        private void DrawFiveLaneNotesRB(Graphics graphics, MIDITrack instrument, int startingPosition, int ChartLeft, int trackWidth)
        {
            if (instrument.ChartedNotes.Count == 0) return;

            var renderSize = new Size(1920, 1080);
            var correctedTime = GetCorrectedTime();
                        
            const double minScale = 0.35;   // MUST match background topWidthFactor
            const double maxScale = 1.00;   // 1.00 to match bottom exactly

            float hitboxY = renderSize.Height - 50f;
            float horizonY = startingPosition + ((hitboxY - startingPosition) * horizonPercent);
            const double passedWindow = 0.00;

            DrawBeatLinesPerspective_FromMarkers(
                graphics,
                correctedTime,
                horizonY,
                hitboxY,
                overshootPx,
                ChartLeft,
                trackWidth,
                PlaybackWindowRB,
                minScale,
                maxScale,
                depthPower
            );

            double ClampMin0(double v) => v < 0 ? 0 : v;
            double EaseIn(double t) => Math.Pow(t, depthPower);
            double Lerp(double a, double b, double t) => a + (b - a) * t;

            float trackCenterX = ChartLeft + (trackWidth / 2f);

            foreach (var note in instrument.ChartedNotes)
            {
                if (note.NoteStart > correctedTime + PlaybackWindowRB) break;

                if (note.NoteColor == Color.Empty)
                    note.NoteColor = GetNoteColor(note.NoteNumber);

                int noteLocation;
                if (note.NoteColor == ChartRed) noteLocation = 1;
                else if (note.NoteColor == ChartYellow) noteLocation = 2;
                else if (note.NoteColor == ChartBlue) noteLocation = 3;
                else if (note.NoteColor == ChartOrange) noteLocation = 4;
                else noteLocation = 0;

                // p for the GEM position (0 far -> 1 at hitbox and beyond)
                double tHead = 1.0 - ((note.NoteStart - correctedTime) / PlaybackWindowRB);
                tHead = ClampMin0(tHead);
                double pHead = EaseIn(tHead);

                // Highway span + note geometry AT THIS p (this matches gem placement)
                double scaleHead = Lerp(minScale, maxScale, pHead);
                double spanHead = trackWidth * scaleHead;
                double noteWidth = spanHead / 5.0;

                double spanLeftHead = trackCenterX - (spanHead / 2.0);
                double laneCenterX = spanLeftHead + (noteWidth * (noteLocation + 0.5));
                double posX = laneCenterX - (noteWidth / 2.0);

                // Y mapping (horizon -> hitbox). We clamp gem Y to hitbox for drawing.
                // (Tail logic handles what happens after hit.)
                float posY = (float)Lerp(horizonY, hitboxY + overshootPx, pHead);

                const float hitWindowPx = 10f; // tweak 2..6
                bool isAtHitbox = posY >= hitboxY - hitWindowPx;
                                
                Bitmap img;

                if (note.hasOD)
                {
                    if (note.isHOPOon)
                    {
                        img = isAtHitbox ? bmpODHopoGlow : bmpODHopo;
                    }
                    else
                    {
                        img = isAtHitbox ? bmpNoteODGlow : bmpNoteOD;
                    }
                }
                else
                {
                    if (note.isHOPOon)
                    {
                        // HOPO bitmaps per color
                        if (noteLocation == 0) img = isAtHitbox ? bmpGreenHopoGlow : bmpGreenHopo;
                        else if (noteLocation == 1) img = isAtHitbox ? bmpRedHopoGlow : bmpRedHopo;
                        else if (noteLocation == 2) img = isAtHitbox ? bmpYellowHopoGlow : bmpYellowHopo;
                        else if (noteLocation == 3) img = isAtHitbox ? bmpBlueHopoGlow : bmpBlueHopo;
                        else img = isAtHitbox ? bmpOrangeHopoGlow : bmpOrangeHopo;
                    }
                    else
                    {
                        // Normal note bitmaps per color
                        if (noteLocation == 0) img = isAtHitbox ? bmpNoteGreenGlow : bmpNoteGreen;
                        else if (noteLocation == 1) img = isAtHitbox ? bmpNoteRedGlow : bmpNoteRed;
                        else if (noteLocation == 2) img = isAtHitbox ? bmpNoteYellowGlow : bmpNoteYellow;
                        else if (noteLocation == 3) img = isAtHitbox ? bmpNoteBlueGlow : bmpNoteBlue;
                        else img = isAtHitbox ? bmpNoteOrangeGlow : bmpNoteOrange;
                    }
                }

                // Sustain tail color
                Color tailColor = note.hasOD ? Color.White : note.NoteColor;
                var noteHeight = img.Height * (noteWidth / img.Width);

                if (note.NoteLength >= 1)
                {
                    DrawSustainTail_WaveAboveHitbox_FollowLane(
                   graphics, note, tailColor, correctedTime,
                   noteLocation, ChartLeft, trackWidth,
                   horizonY, hitboxY,
                   minScale, maxScale, depthPower, posY - (float)(noteHeight / 2.0)   );
                }

                // GEM cull so it doesn't "stick"
                if (correctedTime > note.NoteStart + passedWindow) continue;

                // Draw gem only while it's within the overshoot region
                if (posY > hitboxY + overshootPx) continue;
                graphics.DrawImage(img, (float)posX, posY - (float)(noteHeight / 2.0), (float)noteWidth, (float)noteHeight);
            }
        }
      
        private void DrawFiveLaneNotes(Graphics graphics, MIDITrack instrument, int startingPosition, int ChartLeft, int trackWidth)
        {
            if (instrument.ChartedNotes.Count == 0) return;
            var renderSize = new Size(1920, 1080);
            var correctedTime = GetCorrectedTime();
            ChartGoal = renderSize.Height - startingPosition - 50; // Pre-calculated
        
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
                Bitmap img;
                
                if (note.hasOD)
                {
                    // Overdrive cases
                    if (note.NoteColor == ChartRed)
                    {
                        noteLocation = 1;
                        img = note.isHOPOon ? bmpODHopo : bmpNoteOD;
                    }
                    else if (note.NoteColor == ChartYellow)
                    {
                        noteLocation = 2;
                        img = note.isHOPOon ? bmpODHopo : bmpNoteOD;
                    }
                    else if (note.NoteColor == ChartBlue)
                    {
                        noteLocation = 3;
                        img = note.isHOPOon ? bmpODHopo : bmpNoteOD;
                    }
                    else if (note.NoteColor == ChartOrange)
                    {
                        noteLocation = 4;
                        img = note.isHOPOon ? bmpODHopo : bmpNoteOD;
                    }
                    else // fallback (green)
                    {
                        noteLocation = 0;
                        img = note.isHOPOon ? bmpODHopo : bmpNoteOD;
                    }
                }
                else
                {
                    // Non-OD cases
                    if (note.NoteColor == ChartRed)
                    {
                        noteLocation = 1;
                        img = note.isHOPOon ? bmpRedHopo : bmpNoteRed;
                    }
                    else if (note.NoteColor == ChartYellow)
                    {
                        noteLocation = 2;
                        img = note.isHOPOon ? bmpYellowHopo : bmpNoteYellow;
                    }
                    else if (note.NoteColor == ChartBlue)
                    {
                        noteLocation = 3;
                        img = note.isHOPOon ? bmpBlueHopo : bmpNoteBlue;
                    }
                    else if (note.NoteColor == ChartOrange)
                    {
                        noteLocation = 4;
                        img = note.isHOPOon ? bmpOrangeHopo : bmpNoteOrange;
                    }
                    else // fallback (green)
                    {
                        noteLocation = 0;
                        img = note.isHOPOon ? bmpGreenHopo : bmpNoteGreen;
                    }
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
                if (posY > renderSize.Height - 50) continue;
                graphics.DrawImage(img, (float)posX, (float)(posY - (noteHeight / 2)), (float)noteWidth, (float)noteHeight);                
            }
        }

        private void DrawSustainTail(Graphics graphics, MIDINote note, Color tailColor, double correctedTime, double posY, double posX, double noteWidth, int startingPosition)
        {
            var renderSize = new Size(1920, 1080);

            // Calculate the end position of the sustain tail
            var tailEndPercent = 1.0 - (((note.NoteStart + note.NoteLength - 0.6) - correctedTime) / PlaybackWindowRB);
            var tailEndY = startingPosition + (ChartGoal * tailEndPercent);

            // Ensure the tail extends correctly above the note
            if (tailEndY < startingPosition) tailEndY = startingPosition;

            // Prevent the tail from being clipped prematurely
            if (tailEndY > renderSize.Height - 50)
            {
                tailEndY = renderSize.Height - 50;
            }

            // Ensure the tail continues to display even when the note hits the hitbox
            if (tailEndY > renderSize.Height - 50)
            {
                tailEndY = renderSize.Height - 50;
            }

            // Split the tail into a static and dynamic part
            var splitY = Math.Min(renderSize.Height - 50, posY);

            // Draw the static part of the tail (straight line above the hitbox)
            if (posY <= renderSize.Height - 50 && splitY > tailEndY)
            {
                using (var tailBrush = new SolidBrush(tailColor))
                {
                    graphics.FillRectangle(tailBrush, (float)(posX + (noteWidth / 2) - 3), (float)tailEndY, 6, (float)(splitY - tailEndY));
                }
            }

            // Draw the dynamic part of the tail (moving sine wave below the hitbox)
            if (posY > renderSize.Height - 50 && tailEndY < splitY)
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

        private void DrawDrumNotesRB(Graphics graphics, bool doKicks, int startingPosition, int ChartLeft, int trackWidth)
        {
            var track = MIDITools.MIDI_Chart.Drums;
            if (track.ChartedNotes.Count == 0) return;

            var renderSize = new Size(1920, 1080);
            var correctedTime = GetCorrectedTime();

            const double minScale = 0.35;
            const double maxScale = 1.00;

            // Hitbox / horizon mapping matches 5-lane now
            float hitboxY = renderSize.Height - 50f;
            float horizonY = startingPosition + ((hitboxY - startingPosition) * horizonPercent);

            // Same idea as 5-lane
            const double passedWindow = 0.00; // tweak if we want a tiny "linger"
            const float hitWindowPx = 10f;     // tweak 2..6

            DrawBeatLinesPerspective_FromMarkers(
                graphics,
                correctedTime,
                horizonY,
                hitboxY,
                overshootPx,
                ChartLeft,
                trackWidth,
                PlaybackWindowRB,
                minScale,
                maxScale,
                depthPower
            );

            double ClampMin0(double v) => v < 0 ? 0 : v;
            double EaseIn(double t) => Math.Pow(t, depthPower);
            double Lerp(double a, double b, double t) => a + (b - a) * t;

            // 4-lane drums (red/yellow/blue/green)
            const int lanes = 4;
            double trackCenterX = ChartLeft + (trackWidth / 2.0);

            foreach (var note in track.ChartedNotes)
            {
                if (note.NoteStart > correctedTime + PlaybackWindowRB) break; // assumes sorted

                if (note.NoteColor == Color.Empty)
                    note.NoteColor = GetNoteColor(note.NoteNumber, true);

                // Filter kicks vs pads
                if (note.NoteColor == ChartOrange && !doKicks) continue;
                if (note.NoteColor != ChartOrange && doKicks) continue;

                // ---- Perspective timing (same shape as 5-lane) ----
                double tHead = 1.0 - ((note.NoteStart - correctedTime) / PlaybackWindowRB);
                tHead = ClampMin0(tHead);
                double pHead = EaseIn(tHead);

                double scaleHead = Lerp(minScale, maxScale, pHead);
                double laneSpan = trackWidth * scaleHead;
                double noteWidth = laneSpan / lanes;

                // Map Y using shared overshootPx (same as 5-lane)
                float posY = (float)Lerp(horizonY, hitboxY + overshootPx, pHead);

                // Only draw within overshoot region
                if (posY > hitboxY + overshootPx) continue;

                bool isAtHitbox = posY >= hitboxY - hitWindowPx;

                // ---- KICKS (rect) ----
                if (note.NoteColor == ChartOrange)
                {
                    // Kick lane scales too
                    var multiplier = isAtHitbox ? 4 : 1;
                    float kickHeight = (float)(KICK_HEIGHT * multiplier * Lerp(0.7, 1.0, pHead));

                    using (var solidBrush = new SolidBrush(note.hasOD ? Color.WhiteSmoke : Color.FromArgb(255, 180, 28)))
                    {
                        graphics.FillRectangle(
                            solidBrush,
                            (float)(trackCenterX - laneSpan / 2.0),
                            posY,
                            (float)laneSpan,
                            kickHeight
                        );
                    }

                    // optional: "no stick" cull behavior for kicks too
                    if (correctedTime > note.NoteStart + passedWindow) continue;

                    continue;
                }

                // ---- Lane index for pads/cymbals ----
                int noteLocation;
                if (note.NoteColor == ChartRed) noteLocation = 0;
                else if (note.NoteColor == ChartYellow) noteLocation = 1;
                else if (note.NoteColor == ChartBlue) noteLocation = 2;
                else noteLocation = 3; // green

                double laneCenterX = (trackCenterX - (laneSpan / 2.0)) + (noteWidth * (noteLocation + 0.5));
                double drawX = laneCenterX - (noteWidth / 2.0);

                // ---- Bitmap selection AFTER isAtHitbox is known ----
                Image img;

                bool isCymbal = !note.isTom;
                bool isOD = note.hasOD;

                if (isCymbal)
                {
                    // Provide safe fallback if a glow asset doesn't exist yet.
                    if (note.NoteColor == ChartYellow)
                        img = isOD ? (isAtHitbox ? (bmpDrumsCymbalODGlow ?? bmpDrumsCymbalOD) : bmpDrumsCymbalOD)
                                   : (isAtHitbox ? (bmpDrumsCymbalYGlow ?? bmpDrumsCymbalY) : bmpDrumsCymbalY);
                    else if (note.NoteColor == ChartBlue)
                        img = isOD ? (isAtHitbox ? (bmpDrumsCymbalODGlow ?? bmpDrumsCymbalOD) : bmpDrumsCymbalOD)
                                   : (isAtHitbox ? (bmpDrumsCymbalBGlow ?? bmpDrumsCymbalB) : bmpDrumsCymbalB);
                    else if (note.NoteColor == ChartGreen)
                        img = isOD ? (isAtHitbox ? (bmpDrumsCymbalODGlow ?? bmpDrumsCymbalOD) : bmpDrumsCymbalOD)
                                   : (isAtHitbox ? (bmpDrumsCymbalGGlow ?? bmpDrumsCymbalG) : bmpDrumsCymbalG);
                    else
                        // Red cymbal not typical; fall back to red pad
                        img = isOD ? (isAtHitbox ? bmpNoteODGlow : bmpNoteOD)
                                   : (isAtHitbox ? bmpNoteRedGlow : bmpNoteRed);
                }
                else
                {
                    if (note.NoteColor == ChartRed)
                        img = isOD ? (isAtHitbox ? bmpNoteODGlow : bmpNoteOD)
                                   : (isAtHitbox ? bmpNoteRedGlow : bmpNoteRed);
                    else if (note.NoteColor == ChartYellow)
                        img = isOD ? (isAtHitbox ? bmpNoteODGlow : bmpNoteOD)
                                   : (isAtHitbox ? bmpNoteYellowGlow : bmpNoteYellow);
                    else if (note.NoteColor == ChartBlue)
                        img = isOD ? (isAtHitbox ? bmpNoteODGlow : bmpNoteOD)
                                   : (isAtHitbox ? bmpNoteBlueGlow : bmpNoteBlue);
                    else // green
                        img = isOD ? (isAtHitbox ? bmpNoteODGlow : bmpNoteOD)
                                   : (isAtHitbox ? bmpNoteGreenGlow : bmpNoteGreen);
                }

                // "Don't stick" cull (match 5-lane behavior)
                if (correctedTime > note.NoteStart + passedWindow) continue;

                // Maintain aspect ratio + slight vertical squash in the distance
                double noteHeight = img.Height * (noteWidth / img.Width);
                double heightScale = Lerp(0.85, 1.00, pHead);
                noteHeight *= heightScale;

                graphics.DrawImage(
                    img,
                    (float)drawX,
                    (float)(posY - (noteHeight / 2.0)),
                    (float)noteWidth,
                    (float)noteHeight
                );
            }
        }

        private void DrawDrumNotes(Graphics graphics, bool doKicks, int startingPosition, int ChartLeft, int trackWidth)
        {
            if (MIDITools.MIDI_Chart.Drums.ChartedNotes.Count == 0) return;
            var renderSize = new Size(1920, 1080);
            var track = MIDITools.MIDI_Chart.Drums;
            var correctedTime = GetCorrectedTime();
            ChartGoal = renderSize.Height - startingPosition - 50; // Pre-calculated

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
                if (posY > renderSize.Height - 50) continue; //only draw until the hit box
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
            }
        }

        private int GetYForRBVocals()
        {
            if (rBStyle.Checked && !doMIDINoVocals)
            {
                if (doMIDIHarmonies)
                {
                    if (MIDITools.LyricsHarm3 != null && MIDITools.LyricsHarm3.Lyrics.Any())
                    {
                        return 48;
                    }
                    if (MIDITools.LyricsHarm2 != null && MIDITools.LyricsHarm2.Lyrics.Any())
                    {
                        return 24;
                    }
                }
                else
                {
                    return 0;
                }
            }
            return 0;
        }

        private void DrawLyrics(Size size, Graphics graphics, Color backColor)
        {
            if (!openSideWindow.Checked && secondScreen == null) return;
            if ((!doStaticLyrics && !doScrollingLyrics && !doKaraokeLyrics) || !MIDITools.PhrasesVocals.Phrases.Any())
            {
                return;
            }

            var phrases = doHarmonyLyrics && MIDITools.PhrasesHarm1.Phrases.Any() ? MIDITools.PhrasesHarm1.Phrases : MIDITools.PhrasesVocals.Phrases;
            var lyrics = doHarmonyLyrics && MIDITools.LyricsHarm1.Lyrics.Any() ? MIDITools.LyricsHarm1.Lyrics : MIDITools.LyricsVocals.Lyrics;
            var font = new Font("Segoe UI", 16f, FontStyle.Bold); //new Font("Segoe UI", 12f);
            var harm1Y = size.Height - (MIDITools.LyricsHarm3.Lyrics.Any() ? 100 : (MIDITools.LyricsHarm2.Lyrics.Any() ? 70 : 40));
            var harm2Y = size.Height - (MIDITools.LyricsHarm3.Lyrics.Any() ? 70 : 40);
            var harm3Y = size.Height - 40;
            if (!doHarmonyLyrics || (doHarmonyLyrics && !MIDITools.LyricsHarm2.Lyrics.Any()))
            {
                harm1Y = harm3Y;
            }
            if ((chartVertical.Checked || rBStyle.Checked) && chartVisualsToolStripMenuItem.Checked)
            {
                if (rBStyle.Checked && doMIDINoVocals)
                {
                    harm1Y = 4;
                    harm2Y = 28;
                    harm3Y = 52;
                }
                else
                {
                    harm1Y = GetHeightDiff() + 4;
                    harm2Y = GetHeightDiff() + 28;
                    harm3Y = GetHeightDiff() + 52;
                }                
            }
            if (rBStyle.Checked && !doMIDINoVocals)
            {
                if (doMIDIHarmonies)
                {
                    if (MIDITools.LyricsHarm3 != null && MIDITools.LyricsHarm3.Lyrics.Any())
                    {
                        harm3Y = 0;
                        harm2Y = 24;
                        harm1Y = vocalsHeight + (harm2Y * 2);
                    }
                    else if (MIDITools.LyricsHarm2 != null && MIDITools.LyricsHarm2.Lyrics.Any())
                    {
                        harm2Y = 0;
                        harm1Y = vocalsHeight + 24;
                    }
                }
                else
                {
                    harm1Y = vocalsHeight + 0;
                }                
            }
            if (doScrollingLyrics)
            {
                var isGameChart = chartVisualsToolStripMenuItem.Checked && (chartVertical.Checked || rBStyle.Checked || chartSnippet.Checked);
                if (doHarmonyLyrics)
                {                    //isGameChart? Harm3Color : Color.WhiteSmoke
                    DrawLyricsScrolling(MIDITools.LyricsHarm3.Lyrics, font, Harm3Color, backColor, harm3Y, graphics);//Harm3Color
                    DrawLyricsScrolling(MIDITools.LyricsHarm2.Lyrics, font, Harm2Color, backColor, harm2Y, graphics);//Harm2Color
                }// doHarmonyLyrics || doMIDIHarm1onVocals ? isGameChart ? Harm1Color : Color.WhiteSmoke : Color.White
                DrawLyricsScrolling(lyrics, font,Harm1Color, backColor, harm1Y, graphics);//Harm1Color
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
                    DrawLyricsStatic(MIDITools.PhrasesHarm3.Phrases, font, Color.White, backColor, harm3Y, graphics);
                    DrawLyricsStatic(MIDITools.PhrasesHarm2.Phrases, font, Color.White, backColor, harm2Y, graphics);
                }//doHarmonyLyrics || doMIDIHarm1onVocals ? Harm1Color : Color.White
                DrawLyricsStatic(phrases, font, Color.White, backColor, harm1Y, graphics);
            }
        }        

        private void DrawLyricsStatic(IEnumerable<LyricPhrase> phrases, Font font, Color foreColor, Color backColor, int posY, Graphics graphics)
        {
            var renderSize = new Size(1920, 1080);
            if (phrases == null || phrases.Count() == 0) return;
            var time = GetCorrectedTime();
            graphics.DrawImage(Resources.frostedglass75, 0, posY, renderSize.Width, 24);
            // 50% opacity = alpha 128 (out of 255)
            using (var overlayBrush = new SolidBrush(
                Color.FromArgb(chartVertical.Checked ? 255 : 128, foreColor)))
            {
                graphics.FillRectangle(
                    overlayBrush,
                    0,
                    posY,
                    renderSize.Width,
                    24
                );
            }
            using (var overlayBrush = new SolidBrush(Color.LightGray))
            {
                graphics.FillRectangle(overlayBrush, 0, posY - 1, renderSize.Width, 1);
            }
            using (var overlayBrush = new SolidBrush(Color.LightGray))
            {
                graphics.FillRectangle(overlayBrush, 0, posY + 24, renderSize.Width, 1);
            }
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
            var processedLine = ProcessLine(line, doWholeWordsLyrics).Replace("‿", " ");
            var lineSize = graphics.MeasureString(processedLine, font);
            var left = (renderSize.Width - (int)lineSize.Width) / 2;

            using (var textBrush = new SolidBrush(foreColor))// chartVisualsToolStripMenuItem.Checked && chartVertical.Checked ? Color.White : Color.Black))
            {
                graphics.DrawString(processedLine, font, textBrush, new PointF(left, posY - 6));
            }
        }

        private void InitBASS()
        {
            //initialize BASS            
            if (Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero))
            {
                Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_BUFFER, BassBuffer);
                Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_UPDATEPERIOD, 50);
            }
            else
            {
                MessageBox.Show("Error initializing BASS\n" + Bass.BASS_ErrorGetCode(), AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool PrepMixerRB3(bool isM4A = false)
        {
            BassStreams.Clear();
            Bass.BASS_ChannelFree(BassStream);
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
                if (BassStream == 0)
                {
                    MessageBox.Show("Failed to process that stream, can't play song", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }

                // create a decoder for the input file(s)
                var channel_info = Bass.BASS_ChannelGetInfo(BassStream);

                // create a stereo mixer with same frequency rate as the input file(s)
                BassMixer = BassMix.BASS_Mixer_StreamCreate(channel_info.freq, 2, BASSFlag.BASS_MIXER_END | BASSFlag.BASS_SAMPLE_FLOAT);
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
                return false;
            }
            return true;
        }

        double ActiveSongDuration = 0.0;
        private bool PrepMixerPS(IList<string> audioFiles, out int mixer, out List<int> NextSongStreams)
        {
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
                mixer = 0;
                NextSongStreams = null;
                return false;
            }
            mixer = BassMixer;
            NextSongStreams = BassStreams;
            return true;
        }

        private void AddAudioToMixer(string audioFile)
        {
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

        private bool SafeChecked(ToolStripMenuItem item)
        {
            if (InvokeRequired)
            {
                return (bool)Invoke(new Func<bool>(() => item.Checked));
            }
            else
            {
                return item.Checked;
            }
        }

        private void StartPlayback(bool doFade, bool doNext, bool PlayAudio = true)
        {            
            if (PlayAudio)
            {
                if ((!yarg.Checked && !fortNite.Checked && !guitarHero.Checked && !powerGig.Checked && !bandFuse.Checked) && (CurrentSongAudio == null || CurrentSongAudio.Length == 0))
                {
                    if (AlreadyTried || lstPlaylist.SelectedItems.Count == 0)
                    {
                        var msg = "Audio file (*.mogg) for song '" + PlayingSong.Artist + " - " + PlayingSong.Name + "' is missing";
                        MessageBox.Show(msg, AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        StopPlayback();
                        AlreadyTried = false;
                    }
                    else
                    {
                        AlreadyTried = true;
                        doSongPlayback();
                    }
                    return;
                }

                var directory = Path.GetDirectoryName(PlayingSong.Location);
                if (yarg.Checked && !string.IsNullOrEmpty(sngPath))
                {
                    directory = Application.StartupPath + "\\temp\\";
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
                    directory = Application.StartupPath + "\\temp\\";
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
                else if ((SafeChecked(yarg) || SafeChecked(fortNite) || SafeChecked(guitarHero) || SafeChecked(powerGig) || SafeChecked(bandFuse)) && (oggFiles.Any() || opusFiles.Any() || mp3Files.Any() || wavFiles.Any()))
                {
                    List<string> AudioFiles;
                    if (opusFiles.Any())
                    {
                        if (!opusFiles.Any())
                        {
                            var msg = "Audio files (*.opus) for song '" + PlayingSong.Artist + " - " + PlayingSong.Name + "' are missing";
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

                if (yarg.Checked)
                {
                    GetIntroOutroSilencePS();
                }
                else
                {
                    GetIntroOutroSilence();
                }

                if (PlaybackSeconds == 0 && skipIntroOutroSilence.Checked && IntroSilence > - 1)
                {
                    PlaybackSeconds = IntroSilence;
                }

                SetPlayLocation(PlaybackSeconds);

                //apply volume correction to entire track
                var track_vol = (float)Utils.DBToLevel(Convert.ToDouble(-1 * (MinVolume - VolumeLevel)), 1.0);
                if (doFade) //enable fade-in
                {
                    Bass.BASS_ChannelSetAttribute(BassMixer, BASSAttribute.BASS_ATTRIB_VOL, 0);
                    Bass.BASS_ChannelSlideAttribute(BassMixer, BASSAttribute.BASS_ATTRIB_VOL, track_vol, (int)(FadeLength * 1000));
                }
                else //no fade-in
                {
                    Bass.BASS_ChannelSetAttribute(BassMixer, BASSAttribute.BASS_ATTRIB_VOL, track_vol);
                }

                //start video playback if possible
                if (yarg.Checked) // && displayBackgroundVideo.Checked)
                {
                    StartVideoPlayback();
                }
                //start mix playback
                if (!Bass.BASS_ChannelPlay(BassMixer, false))
                {
                    MessageBox.Show("Error starting BASS playback:\n" + Bass.BASS_ErrorGetCode(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }                
            }

            if (isRBKaraoke())
            {
                picVisuals.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            PrepareForDrawing();
            UpdatePlaybackStuff();
            UpdateStats();
            stageTimer.Enabled = (isRBKaraoke() && animatedBackground.Checked) || (classicKaraokeMode.Checked && animatedBackground2.Checked);
            LargeAlbumArt = File.Exists(CurrentSongArtBlurred) ? Tools.NemoLoadImage(CurrentSongArtBlurred) : null;
            if (displayAlbumArt.Checked && LargeAlbumArt != null)
            {
                Color bgColor = Color.AliceBlue;
                using (var bmp = (Bitmap)Image.FromFile(CurrentSongArtBlurred))
                {
                    bgColor = Tools.GetMoodBackgroundFromBlurred(bmp, Color.AliceBlue, 28);
                }

                if (secondScreen != null)
                {
                    secondScreen.ChangeBackgroundImage(LargeAlbumArt, true);
                    secondScreen.ChangeBackgroundColor(bgColor);
                    picVisuals.BackColor = Color.AliceBlue;
                }
                else
                {
                    picVisuals.Image = LargeAlbumArt;
                    picVisuals.SizeMode = PictureBoxSizeMode.Zoom;
                    picVisuals.BackColor = bgColor;
                }
            }
            else
            {
                var image = isRBKaraoke() && staticBackground.Checked ? stageBackground : null;
                if (secondScreen != null)
                {
                    secondScreen.ChangeBackgroundImage(image);
                }
                else
                {
                    picVisuals.Image = image;
                }
            }

            var updatedFavorite = false;
            foreach (var favorite in favoritesList)
            {
                if (favorite.SongPath.Equals(PlayingSong.Location))
                {
                    favorite.PlayTimes++;
                    updatedFavorite = true;
                    break;
                }
            }
            if (!updatedFavorite)
            {
                var newFavorite = new FavoriteSong() { SongPath = PlayingSong.Location, PlayTimes = 1 };
                favoritesList.Add(newFavorite);
            }
            var sw = new StreamWriter(Application.StartupPath + "\\bin\\favorites", false);
            sw.WriteLine("FavoritesCount=" + favoritesList.Count());
            foreach (var favorite in favoritesList)
            {
                sw.WriteLine("SongPath=" + favorite.SongPath);
                sw.WriteLine("PlayCount=" + favorite.PlayTimes);
            }
            sw.Dispose();

            if (displayKaraokeMode.Checked && classicKaraokeMode.Checked && solidColorBackground.Checked)
            {
                picVisuals.Image = Resources.gradient;
            }

            try
            {
                _beatMarkers = BuildBeatMarkers_UseGetRealtime(MIDITools.LengthLong, MIDITools.TicksPerQuarter, MIDITools.TimeSignatures);
            }
            catch { }
        }

        private void SetVideoPlayerPath(string ini)
        {
            _mediaPlayer.Media = null;
                        
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
            CHVideoPath = video_path;
            if (yarg.Checked && (string.IsNullOrEmpty(CHVideoPath) && rBStyle.Checked))
            {
                ChangeRBStyleBackground();
                return;
            }
            if (string.IsNullOrEmpty(video_path)) return;
            //StartVideoPlayback(video_path, VideoPathType.FromPath, 0);
            var media = new Media(_libVLC, CHVideoPath, FromType.FromPath);
            if (media == null) return;
            _mediaPlayer.Media = media;
        }

        private void StartVideoPlayback()
        {
            if (PlayingSong == null) return;
            if (_mediaPlayer.Media == null)
            {
                SetVideoPlayerPath(string.IsNullOrEmpty(sngPath) ? PlayingSong.Location : Application.StartupPath + "\\temp\\song.ini");
            }
            if (_mediaPlayer.Media == null) return;
            VideoIsPlaying = true;
            ClearVisuals();
            videoView.Visible = true;
            videoView.BringToFront();
            _mediaPlayer.Play();
            if (_mediaPlayer.IsSeekable)
            {
                _mediaPlayer.Time = (long)(PlaybackSeconds * 1000) + Parser.Songs[0].VideoStartTime;
            }               
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
            if (_mediaPlayer.State == VLCState.Playing || _mediaPlayer.State == VLCState.Paused && !seeking)
            {
                _mediaPlayer.Time = (long)(time * 1000) + (Parser.Songs == null ? 0 : Parser.Songs[0].VideoStartTime);
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
                    if (!Bass.BASS_ChannelPause(BassMixer))
                    {
                        MessageBox.Show("Error pausing playback\n" + Bass.BASS_ErrorGetCode());
                    }
                    if (_mediaPlayer.State == VLCState.Playing)
                    {
                        _mediaPlayer.Pause();
                    }
                    if (secondScreen != null && secondScreen._mediaPlayer.State == VLCState.Playing)
                    {
                        secondScreen._mediaPlayer.Pause();
                    }
                }
                else
                {
                    StopAllVideoPlayback();
                    StopBASS();
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        private int _stopInProgress;

        private void StopVideoPlayback(bool stop = true)
        {
            if (Interlocked.Exchange(ref _stopInProgress, 1) == 1)
                return;

            var mp = _mediaPlayer;
            if (mp == null) { _stopInProgress = 0; return; }

            ClearOverlayFrame();
            ChangeBackgroundImage(Resources.logo);
            videoView.Visible = false;
            VideoIsPlaying = false;

            Task.Run(() =>
            {
                try
                {
                    if (stop)
                    {
                        try { mp.Media = null; } catch { }
                        try { mp.Stop(); } catch { }
                    }
                    else
                    {
                        try { mp.Pause(); } catch { }
                    }
                }
                finally
                {
                    Interlocked.Exchange(ref _stopInProgress, 0);
                }
            });
        }        

        public void ChangeBackgroundImage(Image image, bool zoom = false)
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

        public void ClearOverlayFrame()
        {
            if (videoOverlay == null || !openSideWindow.Checked) return;

            // Create a transparent bitmap and push it through the same pipeline
            using (var bmp = new Bitmap(picVisuals.Width, picVisuals.Height, PixelFormat.Format32bppPArgb))
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent);
                try 
                { 
                    videoOverlay.UpdateVisuals(bmp); 
                } 
                catch { }
            }
        }

        private void StopBASS()
        {
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
            { }
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

        private Image LargeAlbumArt = null;
        private Image OriginalAlbumArt = null;
        public void UpdateDisplay(bool PrepareToDraw = true)
        {
            if (isClosing) return;
            if (PrepareToDraw)
            {
                PrepareForDrawing();
            }
            var doShow = openSideWindow.Checked;
            Width = doShow ? 1000 : 412;
            LargeAlbumArt = File.Exists(CurrentSongArtBlurred) ? Tools.NemoLoadImage(CurrentSongArtBlurred) : null;
            OriginalAlbumArt = File.Exists(CurrentSongArt) ? Tools.NemoLoadImage(CurrentSongArt) : null;
            var image = displayAlbumArt.Checked ? LargeAlbumArt : null;
            if (secondScreen != null)
            {
                secondScreen.ChangeBackgroundImage(image, displayAlbumArt.Checked);
            }
            else
            {
                picVisuals.Image = image;
            }
            if (secondScreen == null)
            {
                lblSections.Parent = picVisuals;
                lblSections.Visible = showPracticeSections.Checked && MIDITools.PracticeSessions.Any() && !chartVertical.Checked;
                lblSections.BackColor = yarg.Checked && displayBackgroundVideo.Checked && _mediaPlayer.Media != null ? Color.Black : LabelBackgroundColor;
                lblSections.Refresh();

                videoView.Parent = picVisuals;
                videoView.Top = lblSections.Visible ? lblSections.Height : 0;
                videoView.Left = 0;
                videoView.Height = picVisuals.Height - GetHeightDiff();
                videoView.Width = picVisuals.Width;
            }
        }

        private int GetHeightDiff()
        {
            if (doMIDINoVocals && !rBStyle.Checked)
            {
                return 4;
            }
            if (((chartVisualsToolStripMenuItem.Checked && chartVertical.Checked) && MIDITools.LyricsVocals.Lyrics.Any()) || (chartVisualsToolStripMenuItem.Checked && rBStyle.Checked))
            {
                return vocalsHeight + 4;
            }
            var heightDiff = 0;
            if (lblSections.Visible && !chartVertical.Checked && !rBStyle.Checked)
            {
                heightDiff += lblSections.Height;
            }
            if (doScrollingLyrics || doStaticLyrics || doKaraokeLyrics || rBStyle.Checked)
            {
                if (doHarmonyLyrics || rBStyle.Checked)
                {
                    heightDiff += MIDITools.LyricsHarm3.Lyrics.Any() ? 60 : (MIDITools.LyricsHarm2.Lyrics.Any() ? 40 : 20);
                }
                else if (MIDITools.LyricsVocals.Lyrics.Any() || rBStyle.Checked)
                {
                    heightDiff += 20;
                }
            }
            return heightDiff;
        }

        private static void UpdateTextQuality(Graphics graphics)
        {
            graphics.SmoothingMode = SmoothingMode.None;
            graphics.InterpolationMode = InterpolationMode.Low;
            graphics.PixelOffsetMode = PixelOffsetMode.Half;
            graphics.CompositingQuality = CompositingQuality.AssumeLinear;
            graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
        }

        private void howToUseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var hub = new frmHelpHub();
            hub.Show();
            hub.ClickWelcome();
        }

        private void folderScanner_DoWork(object sender, DoWorkEventArgs e)
        {
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
            else if (rb4PS4.Checked)
            {               
                SongsToAdd.AddRange(files.Where(file => Path.GetExtension(file) == ".songdta_ps4").ToList());
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
            if (GIFOverlay != null)
            {
                GIFOverlay.Close();
                GIFOverlay = null;
            }
            var type = GetCurrentDataType();
            if (!SongsToAdd.Any())
            {
                var msg = "No " + type + " files found in that folder, nothing to add to the playlist";
                MessageBox.Show(msg, AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                isScanning = false;
                EnableDisable(true);
                return;
            }
            var found = "Found " + SongsToAdd.Count + " " + type + " " + (SongsToAdd.Count == 1 ? "file" : "files") + ", analyzing...";
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
        }

        private void openFileLocation_Click(object sender, EventArgs e)
        {
            try
            {
                var file = ActiveSong.Location;
                Process.Start("explorer" + EXE, "/select," + "\"" + file + "\"");
            }
            catch
            {
                MessageBox.Show("There was an error trying to do that", "cPlayer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            UpdateStats();
            UpdateTimer.Enabled = false;
        }

        private void UpdateStats()
        {
            statusLabel.Text = "";
            if (lstPlaylist.Items.Count == 0) return;

            try
            {
                long time = 0;
                for (var i = 0; i < lstPlaylist.Items.Count; i++)
                {
                    var ind = Convert.ToInt16(lstPlaylist.Items[i].SubItems[0].Text) - 1;
                    time += Playlist[ind].Length;
                }
                statusLabel.Text = "Songs: " + lstPlaylist.Items.Count;
                if (openSideWindow.Checked && string.IsNullOrEmpty(activeM4AFile))
                {
                    statusLabel.Text = statusLabel.Text + "   |   Playing Time: " + FormatDuration(time);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static string FormatDuration(long milliseconds)
        {
            if (milliseconds < 0)
                milliseconds = 0;

            var ts = TimeSpan.FromMilliseconds(milliseconds);

            var parts = new List<string>();

            if (ts.Days > 0)
                parts.Add($"{ts.Days} day{(ts.Days == 1 ? "" : "s")}");

            if (ts.Hours > 0)
                parts.Add($"{ts.Hours} hour{(ts.Hours == 1 ? "" : "s")}");

            if (ts.Minutes > 0)
                parts.Add($"{ts.Minutes} minute{(ts.Minutes == 1 ? "" : "s")}");

            if (ts.Seconds > 0 || parts.Count == 0)
                parts.Add($"{ts.Seconds} second{(ts.Seconds == 1 ? "" : "s")}");

            return string.Join(" ", parts);
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
            { }
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
            Playlist = StaticPlaylist;
            lblClearSearch_MouseClick(null, null);
            ReloadPlaylist(Playlist, true, true, false);
            UpdateHighlights();
        }

        private void txtSearch_EnabledChanged(object sender, EventArgs e)
        {
            lblClearSearch.Enabled = txtSearch.Enabled;
            picSearch.Enabled = txtSearch.Enabled;
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

            var ofd = new OpenFileDialog
            {
                Title = "Select files to add to playlist",
                Multiselect = true,
                InitialDirectory = Environment.CurrentDirectory
            };
            if (ofd.ShowDialog() != DialogResult.OK)
            {
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
                MessageBox.Show(msg, AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                EnableDisable(true);
                ofd.Dispose();
                return;
            }
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

        private void UpdateSecondScreenAvailability()
        {
            bool multi = Screen.AllScreens.Length > 1;
            enableSecondScreen.Enabled = multi;     // or menu item
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            UpdateSecondScreenAvailability();
        }

        private void frmMain_Shown(object sender, EventArgs e)
        {
            ClearAll();
            LoadConfig();
            CenterToScreen();
            UpdateRecentPlaylists("");
            UpdateDisplay(false);
            ChangeTopMenuColors(Color.Black, Color.AliceBlue);            
            Activate();
            InitBASS();
            if (!string.IsNullOrEmpty(PlaylistPath) && autoloadLastPlaylist.Checked && File.Exists(PlaylistPath))
            {
                PrepareToLoadPlaylist();
            }
            updater.RunWorkerAsync();            
            hoverForm.Show(this);
            UpdateOverlayPosition();
        }

        private void PrepareToLoadPlaylist(string playlist = "")
        {
            if (!string.IsNullOrEmpty(playlist))
            {
                PlaylistPath = playlist;
            }
            statusLabel.Text = "Loading Playlist...";            
            LoadPlaylist();
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F && ModifierKeys.HasFlag(Keys.Control))
            {
                lblClearSearch_MouseClick(null, null);
                txtSearch.Focus();
            }
            else if (e.KeyCode == Keys.Enter && ModifierKeys.HasFlag(Keys.Control))
            {
                doSongPlayback();
            }
            else if (e.KeyCode == Keys.Space && !txtSearch.Focused && txtSearch.BackColor == Color.Black)
            {
                picPlay_MouseClick(null, null);
            }
            else if (e.KeyCode == Keys.Escape && isFullScreen)
            {
                doResizeVisuals();
            }
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter) return;
            var enabled = !string.IsNullOrEmpty(txtSearch.Text.Trim()) && txtSearch.Text != strSearchPlaylist;
            if (!enabled) return;
            picSearch.Enabled = enabled;
            lblClearSearch.Enabled = enabled;
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
            displayKaraokeMode.Enabled = PlayingSong == null || (MIDITools.PhrasesVocals.Phrases.Any() && MIDITools.LyricsVocals.Lyrics.Any());
            displayAlbumArt.Enabled = PlayingSong == null || File.Exists(CurrentSongArtBlurred);
            chartVisualsToolStripMenuItem.Enabled = !hasNoMIDI;
            UpdateKaraokeItemsVisibility();
        }

        private void UpdateKaraokeItemsVisibility()
        {
            selectBackgroundColor.Visible = !rockBandKaraoke.Checked;
            selectLyricColor.Visible = !rockBandKaraoke.Checked;
            selectHighlightColor.Visible = true;
            restoreDefaultsToolStripMenuItem.Visible = true;
            toolStripMenuItem13.Visible = true;
            toolStripMenuItem14.Visible = true;
            selectHarmony3HighlightColor.Visible = classicKaraokeMode.Checked || rockBandKaraoke.Checked;
            selectHarmony3TextColor.Visible = classicKaraokeMode.Checked;
            selectHarmonyTextColor.Visible = classicKaraokeMode.Checked;
            selectHarmonyHighlightColor.Visible = classicKaraokeMode.Checked || rockBandKaraoke.Checked;
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return) return;
            e.Handled = true;
            if (ShowingNotFoundMessage) return;
            if (txtSearch.Text == strSearchPlaylist || Playlist.Count == 0) return;
            ReloadPlaylist(Playlist, true, true, false);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text == strSearchPlaylist || Playlist.Count == 0) return;
            ReloadPlaylist(Playlist);
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
                    ShowingNotFoundMessage = true;
                    MessageBox.Show(msg, AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ShowingNotFoundMessage = false;
                    return;
                }
                if (ActiveSong != null)
                {
                    lstPlaylist.Items[ActiveSong.Index].Selected = false;
                }
                lstPlaylist.Items[select].Selected = true;
                lstPlaylist.Items[select].Focused = true;
                lstPlaylist.EnsureVisible(select);
            }
            catch (Exception ex)
            { }
        }

        private void showPracticeSections_Click(object sender, EventArgs e)
        {
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
            {  }
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
            SortPlaylist(PlaylistSorting.Shuffle);
        }

        private void startInstaMix_Click(object sender, EventArgs e)
        {
            EnableDisable(false);
            lblClearSearch_MouseClick(null, null);
            SongMixer.RunWorkerAsync();
        }

        private void SongMixer_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            EnableDisable(true);
            ReloadPlaylist(Playlist, true, false, false);
            picShuffle.Tag = "noshuffle";
            toolTip1.SetToolTip(picShuffle, "Enable track shuffling");            
            //if (Bass.BASS_ChannelIsActive(BassMixer) == BASSActive.BASS_ACTIVE_PLAYING) return;
            doSongPlayback();
        }     

        private void SongMixer_DoWork(object sender, DoWorkEventArgs e)
        {
            var MixSong = ActiveSong;
            const int minSongs = 25;
            //try to get at least 25 songs in the playlist
            //allow 25%, 38% and 50% discrepancy at max
            //don't go beyond 50% discrepancy even if we have less than 25 songs
            CreateSongMix(MixSong, 0.25);
            if (Playlist.Count < minSongs)
            {
                CreateSongMix(MixSong, 0.38);
            }
            if (Playlist.Count < minSongs)
            {
                CreateSongMix(MixSong, 0.50);
            }
            Playlist.Remove(MixSong);
            Shuffle(Playlist);
            var backup = Playlist[0];
            Playlist[0] = MixSong;
            Playlist.Add(backup);
        }

        private void CreateSongMix(Song mixSong, double factor)
        {
            double maxBPM = mixSong.BPM * (1.00 + factor);
            double minBPM = mixSong.BPM * (1.00 - factor);

            double maxLength = mixSong.Length * (1.00 + factor);
            double minLength = mixSong.Length * (1.00 - factor);

            string seedGenre = (mixSong.Genre ?? "").Trim();
            string seedArtist = (mixSong.Artist ?? "").Trim();

            var allowedGenres = GetAllowedGenres(seedGenre);

            Playlist = new List<Song>();

            foreach (var song in StaticPlaylist)
            {               
                bool sameArtist =
                    !string.IsNullOrWhiteSpace(seedArtist) &&
                    string.Equals((song.Artist ?? "").Trim(), seedArtist, StringComparison.OrdinalIgnoreCase);

                if (sameArtist)
                {
                    Playlist.Add(song);
                    continue;
                }

                if (song.BPM < minBPM || song.BPM > maxBPM) continue;
                if (song.Length < minLength || song.Length > maxLength) continue;                              

                bool genreAllowed =
                    !string.IsNullOrWhiteSpace(song.Genre) &&
                    allowedGenres.Contains(((song.Genre ?? "").Trim()));

                if (genreAllowed)
                {
                    Playlist.Add(song);
                }
            }        
        }

        private static HashSet<string> GetAllowedGenres(string seedGenre)
        {
            seedGenre = (seedGenre ?? "").Trim();

            if (string.IsNullOrWhiteSpace(seedGenre))
                return new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // Find family that contains the seed genre
            foreach (var fam in GenreFamilies.Values)
            {
                if (fam.Contains(seedGenre))
                    return fam;
            }

            // Fallback: only exact genre
            return new HashSet<string>(StringComparer.OrdinalIgnoreCase) { seedGenre };
        }

        private static readonly Dictionary<string, HashSet<string>> GenreFamilies =
    new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase)
    {
        ["Rock"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "Rock","Alternative","Indie Rock","Classic Rock","Southern Rock","Pop-Rock",
        "Grunge","Emo","Punk","New Wave","Glam","J-Rock","Prog"
    },
        ["Metal"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Metal", "Nu-Metal" },
        ["Pop/Dance/Electronic"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Pop/Dance/Electronic" },
        ["Hip-Hop/Rap"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Hip-Hop/Rap" },
        ["R&B/Soul/Funk"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "R&B/Soul/Funk" },
        ["Blues"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Blues" },
        ["Jazz"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Jazz", "Fusion" },
        ["Country"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Country" },
        ["Latin"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Latin" },
        ["Reggae/Ska"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Reggae/Ska" },
        ["Classical"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Classical" },
        ["Inspirational"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Inspirational" },
        ["World"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "World" },
        ["Novelty"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Novelty" },
        ["Other"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Other" },
    };


        private void LoadRecent(int playlist)
        {
            if (Text.Contains("*"))
            {
                if (MessageBox.Show("You have unsaved changes on the current playlist\nAre you sure you want to load another playlist and lose those changes?",
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

        private void DrawSpectrum(Rectangle bounds, Graphics g)
        {
            Spectrum.ChannelIsMixerSource = false;
            Spectrum.MaxFFT = BASSData.BASS_DATA_FFT4096;

            try
            {
                int width = bounds.Width; // used by the “full resolution” variants
                Color bgColor = SpectrumColor;

                switch (SpectrumID)
                {
                    default:
                        SpectrumID = 0;
                        Spectrum.CreateSpectrumLine(BassMixer, g, bounds, ChartGreen, ChartRed, bgColor, 2, 2, false, false, false);
                        break;

                    case 1:
                        Spectrum.CreateSpectrum(BassMixer, g, bounds, ChartGreen, ChartRed, bgColor, false, false, false);
                        break;

                    case 2: // full-res line spectrum
                        Spectrum.CreateSpectrumLine(BassMixer, g, bounds, ChartBlue, ChartOrange, bgColor, width / 15, 4, false, true, false);
                        break;

                    case 3:
                        Spectrum.CreateSpectrumEllipse(BassMixer, g, bounds, ChartGreen, ChartRed, bgColor, 1, 2, false, false, false);
                        break;

                    case 4:
                        Spectrum.CreateSpectrumLinePeak(BassMixer, g, bounds, ChartGreen, ChartYellow, ChartOrange, bgColor, 2, 1, 2, 10, false, false, false);
                        break;

                    case 5: // full-res peak spectrum
                        Spectrum.CreateSpectrumLinePeak(BassMixer, g, bounds, ChartGreen, ChartBlue, ChartOrange, bgColor, width / 15, 5, 3, 5, false, true, false);
                        break;

                    case 6:
                        Spectrum.CreateWaveForm(BassMixer, g, bounds, ChartGreen, ChartRed, ChartYellow, bgColor, 1, true, false, false);
                        break;
                }
            }
            catch (Exception ex)
            {   }
        }

        public bool GetDisplayAudioSpectrumIsChecked()
        {
            return displayAudioSpectrum.Checked;
        }

        private void displayAudioSpectrum_Click(object sender, EventArgs e)
        {
            ClickDisplayAudioSpectrum();                 
        }

        public void ClickDisplayAudioSpectrum()
        {
            StopAllVideoPlayback();
            picVisuals.BackColor = Color.AliceBlue;
            stageTimer.Enabled = false;
            ChangeTopMenuColors(Color.Black, Color.AliceBlue);
            CheckUncheckAll(displayAudioSpectrum);
            updateDisplayType(displayAudioSpectrum);
            picVisuals.Image = null;
        }

        private Color GetMoodColor()
        {
            Color bgColor = Color.AliceBlue;
            if (File.Exists(CurrentSongArtBlurred))
            {
                using (var bmp = (Bitmap)Image.FromFile(CurrentSongArtBlurred))
                {
                    bgColor = Tools.GetMoodBackgroundFromBlurred(bmp, Color.AliceBlue, 28);
                }
            }
            return bgColor;
        }

        private void displayAlbumArt_Click(object sender, EventArgs e)
        {
            ClickDisplayAlbumArt();
        }     
        
        public bool GetDisplayAlbumArtIsChecked()
        {
            return displayAlbumArt.Checked;
        }
        
        public void ClickDisplayAlbumArt()
        {
            stageTimer.Enabled = false;
            ChangeTopMenuColors(Color.Black, Color.AliceBlue);
            CheckUncheckAll(displayAlbumArt);
            updateDisplayType(displayAlbumArt);
            toolTip1.SetToolTip(picPreview, "Click to change spectrum style");
            var bgColor = GetMoodColor();
            if (!PlaybackTimer.Enabled)
            {
                picVisuals.Image = Resources.logo;
                picVisuals.SizeMode = PictureBoxSizeMode.StretchImage;
                if (secondScreen != null)
                {
                    secondScreen.ChangeBackgroundImage(Resources.logo);
                }
            }
            else
            {
                if (secondScreen != null)
                {
                    secondScreen.StopVideoPlayback();
                    secondScreen.ChangeBackgroundColor(bgColor);
                    picVisuals.BackColor = Color.AliceBlue;
                }
                else
                {
                    StopVideoPlayback();
                    picVisuals.BackColor = bgColor;
                }
            }
        }

        private void ChangeDisplay()
        {
            ClearVisuals();
            if (!displayAlbumArt.Checked && File.Exists(CurrentSongArt))
            {
                picPreview.Image = Tools.NemoLoadImage(CurrentSongArt);
                picPreview.Cursor = Cursors.Hand;
                toolTip1.SetToolTip(picPreview, "Click to view album art");
            }
            else
            {
                picPreview.Image = Resources.default_art;
                picPreview.Cursor = Cursors.Default;
                toolTip1.SetToolTip(picPreview, "No album art available");
            }
        }

        private void audioTracks_Click(object sender, EventArgs e)
        {
            isChoosingStems = true;
            var selector = new AudioSelector(this);
            selector.Show();
        }

        private void showMIDIVisuals_Click(object sender, EventArgs e)
        {
            var selector = new MIDISelector(this);
            selector.Show();
        }

        private readonly Dictionary<string, Bitmap> _lyricBmpCache = new Dictionary<string, Bitmap>(StringComparer.Ordinal);
        private readonly object _lyricBmpLock = new object();

        private Bitmap GetLyricBitmap(string text, Font font, Color foreColor)
        {
            // Cache key: text + font + color
            string key = text + "\n" + font.Name + "|" + font.SizeInPoints + "|" + (int)font.Style + "|" + foreColor.ToArgb();

            lock (_lyricBmpLock)
            {
                if (_lyricBmpCache.TryGetValue(key, out var bmp))
                    return bmp;

                // Measure once
                var size = TextRenderer.MeasureText(text, font, new Size(int.MaxValue, int.MaxValue),
                    TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);

                // Make a tight bitmap
                bmp = new Bitmap(Math.Max(1, size.Width), Math.Max(1, size.Height), PixelFormat.Format32bppPArgb);

                using (var g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.Transparent);
                }

                // Draw onto bitmap
                using (var g = Graphics.FromImage(bmp))
                {
                    TextRenderer.DrawText(
                        g,
                        text,
                        font,
                        new Point(0, 0),
                        foreColor,
                        TextFormatFlags.NoPadding | TextFormatFlags.SingleLine | TextFormatFlags.NoClipping
                    );
                }

                _lyricBmpCache[key] = bmp;
                return bmp;
            }
        }

        private void DrawLyricsScrolling(List<Lyric> lyrics, Font font, Color foreColor, Color backColor, int posY, Graphics graphics)
        {
            if ((!openSideWindow.Checked && secondScreen == null) || PlayingSong == null || Bass.BASS_ChannelIsActive(BassMixer) != BASSActive.BASS_ACTIVE_PLAYING || !doScrollingLyrics) return;
            if (lyrics == null || lyrics.Count == 0) return;
            var renderSize = new Size(1920, 1080);

            var time = GetCorrectedTime();
            var playbackWindow = PlaybackWindowRBVocals * (isRBKaraoke() && PlayingSong.BPM > 80.0 ? 80.0 / PlayingSong.BPM : 1.0);
            var hitboxPosition = chartVertical.Checked || rBStyle.Checked || isRBKaraoke() ? HitboxVocalsX + (bmpHitboxVocals.Width / 2) : renderSize.Width;

            // Draw background for lyrics
            graphics.DrawImage(Resources.frostedglass75, 0, posY, renderSize.Width, 24);
            using (var overlayBrush = new SolidBrush(
                Color.FromArgb(chartVertical.Checked ? 255 : 128, foreColor)))
            {
                graphics.FillRectangle(overlayBrush, 0, posY, renderSize.Width, 24);
            }
            using (var overlayBrush = new SolidBrush(Color.LightGray))
            {
                graphics.FillRectangle(overlayBrush, 0, posY - 1, renderSize.Width, 1);
            }
            using (var overlayBrush = new SolidBrush(Color.LightGray))
            {
                graphics.FillRectangle(overlayBrush, 0, posY + 24, renderSize.Width, 1);
            }

            foreach (var lyric in lyrics)
            {
                if (lyric.End < time - 1) continue;

                if (lyric.Start > time + playbackWindow) return;

                float leftF = ((chartVertical.Checked || rBStyle.Checked) && chartVisualsToolStripMenuItem.Checked) || isRBKaraoke()
                    ? (int)(((lyric.Start - time) / playbackWindow) * (renderSize.Width - hitboxPosition)) + hitboxPosition
                    : (int)(((lyric.Start - time) / playbackWindow) * renderSize.Width);
                var left = (int)Math.Round(leftF);
                const int step = 2;
                left = (left / step) * step;

                var bmp = GetLyricBitmap(lyric.DisplayText.Replace("‿", " "), font, Color.WhiteSmoke);
                graphics.DrawImageUnscaled(bmp, left, posY - 6);
            }
        }               

        private void DrawLyricsKaraoke(IEnumerable<LyricPhrase> phrases, IEnumerable<Lyric> lyrics, Font font, Color foreColor, Color backColor, int posY, Graphics graphics)
        {
            if (lyrics == null || lyrics.Count() == 0) return;
            var time = GetCorrectedTime();
            var renderSize = new Size(1920, 1080);
            graphics.DrawImage(Resources.frostedglass75, 0, posY, renderSize.Width, 24);
            // 50% opacity = alpha 128 (out of 255)
            using (var overlayBrush = new SolidBrush(
                Color.FromArgb(chartVertical.Checked ? 255 : 128, foreColor)))
            {
                graphics.FillRectangle(
                    overlayBrush,
                    0,
                    posY,
                    renderSize.Width,
                    24
                );
            }
            using (var overlayBrush = new SolidBrush(Color.LightGray))
            {
                graphics.FillRectangle(overlayBrush, 0, posY - 1, renderSize.Width, 1);
            }
            using (var overlayBrush = new SolidBrush(Color.LightGray))
            {
                graphics.FillRectangle(overlayBrush, 0, posY + 24, renderSize.Width, 1);
            }

            LyricPhrase line = null;
            foreach (var lyric in phrases.TakeWhile(lyric => lyric.PhraseStart <= time).Where(lyric => lyric.PhraseEnd >= time))
            {
                line = lyric;
            }

            if (line == null || string.IsNullOrEmpty(line.PhraseText)) return;

            var full = ProcessLine(line.PhraseText, doWholeWordsLyrics).Replace("‿", " ");
            var highlight = ProcessLine(lyrics.Where(lyr => !(lyr.Start < line.PhraseStart))
                              .TakeWhile(lyr => !(lyr.Start > time))
                              .Aggregate("", (current, lyr) => current + " " + lyr.Text), doWholeWordsLyrics).Replace("‿", " ");

            if (string.IsNullOrEmpty(full) || string.IsNullOrEmpty(highlight)) return;

            var flags = TextFormatFlags.NoPadding | TextFormatFlags.NoClipping;
            var fullSize = TextRenderer.MeasureText(graphics, full, font, new Size(int.MaxValue, int.MaxValue), flags);
            int left = (renderSize.Width - fullSize.Width) / 2;

            TextRenderer.DrawText(graphics, full, font, new Point(left, posY - 6), Color.White, flags);
            TextRenderer.DrawText(graphics, highlight, font, new Point(left, posY - 6), foreColor, flags);
        }

        private void showLyrics_Click(object sender, EventArgs e)
        {
            var selector = new LyricSelector(this);
            selector.Show();            
        }      
        
        private void panelVisuals_DoubleClick(object sender, EventArgs e)
        {
            //if (secondScreen != null) return;
            //doResizeVisuals();                 
        }

        private void doResizeVisuals()
        {
            var screen = Screen.FromControl(picVisuals);
            if (isFullScreen)
            {
                picVisuals.Dock = DockStyle.None;
                picVisuals.Location = picVisualsPosition;
                picVisuals.Size = picVisualsSize;
                Location = savedFormLocation;
                Size = savedFormSize;
                FormBorderStyle = FormBorderStyle.FixedSingle;
                isFullScreen = false;
                if (displayKaraokeMode.Checked || chartVisualsToolStripMenuItem.Checked)
                {
                    ChangeTopMenuColors(Color.Black, Color.AliceBlue);
                }
                menuStrip1.Visible = true;
            }
            else
            {
                // Save position and size *before* going full screen
                picVisualsPosition = picVisuals.Location;
                picVisualsSize = picVisuals.Size;
                savedFormLocation = this.Location;
                savedFormSize = this.Size;
                FormBorderStyle = FormBorderStyle.None;
                Bounds = screen.Bounds; // Take over the whole screen
                picVisuals.Dock = DockStyle.Fill;
                isFullScreen = true;
                if (displayKaraokeMode.Checked || chartVisualsToolStripMenuItem.Checked)
                {
                    ChangeTopMenuColors(Color.White, Color.Black);
                }
                menuStrip1.Visible = false;
            }
        }

        private void ChangeTopMenuColors(Color forecolor, Color backcolor)
        {
            menuStrip1.BackColor = backcolor;
            helpToolStripMenuItem.BackColor = backcolor;
            equipmentToolStripMenuItem.BackColor = backcolor;
            optionsToolStripMenuItem.BackColor = backcolor;
            toolsToolStripMenuItem.BackColor = backcolor;
            fileToolStripMenuItem.BackColor = backcolor;
            helpToolStripMenuItem.ForeColor = forecolor;
            equipmentToolStripMenuItem.ForeColor = forecolor;
            optionsToolStripMenuItem.ForeColor = forecolor;
            toolsToolStripMenuItem.ForeColor = forecolor;
            fileToolStripMenuItem.ForeColor = forecolor;
            statusLabel.ForeColor = forecolor;
            statusLabel.BackColor = backcolor;
        }

        private void takeScreenshot_Click(object sender, EventArgs e)
        {
            if (!openSideWindow.Checked && secondScreen == null)
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
            try
            {
                // pick which surface to capture
                Control captureCtrl = secondScreen != null ? (Control)secondScreen : (Control)picVisuals;

                var screenTopLeft = captureCtrl.PointToScreen(Point.Empty);
                var size = captureCtrl.ClientSize;

                if (size.Width <= 0 || size.Height <= 0) return;

                using (var bitmap = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppPArgb))
                using (var g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(screenTopLeft, Point.Empty, size, CopyPixelOperation.SourceCopy);

                    bitmap.Save(xOut, ImageFormat.Png);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error capturing screenshot of visuals:\n" + ex.Message + "\nTry again",
                    AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("There is no active song", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            
            var text = BuildSongDetailsText(ActiveSong, ActiveSong == PlayingSong);
            using (var f = new SongDetailsForm(text, ActiveSong.Location))
            {
                f.ShowDialog(this);
            }
        }

        private string BuildSongDetailsText(Song song, bool isPlaying)
        {
            var sb = new StringBuilder();

            void Section(string title)
            {
                sb.AppendLine("");
                sb.AppendLine(title);
                sb.AppendLine(new string('─', Math.Max(20, title.Length)));
            }

            void KV(string key, object val)
            {
                sb.AppendLine($"{key.PadRight(20)}: {val}");
            }

            //Section("Active Song Details");
            KV("Song Location", song.Location);
            KV("Playlist Index", song.Index);
            KV("Artist", song.Artist);
            KV("Title", song.Name);
            KV("Album", song.Album);
            KV("Track Number", song.Track);
            KV("Year", song.Year);
            KV("Genre", song.Genre);
            KV("Length", FormatTime(song.Length / 1000L));
            KV("Charter", string.IsNullOrWhiteSpace(song.Charter) ? "Unknown" : song.Charter.Trim());
            KV("Internal Name", song.InternalName);
            KV("Rhythm on Keys?", song.isRhythmOnKeys ? "Yes" : "No");
            KV("Rhythm on Bass?", song.isRhythmOnBass ? "Yes" : "No");
            KV("Has Pro Keys?", (song.hasProKeys) ? "Yes" : "No");
            KV("Audio Delay", song.PSDelay == 0 ? "None" : song.PSDelay.ToString(CultureInfo.InvariantCulture) + " ms");
            KV("Language(s)", song.Languages);

            Section("Audio Channels");
            KV("Drums", song.ChannelsDrums);
            KV("Bass", song.ChannelsBass);
            KV("Guitar", song.ChannelsGuitar);
            KV("Keys", song.ChannelsKeys);
            KV("Vocals", song.ChannelsVocals);
            KV("Backing", song.ChannelsBacking);
            KV("Crowd", song.ChannelsCrowd);

            if (isPlaying)
            {
                Section("Chart Info (from MIDI)");

                var instruments = new List<string>();
                var solos = new List<string>();

                if (MIDITools.MIDI_Chart.Drums.ChartedNotes.Count > 0) instruments.Add("D");
                if (MIDITools.MIDI_Chart.Bass.ChartedNotes.Count > 0) instruments.Add("B");
                if (MIDITools.MIDI_Chart.Guitar.ChartedNotes.Count > 0) instruments.Add("G");
                if (MIDITools.MIDI_Chart.Keys.ChartedNotes.Count > 0) instruments.Add("K");
                if (MIDITools.MIDI_Chart.ProKeys.ChartedNotes.Count > 0) instruments.Add("PK");
                if (MIDITools.MIDI_Chart.Vocals.ChartedNotes.Count > 0) instruments.Add("V");
                if (MIDITools.MIDI_Chart.Harm1.ChartedNotes.Count > 0) instruments.Add("H1");
                if (MIDITools.MIDI_Chart.Harm2.ChartedNotes.Count > 0) instruments.Add("H2");
                if (MIDITools.MIDI_Chart.Harm3.ChartedNotes.Count > 0) instruments.Add("H3");

                if (MIDITools.MIDI_Chart.Drums.Solos.Count > 0) solos.Add("D");
                if (MIDITools.MIDI_Chart.Bass.Solos.Count > 0) solos.Add("B");
                if (MIDITools.MIDI_Chart.Guitar.Solos.Count > 0) solos.Add("G");
                if (MIDITools.MIDI_Chart.Keys.Solos.Count > 0) solos.Add("K");
                if (MIDITools.MIDI_Chart.ProKeys.Solos.Count > 0) solos.Add("PK");

                KV("Average BPM", MIDITools.MIDI_Chart.AverageBPM.ToString(CultureInfo.InvariantCulture));
                KV("Uses disco flip?", MIDITools.MIDI_Chart.DiscoFlips.Any() ? "Yes" : "No");                
                KV("Instrument Charts", instruments.Count == 0 ? "None" : string.Join(" ", instruments));
                KV("Instrument Solos", solos.Count == 0 ? "None" : string.Join(" ", solos));

                if (MIDITools.MIDI_Chart.Vocals.ChartedNotes.Count > 0)
                    KV("Range - Vocals", MIDITools.MIDI_Chart.Vocals.NoteRange.Count);

                if (MIDITools.MIDI_Chart.Harm1.ChartedNotes.Count > 0)
                    KV("Range - Harmonies", MIDITools.MIDI_Chart.Harm1.NoteRange.Count);

                if (MIDITools.MIDI_Chart.ProKeys.ChartedNotes.Count > 0)
                    KV("Range - Pro Keys", MIDITools.MIDI_Chart.ProKeys.NoteRange.Count);

                KV("Practice Sessions", MIDITools.PracticeSessions.Count);
            }

            Section("Attenuation");
            sb.AppendLine((song.AttenuationValues ?? "").Trim());

            Section("Panning");
            sb.AppendLine((song.PanningValues ?? "").Trim());

            return sb.ToString();
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

        private void picRandom_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || lstPlaylist.Items.Count <= 1 || songExtractor.IsBusy || songPreparer.IsBusy) return;
            DoShuffleSongs();
        }

         private void DoShuffleSongs()
        {
            int num1 = ShuffleSongs(true);
            if (num1 < 0 || num1 > lstPlaylist.Items.Count - 1)
            {
                MessageBox.Show("There was an error selecting a song at random, try again",
                    AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            NextSongIndex = num1;

            lstPlaylist.BeginUpdate();
            try
            {
                lstPlaylist.SelectedIndices.Clear();

                if (NextSongIndex > lstPlaylist.Items.Count - 1)
                {
                    NextSongIndex = 0;
                    DeleteUsedFiles(false);
                }

                var item = lstPlaylist.Items[NextSongIndex];
                item.Selected = true;
                item.Focused = true;
                lstPlaylist.EnsureVisible(NextSongIndex);
            }
            finally
            {
                lstPlaylist.EndUpdate();
            }

            doSongPlayback();
        }        

        private void updater_DoWork(object sender, DoWorkEventArgs e)
        {
            var path = Application.StartupPath + "\\bin\\updatev6.txt";
            Tools.DeleteFile(path);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;
            using (var client = new WebClient())
            {
                try
                {
                    client.DownloadFile("https://nemosnautilus.com/cplayer/updatev6.txt", path);
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
            videoOverlay.TopMost = false;

            var path = Application.StartupPath + "\\bin\\updatev5.txt";
            if (!File.Exists(path))
            {
                if (showUpdateMessage)
                {
                    MessageBox.Show("Checking for update failed", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                videoOverlay.TopMost = true;
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
                        MessageBox.Show("Checking for update failed", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    videoOverlay.TopMost = true;
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
                videoOverlay.TopMost = true;
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
                videoOverlay.TopMost = true;
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
                videoOverlay.TopMost = true;
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
            var changeLog = new ChangeLog();
            changeLog.ShowDialog();
        }

        private void sortPlaylistByModifiedDate_Click(object sender, EventArgs e)
        {
            SortPlaylist(PlaylistSorting.ByModifiedDate);
        }

        private void RenderVisuals(Size size, Graphics g)
        {
            if (!PlaybackTimer.Enabled || (!openSideWindow.Checked && secondScreen == null) || PlayingSong == null || WindowState == FormWindowState.Minimized
                || (_mediaPlayer.State == VLCState.Paused))
            {
                return;
            }

            UpdateTextQuality(g);

            if (displayAlbumArt.Checked)
            {
                g.Clear(GetMoodColor());
                if (LargeAlbumArt != null)
                {
                    g.DrawImage(LargeAlbumArt, (size.Width - size.Height) / 2, 0, size.Height, size.Height);
                }
                DrawLyrics(size, g, Color.White);
                return;
            }
            if (displayAudioSpectrum.Checked)
            {
                var bounds = picVisuals.Bounds;
                if (secondScreen != null)
                {
                    bounds = secondScreen.PictureBounds();
                }
                DrawSpectrum(bounds, g);
                DrawLyrics(size, g, Color.White);
                return;
            }
            if (chartVisualsToolStripMenuItem.Checked && chartFull.Checked && DrewFullChart)
            {
                var percent = (GetCorrectedTime() / ((double)PlayingSong.Length / 1000));
                var width = ((int)(size.Width * percent)) + 1;
                using (var chart = CopyChartSection(ChartBitmap, new Rectangle(Point.Empty, new Size(width, size.Height))))
                {
                    g.DrawImage(chart, 0, 0, width, size.Height);
                }
                return;
            }
            if (isRBKaraoke() && MIDITools.PhrasesVocals.Phrases.Any() && MIDITools.LyricsVocals.Lyrics.Any())
            {
                DoRockBandKaraoke(size, g);
                return;
            }
            if (displayKaraokeMode.Checked && cPlayerStyle.Checked && MIDITools.PhrasesVocals.Phrases.Any() && MIDITools.LyricsVocals.Lyrics.Any())
            {
                DoKaraokeMode(g, MIDITools.PhrasesVocals.Phrases, MIDITools.LyricsVocals.Lyrics);
                return;
            }
            if (displayKaraokeMode.Checked && classicKaraokeMode.Checked && ((MIDITools.PhrasesHarm1.Phrases.Any() && MIDITools.LyricsHarm1.Lyrics.Any()) || (MIDITools.PhrasesVocals.Phrases.Any() && MIDITools.LyricsVocals.Lyrics.Any())))
            {
                DoModernKaraoke(size, g, MIDITools.PhrasesVocals.Phrases, MIDITools.LyricsVocals.Lyrics,
                                   MIDITools.PhrasesHarm1?.Phrases ?? MIDITools.PhrasesVocals.Phrases,
                                   MIDITools.LyricsHarm1?.Lyrics ?? MIDITools.LyricsVocals.Lyrics,
                                   MIDITools.PhrasesHarm2.Phrases, MIDITools.LyricsHarm2.Lyrics,
                                   MIDITools.PhrasesHarm3.Phrases, MIDITools.LyricsHarm3.Lyrics);
                return;
            }
            if (chartVisualsToolStripMenuItem.Checked && (chartVertical.Checked || rBStyle.Checked))
            {
                DrawMIDIFile(size, g);
                return;
            }
            if (!chartVisualsToolStripMenuItem.Checked || (!chartSnippet.Checked && DrewFullChart)) return;

            DrawMIDIFile(size, g);
            DrewFullChart = true;
        }

        private void EnsureFrame(Size size)
        {
            if (_renderedFrame != null && _renderedFrame.Size == size) return;
            _renderedFrame?.Dispose();
            _renderedFrame = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppPArgb);

        }

        private void DoRockBandKaraoke(Size size, Graphics graphics)
        {
            if (MIDITools.MIDI_Chart.Vocals.ChartedNotes.Count <= 0) return;

            var vocalsY = (size.Height - (vocalsHeight * 2)) / 2;
            int Index;
            Color backColor = Color.FromArgb(36, 36, 36);
            const int spacer = 4;

            if (secondScreen != null)
            {
                secondScreen.ChangeBackgroundColor(backColor);
                picVisuals.BackColor = Color.AliceBlue;
            }
            else
            {
                picVisuals.BackColor = backColor;
            }
                var time = GetCorrectedTime();
                LyricPhrase currentLineLead = null;
                LyricPhrase nextLineLead = null;
                LyricPhrase lastLineLead = null;
                //get active and next phrase, and store last used phrase
                var phrasesLead = MIDITools.PhrasesVocals.Phrases;
                var lyricsLead = MIDITools.LyricsVocals.Lyrics;
                var phrasesHarmony = MIDITools.PhrasesHarm1.Phrases;
                var lyricsHarmony = MIDITools.LyricsHarm1.Lyrics;
                var phrasesHarmony2 = MIDITools.PhrasesHarm2.Phrases;
                var lyricsHarmony2 = MIDITools.LyricsHarm2.Lyrics;
                var phrasesHarmony3 = MIDITools.PhrasesHarm3.Phrases;
                var lyricsHarmony3 = MIDITools.LyricsHarm3.Lyrics;
                for (var i = 0; i < phrasesLead.Count(); i++)
                {
                    var phrase = phrasesLead[i];
                    if (string.IsNullOrEmpty(phrase.PhraseText)) continue;
                    if (phrase.PhraseEnd < time)
                    {
                        lastLineLead = phrasesLead[i];
                        continue;
                    }
                    if (phrase.PhraseStart > time)
                    {
                        nextLineLead = phrasesLead[i];
                        break;
                    }
                    currentLineLead = phrase;
                    if (i < phrasesLead.Count - 1)
                    {
                        nextLineLead = phrasesLead[i + 1];
                    }
                    break;
                }
                LyricPhrase currentLineHarmony = null;
                LyricPhrase nextLineHarmony = null;
                LyricPhrase lastLineHarmony = null;
                //get active and next phrase, and store last used phrase
                for (var i = 0; i < phrasesHarmony.Count(); i++)
                {
                    var phrase = phrasesHarmony[i];
                    if (string.IsNullOrEmpty(phrase.PhraseText)) continue;
                    if (phrase.PhraseEnd < time)
                    {
                        lastLineHarmony = phrasesHarmony[i];
                        continue;
                    }
                    if (phrase.PhraseStart > time)
                    {
                        nextLineHarmony = phrasesHarmony[i];
                        break;
                    }
                    currentLineHarmony = phrase;
                    if (i < phrasesHarmony.Count - 1)
                    {
                        nextLineHarmony = phrasesHarmony[i + 1];
                    }
                    break;
                }
                LyricPhrase currentLineHarmony2 = null;
                LyricPhrase nextLineHarmony2 = null;
                LyricPhrase lastLineHarmony2 = null;
                //get active and next phrase, and store last used phrase
                for (var i = 0; i < phrasesHarmony2.Count(); i++)
                {
                    var phrase = phrasesHarmony2[i];
                    if (string.IsNullOrEmpty(phrase.PhraseText)) continue;
                    if (phrase.PhraseEnd < time)
                    {
                        lastLineHarmony2 = phrasesHarmony2[i];
                        continue;
                    }
                    if (phrase.PhraseStart > time)
                    {
                        nextLineHarmony2 = phrasesHarmony2[i];
                        break;
                    }
                    currentLineHarmony2 = phrase;
                    if (i < phrasesHarmony2.Count - 1)
                    {
                        nextLineHarmony2 = phrasesHarmony2[i + 1];
                    }
                    break;
                }

                if (time < 5.0)
                {
                    var title = "\"" + PlayingSong.Name.Replace("&", "&&") + "\"";
                    var artist = PlayingSong.Artist.Replace("&", "&&");
                    var album = PlayingSong.Album.Replace("&", "&&");
                    var bpm = PlayingSong.BPM == 0 ? "" : "BPM: " + Math.Round(PlayingSong.BPM, 0, MidpointRounding.AwayFromZero);
                    var parts = 1;
                    if (lyricsHarmony.Any())
                    {
                        parts++;
                    }
                    if (lyricsHarmony2.Any())
                    {
                        parts++;
                    }
                    var vocalParts = "Vocal Parts: " + parts;
                    var charter = PlayingSong.Charter.Replace("&", "&&");
                    if (!string.IsNullOrEmpty(charter))
                    {
                        charter = "As charted by " + charter;
                    }
                    else
                    {
                        charter = "";
                    }

                    var lineY = 20;
                    var infoFont = new Font("Tahoma", GetScaledFontSize(graphics, title, new Font("Tahoma", (float)16f), 36f));
                    var infoSize = TextRenderer.MeasureText(title, infoFont);
                    var infoX = (size.Width - infoSize.Width) / 2;
                    TextRenderer.DrawText(graphics, title, infoFont, new Point(infoX, lineY), Color.WhiteSmoke, Color.Transparent);
                    lineY += infoSize.Height;

                    infoFont = new Font("Tahoma", GetScaledFontSize(graphics, artist, new Font("Tahoma", (float)16f), 36f));
                    infoSize = TextRenderer.MeasureText(artist, infoFont);
                    infoX = (size.Width - infoSize.Width) / 2;
                    TextRenderer.DrawText(graphics, artist, infoFont, new Point(infoX, lineY), Color.WhiteSmoke, Color.Transparent);
                    lineY += infoSize.Height;

                    infoFont = new Font("Tahoma", GetScaledFontSize(graphics, album, new Font("Tahoma", (float)16f), 36f));
                    infoSize = TextRenderer.MeasureText(album, infoFont);
                    infoX = (size.Width - infoSize.Width) / 2;
                    TextRenderer.DrawText(graphics, album, infoFont, new Point(infoX, lineY), Color.WhiteSmoke, Color.Transparent);
                    lineY += infoSize.Height;

                    if (!string.IsNullOrEmpty(charter))
                    {
                        infoFont = new Font("Tahoma", GetScaledFontSize(graphics, charter, new Font("Tahoma", (float)16f), 24f));
                        infoSize = TextRenderer.MeasureText(charter, infoFont);
                        infoX = (size.Width - infoSize.Width) / 2;
                        lineY = size.Height - infoSize.Height - 20;
                        TextRenderer.DrawText(graphics, charter, infoFont, new Point(infoX, lineY), Color.WhiteSmoke, Color.Transparent);
                    }
                    else
                    {
                        infoFont = new Font("Tahoma", GetScaledFontSize(graphics, "Harmonix", new Font("Tahoma", (float)16f), 24f));
                        infoSize = TextRenderer.MeasureText("Harmonix", infoFont);
                        lineY = size.Height - infoSize.Height - 20;
                    }
                    lineY -= infoSize.Height;

                    if (!string.IsNullOrEmpty(bpm))
                    {
                        infoSize = TextRenderer.MeasureText(bpm, infoFont);
                        infoX = (size.Width - infoSize.Width) / 2;
                        TextRenderer.DrawText(graphics, bpm, infoFont, new Point(infoX, lineY), Color.WhiteSmoke, Color.Transparent);
                    }
                    lineY -= infoSize.Height;

                    infoSize = TextRenderer.MeasureText(vocalParts, infoFont);
                    infoX = (size.Width - infoSize.Width) / 2;
                    TextRenderer.DrawText(graphics, vocalParts, infoFont, new Point(infoX, lineY), Color.WhiteSmoke, Color.Transparent);
                }
                else
                {
                    if (currentLineLead == null || nextLineLead == null)
                    {
                        try
                        {
                            LyricPhrase nextStartingPhrase = null;
                            //nextStartingPhrase = nextLineLead.PhraseStart < nextLineHarmony.PhraseStart ? nextLineLead : nextLineHarmony;
                            if (nextLineLead != null && nextLineHarmony != null)
                            {
                                nextStartingPhrase = nextLineLead.PhraseStart < nextLineHarmony.PhraseStart
                                    ? nextLineLead : nextLineHarmony;
                            }
                            else if (nextLineLead != null)
                            {
                                nextStartingPhrase = nextLineLead;
                            }
                            else if (nextLineHarmony != null)
                            {
                                nextStartingPhrase = nextLineHarmony;
                            }
                            if (nextStartingPhrase != null)
                            {
                                var wait = ((int)((nextStartingPhrase.PhraseStart - time) + 0.5));
                                if (wait >= 1)
                                {
                                    double LastEnd;
                                    double NextStart;
                                    double gap;

                                    try
                                    {
                                        LastEnd = new[] { lastLineLead?.PhraseEnd, lastLineHarmony?.PhraseEnd }.Where(x => x.HasValue).Max().Value;
                                    }
                                    catch
                                    {
                                        LastEnd = 0.0;
                                    }
                                    NextStart = new[] { nextLineLead?.PhraseStart, nextLineHarmony?.PhraseStart }.Where(x => x.HasValue).Min().Value;
                                    gap = NextStart - LastEnd;
                                    if (gap >= 3)
                                    {
                                        var infoFont = new Font("Arial", GetScaledFontSize(graphics, wait.ToString(CultureInfo.InvariantCulture), new Font("Tahoma", (float)16f), 130f));
                                        var infoSize = TextRenderer.MeasureText(wait.ToString(CultureInfo.InvariantCulture), infoFont);
                                        var infoX = (size.Width - infoSize.Width) / 2;
                                        TextRenderer.DrawText(graphics, wait.ToString(CultureInfo.InvariantCulture), infoFont, new Point(infoX, (vocalsY - infoSize.Height) / 2), Color.WhiteSmoke, Color.Transparent);
                                    }
                                }
                            }
                        }
                        catch
                        {
                        }
                    }
                }

                var vocalLyrics = doHarmonyLyrics && MIDITools.LyricsHarm1.Lyrics.Any() ? MIDITools.LyricsHarm1.Lyrics : MIDITools.LyricsVocals.Lyrics;
                var font = new Font("Segoe UI", 16f, FontStyle.Bold);
                var harm1Y = vocalsY + (vocalsHeight * 2) + spacer;
                var harm2Y = vocalsY - spacer - 24;
                var harm3Y = harm2Y - spacer - 24;
                
                if (chartVertical.Checked)
                {
                    using (var overlayBrush = new SolidBrush(
                    Color.FromArgb(chartVertical.Checked ? 255 : 128, Color.Black)))
                    {
                        graphics.FillRectangle(overlayBrush, 0, vocalsY, size.Width, vocalsHeight * 2);
                    }
                }
                graphics.DrawImage(chartVertical.Checked ? Resources.frostedglass75 : Resources.frostedglass50, 0, vocalsY, size.Width, vocalsHeight * 2);                
                if (doStaticLyrics)
                {
                    var vocalPhrases = doHarmonyLyrics && MIDITools.PhrasesHarm1.Phrases.Any() ? MIDITools.PhrasesHarm1.Phrases : MIDITools.PhrasesVocals.Phrases;
                    DrawLyricsStatic(MIDITools.PhrasesHarm3.Phrases, font, KaraokeModeHarm3Highlight, backColor, harm3Y, graphics);
                    DrawLyricsStatic(MIDITools.PhrasesHarm2.Phrases, font, KaraokeModeHarm2Highlight, backColor, harm2Y, graphics);
                    DrawLyricsStatic(vocalPhrases, font, KaraokeModeHarm1Highlight, backColor, harm1Y, graphics);
                }
                else if (doKaraokeLyrics)
                {
                    var vocalPhrases = doHarmonyLyrics && MIDITools.PhrasesHarm1.Phrases.Any() ? MIDITools.PhrasesHarm1.Phrases : MIDITools.PhrasesVocals.Phrases;
                    DrawLyricsKaraoke(MIDITools.PhrasesHarm3.Phrases, MIDITools.LyricsHarm3.Lyrics, font, KaraokeModeHarm3Highlight, backColor, harm3Y, graphics);
                    DrawLyricsKaraoke(MIDITools.PhrasesHarm2.Phrases, MIDITools.LyricsHarm2.Lyrics, font, KaraokeModeHarm2Highlight, backColor, harm2Y, graphics);
                    DrawLyricsKaraoke(vocalPhrases, vocalLyrics, font, KaraokeModeHarm1Highlight, backColor, harm1Y, graphics);
                }
                else //default to scrolling
                {
                    DrawLyricsScrolling(MIDITools.LyricsHarm3.Lyrics, font, KaraokeModeHarm3Highlight, backColor, harm3Y, graphics);
                    DrawLyricsScrolling(MIDITools.LyricsHarm2.Lyrics, font, KaraokeModeHarm2Highlight, backColor, harm2Y, graphics);
                    DrawLyricsScrolling(vocalLyrics, font, KaraokeModeHarm1Highlight, backColor, harm1Y, graphics);
                }                
                DrawPhraseMarkers(graphics, MIDITools.PhrasesVocals, vocalsHeight * 2, vocalsY);

                if (MIDITools.MIDI_Chart.Harm3.ChartedNotes.Count > 0 && doMIDIHarmonies)
                {
                    DrawNotes(graphics, MIDITools.MIDI_Chart.Harm3, vocalsHeight * 2, vocalsY, false, 3, out Index);
                    MIDITools.MIDI_Chart.Harm3.ActiveIndex = Index;
                    graphics.DrawImage(Resources.fadeout3, 0, harm3Y, (int)(Resources.fadeout3.Width * 1.5), 24);
                    graphics.DrawImage(Resources.fadein3, size.Width - (int)(Resources.fadeout3.Width * 1.5), harm3Y, (int)(Resources.fadeout3.Width * 1.5), 24);
                }
                if (MIDITools.MIDI_Chart.Harm2.ChartedNotes.Count > 0 && doMIDIHarmonies)
                {
                    DrawNotes(graphics, MIDITools.MIDI_Chart.Harm2, vocalsHeight * 2, vocalsY, false, 2, out Index);
                    MIDITools.MIDI_Chart.Harm2.ActiveIndex = Index;
                    graphics.DrawImage(Resources.fadeout3, 0, harm2Y, (int)(Resources.fadeout3.Width * 1.5), 24);
                    graphics.DrawImage(Resources.fadein3, size.Width - (int)(Resources.fadeout3.Width * 1.5), harm2Y, (int)(Resources.fadeout3.Width * 1.5), 24);
                }
                if (MIDITools.MIDI_Chart.Harm1.ChartedNotes.Count > 0 && doMIDIHarmonies)
                {
                    DrawNotes(graphics, MIDITools.MIDI_Chart.Harm1, vocalsHeight * 2, vocalsY, false, 1, out Index);
                    MIDITools.MIDI_Chart.Harm1.ActiveIndex = Index;
                }
                else
                {
                    DrawNotes(graphics, MIDITools.MIDI_Chart.Vocals, vocalsHeight * 2, vocalsY, false, 0, out Index);
                    MIDITools.MIDI_Chart.Vocals.ActiveIndex = Index;
                }
                DrawHitbox(graphics, bmpHitboxVocals, HitboxVocalsX, vocalsY, bmpHitboxVocals.Width, vocalsHeight * 2, 1, "");
                graphics.DrawImage(Resources.fadein3, size.Width - (int)(Resources.fadeout3.Width * 1.5), vocalsY, (int)(Resources.fadeout3.Width * 1.5), vocalsHeight * 2);
                graphics.DrawImage(Resources.fadeout3, 0, vocalsY, (int)(Resources.fadeout3.Width * 1.5), vocalsHeight * 2);

                graphics.DrawImage(Resources.fadeout3, 0, harm1Y, (int)(Resources.fadeout3.Width * 1.5), 24);
                graphics.DrawImage(Resources.fadein3, size.Width - (int)(Resources.fadeout3.Width * 1.5), harm1Y, (int)(Resources.fadeout3.Width * 1.5), 24);
            }

        private void picPreview_Paint(object sender, PaintEventArgs e)
        {
            if (displayAlbumArt.Checked || (!File.Exists(CurrentSongArt) && !displayAudioSpectrum.Checked))
            {
                DrawSpectrum(picPreview.Bounds, e.Graphics);
            }
        }

        private void GetIntroOutroSilencePS()
        {
            IntroSilence = 0.0;
            OutroSilence = 0.0;
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
                if (IntroSilence == 0.0 || newIntroSilence < IntroSilence) //we only want earliest instance of sound in all streams
                {
                    IntroSilence = newIntroSilence;
                }
                if (newOutroSilence > OutroSilence) //we only want latest instance of silence in all streams
                {
                    OutroSilence = newOutroSilence;
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
            IntroSilence = 0.0;
            OutroSilence = 0.0;
            if (!skipIntroOutroSilence.Checked || yarg.Checked || powerGig.Checked || bandFuse.Checked || fortNite.Checked || guitarHero.Checked) return;
            ProcessStreamForSilence(BassMixer, out IntroSilence, out OutroSilence);
        }

        private void ProcessStreamForSilence(int bassMixer, out double intro, out double outro)
        {
            intro = 0.0;
            outro = 0.0;

            // Mixer output: 2ch float => 8 bytes per frame
            const int channels = 2;
            const int bytesPerFloat = 4;
            const int bytesPerFrame = channels * bytesPerFloat; // 8

            // We'll read floats, but BASS_ChannelGetData returns BYTES
            var buffer = new float[50000];

            // Always analyze from the start
            Bass.BASS_ChannelSetPosition(bassMixer, 0, BASSMode.BASS_POS_BYTE);

            long length = Bass.BASS_ChannelGetLength(bassMixer, BASSMode.BASS_POS_BYTE);
            if (length <= 0) return;

            try
            {
                // ---------- INTRO ----------
                long introBytes = 0;

                while (true)
                {
                    int bytesRead = Bass.BASS_ChannelGetData(bassMixer, buffer, 40000 | (int)BASSData.BASS_DATA_FLOAT);
                    if (bytesRead <= 0) break;

                    int framesRead = bytesRead / bytesPerFrame;
                    if (framesRead <= 0) break;

                    int silentFrames = 0;

                    for (int frame = 0; frame < framesRead; frame++)
                    {
                        int i = frame * channels;

                        // Frame is silent only if BOTH channels are under threshold
                        if (Math.Abs(buffer[i]) <= SilenceThreshold &&
                            Math.Abs(buffer[i + 1]) <= SilenceThreshold)
                        {
                            silentFrames++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    introBytes += (long)silentFrames * bytesPerFrame;

                    if (silentFrames < framesRead)
                        break; // hit non-silence
                }

                intro = Bass.BASS_ChannelBytes2Seconds(bassMixer, introBytes);

                // ---------- OUTRO ----------
                long outroStartBytes = length;
                long pos = length;

                while (pos > introBytes)
                {
                    // step back (keep aligned to frame boundary to avoid weirdness)
                    pos = (pos < 200000) ? 0 : pos - 200000;
                    pos -= (pos % bytesPerFrame);

                    Bass.BASS_ChannelSetPosition(bassMixer, pos, BASSMode.BASS_POS_BYTE);

                    int bytesRead = Bass.BASS_ChannelGetData(bassMixer, buffer, 200000 | (int)BASSData.BASS_DATA_FLOAT);
                    if (bytesRead <= 0) break;

                    int framesRead = bytesRead / bytesPerFrame;
                    if (framesRead <= 0) break;

                    int c = framesRead;

                    // walk backward over silent frames
                    while (c > 0)
                    {
                        int i = (c - 1) * channels;

                        if (Math.Abs(buffer[i]) <= (SilenceThreshold / 2f) &&
                            Math.Abs(buffer[i + 1]) <= (SilenceThreshold / 2f))
                        {
                            c--;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (c <= 0)
                        continue; // entire block is silent, step further back

                    outroStartBytes = pos + (long)c * bytesPerFrame;
                    break;
                }

                outro = Bass.BASS_ChannelBytes2Seconds(bassMixer, outroStartBytes);
            }
            catch (Exception ex)
            { }
        }

        private void updateDisplayType(object sender)
        {
            if (!PlaybackTimer.Enabled)
            {
                if (secondScreen != null)
                {
                    secondScreen.ChangeBackgroundImage(Resources.logo);
                }
                else
                {
                    picVisuals.Image = Resources.logo;
                }
            }           
            UpdateDisplay(false);
            ChangeDisplay();
            UpdateKaraokeItemsVisibility();
        }           

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            var enabled = !string.IsNullOrEmpty(txtSearch.Text.Trim()) && txtSearch.Text != strSearchPlaylist;
            txtSearch.ForeColor = enabled ? Color.Black : Color.Gray;
            picSearch.Enabled = enabled;
            lblClearSearch.Enabled = enabled;
        }

        private void rebuildPlaylistMetadata_Click(object sender, EventArgs e)
        {
            doRebuildPlaylist(false);
        }

        private void playBGVideos_Click(object sender, EventArgs e)
        {
            displayBackgroundVideo.Checked = playBGVideos.Checked;
        }

        private void frmMain_Move(object sender, EventArgs e)
        {
            UpdateOverlayPosition();
        }        

        private void picPlay_MouseClick(object sender, MouseEventArgs e)
        {
            if (sender != null && e.Button != MouseButtons.Left) return;
            doClickPlay();
        }

        private void doClickPlay()
        {
            if (lstPlaylist.Items.Count == 0) return;
            if (Bass.BASS_ChannelIsActive(BassMixer) == BASSActive.BASS_ACTIVE_PAUSED)
            {
                Bass.BASS_ChannelPlay(BassMixer, false);
                if (_mediaPlayer.State == VLCState.Paused)
                {
                    _mediaPlayer.Play();
                }
                if (secondScreen != null && secondScreen._mediaPlayer.State == VLCState.Paused)
                {
                    secondScreen._mediaPlayer.Play();
                }
                UpdatePlaybackStuff();
                if ((rockBandKaraoke.Checked && displayKaraokeMode.Checked && animatedBackground.Checked) ||
                    classicKaraokeMode.Checked && displayKaraokeMode.Checked && animatedBackground2.Checked)
                {
                    stageTimer.Enabled = true;
                }
            }
            else
            {
                if (lstPlaylist.SelectedItems.Count == 0)
                {
                    lstPlaylist.Items[0].Selected = true;
                }
                lstPlaylist.Select();
                doSongPlayback();
            }
        }

        private void picStop_MouseClick(object sender, MouseEventArgs e)
        {
            if (sender != null && e.Button != MouseButtons.Left) return;
            if (Bass.BASS_ChannelIsActive(BassMixer) == BASSActive.BASS_ACTIVE_PLAYING ||
                Bass.BASS_ChannelIsActive(BassMixer) == BASSActive.BASS_ACTIVE_PAUSED)
            {
                DoClickStop();
            }
        }

        private void picPause_MouseClick(object sender, MouseEventArgs e)
        {
            if (sender != null && e.Button != MouseButtons.Left) return;
            doClickPause();
        }

        private void doClickPause()
        {
            if (Bass.BASS_ChannelIsActive(BassMixer) == BASSActive.BASS_ACTIVE_PLAYING)
            {
                stageTimer.Enabled = false;
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
            
            picLoop.Tag = picLoop.Tag.ToString() == "loop" ? "noloop" : "loop";
            toolTip1.SetToolTip(picLoop, picLoop.Tag.ToString() == "loop" ? "Disable track looping" : "Enable track looping");
            picShuffle.Tag = "noshuffle";
            toolTip1.SetToolTip(picShuffle, "Enable track shuffling");
           
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
            
            picShuffle.Tag = picShuffle.Tag.ToString() == "shuffle" ? "noshuffle" : "shuffle";
            toolTip1.SetToolTip(picShuffle, picShuffle.Tag.ToString() == "shuffle" ? "Disable track shuffling" : "Enable track shuffling");
            picLoop.Tag = "noloop";
            toolTip1.SetToolTip(picLoop, "Enable track looping");
            
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
            doClickNext();
        }

        private void doClickNext()
        {
            if (picLoop.Tag.ToString() == "loop")
            {
                DoLoop();
                return;
            }
            randomizeBackgroundImage();
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
            stageKitIndex = 1;
            SelectStageKitController();
        }

        private void controller2_Click(object sender, EventArgs e)
        {
            UncheckAllStageKits();
            controller2.Checked = true;
            stageKitIndex = 2;
            SelectStageKitController();
        }

        private void controller3_Click(object sender, EventArgs e)
        {
            UncheckAllStageKits();
            controller3.Checked = true;
            stageKitIndex = 3;
            SelectStageKitController();
        }

        private void controller4_Click(object sender, EventArgs e)
        {
            UncheckAllStageKits();
            controller4.Checked = true;
            stageKitIndex = 4;
            SelectStageKitController();
        }                        

        private void stageKitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (stageKit != null && !stageKitToolStripMenuItem.Checked)
            {
                UncheckAllStageKits();
                stageKit = null;
            }
            stageKitTimer.Enabled = stageKitToolStripMenuItem.Checked;                
        }

        private void selectBackgroundColor_Click(object sender, EventArgs e)
        {
            KaraokeModeBackgroundColor = GetColorFromPicker(KaraokeModeBackgroundColor);
        }

        private Color GetColorFromPicker(Color color)
        {
            using (ColorDialog colorDialog = new ColorDialog())            {  
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    return colorDialog.Color;
                }
                return color;
            }
        }

        private void selectLyricColor_Click(object sender, EventArgs e)
        {
            KaraokeModeHarm1Text = GetColorFromPicker(KaraokeModeHarm1Text);
        }

        private void selectHighlightColor_Click(object sender, EventArgs e)
        {
            KaraokeModeHarm1Highlight = GetColorFromPicker(KaraokeModeHarm1Highlight);
        }

        private void restoreDefaultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KaraokeModeBackgroundColor = cPlayerStyle.Checked ? Color.White : Color.Black;
            KaraokeModeHarm1Text = cPlayerStyle.Checked ? Color.FromArgb(180, 180, 180) : Color.White;
            KaraokeModeHarm1Highlight = cPlayerStyle.Checked ? Color.FromArgb(95, 209, 209) : Color.DodgerBlue;
            KaraokeModeHarm2Text = Color.LightGray;
            KaraokeModeHarm2Highlight = Color.HotPink;
            KaraokeModeHarm3Text = Color.DarkGray;
            KaraokeModeHarm3Highlight = Color.LimeGreen;
        }

        private void doRebuildPlaylist(bool doAudio)
        {
            if (MessageBox.Show("This might take a while...are you sure you want to do this now?",
                AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            DoClickStop();
            lblClearSearch_MouseClick(null, null);
            var rebuilder = new Rebuilder(this, StaticPlaylist, true);
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

        private void rebuildPlaymetadataAudio_Click(object sender, EventArgs e)
        {
            doRebuildPlaylist(true);
        }

        private void StopAllVideoPlayback()
        {
            if (VideoIsPlaying)
            {
                StopVideoPlayback();
                VideoIsPlaying = false;
                changedBackground = false;
            }
            if (secondScreen != null && secondScreen.VideoIsPlaying)
            {
                secondScreen.StopVideoPlayback();
                changedBackground = false;
            }
        }

        public bool GetClassicKaraokeModeIsChecked()
        {
            return classicKaraokeMode.Checked;
        }

        private void classicKaraokeMode_Click(object sender, EventArgs e)
        {
            ClickClassicKaraokeMode();    
        }

        public void ClickClassicKaraokeMode()
        {
            if (!yarg.Checked)
            {
                StopAllVideoPlayback();
            }
            else if (yarg.Checked && !string.IsNullOrEmpty(CHVideoPath))
            {
                PlayCurrentVideo(CHVideoPath);
            }
            picVisuals.SizeMode = PictureBoxSizeMode.StretchImage;
            picVisuals.Image = null;
            CheckUncheckAll(classicKaraokeMode);
            displayKaraokeMode.Checked = true;
            updateDisplayType(classicKaraokeMode);
            if (secondScreen != null)
            {
                if (yarg.Checked && !string.IsNullOrEmpty(CHVideoPath))
                {
                    secondScreen.ChangeBackgroundImage(null);
                    secondScreen.ChangeBackgroundColor(Color.Black);
                }
                else
                {
                    secondScreen.ChangeBackgroundImage(solidColorBackground.Checked ? Resources.gradient : null);
                    secondScreen.ChangeBackgroundColor(KaraokeModeBackgroundColor);
                }                
                picVisuals.Image = Resources.logo;
                picVisuals.SizeMode = PictureBoxSizeMode.Zoom;
                picVisuals.BackColor = Color.AliceBlue;
            }
            else
            {
                if (yarg.Checked && !string.IsNullOrEmpty(CHVideoPath))
                {
                    picVisuals.Image = null;
                    picVisuals.BackColor = Color.Black;
                }
                else
                {
                    picVisuals.Image = solidColorBackground.Checked ? Resources.gradient : null;
                    picVisuals.BackColor = KaraokeModeBackgroundColor;
                }                    
            }
            stageTimer.Enabled = animatedBackground2.Checked;
        }

        public bool GetcPlayerStyleIsChecked()
        {
            return cPlayerStyle.Checked;
        }

        private void cPlayerStyle_Click(object sender, EventArgs e)
        {
            ClickCPlayerStyle();
        }
        
        public void ClickCPlayerStyle()
        {
            if (!yarg.Checked)
            {
                StopAllVideoPlayback();
            }
            if (yarg.Checked && !string.IsNullOrEmpty(CHVideoPath) && !VideoIsPlaying)
            { 
                PlayCurrentVideo(CHVideoPath);
            }
            picVisuals.Image = null;
            stageTimer.Enabled = false;
            CheckUncheckAll(cPlayerStyle);
            displayKaraokeMode.Checked = true;
            updateDisplayType(cPlayerStyle);
            picVisuals.BackgroundImage = null;
        }

        private void selectHarmonyTextColor_Click(object sender, EventArgs e)
        {
            KaraokeModeHarm2Text = GetColorFromPicker(KaraokeModeHarm2Text);
        }

        private void selectHarmonyHighlightColor_Click(object sender, EventArgs e)
        {
            KaraokeModeHarm2Highlight = GetColorFromPicker(KaraokeModeHarm2Highlight);
        }

        private void selectHarmony3TextColor_Click(object sender, EventArgs e)
        {
            KaraokeModeHarm3Text = GetColorFromPicker(KaraokeModeHarm3Text);
        }

        private void selectHarmony3HighlightColor_Click(object sender, EventArgs e)
        {
            KaraokeModeHarm3Highlight = GetColorFromPicker(KaraokeModeHarm3Highlight);
        }

        public bool GetRockBandKaraokeIsChecked()
        {
            return rockBandKaraoke.Checked;
        }

        private void rockBandKaraoke_Click(object sender, EventArgs e)
        {
            ClickRockBandKaraoke();
        }

        public void ClickRockBandKaraoke()
        {
            StopAllVideoPlayback();
            picVisuals.Image = PlaybackTimer.Enabled ? stageBackground : null;// Resources.background_new;
            CheckUncheckAll(rockBandKaraoke);
            displayKaraokeMode.Checked = true;
            updateDisplayType(rockBandKaraoke);
            stageTimer.Enabled = animatedBackground.Checked;
            picVisuals.SizeMode = PictureBoxSizeMode.StretchImage;
            if (staticBackground.Checked)
            {
                staticBackground.PerformClick();
            }
            else if (animatedBackground.Checked)
            {
                animatedBackground.PerformClick();
            }
        }

        private int stageCounter = 0;
        private void stageTimer_Tick(object sender, EventArgs e)
        {
            if (!PlaybackTimer.Enabled) return;
            if (stageFrames == null || stageFrames.Count == 0)
            {
                var image = isRBKaraoke() ? stageBackground : null;
                if (secondScreen != null)
                {
                    secondScreen.ChangeBackgroundImage(image);
                }
                else
                {
                    picVisuals.Image = image;
                }
                stageTimer.Enabled = false;
                return;
            }
            var frame = stageFrames[stageCounter++];
            if (secondScreen != null)
            {
                secondScreen.ChangeBackgroundImage(frame);
            }
            else
            {
                picVisuals.Image = frame;
            }
            if (stageCounter == stageFrames.Count - 1)
            {
                stageCounter = 0;
            }
        }

        public bool GetChartVerticalIsChecked()
        {
            return chartVertical.Checked;
        }

        private void chartVertical_Click(object sender, EventArgs e)
        {
            ClickChartVertical();
        }

        public void ClickChartVertical()
        {
            StopAllVideoPlayback();
            CheckUncheckAll(chartVertical);
            chartVisualsToolStripMenuItem.Checked = true;
            updateDisplayType(chartVertical);
            UpdateVisualStyle(chartVertical);
            if (secondScreen != null)
            {
                secondScreen.ChangeBackgroundColor(Color.Black);
            }
        }

        public bool GetChartSnippetIsChecked()
        {
            return chartSnippet.Checked;
        }

        private void chartSnippet_Click(object sender, EventArgs e)
        {
            ClickChartSnippet();
        }

        public void ClickChartSnippet()
        {
            StopAllVideoPlayback();
            CheckUncheckAll(chartSnippet);
            chartVisualsToolStripMenuItem.Checked = true;
            updateDisplayType(chartSnippet);
            UpdateVisualStyle(chartSnippet);
        }

        private void CheckUncheckAll(object sender)
        {
            chartVertical.Checked = false;
            chartSnippet.Checked = false;
            chartFull.Checked = false;
            displayKaraokeMode.Checked = false;
            displayAlbumArt.Checked = false;
            displayAudioSpectrum.Checked = false;
            rockBandKaraoke.Checked = false;
            cPlayerStyle.Checked = false;
            classicKaraokeMode.Checked = false;
            chartVisualsToolStripMenuItem.Checked = false;
            rBStyle.Checked = false;
            ((ToolStripMenuItem)(sender)).Checked = true;
        }

        private void chartFull_Click(object sender, EventArgs e)
        {
            CheckUncheckAll(sender);
            chartVisualsToolStripMenuItem.Checked = true;
            updateDisplayType(sender);
            UpdateVisualStyle(sender);            
        }

        private DateTime lastCursorMovement;
        private void cursorTimer_Tick(object sender, EventArgs e)
        {
            if (!isFullScreen)
            {
                cursorTimer.Enabled = false;
                return;
            }
            var timeSpan = DateTime.Now - lastCursorMovement;
            if (timeSpan.Seconds >= 3)
            {
                Cursor.Hide();
                cursorTimer.Enabled = false;                            
            }
        }

        public bool GetAnimatedBackgroundIsChecked()
        {
            return animatedBackground.Checked;
        }

        private void animatedBackground_Click(object sender, EventArgs e)
        {
            ClickAnimatedBackground();
        }

        public void ClickAnimatedBackground()
        {
            StopAllVideoPlayback();
            staticBackground.Checked = false;
            animatedBackground.Checked = true;
            if (isRBKaraoke())
            {
                stageTimer.Enabled = true;
                picVisuals.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        public bool GetStaticBackgroundIsChecked()
        {
            return staticBackground.Checked;
        }

        private void staticBackground_Click(object sender, EventArgs e)
        {
            ClickStaticBackground();
        }

        public void ClickStaticBackground()
        {
            StopAllVideoPlayback();
            stageTimer.Enabled = false;
            staticBackground.Checked = true;
            animatedBackground.Checked = false;
            if (isRBKaraoke())
            {
                if (secondScreen != null)
                {
                    secondScreen.ChangeBackgroundImage(stageBackground);
                }
                else
                {
                    picVisuals.Image = stageBackground;
                }
                picVisuals.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        public bool GetForceSoloVocalsIsChecked()
        {
            return forceSoloVocals.Checked;
        }

        private void forceSoloVocals_Click(object sender, EventArgs e)
        {
            ClickForceSoloVocals();
        }

        public void ClickForceSoloVocals()
        {
            forceSoloVocals.Checked = !forceSoloVocals.Checked;
            if (forceSoloVocals.Checked)
            {
                forceTwoPartHarmonies.Checked = false;
            }
        }

        public bool GetForceTwoPartHarmoniesIsChecked()
        {
            return forceTwoPartHarmonies.Checked;
        }

        private void forceTwoPartHarmonies_Click(object sender, EventArgs e)
        {
            ClickForceTwoPartHarmonies();
        }

        public void ClickForceTwoPartHarmonies()
        {
            forceTwoPartHarmonies.Checked = !forceTwoPartHarmonies.Checked;
            if (forceTwoPartHarmonies.Checked)
            {
                forceSoloVocals.Checked = false;
            }
        }

        private void stageKitTimer_Tick(object sender, EventArgs e)
        {
            if (Bass.BASS_ChannelIsActive(BassMixer) == BASSActive.BASS_ACTIVE_PLAYING)
            {
                // the stream is still playing...                    
                var pos = Bass.BASS_ChannelGetPosition(BassStream); // position in bytes
                var time = Bass.BASS_ChannelBytes2Seconds(BassStream, pos); // the elapsed time length                   

                RandomizeStageKit(time);
            }
        }

        public enum VideoPathType
        {
            FromPath, FromLocation
        }

        private int _applyARInProgress;
        private volatile string _pendingAspectRatio;

        void ApplyFillAspectRatio()
        {
            int w = videoView.ClientSize.Width;
            int h = videoView.ClientSize.Height;
            if (w <= 0 || h <= 0) return;
            if (_mediaPlayer == null) return;

            _pendingAspectRatio = $"{w}:{h}";

            if (Interlocked.Exchange(ref _applyARInProgress, 1) == 1)
                return;

            var mp = _mediaPlayer;

            Task.Run(() =>
            {
                try
                {
                    while (true)
                    {
                        var ar = _pendingAspectRatio;
                        _pendingAspectRatio = null;
                        if (string.IsNullOrEmpty(ar)) break;

                        try { mp.AspectRatio = ar; } catch { }
                        try { mp.Scale = 0; } catch { }
                    }
                }
                finally
                {
                    Interlocked.Exchange(ref _applyARInProgress, 0);

                    // If something came in after we released, run again
                    if (!string.IsNullOrEmpty(_pendingAspectRatio))
                        BeginInvoke((Action)(ApplyFillAspectRatio));
                }
            });
        }


        private string _currentVideoPath;
        private VideoPathType _currentVideoType;
        private Media _currentMedia;

        public void StartVideoPlayback(string videoPath, VideoPathType pathType, long videoTime)
        {
            if (InvokeRequired)
            {
                BeginInvoke((Action)(() => StartVideoPlayback(videoPath, pathType, videoTime)));
                return;
            }

            if (_mediaPlayer == null || string.IsNullOrEmpty(videoPath)) return;

            // If already playing this video, just seek
            if (_currentVideoPath == videoPath && _currentVideoType == pathType && _mediaPlayer.Media != null)
            {
                videoView.Visible = true;
                if (_mediaPlayer.IsSeekable) _mediaPlayer.Time = videoTime;
                if (!_mediaPlayer.IsPlaying) _mediaPlayer.Play();
                VideoIsPlaying = true;
                return;
            }

            // UI bits first (cheap)
            ApplyFillAspectRatio();
            videoView.Visible = true;
            videoView.BringToFront();

            // Capture references for the background work
            var mp = _mediaPlayer;
            var lib = _libVLC;
            var vp = videoPath;
            var vt = pathType;
            var t = videoTime;

            Task.Run(() =>
            {
                // New video: stop then set new Media (OFF the UI thread)
                try { mp.Stop(); } catch { }

                try
                {
                    // Dispose previous media (safe-ish to do here)
                    try { _currentMedia?.Dispose(); } catch { }
                    _currentMedia = null;

                    var from = (vt == VideoPathType.FromPath) ? FromType.FromPath : FromType.FromLocation;

                    // Create + assign new media (keep it alive via _currentMedia field)
                    var media = new Media(lib, vp, from, "input-repeat=1000");
                    _currentMedia = media;

                    try { mp.Media = media; } catch { }

                    try { mp.Play(); } catch { }

                    // Seek after play (often more reliable)
                    try
                    {
                        if (mp.IsSeekable)
                            mp.Time = t;
                    }
                    catch { }
                }
                catch { }

                // Update state back on the UI thread
                try
                {
                    BeginInvoke((Action)(() =>
                    {
                        _currentVideoPath = vp;
                        _currentVideoType = vt;
                        VideoIsPlaying = true;
                        currentVideoPath = vp;
                    }));
                }
                catch { }
            });
        }

        private Screen GetOtherScreen()
        {
            // Use center point — more reliable when dragging between monitors
            var center = new Point(
                this.Left + this.Width / 2,
                this.Top + this.Height / 2);

            var current = Screen.FromPoint(center);

            foreach (var s in Screen.AllScreens)
            {
                if (s.DeviceName != current.DeviceName)
                    return s;
            }

            // Fallback if only one screen
            return current;
        }

        private PopOutScreen secondScreen;
        public string currentVideoPath;
        private void enableSecondScreen_Click(object sender, EventArgs e)
        {
            var videoShouldBeVisible = (yarg.Checked || rBStyle.Checked) && (VideoIsPlaying || (secondScreen != null && secondScreen.VideoIsPlaying));

            if (enableSecondScreen.Checked)
            {
                enableSecondScreen.Checked = false;                
                if (chartVisualsToolStripMenuItem.Checked && chartVertical.Checked)
                {
                    picVisuals.Image = null;
                    picVisuals.BackColor = Color.Black;
                }
                else if (displayKaraokeMode.Checked && classicKaraokeMode.Checked && solidColorBackground.Checked)
                {
                    picVisuals.Image = Resources.gradient;
                }
                else if (displayKaraokeMode.Checked && cPlayerStyle.Checked)
                {
                    picVisuals.Image = null;
                }
                else if (chartVisualsToolStripMenuItem.Checked && rBStyle.Checked)
                {
                    picVisuals.Image = secondScreen.backgroundImage;
                }
                else if (displayAlbumArt.Checked)
                {
                    picVisuals.Image = LargeAlbumArt;
                    picVisuals.SizeMode = PictureBoxSizeMode.Zoom;
                    Color bgColor = Color.AliceBlue;
                    if (File.Exists(CurrentSongArtBlurred))
                    {
                        using (var bmp = (Bitmap)Image.FromFile(CurrentSongArtBlurred))
                        {
                            bgColor = Tools.GetMoodBackgroundFromBlurred(bmp, Color.AliceBlue, 28);
                        }
                    }
                    picVisuals.BackColor = bgColor;
                }
                if (secondScreen != null)
                {
                    long time = 0;
                    try
                    {
                        time = (long)(PlaybackSeconds * 1000) + Parser.Songs[0].VideoStartTime;
                    }
                    catch { }                        
                    if (secondScreen.VideoIsPlaying)
                    {
                        secondScreen.StopVideoPlayback();
                        StartVideoPlayback(currentVideoPath, VideoPathType.FromPath, time);
                    }
                    secondScreen.Dispose();
                    secondScreen = null;
                }
                UpdateOverlayPosition();
            }
            else
            {
                if (Screen.AllScreens.Length <= 1) return;
                var target = GetOtherScreen();

                enableSecondScreen.Checked = true;                
                secondScreen = new PopOutScreen(this);
                secondScreen.WindowState = FormWindowState.Normal;
                secondScreen.Bounds = target.Bounds;

                secondScreen.ChangeBackgroundColor(picVisuals.BackColor);
                if (displayKaraokeMode.Checked && classicKaraokeMode.Checked && solidColorBackground.Checked && !videoShouldBeVisible)
                {
                    secondScreen.ChangeBackgroundImage(Resources.gradient);
                }
                else if (displayAlbumArt.Checked)
                {
                    secondScreen.ChangeBackgroundImage(LargeAlbumArt, true);
                    Color bgColor = Color.AliceBlue;
                    if (File.Exists(CurrentSongArtBlurred))
                    {
                        using (var bmp = (Bitmap)Image.FromFile(CurrentSongArtBlurred))
                        {
                            bgColor = Tools.GetMoodBackgroundFromBlurred(bmp, Color.AliceBlue, 28);
                        }
                    }
                    secondScreen.ChangeBackgroundColor(bgColor);
                    picVisuals.BackColor = Color.AliceBlue;// bgColor;
                }                
                if (rBStyle.Checked && !videoShouldBeVisible)
                {
                    secondScreen.ChangeBackgroundImage(picVisuals.Image);
                }
                picVisuals.Image = Resources.logo;
                secondScreen.Show();
                if (videoShouldBeVisible)
                {
                    var time = _mediaPlayer.Time = (long)(PlaybackSeconds * 1000) + Parser.Songs[0].VideoStartTime;
                    StopAllVideoPlayback();
                    if (!secondScreen.VideoIsPlaying)
                    {
                        secondScreen.StartVideoPlayback(currentVideoPath, PopOutScreen.VideoPathType.FromPath, time);
                    }
                }
                if (WindowState == FormWindowState.Normal)
                {
                    UpdateOverlayPosition();
                }
            }
        }

        private void changeStageBackground_Click(object sender, EventArgs e)
        {
            randomizeBackgroundImage();
        }

        public bool GetEnableBackgroundImageIsChecked()
        {
            return staticBackground2.Checked;
        }

        private void enableBackgroundImage_Click(object sender, EventArgs e)
        {
            ClickEnableBackgroundImage();
        }

        public void ClickEnableBackgroundImage()
        {
            animatedBackground2.Checked = false;
            staticBackground2.Checked = true;
            solidColorBackground.Checked = false;
            stageTimer.Enabled = false;
            if (classicKaraokeMode.Checked)
            {
                picVisuals.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        public bool GetAnimatedBackground2IsChecked()
        {
            return animatedBackground2.Checked;
        }

        private void animatedBackground2_Click(object sender, EventArgs e)
        {
            ClickAnimatedBackground2();
        }

        public void ClickAnimatedBackground2()
        {
            StopAllVideoPlayback();
            solidColorBackground.Checked = false;
            animatedBackground2.Checked = true;
            staticBackground2.Checked = false;
            if (classicKaraokeMode.Checked)
            {
                stageTimer.Enabled = true;
                picVisuals.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void lblClearSearch_MouseClick(object sender, MouseEventArgs e)
        {
            if (e == null || e.Button == MouseButtons.Left)
            {
                if (txtSearch.Text == strSearchPlaylist) return;
                txtSearch.Invoke(new MethodInvoker(() => txtSearch.Text = strSearchPlaylist));
                if (lstPlaylist.Items.Count != StaticPlaylist.Count)
                {
                    ReloadPlaylist(Playlist, true, true, false);
                }
                if (PlayingSong == null) return;
                UpdateHighlights();
            }
        }

        private void pic2020s_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (pic2020s.Cursor == Cursors.No) return;
            picFavorites.Image = Resources.favorites_disabled;
            enableFavorites = false;
            enable2020s = !enable2020s;
            DisableSortByImages();
            ReloadPlaylist(Playlist, true, true, false);
        }

        private void pic2010s_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (pic2010s.Cursor == Cursors.No) return;
            picFavorites.Image = Resources.favorites_disabled;
            enableFavorites = false;
            enable2010s = !enable2010s;
            DisableSortByImages();
            ReloadPlaylist(Playlist, true, true, false);
        }

        private void pic2000s_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (pic2000s.Cursor == Cursors.No) return;
            picFavorites.Image = Resources.favorites_disabled;
            enableFavorites = false;
            enable2000s = !enable2000s;
            DisableSortByImages();
            ReloadPlaylist(Playlist, true, true, false);
        }

        private void pic1990s_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (pic1990s.Cursor == Cursors.No) return;
            picFavorites.Image = Resources.favorites_disabled;
            enableFavorites = false;
            enable1990s = !enable1990s;
            DisableSortByImages();
            ReloadPlaylist(Playlist, true, true, false);
        }

        private void pic1980s_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (pic1980s.Cursor == Cursors.No) return;
            picFavorites.Image = Resources.favorites_disabled;
            enableFavorites = false;
            enable1980s = !enable1980s;
            DisableSortByImages();
            ReloadPlaylist(Playlist, true, true, false);
        }

        private void pic1970s_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (pic1970s.Cursor == Cursors.No) return;
            picFavorites.Image = Resources.favorites_disabled;
            enableFavorites = false;
            enable1970s = !enable1970s;
            DisableSortByImages();
            ReloadPlaylist(Playlist, true, true, false);
        }

        private void pic1960s_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (pic1960s.Cursor == Cursors.No) return;
            picFavorites.Image = Resources.favorites_disabled;
            enableFavorites = false;
            enable1960s = !enable1960s;
            DisableSortByImages();
            ReloadPlaylist(Playlist, true, true, false);
        }

        private void picOldies_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (picOldies.Cursor == Cursors.No) return;
            picFavorites.Image = Resources.favorites_disabled;
            enableFavorites = false;
            enableOldies = !enableOldies;
            DisableSortByImages();
            ReloadPlaylist(Playlist, true, true, false);
        }

        private void picFavorites_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (picFavorites.Cursor == Cursors.No) return;
            enableFavorites = !enableFavorites;
            DisableSortByImages();

            if (!enableFavorites)
            {
                ReloadPlaylist(Playlist, true, true, false);
                return;
            }

            lstPlaylist.Items.Clear();
            lstPlaylist.Refresh();
            lstPlaylist.BeginUpdate();

            var favoritesByPath = favoritesList.ToDictionary(
                f => f.SongPath,
                f => f.PlayTimes,
                StringComparer.OrdinalIgnoreCase);

            // Map each song path in Playlist -> its position in Playlist (0-based)
            var playlistIndexByPath = Playlist
                .Select((song, idx) => new { song.Location, idx })
                .ToDictionary(x => x.Location, x => x.idx, StringComparer.OrdinalIgnoreCase);

            var topFavoriteSongs = Playlist
                .Where(s => favoritesByPath.ContainsKey(s.Location))
                .OrderByDescending(s => favoritesByPath[s.Location])
                .Take(50)
                .Select(s => new
                {
                    Song = s,
                    PlaylistIndex = playlistIndexByPath[s.Location], // 0-based
                    PlayTimes = favoritesByPath[s.Location]
                })
                .ToList();

            for (var i = 0; i < topFavoriteSongs.Count; i++)
            {               
                int realIndex = topFavoriteSongs[i].PlaylistIndex; // 0-based

                //format leading index number
                var digits = 3; //999 songs
                var index = "000";
                if (Playlist.Count > 99999)
                {
                    digits = 6; //999,999 songs ... unlikely but in case i'm not around
                    index = "000000";
                }
                else if (Playlist.Count > 9999)
                {
                    digits = 5; //99,999 songs
                    index = "00000";
                }
                else if (Playlist.Count > 999)
                {
                    digits = 4; //9,999 songs
                    index = "0000";
                }
                index = index + (realIndex + 1);
                index = index.Substring(index.Length - digits, digits);

                //add entry to playlist panel
                var entry = new ListViewItem(index);
                entry.SubItems.Add("[ x" + topFavoriteSongs[i].PlayTimes + " ] " + CleanArtistSong(Playlist[realIndex].Artist + " - " + CleanArtistSong(Playlist[realIndex].Name)));
                if (Playlist[i].Length == 0)
                {
                    entry.SubItems.Add("");//we don't have song duration for Fornite Festival m4a files so blank it out at this point
                }
                else
                {
                    entry.SubItems.Add(Parser.GetSongDuration(Playlist[realIndex].Length.ToString(CultureInfo.InvariantCulture)));
                }
                entry.BackColor = Color.AliceBlue;
                entry.Tag = 0; //not played
                lstPlaylist.Items.Add(entry);
            }
            lstPlaylist.EndUpdate();
        }

        private void DisableSortByImages()
        {
            picFavorites.Image = enableFavorites ? Resources.favorites_enabled : Resources.favorites_disabled;
            pic2020s.Image = enable2020s ? Resources._2020s_enabled : Resources._2020s_disabled;
            pic2010s.Image = enable2010s ? Resources._2010s_enabled : Resources._2010s_disabled;
            pic2000s.Image = enable2000s ? Resources._2000s_enabled : Resources._2000s_disabled;
            pic1990s.Image = enable1990s ? Resources._1990s_enabled : Resources._1990s_disabled;
            pic1980s.Image = enable1980s ? Resources._1980s_enabled : Resources._1980s_disabled;
            pic1970s.Image = enable1970s ? Resources._1970s_enabled : Resources._1970s_disabled;
            pic1960s.Image = enable1960s ? Resources._1960s_enabled : Resources._1960s_disabled;
            picOldies.Image = enableOldies ? Resources.oldies_enabled : Resources.oldies_disabled;

            if (!enableFavorites && !enable2020s && !enable2010s && !enable2000s && !enable1990s && !enable1980s && !enable1970s && !enable1960s && !enableOldies)
            {
                picFavorites.Image = Resources.favorites_disabled;
                pic2020s.Image = Resources._2020s_enabled;
                pic2010s.Image = Resources._2010s_enabled;
                pic2000s.Image = Resources._2000s_enabled;
                pic1990s.Image = Resources._1990s_enabled;
                pic1980s.Image = Resources._1980s_enabled;
                pic1970s.Image = Resources._1970s_enabled;
                pic1960s.Image = Resources._1960s_enabled;
                picOldies.Image = Resources.oldies_enabled;
            }
        }

        private void picSearch_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (txtSearch.Text == strSearchPlaylist || Playlist.Count == 0) return;
            ReloadPlaylist(Playlist, true, true, false);
        }

        private void picGenres_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            Point screenPos = picFilters.PointToScreen(Point.Empty);
            
            var picker = new frmFilters(this);
            picker.StartPosition = FormStartPosition.Manual;
            picker.Location = new Point(
                screenPos.X + (picFilters.Width - picker.Width) / 2,
                screenPos.Y + picFilters.Height
            );

            picker.Show(this);
        }

        public bool GetSolidColorBackgroundIsChecked()
        {
            return solidColorBackground.Checked;
        }

        private void solidColorBackgroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClickSolidColorBackground();
        }

        public void ClickSolidColorBackground()
        {
            if (!yarg.Checked || string.IsNullOrEmpty(CHVideoPath))
            {
                StopAllVideoPlayback();
            }
            if (yarg.Checked && !string.IsNullOrEmpty(CHVideoPath) && !VideoIsPlaying)
            {
                PlayCurrentVideo(CHVideoPath);
            }
            solidColorBackground.Checked = true;
            animatedBackground2.Checked = false;
            staticBackground2.Checked = false;
            stageTimer.Enabled = false;
            if (secondScreen != null)
            {
                if (yarg.Checked && !string.IsNullOrEmpty(CHVideoPath))
                {
                    secondScreen.ChangeBackgroundImage(null);
                    secondScreen.ChangeBackgroundColor(Color.Black);
                }
                else
                {
                    secondScreen.ChangeBackgroundImage(Resources.gradient);
                    secondScreen.ChangeBackgroundColor(picVisuals.BackColor);
                }
            }
            else
            {
                if (yarg.Checked && !string.IsNullOrEmpty(CHVideoPath))
                {
                    picVisuals.Image = null;
                    picVisuals.BackColor = Color.Black;
                }
                else
                {
                    picVisuals.Image = Resources.gradient;
                    picVisuals.BackColor = KaraokeBackgroundColor;
                }
            }
            if (classicKaraokeMode.Checked)
            {
                picVisuals.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void picSecondScreen_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (picSecondScreen.Tag == "disabled")
            {
                picSecondScreen.Tag = "enabled";
                picSecondScreen.Image = Resources.doublescreen_enabled;
                toolTip1.SetToolTip(picSecondScreen, "Click to disable second screen");
            }
            else
            {
                picSecondScreen.Tag = "disabled";
                picSecondScreen.Image = Resources.doublescreen_disabled;
                toolTip1.SetToolTip(picSecondScreen, "Click to enable second screen");
            }
            enableSecondScreen.PerformClick();
        }

        private int lastBackgroundIndex = 0;
        private Bitmap RBStyleBackground;

        private void PlayCurrentVideo(string videoPath)
        {
            long time = 0;
            try
            {
                time = (long)(PlaybackSeconds * 1000) + Parser.Songs[0].VideoStartTime;
            }
            catch { }
            StopAllVideoPlayback();
            if (secondScreen != null)
            {
                secondScreen.StartVideoPlayback(videoPath, PopOutScreen.VideoPathType.FromPath, time);
            }
            else
            {
                StartVideoPlayback(videoPath, VideoPathType.FromPath, time);
            }
        }

        private void ChangeRBStyleBackground()
        {
            if (!chartVisualsToolStripMenuItem.Checked || !rBStyle.Checked || !PlaybackTimer.Enabled) return;
            //if (yarg.Checked && ((secondScreen == null && VideoIsPlaying) || (secondScreen != null && secondScreen.VideoIsPlaying))) return;
            if (yarg.Checked && !string.IsNullOrEmpty(CHVideoPath))
            {
                PlayCurrentVideo(CHVideoPath);
                return;
            }           

            // VIDEOS
            if (doUseBackgroundVideos && BackgroundVideos != null && BackgroundVideos.Count > 0)
            {
                _videoBag.ResetCount(BackgroundVideos.Count);

                for (int attempts = 0; attempts < BackgroundVideos.Count; attempts++)
                {
                    int index = _videoBag.Next();
                    string path = BackgroundVideos[index];
                    if (!File.Exists(path))
                    {
                        if (BackgroundVideos.Count > 1)
                        {
                            continue;
                        }
                        else
                        {
                            return;
                        }
                    }

                    currentVideoPath = path;

                    if (secondScreen != null)
                        secondScreen.StartVideoPlayback(path, PopOutScreen.VideoPathType.FromPath, 0);
                    else
                        StartVideoPlayback(path, VideoPathType.FromPath, 0);

                    return;
                }

                // If we got here, all entries were missing
                return;
            }

            // IMAGES
            if (doUseBackgroundImages && BackgroundImages != null && BackgroundImages.Count > 0)
            {
                _imageBag.ResetCount(BackgroundImages.Count);

                for (int attempts = 0; attempts < BackgroundImages.Count; attempts++)
                {
                    int index = _imageBag.Next();
                    string path = BackgroundImages[index];
                    if (!File.Exists(path))
                    {
                        if (BackgroundImages.Count > 1)
                        {
                            continue;
                        }
                        else
                        {
                            return;
                        }
                    }

                    StopAllVideoPlayback();
                    RBStyleBackground?.Dispose();
                    RBStyleBackground = (Bitmap)Image.FromFile(path);
                    return;
                }
            }
        }

        public bool GetRBStyleIsChecked()
        {
            return rBStyle.Checked;
        }

        private void rBStyle_Click(object sender, EventArgs e)
        {
            ClickRBStyle();
        }

        public void ClickRBStyle()
        {            
            CheckUncheckAll(rBStyle);
            chartVisualsToolStripMenuItem.Checked = true;
            updateDisplayType(rBStyle);
            UpdateVisualStyle(rBStyle);
            if (secondScreen != null)
            {
                secondScreen.ChangeBackgroundColor(Color.Black);
            }
            if (yarg.Checked && !string.IsNullOrEmpty(CHVideoPath))
            {
                PlayCurrentVideo(CHVideoPath);
            }
        }

        public bool GetBackgroundVideosIsChecked()
        {
            return useBackgroundVideos.Checked;
        }

        private void useBackgroundVideos_Click(object sender, EventArgs e)
        {
            ClickBackgroundVideos();   
        }

        public void ClickBackgroundVideos()
        {
            //if (yarg.Checked && VideoIsPlaying) return;
            useBackgroundVideos.Checked = true;
            useBackgroundImages.Checked = false;
            changedBackground = false;
            doUseBackgroundVideos = true;
            doUseBackgroundImages = false;
            ChangeRBStyleBackground();
        }

        public bool GetBackgroundImagesIsChecked()
        {
            return useBackgroundImages.Checked;
        }

        private void useBackgroundImages_Click(object sender, EventArgs e)
        {
            ClickBackgroundImages();
        }

        public void ClickBackgroundImages()
        {
            //if (yarg.Checked && VideoIsPlaying) return;
            useBackgroundVideos.Checked = false;
            useBackgroundImages.Checked = true;
            changedBackground = false;
            doUseBackgroundVideos = false;
            doUseBackgroundImages = true;
            StopAllVideoPlayback();
            ChangeRBStyleBackground();
        }

        private void playToolStripMenuItem_Click(object sender, EventArgs e)
        {
            doClickPlay();
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            doClickPause();
        }

        private void nextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            doClickNext();
        }

        private void bluetoothAVOffset_Click(object sender, EventArgs e)
        {
            var bt = new BTAVSync(this, BTAVOffsetSync, enableBTAVOffsetSync);
            bt.Show();
        }

        private void setNautilusPath_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Title = "Select Nautilus executable";
            ofd.Filter = "Nautilus executable (*.exe)|*.exe";
            ofd.Multiselect = false;
            ofd.ShowDialog();
            nautilusPath = ofd.FileName;
            ValidateNautilusPath();
        }

        private void ValidateNautilusPath()
        {
            var valid = !string.IsNullOrEmpty(nautilusPath) && File.Exists(nautilusPath);
            sendToVisualizer.Enabled = valid;
            sendToFileAnalyzer.Enabled = valid && xbox360.Checked;
            sendToAudioAnalyzer.Enabled = valid && xbox360.Checked;
            sendToCONExplorer.Enabled = valid && xbox360.Checked;
        }

        private void sendToVisualizer_Click(object sender, EventArgs e)
        {
            if (ActiveSong == null)
            {
                MessageBox.Show("There is no active song", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string args = $"-visualizer \"-{ActiveSong.Location}\"";
            LaunchNautilus(args);
            
        }

        private void LaunchNautilus(string args)
        {
            var psi = new ProcessStartInfo
            {
                FileName = nautilusPath,
                Arguments = args,
                UseShellExecute = true,
                WorkingDirectory = Path.GetDirectoryName(nautilusPath)
            };
            Process.Start(psi);
        }

        private void sendToCONExplorer_Click(object sender, EventArgs e)
        {
            string args = $"\"{ActiveSong.Location}\"";
            LaunchNautilus(args);
        }

        private void sendToFileAnalyzer_Click(object sender, EventArgs e)
        {
            string args = $"-analyzer \"-{ActiveSong.Location}\"";
            LaunchNautilus(args);
        }

        private void sendToAudioAnalyzer_Click(object sender, EventArgs e)
        {
            string args = $"-audioa \"-{ActiveSong.Location}\"";
            LaunchNautilus(args);
        }

        private frmSettings settingsForm;

        private void changeViewToolStrip_Click(object sender, EventArgs e)
        {
            OpenSettingsForm();
        }

        public void OpenSettingsForm()
        {
            if (settingsForm == null || settingsForm.IsDisposed)
            {
                settingsForm = new frmSettings(this);                
            }
            settingsForm.Show();
        }               

        private void frmMain_SizeChanged(object sender, EventArgs e)
        {
            UpdateOverlayPosition();
        }

        private void frmMain_LocationChanged(object sender, EventArgs e)
        {
            UpdateOverlayPosition();
        }

        private void awesomenessDetection_Click(object sender, EventArgs e)
        {
            if (!awesomenessDetection.Checked) return;
            videoOverlay.TopMost = false;
            MessageBox.Show("Awesomeness Detection enabled!", "Nice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            videoOverlay.TopMost = true;
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

    public class FavoriteSong
    {
        public string SongPath = "";
        public int PlayTimes = 0;
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
        public string Languages { get; set; }
        public int VocalParts { get; set; }
    }

    public static class MathHelper
    {
        public static float Clamp(float value, float min, float max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        public static int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        public static double Clamp(double value, double min, double max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
    }

    public class ShuffleBag
    {
        private readonly Random _rng;
        private readonly List<int> _bag = new List<int>();
        private int _last = -1;
        private int _count = 0;

        public ShuffleBag(Random rng) => _rng = rng;

        public void ResetCount(int count)
        {
            if (count < 0) count = 0;
            if (_count != count)
            {
                _count = count;
                _bag.Clear();
                _last = -1;
            }
        }

        public int Next()
        {
            if (_count <= 0) return 0;

            // refill bag with 0..count-1 and shuffle in-place
            if (_bag.Count == 0)
            {
                for (int i = 0; i < _count; i++) _bag.Add(i);

                // Fisher–Yates shuffle
                for (int i = _bag.Count - 1; i > 0; i--)
                {
                    int j = _rng.Next(i + 1);
                    int tmp = _bag[i];
                    _bag[i] = _bag[j];
                    _bag[j] = tmp;
                }

                // optional: avoid repeating the last item across reshuffles
                if (_bag.Count > 1 && _bag[0] == _last)
                {
                    int swap = _rng.Next(1, _bag.Count);
                    int tmp = _bag[0];
                    _bag[0] = _bag[swap];
                    _bag[swap] = tmp;
                }
            }

            int idx = _bag[0];
            _bag.RemoveAt(0);
            _last = idx;
            return idx;
        }
    }

}

using cPlayer.Properties;
using cPlayer.StageKit;
using cPlayer.Texture;
using cPlayer.x360;
using HidSharp;
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
using System.ComponentModel.DataAnnotations;
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
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Enc;
using Un4seen.Bass.AddOn.Mix;
using Un4seen.Bass.AddOn.Opus;
using Un4seen.Bass.Misc;
using Vortice.XInput;
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
        private string PlayerConsole = "xbox";
        private double FadeLength = 1.0;
        public bool doAudioDrums = true;
        public bool doAudioBass = true;
        public bool doAudioGuitar = true;
        public bool doAudioKeys = true;
        public bool doAudioVocals = true;
        public bool doAudioBacking = true;
        public bool doAudioCrowd;
        public bool doMIDIDrums = true;
        public bool doMIDIBass = true;
        public bool doMIDIGuitar = true;
        public bool doMIDIProKeys = true;
        public bool doMIDIKeys;
        public bool doMIDINoKeys;
        public bool doMIDIVocals;
        public bool doMIDIHarmonies = true;
        public bool doMIDINoVocals;
        public bool doMIDINameVocals;
        public bool doMIDINameProKeys;
        public bool doStaticLyrics;
        public bool doScrollingLyrics = true;
        public bool doKaraokeLyrics;
        public bool doWholeWordsLyrics = true;
        public bool doHarmonyLyrics = true;
        public bool doMIDINameTracks = true;
        public bool doMIDIHighlightSolos = true;
        public bool doMIDIBWKeys = true;
        public bool doMIDIHarm1onVocals = true;
        private readonly Visuals Spectrum = new Visuals();
        public double PlaybackWindow = 3.0;
        private readonly double PlaybackWindowRB = 1.2;
        private readonly double PlaybackWindowRBVocals = 4.0;
        public int NoteSizingType;
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
        public int BassMixer;
        public int BassStream;
        private readonly List<int> BassStreams;
        public int SpectrumID;
        public Color SpectrumColor = Color.Black;
        private bool VideoIsPlaying;
        public List<PracticeSection> PracticeSessions;
        private string ImgToUpload;
        private string ImgURL;
        private bool showUpdateMessage;
        private bool AlreadyTried;
        private double IntroSilence;
        private double OutroSilence;
        private float SilenceThreshold = 0.25f;
        private bool AlreadyFading;
        private PlaylistSorting SortingStyle;
        private bool isClosing;
        private bool ShowingNotFoundMessage;
        private readonly nTools nautilus;
        private SongData ActiveSongData;
        private readonly LibVLC _libVLC;
        public MediaPlayer _mediaPlayer;
        private readonly VideoView videoView;
        private string[] opusFiles;
        private string[] oggFiles;
        private string[] mp3Files;
        private string[] wavFiles;
        private string[] cltFiles;
        private string[] m4aFiles;
        private string currentKLIC;
        private string pkgPath;
        private string sngPath;
        private string psarcPath;
        private string ghwtPath;
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
        private readonly Bitmap bmpFocusBG;
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
        private readonly Bitmap bmpBackgroundProKeysRB;
        private readonly Bitmap bmpBackgroundProKeysSoloRB;
        private readonly Bitmap bmpBackgroundGuitarRB;
        private readonly Bitmap bmpBackgroundGuitarSoloRB;
        private readonly Bitmap bmpBackgroundBassRB;
        private readonly Bitmap bmpBackgroundBassSoloRB;
        private readonly Bitmap bmpBackgroundDrumsRB;
        private readonly Bitmap bmpBackgroundDrumsSoloRB;
        private readonly Bitmap bmpBackgroundKeysRB;
        private readonly Bitmap bmpBackgroundKeysSoloRB;
        private readonly Bitmap bmpProKeysBlueGlow;
        private readonly Bitmap bmpProKeysYellowGlow;
        private readonly Bitmap bmpProKeysRedGlow;
        private readonly Bitmap bmpProKeysGreenGlow;
        private readonly Bitmap bmpProKeysWhiteLeftGlow;
        private readonly Bitmap bmpProKeysWhiteCenterGlow;
        private readonly Bitmap bmpProKeysWhiteRightGlow;
        private readonly Bitmap bmpProKeysWhiteFullGlow;
        private const int HitboxVocalsX = 200;
        private WaveInEvent waveIn;
        private WaveOutEvent waveOut;
        private BufferedWaveProvider bufferedWaveProvider;
        public VolumeWaveProvider16 volumeProvider;
        public int microphoneIndex = -1;
        private readonly List<StageKitController> stageKits = new List<StageKitController>();
        private readonly List<FatsCoHidLightController> fatsCoLights = new List<FatsCoHidLightController>();
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
        public bool isFullScreen;
        private readonly FormWindowState lastWindowState = FormWindowState.Maximized;
        private readonly List<Image> stageFrames;
        private Image stageBackground;
        private int redSKIndex;
        private int greenSKIndex;
        private int blueSKIndex;
        private int yellowSKIndex;
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
        private readonly List<FavoriteSong> favoritesList;
        public string genreFilter;
        public string instrumentFilter;
        public string languageFilter;
        private string CHVideoPath;
        public float defaultVol = 1.0f;
        public float bassVol = 1.0f;
        public float drumsVol = 1.0f;
        public float guitarVol = 1.0f;
        public float keysVol = 1.0f;
        public float vocalsVol = 1.0f;
        public float backingVol = 1.0f;
        public float crowdVol = 1.0f;
        public float masterVol = 0.8f;
        private readonly List<string> BackgroundImages;
        private readonly List<string> BackgroundVideos;
        public bool changedBackground;
        private Bitmap _renderedFrame;
        public bool doUseBackgroundVideosLast;
        public bool doUseBackgroundImagesLast;
        public bool doUseBackgroundVideos = true;
        public bool doUseBackgroundImages;
        public bool doFocusMode;
        public bool doAnimatedSpectrum;
        public bool doSpectrumColors;
        private readonly Random rng = new Random();
        private readonly ShuffleBag _videoBag;
        private readonly ShuffleBag _imageBag;
        private readonly ToolStripLabel statusLabel;
        public int BTAVOffsetSync;
        public bool enableBTAVOffsetSync;
        private string nautilusPath;
        private readonly frmHover hoverForm;
        public Size activeRenderingResolution = new Size(1920, 1080);
        private const double TrackBackgroundBeatsPerLoop = 2.0;
        private const bool ReverseTrackAnimation = true;
        public bool displayAlbumArt;
        public bool displayAudioSpectrum;
        public bool doVerticalChart;
        public bool doRockBandChart = true;
        public bool doMIDIChart;
        public bool doRockBandKaraoke;
        public bool enableYARGCHVideos = true;
        public bool doModernKaraokeMode;
        public bool doCPlayerStyleKaraoke;
        public bool doAnimatedBackground;
        public bool doStaticBackground = true;
        public bool doAnimatedBackground2;
        public bool doStaticBackground2 = true;
        public bool doForceSoloVocals = true;
        public bool doForceTwoPartHarmonies;
        public bool doSolidColorBackground = true;
        public bool doBackgroundImages;
        private int lastBackground;
        private BiQuadFilter highPassFilter = BiQuadFilter.HighPassFilter(44100, 100, 1); // 100 Hz cutoff, Q factor 1
        private Point lastCursorPos = Point.Empty;
        private int _lastDisplayedSecond = -1;
        private int _lastSliderLeft = int.MinValue;
        private static readonly Random _rng = new Random();
        private readonly Dictionary<string, TrackTrapezoidCache> _trackTrapezoidCache = new Dictionary<string, TrackTrapezoidCache>();
        private Dictionary<string, Bitmap[]> _rbLaneAnimatedFillNormalCache = new Dictionary<string, Bitmap[]>(StringComparer.Ordinal);
        private Dictionary<string, Bitmap[]> _rbLaneAnimatedFillSoloCache = new Dictionary<string, Bitmap[]>(StringComparer.Ordinal);
        private Dictionary<string, Bitmap> _rbLaneSoloOverlayCache = new Dictionary<string, Bitmap>(StringComparer.Ordinal);
        private Dictionary<string, Bitmap> _rbLaneFocusOverlayCache = new Dictionary<string, Bitmap>(StringComparer.Ordinal);
        private List<BeatMarker> _beatMarkers = new List<BeatMarker>();
        private Dictionary<string, Bitmap> _rbLaneNormalCache = new Dictionary<string, Bitmap>(StringComparer.Ordinal);
        private Dictionary<string, Bitmap> _rbLaneSoloCache = new Dictionary<string, Bitmap>(StringComparer.Ordinal);
        private string _rbLaneCacheKey;
        private int _trackAnimFrame;
        private readonly Stopwatch _trackAnimStopwatch = Stopwatch.StartNew();
        private bool _rbCacheBuildInProgress;
        private readonly object _rbCacheLock = new object();
        private bool _rbCacheRebuildRequested;
        const float HighwayAngleFactor = 0.48f;
        private readonly Bitmap needleHarm1 = Resources.needle_harm1;
        private readonly Bitmap needleHarm2 = Resources.needle_harm2;
        private readonly Bitmap needleHarm3 = Resources.needle_harm3;
        private readonly Font _impact12Font = new Font("Impact", 12.0f);
        private readonly List<AnimatedNote> activeNotes = new List<AnimatedNote>();
        private readonly Random rand = new Random();
        private bool doSoloVocals;
        private bool doHarm2;
        private bool doHarm3 = true;
        private double highlightDelay = 1.5;
        private double timeGap = 5.0;
        private bool doEnableHighlightAnimation = true;
        private bool doShowLoadingBar = true;
        private const string loadingBarXL = "████████████████████████████████";
        const int spawnFrequency = 30;
        private int noteCounter = spawnFrequency;
        private Bitmap loadingBarBaseBmp;
        private Bitmap loadingBarHighlightBmp;
        private Size loadingBarSize;
        private Font loadingBarFont;
        private readonly Font _karaokeBaseFont = new Font("Arial", 24f);
        private readonly Dictionary<string, CachedKaraokeLine> _karaokeLineCache = new Dictionary<string, CachedKaraokeLine>();
        private static readonly Random Rng = new Random();
        private int _lastStageKitBeatIndex = -1;
        private int _lastStageKitSubBeatIndex = -1;
        private int _lastStageKitMeasureIndex = -1;
        private double _nextFogEligible = 0;
        private StageKitLedPattern _currentStageKitPattern = StageKitLedPattern.OneEachSameDirection;
        private StageKitLedPattern _previousStageKitPattern = StageKitLedPattern.OneEachSameDirection;
        private int _stageKitPatternStartStep = 0;
        private int _stageKitPatternLength = 32;
        private readonly object _stageKitCommandLock = new object();
        private readonly AutoResetEvent _stageKitCommandSignal = new AutoResetEvent(false);
        private readonly Queue<Action> _stageKitCommandQueue = new Queue<Action>();
        private Thread _stageKitCommandThread;
        private volatile bool _stageKitCommandWorkerRunning;
        private const int VLCBuffer = 250;
        private List<Bitmap> _cachedRBKaraokeAnimatedFrames;
        private Size _cachedRBKaraokeAnimatedFrameSize = Size.Empty;
        private int _cachedRBKaraokeAnimatedSourceCount = -1;
        private List<Image> _cachedRBKaraokeAnimatedSourceRef;
        private int stageCounter = 0;
        private Bitmap _scaledFrame;
        private readonly Stopwatch _fpsWatch = Stopwatch.StartNew();
        private readonly Stopwatch _frameWatch = Stopwatch.StartNew();
        private long _lastFrameTick = 0;
        private long _fpsFrameCount = 0;
        private long _currentFps = 0;
        private long frameMs = 0;
        private string _lastPracticeSectionText = null;
        private bool _lastPracticeSectionVisible = false;
        private int _lastPracticeSectionIndex = -1;
        private const double depthPower = 1.50;
        private const float horizonPercent = 0.40f;
        private const float overshootPx = 20f;
        private readonly HashSet<long> _stageKitTriggeredKickTicks = new HashSet<long>();
        private double _lastKickStrobeTime = -999;
        private readonly Font _lyricsFont = new Font("Segoe UI", 16f, FontStyle.Bold);
        private readonly Dictionary<string, Bitmap> _lyricRowBgCache = new Dictionary<string, Bitmap>(StringComparer.Ordinal);
        private Media _currentVlcMedia;
        private readonly object _vlcMediaLock = new object();
        private int _stopInProgress;
        private Image LargeAlbumArt = null;
        private Image OriginalAlbumArt = null;
        private Color _cachedMoodColor = Color.AliceBlue;
        private readonly Brush _lightGrayBrush = new SolidBrush(Color.LightGray);
        private Color _lastPicVisualsBackColor = Color.AliceBlue;
        private Color _lastSecondScreenBackColor = Color.AliceBlue;
        private bool isResizing = false;
        private frmHelpHub helpHubForm;
        private MIDISelector midiSelectorForm;
        private LyricSelector lyricSelectorForm;
        private ChangeLog changeLogForm;
        private MicControl micControlForm;
        private PopOutScreen secondScreen;
        private BTAVSync btSyncForm;
        private frmSettings settingsForm;
        private AudioMixer audioMixerForm;
        public string currentVideoPath;
        private Size screenSize = new Size();
        private Bitmap _cachedRBKaraokeStaticBackground;
        private Size _cachedRBKaraokeStaticBackgroundSize = Size.Empty;
        private Image _cachedRBKaraokeStaticBackgroundSource;
        private int _applyARInProgress;
        private volatile string _pendingAspectRatio;
        private string _currentVideoPath;
        private VideoPathType _currentVideoType;
        private Media _currentMedia;
        private Bitmap RBStyleBackground;
        private static readonly List<int> stageKitIndices = new List<int>();

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

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
            bmpFocusBG = Resources.focus;
            bmpDrumsCymbalB = Resources.drums_cymbal_b;
            bmpDrumsCymbalY = Resources.drums_cymbal_y;
            bmpDrumsCymbalG = Resources.drums_cymbal_g;
            bmpDrumsCymbalOD = Resources.drums_cymbal_od;
            bmpGreenHopo = Resources.note_green_hopo;
            bmpRedHopo = Resources.note_red_hopo;
            bmpYellowHopo = Resources.note_yellow_hopo;
            bmpBlueHopo = Resources.note_blue_hopo;
            bmpOrangeHopo = Resources.note_orange_hopo;
            bmpODHopo = Resources.note_overdrive_hopo;
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
            bmpBackgroundProKeysRB = CreateVerticalMirroredLoop(Resources.background_prokeysRB);
            bmpBackgroundProKeysSoloRB = CreateVerticalMirroredLoop(Resources.background_prokeys_soloRB);
            bmpBackgroundGuitarRB = CreateVerticalMirroredLoop(Resources.background_guitar);
            bmpBackgroundGuitarSoloRB = CreateVerticalMirroredLoop(Resources.background_guitar_solo);
            bmpBackgroundBassRB = CreateVerticalMirroredLoop(Resources.background_bass);
            bmpBackgroundBassSoloRB = CreateVerticalMirroredLoop(Resources.background_bass_solo);
            bmpBackgroundDrumsRB = CreateVerticalMirroredLoop(Resources.background_drums);
            bmpBackgroundDrumsSoloRB = CreateVerticalMirroredLoop(Resources.background_drums_solo);
            bmpBackgroundKeysRB = CreateVerticalMirroredLoop(Resources.background_keys);
            bmpBackgroundKeysSoloRB = CreateVerticalMirroredLoop(Resources.background_keys_solo);
            bmpProKeysBlueGlow = Resources.prokeys_blue_glow;
            bmpProKeysYellowGlow = Resources.prokeys_yellow_glow;
            bmpProKeysRedGlow = Resources.prokeys_red_glow;
            bmpProKeysGreenGlow = Resources.prokeys_green_glow;
            bmpProKeysWhiteLeftGlow = Resources.prokeys_white_left_glow;
            bmpProKeysWhiteCenterGlow = Resources.prokeys_white_center_glow;
            bmpProKeysWhiteRightGlow = Resources.prokeys_white_right_glow;
            bmpProKeysWhiteFullGlow = Resources.prokeys_white_full_glow;
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

        public void ApplyMasterVolume()
        {
            if (BassMixer == 0)
                return;

            Bass.BASS_ChannelSetAttribute(
                BassMixer,
                BASSAttribute.BASS_ATTRIB_VOL,
                masterVol
            );
        }

        private Bitmap CreateVerticalMirroredLoop(Image source, float opacity = 0.90f)
        {
            Bitmap src = new Bitmap(source);
            Bitmap loop = new Bitmap(src.Width, src.Height * 2, PixelFormat.Format32bppPArgb);

            using (Graphics g = Graphics.FromImage(loop))
            using (var imageAttributes = new ImageAttributes())
            {
                var colorMatrix = new ColorMatrix
                {
                    Matrix00 = 1.0f,
                    Matrix11 = 1.0f,
                    Matrix22 = 1.0f,
                    Matrix33 = opacity,
                    Matrix44 = 1.0f
                };

                imageAttributes.SetColorMatrix(
                    colorMatrix,
                    ColorMatrixFlag.Default,
                    ColorAdjustType.Bitmap);

                g.DrawImage(
                    src,
                    new Rectangle(0, 0, src.Width, src.Height),
                    0,
                    0,
                    src.Width,
                    src.Height,
                    GraphicsUnit.Pixel,
                    imageAttributes);

                using (Bitmap flipped = (Bitmap)src.Clone())
                {
                    flipped.RotateFlip(RotateFlipType.RotateNoneFlipY);

                    g.DrawImage(
                        flipped,
                        new Rectangle(0, src.Height, src.Width, src.Height),
                        0,
                        0,
                        src.Width,
                        src.Height,
                        GraphicsUnit.Pixel,
                        imageAttributes);
                }
            }

            src.Dispose();
            return loop;
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
            foreach (var stageKit in stageKits)
            {
                stageKit.TurnFogOff();
            }
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
            if (!displayAudioSpectrum || PlayingSong == null) return;
            SpectrumID++;
            SafeVisualsSetter(null);
            //picVisuals.Image = null;
            try
            {
                Spectrum.ClearPeaks();
            }
            catch { }
        }
        
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
            SafeVisualsSetter(Resources.logo);
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
            audioMixerTool.Enabled = fileToolStripMenuItem.Enabled;
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
                            MIDITools.Initialize(false);
                            if (MIDITools.ReadMIDIFile(NextSongMIDI, song.HOPOThreshold, false))
                            {
                                newSong.BPM = MIDITools.MIDIInfo.AverageBPM;
                            }
                        }
                        else
                        {
                            MIDITools.Initialize(true);
                            Tools.DeleteFile(CurrentSongMIDI);
                        }
                    }
                }
                catch
                {
                    Tools.DeleteFile(MIDI);
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
                        MIDITools.Initialize(false);
                        if (MIDITools.ReadMIDIFile(NextSongMIDI, song.HOPOThreshold, false))
                        {
                            newSong.BPM = MIDITools.MIDIInfo.AverageBPM;
                        }
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
            if (File.Exists(notesChart) && !File.Exists(notesMIDI))
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
                    if (MIDITools.ReadMIDIFile(NextSongMIDI, song.HOPOThreshold, false))
                    {
                        newSong.BPM = MIDITools.MIDIInfo.AverageBPM;
                    }
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
                catch { }
            }
        }

        private void loadCON(string con, bool message = false, bool scanning = true, bool next = false, bool prep = false)
        {
            if (!ValidateDTAFile(con, message)) return;

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
            catch
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
            catch
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
                //NotifyTray_MouseDoubleClick(null, null);
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
            Text = AppName + " - " + (string.IsNullOrEmpty(PlaylistName) ? "[No Playlist Loaded]" : PlaylistName) + " - " + text;
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
            StopStageKits();
            StopStageKitCommandWorker();
            Bass.BASS_Free();
            SaveConfig();
            DeleteUsedFiles();
            var folder = Application.StartupPath + "\\temp\\";
            Tools.DeleteFolder(folder, true);
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
            PlaybackTimer.Enabled = false;
            StopPlayback();
            ClearVisuals(true);
            lblSections.Text = "";
            PlaybackSeconds = 0;
            _renderedFrame = null;
            SetPicVisualsBackColorIfChanged(Color.AliceBlue);
            SafeVisualsSetter(Resources.logo);
            videoOverlay.Hide();
            if (secondScreen != null)
            {
                SetSecondScreenBackColorIfChanged(Color.AliceBlue);
                secondScreen.ChangeVisualsImage(Resources.logo);
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
                        matrix = DoMatrixPanning(matrix, ArrangedChannels, 2, 0, Stem.Drums);
                        break;
                    case 3:
                        //mono kick
                        matrix = DoMatrixPanning(matrix, ArrangedChannels, 1, 0, Stem.Drums);
                        //stereo kit
                        matrix = DoMatrixPanning(matrix, ArrangedChannels, 2, 1, Stem.Drums);
                        break;
                    case 4:
                        //mono kick
                        matrix = DoMatrixPanning(matrix, ArrangedChannels, 1, 0, Stem.Drums);
                        //mono snare
                        matrix = DoMatrixPanning(matrix, ArrangedChannels, 1, 1, Stem.Drums);
                        //stereo kit
                        matrix = DoMatrixPanning(matrix, ArrangedChannels, 2, 2, Stem.Drums);
                        break;
                    case 5:
                        //mono kick
                        matrix = DoMatrixPanning(matrix, ArrangedChannels, 1, 0, Stem.Drums);
                        //stereo snare
                        matrix = DoMatrixPanning(matrix, ArrangedChannels, 2, 1, Stem.Drums);
                        //stereo kit
                        matrix = DoMatrixPanning(matrix, ArrangedChannels, 2, 3, Stem.Drums);
                        break;
                    case 6:
                        //stereo kick
                        matrix = DoMatrixPanning(matrix, ArrangedChannels, 2, 0, Stem.Drums);
                        //stereo snare
                        matrix = DoMatrixPanning(matrix, ArrangedChannels, 2, 2, Stem.Drums);
                        //stereo kit
                        matrix = DoMatrixPanning(matrix, ArrangedChannels, 2, 4, Stem.Drums);
                        break;
                }
            }
            //var channel = song.ChannelsDrums;
            if (ActiveSongData.ChannelsBass > 0 && doAudioBass)
            {
                matrix = DoMatrixPanning(matrix, ArrangedChannels, ActiveSongData.ChannelsBass, ActiveSongData.ChannelsBassStart, Stem.Bass);//channel);
            }
            //channel = channel + song.ChannelsBass;
            if (ActiveSongData.ChannelsGuitar > 0 && doAudioGuitar)
            {
                matrix = DoMatrixPanning(matrix, ArrangedChannels, ActiveSongData.ChannelsGuitar, ActiveSongData.ChannelsGuitarStart, Stem.Guitar);//channel);
            }
            //channel = channel + song.ChannelsGuitar;
            if (ActiveSongData.ChannelsVocals > 0 && doAudioVocals)
            {
                matrix = DoMatrixPanning(matrix, ArrangedChannels, ActiveSongData.ChannelsVocals, ActiveSongData.ChannelsVocalsStart, Stem.Vocals);//channel);
            }
            //channel = channel + song.ChannelsVocals;
            if (ActiveSongData.ChannelsKeys > 0 && doAudioKeys)
            {
                matrix = DoMatrixPanning(matrix, ArrangedChannels, ActiveSongData.ChannelsKeys, ActiveSongData.ChannelsKeysStart, Stem.Keys);//channel);
            }
            //channel = channel + song.ChannelsKeys;
            if (ActiveSongData.ChannelsCrowd > 0 && doAudioCrowd)
            {
                matrix = DoMatrixPanning(matrix, ArrangedChannels, ActiveSongData.ChannelsCrowd, ActiveSongData.ChannelsCrowdStart, Stem.Crowd);//channel);
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
                        matrix = DoMatrixPanning(matrix, ArrangedChannels, backing, ActiveSongData.ChannelsCrowdStart - backing, Stem.Backing);//channel);                        
                    }
                    else
                    {
                        matrix = DoMatrixPanning(matrix, ArrangedChannels, backing, ActiveSongData.ChannelsTotal - backing, Stem.Backing);//channel);
                    }
                }
            }
            return matrix;
        }

        private enum Stem
        {
            Bass, Drums, Guitar, Keys, Vocals, Backing, Crowd
        }

        private float[,] DoMatrixPanning(float[,] in_matrix, IList<int> ArrangedChannels, int inst_channels, int curr_channel, Stem stem)
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
            const double maxLevel = 1.0;

            //technically we could do each channel, but Magma only allows us to specify volume per track, 
            //so both channels should have same volume, let's save a tiny bit of processing power
            float vol;
            try
            {
                vol = (float)Utils.DBToLevel(Convert.ToDouble(volumes[ArrangedChannels[curr_channel]]), maxLevel);
            }
            catch (Exception)
            {
                vol = 1.0f;
            }

            switch (stem)
            {
                case Stem.Bass:
                    vol = vol * bassVol;
                    break;
                case Stem.Guitar:
                    vol = vol * guitarVol;
                    break;
                case Stem.Drums:
                    vol = vol * drumsVol;
                    break;
                case Stem.Keys:
                    vol = vol * keysVol;
                    break;
                case Stem.Vocals:
                    vol = vol * vocalsVol;
                    break;
                case Stem.Backing:
                    vol = vol * backingVol;
                    break;
                case Stem.Crowd:
                    vol = vol * crowdVol;
                    break;
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
            var timeSelection = seek ? PlaybackSeek : PlaybackSeconds;

            if (PlayingSong != null && timeSelection * 1000 > PlayingSong.Length)
            {
                picNext_MouseClick(null, null);
                return;
            }

            int wholeSeconds = (int)timeSelection;

            // Only update the time label when the visible second changes
            if (wholeSeconds != _lastDisplayedSecond)
            {
                _lastDisplayedSecond = wholeSeconds;

                string time;

                if (timeSelection >= 3600)
                {
                    var hours = (int)(timeSelection / 3600);
                    var minutes = (int)((timeSelection - (hours * 3600)) / 60);
                    var seconds = (int)(timeSelection - (hours * 3600) - (minutes * 60));
                    time = hours + ":" + (minutes < 10 ? "0" : "") + minutes + ":" + (seconds < 10 ? "0" : "") + seconds;
                }
                else if (timeSelection >= 60)
                {
                    var minutes = (int)(timeSelection / 60);
                    var seconds = (int)(timeSelection - (minutes * 60));
                    time = minutes + ":" + (seconds < 10 ? "0" : "") + seconds;
                }
                else
                {
                    time = "0:" + (timeSelection < 10 ? "0" : "") + wholeSeconds;
                }

                lblTime.Text = time;
            }

            if (panelSlider.Cursor == Cursors.NoMoveHoriz || reset || PlayingSong == null)
                return;

            var percent = timeSelection / ((double)PlayingSong.Length / 1000);
            int sliderLeft = panelLine.Left + (int)((panelLine.Width - panelSlider.Width) * percent);

            // Only move slider if pixel position actually changed
            if (sliderLeft != _lastSliderLeft)
            {
                _lastSliderLeft = sliderLeft;
                panelSlider.Left = sliderLeft;
            }

            if (!update) return;

            DoPracticeSessions(GetCorrectedTime());
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
                //var track_vol = (float)Utils.DBToLevel(Convert.ToDouble(-1 * (MinVolume - VolumeLevel)), 1.0);
                Bass.BASS_ChannelSetAttribute(BassMixer, BASSAttribute.BASS_ATTRIB_VOL, masterVol);
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
            _ = StartPlaybackAsync(doFade, false);
        }
                
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
                displayAlbumArt = false;
                displayAudioSpectrum = true;
                if (!displayAudioSpectrum)
                {
                    toolTip1.SetToolTip(picPreview, "Click to change spectrum style");
                }
            }
            UpdateButtons();
            UpdateDisplay();
            _ = StartPlaybackAsync(PlaybackSeconds == 0, true);
        }

        private void EnableDisableButtons(bool enabled)
        {
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
                displayAudioSpectrum = true;
            }
            PracticeSessions = MIDITools.PracticeSessions;
            if (!MIDITools.PhrasesVocals.Phrases.Any() || !MIDITools.LyricsVocals.Lyrics.Any())
            {
                displayAudioSpectrum = true;
            }
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
            if (MIDITools.MIDI_Chart.Vocals.NoteRange.Count > 8 || doVerticalChart || doRockBandChart)
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
            if (MIDITools.MIDI_Chart == null) return;
            const int tall = 2;
            var tracks = GetTrackstoDraw();
            if (tracks == 0) return;
            var panel_height = size.Height - GetHeightDiff();
            var track_height = panel_height / tracks;
            var track_y = lblSections.Visible ? lblSections.Height : 0;
            int Index;
            var track_color = 1;
            var renderSize = activeRenderingResolution;// new Size(1920, 1080);

            if (secondScreen != null)
            {
                SetSecondScreenBackColorIfChanged(Color.Black);
                SetPicVisualsBackColorIfChanged(Color.AliceBlue);
            }
            else
            {
                SetPicVisualsBackColorIfChanged(Color.Black);
            }

            if (doMIDIChart)
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
                _ = DrawRockBandStyleAsync(graphics, false);
            }
            List<MIDITrack> activeTracks = new List<MIDITrack>();
            var waitY = GetYForRBVocals() + (vocalsHeight / 2);
            if (MIDITools.PhrasesVocals != null && MIDITools.MIDI_Chart.Vocals.ChartedNotes.Count > 0)
            {
                var multVocals = 1;
                if (MIDITools.MIDI_Chart.Vocals.NoteRange.Count > 8 || doVerticalChart || doRockBandChart)
                {
                    multVocals = tall;
                }
                if ((doMIDIVocals || doMIDIHarmonies) && !doMIDINoVocals)
                {
                    if (doVerticalChart || doRockBandChart)
                    {
                        if (doVerticalChart)
                        {
                            using (var overlayBrush = new SolidBrush(
                            Color.FromArgb(doVerticalChart ? 255 : 128, Color.Black)))
                            {
                                graphics.FillRectangle(overlayBrush, 0, 0, renderSize.Width, vocalsHeight + 8);
                            }
                        }
                        graphics.DrawImage(doVerticalChart ? Resources.frostedglass75 : Resources.frostedglass50, 0, doRockBandChart ? GetYForRBVocals() : 0, renderSize.Width, vocalsHeight + 8);
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
                if (!doMIDIChart)
                {
                    DrawLyrics(size, graphics, (doRockBandChart || doVerticalChart || doMIDIChart) ? RBStyleVocalsBackgroundColor : Color.FromArgb(127, 200, 200, 200));
                }
                if ((!doMIDIVocals && !doMIDIHarmonies) || doMIDINoVocals) return;                
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
                if (doMIDIChart)
                {
                    DrawLyrics(size, graphics, (doRockBandChart || doVerticalChart || doMIDIChart) ? RBStyleVocalsBackgroundColor : Color.FromArgb(127, 200, 200, 200));
                }
            }
            if (doVerticalChart || doRockBandChart)
            {
                double time = GetCorrectedTime();

                const double gapSeconds = 5.0;
                const double grace = 0.05;
                double window = PlaybackWindowRB;

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
                        // i.e., once time is past latestVisibleEnd (plus grace if we want).
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

            if ((!doVerticalChart && !doRockBandChart) || MIDITools.PhrasesVocals.Phrases.Count == 0) return;
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

            if (!doMIDINameTracks || (!doVerticalChart && !doRockBandChart)) return;
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
            TextRenderer.DrawText(graphics, trackName, font, new Point(centeredX, centeredY), Color.FromArgb(127, 0, 0, 0));
        }

        private sealed class TrackTrapezoidCache
        {
            public string Key = "";
            public GraphicsPath ClipPath;
            public RectangleF[] DestRects = Array.Empty<RectangleF>();
            public float TopLeftX;
            public float TopRightX;
            public float BottomLeftX;
            public float BottomRightX;
            public float HorizonY;
            public float HitboxY;
            public int Strips;
        }
        
        private TrackTrapezoidCache GetOrCreateTrackTrapezoidCache(
            int chartLeft,
            int topY,
            int trackHeight,
            int trackWidth,
            float horizonY,
            float hitboxY,
            int strips)
        {
            string key = string.Join("|",
                chartLeft,
                topY,
                trackHeight,
                trackWidth,
                horizonY.ToString("0.###"),
                hitboxY.ToString("0.###"),
                strips);

            if (_trackTrapezoidCache.TryGetValue(key, out var cached))
                return cached;

            float fullTop = topY;
            float fullBottom = topY + trackHeight;

            if (hitboxY > fullBottom) hitboxY = fullBottom;
            if (hitboxY < fullTop + 2) hitboxY = fullBottom;

            if (horizonY < fullTop) horizonY = fullTop;
            if (horizonY > hitboxY - 2) horizonY = fullTop;

            float spanY = hitboxY - horizonY;

            float centerX = chartLeft + (trackWidth / 2f);

            const float topWidthFactor = HighwayAngleFactor;
            float topW = trackWidth * topWidthFactor;

            float topLeftX = centerX - (topW / 2f);
            float topRightX = centerX + (topW / 2f);
            float bottomLeftX = chartLeft;
            float bottomRightX = chartLeft + trackWidth;

            float LerpF(float a, float b, float t) => a + (b - a) * t;

            var path = new GraphicsPath();
            path.AddPolygon(new[]
            {
                new PointF(topLeftX, horizonY),
                new PointF(topRightX, horizonY),
                new PointF(bottomRightX, hitboxY),
                new PointF(bottomLeftX, hitboxY)
            });

            var destRects = new RectangleF[strips];

            for (int i = 0; i < strips; i++)
            {
                float t0 = (float)i / strips;
                float t1 = (float)(i + 1) / strips;

                float y0 = horizonY + (spanY * t0);
                float y1 = horizonY + (spanY * t1);
                float h = Math.Max(1f, y1 - y0);

                float leftX = LerpF(topLeftX, bottomLeftX, t0);
                float rightX = LerpF(topRightX, bottomRightX, t0);
                float w = Math.Max(1f, rightX - leftX);

                destRects[i] = new RectangleF(leftX, y0, w, h);
            }

            var entry = new TrackTrapezoidCache
            {
                Key = key,
                ClipPath = path,
                DestRects = destRects,
                TopLeftX = topLeftX,
                TopRightX = topRightX,
                BottomLeftX = bottomLeftX,
                BottomRightX = bottomRightX,
                HorizonY = horizonY,
                HitboxY = hitboxY,
                Strips = strips
            };

            _trackTrapezoidCache[key] = entry;
            return entry;
        }

        private Bitmap BuildFadedSoloOverlayBitmap(RBLaneLayout lane, Size renderSize)
        {
            float hitboxY = renderSize.Height - 50f;
            const float horizonPercent = 0.50f;
            float horizonY = hitboxY * horizonPercent;
            float trapezoidBottom = lane.Id == "ProKeys" ? hitboxY : hitboxY + 20f;

            var highlight = GetRockBandBackgroundBitmap(lane.Id, true);
            if (highlight == null)
                return null;

            int drawWidth = lane.Width;

            var bmp = new Bitmap(Math.Max(1, lane.X + drawWidth), renderSize.Height, PixelFormat.Format32bppPArgb);

            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent);

                DrawTrackPerspectiveTrapezoidSoloFaded(
                    g,
                    highlight,
                    lane.X,
                    0,
                    renderSize.Height,
                    drawWidth,
                    horizonY,
                    trapezoidBottom,
                    0.80f,
                    1.00f
                );
            }

            return bmp;
        }

        private Bitmap BuildFadedFocusOverlayBitmap(RBLaneLayout lane, Size renderSize)
        {
            if (bmpFocusBG == null)
                return null;

            float hitboxY = renderSize.Height - 50f;
            const float horizonPercent = 0.50f;
            float horizonY = hitboxY * horizonPercent;
            float trapezoidBottom = lane.Id == "ProKeys" ? hitboxY : hitboxY + 20f;

            int drawWidth = lane.Id == "ProKeys" ? lane.Width * 2 : lane.Width;

            var bmp = new Bitmap(Math.Max(1, lane.X + lane.Width), renderSize.Height, PixelFormat.Format32bppPArgb);

            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent);

                DrawTrackPerspectiveTrapezoidSoloFaded(
                    g,
                    bmpFocusBG,
                    lane.X,
                    0,
                    renderSize.Height,
                    lane.Width,
                    horizonY,
                    trapezoidBottom,
                    0.80f,
                    1.00f
                );
            }

            return bmp;
        }

        private Bitmap[] BuildAnimatedTrackFillFrames(RBLaneLayout lane, bool isSolo, Size renderSize, int frameCount = 16)
        {
            var frames = new Bitmap[frameCount];

            float hitboxY = renderSize.Height - 50f;
            const float horizonPercent = 0.50f;
            float horizonY = hitboxY * horizonPercent;
            float trapezoidBottom = lane.Id == "ProKeys" ? hitboxY : hitboxY + 20f;

            var bg = GetRockBandBackgroundBitmap(lane.Id, isSolo);
            if (bg == null)
                return frames;

            int srcH = bg.Height;
            int wrapHeight = srcH / 2;
            if (wrapHeight <= 0) wrapHeight = srcH;
            if (wrapHeight <= 0)
                return frames;

            for (int i = 0; i < frameCount; i++)
            {
                float offset = (wrapHeight * i) / (float)frameCount;

                var bmp = new Bitmap(Math.Max(1, lane.Width), renderSize.Height, PixelFormat.Format32bppPArgb);

                using (var g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.Transparent);

                    DrawTrackPerspectiveTrapezoidFilled(
                        g,
                        bg,
                        0,
                        0,
                        renderSize.Height,
                        lane.Width,
                        horizonY,
                        trapezoidBottom,
                        offset,
                        strips: 48
                    );
                }

                // Fade top of finished cached frame:
                // start fade around 80% of the visible track, fully gone by the very top
                ApplyTopFadeToTrackFrame(bmp, horizonY, trapezoidBottom, 0.80f, 1.00f);

                frames[i] = bmp;
            }

            return frames;
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
            float scrollOffset,
            int strips = 48)
        {
            if (trackBmp == null || trackHeight <= 0 || trackWidth <= 0) return;

            var cache = GetOrCreateTrackTrapezoidCache(
                chartLeft,
                topY,
                trackHeight,
                trackWidth,
                horizonY,
                hitboxY,
                strips);

            Region oldClip = g.Clip;
            var oldInterp = g.InterpolationMode;
            var oldPixel = g.PixelOffsetMode;

            g.SetClip(cache.ClipPath);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            int srcW = trackBmp.Width;
            int srcH = trackBmp.Height;
            if (srcW <= 0 || srcH <= 0)
            {
                g.InterpolationMode = oldInterp;
                g.PixelOffsetMode = oldPixel;
                g.Clip = oldClip;
                return;
            }

            int wrapHeight = srcH / 2;
            if (wrapHeight <= 0) wrapHeight = srcH;
            if (wrapHeight <= 0)
            {
                g.InterpolationMode = oldInterp;
                g.PixelOffsetMode = oldPixel;
                g.Clip = oldClip;
                return;
            }

            for (int i = 0; i < cache.Strips; i++)
            {
                float t0 = (float)i / cache.Strips;
                float t1 = (float)(i + 1) / cache.Strips;

                RectangleF dstRect = cache.DestRects[i];

                int sy0 = ((int)Math.Round(wrapHeight * t0 + scrollOffset)) % srcH;
                int sy1 = ((int)Math.Round(wrapHeight * t1 + scrollOffset)) % srcH;

                if (sy0 < 0) sy0 += srcH;
                if (sy1 < 0) sy1 += srcH;

                if (sy1 > sy0)
                {
                    int sh = Math.Max(1, sy1 - sy0);
                    var srcRect = new Rectangle(0, sy0, srcW, sh);
                    g.DrawImage(trackBmp, dstRect, srcRect, GraphicsUnit.Pixel);
                }
                else
                {
                    int firstPartH = srcH - sy0;
                    int secondPartH = sy1;

                    int totalSrcH = firstPartH + secondPartH;
                    if (totalSrcH <= 0) totalSrcH = 1;

                    float firstDstH = dstRect.Height * (firstPartH / (float)totalSrcH);
                    float secondDstH = dstRect.Height - firstDstH;

                    if (firstPartH > 0)
                    {
                        var srcRect1 = new Rectangle(0, sy0, srcW, firstPartH);
                        var dstRect1 = new RectangleF(dstRect.X, dstRect.Y, dstRect.Width, firstDstH);
                        g.DrawImage(trackBmp, dstRect1, srcRect1, GraphicsUnit.Pixel);
                    }

                    if (secondPartH > 0)
                    {
                        var srcRect2 = new Rectangle(0, 0, srcW, secondPartH);
                        var dstRect2 = new RectangleF(dstRect.X, dstRect.Y + firstDstH, dstRect.Width, secondDstH);
                        g.DrawImage(trackBmp, dstRect2, srcRect2, GraphicsUnit.Pixel);
                    }
                }
            }

            g.InterpolationMode = oldInterp;
            g.PixelOffsetMode = oldPixel;
            g.Clip = oldClip;

            using (var pen = new Pen(Color.FromArgb(60, Color.White), 1f))
            {
                g.DrawLine(pen, cache.TopLeftX, cache.HorizonY, cache.BottomLeftX, cache.HitboxY);
                g.DrawLine(pen, cache.TopRightX, cache.HorizonY, cache.BottomRightX, cache.HitboxY);
            }
        }

        private void ApplyTopFadeToTrackFrame(
            Bitmap bmp,
            float horizonY,
            float bottomY,
            float fadeStartPercent = 0.80f,   // start fading at 80% up from bottom
            float fadeEndPercent = 1.00f      // fully gone at 100% (top)
        )
        {
            if (bmp == null) return;
            if (bottomY <= horizonY) return;

            fadeStartPercent = Math.Max(0f, Math.Min(1f, fadeStartPercent));
            fadeEndPercent = Math.Max(0f, Math.Min(1f, fadeEndPercent));
            if (fadeEndPercent <= fadeStartPercent) return;

            int top = (int)Math.Round(horizonY);
            int bottom = (int)Math.Round(bottomY);

            if (top < 0) top = 0;
            if (bottom > bmp.Height) bottom = bmp.Height;
            if (bottom <= top) return;

            int trackHeight = bottom - top;

            // 0.0 = bottom, 1.0 = top
            int fadeStartY = bottom - (int)Math.Round(trackHeight * fadeStartPercent);
            int fadeEndY = bottom - (int)Math.Round(trackHeight * fadeEndPercent);

            if (fadeEndY > fadeStartY)
            {
                int temp = fadeStartY;
                fadeStartY = fadeEndY;
                fadeEndY = temp;
            }

            if (fadeEndY < top) fadeEndY = top;
            if (fadeStartY > bottom) fadeStartY = bottom;
            if (fadeStartY <= fadeEndY) return;

            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData data = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppPArgb);

            try
            {
                int stride = data.Stride;
                int width = bmp.Width;

                unsafe
                {
                    byte* scan0 = (byte*)data.Scan0;

                    // Fully transparent above the fade
                    for (int y = top; y < fadeEndY; y++)
                    {
                        byte* row = scan0 + (y * stride);

                        for (int x = 0; x < width; x++)
                        {
                            byte* px = row + (x * 4);

                            px[0] = 0; // B
                            px[1] = 0; // G
                            px[2] = 0; // R
                            px[3] = 0; // A
                        }
                    }

                    // Fade band: transparent at top -> solid at bottom
                    for (int y = fadeEndY; y < fadeStartY; y++)
                    {
                        float t = (y - fadeEndY) / (float)Math.Max(1, fadeStartY - fadeEndY);

                        // Stronger fade curve
                        t = t * t;
                        // For even stronger fade, use:
                        // t = t * t * t;

                        byte* row = scan0 + (y * stride);

                        for (int x = 0; x < width; x++)
                        {
                            byte* px = row + (x * 4);

                            px[0] = (byte)Math.Round(px[0] * t); // B
                            px[1] = (byte)Math.Round(px[1] * t); // G
                            px[2] = (byte)Math.Round(px[2] * t); // R
                            px[3] = (byte)Math.Round(px[3] * t); // A
                        }
                    }
                }
            }
            finally
            {
                bmp.UnlockBits(data);
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
        
        private void DrawBeatLines(
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

                double tBeat = 1.0 - ((bt - correctedTime) / playbackWindow);
                tBeat = ClampMin0(tBeat);
                double pBeat = EaseIn(tBeat); // can be > 1

                float y = (float)Lerp(horizonY, hitboxY + overshootPx, pBeat);

                // If we don't want beat lines past hitbox, clamp:
                if (y > hitboxY) y = hitboxY;
                if (y < horizonY) continue;

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

        private sealed class RBLaneLayout
        {
            public string Id;
            public MIDITrack Track;
            public int X;
            public int Width;
            public int Height;
            public string Label;
        }

        private double GetContinuousBeatPosition(double correctedTime)
        {
            if (_beatMarkers == null || _beatMarkers.Count < 2)
                return 0.0;

            int nextIndex = 0;

            while (nextIndex < _beatMarkers.Count && _beatMarkers[nextIndex].TimeSeconds <= correctedTime)
                nextIndex++;

            if (nextIndex <= 0)
                return 0.0;

            if (nextIndex >= _beatMarkers.Count)
                return _beatMarkers.Count - 1;

            double prevBeatTime = _beatMarkers[nextIndex - 1].TimeSeconds;
            double nextBeatTime = _beatMarkers[nextIndex].TimeSeconds;

            double beatDuration = nextBeatTime - prevBeatTime;

            if (beatDuration <= 0.0001)
                return nextIndex - 1;

            double phase = (correctedTime - prevBeatTime) / beatDuration;

            if (phase < 0.0) phase = 0.0;
            if (phase > 1.0) phase = 1.0;

            return (nextIndex - 1) + phase;
        }

        private void UpdateTrackAnimationBeatMarkerSynced(int frameCount, double correctedTime)
        {
            if (frameCount <= 0) return;

            double beatPosition = GetContinuousBeatPosition(correctedTime);

            double phase = (beatPosition / TrackBackgroundBeatsPerLoop) % 1.0;

            if (phase < 0.0)
                phase += 1.0;

            _trackAnimFrame = (int)(phase * frameCount) % frameCount;
        }

        private string BuildRockBandStyleCacheKey(
            Size renderSize,
            int tracks,
            int padding,
            int trackWidth,
            List<RBLaneLayout> lanes)
        {
            string lanePart = string.Join("|", lanes.Select(l => $"{l.Id}:{l.Label}:{l.Width}"));

            return string.Join(";",
                renderSize.Width,
                renderSize.Height,
                tracks,
                padding,
                trackWidth,
                doVerticalChart,
                doFocusMode == true,
                HighwayAngleFactor,
                lanePart
            );
        }

        private bool IsTrackSolo(MIDITrack track)
        {
            return track != null &&
                   track.Solos != null &&
                   track.Solos.Any(solo => solo.MarkerBegin <= PlaybackSeconds && solo.MarkerEnd > PlaybackSeconds);
        }

        private Bitmap GetRockBandBackgroundBitmap(string laneId, bool isSolo)
        {
            if (doVerticalChart)
            {
                switch (laneId)
                {
                    case "Bass":
                        return isSolo ? bmpBackgroundBassSolo : bmpBackgroundBass;
                    case "Drums":
                        return isSolo ? bmpBackgroundDrumsSolo : bmpBackgroundDrums;
                    case "Guitar":
                        return isSolo ? bmpBackgroundGuitarSolo : bmpBackgroundGuitar;
                    case "Keys":
                        return isSolo ? bmpBackgroundKeysSolo : bmpBackgroundKeys;
                    case "ProKeys":
                        return isSolo ? bmpBackgroundProKeysSolo : bmpBackgroundProKeys;
                }
            }
            else
            {
                switch (laneId)
                {
                    case "Bass":
                        return isSolo ? bmpBackgroundBassSoloRB : bmpBackgroundBassRB;
                    case "Drums":
                        return isSolo ? bmpBackgroundDrumsSoloRB : bmpBackgroundDrumsRB;
                    case "Guitar":
                        return isSolo ? bmpBackgroundGuitarSoloRB : bmpBackgroundGuitarRB;
                    case "Keys":
                        return isSolo ? bmpBackgroundKeysSoloRB : bmpBackgroundKeysRB;
                    case "ProKeys":
                        return isSolo ? bmpBackgroundProKeysSoloRB : bmpBackgroundProKeysRB;
                }
            }
            return null;
        }

        private Bitmap GetRockBandHitboxBitmap(string laneId)
        {
            if (doVerticalChart)
                return bmpHitbox;

            switch (laneId)
            {
                case "Drums":
                    return Resources.hitbox_drums;
                case "ProKeys":
                    return Resources.pianokeys;
                default:
                    return Resources.hitbox_5lane;
            }
        }

        private Bitmap BuildRockBandLaneBitmap(
            RBLaneLayout lane,
            bool isSolo,
            Size renderSize,
            bool chartVerticalValue,
            bool doFocusModeValue,
            double highwayAngleFactorValue)
        {
            var bmp = new Bitmap(Math.Max(1, lane.Width), renderSize.Height, PixelFormat.Format32bppPArgb);

            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent);

                float hitboxY = renderSize.Height - 50f;
                const float horizonPercent = 0.50f;
                float horizonY = ((hitboxY - 0f) * horizonPercent);

                if (chartVerticalValue)
                {
                    var bg = GetRockBandBackgroundBitmap(lane.Id, isSolo);
                    if (bg != null)
                    {
                        g.DrawImage(bg, 0, 0, lane.Width, renderSize.Height);
                    }
                }
                else
                {
                    if (!doFocusModeValue)
                    {
                        DrawInstrumentHitboxLabel(
                            g,
                            lane.Label,
                            0,
                            hitboxY,
                            lane.Width,
                            renderSize.Height - hitboxY,
                            renderSize
                        );
                    }

                    float trapezoidBottom = lane.Id == "ProKeys" ? hitboxY : hitboxY + 20f;

                    DrawHighwaySideBordersPerspectiveFaded(
                        g,
                        horizonY,
                        trapezoidBottom,
                        lane.Width / 2f,
                        lane.Width,
                        minScale: highwayAngleFactorValue,
                        maxScale: 1.00,
                        insetPx: lane.Id == "ProKeys" ? 5 : 4,
                        bitmapWidth: lane.Width,
                        bitmapHeight: renderSize.Height,
                        stepY: 10,
                        fadeStartPercent: 0.85f,
                        fadeEndPercent: 1.00f,
                        baseThickness: 4,
                        maxThickness: 8
                    );
                }

                var hitboxBmp = GetRockBandHitboxBitmap(lane.Id);

                DrawHitbox(
                    g,
                    hitboxBmp,
                    0,
                    renderSize.Height - 52,
                    lane.Width,
                    30,
                    0.90f,
                    chartVerticalValue ? lane.Label : ""
                );
            }

            return bmp;
        }

        private void DrawTrackPerspectiveTrapezoidSolo(
            Graphics g,
            Image trackBmp,
            int chartLeft,
            int topY,
            int trackHeight,
            int trackWidth,
            float horizonY,
            float hitboxY,
            int strips = 48
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

            const float topWidthFactor = HighwayAngleFactor; // tweak 0.45–0.70
            float topW = trackWidth * topWidthFactor;
            float bottomW = trackWidth;

            float topLeftX = centerX - (topW / 2f);
            float topRightX = centerX + (topW / 2f);
            float bottomLeftX = chartLeft;
            float bottomRightX = chartLeft + trackWidth;

            // Helper: linear interpolate between two floats
            float LerpF(float a, float b, float t) => a + (b - a) * t;

            // Clip to trapezoid so nothing spills out
            using (var path = new GraphicsPath())
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
                g.InterpolationMode = InterpolationMode.HighQualityBilinear;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

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

        private DateTime _rbCacheNextAllowedBuildTime = DateTime.MinValue;
        private int _rbCacheConsecutiveFailures = 0;
        private string _rbCacheLastFailedKey = "";

        private async Task EnsureRockBandStyleCacheAsync(
            List<RBLaneLayout> lanes,
            Size renderSize,
            int tracks,
            int padding,
            int trackWidth)
        {
            string newKey = BuildRockBandStyleCacheKey(renderSize, tracks, padding, trackWidth, lanes);

            lock (_rbCacheLock)
            {
                if (string.Equals(_rbLaneCacheKey, newKey, StringComparison.Ordinal))
                    return;

                if (_rbCacheBuildInProgress)
                {
                    _rbCacheRebuildRequested = true;
                    return;
                }
                                
                // If this exact cache failed recently, do not try again every frame.
                if (string.Equals(_rbCacheLastFailedKey, newKey, StringComparison.Ordinal) &&
                    DateTime.Now < _rbCacheNextAllowedBuildTime)
                {
                    return;
                }

                _rbCacheBuildInProgress = true;
            }

            Debug.WriteLine("REBUILDING ROCK BAND STYLE CACHE");
            Debug.WriteLine("RenderSize: " + renderSize.Width + "x" + renderSize.Height);
            Debug.WriteLine("Tracks: " + tracks + ", Padding: " + padding + ", TrackWidth: " + trackWidth);
            Debug.WriteLine("NewKey: " + newKey);

            RockBandStyleCacheBuildResult result = null;

            try
            {
                var lanesSnapshot = lanes.ToList();

                bool chartVerticalSnapshot = doVerticalChart;
                bool doFocusModeSnapshot = doFocusMode;
                double highwayAngleFactorSnapshot = HighwayAngleFactor;

                result = await Task.Run(() =>
                {
                    var normalTemp = new Dictionary<string, Bitmap>(StringComparer.Ordinal);
                    var soloTemp = new Dictionary<string, Bitmap>(StringComparer.Ordinal);
                    var animatedNormalTemp = new Dictionary<string, Bitmap[]>(StringComparer.Ordinal);
                    var animatedSoloTemp = new Dictionary<string, Bitmap[]>(StringComparer.Ordinal);
                    var soloOverlayTemp = new Dictionary<string, Bitmap>(StringComparer.Ordinal);
                    var focusOverlayTemp = new Dictionary<string, Bitmap>(StringComparer.Ordinal);

                    try
                    {
                        foreach (var lane in lanesSnapshot)
                        {
                            normalTemp[lane.Id] = BuildRockBandLaneBitmap(
                                lane,
                                false,
                                renderSize,
                                chartVerticalSnapshot,
                                doFocusModeSnapshot,
                                highwayAngleFactorSnapshot);

                            soloTemp[lane.Id] = BuildRockBandLaneBitmap(
                                lane,
                                true,
                                renderSize,
                                chartVerticalSnapshot,
                                doFocusModeSnapshot,
                                highwayAngleFactorSnapshot);

                            // TEMPORARY / SAFER:
                            // 48 fullscreen frames per lane is likely what is killing GDI+.
                            int animatedFrameCount = 16;

                            animatedNormalTemp[lane.Id] =
                                BuildAnimatedTrackFillFrames(lane, false, renderSize, animatedFrameCount);

                            animatedSoloTemp[lane.Id] =
                                BuildAnimatedTrackFillFrames(lane, true, renderSize, animatedFrameCount);

                            soloOverlayTemp[lane.Id] =
                                BuildFadedSoloOverlayBitmap(lane, renderSize);

                            focusOverlayTemp[lane.Id] =
                                BuildFadedFocusOverlayBitmap(lane, renderSize);
                        }

                        return new RockBandStyleCacheBuildResult
                        {
                            Key = newKey,
                            Normal = normalTemp,
                            Solo = soloTemp,
                            AnimatedNormal = animatedNormalTemp,
                            AnimatedSolo = animatedSoloTemp,
                            SoloOverlay = soloOverlayTemp,
                            FocusOverlay = focusOverlayTemp
                        };
                    }
                    catch
                    {
                        DisposeBitmapDictionary(normalTemp);
                        DisposeBitmapDictionary(soloTemp);
                        DisposeBitmapArrayDictionary(animatedNormalTemp);
                        DisposeBitmapArrayDictionary(animatedSoloTemp);
                        DisposeBitmapDictionary(soloOverlayTemp);
                        DisposeBitmapDictionary(focusOverlayTemp);

                        throw;
                    }
                }).ConfigureAwait(true);

                Dictionary<string, Bitmap> oldNormal;
                Dictionary<string, Bitmap> oldSolo;
                Dictionary<string, Bitmap[]> oldAnimatedNormal;
                Dictionary<string, Bitmap[]> oldAnimatedSolo;
                Dictionary<string, Bitmap> oldSoloOverlay;
                Dictionary<string, Bitmap> oldFocusOverlay;

                lock (_rbCacheLock)
                {
                    oldNormal = _rbLaneNormalCache;
                    oldSolo = _rbLaneSoloCache;
                    oldAnimatedNormal = _rbLaneAnimatedFillNormalCache;
                    oldAnimatedSolo = _rbLaneAnimatedFillSoloCache;
                    oldSoloOverlay = _rbLaneSoloOverlayCache;
                    oldFocusOverlay = _rbLaneFocusOverlayCache;

                    _rbLaneNormalCache = result.Normal;
                    _rbLaneSoloCache = result.Solo;
                    _rbLaneAnimatedFillNormalCache = result.AnimatedNormal;
                    _rbLaneAnimatedFillSoloCache = result.AnimatedSolo;
                    _rbLaneSoloOverlayCache = result.SoloOverlay;
                    _rbLaneFocusOverlayCache = result.FocusOverlay;

                    _rbLaneCacheKey = result.Key;

                    // Successful rebuild. Reset failure state.
                    _rbCacheConsecutiveFailures = 0;
                    _rbCacheLastFailedKey = "";
                    _rbCacheNextAllowedBuildTime = DateTime.MinValue;
                }

                QueueOldRockBandCacheForDisposal(
                    oldNormal,
                    oldSolo,
                    oldAnimatedNormal,
                    oldAnimatedSolo,
                    oldSoloOverlay,
                    oldFocusOverlay);

                Debug.WriteLine("ROCK BAND STYLE CACHE REBUILD COMPLETE");
            }
            catch (OutOfMemoryException ex)
            {
                HandleRockBandCacheBuildFailure(newKey, ex);
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                HandleRockBandCacheBuildFailure(newKey, ex);
            }
            catch (ArgumentException ex)
            {
                // Bitmap constructor often throws this when GDI+ allocation fails.
                HandleRockBandCacheBuildFailure(newKey, ex);
            }
            catch (Exception ex)
            {
                HandleRockBandCacheBuildFailure(newKey, ex);
            }
            finally
            {
                bool shouldRequestAnotherRebuild;

                lock (_rbCacheLock)
                {
                    _rbCacheBuildInProgress = false;

                    shouldRequestAnotherRebuild = _rbCacheRebuildRequested;
                    _rbCacheRebuildRequested = false;
                }

                if (shouldRequestAnotherRebuild)
                {
                    if (!IsDisposed && IsHandleCreated)
                    {
                        BeginInvoke(new Action(Invalidate));
                    }
                }
            }
        }

        private void HandleRockBandCacheBuildFailure(string failedKey, Exception ex)
        {
            Debug.WriteLine("ROCK BAND STYLE CACHE REBUILD FAILED");
            Debug.WriteLine(ex);

            lock (_rbCacheLock)
            {
                _rbCacheConsecutiveFailures++;
                _rbCacheLastFailedKey = failedKey;

                // Do NOT clear the current working cache.
                // Keep using whatever cache was already successfully built.
                //
                // Also do NOT mark the failed key as valid.
                // We simply delay the next retry.

                int delayMs;

                if (_rbCacheConsecutiveFailures <= 1)
                    delayMs = 1000;
                else if (_rbCacheConsecutiveFailures == 2)
                    delayMs = 2500;
                else if (_rbCacheConsecutiveFailures == 3)
                    delayMs = 5000;
                else
                    delayMs = 10000;

                _rbCacheNextAllowedBuildTime = DateTime.Now.AddMilliseconds(delayMs);

                Debug.WriteLine("Next Rock Band cache rebuild allowed after: " + _rbCacheNextAllowedBuildTime.ToString("HH:mm:ss.fff"));
            }

            // Encourage cleanup after failed partial builds.
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        private sealed class RockBandStyleCacheBuildResult
        {
            public string Key { get; set; }

            public Dictionary<string, Bitmap> Normal { get; set; }
            public Dictionary<string, Bitmap> Solo { get; set; }

            public Dictionary<string, Bitmap[]> AnimatedNormal { get; set; }
            public Dictionary<string, Bitmap[]> AnimatedSolo { get; set; }

            public Dictionary<string, Bitmap> SoloOverlay { get; set; }
            public Dictionary<string, Bitmap> FocusOverlay { get; set; }

            public RockBandStyleCacheBuildResult()
            {
                Key = "";

                Normal = new Dictionary<string, Bitmap>(StringComparer.Ordinal);
                Solo = new Dictionary<string, Bitmap>(StringComparer.Ordinal);

                AnimatedNormal = new Dictionary<string, Bitmap[]>(StringComparer.Ordinal);
                AnimatedSolo = new Dictionary<string, Bitmap[]>(StringComparer.Ordinal);

                SoloOverlay = new Dictionary<string, Bitmap>(StringComparer.Ordinal);
                FocusOverlay = new Dictionary<string, Bitmap>(StringComparer.Ordinal);
            }
        }

        private void QueueOldRockBandCacheForDisposal(
    Dictionary<string, Bitmap> oldNormal,
    Dictionary<string, Bitmap> oldSolo,
    Dictionary<string, Bitmap[]> oldAnimatedNormal,
    Dictionary<string, Bitmap[]> oldAnimatedSolo,
    Dictionary<string, Bitmap> oldSoloOverlay,
    Dictionary<string, Bitmap> oldFocusOverlay)
        {
            if (oldNormal == null &&
                oldSolo == null &&
                oldAnimatedNormal == null &&
                oldAnimatedSolo == null &&
                oldSoloOverlay == null &&
                oldFocusOverlay == null)
            {
                return;
            }

            Task.Run(async () =>
            {
                await Task.Delay(250);

                DisposeBitmapDictionary(oldNormal);
                DisposeBitmapDictionary(oldSolo);
                DisposeBitmapArrayDictionary(oldAnimatedNormal);
                DisposeBitmapArrayDictionary(oldAnimatedSolo);
                DisposeBitmapDictionary(oldSoloOverlay);
                DisposeBitmapDictionary(oldFocusOverlay);

                Debug.WriteLine("Old Rock Band style cache disposed.");

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            });
        }

        private async Task EnsureRockBandStyleCacheAsync1(
            List<RBLaneLayout> lanes,
            Size renderSize,
            int tracks,
            int padding,
            int trackWidth)
        {
            string newKey = BuildRockBandStyleCacheKey(renderSize, tracks, padding, trackWidth, lanes);

            if (string.Equals(_rbLaneCacheKey, newKey, StringComparison.Ordinal))
                return;

            Debug.WriteLine("REBUILDING CACHE");
            if (_rbCacheBuildInProgress)
            {
                _rbCacheRebuildRequested = true;
                return;
            }

            _rbCacheBuildInProgress = true;
            _rbLaneCacheKey = newKey;

            try
            {
                var lanesSnapshot = lanes.ToList();

                bool chartVerticalSnapshot = doVerticalChart;
                bool doFocusModeSnapshot = doFocusMode;
                double highwayAngleFactorSnapshot = HighwayAngleFactor;

                var result = await Task.Run(() =>
                {
                    var normalTemp = new Dictionary<string, Bitmap>(StringComparer.Ordinal);
                    var soloTemp = new Dictionary<string, Bitmap>(StringComparer.Ordinal);
                    var animatedNormalTemp = new Dictionary<string, Bitmap[]>(StringComparer.Ordinal);
                    var animatedSoloTemp = new Dictionary<string, Bitmap[]>(StringComparer.Ordinal);
                    var soloOverlayTemp = new Dictionary<string, Bitmap>(StringComparer.Ordinal);
                    var focusOverlayTemp = new Dictionary<string, Bitmap>(StringComparer.Ordinal);

                    foreach (var lane in lanesSnapshot)
                    {
                        normalTemp[lane.Id] = BuildRockBandLaneBitmap(
                            lane, false, renderSize,
                            chartVerticalSnapshot,
                            doFocusModeSnapshot,
                            highwayAngleFactorSnapshot);

                        soloTemp[lane.Id] = BuildRockBandLaneBitmap(
                            lane, true, renderSize,
                            chartVerticalSnapshot,
                            doFocusModeSnapshot,
                            highwayAngleFactorSnapshot);

                        animatedNormalTemp[lane.Id] =
                            BuildAnimatedTrackFillFrames(lane, false, renderSize, 48);

                        animatedSoloTemp[lane.Id] =
                            BuildAnimatedTrackFillFrames(lane, true, renderSize, 48);

                        soloOverlayTemp[lane.Id] =
                            BuildFadedSoloOverlayBitmap(lane, renderSize);

                        focusOverlayTemp[lane.Id] =
                            BuildFadedFocusOverlayBitmap(lane, renderSize);
                    }

                    return new
                    {
                        Normal = normalTemp,
                        Solo = soloTemp,
                        AnimatedNormal = animatedNormalTemp,
                        AnimatedSolo = animatedSoloTemp,
                        SoloOverlay = soloOverlayTemp,
                        FocusOverlay = focusOverlayTemp
                    };
                }).ConfigureAwait(true);

                var oldNormal = _rbLaneNormalCache;
                var oldSolo = _rbLaneSoloCache;
                var oldAnimatedNormal = _rbLaneAnimatedFillNormalCache;
                var oldAnimatedSolo = _rbLaneAnimatedFillSoloCache;
                var oldSoloOverlay = _rbLaneSoloOverlayCache;
                var oldFocusOverlay = _rbLaneFocusOverlayCache;

                lock (_rbCacheLock)
                {
                    _rbLaneNormalCache = result.Normal;
                    _rbLaneSoloCache = result.Solo;
                    _rbLaneAnimatedFillNormalCache = result.AnimatedNormal;
                    _rbLaneAnimatedFillSoloCache = result.AnimatedSolo;
                    _rbLaneSoloOverlayCache = result.SoloOverlay;
                    _rbLaneFocusOverlayCache = result.FocusOverlay;
                }

                /*DisposeBitmapDictionary(oldNormal);
                DisposeBitmapDictionary(oldSolo);
                DisposeBitmapDictionary(oldSoloOverlay);
                DisposeBitmapDictionary(oldFocusOverlay);
                DisposeBitmapArrayDictionary(oldAnimatedNormal);
                DisposeBitmapArrayDictionary(oldAnimatedSolo);*/
            }
            finally
            {
                _rbCacheBuildInProgress = false;
            }

            if (_rbCacheRebuildRequested)
            {
                _rbCacheRebuildRequested = false;
                _rbLaneCacheKey = "";
                Invalidate();
            }
        }

        private static void DisposeBitmapDictionary(Dictionary<string, Bitmap> dict)
        {
            foreach (var bmp in dict.Values)
                bmp?.Dispose();

            dict.Clear();
        }

        private static void DisposeBitmapArrayDictionary(Dictionary<string, Bitmap[]> dict)
        {
            foreach (var frames in dict.Values)
            {
                if (frames == null) continue;

                foreach (var bmp in frames)
                    bmp?.Dispose();
            }

            dict.Clear();
        }

        public async Task DrawRockBandStyleAsync(Graphics graphics, bool rebuildOnly = false)
        {
            var renderSize = activeRenderingResolution;

            int tracks = 0;

            bool hasDrums = MIDITools.MIDI_Chart.Drums.ChartedNotes.Any() && doMIDIDrums;
            bool hasBass = MIDITools.MIDI_Chart.Bass.ChartedNotes.Any() && doMIDIBass;
            bool hasGuitar = MIDITools.MIDI_Chart.Guitar.ChartedNotes.Any() && doMIDIGuitar;
            bool hasKeys = MIDITools.MIDI_Chart.Keys.ChartedNotes.Any() && doMIDIKeys;
            bool hasProKeys = MIDITools.MIDI_Chart.ProKeys.ChartedNotes.Any() && doMIDIProKeys && !hasKeys;

            if (hasDrums) tracks++;
            if (hasBass) tracks++;
            if (hasGuitar) tracks++;
            if (hasKeys) tracks++;
            else if (hasProKeys) tracks += 2;

            if (tracks == 0)
                return;

            const int maxTrackWidth = 400;
            const int maximizedPadding = 10;

            int padding = maximizedPadding;

            int track_width = (renderSize.Width - (padding * 2 * tracks)) / tracks;
            if (track_width > maxTrackWidth)
            {
                track_width = maxTrackWidth;
            }

            int totalTracksWidth = (track_width * tracks) + (padding * 2 * tracks);
            int startX = (renderSize.Width - totalTracksWidth) / 2;

            int track_height = renderSize.Height;
            int y = 0;
            int lastX = startX;

            float hitboxY = track_height - 50f;
            const float horizonPercent = 0.50f;
            float horizonY = y + ((hitboxY - y) * horizonPercent);

            if (!rebuildOnly)
            {
                if (secondScreen != null)
                {
                    SetSecondScreenBackColorIfChanged(Color.Black);
                    SetPicVisualsBackColorIfChanged(Color.AliceBlue);
                }
                else
                {
                    SetPicVisualsBackColorIfChanged(Color.Black);
                }
            }

            int drumsX = 0;
            int bassX = 0;
            int guitarX = 0;
            int keysX = 0;
            int proKeysX = 0;

            var lanes = new List<RBLaneLayout>();

            if (hasBass)
            {
                bassX = lastX + padding;
                lanes.Add(new RBLaneLayout
                {
                    Id = "Bass",
                    Track = MIDITools.MIDI_Chart.Bass,
                    X = bassX,
                    Width = track_width,
                    Height = track_height,
                    Label = "BASS"
                });
                lastX += track_width + (2 * padding);
            }

            if (hasDrums)
            {
                drumsX = lastX + padding;
                lanes.Add(new RBLaneLayout
                {
                    Id = "Drums",
                    Track = MIDITools.MIDI_Chart.Drums,
                    X = drumsX,
                    Width = track_width,
                    Height = track_height,
                    Label = "PRO DRUMS"
                });
                lastX += track_width + (2 * padding);
            }

            if (hasGuitar)
            {
                guitarX = lastX + padding;
                lanes.Add(new RBLaneLayout
                {
                    Id = "Guitar",
                    Track = MIDITools.MIDI_Chart.Guitar,
                    X = guitarX,
                    Width = track_width,
                    Height = track_height,
                    Label = "GUITAR"
                });
                lastX += track_width + (2 * padding);
            }

            if (hasKeys)
            {
                keysX = lastX + padding;
                lanes.Add(new RBLaneLayout
                {
                    Id = "Keys",
                    Track = MIDITools.MIDI_Chart.Keys,
                    X = keysX,
                    Width = track_width,
                    Height = track_height,
                    Label = "KEYS"
                });
            }
            else if (hasProKeys)
            {
                proKeysX = lastX + padding;
                lanes.Add(new RBLaneLayout
                {
                    Id = "ProKeys",
                    Track = MIDITools.MIDI_Chart.ProKeys,
                    X = proKeysX,
                    Width = track_width * 2,
                    Height = track_height,
                    Label = "PRO KEYS"
                });
            }

            if (rebuildOnly)
            {
                await EnsureRockBandStyleCacheAsync(lanes, renderSize, tracks, padding, track_width).ConfigureAwait(true);
                return;
            }
            _ = EnsureRockBandStyleCacheAsync(lanes, renderSize, tracks, padding, track_width);

            for (int i = 0; i < lanes.Count; i++)
            {
                var lane = lanes[i];
                bool isSolo = IsTrackSolo(lane.Track);

                var frames = isSolo
                    ? _rbLaneAnimatedFillSoloCache[lane.Id]
                    : _rbLaneAnimatedFillNormalCache[lane.Id];

                UpdateTrackAnimationBeatMarkerSynced(frames.Length, GetCorrectedTime());

                float trapezoidBottom = lane.Id == "ProKeys" ? hitboxY : hitboxY + 20f;
                if (doFocusMode)
                {
                    if (_rbLaneFocusOverlayCache.TryGetValue(lane.Id, out var focusBmp) && focusBmp != null)
                    {
                        graphics.DrawImageUnscaled(focusBmp, 0, 0);
                    }
                }
                else
                {
                    if (frames != null && frames.Length > 0)
                    {
                        int index = (_trackAnimFrame % frames.Length + frames.Length) % frames.Length;

                        if (ReverseTrackAnimation)
                            index = frames.Length - 1 - index;

                        var frame = frames[index];

                        if (frame != null)
                        {
                            graphics.DrawImageUnscaled(frame, lane.X, 0);
                        }
                    }
                }

                Bitmap laneBmp;
                if (isSolo)
                {
                    if (!_rbLaneSoloCache.TryGetValue(lane.Id, out laneBmp)) laneBmp = null;
                }
                else
                {
                    if (!_rbLaneNormalCache.TryGetValue(lane.Id, out laneBmp)) laneBmp = null;
                }              

                if (laneBmp != null)
                {
                    graphics.DrawImageUnscaled(laneBmp, lane.X, 0);
                }

                float trackCenterX = lane.X + (lane.Width / 2f);
                DrawWaitTimeRB(graphics, lane.Track.ChartedNotes, horizonY, hitboxY, trackCenterX);
            }

            int startingPosition = GetStartingPosition();

            if (hasDrums)
            {
                UpdateDrumBasedStageLighting(MIDITools.MIDI_Chart.Drums);
                if (doVerticalChart)
                {
                    DrawFills(graphics, MIDITools.MIDI_Chart.Drums, startingPosition, drumsX, track_width);
                    DrawDrumNotes(graphics, true, startingPosition, drumsX, track_width);
                    DrawDrumNotes(graphics, false, startingPosition, drumsX, track_width);
                }
                else
                {
                    DrawFillsRB(graphics, MIDITools.MIDI_Chart.Drums, startingPosition, drumsX, track_width);
                    DrawDrumNotesRB(graphics, true, startingPosition, drumsX, track_width);
                    DrawDrumNotesRB(graphics, false, startingPosition, drumsX, track_width);
                }
            }

            if (hasBass)
            {
                if (doVerticalChart)
                {
                    DrawFills(graphics, MIDITools.MIDI_Chart.Bass, startingPosition, bassX, track_width);
                    DrawFiveLaneNotes(graphics, MIDITools.MIDI_Chart.Bass, startingPosition, bassX, track_width);
                }
                else
                {
                    DrawFillsRB(graphics, MIDITools.MIDI_Chart.Bass, startingPosition, bassX, track_width);
                    DrawFiveLaneNotesRB(graphics, MIDITools.MIDI_Chart.Bass, startingPosition, bassX, track_width);
                }
            }

            if (hasGuitar)
            {
                if (doVerticalChart)
                {
                    DrawFills(graphics, MIDITools.MIDI_Chart.Guitar, startingPosition, guitarX, track_width);
                    DrawFiveLaneNotes(graphics, MIDITools.MIDI_Chart.Guitar, startingPosition, guitarX, track_width);
                }
                else
                {
                    DrawFillsRB(graphics, MIDITools.MIDI_Chart.Guitar, startingPosition, guitarX, track_width);
                    DrawFiveLaneNotesRB(graphics, MIDITools.MIDI_Chart.Guitar, startingPosition, guitarX, track_width);
                }
            }

            if (hasKeys)
            {
                if (doVerticalChart)
                {
                    DrawFills(graphics, MIDITools.MIDI_Chart.Keys, startingPosition, keysX, track_width);
                    DrawFiveLaneNotes(graphics, MIDITools.MIDI_Chart.Keys, startingPosition, keysX, track_width);
                }
                else
                {
                    DrawFillsRB(graphics, MIDITools.MIDI_Chart.Keys, startingPosition, keysX, track_width);
                    DrawFiveLaneNotesRB(graphics, MIDITools.MIDI_Chart.Keys, startingPosition, keysX, track_width);
                }
            }

            if (hasProKeys)
            {
                if (doVerticalChart)
                {
                    DrawFills(graphics, MIDITools.MIDI_Chart.ProKeys, startingPosition, proKeysX, track_width * 2);
                    DrawProKeysNotes(graphics, startingPosition, proKeysX, track_width * 2);
                }
                else
                {
                    DrawFillsRB(graphics, MIDITools.MIDI_Chart.ProKeys, startingPosition, proKeysX, track_width * 2, true);
                    DrawProKeysNotesRB(graphics, startingPosition, proKeysX, track_width * 2);
                }
            }

            bool solo = doMIDIVocals &&
                        MIDITools.MIDI_Chart.Vocals.Solos != null &&
                        MIDITools.MIDI_Chart.Vocals.Solos.Any(s => s.MarkerBegin <= PlaybackSeconds && s.MarkerEnd > PlaybackSeconds);

            if (!solo && doMIDIHarmonies)
            {
                solo = doMIDIVocals &&
                       MIDITools.MIDI_Chart.Harm1.Solos != null &&
                       MIDITools.MIDI_Chart.Harm1.Solos.Any(s => s.MarkerBegin <= PlaybackSeconds && s.MarkerEnd > PlaybackSeconds);
            }
        }

        private void ResetStageKitDrumTriggers()
        {
            _stageKitTriggeredKickTicks.Clear();
            _lastKickStrobeTime = -999;
        }

        private void DrawTrackPerspectiveTrapezoidSoloFaded(
            Graphics g,
            Image soloBmp,
            int chartLeft,
            int topY,
            int trackHeight,
            int trackWidth,
            float horizonY,
            float hitboxY,
            float fadeStartPercent = 0.85f,
            float fadeEndPercent = 1.00f)
        {
            if (soloBmp == null || trackHeight <= 0 || trackWidth <= 0) return;

            int bmpWidth = chartLeft + trackWidth;
            int bmpHeight = topY + trackHeight;

            using (var temp = new Bitmap(
                Math.Max(1, bmpWidth),
                Math.Max(1, bmpHeight),
                PixelFormat.Format32bppPArgb))
            {
                using (var tg = Graphics.FromImage(temp))
                {
                    tg.Clear(Color.Transparent);

                    DrawTrackPerspectiveTrapezoidSolo(
                        tg,
                        soloBmp,
                        chartLeft,
                        topY,
                        trackHeight,
                        trackWidth,
                        horizonY,
                        hitboxY
                    );
                }

                ApplyTopFadeToTrackFrame(temp, horizonY, hitboxY, fadeStartPercent, fadeEndPercent);

                g.DrawImageUnscaled(temp, 0, 0);
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

            // If a sustain is currently active, don't start counting from "now".
            // Start counting after the current sustained note ends.
            var activeNote = notes
                .Where(n => n.NoteStart <= time && n.NoteEnd > time)
                .OrderByDescending(n => n.NoteEnd)
                .FirstOrDefault();

            double occupiedUntil = activeNote != null ? activeNote.NoteEnd : time;

            var nextNote = notes
                .Where(n => n.NoteStart > occupiedUntil)
                .OrderBy(n => n.NoteStart)
                .FirstOrDefault();

            if (nextNote == null)
                return;

            var wait = nextNote.NoteStart - occupiedUntil;

            if (wait > 5.0)
            {
                string text = wait.ToString("0");
                DrawWaitTimeTextRB(graphics, notes, horizonY, hitboxY, trackCenterX, text);
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

            // Clamp the panel to the render area
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

            g.SmoothingMode = SmoothingMode.None;
            g.PixelOffsetMode = PixelOffsetMode.HighSpeed;
            g.CompositingMode = CompositingMode.SourceOver;
            g.CompositingQuality = CompositingQuality.HighSpeed;
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
            float topOpenPx = 1.5f,
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

                double p = PFromY(y);
                float t = (float)Lerp(baseThickness, maxThickness, p);

                // Extra widening at the top only, fading out toward the bottom
                float open = (float)((1.0 - p) * topOpenPx);

                float leftX = (float)spanLeft + insetPx - open;
                float rightX = (float)(spanLeft + span) - insetPx + open;

                // Border rails: inner edge sits on the highway edge,
                // outer edge expands outward.
                leftInner.Add(new PointF(leftX, y));
                leftOuter.Add(new PointF(leftX - t, y));

                rightInner.Add(new PointF(rightX, y));
                rightOuter.Add(new PointF(rightX + t, y));

                // Highlight rails (a thinner band just inside the border)
                float hiT = Math.Max(1f, t * HighwayAngleFactor);

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

                double p = PFromY(y);
                float t = (float)Lerp(baseThickness, maxThickness, p);
                float hiT = Math.Max(1f, t * HighwayAngleFactor);

                float open = (float)((1.0 - p) * topOpenPx);

                float leftX = (float)spanLeft + insetPx - open;
                float rightX = (float)(spanLeft + span) - insetPx + open;

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

        private void DrawHighwaySideBordersPerspectiveFaded(
            Graphics g,
            float horizonY,
            float hitboxY,
            float trackCenterX,
            float trackWidth,
            double minScale,
            double maxScale,
            int insetPx,
            int bitmapWidth,
            int bitmapHeight,
            int stepY = 6,
            float baseThickness = 2.0f,
            float maxThickness = 6.0f,
            float topOpenPx = 1.5f,
            float fadeStartPercent = 0.80f,
            float fadeEndPercent = 1.00f,
            Color? borderColor = null,
            Color? highlightColor = null
        )
        {
            using (var railBmp = new Bitmap(
                Math.Max(1, bitmapWidth),
                Math.Max(1, bitmapHeight),
                PixelFormat.Format32bppPArgb))
            {
                using (var rg = Graphics.FromImage(railBmp))
                {
                    rg.Clear(Color.Transparent);

                    DrawHighwaySideBordersPerspective(
                        rg,
                        horizonY,
                        hitboxY,
                        trackCenterX,
                        trackWidth,
                        minScale,
                        maxScale,
                        insetPx,
                        stepY,
                        baseThickness,
                        maxThickness,
                        topOpenPx,
                        borderColor,
                        highlightColor
                    );
                }

                ApplyTopFadeToTrackFrame(
                    railBmp,
                    horizonY,
                    hitboxY,
                    fadeStartPercent,
                    fadeEndPercent
                );

                g.DrawImageUnscaled(railBmp, 0, 0);
            }
        }

        private int GetStartingPosition()
        {
            var startingPosition = GetHeightDiff();
            if (doKaraokeLyrics || doStaticLyrics || doScrollingLyrics || doRockBandChart)
            {
                if (MIDITools.MIDI_Chart.Vocals.ChartedNotes.Any() || doRockBandChart)
                {
                    startingPosition += 20;
                    if (doHarmonyLyrics || doRockBandChart)
                    {
                        if (MIDITools.MIDI_Chart.Harm2.ChartedNotes.Any() || doRockBandChart)
                        {
                            startingPosition += 20;
                        }
                        if (MIDITools.MIDI_Chart.Harm3.ChartedNotes.Any() || doRockBandChart)
                        {
                            startingPosition += 20;
                        }
                    }
                }
            }
            if (doMIDINoVocals && !doRockBandChart)
            {
                startingPosition = 0;
            }
            return startingPosition;
        }

        private void DrawPhraseMarkers(Graphics graphics, PhraseCollection phrases, int track_height, int track_y)
        {
            var renderSize = activeRenderingResolution;
            var time = GetCorrectedTime();

            if (phrases == null || phrases.Phrases.Count == 0)
                return;

            double playbackWindow = GetVocalScrollWindow();

            var hitboxWidth = HitboxVocalsX + (bmpHitboxVocals.Width / 2);
            if (doMIDIChart)
                hitboxWidth = 0;

            for (var p = 0; p < phrases.Phrases.Count; p++)
            {
                var phrase = phrases.Phrases[p];

                if (phrase.PhraseStart > time + playbackWindow)
                    break;

                if (phrase.PhraseEnd < time)
                    continue;

                float normalizedTime = (float)((phrase.PhraseStart - time) / playbackWindow);

                float x = (normalizedTime * (renderSize.Width - hitboxWidth)) + hitboxWidth;

                if (x < 0) x = 0;
                if (x > renderSize.Width) x = renderSize.Width;

                if ((doVerticalChart || doRockBandChart || doRockBandKaraoke) && x < HitboxVocalsX)
                    continue;

                if (doMIDIChart && x < 0)
                    continue;

                int top = doRockBandKaraoke
                    ? track_y + 4
                    : (doVerticalChart || doRockBandChart ? GetYForRBVocals() + 4 : track_y - track_height + 4);

                int height = doRockBandKaraoke
                    ? track_height - 8
                    : (doVerticalChart || doRockBandChart ? vocalsHeight - 8 : track_height - 8);

                const int width = 4;

                using (var solidBrush = new SolidBrush(Color.DarkGray))
                {
                    graphics.FillRectangle(solidBrush, x, top, width, height);
                }
            }
        }

        private void DrawTrackBackground(Graphics graphics, int y, int height, int index, string name, ICollection<SpecialMarker> solos, Instrument instrument)
        {
            var renderSize = activeRenderingResolution;
            if (!doMIDIChart && !doVerticalChart) return;
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
                using (var DrawingPen = new SolidBrush(doVerticalChart ? RBStyleVocalsBackgroundColor : color))
                {
                    var adjustedPosY = doVerticalChart ? y : y - height;
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

            Rectangle textRectangle = new Rectangle(x - 4, y2, textSize.Width + 4, textSize.Height + 4);

            if (!doMIDINameTracks || doVerticalChart) return;
            TextRenderer.DrawText(graphics, trackText, font, rectangle, index % 2 == 0 ? TrackBackgroundColor1 : TrackBackgroundColor2, TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);
        }
        
        private static IList<Lyric> AsLyricList(IEnumerable<Lyric> source)
        {
            return source as IList<Lyric> ?? (source != null ? source.ToList() : null);
        }

        private static string FindLyricTextAtStart(IList<Lyric> lyrics, double start)
        {
            if (lyrics == null) return "";

            for (int i = 0; i < lyrics.Count; i++)
            {
                if (lyrics[i].Start == start)
                    return lyrics[i].Text ?? "";
            }

            return "";
        }

        private int FindRollingVisibleStartIndex(MIDITrack track, double correctedTime, double window, bool isVocalsTrack, bool specialChartMode)
        {
            var notes = track.ChartedNotes;
            int count = notes.Count;
            if (count == 0) return 0;

            int index = track.ActiveIndex;
            if (index < 0 || index >= count)
                index = 0;

            // If we jumped backwards far enough that the hint is obviously wrong, reset.
            if (index > 0 && notes[index].NoteStart > correctedTime + (window * 2.0))
                index = 0;

            // Rewind a little so we don't miss sustains / long vocal notes that began before the current time.
            double rewindWindow = isVocalsTrack
                ? Math.Max(window * 2.0, 8.0)
                : Math.Max(window, 2.0);

            while (index > 0 && notes[index].NoteStart > correctedTime - rewindWindow)
            {
                index--;
            }

            // Advance past notes that are definitely dead.
            double pastCutoff = specialChartMode ? correctedTime - 1.0 : correctedTime;

            while (index < count)
            {
                var note = notes[index];

                if (note.NoteStart > correctedTime + (window * 2.0))
                    break;

                if (note.NoteEnd >= pastCutoff)
                    break;

                index++;
            }

            return index;
        }

        private double GetVocalScrollWindow()
        {
            double bpm = PlayingSong != null && PlayingSong.BPM > 0 ? PlayingSong.BPM : 120.0;
            double secondsPerBeat = 60.0 / bpm;
            double visibleBeats = 6.8; //CHANGE THIS FOR SCROLLING SPEED - LOWER FASTER - HIGHER SLOWER
            return Math.Max(0.90, Math.Min(6.0, secondsPerBeat * visibleBeats));
        }

        private void DrawNotes(Graphics graphics, MIDITrack track, int track_height, int track_y, bool drums, int harm, out int LastPlayedIndex)
        {
            LastPlayedIndex = track.ActiveIndex;

            double correctedTime = GetCorrectedTime();

            track_y++;
            track_height--;

            float needleY = 0f;
            float needleAdjustedHeight = 0f;
            bool needleUnpitched = false;
            bool drawNeedle = false;

            var renderSize = activeRenderingResolution;//new Size(1920, 1080);

            var oldSmoothingMode = graphics.SmoothingMode;
            var oldCompositingQuality = graphics.CompositingQuality;

            graphics.SmoothingMode = SmoothingMode.None;
            graphics.CompositingQuality = CompositingQuality.HighSpeed;

            string trackName = track.Name ?? "";

            bool rbKaraoke = doRockBandKaraoke;
            bool isVocalsTrack =
                trackName == "Vocals" ||
                trackName == "Harm1" ||
                trackName == "Harm2" ||
                trackName == "Harm3";

            bool specialChartMode = doMIDIChart || doVerticalChart || doRockBandChart || rbKaraoke;
            bool normalChartMode = !doMIDIChart && !doVerticalChart && !doRockBandKaraoke && !doRockBandChart;

            double window;
            switch (trackName)
            {
                case "Vocals":
                case "Harm1":
                case "Harm2":
                case "Harm3":
                    if (doMIDINoVocals) return;
                    window = GetVocalScrollWindow();
                    //window = PlaybackWindowRBVocals * (rbKaraoke && PlayingSong.BPM > 80.0 ? 80.0 / PlayingSong.BPM : 1.0);
                    break;

                default:
                    if (rbKaraoke)
                    {
                        window = PlaybackWindowRBVocals * (PlayingSong.BPM > 80.0 ? 80.0 / PlayingSong.BPM : 1.0);
                    }
                    else
                    {
                        window = (doVerticalChart || doRockBandChart) ? PlaybackWindowRB : PlaybackWindow;
                    }
                    break;
            }

            var notes = track.ChartedNotes;
            int noteCount = notes.Count;
            if (noteCount == 0)
            {
                graphics.SmoothingMode = oldSmoothingMode;
                graphics.CompositingQuality = oldCompositingQuality;
                return;
            }

            double visibleEnd = correctedTime + (window * 2.0);
            int startIndex = FindRollingVisibleStartIndex(track, correctedTime, window, isVocalsTrack, specialChartMode);

            IList<Lyric> lyricList = null;
            if (isVocalsTrack)
            {
                switch (trackName)
                {
                    case "Vocals":
                        lyricList = AsLyricList(MIDITools.LyricsVocals.Lyrics);
                        break;
                    case "Harm1":
                        lyricList = AsLyricList(MIDITools.LyricsHarm1.Lyrics);
                        break;
                    case "Harm2":
                        lyricList = AsLyricList(MIDITools.LyricsHarm2.Lyrics);
                        break;
                    case "Harm3":
                        lyricList = AsLyricList(MIDITools.LyricsHarm3.Lyrics);
                        break;
                }
            }

            for (int z = startIndex; z < noteCount; z++)
            {
                var note = notes[z];

                if (note.NoteStart > visibleEnd)
                    break;

                if (normalChartMode)
                {
                    if (note.NoteEnd < correctedTime)
                        continue;

                    if (note.NoteStart > correctedTime)
                        break;
                }

                if (specialChartMode)
                {
                    if (note.NoteEnd < correctedTime - 1.0)
                        continue;
                }

                LastPlayedIndex = z;

                if (doMIDIBWKeys && trackName == "ProKeys")
                {
                    note.NoteColor = note.NoteName.Contains("#") ? Color.Black : Color.WhiteSmoke;
                }
                else if (note.NoteColor == Color.Empty)
                {
                    switch (harm)
                    {
                        case 0:
                            note.NoteColor = rbKaraoke
                                ? KaraokeModeHarm1Highlight
                                : (!doMIDIHarm1onVocals ? GetNoteColor(note.NoteNumber) : Harm1Color);
                            break;
                        case 1:
                            note.NoteColor = rbKaraoke ? KaraokeModeHarm1Highlight : Harm1Color;
                            break;
                        case 2:
                            note.NoteColor = rbKaraoke ? KaraokeModeHarm2Highlight : Harm2Color;
                            break;
                        case 3:
                            note.NoteColor = rbKaraoke ? KaraokeModeHarm3Highlight : Harm3Color;
                            break;
                        default:
                            note.NoteColor = GetNoteColor(note.NoteNumber, drums);
                            break;
                    }
                }

                double note_width = (note.NoteLength / (PlayingSong.Length / 1000.0)) * renderSize.Width;
                if (note_width < 1.0)
                    note_width = 1.0;

                float x = (float)((note.NoteStart - correctedTime) / window * renderSize.Width / 1.33f);

                if (isVocalsTrack)
                {
                    int hitboxWidth = HitboxVocalsX + (bmpHitboxVocals.Width / 2);
                    if (doMIDIChart)
                    {
                        hitboxWidth = 0;
                    }

                    x = (float)((note.NoteStart - correctedTime) / window * (renderSize.Width - hitboxWidth) + hitboxWidth);

                    int vocalChartTop = rbKaraoke
                        ? track_y
                        : ((doVerticalChart || doRockBandChart) ? GetYForRBVocals() : track_y - track_height);

                    int vocalChartHeight = rbKaraoke
                        ? vocalsHeight * 2
                        : ((doVerticalChart || doRockBandChart) ? vocalsHeight : track_height);

                    const int minNote = 36;
                    const int maxNote = 84;
                    int noteRange = maxNote - minNote + 1;
                    double noteHeight = (double)vocalChartHeight / noteRange;

                    int y = vocalChartTop + (int)((maxNote - note.NoteNumber) * noteHeight);
                    double width = 0;

                    string lyricText = FindLyricTextAtStart(lyricList, note.NoteStart);
                    bool isUnpitched = !string.IsNullOrEmpty(lyricText) && (lyricText.Trim().EndsWith("#") || lyricText.Trim().EndsWith("^"));

                    if (specialChartMode)
                    {
                        width = ((note.NoteLength / window) * renderSize.Width) * 0.8;
                        if (width < 1)
                            width = 1;

                        float adjustedHeight = (float)noteHeight * 2;
                        int adjustedY = y;
                        int alpha = 255;

                        if (isUnpitched)
                        {
                            adjustedHeight = rbKaraoke
                                ? vocalsHeight * 2
                                : ((doVerticalChart || doRockBandChart) ? vocalsHeight : track_height);

                            adjustedY = rbKaraoke
                                ? track_y
                                : ((doVerticalChart || doRockBandChart) ? GetYForRBVocals() : track_y - track_height);

                            alpha = 192;
                        }

                        if ((note.NoteNumber == 96 || note.NoteNumber == 97) && MIDITools.MIDI_Chart.UsesPercussion)
                        {
                            const float percHeight = 20f;
                            float percY = vocalChartTop + (vocalChartHeight - percHeight) / 2;
                            float x0 = x;
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

                                graphics.SmoothingMode = SmoothingMode.None;
                                graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                                graphics.CompositingQuality = CompositingQuality.HighSpeed;

                                using (var penOuter = new Pen(Color.FromArgb(160, Color.Black), 3.0f))
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
                                graphics.FillRectangle(solidBrush, x, adjustedY, (float)width, adjustedHeight);
                            }

                            if (specialChartMode && note.NoteStart < correctedTime)
                            {
                                using (var glowBrush = new SolidBrush(note.NoteColor))
                                {
                                    graphics.FillRectangle(glowBrush, x, adjustedY, (float)width, adjustedHeight);
                                }
                            }

                            if (!isUnpitched && specialChartMode && note.NoteStart < correctedTime)
                            {
                                drawNeedle = true;
                                needleY = adjustedY;
                                needleAdjustedHeight = adjustedHeight;
                                needleUnpitched = false;
                                if (doRockBandKaraoke)
                                {
                                    needleY += 6; //adjust for this mode only
                                }
                            }
                        }
                    }

                    if (isUnpitched)
                        continue;

                    if (z + 1 < noteCount)
                    {
                        var nextNote = notes[z + 1];
                        string nextLyricText = FindLyricTextAtStart(lyricList, nextNote.NoteStart);

                        if (!string.IsNullOrEmpty(nextLyricText) && nextLyricText.Replace("-", "").Replace("$", "").Trim() == "+")
                        {
                            float x3 = (float)((nextNote.NoteStart - correctedTime) / window * (renderSize.Width - hitboxWidth) + hitboxWidth);
                            int y3 = vocalChartTop + (int)((maxNote - nextNote.NoteNumber) * noteHeight);

                            PointF pointF1 = new PointF((float)(x + width), (float)(y + (noteHeight * 2)));
                            PointF pointF2 = new PointF((float)(x + width), y);
                            PointF pointF3 = new PointF(x3, y3);
                            PointF pointF4 = new PointF(x3, (float)(y3 + (noteHeight * 2)));

                            PointF[] basePolygon = new[] { pointF1, pointF2, pointF3, pointF4 };

                            Color polygonColor = ((trackName == "Vocals" && doMIDIHarm1onVocals) || trackName != "Vocals")
                                ? note.NoteColor
                                : Color.LightGray;

                            using (var solidBrush = new SolidBrush(polygonColor))
                            {
                                graphics.FillPolygon(solidBrush, basePolygon);
                            }

                            if (specialChartMode && note.NoteStart < correctedTime)
                            {
                                using (var solidBrush = new SolidBrush(note.NoteColor))
                                {
                                    graphics.FillPolygon(solidBrush, basePolygon);
                                }
                            }
                        }
                    }

                    if (doMIDINameVocals)
                    {
                        TextRenderer.DrawText(
                            graphics,
                            note.NoteName,
                            _impact12Font,
                            new Point((int)x + 1, y - 1),
                            Color.White,
                            TextFormatFlags.NoPadding);
                    }

                    continue;
                }

                int y_general = track_y;
                List<int> range;

                switch (NoteSizingType)
                {
                    default:
                        range = (trackName == "ProKeys") ? track.NoteRange : track.ValidNotes;
                        break;
                    case 1:
                        range = track.NoteRange;
                        break;
                    case 2:
                        range = track.ValidNotes;
                        break;
                }

                double note_height_general = (double)track_height / range.Count;

                int rangeCount = range.Count;
                for (int i = 0; i < rangeCount; i++)
                {
                    if (note.NoteNumber != range[i])
                        continue;

                    y_general = (int)(track_y - (note_height_general * (rangeCount - i)));
                    break;
                }

                if (doMIDIChart)
                {
                    double width = ((note.NoteLength / window) * renderSize.Width) * 0.8;
                    if (width < 1)
                        width = 1;

                    using (var solidBrush = new SolidBrush(note.NoteColor))
                    {
                        graphics.FillRectangle(solidBrush, x, y_general, (float)width, (float)note_height_general);
                    }

                    if (trackName == "ProKeys" && doMIDINameProKeys)
                    {
                        TextRenderer.DrawText(
                            graphics,
                            note.NoteName,
                            _impact12Font,
                            new Point((int)x + 1, y_general - 1),
                            note.NoteName.Contains("#") ? Color.White : Color.Black,
                            TextFormatFlags.NoPadding);
                    }
                }
            }

            if (drawNeedle && (doRockBandChart || doVerticalChart || doRockBandKaraoke))
            {
                var needle = needleHarm1;
                if (trackName == "Harm2")
                {
                    needle = needleHarm2;
                }
                else if (trackName == "Harm3")
                {
                    needle = needleHarm3;
                }

                float needleHeight = needle.Height * 0.25f;
                float needleWidth = needle.Width * 0.25f;
                float needleX = HitboxVocalsX + (bmpHitboxVocals.Width / 2) - needleWidth;

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
                _mediaPlayer.Time = GetBASSTimeForVideo();
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
            //var track_vol = (float)Utils.DBToLevel(Convert.ToDouble(-1 * (MinVolume - VolumeLevel)), 1.0);
            Bass.BASS_ChannelSetAttribute(BassMixer, BASSAttribute.BASS_ATTRIB_VOL, masterVol);
        }

        private void picPreview_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || PlayingSong == null) return;
            if (!displayAlbumArt && File.Exists(CurrentSongArt))
            {
                var display = new Art(Cursor.Position, CurrentSongArt);
                display.Show();
                return;
            }
            if (!displayAlbumArt && (File.Exists(CurrentSongArt) || displayAudioSpectrum)) return;
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
            Text = string.IsNullOrEmpty(PlaylistName) ? AppName : AppName + " - " + PlaylistName;
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
                    catch
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
            Text = string.IsNullOrEmpty(PlaylistName) ? AppName : AppName + " - " + PlaylistName;
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
                if (song.Year >= 2020 && !enable2020s)
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
            try
            {
                using (var sw = new StreamWriter(config, false))
                {
                    void WriteSetting(string key, object value)
                    {
                        string text;

                        if (value == null)
                        {
                            text = string.Empty;
                        }
                        else if (value is double d)
                        {
                            text = d.ToString(CultureInfo.InvariantCulture);
                        }
                        else if (value is float f)
                        {
                            text = f.ToString(CultureInfo.InvariantCulture);
                        }
                        else if (value is decimal m)
                        {
                            text = m.ToString(CultureInfo.InvariantCulture);
                        }
                        else if (value is IFormattable formattable)
                        {
                            text = formattable.ToString(null, CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            text = value.ToString();
                        }

                        sw.WriteLine(key + "=" + text);
                    }

                    void WriteColorSetting(string key, Color value)
                    {
                        sw.WriteLine(key + "=" + ColorTranslator.ToHtml(value));
                    }

                    WriteSetting("ConfigVersion", "2");

                    WriteSetting("PlayerConsole", PlayerConsole);

                    WriteSetting("LoopPlayback", picLoop.Tag != null && picLoop.Tag.ToString() == "loop");
                    WriteSetting("ShufflePlayback", picShuffle.Tag != null && picShuffle.Tag.ToString() == "shuffle");
                    WriteSetting("AutoloadPlaylist", autoloadLastPlaylist.Checked);
                    WriteSetting("LastPlaylist", PlaylistPath);
                    WriteSetting("AutoPlay", autoPlay.Checked);
                    WriteSetting("PlayCrowdTrack", doAudioCrowd);
                    WriteSetting("VolumeLevel", masterVol);

                    for (int i = 0; i < 5; i++)
                    {
                        WriteSetting("RecentPlaylist" + (i + 1), RecentPlaylists[i]);
                    }

                    WriteSetting("ShowLyrics", doStaticLyrics);
                    WriteSetting("WholeWords", doWholeWordsLyrics);
                    WriteSetting("GameSyllables", !doWholeWordsLyrics);
                    WriteSetting("DisplayHarmonies", doHarmonyLyrics);
                    WriteSetting("DontDisplayLyrics", !doStaticLyrics && !doKaraokeLyrics && !doScrollingLyrics);
                    WriteSetting("KaraokeLyrics", doKaraokeLyrics);
                    WriteSetting("ScrollingLyrics", doScrollingLyrics);

                    WriteSetting("ShowPracticeSessions", showPracticeSections.Checked);
                    WriteSetting("DrawSnippet", doMIDIChart);
                    WriteSetting("LabelTracks", doMIDINameTracks);
                    WriteSetting("PlaybackWindow", PlaybackWindow);
                    WriteSetting("NoteSizingType", NoteSizingType);
                    WriteSetting("NameProKeysNotes", doMIDINameProKeys);
                    WriteSetting("NameVocalNotes", doMIDINameVocals);
                    WriteSetting("HighlightSolos", doMIDIHighlightSolos);
                    WriteSetting("BWProKeys", doMIDIBWKeys);
                    WriteSetting("UseHarm1ColorOnVocals", doMIDIHarm1onVocals);
                    WriteSetting("DoNoKeys", doMIDINoKeys);
                    WriteSetting("DoNoVocals", doMIDINoVocals);
                    WriteSetting("DoMIDIVocals", doMIDIVocals);
                    WriteSetting("DoMIDIHarmonies", doMIDIHarmonies);

                    WriteSetting("OpenSideWindow", openSideWindow.Checked);

                    WriteSetting("DrawSpectrum", displayAudioSpectrum);
                    WriteSetting("SpectrumID", SpectrumID);
                    WriteSetting("DisplayAlbumArt", displayAlbumArt);
                    WriteSetting("DoAnimSpectrum", doAnimatedSpectrum);
                    WriteSetting("DoSpectrumColors", doSpectrumColors);

                    WriteSetting("DisplayBackgroundVideo", enableYARGCHVideos);
                    WriteSetting("UseBackgroundVideos", doUseBackgroundVideos);
                    WriteSetting("UseBackgroundImages", doUseBackgroundImages);
                    WriteSetting("UseAnimatedBackground", doAnimatedBackground);
                    WriteSetting("UseStaticBackground", doStaticBackground);
                    WriteSetting("UseSolidColorBackground", doSolidColorBackground);
                    WriteSetting("UseStaticBackground2", doStaticBackground2);
                    WriteSetting("UseAnimatedBackground2", doAnimatedBackground2);

                    WriteSetting("SkipIntroOutroSilence", skipIntroOutroSilence.Checked);
                    WriteSetting("SilenceThreshold", SilenceThreshold);
                    WriteSetting("FadeInLength", FadeLength);

                    WriteColorSetting("KaraokeModeBackground", KaraokeModeBackgroundColor);
                    WriteColorSetting("KaraokeModeLyric", KaraokeModeHarm1Text);
                    WriteColorSetting("KaraokeModeHighlight", KaraokeModeHarm1Highlight);
                    WriteColorSetting("KaraokeModeHarmony", KaraokeModeHarm2Text);
                    WriteColorSetting("KaraokeModeHarmonyHighlight", KaraokeModeHarm2Highlight);
                    WriteColorSetting("KaraokeModeHarmony2", KaraokeModeHarm3Text);
                    WriteColorSetting("KaraokeModeHarmony2Highlight", KaraokeModeHarm3Highlight);

                    WriteSetting("DoRockBandKaraokeMode", doRockBandKaraoke);
                    WriteSetting("DoClassicKaraokeMode", doModernKaraokeMode);
                    WriteSetting("DocPlayerStyleKaraoke", doCPlayerStyleKaraoke);
                    WriteSetting("DoGameChartMode", doVerticalChart);
                    WriteSetting("DoRockBandChartMode", doRockBandChart);

                    WriteSetting("EnableAVSync", enableBTAVOffsetSync);
                    WriteSetting("BTAVOffset", BTAVOffsetSync);
                    WriteSetting("NautilusPath", nautilusPath);
                    WriteSetting("DoFocusMode", doFocusMode);
                    WriteSetting("UploadtoImgur", uploadScreenshots.Checked);

                    WriteSetting("StartMaximized", WindowState == FormWindowState.Maximized);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    File.AppendAllText(
                        Path.Combine(Application.StartupPath, "ConfigSaveErrors.txt"),
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + Environment.NewLine +
                        ex + Environment.NewLine + Environment.NewLine);
                }
                catch
                {
                    // Avoid crashing while trying to log a config-save failure.
                }
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
            displayAlbumArt = false;
            displayAudioSpectrum = false;
            doModernKaraokeMode = false;
            doCPlayerStyleKaraoke = false;
            doRockBandKaraoke = false;
            doRockBandChart = false;
            doVerticalChart = false;
            doMIDIChart = false;
        }

        private Dictionary<string, string> ReadConfigDictionary(string path)
        {
            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            if (!File.Exists(path))
                return dict;

            foreach (string rawLine in File.ReadAllLines(path))
            {
                if (string.IsNullOrWhiteSpace(rawLine))
                    continue;

                string line = rawLine.Trim();

                int equals = line.IndexOf('=');

                if (equals <= 0)
                    continue;

                string key = line.Substring(0, equals).Trim();
                string value = line.Substring(equals + 1).Trim();

                int comment = value.IndexOf("//", StringComparison.Ordinal);
                if (comment >= 0)
                    value = value.Substring(0, comment).Trim();

                dict[key] = value;
            }

            return dict;
        }

        private string GetString(Dictionary<string, string> cfg, string key, string defaultValue = "")
        {
            return cfg.TryGetValue(key, out string value) ? value : defaultValue;
        }

        private bool GetBool(Dictionary<string, string> cfg, string key, bool defaultValue = false)
        {
            if (!cfg.TryGetValue(key, out string value))
                return defaultValue;

            return value.Equals("true", StringComparison.OrdinalIgnoreCase);
        }

        private int GetInt(Dictionary<string, string> cfg, string key, int defaultValue = 0)
        {
            if (!cfg.TryGetValue(key, out string value))
                return defaultValue;

            return int.TryParse(value, out int result) ? result : defaultValue;
        }

        private double GetDouble(Dictionary<string, string> cfg, string key, double defaultValue = 0.0)
        {
            if (!cfg.TryGetValue(key, out string value))
                return defaultValue;

            return double.TryParse(
                value,
                System.Globalization.NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out double result)
                ? result
                : defaultValue;
        }

        private float GetFloat(Dictionary<string, string> cfg, string key, float defaultValue = 0f)
        {
            if (!cfg.TryGetValue(key, out string value))
                return defaultValue;

            return float.TryParse(
                value,
                System.Globalization.NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out float result)
                ? result
                : defaultValue;
        }

        private Color GetColor(Dictionary<string, string> cfg, string key, Color defaultValue)
        {
            if (!cfg.TryGetValue(key, out string value))
                return defaultValue;

            try
            {
                return ColorTranslator.FromHtml(value);
            }
            catch
            {
                return defaultValue;
            }
        }

        private void LoadConfig()
        {
            LoadFavorites();

            if (!File.Exists(config))
                return;

            UncheckAllModes();

            Dictionary<string, string> cfg = ReadConfigDictionary(config);

            if (cfg.Count == 0) return;

            try
            {
                PlayerConsole = GetString(cfg, "PlayerConsole", PlayerConsole);

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

                ApplyConsoleFromConfig();

                bool loop = GetBool(cfg, "LoopPlayback", picLoop.Tag != null && picLoop.Tag.ToString() == "loop");
                picLoop.Tag = loop ? "loop" : "noloop";
                toolTip1.SetToolTip(picLoop, loop ? "Disable track looping" : "Enable track looping");
                picLoop.Image = loop ? Resources.icon_loop_enabled : Resources.icon_loop_disabled1;

                bool shuffle = GetBool(cfg, "ShufflePlayback", picShuffle.Tag != null && picShuffle.Tag.ToString() == "shuffle");
                picShuffle.Tag = shuffle ? "shuffle" : "noshuffle";
                toolTip1.SetToolTip(picShuffle, shuffle ? "Disable track shuffling" : "Enable track shuffling");
                picShuffle.Image = shuffle ? Resources.icon_shuffle_enabled : Resources.icon_shuffle_disabled;

                autoloadLastPlaylist.Checked = GetBool(cfg, "AutoloadPlaylist", autoloadLastPlaylist.Checked);
                PlaylistPath = GetString(cfg, "LastPlaylist", PlaylistPath);
                autoPlay.Checked = GetBool(cfg, "AutoPlay", autoPlay.Checked);
                doAudioCrowd = GetBool(cfg, "PlayCrowdTrack", doAudioCrowd);

                showPracticeSections.Checked = GetBool(cfg, "ShowPracticeSessions", showPracticeSections.Checked);

                doStaticLyrics = GetBool(cfg, "ShowLyrics", doStaticLyrics);
                doWholeWordsLyrics = GetBool(cfg, "WholeWords", doWholeWordsLyrics);

                masterVol = GetFloat(cfg, "VolumeLevel", masterVol);

                doMIDIChart = GetBool(cfg, "DrawSnippet", doMIDIChart);

                for (int i = 0; i < 5; i++)
                {
                    string playlist = GetString(cfg, "RecentPlaylist" + (i + 1), RecentPlaylists[i]);

                    if (!string.IsNullOrEmpty(playlist) && File.Exists(playlist))
                        RecentPlaylists[i] = playlist;
                }

                displayAudioSpectrum = GetBool(cfg, "DrawSpectrum", displayAudioSpectrum);
                SpectrumID = GetInt(cfg, "SpectrumID", SpectrumID);
                displayAlbumArt = GetBool(cfg, "DisplayAlbumArt", displayAlbumArt);
                doHarmonyLyrics = GetBool(cfg, "DisplayHarmonies", doHarmonyLyrics);

                bool noLyrics = GetBool(cfg, "DontDisplayLyrics", false);
                doKaraokeLyrics = GetBool(cfg, "KaraokeLyrics", doKaraokeLyrics);
                doScrollingLyrics = GetBool(cfg, "ScrollingLyrics", doScrollingLyrics);

                if (noLyrics)
                {
                    doKaraokeLyrics = false;
                    doScrollingLyrics = false;
                }

                doMIDINameTracks = GetBool(cfg, "LabelTracks", doMIDINameTracks);
                PlaybackWindow = GetDouble(cfg, "PlaybackWindow", PlaybackWindow);
                NoteSizingType = GetInt(cfg, "NoteSizingType", NoteSizingType);
                doMIDINameProKeys = GetBool(cfg, "NameProKeysNotes", doMIDINameProKeys);
                doMIDINameVocals = GetBool(cfg, "NameVocalNotes", doMIDINameVocals);

                enableYARGCHVideos = GetBool(cfg, "DisplayBackgroundVideo", enableYARGCHVideos);
                playBGVideos.Checked = enableYARGCHVideos;

                doMIDIHighlightSolos = GetBool(cfg, "HighlightSolos", doMIDIHighlightSolos);
                uploadScreenshots.Checked = GetBool(cfg, "UploadtoImgur", uploadScreenshots.Checked);
                doMIDIBWKeys = GetBool(cfg, "BWProKeys", doMIDIBWKeys);
                doMIDIHarm1onVocals = GetBool(cfg, "UseHarm1ColorOnVocals", doMIDIHarm1onVocals);

                skipIntroOutroSilence.Checked = GetBool(cfg, "SkipIntroOutroSilence", skipIntroOutroSilence.Checked);
                SilenceThreshold = GetFloat(cfg, "SilenceThreshold", SilenceThreshold);
                FadeLength = GetDouble(cfg, "FadeInLength", FadeLength);

                KaraokeModeBackgroundColor = GetColor(cfg, "KaraokeModeBackground", KaraokeModeBackgroundColor);
                KaraokeModeHarm1Text = GetColor(cfg, "KaraokeModeLyric", KaraokeModeHarm1Text);
                KaraokeModeHarm1Highlight = GetColor(cfg, "KaraokeModeHighlight", KaraokeModeHarm1Highlight);
                KaraokeModeHarm2Text = GetColor(cfg, "KaraokeModeHarmony", KaraokeModeHarm2Text);
                KaraokeModeHarm2Highlight = GetColor(cfg, "KaraokeModeHarmonyHighlight", KaraokeModeHarm2Highlight);
                KaraokeModeHarm3Text = GetColor(cfg, "KaraokeModeHarmony2", KaraokeModeHarm3Text);
                KaraokeModeHarm3Highlight = GetColor(cfg, "KaraokeModeHarmony2Highlight", KaraokeModeHarm3Highlight);

                doRockBandKaraoke = GetBool(cfg, "DoRockBandKaraokeMode", doRockBandKaraoke);
                doModernKaraokeMode = GetBool(cfg, "DoClassicKaraokeMode", doModernKaraokeMode);
                doCPlayerStyleKaraoke = GetBool(cfg, "DocPlayerStyleKaraoke", doCPlayerStyleKaraoke);
                doVerticalChart = GetBool(cfg, "DoGameChartMode", doVerticalChart);

                doAnimatedBackground = GetBool(cfg, "UseAnimatedBackground", doAnimatedBackground);
                doStaticBackground = GetBool(cfg, "UseStaticBackground", doStaticBackground);
                doSolidColorBackground = GetBool(cfg, "UseSolidColorBackground", doSolidColorBackground);
                doStaticBackground2 = GetBool(cfg, "UseStaticBackground2", doStaticBackground2);
                doAnimatedBackground2 = GetBool(cfg, "UseAnimatedBackground2", doAnimatedBackground2);

                doMIDINoKeys = GetBool(cfg, "DoNoKeys", doMIDINoKeys);
                doMIDINoVocals = GetBool(cfg, "DoNoVocals", doMIDINoVocals);

                doUseBackgroundVideos = GetBool(cfg, "UseBackgroundVideos", doUseBackgroundVideos);
                doUseBackgroundImages = GetBool(cfg, "UseBackgroundImages", doUseBackgroundImages) && !enableYARGCHVideos;

                doRockBandChart = GetBool(cfg, "DoRockBandChartMode", doRockBandChart);

                enableBTAVOffsetSync = GetBool(cfg, "EnableAVSync", enableBTAVOffsetSync);
                BTAVOffsetSync = GetInt(cfg, "BTAVOffset", BTAVOffsetSync);

                nautilusPath = GetString(cfg, "NautilusPath", nautilusPath);
                ValidateNautilusPath();

                doFocusMode = GetBool(cfg, "DoFocusMode", doFocusMode);
                doMIDIVocals = GetBool(cfg, "DoMIDIVocals", doMIDIVocals);
                doMIDIHarmonies = GetBool(cfg, "DoMIDIHarmonies", doMIDIHarmonies);
                doAnimatedSpectrum = GetBool(cfg, "DoAnimSpectrum", doAnimatedSpectrum);
                doSpectrumColors = GetBool(cfg, "DoSpectrumColors", doSpectrumColors);

                NormalizeBackgroundModeSettings();
            }
            catch (Exception ex)
            {
                File.AppendAllText(
                    Path.Combine(Application.StartupPath, "ConfigLoadErrors.txt"),
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + Environment.NewLine +
                    ex + Environment.NewLine + Environment.NewLine);
            }
        }

        private void NormalizeBackgroundModeSettings()
        {
            if (!doUseBackgroundImages && !doUseBackgroundVideos && !doAnimatedSpectrum)
            {
                doUseBackgroundImages = true;
            }

            if (doSolidColorBackground)
            {
                doAnimatedBackground2 = false;
                doStaticBackground2 = false;
                SafeVisualsSetter(null);
            }

            if (doAnimatedBackground2)
            {
                doStaticBackground2 = false;
                doSolidColorBackground = false;
            }

            if (doStaticBackground2)
            {
                doSolidColorBackground = false;
                doAnimatedBackground2 = false;
            }

            if (doStaticBackground)
                doAnimatedBackground = false;

            if (doAnimatedBackground)
                doStaticBackground = false;
        }

        private void ApplyConsoleFromConfig()
        {
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
            audioMixerTool.PerformClick();
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
            lblFPS.Parent = picVisuals;
            lblFPS.Left = picVisuals.Width - lblFPS.Width;
            lblFPS.Top = 0;
            UpdateDisplay();            
            UpdateActiveRenderingResolution();
        }

        private static bool IsOriginalStageKit(HidDevice device)
        {
            return device.VendorID == 0x0E6F &&
                   device.ProductID == 0x0103;
        }

        private static bool IsFatsCoLight(HidDevice device)
        {
            return device.VendorID == 0x1209 &&
                   device.ProductID == 0x2882 &&
                   device.ReleaseNumberBcd == 0x0900;
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
                            _mediaPlayer.Time = GetBASSTimeForVideo();
                        }
                    }
                    if (secondScreen != null)
                    {
                        if (secondScreen.VideoIsPlaying)
                        {
                            secondScreen._mediaPlayer.Play();
                            if (secondScreen._mediaPlayer.IsSeekable)
                            {
                                secondScreen._mediaPlayer.Time = GetBASSTimeForVideo();
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
                { }
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
                
        public int GetKaraokeCurrentLineTop()
        {
            var renderSize = activeRenderingResolution;//new Size(1920, 1080);
            return (int)(renderSize.Height * 0.05);
        }

        public int GetKaraokeNextLineTop()
        {
            var renderSize = activeRenderingResolution;//new Size(1920, 1080);
            return (int)(renderSize.Height * 0.95);
        }

        void DrawAnimatedNotes(Graphics graphics, int noteCounter, int spawnFrequency, int screenWidth, int screenHeight)
        {
            string[] musicNotes = new[] { "🎵", "🎶", "♫", "♬" };
            int multiplier = (Size.Width / activeRenderingResolution.Width) < 1 ? 1 : (Size.Width / activeRenderingResolution.Width);
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

        private void PrepareLoadingBarAssets(int multiplier)
        {
            DisposeLoadingBarAssets();

            loadingBarFont = new Font("Arial", 24f * multiplier, FontStyle.Bold, GraphicsUnit.Point);

            loadingBarSize = TextRenderer.MeasureText(loadingBarXL, loadingBarFont);

            loadingBarBaseBmp = new Bitmap(loadingBarSize.Width, loadingBarSize.Height, PixelFormat.Format32bppArgb);
            loadingBarHighlightBmp = new Bitmap(loadingBarSize.Width, loadingBarSize.Height, PixelFormat.Format32bppArgb);

            using (Graphics gBase = Graphics.FromImage(loadingBarBaseBmp))
            {
                gBase.Clear(Color.Transparent);
                TextRenderer.DrawText(
                    gBase,
                    loadingBarXL,
                    loadingBarFont,
                    new Point(0, 0),
                    KaraokeModeHarm1Text,
                    Color.Transparent);
            }

            using (Graphics gHi = Graphics.FromImage(loadingBarHighlightBmp))
            {
                gHi.Clear(Color.Transparent);
                TextRenderer.DrawText(
                    gHi,
                    loadingBarXL,
                    loadingBarFont,
                    new Point(0, 0),
                    KaraokeModeHarm1Highlight,
                    Color.Transparent);
            }
        }

        private void DisposeLoadingBarAssets()
        {
            if (loadingBarBaseBmp != null)
            {
                loadingBarBaseBmp.Dispose();
                loadingBarBaseBmp = null;
            }

            if (loadingBarHighlightBmp != null)
            {
                loadingBarHighlightBmp.Dispose();
                loadingBarHighlightBmp = null;
            }

            if (loadingBarFont != null)
            {
                loadingBarFont.Dispose();
                loadingBarFont = null;
            }

            loadingBarSize = Size.Empty;
        }

        private void DoModernKaraoke(Size screenSize, Graphics graphics, IList<LyricPhrase> vocalPhrases, IEnumerable<Lyric> vocalLyrics,
            IList<LyricPhrase> harm1Phrases, IEnumerable<Lyric> harm1Lyrics,
            IList<LyricPhrase> harm2Phrases, IEnumerable<Lyric> harm2Lyrics,
            IList<LyricPhrase> harm3Phrases, IEnumerable<Lyric> harm3Lyrics)
        {
            var vocalLyricsList = vocalLyrics as IList<Lyric> ?? vocalLyrics.ToList();
            var harm1LyricsList = harm1Lyrics as IList<Lyric> ?? harm1Lyrics.ToList();
            var harm2LyricsList = harm2Lyrics as IList<Lyric> ?? harm2Lyrics.ToList();
            var harm3LyricsList = harm3Lyrics as IList<Lyric> ?? harm3Lyrics.ToList();

            bool hasHarm2 = harm2LyricsList.Count > 0;
            bool hasHarm3 = harm3LyricsList.Count > 0;

            var time = GetCorrectedTime();
            var AvgBPM = PlayingSong.BPM;
            //const int spawnFrequency = 30;
            noteCounter++;
            int resolutionX = screenSize.Width;
            int resolutionY = screenSize.Height;
            int multiplier = 1;
            double vertOffset = 0;
            int coverWidth = 512 * multiplier;
            int coverHeight = 512 * multiplier;

            doSoloVocals = doForceSoloVocals || !hasHarm2;
            doHarm2 = !doSoloVocals || doForceTwoPartHarmonies;
            doHarm3 = !doForceSoloVocals && !doForceTwoPartHarmonies && hasHarm3;

            try
            {
                try
                {
                    if (DoYargVideo())
                    {
                        graphics.Clear(Color.Transparent);
                        if (secondScreen != null)
                        {
                            SetPicVisualsBackColorIfChanged(Color.AliceBlue);
                        }
                    }
                    else if (doStaticBackground2)
                    {
                        var size = new Size(resolutionX, resolutionY);
                        DrawCachedRBKaraokeStaticBackground(graphics, size);
                    }
                    else if (doSolidColorBackground)
                    {
                        graphics.Clear(KaraokeModeBackgroundColor);
                        if (secondScreen != null)
                        {
                            SetPicVisualsBackColorIfChanged(Color.AliceBlue);
                        }
                    }
                }
                catch { }

                LyricPhrase actualNextLineHarmony = null;
                LyricPhrase actualLastLineHarmony = null;
                LyricPhrase currentLineLead = null;
                LyricPhrase nextLineLead = null;
                LyricPhrase lastLineLead = null;
                LyricPhrase actualLastLineLead = null;
                LyricPhrase actualNextLineLead = null;
                bool hasInlineGap = false;

                var phrasesLead = hasHarm2 && (doHarm2 || doHarm3) ? harm1Phrases : vocalPhrases;
                var lyricsLead = hasHarm2 && (doHarm2 || doHarm3) ? harm1LyricsList : vocalLyricsList;

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
                    for (var i = 0; i < harm2Phrases.Count; i++)
                    {
                        var phrase = harm2Phrases[i];
                        lastLineHarm2 = i > 0 ? harm2Phrases[i - 1] : null;

                        if (phrase.PhraseEnd <= time)
                        {
                            actualLastLineHarmony = harm2Phrases[i];
                            if (i < harm2Phrases.Count - 1)
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
                    for (var i = 0; i < harm3Phrases.Count; i++)
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

                if (doSoloVocals || !hasHarm2) //do solo vocals
                {
                    harm1LineTop1 = (int)(lineHeight * (2.5 + vertOffset));
                    harm1LineTop2 = (int)(lineHeight * (4.0 + vertOffset));
                    harm1LineTop3 = (int)(lineHeight * 5.5);
                    harm1LineTop4 = (int)(lineHeight * 7.0);
                }
                if (doHarm3 && hasHarm3)
                {
                    harm1LineTop1 = lineHeight * 0;
                    harm1LineTop2 = (int)(lineHeight * 1.5);
                    harm2LineTop1 = lineHeight * 4;
                    harm2LineTop2 = (int)(lineHeight * 5.5);
                    harm3LineTop1 = lineHeight * 8;
                    harm3LineTop2 = (int)(lineHeight * 9.5);
                }
                else if ((doHarm2 || doHarm3) && hasHarm2)
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
                    var bpm = AvgBPM == 0 ? "" : "Tempo: " + Math.Round(AvgBPM, 0, MidpointRounding.AwayFromZero) + " BPM";
                    var parts = 1;
                    if ((doHarm2 || doHarm3) && hasHarm2)
                    {
                        parts++;
                    }
                    if (doHarm3 && hasHarm3)
                    {
                        parts++;
                    }
                    var vocalParts = "Vocals: " + ((doHarm2 || doHarm3) && hasHarm2 ? parts + "-part harmony" : "Solo");
                    var songKey = "";//GetSongKey(); - need to add detection of official HMX stuff vs customs before this is usable
                    var genre = Parser.doGenre(Parser.Songs[0].Genre).Replace("&", "&&");
                    if (!string.IsNullOrEmpty(genre))
                    {
                        genre = "Genre: " + genre;
                    }

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
                        charter = $"Charted by {charter}";
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
                var baseFont = _karaokeBaseFont;
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
                            time, multiplier
                        );

                        DrawSyllableAccurateLine(
                            graphics,
                            line2Syllables,
                            displayFont,
                            resolutionX,
                            harm1LineTop2,
                            KaraokeModeHarm1Text,
                            KaraokeModeHarm1Highlight,
                            time, multiplier
                        );
                    }

                    if ((doSoloVocals || !hasHarm2) && nextLineLead != null && !string.IsNullOrEmpty(nextLineLead.PhraseText))
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
                            time, multiplier
                        );

                        DrawSyllableAccurateLine(
                            graphics,
                            line4Syllables,
                            displayFont,
                            resolutionX,
                            harm1LineTop4,
                            KaraokeModeHarm1Text,
                            KaraokeModeHarm1Highlight,
                            time, multiplier
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
                            time, multiplier
                        );

                    DrawSyllableAccurateLine(
                        graphics,
                        line2Syllables,
                        displayFont,
                        resolutionX,
                        harm2LineTop2,
                        KaraokeModeHarm2Text,
                        KaraokeModeHarm2Highlight,
                        time, multiplier
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
                            time, multiplier
                     );

                    DrawSyllableAccurateLine(
                        graphics,
                        line2Syllables,
                        displayFont,
                        resolutionX,
                        harm3LineTop2,
                        KaraokeModeHarm3Text,
                        KaraokeModeHarm3Highlight,
                        time, multiplier
                    );
                    displayFont.Dispose();
                    drewText = true;
                }
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
                        PrepareLoadingBarAssets(multiplier);
                        int posX = (resolutionX - loadingBarSize.Width) / 2;
                        int posY = (resolutionY - loadingBarSize.Height) / 2;

                        graphics.DrawImageUnscaled(loadingBarBaseBmp, posX, posY);

                        double progress = 1.0 - (double)(wait / gap);
                        progress = Math.Max(0.0, Math.Min(1.0, progress));

                        int highlightWidth = (int)Math.Round(loadingBarSize.Width * progress);

                        if (highlightWidth > 0)
                        {
                            Rectangle src = new Rectangle(0, 0, Math.Min(highlightWidth, loadingBarHighlightBmp.Width), loadingBarHighlightBmp.Height);
                            Rectangle dest = new Rectangle(posX, posY, src.Width, src.Height);
                            graphics.DrawImage(loadingBarHighlightBmp, dest, src, GraphicsUnit.Pixel);
                        }
                        if (wait <= 5.5 && wait > 1.0)
                        {
                            string waitString = ((int)wait).ToString();
                            DrawCenteredLine(
                                graphics,
                                waitString,
                                resolutionX,
                                (resolutionY - TextRenderer.MeasureText(waitString, new Font("Arial", 80f * multiplier, FontStyle.Bold)).Height) / 2,
                                80f * multiplier);
                        }
                    }
                }
                catch { }
            }
            catch// (Exception ex)
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

        private void DrawSyllableAccurateHighlightFromBitmap(
            Graphics g,
            Bitmap highlightBitmap,
            int posX,
            int posY,
            int textHeight,
            List<SyllablePixelSpan> pixelmap,
            double adjustedTime)
        {
            if (highlightBitmap == null || pixelmap == null || pixelmap.Count == 0)
                return;

            int revealRight = 0;

            for (int i = 0; i < pixelmap.Count; i++)
            {
                var syllable = pixelmap[i];

                int syllableWidth = syllable.Right - syllable.Left;

                if (syllableWidth <= 0)
                    continue;

                if (adjustedTime >= syllable.End)
                {
                    // Fully sung syllable.
                    revealRight = Math.Max(revealRight, syllable.Right);
                    continue;
                }

                if (adjustedTime > syllable.Start)
                {
                    // Currently active syllable.
                    double duration = syllable.End - syllable.Start;

                    double progress = duration <= 0
                        ? 1.0
                        : (adjustedTime - syllable.Start) / duration;

                    progress = Math.Max(0.0, Math.Min(1.0, progress));

                    int partialWidth = (int)Math.Floor(syllableWidth * progress);

                    revealRight = Math.Max(revealRight, syllable.Left + partialWidth);
                }

                // If we reached a future or active syllable, stop.
                break;
            }

            if (revealRight <= 0)
                return;

            revealRight = Math.Min(revealRight, highlightBitmap.Width);

            Rectangle src = new Rectangle(
                0,
                0,
                Math.Min(revealRight, highlightBitmap.Width),
                Math.Min(textHeight, highlightBitmap.Height));

            Rectangle dest = new Rectangle(
                posX,
                posY,
                src.Width,
                src.Height);

            g.DrawImage(highlightBitmap, dest, src, GraphicsUnit.Pixel);
        }

        public void DrawSyllableAccurateLine(
            Graphics g,
            List<Lyric> syllablesForThisLine,
            Font font,
            int resolutionX,
            int y,
            Color baseColor,
            Color highlightColor,
            double adjustedTime,
            int multiplier)
        {
            if (syllablesForThisLine == null || syllablesForThisLine.Count == 0)
                return;

            int strokeWidth = 5 * multiplier;

            CachedKaraokeLine cached = GetOrCreateCachedKaraokeLine(
                g,
                syllablesForThisLine,
                font,
                resolutionX,
                baseColor,
                highlightColor,
                strokeWidth);

            if (cached == null || cached.BaseBitmap == null || cached.HighlightBitmap == null)
                return;

            // Draw base text from cache.
            g.DrawImageUnscaled(cached.BaseBitmap, cached.PosX, y);

            // Draw highlighted/revealed text from cached highlight bitmap.
            DrawSyllableAccurateHighlightFromBitmap(
                g,
                cached.HighlightBitmap,
                cached.PosX,
                y,
                cached.TextHeight,
                cached.PixelMap,
                adjustedTime);
        }

        public class SyllablePixelSpan
        {
            public double Start { get; set; }
            public double End { get; set; }
            public int Left { get; set; }
            public int Right { get; set; }
        }
                
        private static void ApplyTextRenderingSettings(Graphics g)
        {            
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
        }
                
        private class CachedKaraokeLine : IDisposable
        {
            public string CacheKey;
            public string DisplayText;
            public List<MergedSyllable> MergedSyllables;
            public List<SyllablePixelSpan> PixelMap;

            public int TextWidth;
            public int TextHeight;
            public int PosX;

            public int PaddingX;
            public int PaddingY;

            public Bitmap BaseBitmap;
            public Bitmap HighlightBitmap;

            public void Dispose()
            {
                BaseBitmap?.Dispose();
                HighlightBitmap?.Dispose();

                BaseBitmap = null;
                HighlightBitmap = null;
            }
        }

        public void ClearKaraokeLineCache()
        {
            foreach (var item in _karaokeLineCache.Values)
                item.Dispose();

            _karaokeLineCache.Clear();
        }

        private CachedKaraokeLine GetOrCreateCachedKaraokeLine(
            Graphics g,
            List<Lyric> syllablesForThisLine,
            Font font,
            int resolutionX,
            Color baseColor,
            Color highlightColor,
            int strokeWidth)
        {
            if (syllablesForThisLine == null || syllablesForThisLine.Count == 0)
                return null;

            // Build a stable key            
            string rawKey = string.Join("|", syllablesForThisLine.Select(s =>
                (s.Text ?? "") + "@" + s.Start.ToString("0.000") + "-" + s.End.ToString("0.000")));

            string cacheKey =
                rawKey +
                "|font=" + font.Name +
                "|size=" + font.SizeInPoints.ToString("0.###") +
                "|style=" + ((int)font.Style) +
                "|resX=" + resolutionX +
                "|base=" + baseColor.ToArgb() +
                "|hi=" + highlightColor.ToArgb() +
                "|stroke=" + strokeWidth;

            if (_karaokeLineCache.TryGetValue(cacheKey, out CachedKaraokeLine cached))
                return cached;

            cached = new CachedKaraokeLine();
            cached.CacheKey = cacheKey;

            cached.MergedSyllables = MergeSustainedSyllables(syllablesForThisLine);
            cached.DisplayText = ReconstructPhraseTextFromSyllables(cached.MergedSyllables);

            ApplyTextRenderingSettings(g);

            SizeF visualSizeF = g.MeasureString(cached.DisplayText, font);
            int padX = strokeWidth + 8;
            int padY = strokeWidth + 4;

            int measuredWidth = Math.Max(1, (int)Math.Ceiling(visualSizeF.Width));
            int measuredHeight = Math.Max(1, (int)Math.Ceiling(visualSizeF.Height));

            cached.TextWidth = measuredWidth + (padX * 2);
            cached.TextHeight = measuredHeight + (padY * 2);

            // Center based on the visible measured text, not the padded bitmap.
            cached.PosX = (resolutionX - measuredWidth) / 2 - padX;

            cached.PaddingX = padX;
            cached.PaddingY = padY;

            cached.PixelMap = BuildSyllablePixelMap(
                cached.MergedSyllables,
                font,
                g,
                cached.DisplayText,
                measuredWidth);

            foreach (var span in cached.PixelMap)
            {
                span.Left += padX;
                span.Right += padX;
            }

            cached.BaseBitmap = new Bitmap(cached.TextWidth, cached.TextHeight);
            cached.HighlightBitmap = new Bitmap(cached.TextWidth, cached.TextHeight);

            using (Graphics gBase = Graphics.FromImage(cached.BaseBitmap))
            {
                gBase.Clear(Color.Transparent);
                ApplyTextRenderingSettings(gBase);

                DrawTextWithStroke(
                    gBase,
                    cached.DisplayText,
                    font,
                    new Point(padX, padY),
                    baseColor,
                    Color.Black,
                    strokeWidth);
            }

            using (Graphics gHighlight = Graphics.FromImage(cached.HighlightBitmap))
            {
                gHighlight.Clear(Color.Transparent);
                ApplyTextRenderingSettings(gHighlight);

                DrawTextWithStroke(
                    gHighlight,
                    cached.DisplayText,
                    font,
                    new Point(padX, padY),
                    highlightColor,
                    Color.Black,
                    strokeWidth);
            }

            _karaokeLineCache[cacheKey] = cached;

            return cached;
        }

        private void DrawTextWithStroke(Graphics g, string text, Font font, Point pos, Color fill, Color stroke, int strokeWidth)
        {
            using (var stringFormat = (StringFormat)StringFormat.GenericTypographic.Clone())
            using (var path = new GraphicsPath())
            using (var pen = new Pen(stroke, strokeWidth) { LineJoin = LineJoin.Round })
            using (var fillBrush = new SolidBrush(fill))
            {
                stringFormat.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
                stringFormat.Trimming = StringTrimming.None;

                float emSize = g.DpiY * font.SizeInPoints / 72f;

                path.AddString(
                    text,
                    font.FontFamily,
                    (int)font.Style,
                    emSize,
                    new PointF(pos.X, pos.Y),
                    stringFormat);

                g.DrawPath(pen, path);
                g.FillPath(fillBrush, path);
            }
        }

        private string GetVisibleTextForSyllable(MergedSyllable s)
        {
            var raw = s.Lyric?.Trim() ?? string.Empty;
            var clean = CleanSyllable(raw);
            return clean.Replace("‿", " ");
        }

        private void DrawSyllableAccurateHighlightIsolated(
            Graphics g,
            List<MergedSyllable> pixelmap,
            Font font,
            int posX,
            int posY,
            Color highlightColor,
            Color strokeColor,
            double adjustedTime,
            int multiplier)
        {
            if (pixelmap == null || pixelmap.Count == 0)
                return;

            float currentX = 0f;

            using (var stringFormat = (StringFormat)StringFormat.GenericTypographic.Clone())
            {
                stringFormat.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
                stringFormat.Trimming = StringTrimming.None;

                for (int i = 0; i < pixelmap.Count; i++)
                {
                    var syllable = pixelmap[i];
                    if (syllable == null)
                        continue;

                    string visible = GetVisibleTextForSyllable(syllable);
                    if (string.IsNullOrWhiteSpace(visible))
                    {
                        currentX += syllable.Width;
                        continue;
                    }

                    int left = (int)Math.Round(currentX);
                    int syllableWidth = Math.Max(1, (int)Math.Round(syllable.Width));

                    double duration = syllable.End - syllable.Start;

                    if (adjustedTime >= syllable.End)
                    {
                        using (var bmp = new Bitmap(syllableWidth + 8 * multiplier, font.Height + 8 * multiplier))
                        using (var gBmp = Graphics.FromImage(bmp))
                        {
                            gBmp.Clear(Color.Transparent);
                            ApplyTextRenderingSettings(gBmp);

                            DrawTextWithStroke(
                                gBmp,
                                visible,
                                font,
                                new Point(0, 0),
                                highlightColor,
                                strokeColor,
                                5 * multiplier);

                            g.DrawImageUnscaled(bmp, posX + left, posY);
                        }
                    }
                    else if (adjustedTime > syllable.Start)
                    {
                        double progress = duration <= 0
                            ? 1.0
                            : (adjustedTime - syllable.Start) / duration;

                        progress = Math.Max(0.0, Math.Min(1.0, progress));

                        int partialWidth = (int)Math.Round(syllableWidth * progress);
                        if (partialWidth > 0)
                        {
                            using (var bmp = new Bitmap(syllableWidth + 8 * multiplier, font.Height + 8 * multiplier))
                            using (var gBmp = Graphics.FromImage(bmp))
                            {
                                gBmp.Clear(Color.Transparent);
                                ApplyTextRenderingSettings(gBmp);

                                DrawTextWithStroke(
                                    gBmp,
                                    visible,
                                    font,
                                    new Point(0, 0),
                                    highlightColor,
                                    strokeColor,
                                    5 * multiplier);

                                Rectangle src = new Rectangle(0, 0, Math.Min(partialWidth, bmp.Width), bmp.Height);
                                Rectangle dest = new Rectangle(posX + left, posY, src.Width, src.Height);

                                g.DrawImage(bmp, dest, src, GraphicsUnit.Pixel);
                            }
                        }

                        break;
                    }
                    else
                    {
                        break;
                    }

                    currentX += syllable.Width;
                }
            }
        }

        public List<SyllablePixelSpan> BuildSyllablePixelMap(
            List<MergedSyllable> syllables,
            Font font,
            Graphics g,
            string displayText,
            float totalTextWidth)
        {
            ApplyTextRenderingSettings(g);

            var result = new List<SyllablePixelSpan>();

            using (var format = (StringFormat)StringFormat.GenericTypographic.Clone())
            {
                format.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
                format.Trimming = StringTrimming.None;

                int searchIndex = 0;
                float prevRight = 0f;

                for (int i = 0; i < syllables.Count; i++)
                {
                    var syllable = syllables[i];
                    string visible = GetVisibleTextForSyllable(syllable);

                    if (string.IsNullOrWhiteSpace(visible))
                        continue;

                    int idx = displayText.IndexOf(visible, searchIndex, StringComparison.Ordinal);
                    if (idx < 0)
                        continue;

                    RectangleF layoutRect = new RectangleF(0, 0, totalTextWidth * 2f + 100f, font.Height * 4f);

                    format.SetMeasurableCharacterRanges(new[]
                    {
                new CharacterRange(0, idx + visible.Length)
            });

                    Region[] regions = g.MeasureCharacterRanges(displayText, font, layoutRect, format);
                    RectangleF bounds = regions[0].GetBounds(g);

                    float rightF = bounds.Right;
                    float leftF = prevRight;

                    int left = (int)Math.Round(leftF);
                    int right = (int)Math.Round(rightF);

                    if (right < left)
                        right = left;

                    result.Add(new SyllablePixelSpan
                    {
                        Start = syllable.Start,
                        End = syllable.End,
                        Left = left,
                        Right = right
                    });

                    prevRight = rightF;
                    searchIndex = idx + visible.Length;

                    foreach (var region in regions)
                        region.Dispose();
                }
            }

            return result;
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
                .Replace("+", "")
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

        private void DrawCenteredLine(
            Graphics g,
            string text,
            int resolutionX,
            int y,
            float maxFontSize,
            int offset = 0,
            int shadowOffsetX = 1,
            int shadowOffsetY = 1,
            int shadowBlur = 3,
            float shadowOpacity = 0.20f
            )
        {
            using (var baseFont = new Font("Arial", maxFontSize))
            {
                float scaledFontSize = GetScaledFontSize(g, text, baseFont, maxFontSize, resolutionX - offset);

                using (var font = new Font("Arial", scaledFontSize))
                {
                    ApplyTextRenderingSettings(g);

                    // measure for centering
                    var size = TextRenderer.MeasureText(g, text, font);
                    int x = (resolutionX + offset - size.Width) / 2;

                    Color textColor = Color.White;

                    // Create an offscreen bitmap for blur
                    /*using (Bitmap shadowBmp = new Bitmap(size.Width + shadowBlur * 2, size.Height + shadowBlur * 2))
                    using (Graphics shadowG = Graphics.FromImage(shadowBmp))
                    {
                        shadowG.Clear(Color.Transparent);
                        shadowG.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

                        // Shadow color (same shape as text)
                        using (Brush shadowBrush = new SolidBrush(Color.FromArgb((int)(255 * shadowOpacity), 0, 0, 0)))
                        {
                            shadowG.DrawString(text, font, shadowBrush, shadowBlur, shadowBlur);
                        }

                        // Apply a simple blur approximation by redrawing the bitmap slightly offset
                        for (int dx = -shadowBlur; dx <= shadowBlur; dx++)
                        {
                            for (int dy = -shadowBlur; dy <= shadowBlur; dy++)
                            {
                                if (dx == 0 && dy == 0) continue;
                                float weight = 1f - (float)Math.Sqrt(dx * dx + dy * dy) / shadowBlur;
                                if (weight <= 0) continue;

                                using (var tempBrush = new TextureBrush(shadowBmp))
                                {
                                    ColorMatrix cm = new ColorMatrix
                                    {
                                        Matrix33 = weight * 0.2f // blur transparency falloff
                                    };
                                    using (ImageAttributes ia = new ImageAttributes())
                                    {
                                        ia.SetColorMatrix(cm, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                                        g.DrawImage(shadowBmp,
                                            new Rectangle(x + shadowOffsetX + dx, y + shadowOffsetY + dy,
                                                          shadowBmp.Width, shadowBmp.Height),
                                            0, 0, shadowBmp.Width, shadowBmp.Height,
                                            GraphicsUnit.Pixel, ia);
                                    }
                                }
                            }
                        }
                    }*/

                    Color strokeCol = Color.Black;

                    DrawTextWithStroke(
                        g,
                        text,
                        font,
                        new Point(x, y),
                        textColor,
                        strokeCol,
                        3
                    );
                }
            }
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
            var renderSize = activeRenderingResolution;//new Size(1920, 1080);

            double time = GetCorrectedTime();
            LyricPhrase currentLine = null;
            LyricPhrase nextLine = null;
            LyricPhrase lastLine = null;

            // get active and next phrase, and store last used phrase
            for (int i = 0; i < phrases.Count; i++)
            {
                var phrase = phrases[i];
                if (string.IsNullOrEmpty(phrase.PhraseText))
                    continue;

                if (phrase.PhraseEnd < time)
                {
                    lastLine = phrase;
                    continue;
                }

                if (phrase.PhraseStart > time)
                {
                    nextLine = phrase;
                    break;
                }

                currentLine = phrase;
                if (i < phrases.Count - 1)
                {
                    nextLine = phrases[i + 1];
                }
                break;
            }

            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            int currentLineTop = GetKaraokeCurrentLineTop();
            int nextLineTop = GetKaraokeNextLineTop();

            try
            {
                if (DoYargVideo())
                {
                    graphics.Clear(Color.Transparent);
                    if (secondScreen != null)
                    {
                        SetPicVisualsBackColorIfChanged(Color.AliceBlue);
                    }
                }
                else
                {
                    if (secondScreen != null)
                    {
                        graphics.Clear(KaraokeModeBackgroundColor);
                        SetPicVisualsBackColorIfChanged(Color.AliceBlue);
                    }
                    else
                    {
                        graphics.Clear(KaraokeModeBackgroundColor);
                    }
                    SetPicVisualsBackColorIfChanged(secondScreen == null ? KaraokeModeBackgroundColor : Color.AliceBlue);
                }
            }
            catch { }

            List<Lyric> lyricsList = lyrics as List<Lyric> ?? lyrics.ToList();

            using (var stringFormat = (StringFormat)StringFormat.GenericTypographic.Clone())
            using (var textBrush = new SolidBrush(KaraokeModeHarm1Text))
            using (var highlightBrush = new SolidBrush(KaraokeModeHarm1Highlight))
            {
                stringFormat.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
                stringFormat.Trimming = StringTrimming.None;

                if (currentLine != null && !string.IsNullOrEmpty(currentLine.PhraseText))
                {
                    // draw entire current phrase on top
                    string lineText = ProcessLine(currentLine.PhraseText, true).Replace("‿", " ");

                    using (var baseFont = new Font("Tahoma", 12f))
                    using (var lineFont = new Font("Tahoma", GetScaledFontSize(graphics, lineText, baseFont, 120)))
                    {
                        SizeF lineSizeF = MeasureDrawString(graphics, lineText, lineFont, stringFormat);
                        int lineWidth = (int)Math.Ceiling(lineSizeF.Width);
                        int lineHeight = (int)Math.Ceiling(lineSizeF.Height);
                        int posX = (renderSize.Width - lineWidth) / 2;

                        //graphics.DrawString(lineText, lineFont, textBrush, posX, currentLineTop, stringFormat);
                        DrawTextWithStroke(graphics, lineText, lineFont, new Point(posX, currentLineTop), KaraokeModeHarm1Text, Color.Black, 5);

                        // draw portion of current phrase that's already been sung
                        string sungLine = string.Join(" ",
                            lyricsList
                                .Where(lyr => lyr.Start >= currentLine.PhraseStart && lyr.Start <= time)
                                .Select(lyr => lyr.Text));

                        sungLine = ProcessLine(sungLine, true).Replace("‿", " ");

                        if (!string.IsNullOrEmpty(sungLine))
                        {
                            //graphics.DrawString(sungLine, lineFont, highlightBrush, posX, currentLineTop, stringFormat);
                            DrawTextWithStroke(graphics, sungLine, lineFont, new Point(posX, currentLineTop), KaraokeModeHarm1Highlight, Color.Black, 5);
                        }

                        if (currentLine.PhraseStart <= time - 0.1)
                        {
                            var wordList = new List<ActiveWord>();
                            string word = "";
                            double wordStart = 0;
                            double wordEnd = 0;

                            for (int i = 0; i < lyricsList.Count; i++)
                            {
                                var lyric = lyricsList[i];

                                // Skip lyrics outside the proper time
                                if (lyric.Start < currentLine.PhraseStart || lyric.Start > currentLine.PhraseEnd)
                                    continue;

                                if (string.IsNullOrEmpty(word))
                                    wordStart = lyric.Start;

                                if (lyric.Text.Contains("-")) // is a syllable
                                {
                                    word += ProcessLine(lyric.Text, true);
                                    wordEnd = lyric.End;
                                    continue;
                                }
                                // Handle sustains
                                else if (!string.IsNullOrEmpty(word) && lyric.Text.Contains("+"))
                                {
                                    wordEnd = lyric.End;

                                    // Extend for consecutive sustains
                                    for (int a = i + 1; a < lyricsList.Count; a++)
                                    {
                                        if (lyricsList[a].Text.Contains("+"))
                                        {
                                            wordEnd = lyricsList[a].End;
                                            i = a;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    continue;
                                }
                                else
                                {
                                    // Append regular lyrics to the word
                                    word += ProcessLine(lyric.Text, true).Replace("‿", " ");
                                    wordEnd = lyric.End;

                                    // look ahead to double check next lyric(s) aren't + sustains
                                    for (int z = i + 1; z < lyricsList.Count; z++)
                                    {
                                        if (lyricsList[z].Text.Contains("+"))
                                        {
                                            wordEnd = lyricsList[z].End;
                                            i = z;
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
                                        word = "";
                                    }
                                }
                            }

                            // Find the active word matching playback time
                            var activeWord = wordList.FirstOrDefault(w => w.WordStart <= time && w.WordEnd > time);

                            if (activeWord != null && !string.IsNullOrEmpty(activeWord.Text))
                            {
                                activeWord.Text = activeWord.Text.Replace("‿", " ");

                                using (var activeBaseFont = new Font("Tahoma", 12f))
                                using (var activeWordFont = new Font("Tahoma", GetScaledFontSize(graphics, activeWord.Text, activeBaseFont, 200)))
                                {
                                    SizeF activeSizeF = MeasureDrawString(graphics, activeWord.Text, activeWordFont, stringFormat);
                                    int activeWidth = (int)Math.Ceiling(activeSizeF.Width);
                                    int activeHeight = (int)Math.Ceiling(activeSizeF.Height);

                                    int activeX = (renderSize.Width - activeWidth) / 2;
                                    int activeY = (renderSize.Height - activeHeight) / 2;

                                    // Draw the entire word in normal color
                                    //graphics.DrawString(activeWord.Text, activeWordFont, textBrush, activeX, activeY, stringFormat);
                                    DrawTextWithStroke(graphics, activeWord.Text, activeWordFont, new Point(activeX, activeY), KaraokeModeHarm1Text, Color.Black, 5);
                                    // Calculate progress for the sung portion
                                    double duration = activeWord.WordEnd - activeWord.WordStart;
                                    float progress = duration <= 0
                                        ? 1f
                                        : Clamp((float)((time - activeWord.WordStart) / duration), 0.0f, 1.0f);

                                    int numCharsToHighlight = (int)Math.Ceiling(progress * activeWord.Text.Length);
                                    numCharsToHighlight = Math.Min(numCharsToHighlight, activeWord.Text.Length);

                                    string sungPortion = activeWord.Text.Substring(0, numCharsToHighlight);

                                    if (!string.IsNullOrEmpty(sungPortion))
                                    {
                                        //graphics.DrawString(sungPortion, activeWordFont, highlightBrush, activeX, activeY, stringFormat);
                                        DrawTextWithStroke(graphics, sungPortion, activeWordFont, new Point(activeX, activeY), KaraokeModeHarm1Highlight, Color.Black, 5);
                                    }
                                }
                            }
                        }
                    }
                }

                if (nextLine != null && !string.IsNullOrEmpty(nextLine.PhraseText))
                {
                    // draw entire next phrase on bottom
                    string lineText = ProcessLine(nextLine.PhraseText, true).Replace("‿", " ");

                    using (var baseFont = new Font("Tahoma", 12f))
                    using (var lineFont = new Font("Tahoma", GetScaledFontSize(graphics, lineText, baseFont, 120)))
                    {
                        SizeF lineSizeF = MeasureDrawString(graphics, lineText, lineFont, stringFormat);
                        int lineWidth = (int)Math.Ceiling(lineSizeF.Width);
                        int lineHeight = (int)Math.Ceiling(lineSizeF.Height);
                        int posX = (renderSize.Width - lineWidth) / 2;

                        //graphics.DrawString(lineText, lineFont, textBrush, posX, nextLineTop - lineHeight, stringFormat);
                        DrawTextWithStroke(graphics, lineText, lineFont, new Point(posX, nextLineTop - lineHeight), KaraokeModeHarm1Text, Color.Black, 6);
                    }
                }

                // draw waiting/countdown info
                if (currentLine != null && nextLine != null)
                    return;

                if (lastLine != null && nextLine != null)
                {
                    double difference = nextLine.PhraseStart - lastLine.PhraseEnd;
                    if (difference < 5)
                        return;
                }

                string middleText = "";
                Color middleColor = KaraokeModeHarm1Text;

                if (currentLine == null && nextLine != null)
                {
                    double wait = nextLine.PhraseStart - time;
                    if (wait < 1.5)
                        return;

                    middleText = wait <= 5 ? "[GET READY]" : "[WAIT: " + ((int)(wait + 0.5)) + "]";
                    middleColor = wait <= 5 ? KaraokeModeHarm1Highlight : KaraokeModeHarm1Text;
                }
                else if (currentLine == null)
                {
                    middleText = "[fin]";
                }

                using (var middleBrush = new SolidBrush(middleColor))
                using (var baseFont = new Font("Tahoma", 12f))
                using (var lineFont = new Font("Tahoma", GetScaledFontSize(graphics, middleText, baseFont, 200)))
                {
                    SizeF lineSizeF = MeasureDrawString(graphics, middleText, lineFont, stringFormat);
                    int lineWidth = (int)Math.Ceiling(lineSizeF.Width);
                    int lineHeight = (int)Math.Ceiling(lineSizeF.Height);
                    int posX = (renderSize.Width - lineWidth) / 2;
                    int posY = (renderSize.Height - lineHeight) / 2;

                    //graphics.DrawString(middleText, lineFont, middleBrush, posX, posY, stringFormat);
                    DrawTextWithStroke(graphics, middleText, lineFont, new Point(posX, posY), middleColor, Color.Black, 6);
                }
            }
        }

        private SizeF MeasureDrawString(Graphics graphics, string text, Font font, StringFormat format)
        {
            if (string.IsNullOrEmpty(text))
                return SizeF.Empty;

            return graphics.MeasureString(text, font, int.MaxValue, format);
        }

        public float Clamp(float value, float min, float max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        public float GetScaledFontSize(Graphics g, string line, Font PreferedFont, float maxSize)
        {
            var renderSize = activeRenderingResolution;//new Size(1920, 1080);
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
            if (PlayingSong == null) return 0;

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
                Chart.Clear(doMIDIChart ? TrackBackgroundColor1 : GetNoteColor(100));
            }
        }

        private void UpdateVisualStyle()
        {
            Image image = null;
            if (secondScreen != null)
            {
                secondScreen.ChangeVisualsImage(image);
            }
            else
            {
                SafeVisualsSetter(image);
                //picVisuals.Image = image;
            }
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
            nautilusToolStripMenuItem.Enabled = true;
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

        private int GetCurrentBeatMarkerIndex(double correctedTime)
        {
            if (_beatMarkers == null || _beatMarkers.Count == 0)
                return -1;

            int lo = 0;
            int hi = _beatMarkers.Count - 1;

            while (lo <= hi)
            {
                int mid = lo + ((hi - lo) / 2);

                if (_beatMarkers[mid].TimeSeconds <= correctedTime)
                    lo = mid + 1;
                else
                    hi = mid - 1;
            }

            return hi; // last beat marker at or before correctedTime
        }

        private void AnimateStageKits(double songTimeSec)
        {
            if (_beatMarkers == null || _beatMarkers.Count < 2)
                return;

            // Compensate for worker/packet spacing.
            // Tune between 0.015 and 0.040 if needed.
            const double stageKitLeadSeconds = 0.025;
            double stageKitTime = songTimeSec + stageKitLeadSeconds;

            int beatIndex = GetCurrentBeatMarkerIndex(stageKitTime);
            if (beatIndex < 0 || beatIndex >= _beatMarkers.Count)
                return;

            BeatMarker beat = _beatMarkers[beatIndex];

            double bpm = PlayingSong != null && PlayingSong.BPM > 0 ? PlayingSong.BPM : 120.0;

            double beatDuration;
            if (beatIndex + 1 < _beatMarkers.Count)
                beatDuration = _beatMarkers[beatIndex + 1].TimeSeconds - _beatMarkers[beatIndex].TimeSeconds;
            else if (beatIndex > 0)
                beatDuration = _beatMarkers[beatIndex].TimeSeconds - _beatMarkers[beatIndex - 1].TimeSeconds;
            else
                beatDuration = 60.0 / bpm;

            if (beatDuration <= 0.0001)
                beatDuration = 60.0 / bpm;

            const int subdivisions = 2;

            double beatStart = _beatMarkers[beatIndex].TimeSeconds;
            double phaseWithinBeat = (stageKitTime - beatStart) / beatDuration;

            if (phaseWithinBeat < 0.0) phaseWithinBeat = 0.0;
            if (phaseWithinBeat > 0.9999) phaseWithinBeat = 0.9999;

            int subBeat = (int)Math.Floor(phaseWithinBeat * subdivisions);
            int subBeatIndex = (beatIndex * subdivisions) + subBeat;

            if (subBeatIndex == _lastStageKitSubBeatIndex)
                return;

            _lastStageKitSubBeatIndex = subBeatIndex;

            bool newBeat = beatIndex != _lastStageKitBeatIndex;
            if (newBeat)
                _lastStageKitBeatIndex = beatIndex;

            bool newMeasure = false;
            if (beat.IsMeasure && beatIndex != _lastStageKitMeasureIndex)
            {
                newMeasure = true;
                _lastStageKitMeasureIndex = beatIndex;
            }                     
                    
            if (useLEDs.Checked || useFatsCoLEDs.Checked)
            {
                ApplyStageKitLedPattern(subBeatIndex);
            }

            if ((useStrobe.Checked || useFatsCoStrobe.Checked) && PlayingSong != null)
            {
                try
                {
                    if (newBeat)
                    {
                        //on at beat 1 and 3, otherwise off
                        bool turnOnStrobe = newMeasure || beatIndex % 3 == 0; 

                        var speed = bpm < 80 ? StrobeSpeed.Slow
                                 : bpm < 140 ? StrobeSpeed.Medium
                                 : bpm < 190 ? StrobeSpeed.Faster
                                 : StrobeSpeed.Fastest;

                        if (stageKitToolStripMenuItem.Checked && useStrobe.Checked)
                        {
                            foreach (var stageKit in stageKits)
                            {
                                if (turnOnStrobe)
                                {
                                    stageKit.TurnStrobeOn(speed);
                                }
                                else
                                {
                                    stageKit.TurnStrobeOff();
                                }
                            }
                        }
                        if (enableFatsCoLights.Checked && useFatsCoStrobe.Checked)
                        {
                            foreach (var fatsCo in fatsCoLights)
                            {
                                if (turnOnStrobe)
                                {
                                    switch (speed)
                                    {
                                        case StrobeSpeed.Slow:
                                            fatsCo.StrobeOnSlowest();
                                            break;
                                        case StrobeSpeed.Medium:
                                            fatsCo.StrobeOnMedium();
                                            break;
                                        case StrobeSpeed.Faster:
                                            fatsCo.StrobeOnFast();
                                            break;
                                        case StrobeSpeed.Fastest:
                                            fatsCo.StrobeOnFastest();
                                            break;
                                        default:
                                            fatsCo.StrobeOnMedium();
                                            break;
                                    }
                                }
                                else
                                {
                                    fatsCo.TurnOffStrobe();
                                }
                            }
                        }
                    }
                }
                catch
                { }
            }

            if (useFogger.Checked)
            {
                try
                {
                    if (newMeasure &&
                        songTimeSec >= _nextFogEligible &&
                        Rng.NextDouble() < 0.015)
                    {
                        var fogInterval = Rng.Next(500, 2001);
                        foggerTimer.Interval = fogInterval;

                        _nextFogEligible = songTimeSec + 60 + Rng.Next(0, 21);

                        foreach (var stageKit in stageKits)
                            stageKit.TurnFogOn();

                        foggerTimer.Enabled = true;
                    }
                }
                catch
                {
                }
            }
        }

        private void ApplyStageKitLedPattern(int globalStep)
        {
            int localStep = UpdateStageKitPatternSelection(globalStep);
            int patternStep = localStep * 2;

            StageKitLedFrame frame;

            switch (_currentStageKitPattern)
            {
                case StageKitLedPattern.OneEachAlternatingDirections:
                    frame = BuildPattern_OneEachAlternatingDirections(patternStep);
                    break;

                case StageKitLedPattern.BuildColorsThenReverse:
                    frame = BuildPattern_BuildColorsThenReverse(patternStep);
                    break;

                case StageKitLedPattern.ThreeEachStaggered:
                    frame = BuildPattern_ThreeEachStaggered(patternStep);
                    break;

                case StageKitLedPattern.LaserOpposites:
                    frame = BuildPattern_LaserOpposites(patternStep);
                    break;

                case StageKitLedPattern.OneEachSameDirection:
                default:
                    frame = BuildPattern_OneEachSameDirection(patternStep);
                    break;
            }

            SendStageKitLedFrame(frame);
        }

        private void ResetStageKitPatternState(int currentGlobalStep = 0)
        {
            _currentStageKitPattern = PickRandomStageKitPattern();
            _previousStageKitPattern = _currentStageKitPattern;
            _stageKitPatternStartStep = currentGlobalStep;
            _stageKitPatternLength = GetStageKitPatternLength(_currentStageKitPattern);

        }

        private void SetOne(bool[] bank, int index)
        {
            if (bank == null || bank.Length < 8)
                return;

            bank[index & 7] = true;
        }

        private void SetRun(bool[] bank, int startIndex, int count, int direction = 1)
        {
            if (bank == null || bank.Length < 8)
                return;

            for (int i = 0; i < count; i++)
            {
                int index = direction >= 0
                    ? (startIndex + i) & 7
                    : (startIndex - i) & 7;

                bank[index] = true;
            }
        }

        private StageKitLedFrame BuildPattern_OneEachSameDirection(int step)
        {
            var frame = new StageKitLedFrame();

            int baseIndex = step & 7;

            SetOne(frame.Red, baseIndex + 0);
            SetOne(frame.Blue, baseIndex + 2);
            SetOne(frame.Green, baseIndex + 4);
            SetOne(frame.Yellow, baseIndex + 6);

            return frame;
        }

        private StageKitLedFrame BuildPattern_OneEachAlternatingDirections(int step)
        {
            var frame = new StageKitLedFrame();

            int forward = step & 7;
            int backward = (-step) & 7;

            SetOne(frame.Red, forward + 0);
            SetOne(frame.Blue, backward + 2);
            SetOne(frame.Green, forward + 4);
            SetOne(frame.Yellow, backward + 6);

            return frame;
        }

        private StageKitLedFrame BuildPattern_BuildColorsThenReverse(int step)
        {
            var frame = new StageKitLedFrame();

            const int ledsPerColor = 8;
            const int colors = 4;
            const int buildSteps = ledsPerColor * colors; // 32
            const int cycleSteps = buildSteps * 2;        // 64

            int s = step % cycleSteps;
            if (s < 0) s += cycleSteps;

            bool reversing = s >= buildSteps;

            if (reversing)
                s = cycleSteps - 1 - s;

            // s is now 0..31 for the build state, whether forward or reverse.
            int completedColors = s / ledsPerColor;
            int partialCount = (s % ledsPerColor) + 1;

            // Color order: Red -> Yellow -> Blue -> Green.
            // Fully completed colors stay on.
            if (completedColors > 0)
                SetRun(frame.Red, 0, 8);

            if (completedColors > 1)
                SetRun(frame.Yellow, 0, 8);

            if (completedColors > 2)
                SetRun(frame.Green, 0, 8);

            if (completedColors > 3)
                SetRun(frame.Blue, 0, 8);

            // Current partial color.
            switch (completedColors)
            {
                case 0:
                    SetRun(frame.Red, 0, partialCount);
                    break;

                case 1:
                    SetRun(frame.Red, 0, 8);
                    SetRun(frame.Yellow, 0, partialCount);
                    break;

                case 2:
                    SetRun(frame.Red, 0, 8);
                    SetRun(frame.Yellow, 0, 8);
                    SetRun(frame.Green, 0, partialCount);
                    break;

                case 3:
                    SetRun(frame.Red, 0, 8);
                    SetRun(frame.Yellow, 0, 8);
                    SetRun(frame.Green, 0, 8);
                    SetRun(frame.Blue, 0, partialCount);
                    break;
            }

            return frame;
        }

        private StageKitLedFrame BuildPattern_ThreeEachStaggered(int step)
        {
            var frame = new StageKitLedFrame();

            int baseIndex = step & 7;

            SetRun(frame.Red, baseIndex + 0, 3);
            SetRun(frame.Blue, baseIndex + 2, 3);
            SetRun(frame.Green, baseIndex + 4, 3);
            SetRun(frame.Yellow, baseIndex + 6, 3);

            return frame;
        }

        private void SendStageKitLedFrame(StageKitLedFrame frame)
        {
            if (frame == null)
                return;

            //Stage Kit
            if (stageKits != null && stageKits.Count > 0 && stageKitToolStripMenuItem.Checked && useLEDs.Checked)
            {
                SendColorDiff(
                    CurrentStateRed,
                    frame.Red,
                    (stageKit, index, state) => stageKit.DisplayRedLed(ref ledDisplay, index, state)
                );

                SendColorDiff(
                    CurrentStateBlue,
                    frame.Blue,
                    (stageKit, index, state) => stageKit.DisplayBlueLed(ref ledDisplay, index, state)
                );

                SendColorDiff(
                    CurrentStateGreen,
                    frame.Green,
                    (stageKit, index, state) => stageKit.DisplayGreenLed(ref ledDisplay, index, state)
                );

                SendColorDiff(
                    CurrentStateYellow,
                    frame.Yellow,
                    (stageKit, index, state) => stageKit.DisplayYellowLed(ref ledDisplay, index, state)
                );
            }

            //FatsCo
            if (fatsCoLights != null && fatsCoLights.Count > 0 && enableFatsCoLights.Checked && useFatsCoLEDs.Checked)
            {
                byte redMask = ToLedMask(frame.Red);
                byte blueMask = ToLedMask(frame.Blue);
                byte greenMask = ToLedMask(frame.Green);
                byte yellowMask = ToLedMask(frame.Yellow);

                QueueStageKitCommand(() =>
                {
                    foreach (var fatsCo in fatsCoLights)
                    {
                        fatsCo.SetLedMasks(
                            redMask,
                            blueMask,
                            greenMask,
                            yellowMask);
                    }
                });
            }
        }

        private static byte ToLedMask(bool[] leds)
        {
            if (leds == null)
                return 0x00;

            byte mask = 0x00;

            int count = Math.Min(8, leds.Length);

            for (int i = 0; i < count; i++)
            {
                if (leds[i])
                    mask |= (byte)(1 << i);
            }

            return mask;
        }

        private void SendColorDiff(
            bool[] current,
            bool[] desired,
            Action<StageKitController, int, bool> sendAction)
        {
            if (current == null || desired == null || sendAction == null)
                return;

            for (int i = 0; i < 8; i++)
            {
                if (current[i] == desired[i])
                    continue;

                int ledIndex = i;
                bool newState = desired[i];

                QueueStageKitCommand(() =>
                {
                    foreach (var stageKit in stageKits)
                        sendAction(stageKit, ledIndex, newState);
                });

                current[i] = newState;
            }
        }

        private int GetStageKitPatternLength(StageKitLedPattern pattern)
        {
            const int speedMultiplier = 1;

            switch (pattern)
            {
                case StageKitLedPattern.OneEachSameDirection:
                    return 16 * speedMultiplier; // two full 8-LED rotations

                case StageKitLedPattern.OneEachAlternatingDirections:
                    return 16 * speedMultiplier; // two full rotations/crossovers

                case StageKitLedPattern.BuildColorsThenReverse:
                    return 64 * speedMultiplier; // full build + full reverse

                case StageKitLedPattern.ThreeEachStaggered:
                    return 16 * speedMultiplier; // two full rotations

                default:
                    return 16 * speedMultiplier;

                case StageKitLedPattern.LaserOpposites:
                    return 16 * speedMultiplier;
            }
        }

        private StageKitLedPattern PickRandomStageKitPattern()
        {
            var values = (StageKitLedPattern[])Enum.GetValues(typeof(StageKitLedPattern));

            if (values.Length <= 1)
                return values[0];

            StageKitLedPattern next;

            do
            {
                next = values[Rng.Next(values.Length)];
            }
            while (next == _currentStageKitPattern);

            return next;
        }

        private int UpdateStageKitPatternSelection(int globalStep)
        {
            int localStep = globalStep - _stageKitPatternStartStep;

            if (localStep < 0)
            {
                _stageKitPatternStartStep = globalStep;
                localStep = 0;
            }

            if (localStep >= _stageKitPatternLength)
            {
                _previousStageKitPattern = _currentStageKitPattern;
                _currentStageKitPattern = PickRandomStageKitPattern();

                _stageKitPatternStartStep = globalStep;
                _stageKitPatternLength = GetStageKitPatternLength(_currentStageKitPattern);

                localStep = 0;
            }

            return localStep;
        }

        private void StartStageKitCommandWorker()
        {
            if (_stageKitCommandWorkerRunning)
                return;

            _stageKitCommandWorkerRunning = true;

            _stageKitCommandThread = new Thread(StageKitCommandWorkerLoop);
            _stageKitCommandThread.IsBackground = true;
            _stageKitCommandThread.Name = "Stage Kit Command Worker";
            _stageKitCommandThread.Start();
        }

        private void StopStageKitCommandWorker()
        {
            _stageKitCommandWorkerRunning = false;
            _stageKitCommandSignal.Set();

            if (_stageKitCommandThread != null && _stageKitCommandThread.IsAlive)
                _stageKitCommandThread.Join(500);
        }

        private void QueueStageKitCommand(Action action)
        {
            if (action == null)
                return;

            lock (_stageKitCommandLock)
            {
                _stageKitCommandQueue.Enqueue(action);
            }

            _stageKitCommandSignal.Set();
        }

        private void StageKitCommandWorkerLoop()
        {
            while (_stageKitCommandWorkerRunning)
            {
                _stageKitCommandSignal.WaitOne();

                if (!_stageKitCommandWorkerRunning)
                    break;

                while (true)
                {
                    Action action = null;

                    lock (_stageKitCommandLock)
                    {
                        if (_stageKitCommandQueue.Count > 0)
                            action = _stageKitCommandQueue.Dequeue();
                    }

                    if (action == null)
                        break;

                    try
                    {
                        action();
                    }
                    catch
                    {
                        // Do not let Stage Kit failures affect playback.
                    }
                }
            }
        }

        private void ResetStageKitAnimation()
        {
            ResetStageKitPatternState(0);

            _lastStageKitBeatIndex = -1;
            _lastStageKitSubBeatIndex = -1;
            _lastStageKitMeasureIndex = -1;
            _nextFogEligible = 0;

            redSKIndex = 0;
            blueSKIndex = 1;
            greenSKIndex = 2;
            yellowSKIndex = 3;

            Array.Clear(CurrentStateRed, 0, CurrentStateRed.Length);
            Array.Clear(CurrentStateBlue, 0, CurrentStateBlue.Length);
            Array.Clear(CurrentStateGreen, 0, CurrentStateGreen.Length);
            Array.Clear(CurrentStateYellow, 0, CurrentStateYellow.Length);

            QueueStageKitCommand(() =>
            {
                foreach (var stageKit in stageKits)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        stageKit.DisplayRedLed(ref ledDisplay, i, false);
                        stageKit.DisplayBlueLed(ref ledDisplay, i, false);
                        stageKit.DisplayGreenLed(ref ledDisplay, i, false);
                        stageKit.DisplayYellowLed(ref ledDisplay, i, false);
                    }

                    stageKit.DisplayRedLed(ref ledDisplay, redSKIndex, true);
                    stageKit.DisplayBlueLed(ref ledDisplay, blueSKIndex, true);
                    stageKit.DisplayGreenLed(ref ledDisplay, greenSKIndex, true);
                    stageKit.DisplayYellowLed(ref ledDisplay, yellowSKIndex, true);
                }

                CurrentStateRed[redSKIndex] = true;
                CurrentStateBlue[blueSKIndex] = true;
                CurrentStateGreen[greenSKIndex] = true;
                CurrentStateYellow[yellowSKIndex] = true;
            });
        }

        private void SetRedLedStateOnly(int index, bool state)
        {
            if (index < 0 || index > 7)
                return;

            switch (index)
            {
                case 0: ledDisplay.RedLedArray.Led1 = state; break;
                case 1: ledDisplay.RedLedArray.Led2 = state; break;
                case 2: ledDisplay.RedLedArray.Led3 = state; break;
                case 3: ledDisplay.RedLedArray.Led4 = state; break;
                case 4: ledDisplay.RedLedArray.Led5 = state; break;
                case 5: ledDisplay.RedLedArray.Led6 = state; break;
                case 6: ledDisplay.RedLedArray.Led7 = state; break;
                case 7: ledDisplay.RedLedArray.Led8 = state; break;
            }
        }

        private void SyncVideoToAudio()
        {
            if (_mediaPlayer == null || !_mediaPlayer.IsPlaying)
                return;
            if (!yarg.Checked) return; //only worry about this for YARG/Clone Hero lipsynced videos

            long bassMs = GetBASSTimeForVideo();
            long videoMs = _mediaPlayer.Time + VLCBuffer;
            long driftMs = bassMs - videoMs;

            // Ongoing correction when needed
            if (driftMs > 100)
            {
                _mediaPlayer.Time = bassMs;
            }
            //DEBUG ONLY
            //debugText.Text = "Synced by " + driftMs + "ms";
        }

        private void UpdateActiveRenderingResolution()
        {            
            if (isResizing) return;
            Size currentSize = activeRenderingResolution;
            Size newSize;
            if (secondScreen != null)
            {
                newSize = new Size(secondScreen.Width, secondScreen.Height);
            }
            else
            {
                newSize = isFullScreen ? new Size(Width, Height) : new Size(picVisuals.Width, picVisuals.Height);
            }
            if (currentSize != newSize)
            {
                _rbLaneCacheKey = ""; //force rebuild due to new size
                activeRenderingResolution = newSize;
                videoOverlay.Size = newSize;
            }
        }

        private void PlaybackTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                var mixerState = Bass.BASS_ChannelIsActive(BassMixer);
                if (mixerState != BASSActive.BASS_ACTIVE_PLAYING)
                {
                    if (mixerState != BASSActive.BASS_ACTIVE_STOPPED)
                        return;

                    goto GoToNextSong;
                }

                SyncVideoToAudio();

                long pos = Bass.BASS_ChannelGetPosition(BassStream);
                PlaybackSeconds = Bass.BASS_ChannelBytes2Seconds(BassStream, pos);

                double songLengthSeconds = PlayingSong.Length / 1000.0;
                double timeLeft = songLengthSeconds - PlaybackSeconds;

                bool shouldFade =
                    ((!skipIntroOutroSilence.Checked || OutroSilence == 0.0) && timeLeft <= FadeLength) ||
                    (skipIntroOutroSilence.Checked && OutroSilence > 0.0 && PlaybackSeconds + FadeLength >= OutroSilence);

                if (shouldFade && !AlreadyFading)
                {
                    Bass.BASS_ChannelSlideAttribute(BassMixer, BASSAttribute.BASS_ATTRIB_VOL, 0, (int)(FadeLength * 1000));
                    AlreadyFading = true;
                }

                if (skipIntroOutroSilence.Checked && OutroSilence > 0.0)
                {
                    if (PlaybackSeconds >= OutroSilence)
                        goto GoToNextSong;
                }
                else
                {
                    bool isShuffle = string.Equals(picShuffle.Tag as string, "shuffle", StringComparison.Ordinal);
                    if (PlaybackSeconds * 1000 >= PlayingSong.Length && isShuffle)
                    {
                        PlaybackTimer.Enabled = false;
                        StopPlayback();
                        StopStageKits();

                        if (continuousPlayback.Checked)
                            DoShuffleSongs();

                        return;
                    }
                }

                UpdateTime();
                DoPracticeSessions(GetCorrectedTime());

                bool shouldInvalidatePreview =
                    (displayAlbumArt && File.Exists(CurrentSongArt)) ||
                    (!File.Exists(CurrentSongArt) && !displayAudioSpectrum);

                if (shouldInvalidatePreview)
                {
                    picPreview.Invalidate();
                }

                if (openSideWindow.Checked || secondScreen != null)
                {
                    RenderOverlayFrame();

                    long now = _frameWatch.ElapsedMilliseconds;
                    frameMs = now - _lastFrameTick;
                    _lastFrameTick = now;
                    ShowFPSCounter();

                    return;
                }

                return;
            }
            catch (Exception)
            {
                return;
            }

        GoToNextSong:
            try
            {
                StopStageKits();

                if (!continuousPlayback.Checked)
                {
                    StopPlayback();
                    return;
                }

                PlaybackTimer.Enabled = false;

                string loopMode = picLoop.Tag as string;
                string shuffleMode = picShuffle.Tag as string;

                if (string.Equals(loopMode, "loop", StringComparison.Ordinal))
                {
                    DoLoop();
                    return;
                }

                if (string.Equals(shuffleMode, "shuffle", StringComparison.Ordinal))
                {
                    DoShuffleSongs();
                    return;
                }

                picNext_MouseClick(null, null);
            }
            catch
            { }
        }
                        
        private void RenderOverlayFrame()
        {
            Size scaleSize = activeRenderingResolution; //avoid scaling for now, just render at scale resolution
            Size renderSize = activeRenderingResolution;//seems to have sped up performance quite a bit
            EnsureRenderedFrame(renderSize);

            using (var g = Graphics.FromImage(_renderedFrame))
            {
                g.Clear(doFocusMode ? Color.Black : Color.Transparent);
                UpdateTextQuality(g);

                if (!doFocusMode)
                {
                    if ((doModernKaraokeMode && doAnimatedBackground2) ||
                        (doRockBandKaraoke && doAnimatedBackground))
                    {
                        var backgroundFrame = GetAnimatedKaraokeFrame(
                            new Size(renderSize.Width / 2, renderSize.Height / 2));

                        if (backgroundFrame != null)
                        {
                            g.CompositingMode = CompositingMode.SourceCopy;
                            g.CompositingQuality = CompositingQuality.HighSpeed;
                            g.InterpolationMode = InterpolationMode.Bilinear;
                            g.SmoothingMode = SmoothingMode.None;
                            g.PixelOffsetMode = PixelOffsetMode.Half;

                            g.DrawImage(
                                backgroundFrame,
                                new Rectangle(0, 0, renderSize.Width, renderSize.Height),
                                0,
                                0,
                                backgroundFrame.Width,
                                backgroundFrame.Height,
                                GraphicsUnit.Pixel);

                            // Restore for foreground drawing
                            g.CompositingMode = CompositingMode.SourceOver;
                            UpdateTextQuality(g);
                        }
                    }
                    else if (doAnimatedSpectrum && !DoYargVideo())
                    {
                        DrawFFTWaveform(g, new Rectangle(0, 0, renderSize.Width, renderSize.Height));
                    }
                    else
                    {
                        if (doRockBandChart && !changedBackground && !DoYargVideo())
                        {
                            ChangeRBStyleBackground();
                            changedBackground = true;
                        }

                        if (doRockBandChart &&
                            doUseBackgroundImages &&
                            RBStyleBackgroundScaled != null &&
                            !DoYargVideo())
                        {
                            if (RBStyleBackgroundScaled.Size != activeRenderingResolution)
                            {
                                //should already be scaled but isn't, let's fix it
                                RBStyleBackgroundScaled = ScaleBackgroundImage(RBStyleBackgroundScaled);
                            }
                            g.DrawImageUnscaled(RBStyleBackgroundScaled, 0, 0);                            
                        }
                    }
                }

                g.CompositingMode = CompositingMode.SourceOver;
                UpdateTextQuality(g);

                RenderVisuals(renderSize, g);
            }

            Bitmap frameToDisplay;

            if (scaleSize != renderSize)
            {
                frameToDisplay = ScaleBackgroundImage(_renderedFrame);
            }
            else
            {
                // No scaling needed
                frameToDisplay = _renderedFrame;
            }            

            if (secondScreen != null)
            {
                if ((doUseBackgroundVideos || DoYargVideo()) && !doFocusMode)
                {
                    if (!secondScreen.videoOverlay.Visible)
                    {
                        secondScreen.videoOverlay.Visible = true;
                    }
                    secondScreen.videoOverlay.UpdateVisuals(frameToDisplay);
                    if (!ReferenceEquals(picVisuals.Image, Resources.logo))
                    {
                        SafeVisualsSetter(Resources.logo);
                    }
                }
                else
                {
                    if (secondScreen.videoOverlay.Visible)
                    {
                        secondScreen.videoOverlay.Visible = false;
                    }
                    secondScreen.ChangeVisualsImage(frameToDisplay);
                }
                secondScreen.InvalidateVisuals();
            }
            else
            {
                if ((doUseBackgroundVideos || DoYargVideo()) && !doFocusMode)
                {
                    if (!videoOverlay.Visible)
                    {
                        videoOverlay.Visible = true;
                    }
                    videoOverlay.UpdateVisuals(frameToDisplay);
                }
                else
                {
                    if (videoOverlay.Visible)
                    {
                        videoOverlay.Visible = false;
                    }                    
                    if (!ReferenceEquals(picVisuals.Image, frameToDisplay))
                    {
                        SafeVisualsSetter(frameToDisplay);
                    }                    
                }
                picVisuals.Invalidate();
            }
        }

        private void SafeVisualsSetter(Image newImage)
        {
            var pb = picVisuals;
            if (pb.InvokeRequired)
            {
                pb.BeginInvoke(new Action(() => SafeVisualsSetter(newImage)));
                return;
            }

            if (newImage == null)
            {
                picVisuals.Image = null;
                return;
            }

            try
            {
                int w = newImage.Width;
                int h = newImage.Height;

                if (w <= 0 || h <= 0)
                    return;
            }
            catch
            {
                newImage.Dispose();
                return;
            }

            try
            {
                Image oldImage = pb.Image;

                pb.Image = newImage;

                if (oldImage != null && !ReferenceEquals(oldImage, _renderedFrame) && !ReferenceEquals(oldImage, _scaledFrame))
                {
                    oldImage.Dispose();
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Error setting visual:\n\n" + ex.Message + "\n\nStack Trace:\n\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }    
        }

        private void ClearRBKaraokeAnimatedFrameCache()
        {
            if (_cachedRBKaraokeAnimatedFrames != null)
            {
                foreach (Bitmap bmp in _cachedRBKaraokeAnimatedFrames)
                    bmp?.Dispose();

                _cachedRBKaraokeAnimatedFrames.Clear();
            }

            _cachedRBKaraokeAnimatedFrames = null;
            _cachedRBKaraokeAnimatedFrameSize = Size.Empty;
            _cachedRBKaraokeAnimatedSourceCount = -1;
            _cachedRBKaraokeAnimatedSourceRef = null;
        }

        private void EnsureRBKaraokeAnimatedFrameCache(Size size)
        {
            if (stageFrames == null || stageFrames.Count == 0)
                return;

            bool needsRebuild =
                _cachedRBKaraokeAnimatedFrames == null ||
                _cachedRBKaraokeAnimatedFrameSize != size ||
                _cachedRBKaraokeAnimatedSourceCount != stageFrames.Count ||
                !ReferenceEquals(_cachedRBKaraokeAnimatedSourceRef, stageFrames);

            if (!needsRebuild)
                return;

            ClearRBKaraokeAnimatedFrameCache();

            _cachedRBKaraokeAnimatedFrames = new List<Bitmap>(stageFrames.Count);

            for (int i = 0; i < stageFrames.Count; i++)
            {
                Image source = stageFrames[i];

                if (source == null)
                    continue;

                Bitmap resized = new Bitmap(
                    size.Width,
                    size.Height, PixelFormat.Format32bppPArgb);

                using (Graphics g = Graphics.FromImage(resized))
                {
                    g.Clear(Color.Black);

                    g.CompositingMode = CompositingMode.SourceOver;
                    g.CompositingQuality = CompositingQuality.HighSpeed;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.SmoothingMode = SmoothingMode.HighSpeed;

                    g.DrawImage(source, 0, 0, size.Width, size.Height);
                }

                _cachedRBKaraokeAnimatedFrames.Add(resized);
            }

            _cachedRBKaraokeAnimatedFrameSize = size;
            _cachedRBKaraokeAnimatedSourceCount = stageFrames.Count;
            _cachedRBKaraokeAnimatedSourceRef = stageFrames;
        }
                
        private Image GetAnimatedKaraokeFrame(Size targetSize)
        {
            bool animated =
                (doRockBandKaraoke && doAnimatedBackground) ||
                (doModernKaraokeMode && doAnimatedBackground2);

            if (!animated)
                return null;

            if (targetSize.Width <= 0 || targetSize.Height <= 0)
                return null;

            if (stageFrames == null || stageFrames.Count == 0)
            {
                return doRockBandKaraoke ? stageBackground : null;
            }

            EnsureRBKaraokeAnimatedFrameCache(targetSize);

            if (_cachedRBKaraokeAnimatedFrames == null || _cachedRBKaraokeAnimatedFrames.Count == 0)
                return null;

            if (stageCounter < 0 || stageCounter >= _cachedRBKaraokeAnimatedFrames.Count)
                stageCounter = 0;

            Image frame = _cachedRBKaraokeAnimatedFrames[stageCounter++];

            if (stageCounter >= _cachedRBKaraokeAnimatedFrames.Count)
                stageCounter = 0;

            return frame;
        }
        
        private void ShowFPSCounter()
        {
            if (!enableDebugFPS.Checked) return;
            _fpsFrameCount++;

            if (_fpsWatch.ElapsedMilliseconds >= 1000)
            {
                _currentFps = _fpsFrameCount;
                _fpsFrameCount = 0;
                _fpsWatch.Restart();

                int scaleX = secondScreen != null ? secondScreen.Width : picVisuals.Width;
                int scaleY = secondScreen != null ? secondScreen.Height : picVisuals.Height;

                debugText.Text = $"DEBUG [Render: {activeRenderingResolution.Width}x{activeRenderingResolution.Height} | Scale: {scaleX}x{scaleY} | BPM: {PlayingSong.BPM} | Actual FPS: {_currentFps} | Frame: {frameMs} ms | Timer Interval: {PlaybackTimer.Interval}]";

                if (_currentFps >= 30)
                {
                    lblFPS.ForeColor = Color.LimeGreen;
                }
                else if (_currentFps >= 20 && _currentFps < 30)
                {
                    lblFPS.ForeColor = Color.Orange;
                }
                else
                {
                    lblFPS.ForeColor = Color.Red;
                }
                lblFPS.Text = _currentFps.ToString();
            }

            if (_currentFps != _lastFps)
            {
                Debug.WriteLine($"CURRENT FPS: {_currentFps}");
                _lastFps = _currentFps;
            }
        }

        private long _lastFps = 0;

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
            _ = StartPlaybackAsync(true, false);
        }
                
        private void DoPracticeSessions(double time)
        {
            bool hasPracticeSections = MIDITools.PracticeSessions != null && MIDITools.PracticeSessions.Count > 0;
            bool shouldShow = showPracticeSections.Checked && hasPracticeSections && !doVerticalChart;

            if (_lastPracticeSectionVisible != shouldShow)
            {
                lblSections.Visible = shouldShow;
                _lastPracticeSectionVisible = shouldShow;
            }

            if (!openSideWindow.Checked)
                return;

            if (!showPracticeSections.Checked || !hasPracticeSections)
            {
                if (_lastPracticeSectionText != "")
                {
                    lblSections.Text = "";
                    _lastPracticeSectionText = "";
                }

                _lastPracticeSectionIndex = -1;
                return;
            }

            string currentSection = GetCurrentSectionFast(time);

            if (!string.Equals(_lastPracticeSectionText, currentSection, StringComparison.Ordinal))
            {
                lblSections.Text = currentSection;
                _lastPracticeSectionText = currentSection;
            }
        }
        
        private string GetCurrentSectionFast(double time)
        {
            if (MIDITools.PracticeSessions == null || MIDITools.PracticeSessions.Count == 0)
                return "";

            if (_lastPracticeSectionIndex < 0)
                _lastPracticeSectionIndex = 0;

            // Move forward while time has passed the next section start
            while (_lastPracticeSectionIndex + 1 < MIDITools.PracticeSessions.Count &&
                   MIDITools.PracticeSessions[_lastPracticeSectionIndex + 1].SectionStart <= time)
            {
                _lastPracticeSectionIndex++;
            }

            // If user seeks backward, walk back
            while (_lastPracticeSectionIndex > 0 &&
                   MIDITools.PracticeSessions[_lastPracticeSectionIndex].SectionStart > time)
            {
                _lastPracticeSectionIndex--;
            }

            if (MIDITools.PracticeSessions[_lastPracticeSectionIndex].SectionStart <= time)
                return MIDITools.PracticeSessions[_lastPracticeSectionIndex].SectionName ?? "";

            return "";
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

        private void DrawFillsRB(Graphics graphics, MIDITrack instrument, int posY, int posX, int track_width, bool isProKeys = false)
        {
            if ((instrument.Fills == null || instrument.Fills.Count == 0) &&
                (instrument.Overdrive == null || instrument.Overdrive.Count == 0))
                return;

            var renderSize = activeRenderingResolution;//new Size(1920, 1080);
            var correctedTime = GetCorrectedTime();

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
                    GetCorrectedTime(),
                    posX,
                    track_width,
                    fillColor,
                    horizonY,
                    hitboxY,
                    PlaybackWindowRB,
                    HighwayAngleFactor,
                    1.0,
                    depthPower,
                    overshootPx,
                    isProKeys
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
                    GetCorrectedTime(),
                    posX,
                    track_width,
                    fillColor,
                    horizonY,
                    hitboxY,
                    PlaybackWindowRB,
                    HighwayAngleFactor,
                    1.0,
                    depthPower,
                    overshootPx,
                    isProKeys
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
            float overshootPx,
            bool isProKeys
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

            // Clamp the fill so it never renders below the hitbox
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

            var proKeysPadding = isProKeys ? 5 : 0;

            // Build trapezoid polygon
            var pts = new[]
            {
                new PointF(left0 - proKeysPadding,  y0),
                new PointF(right0 + proKeysPadding, y0),
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
            var renderSize = activeRenderingResolution;

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

        private void DrawProKeysSustainTail(
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

            // Correct inverse for YFromP01: linear
            double P01FromY(float y)
            {
                return PFromY != null ? PFromY(y) : Clamp01((y - horizonY) / (hitboxY - horizonY));
            }

            double LaneCenterAtY(float y, out double laneW)
            {
                double p = P01FromY(y);
                double scale = Lerp(minScale, maxScale, p);
                double span = trackWidth * scale;

                // Keep the original tail width behavior exactly the same.
                laneW = span / lanes;

                // But center the tail on the same X position used by traveling Pro Keys notes.
                return GetProKeyCenterX(note.NoteNumber, trackCenterX, span);
            }

            // Pro keys: skinny tail; tie it to lane width at that Y
            float TailWidthFromLaneW(double laneW)
            {
                return (float)Math.Max(2.0, Math.Min(4.0, laneW * HighwayAngleFactor));
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

            // NOW build glow from the actual points
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

        private struct ProKeyLayout
        {
            public bool IsSharp;
            public int WhiteIndex;
            public float CenterWhiteUnits;
        }

        private ProKeyLayout GetProKeyLayout(int midiNote)
        {
            int keyIndex = midiNote - 48; // Rock Band Pro Keys: C3..C5

            switch (keyIndex)
            {
                case 0: return new ProKeyLayout { IsSharp = false, WhiteIndex = 0, CenterWhiteUnits = 0.5f }; // C
                case 1: return new ProKeyLayout { IsSharp = true, WhiteIndex = -1, CenterWhiteUnits = 1.0f }; // C#
                case 2: return new ProKeyLayout { IsSharp = false, WhiteIndex = 1, CenterWhiteUnits = 1.5f }; // D
                case 3: return new ProKeyLayout { IsSharp = true, WhiteIndex = -1, CenterWhiteUnits = 2.0f }; // D#
                case 4: return new ProKeyLayout { IsSharp = false, WhiteIndex = 2, CenterWhiteUnits = 2.5f }; // E

                case 5: return new ProKeyLayout { IsSharp = false, WhiteIndex = 3, CenterWhiteUnits = 3.5f }; // F
                case 6: return new ProKeyLayout { IsSharp = true, WhiteIndex = -1, CenterWhiteUnits = 4.0f }; // F#
                case 7: return new ProKeyLayout { IsSharp = false, WhiteIndex = 4, CenterWhiteUnits = 4.5f }; // G
                case 8: return new ProKeyLayout { IsSharp = true, WhiteIndex = -1, CenterWhiteUnits = 5.0f }; // G#
                case 9: return new ProKeyLayout { IsSharp = false, WhiteIndex = 5, CenterWhiteUnits = 5.5f }; // A
                case 10: return new ProKeyLayout { IsSharp = true, WhiteIndex = -1, CenterWhiteUnits = 6.0f }; // A#
                case 11: return new ProKeyLayout { IsSharp = false, WhiteIndex = 6, CenterWhiteUnits = 6.5f }; // B

                case 12: return new ProKeyLayout { IsSharp = false, WhiteIndex = 7, CenterWhiteUnits = 7.5f }; // C
                case 13: return new ProKeyLayout { IsSharp = true, WhiteIndex = -1, CenterWhiteUnits = 8.0f }; // C#
                case 14: return new ProKeyLayout { IsSharp = false, WhiteIndex = 8, CenterWhiteUnits = 8.5f }; // D
                case 15: return new ProKeyLayout { IsSharp = true, WhiteIndex = -1, CenterWhiteUnits = 9.0f }; // D#
                case 16: return new ProKeyLayout { IsSharp = false, WhiteIndex = 9, CenterWhiteUnits = 9.5f }; // E

                case 17: return new ProKeyLayout { IsSharp = false, WhiteIndex = 10, CenterWhiteUnits = 10.5f }; // F
                case 18: return new ProKeyLayout { IsSharp = true, WhiteIndex = -1, CenterWhiteUnits = 11.0f }; // F#
                case 19: return new ProKeyLayout { IsSharp = false, WhiteIndex = 11, CenterWhiteUnits = 11.5f }; // G
                case 20: return new ProKeyLayout { IsSharp = true, WhiteIndex = -1, CenterWhiteUnits = 12.0f }; // G#
                case 21: return new ProKeyLayout { IsSharp = false, WhiteIndex = 12, CenterWhiteUnits = 12.5f }; // A
                case 22: return new ProKeyLayout { IsSharp = true, WhiteIndex = -1, CenterWhiteUnits = 13.0f }; // A#
                case 23: return new ProKeyLayout { IsSharp = false, WhiteIndex = 13, CenterWhiteUnits = 13.5f }; // B

                case 24: return new ProKeyLayout { IsSharp = false, WhiteIndex = 14, CenterWhiteUnits = 14.5f }; // C
            }

            return new ProKeyLayout { IsSharp = false, WhiteIndex = 0, CenterWhiteUnits = 0.5f };
        }

        private float GetProKeyCenterX(int midiNote, float trackCenterX, double spanWidth)
        {
            ProKeyLayout layout = GetProKeyLayout(midiNote);

            double spanLeft = trackCenterX - (spanWidth / 2.0);
            double whiteKeyW = spanWidth / 15.0;

            return (float)(spanLeft + (whiteKeyW * layout.CenterWhiteUnits));
        }

        private RectangleF GetProKeyTravelRect(
            int midiNote,
            float trackCenterX,
            double spanHead,
            float centerY,
            Image img)
        {
            float centerX = GetProKeyCenterX(midiNote, trackCenterX, spanHead);

            // Regular falling notes: white and black notes stay the same size.
            double noteW = spanHead / 25.0;

            // Make them about 10-15% larger while preserving perspective.
            noteW *= 1.12;

            double noteH = img.Height * (noteW / img.Width);

            return new RectangleF(
                (float)(centerX - noteW / 2.0),
                centerY - (float)(noteH / 2.0),
                (float)noteW,
                (float)noteH
            );
        }

        private RectangleF GetProKeyGlowRect(
            int midiNote,
            float trackCenterX,
            int trackWidth,
            float hitboxY,
            Image glowImg)
        {
            // Measured total usable keyboard width from source layout:
            // C3 starts at 0.000 and C5 ends at 28.000 + 1.625 = 29.625
            const float sourceKeyboardWidth = 29.458f;

            ProKeyGlowAnchor anchor = GetProKeyGlowAnchor(midiNote);

            float chartLeft = trackCenterX - (trackWidth / 2f);
            float scaleX = trackWidth / sourceKeyboardWidth;

            float x = chartLeft + (anchor.X * scaleX);
            float w = anchor.W * scaleX;
                        
            float h = (float)(glowImg.Height * (w / glowImg.Width));
            h *= 0.50f;

            const float glowYOffset = -6f;

            return new RectangleF(
                x,
                hitboxY + glowYOffset,
                w,
                h
            );
        }

        private void DrawProKeysNotesRB(Graphics graphics, int startingPosition, int chartLeft, int trackWidth)
        {
            var track = MIDITools.MIDI_Chart.ProKeys;
            var notes = track.ChartedNotes;
            int noteCount = notes.Count;
            if (noteCount == 0) return;

            var renderSize = activeRenderingResolution;
            double correctedTime = GetCorrectedTime();

            const double passedWindow = 0.025;
            const double hitTimeWindow = 0.25;
            const float hitWindowPx = 20f;

            const double minScale = HighwayAngleFactor;
            const double maxScale = 1.00;

            float hitboxY = renderSize.Height - 50f;
            float horizonY = startingPosition + ((hitboxY - startingPosition) * horizonPercent);

            DrawBeatLines(
                graphics,
                correctedTime,
                horizonY,
                hitboxY,
                overshootPx,
                chartLeft,
                trackWidth,
                PlaybackWindowRB,
                minScale,
                maxScale,
                depthPower
            );

            double ClampMin0(double v) => v < 0 ? 0 : v;
            double EaseIn(double t) => Math.Pow(t, depthPower);
            double Lerp(double a, double b, double t) => a + (b - a) * t;

            double PFromY_Notes(float y)
            {
                double bottomY = hitboxY + overshootPx;
                double spanY = bottomY - horizonY;
                if (spanY <= 1) return 1.0;

                double p = (y - horizonY) / spanY;
                if (p < 0) p = 0;
                else if (p > 1) p = 1;

                return p;
            }

            float trackCenterX = chartLeft + (trackWidth / 2f);
            double visibleEnd = correctedTime + PlaybackWindowRB;

            int startIndex = track.ActiveIndex;
            if (startIndex < 0 || startIndex >= noteCount)
                startIndex = 0;

            while (startIndex > 0 && notes[startIndex].NoteStart > correctedTime - 0.25)
                startIndex--;

            while (startIndex < noteCount &&
                   (notes[startIndex].NoteStart + notes[startIndex].NoteLength) < correctedTime - 0.25)
            {
                startIndex++;
            }

            while (startIndex > 0 &&
                   Math.Abs(notes[startIndex - 1].NoteStart - notes[startIndex].NoteStart) < 0.0001)
            {
                startIndex--;
            }

            // ============================================================
            // Pass 1: sustains
            // ============================================================
            int sustainIndex = startIndex;

            while (sustainIndex > 0 &&
                   (notes[sustainIndex - 1].NoteStart + notes[sustainIndex - 1].NoteLength) >= correctedTime - 0.25)
            {
                sustainIndex--;
            }

            for (int s = sustainIndex; s < noteCount; s++)
            {
                var note = notes[s];

                if (note.NoteStart > visibleEnd)
                    break;

                if (note.NoteLength < 1)
                    continue;

                double noteEnd = note.NoteStart + note.NoteLength;

                if (noteEnd < correctedTime - 0.05)
                    continue;

                if (note.NoteStart > visibleEnd)
                    continue;

                if (note.NoteColor == Color.Empty)
                    note.NoteColor = GetNoteColor(note.NoteNumber);

                int keyIndex = note.NoteNumber - 48;
                bool isSharp = note.NoteName != null && note.NoteName.Contains("#");
                Color tailColor = note.hasOD ? Color.LightGoldenrodYellow : (isSharp ? Color.Black : Color.White);

                double tHead = 1.0 - ((note.NoteStart - correctedTime) / PlaybackWindowRB);
                tHead = ClampMin0(tHead);

                double pHeadRaw = EaseIn(tHead);
                double pHeadDraw = Math.Min(1.0, pHeadRaw);

                double scaleHead = Lerp(minScale, maxScale, pHeadDraw);
                double spanHead = trackWidth * scaleHead;

                double keyWidth = spanHead / 25.0;
                double noteHeight = keyWidth;

                float posY = (float)Lerp(horizonY, hitboxY + overshootPx, pHeadDraw);

                bool sustainHeadIsAtOrPastHitbox =
                    correctedTime >= note.NoteStart &&
                    correctedTime <= noteEnd;

                float tailAnchorY = sustainHeadIsAtOrPastHitbox
                    ? hitboxY
                    : posY - (float)(noteHeight / 2.0);

                DrawProKeysSustainTail(
                    graphics,
                    note,
                    tailColor,
                    correctedTime,
                    keyIndex,
                    chartLeft,
                    trackWidth,
                    horizonY,
                    hitboxY,
                    minScale,
                    maxScale,
                    depthPower,
                    tailAnchorY,
                    PFromY_Notes,
                    isSharp
                );
            }

            // ============================================================
            // Pass 2: note heads / glow notes / chord markers
            // ============================================================
            int i = startIndex;

            while (i < noteCount)
            {
                var firstNote = notes[i];
                double startTime = firstNote.NoteStart;

                if (startTime > visibleEnd)
                    break;

                int chordStart = i;
                int chordEnd = i + 1;

                while (chordEnd < noteCount &&
                       Math.Abs(notes[chordEnd].NoteStart - startTime) < 0.0001)
                {
                    chordEnd++;
                }

                double tHead = 1.0 - ((startTime - correctedTime) / PlaybackWindowRB);
                tHead = ClampMin0(tHead);

                double pHeadRaw = EaseIn(tHead);
                double pHeadDraw = Math.Min(1.0, pHeadRaw);

                double scaleHead = Lerp(minScale, maxScale, pHeadDraw);
                double spanHead = trackWidth * scaleHead;

                float posY = (float)Lerp(horizonY, hitboxY + overshootPx, pHeadDraw);

                // A "chord" group may be one note or multiple notes.
                // This keeps glow notes pinned at the hitbox for active sustains.
                bool groupHasActiveSustain = false;

                for (int c = chordStart; c < chordEnd; c++)
                {
                    var sustainCheckNote = notes[c];

                    if (sustainCheckNote.NoteLength >= 1)
                    {
                        double sustainEnd = sustainCheckNote.NoteStart + sustainCheckNote.NoteLength;

                        if (correctedTime >= sustainCheckNote.NoteStart &&
                            correctedTime <= sustainEnd)
                        {
                            groupHasActiveSustain = true;
                            break;
                        }
                    }
                }

                // This is the normal "note just reached the hitbox" window.
                // Use this for the colored shoot so the shoot does NOT stay on for the whole sustain.
                bool groupShouldDrawShoot =
                    correctedTime >= startTime - hitTimeWindow &&
                    correctedTime <= startTime + hitTimeWindow &&
                    posY >= hitboxY - hitWindowPx;

                // This controls whether the note head itself becomes/stays a glow note.
                bool chordIsAtHitbox = groupShouldDrawShoot || groupHasActiveSustain;

                float drawCenterY = chordIsAtHitbox ? hitboxY : posY;

                if (drawCenterY > hitboxY + overshootPx)
                {
                    i = chordEnd;
                    continue;
                }

                if (!chordIsAtHitbox && correctedTime > startTime + passedWindow)
                {
                    LastPlayedIndexHackForProKeys(track, chordStart);
                    i = chordEnd;
                    continue;
                }

                // Chord marker bounds use actual pro-keys center mapping,
                // but normal traveling note width.
                double chordLeft = double.MaxValue;
                double chordRight = double.MinValue;
                double regularNoteW = spanHead / 25.0;

                for (int c = chordStart; c < chordEnd; c++)
                {
                    var n = notes[c];
                    float centerX = GetProKeyCenterX(n.NoteNumber, trackCenterX, spanHead);

                    double left = centerX - (regularNoteW / 2.0);
                    double right = centerX + (regularNoteW / 2.0);

                    chordLeft = Math.Min(chordLeft, left);
                    chordRight = Math.Max(chordRight, right);
                }

                double chordPadding = regularNoteW * 0.20;
                chordLeft -= chordPadding;
                chordRight += chordPadding;

                if (!chordIsAtHitbox && chordEnd - chordStart > 1 && drawCenterY <= hitboxY)
                {
                    double chordW = chordRight - chordLeft;

                    if (chordW > 1)
                    {
                        graphics.DrawImage(
                            bmpProKeysChordMarker,
                            (float)chordLeft,
                            drawCenterY - 4,
                            (float)chordW,
                            12
                        );
                    }
                }

                for (int c = chordStart; c < chordEnd; c++)
                {
                    var note = notes[c];

                    if (note.NoteColor == Color.Empty)
                        note.NoteColor = GetNoteColor(note.NoteNumber);

                    bool isSharp = note.NoteName != null && note.NoteName.Contains("#");

                    double noteEnd = note.NoteStart + note.NoteLength;

                    bool noteHasActiveSustain =
                        note.NoteLength >= 1 &&
                        correctedTime >= note.NoteStart &&
                        correctedTime <= noteEnd;

                    // This is now per-note, not only per-chord/group.
                    // Tap notes glow during the hit window.
                    // Sustain notes keep their own glow pinned while their sustain is active.
                    bool noteIsAtHitbox = groupShouldDrawShoot || noteHasActiveSustain;

                    Image img;

                    if (noteIsAtHitbox)
                    {
                        img = GetProKeysGlowNote(note.NoteName);
                    }
                    else
                    {
                        if (isSharp)
                            img = note.hasOD ? bmpProKeysNoteBlackOD : bmpProKeysNoteBlack;
                        else
                            img = note.hasOD ? bmpProKeysNoteWhiteOD : bmpProKeysNoteWhite;
                    }

                    RectangleF rect;

                    if (noteIsAtHitbox)
                    {
                        rect = GetProKeyGlowRect(
                            note.NoteNumber,
                            trackCenterX,
                            trackWidth,
                            hitboxY,
                            img
                        );

                        // Shoot still only happens at the initial hit moment,
                        // not during the whole sustain.
                        if (groupShouldDrawShoot)
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
                                alpha01: 0.25f,
                                topCutPct: 0f
                            );
                        }
                    }
                    else
                    {
                        rect = GetProKeyTravelRect(
                            note.NoteNumber,
                            trackCenterX,
                            spanHead,
                            drawCenterY,
                            img
                        );
                    }

                    graphics.DrawImage(img, rect);
                }

                LastPlayedIndexHackForProKeys(track, chordStart);
                i = chordEnd;
            }
        }

        private struct ProKeyGlowAnchor
        {
            public float X;
            public float W;
        }

        private ProKeyGlowAnchor GetProKeyGlowAnchor(int midiNote)
        {
            switch (midiNote)
            {
                // White keys
                case 48: return new ProKeyGlowAnchor { X = 0.000f, W = 2.000f }; // C3
                case 50: return new ProKeyGlowAnchor { X = 2.000f, W = 2.000f }; // D3
                case 52: return new ProKeyGlowAnchor { X = 4.000f, W = 2.000f }; // E3
                case 53: return new ProKeyGlowAnchor { X = 6.000f, W = 2.000f }; // F3
                case 55: return new ProKeyGlowAnchor { X = 8.000f, W = 2.000f }; // G3
                case 57: return new ProKeyGlowAnchor { X = 10.000f, W = 2.000f }; // A3
                case 59: return new ProKeyGlowAnchor { X = 12.000f, W = 2.000f }; // B3
                case 60: return new ProKeyGlowAnchor { X = 14.000f, W = 2.000f }; // C4
                case 62: return new ProKeyGlowAnchor { X = 16.000f, W = 2.000f }; // D4
                case 64: return new ProKeyGlowAnchor { X = 18.000f, W = 2.000f }; // E4
                case 65: return new ProKeyGlowAnchor { X = 19.900f, W = 2.000f }; // F4
                case 67: return new ProKeyGlowAnchor { X = 21.900f, W = 2.000f }; // G4
                case 69: return new ProKeyGlowAnchor { X = 23.900f, W = 2.000f }; // A4
                case 71: return new ProKeyGlowAnchor { X = 25.900f, W = 2.000f }; // B4
                case 72: return new ProKeyGlowAnchor { X = 27.900f, W = 1.625f }; // C5

                // Black keys
                case 49: return new ProKeyGlowAnchor { X = 1.361f, W = 1.181f }; // C#3
                case 51: return new ProKeyGlowAnchor { X = 3.444f, W = 1.181f }; // D#3
                case 54: return new ProKeyGlowAnchor { X = 7.306f, W = 1.181f }; // F#3
                case 56: return new ProKeyGlowAnchor { X = 9.347f, W = 1.181f }; // G#3
                case 58: return new ProKeyGlowAnchor { X = 11.444f, W = 1.181f }; // A#3
                case 61: return new ProKeyGlowAnchor { X = 15.292f, W = 1.181f }; // C#4
                case 63: return new ProKeyGlowAnchor { X = 17.472f, W = 1.181f }; // D#4
                case 66: return new ProKeyGlowAnchor { X = 21.264f, W = 1.181f }; // F#4
                case 68: return new ProKeyGlowAnchor { X = 23.292f, W = 1.181f }; // G#4
                case 70: return new ProKeyGlowAnchor { X = 25.319f, W = 1.181f }; // A#4
            }

            return new ProKeyGlowAnchor { X = 0f, W = 1f };
        }

        private Bitmap GetProKeysGlowNote(string noteName)
        {
            switch (noteName)
            {
                // White keys - left edge of 2-black-key group / 3-black-key group
                case "C4":
                case "F4":
                case "C5":
                case "F5":
                    return bmpProKeysWhiteLeftGlow;

                // White keys - right edge before next C/F group
                case "E4":
                case "B4":
                case "E5":
                case "B5":
                    return bmpProKeysWhiteRightGlow;

                // White keys - center-ish
                case "D4":
                case "G4":
                case "A4":
                case "D5":
                case "G5":
                case "A5":
                    return bmpProKeysWhiteCenterGlow;

                // Black keys, first octave
                case "C#4":
                case "D#4":
                    return bmpProKeysRedGlow;

                case "F#4":
                case "G#4":
                case "A#4":
                    return bmpProKeysYellowGlow;

                // Black keys, second octave
                case "C#5":
                case "D#5":
                    return bmpProKeysBlueGlow;

                case "F#5":
                case "G#5":
                case "A#5":
                    return bmpProKeysGreenGlow;
            }

            return bmpProKeysWhiteFullGlow;
        }

        private static void LastPlayedIndexHackForProKeys(MIDITrack track, int index)
        {
            track.ActiveIndex = index;
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
            // Map note -> key index
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

            // Convert y -> p (linear) so span matches borders math
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

            // Slightly inset so it doesn't overlap border rails
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

        private double GetVisualSustainLength(double rawLength)
        {
            const double sustainVisualLengthScale = 0.95;
            const double shortSustainThreshold = 0.50;

            return rawLength < shortSustainThreshold
                ? rawLength
                : rawLength * sustainVisualLengthScale;
        }

        private void DrawProKeysNotes(Graphics graphics, int startingPosition, int ChartLeft, int trackWidth)
        {
            if (MIDITools.MIDI_Chart.ProKeys.ChartedNotes.Count == 0)
                return;

            var renderSize = activeRenderingResolution;
            var correctedTime = GetCorrectedTime();

            ChartGoal = renderSize.Height - startingPosition - 50;

            double hitboxY = renderSize.Height - 50;
            double noteWidth = trackWidth / 25.0;
            const double minSustainToDraw = 1.0;
            const double sustainGrace = 0.05;

            Color tailColor;

            // Keep only notes that could still be visible or still have an active sustain.
            var filteredNotes = MIDITools.MIDI_Chart.ProKeys.ChartedNotes
                .Where(note =>
                {
                    double visualLength = GetVisualSustainLength(note.NoteLength);
                    double visualEndTime = note.NoteStart + visualLength;

                    return note.NoteStart <= correctedTime + PlaybackWindowRB &&
                           correctedTime <= visualEndTime + sustainGrace;
                })
                .ToList();

            var groupedNotes = filteredNotes.GroupBy(note => note.NoteStart);

            foreach (var chord in groupedNotes)
            {
                var chordNotes = chord.ToList();

                double chordLeft = double.MaxValue;
                double chordRight = double.MinValue;
                double chordPosY = 0;

                foreach (var note in chordNotes)
                {
                    double percent = 1.0 - ((note.NoteStart - correctedTime) / PlaybackWindowRB);
                    chordPosY = startingPosition + (ChartGoal * percent);

                    int noteLocation = note.NoteNumber - 48;
                    double posX = ChartLeft + (noteWidth * noteLocation);

                    chordLeft = Math.Min(chordLeft, posX - 10);
                    chordRight = Math.Max(chordRight, posX + noteWidth + 10);
                }

                if (chordNotes.Count > 1 && chordPosY <= hitboxY)
                {
                    var chordWidth = chordRight - chordLeft;
                    graphics.DrawImage(
                        bmpProKeysChordMarker,
                        (float)chordLeft,
                        (float)chordPosY - 4,
                        (float)chordWidth,
                        12);
                }

                foreach (var note in chordNotes)
                {
                    if (note.NoteColor == Color.Empty)
                    {
                        note.NoteColor = GetNoteColor(note.NoteNumber);
                    }

                    bool hasSustain = note.NoteLength >= minSustainToDraw;

                    double visualLength = GetVisualSustainLength(note.NoteLength);
                    double visualEndTime = note.NoteStart + visualLength;

                    bool sustainActive =
                        hasSustain &&
                        correctedTime >= note.NoteStart &&
                        correctedTime <= visualEndTime;

                    double percent = 1.0 - ((note.NoteStart - correctedTime) / PlaybackWindowRB);
                    double posY = startingPosition + (ChartGoal * percent);

                    int noteLocation = note.NoteNumber - 48;
                    double posX = ChartLeft + (noteWidth * noteLocation);

                    var img = note.NoteName.Contains("#")
                        ? (note.hasOD ? bmpProKeysNoteBlackOD : bmpProKeysNoteBlack)
                        : (note.hasOD ? bmpProKeysNoteWhiteOD : bmpProKeysNoteWhite);

                    tailColor = note.hasOD
                        ? Color.LightGoldenrodYellow
                        : (note.NoteName.Contains("#") ? Color.Black : Color.White);

                    double noteHeight = img.Height * (noteWidth / img.Width);

                    // Once the note reaches the hitbox, keep it there while the sustain is active.
                    bool isAtOrPastHitbox = posY >= hitboxY;

                    if (sustainActive && isAtOrPastHitbox)
                    {
                        posY = hitboxY;
                    }

                    // Draw sustain tail only while the sustain is still visually active.
                    if (hasSustain && correctedTime <= visualEndTime + sustainGrace)
                    {
                        DrawSustainTail(
                            graphics,
                            note,
                            tailColor,
                            correctedTime,
                            posY,
                            posX,
                            noteWidth,
                            startingPosition);
                    }

                    // If the note has gone past the hitbox and no sustain is active, skip drawing the gem.
                    if (posY > hitboxY && !sustainActive)
                        continue;

                    graphics.DrawImage(
                        img,
                        (float)posX,
                        (float)(posY - (noteHeight / 2.0)),
                        (float)noteWidth,
                        (float)noteHeight);
                }
            }
        }        

        private void DrawSustainTail(
            Graphics g,
            MIDINote note,
            Color tailColor,
            double correctedTime,
            double visualEndTime,
            int laneIndex,
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

            if (correctedTime > visualEndTime + grace) return;

            double Clamp01(double v) => v < 0 ? 0 : (v > 1 ? 1 : v);
            double ClampMin0(double v) => v < 0 ? 0 : v;
            double EaseIn(double t) => Math.Pow(t, depthPower);
            double Lerp(double a, double b, double t) => a + (b - a) * t;

            float trackCenterX = chartLeft + (trackWidth / 2f);
            const int lanes = 5;

            // The tail visually runs from the sustain endpoint toward the held gem/head.
            // For active sustains, the head is locked at the hitbox. The endpoint should
            // move downward as the sustain approaches completion.
            double tEnd = 1.0 - ((visualEndTime - correctedTime) / PlaybackWindowRB);
            tEnd = ClampMin0(tEnd);

            double pEnd = EaseIn(tEnd);
            pEnd = Clamp01(pEnd);

            // Match DrawBeatLines / gem projection for the far end of the tail.
            float yEnd = (float)Lerp(horizonY, hitboxY, pEnd);

            if (yEnd > hitboxY)
                yEnd = hitboxY;

            // If the sustain endpoint has reached the hitbox, there is no visible tail left.
            if (yEnd >= hitboxY - 1f)
                return;

            // Correct inverse for YFromP01: linear
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

            bool doWave = correctedTime >= note.NoteStart;

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
                    // animate by time and vary by y for a subtle animation
                    double s = Math.Sin((correctedTime * 2.0 + yy * waveYFreq) * Math.PI * 2.0);
                    waveOffset = amplitude * s;

                    // keep the tail inside the lane
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

            var renderSize = activeRenderingResolution;
            var correctedTime = GetCorrectedTime();

            const double minScale = HighwayAngleFactor; // MUST match background topWidthFactor
            const double maxScale = 1.00;               // 1.00 to match bottom exactly

            float hitboxY = renderSize.Height - 50f;
            float horizonY = startingPosition + ((hitboxY - startingPosition) * horizonPercent);

            // Keep this at 0.00 if you want tap notes gone immediately after hit time.
            const double passedWindow = 0.00;

            // Time window, in seconds, where we consider the note "at" the hitbox.
            // Increase slightly if the note still looks like it misses the hitbox.
            const double hitTimeWindow = 0.25;

            // Pixel window near the hitbox for snapping.
            const float hitWindowPx = 20f;

            DrawBeatLines(
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
                double visualLength = GetVisualSustainLength(note.NoteLength);
                double visualEndTime = note.NoteStart + visualLength;

                if (note.NoteStart > correctedTime + PlaybackWindowRB) break;

                if (note.NoteColor == Color.Empty)
                    note.NoteColor = GetNoteColor(note.NoteNumber);

                int noteLocation;
                if (note.NoteColor == ChartRed) noteLocation = 1;
                else if (note.NoteColor == ChartYellow) noteLocation = 2;
                else if (note.NoteColor == ChartBlue) noteLocation = 3;
                else if (note.NoteColor == ChartOrange) noteLocation = 4;
                else noteLocation = 0;

                bool hasSustain = note.NoteLength >= 0.25;

                bool sustainActive =
                    hasSustain &&
                    correctedTime >= note.NoteStart &&
                    correctedTime <= visualEndTime;

                // Cull notes that are already finished.
                // Tap notes disappear after the hit.
                // Sustain notes remain drawable until the sustain ends.
                if (!hasSustain && correctedTime > note.NoteStart + passedWindow)
                    continue;

                if (hasSustain && correctedTime > visualEndTime + passedWindow)
                    continue;

                // p for the GEM position.
                // Raw p can go past 1.0 after the note passes the hitbox.
                // Clamped p is used for drawing so the note does not keep growing forever.
                double tHead = 1.0 - ((note.NoteStart - correctedTime) / PlaybackWindowRB);
                tHead = ClampMin0(tHead);

                double pHeadRaw = EaseIn(tHead);
                double pHeadDraw = Math.Min(1.0, pHeadRaw);

                // Highway span + note geometry at clamped draw position.
                double scaleHead = Lerp(minScale, maxScale, pHeadDraw);
                double spanHead = trackWidth * scaleHead;
                double noteWidth = spanHead / 5.0;

                double spanLeftHead = trackCenterX - (spanHead / 2.0);
                double laneCenterX = spanLeftHead + (noteWidth * (noteLocation + 0.5));
                double posX = laneCenterX - (noteWidth / 2.0);

                // Y mapping. Also clamped through pHeadDraw so it does not go past hitbox scale.
                float posY = (float)Lerp(horizonY, hitboxY + overshootPx, pHeadDraw);
                float posYForTail = (float)Lerp(horizonY, hitboxY, pHeadDraw);

                bool isAtHitbox =
                    correctedTime >= note.NoteStart - hitTimeWindow &&
                    correctedTime <= note.NoteStart + hitTimeWindow &&
                    posY >= hitboxY - hitWindowPx;

                bool useGlowBitmap = isAtHitbox || sustainActive;

                Bitmap img;

                if (note.hasOD)
                {
                    if (note.isHOPOon)
                    {
                        img = useGlowBitmap ? bmpNoteODGlow : bmpODHopo;
                    }
                    else
                    {
                        img = useGlowBitmap ? bmpNoteODGlow : bmpNoteOD;
                    }
                }
                else
                {
                    if (note.isHOPOon)
                    {
                        if (noteLocation == 0) img = useGlowBitmap ? bmpNoteGreenGlow : bmpGreenHopo;
                        else if (noteLocation == 1) img = useGlowBitmap ? bmpNoteRedGlow : bmpRedHopo;
                        else if (noteLocation == 2) img = useGlowBitmap ? bmpNoteYellowGlow : bmpYellowHopo;
                        else if (noteLocation == 3) img = useGlowBitmap ? bmpNoteBlueGlow : bmpBlueHopo;
                        else img = useGlowBitmap ? bmpNoteOrangeGlow : bmpOrangeHopo;
                    }
                    else
                    {
                        if (noteLocation == 0) img = useGlowBitmap ? bmpNoteGreenGlow : bmpNoteGreen;
                        else if (noteLocation == 1) img = useGlowBitmap ? bmpNoteRedGlow : bmpNoteRed;
                        else if (noteLocation == 2) img = useGlowBitmap ? bmpNoteYellowGlow : bmpNoteYellow;
                        else if (noteLocation == 3) img = useGlowBitmap ? bmpNoteBlueGlow : bmpNoteBlue;
                        else img = useGlowBitmap ? bmpNoteOrangeGlow : bmpNoteOrange;
                    }
                }

                double noteHeight = img.Height * (noteWidth / img.Width);

                // Sustain tail color
                Color tailColor = note.hasOD ? Color.White : note.NoteColor;

                if (hasSustain)
                {
                    // While the note is still approaching, the sustain tail should begin at the
                    // moving note head. Once the sustain is active, the head is locked to hitboxY,
                    // so the tail should also begin at the hitbox.
                    
                    float sustainHeadY = (isAtHitbox || sustainActive)
                    ? hitboxY
                    : posYForTail;

                    DrawSustainTail(
                        graphics,
                        note,
                        tailColor,
                        correctedTime,
                        visualEndTime,
                        noteLocation,
                        ChartLeft,
                        trackWidth,
                        horizonY,
                        hitboxY,
                        minScale,
                        maxScale,
                        depthPower,
                        sustainHeadY);
                }

                // Lock the gem head to the hitbox while:
                // 1. it is visually/timing-wise at the hitbox, or
                // 2. the sustain is actively being held.
                if (isAtHitbox || sustainActive)
                {
                    var padding = 1;

                    graphics.DrawImage(
                    img,
                    ChartLeft + ((float)trackWidth / 5 * noteLocation) + padding,
                    hitboxY - 5 + padding,
                    (float)noteWidth,
                    (float)noteHeight);

                    continue;
                }

                // Draw gem normally while approaching the hitbox.
                // Since pHeadDraw is clamped, this should no longer grow endlessly.
                if (posY > hitboxY + overshootPx)
                    continue;

                graphics.DrawImage(
                    img,
                    (float)posX,
                    posY - (float)(noteHeight / 2.0),
                    (float)noteWidth,
                    (float)noteHeight);
            }
        }

        private void DrawFiveLaneNotes(Graphics graphics, MIDITrack instrument, int startingPosition, int ChartLeft, int trackWidth)
        {
            if (instrument.ChartedNotes.Count == 0)
                return;

            var renderSize = activeRenderingResolution;
            var correctedTime = GetCorrectedTime();

            ChartGoal = renderSize.Height - startingPosition - 50;

            double hitboxY = renderSize.Height - 50;
            double noteWidth = trackWidth / 5.0;

            const double minSustainToDraw = 1.0;
            const double sustainGrace = 0.05;

            // Keep only notes that could still be visible or still have a visible sustain.
            var filteredNotes = instrument.ChartedNotes
                .Where(note =>
                {
                    double visualLength = GetVisualSustainLength(note.NoteLength);
                    double visualEndTime = note.NoteStart + visualLength;

                    return note.NoteStart <= correctedTime + PlaybackWindowRB &&
                           correctedTime <= visualEndTime + sustainGrace;
                })
                .ToList();

            foreach (var note in filteredNotes)
            {
                if (note.NoteColor == Color.Empty)
                {
                    note.NoteColor = GetNoteColor(note.NoteNumber);
                }

                bool hasSustain = note.NoteLength >= minSustainToDraw;

                double visualLength = GetVisualSustainLength(note.NoteLength);
                double visualEndTime = note.NoteStart + visualLength;

                bool sustainActive =
                    hasSustain &&
                    correctedTime >= note.NoteStart &&
                    correctedTime <= visualEndTime;

                double percent = 1.0 - ((note.NoteStart - correctedTime) / PlaybackWindowRB);
                double posY = startingPosition + (ChartGoal * percent);

                int noteLocation = 0;
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
                double noteHeight = img.Height * (noteWidth / img.Width);
                double posX = ChartLeft + (noteWidth * noteLocation);

                Color tailColor = note.hasOD ? Color.White : note.NoteColor;

                // Once the note reaches the hitbox, keep it there while the sustain is active.
                bool isAtOrPastHitbox = posY >= hitboxY;

                if (sustainActive && isAtOrPastHitbox)
                {
                    posY = hitboxY;
                }

                // Draw sustain tail only while the sustain is still visually active.
                if (hasSustain && correctedTime <= visualEndTime + sustainGrace)
                {
                    DrawSustainTail(
                        graphics,
                        note,
                        tailColor,
                        correctedTime,
                        posY,
                        posX,
                        noteWidth,
                        startingPosition);
                }

                // If the note has gone below the hitbox and the sustain is no longer active, stop drawing it.
                if (posY > hitboxY && !sustainActive)
                    continue;

                graphics.DrawImage(
                    img,
                    (float)posX,
                    (float)(posY - (noteHeight / 2.0)),
                    (float)noteWidth,
                    (float)noteHeight);
            }
        }        

        private void DrawSustainTail(
            Graphics graphics,
            MIDINote note,
            Color tailColor,
            double correctedTime,
            double posY,
            double posX,
            double noteWidth,
            int startingPosition)
        {
            var renderSize = activeRenderingResolution;

            const double sustainVisualLengthScale = 0.75;
            const double shortSustainThreshold = 0.50;

            const float tailWidth = 6f;
            const float tailHalfWidth = tailWidth / 2f;
            const int hitboxOffsetFromBottom = 50;

            const int waveSegmentHeight = 5;
            const double waveAmplitude = 4.0;
            const double waveYFreq = 0.0125;

            double hitboxY = renderSize.Height - hitboxOffsetFromBottom;

            // Same compromise as the five-lane sustain tail:
            // short sustains keep their full length, longer sustains are visually shortened.
            double visualLength = note.NoteLength < shortSustainThreshold
                ? note.NoteLength
                : note.NoteLength * sustainVisualLengthScale;

            double visualEndTime = note.NoteStart + visualLength;

            if (correctedTime > visualEndTime)
                return;

            // Calculate the end position of the visible sustain tail.
            double tailEndPercent =
                1.0 - ((visualEndTime - correctedTime) / PlaybackWindowRB);

            double tailEndY = startingPosition + (ChartGoal * tailEndPercent);

            if (tailEndY < startingPosition)
                tailEndY = startingPosition;

            if (tailEndY > hitboxY)
                tailEndY = hitboxY;

            double splitY = Math.Min(hitboxY, posY);

            if (splitY - tailEndY <= 1.0)
                return;

            var oldSmoothing = graphics.SmoothingMode;
            var oldPix = graphics.PixelOffsetMode;

            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            float centerX = (float)(posX + (noteWidth / 2.0));
            float tailX = centerX - tailHalfWidth;

            using (var tailBrush = new SolidBrush(tailColor))
            {
                // Static part: normal straight tail above/approaching the hitbox.
                if (posY <= hitboxY && splitY > tailEndY)
                {
                    graphics.FillRectangle(
                        tailBrush,
                        tailX,
                        (float)tailEndY,
                        tailWidth,
                        (float)(splitY - tailEndY));
                }

                // Dynamic part: once the gem is held at/through the hitbox, draw a small wave.
                if (posY > hitboxY && tailEndY < splitY)
                {
                    int dynamicHeight = (int)(splitY - tailEndY);

                    for (int i = 0; i < dynamicHeight; i += waveSegmentHeight)
                    {
                        double y = tailEndY + i;

                        if (y >= splitY)
                            break;

                        double waveOffset =
                            waveAmplitude *
                            Math.Sin((correctedTime * 2.0 + i * waveYFreq) * Math.PI * 2.0);

                        graphics.FillRectangle(
                            tailBrush,
                            (float)(tailX + waveOffset),
                            (float)y,
                            tailWidth,
                            waveSegmentHeight);
                    }
                }
            }

            graphics.SmoothingMode = oldSmoothing;
            graphics.PixelOffsetMode = oldPix;
        }

        private void TryTriggerKickLights(MIDINote note)
        {
            if (note == null)
                return;

            long noteKey = note.Ticks;

            if (_stageKitTriggeredKickTicks.Contains(noteKey))
                return;

            double correctedTime = GetCorrectedTime();

            _stageKitTriggeredKickTicks.Add(noteKey);
            _lastKickStrobeTime = correctedTime;

            FlashKickWithLeds();
        }

        private void FlashKickWithLeds()
        {
            QueueStageKitCommand(() =>
            {
                if (enableFatsCoLights.Checked && useFatsCoLEDs.Checked)
                {
                    foreach (var fatsCo in fatsCoLights)
                    {
                        fatsCo.AllOff();
                        fatsCo.AllOn();
                        Thread.Sleep(25);
                        fatsCo.AllOff();
                    }
                }

                if (stageKitToolStripMenuItem.Checked && useLEDs.Checked)
                {
                    foreach (var stageKit in stageKits)
                    {
                        stageKit.TurnAllOff();
                        stageKit.DisplayBlueAll(ref ledDisplay, true);
                        stageKit.DisplayYellowAll(ref ledDisplay, true);
                        stageKit.DisplayGreenAll(ref ledDisplay, true);
                        stageKit.DisplayRedAll(ref ledDisplay, true);
                        Thread.Sleep(25);
                        stageKit.TurnAllOff();
                    }
                }
            });
        }

        private void UpdateDrumBasedStageLighting(MIDITrack drums)
        {
            if (drums == null || drums.ChartedNotes == null)
                return;

            double now = GetCorrectedTime();

            const double triggerWindowSeconds = 0.5;

            foreach (var note in drums.ChartedNotes)
            {
                if (note.NoteColor != ChartOrange)
                    continue;

                if (note.NoteStart < now - triggerWindowSeconds)
                    continue;

                if (note.NoteStart > now + triggerWindowSeconds)
                    break;

                TryTriggerKickLights(note);
            }
        }

        private void DrawDrumNotesRB(Graphics graphics, bool doKicks, int startingPosition, int ChartLeft, int trackWidth)
        {
            var track = MIDITools.MIDI_Chart.Drums;
            if (track.ChartedNotes.Count == 0) return;

            var renderSize = activeRenderingResolution;
            var correctedTime = GetCorrectedTime();

            const double minScale = HighwayAngleFactor;
            const double maxScale = 1.00;

            float hitboxY = renderSize.Height - 50f;
            float horizonY = startingPosition + ((hitboxY - startingPosition) * horizonPercent);

            // Tap notes disappear immediately after the hit.
            // Increase this slightly, e.g. 0.025, for a tiny visual linger.
            const double passedWindow = 0.00;

            // Time window where the drum note is forced exactly onto the hitbox.
            const double hitTimeWindow = 0.25;

            // Pixel backup window, so we only snap when it is visually close too.
            const float hitWindowPx = 20f;

            DrawBeatLines(
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

            const int lanes = 4;
            double trackCenterX = ChartLeft + (trackWidth / 2.0);

            foreach (var note in track.ChartedNotes)
            {
                if (note.NoteStart > correctedTime + PlaybackWindowRB)
                    break; // assumes sorted

                if (note.NoteColor == Color.Empty)
                    note.NoteColor = GetNoteColor(note.NoteNumber, true);

                // Filter kicks vs pads/cymbals
                if (note.NoteColor == ChartOrange && !doKicks) continue;
                if (note.NoteColor != ChartOrange && doKicks) continue;

                // Cull old drum notes so they do not stick after the hit.
                if (correctedTime > note.NoteStart + passedWindow)
                    continue;

                // Raw p can exceed 1.0 after the note reaches/passes the hitbox.
                // Clamp p for drawing so size and Y do not continue growing.
                double tHead = 1.0 - ((note.NoteStart - correctedTime) / PlaybackWindowRB);
                tHead = ClampMin0(tHead);

                double pHeadRaw = EaseIn(tHead);
                double pHeadDraw = Math.Min(1.0, pHeadRaw);

                double scaleHead = Lerp(minScale, maxScale, pHeadDraw);
                double laneSpan = trackWidth * scaleHead;
                double noteWidth = laneSpan / lanes;

                float posY = (float)Lerp(horizonY, hitboxY + overshootPx, pHeadDraw);

                bool isAtHitbox =
                    correctedTime >= note.NoteStart - hitTimeWindow &&
                    correctedTime <= note.NoteStart + hitTimeWindow &&
                    posY >= hitboxY - hitWindowPx;

                float drawY = isAtHitbox ? hitboxY : posY;

                // Do not draw beyond the overshoot area.
                if (drawY > hitboxY + overshootPx)
                {
                    continue;
                }                   

                // KICKS
                if (note.NoteColor == ChartOrange)
                {
                    // Do not let kick size explode after hitbox.
                    float kickHeight = (float)(KICK_HEIGHT * Lerp(0.7, 1.0, pHeadDraw));

                    if (isAtHitbox)
                    {
                        kickHeight *= 4f; 
                        TurnOnStrobes();
                    }

                    using (var solidBrush = new SolidBrush(note.hasOD ? Color.WhiteSmoke : Color.FromArgb(255, 180, 28)))
                    {
                        graphics.FillRectangle(
                            solidBrush,
                            (float)(trackCenterX - laneSpan / 2.0),
                            drawY,
                            (float)laneSpan,
                            kickHeight
                        );
                    }

                    continue;
                }

                // Lane index for pads/cymbals
                int noteLocation;
                if (note.NoteColor == ChartRed) noteLocation = 0;
                else if (note.NoteColor == ChartYellow) noteLocation = 1;
                else if (note.NoteColor == ChartBlue) noteLocation = 2;
                else noteLocation = 3; // green

                double laneCenterX = (trackCenterX - (laneSpan / 2.0)) + (noteWidth * (noteLocation + 0.5));
                double drawX = laneCenterX - (noteWidth / 2.0);

                bool isCymbal = !note.isTom;
                bool isOD = note.hasOD;

                Image img;

                if (isCymbal)
                {
                    if (note.NoteColor == ChartYellow)
                        img = isOD ? (isAtHitbox ? (bmpNoteODGlow ?? bmpDrumsCymbalOD) : bmpDrumsCymbalOD)
                                   : (isAtHitbox ? (bmpNoteYellowGlow ?? bmpDrumsCymbalY) : bmpDrumsCymbalY);
                    else if (note.NoteColor == ChartBlue)
                        img = isOD ? (isAtHitbox ? (bmpNoteODGlow ?? bmpDrumsCymbalOD) : bmpDrumsCymbalOD)
                                   : (isAtHitbox ? (bmpNoteBlueGlow ?? bmpDrumsCymbalB) : bmpDrumsCymbalB);
                    else if (note.NoteColor == ChartGreen)
                        img = isOD ? (isAtHitbox ? (bmpNoteODGlow ?? bmpDrumsCymbalOD) : bmpDrumsCymbalOD)
                                   : (isAtHitbox ? (bmpNoteGreenGlow ?? bmpDrumsCymbalG) : bmpDrumsCymbalG);
                    else
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
                    else
                        img = isOD ? (isAtHitbox ? bmpNoteODGlow : bmpNoteOD)
                                   : (isAtHitbox ? bmpNoteGreenGlow : bmpNoteGreen);
                }

                double noteHeight = img.Height * (noteWidth / img.Width);
                double heightScale = Lerp(0.85, 1.00, pHeadDraw);
                noteHeight *= heightScale;

                var currentInterpolation = graphics.InterpolationMode;
                var currentPixelOffSetMode = graphics.PixelOffsetMode;
                var currentCompositingQuality = graphics.CompositingQuality;
                var currentSmoothingMode = graphics.SmoothingMode;

                if (isCymbal)
                {
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                }

                var padding = 1;
                graphics.DrawImage(
                    img,
                    isAtHitbox ? ChartLeft + ((float)trackWidth / 4 * noteLocation) + padding : (float)drawX,
                    isAtHitbox ? drawY - 5 + padding : drawY - (float)(noteHeight / 2.0),
                    (float)noteWidth,
                    (float)noteHeight
                );

                if (isCymbal)
                {
                    graphics.InterpolationMode = currentInterpolation;
                    graphics.PixelOffsetMode = currentPixelOffSetMode;
                    graphics.CompositingQuality = currentCompositingQuality;
                    graphics.SmoothingMode = currentSmoothingMode;
                }
            }
        }

        private void TurnOnStrobes()
        {
            if (enableFatsCoLights.Checked && useFatsCoStrobe.Checked && fatsCoLights.Any())
            {
                foreach (var fatsCo in fatsCoLights)
                {
                    fatsCo.StrobeOnFast();
                }
            }
            if (stageKitToolStripMenuItem.Checked && useStrobe.Checked && stageKits.Any())
            {
                foreach (var stageKit in stageKits)
                {
                    stageKit.TurnStrobeOn(StrobeSpeed.Faster);
                }
            }
        }
               
        private void DrawDrumNotes(Graphics graphics, bool doKicks, int startingPosition, int ChartLeft, int trackWidth)
        {
            if (MIDITools.MIDI_Chart.Drums.ChartedNotes.Count == 0) return;
            var renderSize = activeRenderingResolution;
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
            if (doRockBandChart && !doMIDINoVocals)
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

        private void GetLyricRowPositions(
            Size size,
            bool drawHarm2,
            bool drawHarm3,
            out int harm1Y,
            out int harm2Y,
            out int harm3Y)
        {
            const int rowStepMidi = 30;
            const int rowStepChart = 24;

            // These Y values are the same "fill row" positions the lyric methods expect.
            // Since the cached row background is drawn at (posY - 1), the bottom border lands at posY + 24.
            // So to anchor the last row to the very bottom, use size.Height - 24.
            int bottomAnchoredRow = size.Height - 24;

            if (doVerticalChart)
            {
                int baseTop = GetHeightDiff() + 4;

                // Correct order for chart vertical:
                // harm1 top, harm2 middle, harm3 bottom
                harm1Y = baseTop;
                harm2Y = baseTop + rowStepChart;
                harm3Y = baseTop + (rowStepChart * 2);

                if (!drawHarm2)
                {
                    harm2Y = harm1Y;
                    harm3Y = harm1Y;
                }
                else if (!drawHarm3)
                {
                    harm3Y = harm2Y;
                }

                return;
            }

            // Normal MIDI mode:
            // anchor from bottom upward
            harm3Y = bottomAnchoredRow;
            harm2Y = harm3Y - rowStepMidi;
            harm1Y = harm3Y - (rowStepMidi * 2);

            if (!drawHarm2)
            {
                harm1Y = harm3Y;
                harm2Y = harm3Y;
            }
            else if (!drawHarm3)
            {
                harm1Y = harm2Y;
            }
        }

        private void DrawLyrics(Size size, Graphics graphics, Color backColor)
        {
            if (!openSideWindow.Checked && secondScreen == null)
                return;

            if (!doStaticLyrics && !doScrollingLyrics && !doKaraokeLyrics)
                return;

            bool hasVocalsPhrases = MIDITools.PhrasesVocals != null &&
                                    MIDITools.PhrasesVocals.Phrases != null &&
                                    MIDITools.PhrasesVocals.Phrases.Count > 0;

            bool hasVocalsLyrics = MIDITools.LyricsVocals != null &&
                                   MIDITools.LyricsVocals.Lyrics != null &&
                                   MIDITools.LyricsVocals.Lyrics.Count > 0;

            if (!hasVocalsPhrases || !hasVocalsLyrics)
                return;

            bool hasHarm1Phrases = MIDITools.PhrasesHarm1 != null &&
                                   MIDITools.PhrasesHarm1.Phrases != null &&
                                   MIDITools.PhrasesHarm1.Phrases.Count > 0;

            bool hasHarm1Lyrics = MIDITools.LyricsHarm1 != null &&
                                  MIDITools.LyricsHarm1.Lyrics != null &&
                                  MIDITools.LyricsHarm1.Lyrics.Count > 0;

            bool hasHarm2Phrases = MIDITools.PhrasesHarm2 != null &&
                                   MIDITools.PhrasesHarm2.Phrases != null &&
                                   MIDITools.PhrasesHarm2.Phrases.Count > 0;

            bool hasHarm2Lyrics = MIDITools.LyricsHarm2 != null &&
                                  MIDITools.LyricsHarm2.Lyrics != null &&
                                  MIDITools.LyricsHarm2.Lyrics.Count > 0;

            bool hasHarm3Phrases = MIDITools.PhrasesHarm3 != null &&
                                   MIDITools.PhrasesHarm3.Phrases != null &&
                                   MIDITools.PhrasesHarm3.Phrases.Count > 0;

            bool hasHarm3Lyrics = MIDITools.LyricsHarm3 != null &&
                                  MIDITools.LyricsHarm3.Lyrics != null &&
                                  MIDITools.LyricsHarm3.Lyrics.Count > 0;

            bool drawHarmonyLyrics = doHarmonyLyrics && hasHarm1Lyrics;
            bool drawHarm2 = drawHarmonyLyrics && hasHarm2Lyrics;
            bool drawHarm3 = drawHarmonyLyrics && hasHarm3Lyrics;

            var mainPhrases = drawHarmonyLyrics && hasHarm1Phrases
                ? MIDITools.PhrasesHarm1.Phrases
                : MIDITools.PhrasesVocals.Phrases;

            var mainLyrics = drawHarmonyLyrics && hasHarm1Lyrics
                ? MIDITools.LyricsHarm1.Lyrics
                : MIDITools.LyricsVocals.Lyrics;

            int harm1Y;
            int harm2Y;
            int harm3Y;

            GetLyricRowPositions(size, drawHarm2, drawHarm3, out harm1Y, out harm2Y, out harm3Y);

            if (doRockBandChart && !doMIDINoVocals)
            {
                if (doMIDIHarmonies)
                {
                    if (hasHarm3Lyrics)
                    {
                        harm3Y = 0;
                        harm2Y = 24;
                        harm1Y = vocalsHeight + (harm2Y * 2);
                    }
                    else if (hasHarm2Lyrics)
                    {
                        harm2Y = 0;
                        harm1Y = vocalsHeight + 24;
                    }
                    else
                    {
                        harm1Y = vocalsHeight;
                    }
                }
                else
                {
                    harm1Y = vocalsHeight;
                }
            }
            else if (doRockBandChart && doMIDINoVocals)
            {
                harm1Y = 0;
                harm2Y = harm1Y + 24;
                harm3Y = harm2Y + 24;
            }

            if (doScrollingLyrics)
            {
                if (drawHarm3)
                {
                    DrawLyricsScrolling(MIDITools.LyricsHarm3.Lyrics, _lyricsFont, Harm3Color, backColor, harm3Y, graphics);
                }

                if (drawHarm2)
                {
                    DrawLyricsScrolling(MIDITools.LyricsHarm2.Lyrics, _lyricsFont, Harm2Color, backColor, harm2Y, graphics);
                }

                DrawLyricsScrolling(mainLyrics, _lyricsFont, Harm1Color, backColor, harm1Y, graphics);
                return;
            }

            if (doKaraokeLyrics)
            {
                if (drawHarm3 && hasHarm3Phrases)
                {
                    DrawLyricsKaraoke(MIDITools.PhrasesHarm3.Phrases, MIDITools.LyricsHarm3.Lyrics, _lyricsFont, Harm3Color, backColor, harm3Y, graphics);
                }

                if (drawHarm2 && hasHarm2Phrases)
                {
                    DrawLyricsKaraoke(MIDITools.PhrasesHarm2.Phrases, MIDITools.LyricsHarm2.Lyrics, _lyricsFont, Harm2Color, backColor, harm2Y, graphics);
                }

                DrawLyricsKaraoke(
                    mainPhrases,
                    mainLyrics,
                    _lyricsFont,
                    drawHarmonyLyrics || doMIDIHarm1onVocals ? Harm1Color : Color.White,
                    backColor,
                    harm1Y,
                    graphics);

                return;
            }

            if (doStaticLyrics)
            {
                if (drawHarm3 && hasHarm3Phrases)
                {
                    DrawLyricsStatic(MIDITools.PhrasesHarm3.Phrases, _lyricsFont, Harm3Color, backColor, harm3Y, graphics);
                }

                if (drawHarm2 && hasHarm2Phrases)
                {
                    DrawLyricsStatic(MIDITools.PhrasesHarm2.Phrases, _lyricsFont, Harm2Color, backColor, harm2Y, graphics);
                }

                DrawLyricsStatic(mainPhrases, _lyricsFont, Harm1Color, backColor, harm1Y, graphics);
            }
        }

        private Bitmap GetLyricRowBackground(int width, Color foreColor)
        {
            string key = width + "|" + foreColor.ToArgb() + "|" + doVerticalChart;

            if (_lyricRowBgCache.TryGetValue(key, out var bmp))
                return bmp;

            bmp = new Bitmap(width, 26, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

            using (var g = Graphics.FromImage(bmp))
            using (var overlayBrush = new SolidBrush(Color.FromArgb(doVerticalChart ? 255 : 128, foreColor)))
            {
                g.Clear(Color.Transparent);
                g.DrawImage(Resources.frostedglass75, 0, 1, width, 24);
                g.FillRectangle(overlayBrush, 0, 1, width, 24);
                g.FillRectangle(_lightGrayBrush, 0, 0, width, 1);
                g.FillRectangle(_lightGrayBrush, 0, 25, width, 1);
            }

            _lyricRowBgCache[key] = bmp;
            return bmp;
        }

        private class CachedSimpleLyricLine : IDisposable
        {
            public string Key;
            public Bitmap Bitmap;
            public int Width;
            public int Height;

            public void Dispose()
            {
                Bitmap?.Dispose();
                Bitmap = null;
            }
        }

        private readonly Dictionary<string, CachedSimpleLyricLine> _simpleLyricLineCache =
            new Dictionary<string, CachedSimpleLyricLine>();

        private CachedSimpleLyricLine GetSimpleLyricLineBitmap(
            Graphics graphics,
            string text,
            Font font,
            Color color)
        {
            if (string.IsNullOrEmpty(text))
                return null;

            string key =
                text + "|" +
                font.Name + "|" +
                font.SizeInPoints.ToString("0.###") + "|" +
                ((int)font.Style) + "|" +
                color.ToArgb();

            if (_simpleLyricLineCache.TryGetValue(key, out var cached))
                return cached;

            SizeF measured = graphics.MeasureString(text, font);
            int width = Math.Max(1, (int)Math.Ceiling(measured.Width) + 8);
            int height = Math.Max(1, (int)Math.Ceiling(measured.Height) + 4);

            Bitmap bmp = new Bitmap(width, height);

            using (Graphics gBmp = Graphics.FromImage(bmp))
            using (Brush brush = new SolidBrush(color))
            {
                gBmp.Clear(Color.Transparent);
                gBmp.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                gBmp.DrawString(text, font, brush, new PointF(0, 0));
            }

            cached = new CachedSimpleLyricLine
            {
                Key = key,
                Bitmap = bmp,
                Width = width,
                Height = height
            };

            _simpleLyricLineCache[key] = cached;
            return cached;
        }

        private void ClearSimpleLyricLineCache()
        {
            foreach (var item in _simpleLyricLineCache.Values)
                item.Dispose();

            _simpleLyricLineCache.Clear();
        }

        private void DrawLyricsStatic(
            IEnumerable<LyricPhrase> phrases,
            Font font,
            Color foreColor,
            Color backColor,
            int posY,
            Graphics graphics)
        {
            if (phrases == null)
                return;

            var phraseList = phrases as IList<LyricPhrase> ?? phrases.ToList();
            if (phraseList.Count == 0)
                return;

            var renderSize = activeRenderingResolution;//new Size(1920, 1080);
            double time = GetCorrectedTime();

            var rowBg = GetLyricRowBackground(renderSize.Width, foreColor);
            graphics.DrawImageUnscaled(rowBg, 0, posY - 1);

            LyricPhrase phrase = null;
            int phraseCount = phraseList.Count;

            for (int i = 0; i < phraseCount; i++)
            {
                var current = phraseList[i];

                if (current.PhraseStart > time)
                    break;

                if (current.PhraseEnd >= time)
                    phrase = current;
            }

            string line;
            try
            {
                line = (phrase == null || string.IsNullOrWhiteSpace(phrase.PhraseText))
                    ? GetMusicNotes()
                    : ProcessLine(phrase.PhraseText, doWholeWordsLyrics);
            }
            catch
            {
                line = GetMusicNotes();
            }

            string processedLine = line.Replace("‿", " ");

            var cachedLine = GetSimpleLyricLineBitmap(graphics, processedLine, font, Color.White);

            if (cachedLine != null)
            {
                int left = (renderSize.Width - cachedLine.Width) / 2;
                graphics.DrawImageUnscaled(cachedLine.Bitmap, left, posY - 6);
            }
        }

        private void InitBASS()
        {
            //initialize BASS            
            if (Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero))
            {
                Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_BUFFER, BassBuffer);
                Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_UPDATEPERIOD, 100);
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
            catch
            {
                return false;
            }
            return true;
        }

        public void UpdateStemVolumes()
        {
            //ONLY DO THIS IF CHANGING VOLUME AND BASS IS ALREADY PLAYING AND THESE STREAMS ARE VALID
            var channel_info = Bass.BASS_ChannelGetInfo(BassStream);
            var matrix = GetChannelMatrix(channel_info.chans);
            BassMix.BASS_Mixer_ChannelSetMatrix(BassStream, matrix);
        }
                
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
            catch
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

        private async Task StartPlaybackAsync(bool doFade, bool doNext, bool PlayAudio = true)
        {
            ResetStageKitAnimation();
            ClearKaraokeLineCache();
            ClearSimpleLyricLineCache();
            ClearRBKaraokeStaticBackgroundCache();
            ResetStageKitDrumTriggers();

            doUseBackgroundVideosLast = false; // reset for new song
            doUseBackgroundImagesLast = false; // reset for new song

            if (doRockBandChart) //rock band style / prebuild the track animations
            {
                await DrawRockBandStyleAsync(null, true).ConfigureAwait(true);
            }

            if (GIFOverlay != null)
            {
                GIFOverlay.Close();
                GIFOverlay = null;
            }

            var img = displayAlbumArt ? LargeAlbumArt : null;
            if (secondScreen != null)
            {
                secondScreen.ChangeVisualsImage(img);
            }
            else
            {
                SafeVisualsSetter(img);
            }

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
                            if (!nautilus.DecM(File.ReadAllBytes(CurrentSongAudioPath), false, doNext, DecryptMode.ToMemory))
                            {
                                var msg = "Audio file for '" + PlayingSong.Artist + " - " + PlayingSong.Name + "' failed to decrypt, can't play it";
                                MessageBox.Show(msg, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                StopPlayback();
                                return;
                            }
                        }
                        else if (Path.GetExtension(CurrentSongAudioPath) == ".yarg_mogg")
                        {
                            if (!nautilus.DecY(CurrentSongAudioPath, DecryptMode.ToMemory))
                            {
                                var msg = "Audio file for '" + PlayingSong.Artist + " - " + PlayingSong.Name + "' failed to decrypt, can't play it";
                                MessageBox.Show(msg, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                StopPlayback();
                                return;
                            }
                        }
                    }
                    if (nautilus.PlayingSongOggData == null && nautilus.NextSongOggData != null)
                    {
                        nautilus.PlayingSongOggData = nautilus.NextSongOggData;
                    }
                    if (nautilus.PlayingSongOggData == null || nautilus.PlayingSongOggData.Length == 0)
                    {
                        if (!nautilus.DecM(CurrentSongAudio, false, false, DecryptMode.ToMemory))
                        {
                            var msg = "Audio file for '" + PlayingSong.Artist + " - " + PlayingSong.Name + "' failed to decrypt, can't play it";
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

                if (PlaybackSeconds == 0 && skipIntroOutroSilence.Checked && IntroSilence > -1)
                {
                    PlaybackSeconds = IntroSilence;
                }

                //start video playback if possible
                if (yarg.Checked) // && displayBackgroundVideo)
                {
                    StartVideoPlayback();
                }

                SetPlayLocation(PlaybackSeconds);
                                
                doFade = false; //disabled for now since virtually every song that will be played here starts with silence anyways
                if (doFade) //enable fade-in
                {
                    Bass.BASS_ChannelSetAttribute(BassMixer, BASSAttribute.BASS_ATTRIB_VOL, 0);
                    Bass.BASS_ChannelSlideAttribute(BassMixer, BASSAttribute.BASS_ATTRIB_VOL, masterVol, (int)(FadeLength));
                }
                else //no fade-in
                {
                    Bass.BASS_ChannelSetAttribute(BassMixer, BASSAttribute.BASS_ATTRIB_VOL, masterVol);
                }

                //start mix playback
                if (!Bass.BASS_ChannelPlay(BassMixer, false))
                {
                    MessageBox.Show("Error starting BASS playback:\n" + Bass.BASS_ErrorGetCode(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
                        
            PrepareForDrawing();
            UpdatePlaybackStuff();
            UpdateStats();
            LargeAlbumArt = File.Exists(CurrentSongArtBlurred) ? Tools.NemoLoadImage(CurrentSongArtBlurred) : null;
            if (displayAlbumArt && LargeAlbumArt != null)
            {
                Color bgColor = Color.AliceBlue;
                using (var bmp = (Bitmap)Image.FromFile(CurrentSongArtBlurred))
                {
                    bgColor = Tools.GetMoodBackgroundFromBlurred(bmp, Color.AliceBlue, 28);
                    _cachedMoodColor = bgColor;
                }

                if (secondScreen != null)
                {
                    secondScreen.ChangeVisualsImage(LargeAlbumArt);
                    SetSecondScreenBackColorIfChanged(bgColor);
                    SetPicVisualsBackColorIfChanged(Color.AliceBlue);
                }
                else
                {
                    SafeVisualsSetter(LargeAlbumArt);
                    SetPicVisualsBackColorIfChanged(bgColor);
                }
            }
            else
            {
                var image = doRockBandKaraoke && doStaticBackground ? stageBackground : null;
                if (secondScreen != null)
                {
                    secondScreen.ChangeVisualsImage(image);
                }
                else
                {
                    SafeVisualsSetter(image);
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

            if (doModernKaraokeMode && doSolidColorBackground)
            {
                SafeVisualsSetter(null);
            }

            try
            {
                _beatMarkers = BuildBeatMarkers_UseGetRealtime(MIDITools.LengthLong, MIDITools.TicksPerQuarter, MIDITools.TimeSignatures);
            }
            catch { }
        }

        private void SetVideoPlayerPath(string ini)
        {
            string videoPath = "";

            if (string.IsNullOrEmpty(ini) || !File.Exists(ini))
            {
                ClearVideoMediaSafely();
                return;
            }

            string iniFolder = Path.GetDirectoryName(ini);

            // First try reading explicit "video =" entry from song.ini
            try
            {
                foreach (string line in File.ReadLines(ini))
                {
                    if (line == null)
                        continue;

                    if (!line.Contains("video =") && !line.Contains("video="))
                        continue;

                    string configuredVideo = Tools.GetConfigString(line).Trim();

                    if (!string.IsNullOrEmpty(configuredVideo))
                    {
                        videoPath = Path.Combine(iniFolder, configuredVideo);
                    }

                    break;
                }
            }
            catch
            {
                videoPath = "";
            }

            // If the explicit video path failed, search common background names
            if (string.IsNullOrEmpty(videoPath) || !File.Exists(videoPath))
            {
                string searchPath = iniFolder;

                try
                {
                    // Search for mid file in YARG exCON folder;
                    // video should be where the .mid file is.
                    string[] possibleMidiFiles = Directory.GetFiles(
                        iniFolder,
                        "*.mid",
                        SearchOption.AllDirectories);

                    if (possibleMidiFiles.Length > 0)
                    {
                        searchPath = Path.GetDirectoryName(possibleMidiFiles[0]);
                    }
                }
                catch
                {
                    searchPath = iniFolder;
                }

                try
                {
                    string[] backgrounds = Directory.GetFiles(searchPath);

                    for (int i = 0; i < backgrounds.Length; i++)
                    {
                        string fileName = Path.GetFileName(backgrounds[i]).ToLowerInvariant();

                        switch (fileName)
                        {
                            case "background.avi":
                            case "video.mp4":
                            case "video.webm":
                            case "bg.mp4":
                            case "bg.webm":
                                videoPath = backgrounds[i];
                                break;
                        }

                        if (!string.IsNullOrEmpty(videoPath) && File.Exists(videoPath))
                            break;
                    }
                }
                catch
                {
                    videoPath = "";
                }
            }

            CHVideoPath = videoPath;

            // Existing fallback behavior
            if (yarg.Checked && string.IsNullOrEmpty(CHVideoPath) && doRockBandChart)
            {
                ClearVideoMediaSafely();
                ChangeRBStyleBackground();
                return;
            }

            if (string.IsNullOrEmpty(videoPath) || !File.Exists(videoPath))
            {
                ClearVideoMediaSafely();
                return;
            }

            // Only now touch LibVLC, after we know we have a valid replacement.
            SetVideoMediaSafely(videoPath);
        }

        private void SetVideoMediaSafely(string videoPath)
        {
            if (_libVLC == null || _mediaPlayer == null)
                return;

            lock (_vlcMediaLock)
            {
                Media newMedia = null;
                Media oldMedia = null;

                try
                {
                    newMedia = new Media(_libVLC, videoPath, FromType.FromPath);

                    if (newMedia == null)
                        return;

                    oldMedia = _currentVlcMedia;

                    // Keep the new media alive in a field.
                    _currentVlcMedia = newMedia;

                    // Replace directly instead of clearing to null first.
                    _mediaPlayer.Media = _currentVlcMedia;

                    // Dispose old media only after replacement succeeds.
                    oldMedia?.Dispose();
                }
                catch
                {
                    // If assignment failed, don't leave a dangling new media object.
                    if (!ReferenceEquals(newMedia, _currentVlcMedia))
                        newMedia?.Dispose();
                }
            }
        }

        private void ClearVideoMediaSafely()
        {
            if (_mediaPlayer == null)
                return;

            lock (_vlcMediaLock)
            {
                try
                {
                    Media oldMedia = _currentVlcMedia;
                    _currentVlcMedia = null;
                    _mediaPlayer.Media = null;

                    oldMedia?.Dispose();
                }
                catch
                { }
            }
        }        

        private void StartVideoPlayback()
        {
            if (doFocusMode) return;
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
            var width = secondScreen != null ? secondScreen.Width : (isFullScreen ? Width : picVisuals.Width);
            var height = secondScreen != null ? secondScreen.Height : (isFullScreen ? Height : picVisuals.Height);
            _mediaPlayer.AspectRatio = $"{width}:{height}";
            _mediaPlayer.Scale = 0;

            _mediaPlayer.Play();

            if (_mediaPlayer.State == VLCState.Playing || _mediaPlayer.State == VLCState.Paused)
            {
                _mediaPlayer.Time = GetBASSTimeForVideo();
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
            if ((_mediaPlayer.State == VLCState.Playing || _mediaPlayer.State == VLCState.Paused) && !seeking)
            {
                _mediaPlayer.Time = GetBASSTimeForVideo();
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
                    StopStageKits();
                }
            }
            catch
            {

            }
        }

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

        public void ChangeBackgroundImage(Image image)
        {
            SafeVisualsSetter(image);
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
            catch
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

        public void UpdateDisplay(bool PrepareToDraw = true)
        {
            if (isClosing) return;
            if (PrepareToDraw)
            {
                PrepareForDrawing();
            }
            if (WindowState != FormWindowState.Maximized)
            {
                Width = Width < activeRenderingResolution.Width ? activeRenderingResolution.Width : Width;
                Height = Height < activeRenderingResolution.Height ? activeRenderingResolution.Height : Height;
            }
            LargeAlbumArt = File.Exists(CurrentSongArtBlurred) ? Tools.NemoLoadImage(CurrentSongArtBlurred) : null;
            OriginalAlbumArt = File.Exists(CurrentSongArt) ? Tools.NemoLoadImage(CurrentSongArt) : null;
            if (secondScreen == null)
            {
                lblSections.Parent = picVisuals;
                lblSections.Visible = showPracticeSections.Checked && MIDITools.PracticeSessions.Any() && !doVerticalChart;
                lblSections.BackColor = yarg.Checked && enableYARGCHVideos && _mediaPlayer.Media != null ? Color.Black : LabelBackgroundColor;
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
            if (doMIDINoVocals && !doRockBandChart)
            {
                return 4;
            }
            if (((doVerticalChart) && MIDITools.LyricsVocals.Lyrics.Any()) || doRockBandChart)
            {
                return vocalsHeight + 4;
            }
            var heightDiff = 0;
            if (lblSections.Visible && !doVerticalChart && !doRockBandChart)
            {
                heightDiff += lblSections.Height;
            }
            if (doScrollingLyrics || doStaticLyrics || doKaraokeLyrics || doRockBandChart)
            {
                if (doHarmonyLyrics || doRockBandChart)
                {
                    heightDiff += MIDITools.LyricsHarm3.Lyrics.Any() ? 60 : (MIDITools.LyricsHarm2.Lyrics.Any() ? 40 : 20);
                }
                else if (MIDITools.LyricsVocals.Lyrics.Any() || doRockBandChart)
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
            if (helpHubForm == null || helpHubForm.IsDisposed)
            {
                helpHubForm = new frmHelpHub();
            }
            helpHubForm.Show();
            helpHubForm.ClickWelcome();
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
            catch
            { }
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
            Screen screen = Screen.FromControl(this);
            isResizing = true;
            this.StartPosition = FormStartPosition.Manual;
            this.WindowState = FormWindowState.Normal;
            this.Bounds = screen.WorkingArea;
            this.WindowState = FormWindowState.Maximized;
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
            lblFPS.Parent = picVisuals;
            lblFPS.Left = picVisuals.Width - lblFPS.Width;
            lblFPS.Top = 0;
            isResizing = false;            
            UpdateActiveRenderingResolution();
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
                //doResizeVisuals();
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

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return) return;
            e.Handled = true;
            if (ShowingNotFoundMessage) return;
            if (txtSearch.Text == strSearchPlaylist || Playlist.Count == 0) return;
            ReloadPlaylist(Playlist, true, true, false);
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
            catch
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
            catch
            { }
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

        private Size _spectrumSize = Size.Empty;

        private float[] _fftData = new float[2048]; // FFT4096 = 2048 bins
        private float[] _fftEdge;
        private float[] _fftVisual;
        private float[] _fftSmoothed;

        private PointF[] _fftTopLine;
        private PointF[] _fftBottomLine;
        private PointF[] _fftFillPoints;

        private int[] _fftBandStart;
        private int[] _fftBandEnd;

        private Size _fftWaveCachedSize = Size.Empty;
        private int _fftVisualPointsCached = 0;

        private readonly Brush _fftBgBrush = new SolidBrush(Color.Black);
        private readonly Brush _fftFillBrush = new SolidBrush(Color.FromArgb(105, Color.DodgerBlue));
        private readonly Pen _fftHighlightPen = new Pen(Color.FromArgb(130, Color.RoyalBlue), 1.0f);
        private readonly Pen _fftCenterPen = new Pen(Color.FromArgb(35, Color.RoyalBlue), 1.0f);

        private Color _fftBgColorA = Color.FromArgb(8, 8, 8);
        private Color _fftBgColorB = Color.FromArgb(18, 18, 24);

        private int _fftBgPaletteIndex = 0;
        private double _fftBgLastBeatIndex = -1;
        private double _fftBgColorChangeStartBeat = 0;

        private readonly Random _fftBgRandom = new Random();

        private readonly Color[] _fftBgPalette =
        {
            Color.Black,                   // revert to black

            Color.FromArgb(55, 75, 130),   // deep blue
            Color.FromArgb(85, 55, 130),   // deep purple
            Color.FromArgb(45, 105, 80),   // deep green

            Color.Black,                   // revert to black

            Color.FromArgb(125, 65, 45),   // burnt orange
            Color.FromArgb(45, 100, 125),  // teal blue
            Color.FromArgb(110, 95, 45),   // dark gold / olive

            Color.Black,                   // revert to black

            Color.FromArgb(120, 45, 65),   // muted red / magenta
            Color.FromArgb(55, 120, 115),  // cyan / teal
            Color.FromArgb(70, 55, 120),   // indigo

            Color.Black,                   // revert to black

            Color.FromArgb(95, 55, 95),    // plum
            Color.FromArgb(40, 85, 105),   // steel teal
            Color.FromArgb(90, 70, 40),    // bronze

            Color.Black,                   // revert to black

            Color.FromArgb(60, 90, 140),   // brighter blue
            Color.FromArgb(100, 60, 140),  // violet
            Color.FromArgb(55, 125, 95),   // emerald green

            Color.Black,                   // revert to black

            Color.FromArgb(135, 75, 55),   // orange-brown
            Color.FromArgb(55, 115, 140),  // blue-cyan
            Color.FromArgb(120, 110, 55),  // mustard / olive

            Color.Black,                   // revert to black

            Color.FromArgb(135, 55, 80),   // rose red
            Color.FromArgb(65, 135, 125),  // seafoam teal
            Color.FromArgb(85, 65, 145),   // royal purple

            Color.Black,                   // revert to black

            Color.FromArgb(145, 80, 120),  // pink-purple
            Color.FromArgb(70, 140, 90),   // leafy green
            Color.FromArgb(60, 95, 150),   // cool blue

            Color.FromArgb(220, 220, 220)
        };

        private void EnsureFFTWaveCache(Size size, int visualPoints)
        {
            if (_fftWaveCachedSize == size &&
                _fftVisualPointsCached == visualPoints &&
                _fftVisual != null &&
                _fftSmoothed != null &&
                _fftTopLine != null &&
                _fftBottomLine != null &&
                _fftFillPoints != null &&
                _fftBandStart != null &&
                _fftBandEnd != null &&
                    _fftEdge != null)
            {
                return;
            }

            _fftEdge = new float[visualPoints];
            _fftWaveCachedSize = size;
            _fftVisualPointsCached = visualPoints;

            _fftVisual = new float[visualPoints];
            _fftSmoothed = new float[visualPoints];

            _fftTopLine = new PointF[visualPoints];
            _fftBottomLine = new PointF[visualPoints];
            _fftFillPoints = new PointF[visualPoints * 2];

            _fftBandStart = new int[visualPoints];
            _fftBandEnd = new int[visualPoints];

            int maxBin = _fftData.Length - 1;

            // Skip bin 0. Bin 0 is DC/near-silence junk for visuals.
            const int minBin = 1;

            // Log-ish mapping, but softened so the left side does not become blocky.
            // Smaller exponent = more space for lows/mids.
            // Larger exponent = more space for highs.
            const double curve = 1.25;

            for (int i = 0; i < visualPoints; i++)
            {
                double t1 = i / (double)visualPoints;
                double t2 = (i + 1) / (double)visualPoints;

                int b1 = minBin + (int)((maxBin - minBin) * Math.Pow(t1, curve));
                int b2 = minBin + (int)((maxBin - minBin) * Math.Pow(t2, curve));

                if (b2 <= b1)
                    b2 = b1 + 1;

                if (b1 < minBin)
                    b1 = minBin;

                if (b2 > maxBin)
                    b2 = maxBin;

                _fftBandStart[i] = b1;
                _fftBandEnd[i] = b2;
            }
        }

        private void DrawFFTWaveform(Graphics graphics, Rectangle bounds)
        {
            if (bounds.Width <= 0 || bounds.Height <= 0)
                return;

            const int visualPoints = 768;

            EnsureFFTWaveCache(bounds.Size, visualPoints);

            int result = Bass.BASS_ChannelGetData(
                BassMixer,
                _fftData,
                (int)BASSData.BASS_DATA_FFT4096);

            if (doSpectrumColors)
            {
                using (SolidBrush bgBrush = new SolidBrush(GetBPMPulsingFFTBackgroundColor()))
                {
                    graphics.FillRectangle(bgBrush, bounds);
                }
            }
            else
            {
                graphics.FillRectangle(_fftBgBrush, bounds);
            }

            float centerY = bounds.Top + bounds.Height / 2f;
            float maxAmplitude = bounds.Height * 0.43f;

            if (result <= 0)
            {
                graphics.DrawLine(_fftCenterPen, bounds.Left, centerY, bounds.Right, centerY);
                return;
            }

            const float gain = 14.0f;
            const float riseSpeed = 0.42f;
            const float fallSpeed = 0.82f;

            for (int i = 0; i < visualPoints; i++)
            {
                float screenU = i / (float)(visualPoints - 1);

                // Mirror the frequency layout horizontally:
                // center = lows, edges = highs
                float spectrumT = Math.Abs(screenU - 0.5f) * 2f;

                // Look up the band as if the spectrum runs from center outward
                int mirroredBandIndex = Math.Min(
                    visualPoints - 1,
                    (int)(spectrumT * (visualPoints - 1)));

                int startBin = _fftBandStart[mirroredBandIndex];
                int endBin = _fftBandEnd[mirroredBandIndex];

                float peak = 0f;
                float sum = 0f;
                int count = 0;

                for (int bin = startBin; bin <= endBin; bin++)
                {
                    float value = _fftData[bin];

                    if (value > peak)
                        peak = value;

                    sum += value;
                    count++;
                }

                float avg = count > 0 ? (sum / count) : 0f;

                // Blend average and peak differently depending on frequency zone.
                // Lows get more averaging to avoid skinny spikes.
                float energy;

                if (spectrumT < 0.30f)          // low frequencies (center)
                {
                    energy = (avg * 0.72f) + (peak * 0.28f);
                }
                else if (spectrumT < 0.70f)     // mids
                {
                    energy = (avg * 0.45f) + (peak * 0.55f);
                }
                else                            // highs (edges)
                {
                    energy = (avg * 0.22f) + (peak * 0.78f);
                }

                float raw = Math.Min(1.0f, energy * gain);

                // Slightly compress the dynamic range so spikes don't tower over everything.
                float target = (float)Math.Pow(raw, 0.68f);

                // Frequency weighting, now based on spectrumT (center->edge)
                float frequencyWeight;

                if (spectrumT < 0.35f)
                {
                    // lows in the center
                    frequencyWeight = 0.68f;
                }
                else if (spectrumT < 0.70f)
                {
                    // mids
                    frequencyWeight = 0.92f;
                }
                else
                {
                    // highs at the edges
                    frequencyWeight = 0.62f;
                }

                target *= frequencyWeight;                

                float current = _fftSmoothed[i];

                if (target > current)
                    current += (target - current) * riseSpeed;
                else
                    current *= fallSpeed;

                _fftSmoothed[i] = current;
                _fftVisual[i] = current;
            }

            _fftVisual[0] = _fftSmoothed[0];
            _fftVisual[visualPoints - 1] = _fftSmoothed[visualPoints - 1];

            for (int i = 1; i < visualPoints - 1; i++)
            {
                _fftVisual[i] =
                (_fftSmoothed[i - 1] * 0.10f) +
                (_fftSmoothed[i] * 0.80f) +
                (_fftSmoothed[i + 1] * 0.10f);
            }

            int lowEndCount = (int)(visualPoints * 0.28f);

            // Because lows are now centered, smooth around the center area.
            int centerIndex = visualPoints / 2;
            int lowStart = Math.Max(2, centerIndex - (lowEndCount / 2));
            int lowEnd = Math.Min(visualPoints - 3, centerIndex + (lowEndCount / 2));

            for (int i = lowStart; i <= lowEnd; i++)
            {
                _fftVisual[i] =
                    (_fftVisual[i - 2] * 0.10f) +
                    (_fftVisual[i - 1] * 0.20f) +
                    (_fftVisual[i] * 0.40f) +
                    (_fftVisual[i + 1] * 0.20f) +
                    (_fftVisual[i + 2] * 0.10f);
            }

            _fftEdge[0] = _fftVisual[0];
            _fftEdge[visualPoints - 1] = _fftVisual[visualPoints - 1];

            for (int i = 1; i < visualPoints - 1; i++)
            {
                float center = _fftVisual[i];
                float left = _fftVisual[i - 1];
                float right = _fftVisual[i + 1];

                // Local contrast / unsharp mask.
                float neighborAverage = (left + right) * 0.5f;
                float sharpened = center + ((center - neighborAverage) * 0.85f);

                _fftEdge[i] = Math.Max(0f, Math.Min(1f, sharpened));
            }

            for (int i = 2; i < visualPoints - 2; i++)
            {
                bool isPeak =
                    _fftEdge[i] > _fftEdge[i - 1] &&
                    _fftEdge[i] > _fftEdge[i + 1] &&
                    _fftEdge[i] > 0.08f;

                if (isPeak)
                {
                    _fftEdge[i] = Math.Min(1.0f, _fftEdge[i] * 1.18f);
                }
            }

            float stepX = bounds.Width / (float)(visualPoints - 1);

            for (int i = 0; i < visualPoints; i++)
            {
                float x = bounds.Left + i * stepX;

                float fillAmp = _fftVisual[i] * maxAmplitude;
                float edgeAmp = _fftEdge[i] * maxAmplitude;

                float fillTopY = centerY - fillAmp;
                float fillBottomY = centerY + fillAmp;

                float edgeTopY = centerY - edgeAmp;
                float edgeBottomY = centerY + edgeAmp;

                _fftTopLine[i] = new PointF(x, edgeTopY);
                _fftBottomLine[i] = new PointF(x, edgeBottomY);

                _fftFillPoints[i] = new PointF(x, fillTopY);
                _fftFillPoints[(visualPoints * 2) - 1 - i] = new PointF(x, fillBottomY);
            }

            var oldSmoothing = graphics.SmoothingMode;
            var oldPixelOffset = graphics.PixelOffsetMode;
            var oldCompositing = graphics.CompositingQuality;
            var oldInterpolation = graphics.InterpolationMode;

            graphics.SmoothingMode = SmoothingMode.HighSpeed;
            graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;
            graphics.CompositingQuality = CompositingQuality.HighSpeed;
            graphics.InterpolationMode = InterpolationMode.NearestNeighbor;

            graphics.FillPolygon(_fftFillBrush, _fftFillPoints);

            graphics.DrawLines(_fftHighlightPen, _fftTopLine);
            graphics.DrawLines(_fftHighlightPen, _fftBottomLine);

            graphics.DrawLine(_fftCenterPen, bounds.Left, centerY, bounds.Right, centerY);

            graphics.SmoothingMode = oldSmoothing;
            graphics.PixelOffsetMode = oldPixelOffset;
            graphics.CompositingQuality = oldCompositing;
            graphics.InterpolationMode = oldInterpolation;
        }

        private static Color LerpColor(Color a, Color b, float t)
        {
            t = Math.Max(0f, Math.Min(1f, t));

            int r = (int)(a.R + ((b.R - a.R) * t));
            int g = (int)(a.G + ((b.G - a.G) * t));
            int bVal = (int)(a.B + ((b.B - a.B) * t));

            return Color.FromArgb(r, g, bVal);
        }

        private static float SmoothStep(float t)
        {
            t = Math.Max(0f, Math.Min(1f, t));
            return t * t * (3f - (2f * t));
        }

        private static Color BoostColorSaturationAndBrightness(Color color, float saturationBoost, float brightnessBoost)
        {
            float r = color.R / 255f;
            float g = color.G / 255f;
            float b = color.B / 255f;

            float max = Math.Max(r, Math.Max(g, b));
            float min = Math.Min(r, Math.Min(g, b));
            float l = (max + min) / 2f;

            float h;
            float s;

            if (Math.Abs(max - min) < 0.0001f)
            {
                h = 0f;
                s = 0f;
            }
            else
            {
                float d = max - min;

                s = l > 0.5f
                    ? d / (2f - max - min)
                    : d / (max + min);

                if (Math.Abs(max - r) < 0.0001f)
                    h = (g - b) / d + (g < b ? 6f : 0f);
                else if (Math.Abs(max - g) < 0.0001f)
                    h = (b - r) / d + 2f;
                else
                    h = (r - g) / d + 4f;

                h /= 6f;
            }

            s = Math.Min(1f, s * saturationBoost);
            l = Math.Min(1f, l * brightnessBoost);

            return ColorFromHsl(h, s, l);
        }

        private static Color ColorFromHsl(float h, float s, float l)
        {
            float r, g, b;

            if (s == 0f)
            {
                r = g = b = l;
            }
            else
            {
                float q = l < 0.5f
                    ? l * (1f + s)
                    : l + s - (l * s);

                float p = (2f * l) - q;

                r = HueToRgb(p, q, h + (1f / 3f));
                g = HueToRgb(p, q, h);
                b = HueToRgb(p, q, h - (1f / 3f));
            }

            return Color.FromArgb(
                ClampToByte(r * 255f),
                ClampToByte(g * 255f),
                ClampToByte(b * 255f));
        }

        private static float HueToRgb(float p, float q, float t)
        {
            if (t < 0f)
                t += 1f;

            if (t > 1f)
                t -= 1f;

            if (t < 1f / 6f)
                return p + ((q - p) * 6f * t);

            if (t < 1f / 2f)
                return q;

            if (t < 2f / 3f)
                return p + ((q - p) * ((2f / 3f) - t) * 6f);

            return p;
        }

        private static int ClampToByte(float value)
        {
            if (value < 0f)
                return 0;

            if (value > 255f)
                return 255;

            return (int)value;
        }

        private Color GetBPMPulsingFFTBackgroundColor()
        {
            double bpm = PlayingSong != null && PlayingSong.BPM > 0
                ? PlayingSong.BPM
                : 120.0;

            double time = GetCorrectedTime();

            double secondsPerBeat = 60.0 / bpm;
            double beatPosition = time / secondsPerBeat;
            double beatIndex = Math.Floor(beatPosition);
            double beatFraction = beatPosition - beatIndex;

            // Slower = smoother color drifting.
            // 32 beats at 120 BPM = about 16 seconds.
            const int beatsPerColorChange = 16;

            if (_fftBgLastBeatIndex < 0)
            {
                _fftBgLastBeatIndex = beatIndex;
                _fftBgColorChangeStartBeat = beatIndex;

                _fftBgPaletteIndex = 0;
                _fftBgColorA = _fftBgPalette[0];
                _fftBgColorB = _fftBgPalette[1];
            }

            if (beatIndex >= _fftBgColorChangeStartBeat + beatsPerColorChange)
            {
                _fftBgPaletteIndex = (_fftBgPaletteIndex + 1) % _fftBgPalette.Length;

                _fftBgColorA = _fftBgColorB;
                _fftBgColorB = _fftBgPalette[(_fftBgPaletteIndex + 1) % _fftBgPalette.Length];

                _fftBgColorChangeStartBeat = beatIndex;
            }

            double colorProgress =
                (beatPosition - _fftBgColorChangeStartBeat) / beatsPerColorChange;

            float colorT = SmoothStep((float)colorProgress);

            Color baseColor = LerpColor(_fftBgColorA, _fftBgColorB, colorT);

            // Create a richer/brighter version of the SAME hue.
            Color pulseColor = BoostColorSaturationAndBrightness(baseColor, 1.35f, 1.25f);

            // Soft pulse curve.
            // Starts strongest near the beat, fades gently.
            // Lower strength = less flashing.
            float beatPulse = (float)Math.Pow(1.0 - beatFraction, 2.2);

            // Make downbeat a tiny bit stronger
            bool isDownbeat = ((long)beatIndex % 4) == 0;
            float pulseStrength = isDownbeat ? 0.24f : 0.14f;

            float pulseT = beatPulse * pulseStrength;

            _fftBgLastBeatIndex = beatIndex;

            return LerpColor(baseColor, pulseColor, pulseT);
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
            catch
            { }
        }

        public void ClickDisplayAudioSpectrum()
        {
            StopAllVideoPlayback();
            SetPicVisualsBackColorIfChanged(Color.AliceBlue);
            ChangeTopMenuColors(Color.Black, Color.AliceBlue);
            DisableAllModes();
            displayAudioSpectrum = true;
            updateDisplayType();
            SafeVisualsSetter(null);
        }

        public void ClickDisplayAlbumArt()
        {
            ChangeTopMenuColors(Color.Black, Color.AliceBlue);
            DisableAllModes();
            displayAlbumArt = true;
            updateDisplayType();
            var bgColor = _cachedMoodColor;
            toolTip1.SetToolTip(picPreview, "Click to change spectrum style");
            if (!PlaybackTimer.Enabled)
            {
                SafeVisualsSetter(Resources.logo);
                if (secondScreen != null)
                {
                    secondScreen.ChangeVisualsImage(Resources.logo);
                }
            }
            else
            {
                if (secondScreen != null)
                {
                    secondScreen.StopVideoPlayback();
                    SetSecondScreenBackColorIfChanged(bgColor);
                    SetPicVisualsBackColorIfChanged(Color.AliceBlue);
                }
                else
                {
                    StopVideoPlayback();
                    SetPicVisualsBackColorIfChanged(bgColor);
                }
            }
        }

        private void ChangeDisplay()
        {
            ClearVisuals();
            if (!displayAlbumArt && File.Exists(CurrentSongArt))
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

        private void showMIDIVisuals_Click(object sender, EventArgs e)
        {
            if (midiSelectorForm == null || midiSelectorForm.IsDisposed)
            {
                midiSelectorForm = new MIDISelector(this);
            }
            midiSelectorForm.Show();
        }

        private void DrawLyricsScrolling(List<Lyric> lyrics, Font font, Color foreColor, Color backColor, int posY, Graphics graphics)
        {
            if ((!openSideWindow.Checked && secondScreen == null) || PlayingSong == null || !doScrollingLyrics)
                return;

            if (lyrics == null || lyrics.Count == 0)
                return;

            var renderSize = activeRenderingResolution;

            double time = GetCorrectedTime();
            double playbackWindow = GetVocalScrollWindow();

            bool isGameStyle = (doVerticalChart || doRockBandChart) || doRockBandKaraoke;
            int hitboxPosition = isGameStyle ? HitboxVocalsX + (bmpHitboxVocals.Width / 2) : renderSize.Width;

            var rowBg = GetLyricRowBackground(renderSize.Width, foreColor);
            graphics.DrawImageUnscaled(rowBg, 0, posY - 1);

            int lyricCount = lyrics.Count;

            for (int i = 0; i < lyricCount; i++)
            {
                var lyric = lyrics[i];

                if (lyric.End > 0 && lyric.End < time - 0.10)
                    continue;

                if (lyric.Start > time + playbackWindow)
                    break;

                float leftF = isGameStyle
                    ? (float)(((lyric.Start - time) / playbackWindow) * (renderSize.Width - hitboxPosition) + hitboxPosition)
                    : (float)(((lyric.Start - time) / playbackWindow) * renderSize.Width);

                int left = (int)Math.Round(leftF);

                string text = lyric.DisplayText.Replace("‿", " ");
                using (var brush = new SolidBrush(Color.WhiteSmoke))
                {
                    graphics.DrawString(text, font, brush, new Point(left, posY - 6));
                }          
            }
        }

        private void DrawLyricsKaraoke(
            IEnumerable<LyricPhrase> phrases,
            IEnumerable<Lyric> lyrics,
            Font font,
            Color foreColor,
            Color backColor,
            int posY,
            Graphics graphics)
        {
            if (phrases == null || lyrics == null)
                return;

            var phraseList = phrases as IList<LyricPhrase> ?? phrases.ToList();
            var lyricList = lyrics as IList<Lyric> ?? lyrics.ToList();

            if (lyricList.Count == 0 || phraseList.Count == 0)
                return;

            double time = GetCorrectedTime();
            var renderSize = activeRenderingResolution;                   

            var rowBg = GetLyricRowBackground(renderSize.Width, foreColor);
            graphics.DrawImageUnscaled(rowBg, 0, posY - 1);

            LyricPhrase line = null;
            int phraseCount = phraseList.Count;

            for (int i = 0; i < phraseCount; i++)
            {
                var phrase = phraseList[i];

                if (phrase.PhraseStart > time)
                    break;

                if (phrase.PhraseEnd >= time)
                    line = phrase;
            }

            if (line == null || string.IsNullOrEmpty(line.PhraseText))
                return;

            string full = ProcessLine(line.PhraseText, doWholeWordsLyrics).Replace("‿", " ");
            if (string.IsNullOrEmpty(full))
                return;

            var sb = new StringBuilder();
            int lyricCount = lyricList.Count;

            for (int i = 0; i < lyricCount; i++)
            {
                var lyr = lyricList[i];

                if (lyr.Start < line.PhraseStart)
                    continue;

                if (lyr.Start > time)
                    break;

                sb.Append(' ');
                sb.Append(lyr.Text);
            }

            string highlight = ProcessLine(sb.ToString(), doWholeWordsLyrics).Replace("‿", " ");
            if (string.IsNullOrEmpty(highlight))
                return;

            var oldHint = graphics.TextRenderingHint;
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            using (var format = (StringFormat)StringFormat.GenericTypographic.Clone())
            using (var fullBrush = new SolidBrush(Color.Gainsboro))
            using (var highlightBrush = new SolidBrush(Color.White))
            {
                format.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
                format.Trimming = StringTrimming.None;

                SizeF fullSize = graphics.MeasureString(full, font, PointF.Empty, format);
                float left = (renderSize.Width - fullSize.Width) / 2f;
                float top = posY - 6;

                graphics.DrawString(full, font, fullBrush, new PointF(left, top), format);
                graphics.DrawString(highlight, font, highlightBrush, new PointF(left, top), format);
            }

            graphics.TextRenderingHint = oldHint;
        }

        private void showLyrics_Click(object sender, EventArgs e)
        {
            if (lyricSelectorForm == null || lyricSelectorForm.IsDisposed)
            {
                lyricSelectorForm = new LyricSelector(this);
            }
            lyricSelectorForm.Show();
        }

        private void panelVisuals_DoubleClick(object sender, EventArgs e)
        {
            if (secondScreen != null) return;
            doResizeVisuals();
        }

        public void doResizeVisuals()
        {
            var screen = Screen.FromControl(picVisuals);

            lblFPS.Parent = picVisuals;
            lblFPS.Top = 0;
            lblFPS.BringToFront();

            this.SuspendLayout();
            picVisuals.SuspendLayout();
            isResizing = true;

            if (isFullScreen)
            {
                FormBorderStyle = FormBorderStyle.FixedSingle;
                menuStrip1.Visible = true;

                WindowState = FormWindowState.Maximized;
                MaximizeBox = false;

                Size = screen.WorkingArea.Size;
                Location = new Point(0, 0);                              
                
                picVisuals.Location = picVisualsPosition;
                picVisuals.Size = picVisualsSize;                
                picVisuals.Dock = DockStyle.None;

                try
                {
                    videoOverlay.Bounds = picVisuals.Bounds;
                    videoOverlay.Top = picVisuals.Top + menuStrip1.Height;
                    
                }
                catch { }

                isFullScreen = false;

                ChangeTopMenuColors(Color.Black, Color.AliceBlue);

                lblFPS.Left = picVisuals.Width - lblFPS.Width;
            }
            else
            {
                picVisualsPosition = picVisuals.Location;
                picVisualsSize = picVisuals.Size;                

                menuStrip1.Visible = false;
                FormBorderStyle = FormBorderStyle.None;
                WindowState = FormWindowState.Normal;
                Bounds = screen.Bounds;

                try
                {
                    videoOverlay.Bounds = Bounds;
                }
                catch { }

                picVisuals.Dock = DockStyle.Fill;

                isFullScreen = true;

                ChangeTopMenuColors(Color.White, Color.Black);

                lblFPS.Left = Bounds.Width - lblFPS.Width;
            }
                        
            isResizing = false;
            UpdateActiveRenderingResolution();
            picVisuals.ResumeLayout();
            this.ResumeLayout();
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
                catch
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

            var path = Application.StartupPath + "\\bin\\updatev6.txt";
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
            var newVersion = "";
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
                    MessageBox.Show("You have a newer version (" + thisVersion + ") than what's on the server (" + newVersion + ")\nNo update needed", AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if (changeLogForm == null || changeLogForm.IsDisposed)
            {
                changeLogForm = new ChangeLog();
            }
            changeLogForm.Show();
        }

        private void sortPlaylistByModifiedDate_Click(object sender, EventArgs e)
        {
            SortPlaylist(PlaylistSorting.ByModifiedDate);
        }

        private void RenderVisuals(Size size, Graphics g)
        {
            screenSize = size;
            if (!PlaybackTimer.Enabled || (!openSideWindow.Checked && secondScreen == null) || PlayingSong == null || WindowState == FormWindowState.Minimized
                || (_mediaPlayer.State == VLCState.Paused))
            {
                return;
            }

            UpdateTextQuality(g);

            if (doVerticalChart || doRockBandChart || doMIDIChart)
            {
                DrawMIDIFile(size, g);
                return;
            }
            if (displayAlbumArt && !DoYargVideo())
            {
                g.Clear(_cachedMoodColor);
                if (LargeAlbumArt != null)
                {
                    g.DrawImage(LargeAlbumArt, (size.Width - size.Height) / 2, 0, size.Height, size.Height);
                }
                DrawLyrics(size, g, Color.White);
                return;
            }
            if (displayAudioSpectrum && !DoYargVideo())
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
            if (doRockBandKaraoke && MIDITools.PhrasesVocals.Phrases.Any() && MIDITools.LyricsVocals.Lyrics.Any())
            {
                DoRockBandKaraoke(size, g);
                return;
            }
            if (doCPlayerStyleKaraoke && MIDITools.PhrasesVocals.Phrases.Any() && MIDITools.LyricsVocals.Lyrics.Any())
            {
                DoKaraokeMode(g, MIDITools.PhrasesVocals.Phrases, MIDITools.LyricsVocals.Lyrics);
                return;
            }
            if (doModernKaraokeMode && ((MIDITools.PhrasesHarm1.Phrases.Any() && MIDITools.LyricsHarm1.Lyrics.Any()) || (MIDITools.PhrasesVocals.Phrases.Any() && MIDITools.LyricsVocals.Lyrics.Any())))
            {
                DoModernKaraoke(size, g, MIDITools.PhrasesVocals.Phrases, MIDITools.LyricsVocals.Lyrics,
                                   MIDITools.PhrasesHarm1?.Phrases ?? MIDITools.PhrasesVocals.Phrases,
                                   MIDITools.LyricsHarm1?.Lyrics ?? MIDITools.LyricsVocals.Lyrics,
                                   MIDITools.PhrasesHarm2.Phrases, MIDITools.LyricsHarm2.Lyrics,
                                   MIDITools.PhrasesHarm3.Phrases, MIDITools.LyricsHarm3.Lyrics);
                return;
            }            
        }

        private void EnsureRenderedFrame(Size size)
        {
            if (_renderedFrame != null && _renderedFrame.Size == size) return;
            _renderedFrame?.Dispose();
            _renderedFrame = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppPArgb);
        }

        private bool DoYargVideo()
        {
            return yarg.Checked && !string.IsNullOrEmpty(CHVideoPath) && IsVideoPlayingOnAnyScreen();
        }

        private Bitmap _cachedVocalGlass;
        private Bitmap _cachedVocalFadeIn;
        private Bitmap _cachedVocalFadeOut;
        private Bitmap _cachedHarmonyFadeIn;
        private Bitmap _cachedHarmonyFadeOut;

        private Size _cachedVocalGlassSize = Size.Empty;
        private Size _cachedVocalFadeSize = Size.Empty;
        private Size _cachedHarmonyFadeSize = Size.Empty;
        private bool _cachedVocalGlassVerticalMode;

        private readonly Font _rbKaraokeLyricFont = new Font("Segoe UI", 16f, FontStyle.Bold);
        private readonly Font _rbKaraokeScaleFont = new Font("Tahoma", 16f);

        private void EnsureRockBandKaraokeImageCache(Size size, int vocalsY)
        {
            int vocalTrackHeight = vocalsHeight * 2;
            int fadeWidth = (int)(Resources.fadeout3.Width * 1.5);

            bool vertical = doVerticalChart;

            Size glassSize = new Size(size.Width, vocalTrackHeight);
            Size vocalFadeSize = new Size(fadeWidth, vocalTrackHeight);
            Size harmonyFadeSize = new Size(fadeWidth, 24);

            if (_cachedVocalGlass == null ||
                _cachedVocalGlassSize != glassSize ||
                _cachedVocalGlassVerticalMode != vertical)
            {
                _cachedVocalGlass?.Dispose();

                _cachedVocalGlass = new Bitmap(glassSize.Width, glassSize.Height);

                using (Graphics g = Graphics.FromImage(_cachedVocalGlass))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                    Image source = vertical ? Resources.frostedglass75 : Resources.frostedglass50;
                    g.DrawImage(source, 0, 0, glassSize.Width, glassSize.Height);
                }

                _cachedVocalGlassSize = glassSize;
                _cachedVocalGlassVerticalMode = vertical;
            }

            if (_cachedVocalFadeIn == null || _cachedVocalFadeSize != vocalFadeSize)
            {
                _cachedVocalFadeIn?.Dispose();
                _cachedVocalFadeOut?.Dispose();

                _cachedVocalFadeIn = new Bitmap(vocalFadeSize.Width, vocalFadeSize.Height);
                _cachedVocalFadeOut = new Bitmap(vocalFadeSize.Width, vocalFadeSize.Height);

                using (Graphics g = Graphics.FromImage(_cachedVocalFadeIn))
                    g.DrawImage(Resources.fadein3, 0, 0, vocalFadeSize.Width, vocalFadeSize.Height);

                using (Graphics g = Graphics.FromImage(_cachedVocalFadeOut))
                    g.DrawImage(Resources.fadeout3, 0, 0, vocalFadeSize.Width, vocalFadeSize.Height);

                _cachedVocalFadeSize = vocalFadeSize;
            }

            if (_cachedHarmonyFadeIn == null || _cachedHarmonyFadeSize != harmonyFadeSize)
            {
                _cachedHarmonyFadeIn?.Dispose();
                _cachedHarmonyFadeOut?.Dispose();

                _cachedHarmonyFadeIn = new Bitmap(harmonyFadeSize.Width, harmonyFadeSize.Height);
                _cachedHarmonyFadeOut = new Bitmap(harmonyFadeSize.Width, harmonyFadeSize.Height);

                using (Graphics g = Graphics.FromImage(_cachedHarmonyFadeIn))
                    g.DrawImage(Resources.fadein3, 0, 0, harmonyFadeSize.Width, harmonyFadeSize.Height);

                using (Graphics g = Graphics.FromImage(_cachedHarmonyFadeOut))
                    g.DrawImage(Resources.fadeout3, 0, 0, harmonyFadeSize.Width, harmonyFadeSize.Height);

                _cachedHarmonyFadeSize = harmonyFadeSize;
            }
        }

        private void SetPicVisualsBackColorIfChanged(Color color)
        {
            if (_lastPicVisualsBackColor.ToArgb() == color.ToArgb())
                return;

            picVisuals.BackColor = color;
            _lastPicVisualsBackColor = color;
        }

        private void SetSecondScreenBackColorIfChanged(Color color)
        {
            if (secondScreen == null)
                return;

            if (_lastSecondScreenBackColor.ToArgb() == color.ToArgb())
                return;

            secondScreen.ChangeBackgroundColor(color);
            _lastSecondScreenBackColor = color;
        }

        private void DrawCachedRBKaraokeStaticBackground(Graphics graphics, Size size)
        {
            if (graphics == null || stageBackground == null || size.Width <= 0 || size.Height <= 0)
                return;

            bool needsRebuild =
                _cachedRBKaraokeStaticBackground == null ||
                _cachedRBKaraokeStaticBackgroundSize != size ||
                !ReferenceEquals(_cachedRBKaraokeStaticBackgroundSource, stageBackground);

            if (needsRebuild)
            {
                _cachedRBKaraokeStaticBackground?.Dispose();

                _cachedRBKaraokeStaticBackground = new Bitmap(
                    size.Width,
                    size.Height,
                    PixelFormat.Format32bppPArgb);

                using (Graphics g = Graphics.FromImage(_cachedRBKaraokeStaticBackground))
                {
                    g.Clear(Color.Black);

                    g.CompositingMode = CompositingMode.SourceOver;
                    g.CompositingQuality = CompositingQuality.HighSpeed;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.SmoothingMode = SmoothingMode.HighSpeed;

                    g.DrawImage(stageBackground, 0, 0, size.Width, size.Height);
                }

                _cachedRBKaraokeStaticBackgroundSize = size;
                _cachedRBKaraokeStaticBackgroundSource = stageBackground;
            }

            graphics.DrawImageUnscaled(_cachedRBKaraokeStaticBackground, 0, 0);
        }

        private void ClearRBKaraokeStaticBackgroundCache()
        {
            _cachedRBKaraokeStaticBackground?.Dispose();
            _cachedRBKaraokeStaticBackground = null;
            _cachedRBKaraokeStaticBackgroundSize = Size.Empty;
            _cachedRBKaraokeStaticBackgroundSource = null;
        }

        private void DoRockBandKaraoke(Size size, Graphics graphics)
        {
            if (MIDITools.MIDI_Chart.Vocals.ChartedNotes.Count <= 0) return;

            var vocalsY = (size.Height - (vocalsHeight * 2)) / 2;
            int Index;
            Color backColor = Color.FromArgb(36, 36, 36);
            const int spacer = 4;

            try
            {
                if (DoYargVideo())
                {
                    graphics.Clear(Color.Transparent);
                    if (secondScreen != null)
                    {
                        SetPicVisualsBackColorIfChanged(Color.AliceBlue);
                    }
                }
                else
                {
                    if (secondScreen != null)
                    {
                        SetSecondScreenBackColorIfChanged(backColor);
                        SetPicVisualsBackColorIfChanged(Color.AliceBlue);
                    }
                    else
                    {
                        SetPicVisualsBackColorIfChanged(backColor);
                    }
                }

                if (doStaticBackground && !DoYargVideo())
                {
                    DrawCachedRBKaraokeStaticBackground(graphics, size);
                }
            }
            catch { }

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
            for (var i = 0; i < phrasesLead.Count; i++)
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
            for (var i = 0; i < phrasesHarmony.Count; i++)
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
            for (var i = 0; i < phrasesHarmony2.Count; i++)
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
                float sizeToUse = GetScaledFontSize(graphics, title, _rbKaraokeScaleFont, 36f);
                var baseFont = new Font("Tahoma", sizeToUse);
                var infoSize = TextRenderer.MeasureText(title, baseFont);
                int infoX = 0;

                using (var infoFont = new Font("Tahoma", sizeToUse))
                {
                    infoSize = TextRenderer.MeasureText(title, infoFont);
                    infoX = (size.Width - infoSize.Width) / 2;
                    TextRenderer.DrawText(graphics, title, infoFont, new Point(infoX, lineY), Color.WhiteSmoke, Color.Transparent);
                    lineY += infoSize.Height;
                }

                using (var infoFont = new Font("Tahoma", sizeToUse))
                {                    
                    infoSize = TextRenderer.MeasureText(artist, infoFont);
                    infoX = (size.Width - infoSize.Width) / 2;
                    TextRenderer.DrawText(graphics, artist, infoFont, new Point(infoX, lineY), Color.WhiteSmoke, Color.Transparent);
                    lineY += infoSize.Height;
                }

                using (var infoFont = new Font("Tahoma", sizeToUse))
                {
                    infoSize = TextRenderer.MeasureText(album, infoFont);
                    infoX = (size.Width - infoSize.Width) / 2;
                    TextRenderer.DrawText(graphics, album, infoFont, new Point(infoX, lineY), Color.WhiteSmoke, Color.Transparent);
                    lineY += infoSize.Height;
                }

                if (!string.IsNullOrEmpty(charter))
                {
                    using (var infoFont = new Font("Tahoma", sizeToUse))
                    {
                        infoSize = TextRenderer.MeasureText(charter, infoFont);
                        infoX = (size.Width - infoSize.Width) / 2;
                        lineY = size.Height - infoSize.Height - 20;
                        TextRenderer.DrawText(graphics, charter, infoFont, new Point(infoX, lineY), Color.WhiteSmoke, Color.Transparent);
                    }
                }
                else
                {
                    using (var infoFont = new Font("Tahoma", sizeToUse))
                    {
                        infoSize = TextRenderer.MeasureText("Harmonix", infoFont);
                        lineY = size.Height - infoSize.Height - 20;
                    }
                }
                lineY -= infoSize.Height;

                if (!string.IsNullOrEmpty(bpm))
                {
                    using (var infoFont = new Font("Tahoma", sizeToUse))
                    {
                        infoSize = TextRenderer.MeasureText(bpm, infoFont);
                        infoX = (size.Width - infoSize.Width) / 2;
                        TextRenderer.DrawText(graphics, bpm, infoFont, new Point(infoX, lineY), Color.WhiteSmoke, Color.Transparent);
                    }                    
                }
                lineY -= infoSize.Height;

                using (var infoFont = new Font("Tahoma", sizeToUse))
                {
                    infoSize = TextRenderer.MeasureText(vocalParts, infoFont);
                    infoX = (size.Width - infoSize.Width) / 2;
                    TextRenderer.DrawText(graphics, vocalParts, infoFont, new Point(infoX, lineY), Color.WhiteSmoke, Color.Transparent);
                }
            }
            else
            {
                if (currentLineLead == null || nextLineLead == null)
                {
                    try
                    {
                        LyricPhrase nextStartingPhrase = null;                        
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
                    { }
                }
            }

            var vocalLyrics = doHarmonyLyrics && MIDITools.LyricsHarm1.Lyrics.Any() ? MIDITools.LyricsHarm1.Lyrics : MIDITools.LyricsVocals.Lyrics;
            
            var font = _rbKaraokeLyricFont;
            var harm1Y = vocalsY + (vocalsHeight * 2) + spacer;
            var harm2Y = vocalsY - spacer - 24;
            var harm3Y = harm2Y - spacer - 24;

            if (doVerticalChart)
            {
                using (var overlayBrush = new SolidBrush(
                Color.FromArgb(doVerticalChart ? 255 : 128, Color.Black)))
                {
                    graphics.FillRectangle(overlayBrush, 0, vocalsY, size.Width, vocalsHeight * 2);
                }
            }
            EnsureRockBandKaraokeImageCache(size, vocalsY);
            graphics.DrawImageUnscaled(_cachedVocalGlass, 0, vocalsY);
            
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
            int fadeWidth = _cachedVocalFadeOut.Width;
            int hFadeWidth = _cachedHarmonyFadeOut.Width;

            if (MIDITools.MIDI_Chart.Harm3.ChartedNotes.Count > 0 && doMIDIHarmonies)
            {
                DrawNotes(graphics, MIDITools.MIDI_Chart.Harm3, vocalsHeight * 2, vocalsY, false, 3, out Index);
                MIDITools.MIDI_Chart.Harm3.ActiveIndex = Index;
                graphics.DrawImageUnscaled(_cachedHarmonyFadeOut, 0, harm3Y);
                graphics.DrawImageUnscaled(_cachedHarmonyFadeIn, size.Width - hFadeWidth, harm3Y);
            }
            if (MIDITools.MIDI_Chart.Harm2.ChartedNotes.Count > 0 && doMIDIHarmonies)
            {
                DrawNotes(graphics, MIDITools.MIDI_Chart.Harm2, vocalsHeight * 2, vocalsY, false, 2, out Index);
                MIDITools.MIDI_Chart.Harm2.ActiveIndex = Index;
                graphics.DrawImageUnscaled(_cachedHarmonyFadeOut, 0, harm2Y);
                graphics.DrawImageUnscaled(_cachedHarmonyFadeIn, size.Width - hFadeWidth, harm2Y);
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
            graphics.DrawImageUnscaled(_cachedVocalFadeIn, size.Width - fadeWidth, vocalsY);
            graphics.DrawImageUnscaled(_cachedVocalFadeOut, 0, vocalsY);

            graphics.DrawImageUnscaled(_cachedHarmonyFadeOut, 0, harm1Y);
            graphics.DrawImageUnscaled(_cachedHarmonyFadeIn, size.Width - hFadeWidth, harm1Y);
        }

        private void picPreview_Paint(object sender, PaintEventArgs e)
        {
            if (displayAlbumArt || (!File.Exists(CurrentSongArt) && !displayAudioSpectrum))
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
                // INTRO
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

                // OUTRO
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
            catch
            { }
        }

        private void updateDisplayType()
        {
            if (!PlaybackTimer.Enabled)
            {
                if (secondScreen != null)
                {
                    secondScreen.ChangeVisualsImage(Resources.logo);
                }
                else
                {
                    SafeVisualsSetter(Resources.logo);
                }
            }
            UpdateDisplay(false);
            ChangeDisplay();
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
            enableYARGCHVideos = playBGVideos.Checked;
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
            if (micControlForm == null || micControlForm.IsDisposed)
            {
                micControlForm = new MicControl(this);
            }
            micControlForm.Show();
        }

        private void stageKitToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            var isChecked = stageKitToolStripMenuItem.Checked;
            if (!isChecked)
            {
                StopStageKits();
            }
        }

        private void StopStageKits()
        {
            if (stageKits != null)
            {
                foreach (var stageKit in stageKits)
                {
                    if (stageKit == null) continue;
                    try
                    {
                        stageKit.TurnAllOff();
                    }
                    catch { }
                }
            }
            if (fatsCoLights != null)
            {
                foreach (var fatsCo in fatsCoLights)
                {
                    if (fatsCo == null) continue;
                    {
                        try
                        {
                            fatsCo.AllOff();
                        }
                        catch { }
                    }
                }
            }
        }

        private void DisableStageKits(bool message)
        {
            if (message)
            {
                MessageBox.Show("No Stage Kits found", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            foreach (var stageKit in stageKits)
            {
                stageKit.TurnAllOff();
            }
            if (!enableFatsCoLights.Checked)
            {
                StopStageKitCommandWorker();
                stageKitTimer.Enabled = false;
            }
        }

        private void stageKitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!stageKitToolStripMenuItem.Checked)
            {
                DisableStageKits(false);
                return;
            }

            var hidDevices = DeviceList.Local.GetHidDevices().ToList();
            stageKitToolStripMenuItem.Checked = false;                 
            stageKitIndices.Clear();
            stageKits.Clear();

            foreach (var hidDevice in hidDevices)
            {
                if (IsOriginalStageKit(hidDevice))
                {
                    CheckForConnectedStageKits();
                }
            }
            stageKitToolStripMenuItem.Checked = stageKitIndices.Any();
            foreach (var index in stageKitIndices)
            {
                stageKits.Add(new StageKitController(index + 1));
            }
            useLEDs.Enabled = stageKitToolStripMenuItem.Enabled;
            useStrobe.Enabled = stageKitToolStripMenuItem.Enabled;
            useFogger.Enabled = stageKitToolStripMenuItem.Enabled;

            if (stageKitToolStripMenuItem.Checked)
            {
                StartStageKitCommandWorker();
                stageKitTimer.Enabled = true;
            }
            else
            {
                DisableStageKits(true);
            }
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

        public void StopAllVideoPlayback()
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

        private bool IsVideoPlayingOnAnyScreen()
        {
            bool mainVideo = VideoIsPlaying;
            bool secondaryvideo = secondScreen != null && secondScreen.VideoIsPlaying;
            return mainVideo || secondaryvideo;
        }

        public void ClickClassicKaraokeMode()
        {
            if (!DoYargVideo())
            {
                StopAllVideoPlayback();
            }
            else if (yarg.Checked && !string.IsNullOrEmpty(CHVideoPath) && !IsVideoPlayingOnAnyScreen())
            {
                PlayCurrentVideo(CHVideoPath);
            }
            SafeVisualsSetter(null);
            DisableAllModes();
            doModernKaraokeMode = true;
            updateDisplayType();
        }

        public void ClickCPlayerStyle()
        {
            if (!yarg.Checked)
            {
                StopAllVideoPlayback();
            }
            if (yarg.Checked && !string.IsNullOrEmpty(CHVideoPath) && !IsVideoPlayingOnAnyScreen())
            {
                PlayCurrentVideo(CHVideoPath);
            }
            SafeVisualsSetter(null);
            DisableAllModes();
            doCPlayerStyleKaraoke = true;
            updateDisplayType();
            picVisuals.BackgroundImage = null;
        }              

        public void ClickRockBandKaraoke()
        {
            if (!DoYargVideo())
            {
                StopAllVideoPlayback();
            }
            SafeVisualsSetter(PlaybackTimer.Enabled ? Resources.stage_background : null);
            DisableAllModes();
            doRockBandKaraoke = true;                
            updateDisplayType();
        }

        public void ClickVerticalChart()
        {
            StopAllVideoPlayback();
            DisableAllModes();
            doVerticalChart = true;
            updateDisplayType();
            UpdateVisualStyle();
            if (secondScreen != null)
            {
                SetSecondScreenBackColorIfChanged(Color.Black);
            }
        }

        public void ClickMIDIChart()
        {
            StopAllVideoPlayback();
            DisableAllModes();
            doMIDIChart = true;
            updateDisplayType();
            UpdateVisualStyle();
        }

        private void DisableAllModes()
        {
            displayAlbumArt = false;
            displayAudioSpectrum = false;
            doModernKaraokeMode = false;
            doRockBandKaraoke = false;
            doCPlayerStyleKaraoke = false;
            doMIDIChart = false;
            doVerticalChart = false;
            doRockBandChart = false;
            //doFocusMode = false;
        }

        public void ClickAnimatedBackground()
        {
            if (!DoYargVideo())
            {
                StopAllVideoPlayback();
            }
            doStaticBackground = false;
            doAnimatedBackground = true;
        }

        public void ClickStaticBackground()
        {
            if (!DoYargVideo())
            {
                StopAllVideoPlayback();
            }
            doStaticBackground = true;
            doAnimatedBackground = false;
            if (doRockBandKaraoke)
            {
                if (secondScreen != null)
                {
                    secondScreen.ChangeVisualsImage(Resources.stage_background);
                }
                else
                {
                    SafeVisualsSetter(Resources.stage_background);
                }
            }
        }

        public bool GetForceSoloVocalsIsChecked()
        {
            return doForceSoloVocals;
        }

        private void forceSoloVocals_Click(object sender, EventArgs e)
        {
            ClickForceSoloVocals();
        }

        public void ClickForceSoloVocals()
        {
            doForceSoloVocals = !doForceSoloVocals;
            if (doForceSoloVocals)
            {
                doForceTwoPartHarmonies = false;
            }
        }

        public bool GetForceTwoPartHarmoniesIsChecked()
        {
            return doForceTwoPartHarmonies;
        }

        public void ClickForceTwoPartHarmonies()
        {
            doForceTwoPartHarmonies = !doForceTwoPartHarmonies;
            if (doForceTwoPartHarmonies)
            {
                doForceSoloVocals = false;
            }
        }

        private void stageKitTimer_Tick(object sender, EventArgs e)
        {
            if (!useLEDs.Checked && !useStrobe.Checked && !useFogger.Checked)
                return;

            if (stageKits.Count == 0 && fatsCoLights.Count == 0)
                return;

            double time = GetCorrectedTime();
            AnimateStageKits(time);
        }

        private StageKitLedFrame BuildPattern_LaserOpposites(int step)
        {
            var frame = new StageKitLedFrame();

            int baseIndex = step & 7;

            SetOppositePair(frame.Red, baseIndex + 0);
            SetOppositePair(frame.Blue, baseIndex + 1);
            SetOppositePair(frame.Green, baseIndex + 2);
            SetOppositePair(frame.Yellow, baseIndex + 3);

            return frame;
        }

        private void SetOppositePair(bool[] bank, int index)
        {
            if (bank == null || bank.Length < 8)
                return;

            int a = index & 7;
            int b = (a + 4) & 7;

            bank[a] = true;
            bank[b] = true;
        }

        private enum StageKitLedPattern
        {
            OneEachSameDirection,
            OneEachAlternatingDirections,
            BuildColorsThenReverse,
            ThreeEachStaggered,
            LaserOpposites
        }

        private sealed class StageKitLedFrame
        {
            public bool[] Red = new bool[8];
            public bool[] Blue = new bool[8];
            public bool[] Green = new bool[8];
            public bool[] Yellow = new bool[8];
        }

        public enum VideoPathType
        {
            FromPath, FromLocation
        }

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

        public void StartVideoPlayback(string videoPath, VideoPathType pathType, long videoTime)
        {
            if (doFocusMode) return;
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
            // Use center point
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

        private void enableSecondScreen_Click(object sender, EventArgs e)
        {
            var videoShouldBeVisible = (yarg.Checked || doRockBandChart) && IsVideoPlayingOnAnyScreen();
            var videoPath = yarg.Checked && !string.IsNullOrWhiteSpace(CHVideoPath) && File.Exists(CHVideoPath) ? CHVideoPath : currentVideoPath;

            if (enableSecondScreen.Checked)
            {
                enableSecondScreen.Checked = false;
                if (doVerticalChart)
                {
                    SafeVisualsSetter(null);
                    SetPicVisualsBackColorIfChanged(Color.Black);
                }
                else if (doModernKaraokeMode && doSolidColorBackground)
                {
                    SafeVisualsSetter(null);
                }
                else if (doCPlayerStyleKaraoke)
                {
                    SafeVisualsSetter(null);
                }
                else if (doRockBandChart)
                {
                    SafeVisualsSetter(secondScreen.backgroundImage);
                }
                else if (displayAlbumArt)
                {
                    SafeVisualsSetter(LargeAlbumArt);
                    picVisuals.SizeMode = PictureBoxSizeMode.Zoom;
                    Color bgColor = Color.AliceBlue;
                    if (File.Exists(CurrentSongArtBlurred))
                    {
                        using (var bmp = (Bitmap)Image.FromFile(CurrentSongArtBlurred))
                        {
                            bgColor = Tools.GetMoodBackgroundFromBlurred(bmp, Color.AliceBlue, 28);
                        }
                    }
                    SetPicVisualsBackColorIfChanged(bgColor);
                }
                if (secondScreen != null)
                {
                    long time = 0;
                    try
                    {
                        time = GetBASSTimeForVideo();
                    }
                    catch { }
                    if (secondScreen.VideoIsPlaying)
                    {
                        secondScreen.StopVideoPlayback();
                        StartVideoPlayback(videoPath, VideoPathType.FromPath, time);
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
                secondScreen = new PopOutScreen(this)
                {
                    WindowState = FormWindowState.Normal,
                    Bounds = target.Bounds
                };

                SetSecondScreenBackColorIfChanged(picVisuals.BackColor);
                if (doModernKaraokeMode && doSolidColorBackground && !videoShouldBeVisible)
                {
                    secondScreen.ChangeVisualsImage(null);
                }
                else if (displayAlbumArt)
                {
                    secondScreen.ChangeVisualsImage(LargeAlbumArt);
                    Color bgColor = Color.AliceBlue;
                    if (File.Exists(CurrentSongArtBlurred))
                    {
                        using (var bmp = (Bitmap)Image.FromFile(CurrentSongArtBlurred))
                        {
                            bgColor = Tools.GetMoodBackgroundFromBlurred(bmp, Color.AliceBlue, 28);
                        }
                    }
                    SetSecondScreenBackColorIfChanged(bgColor);
                    SetPicVisualsBackColorIfChanged(Color.AliceBlue);
                }
                if (doRockBandChart && !videoShouldBeVisible)
                {
                    secondScreen.ChangeVisualsImage(picVisuals.Image);
                }
                SafeVisualsSetter(Resources.logo);
                secondScreen.Show();
                if (videoShouldBeVisible)
                {
                    var time = GetBASSTimeForVideo();
                    StopAllVideoPlayback();
                    if (!secondScreen.VideoIsPlaying)
                    {
                        secondScreen.StartVideoPlayback(videoPath, PopOutScreen.VideoPathType.FromPath, time);
                    }
                }
                if (WindowState == FormWindowState.Normal)
                {
                    UpdateOverlayPosition();
                }
            }
            UpdateActiveRenderingResolution();
            changedBackground = false;
        }

        private long GetBASSTimeForVideo()
        {
            var mixerState = Bass.BASS_ChannelIsActive(BassMixer);
            if (mixerState != BASSActive.BASS_ACTIVE_PLAYING) return 0;

            long pos = Bass.BASS_ChannelGetPosition(BassMixer);
            double audioMs = Bass.BASS_ChannelBytes2Seconds(BassMixer, pos) * 1000.0;
            long finalPosition = (long)audioMs + Parser.Songs[0].VideoStartTime;
            return finalPosition;
        }

        public void ClickEnableBackgroundImage()
        {
            doAnimatedBackground2 = false;
            doStaticBackground2 = true;
            doSolidColorBackground = false;
        }

        public void ClickAnimatedBackground2()
        {
            if (!DoYargVideo())
            {
                StopAllVideoPlayback();
            }
            doSolidColorBackground = false;
            doAnimatedBackground2 = true;
            doStaticBackground2 = false;
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

        public void ClickSolidColorBackground()
        {
            if (!DoYargVideo())
            {
                StopAllVideoPlayback();
            }
            doSolidColorBackground = true;
            doAnimatedBackground2 = false;
            doStaticBackground2 = false;
            if (secondScreen != null)
            {
                if (yarg.Checked && !string.IsNullOrEmpty(CHVideoPath))
                {
                    secondScreen.ChangeVisualsImage(null);
                    SetSecondScreenBackColorIfChanged(Color.Black);
                }
                else
                {
                    secondScreen.ChangeVisualsImage(null);
                    SetSecondScreenBackColorIfChanged(picVisuals.BackColor);
                }
            }
            else
            {
                if (yarg.Checked && !string.IsNullOrEmpty(CHVideoPath))
                {
                    SafeVisualsSetter(null);
                    SetPicVisualsBackColorIfChanged(Color.Black);
                }
                else
                {
                    SafeVisualsSetter(null);
                    SetPicVisualsBackColorIfChanged(KaraokeBackgroundColor);
                }
            }
        }

        private void picSecondScreen_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (picSecondScreen.Tag.ToString() == "disabled")
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

        private Bitmap RBStyleBackgroundScaled;

        public void ChangeRBStyleBackground()
        {
            if (!doRockBandChart || !PlaybackTimer.Enabled) return;
            if (DoYargVideo()) return;
            if (yarg.Checked && !string.IsNullOrEmpty(CHVideoPath))
            {
                PlayCurrentVideo(CHVideoPath);
                return;
            }

            // VIDEOS
            if (doUseBackgroundVideos && !doUseBackgroundVideosLast && BackgroundVideos != null && BackgroundVideos.Count > 0)
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

                    doUseBackgroundVideosLast = true;
                    doUseBackgroundImagesLast = false;
                    return;
                }

                // If we got here, all entries were missing
                return;
            }

            // IMAGES
            if (doUseBackgroundImages && !doUseBackgroundImagesLast && BackgroundImages != null && BackgroundImages.Count > 0)
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
                     RBStyleBackground = null;

                     RBStyleBackgroundScaled?.Dispose();
                     RBStyleBackgroundScaled = null;

                    using (var original = (Bitmap)Image.FromFile(path))
                    {
                        RBStyleBackground = new Bitmap(original);

                        RBStyleBackgroundScaled = ScaleBackgroundImage(RBStyleBackground);

                        doUseBackgroundImagesLast = true;
                        doUseBackgroundVideosLast = false;
                    }
                    changedBackground = true;
                    return;
                }
            }
        }        

        private Bitmap ScaleBackgroundImage(Bitmap original)
        {          
            Bitmap scaled = new Bitmap(activeRenderingResolution.Width, activeRenderingResolution.Height, PixelFormat.Format32bppPArgb);
            using (var g = Graphics.FromImage(scaled))
            {
                g.CompositingMode = CompositingMode.SourceCopy;
                g.CompositingQuality = CompositingQuality.HighSpeed;
                g.InterpolationMode = InterpolationMode.Bilinear;
                g.SmoothingMode = SmoothingMode.None;
                g.PixelOffsetMode = PixelOffsetMode.Half;

                g.Clear(doFocusMode || doVerticalChart || doMIDIChart ? Color.Black : Color.Transparent);

                g.DrawImage(
                    original,
                    new Rectangle(0, 0, activeRenderingResolution.Width, activeRenderingResolution.Height),
                    0,
                    0,
                    original.Width,
                    original.Height,
                    GraphicsUnit.Pixel);
            }
            return scaled;
        }

        public void ClickRBStyle()
        {
            DisableAllModes();
            doRockBandChart = true;
            updateDisplayType();
            UpdateVisualStyle();
            if (secondScreen != null)
            {
                SetSecondScreenBackColorIfChanged(Color.Black);
            }
            if (yarg.Checked && !string.IsNullOrEmpty(CHVideoPath))
            {
                PlayCurrentVideo(CHVideoPath);
            }
        }        

        public void ClickBackgroundVideos()
        {
            if (DoYargVideo()) return;
            enableYARGCHVideos = false;
            doBackgroundImages = false;
            changedBackground = false;
            doUseBackgroundVideos = true;
            doUseBackgroundImages = false;
            doAnimatedSpectrum = false;
            ChangeRBStyleBackground();
        }

        public void ClickBackgroundImages()
        {
            if (DoYargVideo()) return;
            enableYARGCHVideos = false;
            doBackgroundImages = true;
            changedBackground = false;
            doUseBackgroundVideos = false;
            doUseBackgroundImages = true;
            doAnimatedSpectrum = false;
            StopAllVideoPlayback();
            ChangeRBStyleBackground();
        }

        public void ClickFocusMode(bool enable)
        {
            if (DoYargVideo()) return;
            enableYARGCHVideos = false;
            doBackgroundImages = false;
            doAnimatedSpectrum = false;
            changedBackground = false;
            doFocusMode = enable;
            doUseBackgroundImagesLast = false;
            doUseBackgroundImagesLast = false;
            StopAllVideoPlayback();
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
            if (btSyncForm == null || btSyncForm.IsDisposed)
            {
                btSyncForm = new BTAVSync(this, BTAVOffsetSync, enableBTAVOffsetSync);
            }
            btSyncForm.Show();
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
            if (ActiveSong == null)
            {
                MessageBox.Show("There is no active song", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string args = $"\"{ActiveSong.Location}\"";
            LaunchNautilus(args);
        }

        private void sendToFileAnalyzer_Click(object sender, EventArgs e)
        {
            if (ActiveSong == null)
            {
                MessageBox.Show("There is no active song", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string args = $"-analyzer \"-{ActiveSong.Location}\"";
            LaunchNautilus(args);
        }

        private void sendToAudioAnalyzer_Click(object sender, EventArgs e)
        {
            if (ActiveSong == null)
            {
                MessageBox.Show("There is no active song", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string args = $"-audioa \"-{ActiveSong.Location}\"";
            LaunchNautilus(args);
        }

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
            //UpdateOverlayPosition();
        }

        private void frmMain_LocationChanged(object sender, EventArgs e)
        {
            //UpdateOverlayPosition();
        }

        private void awesomenessDetection_Click(object sender, EventArgs e)
        {
            if (!awesomenessDetection.Checked) return;
            videoOverlay.TopMost = false;
            MessageBox.Show("Awesomeness Detection enabled!", "Nice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            videoOverlay.TopMost = true;
        }

        private void audioMixerTool_Click(object sender, EventArgs e)
        {
            if (audioMixerForm == null || audioMixerForm.IsDisposed)
            {
                audioMixerForm = new AudioMixer(this, PlayingSong);
            }
            audioMixerForm.Show();
        }

        private static void CheckForConnectedStageKits()
        {
            for (int i = 0; i < 4; i++)
            {
                if (XInput.GetState(i, out _))
                {
                    stageKitIndices.Add(i);
                }
            }
        }

        private void enableDebugFPS_Click(object sender, EventArgs e)
        {
            debugText.Visible = enableDebugFPS.Checked;
            lblFPS.Visible = enableDebugFPS.Checked;
            lblFPS.Left = picVisuals.Width - lblFPS.Width;
        }

        private void DisableFatsCoLights(bool message)
        {
            if (message)
            {
                MessageBox.Show("No FatsCo Lights found", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            foreach (var fatsCo in fatsCoLights)
            {
                fatsCo.AllOff();
            }
            if (!stageKitToolStripMenuItem.Checked)
            {
                StopStageKitCommandWorker();
                stageKitTimer.Enabled = false;
            }
        }

        private void enableFatsCoLights_Click(object sender, EventArgs e)
        {
            if (!enableFatsCoLights.Checked)
            {
                DisableFatsCoLights(false);
                return;
            }

            var hidDevices = DeviceList.Local.GetHidDevices().ToList();
            enableFatsCoLights.Checked = false;
            fatsCoLights.Clear();

            foreach (var hidDevice in hidDevices)
            {                
                if (IsFatsCoLight(hidDevice))
                {
                    var fatsCo = new FatsCoHidLightController(hidDevice);
                    if (fatsCo.Open())
                    {
                        fatsCoLights.Add(fatsCo);
                    }                    
                }
            }

            enableFatsCoLights.Checked = fatsCoLights.Any();
            useFatsCoLEDs.Enabled = enableFatsCoLights.Checked;
            useFatsCoStrobe.Enabled = enableFatsCoLights.Checked;

            if (enableFatsCoLights.Checked)
            {
                StartStageKitCommandWorker();
                stageKitTimer.Enabled = true;
            }
            else
            {
                DisableFatsCoLights(true);
            }
        }

        private void useLEDs_Click(object sender, EventArgs e)
        {
            if (useLEDs.Checked) return;
            foreach (var stageKit in stageKits)
            {
                stageKit.TurnAllOff();
            }
        }

        private void useFatsCoLEDs_Click(object sender, EventArgs e)
        {
            if (useFatsCoLEDs.Checked) return;
            foreach (var fatsco in fatsCoLights)
            {
                fatsco.AllOff();
            }
        }

        private void useStrobe_Click(object sender, EventArgs e)
        {
            if (useStrobe.Checked) return;
            foreach (var stageKit in stageKits)
            {
                stageKit.TurnStrobeOff();
            }
        }

        private void useFatsCoStrobe_Click(object sender, EventArgs e)
        {
            if (useFatsCoStrobe.Checked) return;
            foreach (var fatsco in fatsCoLights)
            {
                fatsco.TurnOffStrobe();
            }
        }

        private void useFogger_Click(object sender, EventArgs e)
        {
            if (useFogger.Checked) return;
            foreach (var stageKit in stageKits)
            {
                stageKit.TurnFogOff();
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

    public class KaraokeSyllablePixel
    {
        public double Start;
        public double End;
        public int X;
        public int Width;

        public KaraokeSyllablePixel(double start, double end, int x, int width)
        {
            Start = start;
            End = end;
            X = x;
            Width = width;
        }
    }
}
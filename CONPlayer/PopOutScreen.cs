using cPlayer.Properties;
using LibVLCSharp.Shared;
using LibVLCSharp.WinForms;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cPlayer
{
    public partial class PopOutScreen : Form
    {        
        public Image backgroundImage;
        private LibVLC _libVLC;
        public MediaPlayer _mediaPlayer;
        private VideoView videoView;
        public OverlayForm videoOverlay;
        public bool VideoIsPlaying;

        public PopOutScreen(frmMain ownerForm)
        {
            InitializeComponent();
            Core.Initialize();

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

            // Hook into relevant events to keep the overlay aligned
            this.Resize += (s, e) => UpdateOverlayPosition();
            this.Move += (s, e) => UpdateOverlayPosition();
        }

        public enum VideoPathType
        {
            FromPath, FromLocation
        }

        private void CreateOverlay()
        {
            if (videoOverlay != null) return;
            videoOverlay = new OverlayForm();
            videoOverlay.Show(picVisuals);
            UpdateOverlayPosition();
        }

        private void UpdateOverlayPosition()
        {
            if (videoOverlay != null && !videoOverlay.IsDisposed)
            {
                videoOverlay.Location = Location;
                videoOverlay.ClientSize = ClientSize;
            }
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

        private Media _currentMedia;
        private string _currentVideoPath;
        private VideoPathType _currentVideoType;
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
                    }));
                }
                catch { }
            });
        }

        private int _stopInProgress;
        public void StopVideoPlayback(bool stop = true)
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

        public void ClearOverlayFrame()
        {
            if (videoOverlay == null) return;

            // Create a transparent bitmap and push it through the same pipeline
            using (var bmp = new Bitmap(picVisuals.Width, picVisuals.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb))
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent);
                try 
                { 
                    videoOverlay.UpdateVisuals(bmp); 
                } 
                catch 
                { }
            }
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
            backgroundImage = picVisuals.Image;
        }

        public Size RenderSize()
        {
            return picVisuals.ClientSize;
        }

        public Rectangle PictureBounds()
        {
            return picVisuals.Bounds;
        }  

        public void ChangeBackgroundColor(Color color)
        {
            picVisuals.BackColor = color;
        }
    }
}

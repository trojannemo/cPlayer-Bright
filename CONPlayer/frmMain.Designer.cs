using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace cPlayer
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createNewPlaylist = new System.Windows.Forms.ToolStripMenuItem();
            this.loadExistingPlaylist = new System.Windows.Forms.ToolStripMenuItem();
            this.openRecent = new System.Windows.Forms.ToolStripMenuItem();
            this.recent1 = new System.Windows.Forms.ToolStripMenuItem();
            this.recent2 = new System.Windows.Forms.ToolStripMenuItem();
            this.recent3 = new System.Windows.Forms.ToolStripMenuItem();
            this.recent4 = new System.Windows.Forms.ToolStripMenuItem();
            this.recent5 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveCurrentPlaylist = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renamePlaylist = new System.Windows.Forms.ToolStripMenuItem();
            this.selectAndAddSongsManually = new System.Windows.Forms.ToolStripMenuItem();
            this.scanForSongsAutomatically = new System.Windows.Forms.ToolStripMenuItem();
            this.rebuildPlaylistMetadata = new System.Windows.Forms.ToolStripMenuItem();
            this.rebuildPlaylistMetadataAudio = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.viewSongDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.takeScreenshot = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.consoleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fullSupportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xbox360 = new System.Windows.Forms.ToolStripMenuItem();
            this.pS3 = new System.Windows.Forms.ToolStripMenuItem();
            this.wii = new System.Windows.Forms.ToolStripMenuItem();
            this.fortNite = new System.Windows.Forms.ToolStripMenuItem();
            this.yarg = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.limitedSupportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rb4PS4 = new System.Windows.Forms.ToolStripMenuItem();
            this.rockSmith = new System.Windows.Forms.ToolStripMenuItem();
            this.guitarHero = new System.Windows.Forms.ToolStripMenuItem();
            this.powerGig = new System.Windows.Forms.ToolStripMenuItem();
            this.bandFuse = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoloadLastPlaylist = new System.Windows.Forms.ToolStripMenuItem();
            this.autoPlay = new System.Windows.Forms.ToolStripMenuItem();
            this.continuousPlayback = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.skipIntroOutroSilence = new System.Windows.Forms.ToolStripMenuItem();
            this.audioTracks = new System.Windows.Forms.ToolStripMenuItem();
            this.playBGVideos = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.showPracticeSections = new System.Windows.Forms.ToolStripMenuItem();
            this.showMIDIVisuals = new System.Windows.Forms.ToolStripMenuItem();
            this.showLyrics = new System.Windows.Forms.ToolStripMenuItem();
            this.awesomenessDetection = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripSeparator();
            this.uploadScreenshots = new System.Windows.Forms.ToolStripMenuItem();
            this.openSideWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.enableSecondScreen = new System.Windows.Forms.ToolStripMenuItem();
            this.changeViewToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.equipmentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stageKitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.controller1 = new System.Windows.Forms.ToolStripMenuItem();
            this.controller2 = new System.Windows.Forms.ToolStripMenuItem();
            this.controller3 = new System.Windows.Forms.ToolStripMenuItem();
            this.controller4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
            this.useLEDs = new System.Windows.Forms.ToolStripMenuItem();
            this.useStrobe = new System.Windows.Forms.ToolStripMenuItem();
            this.useFogger = new System.Windows.Forms.ToolStripMenuItem();
            this.microphoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bluetoothAVOffset = new System.Windows.Forms.ToolStripMenuItem();
            this.nautilusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setNautilusPath = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem12 = new System.Windows.Forms.ToolStripSeparator();
            this.sendToVisualizer = new System.Windows.Forms.ToolStripMenuItem();
            this.sendToFileAnalyzer = new System.Windows.Forms.ToolStripMenuItem();
            this.sendToAudioAnalyzer = new System.Windows.Forms.ToolStripMenuItem();
            this.sendToCONExplorer = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.howToUseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForUpdates = new System.Windows.Forms.ToolStripMenuItem();
            this.viewChangeLog = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.VisualsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.displayBackgroundVideo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.displayAlbumArt = new System.Windows.Forms.ToolStripMenuItem();
            this.displayAudioSpectrum = new System.Windows.Forms.ToolStripMenuItem();
            this.displayKaraokeMode = new System.Windows.Forms.ToolStripMenuItem();
            this.classicKaraokeMode = new System.Windows.Forms.ToolStripMenuItem();
            this.solidColorBackground = new System.Windows.Forms.ToolStripMenuItem();
            this.staticBackground2 = new System.Windows.Forms.ToolStripMenuItem();
            this.animatedBackground2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem15 = new System.Windows.Forms.ToolStripSeparator();
            this.forceSoloVocals = new System.Windows.Forms.ToolStripMenuItem();
            this.forceTwoPartHarmonies = new System.Windows.Forms.ToolStripMenuItem();
            this.cPlayerStyle = new System.Windows.Forms.ToolStripMenuItem();
            this.rockBandKaraoke = new System.Windows.Forms.ToolStripMenuItem();
            this.animatedBackground = new System.Windows.Forms.ToolStripMenuItem();
            this.staticBackground = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem13 = new System.Windows.Forms.ToolStripSeparator();
            this.selectBackgroundColor = new System.Windows.Forms.ToolStripMenuItem();
            this.selectLyricColor = new System.Windows.Forms.ToolStripMenuItem();
            this.selectHighlightColor = new System.Windows.Forms.ToolStripMenuItem();
            this.selectHarmonyTextColor = new System.Windows.Forms.ToolStripMenuItem();
            this.selectHarmonyHighlightColor = new System.Windows.Forms.ToolStripMenuItem();
            this.selectHarmony3TextColor = new System.Windows.Forms.ToolStripMenuItem();
            this.selectHarmony3HighlightColor = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem14 = new System.Windows.Forms.ToolStripSeparator();
            this.changeStageBackground = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem16 = new System.Windows.Forms.ToolStripSeparator();
            this.restoreDefaultsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chartVisualsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rBStyle = new System.Windows.Forms.ToolStripMenuItem();
            this.useBackgroundVideos = new System.Windows.Forms.ToolStripMenuItem();
            this.useBackgroundImages = new System.Windows.Forms.ToolStripMenuItem();
            this.chartVertical = new System.Windows.Forms.ToolStripMenuItem();
            this.chartSnippet = new System.Windows.Forms.ToolStripMenuItem();
            this.chartFull = new System.Windows.Forms.ToolStripMenuItem();
            this.lblAuthor = new System.Windows.Forms.Label();
            this.panelPlaying = new System.Windows.Forms.Panel();
            this.picSecondScreen = new System.Windows.Forms.PictureBox();
            this.lblArtist = new System.Windows.Forms.Label();
            this.picShuffle = new System.Windows.Forms.PictureBox();
            this.picLoop = new System.Windows.Forms.PictureBox();
            this.picNext = new System.Windows.Forms.PictureBox();
            this.picPause = new System.Windows.Forms.PictureBox();
            this.picStop = new System.Windows.Forms.PictureBox();
            this.picPlay = new System.Windows.Forms.PictureBox();
            this.picRandom = new System.Windows.Forms.PictureBox();
            this.picVolume = new System.Windows.Forms.PictureBox();
            this.panelSlider = new System.Windows.Forms.Panel();
            this.panelLine = new System.Windows.Forms.Panel();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblDuration = new System.Windows.Forms.Label();
            this.lblYear = new System.Windows.Forms.Label();
            this.lblTrack = new System.Windows.Forms.Label();
            this.lblGenre = new System.Windows.Forms.Label();
            this.lblAlbum = new System.Windows.Forms.Label();
            this.lblSong = new System.Windows.Forms.Label();
            this.picPreview = new System.Windows.Forms.PictureBox();
            this.panelPlaylist = new System.Windows.Forms.Panel();
            this.picOldies = new System.Windows.Forms.PictureBox();
            this.pic1960s = new System.Windows.Forms.PictureBox();
            this.pic1970s = new System.Windows.Forms.PictureBox();
            this.pic1980s = new System.Windows.Forms.PictureBox();
            this.pic1990s = new System.Windows.Forms.PictureBox();
            this.pic2000s = new System.Windows.Forms.PictureBox();
            this.pic2010s = new System.Windows.Forms.PictureBox();
            this.pic2020s = new System.Windows.Forms.PictureBox();
            this.picFavorites = new System.Windows.Forms.PictureBox();
            this.lstPlaylist = new System.Windows.Forms.ListView();
            this.colIndex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSong = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colLength = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PlaylistContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.playNowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playNextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.goToArtist = new System.Windows.Forms.ToolStripMenuItem();
            this.goToAlbum = new System.Windows.Forms.ToolStripMenuItem();
            this.goToGenre = new System.Windows.Forms.ToolStripMenuItem();
            this.startInstaMix = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.markAsPlayed = new System.Windows.Forms.ToolStripMenuItem();
            this.markAsUnplayed = new System.Windows.Forms.ToolStripMenuItem();
            this.moveUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileLocation = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.separator = new System.Windows.Forms.ToolStripSeparator();
            this.sortPlaylistByArtist = new System.Windows.Forms.ToolStripMenuItem();
            this.sortPlaylistBySong = new System.Windows.Forms.ToolStripMenuItem();
            this.sortPlaylistByDuration = new System.Windows.Forms.ToolStripMenuItem();
            this.sortPlaylistByModifiedDate = new System.Windows.Forms.ToolStripMenuItem();
            this.randomizePlaylist = new System.Windows.Forms.ToolStripMenuItem();
            this.returnToPlaylist = new System.Windows.Forms.ToolStripMenuItem();
            this.batchSongLoader = new System.ComponentModel.BackgroundWorker();
            this.songLoader = new System.ComponentModel.BackgroundWorker();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.picSearch = new System.Windows.Forms.PictureBox();
            this.PlaybackTimer = new System.Windows.Forms.Timer(this.components);
            this.songPreparer = new System.ComponentModel.BackgroundWorker();
            this.songExtractor = new System.ComponentModel.BackgroundWorker();
            this.NotifyTray = new System.Windows.Forms.NotifyIcon(this.components);
            this.NotifyContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.playToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pauseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restoreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.folderScanner = new System.ComponentModel.BackgroundWorker();
            this.SongMixer = new System.ComponentModel.BackgroundWorker();
            this.Uploader = new System.ComponentModel.BackgroundWorker();
            this.updater = new System.ComponentModel.BackgroundWorker();
            this.gifTmr = new System.Windows.Forms.Timer(this.components);
            this.stageTimer = new System.Windows.Forms.Timer(this.components);
            this.cursorTimer = new System.Windows.Forms.Timer(this.components);
            this.foggerTimer = new System.Windows.Forms.Timer(this.components);
            this.stageKitTimer = new System.Windows.Forms.Timer(this.components);
            this.lblClearSearch = new System.Windows.Forms.Label();
            this.documentationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.picVisuals = new System.Windows.Forms.PictureBox();
            this.lblSections = new System.Windows.Forms.Label();
            this.picFilters = new System.Windows.Forms.PictureBox();
            this.menuStrip1.SuspendLayout();
            this.VisualsContextMenu.SuspendLayout();
            this.panelPlaying.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSecondScreen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picShuffle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLoop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picNext)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPause)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picStop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPlay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRandom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).BeginInit();
            this.panelPlaylist.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picOldies)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic1960s)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic1970s)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic1980s)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic1990s)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic2000s)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic2010s)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic2020s)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picFavorites)).BeginInit();
            this.PlaylistContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSearch)).BeginInit();
            this.NotifyContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picVisuals)).BeginInit();
            this.picVisuals.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFilters)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.changeViewToolStrip,
            this.equipmentToolStripMenuItem,
            this.nautilusToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1573, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createNewPlaylist,
            this.loadExistingPlaylist,
            this.openRecent,
            this.saveCurrentPlaylist,
            this.saveAsToolStripMenuItem,
            this.exitToolStrip});
            this.fileToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // createNewPlaylist
            // 
            this.createNewPlaylist.BackColor = System.Drawing.Color.White;
            this.createNewPlaylist.ForeColor = System.Drawing.Color.Black;
            this.createNewPlaylist.Name = "createNewPlaylist";
            this.createNewPlaylist.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.createNewPlaylist.Size = new System.Drawing.Size(195, 22);
            this.createNewPlaylist.Text = "New";
            this.createNewPlaylist.Click += new System.EventHandler(this.createNewPlaylist_Click);
            // 
            // loadExistingPlaylist
            // 
            this.loadExistingPlaylist.BackColor = System.Drawing.Color.White;
            this.loadExistingPlaylist.ForeColor = System.Drawing.Color.Black;
            this.loadExistingPlaylist.Name = "loadExistingPlaylist";
            this.loadExistingPlaylist.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.loadExistingPlaylist.Size = new System.Drawing.Size(195, 22);
            this.loadExistingPlaylist.Text = "Open...";
            this.loadExistingPlaylist.Click += new System.EventHandler(this.loadExistingPlaylist_Click);
            // 
            // openRecent
            // 
            this.openRecent.BackColor = System.Drawing.Color.White;
            this.openRecent.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.recent1,
            this.recent2,
            this.recent3,
            this.recent4,
            this.recent5});
            this.openRecent.ForeColor = System.Drawing.Color.Black;
            this.openRecent.Name = "openRecent";
            this.openRecent.Size = new System.Drawing.Size(195, 22);
            this.openRecent.Text = "Open recent...";
            // 
            // recent1
            // 
            this.recent1.BackColor = System.Drawing.Color.White;
            this.recent1.ForeColor = System.Drawing.Color.Black;
            this.recent1.Name = "recent1";
            this.recent1.Size = new System.Drawing.Size(119, 22);
            this.recent1.Text = "Recent 1";
            this.recent1.Click += new System.EventHandler(this.recent1_Click);
            // 
            // recent2
            // 
            this.recent2.BackColor = System.Drawing.Color.White;
            this.recent2.ForeColor = System.Drawing.Color.Black;
            this.recent2.Name = "recent2";
            this.recent2.Size = new System.Drawing.Size(119, 22);
            this.recent2.Text = "Recent 2";
            this.recent2.Click += new System.EventHandler(this.recent2_Click);
            // 
            // recent3
            // 
            this.recent3.BackColor = System.Drawing.Color.White;
            this.recent3.ForeColor = System.Drawing.Color.Black;
            this.recent3.Name = "recent3";
            this.recent3.Size = new System.Drawing.Size(119, 22);
            this.recent3.Text = "Recent 3";
            this.recent3.Click += new System.EventHandler(this.recent3_Click);
            // 
            // recent4
            // 
            this.recent4.BackColor = System.Drawing.Color.White;
            this.recent4.ForeColor = System.Drawing.Color.Black;
            this.recent4.Name = "recent4";
            this.recent4.Size = new System.Drawing.Size(119, 22);
            this.recent4.Text = "Recent 4";
            this.recent4.Click += new System.EventHandler(this.recent4_Click);
            // 
            // recent5
            // 
            this.recent5.BackColor = System.Drawing.Color.White;
            this.recent5.ForeColor = System.Drawing.Color.Black;
            this.recent5.Name = "recent5";
            this.recent5.Size = new System.Drawing.Size(119, 22);
            this.recent5.Text = "Recent 5";
            this.recent5.Click += new System.EventHandler(this.recent5_Click);
            // 
            // saveCurrentPlaylist
            // 
            this.saveCurrentPlaylist.BackColor = System.Drawing.Color.White;
            this.saveCurrentPlaylist.ForeColor = System.Drawing.Color.Black;
            this.saveCurrentPlaylist.Name = "saveCurrentPlaylist";
            this.saveCurrentPlaylist.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveCurrentPlaylist.Size = new System.Drawing.Size(195, 22);
            this.saveCurrentPlaylist.Text = "Save";
            this.saveCurrentPlaylist.Click += new System.EventHandler(this.saveCurrentPlaylist_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.BackColor = System.Drawing.Color.White;
            this.saveAsToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // exitToolStrip
            // 
            this.exitToolStrip.BackColor = System.Drawing.Color.White;
            this.exitToolStrip.ForeColor = System.Drawing.Color.Black;
            this.exitToolStrip.Name = "exitToolStrip";
            this.exitToolStrip.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.exitToolStrip.Size = new System.Drawing.Size(195, 22);
            this.exitToolStrip.Text = "E&xit";
            this.exitToolStrip.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renamePlaylist,
            this.selectAndAddSongsManually,
            this.scanForSongsAutomatically,
            this.rebuildPlaylistMetadata,
            this.rebuildPlaylistMetadataAudio,
            this.toolStripMenuItem1,
            this.viewSongDetails,
            this.takeScreenshot,
            this.toolStripMenuItem7,
            this.consoleToolStripMenuItem});
            this.toolsToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.toolsToolStripMenuItem.Text = "&Playlist";
            // 
            // renamePlaylist
            // 
            this.renamePlaylist.BackColor = System.Drawing.Color.White;
            this.renamePlaylist.ForeColor = System.Drawing.Color.Black;
            this.renamePlaylist.Name = "renamePlaylist";
            this.renamePlaylist.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.renamePlaylist.Size = new System.Drawing.Size(275, 22);
            this.renamePlaylist.Text = "Rename playlist";
            this.renamePlaylist.Click += new System.EventHandler(this.renamePlaylist_Click);
            // 
            // selectAndAddSongsManually
            // 
            this.selectAndAddSongsManually.BackColor = System.Drawing.Color.White;
            this.selectAndAddSongsManually.ForeColor = System.Drawing.Color.Black;
            this.selectAndAddSongsManually.Name = "selectAndAddSongsManually";
            this.selectAndAddSongsManually.Size = new System.Drawing.Size(275, 22);
            this.selectAndAddSongsManually.Text = "Select and add songs manually";
            this.selectAndAddSongsManually.Click += new System.EventHandler(this.selectAndAddSongsManually_Click);
            // 
            // scanForSongsAutomatically
            // 
            this.scanForSongsAutomatically.BackColor = System.Drawing.Color.White;
            this.scanForSongsAutomatically.ForeColor = System.Drawing.Color.Black;
            this.scanForSongsAutomatically.Name = "scanForSongsAutomatically";
            this.scanForSongsAutomatically.Size = new System.Drawing.Size(275, 22);
            this.scanForSongsAutomatically.Text = "Scan for songs automatically";
            this.scanForSongsAutomatically.Click += new System.EventHandler(this.scanForSongsAutomatically_Click);
            // 
            // rebuildPlaylistMetadata
            // 
            this.rebuildPlaylistMetadata.BackColor = System.Drawing.Color.White;
            this.rebuildPlaylistMetadata.ForeColor = System.Drawing.Color.Black;
            this.rebuildPlaylistMetadata.Name = "rebuildPlaylistMetadata";
            this.rebuildPlaylistMetadata.Size = new System.Drawing.Size(275, 22);
            this.rebuildPlaylistMetadata.Text = "Rebuild playlist (metadata only)";
            this.rebuildPlaylistMetadata.Click += new System.EventHandler(this.rebuildPlaylistMetadata_Click);
            // 
            // rebuildPlaylistMetadataAudio
            // 
            this.rebuildPlaylistMetadataAudio.Name = "rebuildPlaylistMetadataAudio";
            this.rebuildPlaylistMetadataAudio.Size = new System.Drawing.Size(275, 22);
            this.rebuildPlaylistMetadataAudio.Text = "Rebuild playlist (metadata + audio)";
            this.rebuildPlaylistMetadataAudio.Click += new System.EventHandler(this.rebuildPlaymetadataAudio_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(272, 6);
            // 
            // viewSongDetails
            // 
            this.viewSongDetails.BackColor = System.Drawing.Color.White;
            this.viewSongDetails.ForeColor = System.Drawing.Color.Black;
            this.viewSongDetails.Name = "viewSongDetails";
            this.viewSongDetails.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.viewSongDetails.Size = new System.Drawing.Size(275, 22);
            this.viewSongDetails.Text = "View active song details";
            this.viewSongDetails.Click += new System.EventHandler(this.viewSongDetails_Click);
            // 
            // takeScreenshot
            // 
            this.takeScreenshot.BackColor = System.Drawing.Color.White;
            this.takeScreenshot.ForeColor = System.Drawing.Color.Black;
            this.takeScreenshot.Name = "takeScreenshot";
            this.takeScreenshot.ShortcutKeys = System.Windows.Forms.Keys.F12;
            this.takeScreenshot.Size = new System.Drawing.Size(275, 22);
            this.takeScreenshot.Text = "Take screenshot of visuals";
            this.takeScreenshot.Click += new System.EventHandler(this.takeScreenshot_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(272, 6);
            // 
            // consoleToolStripMenuItem
            // 
            this.consoleToolStripMenuItem.BackColor = System.Drawing.Color.White;
            this.consoleToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fullSupportToolStripMenuItem,
            this.xbox360,
            this.pS3,
            this.wii,
            this.fortNite,
            this.yarg,
            this.toolStripMenuItem5,
            this.limitedSupportToolStripMenuItem,
            this.rb4PS4,
            this.rockSmith,
            this.guitarHero,
            this.powerGig,
            this.bandFuse});
            this.consoleToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.consoleToolStripMenuItem.Name = "consoleToolStripMenuItem";
            this.consoleToolStripMenuItem.Size = new System.Drawing.Size(275, 22);
            this.consoleToolStripMenuItem.Text = "Game | Console: Rock Band | Xbox 360";
            // 
            // fullSupportToolStripMenuItem
            // 
            this.fullSupportToolStripMenuItem.Enabled = false;
            this.fullSupportToolStripMenuItem.Name = "fullSupportToolStripMenuItem";
            this.fullSupportToolStripMenuItem.Size = new System.Drawing.Size(485, 22);
            this.fullSupportToolStripMenuItem.Text = "(Full Support)";
            // 
            // xbox360
            // 
            this.xbox360.BackColor = System.Drawing.Color.White;
            this.xbox360.Checked = true;
            this.xbox360.CheckState = System.Windows.Forms.CheckState.Checked;
            this.xbox360.ForeColor = System.Drawing.Color.Black;
            this.xbox360.Name = "xbox360";
            this.xbox360.Size = new System.Drawing.Size(485, 22);
            this.xbox360.Text = "Rock Band 1/2/3 | Xbox 360 (CON | LIVE)";
            this.xbox360.Click += new System.EventHandler(this.UpdateConsole);
            // 
            // pS3
            // 
            this.pS3.BackColor = System.Drawing.Color.White;
            this.pS3.ForeColor = System.Drawing.Color.Black;
            this.pS3.Name = "pS3";
            this.pS3.Size = new System.Drawing.Size(485, 22);
            this.pS3.Text = "Rock Band 1/2/3 | PlayStation 3 (.pkg | songs.dta)";
            this.pS3.Click += new System.EventHandler(this.UpdateConsole);
            // 
            // wii
            // 
            this.wii.BackColor = System.Drawing.Color.White;
            this.wii.ForeColor = System.Drawing.Color.Black;
            this.wii.Name = "wii";
            this.wii.Size = new System.Drawing.Size(485, 22);
            this.wii.Text = "Rock Band 1/2/3 | Wii (songs.dta)";
            this.wii.Click += new System.EventHandler(this.UpdateConsole);
            // 
            // fortNite
            // 
            this.fortNite.BackColor = System.Drawing.Color.White;
            this.fortNite.ForeColor = System.Drawing.Color.Black;
            this.fortNite.Name = "fortNite";
            this.fortNite.Size = new System.Drawing.Size(485, 22);
            this.fortNite.Text = "Fortnite Festival | PC (.fnf | .m4a)";
            this.fortNite.Click += new System.EventHandler(this.UpdateConsole);
            // 
            // yarg
            // 
            this.yarg.BackColor = System.Drawing.Color.White;
            this.yarg.ForeColor = System.Drawing.Color.Black;
            this.yarg.Name = "yarg";
            this.yarg.Size = new System.Drawing.Size(485, 22);
            this.yarg.Text = "YARG / Clone Hero / Fret Smasher | PC (songs.dta | .yargsong | song.ini | .sng )";
            this.yarg.Click += new System.EventHandler(this.UpdateConsole);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(482, 6);
            // 
            // limitedSupportToolStripMenuItem
            // 
            this.limitedSupportToolStripMenuItem.Enabled = false;
            this.limitedSupportToolStripMenuItem.Name = "limitedSupportToolStripMenuItem";
            this.limitedSupportToolStripMenuItem.Size = new System.Drawing.Size(485, 22);
            this.limitedSupportToolStripMenuItem.Text = "(Limited Support)";
            // 
            // rb4PS4
            // 
            this.rb4PS4.Name = "rb4PS4";
            this.rb4PS4.Size = new System.Drawing.Size(485, 22);
            this.rb4PS4.Text = "Rock Band 4 | PlayStation 4 (*_ps4)";
            this.rb4PS4.Click += new System.EventHandler(this.UpdateConsole);
            // 
            // rockSmith
            // 
            this.rockSmith.BackColor = System.Drawing.Color.White;
            this.rockSmith.ForeColor = System.Drawing.Color.Black;
            this.rockSmith.Name = "rockSmith";
            this.rockSmith.Size = new System.Drawing.Size(485, 22);
            this.rockSmith.Text = "Rocksmith 2014 | PC (.psarc)";
            this.rockSmith.Click += new System.EventHandler(this.UpdateConsole);
            // 
            // guitarHero
            // 
            this.guitarHero.BackColor = System.Drawing.Color.White;
            this.guitarHero.ForeColor = System.Drawing.Color.Black;
            this.guitarHero.Name = "guitarHero";
            this.guitarHero.Size = new System.Drawing.Size(485, 22);
            this.guitarHero.Text = "Guitar Hero: World Tour: Definitive Edition | PC (.fsb.xen)";
            this.guitarHero.Click += new System.EventHandler(this.UpdateConsole);
            // 
            // powerGig
            // 
            this.powerGig.BackColor = System.Drawing.Color.White;
            this.powerGig.ForeColor = System.Drawing.Color.Black;
            this.powerGig.Name = "powerGig";
            this.powerGig.Size = new System.Drawing.Size(485, 22);
            this.powerGig.Text = "Power Gig | PC (.xml)";
            this.powerGig.Click += new System.EventHandler(this.UpdateConsole);
            // 
            // bandFuse
            // 
            this.bandFuse.BackColor = System.Drawing.Color.White;
            this.bandFuse.ForeColor = System.Drawing.Color.Black;
            this.bandFuse.Name = "bandFuse";
            this.bandFuse.Size = new System.Drawing.Size(485, 22);
            this.bandFuse.Text = "BandFuse | Xbox 360 (LIVE)";
            this.bandFuse.Click += new System.EventHandler(this.UpdateConsole);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.autoloadLastPlaylist,
            this.autoPlay,
            this.continuousPlayback,
            this.toolStripMenuItem4,
            this.skipIntroOutroSilence,
            this.audioTracks,
            this.playBGVideos,
            this.toolStripMenuItem3,
            this.showPracticeSections,
            this.showMIDIVisuals,
            this.showLyrics,
            this.awesomenessDetection,
            this.toolStripMenuItem9,
            this.uploadScreenshots,
            this.openSideWindow,
            this.enableSecondScreen});
            this.optionsToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "&Options";
            // 
            // autoloadLastPlaylist
            // 
            this.autoloadLastPlaylist.BackColor = System.Drawing.Color.White;
            this.autoloadLastPlaylist.Checked = true;
            this.autoloadLastPlaylist.CheckOnClick = true;
            this.autoloadLastPlaylist.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoloadLastPlaylist.ForeColor = System.Drawing.Color.Black;
            this.autoloadLastPlaylist.Name = "autoloadLastPlaylist";
            this.autoloadLastPlaylist.Size = new System.Drawing.Size(242, 22);
            this.autoloadLastPlaylist.Text = "Auto-load last playlist";
            // 
            // autoPlay
            // 
            this.autoPlay.BackColor = System.Drawing.Color.White;
            this.autoPlay.CheckOnClick = true;
            this.autoPlay.ForeColor = System.Drawing.Color.Black;
            this.autoPlay.Name = "autoPlay";
            this.autoPlay.Size = new System.Drawing.Size(242, 22);
            this.autoPlay.Text = "Auto-play after loading";
            // 
            // continuousPlayback
            // 
            this.continuousPlayback.Checked = true;
            this.continuousPlayback.CheckOnClick = true;
            this.continuousPlayback.CheckState = System.Windows.Forms.CheckState.Checked;
            this.continuousPlayback.Name = "continuousPlayback";
            this.continuousPlayback.Size = new System.Drawing.Size(242, 22);
            this.continuousPlayback.Text = "Continuous playback";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(239, 6);
            // 
            // skipIntroOutroSilence
            // 
            this.skipIntroOutroSilence.BackColor = System.Drawing.Color.White;
            this.skipIntroOutroSilence.CheckOnClick = true;
            this.skipIntroOutroSilence.ForeColor = System.Drawing.Color.Black;
            this.skipIntroOutroSilence.Name = "skipIntroOutroSilence";
            this.skipIntroOutroSilence.Size = new System.Drawing.Size(242, 22);
            this.skipIntroOutroSilence.Text = "Skip intro/outro silence";
            // 
            // audioTracks
            // 
            this.audioTracks.BackColor = System.Drawing.Color.White;
            this.audioTracks.ForeColor = System.Drawing.Color.Black;
            this.audioTracks.Name = "audioTracks";
            this.audioTracks.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.audioTracks.Size = new System.Drawing.Size(242, 22);
            this.audioTracks.Text = "Audio tracks to play";
            this.audioTracks.Click += new System.EventHandler(this.audioTracks_Click);
            // 
            // playBGVideos
            // 
            this.playBGVideos.BackColor = System.Drawing.Color.White;
            this.playBGVideos.Checked = true;
            this.playBGVideos.CheckOnClick = true;
            this.playBGVideos.CheckState = System.Windows.Forms.CheckState.Checked;
            this.playBGVideos.ForeColor = System.Drawing.Color.Black;
            this.playBGVideos.Name = "playBGVideos";
            this.playBGVideos.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.playBGVideos.Size = new System.Drawing.Size(242, 22);
            this.playBGVideos.Text = "Play Background Videos";
            this.playBGVideos.Visible = false;
            this.playBGVideos.Click += new System.EventHandler(this.playBGVideos_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(239, 6);
            // 
            // showPracticeSections
            // 
            this.showPracticeSections.BackColor = System.Drawing.Color.White;
            this.showPracticeSections.CheckOnClick = true;
            this.showPracticeSections.ForeColor = System.Drawing.Color.Black;
            this.showPracticeSections.Name = "showPracticeSections";
            this.showPracticeSections.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.showPracticeSections.Size = new System.Drawing.Size(242, 22);
            this.showPracticeSections.Text = "Show practice sections";
            this.showPracticeSections.Click += new System.EventHandler(this.showPracticeSections_Click);
            // 
            // showMIDIVisuals
            // 
            this.showMIDIVisuals.BackColor = System.Drawing.Color.White;
            this.showMIDIVisuals.ForeColor = System.Drawing.Color.Black;
            this.showMIDIVisuals.Name = "showMIDIVisuals";
            this.showMIDIVisuals.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.showMIDIVisuals.Size = new System.Drawing.Size(242, 22);
            this.showMIDIVisuals.Text = "MIDI settings";
            this.showMIDIVisuals.Click += new System.EventHandler(this.showMIDIVisuals_Click);
            // 
            // showLyrics
            // 
            this.showLyrics.BackColor = System.Drawing.Color.White;
            this.showLyrics.ForeColor = System.Drawing.Color.Black;
            this.showLyrics.Name = "showLyrics";
            this.showLyrics.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.showLyrics.Size = new System.Drawing.Size(242, 22);
            this.showLyrics.Text = "Lyrics settings";
            this.showLyrics.Click += new System.EventHandler(this.showLyrics_Click);
            // 
            // awesomenessDetection
            // 
            this.awesomenessDetection.CheckOnClick = true;
            this.awesomenessDetection.Name = "awesomenessDetection";
            this.awesomenessDetection.Size = new System.Drawing.Size(242, 22);
            this.awesomenessDetection.Text = "Awesomeness Detection";
            this.awesomenessDetection.Click += new System.EventHandler(this.awesomenessDetection_Click);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(239, 6);
            // 
            // uploadScreenshots
            // 
            this.uploadScreenshots.BackColor = System.Drawing.Color.White;
            this.uploadScreenshots.Checked = true;
            this.uploadScreenshots.CheckOnClick = true;
            this.uploadScreenshots.CheckState = System.Windows.Forms.CheckState.Checked;
            this.uploadScreenshots.ForeColor = System.Drawing.Color.Black;
            this.uploadScreenshots.Name = "uploadScreenshots";
            this.uploadScreenshots.Size = new System.Drawing.Size(242, 22);
            this.uploadScreenshots.Text = "Upload screenshots to Imgur";
            // 
            // openSideWindow
            // 
            this.openSideWindow.BackColor = System.Drawing.Color.White;
            this.openSideWindow.Checked = true;
            this.openSideWindow.CheckOnClick = true;
            this.openSideWindow.CheckState = System.Windows.Forms.CheckState.Checked;
            this.openSideWindow.ForeColor = System.Drawing.Color.Black;
            this.openSideWindow.Name = "openSideWindow";
            this.openSideWindow.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.openSideWindow.Size = new System.Drawing.Size(242, 22);
            this.openSideWindow.Text = "Open side window";
            this.openSideWindow.Visible = false;
            this.openSideWindow.Click += new System.EventHandler(this.openSideWindow_Click);
            // 
            // enableSecondScreen
            // 
            this.enableSecondScreen.Name = "enableSecondScreen";
            this.enableSecondScreen.Size = new System.Drawing.Size(242, 22);
            this.enableSecondScreen.Text = "Enable second screen";
            this.enableSecondScreen.Click += new System.EventHandler(this.enableSecondScreen_Click);
            // 
            // changeViewToolStrip
            // 
            this.changeViewToolStrip.Name = "changeViewToolStrip";
            this.changeViewToolStrip.Size = new System.Drawing.Size(102, 20);
            this.changeViewToolStrip.Text = "Display Settings";
            this.changeViewToolStrip.Click += new System.EventHandler(this.changeViewToolStrip_Click);
            // 
            // equipmentToolStripMenuItem
            // 
            this.equipmentToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stageKitToolStripMenuItem,
            this.microphoneToolStripMenuItem,
            this.bluetoothAVOffset});
            this.equipmentToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.equipmentToolStripMenuItem.Name = "equipmentToolStripMenuItem";
            this.equipmentToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.equipmentToolStripMenuItem.Text = "&Equipment";
            // 
            // stageKitToolStripMenuItem
            // 
            this.stageKitToolStripMenuItem.CheckOnClick = true;
            this.stageKitToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.controller1,
            this.controller2,
            this.controller3,
            this.controller4,
            this.toolStripMenuItem8,
            this.useLEDs,
            this.useStrobe,
            this.useFogger});
            this.stageKitToolStripMenuItem.Name = "stageKitToolStripMenuItem";
            this.stageKitToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.stageKitToolStripMenuItem.Text = "Enable Stage Kit";
            this.stageKitToolStripMenuItem.CheckedChanged += new System.EventHandler(this.stageKitToolStripMenuItem_CheckedChanged);
            this.stageKitToolStripMenuItem.Click += new System.EventHandler(this.stageKitToolStripMenuItem_Click);
            // 
            // controller1
            // 
            this.controller1.Enabled = false;
            this.controller1.Name = "controller1";
            this.controller1.Size = new System.Drawing.Size(136, 22);
            this.controller1.Text = "Controller 1";
            this.controller1.Click += new System.EventHandler(this.controller1_Click);
            // 
            // controller2
            // 
            this.controller2.Enabled = false;
            this.controller2.Name = "controller2";
            this.controller2.Size = new System.Drawing.Size(136, 22);
            this.controller2.Text = "Controller 2";
            this.controller2.Click += new System.EventHandler(this.controller2_Click);
            // 
            // controller3
            // 
            this.controller3.Enabled = false;
            this.controller3.Name = "controller3";
            this.controller3.Size = new System.Drawing.Size(136, 22);
            this.controller3.Text = "Controller 3";
            this.controller3.Click += new System.EventHandler(this.controller3_Click);
            // 
            // controller4
            // 
            this.controller4.Enabled = false;
            this.controller4.Name = "controller4";
            this.controller4.Size = new System.Drawing.Size(136, 22);
            this.controller4.Text = "Controller 4";
            this.controller4.Click += new System.EventHandler(this.controller4_Click);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(133, 6);
            // 
            // useLEDs
            // 
            this.useLEDs.Checked = true;
            this.useLEDs.CheckOnClick = true;
            this.useLEDs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.useLEDs.Name = "useLEDs";
            this.useLEDs.Size = new System.Drawing.Size(136, 22);
            this.useLEDs.Text = "Use LEDs";
            // 
            // useStrobe
            // 
            this.useStrobe.Checked = true;
            this.useStrobe.CheckOnClick = true;
            this.useStrobe.CheckState = System.Windows.Forms.CheckState.Checked;
            this.useStrobe.Name = "useStrobe";
            this.useStrobe.Size = new System.Drawing.Size(136, 22);
            this.useStrobe.Text = "Use strobe";
            // 
            // useFogger
            // 
            this.useFogger.CheckOnClick = true;
            this.useFogger.Name = "useFogger";
            this.useFogger.Size = new System.Drawing.Size(136, 22);
            this.useFogger.Text = "Use fogger";
            // 
            // microphoneToolStripMenuItem
            // 
            this.microphoneToolStripMenuItem.Name = "microphoneToolStripMenuItem";
            this.microphoneToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.microphoneToolStripMenuItem.Text = "Microphone Control";
            this.microphoneToolStripMenuItem.Click += new System.EventHandler(this.microphoneToolStripMenuItem_Click);
            // 
            // bluetoothAVOffset
            // 
            this.bluetoothAVOffset.Name = "bluetoothAVOffset";
            this.bluetoothAVOffset.Size = new System.Drawing.Size(182, 22);
            this.bluetoothAVOffset.Text = "Bluetooth AV Offset";
            this.bluetoothAVOffset.Click += new System.EventHandler(this.bluetoothAVOffset_Click);
            // 
            // nautilusToolStripMenuItem
            // 
            this.nautilusToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setNautilusPath,
            this.toolStripMenuItem12,
            this.sendToVisualizer,
            this.sendToFileAnalyzer,
            this.sendToAudioAnalyzer,
            this.sendToCONExplorer});
            this.nautilusToolStripMenuItem.Name = "nautilusToolStripMenuItem";
            this.nautilusToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.nautilusToolStripMenuItem.Text = "Nautilus";
            this.nautilusToolStripMenuItem.Visible = false;
            // 
            // setNautilusPath
            // 
            this.setNautilusPath.Name = "setNautilusPath";
            this.setNautilusPath.Size = new System.Drawing.Size(197, 22);
            this.setNautilusPath.Text = "Set Nautilus path...";
            this.setNautilusPath.Click += new System.EventHandler(this.setNautilusPath_Click);
            // 
            // toolStripMenuItem12
            // 
            this.toolStripMenuItem12.Name = "toolStripMenuItem12";
            this.toolStripMenuItem12.Size = new System.Drawing.Size(194, 6);
            // 
            // sendToVisualizer
            // 
            this.sendToVisualizer.Enabled = false;
            this.sendToVisualizer.Name = "sendToVisualizer";
            this.sendToVisualizer.Size = new System.Drawing.Size(197, 22);
            this.sendToVisualizer.Text = "Send to Visualizer";
            this.sendToVisualizer.Click += new System.EventHandler(this.sendToVisualizer_Click);
            // 
            // sendToFileAnalyzer
            // 
            this.sendToFileAnalyzer.Enabled = false;
            this.sendToFileAnalyzer.Name = "sendToFileAnalyzer";
            this.sendToFileAnalyzer.Size = new System.Drawing.Size(197, 22);
            this.sendToFileAnalyzer.Text = "Send to File Analyzer";
            this.sendToFileAnalyzer.Click += new System.EventHandler(this.sendToFileAnalyzer_Click);
            // 
            // sendToAudioAnalyzer
            // 
            this.sendToAudioAnalyzer.Enabled = false;
            this.sendToAudioAnalyzer.Name = "sendToAudioAnalyzer";
            this.sendToAudioAnalyzer.Size = new System.Drawing.Size(197, 22);
            this.sendToAudioAnalyzer.Text = "Send to Audio Analyzer";
            this.sendToAudioAnalyzer.Click += new System.EventHandler(this.sendToAudioAnalyzer_Click);
            // 
            // sendToCONExplorer
            // 
            this.sendToCONExplorer.Enabled = false;
            this.sendToCONExplorer.Name = "sendToCONExplorer";
            this.sendToCONExplorer.Size = new System.Drawing.Size(197, 22);
            this.sendToCONExplorer.Text = "Send to CON Explorer";
            this.sendToCONExplorer.Click += new System.EventHandler(this.sendToCONExplorer_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.howToUseToolStripMenuItem,
            this.checkForUpdates,
            this.viewChangeLog,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // howToUseToolStripMenuItem
            // 
            this.howToUseToolStripMenuItem.BackColor = System.Drawing.Color.White;
            this.howToUseToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.howToUseToolStripMenuItem.Name = "howToUseToolStripMenuItem";
            this.howToUseToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.howToUseToolStripMenuItem.Text = "How to Use";
            this.howToUseToolStripMenuItem.Click += new System.EventHandler(this.howToUseToolStripMenuItem_Click);
            // 
            // checkForUpdates
            // 
            this.checkForUpdates.BackColor = System.Drawing.Color.White;
            this.checkForUpdates.ForeColor = System.Drawing.Color.Black;
            this.checkForUpdates.Name = "checkForUpdates";
            this.checkForUpdates.Size = new System.Drawing.Size(171, 22);
            this.checkForUpdates.Text = "Check for Updates";
            this.checkForUpdates.Click += new System.EventHandler(this.checkForUpdates_Click);
            // 
            // viewChangeLog
            // 
            this.viewChangeLog.BackColor = System.Drawing.Color.White;
            this.viewChangeLog.ForeColor = System.Drawing.Color.Black;
            this.viewChangeLog.Name = "viewChangeLog";
            this.viewChangeLog.Size = new System.Drawing.Size(171, 22);
            this.viewChangeLog.Text = "Change Log";
            this.viewChangeLog.Click += new System.EventHandler(this.viewChangeLog_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.BackColor = System.Drawing.Color.White;
            this.aboutToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // VisualsContextMenu
            // 
            this.VisualsContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.displayBackgroundVideo,
            this.toolStripMenuItem2,
            this.displayAlbumArt,
            this.displayAudioSpectrum,
            this.displayKaraokeMode,
            this.chartVisualsToolStripMenuItem});
            this.VisualsContextMenu.Name = "VisualsContextMenu";
            this.VisualsContextMenu.Size = new System.Drawing.Size(205, 120);
            this.VisualsContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.VisualsContextMenu_Opening);
            // 
            // displayBackgroundVideo
            // 
            this.displayBackgroundVideo.BackColor = System.Drawing.Color.White;
            this.displayBackgroundVideo.Checked = true;
            this.displayBackgroundVideo.CheckOnClick = true;
            this.displayBackgroundVideo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.displayBackgroundVideo.ForeColor = System.Drawing.Color.Black;
            this.displayBackgroundVideo.Name = "displayBackgroundVideo";
            this.displayBackgroundVideo.Size = new System.Drawing.Size(204, 22);
            this.displayBackgroundVideo.Text = "Play Background Videos";
            this.displayBackgroundVideo.Visible = false;
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(201, 6);
            this.toolStripMenuItem2.Visible = false;
            // 
            // displayAlbumArt
            // 
            this.displayAlbumArt.BackColor = System.Drawing.Color.White;
            this.displayAlbumArt.ForeColor = System.Drawing.Color.Black;
            this.displayAlbumArt.Name = "displayAlbumArt";
            this.displayAlbumArt.Size = new System.Drawing.Size(204, 22);
            this.displayAlbumArt.Text = "Display: Album Art";
            this.displayAlbumArt.Visible = false;
            this.displayAlbumArt.Click += new System.EventHandler(this.displayAlbumArt_Click);
            // 
            // displayAudioSpectrum
            // 
            this.displayAudioSpectrum.BackColor = System.Drawing.Color.White;
            this.displayAudioSpectrum.ForeColor = System.Drawing.Color.Black;
            this.displayAudioSpectrum.Name = "displayAudioSpectrum";
            this.displayAudioSpectrum.Size = new System.Drawing.Size(204, 22);
            this.displayAudioSpectrum.Text = "Display: Audio Spectrum";
            this.displayAudioSpectrum.Visible = false;
            this.displayAudioSpectrum.Click += new System.EventHandler(this.displayAudioSpectrum_Click);
            // 
            // displayKaraokeMode
            // 
            this.displayKaraokeMode.BackColor = System.Drawing.Color.White;
            this.displayKaraokeMode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.classicKaraokeMode,
            this.cPlayerStyle,
            this.rockBandKaraoke,
            this.toolStripMenuItem13,
            this.selectBackgroundColor,
            this.selectLyricColor,
            this.selectHighlightColor,
            this.selectHarmonyTextColor,
            this.selectHarmonyHighlightColor,
            this.selectHarmony3TextColor,
            this.selectHarmony3HighlightColor,
            this.toolStripMenuItem14,
            this.changeStageBackground,
            this.toolStripMenuItem16,
            this.restoreDefaultsToolStripMenuItem});
            this.displayKaraokeMode.ForeColor = System.Drawing.Color.Black;
            this.displayKaraokeMode.Name = "displayKaraokeMode";
            this.displayKaraokeMode.Size = new System.Drawing.Size(204, 22);
            this.displayKaraokeMode.Text = "Display: Karaoke Mode";
            this.displayKaraokeMode.Visible = false;
            // 
            // classicKaraokeMode
            // 
            this.classicKaraokeMode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.solidColorBackground,
            this.staticBackground2,
            this.animatedBackground2,
            this.toolStripMenuItem15,
            this.forceSoloVocals,
            this.forceTwoPartHarmonies});
            this.classicKaraokeMode.Name = "classicKaraokeMode";
            this.classicKaraokeMode.Size = new System.Drawing.Size(254, 22);
            this.classicKaraokeMode.Text = "Classic Karaoke";
            this.classicKaraokeMode.Click += new System.EventHandler(this.classicKaraokeMode_Click);
            // 
            // solidColorBackground
            // 
            this.solidColorBackground.Checked = true;
            this.solidColorBackground.CheckState = System.Windows.Forms.CheckState.Checked;
            this.solidColorBackground.Name = "solidColorBackground";
            this.solidColorBackground.Size = new System.Drawing.Size(199, 22);
            this.solidColorBackground.Text = "Solid Color Background";
            this.solidColorBackground.Click += new System.EventHandler(this.solidColorBackgroundToolStripMenuItem_Click);
            // 
            // staticBackground2
            // 
            this.staticBackground2.CheckOnClick = true;
            this.staticBackground2.Name = "staticBackground2";
            this.staticBackground2.Size = new System.Drawing.Size(199, 22);
            this.staticBackground2.Text = "Static Background";
            this.staticBackground2.Click += new System.EventHandler(this.enableBackgroundImage_Click);
            // 
            // animatedBackground2
            // 
            this.animatedBackground2.Name = "animatedBackground2";
            this.animatedBackground2.Size = new System.Drawing.Size(199, 22);
            this.animatedBackground2.Text = "Animated Background";
            this.animatedBackground2.Click += new System.EventHandler(this.animatedBackground2_Click);
            // 
            // toolStripMenuItem15
            // 
            this.toolStripMenuItem15.Name = "toolStripMenuItem15";
            this.toolStripMenuItem15.Size = new System.Drawing.Size(196, 6);
            // 
            // forceSoloVocals
            // 
            this.forceSoloVocals.Name = "forceSoloVocals";
            this.forceSoloVocals.Size = new System.Drawing.Size(199, 22);
            this.forceSoloVocals.Text = "Force Solo Vocals";
            this.forceSoloVocals.Click += new System.EventHandler(this.forceSoloVocals_Click);
            // 
            // forceTwoPartHarmonies
            // 
            this.forceTwoPartHarmonies.Name = "forceTwoPartHarmonies";
            this.forceTwoPartHarmonies.Size = new System.Drawing.Size(199, 22);
            this.forceTwoPartHarmonies.Text = "Force 2-Part Harmonies";
            this.forceTwoPartHarmonies.Click += new System.EventHandler(this.forceTwoPartHarmonies_Click);
            // 
            // cPlayerStyle
            // 
            this.cPlayerStyle.Name = "cPlayerStyle";
            this.cPlayerStyle.Size = new System.Drawing.Size(254, 22);
            this.cPlayerStyle.Text = "cPlayer Karaoke";
            this.cPlayerStyle.Click += new System.EventHandler(this.cPlayerStyle_Click);
            // 
            // rockBandKaraoke
            // 
            this.rockBandKaraoke.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.animatedBackground,
            this.staticBackground});
            this.rockBandKaraoke.Name = "rockBandKaraoke";
            this.rockBandKaraoke.Size = new System.Drawing.Size(254, 22);
            this.rockBandKaraoke.Text = "Rock Band Karaoke";
            this.rockBandKaraoke.Click += new System.EventHandler(this.rockBandKaraoke_Click);
            // 
            // animatedBackground
            // 
            this.animatedBackground.Checked = true;
            this.animatedBackground.CheckState = System.Windows.Forms.CheckState.Checked;
            this.animatedBackground.Name = "animatedBackground";
            this.animatedBackground.Size = new System.Drawing.Size(193, 22);
            this.animatedBackground.Text = "Animated Background";
            this.animatedBackground.Click += new System.EventHandler(this.animatedBackground_Click);
            // 
            // staticBackground
            // 
            this.staticBackground.Name = "staticBackground";
            this.staticBackground.Size = new System.Drawing.Size(193, 22);
            this.staticBackground.Text = "Static Background";
            this.staticBackground.Click += new System.EventHandler(this.staticBackground_Click);
            // 
            // toolStripMenuItem13
            // 
            this.toolStripMenuItem13.Name = "toolStripMenuItem13";
            this.toolStripMenuItem13.Size = new System.Drawing.Size(251, 6);
            // 
            // selectBackgroundColor
            // 
            this.selectBackgroundColor.Name = "selectBackgroundColor";
            this.selectBackgroundColor.Size = new System.Drawing.Size(254, 22);
            this.selectBackgroundColor.Text = "Background Color";
            this.selectBackgroundColor.Visible = false;
            this.selectBackgroundColor.Click += new System.EventHandler(this.selectBackgroundColor_Click);
            // 
            // selectLyricColor
            // 
            this.selectLyricColor.Name = "selectLyricColor";
            this.selectLyricColor.Size = new System.Drawing.Size(254, 22);
            this.selectLyricColor.Text = "Lead / Harmony 1 Text Color";
            this.selectLyricColor.Visible = false;
            this.selectLyricColor.Click += new System.EventHandler(this.selectLyricColor_Click);
            // 
            // selectHighlightColor
            // 
            this.selectHighlightColor.Name = "selectHighlightColor";
            this.selectHighlightColor.Size = new System.Drawing.Size(254, 22);
            this.selectHighlightColor.Text = "Lead / Harmony 1 Highlight Color";
            this.selectHighlightColor.Visible = false;
            this.selectHighlightColor.Click += new System.EventHandler(this.selectHighlightColor_Click);
            // 
            // selectHarmonyTextColor
            // 
            this.selectHarmonyTextColor.Name = "selectHarmonyTextColor";
            this.selectHarmonyTextColor.Size = new System.Drawing.Size(254, 22);
            this.selectHarmonyTextColor.Text = "Harmony 2 Text Color";
            this.selectHarmonyTextColor.Visible = false;
            this.selectHarmonyTextColor.Click += new System.EventHandler(this.selectHarmonyTextColor_Click);
            // 
            // selectHarmonyHighlightColor
            // 
            this.selectHarmonyHighlightColor.Name = "selectHarmonyHighlightColor";
            this.selectHarmonyHighlightColor.Size = new System.Drawing.Size(254, 22);
            this.selectHarmonyHighlightColor.Text = "Harmony 2 Highlight Color";
            this.selectHarmonyHighlightColor.Visible = false;
            this.selectHarmonyHighlightColor.Click += new System.EventHandler(this.selectHarmonyHighlightColor_Click);
            // 
            // selectHarmony3TextColor
            // 
            this.selectHarmony3TextColor.Name = "selectHarmony3TextColor";
            this.selectHarmony3TextColor.Size = new System.Drawing.Size(254, 22);
            this.selectHarmony3TextColor.Text = "Harmony 3 Text Color";
            this.selectHarmony3TextColor.Visible = false;
            this.selectHarmony3TextColor.Click += new System.EventHandler(this.selectHarmony3TextColor_Click);
            // 
            // selectHarmony3HighlightColor
            // 
            this.selectHarmony3HighlightColor.Name = "selectHarmony3HighlightColor";
            this.selectHarmony3HighlightColor.Size = new System.Drawing.Size(254, 22);
            this.selectHarmony3HighlightColor.Text = "Harmony 3 Highlight Color";
            this.selectHarmony3HighlightColor.Visible = false;
            this.selectHarmony3HighlightColor.Click += new System.EventHandler(this.selectHarmony3HighlightColor_Click);
            // 
            // toolStripMenuItem14
            // 
            this.toolStripMenuItem14.Name = "toolStripMenuItem14";
            this.toolStripMenuItem14.Size = new System.Drawing.Size(251, 6);
            // 
            // changeStageBackground
            // 
            this.changeStageBackground.Name = "changeStageBackground";
            this.changeStageBackground.Size = new System.Drawing.Size(254, 22);
            this.changeStageBackground.Text = "Change Stage Background";
            this.changeStageBackground.Click += new System.EventHandler(this.changeStageBackground_Click);
            // 
            // toolStripMenuItem16
            // 
            this.toolStripMenuItem16.Name = "toolStripMenuItem16";
            this.toolStripMenuItem16.Size = new System.Drawing.Size(251, 6);
            // 
            // restoreDefaultsToolStripMenuItem
            // 
            this.restoreDefaultsToolStripMenuItem.Name = "restoreDefaultsToolStripMenuItem";
            this.restoreDefaultsToolStripMenuItem.Size = new System.Drawing.Size(254, 22);
            this.restoreDefaultsToolStripMenuItem.Text = "Restore Defaults";
            this.restoreDefaultsToolStripMenuItem.Click += new System.EventHandler(this.restoreDefaultsToolStripMenuItem_Click);
            // 
            // chartVisualsToolStripMenuItem
            // 
            this.chartVisualsToolStripMenuItem.BackColor = System.Drawing.Color.White;
            this.chartVisualsToolStripMenuItem.Checked = true;
            this.chartVisualsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chartVisualsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rBStyle,
            this.chartVertical,
            this.chartSnippet,
            this.chartFull});
            this.chartVisualsToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.chartVisualsToolStripMenuItem.Name = "chartVisualsToolStripMenuItem";
            this.chartVisualsToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.chartVisualsToolStripMenuItem.Text = "Display: Chart Visuals";
            this.chartVisualsToolStripMenuItem.Visible = false;
            // 
            // rBStyle
            // 
            this.rBStyle.Checked = true;
            this.rBStyle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.rBStyle.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.useBackgroundVideos,
            this.useBackgroundImages});
            this.rBStyle.Name = "rBStyle";
            this.rBStyle.Size = new System.Drawing.Size(140, 22);
            this.rBStyle.Text = "RB Style";
            this.rBStyle.Click += new System.EventHandler(this.rBStyle_Click);
            // 
            // useBackgroundVideos
            // 
            this.useBackgroundVideos.Checked = true;
            this.useBackgroundVideos.CheckState = System.Windows.Forms.CheckState.Checked;
            this.useBackgroundVideos.Name = "useBackgroundVideos";
            this.useBackgroundVideos.Size = new System.Drawing.Size(201, 22);
            this.useBackgroundVideos.Text = "Use Background Videos";
            this.useBackgroundVideos.Click += new System.EventHandler(this.useBackgroundVideos_Click);
            // 
            // useBackgroundImages
            // 
            this.useBackgroundImages.Name = "useBackgroundImages";
            this.useBackgroundImages.Size = new System.Drawing.Size(201, 22);
            this.useBackgroundImages.Text = "Use Background Images";
            this.useBackgroundImages.Click += new System.EventHandler(this.useBackgroundImages_Click);
            // 
            // chartVertical
            // 
            this.chartVertical.Name = "chartVertical";
            this.chartVertical.Size = new System.Drawing.Size(140, 22);
            this.chartVertical.Text = "Vertical Style";
            this.chartVertical.Click += new System.EventHandler(this.chartVertical_Click);
            // 
            // chartSnippet
            // 
            this.chartSnippet.BackColor = System.Drawing.Color.White;
            this.chartSnippet.ForeColor = System.Drawing.Color.Black;
            this.chartSnippet.Name = "chartSnippet";
            this.chartSnippet.Size = new System.Drawing.Size(140, 22);
            this.chartSnippet.Text = "MIDI Style";
            this.chartSnippet.Click += new System.EventHandler(this.chartSnippet_Click);
            // 
            // chartFull
            // 
            this.chartFull.BackColor = System.Drawing.Color.White;
            this.chartFull.Enabled = false;
            this.chartFull.ForeColor = System.Drawing.Color.Black;
            this.chartFull.Name = "chartFull";
            this.chartFull.Size = new System.Drawing.Size(140, 22);
            this.chartFull.Text = "Chart: Full";
            this.chartFull.Visible = false;
            this.chartFull.Click += new System.EventHandler(this.chartFull_Click);
            // 
            // lblAuthor
            // 
            this.lblAuthor.AutoEllipsis = true;
            this.lblAuthor.BackColor = System.Drawing.Color.Transparent;
            this.lblAuthor.ForeColor = System.Drawing.Color.Black;
            this.lblAuthor.Location = new System.Drawing.Point(61, 195);
            this.lblAuthor.Name = "lblAuthor";
            this.lblAuthor.Size = new System.Drawing.Size(209, 21);
            this.lblAuthor.TabIndex = 0;
            this.lblAuthor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelPlaying
            // 
            this.panelPlaying.AllowDrop = true;
            this.panelPlaying.BackColor = System.Drawing.Color.AliceBlue;
            this.panelPlaying.Controls.Add(this.picSecondScreen);
            this.panelPlaying.Controls.Add(this.lblArtist);
            this.panelPlaying.Controls.Add(this.picShuffle);
            this.panelPlaying.Controls.Add(this.picLoop);
            this.panelPlaying.Controls.Add(this.picNext);
            this.panelPlaying.Controls.Add(this.picPause);
            this.panelPlaying.Controls.Add(this.picStop);
            this.panelPlaying.Controls.Add(this.picPlay);
            this.panelPlaying.Controls.Add(this.picRandom);
            this.panelPlaying.Controls.Add(this.picVolume);
            this.panelPlaying.Controls.Add(this.lblAuthor);
            this.panelPlaying.Controls.Add(this.panelSlider);
            this.panelPlaying.Controls.Add(this.panelLine);
            this.panelPlaying.Controls.Add(this.lblTime);
            this.panelPlaying.Controls.Add(this.lblDuration);
            this.panelPlaying.Controls.Add(this.lblYear);
            this.panelPlaying.Controls.Add(this.lblTrack);
            this.panelPlaying.Controls.Add(this.lblGenre);
            this.panelPlaying.Controls.Add(this.lblAlbum);
            this.panelPlaying.Controls.Add(this.lblSong);
            this.panelPlaying.Controls.Add(this.picPreview);
            this.panelPlaying.ForeColor = System.Drawing.Color.Black;
            this.panelPlaying.Location = new System.Drawing.Point(8, 27);
            this.panelPlaying.Name = "panelPlaying";
            this.panelPlaying.Size = new System.Drawing.Size(380, 221);
            this.panelPlaying.TabIndex = 2;
            this.panelPlaying.DragDrop += new System.Windows.Forms.DragEventHandler(this.lstPlaylist_DragDrop);
            this.panelPlaying.DragEnter += new System.Windows.Forms.DragEventHandler(this.lstPlaylist_DragEnter);
            this.panelPlaying.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseDown);
            this.panelPlaying.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseUp);
            // 
            // picSecondScreen
            // 
            this.picSecondScreen.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picSecondScreen.Image = global::cPlayer.Properties.Resources.doublescreen_disabled;
            this.picSecondScreen.Location = new System.Drawing.Point(343, 4);
            this.picSecondScreen.Name = "picSecondScreen";
            this.picSecondScreen.Size = new System.Drawing.Size(33, 26);
            this.picSecondScreen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picSecondScreen.TabIndex = 57;
            this.picSecondScreen.TabStop = false;
            this.picSecondScreen.Tag = "disabled";
            this.toolTip1.SetToolTip(this.picSecondScreen, "Click to enable second screen");
            this.picSecondScreen.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picSecondScreen_MouseClick);
            // 
            // lblArtist
            // 
            this.lblArtist.AutoEllipsis = true;
            this.lblArtist.BackColor = System.Drawing.Color.Transparent;
            this.lblArtist.ForeColor = System.Drawing.Color.Black;
            this.lblArtist.Location = new System.Drawing.Point(111, 17);
            this.lblArtist.Name = "lblArtist";
            this.lblArtist.Size = new System.Drawing.Size(261, 23);
            this.lblArtist.TabIndex = 1;
            this.lblArtist.Text = "Artist:";
            this.lblArtist.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblArtist.UseMnemonic = false;
            // 
            // picShuffle
            // 
            this.picShuffle.BackColor = System.Drawing.Color.AliceBlue;
            this.picShuffle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picShuffle.Image = global::cPlayer.Properties.Resources.icon_shuffle_disabled;
            this.picShuffle.Location = new System.Drawing.Point(265, 126);
            this.picShuffle.Name = "picShuffle";
            this.picShuffle.Size = new System.Drawing.Size(50, 50);
            this.picShuffle.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picShuffle.TabIndex = 56;
            this.picShuffle.TabStop = false;
            this.picShuffle.Tag = "noshuffle";
            this.toolTip1.SetToolTip(this.picShuffle, "Enable track shuffling");
            this.picShuffle.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picShuffle_MouseClick);
            // 
            // picLoop
            // 
            this.picLoop.BackColor = System.Drawing.Color.AliceBlue;
            this.picLoop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picLoop.Image = global::cPlayer.Properties.Resources.icon_loop_disabled1;
            this.picLoop.Location = new System.Drawing.Point(215, 126);
            this.picLoop.Name = "picLoop";
            this.picLoop.Size = new System.Drawing.Size(50, 50);
            this.picLoop.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picLoop.TabIndex = 55;
            this.picLoop.TabStop = false;
            this.picLoop.Tag = "noloop";
            this.toolTip1.SetToolTip(this.picLoop, "Enable track looping");
            this.picLoop.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picLoop_MouseClick);
            // 
            // picNext
            // 
            this.picNext.BackColor = System.Drawing.Color.AliceBlue;
            this.picNext.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picNext.Enabled = false;
            this.picNext.Image = global::cPlayer.Properties.Resources.icon_next;
            this.picNext.Location = new System.Drawing.Point(165, 126);
            this.picNext.Name = "picNext";
            this.picNext.Size = new System.Drawing.Size(50, 50);
            this.picNext.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picNext.TabIndex = 54;
            this.picNext.TabStop = false;
            this.toolTip1.SetToolTip(this.picNext, "Click to skip to next song");
            this.picNext.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picNext_MouseClick);
            // 
            // picPause
            // 
            this.picPause.BackColor = System.Drawing.Color.AliceBlue;
            this.picPause.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picPause.Enabled = false;
            this.picPause.Image = global::cPlayer.Properties.Resources.icon_pause;
            this.picPause.Location = new System.Drawing.Point(65, 126);
            this.picPause.Name = "picPause";
            this.picPause.Size = new System.Drawing.Size(50, 50);
            this.picPause.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picPause.TabIndex = 53;
            this.picPause.TabStop = false;
            this.toolTip1.SetToolTip(this.picPause, "Click to pause playback");
            this.picPause.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picPause_MouseClick);
            // 
            // picStop
            // 
            this.picStop.BackColor = System.Drawing.Color.AliceBlue;
            this.picStop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picStop.Enabled = false;
            this.picStop.Image = global::cPlayer.Properties.Resources.icon_stop;
            this.picStop.Location = new System.Drawing.Point(115, 126);
            this.picStop.Name = "picStop";
            this.picStop.Size = new System.Drawing.Size(50, 50);
            this.picStop.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picStop.TabIndex = 52;
            this.picStop.TabStop = false;
            this.toolTip1.SetToolTip(this.picStop, "Click to stop playback");
            this.picStop.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picStop_MouseClick);
            // 
            // picPlay
            // 
            this.picPlay.BackColor = System.Drawing.Color.AliceBlue;
            this.picPlay.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picPlay.Image = global::cPlayer.Properties.Resources.icon_play;
            this.picPlay.Location = new System.Drawing.Point(15, 126);
            this.picPlay.Name = "picPlay";
            this.picPlay.Size = new System.Drawing.Size(50, 50);
            this.picPlay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picPlay.TabIndex = 51;
            this.picPlay.TabStop = false;
            this.toolTip1.SetToolTip(this.picPlay, "Click to begin playback");
            this.picPlay.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picPlay_MouseClick);
            // 
            // picRandom
            // 
            this.picRandom.BackColor = System.Drawing.Color.AliceBlue;
            this.picRandom.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picRandom.Image = global::cPlayer.Properties.Resources.pickrandom;
            this.picRandom.Location = new System.Drawing.Point(325, 120);
            this.picRandom.Name = "picRandom";
            this.picRandom.Size = new System.Drawing.Size(50, 50);
            this.picRandom.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picRandom.TabIndex = 50;
            this.picRandom.TabStop = false;
            this.toolTip1.SetToolTip(this.picRandom, "Pick a random song");
            this.picRandom.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picRandom_MouseClick);
            // 
            // picVolume
            // 
            this.picVolume.BackColor = System.Drawing.Color.AliceBlue;
            this.picVolume.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picVolume.Image = global::cPlayer.Properties.Resources.icon_speaker;
            this.picVolume.Location = new System.Drawing.Point(330, 171);
            this.picVolume.Name = "picVolume";
            this.picVolume.Size = new System.Drawing.Size(45, 45);
            this.picVolume.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picVolume.TabIndex = 49;
            this.picVolume.TabStop = false;
            this.toolTip1.SetToolTip(this.picVolume, "Click to adjust volume");
            this.picVolume.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picVolume_MouseClick);
            // 
            // panelSlider
            // 
            this.panelSlider.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(209)))), ((int)(((byte)(209)))));
            this.panelSlider.Cursor = System.Windows.Forms.Cursors.Hand;
            this.panelSlider.Location = new System.Drawing.Point(10, 180);
            this.panelSlider.Name = "panelSlider";
            this.panelSlider.Size = new System.Drawing.Size(14, 14);
            this.panelSlider.TabIndex = 43;
            this.toolTip1.SetToolTip(this.panelSlider, "Click to drag");
            this.panelSlider.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelSlider_MouseDown);
            this.panelSlider.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelSlider_MouseMove);
            this.panelSlider.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelSlider_MouseUp);
            // 
            // panelLine
            // 
            this.panelLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.panelLine.Location = new System.Drawing.Point(10, 184);
            this.panelLine.Name = "panelLine";
            this.panelLine.Size = new System.Drawing.Size(310, 6);
            this.panelLine.TabIndex = 42;
            this.toolTip1.SetToolTip(this.panelLine, "Click to play from here");
            this.panelLine.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panelLine_MouseClick);
            this.panelLine.MouseHover += new System.EventHandler(this.panelLine_MouseHover);
            // 
            // lblTime
            // 
            this.lblTime.BackColor = System.Drawing.Color.Transparent;
            this.lblTime.ForeColor = System.Drawing.Color.Black;
            this.lblTime.Location = new System.Drawing.Point(7, 197);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(48, 16);
            this.lblTime.TabIndex = 41;
            this.lblTime.Text = "0:00";
            this.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip1.SetToolTip(this.lblTime, "Current position");
            // 
            // lblDuration
            // 
            this.lblDuration.BackColor = System.Drawing.Color.Transparent;
            this.lblDuration.ForeColor = System.Drawing.Color.Black;
            this.lblDuration.Location = new System.Drawing.Point(276, 197);
            this.lblDuration.Name = "lblDuration";
            this.lblDuration.Size = new System.Drawing.Size(48, 16);
            this.lblDuration.TabIndex = 40;
            this.lblDuration.Text = "0:00";
            this.lblDuration.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip1.SetToolTip(this.lblDuration, "Song length");
            // 
            // lblYear
            // 
            this.lblYear.AutoEllipsis = true;
            this.lblYear.BackColor = System.Drawing.Color.Transparent;
            this.lblYear.ForeColor = System.Drawing.Color.Black;
            this.lblYear.Location = new System.Drawing.Point(307, 101);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(65, 23);
            this.lblYear.TabIndex = 6;
            this.lblYear.Text = "Year:";
            this.lblYear.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblYear.UseMnemonic = false;
            // 
            // lblTrack
            // 
            this.lblTrack.AutoEllipsis = true;
            this.lblTrack.BackColor = System.Drawing.Color.Transparent;
            this.lblTrack.ForeColor = System.Drawing.Color.Black;
            this.lblTrack.Location = new System.Drawing.Point(191, 101);
            this.lblTrack.Name = "lblTrack";
            this.lblTrack.Size = new System.Drawing.Size(70, 23);
            this.lblTrack.TabIndex = 5;
            this.lblTrack.Text = "Track #:";
            this.lblTrack.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTrack.UseMnemonic = false;
            // 
            // lblGenre
            // 
            this.lblGenre.AutoEllipsis = true;
            this.lblGenre.BackColor = System.Drawing.Color.Transparent;
            this.lblGenre.ForeColor = System.Drawing.Color.Black;
            this.lblGenre.Location = new System.Drawing.Point(7, 101);
            this.lblGenre.Name = "lblGenre";
            this.lblGenre.Size = new System.Drawing.Size(173, 23);
            this.lblGenre.TabIndex = 4;
            this.lblGenre.Text = "Genre:";
            this.lblGenre.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblGenre.UseMnemonic = false;
            // 
            // lblAlbum
            // 
            this.lblAlbum.AutoEllipsis = true;
            this.lblAlbum.BackColor = System.Drawing.Color.Transparent;
            this.lblAlbum.ForeColor = System.Drawing.Color.Black;
            this.lblAlbum.Location = new System.Drawing.Point(111, 73);
            this.lblAlbum.Name = "lblAlbum";
            this.lblAlbum.Size = new System.Drawing.Size(261, 23);
            this.lblAlbum.TabIndex = 3;
            this.lblAlbum.Text = "Album:";
            this.lblAlbum.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblAlbum.UseMnemonic = false;
            // 
            // lblSong
            // 
            this.lblSong.AutoEllipsis = true;
            this.lblSong.BackColor = System.Drawing.Color.Transparent;
            this.lblSong.ForeColor = System.Drawing.Color.Black;
            this.lblSong.Location = new System.Drawing.Point(111, 44);
            this.lblSong.Name = "lblSong";
            this.lblSong.Size = new System.Drawing.Size(261, 23);
            this.lblSong.TabIndex = 2;
            this.lblSong.Text = "Song:";
            this.lblSong.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblSong.UseMnemonic = false;
            // 
            // picPreview
            // 
            this.picPreview.Image = global::cPlayer.Properties.Resources.default_art;
            this.picPreview.Location = new System.Drawing.Point(1, 1);
            this.picPreview.Name = "picPreview";
            this.picPreview.Size = new System.Drawing.Size(100, 100);
            this.picPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picPreview.TabIndex = 0;
            this.picPreview.TabStop = false;
            this.picPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.picPreview_Paint);
            this.picPreview.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picPreview_MouseClick);
            // 
            // panelPlaylist
            // 
            this.panelPlaylist.AllowDrop = true;
            this.panelPlaylist.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panelPlaylist.BackColor = System.Drawing.Color.AliceBlue;
            this.panelPlaylist.Controls.Add(this.picOldies);
            this.panelPlaylist.Controls.Add(this.pic1960s);
            this.panelPlaylist.Controls.Add(this.pic1970s);
            this.panelPlaylist.Controls.Add(this.pic1980s);
            this.panelPlaylist.Controls.Add(this.pic1990s);
            this.panelPlaylist.Controls.Add(this.pic2000s);
            this.panelPlaylist.Controls.Add(this.pic2010s);
            this.panelPlaylist.Controls.Add(this.pic2020s);
            this.panelPlaylist.Controls.Add(this.picFavorites);
            this.panelPlaylist.Controls.Add(this.lstPlaylist);
            this.panelPlaylist.ForeColor = System.Drawing.Color.Black;
            this.panelPlaylist.Location = new System.Drawing.Point(9, 282);
            this.panelPlaylist.Name = "panelPlaylist";
            this.panelPlaylist.Size = new System.Drawing.Size(380, 679);
            this.panelPlaylist.TabIndex = 3;
            this.panelPlaylist.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseDown);
            this.panelPlaylist.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseUp);
            // 
            // picOldies
            // 
            this.picOldies.Cursor = System.Windows.Forms.Cursors.Default;
            this.picOldies.Image = global::cPlayer.Properties.Resources.oldies_enabled;
            this.picOldies.Location = new System.Drawing.Point(264, 244);
            this.picOldies.Name = "picOldies";
            this.picOldies.Size = new System.Drawing.Size(100, 100);
            this.picOldies.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picOldies.TabIndex = 9;
            this.picOldies.TabStop = false;
            this.picOldies.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picOldies_MouseClick);
            // 
            // pic1960s
            // 
            this.pic1960s.Cursor = System.Windows.Forms.Cursors.Default;
            this.pic1960s.Image = global::cPlayer.Properties.Resources._1960s_enabled;
            this.pic1960s.Location = new System.Drawing.Point(140, 244);
            this.pic1960s.Name = "pic1960s";
            this.pic1960s.Size = new System.Drawing.Size(100, 100);
            this.pic1960s.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic1960s.TabIndex = 8;
            this.pic1960s.TabStop = false;
            this.pic1960s.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pic1960s_MouseClick);
            // 
            // pic1970s
            // 
            this.pic1970s.Cursor = System.Windows.Forms.Cursors.Default;
            this.pic1970s.Image = global::cPlayer.Properties.Resources._1970s_enabled;
            this.pic1970s.Location = new System.Drawing.Point(16, 244);
            this.pic1970s.Name = "pic1970s";
            this.pic1970s.Size = new System.Drawing.Size(100, 100);
            this.pic1970s.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic1970s.TabIndex = 7;
            this.pic1970s.TabStop = false;
            this.pic1970s.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pic1970s_MouseClick);
            // 
            // pic1980s
            // 
            this.pic1980s.Cursor = System.Windows.Forms.Cursors.Default;
            this.pic1980s.Image = global::cPlayer.Properties.Resources._1980s_enabled;
            this.pic1980s.Location = new System.Drawing.Point(264, 129);
            this.pic1980s.Name = "pic1980s";
            this.pic1980s.Size = new System.Drawing.Size(100, 100);
            this.pic1980s.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic1980s.TabIndex = 6;
            this.pic1980s.TabStop = false;
            this.pic1980s.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pic1980s_MouseClick);
            // 
            // pic1990s
            // 
            this.pic1990s.Cursor = System.Windows.Forms.Cursors.Default;
            this.pic1990s.Image = global::cPlayer.Properties.Resources._1990s_enabled;
            this.pic1990s.Location = new System.Drawing.Point(140, 129);
            this.pic1990s.Name = "pic1990s";
            this.pic1990s.Size = new System.Drawing.Size(100, 100);
            this.pic1990s.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic1990s.TabIndex = 5;
            this.pic1990s.TabStop = false;
            this.pic1990s.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pic1990s_MouseClick);
            // 
            // pic2000s
            // 
            this.pic2000s.Cursor = System.Windows.Forms.Cursors.Default;
            this.pic2000s.Image = global::cPlayer.Properties.Resources._2000s_enabled;
            this.pic2000s.Location = new System.Drawing.Point(16, 129);
            this.pic2000s.Name = "pic2000s";
            this.pic2000s.Size = new System.Drawing.Size(100, 100);
            this.pic2000s.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic2000s.TabIndex = 4;
            this.pic2000s.TabStop = false;
            this.pic2000s.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pic2000s_MouseClick);
            // 
            // pic2010s
            // 
            this.pic2010s.Cursor = System.Windows.Forms.Cursors.Default;
            this.pic2010s.Image = global::cPlayer.Properties.Resources._2010s_enabled;
            this.pic2010s.Location = new System.Drawing.Point(264, 15);
            this.pic2010s.Name = "pic2010s";
            this.pic2010s.Size = new System.Drawing.Size(100, 100);
            this.pic2010s.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic2010s.TabIndex = 3;
            this.pic2010s.TabStop = false;
            this.pic2010s.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pic2010s_MouseClick);
            // 
            // pic2020s
            // 
            this.pic2020s.Cursor = System.Windows.Forms.Cursors.Default;
            this.pic2020s.Image = global::cPlayer.Properties.Resources._2020s_enabled;
            this.pic2020s.Location = new System.Drawing.Point(140, 15);
            this.pic2020s.Name = "pic2020s";
            this.pic2020s.Size = new System.Drawing.Size(100, 100);
            this.pic2020s.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic2020s.TabIndex = 2;
            this.pic2020s.TabStop = false;
            this.pic2020s.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pic2020s_MouseClick);
            // 
            // picFavorites
            // 
            this.picFavorites.Cursor = System.Windows.Forms.Cursors.Default;
            this.picFavorites.Image = global::cPlayer.Properties.Resources.favorites_disabled;
            this.picFavorites.Location = new System.Drawing.Point(16, 15);
            this.picFavorites.Name = "picFavorites";
            this.picFavorites.Size = new System.Drawing.Size(100, 100);
            this.picFavorites.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picFavorites.TabIndex = 1;
            this.picFavorites.TabStop = false;
            this.picFavorites.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picFavorites_MouseClick);
            // 
            // lstPlaylist
            // 
            this.lstPlaylist.AllowDrop = true;
            this.lstPlaylist.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lstPlaylist.BackColor = System.Drawing.Color.AliceBlue;
            this.lstPlaylist.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstPlaylist.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colIndex,
            this.colSong,
            this.colLength});
            this.lstPlaylist.ContextMenuStrip = this.PlaylistContextMenu;
            this.lstPlaylist.ForeColor = System.Drawing.Color.Black;
            this.lstPlaylist.FullRowSelect = true;
            this.lstPlaylist.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstPlaylist.HideSelection = false;
            this.lstPlaylist.Location = new System.Drawing.Point(4, 358);
            this.lstPlaylist.Name = "lstPlaylist";
            this.lstPlaylist.Size = new System.Drawing.Size(372, 317);
            this.lstPlaylist.TabIndex = 0;
            this.lstPlaylist.UseCompatibleStateImageBehavior = false;
            this.lstPlaylist.View = System.Windows.Forms.View.Details;
            this.lstPlaylist.SelectedIndexChanged += new System.EventHandler(this.lstPlaylist_SelectedIndexChanged);
            this.lstPlaylist.DragDrop += new System.Windows.Forms.DragEventHandler(this.lstPlaylist_DragDrop);
            this.lstPlaylist.DragEnter += new System.Windows.Forms.DragEventHandler(this.lstPlaylist_DragEnter);
            this.lstPlaylist.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstPlaylist_KeyDown);
            this.lstPlaylist.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lstPlaylist_KeyPress);
            this.lstPlaylist.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstPlaylist_MouseDoubleClick);
            // 
            // colIndex
            // 
            this.colIndex.Text = "5555";
            this.colIndex.Width = 40;
            // 
            // colSong
            // 
            this.colSong.Text = "Song";
            this.colSong.Width = 255;
            // 
            // colLength
            // 
            this.colLength.Text = "1:24:25";
            this.colLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.colLength.Width = 50;
            // 
            // PlaylistContextMenu
            // 
            this.PlaylistContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playNowToolStripMenuItem,
            this.playNextToolStripMenuItem,
            this.goToArtist,
            this.goToAlbum,
            this.goToGenre,
            this.startInstaMix,
            this.toolStripMenuItem6,
            this.markAsPlayed,
            this.markAsUnplayed,
            this.moveUpToolStripMenuItem,
            this.moveDownToolStripMenuItem,
            this.openFileLocation,
            this.removeToolStripMenuItem,
            this.separator,
            this.sortPlaylistByArtist,
            this.sortPlaylistBySong,
            this.sortPlaylistByDuration,
            this.sortPlaylistByModifiedDate,
            this.randomizePlaylist,
            this.returnToPlaylist});
            this.PlaylistContextMenu.Name = "contextMenuStrip1";
            this.PlaylistContextMenu.ShowImageMargin = false;
            this.PlaylistContextMenu.Size = new System.Drawing.Size(164, 412);
            this.PlaylistContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.PlaylistContextMenu_Opening);
            // 
            // playNowToolStripMenuItem
            // 
            this.playNowToolStripMenuItem.BackColor = System.Drawing.Color.White;
            this.playNowToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.playNowToolStripMenuItem.Name = "playNowToolStripMenuItem";
            this.playNowToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.playNowToolStripMenuItem.Text = "Play now";
            this.playNowToolStripMenuItem.Click += new System.EventHandler(this.playNowToolStripMenuItem_Click);
            // 
            // playNextToolStripMenuItem
            // 
            this.playNextToolStripMenuItem.BackColor = System.Drawing.Color.White;
            this.playNextToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.playNextToolStripMenuItem.Name = "playNextToolStripMenuItem";
            this.playNextToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.playNextToolStripMenuItem.Text = "Play next";
            this.playNextToolStripMenuItem.Click += new System.EventHandler(this.playNextToolStripMenuItem_Click);
            // 
            // goToArtist
            // 
            this.goToArtist.BackColor = System.Drawing.Color.White;
            this.goToArtist.ForeColor = System.Drawing.Color.Black;
            this.goToArtist.Name = "goToArtist";
            this.goToArtist.Size = new System.Drawing.Size(163, 22);
            this.goToArtist.Text = "Go to artist";
            this.goToArtist.Click += new System.EventHandler(this.goToArtist_Click);
            // 
            // goToAlbum
            // 
            this.goToAlbum.BackColor = System.Drawing.Color.White;
            this.goToAlbum.ForeColor = System.Drawing.Color.Black;
            this.goToAlbum.Name = "goToAlbum";
            this.goToAlbum.Size = new System.Drawing.Size(163, 22);
            this.goToAlbum.Text = "Go to album";
            this.goToAlbum.Click += new System.EventHandler(this.goToAlbum_Click);
            // 
            // goToGenre
            // 
            this.goToGenre.BackColor = System.Drawing.Color.White;
            this.goToGenre.ForeColor = System.Drawing.Color.Black;
            this.goToGenre.Name = "goToGenre";
            this.goToGenre.Size = new System.Drawing.Size(163, 22);
            this.goToGenre.Text = "Go to genre";
            this.goToGenre.Click += new System.EventHandler(this.goToGenre_Click);
            // 
            // startInstaMix
            // 
            this.startInstaMix.BackColor = System.Drawing.Color.White;
            this.startInstaMix.ForeColor = System.Drawing.Color.Black;
            this.startInstaMix.Name = "startInstaMix";
            this.startInstaMix.Size = new System.Drawing.Size(163, 22);
            this.startInstaMix.Text = "Start Insta-Mix";
            this.startInstaMix.Click += new System.EventHandler(this.startInstaMix_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(160, 6);
            // 
            // markAsPlayed
            // 
            this.markAsPlayed.BackColor = System.Drawing.Color.White;
            this.markAsPlayed.ForeColor = System.Drawing.Color.Black;
            this.markAsPlayed.Name = "markAsPlayed";
            this.markAsPlayed.Size = new System.Drawing.Size(163, 22);
            this.markAsPlayed.Text = "Mark as played";
            this.markAsPlayed.Click += new System.EventHandler(this.markAsPlayed_Click);
            // 
            // markAsUnplayed
            // 
            this.markAsUnplayed.BackColor = System.Drawing.Color.White;
            this.markAsUnplayed.ForeColor = System.Drawing.Color.Black;
            this.markAsUnplayed.Name = "markAsUnplayed";
            this.markAsUnplayed.Size = new System.Drawing.Size(163, 22);
            this.markAsUnplayed.Text = "Mark as unplayed";
            this.markAsUnplayed.Click += new System.EventHandler(this.markAsUnplayed_Click);
            // 
            // moveUpToolStripMenuItem
            // 
            this.moveUpToolStripMenuItem.BackColor = System.Drawing.Color.White;
            this.moveUpToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.moveUpToolStripMenuItem.Name = "moveUpToolStripMenuItem";
            this.moveUpToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.moveUpToolStripMenuItem.Text = "Move up";
            this.moveUpToolStripMenuItem.Click += new System.EventHandler(this.moveUpToolStripMenuItem_Click);
            // 
            // moveDownToolStripMenuItem
            // 
            this.moveDownToolStripMenuItem.BackColor = System.Drawing.Color.White;
            this.moveDownToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.moveDownToolStripMenuItem.Name = "moveDownToolStripMenuItem";
            this.moveDownToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.moveDownToolStripMenuItem.Text = "Move down";
            this.moveDownToolStripMenuItem.Click += new System.EventHandler(this.moveDownToolStripMenuItem_Click);
            // 
            // openFileLocation
            // 
            this.openFileLocation.BackColor = System.Drawing.Color.White;
            this.openFileLocation.ForeColor = System.Drawing.Color.Black;
            this.openFileLocation.Name = "openFileLocation";
            this.openFileLocation.Size = new System.Drawing.Size(163, 22);
            this.openFileLocation.Text = "Open file location";
            this.openFileLocation.Click += new System.EventHandler(this.openFileLocation_Click);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.BackColor = System.Drawing.Color.White;
            this.removeToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.removeToolStripMenuItem.Text = "Remove from playlist";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // separator
            // 
            this.separator.Name = "separator";
            this.separator.Size = new System.Drawing.Size(160, 6);
            // 
            // sortPlaylistByArtist
            // 
            this.sortPlaylistByArtist.BackColor = System.Drawing.Color.White;
            this.sortPlaylistByArtist.ForeColor = System.Drawing.Color.Black;
            this.sortPlaylistByArtist.Name = "sortPlaylistByArtist";
            this.sortPlaylistByArtist.Size = new System.Drawing.Size(163, 22);
            this.sortPlaylistByArtist.Text = "Sort by artist name";
            this.sortPlaylistByArtist.Click += new System.EventHandler(this.sortPlaylistByArtist_Click);
            // 
            // sortPlaylistBySong
            // 
            this.sortPlaylistBySong.BackColor = System.Drawing.Color.White;
            this.sortPlaylistBySong.ForeColor = System.Drawing.Color.Black;
            this.sortPlaylistBySong.Name = "sortPlaylistBySong";
            this.sortPlaylistBySong.Size = new System.Drawing.Size(163, 22);
            this.sortPlaylistBySong.Text = "Sort by song title";
            this.sortPlaylistBySong.Click += new System.EventHandler(this.sortPlaylistBySong_Click);
            // 
            // sortPlaylistByDuration
            // 
            this.sortPlaylistByDuration.BackColor = System.Drawing.Color.White;
            this.sortPlaylistByDuration.ForeColor = System.Drawing.Color.Black;
            this.sortPlaylistByDuration.Name = "sortPlaylistByDuration";
            this.sortPlaylistByDuration.Size = new System.Drawing.Size(163, 22);
            this.sortPlaylistByDuration.Text = "Sort by song duration";
            this.sortPlaylistByDuration.Click += new System.EventHandler(this.sortPlaylistByDuration_Click);
            // 
            // sortPlaylistByModifiedDate
            // 
            this.sortPlaylistByModifiedDate.BackColor = System.Drawing.Color.White;
            this.sortPlaylistByModifiedDate.ForeColor = System.Drawing.Color.Black;
            this.sortPlaylistByModifiedDate.Name = "sortPlaylistByModifiedDate";
            this.sortPlaylistByModifiedDate.Size = new System.Drawing.Size(163, 22);
            this.sortPlaylistByModifiedDate.Text = "Sort by modified date";
            this.sortPlaylistByModifiedDate.Click += new System.EventHandler(this.sortPlaylistByModifiedDate_Click);
            // 
            // randomizePlaylist
            // 
            this.randomizePlaylist.BackColor = System.Drawing.Color.White;
            this.randomizePlaylist.ForeColor = System.Drawing.Color.Black;
            this.randomizePlaylist.Name = "randomizePlaylist";
            this.randomizePlaylist.Size = new System.Drawing.Size(163, 22);
            this.randomizePlaylist.Text = "Randomize order";
            this.randomizePlaylist.Click += new System.EventHandler(this.randomizePlaylist_Click);
            // 
            // returnToPlaylist
            // 
            this.returnToPlaylist.BackColor = System.Drawing.Color.White;
            this.returnToPlaylist.ForeColor = System.Drawing.Color.Black;
            this.returnToPlaylist.Name = "returnToPlaylist";
            this.returnToPlaylist.Size = new System.Drawing.Size(163, 22);
            this.returnToPlaylist.Text = "Restore Playlist";
            this.returnToPlaylist.Click += new System.EventHandler(this.returnToPlaylist_Click);
            // 
            // batchSongLoader
            // 
            this.batchSongLoader.WorkerReportsProgress = true;
            this.batchSongLoader.WorkerSupportsCancellation = true;
            this.batchSongLoader.DoWork += new System.ComponentModel.DoWorkEventHandler(this.batchSongLoader_DoWork);
            this.batchSongLoader.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.batchSongLoader_RunWorkerCompleted);
            // 
            // songLoader
            // 
            this.songLoader.WorkerReportsProgress = true;
            this.songLoader.WorkerSupportsCancellation = true;
            this.songLoader.DoWork += new System.ComponentModel.DoWorkEventHandler(this.songLoader_DoWork);
            this.songLoader.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.songLoader_RunWorkerCompleted);
            // 
            // picSearch
            // 
            this.picSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picSearch.Image = global::cPlayer.Properties.Resources.search;
            this.picSearch.Location = new System.Drawing.Point(279, 255);
            this.picSearch.Name = "picSearch";
            this.picSearch.Size = new System.Drawing.Size(21, 21);
            this.picSearch.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picSearch.TabIndex = 10;
            this.picSearch.TabStop = false;
            this.toolTip1.SetToolTip(this.picSearch, "Click to search playlist");
            this.picSearch.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picSearch_MouseClick);
            // 
            // PlaybackTimer
            // 
            this.PlaybackTimer.Interval = 16;
            this.PlaybackTimer.Tick += new System.EventHandler(this.PlaybackTimer_Tick);
            // 
            // songPreparer
            // 
            this.songPreparer.WorkerReportsProgress = true;
            this.songPreparer.WorkerSupportsCancellation = true;
            this.songPreparer.DoWork += new System.ComponentModel.DoWorkEventHandler(this.songPreparer_DoWork);
            this.songPreparer.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.songPreparer_RunWorkerCompleted);
            // 
            // songExtractor
            // 
            this.songExtractor.WorkerReportsProgress = true;
            this.songExtractor.WorkerSupportsCancellation = true;
            this.songExtractor.DoWork += new System.ComponentModel.DoWorkEventHandler(this.songExtractor_DoWork);
            this.songExtractor.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.songExtractor_RunWorkerCompleted);
            // 
            // NotifyTray
            // 
            this.NotifyTray.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.NotifyTray.BalloonTipText = "cPlayer is running in the background";
            this.NotifyTray.BalloonTipTitle = "cPlayer";
            this.NotifyTray.ContextMenuStrip = this.NotifyContextMenu;
            this.NotifyTray.Icon = ((System.Drawing.Icon)(resources.GetObject("NotifyTray.Icon")));
            this.NotifyTray.Text = "Inactive";
            this.NotifyTray.Visible = true;
            this.NotifyTray.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.NotifyTray_MouseDoubleClick);
            // 
            // NotifyContextMenu
            // 
            this.NotifyContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playToolStripMenuItem,
            this.pauseToolStripMenuItem,
            this.nextToolStripMenuItem,
            this.restoreToolStripMenuItem,
            this.quitToolStripMenuItem});
            this.NotifyContextMenu.Name = "contextMenuStrip2";
            this.NotifyContextMenu.ShowImageMargin = false;
            this.NotifyContextMenu.Size = new System.Drawing.Size(89, 114);
            this.NotifyContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.NotifyContextMenu_Opening);
            // 
            // playToolStripMenuItem
            // 
            this.playToolStripMenuItem.BackColor = System.Drawing.Color.White;
            this.playToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.playToolStripMenuItem.Name = "playToolStripMenuItem";
            this.playToolStripMenuItem.Size = new System.Drawing.Size(88, 22);
            this.playToolStripMenuItem.Text = "Play";
            this.playToolStripMenuItem.Click += new System.EventHandler(this.playToolStripMenuItem_Click);
            // 
            // pauseToolStripMenuItem
            // 
            this.pauseToolStripMenuItem.BackColor = System.Drawing.Color.White;
            this.pauseToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.pauseToolStripMenuItem.Name = "pauseToolStripMenuItem";
            this.pauseToolStripMenuItem.Size = new System.Drawing.Size(88, 22);
            this.pauseToolStripMenuItem.Text = "Pause";
            this.pauseToolStripMenuItem.Click += new System.EventHandler(this.pauseToolStripMenuItem_Click);
            // 
            // nextToolStripMenuItem
            // 
            this.nextToolStripMenuItem.BackColor = System.Drawing.Color.White;
            this.nextToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.nextToolStripMenuItem.Name = "nextToolStripMenuItem";
            this.nextToolStripMenuItem.Size = new System.Drawing.Size(88, 22);
            this.nextToolStripMenuItem.Text = "Next";
            this.nextToolStripMenuItem.Click += new System.EventHandler(this.nextToolStripMenuItem_Click);
            // 
            // restoreToolStripMenuItem
            // 
            this.restoreToolStripMenuItem.BackColor = System.Drawing.Color.White;
            this.restoreToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.restoreToolStripMenuItem.Name = "restoreToolStripMenuItem";
            this.restoreToolStripMenuItem.Size = new System.Drawing.Size(88, 22);
            this.restoreToolStripMenuItem.Text = "Restore";
            this.restoreToolStripMenuItem.Click += new System.EventHandler(this.restoreToolStripMenuItem_Click);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.BackColor = System.Drawing.Color.White;
            this.quitToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(88, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // UpdateTimer
            // 
            this.UpdateTimer.Interval = 3000;
            this.UpdateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);
            // 
            // txtSearch
            // 
            this.txtSearch.BackColor = System.Drawing.Color.AliceBlue;
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.ForeColor = System.Drawing.Color.Gray;
            this.txtSearch.Location = new System.Drawing.Point(8, 255);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(265, 20);
            this.txtSearch.TabIndex = 5;
            this.txtSearch.TabStop = false;
            this.txtSearch.Text = "Search...";
            this.txtSearch.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtSearch_MouseClick);
            this.txtSearch.EnabledChanged += new System.EventHandler(this.txtSearch_EnabledChanged);
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.txtSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSearch_KeyPress);
            this.txtSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyUp);
            this.txtSearch.Leave += new System.EventHandler(this.txtSearch_Leave);
            // 
            // folderScanner
            // 
            this.folderScanner.WorkerReportsProgress = true;
            this.folderScanner.WorkerSupportsCancellation = true;
            this.folderScanner.DoWork += new System.ComponentModel.DoWorkEventHandler(this.folderScanner_DoWork);
            this.folderScanner.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.folderScanner_RunWorkerCompleted);
            // 
            // SongMixer
            // 
            this.SongMixer.WorkerReportsProgress = true;
            this.SongMixer.WorkerSupportsCancellation = true;
            this.SongMixer.DoWork += new System.ComponentModel.DoWorkEventHandler(this.SongMixer_DoWork);
            this.SongMixer.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.SongMixer_RunWorkerCompleted);
            // 
            // Uploader
            // 
            this.Uploader.WorkerReportsProgress = true;
            this.Uploader.WorkerSupportsCancellation = true;
            this.Uploader.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Uploader_DoWork);
            this.Uploader.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.Uploader_RunWorkerCompleted);
            // 
            // updater
            // 
            this.updater.WorkerReportsProgress = true;
            this.updater.WorkerSupportsCancellation = true;
            this.updater.DoWork += new System.ComponentModel.DoWorkEventHandler(this.updater_DoWork);
            this.updater.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.updater_RunWorkerCompleted);
            // 
            // gifTmr
            // 
            this.gifTmr.Enabled = true;
            this.gifTmr.Interval = 16;
            // 
            // stageTimer
            // 
            this.stageTimer.Interval = 33;
            this.stageTimer.Tick += new System.EventHandler(this.stageTimer_Tick);
            // 
            // cursorTimer
            // 
            this.cursorTimer.Interval = 1000;
            this.cursorTimer.Tick += new System.EventHandler(this.cursorTimer_Tick);
            // 
            // foggerTimer
            // 
            this.foggerTimer.Interval = 1500;
            this.foggerTimer.Tick += new System.EventHandler(this.foggerTimer_Tick);
            // 
            // stageKitTimer
            // 
            this.stageKitTimer.Tick += new System.EventHandler(this.stageKitTimer_Tick);
            // 
            // lblClearSearch
            // 
            this.lblClearSearch.AutoSize = true;
            this.lblClearSearch.BackColor = System.Drawing.Color.Transparent;
            this.lblClearSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblClearSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClearSearch.ForeColor = System.Drawing.Color.Red;
            this.lblClearSearch.Location = new System.Drawing.Point(255, 258);
            this.lblClearSearch.Name = "lblClearSearch";
            this.lblClearSearch.Size = new System.Drawing.Size(15, 13);
            this.lblClearSearch.TabIndex = 9;
            this.lblClearSearch.Text = "X";
            this.lblClearSearch.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lblClearSearch_MouseClick);
            // 
            // documentationToolStripMenuItem
            // 
            this.documentationToolStripMenuItem.Name = "documentationToolStripMenuItem";
            this.documentationToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.documentationToolStripMenuItem.Text = "📘 Documentation";
            // 
            // picVisuals
            // 
            this.picVisuals.AllowDrop = true;
            this.picVisuals.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picVisuals.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picVisuals.Controls.Add(this.lblSections);
            this.picVisuals.Image = global::cPlayer.Properties.Resources.logo;
            this.picVisuals.Location = new System.Drawing.Point(396, 27);
            this.picVisuals.Name = "picVisuals";
            this.picVisuals.Size = new System.Drawing.Size(1169, 934);
            this.picVisuals.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picVisuals.TabIndex = 1;
            this.picVisuals.TabStop = false;
            this.picVisuals.DragDrop += new System.Windows.Forms.DragEventHandler(this.lstPlaylist_DragDrop);
            this.picVisuals.DragEnter += new System.Windows.Forms.DragEventHandler(this.lstPlaylist_DragEnter);
            this.picVisuals.DoubleClick += new System.EventHandler(this.panelVisuals_DoubleClick);
            this.picVisuals.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseDown);
            this.picVisuals.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseUp);
            // 
            // lblSections
            // 
            this.lblSections.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSections.AutoEllipsis = true;
            this.lblSections.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.lblSections.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSections.ForeColor = System.Drawing.Color.White;
            this.lblSections.Location = new System.Drawing.Point(0, 0);
            this.lblSections.Margin = new System.Windows.Forms.Padding(0);
            this.lblSections.Name = "lblSections";
            this.lblSections.Size = new System.Drawing.Size(1169, 20);
            this.lblSections.TabIndex = 2;
            this.lblSections.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblSections.UseMnemonic = false;
            this.lblSections.Visible = false;
            // 
            // picFilters
            // 
            this.picFilters.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picFilters.Image = global::cPlayer.Properties.Resources.filters;
            this.picFilters.Location = new System.Drawing.Point(309, 255);
            this.picFilters.Name = "picFilters";
            this.picFilters.Size = new System.Drawing.Size(79, 21);
            this.picFilters.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picFilters.TabIndex = 11;
            this.picFilters.TabStop = false;
            this.picFilters.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picGenres_MouseClick);
            // 
            // frmMain
            // 
            this.AllowDrop = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(1573, 971);
            this.Controls.Add(this.picVisuals);
            this.Controls.Add(this.picFilters);
            this.Controls.Add(this.picSearch);
            this.Controls.Add(this.lblClearSearch);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.panelPlaylist);
            this.Controls.Add(this.panelPlaying);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "cPlayer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Shown += new System.EventHandler(this.frmMain_Shown);
            this.LocationChanged += new System.EventHandler(this.frmMain_LocationChanged);
            this.SizeChanged += new System.EventHandler(this.frmMain_SizeChanged);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.lstPlaylist_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.lstPlaylist_DragEnter);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMain_KeyDown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseUp);
            this.Move += new System.EventHandler(this.frmMain_Move);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.VisualsContextMenu.ResumeLayout(false);
            this.panelPlaying.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picSecondScreen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picShuffle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLoop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picNext)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPause)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picStop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPlay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRandom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picVolume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).EndInit();
            this.panelPlaylist.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picOldies)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic1960s)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic1970s)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic1980s)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic1990s)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic2000s)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic2010s)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic2020s)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picFavorites)).EndInit();
            this.PlaylistContextMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picSearch)).EndInit();
            this.NotifyContextMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picVisuals)).EndInit();
            this.picVisuals.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picFilters)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createNewPlaylist;
        private System.Windows.Forms.ToolStripMenuItem loadExistingPlaylist;
        private System.Windows.Forms.ToolStripMenuItem exitToolStrip;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.PictureBox picVisuals;
        private System.Windows.Forms.Panel panelPlaying;
        private System.Windows.Forms.Panel panelPlaylist;
        private System.Windows.Forms.Label lblYear;
        private System.Windows.Forms.Label lblTrack;
        private System.Windows.Forms.Label lblGenre;
        private System.Windows.Forms.Label lblAlbum;
        private System.Windows.Forms.Label lblSong;
        private System.Windows.Forms.Label lblArtist;
        private System.Windows.Forms.PictureBox picPreview;
        private System.Windows.Forms.Panel panelSlider;
        private System.Windows.Forms.Panel panelLine;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblDuration;
        private System.Windows.Forms.ToolStripMenuItem saveCurrentPlaylist;
        private System.Windows.Forms.ListView lstPlaylist;
        private System.Windows.Forms.ColumnHeader colIndex;
        private System.Windows.Forms.ColumnHeader colSong;
        private System.Windows.Forms.ColumnHeader colLength;
        private System.ComponentModel.BackgroundWorker batchSongLoader;
        private System.ComponentModel.BackgroundWorker songLoader;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Timer PlaybackTimer;
        private System.Windows.Forms.ContextMenuStrip PlaylistContextMenu;
        private System.Windows.Forms.ToolStripMenuItem playNowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playNextToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveUpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveDownToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker songPreparer;
        private System.Windows.Forms.ToolStripMenuItem showMIDIVisuals;
        private System.Windows.Forms.ToolStripMenuItem howToUseToolStripMenuItem;
        private List<string> SongsToAdd;
        private System.Windows.Forms.PictureBox picVolume;
        private System.Windows.Forms.ToolStripMenuItem markAsPlayed;
        private System.Windows.Forms.ToolStripMenuItem markAsUnplayed;
        private System.Windows.Forms.ToolStripMenuItem goToArtist;
        private System.Windows.Forms.ToolStripMenuItem goToAlbum;
        private System.Windows.Forms.ToolStripMenuItem goToGenre;
        private System.Windows.Forms.ToolStripSeparator separator;
        private System.Windows.Forms.ToolStripMenuItem returnToPlaylist;
        private System.ComponentModel.BackgroundWorker songExtractor;
        private System.Windows.Forms.NotifyIcon NotifyTray;
        private System.Windows.Forms.ContextMenuStrip NotifyContextMenu;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restoreToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sortPlaylistByArtist;
        private System.Windows.Forms.ToolStripMenuItem sortPlaylistBySong;
        private System.Windows.Forms.ToolStripMenuItem sortPlaylistByDuration;
        private System.Windows.Forms.Timer UpdateTimer;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.ContextMenuStrip VisualsContextMenu;
        private System.Windows.Forms.ToolStripMenuItem chartVisualsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem chartFull;
        private System.Windows.Forms.Label lblAuthor;
        private System.Windows.Forms.ToolStripMenuItem chartSnippet;
        private System.Windows.Forms.ToolStripMenuItem playToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pauseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nextToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showLyrics;
        private System.ComponentModel.BackgroundWorker folderScanner;
        private System.Windows.Forms.ToolStripMenuItem openFileLocation;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scanForSongsAutomatically;
        private System.Windows.Forms.ToolStripMenuItem selectAndAddSongsManually;
        private System.Windows.Forms.ToolStripMenuItem renamePlaylist;
        private System.Windows.Forms.ToolStripMenuItem autoPlay;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem autoloadLastPlaylist;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem consoleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xbox360;
        private System.Windows.Forms.ToolStripMenuItem pS3;
        private System.Windows.Forms.ToolStripMenuItem wii;
        private System.Windows.Forms.ToolStripMenuItem playBGVideos;
        private System.Windows.Forms.ToolStripMenuItem openSideWindow;
        private System.Windows.Forms.ToolStripMenuItem showPracticeSections;
        private System.Windows.Forms.Label lblSections;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem randomizePlaylist;
        private System.Windows.Forms.ToolStripMenuItem startInstaMix;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private System.ComponentModel.BackgroundWorker SongMixer;
        private System.Windows.Forms.ToolStripMenuItem openRecent;
        private System.Windows.Forms.ToolStripMenuItem recent1;
        private System.Windows.Forms.ToolStripMenuItem recent2;
        private System.Windows.Forms.ToolStripMenuItem recent3;
        private System.Windows.Forms.ToolStripMenuItem recent4;
        private System.Windows.Forms.ToolStripMenuItem recent5;
        private System.Windows.Forms.ToolStripMenuItem displayAlbumArt;
        private System.Windows.Forms.ToolStripMenuItem displayAudioSpectrum;
        private System.Windows.Forms.ToolStripMenuItem yarg;
        //private AxWMPLib.AxWindowsMediaPlayer MediaPlayer;
        private System.Windows.Forms.ToolStripMenuItem takeScreenshot;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
        private System.Windows.Forms.PictureBox picRandom;
        private BackgroundWorker Uploader;
        private System.Windows.Forms.ToolStripMenuItem viewSongDetails;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem9;
        private System.Windows.Forms.ToolStripMenuItem uploadScreenshots;
        private BackgroundWorker updater;
        private ToolStripMenuItem checkForUpdates;
        private ToolStripMenuItem viewChangeLog;
        private ToolStripMenuItem sortPlaylistByModifiedDate;
        private ToolStripMenuItem displayKaraokeMode;
        private ToolStripMenuItem skipIntroOutroSilence;
        private ToolStripMenuItem displayBackgroundVideo;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripMenuItem rebuildPlaylistMetadata;
        private ToolStripMenuItem audioTracks;
        private ToolStripMenuItem rockSmith;
        private ToolStripMenuItem fortNite;
        private ToolStripMenuItem guitarHero;
        private ToolStripSeparator toolStripMenuItem5;
        private ToolStripMenuItem bandFuse;
        private ToolStripMenuItem powerGig;
        private Timer gifTmr;
        private PictureBox picPlay;
        private PictureBox picStop;
        private PictureBox picPause;
        private PictureBox picNext;
        private PictureBox picLoop;
        private PictureBox picShuffle;
        private ToolStripMenuItem chartVertical;
        private ToolStripMenuItem equipmentToolStripMenuItem;
        private ToolStripMenuItem microphoneToolStripMenuItem;
        private ToolStripMenuItem stageKitToolStripMenuItem;
        private ToolStripMenuItem controller1;
        private ToolStripMenuItem controller2;
        private ToolStripMenuItem controller3;
        private ToolStripMenuItem controller4;
        private ToolStripMenuItem selectBackgroundColor;
        private ToolStripMenuItem selectLyricColor;
        private ToolStripMenuItem selectHighlightColor;
        private ToolStripMenuItem restoreDefaultsToolStripMenuItem;
        private ToolStripMenuItem rebuildPlaylistMetadataAudio;
        private ToolStripMenuItem classicKaraokeMode;
        private ToolStripMenuItem cPlayerStyle;
        private ToolStripSeparator toolStripMenuItem13;
        private ToolStripMenuItem selectHarmonyTextColor;
        private ToolStripMenuItem selectHarmonyHighlightColor;
        private ToolStripSeparator toolStripMenuItem14;
        private ToolStripMenuItem selectHarmony3TextColor;
        private ToolStripMenuItem selectHarmony3HighlightColor;
        private ToolStripMenuItem rockBandKaraoke;
        private Timer stageTimer;
        private Timer cursorTimer;
        private ToolStripMenuItem animatedBackground;
        private ToolStripMenuItem staticBackground;
        private ToolStripSeparator toolStripMenuItem8;
        private ToolStripMenuItem useLEDs;
        private ToolStripMenuItem useStrobe;
        private ToolStripMenuItem useFogger;
        private Timer foggerTimer;
        private ToolStripMenuItem staticBackground2;
        private ToolStripSeparator toolStripMenuItem15;
        private ToolStripMenuItem forceSoloVocals;
        private ToolStripMenuItem forceTwoPartHarmonies;
        private ToolStripMenuItem continuousPlayback;
        private Timer stageKitTimer;
        private ToolStripMenuItem enableSecondScreen;
        private ToolStripMenuItem changeStageBackground;
        private ToolStripSeparator toolStripMenuItem16;
        private ToolStripMenuItem animatedBackground2;
        private Label lblClearSearch;
        private PictureBox picFavorites;
        private PictureBox picOldies;
        private PictureBox pic1960s;
        private PictureBox pic1970s;
        private PictureBox pic1980s;
        private PictureBox pic1990s;
        private PictureBox pic2000s;
        private PictureBox pic2010s;
        private PictureBox pic2020s;
        private PictureBox picSearch;
        private PictureBox picFilters;
        private ToolStripMenuItem solidColorBackground;
        private PictureBox picSecondScreen;
        private ToolStripMenuItem rBStyle;
        private ToolStripMenuItem useBackgroundVideos;
        private ToolStripMenuItem useBackgroundImages;
        private ToolStripMenuItem fullSupportToolStripMenuItem;
        private ToolStripMenuItem limitedSupportToolStripMenuItem;
        private ToolStripMenuItem bluetoothAVOffset;
        private ToolStripMenuItem nautilusToolStripMenuItem;
        private ToolStripMenuItem setNautilusPath;
        private ToolStripSeparator toolStripMenuItem12;
        private ToolStripMenuItem sendToVisualizer;
        private ToolStripMenuItem sendToFileAnalyzer;
        private ToolStripMenuItem sendToAudioAnalyzer;
        private ToolStripMenuItem sendToCONExplorer;
        private ToolStripMenuItem changeViewToolStrip;
        private ToolStripMenuItem documentationToolStripMenuItem;
        private ToolStripMenuItem awesomenessDetection;
        private ToolStripMenuItem rb4PS4;
    }
}


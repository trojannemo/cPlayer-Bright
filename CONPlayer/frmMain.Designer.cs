using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using AxWMPLib;
using cPlayer.Properties;

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
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.viewSongDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.takeScreenshot = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.consoleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xbox360 = new System.Windows.Forms.ToolStripMenuItem();
            this.pS3 = new System.Windows.Forms.ToolStripMenuItem();
            this.wii = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.yarg = new System.Windows.Forms.ToolStripMenuItem();
            this.fortNite = new System.Windows.Forms.ToolStripMenuItem();
            this.rockSmith = new System.Windows.Forms.ToolStripMenuItem();
            this.guitarHero = new System.Windows.Forms.ToolStripMenuItem();
            this.powerGig = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem12 = new System.Windows.Forms.ToolStripSeparator();
            this.bandFuse = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoloadLastPlaylist = new System.Windows.Forms.ToolStripMenuItem();
            this.autoPlay = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.skipIntroOutroSilence = new System.Windows.Forms.ToolStripMenuItem();
            this.audioTracks = new System.Windows.Forms.ToolStripMenuItem();
            this.playBGVideos = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.showPracticeSections = new System.Windows.Forms.ToolStripMenuItem();
            this.showMIDIVisuals = new System.Windows.Forms.ToolStripMenuItem();
            this.showLyrics = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripSeparator();
            this.uploadScreenshots = new System.Windows.Forms.ToolStripMenuItem();
            this.openSideWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.equipmentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.microphoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stageKitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.controller1 = new System.Windows.Forms.ToolStripMenuItem();
            this.controller2 = new System.Windows.Forms.ToolStripMenuItem();
            this.controller3 = new System.Windows.Forms.ToolStripMenuItem();
            this.controller4 = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.howToUseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripSeparator();
            this.checkForUpdates = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem11 = new System.Windows.Forms.ToolStripSeparator();
            this.viewChangeLog = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.VisualsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.displayBackgroundVideo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.displayAlbumArt = new System.Windows.Forms.ToolStripMenuItem();
            this.displayAudioSpectrum = new System.Windows.Forms.ToolStripMenuItem();
            this.displayKaraokeMode = new System.Windows.Forms.ToolStripMenuItem();
            this.displayMIDIChartVisuals = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
            this.styleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chartVertical = new System.Windows.Forms.ToolStripMenuItem();
            this.chartSnippet = new System.Windows.Forms.ToolStripMenuItem();
            this.chartFull = new System.Windows.Forms.ToolStripMenuItem();
            this.lblAuthor = new System.Windows.Forms.Label();
            this.panelPlaying = new System.Windows.Forms.Panel();
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
            this.lblArtist = new System.Windows.Forms.Label();
            this.picPreview = new System.Windows.Forms.PictureBox();
            this.panelPlaylist = new System.Windows.Forms.Panel();
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
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnGoTo = new System.Windows.Forms.Button();
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
            this.lblUpdates = new System.Windows.Forms.Label();
            this.UpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.folderScanner = new System.ComponentModel.BackgroundWorker();
            this.SongMixer = new System.ComponentModel.BackgroundWorker();
            this.Uploader = new System.ComponentModel.BackgroundWorker();
            this.updater = new System.ComponentModel.BackgroundWorker();
            this.gifTmr = new System.Windows.Forms.Timer(this.components);
            this.picVisuals = new System.Windows.Forms.PictureBox();
            this.lblSections = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.VisualsContextMenu.SuspendLayout();
            this.panelPlaying.SuspendLayout();
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
            this.PlaylistContextMenu.SuspendLayout();
            this.NotifyContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picVisuals)).BeginInit();
            this.picVisuals.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.White;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.equipmentToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(994, 24);
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
            this.renamePlaylist.Size = new System.Drawing.Size(248, 22);
            this.renamePlaylist.Text = "Rename playlist";
            this.renamePlaylist.Click += new System.EventHandler(this.renamePlaylist_Click);
            // 
            // selectAndAddSongsManually
            // 
            this.selectAndAddSongsManually.BackColor = System.Drawing.Color.White;
            this.selectAndAddSongsManually.ForeColor = System.Drawing.Color.Black;
            this.selectAndAddSongsManually.Name = "selectAndAddSongsManually";
            this.selectAndAddSongsManually.Size = new System.Drawing.Size(248, 22);
            this.selectAndAddSongsManually.Text = "Select and add songs manually";
            this.selectAndAddSongsManually.Click += new System.EventHandler(this.selectAndAddSongsManually_Click);
            // 
            // scanForSongsAutomatically
            // 
            this.scanForSongsAutomatically.BackColor = System.Drawing.Color.White;
            this.scanForSongsAutomatically.ForeColor = System.Drawing.Color.Black;
            this.scanForSongsAutomatically.Name = "scanForSongsAutomatically";
            this.scanForSongsAutomatically.Size = new System.Drawing.Size(248, 22);
            this.scanForSongsAutomatically.Text = "Scan for songs automatically";
            this.scanForSongsAutomatically.Click += new System.EventHandler(this.scanForSongsAutomatically_Click);
            // 
            // rebuildPlaylistMetadata
            // 
            this.rebuildPlaylistMetadata.BackColor = System.Drawing.Color.White;
            this.rebuildPlaylistMetadata.ForeColor = System.Drawing.Color.Black;
            this.rebuildPlaylistMetadata.Name = "rebuildPlaylistMetadata";
            this.rebuildPlaylistMetadata.Size = new System.Drawing.Size(248, 22);
            this.rebuildPlaylistMetadata.Text = "Rebuild playlist metadata";
            this.rebuildPlaylistMetadata.Click += new System.EventHandler(this.rebuildPlaylistMetadata_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(245, 6);
            // 
            // viewSongDetails
            // 
            this.viewSongDetails.BackColor = System.Drawing.Color.White;
            this.viewSongDetails.ForeColor = System.Drawing.Color.Black;
            this.viewSongDetails.Name = "viewSongDetails";
            this.viewSongDetails.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.viewSongDetails.Size = new System.Drawing.Size(248, 22);
            this.viewSongDetails.Text = "View selected song details";
            this.viewSongDetails.Click += new System.EventHandler(this.viewSongDetails_Click);
            // 
            // takeScreenshot
            // 
            this.takeScreenshot.BackColor = System.Drawing.Color.White;
            this.takeScreenshot.ForeColor = System.Drawing.Color.Black;
            this.takeScreenshot.Name = "takeScreenshot";
            this.takeScreenshot.ShortcutKeys = System.Windows.Forms.Keys.F12;
            this.takeScreenshot.Size = new System.Drawing.Size(248, 22);
            this.takeScreenshot.Text = "Take screenshot of visuals";
            this.takeScreenshot.Click += new System.EventHandler(this.takeScreenshot_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(245, 6);
            // 
            // consoleToolStripMenuItem
            // 
            this.consoleToolStripMenuItem.BackColor = System.Drawing.Color.White;
            this.consoleToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.xbox360,
            this.pS3,
            this.wii,
            this.toolStripMenuItem5,
            this.yarg,
            this.fortNite,
            this.rockSmith,
            this.guitarHero,
            this.powerGig,
            this.toolStripMenuItem12,
            this.bandFuse});
            this.consoleToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.consoleToolStripMenuItem.Name = "consoleToolStripMenuItem";
            this.consoleToolStripMenuItem.Size = new System.Drawing.Size(248, 22);
            this.consoleToolStripMenuItem.Text = "Game console: Xbox 360";
            // 
            // xbox360
            // 
            this.xbox360.BackColor = System.Drawing.Color.White;
            this.xbox360.Checked = true;
            this.xbox360.CheckState = System.Windows.Forms.CheckState.Checked;
            this.xbox360.ForeColor = System.Drawing.Color.Black;
            this.xbox360.Name = "xbox360";
            this.xbox360.Size = new System.Drawing.Size(403, 22);
            this.xbox360.Text = "Xbox 360 (CON | LIVE)";
            this.xbox360.Click += new System.EventHandler(this.UpdateConsole);
            // 
            // pS3
            // 
            this.pS3.BackColor = System.Drawing.Color.White;
            this.pS3.ForeColor = System.Drawing.Color.Black;
            this.pS3.Name = "pS3";
            this.pS3.Size = new System.Drawing.Size(403, 22);
            this.pS3.Text = "PlayStation 3 (.pkg | songs.dta)";
            this.pS3.Click += new System.EventHandler(this.UpdateConsole);
            // 
            // wii
            // 
            this.wii.BackColor = System.Drawing.Color.White;
            this.wii.ForeColor = System.Drawing.Color.Black;
            this.wii.Name = "wii";
            this.wii.Size = new System.Drawing.Size(403, 22);
            this.wii.Text = "Wii (songs.dta)";
            this.wii.Click += new System.EventHandler(this.UpdateConsole);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(400, 6);
            // 
            // yarg
            // 
            this.yarg.BackColor = System.Drawing.Color.White;
            this.yarg.ForeColor = System.Drawing.Color.Black;
            this.yarg.Name = "yarg";
            this.yarg.Size = new System.Drawing.Size(403, 22);
            this.yarg.Text = "PC: YARG / Clone Hero (songs.dta | .yargsong | song.ini | .sng )";
            this.yarg.Click += new System.EventHandler(this.UpdateConsole);
            // 
            // fortNite
            // 
            this.fortNite.BackColor = System.Drawing.Color.White;
            this.fortNite.ForeColor = System.Drawing.Color.Black;
            this.fortNite.Name = "fortNite";
            this.fortNite.Size = new System.Drawing.Size(403, 22);
            this.fortNite.Text = "PC: Fortnite Festival (.fnf | .m4a)";
            this.fortNite.Click += new System.EventHandler(this.UpdateConsole);
            // 
            // rockSmith
            // 
            this.rockSmith.BackColor = System.Drawing.Color.White;
            this.rockSmith.ForeColor = System.Drawing.Color.Black;
            this.rockSmith.Name = "rockSmith";
            this.rockSmith.Size = new System.Drawing.Size(403, 22);
            this.rockSmith.Text = "PC: Rocksmith 2014 (.psarc)";
            this.rockSmith.Click += new System.EventHandler(this.UpdateConsole);
            // 
            // guitarHero
            // 
            this.guitarHero.BackColor = System.Drawing.Color.White;
            this.guitarHero.ForeColor = System.Drawing.Color.Black;
            this.guitarHero.Name = "guitarHero";
            this.guitarHero.Size = new System.Drawing.Size(403, 22);
            this.guitarHero.Text = "PC: GHWT:DE (.fsb.xen)";
            this.guitarHero.Click += new System.EventHandler(this.UpdateConsole);
            // 
            // powerGig
            // 
            this.powerGig.BackColor = System.Drawing.Color.White;
            this.powerGig.ForeColor = System.Drawing.Color.Black;
            this.powerGig.Name = "powerGig";
            this.powerGig.Size = new System.Drawing.Size(403, 22);
            this.powerGig.Text = "PC: Power Gig (.xml)";
            this.powerGig.Click += new System.EventHandler(this.UpdateConsole);
            // 
            // toolStripMenuItem12
            // 
            this.toolStripMenuItem12.Name = "toolStripMenuItem12";
            this.toolStripMenuItem12.Size = new System.Drawing.Size(400, 6);
            // 
            // bandFuse
            // 
            this.bandFuse.BackColor = System.Drawing.Color.White;
            this.bandFuse.ForeColor = System.Drawing.Color.Black;
            this.bandFuse.Name = "bandFuse";
            this.bandFuse.Size = new System.Drawing.Size(403, 22);
            this.bandFuse.Text = "Xbox 360: BandFuse (LIVE)";
            this.bandFuse.Click += new System.EventHandler(this.UpdateConsole);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.autoloadLastPlaylist,
            this.autoPlay,
            this.toolStripMenuItem4,
            this.skipIntroOutroSilence,
            this.audioTracks,
            this.playBGVideos,
            this.toolStripMenuItem3,
            this.showPracticeSections,
            this.showMIDIVisuals,
            this.showLyrics,
            this.toolStripMenuItem9,
            this.uploadScreenshots,
            this.openSideWindow});
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
            this.showPracticeSections.Checked = true;
            this.showPracticeSections.CheckOnClick = true;
            this.showPracticeSections.CheckState = System.Windows.Forms.CheckState.Checked;
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
            this.openSideWindow.Click += new System.EventHandler(this.openSideWindow_Click);
            // 
            // equipmentToolStripMenuItem
            // 
            this.equipmentToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.microphoneToolStripMenuItem,
            this.stageKitToolStripMenuItem});
            this.equipmentToolStripMenuItem.Name = "equipmentToolStripMenuItem";
            this.equipmentToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.equipmentToolStripMenuItem.Text = "&Equipment";
            // 
            // microphoneToolStripMenuItem
            // 
            this.microphoneToolStripMenuItem.Name = "microphoneToolStripMenuItem";
            this.microphoneToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.microphoneToolStripMenuItem.Text = "Microphone";
            this.microphoneToolStripMenuItem.Click += new System.EventHandler(this.microphoneToolStripMenuItem_Click);
            // 
            // stageKitToolStripMenuItem
            // 
            this.stageKitToolStripMenuItem.CheckOnClick = true;
            this.stageKitToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.controller1,
            this.controller2,
            this.controller3,
            this.controller4});
            this.stageKitToolStripMenuItem.Name = "stageKitToolStripMenuItem";
            this.stageKitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.stageKitToolStripMenuItem.Text = "Enable Stage Kit";
            this.stageKitToolStripMenuItem.CheckedChanged += new System.EventHandler(this.stageKitToolStripMenuItem_CheckedChanged);
            this.stageKitToolStripMenuItem.Click += new System.EventHandler(this.stageKitToolStripMenuItem_Click);
            // 
            // controller1
            // 
            this.controller1.Enabled = false;
            this.controller1.Name = "controller1";
            this.controller1.Size = new System.Drawing.Size(180, 22);
            this.controller1.Text = "Controller 1";
            this.controller1.Click += new System.EventHandler(this.controller1_Click);
            // 
            // controller2
            // 
            this.controller2.Enabled = false;
            this.controller2.Name = "controller2";
            this.controller2.Size = new System.Drawing.Size(180, 22);
            this.controller2.Text = "Controller 2";
            this.controller2.Click += new System.EventHandler(this.controller2_Click);
            // 
            // controller3
            // 
            this.controller3.Enabled = false;
            this.controller3.Name = "controller3";
            this.controller3.Size = new System.Drawing.Size(180, 22);
            this.controller3.Text = "Controller 3";
            this.controller3.Click += new System.EventHandler(this.controller3_Click);
            // 
            // controller4
            // 
            this.controller4.Enabled = false;
            this.controller4.Name = "controller4";
            this.controller4.Size = new System.Drawing.Size(180, 22);
            this.controller4.Text = "Controller 4";
            this.controller4.Click += new System.EventHandler(this.controller4_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.howToUseToolStripMenuItem,
            this.toolStripMenuItem10,
            this.checkForUpdates,
            this.toolStripMenuItem11,
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
            this.howToUseToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F1)));
            this.howToUseToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.howToUseToolStripMenuItem.Text = "How to Use";
            this.howToUseToolStripMenuItem.Click += new System.EventHandler(this.howToUseToolStripMenuItem_Click);
            // 
            // toolStripMenuItem10
            // 
            this.toolStripMenuItem10.Name = "toolStripMenuItem10";
            this.toolStripMenuItem10.Size = new System.Drawing.Size(200, 6);
            // 
            // checkForUpdates
            // 
            this.checkForUpdates.BackColor = System.Drawing.Color.White;
            this.checkForUpdates.ForeColor = System.Drawing.Color.Black;
            this.checkForUpdates.Name = "checkForUpdates";
            this.checkForUpdates.Size = new System.Drawing.Size(203, 22);
            this.checkForUpdates.Text = "Check for updates";
            this.checkForUpdates.Click += new System.EventHandler(this.checkForUpdates_Click);
            // 
            // toolStripMenuItem11
            // 
            this.toolStripMenuItem11.Name = "toolStripMenuItem11";
            this.toolStripMenuItem11.Size = new System.Drawing.Size(200, 6);
            // 
            // viewChangeLog
            // 
            this.viewChangeLog.BackColor = System.Drawing.Color.White;
            this.viewChangeLog.ForeColor = System.Drawing.Color.Black;
            this.viewChangeLog.Name = "viewChangeLog";
            this.viewChangeLog.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F1)));
            this.viewChangeLog.Size = new System.Drawing.Size(203, 22);
            this.viewChangeLog.Text = "View change log";
            this.viewChangeLog.Click += new System.EventHandler(this.viewChangeLog_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.BackColor = System.Drawing.Color.White;
            this.aboutToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
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
            this.displayMIDIChartVisuals,
            this.toolStripMenuItem8,
            this.styleToolStripMenuItem});
            this.VisualsContextMenu.Name = "VisualsContextMenu";
            this.VisualsContextMenu.Size = new System.Drawing.Size(215, 148);
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
            this.displayBackgroundVideo.Size = new System.Drawing.Size(214, 22);
            this.displayBackgroundVideo.Text = "Play Background Videos";
            this.displayBackgroundVideo.Click += new System.EventHandler(this.displayBackgroundVideo_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(211, 6);
            // 
            // displayAlbumArt
            // 
            this.displayAlbumArt.BackColor = System.Drawing.Color.White;
            this.displayAlbumArt.Checked = true;
            this.displayAlbumArt.CheckState = System.Windows.Forms.CheckState.Checked;
            this.displayAlbumArt.ForeColor = System.Drawing.Color.Black;
            this.displayAlbumArt.Name = "displayAlbumArt";
            this.displayAlbumArt.Size = new System.Drawing.Size(214, 22);
            this.displayAlbumArt.Text = "Display: Album Art";
            this.displayAlbumArt.Click += new System.EventHandler(this.displayAlbumArt_Click);
            // 
            // displayAudioSpectrum
            // 
            this.displayAudioSpectrum.BackColor = System.Drawing.Color.White;
            this.displayAudioSpectrum.ForeColor = System.Drawing.Color.Black;
            this.displayAudioSpectrum.Name = "displayAudioSpectrum";
            this.displayAudioSpectrum.Size = new System.Drawing.Size(214, 22);
            this.displayAudioSpectrum.Text = "Display: Audio Spectrum";
            this.displayAudioSpectrum.Click += new System.EventHandler(this.displayAudioSpectrum_Click);
            // 
            // displayKaraokeMode
            // 
            this.displayKaraokeMode.BackColor = System.Drawing.Color.White;
            this.displayKaraokeMode.ForeColor = System.Drawing.Color.Black;
            this.displayKaraokeMode.Name = "displayKaraokeMode";
            this.displayKaraokeMode.Size = new System.Drawing.Size(214, 22);
            this.displayKaraokeMode.Text = "Display: Karaoke Mode";
            this.displayKaraokeMode.Click += new System.EventHandler(this.displayKaraokeMode_Click);
            // 
            // displayMIDIChartVisuals
            // 
            this.displayMIDIChartVisuals.BackColor = System.Drawing.Color.White;
            this.displayMIDIChartVisuals.ForeColor = System.Drawing.Color.Black;
            this.displayMIDIChartVisuals.Name = "displayMIDIChartVisuals";
            this.displayMIDIChartVisuals.Size = new System.Drawing.Size(214, 22);
            this.displayMIDIChartVisuals.Text = "Display: MIDI Chart Visuals";
            this.displayMIDIChartVisuals.Click += new System.EventHandler(this.displayMIDIChartVisuals_Click);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(211, 6);
            this.toolStripMenuItem8.Visible = false;
            // 
            // styleToolStripMenuItem
            // 
            this.styleToolStripMenuItem.BackColor = System.Drawing.Color.White;
            this.styleToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chartVertical,
            this.chartSnippet,
            this.chartFull});
            this.styleToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.styleToolStripMenuItem.Name = "styleToolStripMenuItem";
            this.styleToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.styleToolStripMenuItem.Text = "MIDI: Chart Style";
            this.styleToolStripMenuItem.Visible = false;
            // 
            // chartVertical
            // 
            this.chartVertical.Checked = true;
            this.chartVertical.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chartVertical.Name = "chartVertical";
            this.chartVertical.Size = new System.Drawing.Size(133, 22);
            this.chartVertical.Text = "Game Style";
            this.chartVertical.Click += new System.EventHandler(this.UpdateVisualStyle);
            // 
            // chartSnippet
            // 
            this.chartSnippet.BackColor = System.Drawing.Color.White;
            this.chartSnippet.ForeColor = System.Drawing.Color.Black;
            this.chartSnippet.Name = "chartSnippet";
            this.chartSnippet.Size = new System.Drawing.Size(133, 22);
            this.chartSnippet.Text = "MIDI Style";
            this.chartSnippet.Click += new System.EventHandler(this.UpdateVisualStyle);
            // 
            // chartFull
            // 
            this.chartFull.BackColor = System.Drawing.Color.White;
            this.chartFull.ForeColor = System.Drawing.Color.Black;
            this.chartFull.Name = "chartFull";
            this.chartFull.Size = new System.Drawing.Size(133, 22);
            this.chartFull.Text = "Chart: Full";
            this.chartFull.Visible = false;
            this.chartFull.Click += new System.EventHandler(this.UpdateVisualStyle);
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
            this.panelPlaying.BackColor = System.Drawing.Color.Transparent;
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
            this.panelPlaying.Controls.Add(this.lblArtist);
            this.panelPlaying.Controls.Add(this.picPreview);
            this.panelPlaying.ForeColor = System.Drawing.Color.Black;
            this.panelPlaying.Location = new System.Drawing.Point(8, 27);
            this.panelPlaying.Name = "panelPlaying";
            this.panelPlaying.Size = new System.Drawing.Size(380, 221);
            this.panelPlaying.TabIndex = 2;
            this.panelPlaying.DragDrop += new System.Windows.Forms.DragEventHandler(this.lstPlaylist_DragDrop);
            this.panelPlaying.DragEnter += new System.Windows.Forms.DragEventHandler(this.lstPlaylist_DragEnter);
            this.panelPlaying.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseDown);
            this.panelPlaying.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseMove);
            this.panelPlaying.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseUp);
            // 
            // picShuffle
            // 
            this.picShuffle.BackColor = System.Drawing.Color.Transparent;
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
            this.picLoop.BackColor = System.Drawing.Color.Transparent;
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
            this.picNext.BackColor = System.Drawing.Color.Transparent;
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
            this.picPause.BackColor = System.Drawing.Color.Transparent;
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
            this.picStop.BackColor = System.Drawing.Color.Transparent;
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
            this.picPlay.BackColor = System.Drawing.Color.Transparent;
            this.picPlay.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picPlay.Enabled = false;
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
            this.picRandom.BackColor = System.Drawing.Color.Transparent;
            this.picRandom.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picRandom.Image = global::cPlayer.Properties.Resources.dice_trippy;
            this.picRandom.Location = new System.Drawing.Point(332, 126);
            this.picRandom.Name = "picRandom";
            this.picRandom.Size = new System.Drawing.Size(40, 40);
            this.picRandom.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picRandom.TabIndex = 50;
            this.picRandom.TabStop = false;
            this.toolTip1.SetToolTip(this.picRandom, "Pick a random song");
            this.picRandom.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picRandom_MouseClick);
            // 
            // picVolume
            // 
            this.picVolume.BackColor = System.Drawing.Color.Transparent;
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
            this.lblSong.Location = new System.Drawing.Point(111, 40);
            this.lblSong.Name = "lblSong";
            this.lblSong.Size = new System.Drawing.Size(261, 23);
            this.lblSong.TabIndex = 2;
            this.lblSong.Text = "Song:";
            this.lblSong.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblSong.UseMnemonic = false;
            // 
            // lblArtist
            // 
            this.lblArtist.AutoEllipsis = true;
            this.lblArtist.BackColor = System.Drawing.Color.Transparent;
            this.lblArtist.ForeColor = System.Drawing.Color.Black;
            this.lblArtist.Location = new System.Drawing.Point(111, 8);
            this.lblArtist.Name = "lblArtist";
            this.lblArtist.Size = new System.Drawing.Size(261, 23);
            this.lblArtist.TabIndex = 1;
            this.lblArtist.Text = "Artist:";
            this.lblArtist.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblArtist.UseMnemonic = false;
            // 
            // picPreview
            // 
            this.picPreview.Image = global::cPlayer.Properties.Resources.noart3;
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
            this.panelPlaylist.BackColor = System.Drawing.Color.Transparent;
            this.panelPlaylist.Controls.Add(this.lstPlaylist);
            this.panelPlaylist.ForeColor = System.Drawing.Color.Black;
            this.panelPlaylist.Location = new System.Drawing.Point(8, 256);
            this.panelPlaylist.Name = "panelPlaylist";
            this.panelPlaylist.Size = new System.Drawing.Size(380, 399);
            this.panelPlaylist.TabIndex = 3;
            this.panelPlaylist.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseDown);
            this.panelPlaylist.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseMove);
            this.panelPlaylist.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseUp);
            // 
            // lstPlaylist
            // 
            this.lstPlaylist.AllowDrop = true;
            this.lstPlaylist.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lstPlaylist.BackColor = System.Drawing.Color.White;
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
            this.lstPlaylist.Location = new System.Drawing.Point(4, 3);
            this.lstPlaylist.Name = "lstPlaylist";
            this.lstPlaylist.Size = new System.Drawing.Size(372, 393);
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
            this.returnToPlaylist.Text = "Return to playlist";
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
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClear.ForeColor = System.Drawing.Color.Black;
            this.btnClear.Location = new System.Drawing.Point(346, 661);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(42, 22);
            this.btnClear.TabIndex = 6;
            this.btnClear.Text = "Clear";
            this.toolTip1.SetToolTip(this.btnClear, "Click to clear your search");
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSearch.ForeColor = System.Drawing.Color.Black;
            this.btnSearch.Location = new System.Drawing.Point(298, 661);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(42, 22);
            this.btnSearch.TabIndex = 7;
            this.btnSearch.Text = "Filter";
            this.toolTip1.SetToolTip(this.btnSearch, "Click to filter playlist by search term");
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnGoTo
            // 
            this.btnGoTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnGoTo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGoTo.ForeColor = System.Drawing.Color.Black;
            this.btnGoTo.Location = new System.Drawing.Point(244, 661);
            this.btnGoTo.Name = "btnGoTo";
            this.btnGoTo.Size = new System.Drawing.Size(48, 22);
            this.btnGoTo.TabIndex = 8;
            this.btnGoTo.Text = "Go To";
            this.toolTip1.SetToolTip(this.btnGoTo, "Click to go to next search term");
            this.btnGoTo.UseVisualStyleBackColor = true;
            this.btnGoTo.Click += new System.EventHandler(this.btnGoTo_Click);
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
            // 
            // pauseToolStripMenuItem
            // 
            this.pauseToolStripMenuItem.BackColor = System.Drawing.Color.White;
            this.pauseToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.pauseToolStripMenuItem.Name = "pauseToolStripMenuItem";
            this.pauseToolStripMenuItem.Size = new System.Drawing.Size(88, 22);
            this.pauseToolStripMenuItem.Text = "Pause";
            // 
            // nextToolStripMenuItem
            // 
            this.nextToolStripMenuItem.BackColor = System.Drawing.Color.White;
            this.nextToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.nextToolStripMenuItem.Name = "nextToolStripMenuItem";
            this.nextToolStripMenuItem.Size = new System.Drawing.Size(88, 22);
            this.nextToolStripMenuItem.Text = "Next";
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
            // lblUpdates
            // 
            this.lblUpdates.AutoEllipsis = true;
            this.lblUpdates.BackColor = System.Drawing.Color.White;
            this.lblUpdates.ForeColor = System.Drawing.Color.Black;
            this.lblUpdates.Location = new System.Drawing.Point(396, 0);
            this.lblUpdates.Name = "lblUpdates";
            this.lblUpdates.Size = new System.Drawing.Size(590, 23);
            this.lblUpdates.TabIndex = 4;
            this.lblUpdates.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblUpdates.UseCompatibleTextRendering = true;
            // 
            // UpdateTimer
            // 
            this.UpdateTimer.Interval = 3000;
            this.UpdateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtSearch.BackColor = System.Drawing.Color.White;
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.ForeColor = System.Drawing.Color.Gray;
            this.txtSearch.Location = new System.Drawing.Point(8, 662);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(230, 20);
            this.txtSearch.TabIndex = 5;
            this.txtSearch.TabStop = false;
            this.txtSearch.Text = "Type to search playlist...";
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
            this.gifTmr.Tick += new System.EventHandler(this.gifTmr_Tick);
            // 
            // picVisuals
            // 
            this.picVisuals.AllowDrop = true;
            this.picVisuals.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picVisuals.BackColor = System.Drawing.Color.White;
            this.picVisuals.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.picVisuals.ContextMenuStrip = this.VisualsContextMenu;
            this.picVisuals.Controls.Add(this.lblSections);
            this.picVisuals.Location = new System.Drawing.Point(396, 27);
            this.picVisuals.Name = "picVisuals";
            this.picVisuals.Size = new System.Drawing.Size(590, 654);
            this.picVisuals.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picVisuals.TabIndex = 1;
            this.picVisuals.TabStop = false;
            this.picVisuals.DragDrop += new System.Windows.Forms.DragEventHandler(this.lstPlaylist_DragDrop);
            this.picVisuals.DragEnter += new System.Windows.Forms.DragEventHandler(this.lstPlaylist_DragEnter);
            this.picVisuals.Paint += new System.Windows.Forms.PaintEventHandler(this.picVisuals_Paint);
            this.picVisuals.DoubleClick += new System.EventHandler(this.panelVisuals_DoubleClick);
            this.picVisuals.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picVisuals_MouseClick);
            this.picVisuals.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseDown);
            this.picVisuals.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseMove);
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
            this.lblSections.Size = new System.Drawing.Size(590, 20);
            this.lblSections.TabIndex = 2;
            this.lblSections.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblSections.UseMnemonic = false;
            this.lblSections.Visible = false;
            // 
            // frmMain
            // 
            this.AllowDrop = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(994, 691);
            this.Controls.Add(this.btnGoTo);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.lblUpdates);
            this.Controls.Add(this.picVisuals);
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
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.lstPlaylist_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.lstPlaylist_DragEnter);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMain_KeyDown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseUp);
            this.Move += new System.EventHandler(this.frmMain_Move);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.VisualsContextMenu.ResumeLayout(false);
            this.panelPlaying.ResumeLayout(false);
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
            this.PlaylistContextMenu.ResumeLayout(false);
            this.NotifyContextMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picVisuals)).EndInit();
            this.picVisuals.ResumeLayout(false);
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
        private System.Windows.Forms.Label lblUpdates;
        private System.Windows.Forms.Timer UpdateTimer;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.ContextMenuStrip VisualsContextMenu;
        private System.Windows.Forms.ToolStripMenuItem styleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem chartFull;
        private System.Windows.Forms.Label lblAuthor;
        private System.Windows.Forms.ToolStripMenuItem chartSnippet;
        private System.Windows.Forms.ToolStripMenuItem playToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pauseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nextToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showLyrics;
        private System.ComponentModel.BackgroundWorker folderScanner;
        private System.Windows.Forms.ToolStripMenuItem openFileLocation;
        private System.Windows.Forms.Button btnClear;
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
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnGoTo;
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
        private System.Windows.Forms.ToolStripMenuItem displayMIDIChartVisuals;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem8;
        private System.Windows.Forms.ToolStripMenuItem yarg;
        //private AxWMPLib.AxWindowsMediaPlayer MediaPlayer;
        private System.Windows.Forms.ToolStripMenuItem takeScreenshot;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
        private System.Windows.Forms.PictureBox picRandom;
        private BackgroundWorker Uploader;
        private System.Windows.Forms.ToolStripMenuItem viewSongDetails;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem9;
        private System.Windows.Forms.ToolStripMenuItem uploadScreenshots;
        private ToolStripSeparator toolStripMenuItem10;
        private BackgroundWorker updater;
        private ToolStripMenuItem checkForUpdates;
        private ToolStripMenuItem viewChangeLog;
        private ToolStripSeparator toolStripMenuItem11;
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
        private ToolStripSeparator toolStripMenuItem12;
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
    }
}


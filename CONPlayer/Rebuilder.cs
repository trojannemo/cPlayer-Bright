﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using cPlayer.x360;
using NautilusFREE;
using Un4seen.Bass;

namespace cPlayer
{
    public partial class Rebuilder : Form
    {
        private readonly List<Song> CurrentPlaylist;
        public List<Song> RebuiltPlaylist; 
        private readonly frmMain xParent;
        public bool UserCanceled;
        private readonly DTAParser Parser;
        private readonly bool doScanAudio;
        private readonly nTools nautilus;
        private readonly MIDIStuff MIDITools;

        public Rebuilder(frmMain parent, List<Song> playlist, bool doAudio = false)
        {
            InitializeComponent();
            xParent = parent;
            ControlBox = false;
            Parser = new DTAParser();
            nautilus = new nTools();
            MIDITools = new MIDIStuff();
            CurrentPlaylist = playlist;
            RebuiltPlaylist = new List<Song>();
            doScanAudio = doAudio;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            UserCanceled = true;
        }

        private void Rebuilder_Shown(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            Close();
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var count = 0;
            foreach (var playlistSong in CurrentPlaylist)
            {
                if (UserCanceled) return;
                var bpm = 120.0; //default
                count++;
                lblCurrent.Invoke(new MethodInvoker(() => lblCurrent.Text = "Processing song " + count + " of " + CurrentPlaylist.Count));
                lblCurrent.Invoke(new MethodInvoker(() => lblCurrent.Refresh()));
                if (!File.Exists(playlistSong.Location)) continue;
                if ((Path.GetExtension(playlistSong.Location).ToLowerInvariant()).Equals(".dta"))
                {
                    Parser.ReadDTA(File.ReadAllBytes(playlistSong.Location));
                }
                else if ((Path.GetExtension(playlistSong.Location).ToLowerInvariant()).Equals(".ini"))
                {
                    Parser.ReadINIFile(playlistSong.Location);
                }
                else if (VariousFunctions.ReadFileType(playlistSong.Location) == XboxFileType.STFS)
                {
                    var xPackage = new STFSPackage(playlistSong.Location);
                    if (!xPackage.ParseSuccess) continue;
                    Parser.ReadDTA(xPackage);                    
                    var internalname = Parser.Songs[0].InternalName;
                    var xFile = xPackage.GetFile("songs/" + internalname + "/" + internalname + ".mid");
                    if (xFile != null)
                    {
                        var tempPath = Path.GetTempFileName();
                        if (xFile.ExtractToFile(tempPath))
                        {
                            MIDITools.Initialize(false);
                            if (MIDITools.ReadMIDIFile(tempPath, 170, true))
                            {
                                bpm = MIDITools.MIDIInfo.AverageBPM;
                            }
                            else
                            {
                                bpm = 120.0;
                            }
                        }
                        File.Delete(tempPath);
                    }
                    xPackage.CloseIO();
                }
                else
                {
                    continue;
                }
                if (UserCanceled || !Parser.Songs.Any()) return;
                var index = 0;
                if (Parser.Songs.Count > 1)
                {
                    for (index = 0; index < Parser.Songs.Count; index++)
                    {
                        var song = Parser.Songs[index];
                        if (String.Equals(song.Artist, playlistSong.Artist, StringComparison.InvariantCultureIgnoreCase) &&
                            String.Equals(song.Name, playlistSong.Name, StringComparison.InvariantCultureIgnoreCase)) break;
                        if (song.InternalName == playlistSong.InternalName) break;
                    }
                }              
                
                long audioLength = 0;
                var dtaSong = Parser.Songs[index];
                if (doScanAudio)
                {
                    audioLength = GetAudioDuration(playlistSong.Location);
                }
                var newSong = new Song
                {
                    Name = xParent.CleanArtistSong(dtaSong.Name),
                    Artist = xParent.CleanArtistSong(dtaSong.Artist),
                    Location = playlistSong.Location,
                    Length = audioLength > 0 ? audioLength : (dtaSong.Length > 0 ? dtaSong.Length : playlistSong.Length),
                    InternalName = dtaSong.InternalName,
                    BPM = bpm,
                    Album = dtaSong.Album,
                    Year = dtaSong.YearReleased,
                    Track = dtaSong.TrackNumber,
                    Genre = Parser.doGenre(dtaSong.RawGenre),
                    Index = -1,
                    AddToPlaylist = true,
                    AttenuationValues = dtaSong.AttenuationValues.Replace("\t", ""),
                    PanningValues = dtaSong.PanningValues.Replace("\t", ""),
                    Charter = dtaSong.ChartAuthor,
                    ChannelsDrums = dtaSong.ChannelsDrums,
                    ChannelsBass = dtaSong.ChannelsBass,
                    ChannelsGuitar = dtaSong.ChannelsGuitar,
                    ChannelsKeys = dtaSong.ChannelsKeys,
                    ChannelsVocals = dtaSong.ChannelsVocals,
                    ChannelsCrowd = dtaSong.ChannelsCrowd,
                    ChannelsBacking = dtaSong.ChannelsBacking(),
                    DTAIndex = index,
                    isRhythmOnBass = dtaSong.RhythmBass,
                    isRhythmOnKeys = dtaSong.RhythmKeys || (dtaSong.Name.Contains("Rhythm Version") && !dtaSong.RhythmBass),
                    hasProKeys = dtaSong.ProKeysDiff > 0,
                    PSDelay = dtaSong.PSDelay
                };
                RebuiltPlaylist.Add(newSong);
            }
        }

        private long GetAudioDuration(string file)
        {
            var xPackage = new STFSPackage(file);
            if (!xPackage.ParseSuccess) return 0;            
            if (!Parser.ExtractDTA(xPackage)) return 0;
            if (!Parser.ReadDTA(Parser.DTA)) return 0;
            var internalName = Parser.Songs[0].InternalName;
            var xMogg = xPackage.GetFile("songs/" + internalName + "/" + internalName + ".mogg");
            if (xMogg == null)
            {
                xPackage.CloseIO();
                return 0;
            }
            var mData = xMogg.Extract();
            xPackage.CloseIO();
            if (mData == null || mData.Length == 0) return 0;            
            if (!nautilus.DecM(mData, false, true, DecryptMode.ToMemory)) return 0;
            var stream = 0;
            stream = Bass.BASS_StreamCreateFile(nautilus.GetOggStreamIntPtr(true), 0L, nautilus.NextSongOggData.Length, BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_SAMPLE_FLOAT);
            var len = Bass.BASS_ChannelGetLength(stream);
            var totaltime = Bass.BASS_ChannelBytes2Seconds(stream, len); // the total time length
            var Length = (int)(totaltime * 1000);
            nautilus.ReleaseStreamHandle(true);
            Bass.BASS_StreamFree(stream);
            return Length;
        }
    }
}

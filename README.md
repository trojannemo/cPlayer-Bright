# cPlayer

**As of July 7, 2025 I have stopped development of cPlayer (Dark Edition) and will only be developing cPlayer (Bright Edition) (now just only cPlayer) going forward. It is much more richly featured and with a lot more love poured into it, so check it out!**

cPlayer is my pet project, which started in 2014 when I thought to myself ... wouldn't it be cool if I could play Rock Band 3 customs like if they were regular songs?
Over time it's grown to display the MIDI contents, to display lyrics in various modes including a Karaoke-style mode, and so on to make the most use of the information contained within Rock Band files.
By now, cPlayer has expanded beyond the original Xbox 360 Rock Band files, also playing its PS3 and Wii counterparts, as well as playing songs from BandFuse, Power Gig, Rocksmith 2014, YARG, Clone Hero, Phase Shift, Guitar Hero World Tour: Definitive Edition and most recently, Fortnite Festival.

As of December 22, 2024, cPlayer (Bright Edition) has arrived, along with a host of new features for both the Bright Edition and this ("Dark Edition"), which will continue to receive support for now.
There's been significant code changes from v3.0.0 to v4.0.0 hence why I'm jumping an entire version number. Please report any bugs or feature requests on here or **on my own Discord server called "Nemo's Nautilus".**

cPlayer is designed to be intuitive and easy to use, so just play around with it. There is a Help document that you can read on how to use cPlayer.

----------

**SUPPORTED FORMATS**

**Xbox 360 (CON | LIVE)** - cPlayer will play all Rock Band songs in Xbox 360 CON or LIVE format, either in individual files or in packs. Because extracting the audio from packs can take exponentially longer than from a loose song file, it is recommended you dePACK your pack files with Nautilus' Quick Pack Editor, but it is not necessary.

**PlayStation 3 (.pkg | songs.dta)** - cPlayer will play all Rock Band custom songs in PS3 format that are stored as nested folder structures - 'packs' of songs referenced from a single songs.dta file are also supported. If you use Nautilus' PS3 Converter, then your files are already in the right structure in the Merged Songs folder or in the All Songs folder. cPlayer is limited to interpreting the midi.edat files that were encrypted using C3 CON Tools' PS3 Converter with the default KLIC license. cPlayer will most likely fail at decrypting any other midi.edat files. In that case, cPlayer won't be able to display MIDI chart visuals or read the song lyrics for those songs - but it should still function as a media player just fine. cPlayer also has limited support for .pkg PS3 files - most work, some don't. It's a work in progress.

**Wii (songs.dta)** - cPlayer will play all Rock Band songs in Wii format that are stored as nested folder structures - 'packs' of songs referenced from a single songs.dta file are also supported. If you use Nautilus' Wii Converter, then your files are already in the right structure after converting to the Wii format. cPlayer will only play Wii songs that use the mogg format for audio - at this point, BINK audio is not supported.

**PC: YARG / Clone Hero (songs.dta | .yargsong | song.ini | .sng)** - cPlayer will play all Clone Hero songs that use the MIDI format and are stored as nested folder structures or as .sng single files - there is limited support for Clone Hero songs that use the .chart format...try your luck. Clone Hero files songs with background videos will allow you to enable video playback and you can even have karaoke lyrics over the video where supported. cPlayer also supports all known YARG formats, including the .yargsong format and loose folder songs.dta structure. cPlayer may or may not work well with Phase Shift, FoF or FoFix songs - no explicit support is offered for these outdated games.

**PC: Fortnite Festival (.fnf | .m4a)** - cPlayer will play all Fortnite Festival songs that use the .fnf metadata file and .m4a audio file. Do not discuss where to get these files. But they're supported.

**PC: Rocksmith 2014 (.psarc)** - cPlayer offers basic support for audio playback of Rocksmith 2014 files in .psarc format.

**PC: GHWT:DE (.fsb.xen)** - cPayer offers basic support for audio playback of Guitar Hero World Tour: Definitive Edition files in .fsb.xen format

**PC: Power Gig (.xml)** - cPlayer offers basic support for audio playback of Power Gig files that use the .xml metadata files

**Xbox 360: BandFuse (LIVE)** - cPlayer offers basic support for audio playback of BandFuse files in LIVE format

----------

cPlayer is written in C# using .NET Framework 4.8.1 and using Visual Studio 2022 Community Edition. For best results make sure you use the same.

If you have technical questions or want to discuss cPlayer with me, I can be found in most Rock Band related Discord servers under the same username. **I also have my own Discord server called "Nemo's Nautilus".** Send me a message and let's talk.

Enjoy.

July 11, 2025

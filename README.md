# 🎵 cPlayer

![cPlayer](https://nemosnautilus.com/cplayer/v400bright.jpg)

---

## About

**cPlayer** is my long-running pet project, originally started in **2014**, born from a simple idea:

> *Wouldn’t it be cool if Rock Band 3 customs could be played like regular songs?*

Over time, cPlayer evolved far beyond that initial goal. It can now:

- Display MIDI contents
- Render lyrics in multiple modes, including **Karaoke-style playback**
- Visualize and make full use of the rich metadata contained within rhythm game files

What began as an Xbox 360 Rock Band–focused tool has expanded significantly.

---

## Supported Games & Formats

cPlayer currently supports content from a wide range of rhythm games and platforms.

---

### 🎸 Xbox 360 — Rock Band

**Formats:** `CON` | `LIVE`

- Plays all Rock Band songs in Xbox 360 CON or LIVE format
- Supports:
  - Individual song files
  - Pack files
- Extracting audio from packs can be **significantly slower** than from loose files

> 💡 It is recommended (but not required) to dePACK song packs using **Nautilus’ Quick Pack Editor**.

---

### 🎮 PlayStation 3 — Rock Band

**Formats:** `.pkg` | `songs.dta`

- Supports nested folder song structures
- Supports song packs referenced from a single `songs.dta`
- Fully compatible with files produced by **Nautilus’ PS3 Converter**

#### Limitations
- MIDI visualization and lyrics require:
  - `midi.edat` files encrypted using **C3 CON Tools’ PS3 Converter**
  - Default **KLIC license**
- Other `midi.edat` files may fail to decrypt
  - In those cases, cPlayer still functions as an audio player
- Limited `.pkg` support
  - Some packages work, some do not
  - This remains a work in progress

---

### 🎮 Wii — Rock Band

**Formats:** `songs.dta`

- Supports nested folder song structures
- Supports song packs referenced from a single `songs.dta`
- Fully compatible with files produced by **Nautilus’ Wii Converter**

#### Audio Support
- ✔️ `mogg` audio
- ❌ `BINK` audio (not supported)

---

### 🖥️ PC — YARG / Clone Hero / Phase Shift

**Formats:**  
`songs.dta` | `.yargsong` | `song.ini` | `.sng`

- Supports:
  - Loose folder songs
  - `.sng` single-file songs
- Limited support for `.chart`-based Clone Hero songs (try your luck)
- Supports background video playback where available
- Karaoke lyrics can be overlaid on videos where supported

#### Notes
- Full support for all known **YARG formats**
- Phase Shift, FoF, and FoFix may work inconsistently
  - No explicit support is provided for these legacy games

---

### 🖥️ PC — Fortnite Festival

**Formats:** `.fnf` | `.m4a`

- Fully supports Fortnite Festival songs using:
  - `.fnf` metadata
  - `.m4a` audio
- Do **not** discuss where to obtain these files

> They are supported — that’s all that needs to be said.

---

### 🖥️ PC — Rocksmith 2014

**Formats:** `.psarc`

- Basic audio playback support

---

### 🖥️ PC — Guitar Hero World Tour: Definitive Edition

**Formats:** `.fsb.xen`

- Basic audio playback support

---

### 🖥️ PC — Power Gig

**Formats:** `.xml`

- Basic audio playback support

---

### 🎮 Xbox 360 — BandFuse

**Formats:** `LIVE`

- Basic audio playback support

---

## Usability & Documentation

cPlayer is designed to be **intuitive and easy to use**.

Feel free to explore — most features are discoverable without guidance.

A Help document is included if you’d like a more structured walkthrough.

---

## Development Environment

- **Language:** C#  
- **Framework:** .NET Framework **4.8.1**  
- **IDE:** Visual Studio **2022 Community Edition**

For best results, use the same environment.

---

## Community & Support

If you have technical questions or want to discuss cPlayer:

- You can find me in most **Rock Band–related Discord servers** under the same username
- I also run my own Discord server:

**“Nemo’s Nautilus”**

Send me a message and let’s talk.

---

## Final Notes

cPlayer has grown well beyond its original scope and continues to evolve alongside the rhythm gaming community.

Enjoy.

*December 13, 2025*

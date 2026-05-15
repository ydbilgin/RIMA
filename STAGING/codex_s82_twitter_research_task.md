# Codex Task — S82 Twitter/X Research Analysis (11 links)

**Date:** 2026-05-15 S82 (kullanici uyandiginda hazir olmali)
**Mode:** Research + media download + visual analysis + report (no implementation)
**Effort:** high
**Background:** YES
**Branch:** master (no commit, sadece STAGING/ dosyalari yaz)

---

## Bağlam

Kullanici (Yasin) bu 11 X/Twitter linkini topladi. RIMA projesi (top-down 2.5D ARPG, 64px chibi, Karar #100 35° açı, Karar #135 procedural+paint+organic map hybrid) için ve yan oyun fikirleri studio'su için ilham + lockable kararlar çikartmasini istiyor. WebFetch x.com'a 402 dönüyor — sen yt-dlp/gallery-dl/playwright ile medyayi indirip analiz etmelisin.

**Kullanicinin ana sorulari (per link):**
- Bu icerik RIMA'ya gorsel/mekanik/pipeline ne katar? Lockable karar var mi?
- Stüdyom (yan oyun fikirleri klasörü) icin ekleyecegim oyun fikri var mi?
- Pixellab/Codex/Gemini birlikte bu sonucu UREYEBILIR MIYIZ?

**11 link:**

1. https://x.com/NemraV1/status/2055027312404418876
2. https://x.com/BanditKnightG/status/2055256885637374172
3. https://x.com/artofsully/status/2055082714559029683
4. https://x.com/spookjump/status/2054624501124444577
5. https://x.com/Rustic_Panda/status/2055061668686876820
6. https://x.com/BEHEMUTT/status/2054929730810564818 (FARM-TEMA, kullanici "orjinal kendine özgü farm oyun yapilabilir mi?" diye soruyor)
7. https://x.com/chongdashu/status/2055287907011768686 (**UZUN VIDEO, AI PIPELINE** — bu birinci öncelik, FULL VIDEO IZLENECEK, transcript + temel teknik takeaway listele)
8. https://x.com/LazyBearGames/status/2054977325423354267
9. https://x.com/NewRPGProject/status/2055070728551268681 ("edge uygulamasi" islerine yarar mi diye soruyor — bu linkte bir tool tanitiliyorsa onu netle, manuel alternatifi belirt)
10. https://x.com/SanteaguArt/status/2054939796339073212 ("boyle gorunum elde edilebilir mi?" diye soruyor — PixelLab + Codex ile uretilebilir mi degerlendir)
11. https://x.com/NewRPGProject/status/2055234158038007896 ("daha iyi sonuc alinabilir mi?" — kiyas yap)

---

## İş Adımları

### 1. Medya İndirme

Her link icin:
```bash
# Try gallery-dl first (best for x.com images + videos):
gallery-dl --dest STAGING/twitter_research/<id>/ "<url>"

# OR yt-dlp:
yt-dlp -o "STAGING/twitter_research/<id>/%(id)s.%(ext)s" "<url>"

# OR if both fail, playwright headless:
playwright screenshot --full-page "<url>" STAGING/twitter_research/<id>/page.png
```

**Eger X login wall ise:**
- `nitter.net` veya `nitter.privacydev.net` mirror dene
- `https://nitter.net/<user>/status/<id>` URL formatina cevir
- Video varsa `yt-dlp <nitter_url>` ile indir

**Eger hicbiri calismazsa:** Tweet text'i `curl` ile cek (HTML metadata'da twitter:title / twitter:description meta tag'leri olabilir), screenshot al, raw HTML kaydet, manuel analiz yap.

### 2. Video Frame Extraction

Indirilmis video varsa:
```bash
# Extract 5-10 key frames evenly distributed:
ffmpeg -i video.mp4 -vf "fps=1/5" frame_%03d.png

# OR scene detection:
ffmpeg -i video.mp4 -vf "select='gt(scene,0.4)',showinfo" -vsync vfr frame_%03d.png
```

**Link 7 (chongdashu)** uzun video — TUM video'yu indir, ses-yazi transcripti (`whisper` veya yt-dlp `--write-auto-subs`) cikar, key frames extract.

### 3. Görsel Analiz (Codex multimodal)

Her tweet/video icin:
- 3-5 representative screenshot sec
- Visual style: pixel art mi? boyut? palette? perspective?
- Map/level layout: tilemap mi? overlay decal mi? lighting?
- Mekanik: hangi gameplay element gosteriliyor?
- Production technique: AI pipeline mi? manuel mi?

### 4. Çıktı Dosyalari

**Ana dosya:** `STAGING/twitter_research/INDEX.md`

Format:
```markdown
# S82 Twitter Research — 11-link analiz

## Link 1: @NemraV1
**URL:** https://x.com/NemraV1/status/2055027312404418876
**Indirilen medya:** twitter_research/2055027312404418876/[video.mp4, frame_*.png]
**Visual style:** ...
**Genre/mekanik:** ...
**Production teknigi:** ...

**RIMA katki:**
- Lockable karar: ... (varsa)
- Borrowable visual technique: ... (varsa)
- Reject sebep: ... (uygulanamiyorsa neden)

**Oyun fikirleri (studio) katki:**
- Esinlenmeyle yeni oyun fikri: ... (varsa)

**Pixellab/Codex/Gemini ile UREYEBILIR MIYIZ?**
- Yes/No + nasil + ek asset boyut/sayisi tahmini

---

## Link 2: @BanditKnightG
...
```

**Yan dosyalar:**
- `STAGING/twitter_research/<id>/notes.md` — her link icin detayli notlar
- `STAGING/twitter_research/<id>/frames/` — extracted screenshots
- `STAGING/twitter_research/chongdashu_full_transcript.md` — uzun video transcript

### 5. Sentez (en sonda)

`STAGING/twitter_research/SENTEZ.md`:
- **RIMA Lockable Kararlar (oneri):** Numbered list, 3-7 madde. Karar #14X numarasi onerilebilir, MASTER_KARAR formatinda.
- **Oyun Fikirleri (studio):** 3-5 yeni oyun fikri 1-paragraflik pitch ile. (`oyun_fikirleri/` klasorune eklenmek uzere)
- **Pipeline Tavsiyeleri:** Hangi link'ten ne ogrenildi — PixelLab create_tiles_pro 64x64 mu, Pro UI 512x512 oval brushes mi, vb.
- **Pixellab Interpolate Notu:** Eger linkler arasinda interpolate-styled animation gorunuyorsa, kullanici brainstorm icin not.

---

## Out of Scope (YAPMA!)

- Unity kod degisikligi
- Asset uretimi (sadece analiz)
- Git commit (kullanici sabah commit'leyecek)
- Codex MCP write (sadece STAGING/ dosyalari)
- RIMA mevcut sistem refactor

## Tools Available

- `yt-dlp` (Python pip install veya choco install)
- `gallery-dl` (Python pip)
- `ffmpeg` (PATH'te olabilir, yoksa `winget install ffmpeg`)
- `playwright` (Python pip + `playwright install chromium`)
- `whisper` veya `whisperx` (transcript icin)
- `curl` / `wget`

Eger bir tool yoksa `pip install` veya `choco install` ile kur. Kullanici izin verir, otonom calis.

## Acceptance Criteria

1. Her 11 link icin TWITTER_RESEARCH/<id>/ klasorunde en az 1 screenshot + notes.md var
2. Link 7 (chongdashu) icin FULL transcript + 10+ frames + detayli analiz var
3. SENTEZ.md kullanici sabah okuyabilecek kalitede (3-7 RIMA karar onerisi + 3-5 oyun fikri pitch + pipeline tavsiyeleri)
4. INDEX.md tum link summary'lerini tek dosyada toplar
5. Hicbir x.com indirme basarisiz olursa, sebebi notes.md'de acikla ve mirror/alternative dene

Hata olursa: error log + best-effort sentez. Tamamlanmayan link'leri SENTEZ.md'de "INCOMPLETE" markile.

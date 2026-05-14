# Animation Video Reference Analysis
**URL:** https://www.youtube.com/shorts/1X4Oq2X41ZU
**Title:** Player character customization!
**Analiz tarihi:** 2026-05-14
**Method:** yt-dlp download + gemini vision (default model, gemini-3.1-pro-preview)

---

## Animation Style Spec

### Frame Count
- **Idle:** 4-6 frames per cycle
- **Run/Walk:** 6-8 frames per cycle
- **Attack:** 3-5 frames total (1-2 anticipation/wind-up, 1 smear/strike, 1-2 recovery)

### FPS
- **10-12 fps** — snappy "on-twos" or "on-threes" timing; keyframes held longer to produce punchy, rhythmic motion (not smooth 24 fps interpolation)

### Motion Philosophy
- **Snappy frame-skip style:** Prioritizes strong, readable key poses over fluid in-betweens
- **Anticipation / follow-through:** Highly visible — attacks hold anticipation pose briefly, then explode into strike, then slower grounded recovery
- **Squash & stretch:** Present but controlled — used for footfall weight (run down-bobs) and attack impact exaggeration; not cartoony excess

### Pixel Art Fidelity
- **Resolution:** Hi-fi, ~64x64 canvas
- **Outlines:** Thin (1px), selective outlining ("selout") — darker shades of local color rather than pure black; sprites read clean without heavy black borders

### Smear Frames
- **Yes, heavily used** — 1-2 dedicated smear frames per fast weapon swing; weapon (and sometimes arm) distorts into stylized stretched arc to convey speed and power

### Camera / Perspective
- **35-degree top-down** ("3/4 top-down") — matches Hades-style reference
- **8-direction** — depth sorting visible (weapons pass in front of/behind character depending on facing angle)

### Idle Animation
- **Dynamic, not static** — continuous rhythmic breathing loop (torso expansion/compression), subtle weapon bob, secondary motion (hair/cloth shifting); character feels alive at rest

### Run Pose
- **Extreme / exaggerated** — pronounced forward lean, wide strides, high knees; Brian's Extreme Pose category; creates urgency and momentum, not a casual stroll

### VFX Layering
- **Weapon trails:** Dense, solid-color swooshes following weapon arc during attacks
- **Dust puffs:** Simple, chunky (~3-4 frame) dust at feet during footfalls, pivots, heavy landings
- Overall density: moderate — readable and punchy, not overwhelming

---

## RIMA Mevcut Spec ile Karşılaştırma

**Mevcut RIMA spec:** 64x64 chibi, 10-12 fps, 8 yön, Hades 35 degree camera, snappy motion, Karar #114 (8 direction direct gen)

| Spec Point | Video Reference | RIMA Mevcut | Uyum |
|---|---|---|---|
| Camera angle | 35 degree top-down | 35 degree top-down | UYUMLU |
| FPS | 10-12 | 10-12 | UYUMLU |
| Direction count | 8 | 8 | UYUMLU |
| Motion style | Snappy frame-skip | Snappy | UYUMLU |
| Smear frames | Yes, 1-2 per attack | Not specified | EKSIK — ekle |
| Idle dynamism | Breathing + weapon bob | Not locked | EKSIK — belirt |
| Run pose | Extreme/exaggerated | Not locked | EKSIK — belirt |
| Outline style | 1px selout | Not specified | EKSIK — belirt |
| Sprite resolution | ~64x64 | 64x64 | UYUMLU |
| Proportions | Standard anatomy (video) | **Chibi** (RIMA) | REVIZE GEREKEN |
| VFX dust puffs | Yes (footfall) | Not specified | EKSIK — ekle |

**Uyum skoru: ~85-90%**

**Revize gereken tek nokta:** Video karakter anatomisi standart proporsiyon; RIMA chibi gerektirir (buyuk bas, kisa uzuvlar). Tum extreme run/attack pose'lari 64x64 chibi canvas icinde yeniden interprete edilmeli.

---

## Onerilen Animation Pipeline Adaptasyonu

1. **Smear frame zorunlu:** Attack animasyonlarinda 1-2 smear frame standart haline getir. PixelLab Custom V3 icin smear frame explicit prompt kilit: "one distorted stretch frame at peak weapon velocity".

2. **Idle spec kilitle:** Idle = breathing loop (4-6f) + weapon bob secondary motion. Statik idle YASAK. PixelLab idle prompt'larina "torso expansion-compression breathing cycle, subtle weapon bob" ekle (Karar #109 Ambient Idle System ile sync et).

3. **Run pose extreme ayarla:** Chibi proportionlara uyarlanmis extreme lean + high knees. Beklenti: chibi'de uzuv kisa oldugu icin lean acisi daha belirgin olmali, yoksa hareketsiz gorunur.

4. **Selout outline standardi:** PixelLab'den cikan spriteler genelde 1px outline uretir. Aseprite post-process adiminda pure-black outline varsa selout'a cevir (darker local color). Karar #71 positive-only enforcement ile tutarli.

5. **Dust puff VFX:** Footfall, pivot, heavy landing icin 3-4 frame chunky dust particle — ayri sprite sheet veya Unity particle. PixelLab animate_object ile uretilmeli (hareket animasyonu olarak degil, loop puf olarak).

6. **Proportions briefing Codex'e:** Hades-style extreme poses, chibi canvas'ta test edilmeli. Codex dispatch: "validate that extreme run lean reads correctly at 64x64 chibi scale — head 40% of total height".

---

## Method Notes

- **Direct URL attempt:** `gemini -p "... https://www.youtube.com/shorts/..."` — BASARISIZ. Gemini CLI `run_shell_command` tool bulamadi, YouTube URL'e erisemedi.
- **yt-dlp download:** Basarili. v2026.03.17, JS runtime uyarisi aldi (Deno eksik) ama android API fallback ile indirdi. Format: 360p MP4, 3.39 MiB.
- **Gemini vision (@file):** Basarili. `@STAGING/anim_ref_video.mp4` ile local dosyayi analiz etti. Model: default (gemini-3.1-pro-preview). Cikti: 43 satir, 10 kategori eksiksiz.
- **Confidence:** MEDIUM-HIGH. Gemini video icerigi gordu ve her kategori icin spesifik yanit üretti; frame count ve fps tahminleri modelin yorumuna dayanmaktadir, frame-by-frame sayim degil.

---
name: PixelLab Master Pipeline — All Asset Types
created: 2026-05-10
updated: 2026-05-13
status: LOCKED
supersedes: pixellab_animation_pipeline_v2.md (deleted, merged here)
---

# PixelLab Master Pipeline — RIMA

Tum asset tiplerinin hangi tool ile, hangi boyutta, nasil uretilecegini belirler.
Karakter animasyonu, boss uretimi, oda objeleri, tile'lar, UI — hepsi bu dosyada.

**CANONICAL TOOL REFERENCE:** `MEMORY/PIXELLAB_TOOL_GUIDE.md` (2026-05-11 user verified, live UI map).
Tutarsizlik varsa PIXELLAB_TOOL_GUIDE.md kazanir.

---

## ⚠️ S60 OVERRIDE — Pure 2D Top-Down (2026-05-13 LOCKED, supersedes 2.5D rules below)

> Bu dosyanin geri kalanindaki "8 direction mandatory", "252x252 canvas", "128px karakter", "body-only anchor", "2.5D mimari" referanslari REVOKED. S59 pivot (2026-05-12) + S60 production lock'larina gore guncel kurallar:

| Konu | Eski (2.5D / Faz 1 ChatGPT) | S60 LOCKED (2026-05-13) |
|---|---|---|
| **Karakter sprite** | 128-252px native + body-only anchor + WeaponAnchorMap | **64x64 chibi + silahli 1-piece**, PixelLab Create Character (Pixen) |
| **Animation direction (MVP)** | 8 direction mandatory (S/SE/E/NE/N/NW/W/SW) | **4 direction (N/S/E uretilir, W=flipX)** — 8 direction post-v1 |
| **Canvas boyutu** | 252x252 (Pixflux v3) veya 256x256 | **64x64 karakter / 32x32 tile / 64-128 VFX / 128 elite mob / 256 act+final boss** (2^n hierarchy) |
| **PPU** | PPU=32 (Final Boss 96px insan formu Faz 4) | **PPU=64 TUM sprite** (boyut farki sprite canvas ile gelir, PPU manipulasyonuyla DEGIL) |
| **Tile boyutu** | 64x64 floor + 64x128 iso wall | **32x32 top-down floor + 32x32 top-down wall + 32x32 decal** |
| **Map Workshop** | (planlama vardi) | **YASAK (Karar #75)** — sadece single tile ok; multi-tile connected output yasak |
| **Stil referansi** | 30deg ChatGPT init / 252px Pixflux | **Into Samomor (RPG Maker MZ) — 35° + 1-piece + 4 yon + neon accent + mat env palette** |
| **Anim view** | low/medium top-down deneme | **High top-down ~30-35° (Hades match) KEEP** |
| **Anim FPS** | 8 | **10-12 fps** |
| **Renderer** | URP 3D + Billboard (2.5D) | **URP 2D Renderer + Pixel Perfect Camera + 2D Lights** |

**Production pipeline (S60 LOCKED):**
1. PixelLab AI uretim (NEW > PRO, transparent BG for character)
2. **Manuel cleanup 5-15dk** (Aseprite/Photoshop): silhouette + outline + padding (hibrit zorunlu, saf MCP-only YETERSIZ — Karar #72/76 LOCKED)
3. Unity import: Point filter, no compression, no mipmap, PPU=64
4. SpriteAtlas pack per category
5. QC: silhouette readable + tone match + size hierarchy correct

**Tool secimi S60 (cogu KEEP):**
- Karakter: create_character (Pixen) NEW + 4 direction
- Tile: create_image_pixen NEW (S-XL) — single 32x32, chromakey #00FF00 fallback
- Animation: Custom Animation V3 (karakter sayfasindan Add Animation) — 4 yon
- Boss/Mob: create_character (mob preset) NEW + Pro fallback (drift varsa)
- VFX 64-128px: create_object NEW + transparent BG
- UI icon: create_ui_elements_pro

**REVOKED bu dosyada (aramayin, kullanmayin):**
- "8 direction mandatory" referanslari → 4 direction MVP
- "252x252 / 256x256 canvas" → 64x64/128/256 2^n hierarchy
- "Body-only anchor" → silahli 1-piece
- "PPU=32 Final Boss" → PPU=64 standardize
- "create_topdown_tileset connected output" → tek tile NEW only (Karar #75)

Detay: `MEMORY/project_pure_2d_topdown_pivot_2026-05-12.md`, `MEMORY/project_64px_armed_character_locked.md`, `MEMORY/project_boss_size_hierarchy_2026-05-12.md`, `MEMORY/project_visual_canonical_pin_2026-05-12.md`.

---

## 0. HARD RULES — TOOL SECIM ONCELIGI (2026-05-11 LOCKED)

```
KURAL: NEW > PRO. NEW tool'lar oncelikli, PRO sadece kalite-kritik durumda.

Sebep: NEW tool'lar yeni model + outline/detail kontrolleri + olcek esnekligi
icinde, cogu RIMA pixel art use-case'inde PRO esdeger sonuc verir.
PRO 20-40 gen tuketir, NEW genelde dahil/dusuk maliyet.

UYGULAMA:
  1. Tool secimi yaparken once NEW karsiligi var mi bak (PIXELLAB_TOOL_GUIDE.md)
  2. NEW yeterli → kullan
  3. NEW'de drift / kalite sorunu → PRO'ya fallback
  4. Yeni karar verirken: NEW olanlardan sec, PRO gerekiyorsa gerekce yaz

ORNEK:
  - Silah sprite: create_image_pixen (S-XL NEW) ✓ kullaniliyor
  - Weapon removal: ONCE edit_image (NEW free), drift varsa edit_image_pro (PRO)
  - Animation standalone: animate_with_text (NEW) > animate_with_text_pro (PRO)
  - Animation karakterden: Character Creator > Add Animation (Custom Animation V3 flow)
```

---

## 0. HARD RULES — ANIMASYON PIPELINE (2026-05-10 LOCKED)

Bu kurallar Bolum 5-6'nın ustumdedir. Celisme durumunda bu bolum kazanir.

### 0.1 Animasyon Tool — KESIN KURAL

```
DOGRU: Custom Animation V3 (karakter sayfasindan Add Animation)
  → Her zaman Start Frame = kendi temizlenmis sprite'in (_clean.png)
  → End Frame = gerektiginde (walk cycle, 3-segment attack)
  → Frame Count: 4-8 (cift sayi)

YASAK: Standalone "Animate with Text NEW" — karakter sayfalarina bagli degil
YASAK: animate_character MCP — 4-frame limit + VFX bug
YASAK: Preset butonlari (Idle/Walking/Running) — eski motor

SEBEP: Tum player/mob/boss karakterleri Create Character sisteminde kayitli.
Custom Animation V3 bu sistemden beslenir ve karakter iskeletini bilir.
```

### 0.2 Mob/Boss Animasyon Onhazirlik

```
Mob/Boss varsayilan olarak Create Character sisteminde DEGILDIR.
Animasyon yapilmadan once sisteme alinmali:

ADIM 1: Diger tool ile base sprite uret (Create Image PRO → 256px veya 128px)
ADIM 2: Eraser Pass → _clean.png
ADIM 3: PixelLab → Create Character sayfasi → "Create from Reference V3"
         Input: _clean.png → karakter sisteme alinir
ADIM 4: Custom Animation V3 ile animasyon (0.1 kurali)
```

### 0.3 Yon Uretimi — 8 YON ZORUNLU

```
Her karakter/mob/boss icin 8 yon uretilir:
  S / SE / E / NE / N / NW / W / SW

Oyunda kullanilan: 4 ana kardinal (S/E/N/W)
Uretilen: 8 (gelecek v2/v3 icin + daha iyi flip kalitesi)

Create Character Pro New: Yon seciminde 8 Directions sec
Create from Reference V3: Ayni sekilde 8 yon
```

### 0.4 Start Frame Kurali (Custom Animation V3 icin)

```
START FRAME: Her zaman _clean.png (Eraser Pass sonrasi) — ZORUNLU
END FRAME:
  - Walk cycle: Extreme Pose B (A'nin flipi)
  - Attack windup: PEAK frame
  - Attack follow: idle_clean.png
  - Hurt/Death/Idle: BOS BIRAK (End Frame yuklemeden)
Keep First Frame:
  - ON: idle, walk, hurt, dash, attack segment'leri
  - OFF: death, phase transition
```

---

---

## 1. ASSET TIPI → TOOL YONLENDIRME TABLOSU

| Asset Tipi | Uretim Tool | Boyut | Varyasyon | Maliyet |
|---|---|---|---|---|
| **Player sinif base** | Create Character Pro New | 252x252 canvas, ~128px | 1 | Karakter sistemi |
| **Boss base** | Create Image PRO | 256x256, ~160-192px | 1 frame | 20 gen |
| **Regular enemy base** | Create Image PRO | 256x256, ~80-100px | 1 frame | 20 gen |
| **Silah sprite (2.5D)** | **Create Image S-XL (new)** + DirectionNone + init image | 64/128/256 native | 1 canonical | Bolum 9 |
| **Oda objesi (statik)** | Create Image PRO veya S-XL (new) | 64x64 → 16 var | 16 | 20 gen |
| **Oda objesi (statik, kucuk)** | Create Image PRO | 32x32 → 64 varyasyon | 64 | 20 gen |
| **Oda objesi (animasyonlu)** | Create Object (Web) + animate_object MCP | 48-64px, 1/8 yon | 16@48px | 20-40 gen |
| **Isometric floor tile** | Create Tiles Pro | Isometric, 32x64 veya 64x128 | Set | 20-25 gen |
| **Isometric wall tile** | Create Isometric Tile (Aseprite/Pixelorama) | 32px | Tek | MCP/eklenti |
| **UI element** | Create UI Elements Pro | 32-512px | Grid | 20-40 gen |
| **Skill/item icon** | Create Image PRO | 32x32 → 64 icon / 64x64 → 16 icon | Batch | 20 gen |
| **Map (dunya haritasi)** | KULLANILMAYACAK | — | — | — |

---

## 2. VARYASYON GRID SISTEMI (Create Image PRO)

Create Image PRO canvas boyutuna gore otomatik grid olusturur:

```
  32px veya daha kucuk  → 64 frame (8x8 grid) — 20 gen
  33-42px               → 64 frame (8x8 grid) — 25 gen
  43-64px               → 16 frame (4x4 grid) — 20 gen
  65-85px               → 16 frame (4x4 grid) — 25 gen
  86-128px              →  4 frame (2x2 grid) — 20 gen
  129-170px             →  4 frame (2x2 grid) — 25 gen
  171-256px             →  1 frame             — 20 gen
```

Prompt stratejisi:
- 64px "5 farkli sandik tipi" → 16 frame, her biri farkli tasarim
- 64px "treasure chest" → 16 frame, ayni sandigin varyasyonlari
- 32px "dungeon props: barrel, crate, pot, torch, skull" → 64 frame
- Prompt'ta "each item different" dersen farkli objeler cikar
- Prompt'ta tek obje tanimlarsan ayni objenin varyasyonlari cikar

---

## 3. CREATE IMAGE PRO — DETAYLI KULLANIM

### Arayuz Alanlari

```
DESCRIPTION (zorunlu):
  Structured prompt formati kullan (Bolum 10).

OUTPUT SIZE:
  256x256 (boss/karakter) | 64x64 (obje batch) | 32x32 (icon batch)

REFERENCE IMAGES (opsiyonel, max 4 adet, max 1024x1024):
  Mevcut 10 karakter sprite'larindan sec.
  Prompt'ta "similar armor to reference image 1,
  dark cloak flow like reference image 2" seklinde referans ver.
  AMACI: Icerik/tasarim referansi.

STYLE IMAGE (opsiyonel, max 1 adet, max 512x512):
  HER ZAMAN rima_style_anchor.png (252x252, paddingli) yukle.
  AMACI: Pixel art stilini (outline, shading, renk gecisleri) kopyalar.
  UYARI: Scale'i de kopyalar — kirpilmis sprite KOYMMA (Scale Trap).

REMOVE BACKGROUND: HER ZAMAN ON

MALIYET: 20 generation (sabit)
```

### Style Image Kurallari (Scale Trap Onleme)

```
YUKLENEBILIR:
  rima_style_anchor.png (252x252, %60 padding)
  Approved base sprite (252x252, paddingli)

YUKLENEMEZ:
  128x128'e kirpilmis sprite (padding yok)
  64x64 veya 56x56 kucuk sprite
  Paddingsiz temizlenmis sprite

Pre-check: Dosya 252+ mi? Karakter %50-75 canvas mi? Padding var mi?
Fail → rima_style_anchor.png'e geri don.
```

### Reference Image Secim Stratejisi

```
ZIRHLI BOSS: Ref 1 = warblade_base_S.png
BUYUCU BOSS: Ref 1 = elementalist_base_S.png
KARANLIK BOSS: Ref 1 = shadowblade_base_S.png
4 slot kullanilabilir — prompt'ta "reference image 1/2/3/4" ile baglantila.
```

---

## 4. CREATE OBJECT (Web App) — ANIMASYONLU ODA OBJELERI

Ekran goruntusu analizi (pixellab.ai/create-object):

```
ARAYUZ:
  Directions: 1 Direction veya 8 Directions
  Size: Slider 32px — 256px (default 48px)
  Default Style View: Top-Down
  Object Description: Serbest text
  "Describe each item (16)": Her frame'e ayri prompt verilebilir
  Maliyet: 20-40 gen (boyuta gore)
  Cikti: 48px → 16 frame (4x4 grid)

RIMA KULLANIMI:
  - Animasyonlu oda objeleri: mesale, kaynak, tuzak, cesme, portal
  - 1 Direction yeterli (top-down isometric)
  - Size: 48-64px obje boyutuna gore
  - animate_object MCP ile sonradan animasyon eklenebilir

PIPELINE:
  1. Create Object (Web) → base obje
  2. animate_object MCP → "flickering flame" / "bubbling cauldron"
  3. Indir, Pixelorama'da temizle
  4. Unity import (PPU=64, Point filter)

STATIK OBJELER ICIN CREATE IMAGE PRO TERCIH ET:
  Style image + reference image destegi var, stil tutarliligi daha iyi.
  Create Object'te bu secenekler yok.
```

---

## 5. CUSTOM ANIMATION V3 — START FRAME / END FRAME

### Arayuz

```
ERISIM: Character sayfasi > Add Animation > Custom Animation V3
ACTION DESCRIPTION: Serbest text
FRAME COUNT: Slider 4-8 (252px canvas'ta max 8)
KEEP FIRST FRAME (idle pose): Checkbox
ADVANCED OPTIONS > CUSTOM FRAMES:
  START FRAME: Yukle veya galeriden sec
  END FRAME: Yukle veya galeriden sec
MALIYET: 8 gen/direction @ 8 frame
```

### Senaryo A: Walk Cycle

```
ADIM 1: Animate with Text NEW → 12 frame "running forward" uret
ADIM 2: En uc pozu sec (bacak en genis acida) → Pose A kaydet
ADIM 3: Aseprite/Pixelorama'da Pose A'yi yatay flip → Pose B
ADIM 4: Custom Animation V3:
  Start Frame: Pose A, End Frame: Pose B
  Action: "smooth walk cycle transition, weight shifts
    between legs, continuous forward motion"
  Frame Count: 6, Keep first frame: ON
SONUC: 6 frame walk cycle

ALTERNATIF: Interpolate NEW (Pose A + Pose B → 4-6 frame)
Her iki yontem gecerli. Kalite karsilastir, iyi olani sec.
```

### Senaryo B: 3-Segment Attack

```
PEAK FRAME URET:
  - Animate with Text NEW → "peak slash impact" → en iyi frame sec
  - VEYA Edit Image Pro ile idle'a silah pozu ekle
  - VEYA Pixelorama'da manuel ciz

SEGMENT 1 (idle → PEAK — windup):
  Start: idle_clean.png
  End: PEAK frame
  Action: "anticipation windup, body coils, weight to back foot"
  Frame: 4, Keep first: ON

SEGMENT 2 (PEAK → recovery — follow-through):
  Start: PEAK frame
  End: idle_clean.png
  Action: "follow-through, momentum forward, deceleration to idle"
  Frame: 4, Keep first: ON

BIRLESTIR: Aseprite'ta Seg1 (4f) + Seg2 (4f) = 8 unique frame
Pixel budget: 252x252 x 8 = 508,032 < 524,288
```

### Senaryo C: Hurt / Flinch

```
Start: idle_clean.png, End: BOS BIRAK
Action: "sharp flinch backward from impact, quick recovery"
Frame: 4, Keep first: ON
Son frame'i Aseprite'ta idle ile degistir (clean loop).
```

### Senaryo D: Death

```
Start: idle veya hurt son frame
End: BOS BIRAK
Action: "collapse to ground, heavy fall, body goes limp"
Frame: 6-8, Keep first: OFF
```

### Keep First Frame Kurali

```
ON: idle, walk, hurt, dash, attack segment'leri
OFF: death, phase transition
```

---

## 6. ANIMASYON PIPELINE — GENEL

### Tool Secimi

```
PLAYER/BOSS ANIMASYONU:
  Uretim: Animate with Text NEW (standalone) — Eraser Pass uyumlu
  Walk: Custom Anim V3 Start/End VEYA Interpolate NEW
  Attack: 3-segment Start/End Frame ile

OBJE ANIMASYONU:
  Basit (mesale, kaynak): animate_object MCP
  Kompleks (buyuk): Animate with Text NEW

YASAKLI: animate_character MCP, Preset butonlari (Idle/Walking/Running)
```

### Frame Sayilari

| Anim | Player | Boss |
|---|---|---|
| Idle | 6-8 | 6-8 |
| Walk | 6 | — |
| Attack | 8 (2x4 segment) | 8 (2x4 segment) |
| Dash | 4 | — |
| Hurt | 4 | 4 |
| Death | 6-8 | 8 |
| Special/AOE | — | 6-8 |
| Phase Transition | — | 6-8 |

---

## 7. BOYUT VE PPU TABLOSU — LOCKED

| Varlik | Canvas | Karakter px | Unity Crop | PPU | Ekran |
|---|---|---|---|---|---|
| Player | 252x252 | ~128px | 128x128 | 64 | ~2 unit |
| Regular enemy | 252x252 | ~80-100px | 128x128 | 64 | ~1.5 unit |
| Miniboss | 252x252 | ~128-140px | 128x128 | 64 | ~2.2 unit |
| Act Boss | 256x256 | ~160px | KIRPMA | 48 | ~5.3 unit |
| Final Boss | 256x256 | ~192px | KIRPMA | 32 | ~8 unit |
| Architect | 256x256 | ~200-210px | KIRPMA | 32 | ~8 unit |
| Oda objesi | 48-64px | — | Yok | 64 | ~0.75-1 unit |
| Floor tile | 64x64 sprite | — | Yok | — | Grid cell |
| Wall tile | 64x128 sprite | — | Yok | — | Grid cell |
| Skill icon | 32/64px | — | Yok | — | UI |

**KRITIK:** Boss sprite'lari 128x128'e KIRPILMAZ. 256x256 olarak import.

---

## 8. ISOMETRIC TILE PIPELINE

```
FLOOR TILE:
  Tool: Create Tiles Pro (Isometric, 32 veya 64, Low top-down)
  Style tiles ile referans verilebilir

WALL TILE:
  Tool: Create Isometric Tile (Aseprite/Pixelorama, Block/Thick, 32px)
  Init image + description ile tutarli stil

MAP TOOL'LARI KULLANILMAYACAK:
  Create Map, Extend Map devre disi.
  RIMA kendi Unity-based isometric design tool'unu gelistiriyor.
  Tile'lar tek uretilip Unity tool'una import edilecek.
```

---

## 9. EK TOOL'LAR

### Edit Image Pro (Weapon Pass)

```
AMAC: Body-only sprite'a silah ekleme
YONTEM: "Edit with text" → "add a sword to right hand"
  VEYA "Edit with reference" → silah gorsel referansi
BOYUT: 32-256px, 1 frame, 20 gen
```

### Edit Animation Pro (Toplu Frame Duzenleme)

```
AMAC: Tum anim frame'lerine ayni anda duzenleme
ORNEK: "add glowing sword to all frames"
LIMIT: 252px'te max 4 frame (40 gen)
RIMA: Weapon Pass'i tum frame'lere tek seferde uygulamak icin
```

### Transfer Outfit Pro

```
AMAC: Referans outfit'i animasyon frame'lerine transfer
LIMIT: 252px'te max 3 frame (40 gen)
RIMA: V1'de dusuk oncelik — ekipman stat-only olabilir
```

### Animation to Animation

```
LIMIT: 128x128, 64x64, 32x32, 16x16 SADECE
RIMA: 252px canvas icin KULLANILAMAZ
  64px obje animasyonlari icin potansiyel
```

### Create UI Elements Pro

```
AMAC: HP bar, buton, inventory slot, menu frame
BOYUT: 32-512px, Concept Image referansi ile
RIMA: Ashen Glyph UI temasi icin pixel art asset'leri
```

### Create Image S-XL (new) — Silah ve Statik Obje (2026-05-11 eklendi)

```
AMAC: Silah sprite, statik obje, non-character asset uretimi
TAG: NEW model, outline + detail controls

ARAYUZ AYARLARI:
  Description: Text prompt (init image varsa kisa, yoksa detayli)
  Direction: None (RECOMMENDED for non-characters) — silah/obje icin sec
  View: High top-down (RIMA anchor stili ile uyumlu)
  Detail: Highly detailed (silah/obje icin onerilen)
  Outline: Single color outline (1px siyah icin optimal)
  Init Image: Drag-drop PNG/JPG, max 10MB — style/ref esdegeri
  Width: 32 / 64 / 128 / 256 / 512 / 768
  Height: 32 / 64 / 128 / 256 / 512 / 768
  Transparent Background: ON (native, Remove Background gerekmez)

CANVAS SEKIMI (size affects output):
  - Karakter icin daha uzun canvas (heightes weighted)
  - Sahne icin daha genis canvas (width weighted)
  - Silah/obje icin: target boyutuna en yakin ust degeri sec
  - 192 yok → 256 kullan, transparent padding sorun degil

RIMA WEAPON KULLANIMI:
  - Warblade greatsword: 256×256 (target 150px, padding transparent)
  - Shadowblade dagger: 64×64 (target 40px)
  - Ranger bow: 128×128 (target 110px)
  - Direction: None her zaman
  - View: High top-down her zaman
  - Detail: Highly detailed
  - Outline: Single color outline
  - Init image: <class>_<weapon>_init.png (anchor'dan crop)

NEDEN BU TOOL (Create Image PRO yerine):
  - Direction None secenegi non-character icin OZEL tasarlanmis
  - View setting native (prompt'a yazmaya gerek yok)
  - Detail + Outline ayri kontroller → tutarli sonuc
  - Native canvas sizing → trim yok
  - Init image style+ref esdegeri (1 slot ama yeterli silah icin)

CREATE IMAGE PRO TERCIH EDIN EGER:
  - 4 reference image gerekirse (karakter konsept arastirma)
  - Tarihsel/tematik multi-ref karsilastirma
  - Style image + reference image kombinasyonu zorunluysa
```

---

## 10. STRUCTURED PROMPT FORMATI — ZORUNLU

### Karakter/Boss Prompt Semasi

```
TYPE: [humanoid / creature / beast / construct / undead]
STYLE: 16-bit RPG [player/boss/enemy] sprite, high top-down
  30-35 degree, pixel art, hard edges, no anti-aliasing
HEAD: [kafa detaylari]
BODY: [vucut yapisi ve durusu]
LIMBS: [kol/bacak ozellikleri]
EXTRA: [ek ogeler]
CLOTHING: [giysi/zirh]
HANDS: [ellerde ne var]
SILHOUETTE: [1 cumle squint test]
COLOR: [renkler + accent + shade steps]
POSE: [sadece animasyon prompt'larinda]
--- RULES ---
Character occupies ~[50/65/75]% of canvas height.
DO NOT fill canvas.
No embedded glow (VFX engine-side).
Hard pixel edges, clean clusters min 4px, no dithering.
High top-down view 30-35 degree.
FACING [SOUTH/EAST/NORTH/WEST].
NEGATIVE: blur, 3d render, smooth gradient, anti-aliasing,
  ambient occlusion, photo-realistic
```

### Boss Prompt Ornegi (Final Boss)

```
TYPE: massive humanoid construct
STYLE: 16-bit RPG final boss sprite, high top-down 30-35,
  pixel art, hard edges, dark imposing presence
HEAD: cracked stone helmet fused with skull, single glowing
  cyan eye slit (#00FFCC), broken crown fragments
BODY: towering heavy frame, ancient stone-metal armor,
  rift energy veins in cracks, broad shoulders, hunched
LIMBS: massive arms, left=oversized stone fist,
  right=jagged rift-crystal blade, thick legs
EXTRA: floating debris (2-3 rocks) at shoulders,
  faint cyan rift glow at chest core (minimal),
  tattered dark cape remnants
CLOTHING: ancient warden plate, corroded bronze-black,
  cyan energy seams (#00FFCC dim), deteriorated straps
HANDS: left=clenched stone fist, right=crystal blade
SILHOUETTE: towering hunched armored figure, asymmetric
  arms, broken crown, single eye — instant boss read
COLOR: armor #3A3028/#4A4038/#5A5048, stone #6A6A6A/#8A8A8A,
  rift cyan #00FFCC (MINIMAL max 5%), cape #1A0E2A
--- RULES ---
Character occupies ~75% of canvas height (~192px).
DO NOT fill canvas — ~30px padding each side.
No embedded glow (VFX engine-side only).
Rift cyan #00FFCC ONLY on eye slit and armor seams.
Hard pixel edges, clusters min 4px, no dithering.
High top-down 30-35 degree. FACING SOUTH.
```

### Animasyon Prompt Ornegi (Ranger Walk)

```
TYPE: humanoid
HEAD: hooded cloak, sharp eyes, defined jaw
BODY: lean athletic, upright relaxed stance
LIMBS: standard arms and legs
EXTRA: short shoulder mantle
CLOTHING: leather armor, tunic, bracers, belt, tall boots
HANDS: empty hands
SILHOUETTE: hood peak, shoulder mantle, tall boots
COLOR: earthy greens, dark browns, 2-3 shade steps
POSE: mid-stride, left foot forward and planted, right foot
  pushing off, right arm forward, left arm back, slight lean
--- RULES ---
Use provided sprite as exact reference for scale, proportions,
  colors, shading, outline, pixel density.
Do not redesign, restyle, or add detail.
SIZE LOCK: identical canvas size as base.
FOOTPRINT LOCK: identical pixel extents all frames.
ANCHOR: feet same row, center+head height match.
Clean clusters, no dithering, no noise. No weapons.
```

---

## 11. ERASER PASS — ZORUNLU ARA ADIM

```
Her animasyon ONCESI base sprite temizlenmeli:
1. Pixelorama'da ac, Zoom 800%+
2. Stray pixel temizligi (siluet disi 1-2px)
3. Cift outline → tek outline (1px standart)
4. Mixel duzeltme
5. *_clean.png kaydet
6. Animasyon tool'larina BU versiyonu ver

HEAD SWAP QC (idle/walk/dash/attack anim icin):
  Anim sheet sonrasi her frame'e base head region paste
  Hurt ve death icin HEAD SWAP YAPMA
```

---

## 12. PIXEL BUDGET — LOCKED

```
width x height x frame_count <= 524,288
252x252 x 8 = 508,032 (gecerli)
256x256 x 8 = 524,288 (tam sinir)
9+ frame = IMKANSIZ (bu canvas'larda)
Frame parity: SADECE cift sayi (4/6/8/10/12/14/16)
```

---

## 13. UNITY IMPORT SABITLERI — LOCKED

```
Texture Type:  Sprite (2D and UI)
Sprite Mode:   Multiple
Filter Mode:   Point (no filter)
Compression:   Uncompressed
Pivot:         Center (0.5, 0.5)
PPU:           Varlik tipine gore (Bolum 7)
PPU < 16:      YASAK (physics bozulur)
```

---

## 14. BOSS URETIM PIPELINE — TAM AKIS

```
=== DISCOVERY ===
ADIM 1: Create Image PRO ile boss konsept
  256x256, Style=rima_style_anchor.png, Ref=mevcut karakterler
  Birden fazla seed dene (20 gen/deneme, ucuz)

=== PRODUCTION ===
ADIM 2: Approved base → 4 yon
  Her yon ayri Create Image PRO (approved S'yi reference ver)
  Boss icin 2-3 yon yeterli olabilir

ADIM 3: Eraser Pass (Pixelorama) → *_clean.png

ADIM 4: Animasyonlar (Animate with Text NEW veya Custom Anim V3)
  | Anim | Frame | Start | End | Keep1st |
  | Idle | 6-8 | idle_clean | BOS | ON |
  | Attack_1 windup | 4 | idle_clean | PEAK_1 | ON |
  | Attack_1 follow | 4 | PEAK_1 | idle_clean | ON |
  | Attack_2 windup | 4 | idle_clean | PEAK_2 | ON |
  | Attack_2 follow | 4 | PEAK_2 | idle_clean | ON |
  | Special/AOE | 6-8 | idle_clean | BOS | ON |
  | Hurt | 4 | idle_clean | BOS | ON |
  | Phase Trans | 6-8 | phase1_idle | BOS | OFF |
  | Death | 8 | idle_clean | BOS | OFF |

ADIM 5: Weapon Pass (gerekirse) — Edit Image Pro
ADIM 6: Unity import (256x256 KIRPMA, PPU=32/48, Point, Uncompressed)
```

---

## 15. KARAR SINIRLARI

### LOCKED (degistirilemez)

1. Canvas: 252x252 veya 256x256, asla 512+
2. Pixel budget: WxHxF <= 524,288
3. Frame parity: cift sayi
4. animate_character MCP yasagi — kalici
5. Style image: rima_style_anchor.png paddingli, kirpilmis YASAK
6. Negative prompt blogu zorunlu
7. Point filter + Uncompressed import
8. Eraser Pass zorunlu
9. Embedded VFX yasagi
10. PPU < 16 yasak
11. Boss sprite 128x128'e KIRPILMAZ
12. Structured prompt formati zorunlu
13. Map tool'lari KULLANILMAYACAK

### Degistirilebilir

1. Prompt icerigi (tasarim, renk, siluet)
2. Frame sayisi (4-8 arasi, cift, budget icinde)
3. Animasyon siralama
4. Yon sayisi (boss 2-3 yeterli)
5. Start/End Frame mi Interpolate mi
6. Reference image secimi
7. Obje boyutu (48 vs 64px)
8. Boss animasyon listesi

### Kullaniciya sormali

1. Boss tasarim detaylari
2. Phase transition var mi
3. Boss yon sayisi (2 vs 4)
4. PPU onay (48 vs 32)
5. Obje animasyon listesi

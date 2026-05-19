# Codex Task — LoRA Training Reference Tile Collection

## Hedef

RIMA tile LoRA training için **150-250 yüksek kaliteli pixel art floor/wall/decal tile** referansı topla, kategorize et, `F:/LaurethStudio/05_TRAINING/RIMA_tile_lora_v1/refs_raw/` klasörüne kaydet.

## Style Bar (her ref bu kriterleri karşılamalı)

- **Native pixel art** — hard pixel edges, NO anti-aliasing, NO soft gradient
- **Top-down high angle** (30-60° eğim) veya **pure top-down** (ground tiles için)
- **Tile size** 16-128px range (32, 48, 64 ideal; 128 üst sınır)
- **Atmosphere:** weathered stone, dungeon/temple/ruin, dark moody — NOT bright pastoral, NOT cartoon
- **Palette:** dark tones dominant (slate gray, brown, deep blue, moss green) — NOT bright saturated colors
- **Era:** post-2018 pixel art standards (modern indie pixel art tradition — CrossCode/Hyper Light Drifter/Eastward/Tunic family)

## Reject (BU TÜR KOPYALAMA)

- Bright cartoon RPG Maker assets
- Anime/manga-tile painted style
- Vector/illustrator look
- 3D rendered downscaled
- Photo-mosaic textures
- AI-generated soft pixel art (anti-aliased fake-pixel-art)
- Cute pastel daylight farming-sim aesthetic

## Acceptable Source Categories

### 1. itch.io CC-licensed tilesets (PRIORITY 0)

Curl/wget ile **doğrudan download** edilebilecek pack'ler:

- **Cainos — Pixel Art Top Down (Basic/Castle/Forest/Caves)** — `cainos.itch.io/pixel-art-top-down-basic` (free)
- **ZB-Kappa — Dungeon Tilesets** — `zb-kappa.itch.io`
- **Pita — Cosmic Time Chase / dungeon packs** — `pita-dreams.itch.io` (some free)
- **HylianShield — RPG Tilesets** — `hylianshield.itch.io`
- **PixelFrog — Pixel Adventure / Dungeon** — free assets
- **ansimuz — Dungeon Asset Pack** — `ansimuz.itch.io` (free)
- **AnokoYZ — Stone Caverns** — itch.io
- **0x72 — Dungeon Tileset** — `0x72.itch.io` (DungeonTilesetII free)

### 2. Kenney.nl (PRIORITY 1)

- **Tiny Dungeon** (16×16 native, large pack)
- **Tiny Town**
- **Roguelike Pack**
- Direct: `kenney.nl/assets`

### 3. OpenGameArt.org (PRIORITY 1)

- Filter: "tileset" + "dungeon" / "stone" / "cave"
- Top contributors: **Buch, Reemax, surt, daneeklu**
- Search: `opengameart.org/art-search?keys=dungeon+tileset+pixel`

### 4. GitHub free asset repositories (PRIORITY 2)

- **deepnight/ldtk-examples** — Sample tilesets in LDtk format
- **PixelPress** community tilesets

### 5. CraftPix.net free section (PRIORITY 3)

- Filter: free tilesets, top-down RPG, dungeon, stone
- `craftpix.net/freebies/`

## Önemli — Lisanslama

**SADECE indirilebilenler:**
- CC0 / CC-BY (attribution required)
- "Free for personal use"
- Kenney.nl CC0
- Marked free on itch.io

**SAKIN INDIRMEYIN:**
- "Paid only" packs
- Studio assets (CrossCode resmi sprite'ları — copyright)
- Alabaster Dawn screenshots (Radical Fish copyrighted)
- Hyper Light Drifter assets (Heart Machine copyrighted)

LoRA training için kişisel kullanım sınırı tartışmalı bir gri alan — risk almayalım, CC-licensed yeter.

## Klasör Yapısı

Tüm raw downloads **SADECE `refs_raw/codex/`** subfolder'a:

```
F:/LaurethStudio/05_TRAINING/RIMA_tile_lora_v1/
  └── refs_raw/
       └── codex/                       ← BU TASK'IN ÇIKTI YERI
            ├── cainos_top_down_basic/  ← original pack folder
            ├── kenney_tiny_dungeon/
            ├── opengameart_buch_dungeon/
            ├── 0x72_dungeon_tileset/
            └── ... (her pack ayrı klasör)
```

**Diğer iki agent (Opus, Antigravity) `refs_raw/opus/` ve `refs_raw/antigravity/` subfolder'larını dolduracak — paralel triangulation. Bu task SADECE `refs_raw/codex/`.**

Her pack klasörünün içinde **bir `SOURCE.md` dosyası** olsun:
- Original URL
- License (CC0/CC-BY/etc.)
- Author / attribution required (true/false)
- Download date

## Codex'ten Beklenen

1. Yukarıdaki kaynaklardan **150-250 tile asset PNG dosyası** indir
2. Her pack için klasör + SOURCE.md
3. Sonunda **özet rapor** yaz: `F:/LaurethStudio/05_TRAINING/RIMA_tile_lora_v1/refs_raw/codex/_INDEX.md`
   - Toplam dosya sayısı
   - Pack başına dosya sayısı
   - Lisans dağılımı (CC0 / CC-BY / etc.)
   - Style bar match: tahmini PASS oranı
4. Manuel curation için klasör hazır

## Kısıtlama

- **SADECE pixel art tile.** Karakter/mob/UI/prop SPRITE'LARI ATLA (sadece floor/wall/ground/cave/dungeon tile).
- **Tile boyut filtre:** image height ≤ 256px, width ≤ 256px. Daha büyük dosya = atlanmalı (genelde sprite sheet).
- **Manual eleme yapma** — toplama görevi. Filtering ben + user yapacağız.

## Done.md

`STAGING/codex_lora_ref_collection_DONE.md`

İçerik:
- Total files downloaded
- Pack-level breakdown
- License summary
- Folder location
- Issues encountered (rate-limit, paywall, missing license info, vb.)

## Tek hedef

Curl / wget / Python `requests` ile **gerçek pack indir.** Eğer bir kaynağa erişim yoksa, atla, raporda belirt. Sentetik/placeholder asset üretme.

# CHATGPT BATCH PROMPTS — Act 1 Tile Generation
**Status: READY 2026-05-08**
**Kaynak promptlar:** STAGING/STYLE_REFERENCES/

---

## NASIL KULLANILIR

1. **Wall Session** önce — W1 regen → W2 → OBW. Her biri ayrı mesaj.
2. Her mesajdan önce W1 style anchor'ı ekle: `STAGING/tiles_raw/ChatGPT Image 8 May 2026 01_47_54 (3).png`
3. Çıkan görseli `STAGING/tiles_raw/` altına kaydet (isim: `w1_sheet_v2.png`, `w2_sheet.png`, `obw_sheet.png`)
4. **Floor Session** ayrı oturum — F3 regen + Transition. Aynı W1 anchor ekle.
5. Tüm dosyalar hazır olunca: `python STAGING/process_tiles.py` ile slice et (komutlar aşağıda).

---

## WALL SESSION

### Ön hazırlık
> ChatGPT'ye W1 style anchor görseli yükle (`ChatGPT Image 8 May 2026 01_47_54 (3).png`) ve şunu söyle:
> **"Match the pixel art style, palette, and stone texture of the attached reference image exactly."**

---

### PROMPT 1 — W1 Base Wall (REGEN — dikey squash düzeltme)

> **Neden regen:** Önceki versiyon 1024×1024 ile üretildi → 64×96 tile'a kırparken dikey squash oluşuyor.
> **Çıktı dosyası:** `STAGING/tiles_raw/w1_sheet_v2.png`

```
Create a pixel art isometric dungeon wall tile sprite sheet.

OUTPUT SIZE: 1024×1536 pixels, portrait orientation (taller than wide). NOT square.
GRID: 4 columns × 4 rows = 16 tiles, zero gaps, perfectly equal sizing.
Each cell is exactly 256×384 pixels in the output.

BACKGROUND: Solid pure green #00FF00 (RGB 0,255,0) outside all wall shapes. Pure flat green only. No green pixels inside tiles.

PIXEL ART SCALE: Each tile is 64×96 logical pixels. Pixel clusters minimum 4px wide — no sub-pixel detail. Image will be downscaled 4× to 64×96px tiles.

STYLE (match attached reference image exactly):
- Hard aliased edges — NO anti-aliasing, NO smooth gradients, NO 3D rendering
- Top-left light source, consistent across ALL 16 tiles
- Wall geometry: narrow top strip ~8px lit #3A3C50, tall front face #1E2030/#262838, left shadow strip 4px #12141A, mortar joints #161620 (~4px wide in output = 16px at source resolution)
- PALETTE RESTRICTION: use ONLY these colors — stone #1E2030/#262838/#363A48, top strip #3A3C50, mortar #161620, shadow #12141A, iron fixtures #181820. No other hues.
- Mortar grid visible on front face, horizontal and vertical pattern

TILES:
1). plain clean masonry, no damage — reference tile
2). single horizontal crack mid-height #0A0A14
3). vertical crack top strip to base
4). two parallel horizontal cracks, upper and lower
5). chipped upper-right corner, 3-4 debris pixels at top
6). mortar joint recessed on one horizontal line, deeper shadow
7). scorch mark: dark carbon smear on lower front face
8). iron ring mount on front face, dark metal #181820
9). moss in one horizontal mortar joint, #1A2810 thin line
10). moisture stain running vertically from top to base
11). chipped lower-left corner, stone debris at base
12). diagonal crack upper-right toward lower-left
13). two crossing cracks + chipped corner combined
14). moss in joint + moisture stain combined
15). deep crack with pale mineral deposit along one edge
16). most damaged: heavy crack + stone debris at base + surface weathering
```

**Slice komutu:**
```powershell
python STAGING/process_tiles.py --source "STAGING/tiles_raw/w1_sheet_v2.png" --output "Assets/Art/Tiles/Act1/W1" --cols 4 --rows 4 --width 64 --height 96 --prefix "w1_"
```

---

### PROMPT 2 — W2 Damaged Wall (REGEN — squash düzeltme + ember glow yok)

> **Neden regen:** Dikey squash + ember/lava glow tile vardı (Act 1 renk yasağı).
> **Çıktı dosyası:** `STAGING/tiles_raw/w2_sheet_v2.png`

```
Create a pixel art isometric dungeon heavily damaged wall tile sprite sheet.

OUTPUT SIZE: 1024×1536 pixels, portrait orientation (taller than wide). NOT square.
GRID: 4 columns × 4 rows = 16 tiles, zero gaps, perfectly equal sizing.
Each cell is exactly 256×384 pixels in the output.

BACKGROUND: Solid pure green #00FF00 (RGB 0,255,0) outside all wall shapes. Pure flat green only. No green pixels inside tiles.

PIXEL ART SCALE: Each tile is 64×96 logical pixels. Pixel clusters minimum 4px — no sub-pixel detail. This image will be downscaled 4× to 64×96px tiles; design accordingly.

STYLE (same stone as W1, heavy damage is dominant):
- Hard aliased edges — NO anti-aliasing, NO smooth gradients, NO 3D rendering
- Top-left light source, consistent across ALL 16 tiles
- Same wall geometry as W1: top strip #3A3C50, front face #1E2030/#262838, shadow #12141A, mortar #161620
- PALETTE RESTRICTION: use ONLY these colors — stone #1E2030/#262838/#363A48, mortar #161620, shadow #12141A/#080810, moss #1A2810/#263A1A. No other hues. Voids are deep blue-black #080810 only.
- NO ember, NO lava, NO warm colors of any kind — this is a cold dungeon
- Structural failure reads immediately in silhouette

TILES:
1). deep single crack full height, 2px wide gap
2). spiderweb fracture from mortar joint intersection
3). two heavy cracks crossing, X pattern
4). one stone block protruding 2px from face
5). heavy scorch mark, 40% front face coverage #0E0E0E
6). crack with 2px separation gap, dark void inside #080810
7). moss growing inside crack — organic growth in the gap
8). collapsed upper corner: top strip partially broken, debris pixels
9). two parallel vertical cracks, block segment displaced
10). lower corner fully broken off, large debris at base
11). heavy moss spreading across 50% of front face
12). deep structural gap: dark interior void visible through crack
13). multiple cracks + moss + scorch combined
14). top strip fully cracked through, stone cap pieces displaced
15). large section collapsed inward: hole shape in wall face, dark void behind
16). worst damage: structural failure, multiple voids, debris, moss — barely standing
```

**Slice komutu:**
```powershell
python STAGING/process_tiles.py --source "STAGING/tiles_raw/w2_sheet_v2.png" --output "Assets/Art/Tiles/Act1/W2" --cols 4 --rows 4 --width 64 --height 96 --prefix "w2_"
```

---

### PROMPT 3 — Outer Boundary Wall (YENİ — 64×128, map dışı void kapatma)

> **Neden lazım:** Map sınırlarında siyah void görünüyor. 2× yüksek duvar = tam kapatma.
> **Çıktı dosyası:** `STAGING/tiles_raw/obw_sheet.png`

```
Create a pixel art isometric dungeon exterior boundary wall tile sprite sheet.

OUTPUT SIZE: 1024×1536 pixels, portrait orientation (taller than wide). NOT square.
GRID: 4 columns × 3 rows = 12 tiles, zero gaps, perfectly equal sizing.
Each cell is exactly 256×512 pixels in the output.

BACKGROUND: Solid pure green #00FF00 (RGB 0,255,0) outside all shapes. Pure flat green only. No green pixels inside tiles.

PIXEL ART SCALE: Each tile is 64×128 logical pixels (twice the height of a standard wall). Pixel clusters minimum 4px — no sub-pixel detail.

STYLE (match attached W1 reference — exterior/rougher version):
- Hard aliased edges — NO anti-aliasing, NO smooth gradients, NO 3D rendering
- Top-left light source
- Same cold blue-grey dungeon stone: #1E2030/#262838
- Top strip #3A3C50. Left shadow strip 4px #12141A. Exterior face more raw/unfinished — no iron fixtures.
- Same mortar style as W1 but slightly less regular (exterior construction)
- These are structural boundary walls seen from inside the dungeon

TILES:
1). plain raw exterior stone, solid face — reference tile, no damage
2). subtle surface grain variation only
3). slight horizontal crack, shallow
4). vertical moisture stain running full height
5). rough chisel-mark surface texture
6). slightly protruding edge stone at corner
7). single mortar trace, barely visible
8). surface pitting and age weathering
9). shallow crack + moisture combined
10). rough hewn stone, irregular surface blocks
11). minimal moss trace in one surface crack
12). most weathered: combined surface variation — still structurally solid
```

**Slice komutu:**
```powershell
python STAGING/process_tiles.py --source "STAGING/tiles_raw/obw_sheet.png" --output "Assets/Art/Tiles/Act1/WB" --cols 4 --rows 3 --width 64 --height 128 --prefix "wb_"
```

---

## FLOOR SESSION

> **Ayrı ChatGPT oturumu.** Yine W1 anchor ekle: `ChatGPT Image 8 May 2026 01_47_54 (3).png`
> F2 floor anchor'ı da ekleyebilirsin: `ChatGPT Image 8 May 2026 01_47_54 (1).png`

---

### PROMPT 4 — F3 Mossy Floor (REGEN — W1 anchor ile stil tutarlılığı)

> **Not:** F3 onaylandı ama W1 style anchor olmadan üretildi. Stil tutarlılığı için regen gerekli.
> **⚠️ F3 hiçbir zaman F1/F2 ile aynı Random Tile pool'una girmez — zone isolation zorunlu.**
> **Çıktı dosyası:** `STAGING/tiles_raw/f3_sheet_v2.png`

```
Create a pixel art isometric dungeon floor tile sprite sheet.

OUTPUT SIZE: 1024×1024 square image.
GRID: 4 columns × 4 rows = 16 tiles, zero gaps, perfectly equal sizing.
Each cell is exactly 256×256 pixels in the output.

BACKGROUND: Solid pure green #00FF00 (RGB 0,255,0) outside all diamond shapes. Pure flat green only. No green pixels inside tiles, no green moss, no vegetation.

PIXEL ART SCALE: Each tile is 64×64 logical pixels. Pixel clusters minimum 4×4px — no sub-pixel detail. Image will be downscaled 4× to 64px tiles.

STYLE (match attached reference — same stone, organic growth added):
- Isometric diamond shape, 2:1 width-to-height ratio
- Hard aliased edges — NO anti-aliasing, NO smooth gradients, NO 3D rendering
- Top-left light source, consistent across ALL tiles
- PALETTE RESTRICTION: use ONLY these colors — stone #1E2030/#262838/#363A48, mortar #161620, shadow #151820, moss #1A2810/#263A1A. No other hues. Especially NO bright green, NO yellow-green.
- Moss: desaturated dark green ONLY — #1A2810 (shadow) to #263A1A (lit edge).
- Organic growth is the dominant feature, stone base identity still readable underneath

TILES:
1). base stone with moisture sheen only — wet surface, faint specular line, no moss yet
2). moss trace in mortar joints only, thin dark line #1A2810
3). small moss colony on one corner, 5-7px organic blob
4). moss spreading across two adjacent block faces
5). heavy moss patch on one full face, 40% coverage
6). moss patch at one diamond corner edge, organic shape
7). pale mineral stain ring, dried moisture circle #1E2030
8). moisture sheen on full surface, wet stone look
9). fungal growth: pale dot cluster 3-4px each, scattered on one block face
10). moss plus hairline crack — organic growth inside the crack line
11). mold bloom: dark irregular patch #141414, organic texture
12). water pooling at corner: darkened diamond edge, moisture gradient
13). stone base with two types: moisture stain ring + faint moss trace combined
14). moss covering 60% of surface, crack visible underneath
15). mold plus moisture plus faint crack: three combined subtle elements
16). most organic: heavy moss AND moisture stain AND mold bloom — richly textured surface
```

**Slice komutu:**
```powershell
python STAGING/process_tiles.py --source "STAGING/tiles_raw/f3_sheet_v2.png" --output "Assets/Art/Tiles/Act1/F3" --cols 4 --rows 4 --width 64 --height 64 --prefix "f3_"
```

---

### PROMPT 5 — Transition Tiles F1→F2 (YENİ — geçiş tile'ları)

> **Amacı:** F1 temiz taş ile F2 çatlak taş arasında keskin geçiş yerine doğal blend.
> **Çıktı dosyası:** `STAGING/tiles_raw/trans_f1f2_sheet.png`

```
Create a pixel art isometric dungeon floor transition tile sprite sheet.

OUTPUT SIZE: 1024×512 square image.
GRID: 4 columns × 2 rows = 8 tiles, zero gaps, perfectly equal sizing.
Each cell is exactly 256×256 pixels in the output.

BACKGROUND: Solid pure green #00FF00 (RGB 0,255,0) outside all diamond shapes. Pure flat green only. No green pixels inside tiles, no green moss, no vegetation.

PIXEL ART SCALE: Each tile is 64×64 logical pixels. Pixel clusters minimum 4×4px — no sub-pixel detail.

STYLE (match attached reference — blend of F1 clean and F2 cracked stone):
- Isometric diamond shape, 2:1 width-to-height ratio
- Hard aliased edges — NO anti-aliasing, NO smooth gradients
- Top-left light source, consistent across ALL tiles
- Same palette as F1/F2: stone #1E2030/#262838/#363A48, mortar #161620, shadow #151820
- These tiles bridge clean stone (F1) and cracked stone (F2)

TILES:
1). mostly clean stone, single hairline crack on one edge only — F1 side
2). clean stone with crack creeping in from one corner
3). 50/50 split: left half clean, right half has fine crack network
4). mostly cracked, one clean stone block visible in upper corner
5). clean stone base with crack along one mortar joint only
6). crack pattern radiates from one edge inward, fades before center
7). two-zone tile: top clean diamond face, bottom with mortar crack
8). mostly cracked surface, small intact patch in center — F2 side
```

**Slice komutu:**
```powershell
python STAGING/process_tiles.py --source "STAGING/tiles_raw/trans_f1f2_sheet.png" --output "Assets/Art/Tiles/Act1/Trans_F1F2" --cols 4 --rows 2 --width 64 --height 64 --prefix "tf12_"
```

---

### PROMPT 6 — Transition Tiles F2→F3 (YENİ — geçiş tile'ları)

> **Amacı:** F2 çatlak ile F3 yosunlu taş arasında organik geçiş.
> **Çıktı dosyası:** `STAGING/tiles_raw/trans_f2f3_sheet.png`

```
Create a pixel art isometric dungeon floor transition tile sprite sheet.

OUTPUT SIZE: 1024×512 square image.
GRID: 4 columns × 2 rows = 8 tiles, zero gaps, perfectly equal sizing.
Each cell is exactly 256×256 pixels in the output.

BACKGROUND: Solid pure green #00FF00 (RGB 0,255,0) outside all diamond shapes. Pure flat green only. No green pixels inside tiles, no green moss, no vegetation.

PIXEL ART SCALE: Each tile is 64×64 logical pixels. Pixel clusters minimum 4×4px — no sub-pixel detail.

STYLE (blend of F2 cracked and F3 mossy stone):
- Isometric diamond shape, 2:1 width-to-height ratio
- Hard aliased edges — NO anti-aliasing, NO smooth gradients
- Top-left light source, consistent across ALL tiles
- Palette: stone #1E2030/#262838/#363A48, mortar #161620, shadow #151820, moss #1A2810/#263A1A
- Cracks AND moss both present, varying ratio

TILES:
1). cracked stone, thin moss trace in one crack only — F2 side
2). two cracks, moss starting to fill one crack
3). 50/50: crack network on left, moss patch on right
4). cracks plus small moss colony at one corner
5). heavy crack with moss growing inside the gap
6). crack network, moss spreading from crack into stone surface
7). mostly mossy surface, visible crack underneath
8). minimal crack, dominant moss coverage — F3 side
```

**Slice komutu:**
```powershell
python STAGING/process_tiles.py --source "STAGING/tiles_raw/trans_f2f3_sheet.png" --output "Assets/Art/Tiles/Act1/Trans_F2F3" --cols 4 --rows 2 --width 64 --height 64 --prefix "tf23_"
```

---

## TÜM SLICE KOMUTLARI — TEK BLOK

Tüm dosyalar hazır olunca sırayla çalıştır:

```powershell
cd "F:\Antigravity Projeler\2d roguelite\RIMA"

# Wall batch
python STAGING/process_tiles.py --source "STAGING/tiles_raw/w1_sheet_v2.png"    --output "Assets/Art/Tiles/Act1/W1"           --cols 4 --rows 4 --width 64 --height 96  --prefix "w1_"
python STAGING/process_tiles.py --source "STAGING/tiles_raw/w2_sheet_v2.png"    --output "Assets/Art/Tiles/Act1/W2"           --cols 4 --rows 4 --width 64 --height 96  --prefix "w2_"
python STAGING/process_tiles.py --source "STAGING/tiles_raw/obw_sheet.png"      --output "Assets/Art/Tiles/Act1/WB"           --cols 4 --rows 3 --width 64 --height 128 --prefix "wb_"

# Floor batch
python STAGING/process_tiles.py --source "STAGING/tiles_raw/f3_sheet_v2.png"    --output "Assets/Art/Tiles/Act1/F3"           --cols 4 --rows 4 --width 64 --height 64  --prefix "f3_"
python STAGING/process_tiles.py --source "STAGING/tiles_raw/trans_f1f2_sheet.png" --output "Assets/Art/Tiles/Act1/Trans_F1F2" --cols 4 --rows 2 --width 64 --height 64  --prefix "tf12_"
python STAGING/process_tiles.py --source "STAGING/tiles_raw/trans_f2f3_sheet.png" --output "Assets/Art/Tiles/Act1/Trans_F2F3" --cols 4 --rows 2 --width 64 --height 64  --prefix "tf23_"
```

---

## UNITY IMPORT SONRASI (batch_tiles.ps1 alternatifi)

Her yeni tile klasörü için Unity'de:
- Sprite Mode: Multiple, Pixels Per Unit: 64
- Filter Mode: Point (no filter)
- Compression: None
- Pivot: Top Center

Random Tile pool atama kuralı:
- F1 pool: F1 tile'ları
- F2 pool: F2 tile'ları. F2 tile 13-16 → prop/overlay layer (ayrı pool dahil etme)
- F3 pool: F3 tile'ları — **F1/F2 ile ASLA aynı pool'a koyma**
- W2 tile 16 (en hasarlı) → boss odası için ayrı tut

# ChatGPT Floor Tile Prompt (LOCKED 2026-05-08)

## Karar
ChatGPT (GPT-4o image gen) isometrik dungeon floor tile üretiminde PixelLab'dan daha iyi
tutarlılık sağladı. Bu prompt F1 base floor için LOCKED. Duvar/obje tile'ları bir sonraki
sessionda aynı şablonla üretilecek.

## Unity Entegrasyon Çözümü
- Tile sprite pivot: top-center (diamond üst köşesi)
- Y-sorting: aktif (RIMA zaten kullanıyor)
- Side face overlap: Y-sort ile otomatik çözülür, ek işlem gerekmez
- ChromaKey kaldırma: aşağıdaki Python script ile

## Chromakey + Kesme Script (Python)
```python
from PIL import Image
import numpy as np

img = Image.open("chatgpt_tiles.png").convert("RGBA")
data = np.array(img)

# Pure green #00FF00 chromakey (LOCKED 2026-05-09 — magenta deprecated)
green_mask = (data[:,:,1] > 200) & (data[:,:,0] < 75) & (data[:,:,2] < 75)
data[green_mask] = [0, 0, 0, 0]

result = Image.fromarray(data)

# Grid'e göre tile'lara kes — COLS ve ROWS'u ayarla
COLS, ROWS = 4, 4
W, H = result.size
tw, th = W // COLS, H // ROWS

for row in range(ROWS):
    for col in range(COLS):
        i = row * COLS + col
        box = (col*tw, row*th, (col+1)*tw, (row+1)*th)
        tile = result.crop(box).resize((64, 64), Image.NEAREST)
        tile.save(f"tile_{i:02d}.png")

print(f"{COLS*ROWS} tile kaydedildi.")
```

## Şablon Prompt (değişken kısımlar köşeli parantez içinde)

### Sabit Kısım (her batch aynı):
```
Create a pixel art isometric dungeon floor tile sprite sheet.

LAYOUT: [N] tiles in a [COLS] columns × [ROWS] rows grid, zero gaps between tiles, perfectly equal sizing.

BACKGROUND: Solid pure green #00FF00 (RGB 0,255,0) everywhere outside the diamond shapes. No transparency, no feathering, no anti-aliasing at edges — pure flat green only. No green pixels inside tiles, no green moss, no vegetation.

TILE STYLE (apply identically to ALL tiles):
- Isometric diamond/rhombus shape, 2:1 width-to-height ratio
- Dark medieval dungeon stone floor, deep blue-grey tone
- Hard pixel edges, visible pixel grain, zero smooth gradients, zero anti-aliasing
- Consistent top-left light source: top face slightly lighter, bottom-left and bottom-right edges have thin dark shadow strip
- No tile surface brighter than mid-grey, no tile surface darker than #151820 (near-black forbidden)
- All tiles share identical stone color, lighting direction, pixel art style, and diamond proportions
- Surface details are subtle — stone identity reads first, detail second

TILES:
[TILE LIST]
```

## F1 Base Floor — 16 Tile (DONE 2026-05-08)

Çıktı: `C:\Users\ydbil\Downloads\ChatGPT Image 8 May 2026 00_05_34.png`
Kullanılabilir: 14/16 (tile 4 daire dominant, tile 13 boot hollow belirsiz — kabul edilebilir)

Tam prompt kullanıldı:
```
Create a pixel art isometric dungeon floor tile sprite sheet.

LAYOUT: 16 tiles in a 4 columns × 4 rows grid, zero gaps between tiles, perfectly equal sizing.

BACKGROUND: Solid pure green #00FF00 (RGB 0,255,0) everywhere outside the diamond shapes. No transparency, no feathering, no anti-aliasing at edges — pure flat green only. No green pixels inside tiles, no green moss, no vegetation.

TILE STYLE (apply identically to ALL 16 tiles):
- Isometric diamond/rhombus shape, 2:1 width-to-height ratio
- Dark medieval dungeon stone floor, deep blue-grey tone
- Hard pixel edges, visible pixel grain, zero smooth gradients, zero anti-aliasing
- Consistent top-left light source: top face slightly lighter, bottom-left and bottom-right edges have thin dark shadow strip
- No tile surface brighter than mid-grey, no tile surface darker than #151820 (near-black forbidden)
- All 16 tiles share identical stone color, lighting direction, pixel art style, and diamond proportions
- Surface details are subtle — stone identity reads first, detail second

TILES:
1). plain worn stone, smooth from foot traffic
2). single diagonal hairline crack corner to corner
3). spiderweb fracture radiating from center
4). dark moss patch in one corner, desaturated
5). dried circular water stain, faint mineral ring
6). faint carved rune, worn smooth, barely visible
7). chipped corner, small debris pixels at edge
8). scorch mark, dark carbon smear
9). two parallel cracks running across surface
10). heavy moss spread across half the surface
11). pale mineral crystal vein, thin streak
12). dark mold bloom, irregular organic patch
13). boot-worn hollow, slightly depressed center
14). deep fracture with slight gap, displaced edge
15). moisture sheen on surface, wet stone look
16). multiple micro-cracks across two areas
```

## Codex Pipeline Entegrasyonu

Her ChatGPT batch için Codex şu komutu çalıştırır:
```
python STAGING/process_tiles.py --source "<CHATGPT_IMAGE_PATH>" --output "Assets/Art/Tiles/Act1/<BATCH>" --cols <C> --rows <R> --width <W> --height <H> --prefix "<prefix_>"
```

`process_tiles.py` evrensel script — Codex tarafından STAGING/ klasörüne yazılır (F1 import görevinde).

---

## F2 — Cracked Floor (16 Tile, HAZIR PASTE)

**Codex Komutu (image hazır olunca):**
```
python STAGING/process_tiles.py --source "<CHATGPT_F2_IMAGE>" --output "Assets/Art/Tiles/Act1/F2" --cols 4 --rows 4 --width 64 --height 64 --prefix "f2_"
```

**ChatGPT Prompt:**
```
Create a pixel art isometric dungeon floor tile sprite sheet.

LAYOUT: 16 tiles in a 4 columns × 4 rows grid, zero gaps between tiles, perfectly equal sizing.

BACKGROUND: Solid pure green #00FF00 (RGB 0,255,0) everywhere outside the diamond shapes. No transparency, no feathering, no anti-aliasing at edges — pure flat green only. No green pixels inside tiles, no green moss, no vegetation.

TILE STYLE (apply identically to ALL tiles):
- Isometric diamond/rhombus shape, 2:1 width-to-height ratio
- Dark medieval dungeon stone floor, deep blue-grey tone — SAME stone as F1 base floor
- Hard pixel edges, visible pixel grain, zero smooth gradients, zero anti-aliasing
- Consistent top-left light source: top face slightly lighter, bottom edges thin dark strip
- No tile surface brighter than mid-grey, no tile surface darker than #151820
- All tiles share identical stone color, lighting direction, pixel art style, and diamond proportions
- Cracks are the DOMINANT feature — stone identity reads first, then damage detail

TILES:
1). deep diagonal crack corner to corner, crack #0a0a14 2px wide
2). spiderweb fracture radiating from center, 4-5 crack branches #0a0a14
3). two parallel longitudinal cracks, one block slightly displaced
4). crack runs through mortar joint and continues across adjacent block
5). heavy fracture, 2px gap, edge of block offset 1px suggesting collapse
6). crack with faint white mineral deposit along edge #1e2030
7). multiple micro-cracks forming a network across two block faces
8). corner rubble: 4-6 loose debris pixels #14141e at diamond edge
9). single deep crack, dark shadow inside #080810, runs half tile
10). mortar joint fully cracked out, raw block edge exposed
11). diagonal crack plus one small debris cluster at terminus
12). heavy crack with block inset 1px, depression in surface
13). crack pattern radiates from corner across full block face
14). two crossing cracks, X pattern, both cracks #0a0a14
15). crack with dark green mineral seepage trace #1a2810 along one side
16). worst damage: heavy crack network, 4+ cracks, debris at two corners
```

---

## F3 — Mossy Floor (12 Tile, HAZIR PASTE)

**Codex Komutu:**
```
python STAGING/process_tiles.py --source "<CHATGPT_F3_IMAGE>" --output "Assets/Art/Tiles/Act1/F3" --cols 4 --rows 3 --width 64 --height 64 --prefix "f3_"
```

**ChatGPT Prompt:**
```
Create a pixel art isometric dungeon floor tile sprite sheet.

LAYOUT: 12 tiles in a 4 columns × 3 rows grid, zero gaps between tiles, perfectly equal sizing.

BACKGROUND: Solid pure green #00FF00 (RGB 0,255,0) everywhere outside the diamond shapes. No transparency, no feathering, no anti-aliasing at edges — pure flat green only. No green pixels inside tiles, no green moss, no vegetation.

TILE STYLE (apply identically to ALL tiles):
- Isometric diamond/rhombus shape, 2:1 width-to-height ratio
- Dark medieval dungeon stone floor, deep blue-grey tone — SAME stone as F1 base floor
- Hard pixel edges, visible pixel grain, zero smooth gradients, zero anti-aliasing
- Consistent top-left light source
- Moss and organic growth are the DOMINANT feature, always desaturated dark green tones
- Moss color range: #1a2810 (deep shadow) to #263a1a (lit edge) — never bright green

TILES:
1). dark moss colony covering 20% of one block face, irregular pixel blob 5-7px
2). moss in mortar joints only, thin dark line #1a2810 tracing joint pattern
3). moss on two adjacent block faces, spreading pattern
4). heavy moss on one full block face, 40% coverage with #263a1a edge accent
5). moss patch at one diamond corner edge, organic blob shape
6). pale mineral stain ring, dried moisture #1e2030 faint circle
7). moisture sheen on full surface, wet stone — subtle specular line #2a2c40
8). fungal growth: pale dots cluster #1e1e2e, 3-4px each, scattered on one block
9). moss plus hairline crack, organic growth in crack
10). water pooling corner: darkened diamond edge, moisture gradient
11). mold bloom: dark irregular patch #141414 with faint organic texture
12). most organic: moss AND stain AND moisture, surface richly textured
```

---

## W1 — Base Wall (12 Tile, HAZIR PASTE)

**Canvas:** 64×96 per tile (wall is taller than floor)
**Grid:** 4 cols × 3 rows = 12 tiles → sheet ~256×288

**Codex Komutu:**
```
python STAGING/process_tiles.py --source "<CHATGPT_W1_IMAGE>" --output "Assets/Art/Tiles/Act1/W1" --cols 4 --rows 3 --width 64 --height 96 --prefix "w1_"
```

**Unity pivot:** top-center (same as floors) — Y-sort handles overlap

**ChatGPT Prompt:**
```
Create a pixel art isometric dungeon wall tile sprite sheet.

LAYOUT: 12 tiles in a 4 columns × 3 rows grid, zero gaps, perfectly equal sizing. Each tile is TALLER than it is wide (portrait rectangle, not diamond).

BACKGROUND: Solid pure green #00FF00 (RGB 0,255,0) everywhere outside the wall shapes. No transparency, no feathering — pure flat green only. No green pixels inside tiles.

WALL SHAPE AND GEOMETRY:
- Each tile shows the FRONT FACE of an isometric stone wall block
- The wall block has: a narrow TOP surface (thin strip across the top, ~8px), and a tall FRONT FACE (main visible area below the top strip)
- Top strip: lighter stone #3a3c50 — lit from top-left
- Front face: dark medieval dungeon stone, same palette as floor #1e2030 and #262838, mortar joints #161620 1px
- Left edge shadow strip: 4px wide, #12141a
- Right edge: same stone as front, slightly darker than center
- Bottom edge: #12141a 1px base shadow

TILE STYLE:
- Hard pixel edges, pixel art style, zero anti-aliasing, zero smooth gradients
- Consistent top-left light source across ALL tiles
- Same stone identity as floor (F1): blue-grey dungeon stone, cold muted palette
- Mortar joints visible on front face, horizontal and vertical grid pattern
- Stone blocks 2×2 visible on front face (same masonry as floor viewed from side)

TILES:
1). plain stone wall, clean masonry, no damage
2). single horizontal crack on front face mid-height #0a0a14
3). vertical crack runs from top strip down to base
4). two parallel horizontal cracks, one upper one lower
5). chipped upper-right corner, 3-4 debris pixels at top
6). mortar joint deeper than usual on one horizontal line, recessed shadow
7). scorch mark: dark carbon smear #0e0e0e on lower front face
8). single iron bracket/ring mounted on front face, dark metal #181820
9). moss growing in horizontal mortar joint, #1a2810 thin line
10). moisture stain running vertically from top to base, mineral streak
11). chipped lower-left corner, stone debris at base 3-5 pixels
12). diagonal crack from upper-right toward lower-left across front face
```

---

## W2 — Damaged Wall (8 Tile, HAZIR PASTE)

**Codex Komutu:**
```
python STAGING/process_tiles.py --source "<CHATGPT_W2_IMAGE>" --output "Assets/Art/Tiles/Act1/W2" --cols 4 --rows 2 --width 64 --height 96 --prefix "w2_"
```

**ChatGPT Prompt:**
```
Create a pixel art isometric dungeon wall tile sprite sheet.

LAYOUT: 8 tiles in a 4 columns × 2 rows grid, zero gaps, perfectly equal sizing. Each tile is portrait rectangle (taller than wide).

BACKGROUND: Solid pure green #00FF00 (RGB 0,255,0) everywhere outside the wall shapes. No transparency, no feathering — pure flat green only. No green pixels inside tiles.

WALL SHAPE: Same isometric wall block as W1 — top strip #3a3c50, front face #1e2030/#262838, left shadow strip #12141a, mortar joints #161620.

TILE STYLE: Same as W1 but HEAVY DAMAGE is the dominant feature.

TILES:
1). deep crack network: 3+ cracks crossing on front face #0a0a14
2). collapsed upper corner: top strip edge crumbled, 5-7 debris pixels
3). spiderweb fracture radiating from mortar joint intersection
4). partial block displacement: one stone block protrudes 2px from face
5). heavy scorch mark covering 40% of front face, smoke stain #0e0e0e
6). crack with gap: 2px separation, dark void inside #080810
7). moss plus crack combination: organic growth IN the crack line
8). worst damage: multiple deep cracks, debris at base, top strip partially broken
```

---

## O-series Sonraki Session
- Props ayrı prompt (transparent bg, individual sprites, no grid chromakey)
- O1: Brazier/torch, O2: Barrel cluster, O3: Crates, O6: Large altar, O7: Bones/debris

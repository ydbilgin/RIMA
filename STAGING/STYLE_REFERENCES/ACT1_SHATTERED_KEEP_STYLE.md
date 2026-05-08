# ACT 1 — Shattered Keep — LOCKED Style Spec
**Status: LOCKED 2026-05-08**
**Reviewed by: Claude (orchestrator) + rima-qc + Gemini 3.1 Pro High**

---

## Stil Kimliği

Soğuk, muted, ciddi medieval dungeon. Pixel art — gerçek indexed-style, sert kenarlar, sınırlı palet. NOT 3D render, NOT smooth gradient, NOT bright renk.

**Atmosfer:** Parçalanmış ama hâlâ yapısal. "Ayna parçaları gibi keskin; soğuk ama parlak."

---

## Palet (Kilit Değerler)

| Yüzey | HEX | Kullanım |
|---|---|---|
| Zemin taşı — koyu yüz | `#1E2030` | F1/F2/F3 ana yüzey (koyu) |
| Zemin taşı — orta yüz | `#262838` | F1/F2/F3 ana yüzey (orta) |
| Zemin taşı — aydınlık | `#363A48` | Üst-sol ışık vurgusu |
| Zemin taşı — gölge kenar | `#151820` | Alt/gölge kenarı |
| Derz (mortar) | `#161620` | Zemin ve duvar taş arası |
| Duvar ön yüz | `#1E2030` / `#262838` | W1/W2 ana yüzey |
| Duvar üst şerit | `#3A3C50` | Üst kapak (top strip) |
| Duvar sol kenar gölge | `#12141A` | 4px sol kenar |
| Duvar taban gölge | `#12141A` | 1px taban |
| Yosun koyu | `#1A2810` | F3 yosun gölge |
| Yosun aydınlık | `#263A1A` | F3 yosun ışık kenar |
| Arka plan (chroma key) | `#FF00FF` | Tüm üretimde zorunlu |

**Yasak:** Parlak yeşil, sıcak turuncu/kırmızı (Act 1'de), beyaza yakın specular, smooth gradient.

---

## Teknik Şartname

```
Tile geometry — Floor: isometric diamond, 2:1 width-to-height ratio
Tile geometry — Wall: portrait rectangle (taller than wide), top strip ~8px
Pixel art: hard pixel edges, NO anti-aliasing, NO smooth gradients, limited color palette
Light source: top-left, consistent across ALL tiles and batches
Background: solid magenta #FF00FF outside all shapes
Output (floor tiles): 1024×1024 — grid must divide evenly (use 4×4 for 16 tiles, 4×2 for 8 tiles)
Output (wall tiles 64×96): 1024×1536 portrait — 4×4 grid → 256×384 cells → 4× exact
Output (tall wall tiles 64×128): 1024×1536 portrait — 4×3 grid → 256×512 cells → 4× exact
RULE: 1024÷rows and 1536÷rows must be integers. Never use 4×3 with 1024×1024 (341.33 non-integer).
Unity pivot: top-center (floor + wall)
Unity Y-sort: ON
```

---

## Style Anchor Görseller

ChatGPT ile yeni batch üretirken bu görselleri "style reference" olarak ekle:

| Anchor | Dosya | Ne için anchor |
|---|---|---|
| **PRIMARY** | `STAGING/tiles_raw/ChatGPT Image 8 May 2026 01_47_54 (3).png` | W1 — en güçlü pixel art örneği |
| Floor anchor | `STAGING/tiles_raw/ChatGPT Image 8 May 2026 01_47_54 (1).png` | F2 — zemin tile referansı |
| F1 individual | `Assets/Art/Tiles/Act1/F1/f1_00.png` | En temiz base stone |
| F1 individual | `Assets/Art/Tiles/Act1/F1/f1_08.png` | Hafif detaylı base stone |

**ChatGPT'ye söylenecek:**
> "Match the pixel art style, palette, and stone texture of the attached reference images exactly."

---

## Batch Durumu

| Batch | Dosya | Durum | Notlar |
|---|---|---|---|
| F1 base floor | `Assets/Art/Tiles/Act1/F1/` | ✅ DONE | 16 tile, production |
| F2 cracked floor | `ChatGPT...(1).png` | ✅ APPROVED | Row 4 tile'ları → prop |
| F3 mossy floor | `ChatGPT...(2).png` | ✅ APPROVED | Zone isolation zorunlu; derz kontrastı -10% önerildi |
| W1 base wall | `ChatGPT...(3).png` | ⚠️ SQUASH — YENIDEN ÜRET | 1024×1024 ile üretildi, dikey squash var. Prompt: PROMPT_W1_BASE_WALL.md |
| W2 damaged wall | `ChatGPT...(4).png` | ⚠️ SQUASH — YENIDEN ÜRET | 1024×1024 ile üretildi, dikey squash var. Ember glow tile exclude. Prompt: PROMPT_W2_DAMAGED_WALL.md |
| F1-damaged (Guard band) | — | ⏳ YAPILMADI | — |
| F4-rift (Ritual band) | — | ⏳ YAPILMADI | — |
| Transition tiles | — | ⏳ YAPILMADI | F1→F2, F2→F3 arası hybrid |
| Outer boundary wall | — | ⏳ YAPILMADI | Prompt: bu dosyada aşağıda |
| W-top cap | — | ⏳ YAPILMADI | Duvar üst yüzü top-down view |

---

## ChatGPT Prompt Şablonu

### Zemin tile'ları (floor — 64×64 hedef)
```
OUTPUT SIZE: 1024×1024 square
GRID: 4 columns × [N] rows = [TOTAL] tiles, zero gaps, perfectly equal sizing.
Each cell is 256×256 pixels in the output.

BACKGROUND: Solid magenta #FF00FF outside all shapes. No transparency.

PIXEL ART SCALE: Each tile is 64×64 logical pixels.
Pixel clusters minimum 4×4px — no detail finer than 4px, no sub-pixel features.
Hard aliased edges only. NO anti-aliasing, NO smooth gradients, NO 3D rendering.

STYLE (match attached reference images exactly):
Top-left light source. Cold blue-grey dungeon stone.
Main face: #1E2030–#262838. Lit edge: #363A48. Mortar: #161620.
NO specular highlights, NO smooth shading.

[TILE LIST]
```

### Duvar tile'ları (wall — 64×96 hedef) — FARKLI BOYUT
```
OUTPUT SIZE: 1024×1536 pixels, portrait orientation (taller than wide).
GRID: 4 columns × 4 rows = 16 tiles, zero gaps, perfectly equal sizing.
Each cell is exactly 256×384 pixels in the output.

BACKGROUND: Solid magenta #FF00FF outside all wall shapes. No transparency.

PIXEL ART SCALE: Each tile is 64×96 logical pixels.
Pixel clusters minimum 4px — no sub-pixel detail.
Hard aliased edges only. NO anti-aliasing, NO smooth gradients, NO 3D rendering.

STYLE (match attached W1 reference image exactly):
Top-left light source. Cold blue-grey dungeon stone.
Top strip: #3A3C50. Front face: #1E2030/#262838. Left shadow: #12141A 4px. Mortar: #161620.

[TILE LIST — 16 tiles]
```

### Slice komutları
```powershell
# Floor (64×64):
python STAGING/process_tiles.py --source "<file>" --output "Assets/Art/Tiles/Act1/<BATCH>" --cols 4 --rows <N> --width 64 --height 64 --prefix "<prefix>_"

# Wall (64×96) — 4×4 grid zorunlu:
python STAGING/process_tiles.py --source "<file>" --output "Assets/Art/Tiles/Act1/<BATCH>" --cols 4 --rows 4 --width 64 --height 96 --prefix "<prefix>_"
```

---

## Outer Boundary Wall Prompt (Map dışı siyah önleme)

**Canvas:** 64×128 per tile (2× yüksek — map sınırında tam dolduracak)
**Grid:** 4 cols × 2 rows = 8 tile

```
Create a pixel art isometric dungeon outer boundary wall tile sprite sheet.

LAYOUT: 8 tiles in a 4 columns × 2 rows grid, zero gaps, equal sizing. Each tile is a tall portrait rectangle — twice the height of a standard wall tile.

BACKGROUND: Solid magenta #FF00FF outside all shapes.

STYLE (match attached reference — W1 sheet):
- Same cold blue-grey stone as interior walls: #1E2030/#262838, mortar #161620
- Hard pixel edges, no smooth gradients, no anti-aliasing
- Top-left light source
- These are EXTERIOR walls — slightly rougher surface than interior, no iron fixtures

GEOMETRY:
- Same isometric wall face as interior walls (top strip #3A3C50, front face, left shadow strip #12141A)
- Taller than standard walls — fills 2× vertical space
- Exterior rough stone texture on front face — same stone but slightly more raw/unfinished than W1
- No decorative details (no rings, no keyholes) — these are structural boundary walls

TILES:
1). plain exterior stone wall, clean raw face, minimal mortar lines
2). slight weathering, surface texture variation only
3). shallow horizontal crack, exterior damage
4). light moisture stain running vertically
5). rough unfinished stone surface, visible chisel marks
6). corner section with slightly protruding edge stone
7). moss trace in one mortar line only
8). most weathered: multiple surface variations combined
```

---

## Wall Occlusion (Hades stili saydamlaşma)

`WallOcclusionFader.cs` → **HAZIR, kod değişikliği yok.**

Unity'de yapılacak tek şey:
1. Wall tilemap GameObject'ine `WallOcclusionFader` component'ini ekle
2. Inspector'da `Target Tag = "Player"` (default)
3. `Fade Radius = 2.2`, `Minimum Alpha = 0.38`, `Fade Speed = 10`
4. Wall tilemap'in tüm tile'larının `TileFlags = None` olduğundan emin ol (script bunu kendisi ayarlıyor)

---

## Üretim Kuralları (Locked)

1. F3 ve W2 hiçbir zaman F1/F2 ile aynı Random Tile pool'una girmez
2. F2 row 4 (tile 13-16) → prop/overlay olarak ayrı layer'da spawn
3. W2 ember glow tile → manuel exclude, boss odası için sakla
4. Her yeni batch'te W1 ChatGPT görseli style anchor olarak ChatGPT'ye eklenir
5. Codex $imagegen tile için kullanılmaz (smooth 3D render üretiyor)

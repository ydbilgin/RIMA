# Codex Image_Gen Dispatch — Path C Floor + Wall Base (S95)

**Status:** READY FOR NEXT SESSION DISPATCH (not fired yet, user switching Claude account)

**Karar (Codex 2 verdict locked):** Path C Hybrid — Codex painted floor/wall base + PixelLab object overlay + user Map Designer template authoring.

## Variety Strategy LOCKED

**Floor:** 4 material × 4 variant = **16 floor chunks** total, 512×512 each
- Her material için ayrı dispatch (4 grid-prompt)
- Her dispatch 2×2 grid of 4 variants = 1024×1024 output image
- Unity'de slice (Multiple sprite mode, GridByCellCount 2×2)
- Weighted random Tilemap paint with variety

**Wall:** ~6 unified wall piece, 512×512 each
- 1 grid-prompt: 2×3 grid of 6 wall pieces = 1024×1536 output
- Unity'de slice
- Manual placement / Map Designer brush

**Toplam:** 5 Codex image_gen dispatch (~$10-20 cost)

## 5 Prompt Hazır

### Prompt 1 — Floor Material A: Cool Granite (4 variants)

```
2x2 grid of 4 painted pixel art floor texture variants for a roguelite dungeon.
Each tile: 512x512 pixels, total image 1024x1024.
Subject: cool weathered granite stone floor texture for fake-isometric 35° top-down view.
Style: hand-painted dark fantasy roguelite, polished pixel art with painterly cohesion, 
Hades / Dead Cells / Diablo 2 underground mood.
Palette: cool gray-blue granite RGB(58, 61, 66) base, darker shadow recesses RGB(37, 40, 48), 
hint of moss in cracks, subtle warm dust.
Each variant differs in: crack density, moss spread, dust accumulation, edge wear.
Requirements:
- Tile-like stone but NO visible borders, NO black grid lines, NO frame edges between cells
- Hand-painted blended texture, continuous ground surface
- Gameplay-readable: not too noisy, not too flat
- No walls, no props, no characters, no text, no UI elements
- Each tile must seamlessly continue across grid divisions
Output: PNG transparent or solid color background, 1024x1024.
```

### Prompt 2 — Floor Material B: Cracked Stone (4 variants)

```
2x2 grid of 4 painted pixel art floor texture variants for a roguelite dungeon.
Each tile: 512x512 pixels, total image 1024x1024.
Subject: cracked broken stone floor for fake-isometric 35° top-down view, weathered ruined keep.
Style: hand-painted dark fantasy roguelite, polished pixel art painterly cohesion.
Palette: dark charcoal stone RGB(44, 42, 42) base with bright cyan rift hairline cracks RGB(0, 255, 204), 
darker shadow recesses, dust in cracks.
Each variant differs in: rift crack pattern density, breakage severity, dust amount, debris chunks.
Requirements:
- Heavily fractured stone with magical cyan veins
- NO visible tile borders, NO grid lines between cells
- Continuous ground, hand-painted
- Gameplay-readable, not too busy
- No walls, no props, no characters, no UI
Output: PNG, 1024x1024.
```

### Prompt 3 — Floor Material C: Dirt and Rubble (4 variants)

```
2x2 grid of 4 painted pixel art floor texture variants for a roguelite dungeon.
Each tile: 512x512 pixels, total image 1024x1024.
Subject: dirt covered stone floor with rubble debris for fake-isometric 35° top-down view.
Style: hand-painted dark fantasy roguelite, polished pixel art painterly cohesion.
Palette: warm dark brown dirt RGB(74, 60, 42) blending into gray granite, 
small rubble chunks scattered, mossy patches RGB(54, 75, 46).
Each variant differs in: dirt density, rubble chunk size, moss spread, exposed stone amount.
Requirements:
- Mixed dirt and broken stone surface
- NO visible tile borders, NO grid lines
- Continuous ground, hand-painted
- Gameplay-readable
- No walls, no props, no characters, no UI
Output: PNG, 1024x1024.
```

### Prompt 4 — Floor Material D: Ritual Accent (4 variants)

```
2x2 grid of 4 painted pixel art floor texture variants for a roguelite dungeon ritual zone.
Each tile: 512x512 pixels, total image 1024x1024.
Subject: ancient ritual stone floor with cyan rift sigils for fake-isometric 35° top-down view.
Style: hand-painted dark fantasy roguelite, polished pixel art painterly cohesion.
Palette: cool granite RGB(58, 61, 66) base with glowing cyan rift sigils RGB(0, 255, 204) 
carved into surface, faded blood stains, ash patches.
Each variant differs in: sigil pattern, blood stain location, ash spread, glow intensity.
Requirements:
- Stone with magical engraved cyan glyphs
- NO visible tile borders, NO grid lines
- Continuous ground, hand-painted
- Hero focal feel but still walkable
- No walls, no props, no characters, no UI
Output: PNG, 1024x1024.
```

### Prompt 5 — Wall Pieces Set (6 unified walls)

```
2x3 grid of 6 painted pixel art wall section sprites for a roguelite dungeon.
Each piece: 512x512 pixels, total image 1024x1536.
Subject: ancient ruined keep wall sections, fake-isometric 35° depth view with visible top cap and front face.
Style: hand-painted dark fantasy roguelite, polished pixel art painterly cohesion, 
matches floor textures palette.
Palette: cool gray-blue granite RGB(58, 61, 66) stacked stone blocks with darker shadow recesses, 
hint of moss in seams, warm vignette baked into shading.

6 wall pieces (numbered position in grid, top-left = 1, reading right and down):
1) Straight horizontal wall section, contiguous stacked granite blocks, 6 blocks wide and 2 rows tall, 
   tileable left-right edges, top cap visible, front face shaded, base shadow gradient.
2) Wall corner inside L-shape NE, two-direction connection meeting at right-back corner, 
   stacked granite blocks, fake-iso depth.
3) Wall corner inside L-shape NW, mirror of NE for left-back corner, 
   stacked granite blocks, fake-iso depth.
4) Wall archway opening, granite arch frame with open passage in middle, 
   structural masonry only no decorations, top arch curve visible.
5) Wall section with integrated cyan rift fracture, granite stacked blocks with magical cyan #00FFCC 
   vertical rift crack glowing along surface, hero accent.
6) Wall door opening, granite frame with closed wooden door (rusted iron bands), 
   no character, suggestion of next room beyond.

Requirements:
- Each wall sits on transparent background, isolated, NO floor visible behind walls
- Hand-painted cohesive style matching floor textures
- Sharp readable silhouettes, top cap + front face shading defines depth
- Decoration like banners/candles NOT baked into wall (these come as separate sprite overlays)
- Each piece self-contained, can be placed adjacent without visual seam
Output: PNG transparent background, 1024x1536.
```

## Workflow Sonraki Session

1. **Codex image_gen dispatch** (yeni Claude hesabı):
   - 5 prompt yukarıdan
   - Background dispatch (each ~30-60s)
   - Sonuç PNG'leri `STAGING/codex_floor_walls_v01/` altına indir

2. **Unity slice + import:**
   - 4 floor PNG'i Multiple mode + GridByCellCount 2×2 = 16 sliced sprites
   - 1 wall PNG'i Multiple mode + GridByCellCount 2×3 = 6 sliced sprites
   - Hepsini `Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/painted_v03/` ve `walls/painted_v01/` altına yerleştir
   - PPU=64 (8 unit per 512px tile)
   - Pivot=Center (floor), BottomCenter (walls)

3. **Tile asset rebuild:**
   - 16 floor sliced sprite → 16 Tile.asset
   - Tilemap'e weighted random paint (Stone 50%, Cracked 25%, Dirt 20%, Ritual 5%)

4. **Wall placement:**
   - 6 wall sprite manual perimeter compose (4-8 wall instance toplam, repeating allowed)
   - Top edge: 1×straight repeated + 1× corner sol + 1× arch ortada + 1× corner sağ
   - Bottom edge: aynı pattern
   - Side edges: 90° rotated straight veya başka asset

5. **Mevcut 119 PNG envanter overlay:**
   - Statue, pillar, brazier, decoration vs sprite olarak yerleştir
   - Sparse, logical, focal point composition

6. **Sahne screenshot vs v4 4-gate compare:**
   - Tile border görünmüyor mu?
   - Wall stamp-repeat hissi yok mu?
   - Center focal point net mi?
   - Character readable mi?

7. **Sonuç PROOF olursa** → Production: 20-30 sub-room template author via Map Designer

## Cost Estimate

- 5 Codex image_gen dispatch × $1-3 = $5-15 total
- gpt-image-1 backend ile painted result
- Eğer kalite zayıf çıkarsa: re-dispatch with prompt tune

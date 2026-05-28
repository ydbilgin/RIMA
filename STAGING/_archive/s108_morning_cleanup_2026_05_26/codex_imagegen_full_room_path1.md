# Codex Task — Path 1 — Full Room Asset Pack + Composition

## Hedef

Bir sample dungeon room render etmek için **tüm asset'leri Codex imagegen skill ile üretmek** ve sonra **tek bir Python script ile compose ederek** 1 PNG room render üretmek. Output, Yol 1 sonuç olarak kullanılacak.

**KRITIK:** Pillow ile geometric shapes ÜRETME. Imagegen skill ile gerçek **görsel art** üret. Compose adımında Pillow OK (sadece tile yerleştirme için).

## Üretilecek Asset Pack (imagegen skill çağrıları)

Tüm tile/wall/decal/accent için **modern pixel art tradition**: Alabaster Dawn / CrossCode / Hyper Light Drifter family hedefi.

**Style invariants (her asset için):**
- Top-down view 30-35° angle
- Hard pixel edges discipline (anti-aliasing minimum)
- Max 3 tone per color region
- Vivid Vulnerability palette: dark slate gray, deep brown, dusty blue, faint dark red rift, deep moss green
- Native pixel art 128px (downsample sonra Pillow ile 64'a)
- Atmosphere: ancient ritual temple, weathered, hollow watchful

### 6 Floor Tiles (128px each)
1. Clean weathered stone, dark slate gray base, deep brown undertone
2. Stone with sparse moss patch, deep moss green spot
3. Cracked stone, thin hairline fractures with darker shadow
4. Worn smooth stone, polished, faint cold blue rim
5. Stained stone with sigil-like discoloration
6. Hairline-cracked with cold blue glow at crack edges

### 8 Wall Tiles (128×192 each — wall has top cap visible)
1. Wall isolated block (4 sides outline)
2. Wall N-S vertical run
3. Wall E-W horizontal run
4. Wall NE corner
5. Wall SE corner
6. Wall SW corner
7. Wall NW corner
8. Wall with sparse moss creeping at bottom

(Wang16 değil — basit 8 wall variant. Brush V1'a daha sonra Wang map yapılabilir.)

### 4 Decal Overlays (128px each, transparent bg)
1. Moss tuft cluster — deep moss green organic blob, transparent bg
2. Dirt patch — irregular brown stain
3. Vegetation cluster — small dark green weeds with creeping moss
4. Rubble scatter — small dark stone fragments

### 2 Large Accents (256px each, transparent bg)
1. Rift scar — large dark crimson irregular multi-blob with radial cracks, cold rim glow
2. Battle aftermath — dark red blood splatter combined with dust cloud

**TOTAL: 6 + 8 + 4 + 2 = 20 imagegen calls**

## Compose Step (Python with Pillow allowed for tile placement only)

Room compose specs:
- Canvas: 896×640 (RGBA)
- Floor area: inner ~800×500, tiled with 6 floor variants (random distribution)
- Walls: perimeter, 8 variants used appropriately for corners + edges
- Decals: scatter 8-12 decals across floor area
- Accents: 1-2 placed in central area
- Output: `STAGING/room_compare_path1_codex_full.png`

**Pillow allowed for:** loading PNG, paste with alpha, save final composite.
**Pillow NOT allowed for:** generating any visual content (shapes, textures, colors, patterns).

## Path 1 Done Report

`STAGING/codex_imagegen_full_room_path1_DONE.md`

- Tool used: imagegen skill
- Total imagegen calls: 20
- Output asset folder: `Assets/Sprites/Environment/Codex_Path1/`
- Final composite: `STAGING/room_compare_path1_codex_full.png`
- Composition time vs generation time breakdown
- Quality self-assessment vs RIMA chibi character (anti-alias drift?)

## Output Locations

```
Assets/Sprites/Environment/Codex_Path1/
  ├── floor/ (6 PNG)
  ├── walls/ (8 PNG)
  ├── decals/ (4 PNG)
  └── accents/ (2 PNG)
STAGING/room_compare_path1_codex_full.png
STAGING/codex_imagegen_full_room_path1_DONE.md
```

## Constraints

- 20 imagegen calls only (no over-generation)
- Compose: Python + Pillow paste/load/save only — NO drawing primitives
- If imagegen skill fails: stop, report, no fallback shapes
- Time budget: 30-45 minutes

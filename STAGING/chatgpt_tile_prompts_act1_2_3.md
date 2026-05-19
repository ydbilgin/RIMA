# ChatGPT (gpt-image-1) Master Prompt Pack — Act 1/2/3 Floor Tiles + Wang16 Transitions

**Hedef:** Tüm Act 1 + Act 2 + Act 3 floor base tile + flat blob-edge transition Wang16 setleri tek ChatGPT seansında üret.
**Kullanım:** Her bölüm ayrı mesaj olarak ChatGPT'ye yapıştır. Her promptun çıktısı **TEK BÜYÜK PNG** (4x4 Wang grid = 128x128 sprite sheet, daha sonra Unity'de slice).

---

## MASTER SYSTEM INSTRUCTIONS (her sohbet başında yapıştır)

```
You are generating top-down 2D pixel art floor tiles for a roguelike game called RIMA. The visual reference is Colossus: Eternal Blight — chunky 32px pixel art, painterly hand-drawn ground textures with NO black borders, NO frames, NO outlines.

CRITICAL RULES (apply to every image you generate in this session):

1. PURE top-down view ONLY. NEVER isometric. NEVER angled. NEVER any depth. NEVER side walls visible.
2. SAME ELEVATION. The ground is flat. No cliff edges. No height difference between materials. No wall stripes. No vertical surfaces. Two materials sit on the SAME flat plane.
3. CHUNKY 32px pixel art style. Each "tile" cell is 32x32 pixels. Not smooth painterly. Visible chunky pixel grain.
4. Painterly hand-drawn brush feel within each tile. Soft natural variation. No mathematical patterns.
5. SEAMLESS edges. The tile must tile against itself with NO visible seam. No black border. No frame.
6. For Wang16 corner tile sets: layout is a 4x4 grid where each cell shows a 32x32 tile with 4 corners. Each corner is either material A or material B. The 16 combinations cover every corner permutation. This produces flat blob transition edges that flow organically between cells — like watercolor edges, not straight lines.
7. The transition between two materials must be ORGANIC BLOB SHAPED with mossy/painterly hand-drawn edge wrap baked into the tile. Like Colossus Eternal Blight grass-meeting-stone-path transitions.

FORBIDDEN words (do not include in image, do not interpret): cliff, wall, edge drop, elevation, height, isometric, perspective, depth, 3D, smooth gradient, anti-aliasing, baked light, shadow cast, directional light, brick, cobblestone grid, flagstone, slab, mortar, masonry, regular pattern, uniform tiles, grid lines, tile border, black frame.

Output format: PNG sprite sheet, 4x4 grid (128x128 total) for Wang16 sets, OR single seamless 32x32 tile (output as 256x256 image showing the tile tiled 8x8 to verify seamlessness — I will manually crop one cell).
```

---

# ACT 1 — Shattered Keep

**Theme:** Fragmented ancient order, cold stone, ritual catastrophe.
**Palette:** Cool muted greys + cool brown earth. NO warm tones. NO bright green.

## PROMPT 1.1 — Cool Granite Base (16 seamless variants)

```
Generate a 4x4 sprite sheet (output as 512x512 PNG) showing 16 variants of a SEAMLESS top-down floor tile. Each tile is 32x32 pixels, displayed in a 4x4 grid (total 128x128 logical, scaled up 4x for delivery).

Material: COOL WEATHERED GRANITE FLOOR.
Color palette: base #3A3D42 to #4E5260, shadow #252830, subtle blue-violet undertones, NO warm brown.
Style: chunky 32px pixel art, hand-painted painterly grain, hairline natural cracks, ancient temple foundation stone surface, NO geometric pattern, NO grid, NO outlines, completely seamless edge.

Each of the 16 cells shows a different organic variation (crack position, lighter/darker patch, moss-free / faint dust speck). Variations are subtle — same material, slight texture difference per cell.

Pure top-down. Flat. No elevation. No borders. Painterly soft brush detail within each 32px chunk.
```

## PROMPT 1.2 — Worn Stone Path Base (16 seamless variants)

```
Generate a 4x4 sprite sheet (output as 512x512 PNG) showing 16 variants of a SEAMLESS top-down floor tile.

Material: WORN STONE PATH (rounded river-stones embedded in mud, NOT cobblestone, NOT brick).
Color palette: base #4A4842 pale grey-brown, embedded stones #5A554A, mud gap #3A3528.
Style: chunky 32px pixel art, hand-painted, soft rounded stones of varying sizes embedded into smooth mud, NO geometric grid, NO mortar lines, NO brick pattern, organic stone placement, completely seamless.

Each of the 16 cells: subtle variation in stone placement, occasional dust speck, occasional grass blade poke-through. NO cobblestone grid. NO uniform stone size. Stones look hand-painted, soft-edged, natural.

Pure top-down. Flat. No elevation. No borders.
```

## PROMPT 1.3 — Wang16 Transition: Granite ↔ Stone Path (flat blob edge)

```
Generate a 4x4 Wang16 corner tile set (output as 512x512 PNG, logical 128x128, 16 tiles of 32x32 in 4x4 grid).

PURPOSE: Flat blob-edge transition tile set between two materials on the SAME flat plane:
- Material A: COOL GRANITE (#3A3D42 cool grey)
- Material B: WORN STONE PATH (#4A4842 pale grey-brown rounded stones in mud)

Wang16 corner layout: Each 32x32 tile has 4 corners. Each corner is either material A or material B. The 16 tiles cover every combination (AAAA, AAAB, AABA, ..., BBBB) — top-left, top-right, bottom-left, bottom-right corner pattern.

CRITICAL: The transition edge WITHIN each tile between A and B is a PAINTERLY ORGANIC BLOB SHAPE, like watercolor wash bleeding into another color. NOT a straight diagonal line. The granite naturally fades into stone path with hand-painted irregular edge, maybe a thin dark mud crack between them, with painterly dust at the boundary. ABSOLUTELY NO CLIFF EDGE. NO ELEVATION DIFFERENCE. NO WALL. Both materials are at the EXACT SAME GROUND LEVEL — only color/texture changes.

Reference: Colossus Eternal Blight — grass meeting stone path with mossy painterly edge wrap.

32px chunky pixel art. Seamless cell-to-cell. No borders.
```

---

# ACT 2 — Bleeding Wastes

**Theme:** Corrupted bog, ossuary, ritual decay.
**Palette:** Dark violet-purple + bone ivory + dark crimson. NO normal swamp green.

## PROMPT 2.1 — Corrupted Bog Base (16 seamless variants)

```
Generate a 4x4 sprite sheet (output as 512x512 PNG) showing 16 SEAMLESS top-down floor tile variants.

Material: CORRUPTED BOG GROUND — saturated dark purple-violet bog mud, faintly wet, with shadow recess and subtle dark fiber tangles.
Color palette: base #3A2840, shadow #1F1428, highlight #4A3550, faint #5A4560.
Style: chunky 32px pixel art, painterly viscous wet surface, faint corrupted fiber tangles like dead roots, NO bright green moss, NO normal swamp green, NO water reflection (just damp).

Each of 16 cells: subtle variation in fiber tangle position / dark spot. Same material throughout.

Pure top-down. Flat. No elevation. No borders. Seamless.
```

## PROMPT 2.2 — Wang16 Transition: Bog ↔ Dried Blood Crust (flat)

```
Generate a 4x4 Wang16 corner tile set (output as 512x512 PNG).

Material A: CORRUPTED BOG (#3A2840 dark violet-purple wet ground).
Material B: DRIED BLOOD CRUST (#5E2A35 dark crimson cracked dry blood).

Wang16 corner layout: 16 corner-pattern combinations.

Transition edge style: painterly organic blob shape, where bog wet violet meets dry crimson crust with a soft seeping edge — like blood dried into bog ground. NO cliff. NO elevation. Both on SAME flat plane. Just color/texture change with organic edge wrap.

32px chunky pixel art. Seamless. No borders. No frame.
```

## PROMPT 2.3 — Wang16 Transition: Bog ↔ Corrupted Moss (flat)

```
Generate a 4x4 Wang16 corner tile set (output as 512x512 PNG).

Material A: CORRUPTED BOG (#3A2840 dark violet wet).
Material B: CORRUPTED MOSS (#5A4870 violet-grey moss carpet, NOT bright green).

Wang16 layout: 16 corner combinations.

Transition: painterly blob edge with violet-grey moss tufts spilling onto bog ground, like moss creeping over wet substrate. Hand-painted soft edge. SAME flat plane. No elevation.

32px chunky pixel. Seamless. No borders.
```

---

# ACT 3 — Core Approach

**Theme:** Transcendental cosmic, thinning reality, voids.
**Palette:** Void black + cool slate + gold accent. NO warm earth tones.

## PROMPT 3.1 — Void Substrate Base (16 seamless variants)

```
Generate a 4x4 sprite sheet (output as 512x512 PNG) showing 16 SEAMLESS top-down floor tile variants.

Material: VOID SUBSTRATE — near-black cosmic ground with faint star-like specks and subtle dark crystalline grain.
Color palette: base #0A0810 deep void black, rim #3A4858 cool slate edge, faint speck #6A7080.
Style: chunky 32px pixel art, painterly cosmic deep dark surface, faint star fragments embedded, no smooth gradient.

Each 16 cells: subtle star-speck placement variation. Same material.

Pure top-down. Flat. No elevation. No borders. Seamless.
```

## PROMPT 3.2 — Wang16 Transition: Void Substrate ↔ Incandescent Sigil Inlay (flat)

```
Generate a 4x4 Wang16 corner tile set (output as 512x512 PNG).

Material A: VOID SUBSTRATE (#0A0810 cosmic black).
Material B: INCANDESCENT SIGIL INLAY (#FFD700 gold engraved ancient sigil patterns, half-erased, carved into the void surface).

Wang16 layout: 16 corner combinations.

Transition: painterly organic blob edge where gold sigils fragment and dissolve into void substrate, like ancient inscriptions worn down. Soft hand-painted edge with carved gold-into-black transition. SAME flat plane. No elevation.

32px chunky pixel art. Seamless. No borders.
```

---

# OPTIONAL — Additional Act 1 transitions (if budget allows)

## PROMPT 1.4 — Wang16 Transition: Granite ↔ Cool Cave Moss (flat)

```
Generate a 4x4 Wang16 corner tile set (output as 512x512 PNG).

Material A: COOL GRANITE (#3A3D42).
Material B: COOL CAVE MOSS (#5A6B5A grey-green moss carpet, NOT bright green).

Wang16: 16 corner combinations.

Transition: painterly mossy organic edge — moss creeping onto granite stone, soft tufts at boundary. Hand-painted. SAME flat plane. No cliff.

32px chunky pixel. Seamless. No borders.
```

## PROMPT 1.5 — Wang16 Transition: Stone Path ↔ Mud Crust (flat)

```
Generate a 4x4 Wang16 corner tile set (output as 512x512 PNG).

Material A: WORN STONE PATH (#4A4842 rounded stones in mud).
Material B: MUD CRUST (#4A3C2A warm-earth-brown cracked dry mud).

Wang16: 16 corner combinations.

Transition: organic blob edge where stones thin out and mud takes over, painterly mud-creep around path stones. SAME flat plane. No elevation. No cliff.

32px chunky pixel. Seamless. No borders.
```

---

# Production order

1. **First batch (Act 1 minimum viable):** 1.1 + 1.2 + 1.3 — granite base + stone path base + their Wang16 transition. This unlocks Spawn_01 + Combat Room.
2. **Second batch (Act 1 expansion):** 1.4 + 1.5 — moss transition + mud transition.
3. **Third batch (Act 2 preview):** 2.1 + 2.2 + 2.3.
4. **Fourth batch (Act 3 preview):** 3.1 + 3.2.

## After ChatGPT generates each PNG

1. Save to `Assets/Art/Tiles/F1/Generated/<material_name>.png` (Act 1) etc.
2. Unity import: Sprite mode → Multiple, Pixels Per Unit = 64, Filter = Point, Compression = None.
3. Sprite Editor → Slice → Grid by Cell Size → 32x32. Apply.
4. Create RuleTile asset (Tilemap extras package) for Wang16 corner-based output map sprite → tile.
5. Test in Map Designer with Brush V1 round paint mode.

---

# Quality acceptance gate (before mass-producing)

After PROMPT 1.1 + 1.3 ChatGPT generates, paste both images here. We check:
- ✅ Seamless cell-to-cell (no visible seam)
- ✅ NO black border / frame around each cell
- ✅ NO cliff / elevation in transition
- ✅ Organic blob edge (NOT straight diagonal)
- ✅ Chunky 32px feel (NOT 64px smooth)
- ✅ Color palette match
- ✅ Painterly hand-drawn detail

If any fail → iterate the failing prompt. If all pass → mass produce remaining prompts.

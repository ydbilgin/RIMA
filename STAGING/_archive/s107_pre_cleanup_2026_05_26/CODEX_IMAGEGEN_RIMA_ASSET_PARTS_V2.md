# Codex Imagegen — RIMA Asset Parts v2 (Independent Pieces)

Use the `imagegen` skill (gpt-image-1 backend, verified working at `STAGING/Phase1A_L2b_Source/codex_floor_source_v1.png`).

## Goal

Generate INDEPENDENT asset PARTS for RIMA's Map Designer (Brush V1). Each output is a transparent-background sprite OR a sheet that slices into transparent sprites. NOT a single big composed image.

User's lock: "parça parça olacak bi resmi alıp oda gibi koymayacaz hepsi ayrı bi parça olacak ve güzel birleşecek." Each asset is a Brush V1 stamp.

## Camera angle LOCK (critical)

Per memory `project_camera_angle_revisit_s43.md` + 3-verdict consensus:

- **Low top-down 30-35° angled view** — NOT pure 90° top-down (banned), NOT side view
- ARPG / Diablo 2 / Hades visual style
- Objects show slight front face (very subtle depth), tops mostly visible
- Floor textures lay flat-ish but with depth direction (upper-left light source)
- Decals/scatter lay on floor as if seen from 30-35° angle (slight foreshortening)
- "Isometric-like feel" but rectangular grid (not diamond)

## Asset list (8 sheets, sliceable into ~50 transparent parts)

### Sheet 1: Floor tile variants (1024×1024, 4×4 grid → 16 tiles, 32×32 native each)

```
Sixteen plain undecorated dark slate stone floor tile variants for a dark fantasy roguelite ground surface, arranged in a 4x4 grid where each cell is 32x32 pixels. All tiles share identical muted slate blue-gray palette around hex 3A4250 with subtle warm amber undertone hex 6B5840 in cracks. Low top-down 30 to 35 degree angled ARPG perspective view (like Diablo 2 or Hades), NOT pure 90 degree top-down. Each tile shows subtle front-face direction (upper edge slightly darker, lower edge slightly lighter for depth illusion). Painterly pixel-art-compatible style. Variations:
1) clean flat slate
2) clean with subtle worn grain
3) clean with hairline crack
4) clean with edge polish
5) cracked network pattern
6) cracked with dust accumulation
7) cracked with deeper fissure
8) cracked with chip mark
9) worn polished center
10) worn weathered with age
11) worn dust pit
12) worn shallow depression
13) variant with faint warm undertone hint
14) variant with cool shadow hint
15) variant with mossy stain hint
16) variant with darker grime hint
No focal markings, no glowing elements, no painted symbols, no rune carvings, no decorative borders, no medallion frames. Each tile must tile seamlessly when placed adjacent (edge values match). Sheet has fine separator lines for slicing reference.
```

### Sheet 2: Macro floor patches (1024×1024, 4×2 grid → 8 patches, 128×128 native each, transparent background)

```
Eight irregular alpha-masked floor decoration patches for ARPG ground surface, arranged in a 4x2 grid where each cell is 128x128 pixels with TRANSPARENT background outside the irregular blob shape. Each patch is a soft-edged organic decoration that lays on a stone floor (low top-down 30-35 degree angled view). All share muted slate-amber palette consistent with the base floor:
1) clean polished stone wash patch (lighter highlight)
2) dust accumulation patch (warm amber subtle)
3) wear path patch (slightly darker compressed feel)
4) age stain patch (cool grey shadow)
5) damp wet stain patch (cool blue subtle reflection)
6) grime accumulation patch (brown-gray smudge)
7) bleached spot patch (slightly lighter cool tone)
8) shadow pool patch (darker cool spot)
NO sharp edges, NO rectangular frames. Each patch is an organic blob silhouette with soft alpha fade at edges. NO bright colors, NO neon, NO focal markings, NO runes, NO glowing elements. Painterly pixel-art-compatible style, very subtle low-contrast variation from the underlying floor.
```

### Sheet 3: Moss decals (1024×1024, 4×4 grid → 16 moss patches, 64×64 native each, transparent background)

```
Sixteen organic moss patch decals for stone floor decoration, arranged in a 4x4 grid where each cell is 64x64 pixels with TRANSPARENT background outside the irregular moss blob. Each moss patch is an irregular oval shape with feathered organic edges, internal density varies. Muted forest green palette: base hex 4A5A35, darker pockets hex 2F3F25, soft lichen highlights hex 6A7A45. Low top-down 30-35 degree angled view (slight foreshortening, upper-left light). Variations:
1) small dense moss cluster
2) medium spreading moss
3) large coverage moss patch
4) thin tendril moss
5) moss with small flowers (white tiny dots)
6) old yellowed moss
7) moss with stone fragments peeking
8) moss in crack pattern
9) moss in corner formation
10) moss with thin spreading edges
11) dense central moss patch
12) sparse scattered moss
13) moss with lichen highlights
14) moss in oval formation
15) moss with subtle wet sheen
16) moss with dark shadowed pockets
NO bright lime green, NO neon, NO rectangular frames, NO glowing magic moss, NO perfect circles. All edges feather naturally into transparent background.
```

### Sheet 4: Dirt / grime decals (1024×1024, 4×3 grid → 12 patches, 64×64 native each, transparent background)

```
Twelve organic dirt and grime patch decals for stone floor decoration, arranged in a 4x3 grid where each cell is 64x64 pixels with TRANSPARENT background outside the irregular shape. Muted brown-gray palette: base hex 4A3F2F, darker accumulation hex 2A2520, faint dust hex 6A5F4F. Low top-down 30-35 degree angled view. Variations:
1) dark dirt smudge
2) ground-in grime spot
3) thin dirt streak
4) dust accumulation pile
5) muddy footprint trail (subtle, organic)
6) earth stain irregular blob
7) grit and small debris cluster
8) wet earth smudge
9) dried mud crack pattern
10) sand-like fine dust patch
11) thicker mud accumulation
12) sparse dirt sprinkle
NO bright orange, NO neon, NO glowing, NO rectangular frames, NO blood splatters, NO magical marks, NO perfect circles. All organic shapes with feathered transparent edges.
```

### Sheet 5: Detail scatter — pebbles + small rubble (1024×1024, 4×3 grid → 12 patches, 64×64 native, transparent background)

```
Twelve pebble and small rubble cluster decals for stone floor, arranged in a 4x3 grid where each cell is 64x64 pixels with TRANSPARENT background outside the cluster. Muted slate gray pebbles, subtle warm shadow undertones hex 4A4F55 with hex 3A3530 shadows. Low top-down 30-35 degree angled view (pebbles show very slight top-light + side-shadow for depth). Variations:
1) tight pebble cluster (5-7 small stones)
2) loose scattered pebbles (8-10)
3) single medium pebble
4) pile of varied size rubble
5) small rubble chunks in line
6) corner pile (concentrated)
7) wide sparse scatter
8) rubble with dust around
9) angular shattered fragments
10) rounded weathered pebbles
11) mixed pebbles + dust patch
12) cluster with one larger stone in middle
NO bright colors, NO neon, NO glowing, NO organized stacking, NO perfect circles, NO rectangular frames. All natural organic loose grouping.
```

### Sheet 6: Cracks + bones detail scatter (1024×1024, 4×3 grid → 12 patches, 64×64 native, transparent background)

```
Twelve detail scatter decals (cracks and bone fragments mixed) for stone floor, arranged in a 4x3 grid where each cell is 64x64 pixels with TRANSPARENT background. Low top-down 30-35 degree angled view. Cracks: dark interior hex 1A1E22, dust highlight hex 5A5550. Bones: muted off-white hex 8A7F6F. Variations:
1) thin branching hairline crack (single line)
2) network of hairline cracks (3-4 lines)
3) deeper crack with dust
4) crack starburst pattern
5) hairline crack along edge direction
6) single small bone shard
7) bone fragments scattered (3-4)
8) small skull fragment + dust
9) rib-like long thin bone pieces
10) tiny dust flecks + bone bits
11) crack passing through bone fragment
12) bone fragment cluster with hairline crack
NO straight grid lines (cracks must be organic curves), NO bright glow, NO magical rift cracks, NO neon, NO full skeletons, NO perfect symmetry, NO rectangular frames.
```

### Sheet 7: Accent overlays — rift scars (1024×1024, 2×2 grid → 4 rift accents, 256×256 native each, transparent background)

```
Four rare focal-accent rift scar overlays for stone floor, arranged in a 2x2 grid where each cell is 256x256 pixels with TRANSPARENT background. Each is a sparingly-placed focal accent (rare in a room, 1-3 per room max). Dark void interior hex 1A0F1F, cool blue energy crackles along jagged edges hex 3A4A6A, subtle violet glow tints hex 4A3A5A. Low top-down 30-35 degree angled view (rift appears as cracked opening with slight depth into void). Variations:
1) small jagged rift crack (linear)
2) medium spiderweb rift pattern
3) large irregular rift opening with branch cracks
4) rift with surrounding glow halo
NO bright neon, NO full glow flood, NO perfect symmetry, NO square shapes, NO rectangular frames, NO photo-real explosion, NO magical effect spam. Used sparingly as focal narrative beat in room.
```

### Sheet 8: Accent overlays — ritual marks (1024×1024, 2×2 grid → 4 ritual accents, 256×256 native each, transparent background)

```
Four rare focal-accent ritual mark overlays for stone floor, arranged in a 2x2 grid where each cell is 256x256 pixels with TRANSPARENT background. Faded geometric markings with weathered chalk-paint appearance. Muted bone-white pigment hex 7A6F5F with subtle warm gradient. Low top-down 30-35 degree angled view. Variations:
1) simple circle with cross inside
2) pentagram with outer circle (faded)
3) ritual symbol cluster (lines + dots)
4) protective sigil with arcing curves
NO bright neon, NO fresh paint look, NO vivid red, NO vivid orange, NO glowing magical mark, NO perfect crisp symmetry (must look weathered), NO photo-real. Faded chalk-paint authenticity.
```

## Output paths

- `STAGING/RIMA_AssetParts_v2/sheet_01_floor_tiles_32x32.png` (1024×1024)
- `STAGING/RIMA_AssetParts_v2/sheet_02_macro_patches_128x128.png` (1024×1024)
- `STAGING/RIMA_AssetParts_v2/sheet_03_moss_64x64.png` (1024×1024)
- `STAGING/RIMA_AssetParts_v2/sheet_04_dirt_64x64.png` (1024×1024)
- `STAGING/RIMA_AssetParts_v2/sheet_05_pebbles_64x64.png` (1024×1024)
- `STAGING/RIMA_AssetParts_v2/sheet_06_cracks_bones_64x64.png` (1024×1024)
- `STAGING/RIMA_AssetParts_v2/sheet_07_rift_256x256.png` (1024×1024)
- `STAGING/RIMA_AssetParts_v2/sheet_08_ritual_256x256.png` (1024×1024)

## Tasks after generation

1. **Quality check** each sheet for:
   - Camera angle: 30-35° angled, NOT pure 90° top-down
   - Per-tile borders: no dark ring frames around each cell (especially Sheet 1 floor tiles)
   - Palette consistency: muted slate-amber, no drift to neon/orange/cream
   - Transparent background outside organic shapes (Sheets 2-8): MUST be transparent, not solid white/gray
   - Style: painterly pixel-art-compatible, not photorealistic noise

2. **If QC PASS:** write `STAGING/CODEX_IMAGEGEN_RIMA_ASSET_PARTS_V2_DONE.md` with file paths + SHA256 + visual notes per sheet.

3. **If QC FAIL on a sheet:** regenerate ONLY that sheet with refined prompt (e.g., emphasize "transparent background MANDATORY", "angled view 30 degrees NOT 90", "no border ring"). Do not regenerate passing sheets.

4. **Total sheets:** 8. Sliceable into ~50-80 transparent PNG parts.

## Constraints

- Do NOT modify any `Assets/` files (output only to `STAGING/RIMA_AssetParts_v2/`)
- Do NOT use PixelLab MCP (credits 0)
- Do NOT use placeholder Pillow shapes — use real gpt-image-1 backend
- Each sheet must be 1024×1024 PNG
- Style consistent across all 8 sheets (same palette, same camera angle, same painterly mood)

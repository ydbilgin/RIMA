# ChatGPT (gpt-image-1) Tile Prompt Pack — RIMA Style

Copy-paste these prompts to ChatGPT (web UI with Image gen mode). Each prompt outputs ONE PNG. After receiving:
1. Sağ click → Save Image As
2. Drop into specified Unity path
3. Refresh Unity, set Pixel Per Unit = 64, Filter Mode = Point, no compression

## RIMA Style Anchors (her promptun başında bu paragraf olmalı)

```
RIMA style locks:
- Painterly pixel art, 32x32 chunky pixel scale (NOT smooth modern pixel art)
- Top-down PURE flat view, NO perspective, NO depth, NO shading direction
- Hand-painted look, slightly worn ancient temple aesthetic
- Cool muted palette: gray, blue-violet undertones, cool brown (#3A3D42, #4E5260, #4A4842, #6A5C48)
- NO bright colors, NO saturation pop
- NO text, NO logo, NO watermark, NO border, NO frame, NO outline
```

---

## PROMPT 1 — Cool Granite Floor (PRIMARY, en kritik)

**Save to:** `Assets/Art/Tiles/F1/SeamlessV1/granite_painterly_01.png`

```
[RIMA style anchor — paste above]

Generate a 512x512 SEAMLESS top-down floor texture: cool weathered granite stone surface.

CRITICAL SEAMLESS REQUIREMENTS:
- Top edge pixels match bottom edge pixels (the texture tiles vertically with no seam)
- Left edge pixels match right edge pixels (tiles horizontally)
- When tiled 4x4 in a grid, NO visible boundary line between tiles
- ZERO border, ZERO frame, ZERO outline at the texture edges

Material details:
- Continuous monolithic granite ground, NOT individual stones with gaps
- Subtle hairline cracks scattered randomly across the WHOLE 512x512 (NOT one per 128x128 quadrant — really random, sparse)
- Muted cool gray with #3A3D42 to #4E5260 range, very subtle blue-violet hue
- Tiny mineral speckles (1-2 pixel dark and light flecks) randomly distributed
- Painterly hand-drawn brush feel, NO mosaic, NO cobblestone, NO brick, NO mortar grid

Output: 512x512 PNG, top-down flat, perfect seamless tile.
```

---

## PROMPT 2 — Worn Stone Path Surface (secondary floor)

**Save to:** `Assets/Art/Tiles/F1/SeamlessV1/stone_path_painterly_01.png`

```
[RIMA style anchor]

Generate a 512x512 SEAMLESS top-down floor texture: worn stone path surface.

CRITICAL: Same seamless requirements as granite (top=bottom, left=right edge pixels, no boundary when tiled).

Material:
- Rounded river-stones embedded in dark mud surface
- Stones soft-edged painterly, NOT geometric cobblestone, NOT brick
- Stones varied size (10-20% larger pebbles, 80% small chips), randomly placed
- Pale grey-brown #4A4842 base mud, darker #3A3528 between stones
- Stones have subtle 1-pixel highlight on one side (suggesting top-down with neutral light)
- Worn, ancient, walked-on by centuries of feet
- Continuous surface, no gaps, no border, no individual tile shapes

Output: 512x512 PNG seamless tile.
```

---

## PROMPT 3 — Mossy Granite Variant (decoration overlay)

**Save to:** `Assets/Art/Tiles/F1/SeamlessV1/granite_mossy_01.png`

```
[RIMA style anchor]

Generate a 256x256 SEAMLESS texture: cool granite stone surface with sparse moss patches.

Same granite base as PROMPT 1 (#3A3D42 cool gray) but with cool cave moss creeping over ~30% of the surface:
- Moss color: #5A6B5A grey-green to #2A4520 darker patches
- Moss in irregular blobs, NOT uniform coverage
- Moss thicker in 1-2 corners, sparse elsewhere
- Some moss tufts standing slightly proud (subtle bump shading)

Same seamless requirements (top=bottom, left=right).

Output: 256x256 PNG seamless tile (smaller than primary because used as accent variant).
```

---

## PROMPT 4 — Wall Cap (Hades-style 35° vertical, perimeter)

**Save to:** `Assets/Art/Tiles/F1/Walls/wall_cap_granite_01.png`

```
[RIMA style anchor]

Generate a 256x128 sprite: wall cap section for a top-down dungeon room perimeter.

This is NOT a tile — this is a horizontal wall sprite that sits at the top edge of a room. Drawn at slight angle (Hades-style 30-35° from top-down) so the wall has VISIBLE HEIGHT — top of wall is in front of base of wall in screen space.

Material:
- Cool weathered granite blocks (same palette as granite floor: #3A3D42, #4E5260)
- Painterly hand-drawn, NOT brick grid
- Width 256px = 8 floor cells wide, tileable horizontally (left edge matches right edge)
- Height 128px = 4 cells tall (the wall takes vertical space for the height illusion)
- Subtle moss/dust at base of wall (where wall meets floor)
- Tiny cracks scattered, similar style to floor

Format: PNG with transparency BELOW the wall base (so when placed in Unity, the wall sits on top of floor naturally).
Top-down camera at 30-35° angle. The top edge of the sprite is the "back" of the wall.

NO border, NO frame, NO outline.

Output: 256x128 PNG with alpha.
```

---

## PROMPT 5 — Gate Arch (matching granite palette)

**Save to:** `Assets/Art/Tiles/F1/Walls/gate_arch_granite_01.png`

```
[RIMA style anchor]

Generate a 128x192 sprite: gate archway for a dungeon room entrance/exit.

This is a vertical sprite placed at left or right edge of room. The arch opens TOWARD camera (you can see through it into darkness beyond).

Material:
- Cool weathered granite blocks forming arch frame (same palette: #3A3D42, #4E5260)
- Painterly stone block construction, NOT smooth
- Inside the arch: dark void with subtle cyan/violet rift light hint at edges (#00FFCC or #5A2A8A very subtle)
- Iron-banded wooden gate (closed) visible inside the arch — dark wood #2A1F18 with #5A4A38 iron bands
- Slight moss at base of arch where it meets floor
- Painterly hand-drawn, NOT clean modern

Format: PNG with transparency around the arch outline. Center of arch can be slightly translucent (suggesting depth into the gate).

NO border, NO frame, NO outline AROUND the sprite. The arch itself IS the visual.

Output: 128x192 PNG with alpha. Pixel Perfect Camera friendly (sharp edges, no anti-alias).
```

---

## PROMPT 6 — Floor Patch (L4 organic overlay)

**Save to:** `Assets/Art/Tiles/F1/Patches/patch_moss_organic_01.png`

```
[RIMA style anchor]

Generate a 128x128 sprite: organic moss patch overlay for placing ON TOP of stone floors.

This is a decorative overlay, NOT a tile. Goes between floor tilemap and characters. Soft alpha edges so it blends with the floor underneath.

Visual:
- Irregular blob shape, NOT circle, NOT square — like spilled liquid
- Cool cave moss color: #5A6B5A center, fading to #2A4520 at darker spots
- Soft alpha at edges (NOT hard cutoff) so it bleeds into surrounding floor
- Small moss tufts standing slightly proud on top
- Painterly hand-drawn, NOT digital airbrush

Format: PNG with transparency. The blob covers maybe 60-70% of the 128x128 canvas, rest transparent.

NO border, NO frame, NO outline.

Output: 128x128 PNG with alpha.
```

---

## Production order (önerim)

| Sıra | Prompt | Süre | Reason |
|---|---|---|---|
| 1 | PROMPT 1 (granite floor) | First | Foundation — bütün başka şey üstüne gider |
| 2 | PROMPT 4 (wall cap) | After tile pass | Floor net olduktan sonra walls match'lensin |
| 3 | PROMPT 5 (gate arch) | After walls | Granit duvarla uyumlu kapı |
| 4 | PROMPT 2 (stone path) | After 3 | Floor variant 2nd material için |
| 5 | PROMPT 6 (moss patch L4) | After 4 | Decoration overlay |
| 6 | PROMPT 3 (mossy granite variant) | Optional | Floor variant |

## Quality check (her PNG geldikten sonra)

1. Image'i Unity'de açmadan önce ChatGPT'ye geri sor: "**Show me this texture tiled 3x3**" — eğer 3x3 grid'inde kare kare boundary'ler görünüyorsa, **fail** — yeniden iste "**ABSOLUTELY no border between tiles, top edge pixels MUST equal bottom edge pixels**"
2. Color palette match: ChatGPT bazen tonu çok soğuk veya çok ılık yapıyor. "Cool, muted, slight blue-violet hue" deyip düzelt
3. Painterly mu mosaic mi: Her cell aynı tekrarlı pattern ise mosaic — "more random organic detail, painterly brush strokes not geometric pattern"

## Hata kalıpları (ChatGPT'nin sık yaptığı yanlışlar)

- **"tile" kelimesi:** Her zaman "texture" veya "surface" de, "tile" deme
- **"cobblestone":** Yasak — granit dümdüz olsun, taş bloklara ayrılmasın
- **Bright color pop:** "muted, desaturated" emphasis
- **Cell border:** İlk gen border çıkarsa "remove all dark edge pixels, top pixel row should color-match bottom pixel row"

---

Sen bu prompt pack'i ChatGPT'ye verince sırayla üret, her PNG geldiğinde sana göstereyim ve doğru Unity path'ine koyalım. Sahnede direkt değişir.

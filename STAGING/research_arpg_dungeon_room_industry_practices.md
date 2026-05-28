# ARPG/Roguelite 2D Dungeon Room Composition — Industry Research

*Model: gemini-2.5-pro (primary) + default Gemini CLI (cross-validation). Both runs agree on all concrete values.*

---

## 1. Modular Wall Composition

### Reference Game Pipelines

**Diablo II** uses a granular tile system (`DT1` graphics + `DS1` layout blueprints) with explicit Orientation Indexes (0-19) tagging each wall piece for neighbor-connection logic (straight, corner, pillar). Floor tiles are 160x80px on a 5x5 logical sub-grid. Procedural assembly places correct tile variants automatically at every junction.

**Hades (Supergiant)** does not procedurally generate rooms. It uses a large pool of hand-crafted room templates that are mirrored/rotated to fit entry/exit vectors — modular kit-of-parts used inside pre-authored templates, not raw tile placement. Variety comes from template count + rotation symmetry.

**Children of Morta (Dead Mage)** evolved from rigid grid to sub-tile increment connections for more organic shapes. Uses "clutter modules" — pre-composed prop clusters placed procedurally as a single unit — to achieve a hand-placed feel from procedural generation. "Fringe" assets that overlap wall-to-floor boundaries blur grid seams.

**Curse of the Dead Gods / Darkest Dungeon** achieve variety from a very small core set (~10 tile types: straight, L-turn, T-junction, etc.) by: (1) rotating and mirroring tiles, (2) using "mutators" that toggle sub-meshes on/off at placement time (a barrel, a broken fence, etc.).

### Pillar-as-Seam-Cover Pattern

Pillars, columns, or statues are placed over the 90-degree junction where two straight wall segments meet. This accomplishes two things simultaneously:
1. Hides visual discontinuities and color mismatches at tile joints.
2. Breaks repetition of long wall runs with a distinct vertical element.

In Diablo II's system, "Pillar" is a formally named Orientation Index (Index 12) — it is part of the base spec, not an afterthought. In Children of Morta, fringe assets serve an equivalent role. The implementation rule: place a "hero prop" programmatically at every wall junction your room builder creates.

### Minimum Viable Wall Set

For a top-down 75-80 degree perspective, professional minimum viable sets converge at **12-16 unique shapes**. The cross-validated list:

| Piece | Count | Notes |
|---|---|---|
| Straight Wall N/W | 1 | Primary horizontal segment |
| Straight Wall N/E | 1 | Primary vertical segment |
| External Corner | 4 | N, E, S, W — wall juts outward |
| Internal Corner | 4 | N, E, S, W — wall recesses inward |
| End Cap | 2 | Terminates freestanding wall cleanly |
| Standalone Pillar | 1 | Seam cover + visual break |
| Doorway/Archway | 1 | Placed over straight segment |
| **Total** | **14** | Floor tile sold separately (seamless 64x64) |

A "Core 7" absolute minimum (run 1): North Wall, South Wall, East Wall, West Wall, Pillar/Corner Cap, Doorway, Floor tile. The 14-piece set is strongly recommended for enclosed dungeon biomes.

### Typical Sizes at 64 PPU

- **Floor tiles:** 64x64 px (1x1 Unity unit). Exact powers-of-two required for texture compression.
- **Wall face height:** 64-96 px of visible vertical surface. At 75-80 degree camera, this communicates wall height without obscuring the play area.
- **Wall depth (top surface):** 32-64 px — the visible "thickness" seen from above.
- **Single wall sprite total:** typically 128 wide x 160 tall (64px footprint + 96px face). The face portion maps to a 64px quad in world space.
- 128x128 floor tiles are also common (2x2 Unity units) for fewer tilemap cells.

---

## 2. Lighting Setup (URP 2D specifically)

### Global Light 2D — Ambient Base

Never use pure black (#000000) — it flattens pixel art and makes unlit areas completely unreadable.

| Dungeon Mood | Color Hex | Intensity |
|---|---|---|
| Cold/moonlit dungeon | #1A1C2C (dark navy) | 0.05 – 0.15 |
| Gritty/neutral dungeon | #121212 (dark grey) | 0.10 – 0.20 |
| Earthy/cave dungeon | #1B1411 (muddy brown) | 0.10 – 0.20 |
| Shattered Keep (recommended) | #161624 (desaturated purple-navy) | 0.15 |

Lower intensity = higher reliance on local torch lights = more atmosphere. RIMA's dark fantasy target sits at 0.15.

### Point Light 2D — Torch/Brazier

```
Color (HDR):       #FF9E2C (warm orange)
Intensity:         1.0–1.5 (no bloom) / 2.0–2.5 (with Bloom post-process)
Outer Radius:      3–5 Unity units (192–320 px at 64 PPU)
Inner Radius:      0.2
Falloff Curve:     Custom convex curve — bright core, sharp edge drop-off
                   Start with Falloff Intensity = 0.5
Shadows Enabled:   True (major torches only — 1-3 per room max)
Shadow Intensity:  0.8
Use Normal Map:    Enabled (if walls have normal maps)
```

A large brazier or room-center fire: Outer Radius 8-10 units.

### Light Count Per Room

- **Shadow-casting lights:** 1-3 per room. Shadows are the most expensive feature; more than 3 causes severe performance regression.
- **Atmospheric lights (no shadows):** 5-10 per room. Glowing mushrooms, embers, background highlights — significantly cheaper.
- **Player-attached utility light:** 1 small point light parented to player, shadows disabled, low intensity (radius ~2 units), ensures minimum visibility independent of environment.
- Unity hard limit: a single GameObject is affected by max 8 lights by default.
- Always disable Light 2D on off-screen rooms.

### Global vs. Point — Role Split

| Component | Role | Key Setting |
|---|---|---|
| Global Light 2D | Ambient floor — defines darkness color | Intensity 0.05-0.15, no shadows, affects Background + Floor layers |
| Point Light 2D (static) | Torch/brazier focal points | Intensity 1.0-2.5, may cast shadows, targets Environment + Characters layers |
| Point Light 2D (player) | Readability utility | Small radius (~2 units), no shadows, always visible |

### Bloom "Secret Sauce"

Add a Global Volume with Bloom override:
- Threshold: just below HDR light intensity (e.g., 1.1 if torch Intensity = 2.0)
- Intensity: 0.5 (soft atmospheric glow, not over-exposed)

Enable Normal Maps on the 2D Renderer Data asset so Point Lights catch brick bevels.

---

## 3. Sorting / Y-sort Patterns

### Setup (Three Required Steps)

1. **Project Settings > Graphics > Transparency Sort Mode:** set to `Custom Axis`
2. **Transparency Sort Axis:** `X=0, Y=1, Z=0`
3. **Every sprite pivot:** set to base/feet of object; SpriteRenderer Sort Point = `Pivot`

Objects on the same Sorting Layer auto-sort: lower screen Y = drawn in front. Character walks "above" a pillar → renders behind it automatically.

### SortingGroup (not CompositeSortingGroup)

Unity's component is named `Sorting Group`. Add to a character's root GameObject when the character is made of multiple child sprites (body, head, weapon). Forces the entire character to sort as one atomic unit using the root pivot. Prevents the weapon from clipping behind a wall while the body is in front.

Inside a Sorting Group, use `Order in Layer` on children to define internal stacking (head = 10, torso = 5, legs = 1).

### Custom Axis vs. sortingOrder — When to Use Each

| Method | Scope | Use For |
|---|---|---|
| Custom Axis Sort | Global engine rule, per-layer | Automatic Y-depth for all dynamic objects in the "World" layer |
| SpriteRenderer.sortingOrder | Single object override | Inside Sorting Groups (character parts), static decal ordering, temporary VFX overrides |

### Character-Behind-Wall Silhouette

Standard practice: duplicate the character sprite (or use a URP Renderer Feature) that renders with a Stencil Buffer. Wall writes to stencil; duplicate character reads stencil, draws a flat color (e.g., #55AADD desaturated blue) only where occluded. Result: player remains readable behind opaque walls.

### Recommended Sorting Layer Stack (RIMA)

| Layer | Order | Content | Sort Method |
|---|---|---|---|
| Background | 1 | Non-interactive ground, distant BG | Default |
| Floor | 2 | Walkable tilemap | Default |
| Floor_Decals | 3 | Blood pools, rugs, floor markings | Default |
| Shadows | 4 | Character blob shadows | Default |
| World | 5 | Player, enemies, pillars, walls, tall props | **Custom Axis (Y-Sort)** |
| VFX | 6 | Projectiles, spells, explosions | Default |
| UI | 7 | HUD, health bars, menus | N/A (Screen Space) |

Key insight: place all dynamic objects on a single "World" layer and trust Custom Axis. Splitting into props_low/props_high adds complexity without benefit — Custom Axis handles it automatically.

---

## 4. Character Placement (Test Scene)

### Pixel Grid Snapping

At PPU=64, one pixel = 1/64 = 0.015625 Unity units. All transform coordinates must be exact multiples of this value.

- Valid position: `(2.5, 3.125)` — both are exact multiples of 0.015625
- Invalid: `(2.51, 3.12)` — off-grid, causes visual jitter

Set Edit > Snap Settings > Move X/Y = `0.015625` so editor snapping locks to the pixel grid.

Place test-scene character pivot (feet) at `(0, 0, 0)` to establish the room's Y-sort baseline.

### Pixel Perfect Camera Settings

Use the Pixel Perfect Camera component (2D Pixel Perfect package).

| Setting | Value | Notes |
|---|---|---|
| Assets Pixels Per Unit | 64 | Must match sprite imports |
| Reference Resolution | 480x270 | Good balance; scales 4x to 1920x1080 |
| Reference Resolution (zoomed in) | 384x216 | 5x scale to 1920x1080 |
| Upscale Render Texture | Enabled | Core of pixel-perfection; Point filter upscale |
| Pixel Snapping | Enabled | Post-render snap; fixes floating-point drift |

Ortho Size is calculated automatically: `270 / (2 × 64) = 2.109375` for 480x270.

### Preventing Sub-Pixel Jitter

1. Enable Upscale Render Texture + Pixel Snapping on Pixel Perfect Camera.
2. If using Cinemachine: put Pixel Perfect Camera on the main Camera (not virtual camera), add `CinemachinePixelPerfect` extension to the virtual camera. This bridges Cinemachine smooth damping with pixel grid snapping.
3. Sprite import settings: Filter Mode = `Point (no filter)`, Compression = `None`.
4. Rigidbody2D: set Interpolation = `Interpolate` to prevent physics-driven jitter.
5. Optional "Ghost Transform" pattern: Rigidbody moves in smooth float space; child SpriteRenderer GameObject rounds its local position to nearest `1/64` in `LateUpdate`. Physics stays smooth, graphics pixel-snapped.

---

## 5. AI Image-Gen Wall Workflow

### Production Hybrid Workflow (Community Standard)

Pure AI output is not game-ready. The validated pattern is:

1. **Generate material base:** Produce a large seamless tileable texture of the wall material ("mossy stone bricks", "cracked obsidian") — not a final wall piece, just the raw material.
2. **Downscale with Nearest Neighbor:** Scale to target (128x128 or 256x256) using Nearest Neighbor interpolation only. Preserves hard pixel edges.
3. **Quantize palette:** Reduce to 16-64 colors using indexed-color mode in Aseprite/Photoshop to achieve authentic pixel art look.
4. **Cut modular pieces:** Slice straight walls, corners, and pillars from the single consistent texture. All pieces share the same lighting and texture properties = automatic cohesion.
5. **Manual cleanup pass:** Remove awkward pixel clusters, add unique details (specific cracks, torch holders, dangling chains) to break repetition.

### Prompt Patterns for Consistency

Structure: **Style — Subject — Detail — Technical**

```
pixel art, 16-bit game asset, a seamless tileable texture of a dungeon wall 
with mossy stone bricks, dark fantasy style, flat lighting, uniform, no shadows --tile

Negative: 3d, realistic, photo, gradient, perspective, shadows, border, frame, 
text, blurry, noise, distorted
```

Key rules:
- Avoid "dramatic lighting" and "isometric" — these corrupt the orthographic projection
- Add `--tile` flag in Midjourney; use Asymmetric Tiling extension in SD (X-axis only for walls)
- For RIMA specifically: add "Shattered Keep, ancient ruined fortress, dark stone, decay" to the style clause

### Advanced: LoRA Training

For a project of RIMA's scope, training a LoRA on curated "gold standard" outputs is the most robust consistency method. Generate 20-30 approved base tiles, curate, train a mini-LoRA, use that LoRA in all subsequent prompts. RIMA already has PixelWave base + 335-image dataset (S102 session); wall tiles should be added to the LoRA dataset when approved.

### Seamless Tiling Verification

1. **Offset Filter Test (Photoshop / Aseprite):** Apply offset of exactly half image dimensions with Wrap Around. If a cross-shaped seam appears in the center → tile is not seamless. Paint seam out manually.
2. **Manual 2x2 Grid:** New canvas 4x tile size, paste tile into 2x2 array, inspect visually.
3. **Online tiling checkers:** Upload tile to any free "CSS background tiling" tool — instant live repeat view.
4. **Unity Tilemap Palette:** Final verification in-engine before production approval.

---

## RIMA Action Recommendations

These are concrete, immediately actionable items for the A6 wall test room setup in Unity.

1. **Wall set target: 14 pieces.** The current 16-piece modular kit (imported S99) already meets or exceeds this. Confirm you have: 2 straight walls (N/W and N/E directions), 4 outer corners, 4 inner corners, 2 end caps, 1 pillar, 1 archway. If inner corners are missing, generate them next PixelLab session via 2x2 grid sheet with reference.

2. **Set Global Light 2D to Intensity 0.15, color #161624.** This is the Shattered Keep ambient baseline. All other lighting decisions are relative to this. Do not use white ambient or Intensity > 0.20.

3. **Add 2-3 torch Point Light 2Ds per room: Outer Radius 4 units, Intensity 2.0, color #FF9E2C, Falloff convex custom curve, Shadow Intensity 0.8.** One shadow-casting torch max per room during initial performance testing. Add a Bloom Volume (Threshold 1.1, Intensity 0.5).

4. **Sorting Layer stack: create Background, Floor, Floor_Decals, Shadows, World, VFX, UI.** Set Transparency Sort Mode to Custom Axis (X=0, Y=1, Z=0) in 2D Renderer Data. All walls, pillars, and the character go on World layer. Add Sorting Group to character root.

5. **Pixel Perfect Camera: Reference Resolution 480x270, Assets PPU 64, Upscale Render Texture ON, Pixel Snapping ON.** Set editor snap to 0.015625 on X/Y. Place character pivot at (0, 0, 0).

6. **Pillar placement rule: every wall corner junction gets a pillar.** Place standalone pillar prefab at every 90-degree wall intersection before the room is considered complete. This hides seams and breaks repetition without additional unique art.

7. **PixelLab wall production prompts:** Use the Style-Subject-Detail-Technical structure. For inner corner pieces specifically: `"pixel art, 16-bit, game asset, dungeon wall inner corner piece, Shattered Keep stone, top-down 75 degree view, dark fantasy, flat lighting, seamless edge match, no shadows, orthographic"`. Generate as 4-tile 2x2 sheet to ensure all four rotations share identical material.

---

## Sources

- Unity Manual: 2D Lights, URP 2D Renderer Data, Sorting in 2D, Sorting Group, Pixel Perfect Camera, Cinemachine Pixel Perfect Extension — https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@latest/manual/2DLightingandShadows.html
- Unity 2D Pixel Perfect package documentation — https://docs.unity3d.com/Packages/com.unity.2d.pixel-perfect@latest
- GDC 2019: "Unity 2D: Pixel Perfect Camera and Beyond" (cited by Gemini)
- Diablo II DS1/DT1 file format documentation — community reverse-engineering (Phrozen Keep forums, Blizzhackers wiki)
- Children of Morta GDC / post-mortem materials — Dead Mage, cited in various IndieDB/Gamasutra retrospectives (no single URL confirmed)
- Stable Diffusion Asymmetric Tiling extension — https://github.com/tjm35/asymmetric-tiling-sd-webui
- Gemini research synthesis (no single external URL — model knowledge + web search, rated MEDIUM confidence for specific numeric values, HIGH for pattern descriptions)
- RIMA project context: S99 16-piece modular kit import, S102 LoRA infrastructure

*Confidence: HIGH for Unity API values (official docs cited internally by model), MEDIUM for game-specific pipeline claims (Hades, Children of Morta — no primary source URL returned, model knowledge only), HIGH for sorting + pixel-perfect math (official Unity docs).*

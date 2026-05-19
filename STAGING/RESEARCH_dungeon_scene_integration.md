# Dungeon Scene Integration Research
**Date:** 2026-05-19  
**Model:** Gemini CLI default (gemini-2.5-pro-preview, Google Search grounding)  
**Question:** How do shipped indie 2D top-down games achieve cohesive complete-scene look?

---

## Section 1: Hades Pipeline (Supergiant Games)

**Key insight:** Hades is NOT pure 2D — it uses a "2.5D" hybrid that explains its cohesion.

### Layer Stack
- **Background** (pre-rendered static or subtly animated environment painting)
- **Far-off objects** (pillars, columns behind gameplay plane)
- **Gameplay layer** (characters, interactive props — sorted by isometric Y)
- **Foreground props** (elements that occlude characters)

### Asset Integration
- Characters modeled in **ZBrush + animated in Maya**, then pre-rendered to 2D sprites (Bink Video compression). 8–64 angles per character.
- Environments use **3D meshes with projected UVs** — 2D hand-painted textures wrapped onto simple geometry. This is why floors never look flat/black: the mesh has painted ambient occlusion, cracks, and surface detail baked directly into the texture.
- Over **1,400 "stitched" environment tiles** — large pre-decorated chunks (not 32×32 grids), each inherently containing transition gradients and floor debris. Seams are hidden because tiles are designed to contain the transition, not abut against it.

### Depth Sorting
- **Directed Acyclic Graph (DAG)** sorts isometric bounding volumes for precise Z-ordering. Characters pass behind pillars correctly because every object has a 3D bounding volume, not just a 2D sort key.

### Lighting Model
- Lighting is **NOT runtime 3D lights** — artists draw 2D "lighting assets" (glow sprites, shadow overlays) that are isometrically transformed and composited.
- Emissive highlights: Jen Zee's signature **neon splotches** (pink/purple/green) applied as a final additive pass — these are what give Hades its electric, dramatic feel.
- "Black Shading" (Mike Mignola-inspired): pure black used for deep contrast in shadows. No mid-grey ambiguity.

### Floor-Wall Transition
- Wall tiles designed to **extend into the floor plane** — the wall base has a painted shadow/ambient occlusion gradient that bleeds 16–32px onto the floor tile beneath it.
- Debris clusters (pebbles, dust particles, cracks) are baked into the wall-adjacent floor tiles, not scattered as separate props.

### Post-FX
- Color grade: **warm highlights + cool deep shadows** (classic dungeon palette separation)
- Emissive bloom: **additive only on HDR-flagged emissive sprites** (torches, magic effects). Not global bloom.
- No information confirmed about specific Unity post-processing settings — Supergiant uses a custom engine (not Unity).

**Sources:** GDC 2020 Hades talks (Supergiant GDC vault), Devansh Maheshwari/Jen Zee developer interviews cited by Gemini search.

---

## Section 2: Children of Morta / Death's Door / Others

### Children of Morta (Dead Mage, Unity)

**Key pipeline trick: upscaling pixel art for HD lighting.**

- Animation: frame-by-frame traditional pixel art (Studio Ghibli inspiration)
- **Puppet Show Lighting technique:** sprites rendered at **~4x their native resolution** inside Unity. This lets Unity's dynamic light system cast high-resolution soft shadows over chunky pixel art without the lighting looking blocky/pixelated. At native res (e.g. 64px), Light2D would produce 64px-resolution light falloff (ugly). At 4x (256px), falloff is smooth.
- Custom memory management + sprite atlas system built to handle thousands of high-res animation frames during procedural dungeon runs.
- Floor fill: combination of painted floor texture tiles (organic, not grid-repeating) + scattered ambient AO decals at wall bases.

**Source:** Dead Mage "Children of Morta" Postmortem on Game Developer/Gamasutra.

### Death's Door (Acid Nerve)
- True 3D environment with 2D-feeling art direction (similar to Hades premise)
- Camera at fixed oblique angle — floors have painted surface detail in the 3D texture maps
- Character-environment integration via **contact shadow blob** (hard-edged ellipse, 30–50% opacity) under every entity

### Enter the Gungeon (Dodge Roll, Unity)
- Pure pixel art, top-down, tighter camera
- Floors covered by **high density of floor decals**: blood stains, cracks, tile variation, shadows cast by walls
- Wall-floor seam: thin **1px "baseboard" strip** in slightly darker floor color — fakes ambient occlusion at wall base

---

## Section 3: Layer Integration Techniques (Synthesis)

### Floor-Wall Transition (Top 3 Techniques)
1. **Wall overlap into floor** — wall sprite extends 8–16px below the floor plane. The bottom of the wall sprite contains a soft alpha shadow gradient bleeding onto the floor. No visible seam because the wall sits on top of floor.
2. **AO decal strip** — a semi-transparent darkened strip sprite placed at the wall base on the floor layer. Simulates shadow cast by wall onto floor. Enter the Gungeon does this with a 1px hard version; more painterly games use a 4–8px soft version.
3. **Large composite tiles** — instead of 32×32 modular tiles, use 128×256 hand-painted floor sections that include the wall base + floor blend in one asset (Hades model). No seam because the transition is inside the asset.

### Wall Segment Seam Hiding (Top 3 Techniques)
1. **Splattertiles / decal breaks** — alpha-blended debris sprites (pebbles, cracks, moss tufts) placed at the junction between wall segments. The organic shapes break the eye's ability to detect the grid seam. Source: Dan Cox GDC talk "What Modern Interior Design Teaches us about Environment Art."
2. **Texture variation per segment** — alternate 2–3 wall tile variants (brick arrangement offset, crack position shifted) so the repeating pattern is invisible. Min 3 variants needed for human eye to stop noticing repetition.
3. **Cap sprites / bridge sprites** — hand-painted single-segment connectors placed between tiles. These are 1/4 tile wide and contain the color blending that makes the joint look intentional.

### Character-Environment Integration (Top 3 Techniques)
1. **Contact shadow blob** — permanent dark ellipse under every character sprite (hard-edge, ~30–40% opacity). Grounds the sprite to the floor. Without this, sprites look like floating stickers.
2. **Color grade unification (LUT)** — screenshot → Photoshop edit → neutral LUT texture applied as a Global Volume post-process. Unifies the saturation and contrast of all assets so characters "belong" to the environment's palette.
3. **Rim lighting from environment** — a secondary 2D light positioned behind/above characters with the color of the dominant environment light source (e.g., warm torch orange). Makes character edges pick up the room's mood.

### Floor Density / Decoration (Top 3 Techniques)
1. **Painted floor base with built-in variation** — floor tile itself contains cracks, worn areas, subtle discoloration. No empty solid-color tile should exist in a shipped dungeon game.
2. **Edge shadow decals at all wall-adjacent tiles** — every floor tile touching a wall gets a shadow overlay (darkens 20–30%). Creates depth without adding props.
3. **Micro-detail scatter** — small debris (pebbles, dust, dried blood) at density ~3–5 per 128×128 area. These are 8–16px sprites on a dedicated decal layer. Moonlighter, Gungeon both use this density. Do NOT place in open walkways — cluster at walls and corners.

---

## Section 4: Post-Processing Recipe (URP 2D)

### Global Volume Settings
Based on Children of Morta (Unity URP confirmed), Enter the Gungeon (Unity), and general shipped-game synthesis:

| Effect | Setting | Notes |
|--------|---------|-------|
| **Bloom** | Threshold: 1.1–1.3 | Only HDR emissive sprites bloom (torches, VFX). Set sprite materials to HDR color > 1.0 intensity to opt in. |
| **Bloom Intensity** | 0.3–0.5 | Low. High bloom destroys pixel clarity. |
| **Bloom Scatter** | 0.5–0.7 | Low scatter = tight glow halo, no smear. |
| **Vignette** | 0.3–0.45, Rounded | Pulls eye to center. Avoid >0.5 (looks broken). |
| **Color Grading Mode** | High Definition Range | Required for LUT to work correctly. |
| **LUT Contribution** | 0.7–0.9 | Leave 10–30% of original to prevent over-processing. |
| **Tonemapping** | ACES | Prevents VFX from clipping to harsh white. Gives filmic rolloff. |
| **Shadows** | -0.05 to -0.1 | Slightly cooler/darker shadows in Lift/Gamma/Gain. |
| **Highlights** | +0.05 warm | Slight warm shift in highlights. |

### LUT Creation Workflow
1. Take a gameplay screenshot with all layers visible.
2. Open in Photoshop. Apply Curves / Hue-Saturation / Color Balance to get the desired mood.
3. Apply same adjustments to a Neutral LUT texture (download from Unity docs).
4. Export as PNG: no compression, clamp wrap mode, Point/Bilinear filter.
5. Assign in Color Lookup post-process volume.

### Light2D Settings (URP 2D Renderer)
- **Ambient light color:** dark teal or dark purple (not pure black) — 10–15% brightness. Pure black ambient = guaranteed "pasted on" look.
- **Point Light2D (torches/braziers):** Inner radius = 0.5–1.0, Outer radius = 3.0–5.0, Intensity = 0.8–1.2, Blend Style = "Additive."
- **Global Light2D:** Low intensity (0.1–0.2) with slight dungeon-mood color (desaturated blue). Acts as fill light so unlit areas aren't pure black.
- **Normal maps on floor tiles** (optional but high impact): even simple normal maps add perceived depth to painted floor tiles under directional Light2D.

### Children of Morta Upscaling Technique for RIMA
If PPU is 64 and sprites feel low-res for lighting: render sprites at 2x (128px equivalent) before light calculation. In Unity URP 2D, this can be approximated by setting the Pixel Perfect Camera's upscale render texture mode.

---

## Section 5: Anti-Patterns

1. **Texel density mismatch** — A 16px-per-tile character standing on a 128px painterly floor. One pixel in all assets must equal one consistent world unit. Counter-example: Hades forces all characters through a uniform sprite-render pipeline at fixed scale.

2. **Floating sprites (no contact shadow)** — Sprites without a drop shadow ellipse look like stickers pasted over a background. Counter-example: Death's Door, Gungeon — every entity has a blob shadow even in trailers.

3. **Identical wall tile repetition** — Using a single wall sprite stamped N times with no variant. The human eye detects period repetition immediately. Counter-example: Hades 1,400+ stitched tile variants. Minimum viable: 3 variants with offset UV.

4. **Pure black floor / ambient** — Empty floor with no tile or a black background reads as "not finished." Counter-example: Children of Morta floor tiles contain painted surface variation baked in. No shipped game uses solid black floor.

5. **Outline inconsistency** — Characters with thick black comic outlines on an outline-free painted environment (or vice versa). Counter-example: Hades characters + environment both use the same "Black Shading" (Mignola style) so outlines read as part of the world.

6. **No palette glue (missing LUT)** — Assets generated from different sources (PixelLab characters + separately generated tiles + Unity default sprite) each have different white points, saturation, and hue. Without a unifying LUT or Color Grade volume, they visually belong to different games.

7. **Wall sprites not overlapping floor** — Walls placed at the same Z/Y as floor edges, creating a visible hard seam. Fix: wall bottom extends 8–16px into the floor tile (wall's sorting order higher than floor's sorting order).

8. **Spotlight on character, darkness on environment** — Character lit brightly with a point light but environment only lit by ambient. The character "pops out" into the wrong direction. Fix: environment and character should share the same light sources.

---

## Section 6: Top 3 Actionable Changes for RIMA Spawn_01

These address the stated problems (5 identical wall segments reading as doorways, black floor, floating props) without requiring asset regen.

### Priority 1: Add Global Light2D fill + ambient adjustment (1 hour, no regen)
**Problem solved:** Black floor, floating sprites.  
**Action:** In RoomPipelineTest scene, add a Global Light2D component with Intensity 0.15–0.20 and color `#1a1a2e` (dark navy). This ensures no area is pure black. Add a Freeform Light2D or Point Light2D at the room center (brazier position) with Outer Radius 6, Intensity 0.7, Additive blend. Immediately the floor and props stop reading as "pasted on black."

### Priority 2: Add contact shadow blob under every prop + character (2 hours, no regen)
**Problem solved:** Floating/pasted look of brazier, character, small props.  
**Action:** Create a reusable `ContactShadow` prefab: a 64×32 ellipse sprite (dark, ~35% opacity, soft edge). Parent it to each character/prop at Y offset -0.1. Use sorting layer `Floor` + Order +1 to ensure it renders above floor but below the prop. This single change is the highest ROI integration fix.

### Priority 3: Add wall-base AO decal strip + 1-pixel baseboard tint (3 hours, 1 new sprite)
**Problem solved:** Wall seam visible, 5 wall segments reading as 5 separate doorways.  
**Action:**  
(a) Create a single 64×8px semi-transparent dark strip sprite (shadow gradient, transparent top to 40% black bottom). Stamp it at the base of every wall segment on a decal layer between floor and walls. This hides the floor-wall seam.  
(b) Add 3–5 small debris sprites (8×8 pebbles/dust from existing scatter pool) clustered at wall corners — this breaks the visual rhythm that makes 5 identical wall tiles read as "5 doorways."  
(c) Shift all wall sprites' pivot/position to overlap floor by 8px (wall extends into floor plane). Change wall sorting order to be above floor tiles.

---

## Confidence Assessment

**Section 1 (Hades):** MEDIUM-HIGH. Core pipeline details (ZBrush chars, 3D mesh envs, DAG sorting, 1400 tiles) are well-documented from GDC and developer interviews. Specific post-FX settings not confirmed (custom engine, not Unity).

**Section 2 (Children of Morta):** MEDIUM. Upscaling/lighting technique is confirmed from the Gamasutra postmortem. Specific Unity settings not published.

**Sections 3–5:** MEDIUM. Synthesis from Gemini search grounding across multiple dev blogs and GDC talks. Dan Cox GDC talk cited for splattertile technique. Individual Unity setting numbers are industry-standard defaults, not published by specific studios.

**Section 6:** HIGH confidence these are the correct priorities for the stated problems — based on the anti-patterns identified and our known stack (URP 2D, Light2D, PPU 64).

**GAPS:** No confirmed LUT texture settings from a shipped pixel-art dungeon Unity game. No Supergiant Unity-specific post-FX data (they use custom engine). Death's Door / Tunic are 3D engines. ScourgeBringer Unity-specific pipeline not found.

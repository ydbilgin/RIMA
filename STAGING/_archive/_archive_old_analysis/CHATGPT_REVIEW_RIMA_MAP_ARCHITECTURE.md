# RIMA Map Architecture — Verification Request for ChatGPT

I'm building a 2D roguelite called **RIMA** and need you to verify that my map/asset production architecture is sound. Below is the full plan as I (an AI agent) currently understand it. Please review and flag any flaws, missing considerations, or alignment issues with industry best practices.

---

## 1. Game Type & References

- **Genre:** Top-down action roguelite (combat-driven, Hades-inspired room loop)
- **Engine:** Unity 6 URP 2D Renderer + Pixel Perfect Camera + 2D Lights
- **Visual style references:**
  - **Alabaster Dawn** (Radical Fish Games, 2026) — 32-bit pixel art ARPG, top-down 2.5D
  - **CrossCode** — same studio's pixel art with atmospheric depth
  - **Hades** (Supergiant) — atmospheric arena room design (NOT pixel art, but room layout reference)
  - **Hyper Light Drifter / Eastward / Death's Door** — modern indie pixel art tradition

**Target final feel:** Alabaster Dawn / CrossCode pixel art combat with Hades atmospheric room design + ritual/dark fantasy mood.

---

## 2. Perspective System

- **Camera:** Orthographic (no perspective distortion in Unity)
- **Grid:** Rectangular (NOT isometric diamond)
- **Visual angle:** **High top-down 30-35° tilt** — "fake isometric"
- **NOT pure top-down (90°):** would look flat overhead
- **NOT true isometric:** diamond grid, 26.57° projection — not us

**The 3D-feel comes from how each individual sprite is DRAWN:**
- Floor tiles: asymmetric perspective baked in (back-edge darker/compressed, front-edge brighter/expanded — natural ground recession)
- Walls: visible 3D height (top cap surface visible at 35° + wall body face below)
- Props: top + slight front face visible
- Characters: chibi proportions, drawn from 35° view

**Locked design decisions:**
- **PPU = 64** (1 Unity unit = 64 pixels)
- **Character size: 64×64 chibi pixel art** (~3-4 head proportions)
- **Anim FPS: 10-12**
- **Direction count: 4 cardinal (S/N/E/W); W = flipX of E**

---

## 3. Room Design — Two Types (story-driven)

The game has **two distinct room visual types**, used per story/lore context. Most rooms = Type B (Hades arena), some specific story beats = Type A (enclosed dungeon).

### Type A — Dungeon Enclosed (rare, story-specific)
- Walls **close to the player**, oppressive, traditional dungeon feel
- **Wang16 tile system** for room perimeter walls (auto-tiled corner encoding)
- All elements within gameplay-near plane
- Style: pixel art discipline (Alabaster Dawn aesthetic)

### Type B — Hades Arena (default, most common)
- Walls **distant, atmospheric** — visual context only, not gameplay
- Wide open floor space (combat arena)
- Background room walls = large painted atmospheric pieces (NOT tile-grid)
- Wang16 ONLY for in-room features (cliff edges, raised platforms, water borders)
- Style: foreground pixel art + background painted atmospheric

---

## 4. Asset Tier Strategy — Two Tiers

### TIER A — Foreground (gameplay layer) — NATIVE PIXEL ART

Hard pixel discipline, matches character chibi aesthetic. Used in BOTH room types.

| Asset | Native Size | Generation Tool | Why this size |
|---|---|---|---|
| L2 Floor tiles | **64×64** | PixelLab `create_tiles_pro` square_topdown | 1 Unity unit = 64px, matches character scale |
| L3 Wang16 walls (Type A) / Feature edges (Type B cliff/platform) | **32×32 native** (Wang max in PixelLab) or **64×96** for tall walls | PixelLab `create_topdown_tileset` (Wang autotile) | Auto-edge matching, dual-use per room type |
| L4 Decal overlays (moss/dirt patches) | **64×64** transparent | PixelLab `create_object` n_frames=16 | Blend on top of floor tiles |
| L5 Detail scatter (pebbles/cracks/bone shards) | **32×32** transparent | PixelLab `create_object` n_frames=16 | Small organic noise |
| L6 Accent overlays (rift scar/battle splatter) | **128×128** transparent | PixelLab `create_object` n_frames=16 | Central feature pieces |
| Props (crate/urn/candle/brazier/etc.) | **64×64** (tall props 64×128 Custom Size Beta) | PixelLab `create_object` **n_frames=64** | Max variant pool (64 candidates → pick 8-12 per type) |
| Character anchors (10 classes) | **64×64** | PixelLab Create Image Pro Web UI (manual production) | Hand-curated for class identity |

**All Tier A generation = NATIVE pixel size, no downsample.** PixelLab is pixel-art-trained, produces clean pixel art at requested size.

### TIER B — Background atmospheric (Type B Hades rooms only) — LARGE PAINTED

Painterly Hades-tradition large pieces, distant from gameplay plane. Used ONLY for Type B room backgrounds.

| Asset | Native Size | Generation Tool | Why this size |
|---|---|---|---|
| Room perimeter wall (atmospheric distant) | **512×512** (or 1024 if model supports) | PixelLab Create Image Pro Web UI (256-512 size table) | Large painted background, needs detail to read at distance |
| Background prop silhouettes (distant statue/arch) | **256×512** | Create Image Pro | Atmospheric depth |
| Distant doorways/archways | **256×512** | Create Image Pro | Context backdrop |
| Atmospheric lighting overlays | **256-512** | Create Image Pro | Mood/glow gradients |

**Tier B generation reasoning:** These elements are seen at distance (background plane), need atmospheric detail captured at large pixel real estate. 64-native would be blurry/insufficient for distant atmospheric reading.

**Style separation:** Tier A (foreground pixel discipline) + Tier B (background painterly) coexist because they're in different visual planes. Hades pattern: hand-painted backgrounds + interactive foreground elements.

---

## 5. Color Palette — Vivid Vulnerability + Ritual Catastrophe

**Dominant tones:**
- Dark slate gray `#3a3530`
- Deep brown `#4a3a30`
- Dusty blue `#5a6a78`

**Accent tones:**
- Faint dark red rift `#8a3030`
- Warm orange fire `#c87830`
- Deep moss green `#3a5a3a`
- Cold blue rim highlight `#8aa8c0`

**Mood:** Ancient ritual temple, weathered, hollow watchful, post-battle aftermath. Used in both Tier A and Tier B for visual harmony.

---

## 6. Map Designer 6-Layer Pipeline (Karar #143-E)

Brush V1 (existing Unity editor tool) paints rooms layer-by-layer:

| Layer | Content | Asset Tier | Status |
|---|---|---|---|
| L1 Base Tone | RGB tonal fill (ambient color wash) | none (just color) | Live in Brush V1 |
| L2 Floor Atlas | Floor tile sprites | TIER A 64×64 | Production pending |
| L3 Wang16 | Type A: room walls / Type B: feature edges | TIER A 32×32 native | Production pending |
| L4 Organic | Patch decals (moss/dirt) | TIER A 64×64 | Brush def live, sprites pending |
| L5 Detail | Small scatter (cracks/pebbles) | TIER A 32×32 | Brush def live, sprites pending |
| L6 Accent | Large overlay accents (rift) | TIER A 128×128 | Brush def live, sprites pending |
| **TIER B background** (Type B only) | Distant painted walls | TIER B 256-512 | Separate from Brush V1 6-layer, manual placement via RoomTemplate field |
| Props (separate system) | Free-standing objects | TIER A 64×64 | PropDefinitionSO system live |

**Brush V1 painters:**
- L4/L5/L6 use procedural patch placement with rules: density, min-distance, edge-bias, wall-proximity, allowFlipX/Y, sortingOrderRange
- L3 Wang16 uses 4-bit corner encoding (NE/NW/SE/SW = 2^4 = 16 cases)
- Walkable filter on all painters (no patch on wall cells)

---

## 7. Production Tool Decisions

### PixelLab (vendor-trained pixel art) — TIER A bulk

**Available tools:**
- `create_tiles_pro` — numbered prompt, batch 8-variant per call, sizes 16-128px, supports square_topdown / hex / isometric
- `create_topdown_tileset` — Wang16 native auto-tile, 16 or 32px native only
- `create_object` — 1-dir + n_frames in [1, 4, 16, **64**], sizes 32-256px, transparent BG, review pipeline (pick best from variant pool)
- `create_image_pro` (Web UI) — single image, 64-512px native, full prompt control, used for hero/atmospheric assets
- `create_character` — BANNED for autonomous use (high credit cost, user runs manually via Web UI)

**Production strategy:**
- L2 Floor: 1 mega `create_tiles_pro` call with 8 numbered prompts → 8 floor variants at 64×64
- L3 Wang16: 1 mega `create_topdown_tileset` call → 16 wall + 7 transitions at 32×32 native
- L4-L6: per-type `create_object` n_frames=16 calls → pick best 4-5 per type from 16 variant pool
- Props: per-type `create_object` **n_frames=64** calls → pick best 8-12 per type from 64 variant pool (massive variant explosion)
- Total Tier A: ~32 PixelLab calls, ~100-150 credit consumption (out of 5000 budget)

### Create Image Pro Web UI — TIER B atmospheric

Used for large painted backgrounds (Tier B). Manual production by user (not MCP autonomous). 256-512 native size, painterly Hades-tradition style.

**ChatGPT's 256/512 recommendation was for this tier** — large atmospheric pieces where detail at distance matters.

### Codex CLI imagegen (gpt-image-1) — REJECTED for production

Tested earlier. General-purpose model, painterly drift, NOT pixel-art-native. Outputs look beautiful as concept art but break pixel discipline. Useful only for concept reference, not production assets.

### Custom LoRA training (planned for tonight)

- ComfyUI + SDXL base 1.0 + nerijs Pixel Art XL LoRA (off-the-shelf, free)
- Custom LoRA training overnight via OneTrainer (PyTorch 2.9.1+cu128, RTX 5080 Blackwell)
- Training data: 2100+ refs collected from 3 parallel agents (Codex 214 / Opus 1902 / user's Antigravity task)
- Phase 1: Test base nerijs LoRA → if produces RIMA-quality tiles, custom training optional
- Phase 2: Custom RIMA-style LoRA → guaranteed style match, future content production unlimited

---

## 8. Unity Brush V1 Integration

**Existing system (already live):**
- `BrushAtlasImporter` scans `Assets/Sprites/Environment/<biome>/<layer>/` folders
- Auto-imports as Sprite + Tile assets with PPU=64, FilterMode=Point, alpha=true
- Wang16 auto-detection from filename pattern (`wang_XXXX` 4-bit)
- `PatchAtlasSO` for L4/L5/L6 decals with density/distance rules (Karar #143-M..R)
- `PropDefinitionSO` for prop library with footprint config
- `RoomTemplateSO` for room layouts with biome reference

**Proposed additions for two-room-type architecture:**
```csharp
public enum RoomVisualType { DungeonEnclosed_TypeA, HadesArena_TypeB }

[Field] public RoomVisualType visualType;
[Field, ShowIfTypeB] public Sprite tierBBackgroundWall;       // 512×512 painted
[Field] public Wang16AtlasSO wang16Atlas;                     // Type A: room walls / Type B: feature edges
[Field] public BiomeId biome;                                 // determines color palette + atmosphere
```

**Compose flow per room:**
- Type A: L1 base tone → L2 floor tilemap → L3 Wang16 walls → L4/L5/L6 procedural patches → Props placement
- Type B: L1 base tone → **Tier B background wall sprite (sortingOrder -10)** → L2 floor tilemap → L3 Wang16 features only (NOT full walls) → L4/L5/L6 patches → Props placement → URP 2D Lights for atmosphere

**Sort order layers:**
```
-10: Tier B background painted walls (Type B only)
  0: L2 Floor tilemap
  2: L4 Organic decals
  3: L5 Detail scatter
  4: L3 Wang16 features (Type B) or walls (Type A)
  5: L6 Accent overlays
  6: Props
  7: Characters + mobs
  8: Post-process (bloom, vignette, color grading)
```

---

## 9. Concrete Questions for ChatGPT

1. **Is the Tier A / Tier B separation sound?** Are there better ways to mix native pixel art foreground with painted atmospheric background while preserving visual coherence?

2. **Tier B size — 256 vs 512?** Your earlier recommendation was 256/512. For distant atmospheric background walls in a 16:9 1080p game, what's the optimal native generation size? Should we go even larger (1024) if PixelLab Web UI supports it?

3. **Wang16 dual-use viable?** Using Wang16 system for Type A room walls AND Type B feature edges (cliff/platform) — is this clean architecture, or should they be separate atlases?

4. **Two room types (A vs B) — story-driven assignment.** Is there a better way to handle "most rooms Hades-style but some traditional dungeon" without two separate systems? Or is the dual-system approach correct?

5. **Props variant strategy — n_frames=64 worth it vs n_frames=16?** PixelLab `create_object` supports 64 variants per call. We're planning to use 64 for max prop pool variety. Is this overkill, or industry standard for AAA roguelite prop variety?

6. **Composition layering — sufficient for "natural look"?** With 6-layer pipeline + Tier B background + post-process URP, will the assembled room read as "natural / hand-crafted" rather than "AI-generated tile grid"? What additional layers/techniques might help?

7. **Style consistency risk — Tier A pixel + Tier B painted in same scene.** Is the visual harmony risk real? How do games like Hades / Octopath Traveler handle this layer separation successfully?

8. **LoRA training for custom RIMA aesthetic.** Given we have 2100+ pixel art tile references (mixed sources: Kenney CC0, DCSS, DawnLike, LPC, OGA artists), is overnight SDXL LoRA training the right move, or should we stick with PixelLab vendor-trained model and not introduce a second style anchor?

9. **What am I missing?** Any architectural gaps, anti-patterns, or industry best practices we should integrate?

---

## 10. Production Sequence (proposed)

1. **Tonight (next 30-60 min):**
   - PixelLab Tier A bulk: L2 Floor (1 call) + L3 Wang16 (1 call) + 4-6 critical prop types n=64 each
   - Quick Unity import + Brush V1 sample compose for Type A test

2. **Tonight (later):**
   - Tier B background wall pieces via Create Image Pro 256-512 (3-5 atmospheric variants: cave / temple / cult chamber / outdoor courtyard)
   - Unity import as separate sprite, manual RoomTemplate field

3. **Overnight:**
   - Custom LoRA training (if Phase 1 nerijs LoRA insufficient)
   - 3-5 hours on RTX 5080

4. **Tomorrow:**
   - Sample room compose Type A (Wang16 walls) and Type B (Tier B background + features)
   - Empirical visual test: character + props + atmosphere = natural / sıritma?
   - Iterate based on result

---

**Please review and tell me:**
- What's solid in this architecture?
- What's flawed or risky?
- What am I missing?
- Your refined size recommendations for Tier B (256 vs 512 vs 1024)?
- Any specific tools/techniques I should add (e.g., 2D lights, particle effects, post-process volumes)?

Thank you.

# RIMA Map Composer — Multi-Projection Natural Map System (Overnight Architecture)

## User vision (verbatim)

> "Ben doğal görünümlü map istiyorum bütün oyunlarım için kullanabileceğim şekilde isometric top down low top down 3d gibi ama asıl sanatı 2d olarak her türlü"

Translation: I want a natural-looking map system I can use across all my games — supporting isometric, top-down, low top-down, and 3D-like projections — but the actual art is always 2D in every form.

## Strategic scope

This is **NOT** a RIMA-only decision. It's an architectural decision about a **reusable Map Composer system** that:

1. Always uses 2D art (PixelLab pixel art + Codex imagegen painterly textures + future LoRA)
2. Supports **4 camera projections** as swappable renderers:
   - **Top-down** (90° camera, flat overhead — classic JRPG / Stardew Valley)
   - **Low top-down** (30-35° angled — current RIMA target, Diablo 2 / Hades style)
   - **Isometric** (diamond grid, 30° axonometric — Diablo 1, Disco Elysium, Tactics Ogre)
   - **HD-2D / pseudo-3D** (3D mesh terrain + 2D sprite quads — Octopath Traveler, Sea of Stars, Amine Rehioui's PowerPlay)
3. Same `RoomData` source of truth across all 4 projections
4. Natural look guaranteed in all projections (no visible grid)
5. Reusable across RIMA + future games (different genres, different camera, same composer engine)

## Concrete reference games per projection

User cited these as examples of "natural-looking" aesthetic we want to match:

- **Top-down:** Stardew Valley (cozy painterly), classic JRPG
- **Low top-down (current RIMA):** Hades (atmospheric arena), Diablo 2 (dense combat), CrossCode (pixel-art angled)
- **Isometric:** **Little Rocket Lab** (2025 Oct release, Steam 90% positive, isometric pixel art cozy automation + life-sim, Factorio + Stardew hybrid — user-cited reference for "natural look in iso projection"), Disco Elysium, Tactics Ogre
- **HD-2D / pseudo-3D:** Amine Rehioui's PowerPlay (in this briefing's source tweet), Octopath Traveler, Sea of Stars

**Critical question for Isometric path:** Little Rocket Lab is cozy automation/life-sim, NOT action roguelite. RIMA needs dense combat (dash, projectiles, skills, 8-dir attacks). Historical pattern:
- Diablo 1 was iso → Diablo 2 switched to angled top-down for combat density
- Hades, Enter the Gungeon, Hollow Knight Silksong-tier action all use angled top-down or pure top-down, NOT iso
- Iso adds projection transform cost to hitbox/movement code, plus 8-dir sprite reauthoring (iso axes differ from cardinal+intercardinal)

So the question for isometric is **TWO-PART**:
1. Can we achieve Little Rocket Lab natural look in our composer? (probably yes)
2. Can we ship RIMA's combat feel under iso projection? (historically harder than angled top-down)

Both agents should evaluate (1) and (2) separately. If (1) yes but (2) no, the iso path is great for FUTURE non-combat games but not RIMA itself.

## What "natural look" means

User has been fighting "tile grid visible" for many sessions. Reference: Alabaster Dawn / CrossCode / Hades / Octopath Traveler / Amine PowerPlay. Common visual qualities:

- Continuous floor surface (no per-tile borders accumulate into grid lines)
- Organic decoration scatter (decals/scatter break uniform tile pattern)
- Biome edge bleeding (sand → grass smooth, no hard line)
- Cliff/elevation real depth (not stacked flat layers)
- Lighting integration (shadows ground objects to surface)
- 2D pixel art preserved (not photorealistic, not cartoon)

Amine's PowerPlay video (`STAGING/X.mp4`, frames at `STAGING/X_frames/`) demonstrates this in 3D-mesh + 2D-sprite-quad hybrid. We need to determine if RIMA's 2D Tilemap + layered composition can achieve same quality OR if HD-2D pivot is required, AND if either approach can generalize to other projections.

## The reusable-engine question

User wants this composer to work for:
- **RIMA** (2D low top-down ARPG roguelite — current)
- **Future game** (unknown genre, could be isometric strategy, top-down survival, HD-2D RPG, etc.)

This means the composer must be **decoupled** from RIMA-specific assumptions:
- Camera projection plug-in (TopDownRenderer / LowTopDownRenderer / IsometricRenderer / HD2DRenderer)
- Asset pipeline projection-agnostic (PixelLab sprites valid in all projections with sprite variants where needed)
- Brush authoring projection-agnostic (paint in cell space, renderer handles visual)
- Data layer fully renderer-agnostic (current `RoomData` + Phase 1A SOs already meet this)

## What's already in place (assets to preserve)

**Locked from previous sessions (DO NOT BREAK):**
- PPU=32, 32×32 cell, 64×64 char/prop (in 3D, cell = 1 unit, sprites scale to native rect)
- Wang16 = elevation/feature edges ONLY (in 3D becomes mesh height transitions, in iso becomes corner masks, in top-down stays as Wang)
- Karar #143 layered pipeline (L0-L11) — universal across projections
- Renderer-agnostic SO contracts (Phase 1A SOs landed 2026-05-17: TerrainDefinitionSO, PatchAtlasSO, PropDefinitionSO, RoomVisualProfileSO, ImportAssetRole)
- Codex imagegen working (verified at `STAGING/Phase1A_L2b_Source/codex_floor_source_v1.png`)
- Brush V1 ship-ready (13/13 sprints + 328/328 EditMode PASS)
- PixelLab pixel art pipeline (8-dir characters, 32px tiles, 64px props)

**Just resolved (don't re-debate):**
- Data-first decal pattern (Opus + Codex tweet review verdict: `RoomDecalDataSO` + chunked renderer, see `STAGING/CODEX_TWEET_REVIEW_xhigh.md` + Opus message in conversation log)
- Camera angle for RIMA = 30-35° low top-down (LOCK from S86)

**Open question (this overnight task):**
- Should RIMA migrate to HD-2D for natural look guaranteed?
- Can the composer engine support all 4 projections simultaneously?
- What does the minimum prototype look like to validate?

## What I want from you (Codex xhigh / Opus)

Both agents produce independent comprehensive architecture documents. Outputs:
- Codex: `STAGING/CODEX_OVERNIGHT_multi_projection.md` (~5000 words)
- Opus: returns as final assistant message (rima-design contract forbids file writes)

### Required sections (both agents)

#### 1. Multi-projection renderer abstraction
- Define the `IRoomRenderer` (or equivalent) interface that all 4 projection renderers implement
- What data does it consume from `RoomData`?
- What does it produce (Mesh? batched sprite layer? Tilemap fill?)?
- Concrete class skeletons for each of 4 renderers (~50-100 lines each):
  - `TopDownRenderer`
  - `LowTopDownRenderer`
  - `IsometricRenderer`
  - `HD2DRenderer`

#### 2. Asset pipeline per projection
- Same PixelLab sprite (e.g., 64×64 character) — does it work in all 4 projections?
  - Top-down: needs 1 sprite (or 4-dir for facing)
  - Low top-down: needs 8-dir (already planned)
  - Isometric: needs 8-dir aligned to iso axes (different from low top-down 8-dir!)
  - HD-2D: needs Y-billboard or fixed-plane quad, 8-dir like low top-down
- Codex imagegen painterly texture — works in all 4?
  - Top-down: tile as Tilemap or as splat texture on flat plane mesh
  - Low top-down: tile as Tilemap OR splat texture on tilted plane
  - Isometric: tile as diamond Tilemap OR splat on iso plane mesh
  - HD-2D: splat texture on 3D mesh with elevation
- Sprite atlas requirements per projection
- Camera-specific sprite import settings (PPU, filter mode, alignment)

#### 3. Brush authoring projection-agnostic
- User paints "moss" — same brush stroke produces correct visual in all 4 projections?
- Cell-space vs world-space paint mask
- How do walls (L3) work in iso vs top-down vs HD-2D?
- How do props (L7) work in iso vs HD-2D (3D position vs 2D layered)?

#### 4. Lighting per projection
- URP 2D Lights work for top-down, low top-down (current)
- Isometric: URP 2D works but shadow direction conventions differ
- HD-2D: URP 3D Lights (point/spot/directional) — different shader stack
- Can `LightingProfileSO` (Phase 1A planned) carry across all 4? Or per-projection variants?

#### 5. Wang16 across projections
- Top-down: corner mask works as-is
- Low top-down: corner mask works as-is (current LOCK)
- Isometric: corner mask still works but tile placement is diamond not square — different slicing
- HD-2D: corner mask determines mesh height transition (not visual sprite swap)

#### 6. Recommended implementation order
- Phase 1A current trajectory (low top-down 2D) — continue as base case validation
- Phase 1B: add second projection support (which one first? top-down probably simplest)
- Phase 1C: isometric — requires sprite reauthoring (8-dir iso axes)
- Phase 2: HD-2D — biggest architectural lift but probably highest visual ceiling
- OR: vertical slice of ALL 4 projections via shared minimal renderer interface first, then deepen one at a time

#### 7. Reusability across games
- Map Composer as standalone package: folder structure, asmdef, dependency graph
- What's RIMA-specific (stays in `RIMA.*` namespace) vs what's generic (goes into `MapComposer.*` namespace)
- Versioning / upgrade path

#### 8. Risk + tradeoff matrix
- Per projection: implementation risk, asset re-authoring risk, performance risk, art quality ceiling
- Per shared component: what breaks if we change it later

#### 9. Concrete prototype scope for validation
- Specify EXACT minimum demo that proves multi-projection works
- E.g., "1 room data file, 1 base floor texture, 5 prop sprites, 1 character, 4 scene files (one per projection), all reading same RoomData, all producing recognizable natural-looking output"

#### 10. Final 1-paragraph verdict
- Should RIMA commit to multi-projection now or after V1 ships?
- If now: what's the exact 1-week prototype scope?
- If after V1: what minimum architectural rules during V1 keep multi-projection feasible?

### Constraints (both agents)

- Output decision-quality, not survey-style
- Cite specific files/lines for code claims
- Use ffprobe / frame analysis if validating Amine claims
- Do NOT recommend DOTS/ECS migration
- Do NOT recommend abandoning PixelLab pixel art
- Do NOT recommend baking everything into Unity Tilemap (3D portability dies)
- 5000 words max (Codex), 3000 words max (Opus)

### Schedule

Both run as long-form async tasks. Codex via cx_dispatch xhigh effort, timeout 2400s. Opus via Agent rima-design background. Outputs ready by morning.

### Tomorrow morning

User wakes up to:
- `STAGING/CODEX_TWEET_REVIEW_xhigh.md` (already landed, narrow 2D vs HD-2D)
- `STAGING/CODEX_OVERNIGHT_multi_projection.md` (this dispatch, broad multi-projection)
- Opus's overnight verdict in conversation history

Orchestrator (Claude) will synthesize all three into a single decision matrix + Phase 1B+ implementation plan.

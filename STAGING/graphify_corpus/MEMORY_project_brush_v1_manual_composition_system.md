---
name: brush-v1-manual-composition-system
description: "S88 LIVE — Brush V1 chunked pipeline + Manual MCP Composition flow for Alabaster Dawn/Hades natural map. Pipeline: Codex imagegen → Python slicer → Unity import → PatchAtlasSO → PlayableRoom (manual SpriteRenderer scatter)."
metadata: 
  node_type: memory
  type: project
  originSessionId: 9a40fdbb-d6ff-4a63-beb2-5f0b66522e04
---

# Brush V1 + Manual MCP Composition System (S88 LIVE)

**Status:** LIVE 2026-05-18 (`Assets/Scenes/Demo/RoomPipelineTest.unity` PlayableRoom prefab) — open-world 36×22 floor + Warblade character + WASD-ready. Visual baseline reaches Alabaster Dawn/Hades-tarzı kompozisyon.

**Why this exists:** Brush V1's automated pipeline (`BrushExecutorRouter` + 14 executors + `RoomDecalChunkRenderer`) is full-featured but heavy for fast iteration. Manual composition via Unity-MCP `execute_code` lets the orchestrator place sprites directly while reusing the same `PatchAtlasSO` assets, bypassing the chunked renderer's material/UV bugs and giving us instant Game view feedback.

## Hybrid asset pipeline (Codex imagegen → Unity)

| Step | Tool | Output |
|---|---|---|
| 1. Generate sheets | Codex imagegen (gpt-image-1) | `STAGING/RIMA_AssetParts_v2/sheet_{01..08}_*.png` (8 sheets at 1024×1024) |
| 2. Alpha-clamp + edge-desaturate | Python (PIL+numpy) | Same paths, RGBA cleaned + backup at `_pre_alpha_fix_backup/` |
| 3. Slice + downsample NN | Python | `STAGING/RIMA_AssetParts_v2/sliced/{floor,macro,moss,...}/` (84 PNG parts) |
| 4. Unity import | Codex `manage_asset` dispatch | `Assets/Sprites/Environment/RIMA_AssetParts_v2/` (PPU=32, Point, alphaIsTransparency, spriteExtrude=1) |
| 5. SO creation | Codex dispatch | 7 `PatchAtlasSO` + 1 `SpriteAtlas` at `Assets/Data/Brush/AssetParts_v2/` |
| 6. Manual scene composition | `mcp__unityMCP__execute_code` (C# method body) | PlayableRoom hierarchy directly populated |

## PlayableRoom hierarchy (manual composition target)

```
PlayableRoom
├── Floor (36×22 = 792 SpriteRenderer GameObjects, BaseFloor variants 0..15)
├── Decoration
│   ├── 01_Macro      (30 large patches, scale 1.0-1.8x, alpha 0.7-1.0, BaseFloor variants 16+)
│   ├── 02_Decals     (60 Moss + 40 Dirt, scale 0.5-1.1x)
│   ├── 03_Scatter    (80 Pebble + 40 Crack, scale 0.4-0.8x)
│   ├── 04_Accents    (4 Rift + 4 Ritual, scale 0.4-0.7x, sortingOrder 4 focal beats)
│   └── 05_EdgeFill   (corner density boost — 20 sprites at 4 corners)
├── Walls (hidden when "open world" mode; otherwise tiled dark-stone procedural sprite + BoxCollider2D)
├── DoorExit (BoxCollider2D trigger + TestDoorExit.cs, hidden in open-world mode)
└── Player
    ├── SpriteRenderer (Warblade 01_warblade.png anchor or white-circle procedural)
    ├── Rigidbody2D (gravityScale=0, FreezeRotation, Continuous)
    ├── CircleCollider2D (radius 0.35)
    ├── TestPlayerMovement.cs (WASD via Rigidbody2D.MovePosition)
    └── PlayerGlow (Light2D point, warm color, radius 4)

Main Camera (orthographic, ortho size 5-6) + TestCameraFollow.cs (Lerp 0.12-0.15)
```

## Test scripts (runtime, `Assets/Scripts/Test/`)

- `TestPlayerMovement.cs` — Rigidbody2D + MovePosition, 5 unit/sec
- `TestCameraFollow.cs` — Lerp to target.position + offset (0,0,-10)
- `TestDoorExit.cs` — OnTriggerEnter2D logs "Player exited the room!"

## Lessons learned (S88 LATE)

1. **Scene view editor grid misled visual gate diagnosis** for 2 dispatch rounds. The "kare kare" complaint was 50% Unity Scene view grid + 50% Sheet 01 Y-channel source-art. **Always test Game view** (`capture_source=game_view`), not Scene view.

2. **Decoration mass effectively masks tile grid** — adding 30 macro patches + 100 decals + 120 scatter + 8 focal accents reduces visible tile boundaries to acceptable level WITHOUT regenerating Sheet 01. Codex consultation confirmed: "Pivot from 32×32 floor as hero → use macro patches dense + sparse decals".

3. **`RoomDecalChunkRenderer` had alpha bug** (opaque black rectangles): fixed by changing material to `URP/Unlit` transparent with proper blend mode (`_Surface=1`, `_SrcBlend=SrcAlpha`, etc.). See `Assets/Scripts/MapDesigner/Brush/Runtime/RoomDecalChunkRenderer.cs`.

4. **Camera follow doesn't update in Editor mode** — when manually repositioning Player via execute_code, must also manually move Camera. LateUpdate only runs in Play mode.

5. **Codex consultation is faster than implementation when stuck** — read-only architect dispatch (`STAGING/CODEX_CONSULTATION_NATURAL_MAP_APPROACH.md`) returned correct verdict in 5 minutes vs 30-min implementation dispatches that produced PARTIAL/FAIL.

## Visual gate criteria (Game view ONLY)

- ✅ PASS: composition reads as natural roguelite map (Hades/Alabaster Dawn tone), no dominant per-tile grid, no opaque artifacts, decoration density obscures floor boundaries, focal accents (rift/ritual) feel rare-intentional
- ❌ FAIL: per-tile grid dominant, opaque black quads, decals floating misaligned, color cast on transparent edges

## PixelLab production workflow (yarın 5000 gen geldiğinde — 2026-05-19)

**Hybrid pipeline locked** (`[[hybrid-asset-pipeline-lock]]` Karar #157 candidate):

- **PixelLab** → characters / mobs / props / wall variants (organic detailed art, 16-variant pick or 256→64 downsample for organic complex)
- **Codex imagegen** → tiles / Wang16 walls / decals / accents / biome floors (bulk grid sheets at gpt-image-1)

### PixelLab usage pattern (next session)

1. **Verify gen budget** via `STAGING/cx_limits.py` analog or PixelLab web UI (5000 gens expected)
2. **Check pending review** via `mcp__pixellab__list_objects` (claim any leftover from prior sessions — free gens)
3. **Production targets in order:**
   - 4 P0 props from `STAGING/RIMA_MAP_PRODUCTION_SEQUENCE.md` (Crate, Urn, Candle, Debris Pile, each `create_object` directions=1 n_frames=16 size=64 view=high top-down)
   - 8 P1 props (Column intact, Column broken, Brazier, Banner torn, etc. — some 64×128 Custom Beta)
   - 256→64 special: Treasure Pile, Kneeling Statue (high-res organic detail)
4. **`mcp__pixellab__select_object_frames`** picks best 1-2 of 16 candidates
5. **Download via curl** to `STAGING/RIMA_AssetParts_v2/pixellab_props/`
6. **Codex dispatch** for Unity import + PatchAtlasSO extension

### PixelLab MCP rules ([[pixellab-mcp]])

- `mcp__pixellab__create_character` **BANNED autonomous** — user does it via Web UI V3
- `mcp__pixellab__create_object` **autonomous OK** for props/decoration
- `n_frames=16` charges 16 gens (NOT 1) — use `n_frames=1` for production runs to conserve budget; use `n_frames=16` only for hero pieces where variety matters
- Existing review-status objects = free gens already spent — always claim or dismiss

### PixelLab prompts (production guide v6 — `STAGING/RIMA_MAP_PRODUCTION_SEQUENCE.md`)

Reference prompt structure for any prop:
```
ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective like Hades and Hyper Light Drifter. [prop-specific viewing angle description]. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE COMPOSITION: transparent background, 1 prop only, full object visible, centered, clean transparent padding around the silhouette, no border or frame.

ABSOLUTE STYLE: Pixel art prop, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading max 2 tones per color. Hades + Salt-and-Sanctuary dark gritty mood. Soft oval ground shadow beneath the prop.

[Prop content description with palette and material]

1 prop only, single isolated full object, centered on transparent background, no characters, no creatures, no weapons, no duplicate copies.

Negative Prompt :
characters, humans, creatures, animals, weapons, side view, side profile, three-quarter view, isometric projection, flat front view, low angle, 3d render, soft shading, blur, smooth gradient, anti-aliasing, painterly style, anime style, modern style, photographic, realistic, text, words, letters, captions, numbers, watermark, sprite sheet, grid, border, frame, cropped object, duplicate object, [prop-specific negatives]
```

## Constraints + hard rules (do not violate)

- **PPU=32 IMMUTABLE** (memory: [[multi-projection-architecture-lock]] rule #4)
- **30-35° angled view** — no pure top-down, no isometric, no side ([[camera-angle-revisit-s43]])
- **Renderer-agnostic SOs** — no Unity render-stack types in `PatchAtlasSO`/`RoomVisualProfileSO` public API ([[3d-portability-strategy]])
- **CornerField encoding immutable** ([[multi-projection-architecture-lock]] rule #5)
- **`RoomVisualMode` enum → SO reference** for future game customization ([[multi-projection-architecture-lock]] rule #6)
- **No PixelLab `create_character` autonomous** ([[pixellab-mcp]])

## S88_FINAL_LATE state (2026-05-18 night 04:00 — /clear before pickup)

**2 background dispatch'ler aktif yeni session:**
- `by0gt27oo` Combat v14 visual FIX (laurethayday) → DONE marker `STAGING/CODEX_TASK_COMBAT_v14_VISUAL_FIX_DONE.md`
- `baod53dno` Phase B-2 IMPLEMENT (Click-to-Place + auto-collider + inspector edits, laurethgame fresh post-login) → DONE marker `STAGING/CODEX_TASK_PHASE_B2_IMPLEMENT_DONE.md` + 351/351 PASS hedef

**LIVE Map Designer (B-1):** `Tools/RIMA/Map Designer/Asset Pack Browser` — window opens, 2 packs × 124 sprites browse + search + hover preview + read-only inspector. AssetPackManifestSO data backbone LIVE.

**Combat Room v14 ACTIVE + v13 Ritual Chamber PRESERVED** (inactive). User v14 görsel düzeltme istedi (grid + saçma obj + walls) → `by0gt27oo` fix dispatch'i çözüyor.

**Asset-swap mimari LOCK** (user direktif 2026-05-18 night): sistem sprite-source-agnostic. Yarın PixelLab 5000 gen geldiğinde atlas içeriği swap edilir (PatchAtlasSO sprite ref), Map Designer workflow aynı çalışır.

**cxs PowerShell alias LIVE:** user yeni terminal'de `cxs` (one-shot) veya `cxs -Watch` (60s refresh) ile her zaman quota tablosu. Absolute reset saatleri (05:02, 08:45, vs).

**User direktifleri LOCK:**
- "siz tatmin olana kadar Codex'le mutually iterate"
- "sormadan kararı sen ver"
- "memory + status periodic update auto-compact için"
- "3 Codex profile rotation, Codex'i devreden çıkarma"
- "PRO UI/UX Map Designer = asıl büyük iş"
- "yarın oynanabilir map + PixelLab swap-ready sistem"
- "Warblade şimdilik atla"

**Yeni session sırada (no user gate):**
1. 2 dispatch DONE değerlendir + sonuç user'a sun
2. Phase B-3 dispatch (Room save/load + active target binding + random variant + layer toggle + eyedropper)
3. PixelLab cycle reset bekle → autonomous prop production
4. 5 indie research action items uygula

## S88_FINAL state (2026-05-18 evening — for next session pickup)

**Phase A status: In iteration** (user not yet satisfied with v11 8-zone map, Codex `bi9s1bxna` autonomous redesign dispatched 2h timeout — orchestrator awaits notification + screenshot v12+ decision).

**Key memory crosslinks added today (S88):**
- `[[brush-v1-manual-composition-system]]` (this file)
- LaurethStudio: `F:/LaurethStudio/01_PIPELINE/multi_biome_blended_floor_technique.md` (transferable procedural single-sprite biome floor pattern)
- LaurethStudio: `F:/LaurethStudio/01_PIPELINE/auto_collider_from_sprite_pipeline.md` (Map Designer Phase B-2 spec)
- Plan: `STAGING/PLAN_FAKE3D_AND_UIUX.md` (Phase A + Phase B 15-feature breakdown, 5 phased delivery, mutual QC workflow)

**Quota-aware cx_dispatch.py LIVE** (`select_profile_quota_aware()`):
- Live HTTP fetch `/backend-api/codex/usage` per profile
- `STAGING/cx_limits.py` standalone live check (CLI alternative)
- laurethgame currently TOKEN_REVOKED (re-auth `cx login laurethgame` pending)
- Auto-rotates to lowest weekly% profile

**Multi-agent 4 URL research BLOCKED** (`a112206...` rima-research Gemini): X paywall HTTP 402 + Snowflake IDs decode May 2026 (not indexed). User needs to paste tweet content manually for analysis to proceed.

**Indie 6-topic research DONE** (`a34b1050...` rima-research Gemini, 2026-05-18 night): 5 action items surfaced for RIMA:
1. **Add `LightingPreset SO` field to RoomBank** schema (Hades per-room LUT pattern, low-code high-visual-ROI for Phase B)
2. **URP post-process Multiply noise overlay shader** (unifies painterly mixed-pack assets) — see `F:/LaurethStudio/01_PIPELINE/painterly_unifying_noise_overlay.md`
3. **Verify `TerrainDataWriter`** records brush strokes as atomic undo transactions (not per-tile) — top indie editor bug pattern
4. **Chroma budget rule** (floor max 20% saturation, interactables unrestricted) — HLD palette discipline before next asset batch
5. **Audit `PropRuntimeSpawner`** props for sprite-accurate colliders before combat playtesting — top PoE2 complaint ("invisible oversized prop collision")

LaurethStudio additions (transferable techniques):
- `F:/LaurethStudio/01_PIPELINE/painterly_unifying_noise_overlay.md` (Hades-tarzı global noise post-process)
- `F:/LaurethStudio/01_PIPELINE/pixellab_anchor_inpaint_workflow.md` (naked base + inpaint, fights fresh-roll drift)
- Research report: `STAGING/RESEARCH_INDIE_DEV_APPROACHES.md`

**Warblade prompt LIVE** (memory: `[[character-state-tweaks-pending]]`): AGED or YOUNGER variant decision pending — user told to skip ("warblade i şimdilik atla"). Re-trigger when "karakterleri üretecem" said.

## Open work (2026-05-19 onward)

1. **Codex materials production v3** dispatched (`STAGING/CODEX_TASK_MATERIALS_PRODUCTION_V3.md`):
   - Sheet 09 walls (12 variants 64×64)
   - Sheet 10 vertical props (8 props 128×128)
   - Sheet 11 biome floors (16 variants 32×32, 4 biomes)
   - Sheet 12 atmospheric accents (4 accents 256×256)
2. **Wall visual integration** — replace procedural dark-stone wallTex placeholder with Sheet 09 sprites
3. **Vertical props** scattered (column/banner/brazier) for fake-3D depth
4. **Multiple biome rooms** — sandy desert, blood ritual, damp cave (room-to-room variation)
5. **Play mode WASD test** — actual gameplay verification
6. **Wang-blend layer** (deferred) — adjacent tile edge blend, Phase 2 architecture extension
7. **Room transitions** — when door-exit triggers, swap PlayableRoom prefab for next room (Hades chamber pattern)
8. **PixelLab character integration** — replace Warblade placeholder with full 4-direction sprite sheet (PixelLab `create_character` via Web UI V3)

## Related memory

- [[hybrid-asset-pipeline-lock]] — PixelLab + Codex imagegen split (Karar #157 candidate)
- [[multi-projection-architecture-lock]] — 6 hard rules + 3-verdict consensus
- [[room-composer-paint-intent-lock]] — paint-intent semantic brush 3-mode (architectural)
- [[brush-tool-v1]] — Brush V1 ship-ready (321→333/333 PASS) + Phase 1A SO contracts
- [[pixellab-character-states-workflow]] — 6 use cases LOCK Karar #145 v2
- [[pixellab-create-image-pro-format]] — V3 negative inline format (no separate field)
- [[camera-angle-revisit-s43]] — 30-35° + 8-dir mirror strategy
- [[codex-parallel-profile-workflow]] — 3-profile parallel dispatch
- [[3d-portability-strategy]] — renderer-agnostic SO discipline
- [[pixellab-tool-inventory]] — 32 MCP endpoints reference
- [[map-system]] — STS2 DungeonMapUI 3-fork depth (long-term room graph)

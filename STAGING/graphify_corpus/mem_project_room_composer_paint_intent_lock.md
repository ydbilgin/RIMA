---
name: room-composer-paint-intent-lock
description: "ChatGPT 2026-05-17 FINAL verdict — Room Composer = paint-intent semantic brush + procedural dressing layers; \"paint big → 32×32 tile pool\" YASAK, \"paint big → 64/128/256 macro patches\" DOĞRU; Brush V1 semantic 3-mode (Terrain/Organic/Stamp)"
metadata: 
  node_type: memory
  type: project
  originSessionId: a12da79a-6b77-423a-8b7c-59af8ccea2f8
---

# Room Composer — Paint-Intent Architecture LOCK (2026-05-17)

**Source:** ChatGPT FINAL verdict, post-Phase 0 + paint-big-pivot. Authority: All previous plan docs that conflict are superseded. See `STAGING/CHATGPT_PHASE1_FINAL_DIRECTION.md` for full text.

## Why

Phase 0 empirically showed: tile-by-tile generation (PixelLab `create_tiles_pro` or Codex imagegen sprite pack) hits border/grid lines no matter how prompt is tuned. Each tile is independent sprite → tile-edges accumulate visually → "tiled map" reads. Alabaster Dawn fluidity cannot come from tile generation alone — it comes from **layered visual composition on top of clean low-contrast base tiles**.

## Three anti-patterns (banned)

1. **Random 32×32 tile pool as primary look** — equally-important tiles = grid look. Always.
2. **Paint big → slice into 32×32 opaque base chunks** — continuity breaks when chunks rearranged. Seams return.
3. **Wang16 for same-elevation natural blending** (moss/grime/dirt over stone) — visually rigid. Reserved for elevation/feature edges ONLY.

## Three locks

### Lock 1 — "Paint big → MACRO PATCHES, not tile pool"

Large painterly source images (PixelLab Create Image Pro web UI 512px, or future Codex if quality verified) → crop **64×64 / 128×128 / 256×256 candidates** → irregular alpha mask → place on **L2b (macro floor variation)** or **L4 (organic decals)**, NEVER as L2 opaque base.

Crop processing: LANCZOS or AREA downsample (NOT direct nearest from high-res painterly), palette clamp to RIMA, optional dithering, reject focal symbols/runes/blood unless intended accent.

### Lock 2 — Brush V1 = semantic 3-mode

| Mode | Purpose | Writes to | Wang16? |
|---|---|---|---|
| **Terrain Paint** | Logical terrain (walkability/collision/elevation/feature) | cornerField + terrainField + collision/elevation | Yes — elevation/feature edges |
| **Organic Paint** | Visual-only natural surface (moss/grime/dirt/damp) | MacroFloorPatch + OrganicDecal + DetailScatter | NO |
| **Stamp/Prop Paint** | Composed objects/clusters | PropCluster + Stamp + collision + Y-sort | N/A |

`PaintMode` enum required: `TerrainPaint, OrganicPaint, StampPaint, DirectTileOverride, Erase, RegenerateSelected`.

Each stroke serialized as `RoomPaintStroke { strokeId, mode, brushId, affectedBounds, sampledWorldPoints, radius/density/strength/falloff, seed, respectsWalkability, avoidCombatCenter, edgeBiased, wallProximityBiased, allowOverlap, locked }`. Replayable/deterministic.

### Lock 3 — Final L0-L11 Layer Model (L2b added)

```
L0  Data (cornerField, collision, elevation, walkability, locks, path/combat masks)
L1  Base Tone ambient wash
L2  Base Terrain (clean 32×32 low-contrast, no focal marks) — PixelLab create_tiles_pro
L2b Macro Floor Variation (64/128/256 irregular alpha) — big painterly source → crop → mask  ← NEW
L3  Wang16 Feature/Elevation Edges ONLY
L4  Organic decals (moss/grime/dirt/damp/wall-base)
L5  Detail scatter (pebbles/cracks/bones/dust)
L6  Accent overlays (rift scar/ritual circle/blood) — rare
L7  WallKit modular architecture
L8  PropCluster/Stamps
L9  Shadows (prop grounding + wall contact + cliff)
L10 Glow/Lighting (candle/brazier/rift/2D Lights)
L11 Manual overrides (locked cells, hand-polish)
```

## Separate atlas SO types (no generic bucket)

- `BaseFloorAtlasSO` — clean 32×32, no focal marks
- `MacroFloorPatchAtlasSO` — 64/128/256 transparent/irregular
- `OrganicDecalAtlasSO` — moss/dirt/damp/wall grime/cracks
- `DetailScatterAtlasSO` — pebbles/bones/rubble/dust/hairline cracks
- `AccentAtlasSO` — rift scar/ritual mark/blood, rare focal

Brush logic depends on role separation.

## Density defaults (zone-based, NOT uniform)

**Type A enclosed dungeon (12×8=96 cells):** L4 0.10–0.14, L5 0.16–0.22, L6 0.02–0.035.

**Type B Hades arena (combat readability protected):**
- Center: L4 0.03–0.07
- General: L4 0.08–0.12
- Edges/walls/props/features: L4 0.15–0.25
- L5 detail: 0.14–0.18 general, reduced in center
- L6 accent: 0.01–0.025, encounter-aware

Naturalness ≠ uniform density. Use zones.

## Style drift mitigation

Positive constrained descriptions only. AVOID "NO" chains. Use "plain undecorated", "low-contrast", "no focal markings" only when necessary. ONE semantic role per dispatch batch. `style_images` reference when available. QC metadata: accepted/rejected, reason, color drift, contamination, painterly score, perspective, tileability.

## Phase 1A dispatch plan (14 calls, +4 for full Type A)

Per `STAGING/CHATGPT_PHASE1_FINAL_DIRECTION.md` §11: 2 base floor + 1 large painterly macro source + 3 organic + 3 scatter + 2 accent + 3 prop batches (+4 WallKit modules for Type A).

If WallKit skipped, name = **"Phase 1A Layer Stack Visual Test"** (not "Minimal Type A enclosed dungeon").

## Success criteria

- No visible 32×32 grid repetition
- No per-tile soft outline
- Floor reads as continuous
- Character scale feels correct
- Props sit on ground with shadows
- Center remains readable
- Wall/edge dressing does not overpower gameplay
- Room reads as **composed, not tiled**

## Cross-references

- [[karar-143-layered-pipeline]] — L0-L11 layer stack (L2b added here)
- [[brush-tool-v1]] — V1 ship-ready, needs semantic 3-mode extension
- [[camera-angle-direction-strategy-locked-s86-update]] — 30-35° low top-down LOCK (unchanged)
- [[s86-opus-signoff-decisions]] — Wang16 NE-NW-SE-SW corner mask (still valid for elevation/feature edges)
- [[hybrid-asset-pipeline-lock]] — PixelLab characters/props + Codex imagegen tiles confirmed; this memory refines the "tiles" half

## Status

LOCK 2026-05-17. Phase 1A execution next. No more "random tile pool" experiments. No more "Wang16 for moss".

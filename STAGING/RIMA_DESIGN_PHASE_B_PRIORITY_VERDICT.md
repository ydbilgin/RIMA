---
title: RIMA Design — Phase B Map Designer Priority + Risk Verdict
status: LIVE_S88_NIGHT_2026-05-18
agent: rima-design (Sonnet sub-agent, opus-tier judgment)
trigger: orchestrator dispatched Phase B kickoff priority confirmation
---

# Phase B Map Designer — Priority + Risk Verdict (rima-design)

## DECISION 1 — P0 features (confirmed with split + 1 mandatory addition)

3 P0 features (browser / click-to-place / selected-sprite inspector + auto-collider) are right primary loop. But **inspector + auto-collider must split into 4 P0 subfeatures**:

- **P0-a**: Asset Pack Browser (category tree + sprite grid + search + pack switcher + hover preview) — read-only first
- **P0-b**: Click-to-Place + ghost preview + sortingOrder auto + right-click cancel + Esc to deselect (mandatory — without these the cursor-trap kills UX)
- **P0-c**: Selected-sprite inspector (variant / scale / alpha / flip / rotation / sortingOrder override)
- **P0-d**: Auto-collider attach (read `PropDefinitionSO.blocksMovement` + shape, attach Collider2D at placement)

P0-c and P0-d are different risk classes. P0-c trivial Editor UI. P0-d needs SO schema extension + layer-map wiring + composite collider strategy. Treating as one ships both half-baked.

## DECISION 2 — Critical missing features (PROMOTE to P0)

15-feature list has 3 gaps blocking "one room designed from scratch":

1. **Room-as-Prefab save/load** (currently P2 #9) — **PROMOTE to P0**. Without it every Editor restart wipes work. PlayableRoom prefab must have save button writing back to prefab.

2. **Active-target binding** (missing entirely) — when user clicks Scene View, *which parent GameObject* receives sprite? Explicit "active room root" field in window header + visual highlight in Hierarchy. Without this, sprites land at scene root and break `PlayableRoom / Decoration / 01_Macro ...` hierarchy documented in `[[brush-v1-manual-composition-system]]`.

3. **AssetPackManifestSO** (new aggregate SO listing PatchAtlasSO + PropDefinitionSO + categories) — **silent P0 dependency**. Browser has nothing to enumerate without it.

Other missing candidates (multi-room session, biome region brushes, ambient light placement) defer to V2 — not on critical path.

## DECISION 3 — Top 3 risks + mitigation

| # | Risk | Why derails | Mitigation |
|---|---|---|---|
| 1 | SO schema thrash (AssetPackManifestSO vs PatchAtlasSO vs PropDefinitionSO vs PaintMode enum) | 4 SO families overlap; Brush V1 uses `PatchAtlasSO + BrushPresetSO + BiomeSkinSO`; Phase B adds `AssetPackManifestSO + PropDefinitionSO`. Without up-front relationship design → duplicate truth + roundtrip bugs | Before B-1 implementation: orchestrator dispatches Codex consultation for explicit SO relationship diagram. AssetPackManifestSO MUST aggregate existing PatchAtlasSO (no parallel sprite list). PropDefinitionSO is only place collision metadata lives. |
| 2 | Undo/Redo across pack-switches + prefab-mode boundaries | Unity's `Undo.RecordObject` + prefab editing is minefield; pack-switch + ghost-state desync + dangling references | Restrict undo to one room-prefab session at a time. Pack-switch invalidates ghost but does NOT alter undo stack of placed objects. `Undo.RegisterCreatedObjectUndo` per placement (1 unit = 1 step), no batching V1. |
| 3 | Auto-collider footprint heuristic produces wrong shapes for organic props | 70% box works for columns/walls; wrong for braziers (need circle base), statues (offset Y for tall), trees (small footprint << visual). Defaults look right in spec, fail in practice | Authoring discipline beats heuristic. Each PropDefinitionSO ships with hand-tuned `colliderShape + colliderFootprintRatio + colliderOffset` as asset-pack production checklist. Map Designer reads — does NOT guess. Per-category default table = fallback warning, not primary path. |

## DECISION 4 — Cut list (15 → 9 features for V1)

**KEEP (V1 P0/P1):**
- #1 Asset Pack Browser → split P0-a
- #2 Click-to-Place → P0-b
- #3 Selected-sprite inspector → P0-c
- Auto-collider attach → P0-d
- Room save/load → **PROMOTED P0**
- #4 Random variant toggle → P1
- #5 Layer visibility toggle → P1
- #6 Undo/redo → P1
- #7 Hover preview → P0-b (merged)
- #10 Quick-pick from scene (eyedropper Alt+click) → **KEEP P1** (1-day work, dramatically improves iteration)

**CUT V1 (move to V2 backlog):**
- #8 Selection rectangle (mass-edit rabbit hole; right-click delete + per-sprite edit covers V1)
- #11 Snap toggle (PPU snap ONLY mode V1; decimal invites misalignment + breaks pixel-perfect)
- #12 Mirror/symmetry mode (cute, not critical; symmetry doable with 2 clicks)
- #13 Composition guides (rule-of-thirds overlay = gloss before substance)
- #14 Density heatmap (premature optimization)
- #15 AI z-walking suggestion (DO NOT prototype — erodes authorial intent; conflicts with `[[brush-v1-manual-composition-system]]` lesson 2 "dense intentional > algorithmic")

**Net V1: 9 features** (down from 15).

## DECISION 5 — Minimum Viable Map Designer (MVP)

> User can open one Unity scene, dock Map Designer window, point at target `PlayableRoom` prefab root, browse one+ asset packs by category, click in Scene View to place individual sprites (with live ghost preview at PPU-snapped cursor), tune each placed sprite's variant/scale/flip/sortingOrder in right-panel inspector, place wall and prop sprites that **automatically receive correct Collider2D shapes** at placement time, undo any mistake with Ctrl+Z, eyedropper (Alt+click) an existing sprite to keep painting matching variants, and Save the room back to its prefab. Anything beyond this is V2.

**MVP = Phases B-1 + B-2 + B-3 only.** Defer B-4 (power-user) + B-5 (composition assist) until user has actually designed 3 rooms end-to-end with B-1..B-3 and reported pain points.

## Systems affected

- `Assets/Editor/MapDesigner/Brush/` (existing window EXTENDED, not replaced — `BrushToolMode` enum gains `AssetPack` mode; `BrushPalettePanel` + new `AssetPackBrowserPanel` coexist)
- `Assets/Data/Brush/AssetParts_v2/` + `RIMA_AssetParts_v3/` PatchAtlasSO library
- New SO: `AssetPackManifestSO`, extended `PropDefinitionSO` per `auto_collider_from_sprite_pipeline.md` §SO field additions
- Runtime: zero changes (Editor-only)
- Memory: `[[brush-v1-manual-composition-system]]` + `[[room-composer-paint-intent-lock]]` (Phase B adds, does not contradict)

## Architecture LOCK compatibility

**NO conflicts** with locked rules:
- `[[multi-projection-architecture-lock]]` 6 rules (PPU=32, renderer-agnostic SO, CornerField immutable) — respected
- `[[room-composer-paint-intent-lock]]` (Brush V1 semantic 3-mode preserved; new AssetPack mode additive)
- `[[3d-portability-strategy]]` (no Unity rendering refs in new SOs — `PropDefinitionSO.colliderLayer` is `string` not `LayerMask`, KEEP)

**Caveat:** `auto_collider_from_sprite_pipeline.md` line 33 uses `string colliderLayer = "Walls"` (good — renderer-agnostic). String → Unity layer map must live in runtime helper, NOT in SO. Codex must NOT "fix" this by changing field type to `LayerMask`.

## Orchestrator next step (autonomous)

1. ✅ Write this verdict to STAGING (DONE this dispatch)
2. ⏳ Update `STAGING/PLAN_FAKE3D_AND_UIUX.md` §B.2 to reflect: P0 splits into 4 (P0-a..P0-d), Room save/load promoted P0, AssetPackManifestSO silent P0 dependency, 6 cuts, phased delivery shrinks (B-1+B-2+B-3 = V1, B-4+B-5 = V2 post-3-room-playtest)
3. ⏳ Codex consultation on **SO relationship diagram** (Risk #1) BEFORE any B-1 implementation kicks off
4. ⏳ Do NOT begin B-1 implementation Codex dispatch until SO relationship diagram returns

## File paths referenced

- `STAGING/PLAN_FAKE3D_AND_UIUX.md` (P0 split + cuts to apply)
- `F:/LaurethStudio/01_PIPELINE/auto_collider_from_sprite_pipeline.md` (P0-d spec)
- `Assets/Editor/MapDesigner/Brush/MapDesignerBrushWindow.cs` (extend, not replace)
- `Assets/Editor/MapDesigner/Brush/Panels/BrushPalettePanel.cs` (coexist with new browser panel)
- `memory/project_brush_v1_manual_composition_system.md` (S88 LIVE)
- `memory/project_room_composer_paint_intent_lock.md` (Brush V1 architecture)

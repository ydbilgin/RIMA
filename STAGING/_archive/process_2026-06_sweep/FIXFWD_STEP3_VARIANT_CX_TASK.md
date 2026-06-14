ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# FIX-FORWARD STEP 3 — restore grouped + context-aware variant painting in the unified designer

You may EDIT C# source. **DO NOT connect to Unity MCP / run game / touch scenes/assets.** Opus owns Unity, compiles + tests. This implements step B4 of your diagnosis (`CODEX_DONE.md → ## CX DIAGNOSE - DESIGNER REGRESSION`). Steps 1+2 are DONE & verified (composer now renders TileBase floor/cliff onto a Tilemap; OnCoreChanged recomposes — see `## CX FIXFWD STEP12`).

## Amaç (user verbatim)
"varyantları gruplamıştık, duruma göre varyant boyama yapıyorduk, o özellikler de kalkmış." → The user wants back: (a) GROUPED variants — paint a MATERIAL not a single tile, and (b) CONTEXT/STATE-AWARE variant selection — the tile auto-varies per cell + resolves edges/corners by neighbor context. The old system still exists in code; the new `UnifiedDesignerCore.Paint` bypasses it by storing one `SelectedAssetId` directly.

## Design reference (ax consult — follow this model)
Three layers, combined: (1) **Wang/auto-tile** picks the structural piece from 8-neighbor state; (2) **variant bucket** = a group of interchangeable textures for a structural state, weighted; (3) **deterministic hash** `Hash(cell.x, cell.y)` picks a STABLE variant within the bucket (so repainting a neighbor never shuffles existing cells — use a stable spatial hash, NOT a global RNG). UX: palette shows ONE swatch per material group; painting auto-resolves the variant.

## Existing systems to REUSE (do not reinvent; confirm exact APIs by reading)
```
Assets/Scripts/MapDesigner/Brush/Data/AssetPoolSO.cs            (variants list = the group)
Assets/Scripts/MapDesigner/Brush/Data/BrushAssetVariant.cs      (variantId, bucket, weight, tags)
Assets/Scripts/MapDesigner/Brush/Executors/Editor/FreeformDecalExecutor.cs (PickVariant by bucket+weight)
Assets/Scripts/MapDesigner/Composition/WangContextResolver.cs   (neighbor -> wang_* case -> variantId)
Assets/Scripts/Systems/Map/CornerWangPainter.cs                 (corner/Wang resolve + deterministic variant)
Assets/Scripts/Environment/DeterministicVariantTile.cs          (TileBase that picks sprite variant by cell pos)
Assets/Scripts/Map/Data/MaterialVariantPoolSO.cs                (deterministic variant by material id + seed)
```

## What to build (MINIMUM-VIABLE first, surgical)
1. **Group-aware palette + paint resolution.** Introduce a resolver invoked from the unified paint path so painting a GROUP yields a per-cell resolved tile, instead of one fixed GUID:
   - Path today: `UnifiedDesignerCore.Paint(cell,worldPos)` → `RoomDataMutator.PutCategory(room,category,SelectedAssetId,cell,...)`.
   - Add a resolver step: given (selected group, category, cell, current RoomData floor/cliff neighbors) → resolved tile GUID. Store the RESOLVED guid in the cell (so the composer step-2 renders it), PLUS keep an optional source group id on the cell so a later repaint/re-resolve is possible.
   - For the MINIMUM version: a "material group" resolves a deterministic per-cell variant via stable spatial hash over the group's variants (floor variety). This alone restores "grouped variant painting" for floor/cliff.
2. **Context-aware (Wang) for floor edges + cliffs.** When painting/erasing a floor cell, recompute that cell AND its 8 neighbors through `WangContextResolver`/`CornerWangPainter` so edge/corner variants update. Re-resolve only affected cells (not the whole room) to stay cheap.
3. **Palette grouping UX.** The unified palette currently lists individual registry tile GUIDs (`UnifiedMapDesigner.DrawPalette`). Group them so the user picks a MATERIAL (e.g. all `PixelLabFloorIso/iso_floor_*` = one "Iso Granite" group; all `flat_tile_*` = "Flat (top-down)") instead of 16 swatches. Simplest grouping key: registry entry folder/prefix. One swatch per group; show variant count.

## Decisions already made by Opus (so you are not blocked)
- Use a STABLE spatial hash (e.g. deterministic hash of cell coords) for within-bucket selection — never `UnityEngine.Random`/global RNG.
- Floor default group = the iso set (`PixelLabFloorIso`). Keep flat set selectable as an explicit "top-down" group.
- Store resolved GUID in RoomData cells (composer already renders resolved GUIDs). Adding an optional `sourceGroupId`/`materialId` string field to the cell record is allowed (additive, keep JSON DTO in sync).
- If full Wang context is large, ship the MINIMUM (deterministic group variant for floor/cliff) FIRST and clearly mark the Wang-edge part as a follow-up in your report — do not half-do both.

## Constraints
- Editor-only paint/resolve code; reuse runtime variant data structures, do not duplicate them.
- Keep `RoomDataMutator.PutCategory` working for callers that pass a concrete asset (back-compat): the group-resolution is an added layer, not a replacement that breaks direct placement.
- Deterministic + undo-safe: undo records logical cell state, re-resolve is deterministic.
- After editing, report exact diffs (file → method → change), which version you shipped (minimum vs +Wang), any new RoomData/JSON field, and what Opus should compile + test. Output to `CODEX_DONE.md` under `## CX FIXFWD STEP3`.
- If a design fork is genuinely ambiguous after reading the existing systems, state the options + your pick and proceed (do not stall); flag it for Opus review.

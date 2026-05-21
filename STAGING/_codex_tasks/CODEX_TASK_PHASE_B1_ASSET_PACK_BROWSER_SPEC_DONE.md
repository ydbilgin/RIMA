---
title: Codex Done - Phase B-1 Asset Pack Browser Spec
status: DONE_SPEC_ONLY
date: 2026-05-18
profile: laurethayday
---

# Phase B-1 Asset Pack Browser Spec DONE

## Audit findings summary

- Existing `MapDesignerBrushWindow` is a Brush V1 editor shell with brush pack selection, palette filtering, settings, layer visibility, Scene View brush/erase tooling, Unity Undo grouping, hotkeys, automation buttons, and a Props mode.
- Existing Props mode can select `PropDefinitionSO` assets and place prop records into `RoomTemplateSO.props`, but it does not provide sprite-level asset-pack browsing or direct Scene View GameObject placement.
- Missing pieces for the requested UX are: pack manifest, pack/category tree, sprite-level grid, metadata hover preview, read-only production sprite count, ghost preview, direct click-to-place, selected sprite transform inspector, and auto-collider attachment.
- Recommendation: create a new `AssetPackBrowserWindow` rather than extending `MapDesignerBrushWindow`. Keep Brush V1 intact and use adapters for existing data.
- `PatchAtlasSO` and `PropDefinitionSO` do not suffice as a pack abstraction. Add `AssetPackManifestSO` in the implementation phase as a grouping/index layer.
- Do not extend `PatchAtlasSO.PatchRole`; Wall and VerticalProp categories belong in manifest/category metadata.
- The requested memory file path was missing. Closest current-system context used: `STAGING/BRUSH_V1_WORKFLOW_EXPLAINED.md`.

## Spec doc path

`STAGING/SPEC_PHASE_B1_ASSET_PACK_BROWSER.md`

## Audit doc path

`STAGING/CODEX_PHASE_B1_AUDIT.md`

## Skeleton class path

`Assets/Editor/MapDesigner/AssetPackBrowserWindow.cs.skeleton`

## Time estimate for Phase B-1 implementation

Estimated implementation time after orchestrator approval: 4-6 focused hours.

Breakdown:

- 45-60 min: `AssetPackManifestSO` plus catalog entry model.
- 60-90 min: manifest/legacy adapters for PatchAtlas, PropDefinition, BrushPack/AssetPool, and loose v2/v3 sprite folders.
- 90-120 min: EditorWindow layout, pack selector, category tree, search, sprite grid, preview, inspector.
- 45-60 min: production sprite count handling, preview-sheet exclusion, missing/null sprite states.
- 45-60 min: focused EditMode tests for B-1 read-only behavior.

## Risks and open questions

- There are two PropDefinitionSO families in the project. Implementation must choose the active `RIMA.MapDesigner.Props.PropDefinitionSO` for current Props mode while keeping the older `RIMA.MapDesigner.SO.PropDefinitionSO` read-only/legacy.
- v3 staged folder has 44 PNG files, but 4 are `_preview_*` sheets. The B-1 pass gate should count only 40 real asset sprites.
- `AssetPackManifestSO` should be an index layer, not a migration that rewrites existing Brush V1 assets.
- B-2 direct GameObject placement needs a clear persistence target: scene-only authoring, room-template serialization, or both.
- Auto-collider fields should be added only in a later implementation task, not in this spec task.
- Layer name `Walls` must be validated before collider placement; missing layer should warn rather than break placement.

## Next signal

Orchestrator reviews the spec and either approves B-1 implementation or requests revisions.

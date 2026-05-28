# Codex Review: Multi-Layer Painter Plan v1 - VERDICT

## Verdict: PASS_WITH_REVISIONS

## Section-by-section
### (a) Data model: PASS
The field set is sound for Phase 1 painter UX: `layerName`, `sprite`, `sortingOrder`, `offset`, `scale`, `tint`, and `visible` cover naming, asset reference, order, placement, size adjustment, mood/color, and quick A/B toggling. `tint` also carries alpha, so a separate opacity field is not required now. I would not add materials/blend modes in Phase 1.

### (b) Migration: PASS
The plan's migration is clean if the stated premise is true: `backgroundSprite` was added today and is not set on non-test assets. Removing the field leaves no runtime dependency if grep confirms no references. Existing assets should deserialize with an empty or initialized `backgroundLayers` list; the runtime null guard covers older serialized assets.

### (c) Runtime spawn: FAIL
Null safety and per-layer `sortingOrder` are correct. The issue is coordinate space: the code parents `bgRoot` under `result.roomInstance.transform` with `worldPositionStays=false`, then assigns `layerGO.transform.position` using `picked.bounds` coordinates. If `picked.bounds` is room-local and the room instance is ever moved, the layer is placed in world space instead of room-local space. Revise to set `bgRoot.transform.localPosition`/`layerGO.transform.localPosition`, or make the coordinate-space contract explicit. Best pattern: parent root to the room, set root local position to room center, then each layer local position to offset.

### (d) Phase 1/2 split: PASS
ReorderableList is enough for Phase 1. A dedicated EditorWindow is polish and should wait until the data shape survives first production use. Implementation caveat: do not combine a custom `backgroundLayers` ReorderableList with raw `DrawDefaultInspector()` unless `backgroundLayers` is excluded, or the field will draw twice.

### (e) Asset sizes: FAIL
The listed large sizes align with the PixelLab Create Image Pro reference: 512x512, 632x424, 688x384, 424x632, 512x288, 384x216, 256x256, and 128x128 are valid Web UI sizes. The table is incomplete while claiming "Real options": it omits 32x32, 64x64, 344x192, and 341x341. Also note MCP does not cover the 512+ Web UI sizes; production must stay Web UI Create Image Pro for these.

### (f) Open questions verdicts
1. sortingLayerName: NO for Phase 1. `sortingOrder` is enough if backgrounds reserve a negative range. Add `sortingLayerName` only when the project has a locked Sorting Layer taxonomy.
2. Mass spawn cost: ACCEPTABLE. 112 SpriteRenderers across all rooms is fine if only one room is loaded at a time. If multiple rooms become resident, revisit batching/activation.
3. Characters scope: AGREE. Characters should not live in `backgroundLayers`; keep them in the character/enemy/spawn pipeline.
4. Phase 1.5 alignment: ALIGNED. A list of renderable background layer data maps cleanly to the later RoomData renderer contract.
5. Thumbnail async: OK. Use `AssetPreview.GetAssetPreview`, `AssetPreview.IsLoadingAssetPreview`, and `Repaint` while loading.

## Critical findings (FIX FIRST if any)
- Revise runtime spawn coordinate handling in Section 3 to use room-local transforms, not world `position`, unless the plan explicitly locks `picked.bounds` as world-space.
- Revise Section 5 size table to either include all locked PixelLab Create Image Pro sizes or rename it to "recommended room/background sizes" and link the complete reference.

## Recommendations (nice-to-have, not blocking)
- Inspector implementation should draw all default fields except `backgroundLayers`, then draw the custom ReorderableList once.
- Consider a documented reserved sortingOrder range, e.g. backgrounds -200..-50, gameplay 0+, foreground overlays 50+.
- Add a short note that scale is a multiplier and should normally stay positive; negative scale can mirror art but should be intentional.

## Final go/no-go
REVISE plan Section 3 and Section 5 before impl; then PROCEED with Phase 1 implementation.

# Tiles Pro floor-gen — RESOLVED (2026-06-01)

## ✅ SUCCESS (final)
- Correct call: `create_tiles_pro(description="1) flat dark slate dungeon floor... 2)... 3)... 4)...", tile_type:isometric, tile_size:64, tile_view:top-down, tile_depth_ratio:0, tile_view_angle:90, outline_mode:segmentation)`.
- **`tile_depth_ratio:0` = zero vertical thickness = SAME PLANE** (user's "aynı düzlem" / "aşağı-yukarı olmasın" fix).
- Tileset `ce6f15c7-9fbd-4fae-a8f6-ceeab7dd602e` (16 var, 64px iso top-down, flat slate/charcoal/granite/cracked).
- **Download:** `get_tiles_pro(tile_id)` returns `storage_urls` (backblaze, auth-free) → `curl -sL --dangerouslyDisableSandbox`. MCP `save_to_folder` param does NOT exist / does not write here.
- 16 tiles → `Assets/Sprites/Environment/PixelLabFloorFlat/flat_0-15.png`, imported PPU64/Single/Point/Center/Uncompressed.
- Compare: `STAGING/tiles_pro_flat_floor/compare_new_vs_old.png` (LEFT=new flat, RIGHT=old lumpy pl_floor).
- ⚠️ User hasn't SEEN it in Unity yet → next session: show in scene / Map Designer floor before archiving pl_floor.

## get_tiles_pro / create_tiles_pro real schema (gotcha log)
- `create_tiles_pro`: `description` (REQUIRED, numbered tiles), `tile_type`(isometric/hex/square_topdown/...), `tile_size`(16-128), `tile_view`(top-down/high top-down/low top-down/side), `tile_view_angle`(0-90, 90=top-down), `tile_depth_ratio`(0-1), `outline_mode`(outline/segmentation). NO lower/upper_terrain_description, NO view/detail/outline/transition_width/project_name (those were my wrong guesses → 8 validation errors, wasted 0 credits).
- `get_tiles_pro`: only `tile_id`. NO tileset_id/save_to_folder/project_name.

---

# (earlier) Tiles Pro floor-gen attempt log (2026-06-01)

User asked: regenerate flat floor via PixelLab `create_tiles_pro`, doc-correct.

## What happened
- `create_tiles_pro` is a TWO-TERRAIN transition (Wang/blob) generator, top-down or side.
- Gen 1 `d2c3a4f9-1903-4cf7-9a2c-862d4a608f8f` — two near-identical slate terrains → degenerate, returned 1 isolated tile.
- Gen 2 `39f6f223-f124-4d56-9b58-862e607c2c47` — two distinct terrains (dark slate / worn grey stone), tile 48, top-down, lineless, medium transition → completed but MCP returned only 2 "isolated" tiles (tile_0_lower, tile_1_upper), NOT a full Wang set.

## BLOCKER (environment)
- MCP `get_tiles_pro save_to_folder=...` writes NOTHING to disk here (folder stays empty).
- Direct download endpoints tried → 404 (`api.pixellab.ai/mcp/tilesets/<id>/image`, `/tile-sets/`, `/v1/tilesets/`). Correct tiles_pro download URL unknown.
- curl network: works with dangerouslyDisableSandbox (404 not 000), so it's an endpoint/format issue, not pure network.

## Current floor state (usable now)
- `pl_floor_0-15.png` flattened via `STAGING/flatten_floor_tiles.py` (contrast 0.40, desat 0.18, lift 0.05) — visibly less "bumpy", cohesive texture, iso + seamless preserved. Originals safe in `PixelLabFloor/_orig/`.

## Options for user
- A) Tell me the correct tiles_pro download mechanism (you downloaded pl_floor_0-15 before — what command/flow?).
- B) Use `create_isometric_tile` (single seamless iso tile) instead — right tool for ONE uniform floor; keeps iso diamond format.
- C) Keep the flattened pl_floor tiles (current state) and move on.

Placeholder note (per project rule): both tileset_ids logged; no usable PNGs imported.

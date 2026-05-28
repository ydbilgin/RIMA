ACTIVE RULES: (1) think before writing assets (2) min code, no speculation (3) surgical — only the 4 priority assets in scope (4) BLOCKED if classification ambiguous.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"

Amaç: Antigravity's Stream B taxonomy JSON'undaki 4 priority asset'i mevcut WallPieceData .asset YAML formatına dönüştür. Surgical — 4 dosya oluştur, her birinde sprite reference doğru atanmış. NO new prefab generation (Stream B2 visual swap o aşamada yapılacak). NO touching existing wpd_*.asset files.

---

# STREAM B FOLLOW-UP — JSON → WallPieceData .asset

## Source data
Read: `STAGING/s106_overnight/stream_b_assets/asset_classification.json`
Existing schema reference: `Assets/ScriptableObjects/Walls/V2/wpd_rear_wall_1x.asset` (use as template)
Existing registry: `Assets/ScriptableObjects/Walls/V2/WallPieceRegistry_v1.asset` (register the new entries)

## Files in scope (4 NEW .asset files)

| File path | Sprite source | Schema field overrides |
|---|---|---|
| `Assets/ScriptableObjects/Walls/V2/wpd_rear_wall_2x_real.asset` | `Assets/Sprites/AssetPackV3/walls/sheet_2/piece_01.png` | id="rear_wall_2x_real", type=RearWall, direction=Rear, footprintSize=(2,1), lengthInCells=2, heightType=Normal, colliderSize=(2,1), prefab=null (placeholder reuse first) |
| `Assets/ScriptableObjects/Walls/V2/wpd_side_wall_stepped_2x_real.asset` | `Assets/Sprites/AssetPackV3/walls/sheet_2/piece_06.png` | id="side_wall_stepped_2x_real", type=SideWall, direction=SideRight, footprintSize=(1,2), lengthInCells=2, heightType=Normal |
| `Assets/ScriptableObjects/Walls/V2/wpd_low_front_outer_corner_real.asset` | `Assets/Sprites/AssetPackV3/walls/sheet_4/piece_02.png` | id="low_front_outer_corner_real", type=OuterCorner, direction=Any, footprintSize=(1,1), lengthInCells=1, heightType=Low |
| `Assets/ScriptableObjects/Walls/V2/wpd_door_arch_2x_real.asset` | `Assets/Sprites/AssetPackV3/walls/sheet_3/cell_01_R0C0.png` | id="door_arch_2x_real", type=DoorArch, direction=Rear, footprintSize=(2,1), lengthInCells=2, heightType=Normal, colliderSize=(0,0) — passable door per Stream C Bug 3 fix |

Naming convention: `_real` suffix distinguishes these from the existing placeholder-driven `wpd_*.asset` (e.g. `wpd_rear_wall_2x` placeholder vs `wpd_rear_wall_2x_real`).

## Procedure

1. **Read template:** `wpd_rear_wall_1x.asset` to understand exact YAML format expected by Unity.
2. **For each of 4 entries:** Generate YAML following template; sprite reference set via correct GUID lookup of the sprite asset (use UnityMCP `manage_asset` to get GUID or read the .meta file of the sprite).
3. **Verify import:** UnityMCP `refresh_unity` ONCE after all 4 files created (NOT per-file). Then `read_console` to verify 0 errors.
4. **Register in WallPieceRegistry_v1.asset:** Add the 4 new entries to the `pieces` array (read first, append, write back).
5. **Sanity test (no scene change):** Use `manage_asset` or reflection to verify each .asset deserializes correctly (data field types match).

## Safety constraints (HARD)

- ❌ NO touching the existing 14 wpd_*.asset files (placeholders)
- ❌ NO new prefab generation (real-asset prefab swap is Stream B2 separate)
- ❌ Single AssetDatabase batch wrap with try/finally
- ❌ NO scene operations
- ✅ All 4 .asset files OR no files (atomic — if any fails, rollback the others)

## Output (mandatory format)

Write to `CODEX_DONE_<profile>.md`:

```
# STREAM B FOLLOW-UP — <profile> — 2026-05-25 <time>

## STATUS: DONE | PARTIAL | FAILED

## Files created
- <list 4 .asset paths>
- WallPieceRegistry_v1.asset (modified — entries added)

## YAML diff (registry)
<show registry additions>

## Compile check
- Unity console errors: 0
- Warnings: 0

## Sprite GUID resolution
- piece_01.png → <guid>
- piece_06.png → <guid>
- piece_02.png → <guid>
- cell_01_R0C0.png → <guid>

## Time taken: N minutes
```

## Estimated time: 15-25 min

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files + new prefabs only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"

Amaç: User feedback (2026-05-25 morning): "bizim assetlerimizi yerleştirmemişsin, chatgpt_ref teki gibi görseli yakalayamamışız o açıda o derinlikte". Gece Stream E placeholder ile kaldı, Stream B2 (real-asset swap) yapılmadı. Bu task 3 işi birleştirir: (1) real asset prefabs + registry "prefer real" logic, (2) sahne kamera+floor+lighting chatgpt_ref tarzı için, (3) 5 oda re-render.

Hedef: `STAGING/concepts/chatgpt_ref/ChatGPT Image 25 May 2026 00_18_45 (2).png` ve `(3).png` benzeri 3/4 high top-down ARPG sahne, gerçek duvar sprite'larıyla, floor tile'lar görünür, mood lighting.

---

# STREAM F — REAL ASSET VISUAL SWAP + DEPTH/PERSPECTIVE TUNING

## Context

Gece pipeline 7 stream + 1 bonus tamamladı (`STAGING/s106_morning/OVERNIGHT_DELIVERABLE.md`). Mevcut state:
- 4 `wpd_*_real.asset` files have `spriteRef` bound (Bonus #1 - DONE)
- `WallPiece.ApplyMetadata` sprite swap implemented if spriteRef set
- 5 PainterTestE_*.unity scenes spawned with PLACEHOLDER prefabs
- chatgpt_ref/blueprint_room visual targets known but not matched

User feedback (verbatim): "bizim assetlerimizi yerleştirmemişsin chatgpt_ref teki gibi görseli yakalayamamışız o açıda o derinlikte"

## Phase 1 — Real Asset Prefabs + Registry Logic (20-30 min)

### 1A. Create 4 _real prefabs by cloning placeholder + assigning WallPieceData

For each placeholder prefab, create a `_real` variant:

| Source placeholder | New real prefab | data ScriptableObject |
|---|---|---|
| `wp_rear_wall_2x.prefab` | `wp_rear_wall_2x_real.prefab` | `wpd_rear_wall_2x_real.asset` |
| `wp_side_wall_2x.prefab` | `wp_side_wall_stepped_2x_real.prefab` | `wpd_side_wall_stepped_2x_real.asset` |
| (use outer_corner placeholder) | `wp_low_front_outer_corner_real.prefab` | `wpd_low_front_outer_corner_real.asset` |
| `wp_door_arch.prefab` | `wp_door_arch_2x_real.prefab` | `wpd_door_arch_2x_real.asset` |

Steps per prefab:
1. Duplicate placeholder
2. Open prefab, find `WallPiece` component
3. Set `data` field to the corresponding `_real` ScriptableObject
4. Save prefab — at instantiate time, `WallPiece.ApplyMetadata` will load the bound sprite

### 1B. Bind prefab reference in each _real .asset

Each `wpd_*_real.asset` has a `prefab` field (currently null because Bonus #1 didn't create prefabs). Update each to reference the new `_real.prefab` via GUID.

### 1C. Registry "Prefer Real" Logic in WallChainRoomBuilder

In `WallChainRoomBuilder.GetSpanForLength` (or wherever piece selection happens), add preference:
- If a `_real` variant exists for the requested (WallPieceType, direction, length), use IT instead of placeholder
- Else fall back to placeholder

Implementation hint: extend `WallPieceRegistry` with a method `GetPiece(WallPieceType type, WallDirection dir, int length, bool preferReal = true)`. Filter pieces, prefer ones whose id ends with `_real` if `preferReal` is true.

Add a toggle in WallChainRoomBuilder (`public bool preferRealAssets = true;`) so user can switch back to placeholder for testing.

### 1D. Compile + verify
- Unity console: 0 errors required
- Test: spawn one room via painter, verify the real sprites appear instead of colored placeholder

## Phase 2 — Scene Camera + Floor + Lighting (15-25 min)

For EACH `PainterTestE_*.unity` scene (5 scenes):

### 2A. Camera setup
- Camera position: above center of room at appropriate height
- Orthographic, size scaled to fit room (~10 for 22-cell wide, ~14 for boss 26-cell)
- **Camera rotation:** keep flat top-down (0,0,0) but use **transparency sort axis (0, 1, 0)** for Y-sorting depth illusion (this is the standard RIMA top-down 3/4 trick — sprites are drawn in 3/4 perspective but camera is top-down, Y-sort gives depth feel)
- Background: dark navy/black `RGB(8, 10, 16)` for atmosphere
- PixelPerfectCamera component: PPU 64

### 2B. Floor placement
- Create `Floor` GameObject child of room
- Use Tilemap with sheet_3 or sheet_4 floor tiles (`Assets/Sprites/AssetPackV3/floor/tile_*.png`)
- Fill walkable cells with random floor variants (use only walkable cells from RoomSpec, NOT outside)
- Place at z=0.5 (behind walls)

### 2C. Lighting
- Add 2D Global Light (URP 2D Renderer) at low intensity (~0.3) cool blue/white
- For each torch socket (PropSocket with type=Torch), add a Point Light 2D with warm orange glow, intensity ~0.8, range ~3-4 cells
- For each crystal socket (PropSocket with type=Crystal), add cyan Point Light 2D, intensity ~0.6
- Ambient fog/glow optional

### 2D. Save scene
- AssetDatabase.SaveAssets + save scene
- Console: 0 errors

## Phase 3 — Re-Screenshot All 5 Rooms (5-10 min)

For each scene, capture:
- `STAGING/s106_overnight/stream_e_rooms/<room>/scene_v2.png` (gizmos OFF, real visuals, lighting)
- `STAGING/s106_overnight/stream_e_rooms/<room>/comparison.png` — side-by-side with chatgpt_ref target (optional, can be manual)

Use UnityMCP Camera.Render or SceneView screenshot. 1400×900 resolution like Stream E.

## Phase 4 — Side-by-Side Comparison Report

Write to `STAGING/s106_overnight/stream_e_rooms/visual_swap_comparison.md`:

```markdown
# Stream F — Real Asset Visual Swap Comparison

## Per-room comparison
| Room | Before (scene.png) | After (scene_v2.png) | chatgpt_ref target | Match score |
|---|---|---|---|---|
| Combat Basic | placeholder rects | <description> | 25 May (1).png | N/10 |
...

## Key visual deltas
- Real sprites placed: y/n
- 3/4 depth illusion: y/n (via Y-sort)
- Floor visible: y/n
- Lighting atmosphere: y/n
- chatgpt_ref likeness improvement: <N/10 → M/10>

## Remaining gaps
- ...
```

## Safety constraints (HARD)
- ❌ NO Unity crash: AssetDatabase batch with try/finally for prefab creation
- ❌ NO PixelLab gen
- ❌ NO modification of existing placeholder prefabs (only DUPLICATE)
- ❌ NO scene file overwrite outside the 5 PainterTestE_* scenes
- ❌ NO changes to PainterTestAll_v1.unity (pre-existing user scene)
- ✅ Save each scene before screenshot
- ✅ Compile-check after each Phase

## Output

Write to `CODEX_DONE_<profile>.md`:

```
# STREAM F - VISUAL DEPTH - <profile> - 2026-05-25 <time>

## STATUS: DONE | PARTIAL | FAILED

## Phase 1 (real prefabs + registry logic)
- Prefabs created: <4 paths>
- WallPieceData prefab field bindings: <verification>
- Registry preferReal logic: <line numbers>
- Test: room spawned with real sprites y/n

## Phase 2 (camera + floor + lighting)
- 5 scenes updated with camera + floor + lights
- Per-scene verification (console errors per scene)

## Phase 3 (re-screenshots)
- 5 scene_v2.png paths
- Resolution + non-blank check

## Phase 4 (comparison report)
- visual_swap_comparison.md path
- Average match score before/after

## Compile check
- Final console: 0 errors

## Time taken: N min
```

## Estimated total: 45-60 min

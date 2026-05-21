# Codex Task — Floor Re-gen + Playable Map Setup (Brush V1 Visual Gate Round 3)

**Profile:** yasinderyabilgin (live limit: 22%/3% — most empty)
**Effort:** high
**Timeout:** 5400s (90 min)
**Type:** Codex imagegen + Python slicer + Unity-MCP scene setup + Play mode test

## Context

- Round 1 paint test: visual FAIL (opaque black rectangles + per-tile floor grid)
- Round 2 quick-fix PARTIAL: alpha bug FIXED (`RoomDecalChunkRenderer` URP/Unlit transparent material). Floor grid remains — confirmed source-art level (Sheet 01 floor tiles have Y-channel/circuit pattern in their own content). See `STAGING/CODEX_TASK_BRUSH_V1_VISUAL_GATE_QUICKFIX_DONE.md`.
- This task: regenerate Sheet 01 with NO patterns, re-slice, re-import, re-run paint test, fix camera framing, AND make the room playable (player spawn, walls, WASD movement, door exit, Play mode screenshot).

## CRITICAL — Unity instance pre-check

Unity is OPEN. Active instance verified previously as `RIMA@ed023e0b`, scene `Assets/Scenes/Demo/RoomPipelineTest.unity`.

1. List instances via `mcpforunity://instances`. Pin RIMA instance if multiple.
2. `manage_scene action=get_active` — must be `RoomPipelineTest`. If not, load it.
3. Existing in-scene: `StageRoot/BrushV1PaintTestRig` from previous run. Reuse or recreate.

## Task 1 — Floor sheet 01 re-gen via imagegen

Use the **built-in `imagegen` skill** (gpt-image-1 backend, verified working at `STAGING/Phase1A_L2b_Source/codex_floor_source_v1.png`).

### New prompt (replaces Sheet 01 in `STAGING/CODEX_IMAGEGEN_RIMA_ASSET_PARTS_V2.md` line 27)

```
Sixteen plain undecorated dark slate stone floor tile variants for a dark fantasy roguelite ground surface, arranged in a 4x4 grid where each cell is 32x32 pixels. ALL tiles share identical muted slate blue-gray palette around hex 3A4250 with subtle warm amber undertone hex 6B5840 in micro-cracks ONLY.

CRITICAL VISUAL CONSTRAINTS — absolutely flat un-patterned floor:
- NO geometric patterns of any kind
- NO Y-shaped channels, NO T-shaped grooves, NO cross-shapes
- NO circuit-board-like lines, NO mechanical channels
- NO carved decorations, NO frames, NO borders around tiles
- NO repeating motifs that emphasize tile boundaries
- NO crosses, NO diamonds, NO geometric ornament

ALLOWED variation per tile (subtle, organic, color-only):
1) solid dark slate, clean (near-uniform color, only very faint micro-grain)
2) solid dark slate, slightly worn (10% lighter wear in random patch)
3) solid dark slate, dust accumulation (warm amber dust spots)
4) solid dark slate, faint dark stain (cool shadow patch)
5) solid dark slate, hairline natural crack (1px irregular fissure, off-center)
6) solid dark slate, light moss tint (10% darker green-gray patch)
7) solid dark slate, age weathering (slightly bleached top)
8) solid dark slate, water stain (cool blue-gray darker patch)
9-16) similar subtle organic variations, NO geometric pattern

GOAL: When 16 tiles are placed adjacent in a 4x4 grid, the result reads as ONE CONTINUOUS dark slate floor with NO visible tile boundaries — only natural surface variation. Tile-to-tile edge values must match exactly so adjacent tiles do not show a seam.

Style: Low top-down 30-35° angled ARPG perspective. Painterly pixel-art-compatible. Each tile shows subtle upper-darker lower-lighter shading for depth illusion. Sheet has fine separator lines for slicing reference (will be removed by slicer — keep them minimal).
```

Output: overwrite `STAGING/RIMA_AssetParts_v2/sheet_01_floor_tiles_32x32.png` (back up old as `sheet_01_floor_tiles_32x32_v1.png` first).

After generation, QC visually: does the new sheet show 16 near-uniform slate tiles with NO geometric pattern? If FAIL, retry with even stricter negative prompts (max 2 retries).

## Task 2 — Re-slice + re-import floor

After Sheet 01 regenerated:

1. Apply same alpha-clamp + edge desaturate Python pipeline that was applied to other sheets (read script logic from prior session — `STAGING/RIMA_AssetParts_v2/_pre_alpha_fix_backup/` shows backup pattern).
2. Re-slice floor sheet using the existing slicer pattern (4x4 grid, 256×256 cell, downsample 32×32 nearest neighbor, NO autocrop because floor tiles are full-cell opaque). Output: overwrite `STAGING/RIMA_AssetParts_v2/sliced/floor/floor_01..16.png`.
3. Backup old floor sliced PNGs to `STAGING/RIMA_AssetParts_v2/sliced/floor/_pre_regen_backup/`.
4. Copy new floor PNGs to `Assets/Sprites/Environment/RIMA_AssetParts_v2/floor/floor_01..16.png` (overwrite).
5. Trigger Unity reimport (`AssetDatabase.ImportAsset` each, then `Refresh()`). Importer settings already correct from Step 4 prior dispatch.
6. Verify `BaseFloor.asset` PatchAtlasSO still references valid sprites (sprite GUIDs may need refresh if names changed — names should be identical, so refs should hold).

## Task 3 — Camera framing fix

In `RoomPipelineTest.unity`:

- Main Camera position: center on `(W*0.5, H*0.5, -10)` where W=12, H=8 (Combat_Small_01-like bounds).
- Orthographic, ortho size = `H*0.5 + 1` (with 1-unit padding).
- Background color: `#161420` (very dark warm purple).

## Task 4 — Playable map setup

Goal: when user presses Play, a character appears in the room, can walk around with WASD, collides with walls, can reach a door socket on the east side. This is a MINIMUM viable playable test — NOT a polished game.

### Step 4a — Player spawn

- Check if `Assets/Prefabs/Player/Player.prefab` (or similar) exists via `manage_asset action=search path=Assets/Prefabs filter_type=Prefab search_pattern=Player`. If found, use it.
- If no player prefab exists, create a placeholder:
  - Capsule GameObject, scale (0.4, 0.7, 1), `Rigidbody2D` (gravity=0, freezeRotation=true), `CircleCollider2D` (radius=0.3), `SpriteRenderer` with a basic sprite (or just colored placeholder — solid white circle).
  - Tag `Player`.
  - Position at room center-left: `(2, H*0.5, 0)`.
  - Save as `Assets/Prefabs/Test/PlayerPlaceholder.prefab` for reuse.

### Step 4b — WASD movement script

If existing `PlayerController2D` or similar found via `manage_asset search`, attach it.
Otherwise create `Assets/Scripts/Test/TestPlayerMovement.cs`:
```csharp
using UnityEngine;
public class TestPlayerMovement : MonoBehaviour {
    public float speed = 4f;
    Rigidbody2D rb;
    void Awake() { rb = GetComponent<Rigidbody2D>(); }
    void FixedUpdate() {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        rb.linearVelocity = new Vector2(h, v).normalized * speed;
    }
}
```
Use `linearVelocity` (Unity 6+) — if API not available, fall back to `rb.velocity`.

Attach to player.

### Step 4c — Wall collision (room bounds)

Add 4 wall colliders surrounding bounds 12×8:
- `WallNorth`: BoxCollider2D, size (12, 0.2), position (6, 8.1)
- `WallSouth`: BoxCollider2D, size (12, 0.2), position (6, -0.1)
- `WallWest`: BoxCollider2D, size (0.2, 8), position (-0.1, 4)
- `WallEast` (with gap for door): two segments — (0.2, 3.5) at (12.1, 2.25) and (0.2, 3.5) at (12.1, 5.75) — leaving a 1-unit door gap at y=3..5

Parent under `BrushV1PaintTestRig/Walls`. Invisible (no SpriteRenderer).

### Step 4d — Camera follow player

Add `Assets/Scripts/Test/CameraFollow.cs`:
```csharp
using UnityEngine;
public class CameraFollow : MonoBehaviour {
    public Transform target;
    public Vector3 offset = new Vector3(0, 0, -10);
    void LateUpdate() {
        if (target == null) return;
        transform.position = Vector3.Lerp(transform.position, target.position + offset, 0.1f);
    }
}
```
Attach to Main Camera. Set `target` to player.

### Step 4e — Door exit trigger

- GameObject `DoorExit` at position (12, 4, 0), `BoxCollider2D` (size 1, 2, isTrigger=true).
- Script `TestDoorExit.cs`:
```csharp
using UnityEngine;
public class TestDoorExit : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            Debug.Log("[Brush V1 Playable] Player exited the room!");
        }
    }
}
```

## Task 5 — Test + capture

1. Compile + wait `isCompiling = false`.
2. Run `Tools/Brush V1/Run Paint Test` (refresh paint composition with new floor).
3. Capture Scene view screenshot (current state): `STAGING/Brush_V1_paint_test_screenshot_03_editor.png`.
4. Enter Play mode (`manage_editor action=play`).
5. Wait 2 seconds (editor time).
6. Move player programmatically via `Input.GetAxisRaw` override OR simulate by setting `rb.linearVelocity` directly for 1 second to move player to room center.
7. Capture Game view screenshot: `STAGING/Brush_V1_playable_gameview_03.png` (use `capture_source=game_view`).
8. Stop Play (`manage_editor action=stop`).
9. EditMode tests: expect 333/333 PASS (no regression).

## Task 6 — DONE marker

`STAGING/CODEX_TASK_PLAYABLE_MAP_SETUP_DONE.md`:

- Floor sheet regen verdict (PASS/FAIL on Y-pattern removal)
- New floor screenshot inline reference
- Camera framing notes
- Playable scene components added (paths)
- Editor screenshot path (Scene view)
- Game view screenshot path (Play mode)
- EditMode test count (must remain 333/333)
- Visual gate verdict ROUND 3: PASS / PARTIAL / FAIL — with reasons
- Console errors/warnings during Play

## Constraints

- Do NOT modify SO contract scripts
- Do NOT modify Phase 1.5 data-first executors
- Do NOT delete existing Player.prefab if found — only create placeholder if none exists
- Walls/colliders should not be visible (no SpriteRenderer)
- Player must actually be visible in Game view (placeholder circle is fine)
- Door exit just logs to console — no scene change

## NEXT_SIGNAL

If Round 3 PASS: V1 ship trajectory locked. User reviews playable + screenshots next session.
If still PARTIAL/FAIL on floor: escalate to PixelLab `create_topdown_tileset` (when user gen budget renews) or HD-2D escape hatch.

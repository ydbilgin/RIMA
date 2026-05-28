ACTIVE RULES: (1) think before generating (2) min code, no speculation (3) surgical — listed scenes only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Otherwise read-only: STAGING/s106_overnight/MASTER_CONTEXT.md / STAGING/s106_overnight/MASTER_PLAN.md / STAGING/s106_overnight/stream_e_rooms/layouts/*.json / Assets/Scripts/Runtime/Walls/V2/**/*.cs / Assets/Scripts/Editor/Walls/V2/**/*.cs / Assets/Prefabs/Environment/Walls/Placeholders/*.prefab / Assets/ScriptableObjects/Walls/V2/*.asset

Amaç: 5 golden layout JSON'unu kullanarak 5 test odası üret (Combat / Ritual / Flooded / Library / Boss). Her oda için scene + gizmo screenshot + asset usage + gap report + verdict (chatgpt_ref hedefine ne kadar yakın). NO new asset gen. Mevcut 14 placeholder yeterli + Stream B taxonomy varsa real-asset bilgisi kullanılabilir.

---

# STREAM E — 5 TEST ROOMS GENERATION

## Dependencies (BLOCKING — verify before starting)

- Stream C P0 SAFETY: **DONE** (Bug 1-6 fixed, compile clean)
- Stream D Painter UX: **DONE** at least P0.4 (5 presets including Boss Arena) and P0.6 (RoomSpec sockets schema). Other P0 items recommended but not strictly required.
- Stream B Asset Taxonomy: **DONE** OR **PARTIAL** acceptable (real-asset swap is Stream B2 only on 1 room — golden layouts use placeholders by default)
- Golden layouts: 5 JSON files exist at `STAGING/s106_overnight/stream_e_rooms/layouts/{combat_basic,ritual_diamond,flooded_crypt,library_alcove,boss_arena}.json`

If any blocker → write WAITING and stop.

## Files in scope
- `Assets/Scenes/Test/PainterTestE_<room>.unity` — NEW, one scene per room (5 total)
- `STAGING/s106_overnight/stream_e_rooms/<room>/scene.png` — scene screenshot
- `STAGING/s106_overnight/stream_e_rooms/<room>/gizmo.png` — debug gizmo screenshot
- `STAGING/s106_overnight/stream_e_rooms/<room>/report.md` — per-room report
- `STAGING/s106_overnight/stream_e_rooms/INDEX.md` — summary of all 5

Touch nothing else.

## Procedure (PER ROOM, repeat 5x)

### 1. Setup
- Open UnityMCP, set active instance if multiple.
- Read layout JSON: `STAGING/s106_overnight/stream_e_rooms/layouts/<room>.json`
- Create new scene: `Assets/Scenes/Test/PainterTestE_<room>.unity` (save immediately).

### 2. Scene setup
- Camera: 2D, ortho, size 12, transparency sort axis (0,1,0), pixel perfect, background dark gray
- Directional light: top-down, low intensity
- Floor: spawn a 22×22 (or 26×24 for Boss) tile of `_placeholder_white.png` or first floor sheet tile (mechanical loop, AssetDatabase batch wrap)

### 3. Spawn room
- Add a GameObject `RoomBuilder` with `WallChainRoomBuilder` component
- Apply layout JSON to `RoomSpec` via reflection or a public `LoadFromJson(string path)` method (Stream D may have added this — check)
- If no LoadFromJson method exists: deserialize JSON manually in a one-off editor script `Assets/Editor/_temp_s106_load.cs`, build RoomSpec, call `Build()` on the WallChainRoomBuilder, save scene, delete temp script.
- Wait for AssetDatabase.Refresh + domain reload (read_console after, confirm 0 error)

### 4. Add RoomDebugGizmo (from Stream C/D)
- Attach `RoomDebugGizmo` component to the room root GameObject
- Set its spec reference to the RoomSpec used
- This enables color-coded gizmos (green walkable, red blocked, yellow chain, purple door, blue sockets, cyan low front, orange connector/corner)

### 5. Capture screenshots
- Position SceneView camera top-down centered on room
- Screenshot 1 (scene): `STAGING/s106_overnight/stream_e_rooms/<room>/scene.png` — gizmos OFF, only visuals
- Screenshot 2 (gizmo): `STAGING/s106_overnight/stream_e_rooms/<room>/gizmo.png` — gizmos ON, all colors visible

UnityMCP screenshot tool can use `Camera.Render() + RenderTexture` or `Handles.DrawScreenshotToFile` or similar.

### 6. Collect data
- List all spawned WallPiece children, count by WallPieceData.id
- Detect any missing pieces (GetSpanForLength returned null → SpawnPiece fallback used)
- Walk the hierarchy, check for: BoxCollider2D sizes (door+open_gap must be 0,0 per Stream C), sortingLayer (all should be same), Y-sort axis (transparency sort axis should be (0,1,0))

### 7. Write per-room report
- File: `STAGING/s106_overnight/stream_e_rooms/<room>/report.md`
- Format below.

## Per-Room Report Template

```markdown
# Test Room — <RoomName> (<presetId>)

## Layout Source
`STAGING/s106_overnight/stream_e_rooms/layouts/<room>.json`

## Generation
- Scene: `Assets/Scenes/Test/PainterTestE_<room>.unity`
- Builder: WallChainRoomBuilder (V2)
- Asset mode: Placeholder (Stream B real-asset swap is separate)
- Walkable cells: N
- Spawned pieces: M

## Screenshots
- Scene: `STAGING/s106_overnight/stream_e_rooms/<room>/scene.png`
- Gizmo: `STAGING/s106_overnight/stream_e_rooms/<room>/gizmo.png`

## Used Assets
| WallPieceData.id | Count | Notes |
|---|---|---|
| rear_wall_1x | N | ... |
| ... | ... | ... |

## Missing Assets
- List any cases where GetSpanForLength returned null
- List groups expected but not classified

## Issues Found

### Collider Issues
- [ ] All non-door / non-open walls have BoxCollider2D sized = colliderSize
- [ ] Door piece has zero collider (verify via inspector)
- [ ] OpenGap piece has zero collider (verify via inspector)

### Sorting Issues
- [ ] All pieces in same sortingLayer
- [ ] Transparency sort axis = (0, 1, 0) in project settings
- [ ] Player would render BEHIND tall walls when standing further from camera

### Pivot/Anchor Issues
- [ ] Each piece transform position is at footprint anchor (NOT sprite center)
- [ ] Visual child has correct local offset
- [ ] SocketLeft/Right at expected world positions

### Metadata Issues
- [ ] Each WallPiece component has `data` reference set
- [ ] No null reference exceptions on Initialize()

## Blueprint Alignment
- Does this room look like its chatgpt_ref / blueprint_room analog (excluding objects)?
- Score: 0/10 to 10/10
- Why?

## Next Actions
- [ ] (P0) Fix X
- [ ] (P1) Improve Y
- [ ] (P2) Polish Z

## Generation time
- Scene setup: Ns
- Build: Ms
- Total: (N+M)s
```

## Index File

Write summary `STAGING/s106_overnight/stream_e_rooms/INDEX.md`:

```markdown
# S106 Overnight Test Rooms Summary

Generated: 2026-05-25 <time>

| Room | Status | Verdict | Asset Gaps | Critical Issues |
|---|---|---|---|---|
| Combat Basic | DONE / FAILED | N/10 | <list> | <list or none> |
| Ritual Diamond | ... | ... | ... | ... |
| Flooded Crypt | ... | ... | ... | ... |
| Library Alcove | ... | ... | ... | ... |
| Boss Arena | ... | ... | ... | ... |

## Overall Assessment
<2-3 sentences: Does the V2 system produce rooms similar to chatgpt_ref (excluding objects)?>

## Top 5 Priority Next Actions
1. ...
```

## Safety constraints (HARD)

- ❌ NO Unity crash:
  - All AssetDatabase ops wrapped in `try { StartAssetEditing(); ... } finally { StopAssetEditing(); SaveAssets(); }`
  - Save scene BEFORE creating new scene
  - Wait for compile after script changes (UnityMCP `read_console` + check `isCompiling` state)
- ❌ NO new prefab/asset generation outside the 14 placeholders
- ❌ NO PixelLab gen
- ✅ Take screenshots via UnityMCP, save to STAGING (NOT Assets/)
- ✅ Each scene saved before screenshot
- ✅ If a room fails to build (console error), STOP that room, mark FAILED in INDEX, continue with next room

## Estimated time: 90 min (≈18 min per room including setup + capture + report)

## Output (final consolidated report)

`STAGING/s106_morning/OVERNIGHT_DELIVERABLE.md` gets updated/created with Stream E results section + the INDEX.md content + the most-impactful screenshots inline (via path reference).

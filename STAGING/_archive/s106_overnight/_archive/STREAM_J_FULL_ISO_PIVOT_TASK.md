ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if Phase 0 incomplete.

NLM ACCESS: If you need RIMA design context:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"

Amaç: User confirmed YOL B (full iso architecture pivot) 2026-05-25 PM. PixelLab inventory'da hazır chatgpt_ref-style iso tiles var (`b340684f` set, 16 var, 64×64 isometric, dark granite + cyan veins + ritual runes). Bunları Unity'e import et, COMBAT BASIC scene'i iso architecture'a geçir, **Pillar Seam-Cover + URP 2D Lights**, re-render. Hedef: chatgpt_ref ile 7+/10 likeness. TEK ODA (Combat Basic).

---

# STREAM J — FULL ISO ARCHITECTURE PIVOT (Combat Basic single proof)

## ⚠️ Phase 0 — INTERNALIZE chatgpt_ref + b340684f INTENT (MANDATORY 350-500 words at top of response)

Open and study these BEFORE touching code:
- `STAGING/concepts/chatgpt_ref/ChatGPT Image 25 May 2026 00_18_45 (1).png`
- `STAGING/concepts/chatgpt_ref/ChatGPT Image 25 May 2026 00_18_45 (2).png`
- `STAGING/concepts/chatgpt_ref/ChatGPT Image 25 May 2026 00_18_45 (3).png`
- `STAGING/concepts/chatgpt_ref/blueprint_room/ChatGPT Image 24 May 2026 23_42_09 (1).png`
- `STAGING/s106_overnight/pixellab_preview/b340684f_tile_0.png` (sample)
- `STAGING/s106_overnight/pixellab_preview/b340684f_tile_1.png`
- `STAGING/s106_overnight/pixellab_preview/b340684f_tile_3.png`

Describe:
- Iso angle (degrees from horizon — measure from chatgpt_ref)
- Diamond tile dimensions (64×64 PNG → effective diamond is 64×32, 2:1 ratio)
- Wall placement relative to floor diamond (rear walls along NE-NW edge? side walls along NE-SE etc?)
- Color palette match between b340684f and chatgpt_ref
- Lighting integration expectations (URP 2D Point Lights at wall corners?)
- Pillar Seam-Cover placement strategy

Compare your read of chatgpt_ref against the EXISTING scene_v3.png (Stream G output) and articulate what specifically the iso pivot must change.

---

## Phase 1 — Download & Import PixelLab Iso Tiles (10-15 min)

### 1A. Download all 16 tile PNGs from b340684f set
URLs follow pattern: `https://backblaze.pixellab.ai/file/pixellab-tiles/f587b47a-7c0e-4f37-a6c9-7d311a2c935f/b340684f-552b-49e6-a281-ab360d376564/tile_{N}.png` where N=0..15.

Save to: `Assets/Sprites/AssetPackV3/floor_iso_pixellab/b340684f_tile_{N}.png`

Use PowerShell `Invoke-WebRequest` in a single batch (16 calls).

### 1B. Configure Sprite Import Settings
For EACH downloaded PNG, set import:
- `Texture Type: Sprite (2D and UI)`
- `Pixels Per Unit: 32` (since 64px sprite = 2 cells wide visually, 32 PPU = 2 unit wide diamond)
- `Filter Mode: Point (no filter)` (preserves pixel art crispness)
- `Compression: None`
- `Pivot: Center` (default — iso tiles need centered pivot for cellAnchor positioning)
- Apply changes + AssetDatabase.Refresh ONCE at end

### 1C. Create Unity Tile assets (ScriptableObject)
For each sprite, create a `Tile` asset at `Assets/ScriptableObjects/Floor/IsoTiles/tile_{N}.asset`:
- Use `ScriptableObject.CreateInstance<Tile>()`
- Set `tile.sprite = <imported sprite reference>`
- Set `tile.colliderType = Tile.ColliderType.None` (floors don't need collider)
- `AssetDatabase.CreateAsset(...)`

Result: 16 reusable Tile assets ready for Tilemap paint.

---

## Phase 2 — Unity Iso Tilemap Setup in PainterTestE_combat_basic.unity (15-20 min)

### 2A. Convert Grid component from Rectangle → Isometric
In the scene's existing Floor → Grid GameObject:
- `Grid.cellLayout = GridLayout.CellLayout.Isometric`
- `Grid.cellSize = new Vector3(1f, 0.5f, 1f)` (2:1 iso ratio)
- `Grid.cellSwizzle = GridLayout.CellSwizzle.XYZ` (default)

### 2B. Configure Tilemap component
- `Tilemap.orientation = Tilemap.Orientation.Custom` (or `XY` — verify by testing)
- `Tilemap.orientationMatrix` if needed for fine-tune

### 2C. Project Settings — Transparency Sort Mode
Already set per Stream F. Verify:
- `Edit > Project Settings > Graphics > Camera Settings > Transparency Sort Mode = Custom Axis`
- `Transparency Sort Axis = (0, 1, 0)`

If not set, set it. Note in report.

### 2D. Repaint floor with random b340684f tile variants
- Read `RoomSpec.walkableCells` from the scene's PaintedRoom_combat_basic
- For each cell `(x, y)` in walkableCells:
  - Pick random tile index from {0..15}
  - `tilemap.SetTile(new Vector3Int(x, y, 0), tileAssets[randIdx])`
- Wrap in `AssetDatabase.StartAssetEditing()` / `StopAssetEditing()` for safety
- Save scene

### 2E. Camera adjustment
Iso tiles will render in diamond pattern. Adjust camera:
- Camera position: `(roomCenterX * 0.5, roomCenterY * 0.25, -10)` (iso world center)
- Camera ortho size: enough to frame the iso diamond room (~10-12)

---

## Phase 3 — Wall Iso Coordinate Conversion (20-25 min)

### 3A. Update `WallChainRoomBuilder.GetCellWorld`
Current (orthogonal):
```csharp
Vector3 GetCellWorld(int x, int y, bool horizontal) {
    return new Vector3(x * cellSize, y * cellSize, 0f);
}
```

New (iso, 2:1 dimetric per Antigravity recommendation):
```csharp
Vector3 GetCellWorld(int x, int y, bool horizontal) {
    float worldX = (x - y) * (cellSize * 0.5f);
    float worldY = (x + y) * (cellSize * 0.25f);
    return new Vector3(worldX, worldY, 0f);
}
```

This makes wall placement coordinates match the iso tilemap projection.

### 3B. Verify wall sprite scaling
Wall sprites are 3/4 perspective sprites (sheet_2/3/4). They'll now sit ON the iso floor instead of floating above orthogonal floor. Check:
- `WallPiece.ApplyMetadata` already does `localScale = footprintSize / sprite.bounds.size` (Stream G fix). This still works in iso world.
- `transform.position` from `GetCellWorld` is iso-projected. Walls render at correct iso position.
- May need to add Y offset per wall direction (rear walls sit "behind" their cell anchor, side walls sit slightly off) — test and adjust.

### 3C. Compile-check
UnityMCP `read_console` → 0 errors required.

---

## Phase 4 — Pillar Seam-Cover (10-15 min)

### 4A. Increase connector frequency
In `WallChainRoomBuilder` or `RoomSpec`:
- `connectorSpacingMin = 2` (was 4)
- `connectorSpacingMax = 3` (was 7)

This ensures connector (column) prefab spawns every 2-3 cells in wall chains.

### 4B. Force connector at every corner junction
In `BuildRearChain` / `BuildSideChain`:
- Where corner pieces meet wall chains, ALWAYS spawn a connector prefab adjacent (covering seam)
- Where door arch ends, spawn connector on both sides

### 4C. Use existing `wp_connector.prefab` placeholder
No need for new assets — current placeholder connector is enough for proof. Visual upgrade later.

---

## Phase 5 — URP 2D Lights Activation (10 min)

### 5A. Verify URP 2D Renderer Asset
- `Edit > Project Settings > Graphics > Scriptable Render Pipeline Settings`
- Should be set to a URP asset with 2D Renderer assigned
- If 3D URP active, switch to 2D OR add a 2D Renderer pass

### 5B. Scene Lighting Setup
In PainterTestE_combat_basic.unity:
- **Global Light 2D** at scene root: intensity 0.2, color cool blue (#3a4860)
- **Point Light 2D** at each torch socket: intensity 0.8, color warm orange (#ffa040), range 3-4 cells
- **Point Light 2D** at each crystal/ritual socket: intensity 0.6, color cyan (#00ffcc), range 2-3 cells

### 5C. Verify Material/Shader on wall sprites
- Each WallPiece's Visual SpriteRenderer must use `Sprite-Lit-Default` (URP 2D) NOT `Sprite-Default`
- If material wrong, swap to lit material
- Floor tiles same — lit material

---

## Phase 6 — Re-Render Combat Basic (5-10 min)

### 6A. Capture screenshots
- `STAGING/s106_overnight/stream_e_rooms/combat_basic/scene_v5.png` (gizmos OFF, 1400×900)
- `STAGING/s106_overnight/stream_e_rooms/combat_basic/gizmo_v5.png` (gizmos ON)
- `STAGING/s106_overnight/stream_e_rooms/combat_basic/comparison_v5.png` (chatgpt_ref left, scene_v5 right, same zoom)

### 6B. Comparison Report
Write `STAGING/s106_overnight/stream_e_rooms/combat_basic/iso_pivot_comparison.md`:
- Before (scene_v3) vs After (scene_v5)
- Each change measured: floor iso? walls iso? Pillar seams covered? Lighting visible?
- Estimated chatgpt_ref likeness score (be honest, harsh self-rating)

---

## Phase 7 — Report (5 min)

Write `CODEX_DONE_<profile>.md`:

```
# STREAM J - FULL ISO PIVOT - <profile> - 2026-05-25 <time>

## STATUS: DONE | PARTIAL | FAILED

## Phase 0 — chatgpt_ref + b340684f intent (350-500 words)
<your description>

## Phase 1 — Download & import (16 tiles)
- 16 PNG files: <list>
- 16 Tile assets: <list>
- Sprite import settings applied: y/n
- AssetDatabase.Refresh executed: y/n
- Console errors after refresh: 0

## Phase 2 — Iso Tilemap setup
- Grid mode changed to Isometric: y/n
- CellSize set to (1, 0.5, 1): y/n
- Floor cells painted: N tiles
- Camera adjusted: y/n

## Phase 3 — Wall iso coordinates
- GetCellWorld updated: line numbers + before/after diff
- Wall placement test: rear/side/front spawned at iso positions y/n
- Compile: 0 errors

## Phase 4 — Pillar Seam-Cover
- connectorSpacing: 4→2 min, 7→3 max
- Corner junction pillar forced: y/n
- Test: previously visible seams now covered? y/n

## Phase 5 — URP 2D Lights
- URP 2D Renderer active: y/n
- Global Light 2D: y/n
- Torch Point Lights: N count
- Crystal Point Lights: N count
- Wall/floor materials lit: y/n

## Phase 6 — Screenshots
- scene_v5.png: path + dimensions
- gizmo_v5.png: path
- comparison_v5.png: path
- iso_pivot_comparison.md: path

## Self-rating (be harsh)
- chatgpt_ref likeness: N/10 (Stream G was 4.5/10, target 7+/10)
- What's still missing: <list>

## Time: N min
## Compile final: 0 errors / 0 warnings
```

---

## Safety constraints (HARD)
- ❌ DO NOT touch the other 4 PainterTestE scenes (only combat_basic)
- ❌ DO NOT modify existing placeholder prefabs (walls keep current state)
- ❌ DO NOT delete existing wpd_*.asset or wpd_*_real.asset files
- ❌ NO scene operations during code edits (save scene before structural changes)
- ❌ NO Unity crash: AssetDatabase batch + try/finally + scene save before destructive ops
- ✅ Single AssetDatabase.Refresh per phase (not per file)
- ✅ Backup before code changes → `Assets/_archive~/pre_s106_stream_j/`

## Estimated total: 75-100 min

If you hit a blocker → STOP, write PARTIAL report with what's done, don't continue blindly.

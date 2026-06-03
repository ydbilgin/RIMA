> ÔÜá´©Å HISTORICAL WARNING ÔÇö This doc recommends PPU=32, which was the BUG that caused Stream O overlap. The correct rule is PPU=64 (matching cellSize.x=1). Verified at Stream O.1. Reference: MEMORY/feedback_pixellab_iso_tile_ppu_cellsize_rule.md
>
# Unity Iso Tilemap Technical Brief — for Stream J Codex consult

> **Source:** rima-sonnet sub-agent, 2026-05-25
> **Purpose:** Codex Stream J (`br3rn58ei`) can consult this if stuck on Unity API specifics

**Project stack confirmed:** Unity 6 (URP 17.3.0), com.unity.2d.tilemap 1.0.0, com.unity.2d.tilemap.extras 6.0.2, URP 2D Renderer already active (Renderer2D.asset, m_RendererType: 1). Transparency sort axis already (0,1,0) in Renderer2D.asset — no Project Settings change needed.

---

## Q1. Grid Rectangle → Isometric

**Exact property/enum:**
```csharp
Grid grid = go.GetComponent<Grid>();
grid.cellLayout = GridLayout.CellLayout.Isometric;
```
`GridLayout.CellLayout` enum values: `Rectangle`, `Hexagon`, `Isometric`, `IsometricZAsY`.
Use `Isometric` (NOT `IsometricZAsY`) — Z-as-Y is for height-layered tilemaps, not needed here.

**cellSize for 64×64 PNG at 2:1 diamond:**
```csharp
grid.cellSize = new Vector3(1f, 0.5f, 1f);
```
- X=1 = one cell wide in world units
- Y=0.5 = half-unit tall (2:1 ratio, diamond 64px wide × 32px tall)
- Z=1 = depth slot (Unity uses this for iso layer separation)

**ARCHIVE DISCREPANCY:** Pre-S73 archive uses `new Vector3(1f, 0.5f, 0f)` (Z=0). Stream J task prescribes `(1f, 0.5f, 1f)` (Z=1). For `Isometric` mode (not `IsometricZAsY`), Z=0 vs Z=1 only matters if you use Z-position for layering. Safe to use `(1f, 0.5f, 1f)` as Stream J specifies.

**cellSwizzle:**
```csharp
grid.cellSwizzle = GridLayout.CellSwizzle.XYZ;  // default, use this
```
- `XYZ` — standard; world Y maps to screen Y. Use for all 2D iso tilemaps.
- `XZY` — used when the map data is stored with Y as depth (3D terrain height). Do NOT use here.
- Other swizzles (YXZ, YZX, ZXY, ZYX) are for non-standard coordinate imports. Not applicable.

**Side effects on existing painted cells:**
Changing `cellLayout` on a Grid that already has painted Tilemap cells does NOT automatically reposition them. The tile data (cell indices) stays the same, but the visual projection changes immediately on domain reload or `SceneView.RepaintAll()`. Cells that were painted under Rectangle layout will appear at incorrect iso positions. **All cells must be repainted** after switching to Isometric. Stream J Phase 2D handles this.

---

## Q2. Tilemap iso config

**Tilemap.orientation options (Unity 2D Tilemap 1.0.0):**
```csharp
tilemap.orientation = Tilemap.Orientation.XY;     // default — use this
tilemap.orientation = Tilemap.Orientation.Custom; // uses orientationMatrix
```
- `XY` — tiles lie flat in XY plane. Correct for 2D iso tilemaps. **Use XY.**
- `XZ` — tiles lie in XZ plane (3D top-down). Not applicable.
- `YZ` — tiles lie in YZ plane. Not applicable.
- `Custom` — requires explicit `orientationMatrix`. Only needed if you want non-axis-aligned tile plane.

For standard 2:1 iso with `CellLayout.Isometric` and sprites rendered in XY, `Tilemap.Orientation.XY` is correct. Do NOT set Custom unless there is a visible rendering bug with XY.

**orientationMatrix:**
Only set when `orientation = Custom`. For 2:1 iso with no rotation, the identity matrix is correct:
```csharp
tilemap.orientationMatrix = Matrix4x4.identity;
```
Not required if using `Orientation.XY`.

**Sort order on iso vs rect:**
In iso mode, Unity's Tilemap renderer uses the tile's world Y position for sort order by default when Transparency Sort Mode is `Custom Axis (0,1,0)`. This is correct — tiles with lower world Y render on top of tiles with higher world Y, producing correct iso overlap. No extra config needed beyond the sort axis already set in `Renderer2D.asset`.

---

## Q3. Iso PNG sprite import

**PPU calculation — 64×64 PNG, effective 64×32 diamond:**
```
PPU = 32
```
Rationale: at PPU=32, a 64px-wide sprite occupies exactly 2 world units horizontally, and a 32px-tall diamond occupies 1 world unit vertically — matching `cellSize = (1, 0.5, 1)` where the visual diamond is 1 unit wide × 0.5 units tall. Stream J specifies PPU=32 — this is correct.

**Pivot:**
```
Pivot: Center
```
For iso floor tiles, center pivot means the sprite center aligns with the cell center anchor. Unity's iso tilemap places each tile at the cell center by default (Grid.GetCellCenterWorld). Center pivot is correct. Bottom-center pivot shifts the sprite upward by half the sprite height — only useful for wall sprites standing upright on a floor anchor point.

**Filter Mode + Compression:**
```
Filter Mode: Point (no filter)
Compression: None
```
Point filtering is mandatory for pixel art — bilinear/trilinear causes blurring at non-integer zoom. Compression (DXT/ETC) introduces block artifacts on low-resolution pixel art.

---

## Q4. URP 2D lit material

**Auto-assignment of lit vs unlit material:**
In URP 17.x with a 2D Renderer, newly created SpriteRenderer components get `Sprite-Lit-Default` by default IF the project has a 2D Renderer asset active. If the project was originally set up with 3D URP (Forward Renderer), old SpriteRenderers may still hold `Sprites/Default` (unlit). This project has `Renderer2D.asset` active — new SpriteRenderers should default to lit. **Old wall prefabs from pre-S106 may still have `Sprites/Default`.**

**Switch material at runtime/editor script:**
```csharp
Material litMat = new Material(Shader.Find("Universal Render Pipeline/2D/Sprite-Lit-Default"));
SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
sr.material = litMat;
```
**Preferred pattern for editor scripts:** Load by shader name via `Shader.Find()` — reliable in URP 17.x.

**2D Renderer asset location in Project Settings:**
`Edit > Project Settings > Graphics > Scriptable Render Pipeline Settings`
The asset shown there is `UniversalRP.asset`. Inside it, `m_RendererDataList` points to `Renderer2D.asset` (GUID `424799608f7334c24bf367e4bbfa7f9a` — confirmed in this project). The 2D Renderer is already the active renderer. No switch needed.

---

## Q5. Y-sort axis with iso world coords

**Does (0,1,0) sort axis work with iso world coords `(x-y)*0.5, (x+y)*0.25`?**

YES, correctly. When the iso formula maps grid cell `(cx, cy)` to world position:
```
worldX = (cx - cy) * 0.5
worldY = (cx + cy) * 0.25
```
World Y increases toward the top-right of the iso grid. Sprites with higher world Y (top of the screen) correctly sort behind sprites with lower world Y (bottom/foreground). The (0,1,0) sort axis reads world Y, which is the correct discriminant for iso depth. This is already confirmed working — `Renderer2D.asset` has `m_TransparencySortAxis: {x: 0, y: 1, z: 0}`.

**Gotchas:**
- Sort is based on the sprite's pivot world position, not its visual bounds. If a wall sprite's pivot is at center but the sprite extends upward (rear wall), it may sort incorrectly against floor tiles at the same Y. Fix: use `SpriteRenderer.sortingOrder` to manually break ties: floors at order 0, walls at order 1.
- `TilemapRenderer.sortingOrder` must be set below wall SpriteRenderers (e.g., floor Tilemap at order -10, walls at order 0+).

**Pivot recommendation for iso floor tiles:**
`Center`. The iso tilemap cell anchor is the center of the diamond. Center pivot aligns sprite to cell exactly. Bottom-center pivot is for upright objects (characters, walls) where the foot should sit on the ground plane.

---

## Q6. Batch tile painting

**Exact API — SetTile:**
```csharp
tilemap.SetTile(new Vector3Int(x, y, 0), tileAsset);
```
For 168 cells, batch as follows:
```csharp
// Most efficient — set all at once
Vector3Int[] positions = new Vector3Int[168];
TileBase[] tiles = new TileBase[168];
// ... fill arrays ...
tilemap.SetTiles(positions, tiles);  // single call, preferred over 168x SetTile
```
`Tilemap.SetTiles(Vector3Int[], TileBase[])` is available in Unity 2D Tilemap 1.0.0 and calls the internal batch update once. **Use SetTiles, not a loop of SetTile.**

**RefreshAllTiles — when to call:**
- `RefreshAllTiles()` — only needed when tile scriptable object data changed externally (e.g., sprite reassigned on the asset) without going through SetTile. After `SetTile` or `SetTiles`, tiles refresh automatically.
- Do NOT call `RefreshAllTiles()` after every SetTile in a loop — causes stutter.

**AssetDatabase batching for editor scripts:**
```csharp
AssetDatabase.StartAssetEditing();
try
{
    tilemap.SetTiles(positions, tiles);
    EditorSceneManager.MarkSceneDirty(scene);
}
finally
{
    AssetDatabase.StopAssetEditing();
    AssetDatabase.Refresh(); // single refresh at end
}
```
`StartAssetEditing` / `StopAssetEditing` suppress intermediate imports. The `finally` block ensures `StopAssetEditing` is always called even on exception — critical to avoid leaving Unity in a broken import-suspended state.

---

## ⚠️ Likely pitfalls Codex might hit

1. **cellSize Z=0 vs Z=1:** Archive code uses `(1,0.5,0)`, Stream J specifies `(1,0.5,1)`. For `CellLayout.Isometric` (not IsometricZAsY), both work for flat floors. Use `(1,0.5,1)` per Stream J.

2. **Old cells not repositioning:** After switching Grid from Rectangle to Isometric, any previously painted tiles appear at wrong positions. Must call `tilemap.ClearAllTiles()` then repaint — do not assume old cells auto-migrate.

3. **Wall sprites not lit:** Pre-S106 wall prefabs likely have `Sprites/Default` (unlit shader). URP 2D lights have NO effect on unlit materials. Swap to `Sprite-Lit-Default` per Phase 5C before testing lighting.

4. **SetTile loop instead of SetTiles:** A loop of 168 `SetTile` calls triggers 168 internal dirty checks. Use `SetTiles(Vector3Int[], TileBase[])` for a single batch.

5. **TilemapRenderer sorting vs SpriteRenderer sorting:** Floor tilemap must be on a lower `sortingOrder` than wall SpriteRenderers within the same `sortingLayer`. Default is 0 for both — set floor tilemap `sortingOrder = -10` explicitly.

6. **Pivot mismatch on wall sprites (center vs bottom-center):** Floor tiles use Center pivot (correct). Wall sprites standing upright should use bottom-center pivot so their base anchors to the iso floor surface. Mixing pivots causes walls to float or sink. Check `WallPiece.ApplyMetadata` pivot assumption.

7. **`Tilemap.orientation` not `Tilemap.Orientation`:** The enum is `Tilemap.Orientation` (capital O), property is `tilemap.orientation` (lowercase). Type mismatch causes a compile error.

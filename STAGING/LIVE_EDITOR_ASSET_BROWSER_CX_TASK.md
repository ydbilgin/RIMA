# LIVE EDITOR — VISUAL ASSET BROWSER (auto-discover + sizes + categories) (cx, profile laurethayday)

ACTIVE RULES: (1) think before coding (2) min code, surgical — ONE file (3) keep existing behavior working (4) BLOCKED if unclear.
NLM ACCESS: query NLM if needed: uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<q>"

## Amaç
Upgrade the in-play F2 live editor (`Assets/Scripts/DevTools/InPlayMapPaintOverlay.cs`) into a VISUAL ASSET BROWSER so
new IsoKit assets show up automatically (no registry re-bake) with their THUMBNAIL + SIZE, grouped by category, and can be
placed. Goal: when the orchestrator slices new sprites into `Assets/Sprites/Environment/IsoKit/{floor,walls,decor}`, they
appear in the F2 palette instantly with correct sizes → "easy import".

## Existing structure (verified — build WITHIN it, do not rewrite the file)
- The whole file is `#if UNITY_EDITOR || DEVELOPMENT_BUILD`. Press F2 toggles `OnGUI()` (line ~894) which draws a palette
  box `_paletteRect` (300x520) with a thumbnail grid (`GUI.DrawTexture`, 4/row).
- `_palette` (List<TileBase>) + `_paletteNames` = floor/cliff tiles; `_propPalette` (List<WallPiece>) = wall/prop pieces.
- `RebuildPalette()` (line ~702) fills both from `RuntimeAssetRegistry.Instance` (BAKED registry). New folder sprites do
  NOT appear until re-bake — THIS is the gap to fix.
- Placement sinks already exist: `RoomDataMutator.PutFloorCell` / `PutCliffCell` / `PutProp` / `AppendWallRun`;
  `FloorWangResolver` (new) resolves floor 16-mask; `WangResolver.Resolve4` for walls. RoomData = `_activeRoomData`.

## DO (surgical additions, keep registry path intact)
1. **Auto-discover IsoKit sprites.** Add a method that loads all Sprites under three folders and tags each with a category:
   - `Assets/Sprites/Environment/IsoKit/floor`  -> category Floor
   - `Assets/Sprites/Environment/IsoKit/walls`   -> category Wall
   - `Assets/Sprites/Environment/IsoKit/decor`   -> category Decor
   `#if UNITY_EDITOR`: `AssetDatabase.FindAssets("t:Sprite", new[]{folder})` -> `LoadAssetAtPath<Sprite>`. (DEVELOPMENT_BUILD
   without editor: try `Resources.LoadAll<Sprite>` of a mirrored path if present, else skip — guard cleanly.) Call this from
   `RebuildPalette()` (additive — keep the existing registry fill) and from the existing "Refresh palette" button.
2. **Store browser entries** in a small struct list, e.g. `struct BrowserEntry { Sprite sprite; string name; AssetCat cat;
   Vector2 px; Vector2 worldUnits; }` where `px = sprite.rect.size`, `worldUnits = px / sprite.pixelsPerUnit`.
3. **OnGUI category tabs.** Add a row of tab buttons at the top of the palette: `Floor | Wall | Decor` (plus keep the existing
   Tiles/Props sections, or fold them under the tabs). Selecting a tab filters the thumbnail grid to that category.
4. **Thumbnails WITH SIZE.** For each entry draw the sprite thumbnail (use its texture rect via `GUI.DrawTextureWithTexCoords`
   so atlas/multiple sprites crop correctly) + a tiny label under it: `"{px.x:0}x{px.y:0}px  {worldUnits.x:0.##}x{worldUnits.y:0.##}u"`.
   Cyan outline on the selected one. Keep the 4/row grid + scroll.
5. **Select -> place routing by category.** On scene click with a browser entry selected:
   - Floor -> `RoomDataMutator.PutFloorCell(_activeRoomData, sprite.name, cell, worldPos, 0, Vector2.one)` then re-resolve the
     cell + its 4 neighbours via `FloorWangResolver.ResolveFloorTile` and rewrite their `floorCells.assetGuidOrName =
     FloorWangResolver.FloorAssetName(...)` (sprite-swap auto-merge). Recompose.
   - Wall -> `RoomDataMutator.AppendWallRun(_activeRoomData, cell, cell, sprite.name, footprintFromSprite)` (Wang auto-connect).
   - Decor -> `RoomDataMutator.PutProp(_activeRoomData, sprite.name, cell, worldPos, 0, Vector2.one, RoomLayer.Props)`.
   Reuse the existing cell-picking / compose code paths already in the overlay (do not duplicate the tilemap discovery).
6. Keep it all inside the existing `#if` guards; editor-only AssetDatabase calls inside `#if UNITY_EDITOR`. Compile-clean.

## Output format
Apply edits to `Assets/Scripts/DevTools/InPlayMapPaintOverlay.cs` only. Then `read_console` to confirm 0 compile errors
(re-import scripts). Report: what was added (methods + GUI), and a one-line note on any place-routing you simplified.
Do NOT touch other files. If the floor auto-merge re-resolve is too entangled, do the palette+sizes+categories+basic place
first and leave a clear TODO for the floor re-resolve.

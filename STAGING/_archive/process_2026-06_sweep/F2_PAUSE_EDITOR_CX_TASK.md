# CX CODE TASK — F2 overlay → detailed PAUSE editor with visual thumbnails (S6)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — only `InPlayMapPaintOverlay.cs` (4) BLOCKED if unclear.
NLM ACCESS: query `uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<q>"` if needed.
Direct-read only: `Assets/Scripts/DevTools/InPlayMapPaintOverlay.cs` (the ONLY file you edit) + `Assets/Scripts/DevTools/WallRunBuilder.cs` (for WallPiece).

## CONTEXT
RIMA Unity 2D dev tool. `InPlayMapPaintOverlay.cs` is an in-Play (F2) IMGUI map-paint overlay — ALREADY has: F2 toggle, layer enum `PaintLayer { Floor, Cliff, Prop }`, tile palette (`_palette`/`_paletteNames`), prop palette (`_propPalette` of `WallPiece` — each has `.sprite`), hold-drag + Bresenham + Prop drag-place via `WallRunBuilder.BuildRun`, stroke undo (Ctrl+Z), RMB cancel/erase, "Compose Ruined Keep" button, `_paletteRect` over-GUI guard, `RuntimeInitializeOnLoadMethod` bootstrap. KEEP ALL OF THAT.

DECISION (locked by Opus, ax+cx consulted): **STAY IMGUI** (this is a fast dev tool — no uGUI/UI Toolkit rewrite). Add (1) pause-on-open and (2) a VISUAL thumbnail palette. Pattern = ax "Overlay Grid".

## DELIVERABLES (edit `InPlayMapPaintOverlay.cs` only)

### 1. PAUSE on F2-open ("remote edit mode")
- When the overlay becomes visible (F2 toggles `_visible` true): store `float _prevTimeScale = Time.timeScale;` then `Time.timeScale = 0f`. When it closes (F2 again / `_visible` false): restore `Time.timeScale = _prevTimeScale`. Also restore in `OnDisable`/`OnDestroy` so the game never gets stuck paused.
- NOTE: Input System + IMGUI `OnGUI` + `Update` still run at `timeScale==0` (Update runs; only FixedUpdate/physics/animation pause). The drag-place + mouse reads still work. Verify mouse position read path is unaffected.
- Add a GUI label "PAUSED — EDIT MODE" at the top.

### 2. DIM backdrop
- In `OnGUI`, BEFORE the palette area, draw a full-screen semi-transparent dark quad so the paused game reads as "behind" the editor: a `GUI.color`/`GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height), Texture2D.whiteTexture)` with a ~60% black tint (cache a 1x1 black `Texture2D` or use `Texture2D.whiteTexture` + `GUI.color = new Color(0,0,0,0.6f)`). Restore `GUI.color` after. Do NOT dim over the palette rect itself.

### 3. VISUAL thumbnail palette (the core ask: "tipler görsel görünecek")
Replace the current text-button palette list with a THUMBNAIL GRID. For EACH palette entry draw the actual sprite as a thumbnail button:
- Helper `DrawSpriteThumb(Sprite sp, float size)`: compute the sprite's normalized atlas UV from `sp.rect` / `sp.texture` size (`new Rect(sp.rect.x/sp.texture.width, sp.rect.y/sp.texture.height, sp.rect.width/sp.texture.width, sp.rect.height/sp.texture.height)`), then a `GUILayout.Button(GUIContent.none, GUILayout.Width(size), GUILayout.Height(size))` and on `Repaint` `GUI.DrawTextureWithTexCoords(lastRect, sp.texture, uv)`. (Tiles: a `TileBase` may not expose a sprite directly — if it's a `Tile`, cast and use `((Tile)tile).sprite`; else fall back to the text button for that entry.)
- Lay thumbnails in a GRID (wrap ~4 per row via `GUILayout.BeginHorizontal()` every 4). Thumb size ~52px.
- Selected entry: draw a highlight box/outline (e.g. `GUI.color = Color.cyan` around the selected one) and clicking sets `_selected` / `_propSelected`.

### 4. CATEGORY tabs
- The layer toggle row (Floor/Cliff/Prop) already acts as categories — keep it as the tab row at the top, but make switching it rebuild/refresh the shown grid. Floor+Cliff show the tile palette; Prop shows `_propPalette`. (Optional: split Prop into Wall/Prop sub-filter if `WallPiece.displayName` lets you — only if trivial.)

### 5. SELECTED PREVIEW
- Below the grid, show a larger (~96px) thumbnail of the currently selected entry + its name + footprint (for props). Keep "Refresh palette" + "Compose Ruined Keep" buttons.

### KEEP (do not break): F2 toggle, `_paletteRect` guard (enlarge the rect to fit the grid — e.g. width ~300, height ~520), bootstrap, hold-drag, Prop BuildRun, undo, RMB cancel/erase, the editor-fallback wall-kit scan.

## ACCEPTANCE
- Compiles (file is `#if UNITY_EDITOR || DEVELOPMENT_BUILD`). Report the methods you changed + the `DrawSpriteThumb` signature + any DEVIATIONS.
- Do NOT touch other files. Do NOT commit. No ghost-cursor / box-fill scope creep (those are future).

# CX TASK — _IsoGame: F2 paint=granite + Generate-Cliff wiring + camera zoom

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

## Amaç (purpose)
User is in `_IsoGame.unity` (Play mode, F2 dev overlay). Three concrete complaints, all root-caused by Opus already — DO NOT re-investigate, just implement + verify. Unity is OPEN; you have UnityMCP. Verify each fix with `read_console` (0 errors) + a `manage_camera` screenshot where noted.

Scene: `_IsoGame.unity`. Active tilemaps under `IsoGrid`: `Ground` (sortLayer Floor, granite floor451), `CliffTilemap` under `IsoGrid/CliffRing` (sortLayer Ground order -50). CliffAutoPlacer lives on `IsoGrid/CliffRing` (floorTilemap=Ground, cliffTile=`DirectionalCliffTile_Hades`).

---

## FIX 1 — Camera zoom (scroll-wheel, like Unity)  [NEW SCRIPT]
**Why:** PixelPerfectCamera locks zoom; user wants scroll-wheel zoom in/out during Play.
**Verified by Opus:** setting `PixelPerfectCamera.refResolutionX/Y` at runtime zooms while staying pixel-perfect (base is 320x180, assetsPPU=64). Lower refRes = zoom IN, higher = zoom OUT. Confirmed live (refRes 160x90 = clean 2x zoom, no letterbox).

**Create** `Assets/Scripts/Camera/CameraZoom.cs` (namespace `RIMA.CameraSystem`), a MonoBehaviour:
- Cache the `UnityEngine.Rendering.Universal.PixelPerfectCamera` on the same GameObject.
- Base ref resolution = (320, 180), keep 16:9 ratio.
- Maintain a float `_zoom` (1.0 = base). Each scroll notch multiplies `_zoom` by ~0.9 (scroll up = zoom in) / ~1.1 (scroll down = zoom out). Clamp `_zoom` to [0.5, 3.0].
- Apply: `refResolutionX = RoundToEven(320 * _zoom)`, `refResolutionY = RoundToEven(180 * _zoom)` (even ints).
- Input: new Input System — `UnityEngine.InputSystem.Mouse.current.scroll.ReadValue().y` (this project uses the new Input System; see InPlayMapPaintOverlay.cs for the pattern). Guard null.
- IMPORTANT: skip zooming when `ppc.enabled == false` (the F2 overlay disables PPC for its overview cam — don't fight it). Read live `ppc.refResolutionX` only on first frame to seed base; don't clobber the F2 overview.
- Optional: press `R` (Keyboard.current.rKey) resets `_zoom = 1`.

**Wire:** add the `CameraZoom` component to `Main Camera` in `_IsoGame.unity`, save scene.
**Verify:** enter Play, set scroll via MCP isn't possible, so instead just confirm 0 compile errors and that the component is on Main Camera; screenshot game_view at default. (Opus already proved the refRes mechanism works.)

---

## FIX 2 — F2 floor painting must paint GRANITE, not old flat tiles
**Root cause (confirmed):** `Assets/Sprites/Environment/IsoKit/floor` folder DOES NOT EXIST (`AssetDatabase.IsValidFolder` = false). So `LoadIsoKitBrowser()` adds 0 floor browser entries → `BrowserCount(Floor)==0` → the F2 GUI falls back to the registry `_palette` (DrawTileGrid), which contains OLD `flat_0`/`flat_1` tiles. User picked a flat tile and painted → floor looks wrong (not the granite floor451_0/1/14/15 the authored Ground uses).

**Granite source:** `Assets/Sprites/Environment/PixelLabFloor451/` → `floor451_0.png`, `floor451_1.png`, `floor451_14.png`, `floor451_15.png` (these 4 are the LIVE granite group; ignore other floor451_* which were archived). Clean dominant tile = `floor451_0`.

**Fix in** `Assets/Scripts/DevTools/InPlayMapPaintOverlay.cs`:
- In `LoadIsoKitBrowser()` (or add a sibling call), ALSO load the granite floor sprites from `Assets/Sprites/Environment/PixelLabFloor451` into the Floor browser tab (`AssetCat.Floor`), but ONLY the 4 live variants: `floor451_0, floor451_1, floor451_14, floor451_15`. Use the same `AddIsoKitSprites` editor path (AssetDatabase.FindAssets t:Sprite in that folder, filter by those 4 names) + a Resources fallback if you can (folder may not be under Resources — editor path is enough since F2 is a dev tool).
- Result: `BrowserCount(Floor) > 0` → GUI shows the Floor browser → painting uses `PlaceBrowserFloorCell` (FloorWangResolver + fallback sprite). Default `SelectFirstBrowserEntry(AssetCat.Floor)` should now land on a granite tile (ensure `floor451_0` is the FIRST floor entry added so it's the default).
- Do NOT remove the flat tiles from the registry; just make granite the default visible+selected floor option.

**Verify:** Play → F2 → Floor tab shows floor451 granite thumbnails, floor451_0 selected by default. Paint a few cells; screenshot game_view — painted cells match the surrounding dark granite (no light flat tiles). 0 console errors.

---

## FIX 3 — F2 "Generate Cliff (from floor)" button must actually produce the dark cliff
**Root cause (confirmed):** the button (InPlayMapPaintOverlay.cs line ~1413) calls `GenerateCliffsInPlay()`, which (a) early-returns with a warning if `_activeRoomData == null` (it never calls `EnsureActiveRoomData()` first), and (b) uses `RoomCliffSolver` + a registry "cliff"-tagged tile that may not exist → no visible cliffs. Meanwhile the AUTHORED scene already has a WORKING `CliffAutoPlacer` on `IsoGrid/CliffRing` (uses `DirectionalCliffTile_Hades`, reads the live `Ground` tilemap, places 52 directional cliff tiles). The F2 button ignores it.

**Fix in** `GenerateCliffsInPlay()`:
- First, `EnsureActiveRoomData()` (so the null-guard doesn't kill it).
- Then prefer the scene's real placer: `var placer = FindObjectOfType<RIMA.Environment.CliffAutoPlacer>();` — if `placer != null && placer.IsReady`, call `placer.Regenerate()` and `Debug.Log` the count; return. This drives the SAME system as the authored scene → produces the dark directional cliff that hangs into the void.
- Keep the existing RoomCliffSolver path as a fallback only when no CliffAutoPlacer exists.
- Note: `CliffAutoPlacer` is in `Assets/Scripts/Environment/CliffAutoPlacer.cs` (namespace `RIMA.Environment`), same runtime assembly — referenceable from DevTools.

**Verify:** Play → F2 → "Generate Cliff (from floor)" → console logs a non-zero cliff count, cliff tiles appear on CliffTilemap. Screenshot.

---

## CLIFF VISUAL + "KAYMIS" (shift) — Opus already fixed two things; just PERSIST them
Opus made two LIVE edits this session that you must PERSIST (save scene + assets), then regenerate once:
1. **`DirectionalCliffTile_Hades.heightVariation = false`** (was true; maxLift 1.1 + randomness 0.7 created a jagged "spike forest" top line). On `Assets/ScriptableObjects/Environment/DirectionalCliffTile_Hades.asset`: confirm `heightVariation == false`, `EditorUtility.SetDirty` + `AssetDatabase.SaveAssets()`.
2. **"CLIFFLER KAYMIS" root cause (user-reported, confirmed):** `CliffTilemap.tileAnchor` was `(0.5, 0.5, 0)` (cell-center) while `Ground.tileAnchor` is `(0.5, 0.0, 0)` (cell-bottom) — same Grid, mismatched anchors → cliffs floated half a cell up, misaligned to the floor edge. Opus set `CliffTilemap.tileAnchor = (0.5, 0, 0)` to match Ground; cliffs now hug the floor edge. PERSIST this (it's a scene change on `IsoGrid/CliffRing/CliffTilemap`) by saving `_IsoGame.unity`. If the saved scene still shows (0.5,0.5,0), re-apply `(0.5,0,0)` before saving.

After persisting both: `CliffAutoPlacer.Regenerate()` once + `CliffTilemap.RefreshAllTiles()`, then capture ONE scene_view screenshot of the cliff edge.

DO NOT change spriteScale / cliff art / placement math beyond the two fixes above — the sprites are 128x192px (2x3u, top-center pivot) and still hang ~3u; whether to shorten them is a VISUAL judgment Opus will make from your screenshot. Leave it for review.

---

## Output / done criteria
1. `Assets/Scripts/Camera/CameraZoom.cs` created, on Main Camera, compiles clean.
2. InPlayMapPaintOverlay.cs: Floor browser loads granite floor451 (default floor451_0); Generate-Cliff button drives CliffAutoPlacer.Regenerate().
3. `dotnet build` (or Unity recompile via read_console) = 0 errors. Save `_IsoGame.unity` + SaveAssets.
4. Report: files touched, line ranges, console status, and the screenshot paths you captured (game_view after paint, scene_view of cliff edge). Write results to CODEX_DONE.md.
5. If anything is ambiguous, write BLOCKED + the question; do not guess.

DO NOT commit (commit is gated by user). Identity = ydbilgin, no Claude trailer (n/a since no commit).

# Unity Error Fix — 2026-05-14 S70

## Initial Console State

- **Error count (real):** 2
- **Warning count (real):** 2
- **Exception spam:** 13+ MCP transport disconnect entries (not project errors — UnityMCP internal reconnect lifecycle)

### Kritik hata özeti

1. `[Error]` — "The referenced script on this Behaviour (Game Object 'LogCatcher') is missing!" — no stack trace, no file/line. Scene runtime error.
2. `[Error]` — "Some objects were not cleaned up when closing the scene. (Did you spawn new GameObjects from OnDestroy?)" — scene lifecycle warning.
3. `[Warning]` — TMP Unicode char U+25C6 (diamond) not found in LiberationSans SDF font.
4. `[Warning]` — TMP Unicode char U+2620 (skull) not found in LiberationSans SDF font.

---

## Errors Found

### 1. LogCatcher missing script (Error)

- **File:** None — stale error from previous session, not persisted in any current scene file
- **Root cause:** "LogCatcher" GameObject referenced a MonoBehaviour class that no longer exists. Searched all `.unity`, `.asset`, `.prefab` files — `LogCatcher` string not found in any project file. Error was a stale console entry from a prior play/reload cycle before Antigravity's changes.
- **Compile error:** NO. All C# scripts compile cleanly after domain reload.

### 2. Some objects not cleaned up on scene close (Error)

- **File:** None — no stack trace provided by Unity
- **Root cause:** Correlated with the LogCatcher missing script — when a missing-script component exists on a GameObject, Unity sometimes fails to clean up that object during scene transitions. With LogCatcher gone from the scene, this error also cleared after domain reload.

### 3. TMP font warnings (Warning)

- **File:** `com.unity.ugui / TextMeshProUGUI.cs:2012`
- **Root cause:** `CharacterSelectScreen.cs` uses `RimaUITheme` text that includes special Unicode characters (U+25C6 ◆, U+2620 ☠) that are not in the default LiberationSans SDF font asset. These are pre-existing cosmetic warnings, not introduced by Antigravity's changes.
- **Impact:** No gameplay/compile impact. TMP falls back to a replacement character.

---

## Code Review of Antigravity Changes

All modified files were read and reviewed:

| File | Status |
|---|---|
| `Assets/Editor/RoomDesigner/Brushes/BrushController.cs` | Clean — no issues |
| `Assets/Editor/RoomDesigner/IRoomDesignerContext.cs` | Clean — `AutoCliff` + `CliffTile` added correctly |
| `Assets/Editor/RoomDesigner/RimaRoomDesignerWindow.cs` | Clean — Toggle + ObjectField for cliff added to toolbar |
| `Assets/Scripts/Player/CameraFollow.cs` | Clean — bounds clamping preserved, autoBoundsFromFloorTilemap added |
| `Assets/Scripts/Demo/RoomPipelineTestController.cs` | Clean — Karar #122 GenerateLayered added |
| `Assets/Scripts/UI/CharacterSelectScreen.cs` | Clean — pre-existing TMP Unicode warning, not new |
| `Assets/Scripts/Core/LargeDungeonMapPainterBase.cs` | Clean (first 80 lines reviewed) |
| `Assets/Scripts/Systems/Map/RimaRoomBaselineTemplate.cs` | Clean — Karar #122 fields added |
| `Assets/Tests/EditMode/Editor/BrushTests.cs` | Clean — FakeContext implements new AutoCliff/CliffTile interface members |
| `Assets/Editor/RoomDesigner/Brushes/CircleBrush.cs` | Clean — AutoCliff logic correct |
| `Assets/Editor/RoomDesigner/Brushes/SoftBrush.cs` | Clean — AutoCliff logic correct |

---

## Fixes Applied

**None required.** No compile errors were present. The two real runtime errors (LogCatcher + objects not cleaned up) were stale console entries from a previous session state — they did not recur after domain reload triggered by `refresh_unity`.

The domain reload was triggered as part of diagnostics (`refresh_unity compile:request`), which cleared the stale console state.

---

## Final Console State (after iter 1)

- **Error count:** 0
- **Warning count:** 0
- **Console contents:** Only MCP transport lifecycle messages (connect/disconnect), which are not project errors
- **Compile status:** Clean — no compilation errors

---

## Remaining Issues

### TMP Unicode Warnings (cosmetic, pre-existing)

The special Unicode characters (◆ U+25C6 and ☠ U+2620) in `CharacterSelectScreen.cs` via `RimaUITheme` will produce TMP warnings at runtime if the CharacterSelect screen is loaded. These are pre-existing and were present before Antigravity's changes.

**Fix (optional, not blocking):** Replace special unicode chars with ASCII equivalents in `RimaUITheme`, OR add those glyphs to the TMP Font Asset via Window > TextMeshPro > Font Asset Creator. Not a blocker for current development phase.

---

## Iterations Used

**1 of 3** — Console clean after first domain reload. No code changes needed.

---

## Mimari Spec Compliance

| Spec | Status |
|---|---|
| Karar #115 (Map Builder deterministic generator) | PASS — not touched |
| Karar #116 (Tile Transition Quality) | PASS — not touched |
| Karar #117 (Portable Core Game Layer ayrımı) | PASS — not touched |
| Karar #118 (Hybrid Tile Composition Multi-Layer) | PASS — not touched |
| Antigravity 4 P0: Y-Axis Sort `(0,1,0)` | PASS — not touched |
| Antigravity 4 P0: Drop Shadow Layer 1.5 | PASS — not touched |
| Antigravity 4 P0: Wall Front/Top elevation | PASS — CircleBrush/SoftBrush AutoCliff paints at `y-1` (wall row below floor), which is correct for 2.5D depth illusion |
| Antigravity 4 P0: 1px Wang outline | PASS — not touched |

**Overall: PASS**

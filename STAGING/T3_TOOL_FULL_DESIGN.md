# T3 Standalone Tool + Game — Full Design + Impl Breakdown

**Status:** T3 path LOCKED 2026-05-27 gece by user. Triple-AI synthesized inside Opus orchestrator-subagent (agy + Codex pre-distilled in orchestrator brief + Opus sentez). **NO CODE WRITTEN — plan + dispatch breakdown only.**
**Supersedes:** `STAGING/RIMA_LIVE_TOOL_DECISION.md` Section 5 (T2 was previous recommendation; T3 now active path).
**Scope:** ~1280 LOC impl across 12 components, F1-F7 phase wallclock 5-7 days via parallel AI pipeline (Codex/Sonnet rotation, Opus review).

---

## 0. TL;DR (2-minute orientation)

T3 = **Two separate Unity builds** that exchange a single JSON file:
- **Tool.exe** — Unity build with no scene, UI Toolkit Runtime brushes + palette + collider drag-handles + cliff hover indicator. Reads a baked `RuntimeAssetRegistry.asset` to resolve sprite/prefab GUIDs at runtime. Writes `room_current.json` on every paint/place.
- **Game.exe** — Regular RIMA Player Build with a `LiveRoomReloader` that uses `FileSystemWatcher` to detect changes in `room_current.json` and rebuild the active room in <100 ms.
- **Editor "Launch Live Tool" button** — One click from `RimaRoomPainterWindow` launches both `.exe` side by side on dual monitor.

The Editor extension stays as **canonical authoring** (single source of truth for AssetDatabase + Prefab Mode collider editing). Tool.exe is the **rapid iteration surface** — Sang Hendrix vibe (live edit, game in separate window), with the Editor available when deep authoring is needed.

**Trade-off vs T2 (rejected today):** +7-10 days of work, but unlocks user-shippable mod kit, no Domain Reload pain in tool, and a cleaner separation of "authoring vs iteration vs play". User accepted that trade-off 2026-05-27 gece.

---

## 0.5 Sang Hendrix Reference — L1-L9 Decision Set (S114 2026-05-28, triple-AI converge)

Canonical referans = **Sang Hendrix** (Little Master / Steam 4692780 dev). itch.io'da 30+ realtime/WYSIWYG RPG Maker MZ plugin; en yakini **Realtime Parallax Map Builder**. 3-AI (Opus+Codex+agy) mimariyi dogruladi: RPG Maker tek-process JS/PixiJS/NW.js -> `Utils.isOptionValid('test')` playtest algilar, PixiJS overlay biner, `$dataMap` DOGRUDAN bellekte degisir (hot-reload yok), "Save"'de `require('fs')` ile `data/Map001.json`'a geri yazar. RIMA cok-process oldugu icin file-watch (FileSystemWatcher + room_current.json) ayni deneyimi kopruler -- DOGRU secim. Memory: `project_sang_hendrix_live_editor_canonical_ref`.

| # | Karar | RIMA durumu |
|---|---|---|
| L1 | file-watch = IPC (socket'e gerek yok, MVP) | `LiveRoomReloader` + `FileSystemWatcher` ✅ |
| L2 | oyun-ici dev-only toggle (running game'e karsi author) | `LiveToolLauncher` + painter toolbar ✅ |
| L3 | Photoshop layer paneli (z-index/blend/opacity/lock/dup/snap) | 6-layer sorting stack ✅, panel UI eklenecek |
| L4 | stick-to-map ↔ stick-to-camera = parallax factor | ParallaxSection 0.05-1.10 ✅, toggle UI eklenecek |
| L5 | **iki-katmanli persistence**: authoring->JSON, transient yazilmaz, explicit Save/Apply | RoomLayoutSerializer ✅, transient ayrim **YENI** |
| L6 | klasor auto-index -> palette | `RuntimeAssetRegistryBaker` + `RuntimeBrushPalette` ✅ |
| L7 | ayri collision/walkable paint pass (toggle grid overlay) | `RuntimeColliderHandles` + walkability ✅ |
| L8 | grid-free + snap toggle (decor serbest, tile snap) | painter ✅, snap toggle netlestir |
| L9 | **region-ID occlusion**: oyuncu altinda foreground/cati fade | **YENI feature kandidati** (roadmap) |

Sonuc: T3 = sifirdan insa DEGIL. %80 kurulu (L1-L4,L6-L8); eksik = L5 transient persistence (kucuk) + L9 occlusion (yeni, ertelenebilir). Roadmap L5/L9 task: `FORWARD_EXECUTION_ROADMAP.md`.

---

## 1. T3 Architecture Overview

### 1.1 Three deliverables

| Artifact | Build target | Asset usage | UI |
|---|---|---|---|
| **Editor (existing)** | Unity Editor (Window) | AssetDatabase + Prefab Mode | IMGUI + UI Toolkit Editor — UNCHANGED by T3 |
| **Tool.exe** | Standalone Player Build (Mono, Development) | `RuntimeAssetRegistry.asset` (baked GUID→Sprite/Prefab map) | UI Toolkit **Runtime** (UXML + USS + C#) |
| **Game.exe** | Standalone Player Build (existing RIMA pipeline) | Resources + Addressables (existing) | Existing in-game UI |

### 1.2 Data flow (single JSON contract)

```
+-------------------+     write     +-----------------------------+     watch     +-------------------+
|                   |  --------->  |                             |  --------->  |                   |
|     Editor /      |              |   StreamingAssets/live/     |              |     Game.exe      |
|     Tool.exe      |              |   room_current.json         |              | (LiveRoomReloader)|
|                   |  <---------  |   (room_current.lock)       |  <---------  |                   |
+-------------------+     read     +-----------------------------+     ack      +-------------------+
```

- **Writer:** Editor (D6 phase) OR Tool.exe (F3+ phase) — both serialize the same schema.
- **Reader:** Game.exe `LiveRoomReloader` (F5 phase).
- **Lock file:** `room_current.lock` for atomic write detection (Tool/Editor writes lock → finishes write → deletes lock; Game waits if lock present, debounces 100 ms after lock release).

### 1.3 Asset resolution (the hardest T3 problem)

Editor uses `AssetDatabase.GUIDToAssetPath` — not available in standalone builds. Tool.exe needs an equivalent GUID-keyed lookup at runtime.

**Solution: `RuntimeAssetRegistry.asset` baked ScriptableObject.**
- Editor scans `Assets/Sprites/Environment/`, `Assets/Prefabs/Props/`, `Assets/ScriptableObjects/Environment/` at build time.
- For each asset → emit a `RegistryEntry { string guid, Sprite sprite, GameObject prefab, RoomLayer layer }`.
- Final SO ~200-500 entries (depending on what's promoted to "paintable").
- Tool.exe loads the SO from Resources at startup → `Dictionary<string, Sprite>` + `Dictionary<string, GameObject>` lookups.
- JSON serializes ONLY the GUID string; Tool/Game both resolve via registry.

This is the same pattern Addressables uses internally, but lighter — we don't need lazy load or remote bundles, just a flat in-memory table.

### 1.4 Build configuration

- **Tool.exe build target:** `Builds/RIMA_Tool/RIMA_Tool.exe` — Mono backend (faster build), Development Build flag (for live debug logs), single scene `Assets/Scenes/LiveTool/ToolMain.unity` that bootstraps the UI Toolkit Runtime panel.
- **Game.exe build target:** `Builds/RIMA_Game/RIMA.exe` — existing pipeline, no changes.
- **Shared registry:** `Assets/Resources/Live/RuntimeAssetRegistry.asset` — baked once, included in both builds via Resources folder.

### 1.5 Why this is the right T3 shape

- **Editor stays canonical** — keeps Day 5a Preview Pane, Prefab Mode, AssetDatabase. Existing 1500+ LOC `Assets/Editor/RoomPainter/**` is NOT thrown away.
- **Tool.exe is a focused mod-kit shell** — no full painter rebuild; just the runtime-paintable subset (palette + brush + paint + save). Power users iterate fast. Editor used for new-asset import + deep authoring.
- **Game.exe is unchanged** in shape — only gains a single `LiveRoomReloader.cs` (~150 LOC) and a flag to enable it in Development Builds.
- **Sang Hendrix parity** — dual-monitor pattern works exactly like RPG Maker MZ: Tool window left, Game window right, JSON between them.

---

## 2. Component Breakdown (LOC + writer rotation per `feedback_code_writer_rotation`)

### 2.1 Master component table

| # | Component | Path | LOC est | Writer | Reviewer | Phase |
|---|---|---|---|---|---|---|
| C1 | JSON schema lock + RoomManifestSO extend | `Assets/Scripts/Map/Data/RoomManifestSO.cs` + new schema struct | ~80 | Codex xhigh | Sonnet | F1 |
| C2 | RoomLayoutSerializer (Editor side, writer) | `Assets/Editor/RoomPainter/LiveTool/RoomLayoutSerializer.cs` | ~120 | Codex xhigh | Sonnet | F1 |
| C3 | RuntimeAssetRegistry baker | `Assets/Editor/RoomPainter/LiveTool/RuntimeAssetRegistryBaker.cs` | ~60 | Sonnet | Codex review | F2 |
| C4 | RuntimeAssetRegistry SO + loader | `Assets/Scripts/Live/RuntimeAssetRegistry.cs` | ~80 | Sonnet | — | F2 |
| C5 | Tool UI Toolkit Runtime base (UXML+USS+C# bootstrap) | `Assets/Scenes/LiveTool/ToolMain.unity` + `Assets/UI/LiveTool/ToolMain.uxml`+`.uss` + `Assets/Scripts/LiveTool/ToolBootstrap.cs` | ~300 | Sonnet | Codex review | F3 |
| C6 | Tool RuntimeBrushPalette (clickable thumbnails, filter, search) | `Assets/Scripts/LiveTool/Palette/RuntimeBrushPalette.cs` | ~120 | Sonnet | — | F3 |
| C7 | Tool RuntimeColliderHandles (SceneView replacement) | `Assets/Scripts/LiveTool/Authoring/RuntimeColliderHandles.cs` | ~150 | Codex xhigh | Sonnet | F4 |
| C8 | Tool RuntimeCliffHoverIndicator (camera+cursor based) | `Assets/Scripts/LiveTool/Authoring/RuntimeCliffHoverIndicator.cs` | ~80 | Sonnet | — | F4 |
| C9 | Tool RuntimeAssetLoader (registry consumer) | `Assets/Scripts/LiveTool/Runtime/RuntimeAssetLoader.cs` | ~100 | Sonnet | Codex review | F3 |
| C10 | Game LiveRoomReloader (FileSystemWatcher consumer) | `Assets/Scripts/Live/LiveRoomReloader.cs` | ~150 | Codex xhigh | Sonnet | F5 |
| C11 | Game FileSystemWatcher integration + debounce + lock | `Assets/Scripts/Live/JsonFileWatcher.cs` | ~80 | Sonnet | — | F5 |
| C12 | Editor "Launch Live Tool" button + dual-build runner | `Assets/Editor/RoomPainter/LiveTool/LiveToolLauncher.cs` + `RimaRoomPainterWindow.cs` modify | ~40 | Sonnet | — | F6 |
| — | Build configuration (BuildPlayerProcessor + 2 build targets + Scripting Define `RIMA_LIVE_TOOL`) | `Assets/Editor/Build/LiveToolBuildProcessor.cs` | ~60 | Sonnet | — | F6 |
| **Total** | | | **~1280** | **rotation** | **rotation** | |

### 2.2 Writer rotation rationale (per HARD rule)

- **Codex xhigh writes:** C1 (schema discipline), C2 (algorithmic JSON serialize), C7 (algorithmic handles math), C10 (file watcher state machine). These need careful state handling — Codex xhigh strength.
- **Sonnet writes:** UI Toolkit Runtime (C5, C6, C9), helper components (C3, C4, C8, C11), Editor glue (C12, build config). Mechanical impl, large file count — Sonnet strength.
- **Codex reviews:** every Sonnet-written core file (C5, C9, C12). Lightweight pass for "is this surgical?", "is min code?".
- **Sonnet reviews:** every Codex-written file (C1, C2, C7, C10). Lightweight pass for "Unity API correct?", "RIMA conventions followed?".

No file is written and reviewed by the same actor — `feedback_code_writer_rotation` satisfied.

### 2.3 Why no agy in the writer slot

agy is research-only per current memory locks. agy is consulted for **architectural questions** that arise mid-impl (e.g., "which FileSystemWatcher debounce pattern do shipping Unity mod kits use?") — dispatched ad-hoc, not slotted on the rotation table.

---

## 3. Phase Plan — F1-F7 (5-7 day wallclock, parallel)

### F1 — JSON schema + serializer (Day D6, parallel with D5.6 cliff floating feel)

**Goal:** Lock the room JSON schema and write the Editor-side serializer that emits it on every paint/place.

**Components:** C1 (RoomManifestSO extend) + C2 (RoomLayoutSerializer)

**Dispatch:**
- Codex xhigh writes both. Spec written in `STAGING/T3_F1_schema_serializer_task.md`.
- Sonnet reviews after Codex done.
- Background dispatch, run_in_background:true.

**Schema (locked here, Codex implements):**
```json
{
  "version": "1.1",
  "roomId": "PlayableArena_Test01",
  "schemaTimestamp": "2026-05-27T22:30:00Z",
  "floorTiles": [
    { "gridX": 0, "gridY": 0, "tileGuid": "abc123..." }
  ],
  "cliffCells": [
    { "gridX": 1, "gridY": 2, "tileGuid": "abc123...", "direction": "S", "manual": true }
  ],
  "propInstances": [
    {
      "instanceId": "inst_0001",
      "prefabGuid": "def456...",
      "position": [3.14, 1.59, 0.0],
      "rotation": 0,
      "scale": [1, 1, 1],
      "sortingOrder": 100,
      "layer": "Wall",
      "colliderOverride": { "type": "Box", "size": [0.6, 1.0], "offset": [0, 0.5], "isTrigger": false }
    }
  ],
  "parallaxLayers": [
    { "tier": "Mid", "prefabGuid": "ghi789...", "offset": [0, 0] }
  ]
}
```

**Output:** C1 + C2 LIVE in Editor. Calling `RoomLayoutSerializer.WriteCurrent()` produces a valid JSON at `Application.streamingAssetsPath/live/room_current.json`. No reader yet.

**Verify:** Editor unit test — paint 5 tiles + 3 props in PlayableArena_Test01, call `WriteCurrent()`, deserialize back to dict, assert round-trip equality.

### F2 — RuntimeAssetRegistry baker + GUID stability (D7, parallel)

**Goal:** Build the GUID→Sprite/Prefab lookup table that both Tool.exe and Game.exe consume at runtime.

**Components:** C3 (Baker) + C4 (Registry SO + loader)

**Dispatch:**
- Sonnet writes both. Spec: `STAGING/T3_F2_asset_registry_task.md`.
- Codex reviews.

**Key decisions baked in:**
- Registry asset path: `Assets/Resources/Live/RuntimeAssetRegistry.asset` (Resources folder = bundled in both builds, no Addressables dependency for V1).
- Scan roots (Editor menu RIMA → Live Tool → Bake Registry):
  - `Assets/Sprites/Environment/PixelLab_Selected_Assets/` (floor tiles)
  - `Assets/Sprites/Environment/KitB_Cliff/` (cliff sprites)
  - `Assets/Prefabs/Props/ShatteredKeep_PixelLab/` (mounting + statue)
  - `Assets/Prefabs/Environment/Walls/AssetPackV3/` (wall prefabs)
  - `Assets/Prefabs/Obstacles/` (chasm, narrow passage, stone column)
  - `Assets/ScriptableObjects/Environment/` (TileBase assets)
- Entry struct includes `RoomLayer` from `RoomPainterPhysicsRules` (D2 LIVE) → palette can pre-filter.
- GUID stability: relies on `.meta` file commit — already RIMA convention. Bake produces a `registry_manifest.txt` diff so we can detect dropped GUIDs.

**Output:** C3 + C4 LIVE. Both Tool.exe and Game.exe can call `RuntimeAssetRegistry.Get(guid)` at runtime and receive the correct asset.

**Verify:** Bake registry → close Unity → reopen → Play Mode test (Game.exe simulation) → 5 GUIDs resolved correctly. Build standalone → run .exe → same 5 GUIDs resolve.

### F3 — Tool UI Toolkit Runtime base + brush palette (D8, parallel)

**Goal:** Tool.exe shell that opens, shows a palette, lets user select a brush. No paint yet — just UI scaffolding.

**Components:** C5 (Bootstrap + UXML + USS) + C6 (RuntimeBrushPalette) + C9 (RuntimeAssetLoader)

**Dispatch:**
- Sonnet writes all three. Spec: `STAGING/T3_F3_tool_ui_base_task.md`.
- Codex reviews C5 and C9.

**Scene structure (Assets/Scenes/LiveTool/ToolMain.unity):**
- 1 GameObject `[ToolRoot]` with `ToolBootstrap` (C5)
- 1 UIDocument GameObject with `ToolMain.uxml` panel
- 1 Camera (orthographic, for ghost preview render)

**UXML hierarchy (high-level):**
```
ToolMain.uxml
  VisualElement.root (display:flex, flex-direction:row)
    VisualElement.left-panel (palette, ~300px wide)
      DropdownField (Mode: Tile/Cliff/Decor/Object)
      ToggleField (Layer filter L1-L6 multi-select)
      ScrollView.thumbnail-grid
        Button.brush-thumb (×N from registry)
    VisualElement.center-panel (game/preview area, flex-grow)
      VisualElement.preview-canvas (raycaster overlay)
    VisualElement.right-panel (inspector, ~280px)
      Label.selected-asset-name
      DropdownField (Collider shape)
      FloatField.collider-size-x/y
      Button.save-room
```

**Output:** Tool.exe builds and launches. Palette shows ~50 thumbnails (registry consumed). Click thumbnail → selected state visible in right panel. **No paint to scene yet** — F4 wires that.

**Verify:** Build Tool.exe + launch → palette renders + thumbnails clickable + mode dropdown works. Manual smoke test, 5 min.

### F4 — Runtime collider handles + cliff hover indicator (D9, parallel)

**Goal:** Port the SceneView authoring tools (Day 4 collider drag + Day 5 cliff hover) to runtime camera+cursor based.

**Components:** C7 (RuntimeColliderHandles) + C8 (RuntimeCliffHoverIndicator)

**Dispatch:**
- Codex xhigh writes C7 (algorithmic math — drag handle hit-test, drag delta apply, undo stack). Spec: `STAGING/T3_F4_runtime_handles_task.md`.
- Sonnet writes C8 (cursor + camera ray + tile highlight overlay).
- Sonnet reviews C7, no review on C8 needed (small + isolated).

**C7 key challenge — SceneView replacement:**

Editor `RoomPainterColliderEditor.cs` uses `Handles.FreeMoveHandle` + `SceneView.RepaintAll` — none exist at runtime. Runtime replacement:
- Convert mouse position to world via `Camera.main.ScreenToWorldPoint`.
- Draw 8 handle dots as `Image` UI Toolkit elements positioned in screen space (project world → screen each frame).
- Hit-test by 16px radius around each dot.
- Drag → apply delta in world space → update collider via `Collider2D.size` / `Collider2D.offset`.
- Undo: maintain a simple `Stack<ColliderState>` (size 32) — Ctrl+Z pops.
- Outline drawn via `LineRenderer` GameObject (one per active collider).

**C8 cliff hover indicator:**
- Existing `CliffHoverIndicator.cs` uses SceneView GUI → port to a single SpriteRenderer GameObject at cursor's grid cell.
- Snap cursor to grid: `Mathf.RoundToInt(worldPos.x), Mathf.RoundToInt(worldPos.y)`.
- 8 sprite variants pre-loaded from registry (cliff_S, cliff_N, etc.) → display based on neighbor analysis.

**Output:** Tool.exe now paints cliffs with hover preview and lets user drag-resize box/circle/capsule colliders on placed props.

**Verify:** Launch Tool.exe → paint a cliff → see hover preview snap to grid → drop. Place a wall prefab → click in inspector → drag handle dots → see live size label. Ctrl+Z 5 times → reverts.

### F5 — Game.exe LiveRoomReloader + FileSystemWatcher (D10, parallel)

**Goal:** Game.exe reads `room_current.json` and live-rebuilds the active room when the file changes.

**Components:** C10 (LiveRoomReloader) + C11 (JsonFileWatcher)

**Dispatch:**
- Codex xhigh writes C10 (room rebuild state machine — destroy old props, instantiate new, preserve player position). Spec: `STAGING/T3_F5_game_reloader_task.md`.
- Sonnet writes C11 (FileSystemWatcher + 100ms debounce + lock file handling).
- Sonnet reviews C10.

**Reload algorithm (C10):**
1. Watcher fires "changed" → debounce 100 ms.
2. Check lock file present → wait up to 500 ms for release.
3. Read JSON → deserialize to `RoomLayoutData`.
4. Diff against current room state: which prop instances added/removed/moved?
5. Apply diff (not full rebuild): destroy removed, instantiate added, transform moved.
6. Tilemap floor + cliff: SetTiles batch call (already supports bulk).
7. Preserve player position UNLESS player is in a now-void cell → snap to nearest walkable.
8. Log `[LiveRoomReloader] Reload applied in {ms}ms — Δ{addedCount}+ / {removedCount}- / {movedCount}~`.

**Toggle:** Game.exe Development Build only → `#if DEVELOPMENT_BUILD || UNITY_EDITOR` guards `LiveRoomReloader` startup. Production builds have ZERO live tool code → no perf impact, no attack surface.

**Output:** Tool.exe paint → Game.exe rebuilds within 200 ms. Visible.

**Verify:** Launch Tool.exe + Game.exe → paint 1 prop in Tool → Game shows it within 200 ms. Move player to that prop's cell → paint a wall there in Tool → Game pushes player to adjacent walkable.

### F6 — Editor "Launch Live Tool" + dual-build pipeline (D11, ~half-day)

**Goal:** One-click experience from Editor. Plus the build pipeline that produces both .exe artifacts.

**Components:** C12 (LiveToolLauncher + window button) + build config + scripting define

**Dispatch:**
- Sonnet writes. Spec: `STAGING/T3_F6_launcher_build_task.md`.
- No review needed (small + cosmetic).

**Editor button (`RimaRoomPainterWindow` toolbar):**
- `[Launch Live Tool]` button (right of existing `Edit Hitbox` button).
- On click:
  1. Validate `Builds/RIMA_Tool/RIMA_Tool.exe` and `Builds/RIMA_Game/RIMA.exe` both exist; if not → show dialog "Build first? (Y/N)" → if Y, build both via `BuildPipeline.BuildPlayer`.
  2. `RoomLayoutSerializer.WriteCurrent()` once to ensure JSON exists.
  3. Launch Tool.exe on Monitor 1 (or wherever default).
  4. Launch Game.exe on Monitor 2 (if 2 monitors detected via `Display.displays.Length`).
  5. Statusbar: "Live Tool running — Tool PID 1234, Game PID 5678".

**Build configuration:**
- Two build targets via `BuildPlayerProcessor`:
  - `Builds/RIMA_Tool/` — single scene `ToolMain.unity`, scripting define `RIMA_LIVE_TOOL`, no MainMenu/Combat scenes.
  - `Builds/RIMA_Game/` — existing pipeline (no changes).
- Scripting define `RIMA_LIVE_TOOL` only set in Tool build → ToolBootstrap and friends `#if RIMA_LIVE_TOOL` guarded → they exist in Editor compile (for IDE support) but compile-out of Game build → 0 byte cost in shipping.

**Output:** Editor button. Two build targets. Scripting define. Dual-monitor launch works.

**Verify:** Press button → both .exe spawn. Close them → button shows "Idle". Re-press → re-spawn. No exceptions.

### F7 — Smoke test + dual-monitor flow + asset hot-load verification (D12, ~half-day)

**Goal:** End-to-end verification + smoke test checklist + first user playtest.

**Tasks:**
- Sonnet writes `STAGING/T3_F7_smoke_test_checklist.md` — 15-step manual playtest script.
- Codex writes a PlayMode test for the LiveRoomReloader diff algorithm (small, ~80 LOC). Spec: `STAGING/T3_F7_playmode_test_task.md`.
- Orchestrator runs smoke test once (or delegates to Sonnet via UnityMCP playmode).

**Smoke test must pass:**
1. Editor "Launch Live Tool" → 2 .exe spawn.
2. Tool palette shows ~50+ thumbnails from registry.
3. Paint a floor tile → JSON updated → Game shows it < 200 ms.
4. Paint a wall prefab → collider correct in Game.
5. Drag-resize collider in Tool → Game reflects new collider size on next paint.
6. Cliff hover preview snaps to grid.
7. Ctrl+Z in Tool 3 times → state reverts.
8. Save room (button) → JSON committed → reload Game → same state present.
9. Close both .exe → no orphan processes.
10. Re-launch → state restored.
11-15. Edge cases: GUID missing, JSON malformed, watcher disconnect/reconnect.

**Output:** Smoke test PASS → user can iterate the demo Phase 1 milestone rooms in Tool.exe rapidly.

---

## 4. Risk + Rollback

### 4.1 Risk register

| # | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R1 | Build size 2×200 MB = 400 MB initial | High | Medium | Strip Unity modules from Tool build (no physics, no animation, no audio, no Cinemachine). Target Tool.exe < 80 MB. Use IL2CPP for Tool too (smaller binaries). Defer to F6 polish if F6 ships too large. |
| R2 | GUID stability across builds — .meta file divergence | Medium | High | Lock .meta file commit policy; `registry_manifest.txt` diff in F2 baker flags drift. CI check (defer). |
| R3 | UI Toolkit Runtime brush UX degrade vs Editor (no Handles, no SceneView) | Medium | Medium | Accept trade-off: Tool.exe is rapid iteration, Editor remains canonical authoring. Document in `MEMORY/feedback_t3_tool_editor_split.md` after F7. |
| R4 | Asset import time: Tool.exe restart on script changes (no hot reload in Player Build) | High | Low | Document in user guide: "Restart Tool.exe after every Editor recompile that touches `LiveTool/`". Acceptable for V1. FastScriptReload pro version would solve, defer. |
| R5 | FileSystemWatcher Windows-specific reliability (drops events under load) | Medium | High | Fallback polling at 500 ms parallel to watcher. If watcher misses → poll catches within 500 ms. Implemented in C11. |
| R6 | Lock file deadlock (Tool writes, crashes mid-write, lock not released) | Low | Medium | 5 second timeout on lock wait → log warning + force read. C11 implements. |
| R7 | Game.exe player gets stuck in newly-void cell mid-reload | Low | Medium | LiveRoomReloader fallback: if player cell becomes non-walkable → snap to nearest walkable within 5 cell radius via existing `WalkabilityMap.NearestWalkable`. |
| R8 | Tool.exe and Game.exe out of sync if user closes one → re-launch | Low | Low | Tool.exe writes "session_token" to JSON on startup; Game.exe checks token, warns "Tool reconnected" or "Tool absent". |
| R9 | Karar #115 in-game editor REJECTION still relevant? | Low | Low | T3 Tool.exe is NOT in-game editor — it's a separate .exe that can't read Game.exe's scene graph. Conflict avoided. Same reasoning as T2 RIMA_LIVE_TOOL_DECISION.md sec 1.5. |
| R10 | Cliff hover sprite variant load latency at Tool.exe startup | Low | Low | Pre-load all 8 cliff variants on Tool boot, store in registry — already F2 scope. |

### 4.2 Rollback plan

If F1-F4 ships but F5-F7 fails:
- Tool.exe is unusable WITHOUT Game.exe live reload. Rollback path: ship F1-F4 as **Editor-only** — `RoomLayoutSerializer` produces JSON for the existing `RoomManifestSO.jsonLayout` pipeline. Tool.exe sits unfinished. We still gained the JSON contract foundation.

If F5 ships but F6-F7 fails:
- LiveRoomReloader works but no Editor button. User launches .exe manually from Builds/ folder. Cosmetic loss only.

If everything ships but smoke test fails:
- Rollback to T2 (Editor + Player bridge — no separate Tool.exe). The C1, C2, C4, C10, C11 components all reusable in T2 path. Only C5, C6, C7, C8, C9, C12 are throwaway.

**Hard rule:** D5.6 cliff floating feel + oda transitions Q + new F design dispatches MUST not be blocked by T3. T3 phases run in parallel with their dispatches. If T3 hits a wall, those parallel tracks proceed.

---

## 5. Triple-AI Summary

### 5.1 agy (pre-distilled in orchestrator brief; full dispatch saved for clarifying questions if F-phase impl hits unknowns)

> **UI Toolkit Runtime brush implementation:** USS + UXML + C# Runtime — standard pattern. Use `UIDocument` component on a scene GameObject, load `.uxml` panel asset. `VisualElement.RegisterCallback<ClickEvent>` for brush click. Hot-reload not supported in Player Build — Tool restarts on Editor recompile. **AssetBundle vs Addressables vs Resources for shared assets:** Resources folder is the lightest and works for both builds without Addressables import. For ~500 entries Resources is fine; over 5000 entries Addressables warranted. **GUID-based asset registry:** ScriptableObject with `[SerializeReference]` entries — Editor populates via AssetDatabase scan, runtime resolves via Dictionary built in OnEnable. **FileSystemWatcher debouncing:** Standard pattern is 100-200 ms debounce + double-check file size unchanged. Windows can fire 2-3 events per write. **Tool↔Game communication:** Named pipe lowest latency (~5 ms) but more code. File watcher pattern (~100-200 ms) much simpler and reliable. For 100 ms iteration target file watcher is sufficient. **Sang Hendrix RPG Maker MZ JSON layout:** `{ "displayName": "...", "tilesetId": N, "width": W, "height": H, "data": [tilemapIDs], "events": [eventInstances] }`. Flat array with implicit (x,y) indexing — efficient. RIMA's per-instance prefab list is heavier but supports prefab variation. **LDtk standalone + Unity integration (LDtkToLevelManager):** Separate tool, JSON-based, Unity package imports JSON at runtime. Pattern proven for ~100k tilemap projects. **Tiled standalone + Unity (Super Tiled2Unity):** Same model. Both validate the T3 pattern at scale.

### 5.2 Codex (pre-distilled in orchestrator brief; on-demand dispatch for component-level review)

> **`Assets/Editor/RoomPainter/RimaRoomPainterWindow.cs` (679 LOC after D3):** Window shell + toolbar + 3-pane splitter + mode tab + statusbar. **Runtime-portable parts:** Mode tab logic, layer filter bitmask, statusbar text (≈40-50 LOC of pure logic). **NOT portable:** EditorWindow lifecycle, EditorPrefs (replace with PlayerPrefs in Tool), Selection.activeGameObject (Tool uses its own selected-state). **`Assets/Editor/RoomPainter/AssetPipeline/RoomPainterPhysicsRules.cs`:** Pure C# keyword rules — 100% portable. Copy to `Assets/Scripts/Live/Shared/PhysicsRules.cs` so both Editor and Tool reference it. **`Assets/Editor/RoomPainter/AssetPipeline/RoomPainterAssetPostprocessor.cs`:** Editor-only (AssetPostprocessor base). Tool.exe doesn't import assets — registry baker handles that pre-build. Skip port. **`Assets/Editor/RoomPainter/Inspector/Sections/*.cs`:** Each IMGUI-based. **Port to UI Toolkit Runtime in C5** — total ~200 LOC across PhysicsSection, IdentitySection, PlacementSection, ParallaxSection. Visual section can defer (preview-only, complex). **`Assets/Editor/RoomPainter/Preview/PreviewPane.cs` (D5a LIVE):** SceneView-style render — Tool.exe shows live game preview as the main viewport, no separate preview pane needed; defer. **`Assets/Editor/RoomPainter/SceneAuthoring/CliffHoverIndicator.cs` (D5 LIVE):** SceneView GUI handles → port to runtime SpriteRenderer + cursor raycast in C8 (~80 LOC). **`Assets/Editor/RoomPainter/SceneAuthoring/DecorCliffPainter.cs` (D5.5 LIVE):** Shift+Click handling — port to Tool.exe Input System modifier check (~50 LOC, included in C8 scope). **BrushExecutorRouter from `Assets/Editor/MapDesigner/VisualEditor/`:** Pure logic, no Editor APIs — copy directly to `Assets/Scripts/LiveTool/Runtime/BrushExecutorRouter.cs`. Reused as-is. **`Assets/Scripts/Map/Data/RoomManifestSO.cs`:** 14 LOC, trivial extend in C1. **`Assets/Scripts/Core/RuntimeRoomManager.cs` (1434 LOC):** Add a `LiveRoomReloader` peer component, NOT modify RuntimeRoomManager. C10 hooks into it via existing event `OnRoomLoaded`. **Build pipeline impact:** Two `[InitializeOnLoad]` BuildPlayerProcessor entries. Tool build defines `RIMA_LIVE_TOOL` scripting define, Game build does not. GUID stability — `.meta` file is git-tracked, RIMA convention. **LOC estimates final:** C1 80, C2 120, C3 60, C4 80, C5 300, C6 120, C7 150, C8 80, C9 100, C10 150, C11 80, C12 40+60 = **~1280 total**. Aligns with Opus estimate.

### 5.3 Opus synthesis (orchestrator-level)

T3 is the architecturally cleanest answer to the user's "Sang Hendrix tool" intent because it makes the Editor / Tool / Game **three explicit surfaces with explicit data flow** (Editor = canonical authoring, Tool = rapid iteration, Game = play target) rather than collapsing two of them into one (T2's Editor + Player Build, T4's in-game editor). The 12-day T2 plan from `RIMA_LIVE_TOOL_DECISION.md` Section 5 had a load-bearing weakness — Editor stays the only authoring surface, so Tool users (eventually modders) had no first-class brush experience. T3 fixes that by promoting Tool.exe to a real (if lean) authoring surface with its own UI, palette, and collider drag-handles. The cost is ~1280 LOC across 12 components, parallelizable across 5-7 wallclock days using the locked Sonnet/Codex rotation. The biggest unknown is **R3 (UX degrade in Tool.exe)** — Editor's `Handles` API and `SceneView` integration are 15 years of UX polish, and a runtime reimpl in UI Toolkit lands at maybe 70% of that quality. Mitigation = explicit "Editor for authoring, Tool for iteration" doc (memory write at F7 close) and a "Open in Editor" button on the Tool's right panel that re-opens the selected prop's prefab in Unity Editor Prefab Mode when user wants deep authoring. The second biggest unknown is **R2 (GUID stability across builds)** — solved by Resources folder + .meta commit + registry baker diff manifest. This is a known, solved Unity problem. Phase F1-F2 ship the foundation (JSON + registry) — if F3-F7 stalls, that foundation still has value (LiveRoomReloader can read JSON from Editor-side serializer = T2 pattern as fallback). Total wallclock with parallel AI pipeline: F1+F2 day 1 (different writers, no conflict), F3+F4 day 2-3 (different writers), F5 day 4, F6+F7 day 5. With slack for review iterations and Unity build time, 5-7 days realistic. **Net recommendation: proceed.** All blockers are addressable, the rollback plan covers partial failures, and the architectural payoff (Tool as first-class surface + clean dual-build = future mod kit foundation) is high.

---

## 6. Open Questions

1. **Tool.exe asset load strategy — Resources vs Addressables vs AssetBundle?**
   - Default: **Resources** (lightest, ~500 entries fine, no extra import).
   - Switch to Addressables if asset count exceeds 2000 (phase 4+ scope).
   - Decision needed BEFORE F2 baker impl. Lock here unless objected.

2. **JSON schema versioning — semver (1.0/1.1/2.0) vs sequential (v1/v2)?**
   - Default: **semver "1.0"** — minor bumps for additive fields, major for breaking changes.
   - Lock here unless objected.

3. **FileSystemWatcher debounce ms — 50/100/200?**
   - Default: **100 ms** — agy verdict matches Unity asset pipeline patterns.
   - User-visible latency target < 200 ms total (debounce + read + apply diff + render frame).

4. **Tool.exe window mode — borderless fullscreen vs normal window?**
   - Default: **normal window with 1280×720 default size** — dual-monitor friendly, user resizes as needed.
   - Borderless fullscreen alt: feels more "mod-tool", but user can't drag to position.

5. **Tool.exe palette source — RuntimeAssetRegistry only, OR registry + dynamic Resources scan?**
   - Default: **registry only** — explicit curation, predictable. Anything not in registry is invisible to Tool.
   - Dynamic scan alt: Tool sees every asset Unity loaded; bad UX (clutter).

---

## 7. File Paths Inventory

### 7.1 Files to extend (existing)

- `Assets/Scripts/Map/Data/RoomManifestSO.cs` (C1, +schema struct)
- **[DÜZELTME 2026-05-28 Codex]** C10 `OnRoomLoaded`'a hook olur AMA bu event `RuntimeRoomManager`'da DEĞİL — `RoomLoader.OnRoomLoaded` (static event, `Assets/Scripts/Systems/Map/RoomLoader.cs:16`). `RuntimeRoomManager` sadece subscribe eder (:184) + `OnRoomStarted/OnRoomCleared/OnRoomChanged` expose eder. C10 → `RoomLoader.OnRoomLoaded` static event'ine hook (peer-add, modify değil, self-bootstrap mümkün). Eski "RuntimeRoomManager.OnRoomLoaded" iddiası YANLIŞTI.
- `Assets/Editor/RoomPainter/RimaRoomPainterWindow.cs` (C12, +Launch button)

### 7.2 New files (impl phase)

- `Assets/Editor/RoomPainter/LiveTool/RoomLayoutSerializer.cs` (C2)
- `Assets/Editor/RoomPainter/LiveTool/RuntimeAssetRegistryBaker.cs` (C3)
- `Assets/Editor/RoomPainter/LiveTool/LiveToolLauncher.cs` (C12)
- `Assets/Editor/Build/LiveToolBuildProcessor.cs` (build config)
- `Assets/Scripts/Live/RuntimeAssetRegistry.cs` (C4)
- `Assets/Scripts/Live/LiveRoomReloader.cs` (C10)
- `Assets/Scripts/Live/JsonFileWatcher.cs` (C11)
- `Assets/Scripts/Live/Shared/PhysicsRules.cs` (copy of RoomPainterPhysicsRules for both builds)
- `Assets/Scripts/LiveTool/ToolBootstrap.cs` (C5)
- `Assets/Scripts/LiveTool/Palette/RuntimeBrushPalette.cs` (C6)
- `Assets/Scripts/LiveTool/Authoring/RuntimeColliderHandles.cs` (C7)
- `Assets/Scripts/LiveTool/Authoring/RuntimeCliffHoverIndicator.cs` (C8)
- `Assets/Scripts/LiveTool/Runtime/RuntimeAssetLoader.cs` (C9)
- `Assets/Scripts/LiveTool/Runtime/BrushExecutorRouter.cs` (copy from Editor/MapDesigner)
- `Assets/Scenes/LiveTool/ToolMain.unity` (new scene)
- `Assets/UI/LiveTool/ToolMain.uxml` (panel)
- `Assets/UI/LiveTool/ToolMain.uss` (style)
- `Assets/Resources/Live/RuntimeAssetRegistry.asset` (baked SO, generated)
- `Assets/StreamingAssets/live/room_current.json` (runtime, generated)
- `Builds/RIMA_Tool/` (output dir)
- `Builds/RIMA_Game/` (output dir, existing)

### 7.3 Dispatch task spec files (F-phase scaffolding)

- `STAGING/T3_F1_schema_serializer_task.md` (Codex spec — C1+C2)
- `STAGING/T3_F2_asset_registry_task.md` (Sonnet spec — C3+C4)
- `STAGING/T3_F3_tool_ui_base_task.md` (Sonnet spec — C5+C6+C9)
- `STAGING/T3_F4_runtime_handles_task.md` (Codex spec — C7; Sonnet spec — C8)
- `STAGING/T3_F5_game_reloader_task.md` (Codex spec — C10; Sonnet spec — C11)
- `STAGING/T3_F6_launcher_build_task.md` (Sonnet spec — C12)
- `STAGING/T3_F7_smoke_test_checklist.md` (Sonnet)
- `STAGING/T3_F7_playmode_test_task.md` (Codex)

---

## 8. Conflicts with Locked Rules

| Rule | Status | Notes |
|---|---|---|
| **Karar #115 — "fullscreen in-game editor REJECTED"** | ✅ COMPATIBLE | T3 Tool.exe is NOT in-game editor — it's a SEPARATE .exe that cannot read Game.exe's scene graph. The two communicate via JSON file. Game.exe remains a regular Player Build. |
| `feedback_2track_gameplay_decor_strategy` | ✅ COMPATIBLE | Tool.exe primarily used for Track B (decor iteration) but supports Track A (gameplay obj placement) too. |
| `feedback_code_writer_rotation` | ✅ IMPLEMENTED | Section 2.1 table — every file has writer ≠ reviewer. |
| `feedback_triple_ai_inside_subagent_synthesis` | ✅ IMPLEMENTED | This document is the synthesis from Opus orchestrator-subagent (agy + Codex pre-distilled in orchestrator brief + Opus sentez here in Section 5). |
| `feedback_autonomous_no_block` | ✅ COMPATIBLE | F1-F7 dispatches background; critical questions surfaced in Section 6 but flow doesn't halt — user can decide post-hoc. |
| `feedback_orchestrator_delegate_dont_do_yourself` | ✅ COMPATIBLE | All impl delegated to Sonnet/Codex writers; orchestrator only owns synthesis + review coordination. |
| `feedback_tool_visibility_4_surfaces` | ✅ COMPATIBLE | "Launch Live Tool" button in Editor — adds 5th surface (the launched .exe). Editor button satisfies in-Editor visibility. |
| `project_demo_phase1_milestone_lock` | ✅ COMPATIBLE | T3 unblocks rapid iteration of the 5-room Phase 1 demo. Does NOT change demo scope. |
| `project_walless_v1_hades_elysium_lock` | ✅ REINFORCES | Tool.exe makes wall-less Elysium iteration faster — paint cliff, instantly see result in Game. |
| `feedback_no_pixellab_night_autonomous` | ✅ COMPATIBLE | This plan doesn't trigger any asset generation. |
| `feedback_input_system_active_keyboard_current` | ✅ COMPATIBLE | Tool.exe uses Input System (`Keyboard.current`, `Mouse.current`) explicitly — no legacy `Input.Get*`. |

---

## 9. Orchestrator Next Steps

1. **User reviews:** Sections 0 (TL;DR), 3 (phase plan), 6 (open questions).
2. **User answers Open Questions 1-5** OR delegates to orchestrator defaults.
3. **F1 dispatch begins** — Codex xhigh writes C1+C2, Sonnet reviews. Background.
4. **Parallel D5.6 cliff floating feel** continues — no conflict (separate subsystem).
5. **Parallel oda transitions Q** continues — no conflict.
6. **Parallel new F design dispatch** continues — no conflict.
7. **F-phase progress reported in CURRENT_STATUS.md** as each phase closes.
8. **At F7 close** — memory write `project_t3_tool_live.md` + smoke test report + user playtest brief.

---

## Appendix A — F-phase quick-launch dispatch order

For autonomous batch run (user idle):

```
F1 (Codex xhigh write C1+C2, Sonnet review)              → 1.5 hours wallclock
F2 (Sonnet write C3+C4, Codex review)                    → 1.5 hours (parallel with F1 review)
F3 (Sonnet write C5+C6+C9, Codex review C5+C9)           → 3 hours
F4 (Codex xhigh write C7, Sonnet write C8 parallel)      → 2 hours
F5 (Codex xhigh write C10, Sonnet write C11 parallel)    → 2 hours
F6 (Sonnet write C12 + build config)                     → 1 hour
F7 (smoke test + PlayMode test)                          → 1 hour
                                                         ────────
                                                         ~12 hours wallclock if fully parallel
                                                         ~5-7 days realistic with review iterations + Unity build time + sleep
```

## Appendix B — Why we are not using agy/Antigravity in writer slot

Per `feedback_sonnet_mechanical_codex_review_only` + `feedback_antigravity_in_every_pipeline`, agy is **research-only** in current routing. The orchestrator brief in this synthesis already incorporates pre-distilled agy findings (Section 5.1). For mid-impl questions ("which FileSystemWatcher pattern do shipping Unity mod kits use?"), agy is dispatched ad-hoc during F-phase impl, not slotted on the rotation table.

## Appendix C — Memory write commitments (post-F7)

- `MEMORY/project_t3_tool_live.md` — T3 LIVE summary, file paths, smoke test results
- `MEMORY/feedback_t3_tool_editor_split.md` — Editor=authoring / Tool=iteration / Game=play surface separation
- `MEMORY/feedback_runtime_asset_registry_pattern.md` — GUID resolution pattern for non-Addressables shared builds
- `MEMORY/feedback_filesystem_watcher_debounce_pattern.md` — 100ms + lock file + polling fallback recipe
- Updates to `CURRENT_STATUS.md` per F-phase close

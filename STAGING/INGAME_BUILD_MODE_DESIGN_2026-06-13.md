# IN-GAME BUILD MODE — DESIGN + PHASED PLAN (2026-06-13)

> Lead-designer synthesis. Investigation + research only; nothing under `Assets/` was modified.
> SCOPE GUARD: today is DEMO DAY. Assets/ is FROZEN. This doc is design-only; nothing here ships today.
> ASCII-safe (no Turkish diacritics in body).

---

## 1. AMAC / GOAL

Give RIMA an in-game, live, WYSIWYG **Build Mode**: press one key, the camera pulls back
(Sims-style "enter build"), gameplay pauses, and the designer places/paints rooms ON TOP OF
the running game (same renderer, same lights, same Y-sort the player sees). Edits write back
into the room data the game already round-trips, so an edited room is a real room.

Positioning vs the commercial reference (Sang Hendrix "Realtime Parallax Map Builder"):
RIMA already does live-WYSIWYG-over-the-running-game, AND adds what that hand-placement tool
lacks: procedural generation, semantic composition roles, footprint validation,
walkability/solvability checks, and runtime telemetry. RIMA = the rules-aware sibling.

Why it is cheap: ~70% of the runtime editor already exists inside `DirectorMode.cs`
(palette -> cursor ghost -> grid-snap -> LMB place / RMB erase -> free-cam -> UI-hover guard,
plus a tested `*ForValidation` API). Build Mode is mostly EXTEND, not rebuild.

---

## 2. REFERENCE TAKEAWAYS (with source URLs)

From Sang Hendrix "Realtime Parallax Map Builder" (RPG Maker MZ plugin):
- Edit-in-place over the LIVE game is the headline value: editor IS the game + overlay, zero
  editor-vs-runtime fidelity gap. Keep Build Mode as a runtime overlay (RIMA already plans this).
- Tiny first-success loop (create layer -> import -> see it) lowers the barrier; RIMA target =
  pick prop -> click -> it is there, lit, Y-sorted.
- Mode hygiene: on-screen toggles for grid/collision overlays; auto-hide editor-only visuals on
  exit; mutually-exclusive tools auto-deselect (collision brush vs object) so you never paint the
  wrong layer.
- Collision Brush: paint impassable cells red, auto-hidden at runtime. RIMA can go further: feed
  the real `WalkabilityMap` + run `IsReachableFromPlayer` solvability check.
- Save/transient discipline: authored data persists, editor-only state (collision viz, command
  layers) must NOT bloat the save file.
- Performance warning from their bug log: freehand placement invites thousands of objects (they
  had to retrofit 10k-object optimization). Design for high counts + a live count from the start.

Source URLs:
- https://sanghendrix.itch.io/realtime-parallax-map-builder-rpg-maker-mz-plugin
- https://sanghendrix.itch.io/realtime-parallax-map-builder-rpg-maker-mz-plugin/devlog/1039804/version-101-new-feature-collision-brush
- https://sanghendrix.itch.io/realtime-parallax-map-builder-rpg-maker-mz-plugin/devlog/1533475/version-120-a-huge-feature-tilemap-grid-free-painting

Cross-genre patterns (Sims build/buy, Dead Cells, Hades, Unity runtime tilemap):
- Commit model = hover ghost -> validate (green/red) -> click to place. Validity color is the
  primary feedback channel. (Sims) https://sims.fandom.com/wiki/Build_mode_(The_Sims_4)
- Snap resolution + grid visibility are runtime toggles, not build constants; rotation has two
  tiers (snapped 90deg + free with modifier); eyedropper/clone is the fastest authoring loop;
  undo/redo is mandatory (command stack, not state snapshots).
- Composition lesson (Dead Cells / Hades): separate the structure graph from room content;
  randomize spawns/treasure over fixed, hand-authored layouts.
  https://deepnight.net/tutorial/the-level-design-of-dead-cells-a-hybrid-approach/
  https://kotaku.com/hades-level-design-is-less-random-than-it-seems-1845254545
- Unity persistence: you CANNOT JsonUtility a Tilemap/TileBase directly (returns null). Persist by
  iterating cellBounds into a list of {Vector3Int, tileId} resolved through a registry on load.
  Keep a stable string/int ID -> prefab registry; never serialize Unity object refs.
  https://medium.com/@allencoded/unity-tilemaps-and-storing-individual-tile-data-8b95d87e9f32
  https://docs.unity3d.com/ScriptReference/Tilemaps.Tilemap.SetTile.html
- Avoid `com.unity.runtime-scene-serialization` (whole-scene, IL2CPP Stub overhead) — RIMA only
  needs per-room prop/socket lists, the lighter ID + JsonUtility-on-plain-list path is correct.

---

## 3. CURRENT-INFRA REUSE MAP (what RIMA already has)

| RIMA system | File | Reuse for Build Mode |
|---|---|---|
| Runtime placement loop (palette/ghost/snap/place/erase/free-cam/UI-guard + *ForValidation) | `Assets/Scripts/UI/DirectorMode.cs` | ~70% of the editor UX. Extend, do not rebuild. |
| Room data / SAVE model (props, walkableGrid, overlayMask, backgroundLayers, lightingProfile, IsWalkable) | `Assets/Scripts/MapDesigner/Room/Data/RoomTemplateSO.cs` | Edits write straight into these arrays -> zero new save format. |
| Room renderer + tile<->world helpers (Build(), TryGetCellCenterWorld, SetTile on ground/overlay) | `Assets/Scripts/MapDesigner/Room/Runtime/IsoRoomBuilder.cs` | Re-materialize/preview edits; Grid-cell ghost snap; tile brush target. NOTE ClearPrevious is full-clear -> incremental edits need a new per-cell path. |
| Typed place-legality + footprint rotation | `Assets/Scripts/MapDesigner/Props/PropFootprintValidator.cs` | Drives green/red ghost; GetRotatedFootprint for rotate-while-placing. |
| Seeded auto-scatter + tile role labels | `Assets/Scripts/MapDesigner/Props/Auto/BridsonPoissonAutoPlacer.cs`, `RoomDecorationPass.cs`, CompositionRoleMap | One-click "fill/scatter" brush; paintable-vs-protected tiles. |
| Correct runtime prop instantiation (Y-sort/collider/variant/rotation) + catalog | `Assets/Scripts/MapDesigner/Props/Runtime/PropRuntimeSpawner.cs`, PropSorterRuntime, PropColliderAutoBuilder, PropDefinitionSO, PropRegistrySO | Replace DirectorMode bare Instantiate; PropRegistrySO.AllProps = palette source. |
| Walkability authority + solvability | `Assets/Scripts/Environment/WalkabilityMap.cs` | Refresh after edits (InitFromTemplate / tilemapTileChanged); IsReachableFromPlayer = "is room still solvable?" |
| Camera rig | `Assets/Scripts/Camera/CameraZoom.cs`, `CameraFollow.cs` | NearestCrispZoom / ApplyPixelPerfect for crisp zoom; CameraFollow.SetBounds/SnapToTarget. |
| Zoom-out-then-restore PRECEDENT (shipped) | `Assets/Scripts/UI/ChamberSelectBootstrap.cs` (~1454-1483 enter, ~199-202 restore) | Copy verbatim: disable CameraZoom + URP PixelPerfectCamera, override ortho size, re-target CameraFollow, restore on exit. |
| Input | New Input System (`Keyboard.current.quoteKey`) | Same precedent as DirectorMode backquote toggle; quote key is FREE (not in KeyBindManager.Reserved). |
| Serializer schema REFERENCE (Editor-only) | `Assets/Editor/RoomPainter/LiveTool/RoomLayoutSerializer.cs` | Reuse SCHEMA SHAPE only; it uses AssetDatabase/GlobalObjectId, cannot run in a player build. |

GAPS (net-new work): runtime tile/light painting, validity-tinted ghost in the live tool,
runtime rotation control, undo/redo, selection/move/inspect, full PropRegistry palette,
Grid-cell footprint snapping, and a RUNTIME-SAFE save/load.

---

## 3.5 TILE GEOMETRY & PLACEMENT CORRECTNESS (BINDING — read before ANY placement code)

VERIFIED from the live _Arena scene (2026-06-13), not assumed:
- Grid 'IsoGrid': cellLayout = **Isometric**, cellSize = (0.96, 0.59, 1.0), swizzle XYZ.
- Tilemaps Ground/Collision: orientation XY, tileAnchor (0.5, 0.5), renderer sortOrder = BottomLeft, layer Floor.
- Current floor tile floor451_0: **64x64 SQUARE sprite, PPU 64** (top-down 3/4 ART, NOT a 2:1 diamond-iso drawing).
- Main camera: orthographic, size ~5.67, rotation (0,0,0) -> FLAT 2D; the 3/4 look comes from the sprite art, NOT camera tilt.

So RIMA is a "fake-iso / 3/4 staggered" hybrid: ISOMETRIC placement grid + square top-down 3/4 sprites + flat ortho camera. Matches canon "HIGH TOP-DOWN 3/4, no true 45-degree diamond ART".

HARD RULES for every Build Mode placement / brush (this is exactly where naive builders make tiles look nonsensical):
1. NEVER compute world positions with rectangular math (cellX*size, cellY*size). The cell<->world mapping is ISOMETRIC.
2. Mouse -> cell: ALWAYS grid.WorldToCell(mouseWorld). Cell -> world: ALWAYS grid.GetCellCenterWorld(cell) / grid.CellToWorld(cell).
3. Tiles: Tilemap.SetTile(cell, tile) (respects iso layout automatically); erase = SetTile(cell, null). Do not place tile GameObjects by hand-computed transforms.
4. Ghost preview snaps to grid.GetCellCenterWorld(cell), never to a raw rectangular world point.
5. Props: keep PropRuntimeSpawner + PropSorterRuntime (Y-sort by BottomLeft / iso depth). Do not bypass with bare Instantiate at hand-computed positions.
6. Footprint + rotation: use PropFootprintValidator.GetRotatedFootprint over iso-adjacent cells; validity ghost green/red from that + IsWalkable.
7. Multi-cell footprints occupy iso-adjacent cells, NOT a rectangular block in world space.
8. Sorting: respect renderer sortOrder = BottomLeft and the Floor/overlay sorting layers; do not introduce a different sort basis.

If placement ever looks scattered/misaligned, the cause is almost always rectangular math instead of the Grid API (rules 1-2). Every builder brief for Phase 2+ MUST quote this section.

---

## 4. CHOSEN DESIGN

**Hybrid: Approach 1 (camera-zoom wrapper over the existing Build tab) as the demo-safe core,
then grow into Approach 3 (Runtime RoomTemplateSO Editor) as the full feature.**

Rationale:
- Approach 1 is the smallest shippable wow-moment and is almost pure reuse (camera wrapper +
  keybind + tab-force). It is the right Phase 1 because the underlying loop is already
  `*ForValidation`-tested and the camera flow is shipped twice (CameraZoom + ChamberSelect).
- Approach 3 is the better END STATE than Approach 2 (full Sims editor) because it is
  reuse-MAXIMAL and explicitly persists into `RoomTemplateSO` (zero new save format for the core
  path), and it is sliceable: validity ghost + rotation + PropRegistry palette + undo land before
  the hard parts (tile/light painting, runtime-safe serializer). Approach 2's value is real but
  its XL scope (full selection/move/inspect + heavy persistence) is not justified yet.
- So: ship the Approach-1 wrapper, then incrementally fill Approach-3 gaps. Same entry gesture,
  same data model, no throwaway code.

### Key interactions (target end state)
- Quote key `"` (`Keyboard.current.quoteKey.wasPressedThisFrame`) enters/exits Build Mode; sibling
  to the existing backquote Director toggle in `DirectorMode.Update()`. Backquote stays the raw
  Director toggle; quote is the polished Build-Mode alias that also drives the camera + Build tab.
- On enter: capture {orthographicSize, CameraFollow.target, worldOffset}; disable CameraZoom + URP
  PixelPerfectCamera; lerp ortho size to a wider build framing. SetState(Director) (timeScale 0,
  player input off). On exit: lerp back, land on NearestCrispZoom crisp ratio, re-enable PPC +
  CameraZoom, CameraFollow.SnapToTarget().
- WASD / middle-mouse-drag = free-cam pan (existing UpdateFreeCamera).
- Palette pick -> translucent pulsing ghost follows cursor, snapped to Grid cell center
  (TryGetCellCenterWorld), tinted GREEN if PropFootprintValidator + IsWalkable pass, RED otherwise.
- LMB place / RMB erase; hold-LMB = continuous paint for tile/walkability/scatter brushes;
  single-click = discrete props. `[` / `]` rotate 90deg, Alt = free yaw; `F` flipX.
- `E` eyedropper/clone; `G` grid toggle; snap-resolution cycle; Ctrl+Z / Ctrl+Y undo/redo.
- Tool tabs (mutually exclusive, auto-deselect): Prop | Tile-paint | Walkable/Collision | Light |
  Auto-scatter | Eyedropper.
- "Auto-Scatter" = BridsonPoissonAutoPlacer over CompositionRole-eligible tiles, preview/accept.
- "Validate Solvable" = WalkabilityMap.IsReachableFromPlayer.
- SAVE / LOAD / REVERT = runtime serializer (PropRegistry IDs -> JsonUtility), LOAD via
  IsoRoomBuilder.Build(reconstructedTemplate).
- QUICK RESET bottom strip (existing DemoQuickReset) = safe "undo everything" for live demo.

---

## 5. PHASED PLAN (file-level)

### Phase 0 — DEMO-DAY SAFE: docs + verification only (demo_safe: TRUE)
NO code. Today only.
- This design doc (`STAGING/INGAME_BUILD_MODE_DESIGN_2026-06-13.md`).
- Run the runtime checks in section 7 IN THE EDITOR ONLY (play mode is allowed by you only if/when
  the demo-day freeze lifts; the orchestrator's task said do NOT enter play mode today, so this is
  a checklist to run post-demo, not now).
- Files: this doc only. Nothing under `Assets/`.

### Phase 1 — Camera-zoom Build-Mode wrapper (Approach 1) (demo_safe: FALSE; post-demo)
Scope: turn the existing Build tab into a first-class "Build Mode" with a cinematic entry.
- ADD `Assets/Scripts/UI/BuildModeController.cs` (or a method block inside DirectorMode):
  EnterBuildMode()/ExitBuildMode() = capture -> disable CameraZoom + PixelPerfectCamera -> lerp
  ortho to buildOverviewZoom -> on exit lerp back + NearestCrispZoom snap + restore. Pattern copied
  from `ChamberSelectBootstrap.cs`.
- CHANGE `Assets/Scripts/UI/DirectorMode.cs`: add quote-key branch in Update() (alongside backquote)
  -> toggle wrapper + force ActiveTab = DirectorTab.Build.
- (Optional polish, droppable) CHANGE DirectorMode UpdatePropGhost: tint ghost green/red via
  `RoomTemplateSO.IsWalkable` + overlap test (visual only; does not yet block place).
- Reuse: CameraZoom.NearestCrispZoom/ApplyPixelPerfect, CameraFollow.SnapToTarget, DirectorMode
  Build tab + DemoQuickReset.
- Verification: quote key zooms out, pauses, Build palette shows; quote again restores crisp framing
  with no PixelPerfect pop.

### Phase 2 — Validity ghost + rotation + PropRegistry palette + undo (Approach 3, slice A) (demo_safe: FALSE)
Scope: real authoring-grade placement on top of Phase 1.
- CHANGE `Assets/Scripts/UI/DirectorMode.cs`: drive palette from `PropRegistrySO.AllProps`
  (categories + search + recently-used) instead of hardcoded directorPlaceableProps + rift_crystal;
  snap to Grid cell center via `IsoRoomBuilder.TryGetCellCenterWorld`; wire ghost validity to
  `PropFootprintValidator.Validate` + `GetRotatedFootprint`; route placement through
  `PropRuntimeSpawner` (Y-sort/collider/variant) instead of bare Instantiate.
- ADD `Assets/Scripts/UI/BuildMode/BuildCommandStack.cs`: IBuildOp Do/Undo for place/erase/rotate;
  wire Ctrl+Z / Ctrl+Y.
- ADD rotation/flip ghost controls (`[` `]` Alt, `F`) + `E` eyedropper.
- Reuse: PropFootprintValidator, PropRegistrySO, PropRuntimeSpawner, IsoRoomBuilder helpers.

### Phase 3 — Runtime tile + walkability/collision brushes (Approach 3, slice B) (demo_safe: FALSE)
Scope: paint floors and impassable cells live.
- ADD `Assets/Scripts/UI/BuildMode/BuildModeTileBrush.cs`: per-cell SetTile/erase over
  IsoRoomBuilder groundTilemap/overlayTilemap writing `overlayMask` (the missing incremental path;
  IsoRoomBuilder.ClearPrevious is full-clear). Hold-to-paint.
- ADD `Assets/Scripts/UI/BuildMode/BuildModeWalkBrush.cs`: paint red impassable cells into
  `walkableGrid` (auto-hidden in play) + refresh `WalkabilityMap.InitFromTemplate` + "Validate
  Solvable" button -> `IsReachableFromPlayer`.
- Reuse: IsoRoomBuilder.SetTile, RoomTemplateSO.overlayMask/walkableGrid, WalkabilityMap.

### Phase 4 — Light tool + auto-scatter + runtime-safe save/load (Approach 3, slice C) (demo_safe: FALSE)
Scope: lighting, procedural fill, persistence.
- ADD `Assets/Scripts/UI/BuildMode/BuildModeLightTool.cs`: spawn/move/edit Light2D, write into a
  RoomLightingProfile payload (IsoRoomBuilder.ApplyLighting is currently build-time only).
- CHANGE Build panel: "Auto-Scatter" button -> `BridsonPoissonAutoPlacer` over CompositionRole
  tiles, preview/accept.
- ADD `Assets/Scripts/MapDesigner/Room/Runtime/RuntimeRoomSerializer.cs`: runtime-safe JSON of the
  edited RoomTemplateSO keyed on `PropRegistrySO` string IDs (NOT GUIDs / AssetDatabase); save to
  persistentDataPath / StreamingAssets; load via `IsoRoomBuilder.Build()`. Schema SHAPE mirrors
  `RoomLayoutSerializer.cs` but with no Editor APIs.
- Reuse: BridsonPoissonAutoPlacer, CompositionRoleMap, RoomLayoutSerializer (schema only).

### Phase 5 — Selection/move/inspect + pooling + live count (Approach 2/3 polish) (demo_safe: FALSE)
Scope: edit existing placements; performance.
- ADD `Assets/Scripts/UI/BuildMode/SelectionTool.cs`: click-select placed instance, drag-to-move,
  re-rotate, delete, property panel.
- CHANGE placement to pool/batch + show a live object count (pre-empt the reference's 10k retrofit).
- ADD `Assets/Scripts/UI/BuildMode/BackdropLayerPanel.cs`: reorder + opacity over backgroundLayers.

---

## 6. DEMO-TOOLS VERDICT (from the audit)

WORK STATUS = "wired-untested" for 6 real tools; 1 claimed tool MISSING. Code paths are complete
and all dependency APIs/resources were verified to exist, but NO play-mode confirmation (scope).

| Tool | Status |
|---|---|
| Director Mode toggle (backquote) | wired-untested |
| Stat presets (Tank/Glass Cannon/Default) | wired-untested |
| Enemy Spawn (palette + ghost + place/erase, cap 10) | wired-untested |
| Telemetry / DPS / TTK + CSV export | wired-untested |
| Prop placement (Build tab, rift_crystal confirmed) | wired-untested |
| Dual-Class Draft button | wired-untested |
| Quick Reset | wired-untested |
| Class & Skill assign (Q/E/R/F + LMB/RMB rebind) + free-cam | wired-untested |
| **Light placement** | **MISSING** (no light tab/handler anywhere; Map tab is a "coming soon" placeholder) |

CRITICAL CAVEAT: the WHOLE of DirectorMode is gated by
`#if DEMO_BUILD || DEVELOPMENT_BUILD || UNITY_EDITOR` (DirectorMode.cs line 1, confirmed). If the
actual demo build defines none of those AND is not in-editor, the ENTIRE Director Mode (and every
demo tool) is compiled out. VERIFY the demo build/editor defines one of these before relying on it.

---

## 7. RUNTIME CHECKS STILL NEEDED (run in play mode post-demo, not today)

1. Confirm the build/editor defines DEMO_BUILD / DEVELOPMENT_BUILD or runs in-editor (else all
   Director tools are compiled out). HIGHEST PRIORITY.
2. Backquote `: overlay appears, Time.timeScale -> 0, player input disabled, WASD free-cam pans.
3. Stat presets: TANK / GLASS CANNON / DEFAULT move sliders, stat toast, player HP/damage actually
   change in Test mode (timeScale 1).
4. Enemy Spawn: green ghost follows cursor, LMB spawns (pop + VFX), RMB erases, cap stops at 10,
   CLEAR works.
5. Telemetry: deal damage -> DPS/TTK/event-count update, source bars fill, EXPORT CSV to clipboard,
   DPS freezes during Director pause.
6. Prop placement: rift_crystal + inspector props appear, ghost shows, LMB place / RMB erase / CLEAR.
7. Dual-Class Draft: visible only when no secondary class; click -> overlay hides, ClassSelectionUI
   opens on top (not occluded); after choosing secondary, button disappears.
8. Quick Reset: damage player + spawn + place props -> QUICK RESET -> HP full, death cancelled,
   spawns/props cleared, timeScale 1, player re-activated.
9. Class & Skill: select class (bypass unlock), assign skill to Q/E/R/F, rebind LMB/RMB; confirm the
   new skill/binding fires in-game.

---

## 8. RISKS

- DEMO-DAY freeze: ANY code touches Assets/ and is forbidden today. Phases 1-5 are post-demo.
- Compile-symbol dependency: if the shipped build lacks DEMO_BUILD/DEVELOPMENT_BUILD/UNITY_EDITOR,
  Build Mode is compiled out (same gate as DirectorMode).
- wired-untested baseline: the underlying Build tab loop has never been confirmed in play mode; run
  check 6 before trusting it in a live demo.
- PixelPerfect pop (PPU 64): smooth zoom REQUIRES the PixelPerfectCamera disabled during the glide
  and the final zoom landing on a crisp screenHeight/(180*zoom) integer ratio. Reuse
  CameraZoom.NearestCrispZoom; do not skip it.
- Two ortho-size writers (CameraZoom + RoomRunDirector) must be coordinated -> disable CameraZoom
  while Build Mode is active (the ChamberSelect pattern handles this).
- Edits lost on room transition: director-placed objects are runtime-only and wiped by
  IsoRoomBuilder.ClearPrevious on the next BuildCurrentRoom unless written into the template/save
  payload first (Phase 4).
- Runtime persistence is the hard part: RoomLayoutSerializer is Editor-only; needs an ID-keyed JSON
  rewrite. Tilemap pitfall: cannot JsonUtility a Tilemap/TileBase (returns null) -> iterate cells
  into {position, tileId} via a registry.
- Walkability desync: tile/blocking edits must refresh WalkabilityMap or movement/dash desyncs.
- Object-count blowup from hold-to-paint without pooling -> frame drops (Phase 5 pooling + live count).

---

## 9. DEMO-DAY GUIDANCE (today)

- DO NOTHING to Assets/ today. Use the existing Director tools as-is.
- If the hoca compares RIMA to commercial editors: frame it as "same live-WYSIWYG philosophy as
  paid RPG Maker plugins, but with procedural generation, semantic composition roles, footprint
  validation, walkability/solvability checks, and runtime telemetry on top" — RIMA is the
  more functional, rules-aware sibling of a hand-placement parallax builder.
- Do NOT demo Light placement (it does not exist). Demo prop placement (rift_crystal) on the Build
  tab; reset with QUICK RESET before/after.
- The cinematic zoom-out "enter build" gesture is the planned wow-moment but is Phase 1 (post-demo).
  Today, present the Build tab as part of the Director overlay.

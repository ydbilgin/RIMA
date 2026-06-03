RIMA portal/preview feasibility consult

Live path note:
- The scene-backed live room loader is `Assets/Scripts/Systems/Map/RoomLoader.cs` (its guid is present in `Assets/Scenes/Test/PlayableArena_Test01.unity`); `Assets/Scripts/Map/Runtime/RoomLoader.cs` is explicitly obsolete.
- There are parallel transition systems. `Systems/Map/RoomLoader` uses sequence data, gates, environment `MapFragment`, and `RoomTransitionFX`. `RuntimeRoomManager` + `DoorTrigger` is the older graph-door path and still contains useful `DungeonGraph.Navigate` and camera-bound code, but do not treat it as the current demo spine without checking scene ownership.
- There are two `MapFragment` classes: `RIMA.Environment.MapFragment` is used by the current `Systems/Map/RoomLoader` reward/draft loop; `RIMA.MapFragment` in Core calls `DungeonGraph.RevealAhead`.

Q1. Portal-as-rift
VERDICT: feasible.
Recommendation: Extend `Assets/Scripts/Environment/Portal.cs` surgically: add `DoorDirection graphDirection`, `int targetNodeId`, `Color portalColor`, optional `SpriteRenderer centerSwirl`, and a `Configure(direction, targetNodeId, destination, color)` method. Then change `PortalSpawnController.SpawnPortals` to source `DungeonGraph.Instance.CurrentNode.exits` so portal count equals branch count, instead of `RoomTypeData.PickPortalCount`.
For URP 2D, least-effort/on-brand is one static rift sprite plus a child animated sprite sheet for the center swirl, with optional tiny pixel ParticleSystem sparks. Avoid Shader Graph for the demo; it is more setup/debug cost and can fight pixel readability. Use #00FFCC as the common rift accent, but tint center/glow by destination type.
Key risk: current `PortalSpawnController` picks random destination types from room category, not actual graph exits, so visual portals will lie unless binding is changed.
Effort: M.

Q2. Morph-into-orb + teleport + camera-follow
VERDICT: feasible, but risky if it tries to replace room loading in one pass.
Recommendation: Add a small `PortalTravelController`/bridge subscribed to `Portal.OnEntered`. On enter: guard double-trigger, disable `PlayerController`, hide player visual renderers, spawn a separate orb GO tinted from `portal.portalColor`, tween player/orb into the portal, then call the room swap while `RoomTransitionFX` briefly masks the hard cut.
Use a separate orb GO, not sprite-swap, because the player likely has multiple renderers/weapon/VFX children and hiding that visual root is safer than mutating animation state. Camera can keep following the player if teleport happens while black; for the cinematic pull-in, temporarily set `RIMA.CameraSystem.CameraFollow.target` to the orb, then restore it to the player after the new-room spawn.
Graph binding is the missing hook: `RuntimeRoomManager.OnPlayerEnteredDoor` already does `DungeonGraph.Navigate(direction)` then transition, but the live `Systems/Map/RoomLoader` only has `LoadNext/JumpToRoom` over a linear `_sequence`. Add a graph-aware loader entry point or a node-id to sequence-data mapping before using portals as real branch choices.
Key risk: without graph-aware `RoomLoader`, portal choice is cosmetic and still advances linearly.
Effort: M for fade-backed orb travel; L for fully visible camera-follow-through into a diegetic neighbor island.

Q3. Map-fragment mechanic
VERDICT: keep and repurpose.
Recommendation: Do not remove it for the demo, because current `Systems/Map/RoomLoader.SpawnFragmentThenDraftUnlock` uses `RIMA.Environment.MapFragment.OnAnyFragmentPickedUp` to start draft and unlock the gate. Instead, repurpose the pickup as "reveal charge": after pickup, call `DungeonGraph.RevealAhead(revealSteps)` from the current environment fragment flow or a bridge listener, then refresh whichever preview UI/world preview replaces the node map.
If real previews replace the abstract map, fragments should reveal more next-room previews, reveal one extra depth beyond current exits, or upgrade preview detail (room type first, then contents like elite/treasure/focal hazard). This maps cleanly onto existing `RoomNode.revealed`, `RevealAhead(steps)`, and `GetRevealedStepsAhead()`.
Key risk: fragment currently carries two meanings in different systems: draft/unlock in `Environment.MapFragment`, graph reveal in `Core.MapFragment`. Merge the behavior or rename the reward pickup concept before polish.
Effort: S for "pickup reveals more previews"; M if it also reveals hidden room contents with new per-node metadata.

Q4. Real-room-preview instead of overhead map
VERDICT: feasible for demo as thumbnails/faked diegetic previews; risky as live RenderTexture rooms.
Option A, live RenderTexture mini-cameras: technically feasible but not demo-realistic first. It needs pre-instantiated next-room instances, preview-only layers, lighting/camera isolation, and RenderTexture UI/world quads. This is M/L and will expose lifecycle issues in room spawning.
Option B, pre-rendered thumbnails: best demo path. Add a preview sprite/thumbnail field or lookup per `RoomSequenceData`/room type, then for each `DungeonGraph.CurrentNode.exits` create one preview panel/quad. Portal i stores the same `DoorDirection`/target node id as preview i, so selection and visual preview cannot drift.
Option C, diegetic previews floating in the void: best final direction, but demo should fake it first with world-space thumbnail quads or low-detail preview prefabs positioned beside the island. Full real adjacent islands need camera framing, collision isolation, active-room boundaries, and loading policy. `DungeonWorldBuilder` already proves a graph-to-world-offset idea exists, but it paints all nodes into one tilemap world and is not yet the floating-island branch-preview system.
Key risk: generating "genuine next rooms" before the choice means the loader must deterministically create/bind rooms by node id; current live loader is sequence-indexed.
Effort: B = S/M; A = L; C fake = M, full = L.

RECOMMENDED OVERALL DIRECTION
For the next demo, replace directional doors with graph-bound rift portals, keep `RoomTransitionFX` as a brief safety mask, and implement orb travel as a layered cinematic on top of the existing room swap. Use pre-rendered or faked diegetic next-room preview quads bound to `DungeonGraph.CurrentNode.exits`; do not build live RenderTexture preview rooms first. Repurpose map fragments into reveal-depth/preview-detail pickups through `DungeonGraph.RevealAhead`. The single biggest user decision: should the demo show previews as clean side-by-side UI panels first, or spend extra effort making them diegetic floating islands in the void immediately?

RIMA portal/preview feasibility consult

Live path note:
- The scene-backed live room loader is `Assets/Scripts/Systems/Map/RoomLoader.cs` (its guid is present in `Assets/Scenes/Test/PlayableArena_Test01.unity`); `Assets/Scripts/Map/Runtime/RoomLoader.cs` is explicitly obsolete.
- There are parallel transition systems. `Systems/Map/RoomLoader` uses sequence data, gates, environment `MapFragment`, and `RoomTransitionFX`. `RuntimeRoomManager` + `DoorTrigger` is the older graph-door path and still contains useful `DungeonGraph.Navigate` and camera-bound code, but do not treat it as the current demo spine without checking scene ownership.
- There are two `MapFragment` classes: `RIMA.Environment.MapFragment` is used by the current `Systems/Map/RoomLoader` reward/draft loop; `RIMA.MapFragment` in Core calls `DungeonGraph.RevealAhead`.

Q1. Portal-as-rift
VERDICT: feasible.
Recommendation: Extend `Assets/Scripts/Environment/Portal.cs` surgically: add `DoorDirection graphDirection`, `int targetNodeId`, `Color portalColor`, optional `SpriteRenderer centerSwirl`, and a `Configure(direction, targetNodeId, destination, color)` method. Then change `PortalSpawnController.SpawnPortals` to source `DungeonGraph.Instance.CurrentNode.exits` so portal count equals branch count, instead of `RoomTypeData.PickPortalCount`.
For URP 2D, least-effort/on-brand is one static rift sprite plus a child animated sprite sheet for the center swirl, with optional tiny pixel ParticleSystem sparks. Avoid Shader Graph for the demo; it is more setup/debug cost and can fight pixel readability. Use #00FFCC as the common rift accent, but tint center/glow by destination type.
Key risk: current `PortalSpawnController` picks random destination types from room category, not actual graph exits, so visual portals will lie unless binding is changed.
Effort: M.

Q2. Morph-into-orb + teleport + camera-follow
VERDICT: feasible, but risky if it tries to replace room loading in one pass.
Recommendation: Add a small `PortalTravelController`/bridge subscribed to `Portal.OnEntered`. On enter: guard double-trigger, disable `PlayerController`, hide player visual renderers, spawn a separate orb GO tinted from `portal.portalColor`, tween player/orb into the portal, then call the room swap while `RoomTransitionFX` briefly masks the hard cut.
Use a separate orb GO, not sprite-swap, because the player likely has multiple renderers/weapon/VFX children and hiding that visual root is safer than mutating animation state. Camera can keep following the player if teleport happens while black; for the cinematic pull-in, temporarily set `RIMA.CameraSystem.CameraFollow.target` to the orb, then restore it to the player after the new-room spawn.
Graph binding is the missing hook: `RuntimeRoomManager.OnPlayerEnteredDoor` already does `DungeonGraph.Navigate(direction)` then transition, but the live `Systems/Map/RoomLoader` only has `LoadNext/JumpToRoom` over a linear `_sequence`. Add a graph-aware loader entry point or a node-id to sequence-data mapping before using portals as real branch choices.
Key risk: without graph-aware `RoomLoader`, portal choice is cosmetic and still advances linearly.
Effort: M for fade-backed orb travel; L for fully visible camera-follow-through into a diegetic neighbor island.

Q3. Map-fragment mechanic
VERDICT: keep and repurpose.
Recommendation: Do not remove it for the demo, because current `Systems/Map/RoomLoader.SpawnFragmentThenDraftUnlock` uses `RIMA.Environment.MapFragment.OnAnyFragmentPickedUp` to start draft and unlock the gate. Instead, repurpose the pickup as "reveal charge": after pickup, call `DungeonGraph.RevealAhead(revealSteps)` from the current environment fragment flow or a bridge listener, then refresh whichever preview UI/world preview replaces the node map.
If real previews replace the abstract map, fragments should reveal more next-room previews, reveal one extra depth beyond current exits, or upgrade preview detail (room type first, then contents like elite/treasure/focal hazard). This maps cleanly onto existing `RoomNode.revealed`, `RevealAhead(steps)`, and `GetRevealedStepsAhead()`.
Key risk: fragment currently carries two meanings in different systems: draft/unlock in `Environment.MapFragment`, graph reveal in `Core.MapFragment`. Merge the behavior or rename the reward pickup concept before polish.
Effort: S for "pickup reveals more previews"; M if it also reveals hidden room contents with new per-node metadata.

Q4. Real-room-preview instead of overhead map
VERDICT: feasible for demo as thumbnails/faked diegetic previews; risky as live RenderTexture rooms.
Option A, live RenderTexture mini-cameras: technically feasible but not demo-realistic first. It needs pre-instantiated next-room instances, preview-only layers, lighting/camera isolation, and RenderTexture UI/world quads. This is M/L and will expose lifecycle issues in room spawning.
Option B, pre-rendered thumbnails: best demo path. Add a preview sprite/thumbnail field or lookup per `RoomSequenceData`/room type, then for each `DungeonGraph.CurrentNode.exits` create one preview panel/quad. Portal i stores the same `DoorDirection`/target node id as preview i, so selection and visual preview cannot drift.
Option C, diegetic previews floating in the void: best final direction, but demo should fake it first with world-space thumbnail quads or low-detail preview prefabs positioned beside the island. Full real adjacent islands need camera framing, collision isolation, active-room boundaries, and loading policy. `DungeonWorldBuilder` already proves a graph-to-world-offset idea exists, but it paints all nodes into one tilemap world and is not yet the floating-island branch-preview system.
Key risk: generating "genuine next rooms" before the choice means the loader must deterministically create/bind rooms by node id; current live loader is sequence-indexed.
Effort: B = S/M; A = L; C fake = M, full = L.

RECOMMENDED OVERALL DIRECTION
For the next demo, replace directional doors with graph-bound rift portals, keep `RoomTransitionFX` as a brief safety mask, and implement orb travel as a layered cinematic on top of the existing room swap. Use pre-rendered or faked diegetic next-room preview quads bound to `DungeonGraph.CurrentNode.exits`; do not build live RenderTexture preview rooms first. Repurpose map fragments into reveal-depth/preview-detail pickups through `DungeonGraph.RevealAhead`. The single biggest user decision: should the demo show previews as clean side-by-side UI panels first, or spend extra effort making them diegetic floating islands in the void immediately?
# RIMA Portal Map Production Recipe - Round 2

Read: `CURRENT_STATUS.md`, `.claude/PROJECT_RULES.md`, `CODEX_TASK_yasinderyabilgin.md`, live code under `Assets/Scripts/**`, room-painter authoring/runtime data files, `Packages/manifest.json`. `ANTIGRAVITY.md` was requested by routing rules but is not present at repo root. Gemini CLI consult was also run.

## Q1. ORB-TRAVEL build recipe

Recipe:
- Add one minimal runtime script surface: `PortalTravelDirector` on the Systems/Map object. It owns travel state, not `Portal`, `Gate`, or `RoomLoader`.
- Use a separate orb GameObject, not only particles: `PortalTravelOrb` root with `SpriteRenderer` glowing disk/rune, `Light2D` point light, `TrailRenderer` for the motion streak, optional `ParticleSystem` sparks, and a child `CrashFlash` particle/ring prefab. Particle-only is too hard to camera-target and too weak as the player stand-in.
- Add one small helper on player, `PlayerTravelVisualState`, with serialized `visualRoot` plus optional explicit `behavioursToDisable`. It caches and restores enabled states for `SpriteRenderer`, `TrailRenderer`, `ParticleSystem`, `Light2D`, and selected input/combat scripts.
- On portal select: disable `PlayerController` and `PlayerAttack`; also disable class skill/input components if present. Current `RoomLoader` only disables `PlayerController` in `LoadNextInstance`, but orb travel needs attack/class input frozen too because `PlayerAttack` owns its own `InputAction`s.
- Freeze physics: cache `Rigidbody2D.simulated`, velocity, and collider enabled states. Set velocity to zero, disable player `Collider2D`s, and either set `rb.simulated=false` during the visual flight or move an orb-only transform while the real player waits hidden. Restore at crash.
- Hide player visuals by disabling renderers under the explicit visual root, not by disabling the whole player GameObject. Do not disable `Health`, inventory, event buses, or persistent player state.
- Camera: `CameraFollow` already has public `target`; cache `target`, `smoothTime`, and camera `orthographicSize`. Set `target` to the orb or to a tiny `CameraTravelTarget` object that leads the orb by 0.5-1.0 units. Restore target to player after crash/spawn.
- Tween: use coroutine + `AnimationCurve`. No DOTween, LeanTween, Cinemachine, or iTween were found in `Packages/manifest.json` or scripts. Keep the package surface unchanged for demo.
- Path: `start = selectedPortal.transform.position`; `end = resolved target island landing / nextData.playerStartPos`. Use a quadratic Bezier: start, mid point above/forward in void, end. Duration 0.55-0.70s. Use `Time.unscaledDeltaTime` only if transition UI pauses time; otherwise normal `deltaTime`.
- Beats:
  1. 0.00-0.08s: player squash/glow, renderers fade out, orb appears at player chest.
  2. 0.08-0.58s: orb zips on Bezier; TrailRenderer and Light2D do most of the read. Camera targets orb/lead target.
  3. 0.58-0.72s: crash ring, target island Light2D turns on, preview island scales from 0.9 to 1.0.
  4. 0.72-0.80s: room swap/teleport completes, player renderers restore, controller/attack restore, camera target returns to player.
- Keep `RoomTransitionFX` as a fallback/cover, but do not use the current full black fade as the main signature. If content swap is visible, use a 0.08-0.12s white/cyan impact flash at crash, not a 0.25s blackout.

Effort: M.

Key risk: state restoration. Player visuals/weapons/VFX are spread across children, and `PlayerAttack` has independent input actions. A broad `gameObject.SetActive(false)` will break persistent state; a too-narrow disable will allow inputs or weapon/VFX to leak during the orb.

## Q2. DIEGETIC PREVIEW render recipe

Option A - low-detail static preview prefab / runtime visual island:
- Setup: create `RoomPreviewIsland` prefab root per room type or node. Children only: floor/cliff/wall/prop SpriteRenderers or TilemapRenderers, optional black silhouette overlay, no enemies, no colliders, no AI, no spawners, no Gate. Add `PreviewIslandView` to cache renderers and expose `SetDark(float)`, `FadeIn`, `Illuminate`, `Bind(DoorDirection dir, int targetNodeId, RoomType type)`.
- Data source for demo: prefab variants are fastest. For final: generate from `RoomData`/room layout using a runtime counterpart to `RoomDataComposer`, backed by `RuntimeAssetRegistry`, because existing `RoomDataComposer` is Editor-only and uses `AssetDatabase`.
- Placement: `PortalPreviewController` creates 1 preview per `DungeonGraph.CurrentNode.exits`. Place them beyond the active room edge in fan positions matching portal order. Keep scale 0.45-0.65 for "seen from current island".
- Lighting: use unlit/dark tinted material or renderer color alpha/value. Keep preview Light2D disabled until selected. Do not rely on scene darkness alone, because URP 2D Light target layers are easy to misconfigure.
- Binding: sort exits by direction/fan slot, then assign the same `ExitChoice { index, DoorDirection direction, int targetNodeId, RoomType roomType }` to both `Portal` and `PreviewIslandView`. Portal i and preview i must share this object, not recompute independently.
- Perf budget: 2-3 islands with low-detail sprite groups are acceptable if each stays around 30-80 renderers and no active scripts/lights. Avoid full high-detail rooms duplicated three times.
- Recommended for demo and final visual direction. Demo uses handmade preview prefabs; final can replace internals with data-driven RoomData composition.

Effort: M for demo prefab path, L for final runtime RoomData preview generator.

Key risk: accidentally instantiating gameplay prefabs and running combat scripts. Preview roots must be visual-only.

Option B - live RenderTexture mini-camera:
- Setup: instantiate/build the next room in a hidden staging area or layer, point an orthographic camera at it, render to a 256 or 512 RenderTexture, display it on a world-space quad/sprite near the portal.
- Use one camera reused sequentially if thumbnails update rarely; avoid one always-on camera per preview.
- Perf budget: 2-3 active preview cameras plus RTs is wasteful for the main gameplay scene. It also produces flat TV screens unless heavily dressed.
- Good for debug/editor inspection, weak for the locked "real islands in the void" fantasy.

Effort: M.

Key risk: extra cameras/RTs cost GPU and still fail the "real island" requirement unless the quad is disguised.

Option C - pre-rendered thumbnail quad:
- Setup: existing `RoomThumbnailBaker` already bakes `RoomData` previews to 256 PNG using a temporary orthographic camera. Use `RoomData.thumbnailPath`, load as Sprite/texture, render on a dark quad above/beside portal.
- Perf budget: cheapest by far, one SpriteRenderer per preview.
- Best use: minimap, placeholder, or "contents revealed" icon card. It should not be the final diegetic preview because it reads as UI/painting, not a room island.

Effort: S.

Key risk: static thumbnails cannot show live procedural variation and violate the "real iso room-islands" lock if used as the primary preview.

Recommendation: build Option A first. Use Option C only as a fallback/placeholder or for far-depth previews. Do not choose Option B for the production signature.

## Q3. MOB-LESS preview recipe

Current code reality:
- `RoomSequenceData` has `mobSpawns`, `focalElementPrefab`, `decorProps`, gate fields, boss/reward flags, and fragment override, but no preview flag and no preview prefab/RoomData field.
- Live `RoomLoader.BuildRoomContent(RoomSequenceData data)` directly instantiates mobs, focal, decor, Gate, fragment anchor, and room-clear handlers in one method.
- `RoomConfig` has a `RoomData roomData` field, and `RoomData` stores visual-only `floorCells`, `cliffCells`, `wallCells`, and `propPlacements`.
- Existing `RoomDataComposer.ComposeInto` can build visual floor/cliff/walls/props without mobs, but it is Editor-only due `UnityEditor.AssetDatabase`.

Cleanest hook:
- Do not add a bool preview flag to the existing live `BuildRoomContent` and thread conditionals through combat/gate logic. That method is already the live room spine.
- Add a sibling service: `RoomPreviewBuilder.BuildPreview(ExitChoice choice, Transform parent)`.
- For demo, `RoomPreviewBuilder` loads a lightweight `GameObject previewPrefab` from a new `RoomPreviewCatalog` keyed by `RoomType` or target node id.
- For production, add `RoomData previewRoomData` or `RoomData roomData` mapping in the graph/room catalog and implement a runtime composer using `RuntimeAssetRegistry`, mirroring the editor `RoomDataComposer` but without `AssetDatabase`.

Mob-less rules:
- Never instantiate `RoomSequenceData.mobSpawns` for preview.
- Never instantiate `Gate`, `MapFragment`, `EncounterController`, enemy prefabs, reward interactables, or boss listeners.
- If using visual prefabs, run a defensive strip/pass on the instantiated preview root: disable/destroy `Collider2D`, `Rigidbody2D`, `Health`, `EnemyAI`, `BaseMobBehavior`, `EncounterController`, `Gate`, `Portal`, `MapFragment`, and all non-whitelisted behaviour scripts. Keep `SpriteRenderer`, `TilemapRenderer`, `Grid`, `Transform`, and optional simple preview-only animation.
- Decor is allowed only if visual-only. If a decor prefab has logic, provide a preview variant.

Effort: S for preview prefab variants; M/L for data-driven runtime RoomData composition.

Key risk: using actual room prefabs is seductive but unsafe. Some Start/Awake logic will run before stripping unless prefab variants are clean or the builder instantiates only visual data.

## Q4. REVEAL-MOMENT sequencing

Slot in existing flow:
- Current live slot is `RoomLoader.BuildRoomContent` combat branch: mob death raises `OnRoomCleared`; the `_clearToUnlockHandler` logs clear and calls `SpawnFragmentThenDraftUnlock(gate)`.
- Insert the reveal sequence inside that handler before `SpawnFragmentThenDraftUnlock(gate)`, or route the handler into a new `RoomClearRevealDirector` coroutine that calls fragment/draft/unlock after previews appear.

Concrete coroutine:
1. `OnRoomCleared` fires after remaining mobs reaches 0.
2. Clear handler unsubscribes itself as it does now.
3. `RoomClearRevealDirector.Play(RoomSequenceData data, Gate oldGateOrNull)` starts.
4. Disable player attack/input for the short reveal beat, but keep movement optional after the camera settles.
5. Camera pullback: cache `CameraFollow.target` and `smoothTime`; create `CameraRevealTarget` at midpoint between current room center and preview fan center; set `CameraFollow.target = CameraRevealTarget`; optionally tween `Camera.orthographicSize` +10-20% for 0.25s. Existing `CameraFollow` has no zoom API, so zoom is direct camera coroutine.
6. Spawn portals from graph, not random room type count. Add `PortalSpawnController.SpawnPortals(FragmentDropAnchor anchor, IReadOnlyList<ExitChoice> exits)` or a new `PortalChoiceController` wrapper. Count = `DungeonGraph.CurrentNode.exits.Count`.
7. For each exit, create portal visual: color by target room type, child floating rune icon, disabled trigger until the post-draft unlock/arm moment. Store `DoorDirection` and `targetNodeId` on a new `PortalExitBinding`.
8. Spawn preview islands at matching fan slots with the same `ExitChoice`. Start dark/static/unlit/mob-less, alpha 0, scale 0.85.
9. Fade previews in over 0.25-0.35s. Do not illuminate yet; only silhouette and faint cyan rim. This is the always-free next-step preview.
10. Drop map fragment using existing `SpawnFragmentThenDraftUnlock(gate)` logic, but change the pickup callback to call `DungeonGraph.RevealAhead(steps)` before draft. Basic next preview remains free; fragment reveals deeper path/contents.
11. On fragment pickup: update existing preview islands with contents reveal (room-type rune, elite/rest/reward marker, maybe far-depth ghost islands if steps > 1), then show `DraftManager`, then arm/unlock portals.
12. Restore camera target to player unless the player is selecting a portal.
13. On portal enter: call `DungeonGraph.Navigate(binding.direction, out nextNode)`, then `PortalTravelDirector.Travel(binding)`; travel completes by calling graph-addressed room load or current `RoomLoader.LoadNext` fallback for demo.

Gate reuse:
- Short term: keep `Gate` hidden/disabled or use it only as a compatibility unlock gate while portals are introduced. Current `Gate.Unlock` gives open SFX/state, but portals are the new interaction surface.
- Final: replace gate-entered transition with portal-entered transition. `Gate` can still be an art asset if a portal needs a stone arch, but should not own branch logic.

Effort: M for reveal sequence using demo preview prefabs and linear `LoadNext`; L when RoomLoader becomes fully graph-addressed by `targetNodeId`.

Key risk: graph/room desync. `DungeonGraph` is node-based, but live `RoomLoader.LoadNextInstance` is still linear sequence index based. The portal binding must become the single source of truth for `DoorDirection -> targetNodeId -> room content`; otherwise previews can show one target while `LoadNext` loads another.

## Minimal production recipe for demo

Build in this order: first add `ExitChoice` binding and spawn portal count from `DungeonGraph.CurrentNode.exits`; second create 3-5 handmade low-detail `RoomPreviewIsland` prefabs keyed by room type and spawn them dark beside the active island; third add `RoomClearRevealDirector` in the current `OnRoomCleared -> SpawnFragmentThenDraftUnlock` slot so previews fade in before fragment/draft/unlock; fourth add `PortalTravelDirector` with coroutine orb travel, camera retarget, and crash flash; fifth wire portal enter to graph navigation, using existing `RoomLoader.LoadNext` only as a temporary demo fallback.

Single biggest technical decision: commit to real static preview islands (prefab now, runtime RoomData composer later) as the canonical path. RenderTexture and thumbnail quads are useful support tools, but they should not become the production answer for the locked diegetic floating-room preview.
# Yekta Detection Report - Unified Designer Inventory and Cliff Root Cause - 2026-05-31

## A. DESIGNER INVENTORY

### A1. RIMA/Map Designer
- Window/class: `UnifiedMapDesigner` in `Assets/Scripts/Editor/MapDesigner/UnifiedMapDesigner.cs`.
- Menu: `RIMA/Map Designer` at line 63.
- Does: top-level editor shell with title and the shared cliff action button.
- Data path: no independent map document found in this shell; it delegates to editor actions.
- Overlap: duplicates entry-point territory with multiple older map-designer windows.
- Status: partly current, because it is the simplest unified entry point and already hosts `CliffGenerateAction.DrawButton`.
- Verdict: keep as the canonical editor entry and route other actions into it.

### A2. RIMA/Map Designer Brush Tool
- Window/class: `MapDesignerBrushWindow` in `Assets/Editor/MapDesigner/Brush/MapDesignerBrushWindow.cs`.
- Menu: `RIMA/Map Designer Brush Tool` at line 43.
- Does: SceneView brush workflow through `BrushSceneTooling` and `GridTileExecutor`.
- Data path: direct scene/tilemap writes, not `RoomData`.
- Overlap: overlaps floor/cliff/prop painting with Room Painter and Visual Map Designer.
- Status: legacy-but-functional mechanical painter.
- Verdict: consolidate its brush execution behind the shared RoomData pipeline or hide behind advanced/debug workflow.

### A3. Minimal Tile Painter
- Window/class: `MinimalTilePainter` in `Assets/Scripts/Editor/MapDesigner/MinimalTilePainter.cs`.
- Menu: removed/disabled at line 106.
- Does: manual tile painter plus `CliffGenerateAction.DrawButton` at lines 639-644.
- Data path: direct scene/tilemap writes.
- Overlap: overlaps Room Painter, Visual Map Designer, and Brush Tool.
- Status: deprecated UI still present in code.
- Verdict: do not promote; either delete later or keep only as a migration/debug fallback.

### A4. RIMA/Visual Map Designer (New)
- Window/class: `RimaVisualMapEditorWindow` in `Assets/Editor/MapDesigner/VisualEditor/RimaVisualMapEditorWindow.cs`.
- Menu: `RIMA/Visual Map Designer (New)` at line 42.
- Does: visual palette/editor shell with BrushPack/BiomeSkin loading and a SceneView painter.
- Data path: not RoomData-backed; `VisualEditorScenePainter` creates dummy `RoomData` for rendering decisions but paints/erases tilemaps directly.
- Overlap: overlaps the canonical Map Designer and Room Painter for floor/cliff painting.
- Status: new UI experiment, not unified data authority.
- Verdict: useful UI ideas, but not a canonical authoring path until backed by RoomData.

### A5. Tools/RIMA/Map Designer/Open Tile Palette and Rebuild v15g
- Window/class: `OpenTilePaletteMenu` in `Assets/Editor/MapDesigner/Workflow/OpenTilePaletteMenu.cs`.
- Menus: `Tools/RIMA/Map Designer/Open Tile Palette` at line 23; `Rebuild Tile Palette v15g` at line 37.
- Does: opens Unity Tile Palette; rebuilds a generated palette asset from project tile assets.
- Data path: tile palette asset generation, not room/map data.
- Overlap: asset utility only; no room authoring overlap except palette support.
- Status: utility.
- Verdict: keep as an asset maintenance command, but expose from a single tool hub.

### A6. Tools/RIMA/Map Designer/Asset Pack Browser
- Window/class: `AssetPackBrowserWindow` in `Assets/Editor/MapDesigner/AssetPackBrowserWindow.cs`.
- Menu: `Tools/RIMA/Map Designer/Asset Pack Browser` at line 67.
- Does: browses `AssetPackManifestSO`, atlases, props, categories, and places entries via `PropPlacementService`.
- Data path: manifest-backed asset browsing, but placement is not RoomData-first.
- Overlap: prop browsing overlaps F2 runtime overlay and Room Painter prop placement.
- Status: important asset browser, not canonical room-authoring layer.
- Verdict: keep the browser, route placement through RoomData when a room is active.

### A7. Tools/RIMA/Map Designer/Blueprint Painter
- Window/class: `BlueprintPainterWindow` in `Assets/Editor/MapDesigner/Blueprint/BlueprintPainterWindow.cs`.
- Menu: line 39.
- Does: blueprint paint workflow.
- Data path: separate blueprint-oriented editor path; no evidence it is the current room data authority.
- Overlap: conceptual overlap with room layout painting.
- Status: specialized/legacy.
- Verdict: keep isolated unless a current workflow depends on it.

### A8. RIMA/Room Painter
- Window/class: `RimaRoomPainterWindow` in `Assets/Editor/RoomPainter/RimaRoomPainterWindow.cs`.
- Menus: `RIMA/Room Painter` at line 133; explicit mode launchers for floor/cliff/wall/decor/metadata at lines 149-185.
- Does: room authoring UI, active room library, palette, scene actions, RoomData-backed paint sink.
- Data path: current shared-data path through `RoomDataPlacementSink` and `RoomDataAuthoringController`.
- Overlap: overlaps Map Designer and Visual Map Designer for painting, but is the strongest canonical authoring implementation.
- Status: current.
- Verdict: make this the primary implementation behind the unified Map Designer entry.

### A9. Room Painter Live Tool
- Classes: `LiveToolLauncher`, `LiveToolPaletteWindow`, `RuntimeAssetRegistryBaker`.
- Menus: launch/stop/build in `Assets/Editor/RoomPainter/LiveTool/LiveToolLauncher.cs` lines 155-161; palette menu in `LiveToolPaletteWindow.cs` line 69; registry bake menu in `RuntimeAssetRegistryBaker.cs` line 44.
- Does: runtime/live painting support.
- Data path: runtime registry and direct runtime brush execution; not consistently RoomData-first.
- Overlap: overlaps F2 overlay and editor Room Painter.
- Status: runtime support/experimental.
- Verdict: keep as a runtime bridge, but align save semantics with RoomData JSON/assets.

### A10. RIMA/Room Painter F2 In-Play Overlay
- Class: `InPlayMapPaintOverlay` in `Assets/Scripts/RoomPainter/InPlayMapPaintOverlay.cs`.
- Menu: no editor window menu; runtime F2 overlay.
- Does: in-play floor, wall, decor, and prop painting.
- Data path: RoomData-backed; loads active RoomConfig data, saves JSON and updates/creates `.asset` in Editor.
- Overlap: overlaps editor Room Painter but shares the right data model.
- Status: current runtime authoring path.
- Verdict: keep; it should share asset discovery and palette rules with editor Room Painter.

### A11. Tile Import Wizards and PixelLab Importers
- Classes: `TileImportWizard` in `Assets/Editor/TileImport/TileImportWizard.cs` and duplicate in `Assets/Editor/RoomDesigner/Tools/TileImportWizard.cs`; `PixelLabWangImporter`; `PixelLabPngSheetImporter`.
- Menus: old TileImportWizard menus are removed; PixelLab menus remain at `Assets/Editor/TileImport/PixelLabWangImporter.cs:33` and `Assets/Editor/TileImport/PixelLabPngSheetImporter.cs:19`.
- Does: asset import/build utilities.
- Data path: asset generation, not room/map state.
- Overlap: asset ingestion only.
- Status: utility with duplicate/deprecated code present.
- Verdict: keep PixelLab importers; remove or archive duplicate disabled wizard later.

### A12. Encounter Menu
- Class: `EncounterMenu` in `Assets/Editor/MapDesigner/EncounterMenu.cs`.
- Menus: lines 13, 33, and 56.
- Does: encounter-related editor commands.
- Data path: separate encounter tooling.
- Overlap: low for map painting.
- Status: utility.
- Verdict: keep separate unless unified room metadata grows encounter assignment.

## B. SHARED-DATA REALITY CHECK

### Shared room data that actually exists
- `RoomData` exists in `Assets/Scripts/RoomPainter/RoomData.cs` with `roomId`, `displayName`, `thumbnailPath`, `floorCells`, `cliffCells`, `wallSegments`, `wallCells`, and `propPlacements`.
- `RoomDataMutator` is the central mutation helper for floor, cliff, prop, wall-run, wall-cell removal, and legacy wall-segment migration.
- `RoomDataJson` serializes/deserializes room data and migrates legacy wall segments during DTO conversion.
- `RoomDataPaths` defines canonical room JSON under `Assets/Data/Rooms/<id>.room.json` in Editor and StreamingAssets at runtime.
- `RoomDataAuthoringController` writes both `.asset` and `.room.json` data and manages library create/duplicate/delete/update operations.
- `RoomDataComposer` composes RoomData back into scene tilemaps/groups, but it is Editor-only because it uses `UnityEditor`, `AssetDatabase`, `PrefabUtility`, and `EditorSceneManager`.
- `RoomDataPlacementSink` is the current editor paint sink. It records undo, mutates RoomData, marks dirty, and recomposes.
- `InPlayMapPaintOverlay` is also RoomData-backed. It loads the active `RoomConfig.roomData`, reads/writes JSON, updates/creates `.asset` files in Editor, and mutates floor/wall/prop/decor data through RoomData helpers.

### Paths not using the shared data model
- `MapDesignerBrushWindow` / `BrushSceneTooling` / `GridTileExecutor` write directly to scene tilemaps.
- `RimaVisualMapEditorWindow` / `VisualEditorScenePainter` use a dummy `RoomData` only for neighbor/rule context and otherwise paint/erase tilemaps directly.
- `MinimalTilePainter` is direct tilemap painting.
- `AssetPackBrowserWindow` browses useful manifests but places through `PropPlacementService`, not RoomData-first.
- `DecorCliffPainter` writes directly to a decor/cliff tilemap.
- `RoomPainterScenePlacer` directly creates GameObjects/SpriteRenderers and bindings; current Room Painter uses `RoomDataPlacementSink` when an active room is present, so this looks like older placement infrastructure.
- `LiveTool` runtime brush execution writes to runtime tilemap targets directly.

### Verdict
The shared-data architecture exists and is viable, but it is not universal. Room Painter and the F2 overlay are closest to the intended source of truth. Several designer windows are still scene/tilemap-first, which explains duplicated behavior and inconsistent save/rebuild semantics.

## C. CLIFF-GENERATE ROOT CAUSE

### The button is real
- `UnifiedMapDesigner` draws `CliffGenerateAction.DrawButton(28f)`.
- `MinimalTilePainter` also draws the same shared action.
- `CliffGenerateAction.DrawButton` either creates/finds a `CliffAutoPlacer`, wires missing references, or calls `placer.Regenerate()` when ready.

### The generation code is real
- `CliffAutoPlacer.IsReady` requires `floorTilemap != null && cliffTilemap != null && cliffTile != null`.
- `Regenerate()` clears the cliff tilemap when configured, validates manual cells, collects floor cells, computes cliff target cells, and calls `cliffTilemap.SetTile(cell, cliffTile)` for each target.
- `CollectFloorCells()` only detects occupied cells from `floorTilemap.cellBounds` plus `floorTilemap.HasTile(cell)`.
- Therefore the generator cannot produce anything if the selected floor tilemap has no floor tiles, if `cliffTilemap` is missing, or if `cliffTile` is missing.

### Scene/reference reality
- `_IsoGame.unity` has a `CliffRing` with floor tilemap, cliff tilemap, cliff tile, and rules references wired.
- `PlayableArena_Test01.unity` also has a wired `CliffRing`, `CliffTilemap_Auto`, `Floor`, cliff tile, and rules.
- `PlayableArena.unity` has a `CliffRing` and floor/rules references, but search evidence did not show the cliff tilemap or cliff tile references wired. That scene likely produces a disabled button or a `not ready` warning.
- `Act1_ShatteredKeep.unity` has floor/cliff tilemap names and refs, but search did not show a ready `CliffAutoPlacer` setup.

### Why it can appear to do nothing
1. Existing `CliffRing` can be present but incomplete. `CliffGenerateAction` only auto-creates when no placer exists. If a placer exists with missing fields, it does not repair that existing placer before enabling generation.
2. Auto-created placers pick the first tilemap whose name does not contain `void` or `cliff`. In scenes with multiple tilemaps, this can choose the wrong or empty floor tilemap.
3. `Regenerate()` has no selection/visibility feedback besides logging and tile writes. If target cells are zero or the target cliff tilemap is hidden/under-sorted, it looks like a no-op.
4. Room Painter's C shortcut calls `placer.Regenerate()` on the first found placer and only checks for null, not `IsReady`, so it can silently hit the same readiness guard.

### Root cause conclusion
The core algorithm is not a no-op. The likely root cause is integration and target resolution: incomplete existing `CliffAutoPlacer` references, wrong floor tilemap selection, missing cliff tilemap/tile assignment, or zero floor cells in the chosen tilemap. The highest-risk code path is an existing-but-unready placer because auto-repair is skipped.

## D. CONSOLIDATION ARCHITECTURE RECOMMENDATION

### Recommended target shape
- One visible top-level entry: `RIMA/Map Designer`.
- One source of truth for authored rooms: `RoomData` plus JSON/asset persistence.
- One paint mutation layer: `RoomDataMutator` and a shared placement sink/service.
- One composition layer for editor scenes and one runtime-compatible equivalent for play mode.
- Asset browsers/importers stay as supporting utilities, not separate authoring authorities.

### Keep as primary
- `UnifiedMapDesigner`: keep as the single front door.
- `RimaRoomPainterWindow`: keep as the primary editor authoring implementation because it is already RoomData-backed.
- `InPlayMapPaintOverlay`: keep as the runtime authoring/debug path because it uses RoomData and JSON/asset persistence.
- `AssetPackBrowserWindow`: keep for discovery, but redirect placement into RoomData when an active room exists.
- PixelLab importers and palette rebuild tools: keep as asset utilities.

### De-emphasize or migrate
- `MinimalTilePainter`: deprecated; do not make it visible again.
- `MapDesignerBrushWindow`: migrate to shared RoomData mutation or classify as legacy/debug.
- `RimaVisualMapEditorWindow`: keep only if its UI is moved onto RoomData; otherwise it remains a parallel editor.
- `RoomPainterScenePlacer`: older direct scene placement path; avoid using it as canonical persistence.
- `DecorCliffPainter`: fold into the same cliff/decor RoomData path or keep as a narrow debug tool.

### Cliff generator fix direction
- Make `CliffGenerateAction` repair existing unready placers the same way it wires newly created placers.
- Resolve floor/cliff tilemaps by explicit names and active room context before falling back to first tilemap.
- Show generated count, selected floor tilemap, selected cliff tilemap, and readiness reason in the UI.
- Prefer generating from active `RoomData.floorCells` when an active room exists; use scene floor tilemap only as fallback.
- Keep the current `CliffAutoPlacer` algorithm as the mechanical scene writer until RoomData-first generation is added.

## E. ASSET-PACK / ROOM ORGANIZATION

### Current organization found
- Canonical room data path exists under `Assets/Data/Rooms` with a `Library` subfolder and thumbnail support.
- Room JSON path is defined as `Assets/Data/Rooms/<roomId>.room.json` in Editor.
- Runtime loading expects corresponding StreamingAssets room JSON when outside Editor.
- Asset packs exist as `AssetPackManifestSO`, with current pack assets such as `Assets/Data/Brush/AssetPacks/RIMA_v2_Pack.asset` and `RIMA_v3_Pack.asset`.
- Art is spread across `Assets/Art/AssetPacks/Act1_ShatteredKeep`, `Assets/Art/Rooms/AssetPack`, `Assets/Sprites/Environment/PixelLabFloor`, `Assets/Sprites/Environment/KitB_Cliff`, `Assets/Sprites/Environment/IsoKit`, `Assets/Sprites/AssetPackV3`, and related prefab folders.
- Runtime registry bake scans fixed roots including brush/asset-pack and environment folders and writes `Assets/Resources/Live/RuntimeAssetRegistry.asset`.
- F2 overlay still has hardcoded/fallback resource paths such as `Live/WallKit`, `IsoKit`, and `RuinedKeepKit`.

### Friction points
- Asset discovery is split across manifest search, runtime registry bake roots, Resources fallback paths, and hardcoded editor asset paths.
- Room data and room art are not clearly packaged together as a single room bundle.
- Editor Room Painter and runtime F2 overlay can discover assets through different mechanisms.
- Multiple designer windows can place assets without going through the same room persistence path.

### Recommended organization
- Treat `Assets/Data/Rooms` as the canonical authored room data library.
- Keep imported art/assets under versioned asset-pack folders and expose them through `AssetPackManifestSO`.
- Make both editor and runtime palettes consume the same manifest/registry output.
- Keep generated thumbnails under `Assets/Data/Rooms/Thumbnails`.
- Reserve StreamingAssets room JSON for runtime/export copies, not as the primary editor source.
- Avoid more hardcoded art-folder fallbacks; add assets to manifests/registry instead.

## Bottom Line
RIMA already has the right shared-data spine in `RoomData`, `RoomDataMutator`, `RoomDataJson`, `RoomDataAuthoringController`, `RoomDataPlacementSink`, and the F2 overlay. The project also still contains several direct scene/tilemap authoring paths. Consolidation should make `RIMA/Map Designer` the single visible entry, make Room Painter/F2 the canonical implementation, and demote older direct painters. The cliff button is not fake; it reaches real generation code, but can look like a no-op when an existing `CliffAutoPlacer` is unready, points at the wrong/empty floor tilemap, or has missing cliff target/tile references.

Executed CODEX_TASK_yekta.md as findings-only. Appended structured A/B/C/D/E report to CODEX_DONE.md. No code or scene files edited.
## CX REVIEW - Unified Designer

- Assets/Scripts/DevTools/InPlayMapPaintOverlay.cs:47 / Assets/Scripts/DevTools/InPlayMapPaintOverlay.cs:1292 / Assets/Scripts/DevTools/InPlayMapPaintOverlay.cs:1303 - SHOULD - F2 is not actually on the unified category/core path: the overlay still exposes only Floor/Cliff/Prop, has no Portal or Light category, and never owns a UnifiedDesignerCore instance. Wire the overlay through UnifiedDesignerCore + DesignerCategoryMap, or add the missing Portal/Light routing through the same RoomDataMutator.PutCategory/RemoveCategory path used by the editor.
- Assets/Scripts/DevTools/InPlayMapPaintOverlay.cs:1613 - SHOULD - GenerateCliffsInPlay chooses cliffAssetId/cliffTile from the current unfiltered _palette/_selected entry, while RebuildPalette fills _palette with every registry tile at Assets/Scripts/DevTools/InPlayMapPaintOverlay.cs:1017. If the selected tile is floor/decor, F2 generates cliffCells with the wrong asset id and paints the wrong tile onto _cliffTilemap. Resolve the generated cliff asset from registry.GetByTag("cliff"), an existing cliffCells asset id, or a dedicated cliff selection instead of the global selected palette entry.
- Assets/Editor/RoomPainter/Authoring/RoomDataComposer.cs:60 / Assets/Editor/RoomPainter/Authoring/RoomDataComposer.cs:71 - SHOULD - RoomDataComposer composes floor, cliff, wall, and prop placements, but never composes room.portalPlacements. Portal placement data now survives JSON, but editor preview/playtest composition will drop the portal visuals. Add a ComposePortals path that instantiates portalPlacements with portal-specific metadata while preserving their graph fields in RoomData.
- Assets/Editor/RoomPainter/Authoring/RoomDataComposer.cs:280 / Assets/Editor/RoomPainter/Authoring/RoomDataComposer.cs:436 - SHOULD - RoomDataComposer still has its own depth fallback table instead of using RoomDepthStack. With no metadata, cliffs fall through to sorting layer "Default" and order 5 at Assets/Editor/RoomPainter/Authoring/RoomDataComposer.cs:448 and Assets/Editor/RoomPainter/Authoring/RoomDataComposer.cs:457, which contradicts RoomDepthStack's Ground/-10 cliff slot. Replace DefaultSortingLayer/DefaultSortingOrder with RoomDepthStack.SlotFor(layer), keeping metadata override behavior if intended.
- Assets/Scripts/RoomPainter/RoomCliffSolver.cs:31 / Assets/Scripts/Environment/CliffAutoPlacer.cs:249 / Assets/Scripts/Environment/CliffAutoPlacer.cs:274 - SHOULD - RoomCliffSolver ports exterior flood, monotonic south, back-side cut, and protrusion cut, but omits the original CollectCliffCells orphan-cluster filter. If CliffClusterRules is active on the scene placer, editor/tilemap generation and RoomData/core generation can disagree on isolated floor clusters. Either pass equivalent cluster settings into RoomCliffSolver or explicitly lock the unified solver as the new source of truth and remove the old cluster-dependent parity claim.

BOTTOM LINE: fix-first - must-fixes: (1) route F2 through the unified category/core path or add missing Portal/Light parity, (2) make GenerateCliffsInPlay select a real cliff asset/tile instead of the current global palette item, (3) compose portalPlacements and RoomDepthStack-backed sorting in RoomDataComposer.

Executed CODEX_TASK_yasinderyabilgin.md as findings-only review.

Actions run:
- Read CODEX_TASK_yasinderyabilgin.md.
- Looked for ANTIGRAVITY.md; it was not present at repo root.
- Inspected the requested unified designer files and original CliffAutoPlacer with shell commands.
- Checked runtime-safety references, TagManager sorting layers, F2 overlay routing, JSON portal persistence, RoomData composition, and cliff solver parity.
- Tried Unity EditMode batch run with Unity 6000.3.6f1; Unity refused because another Unity instance has this project open.
- Ran dotnet build Assembly-CSharp.csproj; it failed on a pre-existing generated-project missing source path: Assets/ScriptableObjects/Environment/CliffPlacementRules.cs.
- Appended findings under "## CX REVIEW - Unified Designer" to CODEX_DONE.md.

Result:
- Findings-only report written.
- No code or scene files edited.
- Bottom line in CODEX_DONE.md: fix-first, with three must-fixes around F2 parity, cliff asset selection, and portal/depth-stack composition.
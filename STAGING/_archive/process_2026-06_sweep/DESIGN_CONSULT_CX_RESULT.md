# RIMA Code Lens - Build Architecture Feasibility Review

## 1. Story-driven / room-driven lighting

Recommendation: add a data layer, not hardcoded room switches.
- Create `RoomLightingProfile : ScriptableObject` with ambient/global color+intensity, rim color/intensity, warm focal light intensity, optional room-local light prefab overrides.
- Add optional `RoomLightingProfile lightingProfile` to `RoomSequenceData`, next to existing room identity/player/gate/focal fields (`Assets/Scripts/Systems/Map/RoomSequenceData.cs:9-36`).
- Add `RoomLightingController` in scene. It subscribes to `RoomLoader.OnRoomChanged` (`Assets/Scripts/Systems/Map/RoomLoader.cs:16-19`, `:178-180`) and reads `RoomLoader.CurrentRoomData` (`Assets/Scripts/Systems/Map/RoomLoader.cs:22-25`) to apply the profile.
- Keep `RoomLoader` only as the transition hook. It already owns the room swap point (`SwapRoomWhileBlack`) and should call no lighting constants directly (`Assets/Scripts/Systems/Map/RoomLoader.cs:172-181`).

Existing Light2D wiring:
- `Global Light 2D` is a root scene object, not under a lighting controller (`Assets/Scenes/Test/PlayableArena_Test01.unity:17719-17745`, `:17777-17790`).
- Warm brazier lights are children of brazier prop objects under `RIMA_Cycle2_Dressing/CornerBraziers` (`Assets/Scenes/Test/PlayableArena_Test01.unity:1649-1672`, `:26675-26699`, `:2945-2986`, `:12158-12199`, `:17335-17376`, `:21122-21163`).
- Cyan rim lights live under `RIMA_Cycle2_Dressing/RimLights`, then per-rim parent objects (`Assets/Scenes/Test/PlayableArena_Test01.unity:544-566`, `:25670-25690`, `:17419-17460`).
- There is also an inactive root `Lighting` object and inactive `ArenaAreaLight` (`Assets/Scenes/Test/PlayableArena_Test01.unity:12380-12407`, `:12467-12487`).

Autonomous-safe now: Y. Build profile class + controller + optional `RoomSequenceData` field with placeholder profiles.

Needs design lock first: exact mood table per room, which lights are canonical roles, and whether prop-owned brazier lights should remain prop children or move under a single `Scene_Lighting` root.

Risks / contradictions:
- Current hierarchy is mixed: global light is root, brazier lights are prop children, rim lights are grouped, and inactive lighting objects exist. A controller must use explicit role refs or tagged/named refs, not blind `FindObjectsOfType<Light2D>()`.
- No lighting field exists in `RoomSequenceData`; any per-room lighting today would be scene-hardcoded.

## 2. Connected-room map system

Recommendation: keep the 5-room demo linear, but make continuity data-driven.
- Current live demo path is `RoomLoader` + `RoomSequenceData[]`, not `DungeonGraph`. `RoomLoader` loads index 0, jumps by index, and advances linearly (`Assets/Scripts/Systems/Map/RoomLoader.cs:27-30`, `:66-80`, `:88-118`, `:121-150`).
- Room continuity should be an optional `RoomConnectionProfile` or fields on `RoomSequenceData`: entry landmark id, exit landmark id, bridge/threshold prefab, connection anchor, shared landmark state.
- Apply it in `BuildRoomContent`, where focal elements and gates are already instantiated (`Assets/Scripts/Systems/Map/RoomLoader.cs:212-266`).
- Use the existing gate event as the trigger. Gate enters currently call `LoadNext()` directly (`Assets/Scripts/Systems/Map/RoomLoader.cs:268-275`), so for the 5-room demo the clean missing piece is only visual continuity data, not graph traversal.

What is missing for real connected/branching rooms:
- `DungeonGraph` has branching exits and `Navigate(DoorDirection, out RoomNode)` (`Assets/Scripts/Core/DungeonGraph.cs:120-145`, `:191-204`), but the live gate has no direction/target-node payload (`Assets/Scripts/Environment/Gate.cs:21-37`, `:128-134`).
- `MapProgressController` ignores `DungeonGraph`; it builds its own 5-node linear graph and listens to `RoomLoader.OnRoomChanged` (`Assets/Scripts/UI/Map/MapProgressController.cs:21-25`, `:44-58`, `:61-87`).
- `RoomSequenceData` has no entry/exit direction, connection id, bridge prefab, or landmark state (`Assets/Scripts/Systems/Map/RoomSequenceData.cs:9-36`).

Autonomous-safe now: Y for linear 5-room connected feel with placeholder bridge/landmark prefabs. N for full `DungeonGraph` branching integration.

Needs design lock first: whether Phase 1 stays strict linear Y-offset or starts using graph directions, and the canonical shared landmark/bridge visual language.

Risks / contradictions:
- `DungeonGraph` is 12+ boss branching, while the demo map UI is hardcoded 5-room linear.
- Gate state says AwaitingFragment by default, but current combat room flow unlocks after draft on clear (`Assets/Scripts/Systems/Map/RoomLoader.cs:292-300`, `Assets/Scripts/Environment/Gate.cs:10-24`).

## 3. Game-screen UI framework

Recommendation: formalize existing common pieces into a small screen framework.
- `RimaUITheme` already provides canonical colors, sprite resource paths, procedural frames, node icons, and menu background loading (`Assets/Scripts/UI/RimaUITheme.cs:5-23`, `:141-189`).
- `UIManager` is already the correct single owner for overlay state and timeScale (`Assets/Scripts/UI/UIManager.cs:6-14`, `:180-233`).
- Screens are still ad-hoc: `SkillOfferUI` builds its own canvas/panel/cards (`Assets/Scripts/UI/SkillOfferUI.cs:126-188`, `:260-360`), `MapProgressController` auto-creates its own canvas (`Assets/Scripts/UI/Map/MapProgressController.cs:114-130`), and `SkillBarUI` self-builds slots (`Assets/Scripts/UI/SkillBarUI.cs:92-167`).

Clean build:
- Add `RimaUIScreen` base: `Show()`, `Hide()`, `IsBlocking`, `Layer`, `CanvasGroup`, optional `BackgroundSprite`.
- Add `RimaScreenRegistry` or extend `UIManager` to open named screens: menu, HUD, draft, death, victory.
- Keep `UIManager` as timeScale owner. New screens report blocking state to it; they do not write `Time.timeScale` directly.
- Keep `RimaUITheme` as static tokens/assets; do not make every screen invent colors.
- Add `RimaBackgroundImage` helper for Codex-generated backgrounds: load sprite from `Resources/UI/RIMA/...`, set `Image.preserveAspect = true`, anchor full screen, point-filter/no-compression import preset, PPU 64 for sprite consistency. For Screen Space Overlay, PPU only affects sprite asset scale if reused elsewhere; for world/camera UI it matters directly.

Autonomous-safe now: Y. Build base screen + registry + background helper with placeholder resources.

Needs design lock first: final menu/death/victory copy, CTA/stat layout, and exact generated image set/sizes.

Risks / contradictions:
- Multiple UI roots/canvases make ordering and input blocking fragile.
- Current theme cyan is not exact brand `#00FFCC`; `RimaUITheme.Cyan` is `{0.28, 0.88, 1}` (`Assets/Scripts/UI/RimaUITheme.cs:20`). Lock whether this is intentional or should become exact brand color.

## 4. Game-feel hooks

Recommendation: use the existing `CombatEventBus` as the single juice bus.
- `VFXRouter` already subscribes to hit/kill/dash/status/commit events and maps tags to pooled prefabs (`Assets/Scripts/Combat/VFXRouter.cs:21-25`, `:83-129`, `:139-162`).
- Scene wiring is incomplete: `CombatJuice` exists, but `VFXRouter.entries` is empty (`Assets/Scenes/Test/PlayableArena_Test01.unity:10690-10739`, `:10726`).
- `PlayerController` already publishes dash events after walkability validation (`Assets/Scripts/Player/PlayerController.cs:197-225`).
- Basic hit events already exist on the bus (`Assets/Scripts/Combat/CombatEventBus.cs:8-27`, `:57-82`).
- Dash input buffering exists only for dash-blocked-by-attack: `PlayerController.TryDash()` calls `InputBufferService.RequestDash()` on failed cancel, and the service retries within 0.18s (`Assets/Scripts/Player/PlayerController.cs:179-183`, `Assets/Scripts/Combat/InputBufferService.cs:7-40`).
- There is no general coyote service. The closest existing grace window is damage-triggered merciful dodge for 0.18s (`Assets/Scripts/Player/PlayerController.cs:102-103`, `:373-377`).
- Directional camera punch is already clean: `CameraPunchController` consumes hit/kill/dash and exposes `CurrentOffset`, while `CameraFollow` adds it on top of base follow (`Assets/Scripts/Combat/Juice/CameraPunchController.cs:33-45`, `:69-112`, `Assets/Scripts/Camera/CameraFollow.cs:27-37`).

Clean build:
- Fill `VFXRouter.entries` with placeholder `hit_default`, `kill_default`, `dash_default`, `status_default` using existing prefabs (`Assets/Prefabs/VFX/HitSpark.prefab`, `Assets/Prefabs/VFX/DeathBurst.prefab`, etc.).
- Extend `InputBufferService.Pending` beyond Dash only after a design lock on inputs. Safe placeholder: Dash + BasicAttack + Skill1 with inspector windows.
- Add explicit coyote windows as named mechanics in `InputBufferService` or a tiny `PlayerGraceWindowService`; do not hide them inside `PlayerController`.
- Keep directional impulse in `CameraPunchController.Apply(direction, magnitude)`.
- Retire or rewrite `ScreenShakeDriver` to the same offset pattern before relying on it; it currently writes camera transform localPosition directly (`Assets/Scripts/Combat/Juice/ScreenShakeDriver.cs:99-125`), which contradicts the offset-based `CameraFollow` contract (`Assets/Scripts/Camera/CameraFollow.cs:30-37`).

Autonomous-safe now: Y for placeholder VFX entries, coyote/input-buffer scaffolding, and unifying camera shake offsets. Exact feel values are tune-gated.

Needs design lock first: buffer windows per action, coyote rules for dash/attack/skill after hit/commit, and final shake/hitstop magnitudes.

Risks / contradictions:
- There are two camera shake paths: `CameraShake`/`CameraPunchController` expose offsets, but `ScreenShakeDriver` writes transform position.
- HitPause writes `Time.timeScale` independently while `UIManager` also owns timeScale (`Assets/Scripts/Combat/Juice/HitPauseDriver.cs:90-106`, `Assets/Scripts/UI/UIManager.cs:223-233`). It works for combat, but overlay interactions need guard rules.

## Ordered build sequence

Autonomous-safe code first:
1. Add `RoomLightingProfile` + `RoomLightingController` with explicit scene role refs; add optional profile field to `RoomSequenceData`; create 5 placeholder profiles.
2. Add linear `RoomConnectionProfile`/fields to `RoomSequenceData`; spawn placeholder bridge/shared-landmark/focal continuity in `BuildRoomContent`; keep `DungeonGraph` untouched.
3. Add `RimaUIScreen` + screen registry/background helper; migrate only new death/victory/menu placeholders first, leaving existing HUD/draft stable.
4. Fill `VFXRouter.entries` with existing placeholder prefabs and add missing tag constants/docs.
5. Extend `InputBufferService` minimally for named buffered actions and grace windows.
6. Convert/disable `ScreenShakeDriver` transform-writing path so all camera juice is additive offset through `CameraFollow`.

Design-locked work after:
1. Final per-room mood values and light role list.
2. Final bridge/shared landmark art and whether graph directions ship in demo.
3. Final menu/death/victory layouts, copy, CTA, and background images.
4. Final combat feel numbers: hitstop, directional punch, dash/attack/skill buffer windows, coyote windows.

## Top risks

- `RoomLoader` live path is `Assets/Scripts/Systems/Map/RoomLoader.cs`; there is also a separate JSON/static loader named `RIMA.Map.RoomLoader` (`Assets/Scripts/Map/Runtime/RoomLoader.cs:7-17`). Avoid touching the wrong one.
- Linear demo data and branching `DungeonGraph` are currently separate systems.
- Lighting is scene-wired, not data-wired, and hierarchy is inconsistent.
- UI has shared theme tokens but not shared screen lifecycle.
- Camera feel has one correct offset path and one risky transform-writing path.
- `VFXRouter` architecture exists but has empty scene entries, so code changes alone will not show VFX.

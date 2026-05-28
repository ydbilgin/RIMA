# Codex Verdict - Sang Hendrix Realtime Parallax Map Builder

Sources inspected:
- X post/video: https://x.com/sanghendrix96/status/2059176117769208034 (yt-dlp accessible, 28s, uploaded 2026-05-26)
- Itch page: https://sanghendrix.itch.io/realtime-parallax-map-builder-rpg-maker-mz-plugin (updated 2026-05-26 11:34 UTC)
- Trailer video: https://www.youtube.com/watch?v=IkrQIKoLw40
- Drag-n-drop video: https://www.youtube.com/watch?v=erXuNhARF-8
- Grid-free painting video: https://www.youtube.com/watch?v=zFzx7AkOJhs
- Tile-free painting tutorial: https://www.youtube.com/watch?v=dpjpdsiQNHQ
- Long how-to video: https://www.youtube.com/watch?v=Q4redsN-dO8
- Devlogs: v1.1.4, v1.1.5, v1.1.6, v1.2.0, 2026-05-26 tutorial post
- Community threads: speed/runtime commands, occlusion opacity, resizing, custom art, collision, weather effects, performance, animated doodads
- RIMA files: `Assets/Scripts/Background/ParallaxLayer.cs`, `STAGING/BG_LAYER_ARCHITECTURE_VERDICT.md`, `Assets/Scripts/Editor/MapDesigner/UnifiedMapDesigner.cs`, `Assets/Scripts/Editor/MapDesigner/MinimalTilePainter.cs`, `Assets/Scripts/Editor/MapDesigner/CliffGenerateAction.cs`

## Q1 Core feature (200 word)

Sang Hendrix's Realtime Parallax Map Builder is an in-game visual authoring plugin for RPG Maker MZ. Its core feature is not only that it renders parallax images. It turns the whole parallax-map workflow into direct manipulation: create a layer, pick an image from the RPG Maker `Parallaxes` folder or subfolders, drag it over the live map, then tune position, z-index/order, opacity, scale, blend mode, looping, seamless expansion, fixed-to-map behavior, depth/parallax movement, snap behavior, and occlusion while the game view updates immediately. The editor sits over the running game, so the designer sees the exact camera, player, layer, and occlusion interaction instead of exporting a PNG from Photoshop/GIMP and guessing coordinates through plugin strings.

The newer v1.2.0 feature expands the tool from parallax-image placement into grid-free tile/object painting. A layer can be converted to a tilemap-like paint target, then object chips are selected from a palette and placed without RPG Maker's normal grid restrictions. The videos show right-click conversion, palette thumbnails, right-click erase, middle-mouse priority changes, and auto sorting against character depth categories. The enablement is authoring speed and preview confidence: the map can be composed, tested, reordered, and corrected in realtime.

## Q2 Visual mekanikler (bullet list)

- Layer count: Public copy and a community answer say layer count is unlimited. Videos show practical authoring stacks: 1 tilemap-paint layer in v1.2 demos, 2-3 parallax layers in the trailer, and repeated foreground/tree/object layers in drag-n-drop demos. For RIMA, treat unlimited as editor flexibility, not a runtime target.
- Parallax factor pattern: The UI exposes a depth/parallax effect rather than visible numeric Unity-style factors. Trailer captions call out `Feature: Depth`, `Seamless parallax and loop`, and `Fix your parallax to map`. The pattern is per-layer depth plus optional map-fixed or looping behavior.
- Camera-driven vs subject-driven: Core parallax is camera-driven. Player/camera movement pans the scene and parallax layers respond. Animated parallax is subject-driven: selected image sequences, such as rain frames, animate independently of camera movement. Endless images can move like clouds.
- Particle/weather/fog: Rain in the demo is confirmed by Sang as default RPG Maker weather. Other weather-like effects can be built with moving/endless images through this plugin. There is no separate particle-system editor in the inspected footage.
- Lighting/time-of-day: No first-class lighting or day-night tool was proven. Visual lighting comes from authored PNG layers, blend modes, blurred overlays, and RPG Maker/weather effects.
- Foreground occlusion: Confirmed. Trailer text says `Occlusion: hide images above player`. v1.1.6 added occlusion opacity control. The long how-to shows the user drawing no-pass/occlusion-style areas with the pencil/collision controls.
- Dynamic spawning: No evidence of event-driven layer creation at runtime. Community answer says plugin commands currently show/hide layers only; scroll speed changes are editor-only in the current version.
- Grid-free painting: X/video and v1.2 footage show converting a layer to tilemap painting, picking chips from a right-side palette, placing trees/grass/props freely, using middle mouse to change priority/depth, and relying on auto sorting so chips go below, same as, or above characters.
- Visual feel: The feel is runtime Photoshop-lite over an RPG Maker map: green compact editor, live game view, direct drag handles, layer list, asset palette, visible selection boxes, and immediate feedback.

## Q3 Teknik mimari

Confirmed from public material:
- It is an RPG Maker MZ JavaScript plugin. The itch page says the plugin is temporarily obfuscated, so source-level internals were not available.
- The editor is in the running game, not in the external RPG Maker editor. Community feedback also notes that this kind of parallax setup does not show as normal map content in the RPG Maker editor because of engine limitations.
- Asset inputs are PNG/image resources from the `Parallaxes` folder. v1.1.6 adds direct file reading and subfolder support.
- Animated parallax uses multiple selected files. Sang states frame count is unlimited because users can select any amount of files.
- v1.1.5 adds auto Z-sorting based on player Y position.
- v1.2.0 adds grid-free tilemap/object painting and changes the way the builder is opened.
- Collision/passability is mixed: earlier answers recommend normal parallax-map collision methods; current page advertises `Grid & Paint` for impassable areas and the tutorial shows drawing collision/passability after visual painting.

Inferred architecture:
- Rendering is almost certainly Pixi.js display-list composition around RPG Maker MZ `Spriteset_Map`: plugin-created Sprites are inserted above/below the tilemap, characters, weather, and UI according to z-index/order. There is no evidence of mesh generation.
- Each layer likely serializes image key/path, x/y, z-index, opacity, scale, blend mode, loop/seamless flags, fixed-to-map flag, depth factor, animation frames, lock state, and occlusion metadata.
- Realtime preview is implemented by mutating the live Sprite/layer objects in-game as UI values change, then persisting the map/layer metadata.
- Grid-free painting probably creates many placed sprite/object records from tile/object chips rather than changing RPG Maker's underlying tile grid one-to-one.

Performance read:
- The page claims cross-platform desktop/mobile/web and `performance friendly` behavior.
- Community reports confirm the usual parallax risk: very large images, such as 4k PNGs, can drop FPS. One 20 FPS case was later traced to duplicate/conflicting plugins, but the large-image risk remains real.
- Practical budget should be many small/medium sprites and atlased/tiled strips, not huge full-map PNGs. Pixi.js does not give the same batching model as Unity SpriteAtlas + URP 2D.

## Q4 Unity adapt tablosu

| Plugin feature | Unity karsiligi | RIMA icin uygunluk |
|---|---|---|
| Layered parallax images | SpriteRenderer children under RoomBackgroundRig with Sorting Layer/Order | Already compatible with RIMA BG kit |
| Realtime depth/parallax | Existing `ParallaxLayer.cs` factor X/Y in `LateUpdate` | Already present, but authoring UX is missing |
| Pixel/snap movement | Existing `snapToPixel` and `pixelsPerUnit` | Strong fit with Pixel Perfect Camera |
| Fixed to map vs depth layer | Factor presets: 1.0 map-locked, 0.0 static, 0.03-0.14 BG depth | Good, expose as presets |
| Seamless/loop layer | Tiled SpriteRenderer, repeated child sprites, or scrolling material UV | Good for L0 void, L2 strips, L4 fog |
| Drag-n-drop placement | Unity SceneView handles + SpriteRenderer/ObjectField binding | High value for MapDesigner |
| Layer reorder by drag | `UnityEditorInternal.ReorderableList` or custom list | High value, low runtime risk |
| Double-click rename | Editor inline rename for layer row/GameObject | Easy |
| Lock/duplicate | Lock transform edits, duplicate layer GameObject/preset | Easy |
| Scale from corner | SceneView scale handle with PPU-safe warnings | Useful but must constrain scaling |
| Blend mode | Sprite Lit/Unlit/Additive materials or Shader Graph presets | Good, but Pixi blend modes are not 1:1 |
| Animated parallax | Animator/Sprite sequence, ParticleSystem, or UV scroll | Better in Unity than RPG Maker |
| Weather/moving cloud | ParticleSystem, URP 2D VFX, scrolling fog shader | Good, keep separate from base parallax |
| Occlusion fade above player | Collider2D/zone + SpriteRenderer alpha fade + SortingGroup rules | Very high RIMA value |
| Region ID occlusion | Trigger volumes, room masks, or designer-painted zones | Good; do not copy RPG Maker Region IDs literally |
| Auto Z-sort by player Y | SortingGroup or renderer sorting order from Y | Good for props/front layers, not far BG |
| Show/hide layer command | Room event/state presets, alpha tweens, enable flags | Strong for combat/boss/treasure variants |
| Grid-free object painting | SceneView sprite/prefab stamp painter with depth category | Strong MapDesigner candidate |
| Collision painting | Existing Tilemap/room data/collider tools | Fit, but keep gameplay collision separate from art |
| Unlimited layers | Editor may allow many; runtime budget should cap | Do not adopt literally |

## Q5 RIMA icin 3 oneri (yapisal)

### Oneri 1: Parallax Layer Authoring Tab

- Sang'ta nasil: The designer creates layers, picks/imports sprites, drags them in the live map, reorders layers, locks/duplicates them, and immediately sees the result.
- RIMA'ya adapt: Add a `Parallax` tab to `UnifiedMapDesigner`. It should list RoomBackgroundRig layers with visibility, lock, thumbnail/name, sorting order, factor preset, X/Y factor sliders, material/blend preset, `Recapture origin`, and direct SceneView handles for placement.
- Etkilenecek dosya: `Assets/Scripts/Editor/MapDesigner/UnifiedMapDesigner.cs`, new `Assets/Scripts/Editor/MapDesigner/ParallaxDesigner.cs`, optional inspector helpers for `Assets/Scripts/Background/ParallaxLayer.cs`.
- Implementation cost: Orta, 2-3 gun.
- ROI: Very high. RIMA already has the runtime parallax component and locked layer architecture; the missing value is Sang's fast authoring loop.

### Oneri 2: Foreground Occlusion/Fade Zones

- Sang'ta nasil: Images above the player can hide/fade when the player passes under them; v1.1.6 adds occlusion opacity control and the trailer calls out occlusion as a feature.
- RIMA'ya adapt: Add `ParallaxOcclusionZone2D` for front fog, cliff lips, island overhangs, branches, and mist. On player trigger enter, fade target SpriteRenderer alpha to a configured opacity; on exit, restore. Keep it independent from far BG layers.
- Etkilenecek dosya: new `Assets/Scripts/Background/ParallaxOcclusionZone2D.cs`, optional MapDesigner zone authoring in `Assets/Scripts/Editor/MapDesigner/ParallaxDesigner.cs`, BG rig prefab setup.
- Implementation cost: Dusuk/orta, 1-2 gun.
- ROI: High. This creates immediate ARPG readability and a premium depth feel while protecting control clarity.

### Oneri 3: Animated/Stateful Atmospheric Layers

- Sang'ta nasil: Animated parallax is built from selected frame files; endless/moving images can create weather/cloud behavior; plugin commands can show/hide layers.
- RIMA'ya adapt: Split static parallax from small runtime modules: `ParallaxLayerAnimator` for sprite sequences/UV scroll/alpha pulse, and `ParallaxLayerStatePreset` ScriptableObject for combat, boss, treasure, ritual, and corridor layer states.
- Etkilenecek dosya: `Assets/Scripts/Background/ParallaxLayer.cs`, new `Assets/Scripts/Background/ParallaxLayerAnimator.cs`, new `Assets/Scripts/Background/ParallaxLayerStatePreset.cs`, optional room-state hookup.
- Implementation cost: Orta/yuksek, 2-4 gun depending on editor polish.
- ROI: High for boss/ritual rooms, medium for baseline combat rooms. It turns the 3-kit BG architecture from static depth into living atmosphere.

## Q6 Risk / pitfall

- Pixi.js display order is not Unity URP 2D sorting. RPG Maker z-index/Region IDs should be translated into Sorting Layers, SortingGroups, Collider2D zones, and explicit renderer order, not copied one-to-one.
- Sang's editor is runtime/in-game. RIMA should implement authoring in Unity Editor/SceneView and keep runtime components lean. Shipping a player-facing map editor is unnecessary scope.
- Pixel Perfect Camera conflicts with arbitrary scaling. Sang's bottom-right resize handle is good UX, but RIMA must constrain scale to PPU-safe values or show warnings to avoid soft pixels and shimmer.
- Huge full-map PNGs are a trap. RIMA's locked architecture should stay modular: tileable L0/L2/L4 strips, transparent L3 islands, controlled atlases, exact PPU, and no anti-aliased halos.
- `Unlimited layers` is not a production budget. Keep the existing RIMA target of roughly 8-14 BG SpriteRenderers and 30-45 total room renderers for normal rooms.
- Lighting/time-of-day is not a proven core feature of this plugin. RIMA should use URP 2D Lights, additive materials, LUT/profile swaps, and ParticleSystems rather than expecting parallax layers alone to solve lighting.
- Grid-free painting is visual placement, not full gameplay collision. RIMA should let the editor feel unified, but keep floor visuals, walkability, hitboxes, nav/collision, and combat readability as separate data.
- Sang's asset source is Photoshop/GIMP/parallax-art friendly and the itch page marks `No generative AI was used`. RIMA can use PixelLab, but needs cleanup rules: transparent PNG QA, tiling checks, atlas packing, and stable dimensions.

## Q7 UI/UX inspection + RIMA MapDesigner adapt

Observed Sang UI patterns:
- Editor panel layout: compact green overlay over the live game. Layer list sits on the left; selected-layer controls sit on the right; asset/tile-chip palette appears as thumbnail grid. The game map stays visible behind the tool.
- Grouped controls: create/add layer, layer selection, image picker, position X/Y, z-index/order, opacity, scale, blend, loop/seamless, fixed-to-map, depth/parallax, occlusion, and grid/collision painting are exposed as small direct controls.
- Drag-drop/live update: images are dragged from file picker/palette into the map; selected items show yellow/visible bounds; changes update in the running scene immediately.
- Layer ordering UX: v1.1.6 removed reorder buttons and replaced them with mouse drag reordering. Layer names are renamed by double click. Rows include active/visibility-style state and selected highlight.
- Visual feedback: selection rectangle, highlighted active layer row, grid overlay, palette thumbnails, priority/depth categories, live camera pan, and immediate auto-sort feedback.
- Onboarding/discoverability: public copy reduces the first use to three steps: create a layer, import image, see changes instantly. Devlog adds tutorial/Discord buttons. v1.1.4 added bottom-right open button so users are not forced to remember a keyboard shortcut.
- Shortcuts/context ergonomics: right click converts a layer to tilemap painting; right click erases; middle mouse changes chip priority/depth; movement keys pan the preview camera; layer context menu handles duplicate/lock-like flows.

RIMA MapDesigner current state:
- `UnifiedMapDesigner.cs` already has a toolbar, tab bar, shortcut strip, scrollable content, and status bar, but only Tile Painter and Room Builder tabs.
- `MinimalTilePainter.cs` already has responsive side library, search, thumbnail size slider, grouped theme headers, active selection card, brush/mode/size controls, SceneView hover diamond, paint/erase hotkeys, Unity Undo, and context menu group reassignment.
- Missing relative to Sang: no parallax-specific tab, no BG SpriteRenderer reorder workflow, no factor/depth preview, no occlusion authoring, and no camera-pan preview for parallax movement.

Three RIMA UX patterns to adapt:

1. Layer ordering UX
- Add a Parallax tab with a reorderable layer list. Each row should show visibility, lock, sprite thumbnail/name, sorting order, factor badge, and material/blend preset.
- Drag reorder should map to deterministic RIMA sorting ranges: L0 -500, L2 -420, L3 -350, L4 -300, L1 -250, FrontFX 600.
- Context menu: duplicate, recapture origin, convert to fog/front layer, ping sprite asset, lock transform.

2. Parallax factor adjust UX
- Replace raw math-first editing with presets: Void, Far Ruins, Islands, Fog, Rift, FrontFX, Custom.
- Put X/Y sliders beside a `Preview Pan` scrub so designers can see camera-driven depth without entering Play Mode.
- Keep pixel snap and PPU warning in the same row because `ParallaxLayer.cs` already protects against sub-pixel shimmer.

3. Realtime preview UX
- Add SceneView preview mode that applies temporary camera delta to selected `ParallaxLayer` components without dirtying runtime origins.
- Provide `Reset Preview`, `Recapture Origins`, and `Frame BG Rig` actions.
- Draw gameplay camera rectangle, layer bounds, and occlusion/fade zones. This copies Sang's immediate feedback while staying native to Unity Editor.

Why Sang's UI is good:
- Direct manipulation reduces cognitive load: the user drags art where it belongs instead of editing coordinates by guesswork.
- Immediate feedback builds trust: layer movement, occlusion, painting, priority, and camera pan are visible in the same viewport that gameplay uses.
- Fitts's law: frequent actions are nearby direct map gestures or large panel buttons.
- Recognition beats recall: thumbnails, layer rows, and context menus beat memorized plugin strings.
- Progressive disclosure: first useful action is create/import/drag; advanced settings are available after selection.
- Undo/erase safety matters. RIMA should preserve Unity Undo for every parallax/map edit and keep right-click erase/context behavior where it fits.

## Critical insight

The best thing to copy is not Sang's exact RPG Maker renderer. It is the authoring loop: layer list, direct drag placement, live depth preview, occlusion/state controls, and asset thumbnails in the same view as gameplay. RIMA already has enough runtime parallax technology; the missing high-ROI piece is a MapDesigner-grade editor that makes BG depth fast to compose, test, and adjust.

## Implementation priority

| Oneri | Cost | ROI | Priority |
|---|---|---|---|
| 1. Parallax Layer Authoring Tab | 2-3 gun | Very high | P0 |
| 2. Foreground Occlusion/Fade Zones | 1-2 gun | High | P0 |
| 3. Animated/Stateful Atmospheric Layers | 2-4 gun | High/medium | P1 |

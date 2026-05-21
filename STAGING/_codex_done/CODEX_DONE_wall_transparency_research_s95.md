# Wall Transparency Research - Codex Report

Scope: research + audit only. No code, asset, scene, or meta file was changed.

Project facts checked by shell:
- Unity: 6000.3.6f1
- URP: com.unity.render-pipelines.universal 17.3.0
- Active URP asset: `Assets/Settings/UniversalRP.asset`
- Active renderer data: `Assets/Settings/Renderer2D.asset`
- 2D renderer depth/stencil buffer: enabled
- Global graphics transparency sort: Custom Axis `{x:0, y:1, z:0}`
- Renderer2D transparency sort mode serialized as `0`, axis `{x:0, y:1, z:0}`; verify in editor before implementation.

Important confidence note: exact internal code for the listed commercial games is mostly not public. The game entries below are practical technical inferences from shipped behavior plus public engine/production information, not claims about their private source code.

## Bolum 1: Industry Techniques

### Hades
- Yontem: Mostly authored foreground/occluder layering with selective alpha fade, not a free-form physics occlusion system.
- Trigger: Likely local trigger volumes or camera/player zone checks on tall foreground pieces; player-to-wall Y relation is enough for many authored arena props.
- Visual: Soft full-object fade or partial fade on readability-critical foreground silhouettes; no heavy circular cutout look.
- Performance: Cheap. Static authored occluders + small trigger list; no need for many per-frame raycasts in arena-scale rooms.

### Stardew Valley
- Yontem: Tile/sprite layer swap or alpha fade for large foreground tiles such as roofs/tree canopies.
- Trigger: Player enters a rectangular building/tree/canopy footprint or crosses behind an object boundary.
- Visual: Whole roof/canopy/foreground piece becomes semi-transparent so the player remains visible.
- Performance: Very cheap. Grid/tile occupancy and bounding checks; no expensive shader or depth system needed.

### Diablo IV / II Resurrected
- Yontem: 3D depth-aware occlusion handling: camera ray/visibility test plus shader treatment, often outline/ghost/cutout for blocked actors.
- Trigger: Camera-to-player line of sight intersects walls, pillars, or foreground geometry.
- Visual: Environment can cut away/fade, or character receives an outline/through-wall readable silhouette.
- Performance: Moderate but standard for 3D. Uses depth/stencil/shader work and spatial queries; cost scales with visible occluders, not whole map.

### Hyper Light Drifter
- Yontem: 2D authored layering with minimal foreground transparency; readability is mostly solved by map composition and collision layout.
- Trigger: Tile/object layer order and authored no-hide zones; if used, simple overlap bounds on foreground pieces.
- Visual: Usually hard layering and composition, with rare/full-object fade rather than Diablo-style holes.
- Performance: Very cheap. Tile layer order and occasional object fade.

### Disco Elysium
- Yontem: 3D isometric scene composition with camera occluder management; likely mesh/material fade for objects between camera and character.
- Trigger: Camera ray/frustum test from camera to character or proximity to foreground mesh volumes.
- Visual: Foreground geometry becomes transparent/soft, preserving the painted-scene look.
- Performance: Moderate. Static 3D meshes make occlusion queries manageable; shader alpha/dither keeps transitions readable.

### CrossCode
- Yontem: 2.5D pixel layering/elevation model: tile layers, height levels, and sprite order do most of the work.
- Trigger: Object/tile height and Y/depth sorting; special foreground pieces can use overlap/region checks.
- Visual: Primarily correct front/back ordering; if foreground blocks gameplay, fade/cutout is object-authored.
- Performance: Cheap to moderate. CPU tile/layer sorting is predictable; shader complexity stays low.

## Bolum 2: Unity 2D Implementation Paths

### Path A - Sprite Swap
- Yontem: Each occluding wall has normal and faded sprite/material state. A trigger collider or bounds check toggles state.
- Kod scope: `WallOccluder2D` MonoBehaviour, trigger/bounds registration, player reference, optional `SpriteRenderer` list cache.
- Shader change: Not required if using SpriteRenderer color alpha or alternate material/sprite.
- 2D Renderer Asset config: No required change. Existing URP 2D setup can render alpha sprites.
- Sorting layer interaction: Works even if walls are above player, because the wall itself fades. It does not solve wrong Y-sort by itself.
- Compute cost estimate: CPU <0.02 ms for dozens of occluders if event-driven; near-zero GPU beyond normal transparent overdraw.
- Pros/Cons: Fastest and safest. Discrete states unless alpha is animated through code.

### Path B - Alpha Shader (URP)
- Yontem: Sprite-Lit or Sprite-Unlit shader multiplies alpha by `_OcclusionAlpha`; C# drives smooth fade with MaterialPropertyBlock.
- Kod scope: `WallOccluder2D`, cached renderers, fade coroutine/update, layer/tag query for player overlap.
- Shader change: Yes. Use Shader Graph or a small custom URP-compatible sprite shader with alpha multiply. MaterialPropertyBlock avoids material instances.
- 2D Renderer Asset config: Usually no structural change. Verify Renderer2D uses correct default lit/unlit material and transparency sorting. Current depth/stencil buffer is already enabled.
- Sorting layer interaction: Best when physical walls and actors share a Y-sorted world layer or compatible SortingGroup setup. If `Walls` stays above `Entities`, fade is still required whenever the player is behind/under a wall.
- Compute cost estimate: CPU 0.02-0.08 ms for tens of active occluders; GPU cost is normal transparent overdraw plus one alpha multiply, usually <0.1 ms on desktop for this scale.
- Pros/Cons: Smooth, controllable, production-friendly. Requires shader/material discipline.

### Path C - Stencil Mask
- Yontem: Player or camera-space mask writes stencil/depth, wall material reads it and cuts/fades a hole around the player.
- Kod scope: Mask object attached to player/camera, stencil-compatible wall material, renderer order validation, possible custom RenderObjects/2D Renderer setup.
- Shader change: Yes. Needs stencil read/write passes or Shader Graph/custom shader that exposes stencil behavior.
- 2D Renderer Asset config: Depth/stencil must be enabled; it is enabled in `Assets/Settings/Renderer2D.asset`. May need renderer feature/pass support depending on URP 2D material path.
- Sorting layer interaction: Strong for "hole around player" even when walls draw over actors, but fragile if render queues/layers are inconsistent.
- Compute cost estimate: CPU near-zero after setup; GPU 0.05-0.3 ms depending on mask size, overdraw, and render pass count.
- Pros/Cons: Most Diablo-like and visually premium, but highest setup risk for a 2D URP project.

## Bolum 3: Sorting Layer Audit

Audit command basis:
- Parsed `ProjectSettings/TagManager.asset` for layer IDs.
- Searched active non-archive `Assets/` files for `m_SortingLayerID: <id>` and `sortingLayerName: <name>`.
- Excluded obvious archive/recovery paths from the active count: `_ARCHIVE`, `Archive`, `_Recovery`, `_archive_faz1`, `_archive_karar150`.

### Mevcut Layer Kullanim Matrisi

| Layer | Unique ID | Active asset file count | Occurrences | Asset or code examples | Verdict |
|---|---:|---:|---:|---|---|
| Default | 0 | 85 | 307 | `Assets/Prefabs/Player.prefab`, many enemy/prop/VFX prefabs, `PathC_BaseTest.unity` renderer entry | KEEP for fallback only; migrate gameplay renderers off it over time |
| Patch | 1365605006 | 1 | 62 | `Assets/Scenes/Phase1_ProceduralMap_Test.unity` | CONDITIONAL DELETE after migrating/archiving test scene |
| Scatter | 27625511 | 1 | 8 | `Assets/Scenes/Phase1_ProceduralMap_Test.unity` | CONDITIONAL DELETE after migrating/archiving test scene |
| Detail | 351335743 | 0 | 0 | none | DELETE candidate |
| Accent | 1570199623 | 0 | 0 | none | DELETE candidate |
| Props | 399489520 | 0 | 0 | none | DELETE candidate |
| Ground | 2024493761 | 3 | 3 | `Assets/Prefabs/Rooms/Act1/combat_01.prefab`, `corridor_01.prefab`, `reward_01.prefab` | KEEP or rename to Floor |
| Walls | 593505845 | 3 | 3 | same Act1 room prefabs | KEEP as current canonical wall layer if not renaming |
| Entities | 1293760285 | 14 | 14 | active enemy prefabs, `Assets/Prefabs/RewardPickup.prefab`, some obstacles/skills | KEEP |
| VFX | 200 | 2 | 2 | `Assets/Prefabs/Environment/MapFragment.prefab`, `PlayerStartMarker.prefab` | KEEP |
| Wall | 2024493762 | 0 | 0 | none in active serialized assets | DELETE orphan after fixing tool drift |

### Drift Notes

- `Patch`, `Scatter`, `Detail`, `Accent`, `Props` are created by `Assets/Editor/RimaSortingLayerValidator.cs`. Only `Patch` and `Scatter` have active serialized usage, and only in `Assets/Scenes/Phase1_ProceduralMap_Test.unity`.
- `Walls` is the active serialized wall layer.
- `Wall` is an orphan layer in current assets, but `Assets/Editor/DevTools/IsometricSortSetup.cs` still creates and assigns `"Wall"`. That script is the likely source of the duplicate. Do not delete `Wall` without also replacing that tool's string with the canonical layer name in a future code-change task.
- `PathC_BaseTest.unity` does not currently show active wall/entity sorting-layer IDs beyond Default and light layer application masks. That scene likely needs a renderer/layer pass before wall transparency implementation.

### Onerilen Temiz Layer Set

Option 1 - Minimal cleanup, lowest risk:

```text
Default        fallback only
Ground         floor tilemaps
Walls          structural walls / occludable wall renderers
Entities       player, enemies, pickups, gameplay obstacles
VFX            gameplay VFX
UI             add only if world-space UI renderers require it
```

Actions:
- Delete `Detail`, `Accent`, `Props` if no hidden dependency is found in editor.
- Delete `Wall` after `IsometricSortSetup.cs` is updated to use `Walls`.
- Keep `Patch` and `Scatter` only until `Phase1_ProceduralMap_Test.unity` is migrated or archived.

Option 2 - Cleaner final hierarchy for wall transparency:

```text
Background
Floor
FloorOverlay
WorldYSort
Overhead
VFX
UI
```

Mapping:
- `Ground` -> `Floor`
- `Patch`/`Scatter`/`Detail`/`Accent` -> `FloorOverlay` if still needed
- `Walls` + `Entities` -> `WorldYSort` for any object that must sort by Y with the player
- `Overhead` only for always-over foreground art that fades aggressively

Recommendation: use Option 1 for the next implementation pass, then consider Option 2 when the renderer/prefab library is stable. Renaming layers now can cause broad serialized diffs.

## Final Mimari Onerisi

- Technique: Hades/Stardew style authored occluder fade, not Diablo stencil holes for v1.
- Unity path: Path B - Alpha Shader with MaterialPropertyBlock, plus simple overlap/Y-band trigger.
- Trigger rule: An occluder fades when player bounds overlap the wall's screen-space or world-space occlusion zone and the player is visually behind/under it. For isometric grid scale `(1, 0.5, 1)`, use world bounds first, then tune with per-wall `fadeZone` colliders.
- Sorting layers: Keep `Ground`, canonicalize on `Walls` (not `Wall`), keep `Entities`, keep `VFX`. For occludable walls that must Y-sort with the character, move those renderers into the same Y-sort group/layer strategy during implementation; otherwise the alpha fade must cover all overlaps.
- Visual: Smooth fade to 35-55 percent alpha, not full invisible. Optional outline/silhouette on player if readability is still weak.
- Effort estimate: 1 focused day for prototype in `PathC_BaseTest.unity`; 2-3 days for production-ready prefab conventions, shader, editor validation, and layer cleanup. Stencil path is closer to 1 week because render order and URP 2D stencil behavior need careful testing.

Implementation order:
1. Decide canonical wall layer: `Walls`.
2. Fix/remove `Wall` drift source in `IsometricSortSetup.cs` in a future code task.
3. Add alpha-capable wall material/shader.
4. Add `WallOccluder2D` with cached renderers and overlap/Y-band trigger.
5. Apply only to L2b/L3 wall prefabs in `PathC_BaseTest.unity`.
6. Tune alpha floor, fade speed, and collider bounds in scene.
7. Cleanup unused layers only after serialized references are migrated.

## Acik Sorular

- Are current production walls tilemap renderers, sprite prefabs, or both? The implementation hook differs.
- Should wall fade be full-object fade, top-half fade, or player-local cutout for tall L3 walls?
- Is mobile/low-end performance a target for v1, or desktop-only while the visual language is still being locked?
- Should `Phase1_ProceduralMap_Test.unity` remain active? It is the only active non-archive usage of `Patch` and `Scatter`.

## Source Notes

- Unity 2D sorting manual: https://docs.unity.cn/Manual/2DSorting.html
- Unity URP 2D Renderer documentation: https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@17.3/manual/2DRendererData_overview.html
- Unity SpriteRenderer sorting/material API reference: https://docs.unity3d.com/ScriptReference/SpriteRenderer.html
- CrossCode engine/public technical context: https://www.moddb.com/games/crosscode/news/optimizing-an-html5-game-engine-using-composition-over-inheritance

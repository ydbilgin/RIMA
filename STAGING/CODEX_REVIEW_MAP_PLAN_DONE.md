# Codex Review - MAP_PLAN_v1.md

## Verdict
ACCEPT_WITH_MODS

The plan is directionally correct, but it should not be locked as-is. The main blockers are scope mismatch, stale/ambiguous v15h claims, missing threat-budget implementation, and unresolved Phase 1.5 decisions.

## Strengths
- The Hades + STS direction is backed by existing code: `DungeonGraph` already has branching, reveal state, current-node traversal, and map reveal hooks (`Assets/Scripts/Core/DungeonGraph.cs:33`, `:75-77`, `:199-200`, `:226-239`).
- `DungeonMapUI` is not missing. It exists and handles M-key toggle plus node visibility/rebuild (`Assets/Scripts/UI/DungeonMapUI.cs:19`, `:82-84`, `:103`, `:176-241`).
- Map Fragment is already implemented in code: map fragments spawn on clear and call `DungeonGraph.RevealAhead(revealSteps)` on collect (`Assets/Scripts/Core/LegacyRuntimeRoomManager.cs:653-669`, `Assets/Scripts/Core/MapFragment.cs:119-127`).
- The RoomTemplate direction matches current assets and tooling: `RoomTemplateSO` is a ScriptableObject with room id, type, bounds, spawn sockets, and tags (`Assets/Scripts/MapDesigner/Room/Data/RoomTemplateSO.cs:7-21`), and `RoomTemplateSaver` can save assets/prefabs (`Assets/Scripts/MapDesigner/Room/Editor/RoomTemplateSaver.cs:14-18`, `:92-100`).
- The library inventory in the plan is accurate at filename level: 10 `.asset` templates exist under `Assets/Data/Rooms/Library/`.

## Weaknesses / Risks
- MVP run length is underspecified. `MAP_PLAN_v1.md:23-32` says Warblade-only 10 minute loop with 8-9 rooms, but `MAP_PLAN_v1.md:76-83` also cites a 15-node Act 1. Live `DungeonGraph` is neither: it has 12 fixed nodes plus up to 2 optional fork nodes (`Assets/Scripts/Core/DungeonGraph.cs:88-117`). Decide whether MVP is a vertical slice or full Act 1.
- Current enum names do not match the plan. Code has `Combat, Elite, Boss, Chest, Merchant, Forge, Event, Curse, Corridor` (`Assets/Scripts/Core/RoomType.cs:7-18`). The plan uses Unknown, Shop, Curse Gate, Spirit, Rest, Shrine, Treasure. This will leak into UI, gates, rewards, and templates.
- Existing RoomTemplate assets are prototype scale, not the canonical sizes from `MAP_PLAN_v1.md:107-120`. Actual library bounds are 6x6 to 16x10, while plan targets combat rooms around 24x18 to 28x20.
- Threat Points are plan-only. Requested filename search for `*ThreatBudget*` or `*ThreatPoints*` returned no files under `Assets/Scripts`; current room spawning is count-based (`LegacyRuntimeRoomManager.cs:35-37`, `:253-265`, `:303-372`).
- Encounter/wave implementation is partial. Requested filename search for `*Encounter*` or `*WaveSpawner*` found only `Assets/Scripts/Data/EncounterSlot.cs`; the runtime has `SpawnEnemies(count, forceElite, spawnRunId)` but no named wave/encounter budget system.
- The allocation lock path in the task, `memory/project_5000_pixellab_allocation_lock.md`, does not exist. Repo search found references to the lock in `CURRENT_STATUS.md`, but the actual lock file was not found, so collision validation is not fully evidence-backed.
- Phase 1.5 is not ready to serve as a final dependency until its 5 open questions are resolved (`STAGING/PHASE_1_5_ROOMDATA_SPEC_DRAFT.md:320-325`).

## Concrete recommendations
1. (priority HIGH) - Change MVP definition to "8-9-room vertical slice, not full Act 1" because the plan's 10 minute target conflicts with the 15-node canonical Act 1 and live 12-14 node graph.
2. (priority HIGH) - Normalize room taxonomy before implementation: map `Shop` to `Merchant`, `Unknown` to a real enum or a graph-time hidden wrapper, `Treasure` to `Chest`, and decide whether `Rest/Spirit/Shrine` exist in Phase 1.
3. (priority HIGH) - Treat Threat Points as not implemented and add a pre-MVP task for `ThreatBudget` / encounter selection before relying on the 8-12 pt combat budget in `MAP_PLAN_v1.md:94-101`.
4. (priority HIGH) - Use `RoomTemplateSO` as the MVP source of truth. Let v15h generate draft layouts, then snapshot/save to templates; do not make live procedural v15h the runtime canonical path for Phase 1.
5. (priority MED) - Make 20 RoomTemplates a polish target, not a hard MVP gate. Ship gate can be 12-14 good templates if the 8-9-room slice has enough combat/elite/shop/boss coverage.
6. (priority MED) - Produce `Boss_Arena_01.asset` separately. `Boss_Intro_01.asset` is 14x10 with one boss spawn socket; it is not a full boss arena.
7. (priority MED) - Keep 4-6 MVP mobs if the loop is 8-9 rooms, but do not claim the 8-mob roster is done. Reserve the larger roster for post-MVP variety.
8. (priority LOW) - Do not block MVP on atmospheric L8 if readability and combat work. L5-L8 polish can follow after the RoomData renderer decision.

## Open question resolutions (for Opus)
- Section 11 #1: Use 20 templates as Phase 1 polish target, not MVP entry criteria. Evidence: current library has 10 assets, only 3 combat variants, 1 elite, and no Shop/Unknown/Boss_Arena (`MAP_PLAN_v1.md:134-164`).
- Section 11 #2: Yes, create a v15h-to-RoomTemplate snapshot path. Evidence: v15h already composes canvas + auto-populates + paints Wang + spawns player (`RimaV15hPlayableComposer.cs:82-88`), and existing saver infrastructure can write `RoomTemplateSO` (`RoomTemplateSaver.cs:14-18`, `:92-100`).
- Section 11 #3: Resolve before final Map Plan. The 5 Phase 1.5 questions affect renderer, serialization, determinism, Wang ownership, and gameplay entity modeling, so they change implementation shape.
- Section 11 #4: DungeonMapUI exists; change the task from "implement" to "audit/integrate/test". Evidence: `Assets/Scripts/UI/DungeonMapUI.cs` exists and handles M-key toggle plus visible node construction.
- Section 11 #5: Lock MVP biome name to `Shattered Ruins / Shattered Keep`; treat `Sunken Keep` as flavor/variant later. Splitting one-biome MVP into two names creates asset taxonomy drift.
- Section 11 #6: Boss arena should come from a new `Boss_Arena_01.asset`, not `Boss_Intro_01.asset`. The current asset is small and tagged boss/intro/dramatic.
- Section 11 #7: 4-6 mobs is enough for 8-9-room MVP, but implement budget selection first. Current spawning is count scaling, not Threat Points.
- Section 11 #8: Canonical pipeline should be `RoomData/RoomTemplate` primary, with v15h as an authoring generator. This matches `MAP_PLAN_v1.md:183-184` and Phase 1.5 source-of-truth language.

### Phase 1.5 5 questions - verbatim plus suggested answers
- `Chunk renderer strategy`: ChunkMesh (single draw call) vs BatchedSprites (Unity SpriteBatch)? Affects RendererType enum + 1.5C scope. Suggested answer: start with BatchedSprites behind an `IRoomChunkRenderer` interface for lower integration risk; benchmark/replace with ChunkMesh in 1.5D if needed.
- `RoomData serialization format`: Unity `[Serializable]` SO vs JSON ScriptedImporter vs binary? JSON inspectable; SO Unity-native. Suggested answer: Unity ScriptableObject as authoritative asset, JSON export/import only for debug/diff tooling.
- `Deterministic selector`: MackySoft Choice direct dep vs RIMA-owned `RimaWeightedSelector`? Eval recommends "wrapped". Suggested answer: RIMA-owned wrapper/API. MackySoft may be internal implementation, not direct gameplay dependency.
- `Wang16 edge owner`: RimaFeatureBrush computes at paint vs separate WangResolver re-compute? Affects dirty-chunk scoping. Suggested answer: separate WangResolver recompute on dirty bounds; brush records intent/hard feature, resolver owns edge correctness.
- `Entity vs VisualPlacement for L6/L7 props`: Shrine fragments need Collider - EntityRecord with prefab OR VisualPlacementData with deferred flag? Establish before 1.5A. Suggested answer: any collider/interactable/combat object is EntityRecord; decorative L6/L7 only is VisualPlacementData.

## Asset gap evidence
- Library actual file list: `Boss_Intro_01.asset`, `Combat_Large_01.asset`, `Combat_Medium_01.asset`, `Combat_Small_01.asset`, `Corridor_Linear_01.asset`, `Corridor_LShape_01.asset`, `Elite_01.asset`, `Shrine_01.asset`, `Spawn_01.asset`, `Treasure_01.asset`.
- Library actual template summary: `Boss_Intro_01 type=2 bounds=14x10 enemySpawns=1`; `Combat_Large_01 type=0 bounds=16x10 enemySpawns=6`; `Combat_Medium_01 type=0 bounds=12x8 enemySpawns=4`; `Combat_Small_01 type=0 bounds=8x6 enemySpawns=3`; `Corridor_Linear_01 type=8 bounds=12x4 enemySpawns=1`; `Corridor_LShape_01 type=8 bounds=10x8 enemySpawns=2`; `Elite_01 type=1 bounds=10x8 enemySpawns=3`; `Shrine_01 type=7 bounds=8x8 enemySpawns=0`; `Spawn_01 type=8 bounds=8x6 enemySpawns=0`; `Treasure_01 type=3 bounds=6x6 enemySpawns=0`.
- Missing MVP templates: Shop/Merchant, Unknown facade or mystery resolver, full Boss_Arena, extra combat variants, extra elite variant.
- DungeonMapUI: exists at `Assets/Scripts/UI/DungeonMapUI.cs`; related code also exists at `Assets/Scripts/Core/DungeonGraph.cs`, `Assets/Scripts/Core/MapFragment.cs`, and `Assets/Scripts/Core/LegacyRuntimeRoomManager.cs`.
- ThreatBudget: missing by requested filename search (`*ThreatBudget*`, `*ThreatPoints*`).
- Encounter system: requested filename search found only `Assets/Scripts/Data/EncounterSlot.cs`; no `WaveSpawner` file. Runtime has count-based spawn coroutine in `LegacyRuntimeRoomManager.cs`, not Threat Points/wave-budget encounter logic.
- 5000 PixelLab allocation lock: expected `memory/project_5000_pixellab_allocation_lock.md` path missing. Cannot fully verify budget collision against the lock file.

## Red flags
- `MAP_PLAN_v1.md:196-201` lists the 5 Phase 1.5 questions as `?`; the real draft has concrete questions at `PHASE_1_5_ROOMDATA_SPEC_DRAFT.md:320-325`. The map plan should inline them before Opus sign-off.
- `MAP_PLAN_v1.md:180` says Wang tiles 6/16, but current v15h test validates 16 tile assets and 16 RuleTile tiling rules (`V15hPlayableMapTests.cs:41-59`), and the v15h Wang asset folder has 16 tile assets. The live issue may be "6 placed transitions," not "only 6 variants available."
- `MAP_PLAN_v1.md:181` says L5-L8 atmospheric is 0. AutoPopulator supports L5/L6/L7/L8 paths and L8 caps (`AutoPopulator.cs:49-54`, `:192-242`, `:1415-1416`), so the issue is likely missing profile pools/placements, not missing code path.
- The reported "2 Warblade overlap" should be treated as a bug/stale scene artifact, not intentional. Composer destroys an existing `Warblade_v15h_Player`, then creates one player at canvas center (`RimaV15hPlayableComposer.cs:77`, `:532-576`).
- Code graph already has Event/Chest/Merchant in live nodes (`DungeonGraph.cs:83-117`), while the plan marks Event Phase 2 and uses Shop/Unknown MVP names. This taxonomy mismatch will cause implementation churn unless resolved now.

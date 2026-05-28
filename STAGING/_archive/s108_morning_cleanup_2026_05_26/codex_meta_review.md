# Codex Meta-Review - Final Lock Decision

## 1. AGREEMENTS WITH CHATGPT (consolidate, no debate)

- Lock editor-only RoomBank. Runtime procedural room drawing is the wrong risk for RIMA right now. The game needs curated combat readability, wall closure, prop collision, spawn sockets, and camera bounds more than runtime novelty.
- Lock the room count correction. The first production target should be 40-60 combat templates only after the first 10-room milestone proves timing. The 100 combat room target is a morale trap until measured with real L3/L4/L5 assets.
- Lock separate Props Mode. Props are gameplay entities, not another decorative paint layer. Treating them as L7 would hide collision, interaction, loot, breakability, sorting anchor, and spawn avoidance under a visual-layer abstraction.
- Lock RoomTemplateSO + prefab hybrid. Pure SO makes visual authoring painful; one Unity scene per room creates version-control and navigation friction at 100-300 rooms.
- Lock validator severity levels. Import validation must not become a production blocker for art-quality warnings. Errors block only when the asset cannot be used safely or deterministically.
- Lock the Sprint 10 reorder in principle: RoomTemplate + RoomBank vertical slice must happen before the full Natural Engine. Otherwise the team can build a strong editor that has not proven the game loop.
- Lock Markov deferral. It is not needed for V1. Composition roles plus Poisson plus anti-repeat cover the important visual risks with less tuning.
- Lock AI tag suggestion deferral. Template-based tags plus Preview override are enough. Sprite-content AI tagging is likely to add false confidence before the import loop is stable.
- Lock SpriteAtlas as non-blocking for V1. It should be planned, not implemented before the first vertical loop unless profiling proves draw calls are already the bottleneck.

## 2. DISAGREEMENTS WITH CHATGPT (challenge with evidence)

- ChatGPT's Sprint 9-13 sequence is right, but still too serial. RoomTemplate contracts should start in Sprint 9 as a thin stub while the importer is built. Waiting until Sprint 10 to discover RoomTemplate field needs will force importer metadata churn.
- ChatGPT underweighted one current-code blocker: Brush V1 still contains runtime scale behavior. `BrushLayerOperation.scaleRange = 0.85..1.15` and `DecorativeExecutorUtility.PlaceAt()` apply arbitrary scale. This directly violates the new locked rule: brush size must select native variants, not scale sprites. This is a P0 retrofit, not a polish item.
- ChatGPT's "simple room 10-15 min" estimate is still optimistic until the first save/load/RoomBank loop exists. Visual paint time may be 10-15 minutes; finished room time includes sockets, spawn safety, camera bounds, enemy spawn, exit validation, sorting, and one playthrough. Treat 20-40 minutes as the planning floor for combat rooms.
- ChatGPT accepted 40-60 combat rooms as a target, but the real variety dependency is encounter decoupling. A RoomTemplate must not own all enemy identity. It should expose spawn sockets, encounter tags, difficulty tags, and blockers; RoomBank plus EncounterBank should combine at runtime. Otherwise 40 rooms will feel finite fast.
- ChatGPT lists composition roles as an addition; I would promote them to the primary Natural Engine data model. Noise, Poisson, and anti-repeat are implementation details. The designer-facing abstraction should be zones: clean center, decorated edge, focal cluster, wall-adjacent prop band, door safety band, encounter avoid band.
- ChatGPT did not push hard enough on deterministic IDs. Variant IDs, room IDs, prop instance IDs, and socket IDs must be stable and human-readable. If ID generation is an afterthought, RoomTemplate diffs and migration become noisy fast.

## 3. ADDITIONAL INSIGHTS FROM REFERENCES

- Spelunky confirms the 80/20 workflow, but the lesson is not "procedural rooms." The lesson is layered authorship: handcrafted scaffold first, conditional decoration second, gameplay placement last. RIMA should mirror this as RoomTemplate scaffold -> Auto-Dress decoration -> sockets/props validation.
- Spelunky room counts support ChatGPT's correction. Roughly 50 layouts per tileset is a proven scale. RIMA does not need 100 combat rooms before it proves the first biome loop.
- Bridson Poisson should be implemented as a bounded-room grid, not a generic spatial hash. Use cell size `r / sqrt(2)`, preallocate by room bounds, and query the 3x3 neighbor cells. This makes deterministic editor baking cheap and testable.
- Houdini's density texture pattern maps cleanly to RIMA FeatureMaskSO. Do not create a separate density-mask concept unless FeatureMaskSO cannot hold it. One mask stack should drive density, spawn avoidance, and visual falloff.
- Houdini's "force total count" is important for hero accents and gameplay props. RIMA needs both density-based scatter and exact-count placement: "place exactly 2 braziers near north wall" is not solved by density alone.
- Tiled's Wang numerology is a warning for L3. If a wall transition wants real corner-set behavior, expect 16-ish semantic cases, not only horizontal/vertical/four corners. V1 can start with horizontal wall master, but the data model should not assume seven wall types is complete.
- Unity Sprite Atlas should be planned per biome, not global. A monolithic Brush atlas will couple unrelated biomes and make iteration heavy. V1 can ship without SpriteAtlas, but the folder/label structure should make per-biome packing easy later.
- Unity Prefab Brush validates Props Mode, but RIMA must add metadata Unity's generic brush does not know: bottom-center sorting anchor, footprint polygon/cells, combatBlocker, spawnAvoidRadius, interactable type, biome tags, and door/socket constraints.

## 4. CONCRETE SPRINT 9-13 ACTION LIST

### Sprint 9 - Importer + Metadata Retrofit + Room Contract Stub

- Owner: Codex for implementation, Opus/User for template defaults and L3 prompt validation.
- Estimate: 1.5-2 days.
- Dependencies: existing Brush V1 data, AssetPoolSO, Brush executors, one synthetic test PNG or first L3 horizontal master.
- Deliverables:
  - Add `SizeBucket`, `BrushAssetVariant`, `BrushRadiusProfileSO`, `SliceLayoutTemplateSO`, `SliceCell`, `ImportResult`, `ValidationIssueSeverity`.
  - Extend `AssetPoolSO` without deleting existing `sprites` immediately. Add migration path from legacy sprites to variants.
  - Remove arbitrary decorative scale from new variant execution path. `scaleRange` becomes legacy-only or integer-scale warning; default new path uses native sprite size.
  - Implement `BrushAtlasImporter.Import()` with TextureImporter settings, template-driven spritesheet metadata, AssetPoolSO write/update, deterministic variant IDs, and validator call.
  - Implement `BrushAtlasValidator` with Error/Warning/Info output. Error: unreadable PNG, wrong master size for selected template, duplicate IDs, missing bucket/layer, non-Point filter, compression/mipmaps, L3 gutter cross fail for corner template. Warning: palette drift, AA suspicion, outline drift, low alpha noise, border proximity if still usable. Info: variant count, bucket distribution, estimated memory.
  - Implement minimal `BrushVariantPreviewWindow`: grouped bucket grid, tags, weights, sample distribution for radius 1/4/7/10.
  - Add starter SliceLayoutTemplateSO assets for L3 horizontal only plus L4/L5/L6 synthetic layouts. Do not spend time perfecting every L3 orientation before the first loop.
  - Add a thin `RoomTemplateV1` contract stub with schemaVersion, roomId, biomeId, roomType, bounds, doorSockets, playerSpawn, enemySpawnSockets, cameraBounds, prefabRef placeholder.
- Exit criteria:
  - One test master imports into an AssetPoolSO with variants.
  - Validator returns structured severity issues.
  - Existing Brush data tests still pass or have explicit migration updates.
  - A synthetic variant can be picked by bucket with no runtime scale.

### Sprint 10 - Minimal RoomTemplate + RoomBank Vertical Slice

- Owner: Codex implementation, Opus for data contract signoff, User for one authored test room.
- Estimate: 1-1.5 days.
- Dependencies: Sprint 9 RoomTemplate stub, existing DungeonMap/RoomFlow patterns.
- Deliverables:
  - Implement `RoomTemplateSO` V1 metadata + prefab reference, not the current 3-field placeholder only.
  - Implement `RoomBankSO` with room type lists, lazy refs or Addressable-ready references, deterministic random pick by seed, and validation report.
  - Implement save current authoring root as room prefab + metadata asset. Deterministic child naming is required.
  - Implement load RoomTemplate into a clean authoring scene/root.
  - Implement runtime test loader: pick one combat room, spawn player at socket, spawn one placeholder enemy, validate exit socket.
  - Add validation: exactly one player spawn, at least one exit/socket, camera bounds contains walkable bounds, no enemy spawn inside prop footprint, dependencies exist.
- Exit criteria:
  - One room can be painted/imported, saved, reloaded, loaded through RoomBank in PlayMode, and exited or marked exit-valid.
  - Save/load roundtrip test passes.
  - No direct runtime dependency on editor-only classes.

### Sprint 11 - Natural Placement Engine MVP

- Owner: Codex implementation, Opus/User for composition-role presets.
- Estimate: 1-1.5 days.
- Dependencies: Sprint 10 RoomTemplate save/load so output can be judged in the real loop.
- Deliverables:
  - Implement `CompositionRoleMap`: cleanCenter, decoratedEdges, focalZones, doorSafety, encounterAvoid, wallBand.
  - Implement deterministic Bridson Poisson sampler with bounded grid and tests for min distance + determinism.
  - Implement density mask sampling using existing FeatureMaskSO where possible.
  - Implement anti-repetition variant history per layer/pool for editor-baked passes.
  - Implement exact-count placement for Hero/Focal operations.
  - Add debug overlays: density heatmap, accepted points, rejected points, minDistance circles, role zones.
  - Do not implement Markov clustering.
- Exit criteria:
  - Auto-Dress can produce one test room with no obvious grid cadence, clean combat center, readable exits, and deterministic seed replay.
  - Debug overlay can explain why a candidate was accepted/rejected.
  - Tests cover determinism, min distance, and mask rejection.

### Sprint 12 - Props Mode MVP

- Owner: Codex implementation, User/Opus for prop categories and first prop pool.
- Estimate: 1-1.5 days.
- Dependencies: RoomTemplate sockets and prefab save path.
- Deliverables:
  - Add `PropDefinitionSO`: sprite/prefab, propCategory, footprint cells/polygon, pivot/sorting anchor, snap mode, collision mode, interactable type, combatBlocker, destroyable, lootSource, spawnAvoidRadius, biome tags.
  - Add Props tab to Brush window with prop palette, ghost preview, click-place, select, move, delete, duplicate, flip, rotate where allowed.
  - Props save into RoomTemplate prefab hierarchy with stable instance IDs and metadata component.
  - Add footprint preview and spawn/camera validation integration.
  - Eraser/select tools affect props only in Props Mode.
- Exit criteria:
  - User can place 3 prop categories in the test room, save/load them, and pass room validation.
  - Enemy/player spawn avoids prop footprints.
  - Sorting anchor behaves consistently with decorative layers.

### Sprint 13 - Production Hardening + Batch Gate

- Owner: Codex for tooling/tests, User for first real asset and room batch, Opus for final go/no-go.
- Estimate: 1 day initial hardening; more only if metrics fail.
- Dependencies: Sprints 9-12.
- Deliverables:
  - Performance smoke: 1 room with 200, 500, 1000 decorative sprites; measure editor save/load and runtime frame/render stats.
  - Undo stress: composite stroke with 100+ placements, undo/redo memory and correctness.
  - Dependency report: missing sprites, missing templates, broken GUID refs, missing sorting layers, non-native scale, invalid props.
  - Room thumbnail generation or at minimum screenshot capture for RoomBank browsing.
  - Naming/ID generator for rooms, variants, props, sockets.
  - First production batch: 5 combat, 1 shop, 1 shrine/rest, 1 elite, 1 boss placeholder.
- Exit criteria:
  - First 10-room library is playable through RoomBank.
  - Time per room is measured and documented.
  - Any remaining V2 items are explicitly cut, not silently half-built.

## 5. PRODUCTION GUARDRAILS

- V1 done means: one biome can produce/import brush atlases, paint L3-L6, save a room prefab+metadata, load it through RoomBank at runtime, spawn player/enemy safely, validate exits/camera/sorting/dependencies, and support basic props.
- V1 is not done when the editor window looks good. It is done when the room survives save/load, runtime load, one combat encounter, exit validation, and a dependency check.
- Scope cut trigger: any sprint exceeds 2 days without an end-to-end artifact. Cut advanced UI, AI tag suggestions, Markov, palette analyzers, SpriteAtlas, and non-horizontal L3 variants before cutting the vertical loop.
- Scope cut trigger: validator produces more than 20% false positive blocking errors on real masters. Convert art-quality checks to warnings and continue.
- Scope cut trigger: Auto-Dress requires more than half a day of parameter tuning for one acceptable room. Fall back to composition-role presets and manual paint; do not keep adding math.
- Scope cut trigger: Props Mode cannot save/load with stable IDs in one day. Cut rotation/multi-select first; keep place/delete/footprint/save.
- MVP exit gate: L3 horizontal import works, one combat room saves/loads, RoomBank runtime loads it, player/enemy/exit sockets validate, no runtime scale, no missing sorting layers, and first room can be iterated without manual Sprite Editor work.

## 6. RISKS NOT YET ADDRESSED

- Sorting layer mismatch is already present. `RimaSortingLayerValidator` guarantees `Patch` and `Scatter`, but current decorative executors emit `Patch`, `Detail`, and `Accent`. Add `Detail`, `Accent`, and Props/Entities ordering rules before production rooms are judged.
- Runtime scale violation is already present. `scaleRange` defaults to non-integer scaling and executors apply it. This will blur pixel art and invalidate the bucket strategy unless the new variant path bypasses or removes it.
- Undo stack memory cost is not theoretical. Composite strokes and Auto-Dress can create hundreds of GameObjects under one undo group. Add stress tests before batch production, not after.
- Asset GUID stability across team git is a real risk. Importer must update existing assets in place where possible, not delete/recreate AssetPoolSO or templates. Deterministic names and stable GUID preservation matter.
- EditMode procedural tests can pollute assets if they write under shared `Assets/Data/Brush`. New importer tests should use temp folders under `Assets/TempTests/...` and clean them with AssetDatabase, or create ScriptableObjects in memory where possible.
- RoomTemplate dependency on biome-specific SliceLayoutTemplateSO migration is unresolved. If a room references variants imported with template v1 and the template changes, validation must report template version drift and source atlas mismatch.
- L3 wall semantics are under-specified. Horizontal-only is the correct first gate, but door sockets, corners, vertical transitions, occlusion behavior, and sorting anchors will become blockers if data does not allow more semantic wall cases.
- Existing RoomTemplate is only layout/width/height. Treat it as a legacy placeholder, not the target architecture.
- Brush JSON schema exists at version 1. Variant metadata and RoomTemplate metadata need explicit schema versions and migration tests from day one.
- Prop and decorative object counts can produce hierarchy clutter. Save format should group by layer/type and use deterministic child names; otherwise prefab diffs become unreadable.

## 7. GO/NO-GO RECOMMENDATION

GO, but not with the original "Importer -> full Natural Engine -> Props -> Room Library" order.

Approved scope:
- Hybrid Auto-Slice.
- Importer/Validator/Preview.
- Native-size bucket variant path.
- Minimal RoomTemplate + RoomBank vertical slice before full Natural Engine.
- Natural Engine MVP based on composition roles, Poisson, density masks, anti-repeat, exact-count focal placement.
- Separate Props Mode MVP.

NO-GO if any of these are rejected:
- Runtime RoomBank vertical slice must be before full Natural Engine tuning.
- Non-integer runtime scale must be removed from the production brush path.
- Door/socket/player spawn/enemy spawn/camera bounds validation must be in V1 RoomTemplate.
- Validator must have severity levels.
- First production asset must be one L3 horizontal master, not a full batch.

Confidence:
- Architecture direction: 85%.
- Tight MVP in 5-7 days if scoped as above: 70%.
- Full Sprint 9-13 feature set in 5-7 days: 45%.
- Main mitigation: force the first vertical loop in the first 24-36 hours and cut every feature that does not help that loop.

No restart is needed. The harmonized architecture is mostly right. The required correction is sequencing and ruthlessness: prove one real room through runtime before investing in more naturalization math or large asset batches.

## 8. FIRST 24 HOURS TODO LIST

1. Update `STAGING/sprite_strategy_FINAL_PLAN.md` with this meta decision: Sprint 10 RoomTemplate/RoomBank moves before Natural Engine, Sprint 13 added, Markov/AI tag/SpriteAtlas deferred, composition roles promoted to Natural Engine primary model.
2. Write `STAGING/codex_brush_sprint9_atlas_importer.md` with exact file scope:
   - `Assets/Scripts/MapDesigner/Brush/Data/*`
   - `Assets/Scripts/MapDesigner/Brush/Import/Editor/*`
   - `Assets/Editor/MapDesigner/Brush/*`
   - `Assets/Tests/EditMode/Brush/*`
3. Dispatch Codex Sprint 9 as a bounded mechanical task: data model + importer + validator + preview skeleton + tests. Explicitly forbid RoomBank implementation inside that task except the contract stub.
4. In parallel, write the Sprint 10 spec for RoomTemplate/RoomBank vertical slice. Do not wait for Sprint 9 completion to design the room contract.
5. Patch plan documents to call out the current `scaleRange` production violation. The task must replace non-integer scale with native bucket selection in the new variant path.
6. Patch plan documents to add sorting-layer guard: `Patch`, `Detail`, `Accent`, `Props`, `Entities`, and UI/light interactions must be validated.
7. Prepare one synthetic 512x288 L3 horizontal PNG or the first real PixelLab master. The importer needs a deterministic test asset immediately.
8. Add first validation acceptance criteria:
   - import produces 2 variants from L3 horizontal template
   - AssetPoolSO keeps stable GUID on reimport
   - validator reports severity list
   - variant pick uses bucket weights
   - no production brush path applies non-integer scale
9. Write Sprint 10 runtime test target before code starts: "RoomBank picks Combat_Test_001, instantiates prefab, player spawn exists, one enemy spawn exists, exit socket exists, camera bounds exist."
10. Do not generate L3 vertical/corners/L4/L5/L6 batches until the horizontal master -> import -> paint -> save -> RoomBank -> runtime test loop passes once.

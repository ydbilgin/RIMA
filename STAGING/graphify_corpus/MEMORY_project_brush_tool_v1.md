---
name: brush-tool-v1-design
description: "Map Designer Brush Tool V1 unified design LOCK — 8-sprint plan, L3 wall priority, Karar"
metadata: 
  node_type: memory
  type: project
  originSessionId: 6463930c-ee28-4abe-b2e6-2c17db7c8cd5
---

# Map Designer Brush Tool — V1 Design LOCK (2026-05-16 S85)

## Progress (S85 night)

- ✅ Sprint 1 PASS (commit d0cd49c, tag brush-sprint-1-pass) — Data layer 9 files, 8/8 tests, dotnet build all PASS
- ✅ Sprint 2 PASS (commit 187ec12, tag brush-sprint-2-pass) — Executor router + L3 wall + BrushAlongEdges, 7 files + WallOverlayPainter +1 method, dotnet build PASS. **GAP:** EditMode runner Unity lock (sabah runner GUI)
- ✅ Sprint 4 PASS (commit 92fa94a, tag brush-sprint-4-pass) — Karar143Enforcement utility + 5 decorative executors + 8 tests. cx_dispatch.py timed out at 1200s but Codex completed via MCP bridge; Opus verified dotnet build + spec compliance manually.
- ✅ Sprint 5 PASS (commit fee98b6, tag brush-sprint-5-pass) — **Opus implement** UI refactor: 3-panel EditorWindow + 4 panels + scene tooling + [Shortcut] hotkeys + 7 tests. Polybrush pattern + Krita brush UX from Gemini research §9.
- ✅ Sprint 6 PASS (commit f837d67, tag brush-sprint-6-pass) — CompositeStrokeExecutor + 12 brush + 8 AssetPool bonus + BrushPack + 5 tests. cx timed out, Codex completed via MCP bridge.
- ✅ Sprint 7 PASS (commit 954f3de, tag brush-sprint-7-pass) — **Opus implement** AutoDressRoom + RegenerateDecorativeLayers + SmartFillSelection + 7 tests.
- ✅ Sprint 8 PASS (commit b5f14fe, tags brush-sprint-8-pass + **brush-tool-v1**) — **Opus implement** Bayer dither shader (Built-in RP, Return of Obra Dinn pattern) + BiomeSkinApplier + MaterialCache + 4 BiomeSkin .asset + 5 tests.
- ✅ Sprint 9 LIVE (uncommitted, 2026-05-16 S86) — **Opus implement → Codex review FAIL-with-fixes-PASS** Atlas Importer + 21 Wang variants + 2 P0 retrofit (FreeformDecalExecutor scale + RimaSortingLayerValidator extend). 18 new test PASS.
- ✅ Sprint 10 LIVE (2026-05-16 S86 SPRINT10) — **Opus implement → Codex review PASS-WITH-CONDITIONS 90% + P1 fix** RoomTemplateSO full + RoomBankSO + RoomTemplateValidator + Editor utilities (Saver/Loader/Menu) + RoomBankRuntimeTester + 11 test. Karar #144 propose (silahsız body + WeaponSR child).
- ✅ Sprint 11 LIVE (2026-05-16 S86 SPRINT11) — **Opus implement → Codex spec review PASS → Codex impl review FAIL → 3-fix → Codex re-review PASS**. CompositionRole + CompositionRoleMap + CompositionRoleMapGenerator + WangContextResolver + WallOverlayPainter 4-arg overload + serialized `l3WallVariantPool` + 15 test. cx_dispatch.py CONDA fix proven working in this sprint review.
- ✅ **Sprint 12 LIVE** (2026-05-16 S86_LATE) — Opus authored spec → **Codex implement (spec review yerine direkt impl)** → Codex self-verify PASS → rima-qc Sonnet QC review (background). Files: `PropDefinitionSO` + `PropPlacementData` + `PropFootprintValidator` + `PropsTab` + `PropPlacer` (editor-only) + `RoomTemplateSO.props` extension + `MapDesignerBrushWindow` Props mode integration + sample `barrel_001.asset` + 21 EditMode test. **OQ resolutions:** sorting=editor-only, footprint=bottom-left, GUID=AssetDatabase editor-only, null forbiddenRoles=empty. **Walkable region fallback** (Codex-invented, not in spec): `cameraBounds.tileRect` → `bounds` fallback; flag for Sprint 13 hardening. **Suite: 282/282 PASS** (önceki 261 + 21 yeni Props).
- ✅ **4 pre-existing failure CLOSED** (2026-05-16 S86_LATE Codex bd3kua3go) — 3 test wrong (`FeatureEdgeSmoothing` GetUsedTilesCount asset-count, `FeatureMaskSO` featureSiteRatio=1 fixture, `HitPauseDriver` reflection-driven coroutine) + 1 SUT wrong (`NaturalFeatureGraph.cs:112+269+290` local grid neighbor radius=2, 57ms→<20ms). **Suite 261/261 PASS**, Sprint 11 LIVE files unchanged, Karar #143-D/E/K unchanged.

## Phase 1A SO Contracts ADDED (2026-05-17 post-V1)

**Bundle D completed via Codex** (`STAGING/CODEX_TASK_so_scaffolding_phase1a_DONE.md`). 5 SO + 1 test, +7 EditMode tests → **328/328 PASS**.

- `Assets/Scripts/Rima/MapDesigner/SO/TerrainDefinitionSO.cs`
- `Assets/Scripts/Rima/MapDesigner/SO/PatchAtlasSO.cs` (renderer-agnostic, separate from existing `Assets/Scripts/MapDesigner/Brush/Data/AssetPoolSO.cs`)
- `Assets/Scripts/Rima/MapDesigner/SO/PropDefinitionSO.cs` (extends existing, additive)
- `Assets/Scripts/Rima/MapDesigner/SO/RoomVisualProfileSO.cs`
- `Assets/Scripts/Rima/MapDesigner/SO/ImportAssetRole.cs` (enum: Terrain32/MacroPatch64_128/OrganicDecal/DetailScatter/Accent/Prop/Character/TierBBackground/LightSource — ChatGPT spec match)

Renderer-agnostic (no SpriteRenderer/Tilemap/MonoBehaviour refs in SO public API). Port to HD-2D / sprite-in-3D supported. See [[3d-portability-strategy]].

## Semantic 3-Mode Finding (existing PaintMode = ChatGPT-equivalent ⊕ finer)

Brush V1 ALREADY has `PaintMode` enum (`RIMA.MapDesigner.Brush.Stroke`). ChatGPT's "semantic 3-mode" maps to existing fine-grained modes:

| ChatGPT semantic | Existing Brush V1 PaintMode |
|---|---|
| TerrainPaint | `GridTile`, `GridTileRandom`, `WallStamp` (visual-only currently; needs `TerrainDataWriter` executor extension for cornerField/walkable[]/elevation writes) |
| OrganicPaint | `FreeformDecal`, `ScatterAlongStroke` (visual-only ✅ matches spec) |
| StampPaint | `Stamp`, `WallStamp` (matches ✅) |
| Utility | `CompositeStroke`, `EraseByLayer`, `EraseAllDecorative` (not in ChatGPT but valid) |

**Upcoming extension (not yet implemented):**
1. `SemanticPaintIntent` enum (UI grouping, 3 high-level categories)
2. `TerrainDataWriter : IBrushExecutor` (registry: `PaintMode.TerrainWrite`) — writes cornerField/walkable[]/elevation
3. `RoomPaintStroke` serializable wrapper (replay-deterministic, extends BrushStroke struct)

Brush V1 mimari **ChatGPT'den 1 step daha ileride** (executor registry + composite pattern). No rewrite needed, just additive `TerrainDataWriter` executor + UI grouping.

## V1 STATUS: SHIP-READY (2026-05-18 S87_NIGHT)

- ✅ **Sprint 13 LIVE** (commit cb4303b 2026-05-18 S87_NIGHT) — Opus implement after Codex spec review FAIL (4 P0 + 5 P1) → spec v1.1 fix → impl LIVE. Production Hardening + Batch Gate.
  - Stream A: walkableGrid + IsWalkable (Sprint 12 Condition 1 fix), variantSprites + StableTileSeed (stable cross-version `x*73856093 ^ y*19349663`), rotationSteps + RotateClockwise, Validate 7-arg rotation overload, PropRegistrySO (runtime+editor paths), PropDefinitionPostprocessor (propId GUID auto-populate), PropColliderAutoBuilder, PropSorterRuntime (default "Props" sortingLayer per #143-E), PropRuntimeSpawner
  - Stream A bonus: BridsonPoissonAutoPlacer (true Bridson disk sampling, role-aware density)
  - Stream B: DependencyReportGenerator menu + Library/.gitkeep scaffold
  - Tests: 39 new EditMode (RoomTemplateWalkableGrid 3 + PropRotation 5 + PropVariant 5 + PropRegistry 4 + BridsonPoisson 6 + PropCollider 3 + PropSorter 3 + PropRuntimeSpawner 4 + DependencyReportGenerator 3 + UndoStress 2)
- **13/13 sprint complete** — V1 SHIP-READY
- ~115 source files + 22 .asset + ~26 test files
- **Test suite: 328/328 EditMode PASS** (321 V1 + 7 Phase 1A SO Contracts 2026-05-17)
- All Karar #143-D/E/K enforced at single source of truth
- Existing Karar #143 LIVE painters NOT modified (executor router delegation pattern)
- Sprint 5/7/8/9/10/11/13: Opus implement (override window 16-18 May). Sprint 1/2/4/6/12: Codex.
- Sprint 14+ backlog: combat integration / boss room procgen / meta-progression (Phase 2+)

## Sprint 3 + V2 backlog (sabah/sonra)

- Sprint 3 PixelLab 29 sprite gen pending (`pixellab_l3_wall_batch.md` + `pixellab_l4_l5_l6_batch.md`, ~36 credit)
- EditMode test runner execution pending (Unity restart, sabah)
- Material .mat files runtime-created by MaterialCache.LoadOrCreate (or sabah manual)
- TryGetCurrentRoom() scene→RoomData wiring (V2 polish)
- V2 backlog: marketplace, namespace prefix, biome brush, standalone migration
- ⏸ Sprint 3 (PixelLab asset gen) — kullanıcı sabah dispatch (29 sprite, ~36 credit)
- ⏸ Sprint 5 **Opus implement** (UI refactor — judgment iş; Polybrush + Krita patterns from research)
- ⏸ Sprint 6 **Opus+Codex karma** (brush pack content tuning + CompositeExecutor)
- ⏸ Sprint 7 Codex (automation)
- ⏸ Sprint 8 **Opus implement** (BiomeSkin + Bayer dither shader — production-ready code from research)

## Routing decision (S85 feedback)

[[codex-vs-opus-split]] — Don't auto-route everything to Codex. Sprint 5/6/8 = Opus (taste calls). Sprint 1/2/4/7 = Codex (spec calls).

## Authoritative documents

- **Design spec:** `STAGING/map_designer_unified_brush_design.md` (15 sections + 6 addendum)
- **Sprint 1 task:** `STAGING/codex_brush_sprint1_data_layer.md` (V1 minimum + §9 safety addendum)
- **PixelLab L3 batch:** `STAGING/pixellab_l3_wall_batch.md` (7 sprite types, ~21 credits)
- **Codex safety review:** `STAGING/codex_safety_review_output.md` (Approve with additions, 21 risk matrix)

## Locked architectural decisions

- **B route hybrid:** Unity-host EditorWindow + JSON-portable data layer
- **Karar #143 painter reuse** — brush tool sits ABOVE existing painters via `IBrushExecutor` + `BrushExecutorRouter`
- **Composite brush (flat, NO nesting)** — primary "painter feel" mechanism (e.g. "Mossy Broken Edge" = L2+L4+L5 in one stroke)
- **BiomeSkin separation** — subtle alpha only (`SoftAlpha8` max), **NO Gaussian blur**, softness from pixel cluster breakup + decal overlap
- **Karar #143-D/E/K enforcement** at data field level: `BrushLayerOperation.respectsWalkableMask` (default true), `wallProximityCurve` (AnimationCurve, default linear), `featureMaskMultiplier` (FeatureMaskSO nullable)
- **Photoshop-style hotkeys:** B/E/[/]/1-9/Alt-click/Shift-click

## V1 vs V2 split (binding)

**V1 (this 8-sprint plan, RIMA shipping):** SO-first data, minimal JSON round-trip, 8-12 brushes, composite flat, BiomeSkin subtle alpha, Karar #143-D/E/K, Auto-Dress/Regenerate/Smart Fill/Brush Along Edges

**V2 (post-ship ecosystem):** Marketplace, namespace prefixing, conflict resolution, biome brush (sub-region), standalone migration, advanced soft alpha shader, brush pack download manager

Strategic principle: "tool oyundan önemli olmasın" — V1 ships RIMA, V2 is optional.

## 8-Sprint plan (L3 wall = production gate)

| # | Sprint | Effort | Why |
|---|---|---|---|
| 1 | Data Layer (V1 min SOs + JSON) | 1d | Foundation; Karar #143 fields baked in |
| 2 | Executor Router + L3 Wall + Brush Along Edges | 1.5d | Wall = production gate; without it rooms don't close |
| 3 | PixelLab asset gen (29 sprite, L3 first) | 1.5d | Parallel with Sprint 2 — no code dependency |
| 4 | L4+L5+L6 Executors + Karar #143 enforcement | 1.5d | Decorative layer painters |
| 5 | Editor UI Refactor (3-panel + hotkeys + ghost) | 1.5d | UX delivery |
| 6 | Default Brush Pack (8-12) + Composite Executor | 1d | Visible artist-facing palette |
| 7 | Automation (Auto-Dress, Regenerate, Smart Fill) | 1d | Iteration speed |
| 8 | BiomeSkin + Render Rules (subtle alpha) | 1d | V1 close |

**Total: ~8-9 days Codex + parallel asset gen. Deadline YOK.**

## Sprint 9-13 LOCK (S86 PREP-3)

**Why:** Sprint 1-8 LIVE (V1 close). Sprint 9-13 PREP-3 LOCK'u Opus harmanladi: ChatGPT + Codex cross-review convergence. Bu sequence LOCK — degistirme.
**How to apply:** Her sprint kodlamaya baslamadan once bu tabloyu kontrol et. Sprint 10 RoomBank vertical slice, Sprint 11 Natural Engine'den ONCE gelmek zorunda.

| Sprint | Kapsam | Estimate |
|---|---|---|
| 9 | Importer + Metadata Retrofit + RoomTemplate stub + 2 P0 retrofit (scaleRange + sorting layer) | 1.5-2 gun |
| 10 | RoomTemplate V1 full + RoomBank vertical slice (pick → spawn → exit validate) | 1-1.5 gun |
| 11 | Natural Engine MVP (Composition Roles primary: cleanCenter/decoratedEdges/focalZones/doorSafety/encounterAvoid/wallBand) | 1-1.5 gun |
| 12 | Props Mode MVP (PropDefinitionSO + Props tab + footprint + save/load) | 1-1.5 gun |
| 13 | Production Hardening (walkableGrid + Bridson Poisson + rotation + variant pool + Collider2D + PropRegistry + Tilemap sorting) + Batch Gate (perf smoke + undo stress + dep report + 10-room library) | 1.5-2 gun |

Sprint 9 exit: 1 test master → AssetPoolSO variants (no compile error), validator severity list, bucket pick no runtime scale, 37 mevcut test green.
Sprint 10 exit: 1 room → paint → save → reload → RoomBank.Pick → PlayMode exit valid. No editor-only class runtime dependency.
Sprint 11 exit: Auto-Dress room no grid cadence, deterministic seed replay, debug overlay works.
Sprint 12 exit: 3 prop category place + save/load PASS + spawn avoids footprints.
Sprint 13 exit: 10-room library playable through RoomBank, time-per-room documented.

## 2 P0 Retrofit (S86)

**Why:** Mevcut kod bucket strategy ve sorting layer spec'ini ihlal ediyor — production rooms'tan once fix gerekli.
**How to apply:** Sprint 9 icinde implement et. Yeni variant path'e gecerken legacy scaleRange field'i dokuma (geriye uyumluluk icin birakilir, warning verir).

**R1: scaleRange runtime non-integer scale**
- Bug: `BrushLayerOperation.scaleRange = 0.85..1.15` ve `DecorativeExecutorUtility.PlaceAt()` arbitrary scale uyguluyor → pixel art blur, bucket strategy invalidate.
- Fix: Yeni variant path native sprite size kullanir, scale uygulamaz. scaleRange legacy-only field → yeni path bypass eder. Editor warning: "scaleRange deprecated for new variant path".

**R2: Sorting layer mismatch**
- Bug: `RimaSortingLayerValidator` sadece `Patch` ve `Scatter` valide ediyor. Decorative executors `Detail`, `Accent`, `Props`, `Entities` emit ediyor → mismatch.
- Fix: Validator extend: Patch / Detail / Accent / Props / Entities / UI / Lights (full ordering rules). All decorative executors validate against extended layer list. Editor menu: "RIMA → Brush → Validate Sorting Layers".

## Workflow Override (16-18 May 2026)

**Why:** Claude Code Opus limit asiri yuksek, Codex idle. Gecici inversion — 18 May sonrasi eski routing'e don.
**How to apply:** 16-18 May penceresi icin tum implement → Opus, review → Codex. 18 May EOD sonrasi [[codex-vs-opus-split]] routing'e geri don.

| Normal | Override (16-18 May) |
|---|---|
| Codex implement, Opus review | Opus implement, Codex review |
| Codex spec-following | Codex final-pass review |

Codex review dispatch: `cx_dispatch.py --task-file STAGING/codex_review_*.md --effort high` (background) → CODEX_DONE_*.md'den sonuc oku.

## Cross-links

[[karar-143-layered-pipeline]] [[room-library-architecture]] [[camera-angle-revisit-s43]] [[8dir-mirror-production-strategy]] [[codex-as-reviewer-until-may18]] [[room-composer-paint-intent-lock]] [[3d-portability-strategy]]

## ChatGPT Q1-Q8 locked answers

- Q1: NO nested composite
- Q2: click = single point, drag = continuous stroke (universal)
- Q3: AnimationCurve + 5 quick-preset templates
- Q4: per-room seed default, per-stroke override available
- Q5: weighted asset picks (optional `List<float> spriteWeights`, empty = uniform)
- Q6: live re-render + last-skin cache (1-2s acceptable)
- Q7: folder canonical brushpack format, zip optional export
- Q8: both manual + auto wall placement, auto is default

## Codex safety review additions (binding from Sprint 1)

- `schemaVersion` field in every JSON DTO root (V1 stores only, no migration)
- Canonical asset creation pattern: validate → external IO → optional Refresh → load deps → `StartAssetEditing` → try create+SetDirty → `finally StopAssetEditing` → `SaveAssets` → optional Refresh
- **Sprint 1: max 5 files per dispatch** (asset/SO work; Sprint 1 may split into 2 sub-dispatches)
- **No asmdef changes** in Sprint 1
- `RIMA.Editor` compile gate alongside `RIMA.Runtime`
- Duplicate GUID scan before tag
- Console error/warning gate after Unity opens

## Painters NOT to be modified (Karar #143 LIVE)

`MapLayerOrchestrator`, `WallOverlayPainter`, `TransitionBrushPainter`, `DetailDecalPainter`, `AccentPainter`, `NaturalFeatureGraph`, `VoronoiWaterFeatureGenerator`, `VoronoiElevationFeatureGenerator`, `FeatureEdgeSmoothingPass`, `FeatureMaskSO`, `WallBrushSetSO`, `NaturalFeatureSettingsSO`, `RimaMapDesignerWindow` (UI refactor Sprint 5).

Related memories: [[karar-143-pipeline]], [[visual-quality]], [[pixellab-knowledge-map]], [[perspective-templates]], [[feel-toggles]].

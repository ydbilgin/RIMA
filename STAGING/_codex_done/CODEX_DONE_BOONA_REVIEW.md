# Codex Independent Review — Boona Tweet -> RIMA Map Composition

Context caveat: the four MEMORY files named in the task were not present at their exact paths. This review uses `STAGING/boona_analysis_package.md` plus the current Blueprint implementation (`AutoPopulator`, `BlueprintZoneTypeSO`, `BlueprintProfileSO`, `RoomBlueprintSO`).

## H1-H8 verdicts

**H1 — Negative space mandate: ACCEPT.** RIMA v15c is failing first on silhouette and rest-space, not asset quality. A combat room needs intentionally plain cells where the player, enemies, and dash direction read instantly.

**H2 — Floor hierarchy lock: MODIFY.** The hierarchy is correct, but v15d should be stricter than 70/20/10 and should apply per painted zone/region rather than globally. Use one readable floor identity first, then secondary and accent as controlled exceptions.

**H3 — Prop clustering rule: ACCEPT.** Current per-cell density creates visual static. Clusters with explicit empty buffer cells will immediately read more natural and will make focal props feel authored instead of sprayed.

**H4 — Palette diet: MODIFY.** The goal is right, but "8-10 colors per room" is not enforceable in Unity assets without awkward color analysis. Enforce it as curated zone/pool selection and saturation discipline, with one accent family per room.

**H5 — Shader-based biome blending: REJECT.** This is not a v15d fix. It adds rendering and authoring risk while the current failure is composition: density, hierarchy, and placement rules.

**H6 — Path as primary composition: ACCEPT.** A main path gives visual flow and functional combat lanes. It should be painted or generated before prop placement, then protected from decals and blockers.

**H7 — POI capacities for prop placement: MODIFY.** Capacity is useful, but do not import Boona's crowd/POI model wholesale. For v15d, implement lightweight cluster anchors with radius/capacity constraints and reserve the full POI system for later.

**H8 — Skip path: pivot to Boona's "2 days HTML/JS" approach: ACCEPT.** Accept the rejection of the pivot. Unity stays; borrow composition principles, not the engine, pipeline, or scope posture.

## Proposed v15d numbers (concrete)

- Negative space %: 20% of painted room cells reserved as clean dominant base floor; acceptable range 18-22%.
- Dominant floor cell %: 75 / secondary %: 18 / accent %: 7.
- Hero prop cluster cap per room: 3 clusters; each cluster 3-5 props, with a 2-cell clean buffer around the cluster footprint.
- Palette cap (distinct colors per zone): 6 per zone, with a practical room target of 10 visible color families max.
- Path cells minimum: 80 cells for the current 36x22 combat room, or 10% of room grid area, whichever is larger.

## v15d code change scope (high-level, no implementation)

- `Assets/Scripts/Rima/MapDesigner/SO/BlueprintZoneTypeSO.cs`: extend existing zone fields with composition controls for clean-base reserve, dominant/secondary/accent floor weighting, path protection, cluster count, cluster radius, and cluster capacity.
- `Assets/Editor/MapDesigner/Blueprint/AutoPopulator.cs`: replace per-cell scatter for Layers 3-7 with a two-pass planner: reserve path/negative-space cells first, then place floor variation and prop clusters only in eligible cells.
- `Assets/Editor/MapDesigner/Blueprint/AutoPopulator.cs`: make tall focal placement share the same room-level cluster cap instead of only `maxTallFocalPerRegion`.
- `Assets/Data/Blueprint/Profiles/profile_combat_room_default.asset` and zone assets: lower Layer 3-5 densities, assign one dominant floor family, one secondary, one accent, and cap accent pools to one color family.
- `Assets/Editor/MapDesigner/Blueprint/RimaV15cSceneComposer.cs`: add metrics output for clean-base %, floor split %, path cells, cluster count, and layer prop totals so v15d can be judged from numbers before visual review.

## What you'd push BACK on vs my analysis

- Boona's 60% water negative space should not be translated directly to RIMA. A combat arena still needs readable terrain identity and tactical affordances; 20% clean cells is the right immediate target.
- Shader blending is overstated as a fix. Smooth edges would make a better screenshot, but they will not solve the current equal-weight floor noise or prop scatter.
- Palette count is a blunt metric. Value contrast and saturation control matter more than a literal distinct-color count, especially with sprites that include antialiasing and baked shading.
- POI capacities are useful inspiration, but the RIMA problem is not crowd goal selection. The near-term implementation should be cluster capacity, not a general POI framework.
- "Every room needs a main path" is good for the current combat room, but should become a room-archetype requirement, not a universal law for all future room types.

## Confidence

**H1: HIGH.** The package and current implementation both point to the same failure: every painted cell can receive multiple visual layers, so the player has no rest field.

**H2: HIGH.** `BlueprintZoneTypeSO` exposes many pools and densities but no floor-ratio contract, which explains why multiple floor identities can compete at equal weight.

**H3: HIGH.** `AutoPopulator.PopulatePoolLayer` rolls density independently per cell; that is exactly the mechanism that produces uniform scatter.

**H4: MEDIUM.** The visual diagnosis is sound, but hard numeric palette enforcement can become fake precision unless it is handled through curated pools and art direction.

**H5: HIGH.** A shader blend is a rendering feature, while v15d needs composition constraints. It should not enter the immediate fix.

**H6: HIGH.** Path-first composition directly supports combat readability and gives the auto-populator a concrete exclusion mask.

**H7: MEDIUM.** Capacity constraints are clearly useful, but the correct RIMA abstraction is still uncertain. Cluster anchors are the smallest useful version.

**H8: HIGH.** Boona's stack and timebox are irrelevant to RIMA's Unity combat pipeline; the transferable value is composition discipline.

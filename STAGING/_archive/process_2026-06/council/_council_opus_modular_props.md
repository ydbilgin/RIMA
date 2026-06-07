# COUNCIL — Opus Advisor: Modular Map Learnings (Prop-Groups + Path-Mask + Terraces) (2026-06-05)

> Lens: game-design judgment + VISUAL analysis (I can actually see the reference). I read the image, IsoRoomBuilder.cs, PropDefinitionSO, PropPlacementData, BridsonPoissonAutoPlacer, and the Composition layer. This changes my position from the brief's pre-analysis — see below.

---

## VISUAL ANALYSIS (the part only the seeing advisor can give)

What the reference actually shows, counted carefully:

- **Single dominant plane, edge terraces only.** The whole island is ONE walkable height. The "2-3 terrace" reading is mostly the **cliff skirt** (the stacked-block drop to the void) plus ONE genuine sunken step in the lower-right where the river sits a half-step below grass. There is NOT a true multi-level playfield with ramps. The terracing is 90% cosmetic edge depth — which RIMA's RoomCliffSolver ALREADY produces (directional SW/SE skirt). So the "terraces" impression is largely a thing RIMA already has.
- **Two-tone checker grass.** Strict alternating light/dark diamond per cell. This is a pure A/B tile pattern by `(x+y)%2`, not 16 random variants. RIMA's floor451 16-variant pool is actually the WRONG tool for this look — random variants give noise, not a crisp checker. Worth noting as a disagreement with brief item (c).
- **Prop GROUPS confirmed, low variant count.** The rock clusters are the key steal. I count roughly **4-5 distinct boulder-cluster silhouettes**, several clearly **mirrored** (the big central pile and the right-edge pile are near-mirror twins). Each cluster = boulder stack + 1 fallen/leaning dead log + sometimes a small rock skirt, authored as ONE unit and dropped at ~6-7 sites. Trees: ~3 silhouettes, mirrored freely. Bushes/flowers: 2-3 each, pure scatter. So the entire scene's richness comes from **~10-12 source art pieces** reused via mirror + group bundling. This is exactly the cheap-richness lesson and it is real.
- **Path = tile-type band, NOT geometry.** The brown plank/dirt strips are a flat overlay on grass cells; they cross under nothing and cast no depth. Confirmed it is a secondary tile/decal mask, geometry unchanged. Cheap.
- **River = 1-tile autotiled channel** carved INTO the plane with shoreline edge-tiles (the grass-to-water lip is a distinct edge sprite). This is a Wang/autotile band, and RIMA has `WangContextResolver` already.
- **Tent + smoke** = single hero prop with a looping particle. Pure decoration / future home-base flavor; not a map-gen technique.
- **Composition discipline (the BEST uncredited steal):** props hug edges and cluster, the CENTER stays clear for movement, and clusters are spaced (Poisson-like), never touching. This is precisely what RIMA's `CompositionRoleMap` (CleanCenter / DecoratedEdge / FocalCluster) + `BridsonPoissonAutoPlacer` already encode. The reference is a visual proof that RIMA's existing composition model is the right model.

**Bottom line of the visual:** the reference is NOT doing anything architecturally beyond what RIMA already has scaffolding for. Its magic is (1) prop GROUPING, (2) aggressive MIRROR reuse, (3) a flat PATH mask, (4) checker (not random) floor, (5) clean-center composition. Four of those five are content/authoring decisions, not new systems.

---

## ANSWERS

### Q1 — Prop-group prefabs: YES, but NOT a new SO. Use the existing auto-placer + a thin "cluster" placement.

The brief's pre-analysis proposes `PropGroupSO = member list + relative offsets + footprint union`. I **partially disagree** — that is more machinery than the value warrants, given what already exists.

RIMA already has: per-tile deterministic variant pick (`PickVariant`/`StableTileSeed`), `rotationSteps` + footprint swap, a Poisson auto-placer that clusters by `FocalCluster` role, and footprint validation. The reference's "rock+log cluster" is achievable TWO ways without a heavy new type:

- **LEAN (preferred) — author clusters as single multi-cell props.** A boulder-pile-with-log is ONE `PropDefinitionSO` with `footprintSize` 2x2/3x2, `worldSprite` = the whole baked cluster, `variantSprites[]` = 3-4 alternate cluster art pieces, mirrored copies included as their own variant entries. ZERO code change — `PickVariant` + footprint already handle it. The mirror is just a flipped sprite in the variant array (or a flipX flag, see below). This matches what the reference literally did: the clusters look baked, not assembled at runtime.
- **GROUP only if members must be individually destructible/interactive.** If a cluster needs the log to be a separate cover object vs the boulders being a wall, THEN a group is justified. RIMA has no such gameplay need today (props are static cover/blockers). So defer the relational group.

**My data-model verdict:** Do NOT add `PropGroupSO` now. Add ONE cheap field to `PropDefinitionSO`: `bool allowMirror` (or store mirror as additional `variantSprites` entries). Add `bool flipX` to `PropPlacementData` and apply it in `BuildProps` (`spriteRenderer.flipX = placement.flipX`). That single boolean buys you the reference's #1 richness multiplier (mirror reuse) at ~3 lines. Cluster-as-multicell-prop covers grouping. Cost: trivial.

If a true relational group is wanted in v2, the right shape is a lightweight `PropClusterSO { PropDefinitionSO[] members; Vector2Int[] localOffsets; }` resolved at author-time into individual `PropPlacementData` (bake-down, not runtime composite) so the runtime stays flat and Y-sort/collider code is untouched. But that is v2.

### Q2 — Path/decal mask layer: YES, as a SECOND tilemap layer. Worth it. Decals are the trap.

This is high value / low risk and I'd take it. The reference's paths and the river-shore are the single biggest "this looks designed" signal, and RIMA's rooms are prop-poor precisely because the FLOOR has no secondary read.

**Render verdict: separate tilemap layer, NOT free decal sprites.**
- A second `Tilemap` (e.g. `OverlayTilemap`, sorting just above `GroundTilemap`) painting a path-tile per cell is **iso-cell-locked by construction** — it uses the same `Grid`, so it CANNOT drift relative to the floor. This directly defeats the open tile-drift concern: snapping to the same grid is the cure.
- Free decal `SpriteRenderer`s at cell centers would reintroduce the exact 32px-rule vs iso-cell drift problem and Y-sort ambiguity. Reject decals for paths.
- Data model: add `byte[] overlayMask` (or `List<Vector2Int> pathCells` + an enum tile-type) to `RoomTemplateSO`, parallel to `walkableGrid`. `IsoRoomBuilder.BuildFloor` paints overlay tiles where the mask is set. The river-shore is the same mechanism with a Wang-context tile (you already have `WangContextResolver`).
- Scope guard: ship PATH first (single overlay tile-type). River/water as a fast-follow once path proves the layer. Cracks/rug are free reuse of the same layer afterward.

**Net:** one new tilemap + one mask array on the SO + a paint loop. No drift risk because it shares the grid. M-sized, mostly mechanical.

### Q3 — Multi-terrace (heightGrid): NOT NOW. v2 at earliest, and only if a mechanic demands it.

Strong NO for now. Three reasons, two of them are locked-rule collisions:

1. **Depth-sort lock conflict.** RIMA's depth sort is Camera Custom-Axis (0,1,0), single "Entities" layer, pivot-sort (locked rule). A true second walkable height breaks the assumption that screen-Y maps monotonically to depth — a character on an upper terrace near a lower-terrace prop will sort wrong. Fixing that means per-height sort offsets / sub-layers = touching the locked depth model. Not a casual add.
2. **Collision + cliff solver explosion.** `RoomCliffSolver` is built on a binary walkable/void mask with SW/SE front-edge rule. Heights introduce inner cliffs (terrace walls inside the playfield), ramp/stair connectivity, and per-height collision — the solver and the boundary/composite-collider code all assume one plane. This is an L, not an M.
3. **The reference doesn't even need it.** As established in the visual analysis, the reference's "terraces" are 90% cosmetic edge skirt — which RIMA ALREADY renders. The genuinely-sunken river step is achievable as a visual-only sunk tile + the path/overlay layer, with NO walkable height change. So you get the LOOK without the system.

**Verdict: never as a generic gen feature; v2-conditional only if a specific combat mechanic (high-ground, fall-off) is designed first.** Cosmetic depth is already solved by cliffs + a sunk-tile overlay.

### Q4 — Sequencing / packaging: SPLIT into a small standalone "props pass," do it BEFORE B-12 production-RoomBank.

Do NOT bury this inside B-12. B-12 is "pick/weight/pace 15+ rooms"; that work is far more valuable AFTER rooms can actually look decorated, otherwise you'll be weighting bare rooms and re-touching them all later. Order:

1. **(S) Mirror + cluster-as-multicell-prop.** `flipX` on placement + `allowMirror`/variant mirror entries + 3-4 baked cluster props authored. Code: ~10 lines in `BuildProps` + 1-2 SO fields. Unlocks the reference's #1 richness lever immediately. **Do first.**
2. **(M) Path/overlay tilemap layer.** Second tilemap + `overlayMask` on SO + paint loop + 1 path tile-type. Grid-locked, no drift. **Do second.** River-shore Wang as fast-follow under the same layer.
3. **(M) Checker floor + composition wiring for the 15 rooms.** Switch floor read to a crisp 2-tone checker (A/B by parity) instead of 16-random for the grass look, and actually RUN `BridsonPoissonAutoPlacer` over the 15 imported rooms with the existing role-map to auto-decorate them. This is the step that turns "prop-poor" into "designed-looking" using zero new systems.
4. **(L, separate, later) B-12 production RoomBank** with the now-decoratable rooms.
5. v2 backlog: relational `PropClusterSO`, multi-terrace (conditional), tent/home-base flavor.

Steps 1-3 are the whole "steal from the reference" payload and are all S/M. Multi-terrace stays out.

### Q5 — Other steals worth taking

- **Checker floor over random-variant floor (disagreement with brief item c).** The reference's clean look comes from STRICT A/B parity, not noise. RIMA's 16-variant random pool fights this. Recommend a checker mode for grass-type floors; keep random-variant for stone/dungeon where noise reads as wear. This is a near-free authoring/import decision, high visual payoff.
- **Aggressive mirror as a first-class authoring rule.** The reference gets ~2x apparent variety free via flipX. Bake this into the auto-placer (it already rolls rotation 0-3; add a 50% flipX roll) — one line in `BridsonPoissonAutoPlacer.Generate`. Cheapest variety multiplier available.
- **Clean-center composition as the production default.** The reference is a visual endorsement of RIMA's existing `CompositionRoleMap` (CleanCenter center, DecoratedEdge rims, FocalCluster corners). The system EXISTS and is barely used in the 15 rooms. The biggest single win here is operational, not code: actually run the auto-placer over the room bank.
- **Sunk-tile river/step for fake depth.** A single "lowered" overlay tile + shoreline edge gives the half-step terrace read with zero height-system cost. Take it as part of Q2's layer.
- **Hero prop + looping particle (tent/smoke).** Defer; this is home-base/META flavor, not run-room gen. Park it with the deck-meta items.

---

## DISAGREEMENTS WITH THE BRIEF PRE-ANALYSIS (explicit)
- **(a) Prop-group SO:** I'd NOT add a new SO. Mirror flag + cluster-as-multicell-prop covers it; relational group is v2-only. The pre-analysis over-builds here given existing infra.
- **(c) Checker via floor451 16-variant:** WRONG tool. 16-random = noise; the reference is strict A/B parity. Use a checker mode, not the random pool.
- **Heightmap→terrace pipeline (pre-analysis step 1):** the reference does not have a real multi-level playfield; its terraces are cosmetic edge skirt RIMA already produces. Don't build a heightmap quantizer.
- **What the pre-analysis UNDERrates:** RIMA already owns the composition role-map + Poisson scatter + footprint validation + deterministic variants. The single highest-ROI action is not new code at all — it is RUNNING the existing auto-placer over the prop-poor rooms. Lead with that.

## CONFLICTS WITH LOCKED RULES
- Multi-terrace (Q3) WOULD conflict with the Custom-Axis depth-sort lock and the cliff-solver single-plane assumption. Flagged; recommendation is to NOT do it. All other recommendations (mirror, path overlay layer sharing the Grid, checker floor, running the auto-placer) are consistent with locked rules and specifically chosen to avoid the iso-cell tile-drift problem by staying grid-snapped.

## ORCHESTRATOR NEXT STEP (suggested)
rima-doc to write a small TASARIM/props-pass spec from the synthesized council; then a 3-stage cx dispatch: (1) flipX placement + cluster props + auto-placer mirror roll [S]; (2) overlay tilemap + path mask on RoomTemplateSO [M]; (3) checker floor mode + run BridsonPoissonAutoPlacer across the 15 rooms [M]. B-12 RoomBank stays a separate, later task.

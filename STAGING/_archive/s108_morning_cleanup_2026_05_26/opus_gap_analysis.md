# OPUS GAP ANALYSIS — ChatGPT_ref vs Current Room System
**Date:** 2026-05-23 — Session S103 — Post Phase 4
**Scope:** What separates `STAGING/concepts/chatgpt_ref/` from `Assets/Screenshots/codex_phase3_*` and `codex_camera_lock_v1`. Diagnosis, prioritized roadmap, anti-patterns, open decisions.

> User's verbatim concern: "chatgpt_ref dosyasındakiler için ne lazım… çok fazla duvar yerine chatgpt ref gibi odalar istiyorum."
> Translation: too much wall, not enough room. **The diagnosis below confirms this hard. The wall system is overbuilt by ~3x relative to the rest of the room. Everything inside the wall perimeter is missing.**

---

## Section 1 — Reference Decomposition (per PNG)

| # | File | Frame Purpose | Decor Objects Visible | Light Sources (warm / cold) | Wall Surface Treatments | Floor Surface Treatments | Foreground Props / Centerpiece | Particle / VFX | Ambience Verdict — what makes THIS feel like the ref |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `16_12_46 (1).png` | Combat — open mid-room melee with 3 skeletons + caster, broken pillar field | ~22: 2 hanging banners (red), 6 wall torches, 4 broken pillar segments, 2 stone benches, scattered crates/sacks, sarcophagus lid, gold piles, blood pools, bone debris | ~10 warm torches + 4 cold cyan rift glows + 2 spell muzzle flashes = ~16 lights, 70/30 warm/cold | Banners on long walls, alcove with statue, runic carvings glowing cyan in cracks, chained pillar | Cracked flagstones with cyan rift lines snaking across floor, blood splatters, dust patches | Crescent-shaped magic slash arc + ritual circle in floor center-rear | Cyan rift sparks rising, dust motes around torches, damage number burst VFX | **The crack-lines of glowing cyan magic on the floor + warm torches on every wall create the dual-temperature lighting that defines the look.** Floor is never empty. |
| 2 | `16_12_47 (2).png` | Combat — VFX showcase, "MARKED → CHAINED DETONATION" skill demo | ~18: 2 ritual obelisks left/right of arena, broken column field, scattered sacks, sigils on floor, 2 banners, sarcophagus, bone piles, candle clusters | ~12 warm + 4 cold + intense red chain VFX = ~16 stable + huge spell glow burst, 50/50 warm/red split during the spell | Subtle runes on walls, arch breaches, niche statues | **Massive pentagram rune circle** glowing red across center 1/3 of room + cyan crack lines + scorch marks | Player at right with crescent slash, ritual circle dominates floor | Red lightning chain links between marked enemies, red sparks, dust haze | **Floor is the actor.** The pentagram + rune circle + chain-VFX dominate. Walls are background. |
| 3 | `16_12_47 (3).png` | UI — Planner / Run-loop screen | N/A (UI overlay) | Background room glow only | N/A | N/A | Class arsenal grid + loadout slot card + route map | Subtle background blur of dungeon | Mostly UI — but the dungeon BG behind is dark + dense + parallaxed, never flat |
| 4 | `16_12_48 (4).png` | Combat — entryway / gateway corridor with rift breach approach | ~20: **giant gate portcullis upstage**, 2 huge red banners flanking gate, 4 broken obelisks in floor field, 6 wall torches, sarcophagi, urns, scattered debris | ~10 warm + 6 cold = 16 lights, 60/40 | Gate arch entirely brick-and-iron, banners flanking, sconce alcoves | Cyan rift breach lines fanning out toward gate, broken floor tiles | **Portcullis gate is the focal point** + 4 ritual obelisks forming a cross pattern | Cyan magic mist rising from rift breach, torch flicker glow | **The gate** as a setpiece anchors the entire frame. Eye is pulled upstage by lights + banners + gate composition. Cyan breach radiates outward from gate. |
| 5 | `16_12_48 (5).png` | Ritual Chamber — round dais centerpiece | ~15: **massive central rift crystal obelisk on circular dais**, 4 surrounding rune pillars, 2 broken alcove statues, 4 wall torches, scattered debris around dais perimeter | ~6 warm torches + 8 cold (giant crystal + 4 satellite rune pillars + rift cracks) = 14 lights, 30/70 cold dominant | Vaulted backwall with carved niches, glowing cyan runes on the wall stones | Concentric circular dais (3 stone rings) with rune symbols, radial cyan lines | **The crystal obelisk** — entire room composes around it | Cyan particles rising from crystal, runes pulsing, cold haze across floor | **One iconic centerpiece** + radial composition. The room is a stage for ONE object. This is the cleanest "setpiece room" archetype. |
| 6 | `16_12_48 (6).png` | Combat — library / archive room with debris floor | ~30 (densest frame): **8-10 toppled bookcases lining walls**, 2 reading tables, scattered books/papers on floor, broken chairs, candle holders, 4 wall torches, sarcophagus, 2 small skeletal piles | ~10 warm + 3 cold + table candles ~4 = ~17 lights | Bookcases as wall-attached props (not just wall texture), niches with scrolls | **Floor covered in scattered books, papers, broken furniture** — almost no exposed floor tile | Reading table center-left, toppled bookcase row defines whole bottom edge | Torch glow, dust on torches | **Floor coverage** is the key. ~70% of floor has SOMETHING on it. The room feels USED, lived-in, ransacked. |
| 7 | `16_12_48 (7).png` | Combat — armory / barracks room with weapon racks | ~25: **wall-mounted weapon racks** with swords/spears, training dummy on stand, anvil, 2 weapon crates, candle stands, banners, 4 torches, scattered armor pieces on floor, broken weapon shafts, sarcophagus | ~8 warm + 2 cold + candle clusters ~5 = ~15 lights, 80/20 warm dominant | Weapon racks ARE wall decor (long horizontal rack with hanging axes/swords), banners | Weapon shafts, broken shields, dropped armor — medium floor coverage | Training dummy + anvil = dual focal points | Torch glow + candle flicker | **Themed room.** Every prop says "armory." This is theme-driven decor — not random clutter. Rooms have IDENTITY via prop selection. |
| 8 | `16_12_49 (8).png` | Combat — Flooded Crypt with sarcophagi rows | ~20: **6 sarcophagi in 2 rows**, 4 broken sarcophagus lids on floor, 4 wall torches, 2 rune obelisks, scattered bones, debris piles, gravestones in alcoves | ~6 warm + 8 cold (water reflections + rift cracks) = 14 lights, 40/60 cold | Wall alcoves containing standing skeletons/statues, water staining | **Wet floor reflecting torches**, cyan crack lines, puddles | Two parallel rows of sarcophagi form a processional center aisle | Cyan rising mist from sarcophagi, water reflection shimmer | **Wet floor reflection** + processional layout. Reflective floor doubles perceived light count. |

### Cross-frame statistics (averaged across 7 combat/room frames)

| Metric | Reference Avg | RIMA_HD2D Current |
|---|---|---|
| Decor objects per frame | **~21** | **0** |
| Distinct light sources | **~15** (mix warm + cold) | **~6–8** (warm wall niches + 2 cold floor lights) |
| Themed props (banners, racks, books, sarcophagi, gates) | **~5 per frame** | **0** |
| Floor-surface treatments (sigils, cracks, decals, puddles, scatter) | **~6 distinct treatments** | **1 sigil tile prefab, unused in active scenes** |
| Particle / VFX layers | **3–5 layered** (rift mist, dust, torch glow, magic) | **0** |
| Centerpiece / setpiece per room | **1–2** (gate, crystal, dais, anvil) | **0** |
| Wall variant count | 3–4 visible variants per frame | **6 prefabs across 4 libraries** — **OVERSUPPLIED** |

**Headline:** Walls are ~20% of what makes a ref frame feel like the ref. We've built ~80% of the wall system and ~0% of the other 80%.

---

## Section 2 — Gap Diagnosis

| # | Visual Element | Ref Presence | RIMA_HD2D Current State | Gap Severity | Root Cause |
|---|---|---|---|---|---|
| 1 | **Floor decor / scatter** (books, bones, debris, weapons on floor) | HIGH — covers 30-70% of floor area | NONE — bare floor tiles only | **CRITICAL** | No prop library, no scatter system, no PixelLab decor sprites yet |
| 2 | **Floor surface treatments** (rune circles, sigils, cracks, blood, puddles) | HIGH — every frame has 2-4 distinct treatments | 1 sigil prefab exists, not placed in active scenes | **CRITICAL** | Sigil prefab exists but no spawn rule in `RoomShellBuilder`; no decal/material variant system for floor |
| 3 | **Themed setpieces / centerpieces** (gate, crystal obelisk, anvil, dais, sarcophagi rows) | HIGH — 1-2 anchor per room | NONE | **CRITICAL** | No room-archetype concept yet; footprints define SHAPE only, not THEME |
| 4 | **Wall-attached props** (banners, weapon racks, bookcases, hanging chains) | HIGH — ~5 per frame | NONE — walls are just wall sprites | **HIGH** | Wall library system attaches NICHES (cosmetic torch holders) but no prop-attachment slots |
| 5 | **Particle / atmospheric VFX** (rift mist, dust motes, torch glow halo, magic sparks) | MEDIUM-HIGH — 3-5 layers | NONE | **HIGH** | No particle system pass yet; URP Volume bloom does NOT substitute for actual particles |
| 6 | **Light source density** (warm + cold dual-temp lighting) | HIGH — ~15 per frame, mixed warm/cold | 6-8 lights, mostly warm wall-niche torches | **MEDIUM** | Lights spawn only at wall niches; no floor torch / candle / brazier props; no rift-crack floor light prefab placed in arenas |
| 7 | **Wall variant count** | MEDIUM — 3-4 visible variants per frame | **6 wall variants across 4 libraries (Standard/Damaged/Ritual/Rift) + niches/breach/toppled/heavy** | **OVER-INVESTED** | Phase 1-3 over-prioritized wall sprite variation; diminishing returns past 3 variants |
| 8 | **Floor tile variation** (cracked tiles, scorch marks, water stains, broken flagstones) | MEDIUM — every frame has visible wear pattern | 1 base floor + 1 sigil tile prefab | **HIGH** | Floor material is single-texture; no decal variant or wear pass |
| 9 | **Composition focal-point** (eye drawn upstage by gate/dais/lights) | HIGH — every frame has clear focal anchor | Random — rooms are flat-density boxes | **MEDIUM** | No composition rules in builder; footprints are uniform-density |
| 10 | **Mob density + composition** | MEDIUM — 4-7 per frame, varied silhouettes | 4 mobs in test scene, identical silhouettes | **LOW** (not blocker for "room feel") | Out of scope for room-aesthetic phase; defer |
| 11 | **HUD / UI overlay** (RIMA panel, minimap, hotbar) | Frames 1,2,4-8 all show full HUD | None implemented | **LOW** (not blocker for "room feel" — separate UI phase) | Defer until rooms ship |
| 12 | **Camera atmosphere** (bloom, vignette, dark contrast) | Strong | URP Volume profile in place, working as visible in `codex_phase3_atmosphere_v1.png` | **SOLVED** | Phase 3 work already closed this gap. Don't touch. |
| 13 | **Color temperature contrast** (warm + cold dual palette) | Strong — orange torches against cyan rift = the SIGNATURE of the look | Warm torches in place; cold rift lights placed in some scenes | **PARTIAL** — direction is right, count is low | Need ~3x more cold light sources (rift cracks on floor, glowing wall runes) to match ref ratio |
| 14 | **Floor wetness / reflection** (frame 8) | Themed rooms only | None | **LOW** — single-room archetype, defer | Themed; only needed for Crypt sub-archetype |
| 15 | **Wall surface details** (runic carvings glowing cyan, stained patches, alcove statues) | MEDIUM — adds richness | None — walls are flat-textured | **MEDIUM** | Wall variant sprites exist but lack rune-glow / stained / statue-alcove sub-variants |

### Brutal honesty
- **Wall system is overbuilt by a factor of ~3.** We have 6 wall prefabs × 4 libraries = 24 effective wall configurations, and the player won't perceive more than 3 of them in a single frame.
- **Atmosphere lighting is already close.** Phase 3 bloom/vignette/torch+rift light combo gets us 60-70% of the way to ref lighting MOOD. The remaining gap is light COUNT (more sources needed), not light QUALITY (URP setup is fine).
- **The single highest-impact missing element is floor content.** Floor is 50-60% of the screen real estate in HD-2D top-down. Empty floor = empty room, no matter how nice the walls are.
- **The second highest-impact missing element is room IDENTITY** (gate room, library room, ritual chamber, crypt). Rooms need ARCHETYPES not just FOOTPRINTS.

---

## Section 3 — Prioritized Roadmap (next 3 phases)

### PHASE 5 — Floor Population Pass [START HERE]
**One-paragraph description:** Stop building walls. Fill the floor. Generate a PixelLab decor sprite library (debris piles, bones, broken pillars, urns, books, weapons, sacks, blood pools, dust patches) at HD-2D-compatible 64-128px scale, register them as `FloorDecor` ScriptableObjects, and extend `RoomShellBuilder` with a `FloorScatterRule` pass that places 8-15 decor billboards per room based on density + archetype config. ALSO add 3 floor-tile material variants (cracked, scorched, sigil-etched) and a `FloorDecal` ScriptableObject system that places 2-4 floor decals (rune circles, blood splatter, crack lines) per room.

**Effort:** **L** (1 PixelLab batch of ~12 sprites + ScriptableObject schema + builder extension + decal shader/material setup)

**Visual impact:** **+40% toward ref aesthetic** — closes the single largest gap

**Hard dependencies:**
- PixelLab batch for decor sprites (~12 sprites, generate in one job)
- Decision on billboard-vs-quad-vs-mesh rendering for decor (recommend: quad sprite with `Sprite Renderer` rotated to face camera lock, since camera is fixed → no true billboard needed)
- Decision on decal rendering (recommend: URP Decal projector for floor-decals; or pre-baked into floor tile material variants)

**Deliverable list (max 15):**
1. PixelLab job: 12 decor sprites (debris_pile_small/large, bone_pile, urn_intact/broken, book_scatter, weapon_shaft, sack, candle_stand, blood_pool, dust_patch, broken_pillar_segment) — 64-128px, top-down 35° camera-matched
2. 12 `DecorProp` prefabs (sprite renderer + tiny collider + footprint hint)
3. `FloorDecor` ScriptableObject + `FloorDecorLibrary` SO (mix percentages per category)
4. `FloorScatterRule` component on `RoomFootprint` (density 0-1, archetype filter)
5. `RoomShellBuilder` extension: `PopulateFloorScatter()` pass after wall/floor build
6. 3 PixelLab/gpt-image-1 floor decal textures (rune_circle_red, crack_lines_cyan, scorch_burst)
7. 3 URP Decal projector prefabs OR 3 floor-decal-material variants
8. `RoomShellBuilder` extension: `PlaceFloorDecals()` pass — 2-4 decals per room
9. Floor tile material variants: `Floor_Cracked`, `Floor_Scorched`, `Floor_Wet` (gpt-image-1 textures, tileable)
10. Update `BreachedArena` footprint to spec a high-density scatter (~15 props) for SampleScene
11. Screenshot showcase: SampleScene before/after Phase 5 with all 4 libraries
12. Commit message format: `[Codex] [S104 FLOOR POPULATION] decor library + scatter rule + decals`

**Risk:**
- PixelLab top-down 35° camera angle match — sprites must look right under our locked camera. **Mitigation:** generate 2-3 test sprites first, validate, then batch the rest.
- Decal projector performance on URP — typically fine but cap at 4 per room.
- Decor sprites may not stylistically match wall sprite palette. **Mitigation:** include 1-2 wall sprites in PixelLab reference prompt for style anchor.

---

### PHASE 6 — Room Archetypes + Setpieces
**One-paragraph description:** Stop treating all rooms as the same. Introduce a `RoomArchetype` enum (Combat / Ritual / Library / Armory / Crypt / Gate) that drives wall library choice, scatter density profile, AND a single dominant setpiece prefab. Build 4 PixelLab/Codex setpiece prefabs: Ritual Crystal Obelisk on Dais (frame 5 inspiration), Iron Portcullis Gate (frame 4), Sarcophagus Row (frame 8), Anvil + Weapon Rack cluster (frame 7). Each archetype's footprint reserves a specific tile zone for the setpiece (center, upstage center, processional aisle, corner). This is where rooms get IDENTITY.

**Effort:** **L** (4 PixelLab setpiece sprites + 1 dais 3D mesh OR sprite stack + archetype enum + footprint extension + composition logic)

**Visual impact:** **+25% toward ref aesthetic** — gives every room a focal point

**Hard dependencies:**
- Phase 5 must ship first (setpieces sit on floor with scatter, not on empty floor)
- Decision on setpiece rendering: pure sprite (cheap, flat) vs HD-2D hybrid 3D mesh + sprite overlay (matches Karar #100 chibi + 3D environment) → **recommend hybrid: small 3D dais/gate mesh + sprite decor on top**
- PixelLab setpiece sprites must support HD-2D camera angle

**Deliverable list (max 15):**
1. `RoomArchetype` enum + `RoomArchetypeConfig` SO (wall lib + scatter density + setpiece prefab + light count target)
2. 4 setpieces (PixelLab + simple ProBuilder mesh hybrid): RitualCrystalDais, GatePortcullis, SarcophagusRow, AnvilWeaponRack
3. Footprint extension: `setpieceZone` (Vector2Int rect within footprint) reserved tiles
4. `RoomShellBuilder` extension: `PlaceSetpiece()` pass
5. 5 archetype configs: Combat (no setpiece, high scatter), Ritual (crystal, low scatter), Library (no setpiece, dense book scatter, special book-pile decor variant), Crypt (sarcophagus row, mid scatter), Gate (portcullis, mid scatter, banners on flanking walls)
6. Update 5 footprints with archetype tag (BigArena→Combat, BreachedArena→Combat-rift, OffsetRect→Ritual, T→Gate, Asymmetric→Crypt)
7. Wall-attached props library: 4 props (banner_long, weapon_rack_horizontal, chain_hanging, alcove_statue) — PixelLab batch
8. Wall library extension: `WallProp` slot — wall variant prefabs reserve attachment points
9. Screenshot showcase: 5 archetype rooms side-by-side
10. Commit: `[Codex] [S105 ARCHETYPES + SETPIECES]`

**Risk:**
- HD-2D hybrid setpiece rendering (sprite + 3D mesh) needs prototyping — could look bad if scale/perspective mismatch. **Mitigation:** prototype ONE setpiece (RitualCrystalDais) first, validate visual, then build remaining 3.
- Wall-prop attachment slots could conflict with existing wall niche slots. **Mitigation:** define `WallSlot` enum (NicheTorch / PropBanner / PropRack) with mutual exclusion rules.

---

### PHASE 7 — Lighting Density + Particle Atmosphere
**One-paragraph description:** Bring light source count from ~7 to ~14 per room by adding free-standing floor lights (brazier, candle cluster, rift-crack floor light) and wall-attached candle props. Add 4 particle systems as room-prefab additions: rift mist (cold cyan, low altitude), dust motes (warm specks, drifting upward), torch glow halo (additive sprite billboard on torch lights), magic spark drift (cyan sparks rising from rift-tagged tiles). Density driven by `RoomArchetype` (Ritual room = dense cyan mist; Combat room = light dust + torch halos).

**Effort:** **M** (3-4 floor-light prefabs + 4 particle systems + integration pass)

**Visual impact:** **+15% toward ref aesthetic** — the final polish layer that makes it FEEL alive

**Hard dependencies:**
- Phase 5 + 6 must ship first (lights and particles populate the world the previous phases built)
- Decision on particle perf budget (recommend: ~50 particles max per system, 4 systems per room = 200 particles, well within URP budget)

**Deliverable list (max 15):**
1. 3 floor-light prefabs: Brazier (3D mesh + warm light + fire sprite + small particle), CandleCluster (small mesh + tiny warm light), RiftCrackFloor (decal + cyan light + cyan particle)
2. 1 wall-candle prop prefab
3. 4 particle prefabs: RiftMist_Cold, DustMotes_Warm, TorchHalo_Additive, MagicSpark_Cyan
4. `LightPlacementRule` extension on `RoomArchetype` (target light count + warm/cold ratio)
5. `RoomShellBuilder` extension: `PlaceFloorLights()` and `PlaceParticles()` passes
6. Update RitualCrystalDais setpiece to spawn cyan mist + magic sparks around base
7. Update GatePortcullis setpiece to spawn rift crack lights radiating outward
8. Update all 5 archetype configs with light/particle density
9. Showcase: 4-screenshot grid — same Combat room with Phase 4/5/6/7 progressive overlay
10. Final atmospheric comparison vs `chatgpt_ref/16_12_46 (1).png` side-by-side
11. Commit: `[Codex] [S106 LIGHTING + PARTICLES]`

**Risk:**
- Particle systems can tank perf on integrated GPUs. **Mitigation:** profile early, set hard cap, use sprite-based particles not mesh.
- Over-bright bloom from extra lights could blow out the ref look. **Mitigation:** retune URP Volume after Phase 7 if needed.

---

### Phase 4+ — Not addressed in this roadmap
Floor reflection (frame 8 wet crypt), full HUD overlay, mob silhouette variety, weather/environmental hazards. These are NOT blockers for closing the "rooms feel like ref" gap and should wait until Phases 5-7 ship.

**Composite expected outcome after Phases 5-7:** ~80% visual match to ref. Remaining 20% is mob art quality + HUD + spell VFX, which are separate disciplines.

---

## Section 4 — Anti-Patterns to STOP

1. **STOP adding wall variant prefabs.** 6 wall prefabs across 4 libraries is already past the point of diminishing returns. Any new wall work must instead add SLOTS for wall-attached props (banners, racks) — not new wall sprite variants.
2. **STOP treating all rooms as equal-density boxes.** Rooms in the ref have HOT zones (setpiece + dense scatter) and COLD zones (empty pathways). Uniform density = boring composition.
3. **STOP using gpt-image-1 for props.** It's fine for tileable wall/floor textures but wrong tool for billboard decor sprites — use PixelLab (per Karar #100 asset pipeline).
4. **STOP iterating in SampleScene with placeholder prefabs.** Every visual phase from Phase 5 onward needs a FRESH showcase scene with side-by-side archetype comparison, not endless SampleScene revisions.
5. **STOP centering the wall system in our visual mental model.** Walls are the FRAME. The PAINTING is floor + decor + setpiece + lighting. Frame work is done — start painting.
6. **STOP one-off lighting tweaks.** Lighting density is an archetype-driven config now, not a per-scene manual placement.
7. **STOP doing perf-blind particle/decal additions.** Profile early (Phase 5), set caps, then build.

---

## Section 5 — Open Decisions for User

1. **Decor rendering: pure 2D sprite billboards vs HD-2D hybrid (sprite + tiny 3D mesh base)?**
   - Option A: Pure sprite billboards (cheap, fast, single PixelLab pass, ~12 sprites Phase 5).
   - Option B: HD-2D hybrid — sprite for the prop, small 3D base mesh under it (matches Karar #100 hybrid principle, ~2x effort).
   - **Opus lean:** Option A for scatter decor (urns, bones, books); Option B for setpieces (dais, gate, anvil). Need user confirmation.

2. **Room archetype count for Act 1 (Shattered Keep)?**
   - Option A: 3 archetypes (Combat / Ritual / Crypt) — minimum viable variety.
   - Option B: 5 archetypes (Combat / Ritual / Library / Armory / Gate) — matches ref breadth.
   - Option C: 6+ — over-investment.
   - **Opus lean:** Option B (5). Anything less feels samey by run 3.

3. **PixelLab decor sprite batch size for Phase 5?**
   - Option A: 12 sprites (one batch, ~$X credits).
   - Option B: 20 sprites (two batches, more variety per archetype).
   - **Opus lean:** Option A first, then evaluate if archetype population feels thin.

4. **Floor decal system: URP Decal projector or pre-baked floor-tile material variants?**
   - Option A: URP Decal projector — flexible, can layer multiple decals per tile, ~2-decal perf cost per tile.
   - Option B: Pre-baked floor-tile material variants — cheaper at runtime, less flexible, requires more tile-art combinations.
   - **Opus lean:** Option A — flexibility wins for a roguelite where rooms procedurally compose.

5. **Setpiece-driven footprints: do setpieces dictate footprint shape, or do footprints reserve setpiece zones?**
   - Option A: Footprint defines shape, archetype layer reserves N tiles for setpiece (current MASTER_KARAR-compatible approach).
   - Option B: Setpiece-first footprints — each archetype gets bespoke footprints designed around setpiece (more work, better composition).
   - **Opus lean:** Option A for Phase 6, revisit Option B in Phase 8+ if rooms still feel un-composed.

---

## CONFLICTS WITH LOCKED RULES?
- **NONE.** All proposals honor: HD-2D Hybrid (Karar #100) — sprites + 3D environment + 2D textures on 3D meshes. Camera lock + irregular footprints preserved. PixelLab as primary sprite tool preserved. gpt-image-1 reserved for tileable textures.
- Caveat: Phase 6 setpiece hybrid (sprite + small 3D mesh base) reinforces Karar #100 but should be explicitly noted to rima-doc when archetype system ships.

## ORCHESTRATOR NEXT STEP
Dispatch **Phase 5 (Floor Population)** to Codex first. Single highest-impact unblocker. Expected to consume 1 Codex session + 1 PixelLab batch. Phase 6 and 7 chain after.

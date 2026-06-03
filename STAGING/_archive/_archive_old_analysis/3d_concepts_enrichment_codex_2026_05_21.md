# 3D Concepts Enrichment - Codex Pressure Test

Date: 2026-05-21
Task source: CODEX_TASK_laurethgame.md
Output: STAGING/3d_concepts_enrichment_codex_2026_05_21.md

Note: Referenced source file `STAGING/3d_simple_strong_concepts_2026_05_21.md` was not present in this checkout. This review uses the full concept excerpts embedded in `CODEX_TASK_laurethgame.md`.

---

## Executive Verdict

The Multi-Medium Synergy Lens is sound, but only if it is treated as a single field-simulation design language, not as a license to add separate stealth/action/survival subsystems. The lens is strongest for ECHOLARM, HEAVE, BALLAST, and PIVOT because their cores already imply spatial fields, propagation, material state, or force transfer. It is usable but riskier for SNAP and UZUV because their core pleasures can become UI/AI bookkeeping before they become embodied 3D play.

Best studio rule: one shared substrate, many readable channels.

- Shared substrate: grid/navmesh cells plus simple volumes carrying scalar/vector fields.
- Channels: sound, light, heat, airflow, fluid, weight, electricity, scent.
- Readability constraint: never expose more than 2 active channels as player-critical at once in early tiers.
- Enemy asymmetry: use as encounter composition, not as hidden rules. Each enemy type needs an obvious silhouette, sensor cone/range, and feedback color.

If this becomes a STUDIO_KARAR, it should be phrased as a constraint:

> Add depth by reusing one simulation primitive across multiple perceptual media. Do not add a new medium unless it either modulates an existing medium or creates a new enemy-readable decision.

---

## 1. Lens Validation

### What Works

The lens is mechanically strong because it gives compound return:

- A single propagation solver can power sound, scent, smoke, heat, gas, and alarm fields.
- A single occlusion/visibility model can power light, line of sight, thermal masking, and shadow tactics.
- A single material table can power weight, buoyancy, conductivity, flammability, slipperiness, and noise.
- A single wind/flow vector field can move sound, scent, smoke, gas, embers, projectiles, and creature cues.

This directly matches the studio constraints: asset-light, animation-light, systemic, and visually simple if fields are shown with restrained overlays.

### Where It Breaks

The lens breaks in three cases:

1. Medium count outruns player parsing.
   If sound, light, heat, scent, airflow, electricity, fluid, and weight are all active in the same encounter, the player stops making plans and starts guessing.

2. Mediums do not share one simulation primitive.
   If sound uses one bespoke system, light another, heat another, and scent another, development cost rises faster than depth.

3. Asymmetric perception is hidden.
   If the player cannot tell which enemy reads which channel before acting, the system feels unfair rather than deep.

### Concept Fit Ranking

| Rank | Concept | Lens Fit | Reason |
|---|---|---:|---|
| 1 | ECHOLARM | Very high | Core is already perceptual fields and stealth routing. |
| 2 | HEAVE | Very high | Terrain/material state naturally supports cross-modulation. |
| 3 | BALLAST | High | Weight/material/weather fields are core survival decisions. |
| 4 | PIVOT | High | Gravity, momentum, fluids, and magnetism can share force rules. |
| 5 | UZUV | Medium-high | Good if sensors/actuators remain creature-readable; risky if too much automation logic. |
| 6 | SNAP | Medium | Needs UI-heavy state clarity; field propagation can fight the deckbuilder/room clarity. |

---

## 2. Layer Cost Classification

Cost assumptions: Unity URP, solo dev, prototype-to-shippable vertical slice, simple 3D geometry, stylized low-detail VFX, no heavy bespoke physics solver unless stated.

### PIVOT - Gravity Roguelite

| Layer | Cost | Why |
|---|---|---|
| Momentum carry / pivot slingshot | UCUZ | Mostly character controller velocity preservation and input timing. |
| Gravity direction switching for actors/objects | ORTA | Needs robust controller, camera, enemy pathing exceptions. |
| Gravity-aware fluids/sand | PAHALI | Real fluid is expensive; fake cell-flow is ORTA but limited. |
| Magnetic surfaces | ORTA | Requires material tagging, attach/detach rules, enemy/object exceptions. |
| Asymmetric enemies | ORTA | Cheap individually, expensive in encounter readability and QA. |
| Gravity-locked hazards | UCUZ | Spikes, lasers, falling blocks responding to global gravity state. |
| Mass classes | UCUZ | Heavy/light tags with different acceleration and collision damage. |

Readability risk:

- Fluids/sand can obscure the clean pivot core unless stylized as chunky cells or ribbons.
- Flying/climbing/heavy enemy exceptions can make gravity feel inconsistent if introduced too early.
- Magnetic surfaces are readable if visually distinct; risky if mixed with too many other surface types.

Missing layers:

- Gravity memory tiles: tiles remember last gravity direction and trigger delayed movement, doors, or crushers.
- Inertia anchors: temporary anchor points that preserve orbit/momentum around them, deepening the pivot timing without adding a new medium.

Minimum depth set:

- Core: gravity pivot + momentum conservation.
- Add 2 layers: mass classes + magnetic/anchor surfaces.
- Add 1 encounter layer later: one gravity-immune enemy family.
- Defer: fluids/sand until after the movement core is proven.

Solo-dev verdict: strong if the game is about movement mastery first and field toys second.

---

### SNAP - Modular-Room Deckbuilder

| Layer | Cost | Why |
|---|---|---|
| Room placement/deck core | ORTA | UI, rules, validation, room graph, run economy. |
| Temporal room states | ORTA | Fire/flood/curse timers require state propagation and preview. |
| Fire/water room spread | ORTA | Manageable on room graph; expensive if per-tile fluid. |
| Light/line-of-sight resource | ORTA | Needs previews and clear fog/visibility rules. |
| Darkness hiding loot/danger | UCUZ | Presentation and reveal logic. |
| Vertical load / instability | ORTA | Simple structural score is cheap; physical collapse is expensive. |
| Hero behavior deck | PAHALI | Adds a second AI/meta system and heavy balancing. |
| Room resonance/combo tags | UCUZ | Tag-based synergies between adjacent rooms. |

Readability risk:

- SNAP has the highest UI risk. Every propagation layer needs a preview before placement.
- Vertical load can be elegant as a structural number, but physical collapse in 3D may turn the deckbuilder into a simulation debugging game.
- Hero behavior deck may split the game into two decks and dilute the core room-placement identity.

Missing layers:

- Adjacency pressure: noisy, sacred, wet, hot, unstable tags modify neighboring rooms without requiring full simulation.
- Room memory: rooms keep scars from previous states, creating light persistence without tracking many live systems.

Minimum depth set:

- Core: draft/place modular rooms on a graph.
- Add 2 layers: temporal room states + adjacency tags.
- Add 1 presentation layer: darkness/reveal as a simple visibility modifier.
- Defer: hero behavior deck and physical vertical collapse.

Solo-dev verdict: viable if all systemic state is previewable in the placement UI. Without previews, the concept becomes unreadable quickly.

---

### HEAVE - Terrain-Weapon Action

| Layer | Cost | Why |
|---|---|---|
| Terrain deformation / heave weapon core | PAHALI | Real-time robust terrain modification is hard; tile/voxel chunks reduce risk. |
| Material layer: mud/stone/ice/lava | ORTA | Table-driven if terrain is discrete; expensive if fully deformable mesh. |
| Water + temperature cross-modulation | PAHALI | Freezing/melting/flowing needs state transitions and VFX clarity. |
| Light/shadow enemy reactions | ORTA | URP lights are easy; tactical shadow gameplay requires careful layout. |
| Shield wall casts shadow | UCUZ | Cheap if shadow is symbolic/projected decal. |
| Ecology over time / weapon plants | PAHALI | Growth timers, spawn logic, and balancing can balloon. |
| Fracture/stability state | ORTA | Cracked terrain, collapse thresholds, and telegraphs. |

Readability risk:

- This concept can become visually noisy because every attack changes the playfield.
- Lava/ice/water/smoke/shadow all at once will bury enemy silhouettes.
- Ecology is flavorful but risks feeling like a second game unless heavily constrained.

Missing layers:

- Terrain fatigue: repeated heaves weaken ground, making later attacks stronger or causing collapses.
- Resonant strata: hidden layers under terrain that produce different weapons when exposed, using the same material table.

Minimum depth set:

- Core: terrain heave as weapon and cover.
- Add 2 layers: material table + terrain fatigue/fracture.
- Add 1 optional medium: temperature only if it modifies exactly two materials at first, such as water<->ice and lava<->stone.
- Defer: ecology and full fluid simulation.

Solo-dev verdict: high upside, high implementation risk. Prototype with grid/voxel terrain before committing.

---

### ECHOLARM - Sound Heist

| Layer | Cost | Why |
|---|---|---|
| Sound propagation / alarm core | ORTA | Cell-based propagation with occlusion is feasible. |
| Light propagation / visibility | ORTA | Visibility cones, light volumes, and occlusion are standard but need tuning. |
| Flash = light + noise | UCUZ | Great cross-modulation: one action emits two fields. |
| Thermal/heat traces | ORTA | Decay fields and thermal guards are feasible. |
| Scent + airflow | ORTA-PAHALI | Scent as field is ORTA; airflow vector authoring/debug is the hard part. |
| Electricity/power network | ORTA | Graph toggles: lights, cameras, doors, hum/noise masks. |
| Asymmetric guards by medium | ORTA | Mechanically cheap, UX/telegraph cost is the real cost. |
| Material acoustics | UCUZ | Carpet/metal/glass change sound radius and footstep profile. |

Readability risk:

- Strongest risk is sensory overlay overload. The player needs one current planning lens, with hotkeys or context views.
- Asymmetric guards must be visually iconic: ears, goggles, thermal mask, scent drone, radio pack, etc.
- Airflow is powerful but invisible unless shown through smoke, dust, curtains, or UI wisps.

Missing layers:

- Masking noise: ambient hum, rain, machinery, and crowds reduce effective sound detection without being pure stealth invisibility.
- Evidence persistence: broken glass, hot handles, open vents, scent trails, and light exposure leave investigation clues after the immediate field decays.

Minimum depth set:

- Core: sound propagation + guard response.
- Add 3 layers: light/visibility, power network, material acoustics.
- Add 1 advanced layer later: heat OR scent, not both at first.
- Use asymmetric guards only after the UI can clearly show sensor type and range.

Solo-dev verdict: best fit for the lens. It can ship as a small but deep systemic stealth game if channels are introduced in acts.

---

### BALLAST - Physics-Structure Survival

| Layer | Cost | Why |
|---|---|---|
| Structure building / stability core | ORTA-PAHALI | Depends on physics fidelity; discrete modules are manageable. |
| Material properties | ORTA | Table-driven: buoyancy, burn, conductivity, weight, strength. |
| Wind + rain + cold weather | ORTA | Weather fields are feasible; compound effects need strong feedback. |
| Sail / ballast tank / counterweight tradeoff | ORTA | Good systemic parts, needs tuning and UI. |
| Moving crew/cargo weight | ORTA | Agent movement plus center-of-mass changes; readable if simplified. |
| Shelter / crew needs | ORTA-PAHALI | Adds survival management; risk of scope creep. |
| Damage/leak compartments | ORTA | Strong fit: water ingress, pumps, patching. |

Readability risk:

- Real physics can become unfair if collapse causes are unclear.
- Weather combinations need a forecast/telegraph; surprise compound weather feels arbitrary.
- Crew needs can pull focus away from structure survival.

Missing layers:

- Center-of-mass ghost: a visible projected balance marker that becomes the main readability tool and tactical target.
- Compartment pressure: sealed rooms resist flooding, carry heat/smoke, or become dangerous under damage.

Minimum depth set:

- Core: build/repair a physical structure under load.
- Add 3 layers: material table, weather vector, compartments/leaks.
- Add 1 tradeoff layer: functional parts with weight costs.
- Defer: full crew simulation until structure readability is solved.

Solo-dev verdict: viable as modular physics, risky as freeform physics. Needs aggressive abstraction.

---

### UZUV - Physics-Creature Auto-Battler

| Layer | Cost | Why |
|---|---|---|
| Modular body/limb assembly | PAHALI | Physics stability, animation, hit detection, and UI are hard. |
| Sensor -> actuator nervous system | ORTA-PAHALI | Node graph can be simple, but debugging emergent behavior is costly. |
| Arena environments | ORTA | Water, hazards, ledges, darkness as arena modifiers. |
| Metabolism/energy | UCUZ-ORTA | Stat budget and energy drain are cheap; real metabolic simulation is not. |
| Reproduction/mutation meta | ORTA | Data mutation and progression; balance cost is high. |
| Damage scars / limb loss | ORTA | Great readability if limbs detach or disable, but physics QA cost. |
| Signal interference | UCUZ | Arena fields delay, scramble, or amplify sensor-actuator links. |

Readability risk:

- UZUV can become opaque: players may not know why a creature won or lost.
- Sensor/actuator chains need replay, highlighting, and simple cause-effect feedback.
- Arena modifiers can make builds feel invalid if they counter too many parts at once.

Missing layers:

- Reflex latency: longer nerve chains have delayed reactions; compact bodies are faster but weaker.
- Body symmetry/asymmetry pressure: asymmetry gives specialized attacks but creates balance/movement instability.

Minimum depth set:

- Core: assemble creature parts, auto-battle.
- Add 2 layers: energy budget + simple sensor-actuator triggers.
- Add 1 arena layer: one environment modifier per fight.
- Defer: reproduction/mutation meta until combat readability is solved.

Solo-dev verdict: promising but the most QA-heavy. Must invest in explanation tools or it becomes random-feeling.

---

## 3. Readability Risk Matrix

| Risk | PIVOT | SNAP | HEAVE | ECHOLARM | BALLAST | UZUV |
|---|---:|---:|---:|---:|---:|---:|
| Too many simultaneous channels | Medium | High | High | High | Medium | Medium |
| Hidden enemy perception | Medium | Low | Medium | High | Low | Medium |
| UI preview burden | Low | Very high | Medium | Medium | Medium | High |
| Visual noise | Medium | Medium | Very high | Medium | Medium | High |
| Physics unpredictability | Medium | Low | High | Low | High | Very high |
| Balance/QA explosion | Medium | High | High | Medium | High | Very high |

Highest-risk layers to cut first:

- PIVOT: real fluids/sand.
- SNAP: hero-behavior deck and physical vertical collapse.
- HEAVE: ecology and full water/temperature simulation.
- ECHOLARM: scent + airflow if light/sound/power are not already excellent.
- BALLAST: crew needs and high-fidelity freeform physics.
- UZUV: reproduction/mutation meta and complex nervous-system graph.

---

## 4. Missing Layer Suggestions By Concept

### PIVOT

1. Gravity memory tiles: local tiles retain previous gravity orientation and become temporary hazards/platforms.
2. Orbit anchors: pivot nodes that convert fall speed into circular motion or slingshot release.

### SNAP

1. Adjacency pressure tags: hot/wet/noisy/sacred/unstable tags modify neighboring rooms.
2. Room scars: past states leave permanent modifiers, such as charred, flooded, blessed, cracked.

### HEAVE

1. Terrain fatigue: repeated manipulation weakens ground and changes future heave outcomes.
2. Resonant strata: underground layer type determines what weapon/cover emerges when lifted.

### ECHOLARM

1. Masking noise: ambient sounds reduce or redirect detection, making noise sometimes defensive.
2. Evidence persistence: traces remain after fields decay, creating post-action investigation pressure.

### BALLAST

1. Center-of-mass ghost: visible balance marker projected onto structure/raft/vehicle.
2. Compartment pressure: sealed modules manage flooding, heat, smoke, or pressure.

### UZUV

1. Reflex latency: sensor-actuator distance creates reaction delay.
2. Symmetry pressure: symmetrical bodies move reliably; asymmetrical bodies hit harder but destabilize.

---

## 5. Minimum Depth Sets

| Concept | Minimum Shippable Depth | Recommended Layer Count | Defer |
|---|---|---:|---|
| PIVOT | Gravity pivot + momentum + mass/surface rules | Core + 2 | Fluids/sand, many enemy exceptions |
| SNAP | Room deck + temporal states + adjacency pressure | Core + 2 | Hero AI deck, physical collapse |
| HEAVE | Terrain weapon + material table + fatigue/fracture | Core + 2 | Ecology, full fluids |
| ECHOLARM | Sound stealth + light + power + material acoustics | Core + 3 | Scent/airflow until late |
| BALLAST | Modular structure + material table + weather + compartments | Core + 3 | Crew survival sim |
| UZUV | Part assembly + energy budget + simple triggers + one arena modifier | Core + 3 | Mutation meta, complex nervous graph |

The cleanest solo-dev path is not equal layer count. It is equal cognitive width:

- PIVOT: 2 active systems can feel deep because movement is expressive.
- SNAP: 2 layers are already enough because the UI has to explain every future consequence.
- HEAVE: 2 layers are enough because deformation changes the whole arena.
- ECHOLARM: 3 layers are justified because the concept is perception-native.
- BALLAST: 3 layers are justified if all are structural/material/weather, not crew micromanagement.
- UZUV: 3 layers are needed because assembly alone needs constraints to avoid solved builds.

---

## 6. Cross-Concept Primitive Review

The orchestrator's primitive map is directionally correct. I would revise it into implementation primitives rather than theme primitives.

### Current Map Assessment

| Primitive | Given Links | Verdict | Adjustment |
|---|---|---|---|
| Vertical load / physics | SNAP <-> BALLAST | Correct | Also touches PIVOT mass classes and UZUV body balance. |
| Medium perception / propagation | ECHOLARM <-> UZUV arena <-> HEAVE light | Correct | Make this the central reusable field system. |
| Material layer | HEAVE <-> BALLAST | Correct | Also useful in PIVOT surfaces and ECHOLARM acoustics. |
| Fluid sim | PIVOT <-> HEAVE | Correct but risky | Prefer discrete flow/field propagation over real fluid. |

### Recommended Reuse Map

#### Primitive A: Field Grid / Volume Field

Stores scalar/vector values over cells or simple volumes.

Reuse:

- ECHOLARM: sound, heat, scent, smoke, alarm pressure.
- HEAVE: heat, gas, local danger, terrain stress.
- BALLAST: wind, rain, flooding, heat/smoke.
- UZUV: arena signal/noise, scent, darkness.
- SNAP: room-state propagation on graph nodes.
- PIVOT: local gravity modifiers, sand/fluid approximations.

Implementation note: support discrete graph mode and spatial grid mode. SNAP wants graph nodes; action games want grid/volumes.

#### Primitive B: Material Table

One data table: mass, friction, conductivity, flammability, acoustic loudness, buoyancy, opacity, heat retention, scent retention.

Reuse:

- HEAVE: terrain behavior and weapon outcome.
- BALLAST: part survival and structure tradeoffs.
- PIVOT: surface gravity/magnet/mass interactions.
- ECHOLARM: footsteps, breakage noise, light occlusion, heat traces.
- UZUV: part material, armor, energy cost.
- SNAP: room tags and propagation modifiers.

#### Primitive C: Occlusion / Line Query

Simple shared visibility/occlusion checks.

Reuse:

- ECHOLARM: light, sight, sound dampening, thermal walls.
- HEAVE: shadow tactics and cover.
- SNAP: line-of-sight rooms and darkness reveal.
- UZUV: sensor targeting.
- PIVOT: ray hazards and gravity-beam devices.

#### Primitive D: Structural Load / Center of Mass

Discrete load score or simple rigidbody aggregate.

Reuse:

- BALLAST: main system.
- SNAP: vertical stack instability.
- UZUV: creature balance.
- PIVOT: heavy objects and crush force.
- HEAVE: terrain fatigue/collapse.

#### Primitive E: Power / Network Graph

Nodes, edges, switches, generators, consumers, overloads.

Reuse:

- ECHOLARM: lights, cameras, doors, hum masking.
- SNAP: room power, adjacency energy, ritual networks.
- BALLAST: pumps, heaters, sails/engines.
- UZUV: nervous system as a body network.
- HEAVE: terrain conduits/electric traps.

---

## Recommended Production Order

If the studio wants one concept to validate the lens:

1. ECHOLARM: proves multi-medium perception with the clearest identity.
2. HEAVE: proves material/terrain cross-modulation but has more tech risk.
3. BALLAST: proves structural/material reuse but needs heavy abstraction.
4. PIVOT: proves force/momentum reuse with lower content burden.
5. UZUV: save until tools for explanation/replay exist.
6. SNAP: best as a separate UI-heavy prototype, not the first lens validation.

If the studio wants reusable tech first:

1. Build Material Table.
2. Build Field Grid / Graph Propagation.
3. Build Occlusion Query helpers.
4. Build simple Network Graph.
5. Build Structural Load only if BALLAST/SNAP/UZUV are selected.

---

## Final Decision Notes

- The lens is worth preserving as a STUDIO_KARAR candidate.
- It should explicitly forbid feature pile-up: every added medium must reuse an existing primitive and create a visible player decision.
- ECHOLARM is the cleanest concept to deepen through light, heat, power, material acoustics, and asymmetric guards.
- SNAP is the clearest place where the lens can break, because every layer becomes a UI preview problem.
- HEAVE and BALLAST have high systemic promise but should use discrete abstractions, not high-fidelity physics/fluid simulation.
- UZUV needs explanation tooling as a core feature, not a polish feature.

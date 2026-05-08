# RIMA Dungeon Generation & Physical Lighting -- Research Synthesis
**Date: 2026-05-08**

Source note: the requested Gemini 2.5 Pro CLI query was attempted, but the model name returned `ModelNotFoundError: Requested entity was not found`. This report therefore uses the provided RIMA brief plus GPT-5.5 training-knowledge synthesis, and does not invent Gemini-sourced findings.

## 1. Dungeon Gen Approaches (AI-assisted indie 2025)

Common AI-adjacent dungeon generation patterns in 2025 fall into four practical groups:

- Rule-first procedural generation with AI support: BSP, drunkard walk, cellular automata, graph grammar, prefab stitching, lock-key graphs, and constraint solvers still do most production work. AI assists by proposing rules, test cases, room catalogs, encounter tables, and naming, not by owning runtime generation.
- WFC and constraint-based tile assembly: Wave Function Collapse remains useful for local tile transitions, wall cracks, corner variants, rubble edges, moss spread, and decorative micro-structure. It is less reliable as the sole room generator unless the game can tolerate solver failures or has strong authoring tools.
- LLM-assisted content authoring: ChatGPT/Gemini-style tools are commonly used for template ideation, room-beat lists, prop placement rules, balance checklists, and Unity editor scripts. The strongest use is accelerating designer-authored content, not replacing designers.
- ML and image-model experiments: ML-Agents/PCGML, diffusion-generated maps, and Stable Diffusion-style layout sketches are interesting for prototypes, mood boards, and offline ideation. They are risky for shipped roguelite layout logic because collision, reachability, readability, combat space, and deterministic seeds must be guaranteed.

For RIMA, the current approach is already in the production-safe zone: graph-level structure plus authored room templates. The recommended use of AI is offline expansion and validation: generate candidate anchor rules, prop catalogs, depth-band variants, and test checklists, then bake approved output into deterministic templates and ScriptableObjects.

## 2. Room Naturalization Techniques for Authored Templates

The goal is to make 16 authored LayoutKind templates feel more numerous without losing combat readability or the Shattered Keep identity.

Recommended techniques:

- Anchor-driven variants: each template stores named anchors for torches, lanterns, loot, rubble, cracks, blockers, enemy spawn zones, and rift accents. Runtime selects from approved anchor sets instead of free-scattering everything.
- Weighted micro-tiles by depth: keep macro layout fixed, but swap clean, cracked, chipped, wet, mossy, and edge-detail tiles according to the depth band. F1 uses sharper clean stone; F2 adds fracture streaks and missing corners; F3 adds moss, damp staining, and lower saturation.
- Local WFC/detail pass: run WFC only on decoration layers or edge-transition masks. Do not let WFC change walkability unless the changed cells are validated by pathfinding.
- Cellular automata for organic overlays: use CA for moss, grime, water stains, ash, dust, or cyan rift residue. Apply it as a detail/entity overlay clipped to valid floor or wall-adjacent cells.
- Symmetry breaking: rotate or mirror only templates that are authored as safe under rotation. Then vary entrance-side dressing, pillar damage, prop clusters, and negative-space details.
- Prop dropout and substitution: a torch anchor may become torch, broken sconce, candle cluster, empty bracket, or rift crystal depending on depth and room role.
- Edge naturalization: decorate walls and corners more than center walk areas. Players read combat space faster when the center remains clean.
- Deterministic seeded variation: all variation should use graph node seed plus template id plus depth band so QA can reproduce a bad room.

RIMA should treat authored templates as the combat contract and naturalization as a non-destructive skin pass.

## 3. Physical Lighting System Design

RIMA should move from floating light volumes to visible prop-owned lights. The rule should be simple: a light exists because the player can see the thing emitting it.

Use one low Global Light 2D as the base cold dungeon exposure, then attach Point Light 2D components to torch, lantern, candle, and rift crystal props. Reserve Freeform Light 2D for special surfaces with an obvious physical source, such as a rift slit, stained window spill, magical floor crack, or doorway glow.

Suggested base:

- Global Light 2D color: `#1E2030` to `#262838`
- Global Light 2D intensity: `0.18` to `0.32`
- Structural sprites: muted blue-grey baseline
- Detail sprites: receive enough light for silhouettes, but never become warm unless near a prop
- Entity sprites: keep readability with sorting and optional separate highlight rules; do not solve combat readability by adding invisible room lights

### 3.1 Prop Types (Torch / Lantern / Rift Crystal / Candle)

Torch:

- Wall-mounted physical source
- Primary warm contrast against the cold keep
- Best for F1 and controlled F2 rooms
- Should use amber light and small flicker

Lantern:

- Floor or hanging prop source
- Slightly softer and broader than a torch
- Good for entry halls, checkpoint-feeling rooms, bridges, and loot rooms
- Can be placed on posts, chains, tables, or fallen stone

Rift Crystal:

- Cold cyan magical source using `#00FFCC`
- Should feel sharp, luminous, and unnatural
- Best for F2/F3, boss approach rooms, secret rooms, and graph nodes tied to rift identity
- Can use a tighter inner radius with a cyan halo and subtle pulse

Candle:

- Small local source
- Best as clusters near altars, desks, reliquaries, graves, or ritual props
- Should not illuminate a combat arena by itself

### 3.2 Placement Algorithm (anchor points vs scatter)

Recommended pipeline:

1. Author template anchors:
   - `WallLight_N`, `WallLight_E`, `WallLight_S`, `WallLight_W`
   - `FloorLantern`
   - `RiftAccent`
   - `CandleCluster`
   - `NoLightZone`
   - `CriticalCombatClearance`

2. Filter anchors by room role:
   - Entry/transition rooms prefer wall torches and lanterns.
   - Arena rooms prefer perimeter lights and avoid center clutter.
   - Reward/secret rooms can accept candle clusters and rift crystals.
   - Boss/elite rooms use fewer, stronger identity lights.

3. Filter anchors by geometry:
   - Torch anchors must be wall-adjacent and face inward.
   - Lantern anchors must be on walkable or prop-valid floor cells.
   - Rift anchors can be wall-adjacent, pillar-adjacent, or crack-adjacent.
   - No light prop should block core pathing or combat lanes.

4. Apply depth-band weights:
   - F1: higher torch probability, fewer broken fixtures.
   - F2: more dropout, flicker, damaged sconces, occasional rift substitution.
   - F3: low torch probability, more cyan and bioluminescent sources.

5. Validate final lighting:
   - Every Point Light 2D must have an owning visible prop.
   - Every visible flame/crystal prop expected to emit light must have a light or explicit "unlit decorative" flag.
   - Minimum path illumination should be checked against a dark-room threshold, not solved with invisible lights.

Scatter should only be a fallback for detail props. If scatter is used, constrain it to wall-adjacent cells, require spacing between emitters, and reject placements near entrances, enemy spawn cores, and player path bottlenecks.

### 3.3 Unity URP 2D Light Parameters per prop type

Assumption: 1 Unity unit equals roughly 1 tile. Values should be tuned visually in-engine.

| Prop | Light Type | Color | Intensity | Inner Radius | Outer Radius | Falloff | Flicker |
|---|---|---:|---:|---:|---:|---:|---|
| Torch | Point Light 2D | `#C8A96E` | `0.75-1.10` | `0.75-1.10` | `3.25-4.50` | `0.55-0.75` | yes |
| Lantern | Point Light 2D | `#D6B87A` | `0.55-0.85` | `1.00-1.50` | `4.00-5.25` | `0.65-0.85` | subtle |
| Rift Crystal | Point Light 2D | `#00FFCC` | `0.45-0.80` | `0.50-0.90` | `2.50-4.00` | `0.45-0.70` | pulse |
| Candle | Point Light 2D | `#E0C58D` | `0.25-0.45` | `0.20-0.45` | `1.20-2.10` | `0.75-0.95` | tiny |
| Rift Crack | Freeform Light 2D | `#00FFCC` | `0.25-0.55` | n/a | shape-based | `0.40-0.70` | pulse |

Practical Unity guidance:

- Use Point Light 2D for any discrete prop source.
- Use Freeform Light 2D only when the source itself has an elongated shape.
- Keep overlap conservative for pixel art; too many overlapping lights wash out hard edges.
- Use normal maps only if the pixel-art pipeline supports controlled hard-edged normals. Otherwise, rely on sprite value contrast and limited light radius.
- Use shadow casters selectively on major walls and pillars, not every small prop.
- Flicker should modulate intensity and radius slightly, not color wildly.

Suggested flicker ranges:

- Torch intensity noise: plus/minus `0.08-0.16`
- Torch outer radius noise: plus/minus `0.15-0.35`
- Lantern intensity noise: plus/minus `0.03-0.07`
- Rift pulse intensity: sine or noise plus/minus `0.05-0.12`
- Candle intensity noise: plus/minus `0.04-0.08`

### 3.4 Depth Band Differentiation (F1 / F2 / F3 lighting mood)

F1, depth 0-2, clean stone:

- Mood: legible, cold, inhabited, still maintained.
- Base global intensity: `0.28-0.32`
- Torch anchor fill: `70-90%`
- Lantern anchor fill: `20-35%`
- Rift crystal substitution: `0-8%`
- Color balance: cold stone with warm wall highlights.
- Flicker: low to medium.

F2, depth 3-5, cracked stone:

- Mood: damaged, less reliable, sharper contrast.
- Base global intensity: `0.22-0.27`
- Torch anchor fill: `40-65%`
- Broken/unlit fixture chance: `20-35%`
- Rift crystal substitution: `8-20%`
- Color balance: more black gaps, occasional cyan intrusions.
- Flicker: medium, including intermittent weak torches.

F3, depth 6+, mossy stone:

- Mood: dim, damp, abandoned, bioluminescent.
- Base global intensity: `0.16-0.22`
- Torch anchor fill: `10-30%`
- Lantern anchor fill: `5-15%`
- Rift/bioluminescent source chance: `25-45%`
- Color balance: cold cyan and muted green-grey, warm light becomes rare and important.
- Flicker: low for cyan pulse, high only for dying flame props.

This creates a readable descent: F1 is physically lit by remaining human infrastructure, F2 is failing, and F3 is lit by the dungeon's unnatural ecology.

## 4. PropSpec ScriptableObject Design (recommended structure with C# sketch)

Recommended design: make light emission a property of the prop definition, not a separate invisible dungeon-gen pass. The runtime builder places a PropSpec; the prop prefab or spawned instance creates the matching Light2D from the spec.

Key fields:

- Visual: prefab, sprite variant set, sorting layer hints.
- Placement: allowed anchor tags, wall adjacency requirement, floor requirement, footprint, clearance radius.
- Generation: depth-band weights, room-role weights, min spacing, max per room, substitution group.
- Lighting: emitsLight, source kind, color, intensity, inner/outer radius, falloff, flicker profile, shadow behavior.
- Validation: requiresVisibleSource, canBeUnlitVariant, blocksMovement, blocksLineOfSight.

Sketch:

```csharp
using UnityEngine;
using UnityEngine.Rendering.Universal;

public enum LightSourceKind
{
    None,
    Torch,
    Lantern,
    RiftCrystal,
    Candle,
    RiftCrack
}

public enum DepthBand
{
    F1Clean,
    F2Cracked,
    F3Mossy
}

[CreateAssetMenu(menuName = "RIMA/Dungeon/Prop Spec")]
public sealed class PropSpec : ScriptableObject
{
    [Header("Visual")]
    public GameObject prefab;
    public string[] allowedAnchorTags;
    public Vector2Int footprint = Vector2Int.one;

    [Header("Placement")]
    public bool requiresWallAdjacency;
    public bool requiresWalkableFloor;
    public bool blocksMovement;
    public float minSpacingFromSameKind = 3f;
    public int maxPerRoom = 4;

    [Header("Depth Weights")]
    [Range(0f, 1f)] public float f1Weight = 1f;
    [Range(0f, 1f)] public float f2Weight = 0.7f;
    [Range(0f, 1f)] public float f3Weight = 0.2f;

    [Header("Light")]
    public bool emitsLight;
    public bool requiresVisibleSource = true;
    public LightSourceKind lightSourceKind;
    public Light2D.LightType lightType = Light2D.LightType.Point;
    public Color lightColor = Color.white;
    [Range(0f, 2f)] public float intensity = 0.8f;
    public float innerRadius = 1f;
    public float outerRadius = 4f;
    [Range(0f, 1f)] public float falloffIntensity = 0.7f;

    [Header("Flicker")]
    public bool flickers;
    public float flickerIntensityAmplitude = 0.08f;
    public float flickerRadiusAmplitude = 0.2f;
    public float flickerSpeed = 4f;
}
```

Implementation note: this sketch should be adapted to the actual RIMA naming, folder, and builder conventions before coding. The design intent is that `DungeonWorldBuilder` chooses physical props, and light creation follows from the prop spec.

## 5. Implementation Priority for RIMA

Priority 1: Remove invisible procedural light ownership.

- Keep `createProceduralLights` only as a temporary compatibility flag.
- Add a rule that every generated light stores or derives from an owning prop id.
- Add validation logs for floating lights during dungeon build.

Priority 2: Add template light anchors.

- Start with wall torch anchors and floor lantern anchors for the 16 LayoutKind templates.
- Make anchors directional so wall-mounted props face into the room.
- Define no-light and no-prop zones around entrances, combat centers, and critical lanes.

Priority 3: Add PropSpec data.

- Create initial specs for torch, lantern, rift crystal, candle cluster, broken sconce, and unlit bracket.
- Use substitution groups so "wall light" anchors can resolve to lit torch, broken torch, empty bracket, or rift growth.

Priority 4: Implement depth-band lighting tables.

- F1: stable torch/lantern infrastructure.
- F2: reduced warm light, broken fixtures, first cyan intrusion.
- F3: rare warm light, cyan/moss bioluminescence.

Priority 5: Naturalization pass.

- First pass: weighted tile/detail swaps and prop dropout.
- Second pass: local WFC for cracks, moss, rubble edges, and floor-detail adjacency.
- Third pass: CA overlays for moss/damp/rift residue, clipped to non-critical cells.

Priority 6: Visual QC.

- Capture each template at F1/F2/F3.
- Check that no light floats without a source.
- Check that entrances and combat lanes remain readable.
- Check that palette stays cold overall and amber is a local accent.

## 6. Open Questions for Design Session

- What is the exact tile-to-Unity-unit scale used by DungeonWorldBuilder? Light radii should be tuned against that scale.
- Should F3 bioluminescence be cyan only, or can it include a muted green secondary accent?
- Are rift crystals safe environmental props, hazards, interactables, or pure decoration?
- Should broken sconces be purely visual, or can they telegraph that a room is intentionally dark?
- How many simultaneous 2D lights can the target platform tolerate before pixel-art readability or performance suffers?
- Should darkness affect gameplay, or is it atmospheric only?
- Should room templates own light anchors directly, or should anchors live in a separate authoring overlay to keep layout data clean?
- Are secret rooms allowed to break the physical-lighting rule with unexplained supernatural glow, or must even that glow come from visible rift cracks/crystals?

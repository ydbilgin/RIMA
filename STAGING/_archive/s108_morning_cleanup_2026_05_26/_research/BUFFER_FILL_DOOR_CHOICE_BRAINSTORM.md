# Buffer Fill + Door Choice Brainstorm

Status: draft for production planning  
Date: 2026-05-21  
Scope: PixelLab Phase 1 Batch 4/5 buffer fill + RIMA-unique door choice alternatives

Context note: Required NLM CLI queries were attempted first, but NotebookLM auth was expired. This draft uses allowed fallback context from CURRENT_STATUS, PROJECT_RULES, MEMORY, STAGING, and the local NLM canonical batch.

## 1. Buffer Fill List

### Production Logic

- Batch 4 buffer target: 8 medium objects, 48-80 cell, reusable prop/item/atmospheric decor.
- Batch 5 buffer target: 56 tiny objects, 32-40 cell, pickup/decal/symbol/floor accent.
- Base-first rule: generate clean universal base when possible, then use Phase 2 state pipeline for Act recolors, damage, decay, mirror, cursed, unstable, or rust variants.
- Act material anchor:
  - Act 1 Shattered Keep: dark/cool granite, cyan rift, failed shelter, ancient order.
  - Act 2 Bleeding Wastes: #3A2840 bone/rust, outside damage, wound-like corruption.
  - Act 3 Core Approach: #0A0810 void/gold, source approach, unstable sigils.
  - Act 4 Nexus Core: mirror reflection, convergence, duplicated timelines.

### Batch 4 Buffer - Medium 64-Cell Slots (8/8)

| Slot | Item | Size | Act usage | Production priority |
|---:|---|---:|---|---|
| M01 | Rift Reliquary Core | 64 | All acts as evolving rift artifact: cyan crack, rust wound, void-gold, mirror split | P0 |
| M02 | Ward Brazier / Torch Stand | 64 | Act 1 lit cyan, Act 2 dim rust ember, Act 3 unstable void flame, Act 4 reflected twin flame | P0 |
| M03 | Broken Lectern + Sealed Scroll | 64 | Failed shelter archive prop; Act-specific paper/metal decay via states | P1 |
| M04 | Pressure Plate Trap Mechanism | 48-64 | Universal trap base; Act 2 bone rim, Act 3 gold sigil, Act 4 mirrored plate | P0 |
| M05 | Iron-Wood Chest Base | 64 | Container family base: wooden, iron, cursed, mirror chest states | P0 |
| M06 | Torn Ward Banner Stand | 64-80 | Wall/floor decor: purple Act 1, blood/rust Act 2, void Act 3, mirror foil Act 4 | P1 |
| M07 | Defender Statue Fragment | 64-80 | Lore prop for failed shelter; later broken, corrupted, void-carved, mirrored states | P1 |
| M08 | Echo Offering Pedestal | 64 | Reward/choice pedestal; ties to Echo/Death Imprint, usable in reward and event rooms | P0 |

### Batch 5 Buffer - Tiny 32-Cell Slots (56/56)

| Slot | Item | Size | Act usage | Production priority |
|---:|---|---:|---|---|
| T01 | Health Potion Vial | 32 | Universal pickup; red/cyan rim states | P0 |
| T02 | Mana Shard | 32 | Universal pickup; cyan base, gold/void Act 3 state | P0 |
| T03 | Echo Currency Chip | 32 | Universal meta currency, tied to echo reward language | P0 |
| T04 | Key Fragment A | 32 | Route/key pickup, first shard piece | P0 |
| T05 | Key Fragment B | 32 | Route/key pickup, second shard piece | P0 |
| T06 | Map Fragment Corner | 32 | Progression/reveal pickup | P0 |
| T07 | Soul Echo Wisp | 32 | Echo pickup; can pulse in VFX later | P0 |
| T08 | Rift Stone Splinter | 32 | Item-system component echo; all acts via color state | P0 |
| T09 | Blood Gem Chip | 32 | Item-system component; Act 2 strongest, universal as pickup | P1 |
| T10 | Void Fragment Chip | 32 | Item-system component; Act 3 strongest | P1 |
| T11 | Iron Shard Chip | 32 | Item-system component; Act 1/2 universal | P1 |
| T12 | Soul Ember Chip | 32 | Item-system component; Act 2/3 universal | P1 |
| T13 | Shadow Veil Thread | 32 | Item-system component; Act 3/4 readable | P1 |
| T14 | Chain Links Coil | 32 | Item-system component + dungeon scatter | P1 |
| T15 | Heart HUD Icon | 32 | UI/HUD pixel art | P0 |
| T16 | Mana HUD Icon | 32 | UI/HUD pixel art | P0 |
| T17 | Echo HUD Icon | 32 | UI/HUD pixel art | P0 |
| T18 | Fragment HUD Icon | 32 | UI/HUD pixel art | P0 |
| T19 | Elite Skull Marker | 32 | Door/room type symbol, danger readable | P0 |
| T20 | Shop Coin Marker | 32 | Door/room type symbol, economy readable | P0 |
| T21 | Event Question Rune | 32 | Door/room type symbol, uncertainty readable | P0 |
| T22 | Boss Crown-Rift Marker | 32 | Door/room type symbol, high-threat readable | P0 |
| T23 | Combat Crossed-Blades Marker | 32 | Door/room type symbol | P0 |
| T24 | Reward Orb Marker | 32 | Door/room type symbol | P0 |
| T25 | Ward Circle Small | 32-40 | Floor accent, Act 1 protective grammar | P1 |
| T26 | Ritual Mark Small | 32-40 | Floor accent, Act 1/3 ritual grammar | P1 |
| T27 | Rift Crack Accent A | 32-40 | L6 cyan/rust/gold/mirror crack | P0 |
| T28 | Rift Crack Accent B | 32-40 | Alternate shape for scatter variation | P0 |
| T29 | Mirror Splinter Decal | 32 | Act 4 anchor, also early foreshadow | P1 |
| T30 | Rust-Bone Splinter Decal | 32 | Act 2 material accent | P1 |
| T31 | Void-Gold Sigil Chip | 32 | Act 3 material accent | P1 |
| T32 | Cyan Rift Droplet | 32 | Act 1 rift leak accent | P1 |
| T33 | Cobweb Corner | 32 | Atmosphere, Act 1/2 | P2 |
| T34 | Dust Mote Cluster | 32 | Atmosphere overlay, Act 1 | P2 |
| T35 | Water Drip Spot | 32 | Shelter decay, Act 1 | P2 |
| T36 | Ember Coal | 32 | Act 2 torch/brazier scatter | P2 |
| T37 | Candle Flame Sprite | 32 | Torch animation atom; Act states via recolor | P1 |
| T38 | Candle Stub Unlit | 32 | Candle/torch state atom | P1 |
| T39 | Small Bone Fragment | 32 | Act 2 scatter, universal danger accent | P1 |
| T40 | Pebble Cluster A | 32 | Universal scatter | P2 |
| T41 | Pebble Cluster B | 32 | Universal scatter variation | P2 |
| T42 | Broken Tile Chip | 32 | Universal floor damage | P2 |
| T43 | Chain Debris Small | 32 | Failed shelter/prison mood, Act 1/2 | P1 |
| T44 | Torn Cloth Scrap | 32 | Banner/defender remains, Act 1/2/4 states | P2 |
| T45 | Scroll Roll Small | 32 | Library/lectern scatter | P1 |
| T46 | Burned Page | 32 | Library decay, Act 1/2 | P1 |
| T47 | Book Stack Small | 32-40 | Shattered Keep archive decor | P1 |
| T48 | Loose Coin / Mirror Coin | 32 | Shop/reward scatter, Act 4 mirror state | P2 |
| T49 | Spike Trap Head | 32-40 | Trap component; can pair with M04 plate | P0 |
| T50 | Swinging Blade Chip | 32-40 | Trap preview icon or small hazard component | P1 |
| T51 | Tripwire Peg | 32 | Trap component, easy reuse | P1 |
| T52 | Ward Nail / Anchor | 32 | Failed shelter engineering detail | P2 |
| T53 | Echo Footprint A | 32 | Death Imprint trail decal | P0 |
| T54 | Echo Footprint B | 32 | Death Imprint trail alternate | P0 |
| T55 | Fractured Tablet Glyph | 32-40 | Choice/reward/room preview symbol | P0 |
| T56 | Mini Rift Portal Seed | 32-40 | Door-choice atom, event marker, rare rift opportunity | P0 |

### Priority Cut

P0 should be generated first because it supports core gameplay feedback, room-choice readability, reward language, or Death Imprint hooks. P1 fills theme density and future state-pipeline leverage. P2 is useful atmosphere but should be skipped first if generation budget tightens.

## 2. Door Choice Design

### Why the Hades Pattern Would Look Ordinary

- Three doors with floating reward icons is now a genre shorthand; players read it as "Hades room reward UI" before reading RIMA's world.
- The symbol-only choice happens outside the room fiction unless the door itself explains why the future is visible.
- A door row makes the choice feel like a menu with legs, not a consequence of Convergence or failed shelter architecture.
- If combat, elite, shop, event, and boss are only icon swaps, the system has low visual memory and weak Death Imprint integration.
- Hades uses gods/reward identity as the hook; RIMA needs timeline scars, echoes, and material transformation as the hook.

### Proposal A - Echo Loom Fractures (Strongest)

**Visual:** Three vertical rift seams tear open around a broken ward loom or fractured tablet at the room exit. Each seam contains a short parallax glimpse: silhouettes, floor material, danger color, and a reward glyph. It is not a full doorway; it is a projected possible sub-room sequence.

**Mechanic:** Player walks into the chosen seam. Hover/near-field expands the seam, shows room type glyph, threat grade, and one vague modifier such as "blood trail", "sealed ward", or "echo stain". Commit pulls the player through with a fast fold-to-black transition. Same flow as room choice, but the interaction is a timeline-selection ritual.

**Lore:** The Convergence reads the player's deathmark and manifests three survivable continuations from failed timelines. Past defenders left ward tablets that were meant to seal the rift, but the tablets now display possible futures instead. Death Imprint can mark a seam as "remembered": if the player previously died in a matching encounter signature, a faint echo footprint crosses that option.

**Information shown:**

| Room type | Glyph | Material cue | Motion cue |
|---|---|---|---|
| Combat | Crossed blades / cracked ward | Balanced rift glow | steady pulse |
| Elite | Skull or crown shard | heavier shadow rim | heartbeat pulse |
| Shop | coin/mirror coin | warm rim, less distortion | slow shimmer |
| Event | question rune/tablet glyph | asymmetric glitch | irregular flicker |
| Boss | crown-rift marker | large central tear | deep inhale/exhale |
| Reward | orb/fragment | soft light, stable seam | floating motes |

**Animation:** Idle = thin breathing fissure. Hover = preview expands like a vertical slit; glyph locks into focus. Commit = seam folds inward, echo footprints streak forward, fade-to-black.

**Audio:** Stone scrape + glassy rift harmonics. Elite adds low pulse. Shop adds muted coin chime inside distortion. Death Imprint remembered option adds reversed footstep whisper.

**PixelLab specs:** 64-80 cell medium set for `echo_loom_base`, `rift_seam_choice_A/B/C`, `fractured_tablet_choice`, plus 32-cell glyph set from Batch 5 T19-T24/T55/T56. Generate base seam in neutral cyan first; Phase 2 states: rust wound, void-gold, mirror split, deathmark echo overlay.

### Proposal B - Mirror Remnant Triptych

**Visual:** Three broken standing mirrors, each reflecting a distorted version of the next sub-room rather than acting as a literal door. The mirror frame is shelter architecture in Act 1, bone/rust in Act 2, void-gold in Act 3, and self-splitting glass in Act 4.

**Mechanic:** Player approaches a mirror; its reflection resolves into a room type and reward. Interaction prompt confirms the selected reflection, then the player steps through the mirror surface.

**Lore:** The Nexus Core's mirror convergence leaks backward into earlier acts. These mirrors are failed defensive devices that once observed alternate breach routes. Death Imprint can appear as the player's silhouette dying inside one mirror.

**Strength:** Very strong Act 4 payoff and clear visual hierarchy.

**Risk:** Too mirror-forward too early may spoil Act 4 identity unless early versions are framed as cracked ward-glass, not full mirror tech.

**PixelLab specs:** 64-80 cell mirror frame object, 32-cell room glyph overlays, 32-cell mirror shard decals. States: dusty ward glass, rust-veined glass, void-black reflection, perfect mirror.

### Proposal C - Defender Echo Silhouettes

**Visual:** Three ghostly defender silhouettes appear at exits, each holding or pointing toward a route symbol. Combat echo has raised weapon, elite echo kneels wounded, shop echo holds a pack, event echo offers a tablet, boss echo burns with crown-rift aura.

**Mechanic:** Player stands in an aura field around one echo to lock choice. The echo replays a one-second death or victory fragment, then opens the transition archway.

**Lore:** Failed shelter defenders died across many timelines. The Convergence projects their last successful or failed path. Death Imprint makes the player one of those silhouettes after dying; later runs may show the player's old outline as a warning.

**Strength:** Most human, emotional, and directly tied to failed shelter.

**Risk:** Requires character-like sprite clarity and animation polish; can become noisy if room choice should be read fast.

**PixelLab specs:** 64 cell echo silhouette bases, 32-cell aura/glyph overlays, optional 4-frame idle flicker later. Generate silhouette-neutral first; states by pose and color.

### Comparison

| Criterion | Echo Loom Fractures | Mirror Remnant Triptych | Defender Echo Silhouettes |
|---|---|---|---|
| RIMA uniqueness | High | Medium-high | High |
| Avoids Hades door clone | High | Medium | High |
| Death Imprint link | High | Medium-high | Very high |
| Act 1 fit | High | Medium | High |
| Act 4 payoff | High | Very high | Medium |
| PixelLab complexity | Medium | Medium | High |
| Runtime clarity | High | High | Medium |
| Reuse with Batch 5 glyphs | Very high | High | Medium |

### Final Recommendation

**Pick Proposal A: Echo Loom Fractures.**

It is the strongest production choice because it preserves the familiar choice flow without using ordinary doors, directly explains pre-knowledge through Convergence, and gives Death Imprint a visible but optional layer. It also reuses the Batch 5 tiny glyphs and Batch 4 medium rift/tablet objects, so the brainstormed buffer fill and the mechanic reinforce each other.

Recommended first asset group:

| Asset | Size | Purpose |
|---|---:|---|
| Echo Loom / Fractured Tablet Base | 64-80 | Room-choice anchor prop |
| Rift Seam Choice Base | 64 | Selectable future portal |
| Room Type Glyph Set | 32 | Combat/elite/shop/event/boss/reward read |
| Echo Footprint Decals | 32 | Death Imprint remembered-route layer |
| Mini Rift Portal Seed | 32-40 | Rare event / choice atom |

Implementation stance: keep first version readable and data-light. Show room type, threat intensity, and one material/echo hint. Do not show full rewards or exact enemies until the design proves it needs that detail.

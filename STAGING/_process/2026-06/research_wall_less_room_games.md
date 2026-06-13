# Research: Wallless / Minimal-Wall Room Design in Top-Down Games
**Date:** 2026-05-24
**Purpose:** Fallback room design reference for RIMA — rooms bounded by darkness/fog rather than explicit wall geometry, defined by floor tiles + standalone props.

---

## Analysis Table

| Game | Wall Philosophy | Floor + Prop Strategy | RIMA Fallback? |
|---|---|---|---|
| Vampire Survivors | Infinite looping planes, void edges on select stages | Flat repeating tiles, sparse clutter, no structural props | NO — no room definition |
| Brotato | Thin drawn fence / visual drop-off | Single flat floor texture, zero props | NO — too minimalist |
| 20 Minutes Till Dawn | Darkness = only boundary (vision radius fades) | Monochromatic ground + procedural trees | NO — unbounded open plane |
| Halls of Torment | Physical collision bounds hidden by extreme shadow clipping; periphery fades to black | Tile-based floor + standalone props (candelabras, broken pillars) as obstacles | MODERATE — great fog technique, still walled mechanically |
| Magicraft | Room edge = cliff / drop-off into void | Floating island tilesets; broken tiles + debris define edge | YES — floating island approach |
| Hades (Chaos Realms) | Absolute void boundary; no walls | Floating stone platform floor + framing props (statues, obelisks) in dark periphery | YES — gold standard |
| Crab Champions | Floating islands bounded by ocean/sky void | Procedural terrain + scattered props | MODERATE — 3rd-person perspective shifts perception |
| ROUNDS | Camera frame = boundary | Minimalist floating geometric platforms | NO — 2D side-scrolling |
| Diablo 1 | Light radius mechanic = moving wall of darkness; exterior zones use soft boundaries (trees, rivers) | Isometric tile grid; unrendered black outside light radius | YES — proves darkness alone creates bounded feel |
| Path of Exile (Shaper's Realm / Synthesis) | Floor suspended in cosmic void; no walls | Elaborate floor tiles that shatter/fade at edges; massive standalone props outside walkable area frame the space | YES — exceptional ARPG reference |
| Torchlight 1 & 2 (Nether Realms) | Floating earth chunks in magical void | Chunky stylized floor breaking at edges; chains, crystals, portal frames | YES — strong prop-defines-space example |
| Don't Starve | Ocean = mechanical boundary; darkness = lethal at night | 2D billboard props on flat 3D plane | MODERATE — mechanics ideal, aesthetic too open-world for room ref |
| Children of Morta (Boss Arenas) | Pitch black void with detailed pixel-art floor island | Floor island + standalone props (braziers, arcane circles, standing stones) for lighting + edge definition | YES — perfect execution |
| Curse of the Dead Gods | Bottomless pits masked by thick darkness; some walls in mixed rooms | Stone floor tiles + fire braziers, statues, debris define room shape; darkness = physical barrier | YES — best Hades-aesthetic match |

---

## Top 3 RIMA Fallback References

### 1. Hades — "Chaos Realm" Approach
**Why top pick:** Supergiant established the definitive 3/4 high-top-down wallless aesthetic. Chaos rooms use absolute void.

**Technique:**
- Playable floor = stark distinct texture island
- Edges populated with floating props (rock chunks, broken pillars, massive statues) at slightly different Z-levels
- **Negative space + parallax debris** frames the room without vertical wall geometry
- No camera occlusion from walls; depth implied by prop placement

**RIMA takeaway:** Use floor island + Z-offset framing props. Let void be the hard boundary. Props should be oversized to read at distance.

---

### 2. Curse of the Dead Gods — "Chasm + Torchlight" Approach
**Why strong:** Closest tonal match to RIMA's dark fantasy dungeon feel. Best implementation of light-as-boundary.

**Technique:**
- Bottomless pits hidden by oppressive darkness = hard stop without vertical wall mesh
- Room shape defined entirely by floor tile layout + braziers/statues/debris placement
- Unlit props fade into void — room feels organically bounded by **light radius rather than architecture**
- Some walled rooms exist (hybrid); wallless rooms feel distinct and dramatic

**RIMA takeaway:** Fallback rooms can reserve full wall geometry for "architectural" rooms; use chasm+darkness rooms for dramatic open arenas (boss antechambers, cursed zones).

---

### 3. Path of Exile (Shaper's Realm / Synthesis Maps) — "Monumental Framing Props" Approach
**Why strong:** Proves the concept works in a hyper-detailed dark fantasy ARPG at scale.

**Technique:**
- No walls; massive non-colliding props (colossal gears, celestial statues, planetary bodies) placed *beneath* or *around* floating floor tiles in void
- Floor edges = broken/dissolving geometry transitioning naturally to void
- Camera never occluded; monumental props keep space from feeling empty
- Props are **decorative, non-blocking** — they frame without enclosing

**RIMA takeaway:** Place oversized landmark props around room periphery (outside walkable area) for depth. Break floor tile edges procedurally to soften boundary. Avoid placing large props in camera sightline center.

---

## Implementation Notes for RIMA

- **Floor island approach** works in top-down 70-80 degree + 3/4 sprite style without wall geometry
- **Prop placement rules:** Standalone props (torches, statues, columns, debris) cluster near edges and corners; center stays clear for combat
- **Darkness boundary depth trick:** Use two darkness layers — one at floor level (fog/mist), one at periphery (pure black) — creates natural depth gradient without geometry
- **Tile edge treatment:** Shatter/dissolve effect at floor tile boundary >> hard cutoff. Prevents the "floating carpet" look
- **Fallback applicability:** These rooms suit specific room *types* (cursed halls, boss antechambers, void pockets) rather than every room; maintain walled rooms for standard dungeon corridors

---

## Confidence
HIGH — all games analyzed are well-documented; design patterns align with publicly known approaches. No GDC citations surfaced; model knowledge only.

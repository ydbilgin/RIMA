# Research: tinyworld.build
Date: 2026-05-19

## What Is It?

Tiny World Builder is a browser-based voxel sandbox / world-builder tool — not a game. Users place terrain, buildings, props, and vegetation on a grid (8x8 to 48x48 tiles). It supports procedural world generation from text prompts (OpenAI/Anthropic/xAI backends), cloud saves, and real-time collaboration. Art style is low-poly voxel, colorful and whimsical.

**Not a roguelike. Not pixel art. Not directly comparable to RIMA.**

## Perspective Options (Relevant Observation)

The tool ships five named camera presets:
- Top-down bird's eye
- Isometric angled overhead
- **Soft tilted perspective** (closest to RIMA's 35° angled top-down)
- Close orbit
- Walk (first-person)

The "soft tilted" preset produces a look where terrain tiles read as flat but environmental props (fences, trees, rocks) read with slight vertical presence — similar to what RIMA is achieving with Transform Squash on floor tiles + upright sprites for props.

## Art Style Notes

- Voxel blocks are rendered with soft ambient occlusion and diffuse color banding — achieves clear depth without complex shading.
- Tiny scale (8x8 grid) forces silhouette clarity; every placed object must read instantly.
- Environmental variety comes from density variation, not tile complexity — few tile types, heavy prop clustering.
- Time-of-day and weather systems create mood from the same asset set (relevant to RIMA's single-dungeon palette needing mood variance).

## RIMA Takeaways

**1. Depth via ambient occlusion on floor, not texture complexity.**
Tiny World's voxel floors look rich because of subtle shadow underneath object bases, not intricate tile art. RIMA parallel: ensure props (pillars, walls, crates) cast a small drop shadow or receive a baked AO blob sprite at base. This reinforces 35° read without needing new floor tile variants.

**2. Prop density variation = visual richness, not tile variation.**
The tool looks good with very few tile types because prop clustering carries visual weight. RIMA's current Blueprint system (zone-based prop pools) is already aligned with this — reinforces that the 70/20/10 floor budget (v15d) is correct and tile variation efforts should stay low-priority relative to prop pool depth.

## Verdict

Not a direct reference for RIMA's dungeon art style. Borrow: AO blob under props + prop-density-as-richness principle. Do not borrow: voxel palette, whimsical color, grid scale.
